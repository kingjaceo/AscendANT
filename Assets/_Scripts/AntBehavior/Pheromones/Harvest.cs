using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

class Harvest : IPheromone
{
    public ResourceType TargetResourceType;

    private Resource _targetResource;
    private Vector3 _targetPosition;
    private Ant _ant;
    private bool _hasTargetResourcePosition = false;
    private HarvestState _harvestState;
    private float _timeElapsed;

    public Harvest(ResourceType target)
    {
        TargetResourceType = target;
    }

    public Harvest(ResourceType target, Ant ant)
    {
        TargetResourceType = target;
        _ant = ant;
    }

    public void Start()
    {
        _timeElapsed = 0;
        _ant.SetLocationTarget(LocationType.Resource);
        _harvestState = HarvestState.AtColony;
    }

    public void Update()
    {
        UpdateTarget();

        // ant approaches target
        if (_hasTargetResourcePosition & _harvestState == HarvestState.AtColony)
        {
            _harvestState = HarvestState.ApproachingTargetResource;
            _ant.AntBehaviorMachine.Approach.SetTargetPosition(_targetPosition);
            _ant.AntBehaviorMachine.TransitionTo(_ant.AntBehaviorMachine.Approach);

            DumpFood();
        }

        // ant returns home
        if (_harvestState == HarvestState.Harvesting & _timeElapsed > 3)
        {
            _harvestState = HarvestState.ApproachingColony;
            _targetPosition = _ant.Colony.Transform.position;
            _ant.AntBehaviorMachine.Approach.SetTargetPosition(_targetPosition);
            _ant.AntBehaviorMachine.TransitionTo(_ant.AntBehaviorMachine.Approach);

            HarvestResource();
        }

        _timeElapsed += Time.deltaTime;
    }

    public void Finish()
    {

    }

    public IPheromone Copy(Ant ant)
    {
        return new Harvest(TargetResourceType, ant);
    }

    public void OnCollision(GameObject collider)
    {
        Location location;

        if (collider.TryGetComponent(out location))
        {
            // if the collider is a colony
            if (_harvestState == HarvestState.ApproachingColony & location.LocationType == LocationType.Colony)
            {
                Debug.Log("Ant" + _ant.ID + " arrives back at the colony");
                _harvestState = HarvestState.AtColony;
                _ant.AntBehaviorMachine.TransitionTo(_ant.AntBehaviorMachine.Approach);
                _timeElapsed = 0;
            }

            // if the collider is a target resource
            if (_harvestState == HarvestState.ApproachingTargetResource & location.LocationType == LocationType.Resource)
            {
                // if the collider is the target resource
                ResourceType colliderResourceType = collider.GetComponent<Resource>().ResourceType;
                bool targetResourceFound = (TargetResourceType & colliderResourceType) == colliderResourceType;
                if (targetResourceFound)
                {
                    _harvestState = HarvestState.Harvesting;
                    _ant.AntBehaviorMachine.TransitionTo(_ant.AntBehaviorMachine.Idle);
                    _targetResource = collider.GetComponent<Resource>();
                    _timeElapsed = 0;
                }
            }
        }
    }

    private void UpdateTarget()
    {
        if ((TargetResourceType & ResourceType.Food) ==  ResourceType.Food)
        {
            Vector3 location;
            if (_ant.Memory.nearestResource.TryGetValue(ResourceType.Food, out location))
            {
                _targetPosition = location;
                _hasTargetResourcePosition = true;
            }
        }

        if (_harvestState == HarvestState.AtColony)
        {
            _ant.Memory.UpdateAntMemory(_ant.Colony.Memory);
        }
    }

    private void HarvestResource()
    {
        float amountHarvested = _targetResource.Harvest(_ant.Caste.HarvestAmount);
        _ant.CarryResource(TargetResourceType, amountHarvested);
    }

    private void DumpFood()
    {
        float amount = _ant.DumpResource(TargetResourceType);
        _ant.Colony.AddFood(amount);
    }

    private enum HarvestState
    {
        ApproachingTargetResource,
        Harvesting,
        ApproachingColony,
        AtColony,
    }
}