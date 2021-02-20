using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.MainMenu
{
    public class UISelectManager : MonoBehaviour
    {
        [Tooltip("this data must be filled in clockwise starting from the arrow pointing.")]
        [SerializeField] private Transform[] buttons;
        private List<string> _buttonsName = new List<string>();
        [SerializeField] private UISettings settings;
        
        
        [Header("Panels")]
        #region UI
        [SerializeField] private GameObject _mainMenuPanel;
        [SerializeField] private GameObject _creditsPanel;
        [SerializeField] private GameObject _settingsPanel;
        #endregion

        private void OnEnable()
        {
            UISettings.UISelectEventHandler += Select;
        }

        private void OnDisable()
        {
            UISettings.UISelectEventHandler -= Select;
        }

        private void Start()
        {
            settings.SelectedCounter = 0;
            
            foreach (var button in buttons)
            {
                _buttonsName.Add(button.name);
            }
        }

        /// <summary>
        /// Button names and case names have to be the same.
        /// </summary>
        /// <param name="counter"></param>
        private void Select(int counter)
        {
            switch (_buttonsName[counter])
            {
                case "NewGame":
                    //Debug.Log("NewGame");
                    SceneManager.LoadScene("LevelScene");
                    break;
                case "Settings":
                    //SelectSetting();
                    Debug.Log("Settings");
                    break;
                case "QuitGame":
                    //Debug.Log("QuitGame");
#if !UNITY_EDITOR
            Application.Quit();          
#elif UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#endif
                    break;
                case "ShopIMG":
                    Debug.Log("Shop - This button is not working now!"); 
                    break;
                case "LanguageIMG":
                    Debug.Log("Language - This button is not working now!");
                    break;
                case "Continue":
                    Debug.Log("Continue - This button is not working now!"); 
                    break;
            }
        }

        private void SelectSetting()
        {
            //Debug.Log("Settings");
            _settingsPanel.SetActive(true);
            _mainMenuPanel.SetActive(false);
        }
    }
}
