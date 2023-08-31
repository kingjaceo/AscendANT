using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PheromoneMachine
{
    public IPheromone CurrentPheromone { get; private set; }

    private List<IPheromone> _pheromoneSequence = new List<IPheromone>();
    private int _currentPheromoneIndex;

    public event Action<IPheromone> PheromoneChanged;

    public PheromoneMachine(Ant ant)
    {

    }

    public void Initialize(List<IPheromone> pheromones, Ant ant)
    {
        foreach (IPheromone pheromone in pheromones)
        {
            _pheromoneSequence.Add(pheromone.Copy(ant));
        }
        
        CurrentPheromone = _pheromoneSequence[0];

        CurrentPheromone.Start();

        PheromoneChanged?.Invoke(CurrentPheromone);
    }

    public void NextPheromone()
    {
        _currentPheromoneIndex++;

        CurrentPheromone.Finish();
        CurrentPheromone = _pheromoneSequence[_currentPheromoneIndex];
        CurrentPheromone.Start();

        PheromoneChanged?.Invoke(CurrentPheromone);
    }

    public void Update()
    {
        if (CurrentPheromone != null)
        {
            CurrentPheromone.Update();
        }
    }
}