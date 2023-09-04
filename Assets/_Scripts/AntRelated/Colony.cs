using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Colony : MonoBehaviour
{ 
    public int NumAnts = 0;

    public Memory Memory;
    public Dictionary<ResourceType, float> ResourceAmounts = new Dictionary<ResourceType, float>();
    public float HatchProgress = 0;

    [SerializeField] private GameObject _antPrefab;
    private List<Ant> _ants;
    public int[] AntCountByCaste;
    public List<Ant>[] AntsByCaste;
    public Caste[] Castes { get; private set;}

    [SerializeField] private GameObject _queenPrefab;
    private Queen _queen;
    public Queen Queen => _queen;

    private Transform _transform;
    public Transform Transform => _transform;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("New Colony starts up...");
        _transform = transform;

        InitializeColony();
        InitializeCastes();
        InitializeQueen();
        StartCoroutine(InitializeAnts());
        StartCoroutine(HatchEggs());
    }

    // Update is called once per frame
    void Start()
    {
        MainGUIPanel.Instance.GiveColony(this);
        ConnectCastesToGUI();
    }

    // initalizes the colony according to default parameters
    private void InitializeColony()
    {
        ResourceAmounts[ResourceType.Food] = 100f;
        ResourceAmounts[ResourceType.Water] = 100f;
        ResourceAmounts[ResourceType.Eggs] = 3f;
        Memory = new Memory(this);        
    }

    private void InitializeCastes()
    {
        // ResourceType target = ResourceType.Food | ResourceType.Curiosity;
        Scout scout = new Scout();
        Harvest harvest = new Harvest(ResourceType.Food);
        TendColony tendColony = new TendColony();

        List<IPheromone> attendantPheromoneSequence = new List<IPheromone> { tendColony };

        List<IPheromone> scoutPheromoneSequence = new List<IPheromone> { scout };

        List<IPheromone> workerPheromoneSequence = new List<IPheromone> { harvest };
        
        Caste caste0 = new Caste("Attendant", 0.1f, 1f, attendantPheromoneSequence);
        Caste caste1 = new Caste("Scout", 0.4f, 2f, scoutPheromoneSequence);
        Caste caste2 = new Caste("Worker", 0.5f, 3f, workerPheromoneSequence);

        Caste[] castes = { caste0, caste1, caste2 };

        Castes = castes;
    }

    private void InitializeQueen()
    {
        // CircleColony circle = new CircleColony();
        List<IPheromone> queenPheromoneSequence = new List<IPheromone>();
        Caste casteQ = new Caste("Queen", 0f, 0.25f, queenPheromoneSequence);
       
        _queen = Instantiate(_queenPrefab).GetComponent<Queen>();
        _queen.AssignColony(this);

        // set the Ant's parent
        _queen.Transform.parent = transform;

        // set the Ant's position, colony, and caste
        _queen.Transform.position = RandomLocationAroundColony(0.5f, 1.5f);
        _queen.Transform.GetChild(0).GetComponent<Renderer>().material.color = PaletteManager.Colors[PaletteManager.Colors.Count - 1];
        _queen.AssignColony(this);
        _queen.AssignCaste(casteQ);
    }

    // initializes the default number and types of ants
    private IEnumerator InitializeAnts()
    {
        int[] amountOfAntsInCastes = { 1, 17, 2 };
        AntCountByCaste = new int[Castes.Length];
        AntsByCaste = new List<Ant>[Castes.Length];

        for (int i = 0; i < Castes.Length; i++) 
        {
            AntCountByCaste[i] = 0;
            AntsByCaste[i] = new List<Ant>();
            for (int j = 0; j < amountOfAntsInCastes[i]; j++)
            {
                NextAnt(i);

                yield return new WaitForSeconds(0.01f);
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
            ResourceAmounts[ResourceType.Food] -= tendAmount / 10;
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
                int casteIndex = NextCaste();
                NextAnt(casteIndex);
            }

            yield return null;
        }
    }

    private void NextAnt(int casteIndex)
    {
        Caste caste = Castes[casteIndex];

        // create the Ant
        GameObject antObject = Instantiate(_antPrefab);
        Ant ant = antObject.GetComponent<Ant>();

        // set the Ant's parent
        ant.transform.parent = transform;

        // set the Ant's position, colony, and caste
        ant.transform.position = RandomLocationAroundColony(0f, 1f);
        ant.AssignColony(this);
        ant.AssignCaste(caste);

        // set the color and name
        Color color = PaletteManager.Colors[casteIndex];
        ant.Transform.GetChild(0).GetComponent<Renderer>().material.color = PaletteManager.Colors[casteIndex*4];
        antObject.name = caste.Name + "Ant" + ant.ID;
        Debug.Log(antObject.name + " gets color: " + color.ToString());

        // update colony variables
        NumAnts++;
        AntCountByCaste[casteIndex] ++;
        AntsByCaste[casteIndex].Add(ant);
    }

    private int NextCaste()
    {
        // compare Caste percentages to real percentages:
        int casteIndex = 0;
        float maxPercentageDifference = 0;
        for (int i = 0; i < Castes.Length; i++) 
        {
            float realPercentage = AntCountByCaste[i] / NumAnts;
            float targetPercentage = Castes[i].Percentage;
            float percentageDifference = targetPercentage - realPercentage;
            if (percentageDifference > maxPercentageDifference)
            {
                casteIndex = i;
                maxPercentageDifference = percentageDifference;
            }
        }

        return casteIndex;
    }

    public void UpdateCastePercentages(float[] newCastePercentages)
    {
        for (int i = 0; i < Castes.Length; i++)
        {
            Castes[i].SetPercentage(newCastePercentages[i]);
        }
    }

    public Vector3 GetQueenPosition()
    {
        return _queen.transform.position;
    }

    public void AddFood(float amount)
    {
        ResourceAmounts[ResourceType.Food] += amount;
    }

    public void ConnectCastesToGUI()
    {
        for (int i = 0; i < Castes.Length; i++)
        {
            Debug.Log("Connecting Caste " + i + " to ColorDropdown " + i);
            
            int k = i;

            MainGUIPanel.Instance.ColorDropdowns[i].onValueChanged.AddListener((v) => { ChangeColor(k); });
            // MainGUIPanel.Instance.PheromoneButtons[i].onClick.AddListener(Castes[i].ChangePheromone);
        }
    }

    public void ChangeColor(int casteIndex)
    {
        Debug.Log("Attempting to change the color of Caste " + casteIndex + " to ");
        
        int colorIndex = MainGUIPanel.Instance.ColorDropdowns[casteIndex].value;
        Color color = PaletteManager.Colors[colorIndex];
        
        for (int j = 0; j < AntCountByCaste[casteIndex]; j++)
        {
            Ant ant = AntsByCaste[casteIndex][j];
            ant.Transform.GetChild(0).GetComponent<Renderer>().material.color = color;
        }  
    }
}
