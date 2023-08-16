using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caste
{ 
    public int speed;
    public Pheromone[] pheromoneSequence;
    
    public Caste(int speed)
    {
        this.speed = speed;

        Scout scout = new Scout("Curiosity");
        pheromoneSequence = { scout };
    }
}
