using System;
using System.Collections.Generic;
using UnityEngine;

public class AntMemory
{
    private Ant _ant;
    private Dictionary<ResourceType, Vector3> _nearestResources = new Dictionary<ResourceType, Vector3>();
    public Dictionary<ResourceType, Vector3> NearestResources => _nearestResources;

    private ResourceType _discoveredResourceType = ResourceType.None;
    public ResourceType DiscoveredResourceType => _discoveredResourceType;
    private Vector3 _discoveredResourceLocation;
    public Vector3 DiscoveredResourceLocation => _discoveredResourceLocation;

    private ResourceType _depletedResourceType = ResourceType.None;
    public ResourceType DepletedResourceType => _depletedResourceType;
    private Vector3 _depletedResourceLocation;
    public Vector3 DepletedResourceLocation => _depletedResourceLocation;

    public AntMemory(Ant ant)
    {
        _ant = ant;
    }

    public void DiscoverResource(ResourceType resourceType, Vector3 location)
    {
        _discoveredResourceType = resourceType;
        _discoveredResourceLocation = location;
    }

    public void DiscoverDepletedResource(ResourceType resourceType, Vector3 location)
    {
        _depletedResourceType = resourceType;
        _depletedResourceLocation = location;
    }

    public void SetNearest(ResourceType resourceType, Vector3 location)
    {
        Debug.Log("ANTMEMORY: " + _ant + " has nearest " + resourceType + " at " + location);
        _nearestResources[resourceType] = location;
    }

    public void RemoveNearest(ResourceType resourceType)
    {
        _nearestResources.Remove(resourceType);
    }

    public void Update(ColonyMemory colonyMemory)
    {
        _discoveredResourceType = ResourceType.None;
        _depletedResourceType = ResourceType.None;

        Vector3 location;
        foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
        {
            if (colonyMemory.NearestResources.TryGetValue(resourceType, out location))
            {
                Debug.Log("ANTMEMORY: " + _ant + " has nearest " + resourceType + " is at " + location);
                _nearestResources[resourceType] = location;
            }
        }
    }
}