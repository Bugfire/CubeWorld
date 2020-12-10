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
        private Shared.OptionsMenu optionsMenu;
        [SerializeField]
        private PauseMenu pauseMenu;
        [SerializeField]
        private SaveMenu saveMenu;
        [SerializeField]
        private ProgressBar progressBar;
        [SerializeField]
        private MessageBox messageBox;
        [SerializeField]
        private GameObject barrier;

        private MenuState lastState;
        private bool lastIsGenerating;

        public MenuState State { get; set; }

        public bool IsPlayable { get { return gameManagerUnity.IsPlayable; } }

        #region Unity lifecycles

        public void Start()
        {
            lastState = MenuState.NONE;
            lastIsGenerating = !gameManagerUnity.IsGenerating;
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

        public void Pause()
        {
            gameManagerUnity.Pause();
        }

        public void SetLastState()
        {
            switch (State) {
                case MenuState.NONE:
                    break;
                case MenuState.PAUSE:
                    gameManagerUnity.Unpause();
                    break;
                default:
                    State = MenuState.PAUSE;
                    break;
            }
        }

        #endregion

        #region Private methods

        private void updateUI()
        {
            var isGenerating = gameManagerUnity.IsGenerating;
            if (State != lastState)
            {
                lastState = State;
                optionsMenu.gameObject.SetActive(State == MenuState.OPTIONS);
                pauseMenu.gameObject.SetActive(State == MenuState.PAUSE);
                saveMenu.gameObject.SetActive(State == MenuState.SAVE);
                barrier.SetActive(isGenerating || (State != MenuState.NONE));
            }
            if (isGenerating != lastIsGenerating)
            {
                progressBar.SetVisible(isGenerating);
                gameController.SetVisible(!isGenerating);
                messageBox.gameObject.SetActive(!isGenerating);
                barrier.SetActive(isGenerating || (State != MenuState.NONE));
                lastIsGenerating = isGenerating;
            }
        }

        #endregion
    }
}
