using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Scout : Pheromone
{
    public string target;
    public bool redirect = false;

    public override PheromoneName pheromoneName { get; set; }

    public Scout(string target)
    {
        pheromoneName = PheromoneName.Scout;

        this.target = target;
        Debug.Log("Scout pheromone created!");
    }
    
    public override Vector3 GetDirection(Ant ant)
    {
        // guesses a new direction within an arc of the ants forward direction
        Vector3 direction = ant.transform.forward;
        float guessAngle;

        guessAngle = Random.Range(-2.5f, 2.5f);
        if (ant.antState == AntState.Collided) 
        {
            guessAngle = Random.Range(90f, 100f);
            guessAngle = guessAngle * (Random.Range(0, 2) * 2 - 1);
        }
        ant.antState = AntState.Searching;

        Vector3 newDirection = Quaternion.AngleAxis(guessAngle, ant.transform.up) * direction;

        return newDirection;
    }

    public override float GetDelay(Ant ant)
    {
        // should calculate the time it will take to travel to the destination
        return 0.5f;
    }

    public override void UpdateStates(Ant ant, Collision collision)
    {        
        Debug.Log("Ant" + ant.id + " collides with " + collision.gameObject.name);
        if (collision.gameObject.name == target)
        {
            ant.pheromoneState = PheromoneState.Complete;
            ant.antState = AntState.Idle;
        }
    }
}