using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

class Scout : IPheromone
{
    private float _timeElapsed = 0;
    private Ant _ant;

    public Scout()
    {
    }

    public Scout(Ant ant)
    {
        _ant = ant;
    }

    public void Start()
    {

    }

    public void Update()
    {
        bool antIdling = _ant.AntBehaviorMachine.CurrentBehavior == _ant.AntBehaviorMachine.Idle;

        // Ant should begin scouting
        if (antIdling & _timeElapsed > 1)
        {
            _ant.AntBehaviorMachine.TransitionTo(_ant.AntBehaviorMachine.Wander);
        }

        _timeElapsed += Time.deltaTime;
    }

    public void Finish()
    {

    }

    public IPheromone Copy(Ant ant)
    {
        return new Scout(ant);
    }

    // public void CollidedWithTarget(GameObject target)
    // {
    //     if (_ant.LocationTarget == LocationType.Resource)
    //     {
    //         // Ant should discover the resource and begin approaching the colony
    //         ResourceType resourceType = target.GetComponent<Resource>().ResourceType;
    //         _ant.Memory.DiscoverResource(resourceType, target.transform.position);

    //         _ant.SetLocationTarget(LocationType.Colony);
    //         _ant.AntBehaviorMachine.Approach.SetTargetPosition(_ant.Colony.Transform.position);
    //         _ant.AntBehaviorMachine.TransitionTo(_ant.AntBehaviorMachine.Approach);
    //     }

    //     else if (_ant.LocationTarget == LocationType.Colony)
    //     {
    //         // Ant should begin idling
    //         _timeElapsed = 0;
    //         _ant.SetLocationTarget(LocationType.Resource);
    //         _ant.AntBehaviorMachine.TransitionTo(_ant.AntBehaviorMachine.Idle);
    //     }
    // }

    public void OnCollision(GameObject collider)
    {
        Location location;

        if (collider.TryGetComponent(out location))
        {
            if (location.LocationType == LocationType.Resource)
            {
                // Ant should discover the resource and begin approaching the colony
                ResourceType resourceType = collider.GetComponent<Resource>().ResourceType;
                _ant.Memory.DiscoverResource(resourceType, collider.transform.position);

                _ant.AntBehaviorMachine.Approach.SetTargetPosition(_ant.Colony.Transform.position);
                _ant.AntBehaviorMachine.TransitionTo(_ant.AntBehaviorMachine.Approach);
            }

            if (location.LocationType == LocationType.Colony)
            {
                // Ant should begin idling
                _timeElapsed = 0;
                _ant.AntBehaviorMachine.TransitionTo(_ant.AntBehaviorMachine.Idle);
            }
        }

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

    public override string ToString()
    {
        return "Scout";
    }
}