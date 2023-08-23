using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColonyStatsDisplay : MonoBehaviour
{
    public TextMeshProUGUI statsText;
    public Colony colony;
 
    public void Update()
    {
        statsText.text = "Food: " + Math.Round(colony.resourceAmounts[ResourceType.Food]) + "\n";
        statsText.text += "Water: " + Math.Round(colony.resourceAmounts[ResourceType.Water]) + "\n";
        statsText.text += "Eggs: " + colony.resourceAmounts[ResourceType.Eggs]; 
    }

    // Start is called before the first frame update
    void Start()
    {
        colony = GameObject.Find("Colony").GetComponent<Colony>();
        statsText = GetComponent<TextMeshProUGUI>();
    }
}
