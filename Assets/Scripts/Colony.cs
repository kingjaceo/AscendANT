using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colony : MonoBehaviour
{
    public int water;
    public int food;
    public int eggs;
    public int eggHatchTime;
    
    public int numAnts = 0;

    public GameObject ant;
    public GameObject[] ants;

    public Caste[] castes;

    // Start is called before the first frame update
    void Awake()
    {
        InitializeColony();
        InitializeCastes();
        InitializeAnts();
    }

    // Update is called once per frame
    void Update()
    {
        // hatchEggs();
    }

    // initalizes the colony according to default parameters
    private void InitializeColony()
    {
        this.water = 100;
        this.food = 100;
        this.eggs = 3;
        this.eggHatchTime = 10;
    }

    private void InitializeCastes()
    {
        Scout scout = new Scout("Curiosity");
        Pheromone[] pheromoneSequence = {scout};
        
        Caste caste0 = new Caste("Attendant", 1f, pheromoneSequence);
        Caste caste1 = new Caste("Scout", 2f, pheromoneSequence);
        Caste caste2 = new Caste("Soldier", 3f, pheromoneSequence);

        Caste[] castes = { caste0, caste1, caste2 };
        this.castes = castes;

        // Debug.Log("Caste 0: " + pheromoneSequence[0].name);
    }

    // initializes the default number and types of ants
    private void InitializeAnts()
    {
        int[] amountOfAntsInCastes = { 1, 7, 2 };
        int numCastes = this.castes.Length;
        this.ants = new GameObject[10];

        int k = 0;

        for (int i = 0; i < numCastes; i++)
        {
            Caste caste = this.castes[i];
            int amountOfAntsInCaste = amountOfAntsInCastes[i];

            GameObject[] antsInCaste = new GameObject[amountOfAntsInCaste];
            
            for (int j = 0; j < amountOfAntsInCaste; j++) {
                GameObject newAnt = Instantiate(ant);
                Ant antObject = newAnt.GetComponent<Ant>();
                this.ants[k] = newAnt;
                k++;
                antObject.AssignColony(this);
                antObject.AssignCaste(caste);
                this.numAnts ++;
                antsInCaste[j] = newAnt;
            }
        }

         this.ants = ants;
    }
}
