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
        
        CurrentPheromone = _pheromoneSequence[0];

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
        foreach (IPheromone pheromone in pheromones)
        {
            Debug.Log("PHEROMONE: Setting pheromone for " + _ant + " " + pheromone.ToString());
            _pheromoneSequence.Add(pheromone.Copy(_ant));
        }
    }

    public void UpdatePheromoneSequence(List<IPheromone> pheromones)
    {
        Debug.Log("PHEROMONE: Updating pheromone list for " + _ant + " to " + pheromones[0]);

        for (int i = 0; i < pheromones.Count; i++)
        {
            if (pheromones[i].PheromoneName != _pheromoneSequence[i].PheromoneName)
            {
                _pheromoneSequence[i] = pheromones[i].Copy(_ant);
            } 
        } 

        CurrentPheromone.Finish();
        CurrentPheromone = _pheromoneSequence[0];
        CurrentPheromone.Start();
    }
}