using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllPheromones
{
    public static AllPheromones Instance;

    public List<IPheromone> Pheromones = new List<IPheromone>();
    public Dictionary<PheromoneName, IPheromone> PheromonesByName = new Dictionary<PheromoneName,IPheromone>();
    
    public AllPheromones()
    {
        Instance = this;

        CreatePheromones();
    }

    private void CreatePheromones()
    {
        IPheromone scout = new Scout();
        Pheromones.Add(scout);
        PheromonesByName[PheromoneName.Scout] = scout;

        IPheromone harvest = new Harvest();
        Pheromones.Add(harvest);
        PheromonesByName[PheromoneName.Harvest] = harvest;

        IPheromone tendColony = new TendColony();
        Pheromones.Add(tendColony);
        PheromonesByName[PheromoneName.TendEggs] = tendColony;
    }
} 