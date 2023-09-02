using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Caste
{ 
    public string Name;
    public float Speed { get; private set; }
    public List<IPheromone> PheromoneSequence { get; private set; }
    public float Percentage { get; private set; }
    public float HarvestAmount { get; private set; }
    
    public Caste(string name, float percentage, float speed, List<IPheromone> pheromoneSequence)
    {
        Speed = speed;

        Name = name;
        
        Percentage = percentage;

        PheromoneSequence = pheromoneSequence;

        HarvestAmount = 5;
    }

    public void SetPercentage(float percentage)
    {
        Percentage = percentage;
    }

}
