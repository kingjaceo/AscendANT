using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

class Scout : IPheromone
{
    private float _timeElapsed;
    private Ant _ant;
    private ScoutState _scoutState;

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
        _scoutState = ScoutState.None;
        _timeElapsed = 0;
    }

    public void Update()
    {
        if (_scoutState == ScoutState.None)
        {
            _ant.AntBehaviorMachine.TransitionTo(_ant.AntBehaviorMachine.Idle);
            _scoutState = ScoutState.Idle;
        }
        
        // Ant should begin scouting
        if (_scoutState == ScoutState.Idle & _timeElapsed > 1)
        {
            _ant.AntBehaviorMachine.TransitionTo(_ant.AntBehaviorMachine.Wander);
        }

        _timeElapsed += Time.deltaTime;
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

                _ant.AntBehaviorMachine.Approach.SetTargetPosition(_ant.Colony.Transform.position);
                _ant.AntBehaviorMachine.TransitionTo(_ant.AntBehaviorMachine.Approach);
                _scoutState = ScoutState.ReturningHome;
            }

            if (location.LocationType == LocationType.Colony)
            {
                // Ant should begin idling
                _timeElapsed = 0;
                _ant.AntBehaviorMachine.TransitionTo(_ant.AntBehaviorMachine.Idle);
                _scoutState = ScoutState.Idle;
            }
        }

    }

    private enum ScoutState
    {
        None,
        Idle,
        ReturningHome,
        Wandering
    }

    public override string ToString()
    {
        return "Scout";
    }
}