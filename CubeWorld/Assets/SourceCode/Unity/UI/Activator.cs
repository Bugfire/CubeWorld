using UnityEngine;

namespace Menu
{
    public class Activator : MonoBehaviour
    {
        [SerializeField]
        private GameManagerUnity gameManagerUnity;
        [SerializeField]
        private Main mainMenu;
        [SerializeField]
        private Generator generatorMenu;
        [SerializeField]
        private Options optionsMenu;
        [SerializeField]
        private Pause pauseMenu;
        [SerializeField]
        private LoadSave loadSaveMenu;
        [SerializeField]
        private JoinMultiplayer joinMultiplayerMenu;
        [SerializeField]
        private About aboutDialog;
        [SerializeField]
        private GameObject barrier;

        private State state = State.NONE;

        private State lastState = State.NONE;

        #region Unity Lifecycles

        public void Update()
        {
            if (state == lastState)
            {
                return;
            }
            lastState = state;
            mainMenu.gameObject.SetActive(state == State.MAIN);
            generatorMenu.gameObject.SetActive(state == State.GENERATOR);
            optionsMenu.gameObject.SetActive(state == State.OPTIONS);
            pauseMenu.gameObject.SetActive(state == State.PAUSE);
            loadSaveMenu.gameObject.SetActive(state == State.LOAD || state == State.SAVE);
            joinMultiplayerMenu.gameObject.SetActive(state == State.JOIN_MULTIPLAYER);
            aboutDialog.gameObject.SetActive(state == State.ABOUT);
            barrier.SetActive(state != State.NONE);
        }

        #endregion

        #region Unity Events

        public void OnClose()
        {
            SetLastState();
        }

        #endregion

        #region Public methods

        public void SetState(State _state)
        {
            state = _state;
        }

        public State GetState()
        {
            return state;
        }

        public void SetLastState()
        {
            if (state == State.MAIN)
            {
                return;
            }
            var gameState = gameManagerUnity.GetState();
            switch (gameState)
            {
                case GameState.MAIN_MENU:
                    state = State.MAIN;
                    break;
                case GameState.PAUSE:
                    if (state == State.PAUSE)
                    {
                        gameManagerUnity.Unpause();
                    }
                    else
                    {
                        state = State.PAUSE;
                    }
                    break;
            }
        }

        #endregion
    }
}
