using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstTutorialComponent : MonoBehaviour
{
    [SerializeField] private Toggle _firstPanelToggle;
    [SerializeField] private GameObject _firstPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableFirstPanel()
    {
        // Debug.Log("FIRST TUTORIAL PANEL: Enable logic is running!");
        if (_firstPanelToggle.isOn)
        {
            _firstPanel.SetActive(true);
        }
    }
}
