using UnityEngine;

// メニュー画面の管理を行う
//   メニューには二つの起動ルートがあり
//      1: TITLE_MENU タイトルメニュー (起動時)
//          TITLE, GENERATOR, OPTIONS(*), JOIN_MULTIPLAYER, LOAD(*), ABONT
//      2: PAUSE_MENU ポーズメニュー (ゲーム中からの呼び出し)
//          PAUSE, OPTIONS(*), SAVE(*)
// (*) は共用されています
//
namespace Menu
{
    public class Activator : MonoBehaviour
    {
        [SerializeField]
        private GameManagerUnity gameManagerUnity;
        [SerializeField]
        private Title titleMenu;
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

        private State lastState = State.NONE;

        public State State { get; set; }

        #region Unity Lifecycles

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
            pauseMenu.gameObject.SetActive(State == State.PAUSE);
            loadSaveMenu.gameObject.SetActive(State == State.LOAD || State == State.SAVE);
            joinMultiplayerMenu.gameObject.SetActive(State == State.JOIN_MULTIPLAYER);
            aboutDialog.gameObject.SetActive(State == State.ABOUT);
            barrier.SetActive(State != State.NONE);
        }

        #endregion

        #region Unity User Events

        public void OnClose()
        {
            SetLastState();
        }

        #endregion

        #region Public methods

        public void SetLastState()
        {
            if (State == State.TITLE)
            {
                return;
            }
            // 1: TITLE では TITLE MENU が最上位
            // 2: PAUSE では、PAUSE MENU が最上位、で閉じるとゲームに戻る
            var gameState = gameManagerUnity.GetState();
            switch (gameState)
            {
                case GameState.TITLE_MENU:
                    State = State.TITLE;
                    break;
                case GameState.PAUSE_MENU:
                    if (State == State.PAUSE)
                    {
                        gameManagerUnity.Unpause();
                    }
                    else
                    {
                        State = State.PAUSE;
                    }
                    break;
            }
        }

        #endregion
    }
}
