using UnityEngine;

namespace TitleScene
{
    public class TitleMenu : MonoBehaviour
    {
        [SerializeField]
        private TitleScene titleScene;

        #region Unity user events

        public void OnCreateRandomWorld()
        {
            titleScene.State = State.GENERATOR;
        }

        public void OnLoadWorld()
        {
            titleScene.State = State.LOAD;
        }

        public void OnJoinMultiPlayer()
        {
            titleScene.State = State.JOIN_MULTIPLAYER;
        }

        public void OnOptions()
        {
            titleScene.State = State.OPTIONS;
        }

        public void OnAbout()
        {
            titleScene.State = State.ABOUT;
        }

        #endregion
    }
}
