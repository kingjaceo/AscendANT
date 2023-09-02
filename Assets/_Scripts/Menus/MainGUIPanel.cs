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

    [SerializeField] private Slider[] _sliders;
    private float[] _previousCasteLevels = new float[] {10, 80, 10};

    [SerializeField] private Slider _hatchProgressSlider;
    [SerializeField] private Slider _layProgressSlider;


    private TMP_Text _caste0Text;
    private TMP_Text _caste1Text;
    private TMP_Text _caste2Text;
 
    public void Update()
    {
        UpdateColonyStats();
        UpdateCasteText();
        _colony.UpdateCastePercentages(_previousCasteLevels);
        UpdateHatchProgress();
        UpdateLayProgress();
    }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        
        _sliders = new Slider[] {_caste0.GetComponent<Slider>(), _caste1.GetComponent<Slider>(), _caste2.GetComponent<Slider>()};
        for (int i = 0; i < _sliders.Length; i++)
        {
            _sliders[i].value = _previousCasteLevels[i];
            _sliders[i].onValueChanged.AddListener((v) => { UpdateSliderLevels();});
        }

        _caste0Text = _caste0.transform.Find("Caste0NameAndAmount").GetComponent<TMP_Text>();
        _caste1Text = _caste1.transform.Find("Caste1NameAndAmount").GetComponent<TMP_Text>();
        _caste2Text = _caste2.transform.Find("Caste2NameAndAmount").GetComponent<TMP_Text>();
    }

    public void GiveColony(Colony colony)
    {
        _colony = colony;
    }

    private void UpdateColonyStats()
    {
        if (_colony != null & _colony.ResourceAmounts != null & _colonyStatsText != null)
        {
            _colonyStatsText.text = "Food: " + Math.Round(_colony.ResourceAmounts[ResourceType.Food]) + "\n";
            _colonyStatsText.text += "Water: " + Math.Round(_colony.ResourceAmounts[ResourceType.Water]) + "\n";
            _colonyStatsText.text += "Eggs: " + _colony.ResourceAmounts[ResourceType.Eggs]; 
        }
    }

    private void UpdateCasteText()
    {
        _caste0Text.text = _colony.Castes[0].Name;
        _caste0Text.text += ": " + _colony.AntsByCaste[_colony.Castes[0].Name] + $" ({_previousCasteLevels[0]}%)";

        _caste1Text.text = _colony.Castes[1].Name;
        _caste1Text.text += ": " + _colony.AntsByCaste[_colony.Castes[1].Name] + $" ({_previousCasteLevels[1]}%)";

        _caste2Text.text = _colony.Castes[2].Name;
        _caste2Text.text += ": " + _colony.AntsByCaste[_colony.Castes[2].Name] + $" ({_previousCasteLevels[2]}%)";
    }
    
    private void UpdateSliderLevels()
    {
        float[] casteLevels = new float[_sliders.Length];
        int changedSliderIndex = 0;

        for (int i = 0; i < _sliders.Length; i++)
        {
            casteLevels[i] = _sliders[i].value;
            if (casteLevels[i] != _previousCasteLevels[i])
            {
                changedSliderIndex = i;
            }
        }

        float remainderPercentage;
        float sumPercentage = 0;

        remainderPercentage = 100 - casteLevels[changedSliderIndex];
        for (int i = 0; i < _sliders.Length; i++)
        {
            if (i != changedSliderIndex)
            {
                sumPercentage += casteLevels[i];
            }
        }

        for (int i = 0; i < _sliders.Length; i++)
        {
            if (i != changedSliderIndex & sumPercentage == 0)
            {
                casteLevels[i] = 0.5f * remainderPercentage;
            }
            else if (i != changedSliderIndex)
            {
                casteLevels[i] = _previousCasteLevels[i] / sumPercentage * remainderPercentage;
            }

            casteLevels[i] = Mathf.Round(Mathf.Clamp(casteLevels[i], 0, 100));
            _sliders[i].SetValueWithoutNotify(casteLevels[i]);
            _previousCasteLevels[i] = casteLevels[i];
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
}
