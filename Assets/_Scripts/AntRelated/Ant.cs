using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;

public class Ant : MonoBehaviour
{
    public Colony Colony { get; private set; }
    public Caste Caste { get; private set; }

    [field: SerializeField] public int ID { get; private set; }
    // [field: SerializeField] public AntState AntState { get; private set; }
    // [field: SerializeField] public PheromoneState PheromoneState { get; private set; }
    // [field: SerializeField] public Pheromone CurrentPheromone { get; private set; }
    public float PheromoneProgress { get; private set; } = 0;
    public Memory Memory { get; private set; }

    public Dictionary<ResourceType, float> Carrying = new Dictionary<ResourceType, float>();

    // private List<Pheromone>  _pheromoneSequence;
    // private int _currentPheromoneIndex;
    protected float _antennaRadius = 0.25f;

    protected Vector3 _direction;
    protected Transform _transform;
    public Transform Transform => _transform;
    public TargetType LocationTarget { get; private set; }
    public ResourceType ResourceTarget { get; private set; }
    public bool CollidedWithTarget = false;

    protected AntBehaviorMachine _antBehaviorMachine;
    public AntBehaviorMachine AntBehaviorMachine => _antBehaviorMachine;

    protected PheromoneMachine _pheromoneMachine;
    public PheromoneMachine PheromoneMachine => _pheromoneMachine;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Ant created!");
        Rigidbody body = GetComponent<Rigidbody>();
        body.freezeRotation = true;

        _transform = transform;

        _antBehaviorMachine = new AntBehaviorMachine(this);
        _pheromoneMachine = new PheromoneMachine(this);
    }

    public virtual void Start()
    {
        _antBehaviorMachine.Initialize(_antBehaviorMachine.Idle);
        _pheromoneMachine.Initialize(Caste.PheromoneSequence, this);

        // AntState = AntState.Idle;
        // PheromoneState = PheromoneState.InProgress;
        // _currentPheromoneIndex = 0;

        foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
        {
            Carrying[resourceType] = 0;
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        _pheromoneMachine.Update();
        _antBehaviorMachine.Update();
        Move();


        // update current pheromone
        // if (PheromoneState == PheromoneState.Complete)
        // {
        //     _currentPheromoneIndex = (_currentPheromoneIndex+1) % _pheromoneSequence.Count;
        //     PheromoneState = PheromoneState.InProgress;
        //     Debug.Log("Pheromone on Ant" + ID + " changes to: " + _pheromoneSequence[_currentPheromoneIndex]);
        // }
        // CurrentPheromone = _pheromoneSequence[_currentPheromoneIndex];

        // // update direction
        // _direction = CurrentPheromone.GetDirection(this);

        // // randomly rotate the direction by a few degrees
        // _direction = Quaternion.AngleAxis(UnityEngine.Random.Range(-5f, 5f), _transform.up) * _direction;
        // _transform.forward = _direction;

        // // move the ant forward
        // _transform.position += Time.deltaTime * Caste.Speed * _transform.forward;
    }

    public virtual void Move()
    {
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
        // CurrentPheromone.UpdateStates(this, collision);

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
        string collisionName = collision.gameObject.name;
        ResourceType collisionResource;
        // TargetType collisionType;
        string debugString = "Ant" + ID + " collides with " + collisionName;
        if (Enum.TryParse(collisionName, out collisionResource))
        {
            debugString += ", which contains " + collisionResource;
            if (ResourceTarget.HasFlag(collisionResource))
            {
                CollidedWithTarget = true;
                Memory.DiscoverResource(collisionResource, collision.gameObject.transform.position);
                Debug.Log("Ant" + ID + " now has memory of nearest resources: " + Memory);
            }
        }

        // if (Enum.TryParse(collisionName, out collisionType))
        // {
        //     if (LocationTarget == collisionType)
        //     {
        //         CollidedWithTarget = true;
        //     }
        // }
        OnCollision(collision);
    }

    // void OnCollisionStay(Collision collision)
    // {
    //     OnCollision(collision);
    // }

    public virtual void AssignColony(Colony colony)
    {
        this.Colony = colony;
        ID = colony.NumAnts;
        Memory = new Memory(colony.Memory);
    }

    public void AssignCaste(Caste caste)
    {
        Caste = caste;
        // _pheromoneSequence = caste.PheromoneSequence;
        // this.CurrentPheromone = this._pheromoneSequence[0];
    }

    // public void SetAntState(AntState antState)
    // {
    //     AntState = antState;
    // }

    // public void SetPheromoneState(PheromoneState pheromoneState)
    // {
    //     PheromoneState = pheromoneState;
    // }

    public void SetColony(Colony colony)
    {
        Colony = colony;
    }

    public void SetMemory(Memory memory)
    {
        Memory = memory;
    }

    public void SetDirection(Vector3 direction)
    {
        _transform.forward = direction;
    }

    public void SetLocationTarget(TargetType target)
    {
        LocationTarget = target;
    }

    public void SetResourceTarget(ResourceType target)
    {
        ResourceTarget = target;
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
