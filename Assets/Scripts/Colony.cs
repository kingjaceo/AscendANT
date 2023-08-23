using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colony : MonoBehaviour
{ 
    public int numAnts = 0;

    public GameObject ant;
    public GameObject[] ants;

    public Caste[] castes;

    public Memory memory;
    public Dictionary<ResourceType, float> resourceAmounts = new Dictionary<ResourceType, float>();
    public float hatchProgress = 0;

    // Start is called before the first frame update
    void Awake()
    {
        InitializeColony();
        InitializeCastes();
        StartCoroutine(InitializeAnts());
    }

    // Update is called once per frame
    void Start()
    {
        StartCoroutine(HatchEggs());
    }

    // initalizes the colony according to default parameters
    private void InitializeColony()
    {
        resourceAmounts[ResourceType.Food] = 100f;
        resourceAmounts[ResourceType.Water] = 100f;
        resourceAmounts[ResourceType.Eggs] = 3f;
        memory = new Memory(this);
    }

    private void InitializeCastes()
    {
        ResourceType target = ResourceType.Food | ResourceType.Curiosity;
        Scout scout = new Scout(target);
        Report report = new Report();
        Harvest harvest = new Harvest(ResourceType.Food);
        TendColony tendColony = new TendColony();

        Pheromone[] scoutPheromoneSequence = {scout, report};

        Pheromone[] workerPheromoneSequence = { harvest };

        Pheromone[] attendantPheromoneSequence = { tendColony };
        
        Caste caste0 = new Caste("Attendant", 1f, attendantPheromoneSequence);
        Caste caste1 = new Caste("Scout", 2f, scoutPheromoneSequence);
        Caste caste2 = new Caste("Worker", 3f, workerPheromoneSequence);

        Caste[] castes = { caste0, caste1, caste2 };
        this.castes = castes;

        // Debug.Log("Caste 0: " + pheromoneSequence[0].name);
    }

    // initializes the default number and types of ants
    private IEnumerator InitializeAnts()
    {
        int[] amountOfAntsInCastes = { 1, 7, 2 };
        int numCastes = castes.Length;
        ants = new GameObject[10];

        int k = 0;

        for (int i = 0; i < numCastes; i++)
        {
            Caste caste = castes[i];
            int amountOfAntsInCaste = amountOfAntsInCastes[i];

            GameObject[] antsInCaste = new GameObject[amountOfAntsInCaste];
            
            for (int j = 0; j < amountOfAntsInCaste; j++) {
                GameObject newAnt = Instantiate(ant);
                Ant antObject = newAnt.GetComponent<Ant>();
                ants[k] = newAnt;
                k++;
                antObject.AssignColony(this);
                antObject.AssignCaste(caste);
                numAnts ++;
                antsInCaste[j] = newAnt;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public void TendEggs()
    {
        float tendAmount = 0.1f;

        if (resourceAmounts[ResourceType.Food] > 10)
        {
            resourceAmounts[ResourceType.Food] -= tendAmount;
            hatchProgress += tendAmount;
        }
    }

    IEnumerator HatchEggs()
    {
        while(true)
        {
            if (hatchProgress > 100) {
                // reduce eggs and reset hatchProgress
                resourceAmounts[ResourceType.Eggs]--;
                hatchProgress = 0;

                // produce a new ant
                NextAnt();
            }

            yield return null;
        }
    }

    private void NextAnt()
    {
        Caste caste = castes[1];
        GameObject newAnt = Instantiate(ant);
        Ant antObject = newAnt.GetComponent<Ant>();
        antObject.AssignColony(this);
        antObject.AssignCaste(caste);
        numAnts ++;
    }
}
