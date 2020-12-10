using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 必要があればクラスを読み込む
// WebGL がターゲットなので Async は使わない。
namespace Shared
{
    public class SceneLoader : MonoBehaviour
    {
        private const string COMMON_SCENE_NAME = "Common";
        private const string TITLE_SCENE_NAME = "Title";
        private const string GAME_SCENE_NAME = "Game";


        private interface GameLaunchMode
        {
            void Go(GameManagerUnity gameManagerUnity);
        }

        private class GameLaunchLoadWorld : GameLaunchMode
        {
            public int number;
            public void Go(GameManagerUnity gameManagerUnity)
            {
                gameManagerUnity.worldManagerUnity.LoadWorld(number);
            }
        }

        private class GameLaunchGenerate : GameLaunchMode
        {
            public Shared.GenerateArgs generateArgs;
            public void Go(GameManagerUnity gameManagerUnity)
            {
                gameManagerUnity.Generate(
                    generateArgs.DayInfoOffset,
                    generateArgs.GeneratorOffset,
                    generateArgs.SizeOffset,
                    generateArgs.GameplayOffset,
                    generateArgs.Multiplayer);
            }
        }

        private class GameLaunchMultiplayer : GameLaunchMode
        {
            public string host;
            public int port;
            public void Go(GameManagerUnity gameManagerUnity)
            {
                gameManagerUnity.worldManagerUnity.JoinMultiplayerGame(host, port);
            }
        }

        private static GameLaunchMode gameLaunchMode;

        #region Unity lifecycles

        void Awake()
        {
            if (gameObject.scene.name == COMMON_SCENE_NAME)
            {
                if (SceneManager.sceneCount == 1)
                {
                    SceneManager.LoadScene(TITLE_SCENE_NAME, LoadSceneMode.Additive);
                }
            }
            else
            {
                if (!hasScene(COMMON_SCENE_NAME))
                {
                    SceneManager.LoadScene(COMMON_SCENE_NAME, LoadSceneMode.Additive);
                }
            }
        }

        #endregion

        #region Public methods

        public static void GoGameWithLoadWorld(int _number)
        {
            gameLaunchMode = new GameLaunchLoadWorld() { number = _number };
            goGame();
        }

        public static void GoGameWithGenerate(ref Shared.GenerateArgs _generateArgs)
        {
            gameLaunchMode = new GameLaunchGenerate() { generateArgs = _generateArgs };
            goGame();
        }

        public static void GoGameWithMultiplayer(string _host, int _port)
        {
            gameLaunchMode = new GameLaunchMultiplayer() { host = _host, port = _port };
            goGame();
        }

        public static void GoTitle()
        {
            if (hasScene(TITLE_SCENE_NAME))
            {
                return;
            }
            SceneManager.LoadScene(TITLE_SCENE_NAME, LoadSceneMode.Additive);
            SceneManager.UnloadScene(GAME_SCENE_NAME);
        }

        public static bool SetupGameWithArgs(GameManagerUnity gameManagerUnity)
        {
            if (gameLaunchMode == null)
            {
                return false;
            }
            var t = gameLaunchMode;
            gameLaunchMode = null;
            t.Go(gameManagerUnity);
            return true;
        }

        #endregion

        #region Private methods

        private static bool hasScene(string sceneName)
        {
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.name == sceneName)
                {
                    return true;
                }
            }
            return false;
        }

        private static void goGame()
        {
            if (hasScene(GAME_SCENE_NAME))
            {
                gameLaunchMode = null;
                return;
            }
            SceneManager.LoadScene(GAME_SCENE_NAME, LoadSceneMode.Additive);
            SceneManager.UnloadScene(TITLE_SCENE_NAME);
        }

        #endregion
    }
}
