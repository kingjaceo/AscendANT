using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Ant
{
    private float timeToLayEgg;
    private float timeSinceLastEgg;

    void Awake()
    {
        Debug.Log("Queen created!");
        Rigidbody body = GetComponent<Rigidbody>();
        body.freezeRotation = true;
        _transform = transform;
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        timeToLayEgg = 10f;
    }

    // Update is called once per frame
    public override void Update()
    {
        Vector3 radius = _transform.position - Colony.transform.position;
        Vector2 radius2D = new Vector2(radius.x, radius.z);
        Vector2 newDirection2D = Vector2.Perpendicular(radius2D);

        _direction = new Vector3(newDirection2D.x, _transform.forward.y, newDirection2D.y);

        float radiusLength = radius.magnitude;
        if (radiusLength > 5f)
        {
            _direction = Quaternion.AngleAxis(Random.Range(-10f, 5f), _transform.up) * _direction;
        }
        else if (radiusLength < 3f)
        {
            _direction = Quaternion.AngleAxis(Random.Range(5f, 10f), _transform.up) * _direction;
        }
        _transform.forward = _direction;
        _transform.position += Time.deltaTime * Caste.Speed * _transform.forward;

        Debug.Log("Queen moves in " + _direction);

        timeSinceLastEgg += Time.deltaTime;

        if (timeSinceLastEgg > timeToLayEgg) 
        {
            LayEgg();
            timeSinceLastEgg = 0;
        }
    }

    private void LayEgg()
    {
        Colony.ResourceAmounts[ResourceType.Eggs]++;
    }

    public new void AssignColony(Colony colony)
    {
        
        SetColony(colony);
        SetMemory(new Memory(colony.Memory));
    }
}