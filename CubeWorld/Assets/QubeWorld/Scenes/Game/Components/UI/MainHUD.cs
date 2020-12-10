using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    public class MainHUD : MonoBehaviour
    {
        [SerializeField]
        private GameManagerUnity gameManagerUnity;
        [SerializeField]
        private ProgressBar progressBar;
        [SerializeField]
        private GameController gameController;

        private GameState lastState;

        /*
        *              NONE
        *                |
        *                v
        *       +--  MAIN_MENU <--+
        *       |                 |
        *       v                 |
        *   GENERATING ------->  GAME
        *       ^                  ^
        *       |                  v
        *       +--------------- PAUSE
        */


        #region Unity lifecycles

        void Start()
        {
            lastState = GameState.NONE;
            progressBar.SetVisible(false);
            gameController.SetVisible(false);
        }

        void Update()
        {
            var state = gameManagerUnity.GetState();
            if (state != lastState)
            {
                switch (state)
                {
                    case GameState.GENERATING:
                        progressBar.SetVisible(true);
                        gameController.SetVisible(false);
                        break;
                    case GameState.GAME:
                        progressBar.SetVisible(false);
                        gameController.SetVisible(true);
                        break;
                    case GameState.PAUSE_MENU:
                        break;
                }
                lastState = state;
            }
        }

        #endregion
    }
}
