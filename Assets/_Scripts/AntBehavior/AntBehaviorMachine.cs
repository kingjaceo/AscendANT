using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AntBehaviorMachine
{
    public IAntBehavior CurrentBehavior { get; private set; }

    private Ant _ant;

    private WanderBehavior _wander;
    public WanderBehavior Wander => _wander;

    private ApproachBehavior _approach;
    public ApproachBehavior Approach => _approach;

    private CircleBehavior _circle;
    public CircleBehavior Circle => _circle;

    private IdleBehavior _idle;
    public IdleBehavior Idle => _idle;

    public event Action<IAntBehavior> BehaviorChanged;

    public AntBehaviorMachine(Ant ant)
    {
        _ant = ant;
        _wander = new WanderBehavior(_ant);
        _approach = new ApproachBehavior(_ant);
        _circle = new CircleBehavior(_ant);
        _idle = new IdleBehavior(_ant);
    }

    public void Initialize(IAntBehavior behavior)
    {
        Debug.Log("AntBehavior machine created!");
        
        CurrentBehavior = behavior;

        CurrentBehavior.Begin();

        BehaviorChanged?.Invoke(CurrentBehavior);
    }

    public void TransitionTo(IAntBehavior nextBehavior)
    {
        CurrentBehavior.End();
        CurrentBehavior = nextBehavior;
        CurrentBehavior.Begin();

        BehaviorChanged?.Invoke(nextBehavior);
    }

    public void Update()
    {
        if (CurrentBehavior != null)
        {
            CurrentBehavior.Update();
        }
    }

    public override string ToString()
    {
        return "AntBehaviorMachine for " + _ant.ID;
    }
}