using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    public Colony colony;
    public Caste caste;

    public int id;



    public AntState antState;
    public PheromoneState pheromoneState;
    private Pheromone[] pheromoneSequence;
    public Pheromone currentPheromone;
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

        // StartCoroutine(Controller());
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
        transform.forward = direction;

        // move the ant forward
        transform.position = new Vector3(transform.position[0], 0.0625f, transform.position[2]);
        transform.position += transform.forward * Time.deltaTime * this.caste.speed;
        // body.MovePosition(transform.position + transform.forward * Time.deltaTime * this.caste.speed);
    }

    // An ant's behavior might change on collision with another object
    void OnCollisionEnter(Collision collision)
    {
       Debug.Log("Ant" + id + " collided with " + collision.gameObject.name);
       // check if the current pheromone is still active
       antState = AntState.Collided;
       currentPheromone.UpdateStates(this, collision);
    }


    // private IEnumerator Controller()
    // {
        // Debug.Log("Ant" + this.id + " begins behaving!");

        // access the first pheromone
        // int currentPheromoneIndex = 0;
        // while (pheromoneSequence.Length == 0)
        // {
        //     yield return null;
        // }
        // currentPheromone = pheromoneSequence[currentPheromoneIndex];
        // currentPheromoneCoroutine = StartCoroutine(Continue());

        // begin obeying pheromones
        // while (true)
        // {
            // if (pheromoneState == PheromoneState.Complete)
            // {
                // stop the ant's current behavior
                // StopCoroutine(currentPheromoneCoroutine);

                // update the pheromone and state
                // currentPheromoneIndex = currentPheromoneIndex++ % pheromoneSequence.Length;
                // currentPheromone = pheromoneSequence[currentPheromoneIndex];
                // pheromoneState = PheromoneState.InProgress;
                
                // begin a new coroutine
                // Debug.Log("Ant" + id + " changes pheromones and begins again: " + currentPheromone.pheromoneName);
                // currentPheromoneCoroutine = StartCoroutine(Continue());
            // }

            // yield return null;
        // }
    // }

    // private IEnumerator Continue()
    // {
    //     Debug.Log("Ant" + this.id + " continues behavior: " + this.currentPheromone.pheromoneName);

    //     // continuing behavior always asks for a destination and waits for some time, both of which depend on the pheromone
    //     while (true) 
    //     {
    //         this.direction = this.currentPheromone.GetDestination(this);
    //         // Debug.Log("Ant" + this.id + " decides to travel in direction " + this.direction);
    //         float delay = this.currentPheromone.GetDelay(this);

    //         yield return new WaitForSeconds(delay);
    //     }
    // }


    public void AssignColony(Colony colony)
    {
        this.colony = colony;
        id = colony.numAnts;
    }

    public void AssignCaste(Caste caste)
    {
        this.caste = caste;
        pheromoneSequence = caste.pheromoneSequence;
        this.currentPheromone = this.pheromoneSequence[0];
        // Debug.Log("Caste Assigned " + caste.name);
    }


    void OnDrawGizmos()
    {
        // Draws a 5 unit long red line in front of the object
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * 2;
        Gizmos.DrawRay(transform.position, direction);

        Gizmos.DrawWireSphere(transform.position, antennaRadius);
    }
}
