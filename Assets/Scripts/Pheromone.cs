using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Pheromone
{
    abstract public PheromoneName pheromoneName { get; set; }
    
    public Pheromone()
    {
        pheromoneName = PheromoneName.Pheromone;
    }
    public virtual Vector3 GetDirection(Ant ant)
    {
        Debug.Log("WARNING: Default Pheromone behavior GetDestination() called, which  does nothing!");
        return ant.transform.position;
    }

    public virtual float GetDelay(Ant ant)
    {
        return 1f;
    }
    
    public virtual void UpdateStates(Ant ant, Collision collision)
    {

    }
}