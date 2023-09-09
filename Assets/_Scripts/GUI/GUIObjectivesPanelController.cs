using TMPro;
using UnityEngine;

public class GUIObjectivesPanelController : MonoBehaviour
{
    public GUIObjectivesPanelController Instance;

    [SerializeField] private TextMeshProUGUI _victoryProgressText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {

    }

    void Update()
    {
        if (CurrentVictoryCondition.Instance != null)
        {
            string progress = CurrentVictoryCondition.Instance.Progress();
            _victoryProgressText.text = "Goal: " + progress;
        }
    }
}