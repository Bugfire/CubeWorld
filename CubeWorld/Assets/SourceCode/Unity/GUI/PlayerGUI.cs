using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CubeWorld.Tiles;
using CubeWorld.World.Objects;

namespace GameScene
{
    public class PlayerGUI : MonoBehaviour
    {
        public int lastFps = 0;

        private int frames;
        private float lastTime;

        public PlayerUnity playerUnity;
        public GameScene gameScene;

        private GUIState normalState;
        private GameController gameController
        {
            get
            {
                return playerUnity.gameManagerUnity.gameController;
            }
        }

        void Start()
        {
            lastTime = Time.realtimeSinceStartup;

            normalState = new GUIStatePlayerNormal(this);
            normalState.OnActivated();
        }

        void Update()
        {
            UpdateFPS();

            if (gameScene.IsPlayable)
                normalState.ProcessKeys(gameController);
        }

        private void UpdateFPS()
        {
            frames++;

            if (Time.realtimeSinceStartup - lastTime >= 1.0f)
            {
                lastFps = (int)((float)frames / (Time.realtimeSinceStartup - lastTime));
                lastTime = Time.realtimeSinceStartup;
                frames = 0;
            }
        }

        void OnGUI()
        {
            playerUnity.DrawUnderWaterTexture();

            if (!gameScene.IsGenerating)
                normalState.Draw();
        }
    }
}
