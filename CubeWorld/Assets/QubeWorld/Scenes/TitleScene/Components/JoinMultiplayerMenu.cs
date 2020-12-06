using UnityEngine;

namespace TitleScene
{
    public class JoinMultiplayerMenu : MonoBehaviour
    {
        private WWW wwwRequest;
        private string[] servers;
        private const string cubeworldWebServerServerList = "http://cubeworldweb.appspot.com/list";

        #region Unity lifecycles

        void OnDisable()
        {
            wwwRequest = null;
            servers = null;
        }

        void Update()
        {
            if (wwwRequest == null && servers == null)
            {
                wwwRequest = new WWW(cubeworldWebServerServerList);
            }

            if (servers == null && wwwRequest != null && wwwRequest.isDone)
            {
                servers = wwwRequest.text.Split(';');
            }

            if (wwwRequest != null && wwwRequest.isDone)
            {
                foreach (string s in servers)
                {
                    string[] ss = s.Split(',');

                    if (ss.Length >= 2)
                    {
                        // MenuSystem.Button("Join [" + ss[0] + ":" + ss[1] + "]", delegate ()
                    }
                }
            }
            //  MenuSystem.TextField("Waiting data from server..");
        }

        #endregion

        #region Unity user events

        public void OnSelectServer(int index)
        {
            wwwRequest = null;
            servers = null;
            // var args = new GameLaunchMultiplayer() { host = ss[0], port = System.Int32.Parse(ss[1]) };
            // Shared.SceneLoader.GoGame(args);
        }

        public void OnRefresh()
        {
            wwwRequest = null;
            servers = null;
        }

        #endregion
    }
}
