using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Dropdown;

public class GUIMainPanelController : MonoBehaviour
{
    public static GUIMainPanelController Instance;

    [SerializeField] private Colony _colony;
    public Colony Colony => _colony;
    [SerializeField] private TextMeshProUGUI _colonyStatsText;

    [SerializeField] private Slider _hatchProgressSlider;
    [SerializeField] private Slider _layProgressSlider;

    [SerializeField] private GameObject _castesParent;
    [SerializeField] private GameObject _casteButtonTemplate;
    public List<GUICastePanelController> CastePanels;

    private float[] _previousCastePercentages = new float[] {10, 80, 10};
 
    public void Update()
    {
        if (_colony != null)
        {
            UpdateColonyStats();
            _colony.UpdateCastePercentages(_previousCastePercentages);
            UpdateHatchProgress();
            UpdateLayProgress();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _casteButtonTemplate.SetActive(false);
        Instance = this;
        int numCastes = 3;

        for (int i = 0; i < numCastes; i++)
        {
            // create the panel
            Debug.Log("Creating Caste " + i + " Button");
            GameObject castePanel = Instantiate(_casteButtonTemplate);
            CastePanels.Add(castePanel.GetComponent<GUICastePanelController>());
            CastePanels[i].name = "Caste" + i;
            CastePanels[i].transform.SetParent(_castesParent.transform);
            CastePanels[i].SetCasteIndex(i);

            // activate and position the panel
            castePanel.SetActive(true);
            CastePanels[i].Transform.position = _castesParent.transform.position + new Vector3(0, -i*100, 0);

            CastePanels[i].SetCastePercentageSliderValue(_previousCastePercentages[i]);
        }
    }

    public void SetColony(Colony colony)
    {
        _colony = colony;
    }

    private void UpdateColonyStats()
    {
        Dictionary<ResourceType, float> resourceAmounts = _colony.ResourceAmounts;

        string foodAmount = Math.Round(resourceAmounts[ResourceType.Food]).ToString();
        string waterAmount = Math.Round(resourceAmounts[ResourceType.Water]).ToString();
        string eggs = resourceAmounts[ResourceType.Eggs].ToString();
        string statsText = "";
        statsText += "Food: " + foodAmount + "\n";
        statsText += "Water: " + waterAmount + "\n";
        statsText += "Eggs: " + eggs;

        _colonyStatsText.text = statsText;
    }
    
    public void UpdateSliderLevels(int changedSliderIndex)
    {        
        // ** this should be called whenever the player changes any slider values **
        // Debug.Log("SLIDER: Changing Slider for Caste " + changedSliderIndex);
        // Debug.Log("SLIDER: Previous Percentages: " + _previousCastePercentages[0] + ", " +  _previousCastePercentages[1] + ", " +  _previousCastePercentages[2]);
        // string debugStr = "SLIDER: Slider levels have been changed: ";
        // calculate the correct new slider values
        float changedValue = CastePanels[changedSliderIndex].GetCastePercentageSliderValue();
        // float delta = _previousCastePercentages[changedSliderIndex] - changedValue;

        // command the caste panels to set their slider values
        // Debug.Log("SLIDER: Delta calculated to be " + delta);
        for (int i = 0; i < CastePanels.Count; i++)
        {
            float newValue = changedValue;
            if (i != changedSliderIndex)
            {
                // float shift = delta / 2;
                // Debug.Log("SLIDER: Shift calculated to be " + shift);
                newValue = (100 - changedValue) / 2;
                CastePanels[i].SetCastePercentageSliderValue(newValue);
            }
            // debugStr += newValue + ", ";

            _previousCastePercentages[i] = Mathf.Clamp(newValue, 0, 100);
        }

        _colony.UpdateCastePercentages(_previousCastePercentages);

        // Debug.Log(debugStr);
        // Debug.Log("SLIDER: New Previous Percentages: " + _previousCastePercentages[0] + ", " +  _previousCastePercentages[1] + ", " +  _previousCastePercentages[2]);
    }

    private void UpdateHatchProgress()
    {
        _hatchProgressSlider.value = _colony.HatchProgress;
    }

    private void UpdateLayProgress()
    {
        _layProgressSlider.value = _colony.Queen.TimeSinceLastEgg / _colony.Queen.TimeToLayEgg * 100;
    }
}
