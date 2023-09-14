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

    // public event Action<IPheromone> PheromoneChanged;

    public PheromoneMachine(Ant ant)
    {
        _ant = ant;
    }

    public void Initialize(List<IPheromone> pheromones)
    {
        _currentPheromoneIndex = 0;
        SetPheromoneSequence(pheromones);
        CurrentPheromone = _pheromoneSequence[_currentPheromoneIndex];
        CurrentPheromone.Start();
        // PheromoneChanged?.Invoke(CurrentPheromone);
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
        _pheromoneSequence = new List<IPheromone>();

        foreach (IPheromone pheromone in pheromones)
        {
            Debug.Log("PHEROMONE MACHINE: Setting pheromone for " + _ant + " " + pheromone.ToString());
            _pheromoneSequence.Add(pheromone.Copy(_ant));
        }

        Debug.Log("PHEROMONE MACHINE: Adding ResupplySelf for " + _ant);
        _pheromoneSequence.Add(new ResupplySelf(_ant));
    }

    public void ForceResupply()
    {
        CurrentPheromone?.Finish();
        
        _currentPheromoneIndex = _pheromoneSequence.Count - 1;
        CurrentPheromone = _pheromoneSequence[_currentPheromoneIndex];
        Debug.Log("PHEROMONE MACHINE: Forced resupply on " + _ant + " ... , " + CurrentPheromone.ToString() + ", " + _currentPheromoneIndex);

        CurrentPheromone.Start();
    }

    public void ForceNextPheromone()
    {
        CurrentPheromone?.Finish();
        
        NextPheromone();
        Debug.Log("PHEROMONE MACHINE: Forced next pheromone for " + _ant + " ... , " + CurrentPheromone + " at " + _currentPheromoneIndex);
    }

    public void NextPheromone()
    {
        Debug.Log("PHEROMONE MACHINE: " +_ant + " has current pheromone index: " + _currentPheromoneIndex);
        
        _currentPheromoneIndex++;
        _currentPheromoneIndex %= _pheromoneSequence.Count;
        CurrentPheromone = _pheromoneSequence[_currentPheromoneIndex];

        Debug.Log("PHEROMONE MACHINE: " +_ant + " starts " + CurrentPheromone.ToString() + " index: " + _currentPheromoneIndex);
        CurrentPheromone.Start();
    }
    
    public PheromoneName GetCurrentPheromone()
    {
        return CurrentPheromone.PheromoneName;
    }
}