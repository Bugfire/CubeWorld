using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    public static class WorldGeneratorFactory
    {
        public static IWorldGenerator Run(GameManagerUnity gameManagerUnity, Shared.IWorldGeneratorArgs args)
        {
            if (args is Shared.LoadWorldGeneratorArgs)
            {
                return LoadWorldGenerator.Factory(gameManagerUnity, args as Shared.LoadWorldGeneratorArgs);
            }
            else if (args is Shared.JoinMultiplayerGeneratorArgs)
            {
                return JoinMultiplayerGenerator.Factory(gameManagerUnity, args as Shared.JoinMultiplayerGeneratorArgs);
            }
            else if (args is Shared.RandomWorldGeneratorArgs)
            {
                return RandomWorldGenerator.Factory(gameManagerUnity, args as Shared.RandomWorldGeneratorArgs);
            }
            else
            {
                Debug.Log("No args. Run with default arguments...");
                var defaultArgs = new Shared.RandomWorldGeneratorArgs();
                defaultArgs.Reset();
                return RandomWorldGenerator.Factory(gameManagerUnity, defaultArgs);
            }
        }
    }
}
