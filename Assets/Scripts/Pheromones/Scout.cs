using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scout : Pheromone
{
    public string target;
    public bool redirect = false;

    public Scout(string target)
    {
        this.target = target;
        this.name = "Scout";
        Debug.Log("Scout pheromone created!");
    }
    
    public override Vector3 GetDestination(Ant ant)
    {
        // guesses a new direction within an arc of the ants forward direction
        Vector3 direction = ant.transform.forward;
        float guessAngle;
        if (this.redirect)
        {
            guessAngle = Random.Range(120f, 240f);
            Debug.Log("Ant" + ant.id + " is redirecting and chooses angle " + guessAngle);
            this.redirect = false;
        }
        else guessAngle = Random.Range(-60f, 60f);

        Vector3 newDirection = Quaternion.AngleAxis(guessAngle, ant.transform.up) * direction;

        ant.direction = newDirection;
        return newDirection;
    }

    public override float GetDelay(Ant ant)
    {
        // should calculate the time it will take to travel to the destination
        return 0.5f;
    }
    
    // public static IEnumerator Activate(Ant ant)
    // {
    //     Vector3 guess;
    //     Debug.Log("Ant begins scouting!");
    //     while (true)
    //     {
    //         // guess a destination and wander
    //         float guessRadius = 3;
    //         float xGuess = Random.Range(-guessRadius, guessRadius + 1);
    //         float zGuess = Random.Range(-guessRadius, guessRadius + 1);
    //         guess = new Vector3(xGuess, 0, zGuess);
    //         ant.destination = ant.transform.position + guess;
    //         float wanderTime = 0.5f;

    //         Debug.Log("Ant" + ant.id + "wanders toward " + guess);
    //         yield return new WaitForSeconds(wanderTime + Random.Range(0f, 0.1f));
    //     }
    // }

    public override bool CheckActive(Ant ant, Collision collision)
    {
        active = true; 
        this.redirect = true;

        // this pheromone should pass control to the next pheromone whenever the ant collides with the target
        // if (collision.gameObject.name == "Curiosity(Clone)")
        // {
        //     active = false;
        //     Debug.Log("Ant " + ant.id + " found a curiosity!");
        // }

        return active;
    }
}