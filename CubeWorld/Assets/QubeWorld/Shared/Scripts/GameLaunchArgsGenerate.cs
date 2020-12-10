namespace Shared
{
    public class GameLaunchArgsGenerate : GameLaunchArgs
    {
        public int GameplayOffset;
        public int SizeOffset;
        public int GeneratorOffset;
        public int DayInfoOffset;
        public bool Multiplayer;

        #region GameLaunchArgs implements

        public void Setup(GameScene.GameManagerUnity gameManagerUnity)
        {
            gameManagerUnity.Generate(this);
        }

        #endregion

        #region Public methods

        public void Reset()
        {
            DayInfoOffset = 0;
            GeneratorOffset = 0;
            SizeOffset = 0;
            GameplayOffset = 0;
            Multiplayer = false;
        }

        #endregion

    }
}
