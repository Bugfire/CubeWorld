using UnityEngine;

namespace Menu
{
    public class JoinMultiplayer : MonoBehaviour
    {
        [SerializeField]
        private Activator activator;

        private WWW wwwRequest;
        private string[] servers;

        private const string CubeworldWebServerServerList = "http://cubeworldweb.appspot.com/list";

        #region Unity Lifecycles

        void OnDisable()
        {
            wwwRequest = null;
            servers = null;
        }

        void Update()
        {
            if (wwwRequest == null && servers == null)
            {
                wwwRequest = new WWW(CubeworldWebServerServerList);
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

        #region Unity Events

        public void OnSelectServer(int index)
        {
            // gameManagerUnity.worldManagerUnity.JoinMultiplayerGame(ss[0], System.Int32.Parse(ss[1]));
            wwwRequest = null;
            servers = null;
            activator.State = State.NONE;
        }

        public void OnRefresh()
        {
            wwwRequest = null;
            servers = null;
        }

        #endregion
    }
}
