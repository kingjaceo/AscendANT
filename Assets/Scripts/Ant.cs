using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;

public class Ant : MonoBehaviour
{
    public Colony colony;
    public Caste caste;

    public int id;
    public AntState antState;
    public PheromoneState pheromoneState;
    public Pheromone currentPheromone;
    public Memory memory;

    public Dictionary<ResourceType, float> carrying = new Dictionary<ResourceType, float>();

    private Pheromone[] pheromoneSequence;
    private int currentPheromoneIndex;
    private Pheromone previousFramePheromone;
    private Coroutine currentPheromoneCoroutine;
    private float antennaRadius = 0.25f;
    private Vector3 direction;


    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Ant created!");
        Rigidbody body = GetComponent<Rigidbody>();
        body.freezeRotation = true;
    }

    void Start()
    {
        transform.position = colony.transform.position;

        antState = AntState.Idle;
        pheromoneState = PheromoneState.InProgress;
        currentPheromoneIndex = 0;

        foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
        {
            carrying[resourceType] = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // update current pheromone
        if (pheromoneState == PheromoneState.Complete)
        {
            currentPheromoneIndex = (currentPheromoneIndex+1) % pheromoneSequence.Length;
            pheromoneState = PheromoneState.InProgress;
            Debug.Log("Pheromone on Ant" + id + " changes to: " + pheromoneSequence[currentPheromoneIndex]);
        }
        currentPheromone = pheromoneSequence[currentPheromoneIndex];

        // update direction
        direction = currentPheromone.GetDirection(this);
        // randomly rotate the direction by a few degrees
        direction = Quaternion.AngleAxis(UnityEngine.Random.Range(-5f, 5f), transform.up) * direction;
        // direction = colony.transform.position - transform.position;
        transform.forward = direction;

        // move the ant forward
        transform.position = new Vector3(transform.position[0], 0.0625f, transform.position[2]);
        transform.position += transform.forward * Time.deltaTime * this.caste.speed;
    }

    void OnCollision(Collision collision)
    {
        if (collision.gameObject.name == "World")
        {
            return;
        }
        
        // check if the current pheromone is still active
        // antState = AntState.Collided;
        currentPheromone.UpdateStates(this, collision);

        // if the ant touched the colony, update its memory
        if (collision.gameObject.name == "Colony")
        {
            Debug.Log("Ant" + id + " memory before update: " + memory);
            Debug.Log("Colony memory before update: " + memory);
            colony.memory.UpdateColonyMemory(memory);
            memory.UpdateAntMemory(colony.memory);
            Debug.Log("Ant" + id + " memory after update: " + memory);
            Debug.Log("Colony memory after update: " + memory);
        }
    }

    // An ant's behavior might change on collision with another object
    void OnCollisionEnter(Collision collision)
    {
        OnCollision(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        OnCollision(collision);
    }

    public void AssignColony(Colony colony)
    {
        this.colony = colony;
        id = colony.numAnts;
        memory = new Memory(colony.memory);
    }

    public void AssignCaste(Caste caste)
    {
        this.caste = caste;
        pheromoneSequence = caste.pheromoneSequence;
        this.currentPheromone = this.pheromoneSequence[0];
        // Debug.Log("Caste Assigned " + caste.name);
    }

    // public void MergeMemory()
    // {
    //     colony.memory.nearestResource = new Dictionary<ResourceType, Vector3>(memory.nearestResource);
    //     Debug.Log("Ant" + id + " shares memory with Colony, colony now has memory of nearest resource: " + colony.memory);
    //     colony.memory.resourceLocations = new Dictionary<ResourceType, List<Vector3>>(memory.resourceLocations);
    // }

    // private void UpdateMemory()
    // {
    //     memory.nearestResource = new Dictionary<ResourceType, Vector3>(colony.memory.nearestResource);
    //     memory.resourceLocations = new Dictionary<ResourceType, List<Vector3>>(colony.memory.resourceLocations);
    // }

    void OnDrawGizmos()
    {
        // Draws a 5 unit long red line in front of the object
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * 2;
        Gizmos.DrawRay(transform.position, direction);

        Gizmos.DrawWireSphere(transform.position, antennaRadius);
    }
}
