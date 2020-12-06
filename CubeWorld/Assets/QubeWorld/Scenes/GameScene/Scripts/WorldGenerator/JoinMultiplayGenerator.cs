using UnityEngine;
using System.Collections.Generic;
using CubeWorld.World.Generator;
using CubeWorld.Gameplay;
using CubeWorld.Configuration;

namespace GameScene
{
    public class JoinMultiplayerGenerator : IWorldGenerator
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
        public bool HasError { get; private set; }
        public bool IsFinished { get; private set; }

        #endregion


        #region Public methods

        public static JoinMultiplayerGenerator Factory(GameManagerUnity gameManagerUnity, Shared.JoinMultiplayerGeneratorArgs args)
        {
            return new JoinMultiplayerGenerator(gameManagerUnity, args.server, args.port);
        }

        #endregion

        #region Private methods

        private JoinMultiplayerGenerator(GameManagerUnity gameManagerUnity, string server, int port)
        {
            gameManagerUnity.DestroyWorld();

            worldGeneratorProcess = new GeneratorProcess(new MultiplayerGameLoaderGenerator(gameManagerUnity, server, port), null);
        }

        #endregion

        private class MultiplayerGameLoaderGenerator : CubeWorldGenerator
        {
            private MultiplayerClientGameplay mutiplayerClientGameplay;
            private GameManagerUnity gameManagerUnity;

            public MultiplayerGameLoaderGenerator(GameManagerUnity gameManagerUnity, string server, int port)
            {
#if UNITY_WEBPLAYER
                Security.PrefetchSocketPolicy(server, port);
#endif
                this.gameManagerUnity = gameManagerUnity;
                mutiplayerClientGameplay = new MultiplayerClientGameplay(server, port);
            }

            public override bool Generate(CubeWorld.World.CubeWorld world)
            {
                mutiplayerClientGameplay.Update(0.0f);

                if (mutiplayerClientGameplay.initializationDataReceived)
                {
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
    }
}
