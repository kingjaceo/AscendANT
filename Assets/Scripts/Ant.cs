using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    public Colony colony;
    public Caste caste;

    public Pheromone currentPheromone;
    public int currentPheromoneIndex;

    public Vector3 destination;
    public Vector3 guess;

    // current pheremone state
    public bool nothingFound;
    public bool reporting;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Ant created!");
        this.destination = new Vector3(0, 1, 0);
        speed = 0.001f;
        transform.position = new Vector3(0, 0.25f, 0);
    }

    public void AssignColony(Colony colony)
    {
        this.colony = colony;
    }

    void Start()
    {
        StartCoroutine(Scout());
    }

    // Update is called once per frame
    void Update()
    {
        // point the ant in the right direction 
        Vector3 direction = (this.destination - transform.position).normalized;
        if (direction[1] != 0)
        {
            direction = new Vector3(0.5f, 0, 0.5f);
        }
        transform.up = direction;

        // move the ant forward
        transform.position = Vector3.MoveTowards(transform.position, this.destination, speed);
    }

    // An ant's behavior might only change when it collides with another object
    void OnCollisionEnter(Collision collision)
    {
       if (!this.currentPheromone.CheckActive())
       {
        // end the current coroutine
        StopCoroutine(this.currentPheromoneCoroutine)

        // access the next pheromone, increment index
        this.currentPheromone = this.caste.NextPheromone(this.currentPheromoneIndex);
        this.currentPheromoneIndex = this.currentPheromoneIndex % this.caste.pheromoneSequence.length;

        // start the next pheromone
        this.currentPheromoneCoroutine = StartCoroutine(this.currentPheromone.Start());
       }

    }

    IEnumerator Report()
    {
        while (true)
        {
            this.destination = colony.transform.position;
            yield return null;
        }
    }
}
