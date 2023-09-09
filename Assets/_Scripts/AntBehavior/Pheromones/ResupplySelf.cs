using UnityEngine;

public class ResupplySelf : IPheromone
{
    public PheromoneName PheromoneName { get; set; } = PheromoneName.ResupplySelf;

    private Ant _ant;
    private Vector3 _direction;
    private Vector3 _targetPosition;
    private ResupplyState _resupplyState;
    private float _idleTime = 0.5f;
    private float _resupplyTime = 2;
    private float _timeElapsed;

    public ResupplySelf()
    {
    }

    public ResupplySelf(Ant ant)
    {
        _ant = ant;
        _targetPosition = _ant.Colony.Transform.position;
    }
    
    public void Start()
    {
        Debug.Log("RESUPPLY: " + _ant + " begins resupply!");
        ChangeState(ResupplyState.ApproachingColony);
    }

    public void Update()
    {
        if (_resupplyState == ResupplyState.Idle && _timeElapsed > _idleTime)
        {
            ChangeState(ResupplyState.ApproachingColony);
        }

        else if (_resupplyState == ResupplyState.Resupplying && _timeElapsed > _resupplyTime)
        {
            ChangeState(ResupplyState.Resupplied);
        }

        _timeElapsed += Time.deltaTime;
    }

    public void Finish()
    {
        Debug.Log("RESUPPLY: " + _ant + " ends resupply!");
        _ant.PheromoneMachine.ForceNextPheromone();
    }

    public IPheromone Copy(Ant ant)
    {
        return new ResupplySelf(_ant);
    }

    public void OnCollision(GameObject collider)
    {
        Location location;

        if (collider.TryGetComponent(out location))
        {
            Debug.Log("RESUPPLY: " + _ant + " runs into " + location.LocationType);
            if (_resupplyState == ResupplyState.ApproachingColony && location.LocationType == LocationType.Colony)
            {
                ChangeState(ResupplyState.Resupplying);
            }
        }
    }

    public override string ToString()
    {
        return "ResupplySelf";
    }

    private void ChangeState(ResupplyState newState)
    {
        switch (newState)
        {
            // case ResupplyState.Idle:
            //     _timeElapsed = 0;
            //     _ant.AntBehaviorMachine.TransitionTo(_ant.AntBehaviorMachine.Idle);
            //     Debug.Log("RESUPPLY: " + _ant + " is resupplied!");
            //     break;
            case ResupplyState.ApproachingColony:
                _ant.AntBehaviorMachine.Approach.SetTargetPosition(_targetPosition);
                _ant.AntBehaviorMachine.TransitionTo(_ant.AntBehaviorMachine.Approach);
                Debug.Log("RESUPPLY: " + _ant + " is approaching the colony!");
                break;
            case ResupplyState.Resupplying:
                _timeElapsed = 0;
                _ant.AntBehaviorMachine.TransitionTo(_ant.AntBehaviorMachine.Idle);
                Debug.Log("RESUPPLY: " + _ant + " is resupplying!");
                break;
            case ResupplyState.Resupplied:
                _ant.ConsumeColonyResources();
                Debug.Log("RESUPPLY: " + _ant + " is resupplied!");
                Finish();
                break;
            default:
                break;
        }

        _resupplyState = newState;
    }

    private enum ResupplyState
    {
        Idle,
        ApproachingColony,
        Resupplying,
        Resupplied
    }
}