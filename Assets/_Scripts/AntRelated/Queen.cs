using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Ant
{
    private float _timeToLayEgg = 10f;
    public float TimeToLayEgg => _timeToLayEgg;
    private float _timeSinceLastEgg = 0f;
    public float TimeSinceLastEgg => _timeSinceLastEgg;

    void Awake()
    {
        Debug.Log("Queen created!");
        Rigidbody body = GetComponent<Rigidbody>();
        body.freezeRotation = true;
        _transform = transform;
        ID = -1;
        base.OnStart();
        _energyBurnedPerSecond = 0;
    }

    // Start is called before the first frame update
    public void Start()
    {
        _timeToLayEgg = 10f;  
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        _timeSinceLastEgg += Time.deltaTime;

        if (_timeSinceLastEgg > _timeToLayEgg) 
        {
            LayEgg();
            _timeSinceLastEgg = 0;
        }
    }

    private void LayEgg()
    {
        Colony.ColonyResources.AddResource(ResourceType.Eggs, 1);
    }

    public new void AssignColony(Colony colony)
    {
        SetColony(colony);
    }
}
