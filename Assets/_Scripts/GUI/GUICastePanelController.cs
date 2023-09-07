using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUICastePanelController : MonoBehaviour
{
    private int _casteIndex;
    private Caste _caste;
    private Colony _colony;
    private Transform _transform;
    public Transform Transform => _transform;

    [SerializeField] private ColorChangingDropdown _colorDropdown;
    [SerializeField] private TMP_Text _casteText;
    [SerializeField] private Button _mainButton;
    [SerializeField] private Button _pheromoneButton;
    [SerializeField] private Slider _castePercentageSlider;
    [SerializeField] private GameObject _casteStatsPanel;
    [SerializeField] private TMP_Text _casteStatsText;
    [SerializeField] private GameObject _pheromonePanel;
    private GUIPheromonePanelController _pheromonePanelController;

    // Start is called before the first frame update
    void Awake()
    {
        _transform = transform;
        _pheromonePanelController = _pheromonePanel.GetComponent<GUIPheromonePanelController>();
        _casteStatsText = _casteStatsPanel.GetComponentInChildren<TMP_Text>();
    }

    void Start()
    {
        _pheromoneButton.onClick.AddListener(PheromoneButtonClicked);
        _mainButton.onClick.AddListener(MainButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCasteText();
        UpdateCasteStats();
    }

    public int GetCastePercentageSliderValue()
    {
        return (int) _castePercentageSlider.value;
    }

    public void SetCastePercentageSliderValue(float value)
    {
        _castePercentageSlider.SetValueWithoutNotify(value);
    }

    public void SetCasteIndex(int casteIndex)
    {
        _casteIndex = casteIndex;
    }

    public void SetColony(Colony colony)
    {
        _colony = colony;
        _colorDropdown.onValueChanged.AddListener((v) => { _colony.ChangeColor(_casteIndex); });
    }

    public void SetCaste(Caste caste)
    {
        _caste = caste;
        _pheromonePanelController.SetCaste(caste);
        _castePercentageSlider.onValueChanged.AddListener((v) => { GUIMainPanelController.Instance.UpdateSliderLevels(_casteIndex);});
        // GUIMainPanelController.Instance.UpdateSliderLevels(_casteIndex);
    }

    public Color GetColor()
    {
        int colorIndex = _colorDropdown.value;
        if (colorIndex * 2 < PaletteManager.Colors.Count)
        {
            return PaletteManager.Colors[colorIndex * 2];
        }
        else return new Color();
    }

    public void UpdateCasteText()
    {
        if (_colony != null)
        {
            string casteName = _colony.Castes[_casteIndex].Name;
            int numAnts = _colony.AntCountByCaste[_casteIndex];
            float currentPercentage = _colony.Castes[_casteIndex].Percentage;

            _casteText.text = casteName;
            _casteText.text += ": " + numAnts + $" ({Mathf.Round(currentPercentage)}%)";
        }
    }

    private void UpdateCasteStats()
    {
        string newText = _caste.CasteStats.ToString();
        _casteStatsText.text = newText;
    }
    
    private void PheromoneButtonClicked()
    {
        bool isActive = _pheromonePanel.activeSelf;
        _pheromonePanel.SetActive(!isActive);
    }

    private void MainButtonClicked()
    {
        Debug.Log("Caste Panel toggled");
        bool isActive = _casteStatsPanel.activeSelf;
        _casteStatsPanel.SetActive(!isActive);
    }
}
