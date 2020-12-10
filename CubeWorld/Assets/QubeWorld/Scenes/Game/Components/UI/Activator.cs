using UnityEngine;

namespace GameScene
{
    public class Activator : MonoBehaviour
    {
        [SerializeField]
        private GameManagerUnity gameManagerUnity;
        [SerializeField]
        private Shared.OptionsMenu optionsMenu;
        [SerializeField]
        private PauseMenu pauseMenu;
        [SerializeField]
        private SaveMenu saveMenu;
        [SerializeField]
        private GameObject barrier;

        private MenuState lastState = MenuState.NONE;

        public MenuState State { get; set; }

        #region Unity lifecycles

        public void Update()
        {
            if (State == lastState)
            {
                return;
            }
            lastState = State;
            optionsMenu.gameObject.SetActive(State == MenuState.OPTIONS);
            pauseMenu.gameObject.SetActive(State == MenuState.PAUSE);
            saveMenu.gameObject.SetActive(State == MenuState.SAVE);
            barrier.SetActive(State != MenuState.NONE);
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
    }
}