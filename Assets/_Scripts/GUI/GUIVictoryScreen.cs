using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIVictoryScreen : MonoBehaviour
{
    [SerializeField] private Button _collectRewardButton;
    [SerializeField] private Button _backToGameButton;
    [SerializeField] private Button _backToMenuButton;

    void Awake()
    {
        _collectRewardButton.onClick.AddListener(RewardCollected);
    }

    void OnEnable()
    {
        _collectRewardButton.GetComponentInChildren<TMP_Text>().text = CurrentVictoryCondition.Instance.Reward;
    }

    private void RewardCollected()
    {
        _backToGameButton.onClick.AddListener(Deactivate);
        _backToMenuButton.onClick.AddListener(GameManager.Instance.ToMainMenu);
    }

    private void Deactivate()
    {
        _backToGameButton.onClick.RemoveListener(Deactivate);
        _backToMenuButton.onClick.RemoveListener(GameManager.Instance.ToMainMenu);
        gameObject.SetActive(false);
    }
}