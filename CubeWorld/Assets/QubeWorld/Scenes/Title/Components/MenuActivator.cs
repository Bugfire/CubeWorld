using UnityEngine;

namespace TitleScene
{
    public class MenuActivator : MonoBehaviour
    {
        [SerializeField]
        private TitleMenu titleMenu;
        [SerializeField]
        private GeneratorMenu generatorMenu;
        [SerializeField]
        private Shared.OptionsMenu optionsMenu;
        [SerializeField]
        private LoadMenu loadMenu;
        [SerializeField]
        private JoinMultiplayerMenu joinMultiplayerMenu;
        [SerializeField]
        private AboutDialog aboutDialog;
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
            titleMenu.gameObject.SetActive(State == MenuState.TITLE);
            generatorMenu.gameObject.SetActive(State == MenuState.GENERATOR);
            optionsMenu.gameObject.SetActive(State == MenuState.OPTIONS);
            loadMenu.gameObject.SetActive(State == MenuState.LOAD);
            joinMultiplayerMenu.gameObject.SetActive(State == MenuState.JOIN_MULTIPLAYER);
            aboutDialog.gameObject.SetActive(State == MenuState.ABOUT);
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
            State = MenuState.TITLE;
        }

        #endregion
    }
}
