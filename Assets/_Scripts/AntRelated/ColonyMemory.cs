using System;
using System.Collections.Generic;
using UnityEngine;

public class ColonyMemory
{
    private Vector3 _colonyPosition;
    private Dictionary<ResourceType, Vector3> _nearestResources = new Dictionary<ResourceType, Vector3>();
    public Dictionary<ResourceType, Vector3> NearestResources => _nearestResources;
    private Dictionary<ResourceType, List<Vector3>> _resourceLocations = new Dictionary<ResourceType, List<Vector3>>();
    
    public ColonyMemory(Vector3 colonyPosition)
    {
        _colonyPosition = colonyPosition;

        foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
        {
            _resourceLocations[resourceType] = new List<Vector3>();
        }
    }

    public void Update(AntMemory antMemory)
    {
        if (antMemory.DiscoveredResourceType != ResourceType.None)
        {
            if (!_resourceLocations[antMemory.DiscoveredResourceType].Contains(antMemory.DiscoveredResourceLocation))
            {
                Debug.Log("COLONY MEMORY: Colony adds " + antMemory.DiscoveredResourceType + " at " + antMemory.DiscoveredResourceLocation + " to memory!");

                _resourceLocations[antMemory.DiscoveredResourceType].Add(antMemory.DiscoveredResourceLocation);
                UpdateNearest(antMemory.DiscoveredResourceType);
            }
        }

        if (antMemory.DepletedResourceType != ResourceType.None)
        {
            Debug.Log("COLONY MEMORY: Colony removes " + antMemory.DepletedResourceType + " at " + antMemory.DepletedResourceLocation + " to memory!");
            _resourceLocations[antMemory.DepletedResourceType].Remove(antMemory.DepletedResourceLocation);
            UpdateNearest(antMemory.DepletedResourceType);
        }
    }

    public void UpdateNearest(ResourceType resourceType)
    {
        Vector3 nearestPosition = new Vector3(1000, 1000, 1000);
        float minDistance = 100000;
        float distance;
        bool nearestFound = false;
        // Debug.Log("COLONY MEMORY: Updating nearest " + resourceType);
        foreach (Vector3 position in _resourceLocations[resourceType])
        {
            // Debug.Log("COLONY MEMORY: Considering resource position " + position + " vs colony position " + _colonyPosition);
            distance = (position - _colonyPosition).sqrMagnitude;
            if (distance < minDistance)
            {
                nearestPosition = position;
                minDistance = distance;
                nearestFound = true;
            }
        }

        if (nearestFound)
        {
            _nearestResources[resourceType] = nearestPosition;
            Debug.Log("COLONY MEMORY: Nearest " + resourceType + " is at " + nearestPosition);
        }
        else
        {
            _nearestResources.Remove(resourceType);
        }
    }

    // public Vector3 TryGetNearest(ResourceType resourceType)
    // {
    //     return _nearestResources[resourceType];
    // }
}