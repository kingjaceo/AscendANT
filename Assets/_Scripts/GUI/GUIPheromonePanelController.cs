using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIPheromonePanelController : MonoBehaviour
{
    private Caste _caste;
    
    [SerializeField] private Button _triggeringButton;
    [SerializeField] private GameObject _pheromoneButtonsParent;
    [SerializeField] private Button _pheromoneButtonTemplate;
    [SerializeField] private Button _selectedPheromoneButton;
    private List<Button> _pheromoneButtons = new List<Button>();

    void Awake()
    {
        // create a button for each pheromone
        foreach (PheromoneName pheromone in Enum.GetValues(typeof(PheromoneName)))
        {
            if (pheromone != PheromoneName.Queen)
            {
                // Debug.Log("Creating button for pheromone: " + pheromone.ToString());
                Button button = Instantiate(_pheromoneButtonTemplate);
                button.transform.SetParent(_pheromoneButtonsParent.transform);
                button.gameObject.SetActive(true);
                button.name = pheromone.ToString();
                button.GetComponentInChildren<TMP_Text>().text = pheromone.ToString();

                PheromoneName pheromoneRef = pheromone;
                button.onClick.AddListener(() => { ChangePheromone(pheromoneRef); });

                _pheromoneButtons.Add(button);
            }
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // connect the buttons to the castes themselves
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCaste(Caste caste)
    {
        _caste = caste;
    }

    private void ChangePheromone(PheromoneName pheromoneName)
    {
        _selectedPheromoneButton.GetComponentInChildren<TMP_Text>().text = pheromoneName.ToString();
        _caste.SetPheromone(pheromoneName, 0);
    }
}