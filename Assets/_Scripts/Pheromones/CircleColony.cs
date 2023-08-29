using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

class CircleColony : Pheromone
{
    public override PheromoneName pheromoneName { get; set; }
    
    private float _maxProgress;

    public CircleColony()
    {
        pheromoneName = PheromoneName.CircleColony;

        Debug.Log("CircleColony pheromone created!");
    }

    public override Vector3 GetDirection(Ant ant)
    {
        // // guesses a new direction within an arc of the ants forward direction
        Vector3 direction = ant.transform.forward;
        // float guessAngle;

        // guessAngle = UnityEngine.Random.Range(0, 0.1f);
        // if (ant.AntState == AntState.Collided) 
        // {
        //     guessAngle -= UnityEngine.Random.Range(2.5f, 5f);
        // }
        // ant.SetAntState(AntState.Idle);

        // Vector3 newDirection = Quaternion.AngleAxis(guessAngle, ant.transform.up) * direction;

        return direction;
    }

    public override float UpdatePheromoneProgress(Ant ant)
    {
        return ant.PheromoneProgress + 1;
    }
}