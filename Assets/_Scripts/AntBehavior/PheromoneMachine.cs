using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

[Serializable]
public class PheromoneMachine
{
    public IPheromone CurrentPheromone { get; private set; }

    private Ant _ant;

    private List<IPheromone> _pheromoneSequence = new List<IPheromone>();
    private int _currentPheromoneIndex;

    public event Action<IPheromone> PheromoneChanged;

    public PheromoneMachine(Ant ant)
    {
        _ant = ant;
    }

    public void Initialize(List<IPheromone> pheromones)
    {
        foreach (IPheromone pheromone in pheromones)
        {
            Debug.Log("Adding pheromone " + pheromone.ToString());
            _pheromoneSequence.Add(pheromone.Copy(_ant));
        }
        
        CurrentPheromone = _pheromoneSequence[0];

        CurrentPheromone.Start();

        PheromoneChanged?.Invoke(CurrentPheromone);
    }

    // public void NextPheromone()
    // {
    //     _currentPheromoneIndex++;

    //     CurrentPheromone.Finish();
    //     CurrentPheromone = _pheromoneSequence[_currentPheromoneIndex];
    //     CurrentPheromone.Start();

    //     PheromoneChanged?.Invoke(CurrentPheromone);
    // }

    public void Update()
    {
        if (CurrentPheromone != null)
        {
            CurrentPheromone.Update();
        }
    }
}