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
    public AntMemory Memory { get; private set; }

    private Dictionary<ResourceType, float> _carrying = new Dictionary<ResourceType, float>();

    protected float _antennaRadius = 0.25f;
    [SerializeField] protected float _energy;
    [SerializeField] protected float _maxEnergy;
    [SerializeField] protected float _energyBurnedPerSecond;
    [SerializeField] protected float _resupplyThreshold;
    [SerializeField] protected float _metabolism;

    protected Vector3 _direction;
    protected Transform _transform;
    public Transform Transform => _transform;

    [SerializeField] private float _rotateSpeed = 10f;

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

        Vector3 originalDirection = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360), _transform.up) * _transform.forward;
        SetDirection(originalDirection);

        _maxEnergy = 200;
        _energyBurnedPerSecond = 0.1f;
        _resupplyThreshold = 20;
        _metabolism = 100;

        OnStart();
    }

    void Start()
    {
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
        UseEnergy();
        
        // PheromoneMachine determines the behavior patstern for the ant
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

    private void UseEnergy()
    {
        // Debug.Log("ANT ENERGY: " + ToString() + " has energy: " + _energy);
        if (_energy <= 0)
        {
            Debug.Log("ANT ENERGY: " + ToString() + " energy falls below 0, dying!");
            Destroy(gameObject);
        }

        _energy -= Math.Max(0, _energyBurnedPerSecond * Time.deltaTime);

        if (_energy < _resupplyThreshold && PheromoneMachine.GetCurrentPheromone() != PheromoneName.ResupplySelf)
        {
            Debug.Log("ANT ENERGY: " + ToString() + " falls below energy threshold: " + Mathf.Round(_energy) + " / " + _resupplyThreshold);
            PheromoneMachine.ForceResupply();
        }
    }

    public virtual void ConsumeColonyResources()
    {
        float requiredFood = Mathf.Max(0, (_maxEnergy - _energy) / _metabolism);
        float amount = Colony.TryTakeFood(requiredFood);
        _energy += amount * _metabolism;
        Debug.Log("ANT ENERGY: " + ToString() + " consumes colony resources, now: " + _energy + " = " + amount + " * " + _metabolism);

    }

    // An ant's behavior might change on collision with another object
    void OnCollisionEnter(Collision collision)
    {
        // Debug.Log("ANT COLLISION: " + ToString() + " collides with a " + collision.gameObject.name);

        Location location;
        LocationType locationType = LocationType.None;
        if (collision.gameObject.TryGetComponent(out location))
        {
            locationType = location.LocationType;
        }

        _pheromoneMachine.CurrentPheromone.OnCollision(collision.gameObject);
    
        if (locationType == LocationType.Colony)
        {
            Colony.Memory.Update(Memory);
            Memory.Update(Colony.Memory);
            
            Debug.Log("ANT MEMORY: " + ToString() + " updates Colony and own memory: ");

            if (Caste.HasNewSequence(_timeOfLastPheromoneChange))
            {
                _pheromoneMachine.SetPheromoneSequence(Caste.PheromoneSequence);
                _timeOfLastPheromoneChange = Time.time;
            }
        }
    }

    public virtual void AssignColony(Colony colony)
    {
        Colony = colony;
        ID = colony.NumAnts;
        Memory = new AntMemory(this);
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

    public void SetMemory(AntMemory memory)
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

    public float TryDumpResource(ResourceType resource)
    {
        if (_carrying.TryGetValue(resource, out float amount))
        {
            _carrying[resource] -= amount;
            return amount;
        }
        return 0;
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
        if (Caste != null)
        {
            return Caste.Name + "Ant" + ID;
        }
        return "Ant" + ID;
    }
}
