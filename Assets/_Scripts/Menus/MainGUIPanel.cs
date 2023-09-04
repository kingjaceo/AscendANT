using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Dropdown;

public class MainGUIPanel : MonoBehaviour
{
    public static MainGUIPanel Instance;

    [SerializeField] private TextMeshProUGUI _colonyStatsText;
    [SerializeField] private Colony _colony;

    [SerializeField] private GameObject _castesParent;
    [SerializeField] private GameObject[] _castes;
    [SerializeField] private GameObject _casteButtonTemplate;

    [SerializeField] private TMP_Text[] _casteText;

    [SerializeField] public ColorChangingDropdown[] ColorDropdowns;

    [SerializeField] public Button[] MainButtons;
    [SerializeField] public Button[] PheromoneButtons;

    [SerializeField] private Slider[] _castePercentageSliders;
    private float[] _previousCastePercentages = new float[] {10, 80, 10};

    [SerializeField] private Slider _hatchProgressSlider;
    [SerializeField] private Slider _layProgressSlider;
 
    public void Update()
    {
        if (_colony != null)
        {
            UpdateColonyStats();
            UpdateCasteText();
            _colony.UpdateCastePercentages(_previousCastePercentages);
            UpdateHatchProgress();
            UpdateLayProgress();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        _casteButtonTemplate.SetActive(false);
        
        int numCastes = 3;

        _castePercentageSliders = new Slider[numCastes];
        ColorDropdowns = new ColorChangingDropdown[numCastes];
        _casteText = new TMP_Text[numCastes];
        MainButtons = new Button[numCastes];

        for (int i = 0; i < numCastes; i++)
        {
            Debug.Log("Creating Caste " + i + " Button");
            GameObject casteButton = Instantiate(_casteButtonTemplate);
            casteButton.name = "Caste" + i;
            casteButton.transform.SetParent(_castesParent.transform);
            _castes[i] = casteButton;
            _castes[i].SetActive(true);
            _castes[i].transform.position = _castesParent.transform.position + new Vector3(0, -i*100, 0);
            
            ColorDropdowns[i] = _castes[i].transform.Find("ColorDropdown").GetComponent<ColorChangingDropdown>();

            MainButtons[i] = _castes[i].transform.Find("MainButton").GetComponent<Button>();
            _casteText[i] = MainButtons[i].transform.Find("Text (TMP)").GetComponent<TMP_Text>();
            // Button pheromoneButton = _castes[i].transform.Find("PheromoneButton").GetComponent<Button>();
            // PheromoneButtons[i] = pheromoneButton;

            _castePercentageSliders[i] = _castes[i].transform.Find("PercentageSlider").GetComponent<Slider>();
            _castePercentageSliders[i].value = _previousCastePercentages[i];
            _castePercentageSliders[i].onValueChanged.AddListener((v) => { UpdateSliderLevels();});
        }
    }

    public void GiveColony(Colony colony)
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

    private void UpdateCasteText()
    {
        for (int i = 0; i < _casteText.Length; i++)
        {
            _casteText[i].text = _colony.Castes[i].Name;
            _casteText[i].text += ": " + _colony.AntCountByCaste[i] + $" ({_previousCastePercentages[i]}%)";
        }
    }
    
    private void UpdateSliderLevels()
    {        
        float[] casteLevels = new float[_castePercentageSliders.Length];
        int changedSliderIndex = 0;

        for (int i = 0; i < _castePercentageSliders.Length; i++)
        {
            casteLevels[i] = _castePercentageSliders[i].value;
            if (casteLevels[i] != _previousCastePercentages[i])
            {
                changedSliderIndex = i;
            }
        }

        float remainderPercentage;
        float sumPercentage = 0;

        remainderPercentage = 100 - casteLevels[changedSliderIndex];
        for (int i = 0; i < _castePercentageSliders.Length; i++)
        {
            if (i != changedSliderIndex)
            {
                sumPercentage += casteLevels[i];
            }
        }

        for (int i = 0; i < _castePercentageSliders.Length; i++)
        {
            if (i != changedSliderIndex & sumPercentage == 0)
            {
                casteLevels[i] = 0.5f * remainderPercentage;
            }
            else if (i != changedSliderIndex)
            {
                casteLevels[i] = _previousCastePercentages[i] / sumPercentage * remainderPercentage;
            }

            casteLevels[i] = Mathf.Round(Mathf.Clamp(casteLevels[i], 0, 100));
            _castePercentageSliders[i].SetValueWithoutNotify(casteLevels[i]);
            _previousCastePercentages[i] = casteLevels[i];
        }
    }

    private void UpdateHatchProgress()
    {
        _hatchProgressSlider.value = _colony.HatchProgress;
    }

    private void UpdateLayProgress()
    {
        _layProgressSlider.value = _colony.Queen.TimeSinceLastEgg / _colony.Queen.TimeToLayEgg * 100;
    }

    public void ShowPheromones()
    {

    }
}
