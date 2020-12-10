namespace Shared
{
    public class GameLaunchArgsMultiplayer : GameLaunchArgs
    {
        public string host;
        public int port;

        #region GameLaunchArgs implements

        public void Setup(GameManagerUnity gameManagerUnity)
        {
            gameManagerUnity.worldManagerUnity.JoinMultiplayerGame(host, port);
        }

        #endregion
    }
}
