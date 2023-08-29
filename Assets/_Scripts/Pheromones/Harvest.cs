using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

class Harvest : Pheromone
{
    public ResourceType target;

    public override PheromoneName pheromoneName { get; set; }

    public Harvest(ResourceType target)
    {
        this.target = target;
    }

    public override Vector3 GetDirection(Ant ant) 
    {
        if (ant.AntState == AntState.Harvesting)
        {
            return GetColonyDirection(ant);
        }
        else
        {
            return GetTargetDirection(ant);
        }        
    }

    public override void UpdateStates(Ant ant, Collision collision)
    {
        string collisionName = collision.gameObject.name;
        ResourceType collisionResource;

        string debugString = "Ant" + ant.ID + " collides with " + collisionName;
        // if the ant finds its target while searching
        debugString += (ant.AntState == AntState.Searching) + ", ";
        debugString += Enum.TryParse(collisionName, out collisionResource);
        if (ant.AntState == AntState.Searching & Enum.TryParse(collisionName, out collisionResource))
        {
            debugString += " which contains " + collisionResource;
            if (collisionResource == target)
            {
                ant.SetAntState(AntState.Harvesting);
                ant.Carrying[target] += 5;
            }
        }

        Debug.Log(debugString);


        // if the ant makes it home while harvesting
        if (ant.AntState == AntState.Harvesting & collisionName == "Colony")
        {
            ant.SetPheromoneState(PheromoneState.Complete);
            ant.SetAntState(AntState.Idle);
            ant.Colony.ResourceAmounts[target] += ant.Carrying[target];
            ant.Carrying[target] -= ant.Carrying[target];
        }
    }

    private Vector3 GetColonyDirection(Ant ant)
    {
        Vector3 direction = ant.Colony.transform.position - ant.transform.position;
        return direction;
    }

    private Vector3 GetTargetDirection(Ant ant)
    {
        // try to search the ants memory for a destination, otherwise, go to the colony
        Vector3 destination;
        try
        {
            destination = ant.Memory.nearestResource[target];
            ant.SetAntState(AntState.Searching);
        }
        catch (KeyNotFoundException)
        {
            Debug.Log("Ant" + ant.ID + " doesn't have knowledge of " + target + " and returns to the colony");
            destination = ant.Colony.transform.position;
            ant.SetAntState(AntState.Idle);
        }

        Vector3 direction = destination - ant.transform.position;
        return direction;
    }
}