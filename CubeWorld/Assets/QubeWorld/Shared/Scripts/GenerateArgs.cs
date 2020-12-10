namespace Shared
{
    public struct GenerateArgs
    {
        public int GameplayOffset;
        public int SizeOffset;
        public int GeneratorOffset;
        public int DayInfoOffset;
        public bool Multiplayer;

        #region Public methods

        public void Reset()
        {
            DayInfoOffset = 0;
            GeneratorOffset = 0;
            SizeOffset = 0;
            GameplayOffset = 0;
        }

        #endregion
    }
}
