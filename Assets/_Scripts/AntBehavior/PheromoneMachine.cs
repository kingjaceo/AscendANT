using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        SetPheromoneSequence(pheromones);
        
        _currentPheromoneIndex = 0;
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

    public void SetPheromoneSequence(List<IPheromone> pheromones)
    {
        CurrentPheromone?.Finish();
        
        _pheromoneSequence = new List<IPheromone>();

        foreach (IPheromone pheromone in pheromones)
        {
            // Debug.Log("PHEROMONE: Setting pheromone for " + _ant + " " + pheromone.ToString());
            _pheromoneSequence.Add(pheromone.Copy(_ant));
        }

        pheromones.Add(new ResupplySelf(_ant));

        CurrentPheromone?.Start();
    }

    public void ForceResupply()
    {
        CurrentPheromone?.Finish();
        
        _currentPheromoneIndex = _pheromoneSequence.Count - 1;
        CurrentPheromone = _pheromoneSequence[_currentPheromoneIndex];
        Debug.Log("PHEROMONE MACHINE: Forced resupply ... , " + CurrentPheromone.ToString());

        CurrentPheromone.Start();
    }

    public void ForceNextPheromone()
    {
        CurrentPheromone?.Finish();
        
        _currentPheromoneIndex = _currentPheromoneIndex++ % _pheromoneSequence.Count;
        CurrentPheromone = _pheromoneSequence[_currentPheromoneIndex];
        Debug.Log("PHEROMONE MACHINE: Forced next pheromone ... , " + CurrentPheromone + " at " + _currentPheromoneIndex);

        CurrentPheromone.Start();
    }
    
    public PheromoneName GetCurrentPheromone()
    {
        return CurrentPheromone.PheromoneName;
    }
}