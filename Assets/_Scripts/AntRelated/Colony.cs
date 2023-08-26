using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colony : MonoBehaviour
{ 
    public int NumAnts = 0;

    [SerializeField] private GameObject _antPrefab;
    private List<Ant> _ants;
    private Caste[] _castes;

    private Transform _transform;

    public Memory Memory;
    public Dictionary<ResourceType, float> ResourceAmounts = new Dictionary<ResourceType, float>();
    public float HatchProgress = 0;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("New Colony starts up...");
        _transform = transform;

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
        ResourceAmounts[ResourceType.Food] = 100f;
        ResourceAmounts[ResourceType.Water] = 100f;
        ResourceAmounts[ResourceType.Eggs] = 3f;
        Memory = new Memory(this);
        // queen = Instantiate(queen);
        // queen.AssignColony(this);
    }

    private void InitializeCastes()
    {
        ResourceType target = ResourceType.Food | ResourceType.Curiosity;
        Scout scout = new Scout(target);
        Report report = new Report();
        Harvest harvest = new Harvest(ResourceType.Food);
        TendColony tendColony = new TendColony();

        Pheromone[] attendantPheromoneSequence = { tendColony };

        Pheromone[] scoutPheromoneSequence = { scout, report };

        Pheromone[] workerPheromoneSequence = { harvest };
        
        Caste caste0 = new Caste("Attendant", 1f, attendantPheromoneSequence);
        Caste caste1 = new Caste("Scout", 2f, scoutPheromoneSequence);
        Caste caste2 = new Caste("Worker", 3f, workerPheromoneSequence);

        Caste[] castes = { caste0, caste1, caste2 };
        _castes = castes;

        // Debug.Log("Caste 0: " + pheromoneSequence[0].name);
    }

    // initializes the default number and types of ants
    private IEnumerator InitializeAnts()
    {
        int[] amountOfAntsInCastes = { 1, 17, 2 };

        for (int i = 0; i < _castes.Length; i++) 
        {
            for (int j = 0; j < amountOfAntsInCastes[i]; j++)
            {
                NextAnt(_castes[i]);

                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public Vector3 RandomLocationAroundColony(float minRadius, float maxRadius)
    {
        var x = _transform.position.x + (UnityEngine.Random.Range(0, 2) * 2 - 1) * UnityEngine.Random.Range(minRadius, maxRadius);
        var y = 1f;
        var z = _transform.position.z + (UnityEngine.Random.Range(0, 2) * 2 - 1) * UnityEngine.Random.Range(minRadius, maxRadius);

        return new Vector3(x, y, z);
    }

    public void TendEggs()
    {
        float tendAmount = 0.1f;

        if (ResourceAmounts[ResourceType.Food] > 10)
        {
            ResourceAmounts[ResourceType.Food] -= tendAmount;
            HatchProgress += tendAmount;
        }
    }

    IEnumerator HatchEggs()
    {
        while(true)
        {
            if (HatchProgress > 100 & ResourceAmounts[ResourceType.Eggs] > 0) {
                // reduce eggs and reset hatchProgress
                ResourceAmounts[ResourceType.Eggs]--;
                HatchProgress = 0;

                // produce a new ant
                NextAnt(_castes[2]);
            }

            yield return null;
        }
    }

    private void NextAnt(Caste caste)
    {
        // create the Ant
        Ant ant = Instantiate(_antPrefab).GetComponent<Ant>();

        // set the Ant's parent
        ant.transform.parent = transform;

        // set the Ant's position, colony, and caste
        ant.transform.position = RandomLocationAroundColony(0f, 1f);
        ant.AssignColony(this);
        ant.AssignCaste(caste);

        // update colony variables
        NumAnts++;
        // _ants.Add(ant);
    }
}
