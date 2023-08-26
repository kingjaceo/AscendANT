using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Memory
{
    public Colony colony;
    public Dictionary<ResourceType, Vector3> nearestResource = new Dictionary<ResourceType, Vector3>();
    public Dictionary<ResourceType, List<Vector3>> resourceLocations = new Dictionary<ResourceType, List<Vector3>>();
    public Dictionary<ResourceType, float> colonyResources = new Dictionary<ResourceType, float>();
    public List<ResourceType> updatedResources = new List<ResourceType>();

    public Memory(Colony colony)
    {
        this.colony = colony;
        colonyResources = colony.ResourceAmounts;
        foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
        {
            resourceLocations[resourceType] = new List<Vector3>();
        }
    }

    public Memory(Memory otherMemory)
    {
        colony = otherMemory.colony;
        foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
        {
            if (otherMemory.nearestResource.ContainsKey(resourceType))
            {
                nearestResource[resourceType] = otherMemory.nearestResource[resourceType];
            }
        
            resourceLocations[resourceType] = new List<Vector3>(otherMemory.resourceLocations[resourceType]);
        }
    }

    public void AddResourceLocation(ResourceType resourceType, Vector3 resourceLocation)
    {
        // update nearest resource
        nearestResource[resourceType] = resourceLocation;
        resourceLocations[resourceType].Add(resourceLocation);
        // TODO: nearest resource gets updated correctly
        // TODO: only add to resourceLocations if the location isn't in the list already
    }

    public Memory ShallowCopy()
    {
        return (Memory) this.MemberwiseClone();
    }

    public override string ToString()
    {
        string returnString = "Nearest Resource {";
        foreach(KeyValuePair<ResourceType, Vector3> entry in nearestResource)
        {
            returnString += " " + entry.Key + "->" + entry.Value + ";";
        }
        returnString += " }";
        return returnString;
    }

    // AntInfoToColonyMemory
    public void UpdateColonyMemory(Memory antMemory)
    {
        // add information to the colony memory
        foreach (ResourceType resourceType in antMemory.updatedResources)
        {
            UpdateResourceLocations(antMemory, resourceType);
        }

        // reset ant Memory information
        antMemory.updatedResources = new List<ResourceType>();
    }

    // ColonyInfoToAntMemory
    public void UpdateAntMemory(Memory colonyMemory)
    {
        foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
        {
            UpdateResourceLocations(colonyMemory, resourceType);
        }

        colonyResources = new Dictionary<ResourceType, float>(colonyMemory.colonyResources);
    }

    public void UpdateResourceLocations(Memory otherMemory, ResourceType resourceType)
    {
        try
        {
            Vector3 resourceLocation = otherMemory.resourceLocations[resourceType].Last();
            AddResourceLocation(resourceType, resourceLocation);
        }
        catch (InvalidOperationException ex)
        {
            Debug.LogException(ex);
        }
    }

    public void DiscoverResource(ResourceType resourceType, Vector3 resourceLocation)
    {
        updatedResources.Add(resourceType);
        AddResourceLocation(resourceType, resourceLocation);
    }
}