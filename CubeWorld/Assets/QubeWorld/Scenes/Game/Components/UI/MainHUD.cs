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

        private bool lastIsGenerating;

        #region Unity lifecycles

        void Start()
        {
            lastIsGenerating = false;
            progressBar.SetVisible(false);
            gameController.SetVisible(false);
        }

        void Update()
        {
            var isGenerating = gameManagerUnity.IsGenerating;
            if (isGenerating == lastIsGenerating)
            {
                return;
            }
            if (isGenerating)
            {
                progressBar.SetVisible(true);
                gameController.SetVisible(false);
            }
            else
            {
                progressBar.SetVisible(false);
                gameController.SetVisible(true);
            }
            lastIsGenerating = isGenerating;
        }

        #endregion
    }
}
