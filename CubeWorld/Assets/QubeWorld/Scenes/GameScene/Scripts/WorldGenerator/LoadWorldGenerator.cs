using UnityEngine;
using System.Collections.Generic;
using CubeWorld.World.Generator;
using CubeWorld.Gameplay;
using CubeWorld.Configuration;

namespace GameScene
{
    public class LoadWorldGenerator : IWorldGenerator
    {
        #region IWorldGenerator implements

        public void Update()
        {
            IsFinished = true;
        }
        public string ProcessText { get { return ""; } }
        public float Progress { get { return 1f; } }
        public bool HasError { get; private set; }
        public bool IsFinished { get; private set; }

        #endregion

        #region Public methods

        public static LoadWorldGenerator Factory(GameManagerUnity gameManagerUnity, Shared.LoadWorldGeneratorArgs args)
        {
            return new LoadWorldGenerator(gameManagerUnity, args.number);
        }

        #endregion

        #region Private methods

        private LoadWorldGenerator(GameManagerUnity gameManagerUnity, int n)
        {
            var data = Shared.WorldFileIO.Load(n);
            if (data == null)
            {
                HasError = true;
                return;
            }

            gameManagerUnity.DestroyWorld();
            gameManagerUnity.LoadCustomTextures();

            var configurations = Shared.Configure.Load();
            var config = new CubeWorld.Configuration.Config();
            config.tileDefinitions = configurations.tileDefinitions;
            config.itemDefinitions = configurations.itemDefinitions;
            config.avatarDefinitions = configurations.avatarDefinitions;
            config.extraMaterials = configurations.extraMaterials;

            gameManagerUnity.extraMaterials = config.extraMaterials;
            gameManagerUnity.world = new CubeWorld.World.CubeWorld(gameManagerUnity.objectsManagerUnity, gameManagerUnity.fxManagerUnity);
            gameManagerUnity.world.Load(config, data);

            gameManagerUnity.surroundingsUnity.CreateSurroundings(gameManagerUnity.world.configSurroundings);
        }

        #endregion
    }
}
