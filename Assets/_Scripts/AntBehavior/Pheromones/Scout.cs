using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

class Scout : IPheromone
{
    private float _timeElapsed;
    private Ant _ant;
    private ScoutState _scoutState;
    private ScoutState _previousState;

    public PheromoneName PheromoneName { get; set; } = PheromoneName.Scout;

    public Scout()
    {
    }

    public Scout(Ant ant)
    {
        _ant = ant;
    }

    public void Start()
    {
        Debug.Log("PHEROMONE: Scout begins for " + _ant);
        ChangeState(ScoutState.Idle);
        _timeElapsed = 0;
    }

    public void Update()
    {      
        // Ant should begin scouting
        if (_scoutState == ScoutState.Idle && _timeElapsed > 1)
        {
            ChangeState(ScoutState.Wandering);
        }

        if (_scoutState == ScoutState.Collided && _timeElapsed > 1)
        {
            ChangeState(_previousState);
        }

        _timeElapsed += Time.deltaTime;
    }

    private void ChangeState(ScoutState newState)
    {
        _scoutState = newState;
        switch (newState)
        {
            case ScoutState.Wandering:
                _ant.AntBehaviorMachine.TransitionTo(_ant.AntBehaviorMachine.Wander);
                Debug.Log("SCOUT: " + _ant + " begins wandering");
                break;
            case ScoutState.DiscoveringResource:
                break;
            case ScoutState.ReturningHome:
                _ant.AntBehaviorMachine.Approach.SetTargetPosition(_ant.Colony.Transform.position);
                _ant.AntBehaviorMachine.TransitionTo(_ant.AntBehaviorMachine.Approach);
                Debug.Log("SCOUT: " + _ant + " finds a resource and is returning home");
                break;
            case ScoutState.Idle:
                _timeElapsed = 0;
                _ant.AntBehaviorMachine.TransitionTo(_ant.AntBehaviorMachine.Idle);
                Debug.Log("SCOUT: " + _ant + " is waiting to begin scouting again");
                break;
            case ScoutState.Collided:
                _ant.AntBehaviorMachine.TransitionTo(_ant.AntBehaviorMachine.Avoid);
                _timeElapsed = 0;
                break;
            default:
                break;
        }
    }

    public void Finish()
    {
        Debug.Log("PHEROMONE: Scout ends for " + _ant);
    }

    public IPheromone Copy(Ant ant)
    {
        return new Scout(ant);
    }

    public void OnCollision(GameObject collider)
    {
        Location location;

        if (collider.TryGetComponent(out location))
        {
            if (location.LocationType == LocationType.Resource)
            {
                // Ant should discover the resource and begin approaching the colony
                ResourceType resourceType = collider.GetComponent<Resource>().ResourceType;
                _ant.Memory.DiscoverResource(resourceType, collider.transform.position);

                ChangeState(ScoutState.ReturningHome);
            }

            if (_scoutState == ScoutState.ReturningHome && location.LocationType == LocationType.Colony)
            {
                // Ant should begin idling
                ChangeState(ScoutState.Idle);
            }
        }
        else if (_scoutState != ScoutState.Collided && _scoutState != ScoutState.ReturningHome)
        {
            _previousState = _scoutState;
            ChangeState(ScoutState.Collided);
        }

    }

    private enum ScoutState
    {
        None,
        Idle,
        ReturningHome,
        Wandering,
        DiscoveringResource,
        Collided
    }

    public override string ToString()
    {
        return "Scout";
    }
}