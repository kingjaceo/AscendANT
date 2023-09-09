using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

class Harvest : IPheromone
{
    public ResourceType TargetResourceType = ResourceType.None;

    private Resource _targetResource;
    private Vector3 _targetPosition;
    private Ant _ant;
    private bool _hasTargetResourcePosition = false;
    private HarvestState _harvestState;
    private float _timeElapsed;

    public PheromoneName PheromoneName { get; set; } = PheromoneName.Harvest;

    public Harvest()
    {}
    
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
       Debug.Log("PHEROMONE: Harvest begins for " + _ant);
        _timeElapsed = 0;
        _harvestState = HarvestState.AtColony;

        if (TargetResourceType == ResourceType.None)
        {
            TargetResourceType = ResourceType.Food;
        }
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

            TryHarvestResource();
        }

        _timeElapsed += Time.deltaTime;
    }

    public void Finish()
    {
        Debug.Log("PHEROMONE: Harvest ends for " + _ant);
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
            if (_ant.Memory.TrySetNearest(TargetResourceType))
            {
                location = _ant.Memory.nearestResource[TargetResourceType];
                _targetPosition = location;
                _hasTargetResourcePosition = true;
            }
        }

        if (_harvestState == HarvestState.AtColony)
        {
            _ant.Memory.UpdateAntMemory(_ant.Colony.Memory);
        }
    }

    private void TryHarvestResource()
    {
        try
        {
            float amountHarvested = _targetResource.Harvest(_ant.Caste.HarvestAmount);
            _ant.CarryResource(TargetResourceType, amountHarvested);
        }
        catch (MissingReferenceException)
        {
            Debug.Log("RESOURCE: " + _ant + " attempts to harvest resource that does not exist");
            _ant.Memory.RememberDepletedResource(TargetResourceType);
        }
    }

    private void DumpFood()
    {
        float amount = _ant.DumpResource(TargetResourceType);
        _ant.Colony.ColonyResources.AddResource(ResourceType.Food, amount);
    }

    private enum HarvestState
    {
        ApproachingTargetResource,
        Harvesting,
        ApproachingColony,
        AtColony,
    }

    public override string ToString()
    {
        return "Harvest";
    }
}