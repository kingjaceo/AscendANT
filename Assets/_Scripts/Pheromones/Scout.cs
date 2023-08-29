using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

class Scout : Pheromone
{
    public ResourceType target;

    public override PheromoneName pheromoneName { get; set; }

    public Scout(ResourceType target)
    {
        pheromoneName = PheromoneName.Scout;

        this.target = target;
        Debug.Log("Scout pheromone created with target: " + target);
    }
    
    public override Vector3 GetDirection(Ant ant)
    {
        // guesses a new direction within an arc of the ants forward direction
        Vector3 direction = ant.transform.forward;
        float guessAngle;

        guessAngle = UnityEngine.Random.Range(-2.5f, 2.5f);
        if (ant.AntState == AntState.Collided) 
        {
            guessAngle += UnityEngine.Random.Range(5f, 10f) * (UnityEngine.Random.Range(0, 2) * 2 - 1);
        }
        ant.SetAntState(AntState.Searching);

        Vector3 newDirection = Quaternion.AngleAxis(guessAngle, ant.transform.up) * direction;

        return newDirection;
    }

    public override void UpdateStates(Ant ant, Collision collision)
    {        
        string collisionName = collision.gameObject.name;
        ResourceType collisionResource;
        ant.SetAntState(AntState.Collided);
        if (Enum.TryParse(collisionName, out collisionResource))
        {
            Debug.Log("Ant" + ant.ID + " collides with " + collisionName + ", which contains " + collisionResource);
            if (target.HasFlag(collisionResource))
            {
                ant.Memory.DiscoverResource(collisionResource, collision.gameObject.transform.position);
                // Debug.Log("Ant" + ant.ID + " now has memory of nearest resources: " + ant.Memory);
                ant.SetPheromoneState(PheromoneState.Complete);
                ant.SetAntState(AntState.Idle);
            }
        }
    }
}