using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;

public class Ant : MonoBehaviour
{
    public Colony Colony { get; private set; }
    public Caste Caste { get; private set; }

    [field: SerializeField] public int ID { get; protected set; }

    public float PheromoneProgress { get; private set; } = 0;
    public Memory Memory { get; private set; }

    private Dictionary<ResourceType, float> _carrying = new Dictionary<ResourceType, float>();

    protected float _antennaRadius = 0.25f;

    protected Vector3 _direction;
    protected Transform _transform;
    public Transform Transform => _transform;

    private float _rotateSpeed = 10f;

    protected AntBehaviorMachine _antBehaviorMachine;
    public AntBehaviorMachine AntBehaviorMachine => _antBehaviorMachine;

    protected PheromoneMachine _pheromoneMachine;
    public PheromoneMachine PheromoneMachine => _pheromoneMachine;
    
    protected float _timeOfLastPheromoneChange;

    void Awake()
    {
        // Debug.Log("ANT: Empty Ant created!");
        Rigidbody body = GetComponent<Rigidbody>();
        body.freezeRotation = true;

        _transform = transform;
    }

    void Start()
    {
        OnStart();
    }

    protected virtual void OnStart()
    {
        _antBehaviorMachine = new AntBehaviorMachine(this);
        _antBehaviorMachine.Initialize(_antBehaviorMachine.Idle);

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

        Turn();
        Move();
    }

    public virtual void Turn()
    {
        float singleStep = _rotateSpeed * Time.deltaTime;
        _transform.forward = Vector3.RotateTowards(_transform.forward, _direction, singleStep, 0.0f);
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
        }

        _pheromoneMachine.CurrentPheromone.OnCollision(collision.gameObject);

        if (locationType == LocationType.Colony)
        {
            Colony.Memory.UpdateColonyMemory(Memory);
            Memory.UpdateAntMemory(Colony.Memory);
            
            if (Caste.HasNewSequence(_timeOfLastPheromoneChange))
            {
                _pheromoneMachine.UpdatePheromoneSequence(Caste.PheromoneSequence);
                _timeOfLastPheromoneChange = Time.time;
            }
        }
    }

    public virtual void AssignColony(Colony colony)
    {
        Colony = colony;
        ID = colony.NumAnts;
        Memory = new Memory(colony.Memory);
    }

    public void AssignCaste(Caste caste)
    {
        Caste = caste;
        _pheromoneMachine = new PheromoneMachine(this);
        _pheromoneMachine.Initialize(Caste.PheromoneSequence);
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
        _direction = direction;
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

    public override string ToString()
    {
        return "Ant" ;
    }
}
