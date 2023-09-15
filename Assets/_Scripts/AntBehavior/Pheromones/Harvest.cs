using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

class Harvest : IPheromone
{
    public ResourceType TargetResourceType = ResourceType.Food;

    private Resource _targetResource;
    private Vector3 _targetPosition;
    private Ant _ant;
    private HarvestState _harvestState;
    private float _timeElapsed;
    private float _errorTimeLimit = 15;

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

    private void ChangeState(HarvestState newState)
    {
        _harvestState = newState;

        switch (newState)
        {
            case HarvestState.WaitingForTarget:
                Debug.Log("HARVEST: " + _ant + " waiting for new target");
                break;
            case HarvestState.FoundTarget:
                BeginApproachTarget();
                Debug.Log("HARVEST: " + _ant + " found target");
                ChangeState(HarvestState.ApproachingTarget);
                break;
            case HarvestState.ApproachingTarget:
                _timeElapsed = 0;
                Debug.Log("HARVEST: " + _ant + " approaching target");
                break;
            case HarvestState.AtTarget:
                break;
            case HarvestState.Harvesting:
                _timeElapsed = 0;
                BeginHarvesting();
                Debug.Log("HARVEST: " + _ant + " harvesting target");
                break;
            case HarvestState.ResourceMissing:
                Debug.Log("HARVEST: " + _ant + " found resource to be missing");
                ChangeState(HarvestState.ApproachingColony);
                break;
            case HarvestState.ApproachingColony:
                BeginApproachColony();
                Debug.Log("HARVEST: " + _ant + " approaching colony");
                break;
            case HarvestState.AtColony:
                DumpFood();
                Debug.Log("HARVEST: " + _ant + " at colony");
                ChangeState(HarvestState.WaitingForTarget);
                break;
            default:
                break;
        }
    }

    public void Start()
    {
       Debug.Log("HARVEST: Harvest begins for " + _ant);
        _timeElapsed = 0;
        ChangeState(HarvestState.AtColony);

        if (TargetResourceType == ResourceType.None)
        {
            TargetResourceType = ResourceType.Food;
        }
    }

    public void Update()
    {
        // Debug.Log("HARVEST: Update tick -- " + _ant + " has " + _harvestState + ", " + _timeElapsed);
        if (_harvestState == HarvestState.WaitingForTarget)
        {
            UpdateTarget();
        }
        
        else if (_harvestState == HarvestState.ApproachingTarget)
        {
            float distance = GetDistanceToTarget();
            if (distance < 0.1f)
            {
                _ant.Memory.DiscoverDepletedResource(TargetResourceType, _ant.AntBehaviorMachine.Approach.TargetPosition);
                ChangeState(HarvestState.ResourceMissing);
            }
        }

        else if (_harvestState == HarvestState.Harvesting && _timeElapsed > 3)
        {
            TryHarvestResource();
            Debug.Log("HARVEST: " + _ant + " begins approaching colony after (failing to) harvest");
            ChangeState(HarvestState.ApproachingColony);
        }

        else if (_harvestState == HarvestState.ApproachingTarget && _timeElapsed > _errorTimeLimit)
        {
            ChangeState(HarvestState.ApproachingColony);
        }

        _timeElapsed += Time.deltaTime;
    }

    private void UpdateTarget()
    {
        // Debug.Log("HARVEST: " + _ant + " updates target...");
        Vector3 location;
        if (_ant.Memory.NearestResources.TryGetValue(TargetResourceType, out location))
        {
            _targetPosition = location;

            Debug.Log("HARVEST: " + _ant + " finds target " + location);
            ChangeState(HarvestState.FoundTarget);
        }
    }

    private void TryHarvestResource()
    {
        Debug.Log("HARVEST: " + _ant + "tries to harvest resource");
        try
        {
            float amountHarvested = _targetResource.Harvest(_ant.Caste.HarvestAmount);
            _ant.CarryResource(TargetResourceType, amountHarvested);
        }
        catch (MissingReferenceException)
        {
            Debug.Log("RESOURCE: " + _ant + " attempts to harvest resource that does not exist");
            _ant.Memory.DiscoverDepletedResource(TargetResourceType, _ant.AntBehaviorMachine.Approach.TargetPosition);
            ChangeState(HarvestState.ResourceMissing);
        }
    }

    private void BeginApproachTarget()
    {
        _ant.AntBehaviorMachine.Approach.SetTargetPosition(_targetPosition);
        _ant.AntBehaviorMachine.TransitionTo(_ant.AntBehaviorMachine.Approach);
    }

    private void BeginApproachColony()
    {
        _ant.AntBehaviorMachine.Approach.SetTargetPosition(_ant.Colony.Transform.position);
        _ant.AntBehaviorMachine.TransitionTo(_ant.AntBehaviorMachine.Approach);
    }

    private void BeginHarvesting()
    {
        _ant.AntBehaviorMachine.TransitionTo(_ant.AntBehaviorMachine.Idle);
    }

    private float GetDistanceToTarget()
    {
        Vector3 diff = _targetPosition - _ant.Transform.position;
        float distance = diff.sqrMagnitude;
        return distance;
    }

    public void Finish()
    {
        Debug.Log("HARVEST: Harvest ends for " + _ant);
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
            if (_harvestState == HarvestState.ApproachingTarget && location.LocationType == LocationType.Resource)
            {
                ResourceType colliderResourceType = collider.GetComponent<Resource>().ResourceType;
                if ((TargetResourceType & colliderResourceType) == colliderResourceType)
                {
                    _targetResource = collider.GetComponent<Resource>();
                    ChangeState(HarvestState.Harvesting);
                }
            }

            if (_harvestState == HarvestState.ApproachingColony & location.LocationType == LocationType.Colony)
            {
                ChangeState(HarvestState.AtColony);
            }
        }
    }

    private void DumpFood()
    {
        float amount = _ant.TryDumpResource(TargetResourceType);
        _ant.Colony.ColonyResources.AddResource(ResourceType.Food, amount);
    }

    private enum HarvestState
    {
        WaitingForTarget,
        FoundTarget,
        ApproachingTarget,
        AtTarget,
        Harvesting,
        ResourceMissing,
        ApproachingColony,
        AtColony,
    }

    public override string ToString()
    {
        return "Harvest";
    }
}