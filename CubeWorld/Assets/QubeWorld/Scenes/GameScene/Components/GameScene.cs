using UnityEngine;

namespace GameScene
{
    public class GameScene : MonoBehaviour
    {
        [SerializeField]
        private GameController gameController;
        [SerializeField]
        private GameManagerUnity gameManagerUnity;
        [SerializeField]
        private Shared.SettingsMenu settingsMenu;
        [SerializeField]
        private PauseMenu pauseMenu;
        [SerializeField]
        private SaveMenu saveMenu;
        [SerializeField]
        private Inventry inventry;
        [SerializeField]
        private ProgressBar progressBar;
        [SerializeField]
        private MessageBox messageBox;
        [SerializeField]
        private GameObject barrier;

        private State lastState;
        public State State { get; set; }

        public bool IsPaused
        {
            get
            {
                return State == State.PAUSE || State == State.SETTINGS || State == State.SAVE;
            }
        }

        public bool IsPlayable
        {
            get
            {
                return State != State.GENERATING && !IsPaused && State != State.INVENTRY;
            }
        }

        public bool IsGenerating { get { return State == State.GENERATING; } }

        #region Unity lifecycles

        public void Start()
        {
            lastState = State.GAME;
            State = State.GENERATING;
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

        public bool Pause()
        {
            if (State != State.GAME)
            {
                return false;
            }
            State = State.PAUSE;
            return true;
        }

        public void Unpause()
        {
            if (!IsPaused)
            {
                return;
            }
            State = State.GAME;
        }

        public void SetLastState()
        {
            switch (State) {
                case State.PAUSE:
                    Unpause();
                    break;
                case State.INVENTRY:
                    State = State.GAME;
                    break;
                case State.SETTINGS:
                case State.SAVE:
                    State = State.PAUSE;
                    break;
                default:
                    break;
            }
        }

        public void ReturnToTitleMenu()
        {
            gameManagerUnity.DestroyWorld();
            Shared.SceneLoader.GoToTitleScene();
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
            settingsMenu.gameObject.SetActive(State == State.SETTINGS);
            pauseMenu.gameObject.SetActive(State == State.PAUSE);
            saveMenu.gameObject.SetActive(State == State.SAVE);
            inventry.gameObject.SetActive(State == State.INVENTRY);
            barrier.SetActive(!IsPlayable);
            progressBar.SetVisible(State == State.GENERATING);
            gameController.SetVisible(State != State.GENERATING);
            messageBox.gameObject.SetActive(State != State.GENERATING);
        }

        #endregion
    }
}
