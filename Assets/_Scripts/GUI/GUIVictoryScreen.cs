using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIVictoryScreen : MonoBehaviour
{
    [SerializeField] private Button _collectRewardButton;
    [SerializeField] private Button _backToGameButton;
    [SerializeField] private Button _backToMenuButton;
    
    // public GameEvent OnRewardButtonClicked;

    void Awake()
    {
        RewardCollected();
    }

    void OnEnable()
    {
        _collectRewardButton.GetComponentInChildren<TMP_Text>().text = CurrentVictoryCondition.Instance.Reward;
    }

    private void RewardCollected()
    {
        _backToGameButton.onClick.AddListener(GameManager.Instance.Resume);
        _backToMenuButton.onClick.AddListener(GameManager.Instance.ToMainMenu);

        // _collectRewardButton.interactable = false;

        // OnRewardButtonClicked.Raise();
    }

    private void Deactivate()
    {
        _backToGameButton.onClick.RemoveListener(Deactivate);
        _backToMenuButton.onClick.RemoveListener(GameManager.Instance.ToMainMenu);
        gameObject.SetActive(false);
    }
}