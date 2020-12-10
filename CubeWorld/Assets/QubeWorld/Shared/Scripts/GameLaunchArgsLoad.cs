namespace Shared
{
    public class GameLaunchArgsLoadWorld : GameLaunchArgs
    {
        public int number;

        #region GameLaunchArgs implements

        public void Setup(GameScene.GameManagerUnity gameManagerUnity)
        {
            gameManagerUnity.worldManagerUnity.LoadWorld(number);
        }

        #endregion
    }
}
