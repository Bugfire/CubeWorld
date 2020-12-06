namespace GameScene
{
    public interface IWorldGenerator
    {
        void Update();

        string ProcessText { get; }
        float Progress { get; }
        bool IsFinished { get; }
        bool HasError { get; }
    }
}
