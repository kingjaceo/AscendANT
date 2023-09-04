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

    public float PheromoneProgress { get; private set; } = 0;
    public Memory Memory { get; private set; }

    private Dictionary<ResourceType, float> _carrying = new Dictionary<ResourceType, float>();

    protected float _antennaRadius = 0.25f;

    protected Vector3 _direction;
    protected Transform _transform;
    public Transform Transform => _transform;
    [field: SerializeField] public LocationType LocationTarget { get; private set; }

    protected AntBehaviorMachine _antBehaviorMachine;
    public AntBehaviorMachine AntBehaviorMachine => _antBehaviorMachine;

    protected PheromoneMachine _pheromoneMachine;
    public PheromoneMachine PheromoneMachine => _pheromoneMachine;

    void Awake()
    {
        Debug.Log("Ant created!");
        Rigidbody body = GetComponent<Rigidbody>();
        body.freezeRotation = true;

        _transform = transform;
    }

    public virtual void Start()
    {
        _antBehaviorMachine = new AntBehaviorMachine(this);
        _antBehaviorMachine.Initialize(_antBehaviorMachine.Idle);
        _pheromoneMachine = new PheromoneMachine(this);
        _pheromoneMachine.Initialize(Caste.PheromoneSequence);

        foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
        {
            _carrying[resourceType] = 0;
        }
    }

    public virtual void Update()
    {
        // PheromoneMachine determines the behavior pattern for the ant
        _pheromoneMachine.Update();
        // AntBehaviorMachine determines the execution of the current specific behavior
        _antBehaviorMachine.Update();

        Move();
    }

    public virtual void Move()
    {
        _transform.position += Time.deltaTime * Caste.Speed * _transform.forward;
    }

    // An ant's behavior might change on collision with another object
    void OnCollisionEnter(Collision collision)
    {
        Location location;
        LocationType locationType = LocationType.None;
        if (collision.gameObject.TryGetComponent(out location))
        {
            locationType = location.LocationType;
            Debug.Log("Ant" + ID + " collides with " + locationType);
        }

        _pheromoneMachine.CurrentPheromone.OnCollision(collision.gameObject);

        if (locationType == LocationType.Colony)
        {
            // Debug.Log("Ant" + ID + " memory before update: " + Memory);
            // Debug.Log("Colony memory before update: " + Memory);
            Colony.Memory.UpdateColonyMemory(Memory);
            Memory.UpdateAntMemory(Colony.Memory);
            // Debug.Log("Ant" + ID + " memory after update: " + Memory);
            // Debug.Log("Colony memory after update: " + Memory);
        }
    }

    public virtual void AssignColony(Colony colony)
    {
        this.Colony = colony;
        ID = colony.NumAnts;
        Memory = new Memory(colony.Memory);
    }

    public void AssignCaste(Caste caste)
    {
        Caste = caste;
    }

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

    public void SetLocationTarget(LocationType target)
    {
        LocationTarget = target;
    }

    public void CarryResource(ResourceType resource, float amount)
    {
        _carrying[resource] += amount;
    }

    public float DumpResource(ResourceType resource)
    {
        float amount = _carrying[resource];
        _carrying[resource] -= amount;
        return amount;
    }

    void OnDrawGizmos()
    {
        // Draws a 5 unit long red line in front of the object
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * 2;
        Gizmos.DrawRay(transform.position, direction);

        Gizmos.DrawWireSphere(transform.position, _antennaRadius);
    }
}
