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

        private static GameLaunchArgs gameLaunchArgs;

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

        public static void GoToGameScene(GameLaunchArgs _gameLaunchArgs)
        {
            gameLaunchArgs = _gameLaunchArgs;
            goToGameScene();
        }

        public static void GoToTitleScene()
        {
            if (hasScene(TITLE_SCENE_NAME))
            {
                return;
            }
            SceneManager.LoadScene(TITLE_SCENE_NAME, LoadSceneMode.Additive);
            SceneManager.UnloadScene(GAME_SCENE_NAME);
        }

        public static bool SetupGameWithArgs(GameScene.GameManagerUnity gameManagerUnity)
        {
            if (gameLaunchArgs == null)
            {
                return false;
            }
            var t = gameLaunchArgs;
            gameLaunchArgs = null;
            t.Setup(gameManagerUnity);
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

        private static void goToGameScene()
        {
            if (hasScene(GAME_SCENE_NAME))
            {
                gameLaunchArgs = null;
                return;
            }
            SceneManager.LoadScene(GAME_SCENE_NAME, LoadSceneMode.Additive);
            SceneManager.UnloadScene(TITLE_SCENE_NAME);
        }

        #endregion
    }
}
