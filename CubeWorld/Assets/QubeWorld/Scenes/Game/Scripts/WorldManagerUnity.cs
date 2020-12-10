using UnityEngine;
using System.Collections.Generic;
using CubeWorld.World.Generator;
using CubeWorld.Gameplay;
using CubeWorld.Configuration;

namespace GameScene
{
    public class WorldManagerUnity
    {
        private GameManagerUnity gameManagerUnity;
        private CubeWorld.World.Generator.GeneratorProcess worldGeneratorProcess;

        public bool IsReady { get { return worldGeneratorProcess == null; } }
        public string ProcessText { get { return worldGeneratorProcess.ToString(); } }
        public float Progress { get { return worldGeneratorProcess.GetProgress() / 100f; } }

        #region Public methods

        public WorldManagerUnity(GameManagerUnity gameManagerUnity)
        {
            this.gameManagerUnity = gameManagerUnity;
        }

        public void Update()
        {
            if (worldGeneratorProcess != null && worldGeneratorProcess.IsFinished() == false)
            {
                worldGeneratorProcess.Generate();
            }
            else
            {
                worldGeneratorProcess = null;
            }
        }

        public bool LoadWorld(int n)
        {
            var data = Shared.WorldFileIO.Load(n);
            if (data == null)
            {
                return false;
            }

            gameManagerUnity.DestroyWorld();

            var configurations = Shared.Configure.Load();

            gameManagerUnity.LoadCustomTextures();

            var config = new CubeWorld.Configuration.Config();
            config.tileDefinitions = configurations.tileDefinitions;
            config.itemDefinitions = configurations.itemDefinitions;
            config.avatarDefinitions = configurations.avatarDefinitions;
            config.extraMaterials = configurations.extraMaterials;

            gameManagerUnity.extraMaterials = config.extraMaterials;
            gameManagerUnity.world = new CubeWorld.World.CubeWorld(gameManagerUnity.objectsManagerUnity, gameManagerUnity.fxManagerUnity);
            gameManagerUnity.world.Load(config, data);
            worldGeneratorProcess = null;

            gameManagerUnity.surroundingsUnity.CreateSurroundings(gameManagerUnity.world.configSurroundings);

            gameManagerUnity.IsGenerating = true;
            return true;
        }

        public void SaveWorld(int n)
        {
            var map = gameManagerUnity.world.Save();

            Shared.WorldFileIO.Save(n, map);
        }

        public void JoinMultiplayerGame(string server, int port)
        {
            gameManagerUnity.DestroyWorld();

            worldGeneratorProcess = new GeneratorProcess(new MultiplayerGameLoaderGenerator(this, server, port), null);

            gameManagerUnity.IsGenerating = true;
        }

        private class MultiplayerGameLoaderGenerator : CubeWorldGenerator
        {
            private MultiplayerClientGameplay mutiplayerClientGameplay;
            private WorldManagerUnity worldManagerUnity;

            public MultiplayerGameLoaderGenerator(WorldManagerUnity worldManagerUnity, string server, int port)
            {
#if UNITY_WEBPLAYER
            Security.PrefetchSocketPolicy(server, port);
#endif
                this.worldManagerUnity = worldManagerUnity;
                mutiplayerClientGameplay = new MultiplayerClientGameplay(server, port);
            }

            public override bool Generate(CubeWorld.World.CubeWorld world)
            {
                mutiplayerClientGameplay.Update(0.0f);

                if (mutiplayerClientGameplay.initializationDataReceived)
                {
                    var gameManagerUnity = worldManagerUnity.gameManagerUnity;

                    gameManagerUnity.world = new CubeWorld.World.CubeWorld(gameManagerUnity.objectsManagerUnity, gameManagerUnity.fxManagerUnity);
                    gameManagerUnity.world.gameplay = mutiplayerClientGameplay;

                    var config = gameManagerUnity.world.LoadMultiplayer(mutiplayerClientGameplay.initializationData);

                    mutiplayerClientGameplay.initializationData = null;

                    gameManagerUnity.extraMaterials = config.extraMaterials;
                    gameManagerUnity.surroundingsUnity.CreateSurroundings(gameManagerUnity.world.configSurroundings);

                    return true;
                }

                return false;
            }
        }

        public void CreateRandomWorld(CubeWorld.Configuration.Config config)
        {
            gameManagerUnity.DestroyWorld();

            gameManagerUnity.LoadCustomTextures();

            gameManagerUnity.extraMaterials = config.extraMaterials;

            gameManagerUnity.world = new CubeWorld.World.CubeWorld(gameManagerUnity.objectsManagerUnity, gameManagerUnity.fxManagerUnity);
            worldGeneratorProcess = gameManagerUnity.world.Generate(config);

            gameManagerUnity.surroundingsUnity.CreateSurroundings(gameManagerUnity.world.configSurroundings);

            gameManagerUnity.IsGenerating = true;
        }

        #endregion
    }
}
