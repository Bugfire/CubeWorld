using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    public class RandomWorldGenerator : IWorldGenerator
    {
        private CubeWorld.World.Generator.GeneratorProcess worldGeneratorProcess;

        #region IWorldGenerator implements

        public void Update()
        {
            if (worldGeneratorProcess != null && worldGeneratorProcess.IsFinished() == false)
            {
                worldGeneratorProcess.Generate();
            }
            else
            {
                worldGeneratorProcess = null;
                IsFinished = true;
            }
        }

        public string ProcessText { get { return worldGeneratorProcess.ToString(); } }
        public float Progress { get { return worldGeneratorProcess.GetProgress() / 100f; } }
        public bool HasError { get { return false; } }
        public bool IsFinished { get; private set; }

        #endregion

        #region Public methods

        public static RandomWorldGenerator Factory(GameManagerUnity gameManagerUnity, CubeWorld.Configuration.Config config)
        {
            return new RandomWorldGenerator(gameManagerUnity, config);
        }

        public static RandomWorldGenerator Factory(GameManagerUnity gameManagerUnity, Shared.RandomWorldGeneratorArgs args)
        {
            var config = gameManagerUnity.CreateConfig(args);
            return new RandomWorldGenerator(gameManagerUnity, config);
        }

        #endregion

        #region Private methods

        private RandomWorldGenerator(GameManagerUnity gameManagerUnity, CubeWorld.Configuration.Config config)
        {
            gameManagerUnity.DestroyWorld();
            gameManagerUnity.LoadCustomTextures();

            gameManagerUnity.extraMaterials = config.extraMaterials;

            gameManagerUnity.world = new CubeWorld.World.CubeWorld(gameManagerUnity.objectsManagerUnity, gameManagerUnity.fxManagerUnity);
            worldGeneratorProcess = gameManagerUnity.world.Generate(config);

            gameManagerUnity.surroundingsUnity.CreateSurroundings(gameManagerUnity.world.configSurroundings);
        }

        #endregion
    }
}
