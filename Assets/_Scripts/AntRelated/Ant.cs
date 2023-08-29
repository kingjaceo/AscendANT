using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;

public class Ant : MonoBehaviour
{
    public Colony Colony { get; private set; }
    public Caste Caste { get; private set; }

    public int ID { get; private set; }
    public AntState AntState { get; private set; }
    public PheromoneState PheromoneState { get; private set; }
    public Pheromone CurrentPheromone { get; private set; }
    public float PheromoneProgress { get; private set; } = 0;
    public Memory Memory { get; private set; }

    public Dictionary<ResourceType, float> Carrying = new Dictionary<ResourceType, float>();

    private List<Pheromone>  _pheromoneSequence;
    private int _currentPheromoneIndex;
    protected float _antennaRadius = 0.25f;
    protected Vector3 _direction;
    protected Transform _transform;


    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Ant created!");
        Rigidbody body = GetComponent<Rigidbody>();
        body.freezeRotation = true;

        _transform = transform;
    }

    public virtual void Start()
    {
        AntState = AntState.Idle;
        PheromoneState = PheromoneState.InProgress;
        _currentPheromoneIndex = 0;

        foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
        {
            Carrying[resourceType] = 0;
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        // PheromoneProgress = CurrentPheromone.UpdatePheromoneProgress(this);

        // update current pheromone
        if (PheromoneState == PheromoneState.Complete)
        {
            _currentPheromoneIndex = (_currentPheromoneIndex+1) % _pheromoneSequence.Count;
            PheromoneState = PheromoneState.InProgress;
            Debug.Log("Pheromone on Ant" + ID + " changes to: " + _pheromoneSequence[_currentPheromoneIndex]);
        }
        CurrentPheromone = _pheromoneSequence[_currentPheromoneIndex];

        // update direction
        _direction = CurrentPheromone.GetDirection(this);

        // randomly rotate the direction by a few degrees
        _direction = Quaternion.AngleAxis(UnityEngine.Random.Range(-5f, 5f), _transform.up) * _direction;
        _transform.forward = _direction;

        // move the ant forward
        _transform.position += Time.deltaTime * Caste.Speed * _transform.forward;
    }

    void OnCollision(Collision collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            return;
        }
        
        // check if the current pheromone is still active
        // antState = AntState.Collided;
        CurrentPheromone.UpdateStates(this, collision);

        // if the ant touched the colony, update its memory
        if (collision.gameObject.name == "Colony")
        {
            // Debug.Log("Ant" + ID + " memory before update: " + Memory);
            // Debug.Log("Colony memory before update: " + Memory);
            Colony.Memory.UpdateColonyMemory(Memory);
            Memory.UpdateAntMemory(Colony.Memory);
            // Debug.Log("Ant" + ID + " memory after update: " + Memory);
            // Debug.Log("Colony memory after update: " + Memory);
        }
    }

    // An ant's behavior might change on collision with another object
    void OnCollisionEnter(Collision collision)
    {
        OnCollision(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        OnCollision(collision);
    }

    public virtual void AssignColony(Colony colony)
    {
        this.Colony = colony;
        ID = colony.NumAnts;
        Memory = new Memory(colony.Memory);
    }

    public void AssignCaste(Caste caste)
    {
        this.Caste = caste;
        _pheromoneSequence = caste.PheromoneSequence;
        this.CurrentPheromone = this._pheromoneSequence[0];
    }

    public void SetAntState(AntState antState)
    {
        AntState = antState;
    }

    public void SetPheromoneState(PheromoneState pheromoneState)
    {
        PheromoneState = pheromoneState;
    }

    public void SetColony(Colony colony)
    {
        Colony = colony;
    }

    public void SetMemory(Memory memory)
    {
        Memory = memory;
    }

    // public void MergeMemory()
    // {
    //     colony.memory.nearestResource = new Dictionary<ResourceType, Vector3>(memory.nearestResource);
    //     colony.memory.resourceLocations = new Dictionary<ResourceType, List<Vector3>>(memory.resourceLocations);
    // }

    // private void UpdateMemory()
    // {
    //     memory.nearestResource = new Dictionary<ResourceType, Vector3>(colony.memory.nearestResource);
    //     memory.resourceLocations = new Dictionary<ResourceType, List<Vector3>>(colony.memory.resourceLocations);
    // }

    void OnDrawGizmos()
    {
        // Draws a 5 unit long red line in front of the object
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * 2;
        Gizmos.DrawRay(transform.position, direction);

        Gizmos.DrawWireSphere(transform.position, _antennaRadius);
    }
}
