using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    public Colony colony;
    public Caste caste;

    public int id;

    public Pheromone[] pheromoneSequence;
    public Pheromone currentPheromone;
    public int currentPheromoneIndex = 0;
    public Coroutine currentPheromoneCoroutine;
    public bool pheromoneChanged = true;
    public bool alive = true;

    public Vector3 direction;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Ant created!");
        this.direction = new Vector3(0, 1, 0);
    }

    public void AssignColony(Colony colony)
    {
        this.colony = colony;
        this.id = colony.numAnts;
    }

    public void AssignCaste(Caste caste)
    {
        this.caste = caste;
        this.pheromoneSequence = this.caste.pheromoneSequence;
        this.currentPheromone = this.pheromoneSequence[0];
        Debug.Log("Caste Assigned " + caste.name);
    }

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        StartCoroutine(Controller());
    }

    // Update is called once per frame
    void Update()
    {
        // point the ant in the right direction 
        // if (direction[1] != 0)
        // {
        //     direction = new Vector3(0.5f, 0, 0.5f);
        // }
        transform.forward = this.direction;
        Rigidbody body = GetComponent<Rigidbody>();
        Debug.Log("Ant" + this.id + " faces: " + direction);

        // move the ant forward
        // Vector3 newPosition
        transform.position = new Vector3(transform.position[0], 0, transform.position[2]);
        transform.position += transform.forward * Time.deltaTime * this.caste.speed;

        // body.MovePosition(transform.position + transform.forward * Time.deltaTime * this.caste.speed);
    }

    // An ant's behavior might only change when it collides with another object
    void OnCollisionEnter(Collision collision)
    {
       // check if the current pheromone is still active
        this.pheromoneChanged = (!this.currentPheromone.CheckActive(this, collision));
        if (this.pheromoneChanged)
        {
            // stop the ant's current behavior
            StopCoroutine(this.currentPheromoneCoroutine);

            // update the current pheromone to the next pheromone
            this.currentPheromoneIndex = this.currentPheromoneIndex + 1;
            this.currentPheromone = this.pheromoneSequence[currentPheromoneIndex];
        }
    }

    IEnumerator Controller()
    {
        Debug.Log("Ant" + this.id + " begins behaving!");

        // begin obeying the first pheromone
        int currentPheromoneIndex = 0;
        this.currentPheromone = this.pheromoneSequence[currentPheromoneIndex];

        while (this.alive)
        {
            // begin pheromone coroutine
            if (this.pheromoneChanged)
            {
                Debug.Log("Ant" + this.id + " changes pheromones and begins again: " + this.currentPheromone.GetType());
                this.currentPheromoneCoroutine = StartCoroutine(Continue());
                this.pheromoneChanged = false;
            }

            yield return null;
        }
    }

    IEnumerator Continue()
    {
        Debug.Log("Ant" + this.id + " continues behavior: " + this.currentPheromone.name);

        // continuing behavior always asks for a destination and waits for some time, both of which depend on the pheromone
        while (true) 
        {
            this.direction = this.currentPheromone.GetDestination(this);
            Debug.Log("Ant" + this.id + " decides to travel in direction " + this.direction);
            float delay = this.currentPheromone.GetDelay(this);

            yield return new WaitForSeconds(delay);
        }
    }

    void OnDrawGizmos()
    {
        // Draws a 5 unit long red line in front of the object
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * 5;
        Gizmos.DrawRay(transform.position, direction);
    }
}
