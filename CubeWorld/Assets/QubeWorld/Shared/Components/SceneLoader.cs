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

        private static IWorldGeneratorArgs worldGeneratorArgs;

        #region Unity lifecycles

        void Awake()
        {
            if (gameObject.scene.name == COMMON_SCENE_NAME)
            {
                if (SceneManager.sceneCount == 1)
                {
                    loadScene(TITLE_SCENE_NAME);
                }
            }
            else
            {
                if (!hasScene(COMMON_SCENE_NAME))
                {
                    loadScene(COMMON_SCENE_NAME);
                }
            }
        }

        #endregion

        #region Public methods

        public static void GoToGameScene(IWorldGeneratorArgs _worldGeneratoArgs)
        {
            worldGeneratorArgs = _worldGeneratoArgs;
            goToGameScene();
        }

        public static void GoToTitleScene()
        {
            if (hasScene(TITLE_SCENE_NAME))
            {
                return;
            }
            loadScene(TITLE_SCENE_NAME, GAME_SCENE_NAME);
        }

        public static IWorldGeneratorArgs GetWorldGeneratorArgs()
        {
            var r = worldGeneratorArgs;
            worldGeneratorArgs = null;
            return r;
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
                worldGeneratorArgs = null;
                return;
            }
            loadScene(GAME_SCENE_NAME, TITLE_SCENE_NAME);
        }

        private static void loadScene(string sceneName, string unloadSceneName = null)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            if (unloadSceneName != null)
            {
                SceneManager.UnloadScene(unloadSceneName);
            }
        }

        #endregion
    }
}
