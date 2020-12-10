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

        static private Dictionary<int, System.DateTime> worldFileInfoCache = new Dictionary<int, System.DateTime>();

        public bool IsReady { get { return worldGeneratorProcess == null; } }
        public string ProcessText { get { return worldGeneratorProcess.ToString(); } }
        public float Progress { get { return worldGeneratorProcess.GetProgress() / 100f; } }

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

        static public System.DateTime GetWorldFileInfo(int n)
        {
            if (worldFileInfoCache.ContainsKey(n))
            {
                return worldFileInfoCache[n];
            }
            try
            {
                var path = GetWorldFilePath(n);
                if (!System.IO.File.Exists(path))
                {
                    return worldFileInfoCache[n] = System.DateTime.MinValue;
                }

                var fs = System.IO.File.OpenRead(path);
                try
                {
                    var br = new System.IO.BinaryReader(fs);
                    if (br.ReadString() == CubeWorld.World.CubeWorld.VERSION_INFO)
                    {
                        worldFileInfoCache[n] = System.IO.File.GetLastWriteTime(path);
                    }
                    else
                    {
                        worldFileInfoCache[n] = System.DateTime.MinValue;
                    }
                }
                finally
                {
                    fs.Close();
                }
            }
            catch (System.Exception)
            {
                return System.DateTime.MinValue;
            }
            return worldFileInfoCache[n];
        }

        public bool LoadWorld(int n)
        {
            if (!System.IO.File.Exists(GetWorldFilePath(n)))
            {
                return false;
            }

            gameManagerUnity.DestroyWorld();

            var configurations = Shared.Configure.Load();

            gameManagerUnity.LoadCustomTextures();

            var data = System.IO.File.ReadAllBytes(GetWorldFilePath(n));

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

            gameManagerUnity.SetState(GameScene.GameState.GENERATING);
            return true;
        }

        public void SaveWorld(int n)
        {
            var map = gameManagerUnity.world.Save();

            System.IO.File.WriteAllBytes(GetWorldFilePath(n), map);

            worldFileInfoCache.Clear();
        }

        public void JoinMultiplayerGame(string server, int port)
        {
            gameManagerUnity.DestroyWorld();

            worldGeneratorProcess = new GeneratorProcess(new MultiplayerGameLoaderGenerator(this, server, port), null);

            gameManagerUnity.SetState(GameScene.GameState.GENERATING);
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

            gameManagerUnity.SetState(GameScene.GameState.GENERATING);
        }

        #region Private methods

        private static string GetWorldFilePath(int n)
        {
            var exePath = System.IO.Directory.GetParent(Application.dataPath).FullName;
            return System.IO.Path.Combine(exePath, "world" + n + ".map");
        }

        #endregion
    }
}
