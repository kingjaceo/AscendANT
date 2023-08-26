using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour 
{
   public static GUIManager Instance { get; private set;}
   
   [SerializeField] private GameObject _mainGUICanvas;

   public TextMeshProUGUI statsText;
   public Colony colony;

   void Awake()
   {
      GameManager.OnAfterStateChanged += GameManager_OnAfterStateChanged;
   }

   void OnDestroy()
   {
      GameManager.OnAfterStateChanged -= GameManager_OnAfterStateChanged;
   }
 
   private void GameManager_OnAfterStateChanged(GameState state)
   {
      switch (state)
      {
         case GameState.InProgress:
            _mainGUICanvas.SetActive(true);
            break;
         default:
            break;
      }
   }
   
   public void Update()
   {
      
   }

    // Start is called before the first frame update
   void Start()
   {

   }

   public void ToggleGUI(bool on) 
   {
      
   }
}
