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
    
    public Caste(string name, float speed, List<IPheromone> pheromoneSequence)
    {
        Speed = speed;

        Name = name;
        
        PheromoneSequence = pheromoneSequence;

        HarvestAmount = 5;
    }

    public void SetPercentage(float percentage)
    {
        // Debug.Log("Percentage set to " + percentage);
        Percentage = percentage;
    }

    public void SetPheromone(PheromoneName pheromoneName, int pheromoneIndex)
    {
        // reference a static global "AllPheromones" list attribute
        PheromoneSequence[pheromoneIndex] = AllPheromones.Instance.PheromonesByName[pheromoneName];
    }

}
