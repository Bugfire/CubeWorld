using UnityEngine;

namespace TitleScene
{
    public class TitleScene : MonoBehaviour
    {
        [SerializeField]
        private TitleMenu titleMenu;
        [SerializeField]
        private GeneratorMenu generatorMenu;
        [SerializeField]
        private LoadMenu loadMenu;
        [SerializeField]
        private JoinMultiplayerMenu joinMultiplayerMenu;
        [SerializeField]
        private Shared.SettingsMenu settingsMenu;
        [SerializeField]
        private AboutDialog aboutDialog;

        public State State { get; set; }

        private State lastState;

        #region Unity lifecycles

        void Start()
        {
            GetComponent<Camera>().enabled = false;
            lastState = State.GENERATOR;
            State = State.TITLE;
            updateUI();
        }

        public void Update()
        {
            updateUI();
        }

        #endregion

        #region Unity user events

        public void OnClose()
        {
            SetLastState();
        }

        #endregion

        #region Public methods

        public void SetLastState()
        {            
            State = State.TITLE;
        }

        #endregion

        #region Private methods

        private void updateUI()
        {
            if (State == lastState)
            {
                return;
            }
            lastState = State;
            titleMenu.gameObject.SetActive(State == State.TITLE);
            generatorMenu.gameObject.SetActive(State == State.GENERATOR);
            loadMenu.gameObject.SetActive(State == State.LOAD);
            joinMultiplayerMenu.gameObject.SetActive(State == State.JOIN_MULTIPLAYER);
            settingsMenu.gameObject.SetActive(State == State.SETTINGS);
            aboutDialog.gameObject.SetActive(State == State.ABOUT);
        }

        #endregion
    }
}
