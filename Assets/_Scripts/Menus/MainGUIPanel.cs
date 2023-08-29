using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainGUIPanel : MonoBehaviour
{
    public static MainGUIPanel Instance;

    [SerializeField] private TextMeshProUGUI _colonyStatsText;
    [SerializeField] private Colony _colony;

    [SerializeField] private GameObject _caste0;
    [SerializeField] private GameObject _caste1;
    [SerializeField] private GameObject _caste2;

    [SerializeField] private Slider _caste0Slider;
    [SerializeField] private Slider _caste1Slider;
    [SerializeField] private Slider _caste2Slider;

    private TMP_Text _caste0Text;
    private TMP_Text _caste1Text;
    private TMP_Text _caste2Text;

    private float _previousCaste0Level = 10;
    private float _previousCaste1Level = 80;
    private float _previousCaste2Level = 10;
 
    public void Update()
    {
        UpdateColonyStats();
        UpdateCasteText();
        // UpdateSliderLevels();
        _colony.UpdateCastePercentages(_previousCaste0Level, _previousCaste1Level, _previousCaste2Level);
    }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        
        _caste0Slider = _caste0.GetComponent<Slider>();
        _caste0Slider.value = _previousCaste0Level;
        _caste0Slider.onValueChanged.AddListener((v) => { UpdateSliderLevels();});
        _caste1Slider = _caste1.GetComponent<Slider>();
        _caste1Slider.value = _previousCaste1Level;
        _caste1Slider.onValueChanged.AddListener((v) => { UpdateSliderLevels();});
        _caste2Slider = _caste2.GetComponent<Slider>();
        _caste2Slider.value = _previousCaste2Level;
        _caste2Slider.onValueChanged.AddListener((v) => { UpdateSliderLevels();});

        _caste0Text = _caste0.transform.Find("Caste0NameAndAmount").GetComponent<TMP_Text>();
        Debug.Log("MainGUIPanel sees " + _caste0Text);
        _caste1Text = _caste1.transform.Find("Caste1NameAndAmount").GetComponent<TMP_Text>();
        _caste2Text = _caste2.transform.Find("Caste2NameAndAmount").GetComponent<TMP_Text>();
    }

    public void GiveColony(Colony colony)
    {
        _colony = colony;
    }

    private void UpdateColonyStats()
    {
        _colonyStatsText.text = "Food: " + Math.Round(_colony.ResourceAmounts[ResourceType.Food]) + "\n";
        _colonyStatsText.text += "Water: " + Math.Round(_colony.ResourceAmounts[ResourceType.Water]) + "\n";
        _colonyStatsText.text += "Eggs: " + _colony.ResourceAmounts[ResourceType.Eggs]; 
    }

    private void UpdateCasteText()
    {
        _caste0Text.text = _colony.Castes[0].Name;
        _caste0Text.text += ": " + _colony.AntsByCaste[_colony.Castes[0].Name];

        _caste1Text.text = _colony.Castes[1].Name;
        _caste1Text.text += ": " + _colony.AntsByCaste[_colony.Castes[1].Name];

        _caste2Text.text = _colony.Castes[2].Name;
        _caste2Text.text += ": " + _colony.AntsByCaste[_colony.Castes[2].Name];
    }
    
    private void UpdateSliderLevels()
    {
        float caste0Level = _caste0Slider.value;
        float caste1Level = _caste1Slider.value;
        float caste2Level = _caste2Slider.value;
        float remainderPercentage;
        float sumPercentage;

        if (caste0Level != _previousCaste0Level)
        {
            remainderPercentage = 100f - caste0Level;
            sumPercentage = caste1Level + caste2Level;
            caste1Level = _previousCaste1Level / sumPercentage * remainderPercentage;
            caste2Level = _previousCaste2Level /sumPercentage * remainderPercentage;
        }

        else if (caste1Level != _previousCaste1Level)
        {
            remainderPercentage = 100f - caste1Level;
            sumPercentage = caste0Level + caste2Level;
            caste0Level = _previousCaste0Level / sumPercentage * remainderPercentage;
            caste2Level = _previousCaste2Level * remainderPercentage;
        }

        else if (caste2Level != _previousCaste2Level)
        {
            remainderPercentage = 100f - caste2Level;
            sumPercentage = caste0Level + caste1Level;
            caste0Level = _previousCaste0Level / sumPercentage * remainderPercentage;
            caste1Level = _previousCaste1Level / sumPercentage * remainderPercentage;
        }

        _caste0Slider.value = caste0Level;
        _caste1Slider.value = caste1Level;
        _caste2Slider.value = caste2Level;

        _previousCaste0Level = caste0Level;
        _previousCaste1Level = caste1Level;
        _previousCaste2Level = caste2Level;
    }
}
