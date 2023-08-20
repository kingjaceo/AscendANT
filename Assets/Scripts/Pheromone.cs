using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pheromone
{
    public bool active;
    public string name = "Pheromone";
    
    public virtual Vector3 GetDestination(Ant ant)
    {
        Debug.Log("WARNING: Default Pheromone behavior GetDestination() called, which  does nothing!");
        return ant.transform.position;
    }

    public virtual float GetDelay(Ant ant)
    {
        return 1f;
    }
    
    public virtual bool CheckActive(Ant ant, Collision collision)
    {
        return true;
    }
}