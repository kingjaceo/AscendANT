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
        if (ant.antState == AntState.Harvesting)
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

        string debugString = "Ant" + ant.id + " collides with " + collisionName;
        // if the ant finds its target while searching
        debugString += (ant.antState == AntState.Searching) + ", ";
        debugString += Enum.TryParse(collisionName, out collisionResource);
        if (ant.antState == AntState.Searching & Enum.TryParse(collisionName, out collisionResource))
        {
            debugString += " which contains " + collisionResource;
            if (collisionResource == target)
            {
                ant.antState = AntState.Harvesting;
                ant.carrying[target] += 5;
            }
        }

        Debug.Log(debugString);


        // if the ant makes it home while harvesting
        if (ant.antState == AntState.Harvesting & collisionName == "Colony")
        {
            ant.pheromoneState = PheromoneState.Complete;
            ant.antState = AntState.Idle;
            ant.colony.ResourceAmounts[target] += ant.carrying[target];
            ant.carrying[target] -= ant.carrying[target];
        }
    }

    private Vector3 GetColonyDirection(Ant ant)
    {
        Vector3 direction = ant.colony.transform.position - ant.transform.position;
        return direction;
    }

    private Vector3 GetTargetDirection(Ant ant)
    {
        // try to search the ants memory for a destination, otherwise, go to the colony
        Vector3 destination;
        try
        {
            destination = ant.memory.nearestResource[target];
            ant.antState = AntState.Searching;
        }
        catch (KeyNotFoundException)
        {
            Debug.Log("Ant" + ant.id + " doesn't have knowledge of " + target + " and returns to the colony");
            destination = ant.colony.transform.position;
            ant.antState = AntState.Idle;
        }

        Vector3 direction = destination - ant.transform.position;
        return direction;
    }
}