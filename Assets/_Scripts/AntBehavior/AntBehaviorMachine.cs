using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AntBehaviorMachine
{
    public IAntBehavior CurrentBehavior { get; private set; }

    private WanderBehavior _wander;
    public WanderBehavior Wander => _wander;

    private ApproachBehavior _approach;
    public ApproachBehavior Approach => _approach;

    // private CircleBehavior _circle;
    // public CircleBehavior Circle => _circle;

    private IdleBehavior _idle;
    public IdleBehavior Idle => _idle;

    public event Action<IAntBehavior> BehaviorChanged;

    public AntBehaviorMachine(Ant ant)
    {
        _wander = new WanderBehavior(ant);
        _approach = new ApproachBehavior(ant);
        // _circle = new CircleBehavior(ant);
        _idle = new IdleBehavior(ant);
    }

    public void Initialize(IAntBehavior behavior)
    {
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
}