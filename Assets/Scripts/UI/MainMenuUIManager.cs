using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuUIManager : MonoBehaviour
    {
        [Header("Buttons")]
        #region MenuPanel
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _creditsButton;
        [SerializeField] private Button _engButton;
        [SerializeField] private Button _trButton;
        [SerializeField] private Button _quitButton;
        #endregion
        
        #region Credits
        [SerializeField] private Button _mainMenuButtonInCredits;
        #endregion

        #region Settings
        [SerializeField] private Button _soundSettingButton;
        [SerializeField] private Button _graphicSettingButton;
        [SerializeField] private Button _mainMenuButtonInSettings;
        #endregion
        

        [Header("Panels")]
        #region UI
        [SerializeField] private GameObject _mainMenuPanel;
        [SerializeField] private GameObject _creditsPanel;
        [SerializeField] private GameObject _settingsPanel;
        #endregion

        #region Setting Panel
        [SerializeField] private GameObject _soundPanel;
        [SerializeField] private GameObject _graphicPanel;
        #endregion

        #region MonoBehaviour

        void Start()
        {
            #region Menu Panel
            //_continueButton.onClick.AddListener(OnClickContinue);
            //_newGameButton.onClick.AddListener(OnClickNewGame);
            //_settingsButton.onClick.AddListener(OnClickSettings);
            _creditsButton.onClick.AddListener(OnClickCredits);
            _engButton.onClick.AddListener(OnClickMakeENG);
            _trButton.onClick.AddListener(OnClickMakeTR);
            //_quitButton.onClick.AddListener(OnClickQuit);
            #endregion

            #region Credits
            _mainMenuButtonInCredits.onClick.AddListener(OnClickOpenMainMenu);
            #endregion

            #region Settings
            _soundSettingButton.onClick.AddListener(OnClickOpenSoundPanel);
            _graphicSettingButton.onClick.AddListener(OnClickOpenGraphicPanel);
            _mainMenuButtonInSettings.onClick.AddListener(OnClickOpenMainMenu);
            #endregion
        }

        #endregion

        #region MenuPanel

        private void OnClickContinue()
        {
           Debug.Log("Continue - This button is not working now!"); 
        }
        
        private void OnClickNewGame()
        {
            Debug.Log("New Game");
            SceneManager.LoadScene("LevelScene");
        }
        
        private void OnClickSettings()
        {
            Debug.Log("Settings");
            _mainMenuPanel.SetActive(false);
            _settingsPanel.SetActive(true);
        }
        
        private void OnClickCredits()
        {
            Debug.Log("Credits"); 
            _mainMenuPanel.SetActive(false);
            _creditsPanel.SetActive(true);
        }
        
        private void OnClickMakeENG()
        {
            Debug.Log("Make ENG"); 
        }
        private void OnClickMakeTR()
        {
            Debug.Log("Make TR"); 
        }
        private void OnClickQuit()
        {
            Debug.Log("Quitting");
#if !UNITY_EDITOR
            Application.Quit();          
#elif UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        #endregion

        #region Settings

        private void OnClickOpenSoundPanel()
        {
            _soundPanel.SetActive(true);
            _graphicPanel.SetActive(false);
        }

        private void OnClickOpenGraphicPanel()
        {
            _soundPanel.SetActive(false);
            _graphicPanel.SetActive(true);
        }

        #endregion
        
        private void OnClickOpenMainMenu()
        {
            _mainMenuPanel.SetActive(true);
            _creditsPanel.SetActive(false);
            _settingsPanel.SetActive(false);
            _soundPanel.SetActive(false);
            _graphicPanel.SetActive(false);
        }
    }
}
