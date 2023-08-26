using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Caste
{ 
    public string name;
    public float speed;
    public Pheromone[] pheromoneSequence;
    
    public Caste(string name, float speed, Pheromone[] pheromoneSequence)
    {
        this.speed = speed;

        this.name = name;

        this.pheromoneSequence = pheromoneSequence;
    }
}
