using UnityEngine;
using System.Collections.Generic;
using CubeWorld.Configuration;
using CubeWorld.Gameplay;

public class GameManagerUnity : MonoBehaviour
{
    public CubeWorld.World.CubeWorld world;

    public Material material;
    public Material materialTransparent;
    public Material materialTranslucid;
    public Material materialDamaged;
    public Material materialItems;
	
	public Material materialLiquidAnimated;

    public CubeWorld.Configuration.ConfigExtraMaterials extraMaterials;

    public SurroundingsUnity surroundingsUnity;
    public WorldManagerUnity worldManagerUnity;

    [SerializeField]
    private Prefabs.ProgressBar progressBar;

    [SerializeField]
    private UnityNativeHandler unityNativeHandler;

    [HideInInspector]
    public PlayerUnity playerUnity;

    private GameState currentState;
    private GameState state;

    [SerializeField]
    public GameController gameController;
    [SerializeField]
    private Menu.Activator menuActivator;

	public SectorManagerUnity sectorManagerUnity;
	
	public CWObjectsManagerUnity objectsManagerUnity;
    public CWFxManagerUnity fxManagerUnity;

    private const string CubeworldWebServerServerRegister = "http://cubeworldweb.appspot.com/register?owner={owner}&description={description}&port={port}";

    public void Start()
    {
        MeshUtils.InitStaticValues();
        CubeWorldPlayerPreferences.LoadPreferences();
        PreferencesUpdated();

        state = currentState = GameState.MAIN_MENU;
        menuActivator.SetState(Menu.State.MAIN);

		sectorManagerUnity = new SectorManagerUnity(this);
		objectsManagerUnity = new CWObjectsManagerUnity(this);
        fxManagerUnity = new CWFxManagerUnity(this);

        worldManagerUnity = new WorldManagerUnity(this);

        unityNativeHandler.GameToWeb("Kernel", "Initialized");
	}

    public void DestroyWorld()
    {
		objectsManagerUnity.Clear();
		
		sectorManagerUnity.Clear();

        if (world != null)
        {
            world.Clear();
            world = null;
        }

        surroundingsUnity.Clear();
		
		playerUnity = null;

        System.GC.Collect(System.GC.MaxGeneration, System.GCCollectionMode.Forced);
    }

    public void SetState(GameState _state)
    {
        state = _state;
    }

    public GameState GetState()
    {
        return state;
    }

    public bool IsPaused()
    {
        return state == GameState.PAUSE;
    }

