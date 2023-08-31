using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

class Scout : IPheromone
{
    public ResourceType Target;

    private float _timeElapsed = 0;
    private Ant _ant;

    public Scout(ResourceType target)
    {
        Target = target;
    }

    public Scout(Ant ant, ResourceType target)
    {
        _ant = ant;
        Target = target;
    }

    public void Start()
    {
        _ant.SetResourceTarget(Target);
    }

    public void Update()
    {
        bool antWandering = _ant.AntBehaviorMachine.CurrentBehavior == _ant.AntBehaviorMachine.Wander;
        bool antIdling = _ant.AntBehaviorMachine.CurrentBehavior == _ant.AntBehaviorMachine.Idle;
        bool antApproachingColony = _ant.AntBehaviorMachine.CurrentBehavior == _ant.AntBehaviorMachine.Approach;
        if (_ant.CollidedWithTarget & antWandering)
        {
            _ant.CollidedWithTarget = false;
            _ant.SetLocationTarget(TargetType.Colony);
            // _ant.AntBehaviorMachine.Approach.SetTarget(TargetType.Colony);
            _ant.AntBehaviorMachine.TransitionTo(_ant.AntBehaviorMachine.Approach);
        }
        else if (_ant.CollidedWithTarget & antApproachingColony)
        {
            _ant.CollidedWithTarget = false;
            _ant.AntBehaviorMachine.TransitionTo(_ant.AntBehaviorMachine.Wander);
        }
        else if (_timeElapsed > 3 & antIdling)
        {
            _ant.SetResourceTarget(Target);
            _ant.AntBehaviorMachine.TransitionTo(_ant.AntBehaviorMachine.Wander);
        }

        _timeElapsed += Time.deltaTime;
    }

    public void Finish()
    {

    }

    public IPheromone Copy(Ant ant)
    {
        return new Scout(ant, Target);
    }
    
    // public override Vector3 GetDirection(Ant ant)
    // {
    //     // guesses a new direction within an arc of the ants forward direction
    //     Vector3 direction = ant.transform.forward;
    //     float guessAngle;

    //     guessAngle = UnityEngine.Random.Range(-2.5f, 2.5f);
    //     if (ant.AntState == AntState.Collided) 
    //     {
    //         guessAngle += UnityEngine.Random.Range(5f, 10f) * (UnityEngine.Random.Range(0, 2) * 2 - 1);
    //     }
    //     ant.SetAntState(AntState.Searching);

    //     Vector3 newDirection = Quaternion.AngleAxis(guessAngle, ant.transform.up) * direction;

    //     return newDirection;
    // }

    // public override void UpdateStates(Ant ant, Collision collision)
    // {        
    //     string collisionName = collision.gameObject.name;
    //     ResourceType collisionResource;
    //     ant.SetAntState(AntState.Collided);
    //     if (Enum.TryParse(collisionName, out collisionResource))
    //     {
    //         Debug.Log("Ant" + ant.ID + " collides with " + collisionName + ", which contains " + collisionResource);
    //         if (target.HasFlag(collisionResource))
    //         {
    //             ant.Memory.DiscoverResource(collisionResource, collision.gameObject.transform.position);
    //             // Debug.Log("Ant" + ant.ID + " now has memory of nearest resources: " + ant.Memory);
    //             ant.SetPheromoneState(PheromoneState.Complete);
    //             ant.SetAntState(AntState.Idle);
    //         }
    //     }
    // }
}