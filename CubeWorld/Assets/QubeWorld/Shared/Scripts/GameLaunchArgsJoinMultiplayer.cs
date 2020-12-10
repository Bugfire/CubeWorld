namespace Shared
{
    public class GameLaunchArgsMultiplayer : GameLaunchArgs
    {
        public string host;
        public int port;

        #region GameLaunchArgs implements

        public void Setup(GameScene.GameManagerUnity gameManagerUnity)
        {
            gameManagerUnity.worldManagerUnity.JoinMultiplayerGame(host, port);
        }

        #endregion
    }
}