    static private string GetConfigText(string resourceName)
    {
        string configText = ((TextAsset)Resources.Load(resourceName)).text;

        #if !UNITY_WEBPLAYER

        if (Application.isEditor == false && Application.platform != RuntimePlatform.WebGLPlayer)
        {
            try
            {
                string exePath = System.IO.Directory.GetParent(Application.dataPath).FullName;
                string fileConfigPath = System.IO.Path.Combine(exePath, resourceName + ".xml");

                if (System.IO.File.Exists(fileConfigPath))
                    configText = System.IO.File.ReadAllText(fileConfigPath);
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }

        #endif

        return configText;
    }

    static public AvailableConfigurations LoadConfiguration()
    {
        AvailableConfigurations availableConfigurations = 
            new ConfigParserXML().Parse(
                GetConfigText("config_misc"),
                GetConfigText("config_tiles"),
                GetConfigText("config_avatars"),
                GetConfigText("config_items"),
                GetConfigText("config_generators"));

        return availableConfigurations;
    }

    public void LoadCustomTextures()
    {
        #if !UNITY_WEBPLAYER

            if (Application.isEditor == false && Application.platform != RuntimePlatform.WebGLPlayer)
            {
                try
                {
                    string exePath = System.IO.Directory.GetParent(Application.dataPath).FullName;
                    string fileTexturePath = System.IO.Path.Combine(exePath, "TexturaPrincipal.png");

                    if (System.IO.File.Exists(fileTexturePath))
                    {
                        Texture2D texture = (Texture2D) material.GetTexture("_MainTex");

                        texture.LoadImage(System.IO.File.ReadAllBytes(fileTexturePath));

                        material.mainTexture = texture;
                        materialTransparent.mainTexture = texture;
                        materialTranslucid.mainTexture = texture;
                        materialDamaged.mainTexture = texture;
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.Log(ex.ToString());
                }
            }

        #endif
    }

    #if !UNITY_WEBPLAYER

    private bool registerInWebServer = false;
    private WWW registerWebServerRequest;
    private float timerUpdate = 0;

    public void RegisterInWebServer()
    {
        timerUpdate -= Time.deltaTime;

        registerInWebServer = true;

        if (timerUpdate <= 0 && (registerWebServerRequest == null || registerWebServerRequest.isDone == true))
        {
            string url = CubeworldWebServerServerRegister;
            url = url.Replace("{owner}", "fede");
            url = url.Replace("{description}", "cwserver");
            url = url.Replace("{port}", CubeWorld.Gameplay.MultiplayerServerGameplay.SERVER_PORT.ToString());
            registerWebServerRequest = new WWW(url);
            timerUpdate = 30;
        }
    }

    #endif
        
    public void StartGame()
    {
        GetComponent<Camera>().enabled = false;

        state = GameState.GAME;
    }

    public bool Pause()
    {
        if (state != GameState.GAME) {
            return false;
        }
        state = GameState.PAUSE;
        menuActivator.SetState(Menu.State.PAUSE);
        return true;
    }

    public void Unpause()
    {
        if (state != GameState.PAUSE) {
            return;
        }
        state = GameState.GAME;
        menuActivator.SetState(Menu.State.NONE);
    }

    public void ReturnToMainMenu()
    {
        DestroyWorld();

        GetComponent<Camera>().enabled = true;

        state = GameState.MAIN_MENU;
        menuActivator.SetState(Menu.State.MAIN);
    }

    public void Update()
    {
#if !UNITY_WEBPLAYER
        if (registerInWebServer)
            RegisterInWebServer();
#endif

        switch (currentState)
        {
            case GameState.GENERATING:
            {
                if (worldManagerUnity.worldGeneratorProcess != null && worldManagerUnity.worldGeneratorProcess.IsFinished() == false)
                {
                    worldManagerUnity.worldGeneratorProcess.Generate();
                }
                else
                {
                    worldManagerUnity.worldGeneratorProcess = null;
				
                    PreferencesUpdated();

                    StartGame();
                }

                break;
            }

            case GameState.PAUSE:
            case GameState.GAME:
            {
                if (world != null && playerUnity != null)
                {
                    surroundingsUnity.UpdateSkyColor();
                    playerUnity.UpdateControlled();
                    world.Update(Time.deltaTime);
                    UpdateAnimatedTexture();
                }
                break;
            }
        }

        currentState = state;
        switch (state) {
            case GameState.GENERATING:
                progressBar.SetText(worldManagerUnity.worldGeneratorProcess.ToString());
                progressBar.SetProgress(worldManagerUnity.worldGeneratorProcess.GetProgress() / 100f);
                break;
        }
    }
	
	private float textureAnimationTimer;
	private int animFrames = 5;
	private int animFrame;
	private int textureAnimationFPS = 2;
	
	private void UpdateAnimatedTexture()
	{
		textureAnimationTimer += Time.deltaTime;
		
		if (textureAnimationTimer > 1.0f / textureAnimationFPS)
		{
			textureAnimationTimer = 0.0f;
			
			animFrame++;
			
			float uvdelta = 1.0f / GraphicsUnity.TILE_PER_MATERIAL_ROW;
			
			materialLiquidAnimated.mainTextureOffset = new Vector2(-(animFrame % animFrames) * uvdelta, 0.0f);
		}
	}

    public void PreferencesUpdated()
    {
        if (playerUnity)
        {
            playerUnity.mainCamera.farClipPlane = CubeWorldPlayerPreferences.farClipPlanes[CubeWorldPlayerPreferences.viewDistance];
        }
    }

    private CubeWorld.Configuration.Config lastConfig; 


    public bool HasLastConfig()
    {
        return lastConfig != null;
    }

    public void Generate(int currentDayInfoOffset, int currentGeneratorOffset, int currentSizeOffset, int currentGameplayOffset, bool multiplayer)
    {
        var availableConfigurations = GameManagerUnity.LoadConfiguration();;
        lastConfig = new CubeWorld.Configuration.Config();
        lastConfig.tileDefinitions = availableConfigurations.tileDefinitions;
        lastConfig.itemDefinitions = availableConfigurations.itemDefinitions;
        lastConfig.avatarDefinitions = availableConfigurations.avatarDefinitions;
        lastConfig.dayInfo = availableConfigurations.dayInfos[currentDayInfoOffset];
        lastConfig.worldGenerator = availableConfigurations.worldGenerators[currentGeneratorOffset];
        lastConfig.worldSize = availableConfigurations.worldSizes[currentSizeOffset];
        lastConfig.extraMaterials = availableConfigurations.extraMaterials;
        lastConfig.gameplay = GameplayFactory.AvailableGameplays[currentGameplayOffset];

        if (multiplayer)
        {
            MultiplayerServerGameplay multiplayerServerGameplay = new MultiplayerServerGameplay(lastConfig.gameplay.gameplay, true);
            GameplayDefinition g = new GameplayDefinition("", "", multiplayerServerGameplay, false);
            lastConfig.gameplay = g;
            RegisterInWebServer();
        }

        worldManagerUnity.CreateRandomWorld(lastConfig);
        menuActivator.SetState(Menu.State.NONE);
    }

    public void ReGenerate() {
        if (!HasLastConfig()) {
            return;
        }
        GetComponent<Camera>().enabled = true;
        worldManagerUnity.CreateRandomWorld(lastConfig);
        menuActivator.SetState(Menu.State.NONE);
    }
}

