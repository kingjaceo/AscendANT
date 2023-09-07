using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColonyResources
{
    public Dictionary<ResourceType, float> ResourceAmounts;
    public Dictionary<ResourceType, float> ResourceCapacities;

    public UnityEvent ResourceIncreased = new UnityEvent();

    public ColonyResources()
    {

    }

    public ColonyResources(Dictionary<ResourceType, float> startingResources, Dictionary<ResourceType, float> startingCapacities)
    {
        ResourceAmounts = startingResources;
        ResourceCapacities = startingCapacities;
    }

    public float Amount(ResourceType resourceType)
    {
        return ResourceAmounts[resourceType];
    }

    public void AddResource(ResourceType resourceType, float amount)
    {
        ResourceAmounts[resourceType] = Math.Min(ResourceAmounts[resourceType] + amount, ResourceCapacities[resourceType]);
        Debug.Log("COLONYRESOURCES: Invoking ResourceIncreased UnityEvent");
        ResourceIncreased?.Invoke();
    }

    public void DepleteResource(ResourceType resourceType, float amount)
    {
        ResourceAmounts[resourceType] = Math.Max(ResourceAmounts[resourceType] - amount, 0);
    }
}