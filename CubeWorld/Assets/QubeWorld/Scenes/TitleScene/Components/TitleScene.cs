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
        private Shared.OptionsMenu optionsMenu;
        [SerializeField]
        private LoadMenu loadMenu;
        [SerializeField]
        private JoinMultiplayerMenu joinMultiplayerMenu;
        [SerializeField]
        private AboutDialog aboutDialog;
        [SerializeField]
        private GameObject barrier;

        private State lastState = State.NONE;

        public State State { get; set; }

        #region Unity lifecycles

        public void Update()
        {
            if (State == lastState)
            {
                return;
            }
            lastState = State;
            titleMenu.gameObject.SetActive(State == State.TITLE);
            generatorMenu.gameObject.SetActive(State == State.GENERATOR);
            optionsMenu.gameObject.SetActive(State == State.OPTIONS);
            loadMenu.gameObject.SetActive(State == State.LOAD);
            joinMultiplayerMenu.gameObject.SetActive(State == State.JOIN_MULTIPLAYER);
            aboutDialog.gameObject.SetActive(State == State.ABOUT);
            barrier.SetActive(State != State.NONE);
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
    }
}
