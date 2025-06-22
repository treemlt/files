// Decompiled with JetBrains decompiler
// Type: GameManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using Audio;
using GUI_2;
using InControl;
using Platform;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;
using Twitch;
using Unity.Collections;
using UnityEngine;

#nullable disable
public class GameManager : MonoBehaviour, IGameManager
{
  public static int frameCount;
  public static float frameTime;
  public static int fixedUpdateCount;
  public AudioSource UIAudioSource;
  public AudioClip BackgroundMusicClip;
  public AudioClip CreditsSongClip;
  public bool DebugAILines;
  public StabilityViewer stabilityViewer;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public BiomeParticleManager biomeParticleManager;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int cameraCullMask;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool bShowBackground = true;
  public static bool enableNetworkdPrioritization = true;
  public static bool unreliableNetPackets = true;
  public static ServerDateTimeResult ServerClockSync;
  public NetPackageMetrics netpackageMetrics;
  public bool showOpenerMovieOnLoad;
  public bool GameHasStarted;
  public Color backgroundColor = Color.white;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int currentBackgroundColorChannel;
  public static bool bPhysicsActive;
  public static bool bTickingActive;
  public static bool bSavingActive = true;
  public static bool bShowDecorBlocks = true;
  public static bool bShowLootBlocks = true;
  public static bool bShowPaintables = true;
  public static bool bShowUnpaintables = true;
  public static bool bShowTerrain = true;
  public static bool bVolumeBlocksEditing;
  public static bool bHideMainMenuNextTime;
  public static bool bRecordNextSession;
  public static bool bPlayRecordedSession;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public static bool isDedicatedChecked = false;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public static bool isDedicated = false;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public World m_World;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool worldCreated;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool chunkClusterLoaded;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int myPlayerId = -1;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public EntityPlayerLocal myEntityPlayerLocal;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public IMapChunkDatabase fowDatabaseForLocalPlayer;
  public FPS fps = new FPS(5f);
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public GameObject m_SoundsGameObject;
  public GUIWindowConsole m_GUIConsole;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float lastTimeWorldTickTimeSentToClients;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float lastTimeGameStateCheckedAndSynced;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float lastTimeDecoSaved;
  public AdminTools adminTools;
  public PersistentPlayerList persistentPlayers;
  public PersistentPlayerData persistentLocalPlayer;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public GUIWindowManager windowManager;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public NGUIWindowManager nguiWindowManager;
  public LootManager lootManager;
  public TraderManager traderManager;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int lastDisplayedValueOfTeamTickets;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public Dictionary<Vector3i, GameObject> m_PositionSoundMap = new Dictionary<Vector3i, GameObject>();
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public List<GameObject> tileEntitiesMusicToRemove = new List<GameObject>();
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int msPassedSinceLastUpdate;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public IBlockTool activeBlockTool;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public IBlockTool blockSelectionTool;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool isEditMode;
  [CompilerGenerated]
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool \u003CGameIsFocused\u003Ek__BackingField;
  public bool bCursorVisible = true;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool bCursorVisibleOverride;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool bCursorVisibleOverrideState;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public DictionarySave<Vector3i, Transform> m_BlockParticles = new DictionarySave<Vector3i, Transform>();
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public CountdownTimer countdownCheckBlockParticles = new CountdownTimer(1.1f);
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public CountdownTimer countdownSendPlayerDataFileToServer = new CountdownTimer(30f);
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public CountdownTimer countdownSaveLocalPlayerDataFile = new CountdownTimer(30f);
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float unloadAssetsDuration;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool isUnloadAssetsReady;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public readonly MicroStopwatch stopwatchUnloadAssets = new MicroStopwatch(false);
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public CountdownTimer countdownSendPlayerInventoryToServer = new CountdownTimer(0.1f, false);
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool sendPlayerToolbelt;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool sendPlayerBag;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool sendPlayerEquipment;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool sendDragAndDropItem;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public Dictionary<ITileEntity, int> lockedTileEntities = new Dictionary<ITileEntity, int>();
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public GameRandomManager gameRandomManager;
  public GameStateManager gameStateManager;
  public PrefabLODManager prefabLODManager;
  public PrefabEditModeManager prefabEditModeManager;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public RespawnType clientRespawnType;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float fpsCountdownTimer = 30f;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float gcCountdownTimer = 120f;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float wsCountdownTimer = 30f;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float playerPositionsCountdownTimer = 10f;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public MicroStopwatch swCopyChunks = new MicroStopwatch();
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public MicroStopwatch swUpdateTime = new MicroStopwatch();
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int lastStatsPlayerCount;
  public static long MaxMemoryConsumption;
  public WaitForTargetFPS waitForTargetFPS;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public List<GameManager.BlockParticleCreationData> blockParticlesToSpawn = new List<GameManager.BlockParticleCreationData>();
  public static GameManager Instance;
  public TriggerEffectManager triggerEffectManager;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int lastPlayerCount;
  public bool bStaticDataLoadSync;
  public bool bStaticDataLoaded;
  public string CurrentLoadAction;
  [CompilerGenerated]
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool \u003CIsStartingGame\u003Ek__BackingField;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int lastTimeAbsPosSentToServer;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool bLastWasAttached;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float lastTime;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float timeToClearAllPools = -1f;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float activityCheck;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool shuttingDownMultiplayerServices;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int testing;
  public bool canSpawnPlayer;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool isDisconnectingLater;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool allowQuit;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool isQuitting;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool firstTimeJoin;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public List<BlockChangeInfo> tempExplPositions = new List<BlockChangeInfo>();
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public List<GameManager.ExplodeGroup> explodeFallingGroups = new List<GameManager.ExplodeGroup>();
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public readonly List<ChunkCluster> ccChanged = new List<ChunkCluster>();
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public static List<string> materialsBefore = (List<string>) null;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public static float unusedAssetsTimer = 0.0f;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public static bool runningAssetsUnused = false;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool gamePaused;
  [CompilerGenerated]
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public static bool \u003CUpdatingRemoteResources\u003Ek__BackingField;
  [CompilerGenerated]
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public static bool \u003CRemoteResourcesLoaded\u003Ek__BackingField;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool retrievingEula;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool retrievingBacktraceConfig;
  public static bool DebugCensorship;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public List<string> persistentPlayerIds;

  [PublicizedFrom(EAccessModifier.Private)]
  public IEnumerator waitForGameStart()
  {
    if (!GameManager.IsDedicatedServer && !this.IsEditMode())
    {
      EntityPlayerLocal epl = (EntityPlayerLocal) null;
      while (this.World != null)
      {
        epl = this.World.GetPrimaryPlayer();
        if (!Object.op_Inequality((Object) epl, (Object) null))
        {
          yield return (object) null;
        }
        else
        {
          while (!epl.IsSpawned())
            yield return (object) null;
          epl.HasUpdated = false;
          while (!epl.HasUpdated)
            yield return (object) false;
          yield return (object) null;
          this.GameHasStarted = true;
          epl = (EntityPlayerLocal) null;
          break;
        }
      }
    }
  }

  public void ShowBackground(bool show)
  {
    if (this.bShowBackground == show)
      return;
    this.bShowBackground = show;
    Camera main = Camera.main;
    if (!Object.op_Inequality((Object) main, (Object) null))
      return;
    if (!this.bShowBackground)
    {
      this.cameraCullMask = main.cullingMask;
      main.cullingMask = LayerMask.GetMask(new string[1]
      {
        "LocalPlayer"
      });
      main.backgroundColor = this.backgroundColor;
    }
    else
      main.cullingMask = this.cameraCullMask;
  }

  public bool ShowBackground() => this.bShowBackground;

  public void IncreaseBackgroundColor()
  {
    switch (this.currentBackgroundColorChannel)
    {
      case 0:
        this.backgroundColor.r += 0.003921569f;
        break;
      case 1:
        this.backgroundColor.g += 0.003921569f;
        break;
      case 2:
        this.backgroundColor.b += 0.003921569f;
        break;
    }
    this.backgroundColor.r = Mathf.Clamp01(this.backgroundColor.r);
    this.backgroundColor.g = Mathf.Clamp01(this.backgroundColor.g);
    this.backgroundColor.b = Mathf.Clamp01(this.backgroundColor.b);
    Camera main = Camera.main;
    if (!Object.op_Inequality((Object) main, (Object) null))
      return;
    main.backgroundColor = this.backgroundColor;
  }

  public void DecreaseBackgroundColor()
  {
    switch (this.currentBackgroundColorChannel)
    {
      case 0:
        this.backgroundColor.r -= 0.003921569f;
        break;
      case 1:
        this.backgroundColor.g -= 0.003921569f;
        break;
      case 2:
        this.backgroundColor.b -= 0.003921569f;
        break;
    }
    this.backgroundColor.r = Mathf.Clamp01(this.backgroundColor.r);
    this.backgroundColor.g = Mathf.Clamp01(this.backgroundColor.g);
    this.backgroundColor.b = Mathf.Clamp01(this.backgroundColor.b);
    Camera main = Camera.main;
    if (!Object.op_Inequality((Object) main, (Object) null))
      return;
    main.backgroundColor = this.backgroundColor;
  }

  public void BackgroundColorNext()
  {
    ++this.currentBackgroundColorChannel;
    if (this.currentBackgroundColorChannel <= 2)
      return;
    this.currentBackgroundColorChannel = 0;
  }

  public void BackgroundColorPrev()
  {
    --this.currentBackgroundColorChannel;
    if (this.currentBackgroundColorChannel >= 0)
      return;
    this.currentBackgroundColorChannel = 2;
  }

  public static bool IsDedicatedServer => true;

  [PublicizedFrom(EAccessModifier.Private)]
  public void OnUserDetailsUpdated(IPlatformUserData userData, string name)
  {
    if (this.persistentPlayers == null)
      return;
    this.persistentPlayers.HandlePlayerDetailsUpdate(userData, name);
  }

  public bool GameIsFocused
  {
    get => this.\u003CGameIsFocused\u003Ek__BackingField;
    [PublicizedFrom(EAccessModifier.Private)] set
    {
      this.\u003CGameIsFocused\u003Ek__BackingField = value;
    }
  }

  public bool IsMouseCursorVisible
  {
    get
    {
      return PlatformManager.NativePlatform.Input.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard && Cursor.visible;
    }
  }

  public event GameManager.OnWorldChangedEvent OnWorldChanged;

  public event GameManager.OnLocalPlayerChangedEvent OnLocalPlayerChanged;

  public event Action<ClientInfo> OnClientSpawned;

  public void ApplyAllOptions()
  {
    if (!Object.op_Inequality((Object) this.windowManager, (Object) null))
      return;
    GameOptionsManager.ApplyAllOptions(this.windowManager.playerUI);
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public void Awake()
  {
    if (!GameEntrypoint.EntrypointSuccess)
      return;
    MicroStopwatch microStopwatch = new MicroStopwatch(true);
    GameManager.Instance = this;
    this.GameIsFocused = !GameManager.IsDedicatedServer && Application.isFocused;
    Log.Out("Awake IsFocused: " + Application.isFocused.ToString());
    Log.Out(nameof (Awake));
    ThreadManager.SetMonoBehaviour((MonoBehaviour) this);
    Utils.InitStatic();
    LoadManager.Init();
    if (Application.isEditor)
    {
      Application.runInBackground = true;
      this.bCursorVisibleOverride = true;
      this.bCursorVisibleOverrideState = true;
    }
    Application.targetFrameRate = 500;
    Application.wantsToQuit += new Func<bool>(this.OnApplicationQuit);
    if (GameManager.IsDedicatedServer)
    {
      if (GamePrefs.GetBool(EnumGamePrefs.TerminalWindowEnabled))
      {
        try
        {
          WinFormInstance _server = new WinFormInstance();
          SingletonMonoBehaviour<SdtdConsole>.Instance.RegisterServer((IConsoleServer) _server);
        }
        catch (Exception ex)
        {
          Log.Error("Could not start Terminal Window:");
          Log.Exception(ex);
        }
      }
    }
    Log.Out("Dedicated server only build");
    this.windowManager = (GUIWindowManager) Object.FindObjectOfType(typeof (GUIWindowManager));
    this.nguiWindowManager = Object.FindObjectOfType<NGUIWindowManager>();
    TaskManager.Init();
    LocalPlayerManager.Init();
    if (!GameManager.IsDedicatedServer)
      GameOptionsManager.LoadControls();
    MeshDataManager.Init();
    OcclusionManager.Load();
    this.waitForTargetFPS = new GameObject("WaitForTargetFPS").AddComponent<WaitForTargetFPS>();
    if (GameManager.IsDedicatedServer)
    {
      GameOptionsManager.ApplyTextureQuality(3);
      QualitySettings.vSyncCount = 0;
      this.waitForTargetFPS.TargetFPS = 0;
    }
    else
    {
      QualitySettings.vSyncCount = GamePrefs.GetInt(PlatformApplicationManager.Application.VSyncCountPref);
      this.waitForTargetFPS.TargetFPS = 0;
    }
    ServerDateTimeRequest.GetNtpTimeAsync((Action<ServerDateTimeResult>) ([PublicizedFrom(EAccessModifier.Internal)] (result) => GameManager.ServerClockSync = result));
    GameObjectPool.Instance.Init();
    MemoryPools.InitStatic(!GameManager.IsDedicatedServer);
    this.gameRandomManager = GameRandomManager.Instance;
    this.gameStateManager = new GameStateManager(this);
    this.prefabLODManager = new PrefabLODManager();
    this.prefabEditModeManager = new PrefabEditModeManager();
    Object.Instantiate<GameObject>(DataLoader.LoadAsset<GameObject>("@:Sound_Mixers/AudioMixerManager.prefab"));
    this.m_SoundsGameObject = GameObject.Find("Sounds");
    this.PhysicsInit();
    ParticleEffect.Init();
    SelectionBoxManager.Instance.CreateCategory("Selection", SelectionBoxManager.ColSelectionActive, SelectionBoxManager.ColSelectionInactive, SelectionBoxManager.ColSelectionFaceSel, false, (string) null);
    SelectionBoxManager.Instance.CreateCategory("StartPoint", SelectionBoxManager.ColStartPointActive, SelectionBoxManager.ColStartPointInactive, SelectionBoxManager.ColStartPointActive, true, "SB_StartPoint", 31 /*0x1F*/);
    SelectionBoxManager.Instance.CreateCategory("DynamicPrefabs", SelectionBoxManager.ColDynamicPrefabActive, SelectionBoxManager.ColDynamicPrefabInactive, SelectionBoxManager.ColDynamicPrefabFaceSel, true, "SB_Prefabs", 31 /*0x1F*/);
    SelectionBoxManager.Instance.CreateCategory("TraderTeleport", SelectionBoxManager.ColTraderTeleport, SelectionBoxManager.ColTraderTeleportInactive, SelectionBoxManager.ColTraderTeleport, true, "SB_TraderTeleport", 31 /*0x1F*/);
    SelectionBoxManager.Instance.CreateCategory("SleeperVolume", SelectionBoxManager.ColSleeperVolume, SelectionBoxManager.ColSleeperVolumeInactive, SelectionBoxManager.ColSleeperVolume, true, "SB_SleeperVolume", 31 /*0x1F*/);
    SelectionBoxManager.Instance.CreateCategory("InfoVolume", SelectionBoxManager.ColInfoVolume, SelectionBoxManager.ColInfoVolumeInactive, SelectionBoxManager.ColInfoVolume, true, "SB_InfoVolume", 31 /*0x1F*/);
    SelectionBoxManager.Instance.CreateCategory("WallVolume", SelectionBoxManager.ColWallVolume, SelectionBoxManager.ColWallVolumeInactive, SelectionBoxManager.ColWallVolume, true, "SB_WallVolume", 31 /*0x1F*/);
    SelectionBoxManager.Instance.CreateCategory("TriggerVolume", SelectionBoxManager.ColTriggerVolume, SelectionBoxManager.ColTriggerVolumeInactive, SelectionBoxManager.ColTriggerVolume, true, "SB_TriggerVolume", 31 /*0x1F*/);
    SelectionBoxManager.Instance.CreateCategory("POIMarker", SelectionBoxManager.ColDynamicPrefabActive, SelectionBoxManager.ColDynamicPrefabInactive, SelectionBoxManager.ColDynamicPrefabFaceSel, true, "SB_Prefabs", 31 /*0x1F*/);
    SelectionBoxManager.Instance.CreateCategory("PrefabFacing", SelectionBoxManager.ColSleeperVolume, SelectionBoxManager.ColSleeperVolumeInactive, SelectionBoxManager.ColSleeperVolume, true, "SB_SleeperVolume", 31 /*0x1F*/);
    if (!GameManager.IsDedicatedServer)
    {
      if (13 != GamePrefs.GetInt(EnumGamePrefs.LastGameResetRevision))
      {
        if (this.ResetGame())
        {
          GamePrefs.Set(EnumGamePrefs.LastGameResetRevision, 13);
          GamePrefs.Set(EnumGamePrefs.OptionsGfxResetRevision, 4);
          GamePrefs.Instance.Save();
          Log.Out("Game Reset");
        }
        else
          Log.Warning("Failed to Reset Game!");
      }
      else
      {
        if (4 != GamePrefs.GetInt(EnumGamePrefs.OptionsGfxResetRevision) && GameOptionsManager.ResetGameOptions(GameOptionsManager.ResetType.Graphics))
        {
          GamePrefs.Set(EnumGamePrefs.OptionsGfxResetRevision, 4);
          GamePrefs.Instance.Save();
          Log.Out("Graphics Reset");
        }
        if (7 != GamePrefs.GetInt(EnumGamePrefs.OptionsControlsResetRevision) && GameOptionsManager.ResetGameOptions(GameOptionsManager.ResetType.Controls))
        {
          GamePrefs.Set(EnumGamePrefs.OptionsControlsResetRevision, 7);
          GamePrefs.Instance.Save();
          Log.Out("Controls Reset");
        }
      }
    }
    DeviceGamePrefs.Apply();
    if (!GameManager.IsDedicatedServer)
    {
      GameOptionsManager.ApplyAllOptions(this.windowManager.playerUI);
      UIUtils.LoadAtlas();
    }
    Manager.Init();
    UIOptions.Init();
    UIRoot objectOfType = Object.FindObjectOfType<UIRoot>();
    if (!GameManager.IsDedicatedServer)
      this.InitMultiSourceUiAtlases(((Component) objectOfType).gameObject);
    ((Component) this.windowManager).gameObject.AddComponent<LocalPlayerUI>();
    this.blockSelectionTool = (IBlockTool) new BlockToolSelection();
    this.nguiWindowManager.ParseWindows();
    this.nguiWindowManager.SetBackgroundScale(GameOptionsManager.GetActiveUiScale());
    this.AddWindows(this.windowManager);
    this.m_GUIConsole = (GUIWindowConsole) this.windowManager.GetWindow(GUIWindowConsole.ID);
    ModManager.LoadMods();
    ThreadManager.RunCoroutineSync(ModManager.LoadPatchStuff(false));
    this.adminTools = new AdminTools();
    SingletonMonoBehaviour<SdtdConsole>.Instance.RegisterCommands();
    IEnumerator enumerator = this.loadStaticData();
    if (GameManager.IsDedicatedServer)
    {
      this.bStaticDataLoadSync = true;
      ThreadManager.RunCoroutineSync(enumerator);
    }
    else
    {
      this.bStaticDataLoadSync = false;
      ThreadManager.StartCoroutine(enumerator);
    }
    if (!GameManager.IsDedicatedServer)
      CursorControllerAbs.LoadStaticData(LoadManager.CreateGroup());
    else
      InputManager.Enabled = false;
    if (GameManager.IsDedicatedServer)
    {
      if (GamePrefs.GetBool(EnumGamePrefs.TelnetEnabled))
      {
        try
        {
          TelnetConsole _server = new TelnetConsole();
          SingletonMonoBehaviour<SdtdConsole>.Instance.RegisterServer((IConsoleServer) _server);
        }
        catch (Exception ex)
        {
          Log.Error("Could not start network console:");
          Log.Exception(ex);
        }
      }
    }
    AuthorizationManager.Instance.Init();
    ModEvents.SGameAwakeData _data;
    ModEvents.GameAwake.Invoke(ref _data);
    this.nguiWindowManager.Show(EnumNGUIWindow.InGameHUD, false);
    ConsoleCmdShow.Init();
    if (!GameManager.IsDedicatedServer)
    {
      GameSenseManager.Instance?.Init();
      if (GamePrefs.GetBool(EnumGamePrefs.OptionsMumblePositionalAudioSupport))
        MumblePositionalAudio.Init();
    }
    DiscordManager instance = DiscordManager.Instance;
    if (Object.op_Implicit((Object) this.BackgroundMusicClip) || Object.op_Implicit((Object) this.CreditsSongClip))
    {
      if (!GameManager.IsDedicatedServer)
      {
        ((Component) this).gameObject.AddComponent<BackgroundMusicMono>();
      }
      else
      {
        Resources.UnloadAsset((Object) this.BackgroundMusicClip);
        Resources.UnloadAsset((Object) this.CreditsSongClip);
      }
    }
    PartyQuests.EnforeInstance();
    Input.simulateMouseWithTouches = false;
    IApplicationStateController applicationState = PlatformManager.NativePlatform?.ApplicationState;
    if (applicationState != null)
    {
      Platform.ApplicationState lastState = Platform.ApplicationState.Foreground;
      applicationState.OnApplicationStateChanged += (IApplicationStateController.ApplicationStateChanged) ([PublicizedFrom(EAccessModifier.Internal)] (state) =>
      {
        if (state != Platform.ApplicationState.Suspended && lastState == Platform.ApplicationState.Suspended)
          this.OnApplicationResume();
        lastState = state;
      });
      applicationState.OnNetworkStateChanged += new IApplicationStateController.NetworkStateChanged(this.OnNetworkStateChanged);
    }
    PlatformUserManager.DetailsUpdated += new PlatformUserDetailsUpdatedHandler(this.OnUserDetailsUpdated);
    if (!GameManager.IsDedicatedServer)
    {
      this.triggerEffectManager = new TriggerEffectManager();
      TriggerEffectManager.SetMainMenuLightbarColor();
    }
    Log.Out($"Awake done in {((Stopwatch) microStopwatch).ElapsedMilliseconds.ToString()} ms");
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void InitMultiSourceUiAtlases(GameObject _parent)
  {
    GameObject gameObject1 = new GameObject("UIAtlases");
    gameObject1.transform.parent = _parent.transform;
    Shader shader = Shader.Find("Unlit/Transparent Colored");
    Shader _shader = Shader.Find("Unlit/Transparent Greyscale");
    MultiSourceAtlasManager _atlasManager1 = MultiSourceAtlasManager.Create(gameObject1, "ItemIconAtlas");
    MultiSourceAtlasManager _atlasManager2 = MultiSourceAtlasManager.Create(gameObject1, "ItemIconAtlasGreyscale");
    ModManager.ModAtlasesDefaults(gameObject1, shader);
    ModManager.RegisterAtlasManager(_atlasManager1, false, shader, new Action<UIAtlas, bool>(this.AddGreyscaleItemIconAtlas));
    ModManager.RegisterAtlasManager(_atlasManager2, false, _shader);
    Resources.Load<UIAtlas>("GUI/Prefabs/SymbolAtlas");
    Resources.Load<UIAtlas>("GUI/Prefabs/ControllerArtAtlas");
    UIAtlas[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll<UIAtlas>();
    for (int index = 0; index < objectsOfTypeAll.Length; ++index)
    {
      string name = ((Object) ((Component) objectsOfTypeAll[index]).gameObject).name;
      if (!name.ContainsCaseInsensitive("icons_"))
      {
        GameObject gameObject2 = Object.Instantiate<GameObject>(((Component) objectsOfTypeAll[index]).gameObject);
        ((Object) gameObject2).name = name;
        UIAtlas component = gameObject2.GetComponent<UIAtlas>();
        MultiSourceAtlasManager _atlasManager3 = MultiSourceAtlasManager.Create(gameObject1, name);
        gameObject2.transform.parent = ((Component) _atlasManager3).transform;
        ModManager.RegisterAtlasManager(_atlasManager3, false, component.spriteMaterial.shader);
        _atlasManager3.AddAtlas(component, false);
      }
    }
    string mipFilter = GameOptionsPlatforms.GetItemIconFilterString();
    LoadManager.AddressableAssetsRequestTask<GameObject> assetsRequestTask = LoadManager.LoadAssetsFromAddressables<GameObject>("iconatlas", (Func<string, bool>) ([PublicizedFrom(EAccessModifier.Internal)] (address) => address.EndsWith(".prefab", StringComparison.OrdinalIgnoreCase) && address.Contains(mipFilter)), _loadSync: true);
    List<GameObject> gameObjectList = new List<GameObject>();
    List<GameObject> _results = gameObjectList;
    assetsRequestTask.CollectResults(_results);
    foreach (GameObject gameObject3 in gameObjectList)
    {
      GameObject gameObject4 = Object.Instantiate<GameObject>(gameObject3);
      gameObject4.transform.parent = ((Component) _atlasManager1).transform;
      UIAtlas component = gameObject4.GetComponent<UIAtlas>();
      _atlasManager1.AddAtlas(component, false);
      this.AddGreyscaleItemIconAtlas(component, false);
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void AddGreyscaleItemIconAtlas(UIAtlas _atlas, bool _isLoadingInGame)
  {
    MultiSourceAtlasManager atlasManager = ModManager.GetAtlasManager("ItemIconAtlasGreyscale");
    Shader shader = Shader.Find("Unlit/Transparent Greyscale");
    UIAtlas _atlas1 = Object.Instantiate<UIAtlas>(_atlas, ((Component) atlasManager).transform);
    _atlas1.spriteMaterial = new Material(shader)
    {
      mainTexture = _atlas1.texture
    };
    atlasManager.AddAtlas(_atlas1, _isLoadingInGame);
  }

  public void AddWindows(GUIWindowManager _guiWindowManager)
  {
    if (Object.op_Equality((Object) _guiWindowManager, (Object) this.windowManager))
    {
      _guiWindowManager.Add(GUIWindowConsole.ID, (GUIWindow) new GUIWindowConsole());
      _guiWindowManager.Add(GUIWindowScreenshotText.ID, (GUIWindow) new GUIWindowScreenshotText());
    }
    _guiWindowManager.Add(EnumNGUIWindow.InGameHUD.ToStringCached<EnumNGUIWindow>(), (GUIWindow) new GUIWindowNGUI(EnumNGUIWindow.InGameHUD));
    _guiWindowManager.Add(GUIWindowEditBlockSpawnEntity.ID, (GUIWindow) new GUIWindowEditBlockSpawnEntity(this));
    _guiWindowManager.Add(GUIWindowEditBlockValue.ID, (GUIWindow) new GUIWindowEditBlockValue(this));
    GUIWindowDynamicPrefabMenu dynamicPrefabMenu = new GUIWindowDynamicPrefabMenu(this);
    _guiWindowManager.Add(GUIWindowWOChooseCategory.ID, (GUIWindow) new GUIWindowWOChooseCategory());
    _guiWindowManager.CloseAllOpenWindows();
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public IEnumerator loadStaticData()
  {
    GameManager gameManager = this;
    gameManager.CurrentLoadAction = Localization.Get("loadActionCharacterModels");
    yield return (object) null;
    gameManager.CurrentLoadAction = Localization.Get("loadActionTerrainTextures");
    yield return (object) null;
    yield return (object) null;
    yield return (object) WorldStaticData.Init(false, GameManager.IsDedicatedServer, new WorldStaticData.ProgressDelegate(gameManager.\u003CloadStaticData\u003Eb__129_0));
    gameManager.CurrentLoadAction = Localization.Get("loadActionDone");
    gameManager.bStaticDataLoaded = true;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public bool ResetGame()
  {
    Log.Out("Resetting Game");
    return GameOptionsManager.ResetGameOptions(GameOptionsManager.ResetType.All) && GameOptionsManager.ResetGameOptions(GameOptionsManager.ResetType.Graphics);
  }

  public bool IsStartingGame
  {
    get => this.\u003CIsStartingGame\u003Ek__BackingField;
    [PublicizedFrom(EAccessModifier.Private)] set
    {
      this.\u003CIsStartingGame\u003Ek__BackingField = value;
    }
  }

  public void StartGame(bool _offline)
  {
    Time.timeScale = 1f;
    GamePrefs.Set(EnumGamePrefs.GameGuidClient, "");
    if (Object.op_Inequality((Object) GameSparksManager.Instance(), (Object) null))
      GameSparksManager.Instance().PrepareNewSession();
    this.StartCoroutine(this.startGameCo(_offline));
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public IEnumerator startGameCo(bool _offline)
  {
    this.IsStartingGame = true;
    PlatformApplicationManager.SetRestartRequired();
    Log.Out("StartGame");
    ModEvents.SGameStartingData _data = new ModEvents.SGameStartingData(SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer);
    ModEvents.GameStarting.Invoke(ref _data);
    this.allowQuit = false;
    this.backgroundColor = Color.white;
    EntityStats.WeatherSurvivalEnabled = true;
    yield return (object) null;
    yield return (object) ModManager.LoadPatchStuff(true);
    yield return (object) null;
    SaveInfoProvider.Instance.ClearResources();
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer || SingletonMonoBehaviour<ConnectionManager>.Instance.IsConnected)
    {
      if (!GameManager.IsDedicatedServer)
      {
        XUiC_MainMenu.CloseGlobalMenuWindows(this.windowManager.playerUI.xui);
        this.windowManager.CloseAllOpenWindows();
        XUiFromXml.ClearData();
        LocalPlayerUI.QueueUIForNewPlayerEntity(LocalPlayerUI.CreateUIForNewLocalPlayer());
        this.windowManager.Open(XUiC_LoadingScreen.ID, false, true);
        if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
        {
          if (SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo.EACEnabled)
            this.windowManager.Open("eacWarning", false);
          if (SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo.AllowsCrossplay)
            this.windowManager.Open("crossplayWarning", false);
        }
        XUiC_ProgressWindow.Open(LocalPlayerUI.primaryUI, Localization.Get("uiLoadStartingGame"));
      }
      yield return (object) null;
      if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
        GamePrefs.Set(EnumGamePrefs.GameWorld, string.Empty);
      this.isEditMode = GameModeEditWorld.TypeName.Equals(GamePrefs.GetString(EnumGamePrefs.GameMode));
      GamePrefs.Set(EnumGamePrefs.DebugStopEnemiesMoving, this.IsEditMode());
      GamePrefs.Set(EnumGamePrefs.DebugMenuEnabled, this.isEditMode || GameUtils.IsPlaytesting());
      GamePrefs.Set(EnumGamePrefs.CreativeMenuEnabled, this.isEditMode || GameUtils.IsPlaytesting());
      GamePrefs.Instance.Save();
      if (!Application.isEditor)
      {
        GameUtils.DebugOutputGamePrefs((GameUtils.OutputDelegate) ([PublicizedFrom(EAccessModifier.Internal)] (_text) => Console.WriteLine("GamePref." + _text)));
        GameUtils.DebugOutputGameStats((GameUtils.OutputDelegate) ([PublicizedFrom(EAccessModifier.Internal)] (_text) => Console.WriteLine("GameStat." + _text)));
      }
      yield return (object) null;
      CraftingManager.InitForNewGame();
      yield return (object) null;
      GameManager.bSavingActive = true;
      GameManager.bPhysicsActive = !this.IsEditMode();
      GameManager.bTickingActive = !this.IsEditMode();
      GameManager.bShowDecorBlocks = true;
      GameManager.bShowLootBlocks = true;
      GameManager.bShowPaintables = true;
      GameManager.bShowUnpaintables = true;
      GameManager.bShowTerrain = true;
      GameManager.bVolumeBlocksEditing = true;
      Block.nameIdMapping = (NameIdMapping) null;
      ItemClass.nameIdMapping = (NameIdMapping) null;
      if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
      {
        yield return (object) this.StartAsServer(_offline);
      }
      else
      {
        if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsConnected)
          yield break;
        XUiC_ProgressWindow.Open(LocalPlayerUI.primaryUI, Localization.Get("uiLoadWaitingForServer"));
        this.StartAsClient();
      }
      DismembermentManager.Init();
      yield return (object) null;
      if (Object.op_Inequality((Object) GameSparksManager.Instance(), (Object) null))
        GameSparksManager.Instance().SessionStarted(GamePrefs.GetString(EnumGamePrefs.GameWorld), GamePrefs.GetString(EnumGamePrefs.GameMode), SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer);
      if (!GameManager.IsDedicatedServer && SingletonMonoBehaviour<ConnectionManager>.Instance.CurrentMode != ProtocolManager.NetworkType.OfflineServer)
        PlatformManager.MultiPlatform.User.StartAdvertisePlaying(SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo : SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo);
      Log.Out("Loading dymesh settings");
      DynamicMeshManager.CONTENT_ENABLED = GamePrefs.GetBool(EnumGamePrefs.DynamicMeshEnabled);
      DynamicMeshSettings.OnlyPlayerAreas = GamePrefs.GetBool(EnumGamePrefs.DynamicMeshLandClaimOnly);
      DynamicMeshSettings.UseImposterValues = GamePrefs.GetBool(EnumGamePrefs.DynamicMeshUseImposters);
      DynamicMeshSettings.MaxViewDistance = GamePrefs.GetInt(EnumGamePrefs.DynamicMeshDistance);
      DynamicMeshSettings.PlayerAreaChunkBuffer = GamePrefs.GetInt(EnumGamePrefs.DynamicMeshLandClaimBuffer);
      DynamicMeshSettings.MaxRegionMeshData = GamePrefs.GetInt(EnumGamePrefs.DynamicMeshMaxRegionCache);
      DynamicMeshSettings.MaxDyMeshData = GamePrefs.GetInt(EnumGamePrefs.DynamicMeshMaxItemCache);
      DynamicMeshSettings.LogSettings();
      if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
        DynamicMeshManager.Init();
      ModEvents.SGameStartDoneData eventData;
      ModEvents.GameStartDone.Invoke(ref eventData);
      if (GameManager.IsDedicatedServer)
        this.waitForTargetFPS.TargetFPS = 20;
      Log.Out("StartGame done");
      this.IsStartingGame = false;
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void updateTimeOfDay()
  {
    if (!GameManager.IsDedicatedServer || this.m_World.Players.list.Count > 0)
    {
      int num1 = GameStats.GetInt(EnumGameStats.TimeOfDayIncPerSec);
      if (num1 == 0)
      {
        this.msPassedSinceLastUpdate += (int) ((double) Time.deltaTime * 1000.0);
        if (this.msPassedSinceLastUpdate < 100)
          return;
        this.m_World.SetTime(this.m_World.worldTime);
        this.msPassedSinceLastUpdate = 0;
        return;
      }
      float v1 = 1000f / (float) num1;
      this.msPassedSinceLastUpdate += (int) ((double) Time.deltaTime * 1000.0);
      if ((double) this.msPassedSinceLastUpdate <= (double) Utils.FastMax(v1, 50f))
        return;
      int num2 = (int) ((double) this.msPassedSinceLastUpdate / (double) v1);
      this.msPassedSinceLastUpdate -= (int) v1 * num2;
      this.m_World.SetTime(this.m_World.worldTime + (ulong) num2);
    }
    PlatformManager.NativePlatform.LobbyHost?.UpdateGameTimePlayers(this.m_World.worldTime, this.m_World.Players.list.Count);
    GameSenseManager.Instance?.UpdateEventTime(this.m_World.worldTime);
    if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer || (double) Time.time - (double) this.lastTimeWorldTickTimeSentToClients <= (double) Constants.cSendWorldTickTimeToClients)
      return;
    SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo.UpdateGameTimePlayers(this.m_World.worldTime, this.m_World.Players.list.Count);
    this.lastTimeWorldTickTimeSentToClients = Time.time;
    SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageWorldTime>().Setup(this.m_World.worldTime), true);
    if (!Object.op_Inequality((Object) WeatherManager.Instance, (Object) null))
      return;
    WeatherManager.Instance.SendPackages();
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void updateSendClientPlayerPositionToServer()
  {
    EntityPlayerLocal primaryPlayer = this.m_World.GetPrimaryPlayer();
    EntityAlive _entity = (EntityAlive) primaryPlayer;
    if (Object.op_Inequality((Object) _entity, (Object) null))
    {
      if (Object.op_Inequality((Object) _entity.AttachedToEntity, (Object) null))
      {
        _entity = _entity.AttachedToEntity as EntityAlive;
        this.bLastWasAttached = true;
        if (_entity.isEntityRemote)
          return;
      }
      else
      {
        if (this.bLastWasAttached)
          this.lastTimeAbsPosSentToServer = int.MaxValue;
        this.bLastWasAttached = false;
      }
    }
    if (Object.op_Equality((Object) _entity, (Object) null))
      return;
    if (primaryPlayer.bPlayerStatsChanged)
    {
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackagePlayerStats>().Setup((EntityAlive) primaryPlayer));
      primaryPlayer.bPlayerStatsChanged = false;
    }
    if (primaryPlayer.bPlayerTwitchChanged)
    {
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackagePlayerTwitchStats>().Setup((EntityAlive) primaryPlayer));
      primaryPlayer.bPlayerTwitchChanged = false;
    }
    if (primaryPlayer.bEntityAliveFlagsChanged)
    {
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageEntityAliveFlags>().Setup((EntityAlive) primaryPlayer));
      primaryPlayer.bEntityAliveFlagsChanged = false;
    }
    Vector3i vector3i1 = NetEntityDistributionEntry.EncodePos(_entity.position);
    Vector3i _deltaPos = vector3i1 - _entity.serverPos;
    int num1 = (double) Utils.FastAbs((float) _deltaPos.x) >= 2.0 || (double) Utils.FastAbs((float) _deltaPos.y) >= 2.0 || (double) Utils.FastAbs((float) _deltaPos.z) >= 2.0 ? 1 : (_entity.emodel.IsRagdollActive ? 1 : 0);
    Vector3i _absRot = NetEntityDistributionEntry.EncodeRot(_entity.rotation);
    Vector3i vector3i2 = _absRot - _entity.serverRot;
    int num2 = (double) Utils.FastAbs((float) vector3i2.x) >= 1.0 || (double) Utils.FastAbs((float) vector3i2.y) >= 1.0 || (double) Utils.FastAbs((float) vector3i2.z) >= 1.0 ? (true ? 1 : 0) : (_entity.emodel.IsRagdollActive ? 1 : 0);
    if ((num1 | num2) != 0)
    {
      if (_deltaPos.x < -256 || _deltaPos.x >= 256 /*0x0100*/ || _deltaPos.y < -256 || _deltaPos.y >= 256 /*0x0100*/ || _deltaPos.z < -256 || _deltaPos.z >= 256 /*0x0100*/)
      {
        this.lastTimeAbsPosSentToServer = 0;
        SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageEntityTeleport>().Setup((Entity) _entity));
      }
      else if (_deltaPos.x < (int) sbyte.MinValue || _deltaPos.x >= 128 /*0x80*/ || _deltaPos.y < (int) sbyte.MinValue || _deltaPos.y >= 128 /*0x80*/ || _deltaPos.z < (int) sbyte.MinValue || _deltaPos.z >= 128 /*0x80*/ || this.lastTimeAbsPosSentToServer > 100)
      {
        this.lastTimeAbsPosSentToServer = 0;
        SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageEntityPosAndRot>().Setup((Entity) _entity));
      }
      else
        SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageEntityRelPosAndRot>().Setup(_entity.entityId, _deltaPos, _absRot, _entity.qrotation, _entity.onGround, _entity.IsQRotationUsed(), 3));
      _entity.serverPos = vector3i1;
      _entity.serverRot = _absRot;
      ++this.lastTimeAbsPosSentToServer;
    }
    if (Object.op_Inequality((Object) _entity, (Object) primaryPlayer))
    {
      if (_entity.bPlayerStatsChanged)
      {
        SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackagePlayerStats>().Setup(_entity));
        _entity.bPlayerStatsChanged = false;
      }
      if (_entity.bPlayerTwitchChanged)
      {
        SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackagePlayerTwitchStats>().Setup(_entity));
        _entity.bPlayerTwitchChanged = false;
      }
      if (_entity.bEntityAliveFlagsChanged)
      {
        SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageEntityAliveFlags>().Setup(_entity));
        _entity.bEntityAliveFlagsChanged = false;
      }
    }
    LocalPlayerUI uiForPlayer = LocalPlayerUI.GetUIForPlayer(primaryPlayer);
    if (this.countdownSendPlayerDataFileToServer.HasPassed() && Object.op_Inequality((Object) uiForPlayer.xui, (Object) null) && uiForPlayer.xui.isReady)
    {
      this.countdownSendPlayerDataFileToServer.ResetAndRestart();
      this.doSendLocalPlayerData(primaryPlayer);
    }
    if (this.countdownSendPlayerInventoryToServer.HasPassed())
    {
      this.countdownSendPlayerInventoryToServer.Reset();
      this.doSendLocalInventory(primaryPlayer);
    }
    if (primaryPlayer.persistentPlayerData == null || !primaryPlayer.persistentPlayerData.questPositionsChanged)
      return;
    SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackagePlayerQuestPositions>().Setup(primaryPlayer.entityId, primaryPlayer.persistentPlayerData));
    primaryPlayer.persistentPlayerData.questPositionsChanged = false;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void FixedUpdate() => ++GameManager.fixedUpdateCount;

  [PublicizedFrom(EAccessModifier.Protected)]
  public void Update() => this.gmUpdate();

  [PublicizedFrom(EAccessModifier.Private)]
  public void gmUpdate()
  {
    GameManager.frameCount = Time.frameCount;
    GameManager.frameTime = Time.time;
    GameManager.fixedUpdateCount = 0;
    GameOptionsManager.CheckResolution();
    ModEvents.SUnityUpdateData _data1;
    ModEvents.UnityUpdate.Invoke(ref _data1);
    this.handleGlobalActions();
    if (!GameManager.ReportUnusedAssets())
      return;
    if ((double) Time.timeScale <= 0.001)
      Physics.SyncTransforms();
    LoadManager.Update();
    PlatformManager.Update();
    InviteManager.Instance.Update();
    this.swUpdateTime.ResetAndRestart();
    this.fps.Update();
    BlockLiquidv2.UpdateTime();
    if (QuestEventManager.Current != null)
      QuestEventManager.Current.Update();
    if (this.m_World != null)
      this.m_World.triggerManager.Update();
    if (TwitchVoteScheduler.Current != null)
      TwitchVoteScheduler.Current.Update(Time.deltaTime);
    if (TwitchManager.Current != null)
      TwitchManager.Current.Update(Time.unscaledDeltaTime);
    if (GameEventManager.Current != null)
      GameEventManager.Current.Update(Time.deltaTime);
    if (PowerManager.HasInstance)
      PowerManager.Instance.Update();
    if (PartyManager.HasInstance)
      PartyManager.Current.Update();
    if (VehicleManager.Instance != null)
      VehicleManager.Instance.Update();
    if (DroneManager.Instance != null)
      DroneManager.Instance.Update();
    if (DismembermentManager.Instance != null)
      DismembermentManager.Instance.Update();
    if (TurretTracker.Instance != null)
      TurretTracker.Instance.Update();
    if ((bool) RaycastPathManager.Instance)
      RaycastPathManager.Instance.Update();
    if (EntityCoverManager.Instance != null)
      EntityCoverManager.Instance.Update();
    if (FactionManager.Instance != null)
      FactionManager.Instance.Update();
    if (NavObjectManager.HasInstance)
      NavObjectManager.Instance.Update();
    if (BlockedPlayerList.Instance != null)
      BlockedPlayerList.Instance.Update();
    if (this.prefabEditModeManager != null)
      this.prefabEditModeManager.Update();
    this.triggerEffectManager?.Update();
    SpeedTreeWindHistoryBufferManager.Instance.Update();
    ThreadManager.UpdateMainThreadTasks();
    if (!GameManager.IsDedicatedServer)
    {
      string _message;
      if (XUiC_MainMenu.openedOnce && !this.isQuitting && PlatformManager.CrossplatformPlatform?.AntiCheatClient?.GetUnhandledViolationMessage(out _message).GetValueOrDefault())
      {
        GUIWindowManager windowManager = LocalPlayerUI.primaryUI.windowManager;
        if (Object.op_Inequality((Object) windowManager, (Object) null))
        {
          string _title = "EAC: " + Localization.Get("eacIntegrityViolation");
          string _text = (string.IsNullOrEmpty(_message) ? "" : _message + "\n") + Localization.Get("eacUnableToPlayOnProtected");
          ((XUiC_MessageBoxWindowGroup) ((XUiWindowGroup) windowManager.GetWindow(XUiC_MessageBoxWindowGroup.ID)).Controller).ShowMessage(_title, _text, _openMainMenuOnClose: !this.gameStateManager.IsGameStarted());
        }
      }
      if (!this.bCursorVisibleOverride && !this.isQuitting)
      {
        bool _e = this.isAnyCursorWindowOpen();
        if (this.GameIsFocused && this.bCursorVisible != _e)
          this.setCursorEnabled(_e);
        if (!_e && Cursor.visible && PlatformManager.NativePlatform.Input.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard)
          this.setCursorEnabled(false);
      }
    }
    lock (((ICollection) this.tileEntitiesMusicToRemove).SyncRoot)
    {
      for (int index = 0; index < this.tileEntitiesMusicToRemove.Count; ++index)
        Object.Destroy((Object) this.tileEntitiesMusicToRemove[index]);
    }
    if (!this.gameStateManager.IsGameStarted())
    {
      GameTimer.Instance.Reset(GameTimer.Instance.ticks);
    }
    else
    {
      GameTimer.Instance.updateTimer(GameManager.IsDedicatedServer && this.m_World.Players.Count == 0);
      this.updateBlockParticles();
      this.updateTimeOfDay();
      Manager.FrameUpdate();
      WaterSimulationNative.Instance.Update();
      WaterEvaporationManager.UpdateEvaporation();
      if (GameTimer.Instance.elapsedTicks > 0 || this.m_World.m_ChunkManager.IsForceUpdate() || this.m_World.Players.list.Count == 0)
        this.m_World.m_ChunkManager.DetermineChunksToLoad();
      if (GameManager.IsDedicatedServer && this.m_World.Players.list.Count == 0 && this.lastPlayerCount > 0)
        this.timeToClearAllPools = 8f;
      this.lastPlayerCount = this.m_World.Players.list.Count;
      if (this.m_World.Players.list.Count == 0 && (double) this.timeToClearAllPools > 0.0 && (double) (this.timeToClearAllPools -= Time.deltaTime) <= 0.0)
      {
        Log.Out("Clearing all pools");
        MemoryPools.Cleanup();
        this.m_World.ClearCaches();
      }
      if (!this.UpdateTick())
        return;
      this.m_World.m_ChunkManager.GroundAlignFrameUpdate();
      int num = GameManager.IsDedicatedServer ? 25000 : 2500;
      this.swCopyChunks.ResetAndRestart();
      do
        ;
      while (this.m_World.m_ChunkManager.CopyChunksToUnity() && this.swCopyChunks.ElapsedMicroseconds < (long) num);
      if (this.prefabLODManager != null)
        this.prefabLODManager.FrameUpdate();
      this.ExplodeGroupFrameUpdate();
      this.fpsCountdownTimer -= Time.deltaTime;
      if ((double) this.fpsCountdownTimer <= 0.0)
      {
        this.fpsCountdownTimer = 30f;
        GameManager.MaxMemoryConsumption = Math.Max(GC.GetTotalMemory(false), GameManager.MaxMemoryConsumption);
        if (!GameManager.IsDedicatedServer || SingletonMonoBehaviour<ConnectionManager>.Instance.ClientCount() > 0 || this.lastStatsPlayerCount > 0)
        {
          this.lastStatsPlayerCount = SingletonMonoBehaviour<ConnectionManager>.Instance.ClientCount();
          Log.Out(ConsoleCmdMem.GetStats(false, this));
        }
      }
      if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
      {
        this.wsCountdownTimer -= Time.deltaTime;
        if ((double) this.wsCountdownTimer <= 0.0)
        {
          this.wsCountdownTimer = 30f;
          if (!this.isEditMode)
          {
            this.m_World.SaveWorldState();
            if (Block.nameIdMapping != null)
              Block.nameIdMapping.SaveIfDirty();
            if (ItemClass.nameIdMapping != null)
              ItemClass.nameIdMapping.SaveIfDirty();
          }
        }
        this.playerPositionsCountdownTimer -= Time.deltaTime;
        if ((double) this.playerPositionsCountdownTimer <= 0.0)
        {
          this.playerPositionsCountdownTimer = 6f;
          if (SingletonMonoBehaviour<ConnectionManager>.Instance.ClientCount() > 0)
            SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackagePersistentPlayerPositions>().Setup(this.persistentPlayers), _onlyClientsNotAttachedToAnEntity: true);
        }
      }
      if (GameManager.IsDedicatedServer)
      {
        this.gcCountdownTimer -= Time.deltaTime;
        if ((double) this.gcCountdownTimer <= 0.0)
        {
          this.gcCountdownTimer = 120f;
          GC.Collect();
        }
        if ((double) ((Stopwatch) this.swUpdateTime).ElapsedMilliseconds > 50.0)
          this.waitForTargetFPS.SkipSleepThisFrame = true;
      }
      else
      {
        GameSenseManager.Instance?.Update();
        if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && this.countdownSaveLocalPlayerDataFile.HasPassed())
        {
          this.countdownSaveLocalPlayerDataFile.ResetAndRestart();
          this.SaveLocalPlayerData();
        }
        this.unloadAssetsDuration += Time.deltaTime;
        if ((double) this.unloadAssetsDuration > 1200.0)
        {
          bool flag = (double) this.unloadAssetsDuration > 3600.0;
          if (!this.isAnyModalWindowOpen())
            this.isUnloadAssetsReady = true;
          else if (this.isUnloadAssetsReady)
            flag = true;
          if (flag)
          {
            this.stopwatchUnloadAssets.ResetAndRestart();
            Resources.UnloadUnusedAssets();
            ((Stopwatch) this.stopwatchUnloadAssets).Stop();
            Log.Out("UnloadUnusedAssets after {0} m, took {1} ms", new object[2]
            {
              (object) (float) ((double) this.unloadAssetsDuration / 60.0),
              (object) ((Stopwatch) this.stopwatchUnloadAssets).ElapsedMilliseconds
            });
            this.unloadAssetsDuration = 0.0f;
            this.isUnloadAssetsReady = false;
          }
        }
      }
      if (this.stabilityViewer != null)
        this.stabilityViewer.Update();
      ModEvents.SGameUpdateData _data2;
      ModEvents.GameUpdate.Invoke(ref _data2);
      GameObjectPool.Instance.FrameUpdate();
    }
  }

  public void LateUpdate()
  {
    ThreadManager.LateUpdate();
    PlatformManager.LateUpdate();
    if (this.m_World != null && this.m_World.aiDirector != null)
      this.m_World.aiDirector.DebugFrameLateUpdate();
    this.UpdateMultiplayerServices();
    MeshDataManager.Instance.LateUpdate();
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public bool UpdateTick()
  {
    GameTimer instance = GameTimer.Instance;
    if (instance.elapsedTicks <= 0 && this.m_World.Players.list.Count != 0)
    {
      this.m_World.TickEntitiesSlice();
      return true;
    }
    this.m_World.TickEntitiesFlush();
    float _partialTicks = (float) (((double) Time.time - (double) this.lastTime) * 20.0);
    this.lastTime = Time.time;
    this.m_World.OnUpdateTick(_partialTicks, this.m_World.m_ChunkManager.GetActiveChunkSet());
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && !this.gameStateManager.OnUpdateTick())
      return false;
    this.m_World.TickEntities(_partialTicks);
    this.m_World.LetBlocksFall();
    if (!GameManager.IsDedicatedServer)
      this.m_World.SetEntitiesVisibleNearToLocalPlayer();
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
    {
      this.m_World.entityDistributer.OnUpdateEntities();
      this.m_World.m_ChunkManager.SendChunksToClients();
      if (GameManager.bSavingActive)
      {
        ChunkCluster chunkCache = this.m_World.ChunkCache;
        if (chunkCache?.ChunkProvider is ChunkProviderGenerateWorld chunkProvider)
          chunkProvider.MainThreadCacheProtectedPositions();
        if (instance.ticks % 40UL == 0UL)
          chunkCache?.ChunkProvider.SaveRandomChunks(2, instance.ticks, this.m_World.m_ChunkManager.GetActiveChunkSet());
        else if ((double) Time.time - (double) this.lastTimeDecoSaved > 60.0)
        {
          this.lastTimeDecoSaved = Time.time;
          this.m_World.SaveDecorations();
        }
      }
    }
    else
      this.updateSendClientPlayerPositionToServer();
    if ((double) this.lastTime - (double) this.activityCheck >= 1.0)
    {
      PlatformManager.MultiPlatform.RichPresence.UpdateRichPresence(IRichPresence.PresenceStates.InGame);
      this.activityCheck = this.lastTime;
    }
    return true;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void OnNetworkStateChanged(bool connectionState)
  {
    if (connectionState)
      return;
    this.ShutdownMultiplayerServices(GameManager.EMultiShutReason.AppNoNetwork);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void OnApplicationResume()
  {
    PlatformApplicationManager.SetRestartRequired();
    if (!this.IsSafeToConnect())
      this.ShutdownMultiplayerServices(GameManager.EMultiShutReason.AppSuspended);
    else
      ThreadManager.StartCoroutine(PlatformApplicationManager.CheckRestartCoroutine());
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void UpdateMultiplayerServices()
  {
    if (GameManager.IsDedicatedServer || this.shuttingDownMultiplayerServices || this.IsSafeToConnect() || !this.IsSafeToDisconnect())
      return;
    ConnectionManager instance = SingletonMonoBehaviour<ConnectionManager>.Instance;
    if (Object.op_Equality((Object) instance, (Object) null))
      return;
    switch (instance.CurrentMode)
    {
      case ProtocolManager.NetworkType.None:
        break;
      case ProtocolManager.NetworkType.OfflineServer:
        break;
      default:
        EUserPerms permissions = PermissionsManager.GetPermissions();
        if (!permissions.HasMultiplayer())
        {
          this.ShutdownMultiplayerServices(GameManager.EMultiShutReason.PermMissingMultiplayer);
          break;
        }
        if (!(SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient ? SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo : SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo).AllowsCrossplay || permissions.HasCrossplay())
          break;
        this.ShutdownMultiplayerServices(GameManager.EMultiShutReason.PermMissingCrossplay);
        break;
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static string GetLocalizationKey(GameManager.EMultiShutReason _reason)
  {
    switch (_reason)
    {
      case GameManager.EMultiShutReason.AppNoNetwork:
        return "app_noNetwork";
      case GameManager.EMultiShutReason.AppSuspended:
        return "app_suspended";
      case GameManager.EMultiShutReason.PermMissingMultiplayer:
        return "permMissing_multiplayer";
      case GameManager.EMultiShutReason.PermMissingCrossplay:
        return "permMissing_crossplay";
      default:
        throw new ArgumentOutOfRangeException(nameof (_reason), (object) _reason, $"Unknown Localization for {"EMultiShutReason"}.{_reason}");
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void ShutdownMultiplayerServices(GameManager.EMultiShutReason _reason)
  {
    ThreadManager.StartCoroutine(this.ShutdownMultiplayerServicesCoroutine(_reason));
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public IEnumerator ShutdownMultiplayerServicesCoroutine(GameManager.EMultiShutReason _reason)
  {
    yield return (object) null;
    if (!GameManager.IsDedicatedServer && !this.shuttingDownMultiplayerServices)
    {
      this.shuttingDownMultiplayerServices = true;
      bool isClient = false;
      bool success = false;
      bool failReasonProvided = false;
      bool flag;
      try
      {
        Log.Out($"Waiting to Shut Down Multiplayer Services ({_reason})...");
        while (this.shuttingDownMultiplayerServices && !this.IsSafeToConnect() && !this.IsSafeToDisconnect())
          yield return (object) null;
        ConnectionManager connectionManager = SingletonMonoBehaviour<ConnectionManager>.Instance;
        do
        {
          yield return (object) null;
          if (!this.shuttingDownMultiplayerServices)
          {
            Log.Warning($"Cancelled Shutting Down Multiplayer Services ({_reason}) because already shutting down.");
            failReasonProvided = true;
            flag = false;
            goto label_27;
          }
          if (this.IsSafeToConnect())
          {
            Log.Warning($"Cancelled Shutting Down Multiplayer Services ({_reason}) because safe to connect.");
            failReasonProvided = true;
            flag = false;
            goto label_27;
          }
          if (!Object.op_Equality((Object) connectionManager, (Object) null))
          {
            switch (connectionManager.CurrentMode)
            {
              case ProtocolManager.NetworkType.None:
              case ProtocolManager.NetworkType.OfflineServer:
                break;
              default:
                continue;
            }
          }
          Log.Warning($"Cancelled Shutting Down Multiplayer Services ({_reason}) because no online connection.");
          failReasonProvided = true;
          flag = false;
          goto label_27;
        }
        while (!this.IsSafeToDisconnect());
        Log.Out($"Shutting Down Multiplayer Services ({_reason})...");
        if (connectionManager.IsClient)
        {
          this.Disconnect();
          isClient = true;
          success = true;
          flag = false;
        }
        else
        {
          ClientInfo[] clientInfos = SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.List.ToArray<ClientInfo>();
          if (clientInfos.Length != 0)
          {
            NetPackagePlayerDenied _package = NetPackageManager.GetPackage<NetPackagePlayerDenied>().Setup(new GameUtils.KickPlayerData(GameUtils.EKickReason.SessionClosed));
            foreach (ClientInfo clientInfo in clientInfos)
              clientInfo.SendPackage((NetPackage) _package);
            yield return (object) new WaitForSecondsRealtime(1f);
            foreach (ClientInfo _cInfo in clientInfos)
            {
              try
              {
                SingletonMonoBehaviour<ConnectionManager>.Instance.DisconnectClient(_cInfo);
              }
              catch (Exception ex)
              {
                Log.Warning($"Failed to disconnect client '{_cInfo.playerName}' : {ex}");
              }
            }
          }
          this.ShutdownMultiplayerServicesNow();
          connectionManager.MakeServerOffline();
          GamePrefs.Set(EnumGamePrefs.ServerMaxPlayerCount, 1);
          success = true;
          connectionManager = (ConnectionManager) null;
          clientInfos = (ClientInfo[]) null;
          yield break;
        }
label_27:;
      }
      finally
      {
        this.shuttingDownMultiplayerServices = false;
        if (success)
        {
          Log.Out($"Multiplayer Services ({_reason}) have been shut down.");
          XUiWindowGroup window = (XUiWindowGroup) this.windowManager.GetWindow(XUiC_MessageBoxWindowGroup.ID);
          string _title = Localization.Get(isClient ? "multiShut_titleClient" : "multiShut_titleHost");
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.AppendFormat(Localization.Get("auth_reason"), (object) Localization.Get(GameManager.GetLocalizationKey(_reason)));
          if (!isClient)
          {
            stringBuilder.Append('\n');
            stringBuilder.Append(Localization.Get("multiShut_commonHost"));
          }
          ((XUiC_MessageBoxWindowGroup) window.Controller).ShowMessage(_title, stringBuilder.ToString());
        }
        else if (!failReasonProvided)
          Log.Warning($"Failed Shutting Down Multiplayer Services ({_reason}).");
      }
      return flag;
    }
  }

  public void CreateStabilityViewer()
  {
    if (this.stabilityViewer != null)
      return;
    this.stabilityViewer = new StabilityViewer();
  }

  public void ClearStabilityViewer()
  {
    if (this.stabilityViewer == null)
      return;
    this.stabilityViewer.worldIsReady = false;
    this.stabilityViewer.Clear();
    this.stabilityViewer = (StabilityViewer) null;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void setLocalPlayerEntity(EntityPlayerLocal _playerEntity)
  {
    _playerEntity.IsFlyMode.Value = this.IsEditMode();
    _playerEntity.SetEntityName(GamePrefs.GetString(EnumGamePrefs.PlayerName));
    this.myPlayerId = _playerEntity.entityId;
    this.myEntityPlayerLocal = _playerEntity;
    this.persistentLocalPlayer = this.getPersistentPlayerData((ClientInfo) null);
    _playerEntity.persistentPlayerData = this.persistentLocalPlayer;
    _playerEntity.InventoryChangedEvent += new Action(this.LocalPlayerInventoryChanged);
    _playerEntity.inventory.OnToolbeltItemsChangedInternal += (XUiEvent_ToolbeltItemsChangedInternal) ([PublicizedFrom(EAccessModifier.Private)] () => this.sendPlayerToolbelt = true);
    _playerEntity.bag.OnBackpackItemsChangedInternal += (XUiEvent_BackpackItemsChangedInternal) ([PublicizedFrom(EAccessModifier.Private)] () => this.sendPlayerBag = true);
    _playerEntity.equipment.OnChanged += (Action) ([PublicizedFrom(EAccessModifier.Private)] () => this.sendPlayerEquipment = true);
    _playerEntity.DragAndDropItemChanged += (Action) ([PublicizedFrom(EAccessModifier.Private)] () => this.sendDragAndDropItem = true);
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && this.persistentPlayers != null)
    {
      if (this.persistentLocalPlayer == null)
      {
        this.persistentLocalPlayer = this.persistentPlayers.CreatePlayerData(this.getPersistentPlayerID((ClientInfo) null), PlatformManager.NativePlatform.User.PlatformUserId, _playerEntity.EntityName, DeviceFlags.Current.ToPlayGroup());
        this.persistentLocalPlayer.EntityId = this.myPlayerId;
        this.persistentPlayers.MapPlayer(this.persistentLocalPlayer);
        SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackagePersistentPlayerState>().Setup(this.persistentLocalPlayer, EnumPersistentPlayerDataReason.New), true);
        this.SavePersistentPlayerData();
      }
      else
      {
        this.persistentLocalPlayer.Update(PlatformManager.NativePlatform.User.PlatformUserId, new AuthoredText(_playerEntity.EntityName, this.persistentLocalPlayer.PrimaryId), DeviceFlags.Current.ToPlayGroup());
        this.persistentLocalPlayer.EntityId = this.myPlayerId;
        this.persistentPlayers.MapPlayer(this.persistentLocalPlayer);
        SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackagePersistentPlayerState>().Setup(this.persistentLocalPlayer, EnumPersistentPlayerDataReason.Login), true);
      }
    }
    this.m_World.SetLocalPlayer(_playerEntity);
    LocalPlayerUI.DispatchNewPlayerForUI(_playerEntity);
    this.MarkPlayerEntityFriends();
    if (this.OnLocalPlayerChanged != null)
      this.OnLocalPlayerChanged(_playerEntity);
    GameSenseManager.Instance?.SessionStarted(_playerEntity);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public IEnumerator StartAsServer(bool _offline)
  {
    while (XUiC_WorldGenerationWindowGroup.IsGenerating())
      yield return (object) null;
    Log.Out(nameof (StartAsServer));
    GameServerInfo.PrepareLocalServerInfo();
    this.CalculatePersistentPlayerCount(GamePrefs.GetString(EnumGamePrefs.GameWorld), GamePrefs.GetString(EnumGamePrefs.GameName));
    PlatformManager.MultiPlatform.RichPresence.UpdateRichPresence(IRichPresence.PresenceStates.Loading);
    XUiC_ProgressWindow.Open(LocalPlayerUI.primaryUI, Localization.Get("uiLoadLoadingXml"));
    yield return (object) null;
    WorldStaticData.Cleanup((string) null);
    Block.nameIdMapping = (NameIdMapping) null;
    ItemClass.nameIdMapping = (NameIdMapping) null;
    string str = GamePrefs.GetString(EnumGamePrefs.GameWorld);
    if (!str.Equals("Empty") && !str.Equals("Playtesting"))
    {
      string path = GameIO.GetSaveGameDir() + "/main.ttw";
      string _filename1 = $"{GameIO.GetSaveGameDir()}/{Constants.cFileBlockMappings}";
      string _filename2 = $"{GameIO.GetSaveGameDir()}/{Constants.cFileItemMappings}";
      if (!SdFile.Exists(path))
      {
        if (!SdDirectory.Exists(GameIO.GetSaveGameDir()))
          SdDirectory.CreateDirectory(GameIO.GetSaveGameDir());
        Block.nameIdMapping = new NameIdMapping(_filename1, Block.MAX_BLOCKS);
        Block.nameIdMapping.WriteToFile();
        ItemClass.nameIdMapping = new NameIdMapping(_filename2, ItemClass.MAX_ITEMS);
        ItemClass.nameIdMapping.WriteToFile();
      }
      else
      {
        Block.nameIdMapping = new NameIdMapping(_filename1, Block.MAX_BLOCKS);
        if (!Block.nameIdMapping.LoadFromFile())
        {
          Log.Warning($"Could not load block-name-mappings file '{_filename1}'!");
          Block.nameIdMapping = (NameIdMapping) null;
        }
        ItemClass.nameIdMapping = new NameIdMapping(_filename2, ItemClass.MAX_ITEMS);
        if (!ItemClass.nameIdMapping.LoadFromFile())
        {
          Log.Warning($"Could not load item-name-mappings file '{_filename2}'!");
          ItemClass.nameIdMapping = (NameIdMapping) null;
        }
      }
    }
    yield return (object) WorldStaticData.LoadAllXmlsCo(false, (WorldStaticData.ProgressDelegate) null);
    yield return (object) null;
    SingletonMonoBehaviour<ConnectionManager>.Instance.ServerReady();
    Manager.CreateServer();
    LightManager.CreateServer();
    this.gameStateManager.InitGame(true);
    yield return (object) null;
    PowerManager.Instance.LoadPowerManager();
    XUiC_ProgressWindow.SetText(LocalPlayerUI.primaryUI, Localization.Get("uiLoadCreatingWorld"));
    yield return (object) null;
    if (this.isEditMode)
    {
      this.persistentPlayers = new PersistentPlayerList();
    }
    else
    {
      this.persistentPlayers = PersistentPlayerList.ReadXML(GameIO.GetSaveGameDir() + "/players.xml");
      if (this.persistentPlayers != null && this.persistentPlayers.CleanupPlayers())
        this.SavePersistentPlayerData();
    }
    yield return (object) this.createWorld(GamePrefs.GetString(EnumGamePrefs.GameWorld), GamePrefs.GetString(EnumGamePrefs.GameName), (List<WallVolume>) null);
    GameServerInfo.SetLocalServerWorldInfo();
    NetPackageWorldInfo.PrepareWorldHashes();
    this.FreeAllTileEntityLocks();
    yield return (object) null;
    XUiC_ProgressWindow.SetText(LocalPlayerUI.primaryUI, Localization.Get("uiLoadCreatingPlayer"));
    yield return (object) null;
    if (!GameManager.IsDedicatedServer)
    {
      string persistentPlayerId = this.getPersistentPlayerID((ClientInfo) null).CombinedString;
      if (!GamePrefs.GetBool(EnumGamePrefs.SkipSpawnButton) && !this.IsEditMode())
      {
        this.canSpawnPlayer = false;
        XUiC_SpawnSelectionWindow.Open(LocalPlayerUI.primaryUI, false, true, !PlayerDataFile.Exists(GameIO.GetPlayerDataDir(), persistentPlayerId));
        while (!this.canSpawnPlayer)
          yield return (object) null;
        yield return (object) new WaitForSeconds(0.1f);
      }
      PlayerDataFile playerDataFile = new PlayerDataFile();
      playerDataFile.Load(GameIO.GetPlayerDataDir(), persistentPlayerId);
      EntityCreationData _ecd = new EntityCreationData();
      Vector3 vector3_1;
      Vector3 vector3_2;
      int num;
      if (playerDataFile.bLoaded)
      {
        vector3_1 = playerDataFile.ecd.pos;
        // ISSUE: explicit constructor call
        ((Vector3) ref vector3_2).\u002Ector(playerDataFile.ecd.rot.x, playerDataFile.ecd.rot.y, 0.0f);
        if (this.isEditMode)
          playerDataFile.id = -1;
        num = playerDataFile.id != -1 ? playerDataFile.id : EntityFactory.nextEntityID++;
        _ecd.entityData = playerDataFile.ecd.entityData;
        _ecd.readFileVersion = playerDataFile.ecd.readFileVersion;
      }
      else
      {
        SpawnPosition randomSpawnPosition = this.GetSpawnPointList().GetRandomSpawnPosition(this.m_World);
        vector3_1 = randomSpawnPosition.position;
        // ISSUE: explicit constructor call
        ((Vector3) ref vector3_2).\u002Ector(0.0f, randomSpawnPosition.heading, 0.0f);
        num = EntityFactory.nextEntityID++;
      }
      if (playerDataFile.bLoaded && playerDataFile.ecd.playerProfile != null && GamePrefs.GetBool(EnumGamePrefs.PersistentPlayerProfiles))
      {
        _ecd.entityClass = EntityClass.FromString(playerDataFile.ecd.playerProfile.EntityClassName);
        _ecd.playerProfile = playerDataFile.ecd.playerProfile;
      }
      else
      {
        _ecd.playerProfile = PlayerProfile.LoadLocalProfile();
        _ecd.entityClass = EntityClass.FromString(_ecd.playerProfile.EntityClassName);
      }
      _ecd.skinTexture = GamePrefs.GetString(EnumGamePrefs.OptionsPlayerModelTexture);
      _ecd.id = num;
      _ecd.pos = vector3_1;
      _ecd.rot = vector3_2;
      _ecd.belongsPlayerId = num;
      EntityPlayerLocal entity = (EntityPlayerLocal) EntityFactory.CreateEntity(_ecd);
      this.setLocalPlayerEntity(entity);
      if (playerDataFile.bLoaded)
      {
        playerDataFile.ToPlayer((EntityPlayer) entity);
        entity.SetFirstPersonView(true, false);
      }
      this.m_World.SpawnEntityInWorld((Entity) entity);
      this.myEntityPlayerLocal.Respawn(playerDataFile.bLoaded ? RespawnType.LoadedGame : RespawnType.NewGame);
      this.myEntityPlayerLocal.ChunkObserver = this.m_World.m_ChunkManager.AddChunkObserver(this.myEntityPlayerLocal.GetPosition(), true, Utils.FastMin(12, GameUtils.GetViewDistance()), -1);
      IMapChunkDatabase.TryCreateOrLoad(this.myEntityPlayerLocal.entityId, out this.myEntityPlayerLocal.ChunkObserver.mapDatabase, (Func<IMapChunkDatabase.DirectoryPlayerId>) ([PublicizedFrom(EAccessModifier.Internal)] () => new IMapChunkDatabase.DirectoryPlayerId(GameIO.GetPlayerDataDir(), persistentPlayerId)));
      LocalPlayerUI uiForPlayer = LocalPlayerUI.GetUIForPlayer(entity);
      uiForPlayer.xui.SetDataConnections();
      uiForPlayer.xui.SetCraftingData(playerDataFile.craftingData);
    }
    Log.Out("Loaded player");
    yield return (object) null;
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
    {
      VehicleManager.Init();
      DroneManager.Init();
      TurretTracker.Init();
      RaycastPathManager.Init();
      EntityCoverManager.Init();
      BlockLimitTracker.Init();
    }
    yield return (object) null;
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && this.m_World.ChunkClusters[0].IsFixedSize && !this.IsEditMode() && Object.op_Inequality((Object) this.m_World.m_WorldEnvironment, (Object) null))
    {
      this.m_World.m_WorldEnvironment.SetColliders((float) ((this.m_World.ChunkClusters[0].ChunkMinPos.x + 1) * 16 /*0x10*/), (float) ((this.m_World.ChunkClusters[0].ChunkMinPos.y + 1) * 16 /*0x10*/), (float) ((this.m_World.ChunkClusters[0].ChunkMaxPos.x - this.m_World.ChunkClusters[0].ChunkMinPos.x - 1) * 16 /*0x10*/), (float) ((this.m_World.ChunkClusters[0].ChunkMaxPos.y - this.m_World.ChunkClusters[0].ChunkMinPos.y - 1) * 16 /*0x10*/), Constants.cSizePlanesAround, 0.0f);
      this.m_World.m_WorldEnvironment.CreateLevelBorderBox(this.m_World);
    }
    if (this.isEditMode)
    {
      this.prefabEditModeManager.Init();
      yield return (object) null;
    }
    yield return (object) null;
    if (GameManager.IsDedicatedServer || !_offline)
    {
      ServerInformationTcpProvider.Instance.StartServer();
      PlatformManager.MultiPlatform.ServerListAnnouncer?.AdvertiseServer((Action) ([PublicizedFrom(EAccessModifier.Internal)] () =>
      {
        PlatformManager.NativePlatform.LobbyHost?.UpdateLobby(SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo);
        ModEvents.SServerRegisteredData _data;
        ModEvents.ServerRegistered.Invoke(ref _data);
      }));
      PlayerInteractions.Instance.JoinedMultiplayerServer(this.persistentPlayers);
      AuthorizationManager.Instance.ServerStart();
    }
    else
      GamePrefs.Set(EnumGamePrefs.ServerMaxPlayerCount, 1);
    yield return (object) GCUtils.UnloadAndCollectCo();
    this.gameStateManager.StartGame();
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void StartAsClient()
  {
    Log.Out(nameof (StartAsClient));
    this.worldCreated = false;
    this.chunkClusterLoaded = false;
    GamePrefs.Set(EnumGamePrefs.GameMode, string.Empty);
    GamePrefs.Set(EnumGamePrefs.GameWorld, string.Empty);
    WorldStaticData.WaitForConfigsFromServer();
    PlatformManager.MultiPlatform.RichPresence.UpdateRichPresence(IRichPresence.PresenceStates.Connecting);
    IAntiCheatClient antiCheatClient = PlatformManager.MultiPlatform.AntiCheatClient;
    if ((antiCheatClient != null ? (!antiCheatClient.ClientAntiCheatEnabled() ? 1 : 0) : 1) != 0)
    {
      Log.Out("Sending RequestToEnterGame...");
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageRequestToEnterGame>());
    }
    else
      PlatformManager.MultiPlatform.AntiCheatClient.WaitForRemoteAuth((Action) ([PublicizedFrom(EAccessModifier.Internal)] () =>
      {
        Log.Out("Sending RequestToEnterGame...");
        SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageRequestToEnterGame>());
      }));
    XUiC_ProgressWindow.SetText(LocalPlayerUI.primaryUI, Localization.Get("uiLoadWaitingForConfigs"));
    BlockLimitTracker.Init();
  }

  public bool IsSafeToConnect()
  {
    return SingletonMonoBehaviour<ConnectionManager>.Instance.CurrentMode == ProtocolManager.NetworkType.None;
  }

  public bool IsSafeToDisconnect()
  {
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.CurrentMode == ProtocolManager.NetworkType.None)
      return true;
    return (!PrefabEditModeManager.Instance.IsActive() || !PrefabEditModeManager.Instance.NeedsSaving) && this.gameStateManager.IsGameStarted() && !this.IsStartingGame && !this.isDisconnectingLater;
  }

  public void Disconnect()
  {
    Log.Out(nameof (Disconnect));
    if (!GameManager.IsDedicatedServer)
    {
      this.windowManager.CloseAllOpenWindows();
      if (this.m_World != null)
      {
        List<EntityPlayerLocal> localPlayers = this.m_World.GetLocalPlayers();
        for (int index = 0; index < localPlayers.Count; ++index)
        {
          LocalPlayerUI uiForPlayer = LocalPlayerUI.GetUIForPlayer(localPlayers[index]);
          if (Object.op_Inequality((Object) null, (Object) uiForPlayer) && Object.op_Inequality((Object) null, (Object) uiForPlayer.windowManager))
          {
            uiForPlayer.windowManager.CloseAllOpenWindows();
            ((Component) uiForPlayer.xui).gameObject.SetActive(false);
          }
        }
      }
      LocalPlayerUI.primaryUI.windowManager.Close(XUiC_SubtitlesDisplay.ID);
      Manager.StopAllLocal();
    }
    this.Pause(false);
    if (!GameManager.IsDedicatedServer && !this.isEditMode && Object.op_Inequality((Object) null, (Object) this.myEntityPlayerLocal))
    {
      GameSenseManager.Instance?.SessionEnded();
      this.myEntityPlayerLocal.FireEvent(MinEventTypes.onSelfLeaveGame);
      this.myEntityPlayerLocal.dropItemOnQuit();
      if (Object.op_Inequality((Object) this.myEntityPlayerLocal.AttachedToEntity, (Object) null))
      {
        this.triggerEffectManager.StopGamepadVibration();
        this.myEntityPlayerLocal.Detach();
      }
    }
    if (!GameManager.IsDedicatedServer)
      PlatformManager.MultiPlatform.User.StopAdvertisePlaying();
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
    {
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackagePlayerDisconnect>().Setup((EntityPlayer) this.myEntityPlayerLocal), true);
      this.StartCoroutine(this.disconnectLater());
    }
    else if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
      SingletonMonoBehaviour<ConnectionManager>.Instance.StopServers();
    else
      SingletonMonoBehaviour<ConnectionManager>.Instance.DisconnectFromServer();
    if (!Object.op_Inequality((Object) GameSparksManager.Instance(), (Object) null))
      return;
    GameSparksManager.Instance().SessionEnded();
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public IEnumerator disconnectLater()
  {
    this.isDisconnectingLater = true;
    yield return (object) new WaitForSeconds(0.2f);
    SingletonMonoBehaviour<ConnectionManager>.Instance.Disconnect();
    GamePrefs.Set(EnumGamePrefs.GameGuidClient, "");
    this.isDisconnectingLater = false;
  }

  public void SaveAndCleanupWorld()
  {
    Log.Out(nameof (SaveAndCleanupWorld));
    ModEvents.SWorldShuttingDownData _data;
    ModEvents.WorldShuttingDown.Invoke(ref _data);
    this.shuttingDownMultiplayerServices = false;
    PathAbstractions.CacheEnabled = false;
    this.OnClientSpawned = (Action<ClientInfo>) null;
    PlayerInputRecordingSystem.Instance.AutoSave();
    this.gameStateManager.EndGame();
    PlatformManager.MultiPlatform.RichPresence.UpdateRichPresence(IRichPresence.PresenceStates.Menu);
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && GameManager.bSavingActive && !this.IsEditMode())
    {
      if (VehicleManager.Instance != null)
        VehicleManager.Instance.RemoveAllVehiclesFromMap();
      if (DroneManager.Instance != null)
        DroneManager.Instance.RemoveAllDronesFromMap();
      if (QuestEventManager.HasInstance)
        QuestEventManager.Current.HandleAllPlayersDisconnect();
      this.SaveLocalPlayerData();
      this.SaveWorld();
      EntityPlayerLocal primaryPlayer = this.m_World?.GetPrimaryPlayer();
      if (this.persistentPlayers != null)
      {
        foreach (KeyValuePair<PlatformUserIdentifierAbs, PersistentPlayerData> player in this.persistentPlayers.Players)
        {
          if (player.Value.EntityId != -1)
          {
            if (Object.op_Implicit((Object) primaryPlayer) && player.Value.EntityId == primaryPlayer.entityId)
              player.Value.Position = new Vector3i(primaryPlayer.position);
            player.Value.LastLogin = DateTime.Now;
          }
        }
        this.SavePersistentPlayerData();
      }
    }
    if (Block.nameIdMapping != null)
    {
      if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
        Block.nameIdMapping.SaveIfDirty();
      Block.nameIdMapping = (NameIdMapping) null;
    }
    if (ItemClass.nameIdMapping != null)
    {
      if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
        ItemClass.nameIdMapping.SaveIfDirty();
      ItemClass.nameIdMapping = (NameIdMapping) null;
    }
    if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && GameManager.bSavingActive && !this.IsEditMode())
    {
      if (this.m_World != null && Object.op_Inequality((Object) this.m_World.GetPrimaryPlayer(), (Object) null) && this.m_World.GetPrimaryPlayer().ChunkObserver.mapDatabase != null)
        ThreadManager.AddSingleTask(new ThreadManager.TaskFunctionDelegate(this.m_World.GetPrimaryPlayer().ChunkObserver.mapDatabase.SaveAsync), (object) new IMapChunkDatabase.DirectoryPlayerId(GameIO.GetPlayerDataLocalDir(), this.persistentLocalPlayer.PrimaryId.CombinedString));
      if (!GameManager.IsDedicatedServer && this.m_World != null)
      {
        foreach (EntityPlayerLocal localPlayer in this.m_World.GetLocalPlayers())
        {
          localPlayer.EnableCamera(false);
          localPlayer.SetControllable(false);
        }
      }
    }
    this.ShutdownMultiplayerServicesNow();
    PlayerInteractions.Instance.OnNewPlayerInteraction -= new PlayerIteractionEvent(this.HandleFirstSpawnInteractions);
    PlayerInteractions.Instance.Shutdown();
    PlatformManager.NativePlatform.GameplayNotifier?.GameplayEnd();
    if (!GameManager.IsDedicatedServer)
    {
      if (Object.op_Inequality((Object) this.myEntityPlayerLocal, (Object) null))
      {
        this.myEntityPlayerLocal.EnableCamera(false);
        this.myEntityPlayerLocal.SetControllable(false);
        if (this.OnLocalPlayerChanged != null)
          this.OnLocalPlayerChanged((EntityPlayerLocal) null);
        this.m_World.RemoveEntity(this.myPlayerId, EnumRemoveEntityReason.Unloaded);
        this.myPlayerId = -1;
        this.myEntityPlayerLocal = (EntityPlayerLocal) null;
      }
      foreach (LocalPlayerUI playerUi in LocalPlayerUI.PlayerUIs)
      {
        if (!playerUi.isPrimaryUI && !playerUi.IsCleanCopy)
        {
          if (Object.op_Implicit((Object) playerUi.entityPlayer))
          {
            playerUi.entityPlayer.EnableCamera(false);
            playerUi.entityPlayer.SetControllable(false);
            this.m_World?.RemoveEntity(playerUi.entityPlayer.entityId, EnumRemoveEntityReason.Unloaded);
          }
          if (Object.op_Implicit((Object) ((Component) playerUi).gameObject))
          {
            playerUi.xui.Shutdown();
            playerUi.windowManager.CloseAllOpenWindows();
            Object.Destroy((Object) ((Component) playerUi).gameObject);
          }
        }
      }
    }
    ModManager.GameEnded();
    if (!GameManager.IsDedicatedServer)
    {
      GameManager.LoadRemoteResources();
      this.windowManager.Close(GUIWindowConsole.ID);
      this.windowManager.Close(XUiC_LoadingScreen.ID);
      if (!GameManager.bHideMainMenuNextTime)
        this.windowManager.Open(XUiC_MainMenu.ID, true);
      GameManager.bHideMainMenuNextTime = false;
    }
    if (PrefabSleeperVolumeManager.Instance != null)
      PrefabSleeperVolumeManager.Instance.Cleanup();
    if (PrefabVolumeManager.Instance != null)
      PrefabVolumeManager.Instance.Cleanup();
    AstarManager.Cleanup();
    DynamicMeshManager.OnWorldUnload();
    if (GameEventManager.HasInstance)
      GameEventManager.Current.Cleanup();
    if (this.m_World != null)
    {
      if (this.OnWorldChanged != null)
        this.OnWorldChanged((World) null);
      this.prefabLODManager.Cleanup();
      if (this.prefabEditModeManager.IsActive())
        this.prefabEditModeManager.Cleanup();
      EnvironmentAudioManager.DestroyInstance();
      LightManager.Clear();
      SkyManager.Cleanup();
      WeatherManager.Cleanup();
      CharacterGazeController.Cleanup();
      WaterSplashCubes.Clear();
      WaterEvaporationManager.ClearAll();
      SleeperVolumeToolManager.CleanUp();
      this.ClearStabilityViewer();
      if (Object.op_Implicit((Object) this.m_World.GetPrimaryPlayer()) && this.m_World.GetPrimaryPlayer().DynamicMusicManager != null)
        this.m_World.GetPrimaryPlayer().DynamicMusicManager.CleanUpDynamicMembers();
      this.m_World.UnloadWorld(true);
      this.m_World.Cleanup();
      this.m_World = (World) null;
      this.GameHasStarted = false;
    }
    WaterSimulationNative.Instance.Cleanup();
    ProjectileManager.Cleanup();
    VehicleManager.Cleanup();
    DroneManager.Cleanup();
    DismembermentManager.Cleanup();
    TurretTracker.Cleanup();
    BlockLimitTracker.Cleanup();
    MapObjectManager.Reset();
    vp_TargetEventHandler.UnregisterAll();
    this.lootManager = (LootManager) null;
    this.traderManager = (TraderManager) null;
    if (QuestEventManager.HasInstance)
      QuestEventManager.Current.Cleanup();
    if (TwitchVoteScheduler.HasInstance)
      TwitchVoteScheduler.Current.Cleanup();
    if (TwitchManager.HasInstance)
      TwitchManager.Current.Cleanup();
    if (PowerManager.HasInstance)
      PowerManager.Instance.Cleanup();
    if (WireManager.HasInstance)
      WireManager.Instance.Cleanup();
    if (PartyManager.HasInstance)
      PartyManager.Current.Cleanup();
    if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && GameManager.bSavingActive)
      this.IsEditMode();
    if (UIDisplayInfoManager.HasInstance)
      UIDisplayInfoManager.Current.Cleanup();
    if (Object.op_Inequality((Object) TextureLoadingManager.Instance, (Object) null))
      TextureLoadingManager.Instance.Cleanup();
    if (NavObjectManager.HasInstance)
      NavObjectManager.Instance.Cleanup();
    SelectionBoxManager.Instance.Clear();
    Origin.Cleanup();
    GameObjectPool.Instance.Cleanup();
    MemoryPools.Cleanup();
    VoxelMeshLayer.StaticCleanup();
    GamePrefs.Instance.Save();
    GameManager.bRecordNextSession = false;
    GameManager.bPlayRecordedSession = false;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void ShutdownMultiplayerServicesNow()
  {
    if (!GameManager.IsDedicatedServer)
      PlatformManager.MultiPlatform.User.StopAdvertisePlaying();
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
      AuthorizationManager.Instance.ServerStop();
    PlatformManager.NativePlatform.LobbyHost?.ExitLobby();
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
    {
      PlatformManager.MultiPlatform.ServerListAnnouncer.StopServer();
      ServerInformationTcpProvider.Instance.StopServer();
    }
    PlatformManager.NativePlatform.GameplayNotifier?.EndOnlineMultiplayer();
  }

  public void SaveWorld()
  {
    if (this.m_World == null)
      return;
    this.m_World.Save();
  }

  public void SaveLocalPlayerData()
  {
    if (this.m_World == null)
      return;
    EntityPlayerLocal primaryPlayer = this.m_World.GetPrimaryPlayer();
    if (Object.op_Equality((Object) primaryPlayer, (Object) null) || !GameManager.bSavingActive)
      return;
    string combinedString = this.getPersistentPlayerID((ClientInfo) null).CombinedString;
    PlayerDataFile playerDataFile = new PlayerDataFile();
    playerDataFile.FromPlayer((EntityPlayer) primaryPlayer);
    playerDataFile.Save(GameIO.GetPlayerDataDir(), combinedString);
    if (primaryPlayer.ChunkObserver.mapDatabase == null)
      return;
    ThreadManager.AddSingleTask(new ThreadManager.TaskFunctionDelegate(primaryPlayer.ChunkObserver.mapDatabase.SaveAsync), (object) new IMapChunkDatabase.DirectoryPlayerId(GameIO.GetPlayerDataDir(), combinedString));
  }

  public void Cleanup()
  {
    Log.Out(nameof (Cleanup));
    WaterSimulationNative.Instance.Cleanup();
    ModEvents.SGameShutdownData _data;
    ModEvents.GameShutdown.Invoke(ref _data);
    AuthorizationManager.Instance.Cleanup();
    VehicleManager.Cleanup();
    Cursor.visible = true;
    Cursor.lockState = SoftCursor.DefaultCursorLockState;
    SingletonMonoBehaviour<SdtdConsole>.Instance.Cleanup();
    WorldStaticData.Cleanup();
    this.adminTools = (AdminTools) null;
    this.m_GUIConsole?.Shutdown();
    GameObjectPool.Instance.Cleanup();
    SaveDataUtils.SaveDataManager.Cleanup();
    LocalPlayerManager.Destroy();
    PlatformManager.Destroy();
    LoadManager.Destroy();
    TaskManager.Destroy();
    MemoryPools.Cleanup();
    GC.Collect();
  }

  public bool IsQuitting => this.isQuitting;

  [PublicizedFrom(EAccessModifier.Protected)]
  public bool OnApplicationQuit()
  {
    this.adminTools?.DestroyFileWatcher();
    if (!this.allowQuit)
    {
      if (!this.isQuitting)
      {
        this.isQuitting = true;
        this.StartCoroutine(this.ApplicationQuitCo(0.3f));
      }
      return false;
    }
    GameSenseManager.Instance?.Cleanup();
    ThreadManager.Shutdown();
    WorldStaticData.QuitCleanup();
    if (Object.op_Inequality((Object) SingletonMonoBehaviour<SdtdConsole>.Instance, (Object) null))
      SingletonMonoBehaviour<SdtdConsole>.Instance.Cleanup();
    Log.Out(nameof (OnApplicationQuit));
    return true;
  }

  public void OnApplicationFocus(bool _focus)
  {
    if (GameManager.IsDedicatedServer)
      return;
    this.GameIsFocused = _focus;
    if (Application.isEditor)
      return;
    if (!_focus)
      this.setCursorEnabled(true);
    else if (this.bCursorVisibleOverride)
      this.setCursorEnabled(this.bCursorVisibleOverrideState);
    else if (!this.isAnyCursorWindowOpen())
      this.setCursorEnabled(false);
    if (ActionSetManager.DebugLevel != ActionSetManager.EDebugLevel.Off)
    {
      Log.Out("Focus: " + _focus.ToString());
      Log.Out("Input state:");
      foreach (PlayerActionsBase actionSet in PlatformManager.NativePlatform.Input.ActionSets)
        Log.Out($"   {((object) actionSet).GetType().Name}: {actionSet.Enabled}");
      Log.Out("Modal window open: " + LocalPlayerUI.PlayerUIs.Any<LocalPlayerUI>((Func<LocalPlayerUI, bool>) ([PublicizedFrom(EAccessModifier.Internal)] (ui) => ui.windowManager.IsModalWindowOpen())).ToString());
      Log.Out("Cursor window: " + this.isAnyCursorWindowOpen().ToString());
    }
    ModEvents.SGameFocusData _data = new ModEvents.SGameFocusData(_focus);
    ModEvents.GameFocus.Invoke(ref _data);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public bool isAnyModalWindowOpen()
  {
    IList<LocalPlayerUI> playerUis = (IList<LocalPlayerUI>) LocalPlayerUI.PlayerUIs;
    for (int index = playerUis.Count - 1; index >= 0; --index)
    {
      if (playerUis[index].windowManager.IsModalWindowOpen())
        return true;
    }
    return false;
  }

  public bool isAnyCursorWindowOpen(LocalPlayerUI _ui = null)
  {
    if (Object.op_Equality((Object) _ui, (Object) null))
    {
      IList<LocalPlayerUI> playerUis = (IList<LocalPlayerUI>) LocalPlayerUI.PlayerUIs;
      for (int index = 0; index < playerUis.Count; ++index)
      {
        if (!playerUis[index].windowManager.IsWindowOpen("timer") && (playerUis[index].windowManager.IsModalWindowOpen() || playerUis[index].windowManager.IsCursorWindowOpen()))
          return true;
      }
    }
    else if (_ui.windowManager.IsModalWindowOpen() || _ui.windowManager.IsCursorWindowOpen())
      return true;
    return false;
  }

  public void SetCursorEnabledOverride(bool _bOverrideOn, bool _bOverrideState)
  {
    if (this.bCursorVisibleOverride == _bOverrideOn)
      return;
    this.bCursorVisibleOverride = _bOverrideOn;
    this.setCursorEnabled(_bOverrideState);
  }

  public bool GetCursorEnabledOverride() => this.bCursorVisibleOverride;

  [PublicizedFrom(EAccessModifier.Private)]
  public void setCursorEnabled(bool _e)
  {
    if (this.IsQuitting)
      return;
    this.bCursorVisible = _e;
    if (ActionSetManager.DebugLevel == ActionSetManager.EDebugLevel.Verbose)
      Log.Out("CursorEnabled: " + _e.ToString());
    SoftCursor.SetCursorVisible(this.bCursorVisible);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public IEnumerator ApplicationQuitCo(float _delay)
  {
    Log.Out("Preparing quit");
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.CurrentMode != ProtocolManager.NetworkType.None)
    {
      try
      {
        this.Disconnect();
      }
      catch (Exception ex)
      {
        Log.Error("Disconnecting failed:");
        Log.Exception(ex);
      }
      yield return (object) new WaitForSeconds(_delay);
    }
    if (!GameManager.IsDedicatedServer)
      this.windowManager.CloseAllOpenWindows();
    GamePrefs.Instance.Save();
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
      SingletonMonoBehaviour<ConnectionManager>.Instance.StopServers();
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
      SingletonMonoBehaviour<ConnectionManager>.Instance.Disconnect();
    this.Cleanup();
    yield return (object) new WaitForSeconds(0.05f);
    this.allowQuit = true;
    Application.Quit();
  }

  public void ShowMessagePlayerDenied(GameUtils.KickPlayerData _kickData)
  {
    Log.Out("[NET] Kicked from server: " + _kickData.ToString());
    (((XUiWindowGroup) this.windowManager.GetWindow(XUiC_MessageBoxWindowGroup.ID)).Controller as XUiC_MessageBoxWindowGroup).ShowMessage(Localization.Get("auth_messageTitle"), _kickData.LocalizedMessage());
  }

  public void ShowMessageServerAuthFailed(string _message)
  {
    Log.Out("Client failed to authorize server: " + _message);
    ((XUiC_MessageBoxWindowGroup) ((XUiWindowGroup) this.windowManager.GetWindow(XUiC_MessageBoxWindowGroup.ID)).Controller).ShowMessage(Localization.Get("auth_serverAuthFailedTitle"), _message);
  }

  public void PlayerLoginRPC(
    ClientInfo _cInfo,
    string _playerName,
    (PlatformUserIdentifierAbs userId, string token) _platformUserAndToken,
    (PlatformUserIdentifierAbs userId, string token) _crossplatformUserAndToken,
    string _compatibilityVersion)
  {
    Log.Out($"PlayerLogin: {_playerName}/{_compatibilityVersion}");
    Log.Out("Client IP: " + _cInfo.ip);
    AuthorizationManager.Instance.Authorize(_cInfo, _playerName, _platformUserAndToken, _crossplatformUserAndToken, _compatibilityVersion);
  }

  public IEnumerator RequestToEnterGame(ClientInfo _cInfo)
  {
    ModEvents.SPlayerJoinedGameData _data = new ModEvents.SPlayerJoinedGameData(_cInfo);
    ModEvents.PlayerJoinedGame.Invoke(ref _data);
    Log.Out($"RequestToEnterGame: {_cInfo.InternalId.CombinedString}/{_cInfo.playerName}");
    IPlatformUserData userData = PlatformUserManager.GetOrCreate(_cInfo.CrossplatformId);
    if (userData != null)
    {
      userData.MarkBlockedStateChanged();
      yield return (object) PlatformUserManager.ResolveUserBlockedCoroutine(userData);
      if (userData.Blocked[EBlockType.Play].IsBlocked())
      {
        Log.Out($"Player {_cInfo.InternalId} is blocked");
        _cInfo.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackagePlayerDenied>().Setup(new GameUtils.KickPlayerData(GameUtils.EKickReason.ManualKick)));
        yield break;
      }
    }
    if ((DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent() && !this.persistentPlayerIds.Contains(_cInfo.InternalId.ToString()))
    {
      if (this.persistentPlayerCount + 1 > 100)
      {
        Log.Out("Persistent player data entries limit reached, rejecting new player {0}", new object[1]
        {
          (object) _cInfo.InternalId.ToString()
        });
        _cInfo.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackagePlayerDenied>().Setup(new GameUtils.KickPlayerData(GameUtils.EKickReason.PersistentPlayerDataExceeded)));
        yield break;
      }
      this.persistentPlayerIds.Add(_cInfo.InternalId.ToString());
    }
    PersistentPlayerList _playerList = this.persistentPlayers != null ? this.persistentPlayers.NetworkCloneRelevantForPlayer() : (PersistentPlayerList) null;
    _cInfo.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageIdMapping>().Setup("blocks", Block.fullMappingDataForClients));
    _cInfo.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageIdMapping>().Setup("items", ItemClass.fullMappingDataForClients));
    _cInfo.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageLocalization>().Setup(Localization.PatchedData));
    WorldStaticData.SendXmlsToClient(_cInfo);
    PlatformUserIdentifierAbs persistentPlayerId = this.getPersistentPlayerID(_cInfo);
    bool _firstTimeJoin = !PlayerDataFile.Exists(GameIO.GetPlayerDataDir(), persistentPlayerId.CombinedString);
    _cInfo.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageWorldInfo>().Setup(GamePrefs.GetString(EnumGamePrefs.GameMode), GamePrefs.GetString(EnumGamePrefs.GameWorld), GamePrefs.GetString(EnumGamePrefs.GameName), this.m_World.Guid, _playerList, GameTimer.Instance.ticks, this.m_World.ChunkCache.IsFixedSize, _firstTimeJoin, this.m_World.GetAllWallVolumes()));
    DecoManager.Instance.SendDecosToClient(_cInfo);
    for (int _idx = 0; _idx < this.m_World.ChunkClusters.Count; ++_idx)
    {
      ChunkCluster chunkCluster = this.m_World.ChunkClusters[_idx];
      if (chunkCluster != null)
        _cInfo.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageChunkClusterInfo>().Setup(chunkCluster));
    }
    _cInfo.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageWorldSpawnPoints>().Setup(this.GetSpawnPointList()));
    _cInfo.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageWorldAreas>().Setup(this.m_World.TraderAreas));
    _cInfo.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageGameStats>().Setup(GameStats.Instance));
  }

  public void WorldInfo(
    string _gameMode,
    string _levelName,
    string _gameName,
    string _guid,
    PersistentPlayerList _playerList,
    ulong _ticks,
    bool _fixedSizeCC,
    bool _firstTimeJoin,
    Dictionary<string, uint> _worldFileHashes,
    long _worldDataSize,
    List<WallVolume> _wallVolumes)
  {
    Log.Out("Received game GUID: " + _guid);
    GamePrefs.Set(EnumGamePrefs.GameMode, _gameMode);
    GamePrefs.Set(EnumGamePrefs.GameGuidClient, _guid);
    GamePrefs.Set(EnumGamePrefs.GameWorld, _levelName);
    this.persistentPlayers = _playerList;
    this.StartCoroutine(this.worldInfoCo(_levelName, _gameName, _fixedSizeCC, _firstTimeJoin, _worldFileHashes, _worldDataSize, _wallVolumes));
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public IEnumerator worldInfoCo(
    string _levelName,
    string _gameName,
    bool _fixedSizeCC,
    bool _firstTimeJoin,
    Dictionary<string, uint> _worldFileHashes,
    long _worldDataSize,
    List<WallVolume> _wallVolumes)
  {
    while (!WorldStaticData.AllConfigsReceivedAndLoaded())
    {
      if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsConnected)
        yield break;
      yield return (object) null;
    }
    GeneratedTextManager.PrefilterText(SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo.ServerLoginConfirmationText);
    XUiC_ProgressWindow.SetText(LocalPlayerUI.primaryUI, Localization.Get("uiLoadCreatingWorld"));
    yield return (object) null;
    string dataDir = GameIO.GetSaveGameLocalDir();
    string rwiFilename = Path.Combine(dataDir, "RemoteWorldInfo.xml");
    bool downloadWorld = false;
    PathAbstractions.AbstractedLocation worldLocation = PathAbstractions.WorldsSearchPaths.GetLocation(GamePrefs.GetString(EnumGamePrefs.GameWorld));
    if (worldLocation.Type == PathAbstractions.EAbstractedLocationType.None || worldLocation.Type == PathAbstractions.EAbstractedLocationType.LocalSave && !SdFile.Exists(worldLocation.FullPath + "/completed"))
    {
      Log.Out("World not found, requesting from server");
      downloadWorld = true;
    }
    else if (worldLocation.Type != PathAbstractions.EAbstractedLocationType.None)
    {
      bool worldValid = true;
      yield return (object) NetPackageWorldFolder.TestWorldValid(worldLocation.FullPath, _worldFileHashes, (Action<bool>) ([PublicizedFrom(EAccessModifier.Internal)] (_valid) => worldValid = _valid));
      if (!worldValid)
      {
        Log.Out("World not matching server files, request from server");
        downloadWorld = true;
      }
    }
    int num1 = SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo.GetValue(GameInfoInt.WorldSize);
    long requiredSpace = 2048L /*0x0800*/ + SaveDataLimitUtils.CalculatePlayerMapSize(new Vector2i(num1, num1));
    if (downloadWorld || worldLocation.Type == PathAbstractions.EAbstractedLocationType.LocalSave)
      requiredSpace += _worldDataSize;
    if (SaveInfoProvider.DataLimitEnabled)
    {
      long num2 = 0;
      SaveInfoProvider.SaveEntryInfo saveEntryInfo;
      if (SaveInfoProvider.Instance.TryGetRemoteSaveEntry(GamePrefs.GetString(EnumGamePrefs.GameGuidClient), out saveEntryInfo))
        num2 = saveEntryInfo.SizeInfo.ReportedSize;
      if (num2 < requiredSpace)
      {
        XUiC_SaveSpaceNeeded confirmationWindow = XUiC_SaveSpaceNeeded.Open(requiredSpace - num2, saveEntryInfo?.SaveDir, canDiscard: false, title: "xuiDmRemoteSaveTitle", body: "xuiDmRemoteSaveBody", confirm: "xuiStart");
        while (confirmationWindow.IsOpen)
          yield return (object) null;
        if (confirmationWindow.Result != XUiC_SaveSpaceNeeded.ConfirmationResult.Confirmed)
          SingletonMonoBehaviour<ConnectionManager>.Instance.Disconnect();
        if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsConnected)
          yield break;
        XUiC_ProgressWindow.Open(LocalPlayerUI.primaryUI, (string) null);
        confirmationWindow = (XUiC_SaveSpaceNeeded) null;
      }
      else if (num2 > requiredSpace)
        requiredSpace = num2;
      SaveInfoProvider.Instance.ClearResources();
    }
    try
    {
      if (!SdDirectory.Exists(dataDir))
        SdDirectory.CreateDirectory(dataDir);
      else
        SdFile.Delete(Path.Combine(dataDir, "archived.flag"));
    }
    catch (Exception ex)
    {
      Log.Error($"Exception creating local save dir: {dataDir} - GUID len: {GamePrefs.GetString(EnumGamePrefs.GameGuidClient).Length.ToString()}");
      Log.Exception(ex);
      throw;
    }
    string path = Path.Combine(dataDir, "hosts.txt");
    string str = $"{SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo.GetValue(GameInfoString.IP)}:{SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo.GetValue(GameInfoInt.Port).ToString()}";
    List<string> stringList = !SdFile.Exists(path) ? new List<string>() : new List<string>((IEnumerable<string>) SdFile.ReadAllLines(path));
    stringList.Remove(str);
    stringList.Insert(0, str);
    SdFile.WriteAllLines(path, stringList.ToArray());
    VersionInformation _result;
    if (VersionInformation.TryParseSerializedString(SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo.GetValue(GameInfoString.ServerVersion), out _result))
      new RemoteWorldInfo(_gameName, _levelName, _result, requiredSpace).Write(rwiFilename);
    else
      Log.Error("Failed writing RemoteWorldInfo. Could not parse LastGameServerInfo information.");
    if (downloadWorld)
    {
      XUiC_ProgressWindow.SetText(LocalPlayerUI.primaryUI, string.Format(Localization.Get("uiLoadDownloadingWorldWait"), (object) 0.0f, (object) 0, (object) 0));
      yield return (object) NetPackageWorldFolder.RequestWorld();
      if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsConnected)
        yield break;
      Log.Out("World received");
    }
    yield return (object) this.createWorld(_levelName, _gameName, _wallVolumes, _fixedSizeCC);
    XUiC_ProgressWindow.SetText(LocalPlayerUI.primaryUI, Localization.Get("uiLoadCreatingPlayer"));
    yield return (object) null;
    this.worldCreated = true;
    this.firstTimeJoin = _firstTimeJoin;
    string confirmationText = GeneratedTextManager.GetDisplayTextImmediately(SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo.ServerLoginConfirmationText, false, _bbSupportMode: GeneratedTextManager.BbCodeSupportMode.Supported);
    if (string.IsNullOrEmpty(confirmationText))
      confirmationText = SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo.ServerLoginConfirmationText.Text;
    if (!string.IsNullOrEmpty(confirmationText))
    {
      LocalPlayerUI playerUI = LocalPlayerUI.GetUIForPrimaryPlayer();
      while (!playerUI.xui.isReady)
        yield return (object) null;
      yield return (object) null;
      if (!string.IsNullOrEmpty(XUiC_ServerJoinRulesDialog.ID) && playerUI.xui.FindWindowGroupByName(XUiC_ServerJoinRulesDialog.ID) != null)
      {
        XUiC_ProgressWindow.Close(LocalPlayerUI.primaryUI);
        this.windowManager.CloseIfOpen("crossplayWarning");
        XUiC_ServerJoinRulesDialog.Show(playerUI, confirmationText);
      }
      else
        this.DoSpawn();
      playerUI = (LocalPlayerUI) null;
    }
    else
      this.DoSpawn();
    confirmationText = (string) null;
    DynamicMeshManager.Init();
  }

  public void DoSpawn()
  {
    if (GamePrefs.GetBool(EnumGamePrefs.SkipSpawnButton))
      this.RequestToSpawn();
    else
      XUiC_SpawnSelectionWindow.Open(LocalPlayerUI.primaryUI, false, true, this.firstTimeJoin);
  }

  public void RequestToSpawn(int _nearEntityId = -1)
  {
    XUiC_ProgressWindow.Open(LocalPlayerUI.primaryUI, (string) null);
    SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageRequestToSpawnPlayer>().Setup(Utils.FastMin(12, GamePrefs.GetInt(EnumGamePrefs.OptionsGfxViewDistance)), PlayerProfile.LoadLocalProfile(), _nearEntityId));
  }

  public void ChunkClusterInfo(
    string _name,
    int _id,
    bool _bInifiniteTerrain,
    Vector2i _cMin,
    Vector2i _cMax,
    Vector3 _pos)
  {
    this.StartCoroutine(this.chunkClusterInfoCo(_name, _id, _bInifiniteTerrain, _cMin, _cMax, _pos));
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public IEnumerator chunkClusterInfoCo(
    string _name,
    int _id,
    bool _bInifiniteTerrain,
    Vector2i _cMin,
    Vector2i _cMax,
    Vector3 _pos)
  {
    while (!this.worldCreated && SingletonMonoBehaviour<ConnectionManager>.Instance.IsConnected)
      yield return (object) null;
    if (this.worldCreated && this.m_World != null)
    {
      ChunkCluster chunkCluster = (ChunkCluster) null;
      if (_id == 0)
        chunkCluster = this.m_World.ChunkClusters[0];
      chunkCluster.Position = _pos;
      chunkCluster.ChunkMinPos = _cMin;
      chunkCluster.ChunkMaxPos = _cMax;
      if (!_bInifiniteTerrain && Object.op_Inequality((Object) this.m_World.m_WorldEnvironment, (Object) null))
      {
        this.m_World.m_WorldEnvironment.SetColliders((float) ((_cMin.x + 1) * 16 /*0x10*/), (float) ((_cMin.y + 1) * 16 /*0x10*/), (float) ((_cMax.x - _cMin.x - 1) * 16 /*0x10*/), (float) ((_cMax.y - _cMin.y - 1) * 16 /*0x10*/), Constants.cSizePlanesAround, 0.0f);
        this.m_World.m_WorldEnvironment.CreateLevelBorderBox(this.m_World);
        this.m_World.ChunkCache.IsFixedSize = true;
      }
      this.chunkClusterLoaded = true;
    }
  }

  public void RequestToSpawnPlayer(
    ClientInfo _cInfo,
    int _chunkViewDim,
    PlayerProfile _playerProfile,
    int _nearEntityId)
  {
    int num1 = GamePrefs.GetInt(EnumGamePrefs.ServerMaxAllowedViewDistance);
    if (num1 < 4)
      num1 = 4;
    else if (num1 > 12)
      num1 = 12;
    _chunkViewDim = Mathf.Clamp(_chunkViewDim, 4, num1);
    PlatformUserIdentifierAbs persistentPlayerId = this.getPersistentPlayerID(_cInfo);
    PlayerDataFile playerDataFile = new PlayerDataFile();
    playerDataFile.Load(GameIO.GetPlayerDataDir(), persistentPlayerId.CombinedString);
    playerDataFile.lastSpawnPosition = SpawnPosition.Undef;
    int _teamNumber = 0;
    int num2 = !playerDataFile.bLoaded || playerDataFile.id == -1 ? EntityFactory.nextEntityID++ : playerDataFile.id;
    if (Object.op_Inequality((Object) this.m_World.GetEntity(num2), (Object) null))
    {
      num2 = EntityFactory.nextEntityID++;
      playerDataFile.id = num2;
    }
    Log.Out($"RequestToSpawnPlayer: {num2}, {_cInfo.playerName}, {_chunkViewDim}");
    if (GameStats.GetBool(EnumGameStats.IsSpawnNearOtherPlayer))
    {
      for (int index = 0; index < this.m_World.Players.list.Count; ++index)
      {
        int x;
        int y;
        int z;
        if (this.m_World.Players.list[index].TeamNumber == _teamNumber && this.m_World.FindRandomSpawnPointNearPlayer((Entity) this.m_World.Players.list[index], 15, out x, out y, out z, 15))
        {
          playerDataFile.lastSpawnPosition = new SpawnPosition(new Vector3i(x, y, z), 0.0f);
          break;
        }
      }
    }
    if (_nearEntityId != -1)
    {
      AllowSpawnNearFriend spawnNearFriendMode = XUiC_SpawnNearFriendsList.SpawnNearFriendMode;
      Entity entity = this.m_World.GetEntity(_nearEntityId);
      if (Object.op_Implicit((Object) entity) && spawnNearFriendMode != AllowSpawnNearFriend.Disabled)
      {
        int num3 = 15;
        Vector3 _position;
        bool flag1;
        do
        {
          --num3;
          flag1 = this.m_World.GetRandomSpawnPositionMinMaxToPosition(entity.position, 40, 150, 1, true, out _position, num2, _retryCount: 20, _checkLandClaim: true);
          if (flag1)
          {
            if (spawnNearFriendMode == AllowSpawnNearFriend.InForest)
            {
              BiomeDefinition.BiomeType? biomeType = this.m_World.GetBiomeInWorld((int) _position.x, (int) _position.z)?.m_BiomeType;
              bool flag2;
              if (biomeType.HasValue)
              {
                switch (biomeType.GetValueOrDefault())
                {
                  case BiomeDefinition.BiomeType.Forest:
                  case BiomeDefinition.BiomeType.PineForest:
                    flag2 = true;
                    goto label_21;
                }
              }
              flag2 = false;
label_21:
              flag1 = flag2;
            }
          }
          else
            break;
        }
        while (num3 > 0 && !flag1);
        if (flag1)
          playerDataFile.lastSpawnPosition = new SpawnPosition(_position, this.m_World.RandomRange(0.0f, 360f));
        else
          Log.Warning($"RequestToSpawnPlayer: Failed getting a valid spawn position near player with entity ID {_nearEntityId}");
      }
    }
    if (playerDataFile.lastSpawnPosition.IsUndef())
      playerDataFile.lastSpawnPosition = this.GetSpawnPointList().GetRandomSpawnPosition(this.m_World);
    if (!playerDataFile.bLoaded)
      playerDataFile.ecd.pos = playerDataFile.lastSpawnPosition.position;
    EntityCreationData _ecd = new EntityCreationData();
    if (!playerDataFile.bLoaded || playerDataFile.ecd.playerProfile == null || !GamePrefs.GetBool(EnumGamePrefs.PersistentPlayerProfiles))
      playerDataFile.ecd.playerProfile = _playerProfile;
    if (playerDataFile.bLoaded)
    {
      _ecd.entityData = playerDataFile.ecd.entityData;
      _ecd.readFileVersion = playerDataFile.ecd.readFileVersion;
    }
    _ecd.entityClass = EntityClass.FromString(playerDataFile.ecd.playerProfile.EntityClassName);
    _ecd.playerProfile = playerDataFile.ecd.playerProfile;
    _ecd.id = num2;
    _ecd.teamNumber = _teamNumber;
    _ecd.pos = playerDataFile.ecd.pos;
    _ecd.rot = playerDataFile.ecd.rot;
    EntityPlayer entity1 = (EntityPlayer) EntityFactory.CreateEntity(_ecd);
    entity1.isEntityRemote = true;
    entity1.Respawn(playerDataFile.bLoaded ? RespawnType.JoinMultiplayer : RespawnType.EnterMultiplayer);
    playerDataFile.ToPlayer(entity1);
    bool flag = false;
    PersistentPlayerData playerData = this.persistentPlayers?.GetPlayerData(persistentPlayerId);
    if (playerData == null)
    {
      playerData = this.persistentPlayers?.CreatePlayerData(persistentPlayerId, _cInfo.PlatformId, _cInfo.playerName, _cInfo.device.ToPlayGroup());
    }
    else
    {
      playerData.Update(_cInfo.PlatformId, new AuthoredText(_cInfo.playerName, persistentPlayerId), _cInfo.device.ToPlayGroup());
      flag = true;
    }
    playerData.LastLogin = DateTime.Now;
    playerData.EntityId = num2;
    if (this.persistentPlayers != null)
      this.persistentPlayers.MapPlayer(playerData);
    this.SavePersistentPlayerData();
    SingletonMonoBehaviour<ConnectionManager>.Instance.SetClientEntityId(_cInfo, num2, playerDataFile);
    _cInfo.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackagePlayerId>().Setup(num2, _teamNumber, playerDataFile, _chunkViewDim));
    GameManager.Instance.World.aiDirector.GetComponent<AIDirectorAirDropComponent>().RefreshCrates(num2);
    this.m_World.SpawnEntityInWorld((Entity) entity1);
    entity1.ChunkObserver = this.m_World.m_ChunkManager.AddChunkObserver(entity1.GetPosition(), false, _chunkViewDim, entity1.entityId);
    IMapChunkDatabase.TryCreateOrLoad(entity1.entityId, out entity1.ChunkObserver.mapDatabase, (Func<IMapChunkDatabase.DirectoryPlayerId>) ([PublicizedFrom(EAccessModifier.Internal)] () => new IMapChunkDatabase.DirectoryPlayerId(GameIO.GetPlayerDataDir(), persistentPlayerId.CombinedString)));
    if (this.persistentPlayers != null)
    {
      this.MarkPlayerEntityFriends();
      this.persistentPlayers.DispatchPlayerEvent(playerData, (PersistentPlayerData) null, EnumPersistentPlayerDataReason.Login);
    }
    if (flag)
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackagePersistentPlayerState>().Setup(playerData, EnumPersistentPlayerDataReason.Login));
    else
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackagePersistentPlayerState>().Setup(playerData, EnumPersistentPlayerDataReason.New));
    ModEvents.SPlayerSpawningData _data = new ModEvents.SPlayerSpawningData(_cInfo, _chunkViewDim, _playerProfile);
    ModEvents.PlayerSpawning.Invoke(ref _data);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void MarkPlayerEntityFriends()
  {
    if (Object.op_Equality((Object) this.myEntityPlayerLocal, (Object) null) || this.persistentLocalPlayer == null)
      return;
    for (int index = 0; index < this.m_World.Players.list.Count; ++index)
    {
      EntityPlayer entityPlayer = this.m_World.Players.list[index];
      if (entityPlayer.entityId != this.myPlayerId)
      {
        PersistentPlayerData dataFromEntityId = this.persistentPlayers?.GetPlayerDataFromEntityID(entityPlayer.entityId);
        entityPlayer.IsFriendOfLocalPlayer = dataFromEntityId != null && this.persistentLocalPlayer.ACL != null && this.persistentLocalPlayer.ACL.Contains(dataFromEntityId.PrimaryId);
      }
    }
  }

  public void PersistentPlayerEvent(
    PlatformUserIdentifierAbs playerID,
    PlatformUserIdentifierAbs otherPlayerID,
    EnumPersistentPlayerDataReason reason)
  {
    PersistentPlayerData playerData1 = this.persistentPlayers != null ? this.persistentPlayers.GetPlayerData(playerID) : (PersistentPlayerData) null;
    if (playerData1 == null)
      return;
    PersistentPlayerData playerData2 = otherPlayerID != null ? this.persistentPlayers.GetPlayerData(otherPlayerID) : (PersistentPlayerData) null;
    if (playerData2 == null && reason != EnumPersistentPlayerDataReason.Login)
      return;
    bool flag = false;
    switch (reason)
    {
      case EnumPersistentPlayerDataReason.ACL_AcceptedInvite:
        playerData1.AddPlayerToACL(playerData2.PrimaryId);
        playerData2.AddPlayerToACL(playerData1.PrimaryId);
        this.MarkPlayerEntityFriends();
        playerData2.Dispatch(playerData1, reason);
        flag = true;
        break;
      case EnumPersistentPlayerDataReason.ACL_DeclinedInvite:
        if (playerData2 == this.persistentLocalPlayer)
        {
          playerData2.Dispatch(playerData1, reason);
          break;
        }
        flag = true;
        break;
      case EnumPersistentPlayerDataReason.ACL_Invite:
        if (playerData2 == this.persistentLocalPlayer)
        {
          EntityPlayerLocal localPlayerFromId = this.m_World.GetLocalPlayerFromID(playerData2.EntityId);
          LocalPlayerUI uiForPlayer = LocalPlayerUI.GetUIForPlayer(localPlayerFromId);
          if (Object.op_Inequality((Object) uiForPlayer, (Object) null))
          {
            NGUIWindowManager nguiWindowManager = uiForPlayer.nguiWindowManager;
            if (uiForPlayer.xui.GetChildByType<XUiC_PlayersList>().AddInvite(playerID))
            {
              EntityPlayer entity = this.m_World.GetEntity(playerData1.EntityId) as EntityPlayer;
              if (Object.op_Inequality((Object) entity, (Object) null))
                GameManager.ShowTooltip(localPlayerFromId, "friendInviteReceived", entity.PlayerDisplayName);
              playerData2.Dispatch(playerData1, reason);
              break;
            }
            break;
          }
          break;
        }
        flag = true;
        break;
      case EnumPersistentPlayerDataReason.ACL_Removed:
        playerData1.RemovePlayerFromACL(playerData2.PrimaryId);
        playerData2.RemovePlayerFromACL(playerData1.PrimaryId);
        this.MarkPlayerEntityFriends();
        playerData1.Dispatch(playerData2, reason);
        playerData2.Dispatch(playerData1, reason);
        flag = true;
        break;
    }
    this.persistentPlayers.DispatchPlayerEvent(playerData1, playerData2, reason);
    if (!(SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer & flag))
      return;
    SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackagePlayerAcl>().Setup(playerData1.PrimaryId, otherPlayerID, reason), true);
  }

  public void PersistentPlayerLogin(PersistentPlayerData ppData)
  {
    if (this.persistentPlayers == null)
      return;
    this.persistentPlayers.SetPlayerData(ppData);
    if (this.myPlayerId != -1 && ppData.EntityId == this.myPlayerId)
    {
      this.persistentLocalPlayer = ppData;
      if (Object.op_Inequality((Object) this.myEntityPlayerLocal, (Object) null))
        this.myEntityPlayerLocal.persistentPlayerData = this.persistentLocalPlayer;
    }
    this.MarkPlayerEntityFriends();
    this.persistentPlayers.DispatchPlayerEvent(ppData, (PersistentPlayerData) null, EnumPersistentPlayerDataReason.Login);
  }

  public void HandlePersistentPlayerDisconnected(int _entityId)
  {
    PersistentPlayerData dataFromEntityId = this.persistentPlayers.GetPlayerDataFromEntityID(_entityId);
    if (dataFromEntityId == null)
      return;
    this.persistentPlayers.DispatchPlayerEvent(dataFromEntityId, (PersistentPlayerData) null, EnumPersistentPlayerDataReason.Disconnected);
    this.persistentPlayers.UnmapPlayer(dataFromEntityId.PrimaryId);
  }

  public void SendPlayerACLInvite(PersistentPlayerData targetPlayer)
  {
    if (targetPlayer.EntityId == -1)
    {
      this.persistentLocalPlayer.Dispatch(targetPlayer, EnumPersistentPlayerDataReason.ACL_DeclinedInvite);
    }
    else
    {
      NetPackage _package = (NetPackage) NetPackageManager.GetPackage<NetPackagePlayerAcl>().Setup(this.persistentLocalPlayer.PrimaryId, targetPlayer.PrimaryId, EnumPersistentPlayerDataReason.ACL_Invite);
      if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
        SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(_package, _attachedToEntityId: targetPlayer.EntityId);
      else
        SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(_package);
    }
  }

  public void ReplyToPlayerACLInvite(PlatformUserIdentifierAbs requestingPlayerId, bool accepted)
  {
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
      this.PersistentPlayerEvent(this.persistentLocalPlayer.PrimaryId, requestingPlayerId, accepted ? EnumPersistentPlayerDataReason.ACL_AcceptedInvite : EnumPersistentPlayerDataReason.ACL_DeclinedInvite);
    else
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackagePlayerAcl>().Setup(this.persistentLocalPlayer.PrimaryId, requestingPlayerId, accepted ? EnumPersistentPlayerDataReason.ACL_AcceptedInvite : EnumPersistentPlayerDataReason.ACL_DeclinedInvite));
  }

  public void RemovePlayerFromACL(PersistentPlayerData targetPlayer)
  {
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
    {
      this.PersistentPlayerEvent(this.persistentLocalPlayer.PrimaryId, targetPlayer.PrimaryId, EnumPersistentPlayerDataReason.ACL_Removed);
    }
    else
    {
      NetPackage _package = (NetPackage) NetPackageManager.GetPackage<NetPackagePlayerAcl>().Setup(this.persistentLocalPlayer.PrimaryId, targetPlayer.PrimaryId, EnumPersistentPlayerDataReason.ACL_Removed);
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(_package);
    }
  }

  public void PlayerId(
    int _playerId,
    int _teamNumber,
    PlayerDataFile _playerDataFile,
    int _chunkViewDim)
  {
    Log.Out($"PlayerId({_playerId}, {_teamNumber})");
    Log.Out("Allowed ChunkViewDistance: " + _chunkViewDim.ToString());
    GameStats.Set(EnumGameStats.AllowedViewDistance, _chunkViewDim);
    this.myPlayerId = _playerId;
    EntityCreationData _ecd = new EntityCreationData();
    _ecd.id = _playerId;
    _ecd.teamNumber = _teamNumber;
    if (_playerDataFile.bLoaded)
    {
      _ecd.entityClass = EntityClass.FromString(_playerDataFile.ecd.playerProfile.EntityClassName);
      _ecd.playerProfile = _playerDataFile.ecd.playerProfile;
    }
    else
    {
      _ecd.playerProfile = PlayerProfile.LoadLocalProfile();
      _ecd.entityClass = EntityClass.FromString(_ecd.playerProfile.EntityClassName);
    }
    _ecd.skinTexture = GamePrefs.GetString(EnumGamePrefs.OptionsPlayerModelTexture);
    _ecd.id = _playerId;
    _ecd.pos = _playerDataFile.ecd.pos;
    _ecd.rot = _playerDataFile.ecd.rot;
    _ecd.belongsPlayerId = _playerId;
    EntityPlayerLocal entity = EntityFactory.CreateEntity(_ecd) as EntityPlayerLocal;
    this.setLocalPlayerEntity(entity);
    Log.Out($"Found own player entity with id {entity.entityId}");
    entity.lastSpawnPosition = _playerDataFile.lastSpawnPosition;
    if (_playerDataFile.bLoaded)
    {
      _playerDataFile.ToPlayer((EntityPlayer) entity);
      this.clientRespawnType = RespawnType.JoinMultiplayer;
    }
    else
      this.clientRespawnType = RespawnType.EnterMultiplayer;
    this.m_World.SpawnEntityInWorld((Entity) entity);
    entity.ChunkObserver = this.m_World.m_ChunkManager.AddChunkObserver(entity.GetPosition(), true, GameUtils.GetViewDistance(), -1);
    IMapChunkDatabase.TryCreateOrLoad(entity.entityId, out entity.ChunkObserver.mapDatabase, (Func<IMapChunkDatabase.DirectoryPlayerId>) ([PublicizedFrom(EAccessModifier.Private)] () =>
    {
      string combinedString = this.getPersistentPlayerID((ClientInfo) null).CombinedString;
      return new IMapChunkDatabase.DirectoryPlayerId(GameIO.GetPlayerDataLocalDir(), combinedString);
    }));
    LocalPlayerUI uiForPlayer = LocalPlayerUI.GetUIForPlayer(entity);
    uiForPlayer.xui.SetDataConnections();
    uiForPlayer.xui.SetCraftingData(_playerDataFile.craftingData);
    this.SetWorldTime(this.m_World.worldTime);
    PlayerInteractions.Instance.JoinedMultiplayerServer(this.persistentPlayers);
    entity.Respawn(this.clientRespawnType);
    this.gameStateManager.InitGame(false);
    this.gameStateManager.StartGame();
  }

  public void PlayerSpawnedInWorld(
    ClientInfo _cInfo,
    RespawnType _respawnReason,
    Vector3i _pos,
    int _entityId)
  {
    Entity entity;
    if (_entityId == -1 || !this.m_World.Entities.dict.TryGetValue(_entityId, out entity))
      return;
    EntityPlayer entityPlayer = entity as EntityPlayer;
    if (Object.op_Equality((Object) entityPlayer, (Object) null))
      return;
    if (_respawnReason == RespawnType.Died && entityPlayer.isEntityRemote)
      entityPlayer.SetAlive();
    if (_respawnReason == RespawnType.EnterMultiplayer || _respawnReason == RespawnType.JoinMultiplayer)
      this.DisplayGameMessage(EnumGameMessages.JoinedGame, _entityId);
    PlayerInteractions.Instance.PlayerSpawnedInMultiplayerServer(this.persistentPlayers, _entityId, _respawnReason);
    bool flag = _respawnReason == RespawnType.NewGame || _respawnReason == RespawnType.EnterMultiplayer || _respawnReason == RespawnType.JoinMultiplayer || _respawnReason == RespawnType.LoadedGame;
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer & flag)
      VehicleManager.Instance.UpdateVehicleWaypointsForPlayer(_entityId);
    ModEvents.SPlayerSpawnedInWorldData _data = new ModEvents.SPlayerSpawnedInWorldData(_cInfo, entityPlayer is EntityPlayerLocal, _entityId, _respawnReason, _pos);
    ModEvents.PlayerSpawnedInWorld.Invoke(ref _data);
    if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
      return;
    Action<ClientInfo> onClientSpawned = this.OnClientSpawned;
    if (onClientSpawned != null)
      onClientSpawned(_cInfo);
    Log.Out("PlayerSpawnedInWorld (reason: {0}, position: {2}): {1}", new object[3]
    {
      (object) _respawnReason.ToStringCached<RespawnType>(),
      _cInfo != null ? (object) _cInfo.ToString() : (object) "localplayer",
      (object) _pos.ToString()
    });
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void HandleFirstSpawnInteractions(PlayerInteraction _interaction)
  {
    if (_interaction.Type != PlayerInteractionType.FirstSpawn)
      return;
    int playerToEntity = this.persistentPlayers.PlayerToEntityMap[_interaction.PlayerData.PrimaryId];
    if (Object.op_Equality((Object) this.myEntityPlayerLocal, (Object) null))
      return;
    int num = playerToEntity;
    int? entityId = this.myEntityPlayerLocal?.entityId;
    int valueOrDefault = entityId.GetValueOrDefault();
    if (num == valueOrDefault & entityId.HasValue)
      return;
    IPlatformUserData platformUserData = PlatformUserManager.GetOrCreate(_interaction.PlayerData.PrimaryId);
    if (playerToEntity != -1 && platformUserData != null && platformUserData.Blocked[EBlockType.Play].IsBlocked())
    {
      this.DisplayGameMessage(EnumGameMessages.BlockedPlayerAlert, playerToEntity);
    }
    else
    {
      if (!GamePrefs.GetBool(EnumGamePrefs.OptionsAutoPartyWithFriends) || this.myEntityPlayerLocal.persistentPlayerData?.ACL == null || !this.myEntityPlayerLocal.persistentPlayerData.ACL.Contains(_interaction.PlayerData.PrimaryId))
        return;
      if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
        SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackagePartyActions>().Setup(NetPackagePartyActions.PartyActions.SendInvite, this.myEntityPlayerLocal.entityId, playerToEntity));
      else
        SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackagePartyActions>().Setup(NetPackagePartyActions.PartyActions.SendInvite, this.myEntityPlayerLocal.entityId, playerToEntity));
    }
  }

  public void PlayerDisconnected(ClientInfo _cInfo)
  {
    if (_cInfo.entityId != -1)
    {
      EntityPlayer entity = (EntityPlayer) this.m_World.GetEntity(_cInfo.entityId);
      Log.Out("Player {0} disconnected after {1} minutes", new object[2]
      {
        (object) GameUtils.SafeStringFormat(entity.EntityName),
        (object) ((float) (((double) Time.timeSinceLevelLoad - (double) entity.CreationTimeSinceLevelLoad) / 60.0)).ToCultureInvariantString("0.0")
      });
    }
    if (GameManager.IsDedicatedServer)
    {
      GC.Collect();
      MemoryPools.Cleanup();
    }
    PersistentPlayerData persistentPlayerData = this.getPersistentPlayerData(_cInfo);
    if (persistentPlayerData != null)
    {
      persistentPlayerData.LastLogin = DateTime.Now;
      persistentPlayerData.EntityId = -1;
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackagePersistentPlayerState>().Setup(persistentPlayerData, EnumPersistentPlayerDataReason.Disconnected), _onlyClientsNotAttachedToAnEntity: true);
    }
    this.SavePersistentPlayerData();
    SingletonMonoBehaviour<ConnectionManager>.Instance.DisconnectClient(_cInfo, _clientDisconnect: true);
  }

  public void SavePlayerData(ClientInfo _cInfo, PlayerDataFile _playerDataFile)
  {
    _cInfo.latestPlayerData = _playerDataFile;
    int entityId = _cInfo.entityId;
    if (entityId != -1)
    {
      EntityPlayer entity = (EntityPlayer) this.m_World.GetEntity(entityId);
      if (Object.op_Inequality((Object) entity, (Object) null))
      {
        _playerDataFile.Save(GameIO.GetPlayerDataDir(), _cInfo.InternalId.CombinedString);
        if (entity.ChunkObserver.mapDatabase != null)
          ThreadManager.AddSingleTask(new ThreadManager.TaskFunctionDelegate(entity.ChunkObserver.mapDatabase.SaveAsync), (object) new IMapChunkDatabase.DirectoryPlayerId(GameIO.GetPlayerDataDir(), _cInfo.InternalId.CombinedString));
        entity.QuestJournal = _playerDataFile.questJournal;
        if (this.persistentPlayers != null)
        {
          foreach (KeyValuePair<PlatformUserIdentifierAbs, PersistentPlayerData> player in this.persistentPlayers.Players)
          {
            if (player.Value.EntityId == _playerDataFile.id)
            {
              player.Value.Position = new Vector3i(_playerDataFile.ecd.pos);
              break;
            }
          }
        }
      }
    }
    ModEvents.SSavePlayerDataData _data = new ModEvents.SSavePlayerDataData(_cInfo, _playerDataFile);
    ModEvents.SavePlayerData.Invoke(ref _data);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public PlatformUserIdentifierAbs getPersistentPlayerID(ClientInfo _cInfo)
  {
    return _cInfo?.InternalId ?? PlatformManager.InternalLocalUserIdentifier;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public PersistentPlayerData getPersistentPlayerData(ClientInfo _cInfo)
  {
    return this.persistentPlayers?.GetPlayerData(this.getPersistentPlayerID(_cInfo));
  }

  public PersistentPlayerList GetPersistentPlayerList() => this.persistentPlayers;

  public PersistentPlayerData GetPersistentLocalPlayer() => this.persistentLocalPlayer;

  [PublicizedFrom(EAccessModifier.Private)]
  public IEnumerator createWorld(
    string _sWorldName,
    string _sGameName,
    List<WallVolume> _wallVolumes,
    bool _fixedSizeCC = false)
  {
    GameManager _gameManager = this;
    Log.Out($"createWorld: {_sWorldName}, {_sGameName}, {GamePrefs.GetString(EnumGamePrefs.GameMode)}");
    GamePrefs.Set(EnumGamePrefs.GameNameClient, _sGameName);
    bool isEditWorld = GameModeEditWorld.TypeName.Equals(GamePrefs.GetString(EnumGamePrefs.GameMode));
    PathAbstractions.CacheEnabled = !isEditWorld;
    if (isEditWorld)
    {
      Constants.cDigAndBuildDistance = 50f;
      Constants.cBuildIntervall = 0.2f;
      Constants.cCollectItemDistance = 50f;
    }
    else if (GameModeCreative.TypeName.Equals(GamePrefs.GetString(EnumGamePrefs.GameMode)))
    {
      Constants.cDigAndBuildDistance = 25f;
      Constants.cBuildIntervall = 0.2f;
      Constants.cCollectItemDistance = 25f;
    }
    else
    {
      Constants.cDigAndBuildDistance = 5f;
      Constants.cBuildIntervall = 0.5f;
      Constants.cCollectItemDistance = 3.5f;
    }
    OcclusionManager.Instance.WorldChanging(isEditWorld);
    yield return (object) null;
    _gameManager.m_World = new World();
    if (GameManager.IsDedicatedServer || _gameManager.IsEditMode())
      _gameManager.GameHasStarted = true;
    else
      _gameManager.StartCoroutine(_gameManager.waitForGameStart());
    _gameManager.m_World.Init((IGameManager) _gameManager, WorldBiomes.Instance);
    if (_wallVolumes != null)
      _gameManager.m_World.SetWallVolumesForClient(_wallVolumes);
    yield return (object) null;
    if (_gameManager.biomeParticleManager == null)
      _gameManager.biomeParticleManager = new BiomeParticleManager();
    if (_gameManager.OnWorldChanged != null)
      _gameManager.OnWorldChanged(_gameManager.m_World);
    PlayerInteractions.Instance.OnNewPlayerInteraction += new PlayerIteractionEvent(_gameManager.HandleFirstSpawnInteractions);
    yield return (object) null;
    yield return (object) _gameManager.m_World.LoadWorld(_sWorldName, _fixedSizeCC);
    yield return (object) null;
    AstarManager.Init(((Component) _gameManager).gameObject);
    yield return (object) null;
    _gameManager.lootManager = new LootManager((WorldBase) _gameManager.m_World);
    yield return (object) null;
    _gameManager.traderManager = new TraderManager((WorldBase) _gameManager.m_World);
    yield return (object) null;
    ResourceRequest weatherLoading = Resources.LoadAsync("Prefabs/WeatherManager");
    while (!((AsyncOperation) weatherLoading).isDone)
      yield return (object) null;
    GameObject gameObject = Object.Instantiate((Object) (weatherLoading.asset as GameObject)) as GameObject;
    gameObject.transform.SetParent(((Component) _gameManager).transform, false);
    WeatherManager.Init(_gameManager.m_World, gameObject);
    yield return (object) null;
    yield return (object) EnvironmentAudioManager.CreateNewInstance();
    yield return (object) null;
    WaterSplashCubes waterSplashCubes = new WaterSplashCubes();
    yield return (object) null;
    WireManager.Instance.Init();
    yield return (object) null;
    LoadManager.AssetRequestTask<GameObject> requestTask = LoadManager.LoadAsset<GameObject>("@:Prefabs/SkySystem/SkySystem.prefab");
    yield return (object) new WaitUntil((Func<bool>) ([PublicizedFrom(EAccessModifier.Internal)] () => requestTask.IsDone));
    SkyManager.Loaded(Object.Instantiate<GameObject>(requestTask.Asset));
    yield return (object) null;
    if (_gameManager.IsEditMode())
    {
      DynamicPrefabDecorator dynamicPrefabDecorator = _gameManager.GetDynamicPrefabDecorator();
      if (dynamicPrefabDecorator != null && _gameManager.IsEditMode())
        dynamicPrefabDecorator.CreateBoundingBoxes();
      SpawnPointList spawnPointList = _gameManager.GetSpawnPointList();
      for (int index = 0; index < spawnPointList.Count; ++index)
      {
        SpawnPoint spawnPoint = spawnPointList[index];
        SelectionBoxManager.Instance.GetCategory("StartPoint").AddBox(spawnPoint.spawnPosition.ToBlockPos().ToString(), (Vector3) spawnPoint.spawnPosition.ToBlockPos(), Vector3i.one, true).facingDirection = spawnPoint.spawnPosition.heading;
      }
      if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
        PrefabSleeperVolumeManager.Instance.StartAsServer();
      else
        PrefabSleeperVolumeManager.Instance.StartAsClient();
    }
    ModEvents.SCreateWorldDoneData _data = new ModEvents.SCreateWorldDoneData();
    ModEvents.CreateWorldDone.Invoke(ref _data);
    Log.Out("createWorld() done");
  }

  public World World
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get => this.m_World;
  }

  public SpawnPointList GetSpawnPointList()
  {
    return this.m_World.ChunkCache.ChunkProvider.GetSpawnPointList();
  }

  public ChunkManager.ChunkObserver AddChunkObserver(
    Vector3 _initialPosition,
    bool _bBuildVisualMeshAround,
    int _viewDim,
    int _entityIdToSendChunksTo)
  {
    return this.m_World.m_ChunkManager.AddChunkObserver(_initialPosition, _bBuildVisualMeshAround, _viewDim, _entityIdToSendChunksTo);
  }

  public void RemoveChunkObserver(ChunkManager.ChunkObserver _observer)
  {
    this.m_World.m_ChunkManager.RemoveChunkObserver(_observer);
  }

  public void ExplosionServer(
    int _clrIdx,
    Vector3 _worldPos,
    Vector3i _blockPos,
    Quaternion _rotation,
    ExplosionData _explosionData,
    int _entityId,
    float _delay,
    bool _bRemoveBlockAtExplPosition,
    ItemValue _itemValueExplosionSource = null)
  {
    if (_bRemoveBlockAtExplPosition)
      this.m_World.SetBlockRPC(_clrIdx, _blockPos, BlockValue.Air);
    if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageExplosionInitiate>().Setup(_clrIdx, _worldPos, _blockPos, _rotation, _explosionData, _entityId, _delay, _bRemoveBlockAtExplPosition, _itemValueExplosionSource));
    else if ((double) _delay <= 0.0)
      this.explode(_clrIdx, _worldPos, _blockPos, _rotation, _explosionData, _entityId, _itemValueExplosionSource);
    else
      this.StartCoroutine(this.explodeLater(_clrIdx, _worldPos, _blockPos, _rotation, _explosionData, _entityId, _itemValueExplosionSource, _delay));
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public IEnumerator explodeLater(
    int _clrIdx,
    Vector3 _position,
    Vector3i _blockPos,
    Quaternion _rotation,
    ExplosionData _explosionData,
    int _entityId,
    ItemValue _itemValueExplosionSource,
    float _delayInSec)
  {
    yield return (object) new WaitForSeconds(_delayInSec);
    this.explode(_clrIdx, _position, _blockPos, _rotation, _explosionData, _entityId, _itemValueExplosionSource);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void explode(
    int _clrIdx,
    Vector3 _worldPos,
    Vector3i _blockPos,
    Quaternion _rotation,
    ExplosionData _explosionData,
    int _entityId,
    ItemValue _itemValueExplosionSource)
  {
    Explosion explosion = new Explosion(this.m_World, _clrIdx, _worldPos, _blockPos, _explosionData, _entityId);
    explosion.AttackBlocks(_entityId, _itemValueExplosionSource);
    explosion.AttackEntites(_entityId, _itemValueExplosionSource, _explosionData.DamageType);
    this.tempExplPositions.Clear();
    explosion.ChangedBlockPositions.CopyValuesTo<Vector3i, BlockChangeInfo>((IList<BlockChangeInfo>) this.tempExplPositions);
    GameManager.ExplodeGroup explodeGroup = new GameManager.ExplodeGroup();
    explodeGroup.pos = _worldPos;
    explodeGroup.radius = _explosionData.BlockRadius;
    explodeGroup.delay = 3;
    foreach (BlockChangeInfo tempExplPosition in this.tempExplPositions)
    {
      if (tempExplPosition.blockValue.isair)
      {
        BlockValue block = this.m_World.GetBlock(tempExplPosition.pos);
        if (!block.isair && block.Block.IsExplosionAffected())
        {
          GameManager.ExplodeGroup.Falling falling;
          falling.pos = tempExplPosition.pos;
          falling.bv = block;
          explodeGroup.fallings.Add(falling);
        }
      }
    }
    if (explodeGroup.fallings.Count > 0)
      this.explodeFallingGroups.Add(explodeGroup);
    GameObject gameObject = this.ExplosionClient(_clrIdx, _worldPos, _rotation, _explosionData.ParticleIndex, _explosionData.BlastPower, (float) _explosionData.EntityRadius, _explosionData.BlockDamage, _entityId, this.tempExplPositions);
    if (Object.op_Inequality((Object) gameObject, (Object) null))
    {
      if ((double) _explosionData.Duration > 0.0)
      {
        TemporaryObject component = gameObject.GetComponent<TemporaryObject>();
        if (Object.op_Inequality((Object) component, (Object) null))
          component.SetLife(_explosionData.Duration);
      }
      ExplosionDamageArea explosionDamageArea;
      if (gameObject.TryGetComponent<ExplosionDamageArea>(ref explosionDamageArea))
      {
        explosionDamageArea.BuffActions = _explosionData.BuffActions;
        explosionDamageArea.InitiatorEntityId = _entityId;
      }
      if (this.m_World.aiDirector != null && !_explosionData.IgnoreHeatMap)
      {
        AudioPlayer component = gameObject.GetComponent<AudioPlayer>();
        if (Object.op_Implicit((Object) component))
          this.m_World.aiDirector.OnSoundPlayedAtPosition(_entityId, _worldPos, component.soundName, 1f);
      }
    }
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.ClientCount() > 0)
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageExplosionClient>().Setup(_clrIdx, _worldPos, _rotation, _explosionData.ParticleIndex, _explosionData.BlastPower, _explosionData.BlockDamage, (float) _explosionData.EntityRadius, _entityId, this.tempExplPositions), true);
    this.tempExplPositions.Clear();
  }

  public GameObject ExplosionClient(
    int _clrIdx,
    Vector3 _center,
    Quaternion _rotation,
    int _index,
    int _blastPower,
    float _blastRadius,
    float _blockDamage,
    int _entityId,
    List<BlockChangeInfo> _explosionChanges)
  {
    if (this.m_World == null)
      return (GameObject) null;
    GameObject gameObject = (GameObject) null;
    if (_index > 0 && _index < WorldStaticData.prefabExplosions.Length && Object.op_Inequality((Object) WorldStaticData.prefabExplosions[_index], (Object) null))
    {
      gameObject = Object.Instantiate<GameObject>(((Component) WorldStaticData.prefabExplosions[_index]).gameObject, Vector3.op_Subtraction(_center, Origin.position), _rotation);
      ApplyExplosionForce.Explode(_center, (float) _blastPower, _blastRadius);
    }
    if (_explosionChanges.Count > 0)
      this.ChangeBlocks((PlatformUserIdentifierAbs) null, _explosionChanges);
    QuestEventManager.Current.DetectedExplosion(_center, _entityId, _blockDamage);
    return gameObject;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void ExplodeGroupFrameUpdate()
  {
    int _et = EntityClass.FromString("fallingBlock");
    GameRandom gameRandom = this.m_World.GetGameRandom();
    for (int index1 = this.explodeFallingGroups.Count - 1; index1 >= 0; --index1)
    {
      GameManager.ExplodeGroup explodeFallingGroup = this.explodeFallingGroups[index1];
      if (--explodeFallingGroup.delay <= 0)
      {
        float num1 = 20f + Mathf.Pow((float) explodeFallingGroup.fallings.Count, 0.73f);
        float num2 = Utils.FastMax(1f, (float) explodeFallingGroup.fallings.Count / num1);
        float num3 = 1f;
        for (int index2 = 0; index2 < explodeFallingGroup.fallings.Count; ++index2)
        {
          if ((double) --num3 <= 0.0)
          {
            num3 += num2;
            GameManager.ExplodeGroup.Falling falling = explodeFallingGroup.fallings[index2];
            Vector3 vector3Center = falling.pos.ToVector3Center();
            vector3Center.y += 1.4f;
            if (Physics.Raycast(Vector3.op_Subtraction(vector3Center, Origin.position), Vector3.down, float.MaxValue, 65536 /*0x010000*/))
            {
              vector3Center.y -= 1.4f;
              Block block = falling.bv.Block;
              block.DropItemsOnEvent((WorldBase) this.m_World, falling.bv, EnumDropEvent.Destroy, 0.5f, vector3Center, Vector3.zero, Constants.cItemExplosionLifetime, -1, true);
              if (block.ShowModelOnFall())
              {
                EntityFallingBlock entity = (EntityFallingBlock) EntityFactory.CreateEntity(_et, -1, falling.bv, this.m_World.GetTextureFullArray(falling.pos.x, falling.pos.y, falling.pos.z), 1, vector3Center, Vector3.zero, -1f, -1, (string) null);
                Vector3 vector3 = Vector3.op_Subtraction(vector3Center, explodeFallingGroup.pos);
                float num4 = (float) (1.0 - (double) Mathf.Clamp01(((Vector3) ref vector3).magnitude / explodeFallingGroup.radius) * 0.60000002384185791);
                float num5 = 18f * num4;
                vector3.y += (float) ((double) gameRandom.RandomFloat * 6.0 - 0.20000000298023224);
                entity.SetStartVelocity(Vector3.op_Multiply(((Vector3) ref vector3).normalized, num5), (float) ((double) gameRandom.RandomFloat * 15.0 + 2.0) * num4);
                this.m_World.SpawnEntityInWorld((Entity) entity);
              }
            }
          }
        }
        this.explodeFallingGroups.RemoveAt(index1);
      }
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void SavePersistentPlayerData()
  {
    if (this.isEditMode || this.persistentPlayers == null)
      return;
    this.persistentPlayers.Write(GameIO.GetSaveGameDir() + "/players.xml");
  }

  public void ChangeBlocks(
    PlatformUserIdentifierAbs persistentPlayerId,
    List<BlockChangeInfo> _blocksToChange)
  {
    if (this.m_World == null)
      return;
    lock (this.ccChanged)
    {
      PersistentPlayerData persistentPlayerData = (PersistentPlayerData) null;
      Entity entity = (Entity) null;
      if (persistentPlayerId == null)
      {
        persistentPlayerData = this.persistentLocalPlayer;
        entity = (Entity) this.myEntityPlayerLocal;
      }
      else if (this.persistentPlayers != null)
      {
        persistentPlayerData = this.persistentPlayers.GetPlayerData(persistentPlayerId);
        if (persistentPlayerData != null && persistentPlayerData.EntityId != -1)
          entity = this.m_World.GetEntity(persistentPlayerData.EntityId);
      }
      bool flag1 = false;
      bool flag2 = false;
      ChunkCluster chunkCluster = (ChunkCluster) null;
      int count1 = 0;
      for (int index = 0; index < _blocksToChange.Count; ++index)
      {
        BlockChangeInfo blockChangeInfo = _blocksToChange[index];
        if (chunkCluster == null)
        {
          chunkCluster = this.m_World.ChunkCache;
          if (chunkCluster != null)
          {
            if (!this.ccChanged.Contains(chunkCluster))
            {
              this.ccChanged.Add(chunkCluster);
              ++count1;
              chunkCluster.ChunkPosNeedsRegeneration_DelayedStart();
            }
          }
          else
            continue;
        }
        bool _isChangeDensity = blockChangeInfo.bChangeDensity;
        bool forceDensityChange = blockChangeInfo.bForceDensityChange;
        sbyte density = chunkCluster.GetDensity(blockChangeInfo.pos);
        sbyte _density = blockChangeInfo.density;
        if (!_isChangeDensity)
        {
          if (density < (sbyte) 0 && blockChangeInfo.blockValue.isair)
          {
            _density = MarchingCubes.DensityAir;
            _isChangeDensity = true;
          }
          else if (density >= (sbyte) 0 && blockChangeInfo.blockValue.Block.shape.IsTerrain())
          {
            _density = MarchingCubes.DensityTerrain;
            _isChangeDensity = true;
          }
        }
        if ((int) density == (int) _density)
          _isChangeDensity = false;
        if (!blockChangeInfo.bChangeDamage || chunkCluster.GetBlock(blockChangeInfo.pos).type == blockChangeInfo.blockValue.type)
        {
          Chunk chunkFromWorldPos = chunkCluster.GetChunkFromWorldPos(blockChangeInfo.pos) as Chunk;
          int blockXz1 = World.toBlockXZ(blockChangeInfo.pos.x);
          int blockXz2 = World.toBlockXZ(blockChangeInfo.pos.z);
          if (chunkFromWorldPos != null)
          {
            if (blockChangeInfo.pos.y >= (int) chunkFromWorldPos.GetHeight(World.toBlockXZ(blockChangeInfo.pos.x), World.toBlockXZ(blockChangeInfo.pos.z)) && blockChangeInfo.blockValue.Block.shape.IsTerrain())
            {
              chunkFromWorldPos.SetTopSoilBroken(blockXz1, blockXz2);
              (blockXz2 != 15 ? chunkFromWorldPos : chunkCluster.GetChunkSync(chunkFromWorldPos.X, chunkFromWorldPos.Z + 1))?.SetTopSoilBroken(blockXz1, World.toBlockXZ(blockXz2 + 1));
              if ((blockXz1 != 15 ? chunkFromWorldPos : chunkCluster.GetChunkSync(chunkFromWorldPos.X + 1, chunkFromWorldPos.Z)) != null)
                chunkFromWorldPos.SetTopSoilBroken(World.toBlockXZ(blockXz1 + 1), blockXz2);
              if ((blockXz2 != 0 ? chunkFromWorldPos : chunkCluster.GetChunkSync(chunkFromWorldPos.X, chunkFromWorldPos.Z - 1)) != null)
                chunkFromWorldPos.SetTopSoilBroken(blockXz1, World.toBlockXZ(blockXz2 - 1));
              if ((blockXz1 != 0 ? chunkFromWorldPos : chunkCluster.GetChunkSync(chunkFromWorldPos.X - 1, chunkFromWorldPos.Z)) != null)
                chunkFromWorldPos.SetTopSoilBroken(World.toBlockXZ(blockXz1 - 1), blockXz2);
            }
            this.m_World.UncullChunk(chunkFromWorldPos);
          }
          TileEntity tileEntity1 = (TileEntity) null;
          if (!blockChangeInfo.blockValue.ischild)
            tileEntity1 = this.m_World.GetTileEntity(blockChangeInfo.pos);
          BlockValue _bvOld = chunkCluster.SetBlock(blockChangeInfo.pos, blockChangeInfo.bChangeBlockValue, blockChangeInfo.blockValue, _isChangeDensity, _density, true, blockChangeInfo.bUpdateLight, forceDensityChange, _changedByEntityId: blockChangeInfo.changedByEntityId);
          if (tileEntity1 != null)
          {
            TileEntity tileEntity2 = this.m_World.GetTileEntity(blockChangeInfo.pos);
            if (tileEntity1 != tileEntity2 && SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
            {
              this.lockedTileEntities.Remove((ITileEntity) tileEntity1);
              tileEntity1.ReplacedBy(_bvOld, blockChangeInfo.blockValue, tileEntity2);
            }
            if (blockChangeInfo.blockValue.isair)
            {
              this.lockedTileEntities.Remove((ITileEntity) tileEntity1);
              chunkFromWorldPos?.RemoveTileEntityAt<TileEntity>(this.m_World, World.toBlock(blockChangeInfo.pos));
            }
            else if (tileEntity1 != tileEntity2)
            {
              this.lockedTileEntities.Remove((ITileEntity) tileEntity1);
              tileEntity2?.UpgradeDowngradeFrom(tileEntity1);
            }
          }
          if (chunkFromWorldPos != null && blockChangeInfo.blockValue.isair)
            chunkFromWorldPos.RemoveBlockTrigger(World.toBlock(blockChangeInfo.pos));
          if (_bvOld.type != blockChangeInfo.blockValue.type)
          {
            Block block1 = blockChangeInfo.blockValue.Block;
            Block block2 = _bvOld.Block;
            QuestEventManager.Current.BlockChanged(block2, block1, blockChangeInfo.pos);
            if (block1 is BlockLandClaim)
            {
              if (persistentPlayerData != null)
              {
                this.persistentPlayers.PlaceLandProtectionBlock(blockChangeInfo.pos, persistentPlayerData);
                flag1 = true;
                if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
                  ((BlockLandClaim) block1).HandleDeactivatingCurrentLandClaims(persistentPlayerData);
                if (this.m_World != null && BlockLandClaim.IsPrimary(blockChangeInfo.blockValue))
                {
                  NavObject navObject = NavObjectManager.Instance.RegisterNavObject("land_claim", blockChangeInfo.pos.ToVector3());
                  if (navObject != null)
                    navObject.OwnerEntity = entity;
                }
              }
            }
            else if (block2 is BlockLandClaim)
            {
              this.persistentPlayers.RemoveLandProtectionBlock(blockChangeInfo.pos);
              flag1 = true;
              flag2 = true;
              if (this.m_World != null)
              {
                NavObjectManager.Instance.UnRegisterNavObjectByPosition(blockChangeInfo.pos.ToVector3(), "land_claim");
                if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
                  SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageEntityMapMarkerRemove>().Setup(EnumMapObjectType.LandClaim, blockChangeInfo.pos.ToVector3()));
              }
            }
            if (block1 is BlockSleepingBag || block2 is BlockSleepingBag)
            {
              EntityAlive ownerEntity = entity as EntityAlive;
              if (Object.op_Implicit((Object) ownerEntity))
              {
                if (block1 is BlockSleepingBag)
                {
                  NavObjectManager.Instance.UnRegisterNavObjectByOwnerEntity((Entity) ownerEntity, "sleeping_bag");
                  ownerEntity.SpawnPoints.Set(blockChangeInfo.pos);
                }
                else
                  this.persistentPlayers.SpawnPointRemoved(blockChangeInfo.pos);
                flag1 = true;
              }
            }
          }
          if (blockChangeInfo.bChangeTexture)
            chunkCluster.SetTextureFullArray(blockChangeInfo.pos, blockChangeInfo.textureFull);
          else if (_bvOld.Block.CanBlocksReplace)
            chunkCluster.SetTextureFullArray(blockChangeInfo.pos, new TextureFullArray(0L));
        }
      }
      if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer & flag1)
      {
        if (flag2 && Object.op_Inequality((Object) entity, (Object) null))
          entity.PlayOneShot("keystone_destroyed");
        this.SavePersistentPlayerData();
      }
      if (count1 <= 0)
        return;
      int count2 = this.ccChanged.Count;
      for (int index = 0; index < count1; ++index)
        this.ccChanged[--count2].ChunkPosNeedsRegeneration_DelayedStop();
      this.ccChanged.RemoveRange(count2, count1);
    }
  }

  public void SetBlocksRPC(
    List<BlockChangeInfo> _changes,
    PlatformUserIdentifierAbs _persistentPlayerId = null)
  {
    this.ChangeBlocks(_persistentPlayerId, _changes);
    NetPackageSetBlock netPackageSetBlock = NetPackageManager.GetPackage<NetPackageSetBlock>().Setup(this.persistentLocalPlayer, _changes, GameManager.IsDedicatedServer ? -1 : this.myPlayerId);
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
      this.SetBlocksOnClients(-1, netPackageSetBlock);
    else
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) netPackageSetBlock);
  }

  public void SetBlocksOnClients(int _exceptThisEntityId, NetPackageSetBlock package)
  {
    SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) package, _allButAttachedToEntityId: _exceptThisEntityId);
  }

  public void SetWaterRPC(NetPackageWaterSet package)
  {
    if (this.m_World != null)
    {
      ChunkCluster chunkCache = this.m_World.ChunkCache;
      if (chunkCache != null)
        package.ApplyChanges(chunkCache);
    }
    package.SetSenderId(GameManager.IsDedicatedServer ? -1 : this.myPlayerId);
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) package);
    else
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) package);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void updateBlockParticles()
  {
    lock (this.blockParticlesToSpawn)
    {
      for (int index = 0; index < this.blockParticlesToSpawn.Count; ++index)
      {
        if (this.m_BlockParticles.ContainsKey(this.blockParticlesToSpawn[index].blockPos))
          this.RemoveBlockParticleEffect(this.blockParticlesToSpawn[index].blockPos);
        Transform transform = GameManager.Instance.SpawnParticleEffectClientForceCreation(this.blockParticlesToSpawn[index].particleEffect, -1, true);
        this.m_BlockParticles[this.blockParticlesToSpawn[index].blockPos] = transform;
      }
      this.blockParticlesToSpawn.Clear();
    }
  }

  public void SpawnBlockParticleEffect(Vector3i _blockPos, ParticleEffect _pe)
  {
    lock (this.blockParticlesToSpawn)
      this.blockParticlesToSpawn.Add(new GameManager.BlockParticleCreationData(_blockPos, _pe));
  }

  public bool HasBlockParticleEffect(Vector3i _blockPos)
  {
    return this.m_BlockParticles.ContainsKey(_blockPos);
  }

  public Transform GetBlockParticleEffect(Vector3i _blockPos) => this.m_BlockParticles[_blockPos];

  public void RemoveBlockParticleEffect(Vector3i _blockPos)
  {
    lock (this.blockParticlesToSpawn)
    {
      if (this.m_BlockParticles.ContainsKey(_blockPos))
      {
        Transform blockParticle = this.m_BlockParticles[_blockPos];
        this.m_BlockParticles.Remove(_blockPos);
        if (!Object.op_Inequality((Object) blockParticle, (Object) null))
          return;
        Object.Destroy((Object) ((Component) blockParticle).gameObject);
      }
      else
      {
        for (int index = this.blockParticlesToSpawn.Count - 1; index >= 0; --index)
        {
          if (this.blockParticlesToSpawn[index].blockPos == _blockPos)
            this.blockParticlesToSpawn.RemoveAt(index);
        }
      }
    }
  }

  public void SpawnParticleEffectServer(
    ParticleEffect _pe,
    int _entityId,
    bool _forceCreation = false,
    bool _worldSpawn = false)
  {
    if (this.m_World == null)
      return;
    ParticleEffect.SpawnParticleEffect(_pe, _entityId, _forceCreation, _worldSpawn);
    if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageParticleEffect>().Setup(_pe, _entityId, _forceCreation, _worldSpawn));
    else
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageParticleEffect>().Setup(_pe, _entityId, _forceCreation, _worldSpawn), _allButAttachedToEntityId: _entityId);
  }

  public Transform SpawnParticleEffectClientForceCreation(
    ParticleEffect _pe,
    int _entityThatCausedIt,
    bool _worldSpawn)
  {
    return ParticleEffect.SpawnParticleEffect(_pe, _entityThatCausedIt, true, _worldSpawn);
  }

  public void SpawnParticleEffectClient(
    ParticleEffect _pe,
    int _entityThatCausedIt,
    bool _forceCreation = false,
    bool _worldSpawn = false)
  {
    ParticleEffect.SpawnParticleEffect(_pe, _entityThatCausedIt, _forceCreation, _worldSpawn);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void PhysicsInit()
  {
    // ISSUE: method pointer
    Physics.ContactEvent += new Physics.ContactEventDelegate((object) this, __methodptr(PhysicsContactEvent));
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void PhysicsContactEvent(
    PhysicsScene scene,
    NativeArray<ContactPairHeader>.ReadOnly pairHeaders)
  {
    int length = pairHeaders.Length;
    for (int index = 0; index < length; ++index)
    {
      ContactPairHeader pairHeader = pairHeaders[index];
      Rigidbody body = ((ContactPairHeader) ref pairHeader).Body as Rigidbody;
      if (Object.op_Implicit((Object) body))
      {
        EntityFallingBlock component = ((Component) body).GetComponent<EntityFallingBlock>();
        if (Object.op_Implicit((Object) component))
          component.OnContactEvent();
      }
    }
  }

  public bool IsEditMode() => this.isEditMode;

  public void GameMessage(
    EnumGameMessages _type,
    EntityAlive _mainEntity,
    EntityAlive _otherEntity)
  {
    if (Object.op_Equality((Object) _mainEntity, (Object) null))
      return;
    int _secondaryEntityId = -1;
    if (_mainEntity is EntityPlayer)
    {
      int entityId;
      switch (_type)
      {
        case EnumGameMessages.PlainTextLocal:
          return;
        case EnumGameMessages.EntityWasKilled:
          entityId = _mainEntity.entityId;
          if (_otherEntity is EntityPlayer)
          {
            _secondaryEntityId = _otherEntity.entityId;
            break;
          }
          break;
        case EnumGameMessages.JoinedGame:
        case EnumGameMessages.LeftGame:
        case EnumGameMessages.Chat:
          entityId = _mainEntity.entityId;
          break;
        case EnumGameMessages.ChangedTeam:
          return;
        default:
          return;
      }
      this.GameMessageServer((ClientInfo) null, _type, entityId, _secondaryEntityId);
    }
    else
    {
      if (_type != EnumGameMessages.EntityWasKilled && _type != EnumGameMessages.Chat)
        return;
      int _mainEntityId = Object.op_Inequality((Object) _mainEntity, (Object) null) ? _mainEntity.entityId : -1;
      this.GameMessageServer((ClientInfo) null, _type, _mainEntityId, _secondaryEntityId);
    }
  }

  public void GameMessageServer(
    ClientInfo _cInfo,
    EnumGameMessages _type,
    int _mainEntityId,
    int _secondaryEntityId)
  {
    if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
    {
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageGameMessage>().Setup(_type, _mainEntityId, _secondaryEntityId));
    }
    else
    {
      string playerDisplayName;
      switch (this.World.GetEntity(_mainEntityId))
      {
        case EntityPlayer entityPlayer:
          playerDisplayName = entityPlayer.PlayerDisplayName;
          break;
        case EntityAlive entityAlive:
          playerDisplayName = Localization.Get(entityAlive.EntityName);
          break;
        default:
          playerDisplayName = Localization.Get("xuiChatServer");
          break;
      }
      this.FinishGameMessageServer(_cInfo, _type, _mainEntityId, _secondaryEntityId, playerDisplayName);
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void FinishGameMessageServer(
    ClientInfo _cInfo,
    EnumGameMessages _type,
    int _mainEntityId,
    int _secondaryEntityId,
    string _mainName)
  {
    string displayName = this.persistentPlayers.GetPlayerDataFromEntityID(_secondaryEntityId)?.PlayerName.DisplayName;
    ModEvents.SGameMessageData _data = new ModEvents.SGameMessageData(_cInfo, _type, _mainName, displayName);
    (ModEvents.EModEventResult emodEventResult, Mod mod) = ModEvents.GameMessage.Invoke(ref _data);
    string str = this.DisplayGameMessage(_type, _mainEntityId, _secondaryEntityId, mod == null);
    if (emodEventResult != ModEvents.EModEventResult.StopHandlersAndVanilla)
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageGameMessage>().Setup(_type, _mainEntityId, _secondaryEntityId), true);
    else
      Log.Out("GameMessage handled by mod '{0}': {1}", new object[2]
      {
        (object) mod.Name,
        (object) str
      });
  }

  public string DisplayGameMessage(
    EnumGameMessages _type,
    int _mainEntity,
    int _secondaryEntity = -1,
    bool _log = true)
  {
    string str1 = (string) null;
    string displayName1 = this.persistentPlayers.GetPlayerDataFromEntityID(_mainEntity)?.PlayerName?.DisplayName;
    string displayName2 = _secondaryEntity == -1 ? (string) null : this.persistentPlayers.GetPlayerDataFromEntityID(_secondaryEntity)?.PlayerName?.DisplayName;
    string str2;
    string _message;
    switch (_type)
    {
      case EnumGameMessages.EntityWasKilled:
        if (!string.IsNullOrEmpty(displayName2))
        {
          str2 = $"GMSG: Player '{displayName1}' killed by '{displayName2}'";
          _message = string.Format(Localization.Get("killedGameMessage"), (object) displayName2, (object) displayName1);
          break;
        }
        str2 = $"GMSG: Player '{displayName1}' died";
        _message = string.Format(Localization.Get("diedGameMessage"), (object) displayName1);
        break;
      case EnumGameMessages.JoinedGame:
        str2 = $"GMSG: Player '{displayName1}' joined the game";
        _message = string.Format(Localization.Get("joinGameMessage"), (object) displayName1);
        break;
      case EnumGameMessages.LeftGame:
        str2 = $"GMSG: Player '{displayName1}' left the game";
        _message = string.Format(Localization.Get("leaveGameMessage"), (object) displayName1);
        break;
      case EnumGameMessages.BlockedPlayerAlert:
        str2 = $"GMSG: Blocked player '{displayName1}' is present on this server!";
        _message = string.Format("[FF0000A0]" + Localization.Get("blockedPlayerMessage"), (object) displayName1);
        break;
      default:
        return str1;
    }
    if (_log)
      Log.Out(str2);
    if (!GameManager.IsDedicatedServer)
    {
      if (_type == EnumGameMessages.BlockedPlayerAlert)
      {
        XUiC_ChatOutput.AddMessage(this.myEntityPlayerLocal.PlayerUI.xui, _type, _message);
      }
      else
      {
        foreach (EntityPlayerLocal localPlayer in this.m_World.GetLocalPlayers())
          XUiC_ChatOutput.AddMessage(LocalPlayerUI.GetUIForPlayer(localPlayer).xui, _type, _message);
      }
    }
    return str2;
  }

  public void ChatMessageServer(
    ClientInfo _cInfo,
    EChatType _chatType,
    int _senderEntityId,
    string _msg,
    List<int> _recipientEntityIds,
    EMessageSender _msgSender,
    GeneratedTextManager.BbCodeSupportMode _bbMode = GeneratedTextManager.BbCodeSupportMode.Supported)
  {
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
    {
      string _mainName = (string) null;
      if (_senderEntityId != -1)
        _mainName = Utils.EscapeBbCodes(this.persistentPlayers.GetPlayerDataFromEntityID(_senderEntityId)?.PlayerName?.AuthoredName?.Text);
      ModEvents.SChatMessageData _data = new ModEvents.SChatMessageData(_cInfo, _chatType, _senderEntityId, _msg, _mainName, _recipientEntityIds);
      (ModEvents.EModEventResult emodEventResult, Mod mod) = ModEvents.ChatMessage.Invoke(ref _data);
      this.ChatMessageClient(_chatType, _senderEntityId, _msg, _recipientEntityIds, _msgSender, GeneratedTextManager.BbCodeSupportMode.Supported);
      string str1 = _cInfo?.PlatformId != null ? _cInfo.PlatformId.CombinedString : "-non-player-";
      string str2 = _msg;
      string str3 = $"Chat (from '{str1}', entity id '{_senderEntityId}', to '{_chatType.ToStringCached<EChatType>()}'): {(_mainName != null ? (object) $"'{_mainName}': " : (object) "")}{str2}";
      if (emodEventResult == ModEvents.EModEventResult.StopHandlersAndVanilla)
        Log.Out("Chat handled by mod '{0}': {1}", new object[2]
        {
          (object) mod.Name,
          (object) str3
        });
      else
        Log.Out(str3);
      if (emodEventResult == ModEvents.EModEventResult.StopHandlersAndVanilla)
        return;
      if (_recipientEntityIds != null)
      {
        foreach (int recipientEntityId in _recipientEntityIds)
          SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.ForEntityId(recipientEntityId)?.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageChat>().Setup(_chatType, _senderEntityId, _msg, (List<int>) null, _msgSender, _bbMode));
      }
      else
        SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageChat>().Setup(_chatType, _senderEntityId, _msg, (List<int>) null, _msgSender, _bbMode), true);
    }
    else
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageChat>().Setup(_chatType, _senderEntityId, _msg, _recipientEntityIds, _msgSender, _bbMode));
  }

  public void ChatMessageClient(
    EChatType _chatType,
    int _senderEntityId,
    string _msg,
    List<int> _recipientEntityIds,
    EMessageSender _msgSender,
    GeneratedTextManager.BbCodeSupportMode _bbMode)
  {
    if (GameManager.IsDedicatedServer)
      return;
    foreach (EntityPlayerLocal localPlayer in this.m_World.GetLocalPlayers())
    {
      if (_recipientEntityIds == null || _recipientEntityIds.Contains(localPlayer.entityId))
        XUiC_ChatOutput.AddMessage(LocalPlayerUI.GetUIForPlayer(localPlayer).xui, EnumGameMessages.Chat, _msg, _chatType, _senderId: _senderEntityId, _senderHandlerId: _senderEntityId.ToString(), _messageSenderType: _msgSender, _filteringMode: GeneratedTextManager.TextFilteringMode.Filter, _bbMode: _bbMode);
    }
  }

  public void RemoveChunk(long _chunkKey) => this.m_World.m_ChunkManager.RemoveChunk(_chunkKey);

  public IBlockTool GetActiveBlockTool()
  {
    return this.activeBlockTool == null ? this.blockSelectionTool : this.activeBlockTool;
  }

  public void SetActiveBlockTool(IBlockTool _tool) => this.activeBlockTool = _tool;

  public DynamicPrefabDecorator GetDynamicPrefabDecorator()
  {
    if (this.m_World == null)
      return (DynamicPrefabDecorator) null;
    return this.m_World.ChunkCache?.ChunkProvider.GetDynamicPrefabDecorator();
  }

  public void SimpleRPC(int _entityId, SimpleRPCType _rpcType, bool _bExeLocal, bool _bOnlyLocal)
  {
    if (_bExeLocal)
    {
      EntityAlive entity = (EntityAlive) this.m_World.GetEntity(_entityId);
      if (Object.op_Inequality((Object) entity, (Object) null))
      {
        switch (_rpcType)
        {
          case SimpleRPCType.OnActivateItem:
            entity.inventory.holdingItem.OnHoldingItemActivated(entity.inventory.holdingItemData);
            break;
          case SimpleRPCType.OnResetItem:
            entity.inventory.holdingItem.OnHoldingReset(entity.inventory.holdingItemData);
            break;
        }
      }
    }
    if (_bOnlyLocal)
      return;
    NetPackage _package = (NetPackage) NetPackageManager.GetPackage<NetPackageSimpleRPC>().Setup(_entityId, _rpcType);
    if (this.m_World.IsRemote())
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(_package);
    else
      this.m_World.entityDistributer.SendPacketToTrackedPlayers(_entityId, _entityId, _package);
  }

  public void ItemDropServer(
    ItemStack _itemStack,
    Vector3 _dropPos,
    Vector3 _randomPosAdd,
    int _entityId = -1,
    float _lifetime = 60f,
    bool _bDropPosIsRelativeToHead = false)
  {
    this.ItemDropServer(_itemStack, _dropPos, _randomPosAdd, Vector3.zero, _entityId, _lifetime, _bDropPosIsRelativeToHead, 0);
  }

  public void ItemDropServer(
    ItemStack _itemStack,
    Vector3 _dropPos,
    Vector3 _randomPosAdd,
    Vector3 _initialMotion,
    int _entityId = -1,
    float _lifetime = 60f,
    bool _bDropPosIsRelativeToHead = false,
    int _clientEntityId = 0)
  {
    if (this.m_World == null)
      return;
    bool flag = SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer;
    Entity entity1 = this.m_World.GetEntity(_entityId);
    if (_clientEntityId != 0)
    {
      if (!Object.op_Implicit((Object) entity1))
        return;
      flag = !entity1.isEntityRemote;
    }
    if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
    {
      if (_clientEntityId == -1)
        _clientEntityId = --this.m_World.clientLastEntityId;
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageItemDrop>().Setup(_itemStack, _dropPos, _initialMotion, _randomPosAdd, _lifetime, _entityId, _bDropPosIsRelativeToHead, _clientEntityId));
      if (!flag)
        return;
    }
    if (_bDropPosIsRelativeToHead)
    {
      if (Object.op_Equality((Object) entity1, (Object) null))
        return;
      _dropPos = Vector3.op_Addition(_dropPos, entity1.getHeadPosition());
    }
    if (!((Vector3) ref _randomPosAdd).Equals(Vector3.zero))
      _dropPos = Vector3.op_Addition(_dropPos, new Vector3(this.m_World.RandomRange(-_randomPosAdd.x, _randomPosAdd.x), this.m_World.RandomRange(-_randomPosAdd.y, _randomPosAdd.y), this.m_World.RandomRange(-_randomPosAdd.z, _randomPosAdd.z)));
    EntityCreationData _ecd = new EntityCreationData();
    _ecd.entityClass = EntityClass.FromString("item");
    _ecd.id = SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer || _clientEntityId >= -1 ? EntityFactory.nextEntityID++ : _clientEntityId;
    _ecd.itemStack = _itemStack.Clone();
    _ecd.pos = _dropPos;
    _ecd.rot = new Vector3(20f, 0.0f, 20f);
    _ecd.lifetime = _lifetime;
    _ecd.belongsPlayerId = _entityId;
    if (_clientEntityId != -1)
      _ecd.clientEntityId = _clientEntityId;
    EntityItem entity2 = (EntityItem) EntityFactory.CreateEntity(_ecd);
    entity2.isPhysicsMaster = flag;
    if ((double) ((Vector3) ref _initialMotion).sqrMagnitude > 0.0099999997764825821)
      entity2.AddVelocity(_initialMotion);
    this.m_World.SpawnEntityInWorld((Entity) entity2);
    Chunk chunkSync = (Chunk) this.m_World.GetChunkSync(World.toChunkXZ((int) _dropPos.x), World.toChunkXZ((int) _dropPos.z));
    if (chunkSync == null)
      return;
    List<EntityItem> entityItemList = new List<EntityItem>();
    for (int index1 = 0; index1 < chunkSync.entityLists.Length; ++index1)
    {
      if (chunkSync.entityLists[index1] != null)
      {
        for (int index2 = 0; index2 < chunkSync.entityLists[index1].Count; ++index2)
        {
          if (chunkSync.entityLists[index1][index2] is EntityItem)
            entityItemList.Add(chunkSync.entityLists[index1][index2] as EntityItem);
        }
      }
    }
    int num = entityItemList.Count - 50;
    if (num <= 0)
      return;
    entityItemList.Sort((IComparer<EntityItem>) new GameManager.EntityItemLifetimeComparer());
    for (int index = entityItemList.Count - 1; index >= 0 && num > 0; --index)
    {
      entityItemList[index].MarkToUnload();
      --num;
    }
  }

  public void AddExpServer(int _entityId, string UNUSED_skill, int _experience)
  {
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
      return;
    SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageEntityAddExpServer>().Setup(_entityId, _experience));
  }

  public void AddScoreServer(
    int _entityId,
    int _zombieKills,
    int _playerKills,
    int _otherTeamnumber,
    int _conditions)
  {
    if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
    {
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageEntityAddScoreServer>().Setup(_entityId, _zombieKills, _playerKills, _otherTeamnumber, _conditions));
    }
    else
    {
      EntityAlive entity = (EntityAlive) this.m_World.GetEntity(_entityId);
      if (Object.op_Equality((Object) entity, (Object) null))
        return;
      if (entity.isEntityRemote)
        SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageEntityAddScoreClient>().Setup(_entityId, _zombieKills, _playerKills, _otherTeamnumber, _conditions), _attachedToEntityId: entity.entityId);
      else
        entity.AddScore(0, _zombieKills, _playerKills, _otherTeamnumber, _conditions);
    }
  }

  public void AwardKill(EntityAlive killer, EntityAlive killedEntity)
  {
    if (killer.isEntityRemote)
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageEntityAwardKillServer>().Setup(killer.entityId, killedEntity.entityId), _attachedToEntityId: killer.entityId);
    else
      QuestEventManager.Current.EntityKilled(killer, killedEntity);
  }

  public void ItemReloadServer(int _entityId)
  {
    if (this.m_World == null)
      return;
    this.ItemReloadClient(_entityId);
    if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageItemReload>().Setup(_entityId));
    else
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageItemReload>().Setup(_entityId), _allButAttachedToEntityId: _entityId);
  }

  public void ItemReloadClient(int _entityId)
  {
    if (this.m_World == null)
      return;
    EntityAlive entity = (EntityAlive) this.m_World.GetEntity(_entityId);
    if (!Object.op_Inequality((Object) entity, (Object) null) || !entity.inventory.IsHoldingGun())
      return;
    entity.inventory.GetHoldingGun().ReloadGun(entity.inventory.holdingItemData.actionData[0]);
  }

  public void ItemActionEffectsServer(
    int _entityId,
    int _slotIdx,
    int _itemActionIdx,
    int _firingState,
    Vector3 _startPos,
    Vector3 _direction,
    int _userData = 0)
  {
    if (this.m_World == null)
      return;
    this.ItemActionEffectsClient(_entityId, _slotIdx, _itemActionIdx, _firingState, _startPos, _direction, _userData);
    if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
    {
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageItemActionEffects>().Setup(_entityId, _slotIdx, _itemActionIdx, (ItemActionFiringState) _firingState, _startPos, _direction, _userData));
    }
    else
    {
      int _allButAttachedToEntityId = _entityId;
      Entity entity = this.m_World.GetEntity(_entityId);
      if (Object.op_Inequality((Object) entity, (Object) null) && Object.op_Inequality((Object) entity.AttachedMainEntity, (Object) null))
        _allButAttachedToEntityId = entity.AttachedMainEntity.entityId;
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageItemActionEffects>().Setup(_entityId, _slotIdx, _itemActionIdx, (ItemActionFiringState) _firingState, _startPos, _direction, _userData), _allButAttachedToEntityId: _allButAttachedToEntityId, _entitiesInRangeOfEntity: _entityId);
    }
  }

  public void ItemActionEffectsClient(
    int _entityId,
    int _slotIdx,
    int _itemActionIdx,
    int _firingState,
    Vector3 _startPos,
    Vector3 _direction,
    int _userData = 0)
  {
    if (this.m_World == null)
      return;
    EntityAlive entity = (EntityAlive) this.m_World.GetEntity(_entityId);
    if (Object.op_Equality((Object) entity, (Object) null))
      return;
    entity.inventory.GetItemActionInSlot(_slotIdx, _itemActionIdx)?.ItemActionEffects(this, entity.inventory.GetItemActionDataInSlot(_slotIdx, _itemActionIdx), _firingState, _startPos, _direction, _userData);
  }

  public void SetWorldTime(ulong _worldTime)
  {
    if (this.m_World == null)
      return;
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
      _worldTime = this.m_World.worldTime;
    this.m_World.SetTime(_worldTime);
  }

  public void AddVelocityToEntityServer(int _entityId, Vector3 _velToAdd)
  {
    if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
    {
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageEntityAddVelocity>().Setup(_entityId, _velToAdd));
    }
    else
    {
      Entity entity = this.m_World.GetEntity(_entityId);
      if (!Object.op_Inequality((Object) entity, (Object) null))
        return;
      entity.AddVelocity(_velToAdd);
    }
  }

  public void CollectEntityServer(int _entityId, int _playerId)
  {
    if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
    {
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageEntityCollect>().Setup(_entityId, _playerId));
    }
    else
    {
      Entity entity = this.m_World.GetEntity(_entityId);
      if (Object.op_Equality((Object) entity, (Object) null))
        return;
      switch (entity)
      {
        case EntityItem _:
        case EntityVehicle _:
        case EntityTurret _:
        case EntityDrone _:
          if (this.m_World.IsLocalPlayer(_playerId))
            this.CollectEntityClient(_entityId, _playerId);
          else
            SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageEntityCollect>().Setup(_entityId, _playerId), _attachedToEntityId: _playerId);
          this.m_World.RemoveEntity(entity.entityId, EnumRemoveEntityReason.Killed);
          break;
      }
    }
  }

  public void CollectEntityClient(int _entityId, int _playerId)
  {
    Entity entity1 = this.m_World.GetEntity(_entityId);
    if (Object.op_Equality((Object) entity1, (Object) null))
      return;
    EntityVehicle entityVehicle = entity1 as EntityVehicle;
    if (Object.op_Implicit((Object) entityVehicle))
    {
      entityVehicle.Collect(_playerId);
    }
    else
    {
      EntityDrone entityDrone = entity1 as EntityDrone;
      if (Object.op_Implicit((Object) entityDrone))
      {
        entityDrone.Collect(_playerId);
      }
      else
      {
        EntityTurret entityTurret = entity1 as EntityTurret;
        if (Object.op_Implicit((Object) entityTurret))
        {
          entityTurret.Collect(_playerId);
        }
        else
        {
          EntityItem entityItem = entity1 as EntityItem;
          if (!Object.op_Implicit((Object) entityItem))
            return;
          EntityPlayerLocal entity2 = this.m_World.GetEntity(_playerId) as EntityPlayerLocal;
          LocalPlayerUI uiForPlayer = LocalPlayerUI.GetUIForPlayer(entity2);
          int num1 = entity2.inventory.IsHoldingItemActionRunning() ? 1 : 0;
          int num2 = num1 != 0 ? uiForPlayer.xui.PlayerInventory.CountAvailableSpaceForItem(entityItem.itemStack.itemValue, false) : -1;
          if ((num1 == 0 || num2 - entityItem.itemStack.itemValue.ItemClass.Stacknumber.Value > entityItem.itemStack.count) && uiForPlayer.xui.PlayerInventory.AddItem(entityItem.itemStack))
            return;
          this.ItemDropServer(entityItem.itemStack, entity1.GetPosition(), Vector3.zero, _playerId, 60f, false);
        }
      }
    }
  }

  public void PickupBlockServer(
    int _clrIdx,
    Vector3i _blockPos,
    BlockValue _blockValue,
    int _playerId,
    PlatformUserIdentifierAbs persistentPlayerId = null)
  {
    if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
    {
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackagePickupBlock>().Setup(_clrIdx, _blockPos, _blockValue, _playerId, this.persistentLocalPlayer));
    }
    else
    {
      if (this.m_World.GetBlock(_clrIdx, _blockPos).type != _blockValue.type)
        return;
      if (this.m_World.IsLocalPlayer(_playerId))
        this.PickupBlockClient(_clrIdx, _blockPos, _blockValue, _playerId);
      else
        SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackagePickupBlock>().Setup(_clrIdx, _blockPos, _blockValue, _playerId, (PersistentPlayerData) null), _attachedToEntityId: _playerId);
      BlockValue _blockValue1 = _blockValue.Block.PickupSource != null ? Block.GetBlockValue(_blockValue.Block.PickupSource) : BlockValue.Air;
      this.SetBlocksRPC(new List<BlockChangeInfo>()
      {
        new BlockChangeInfo(_blockPos, _blockValue1, true)
      }, persistentPlayerId);
    }
  }

  public void PickupBlockClient(
    int _clrIdx,
    Vector3i _blockPos,
    BlockValue _blockValue,
    int _playerId)
  {
    if (this.m_World.GetBlock(_clrIdx, _blockPos).type != _blockValue.type)
      return;
    ItemStack _itemStack = _blockValue.Block.OnBlockPickedUp((WorldBase) this.m_World, _clrIdx, _blockPos, _blockValue, _playerId);
    foreach (EntityPlayerLocal localPlayer in this.m_World.GetLocalPlayers())
    {
      if (localPlayer.entityId == _playerId && localPlayer.PlayerUI.xui.PlayerInventory.AddItem(_itemStack, true))
        return;
    }
    this.ItemDropServer(_itemStack, Vector3.op_Addition(_blockPos.ToVector3(), Vector3.op_Multiply(Vector3.one, 0.5f)), Vector3.zero, _playerId, 60f, false);
  }

  public void PlaySoundAtPositionServer(
    Vector3 _pos,
    string _audioClipName,
    AudioRolloffMode _mode,
    int _distance)
  {
    this.PlaySoundAtPositionServer(_pos, _audioClipName, _mode, _distance, this.m_World.GetPrimaryPlayerId());
  }

  public void PlaySoundAtPositionServer(
    Vector3 _pos,
    string _audioClipName,
    AudioRolloffMode _mode,
    int _distance,
    int _entityId)
  {
    if (this.m_World == null)
      return;
    if (!GameManager.IsDedicatedServer)
    {
      Manager.BroadcastPlay(_pos, _audioClipName);
      if (this.m_World.aiDirector != null)
        this.m_World.aiDirector.NotifyNoise(this.m_World.GetEntity(_entityId), _pos, _audioClipName, 1f);
    }
    if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageSoundAtPosition>().Setup(_pos, _audioClipName, _mode, _distance, _entityId));
    else
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageSoundAtPosition>().Setup(_pos, _audioClipName, _mode, _distance, _entityId), _allButAttachedToEntityId: _entityId);
  }

  public void PlaySoundAtPositionClient(
    Vector3 _pos,
    string _audioClipName,
    AudioRolloffMode _mode,
    int _distance)
  {
    if (this.m_World == null)
      return;
    Manager.Play(_pos, _audioClipName);
    if (this.m_World.aiDirector == null)
      return;
    this.m_World.aiDirector.NotifyNoise((Entity) null, _pos, _audioClipName, 1f);
  }

  public void WaypointInviteServer(
    Waypoint _waypoint,
    EnumWaypointInviteMode _inviteMode,
    int _inviterEntityId)
  {
    if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
    {
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageWaypoint>().Setup(_waypoint, _inviteMode, _inviterEntityId));
    }
    else
    {
      _waypoint = _waypoint.Clone();
      _waypoint.bTracked = false;
      if (_inviteMode == EnumWaypointInviteMode.Friends)
      {
        if (Object.op_Equality((Object) (this.m_World.GetEntity(_inviterEntityId) as EntityPlayer), (Object) null))
          return;
        PersistentPlayerData dataFromEntityId1 = this.persistentPlayers.GetPlayerDataFromEntityID(_inviterEntityId);
        if (dataFromEntityId1 == null)
          return;
        for (int index = 0; index < this.m_World.Players.list.Count; ++index)
        {
          EntityPlayer entityPlayer = this.m_World.Players.list[index];
          if (entityPlayer.entityId != _inviterEntityId)
          {
            PersistentPlayerData dataFromEntityId2 = this.persistentPlayers != null ? this.persistentPlayers.GetPlayerDataFromEntityID(entityPlayer.entityId) : (PersistentPlayerData) null;
            if ((dataFromEntityId2 == null || dataFromEntityId1.ACL == null ? 0 : (dataFromEntityId1.ACL.Contains(dataFromEntityId2.PrimaryId) ? 1 : 0)) != 0)
            {
              if (this.m_World.IsLocalPlayer(entityPlayer.entityId))
                this.WaypointInviteClient(_waypoint, _inviteMode, _inviterEntityId);
              else
                SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageWaypoint>().Setup(_waypoint, _inviteMode, _inviterEntityId), _attachedToEntityId: entityPlayer.entityId);
            }
          }
        }
      }
      else
      {
        for (int index = 0; index < this.m_World.Players.list.Count; ++index)
        {
          EntityPlayer entityPlayer = this.m_World.Players.list[index];
          if (entityPlayer.entityId != _inviterEntityId)
          {
            if (this.m_World.IsLocalPlayer(entityPlayer.entityId))
              this.WaypointInviteClient(_waypoint, _inviteMode, _inviterEntityId);
            else
              SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageWaypoint>().Setup(_waypoint, _inviteMode, _inviterEntityId), _attachedToEntityId: entityPlayer.entityId);
          }
        }
      }
    }
  }

  public void RemovePartyInvitesFromAllPlayers(EntityPlayer _player)
  {
    for (int index = 0; index < this.m_World.Players.list.Count; ++index)
    {
      EntityPlayer entityPlayer = this.m_World.Players.list[index];
      if (Object.op_Inequality((Object) entityPlayer, (Object) _player))
        entityPlayer.RemovePartyInvite(_player.entityId);
    }
  }

  public void WaypointInviteClient(
    Waypoint _waypoint,
    EnumWaypointInviteMode _inviteMode,
    int _inviterEntityId,
    EntityPlayerLocal _player = null)
  {
    if (Object.op_Equality((Object) _player, (Object) null))
      _player = this.myEntityPlayerLocal;
    if (Object.op_Equality((Object) _player, (Object) null))
      return;
    PersistentPlayerData dataFromEntityId = this.persistentPlayers.GetPlayerDataFromEntityID(_inviterEntityId);
    if (dataFromEntityId != null && dataFromEntityId.PlatformData.Blocked[EBlockType.TextChat].IsBlocked() || _player.Waypoints.ContainsWaypoint(_waypoint))
      return;
    for (int index = 0; index < _player.WaypointInvites.Count; ++index)
    {
      if (_player.WaypointInvites[index].Equals((object) _waypoint))
        return;
    }
    _player.WaypointInvites.Insert(0, _waypoint);
    XUiV_Window window = LocalPlayerUI.GetUIForPlayer(_player).xui.GetWindow("mapInvites");
    if (window != null && window.IsVisible)
      ((XUiC_MapInvitesList) window.Controller.GetChildById("invitesList")).UpdateInvitesList();
    string strPlayerName = "?";
    EntityPlayer entity = this.m_World.GetEntity(_inviterEntityId) as EntityPlayer;
    if (Object.op_Inequality((Object) entity, (Object) null))
      strPlayerName = entity.PlayerDisplayName;
    GeneratedTextManager.GetDisplayText(_waypoint.name, (Action<string>) ([PublicizedFrom(EAccessModifier.Internal)] (_filtered) => GameManager.ShowTooltip(_player, string.Format(Localization.Get("tooltipInviteMarker"), (object) strPlayerName, _waypoint.bUsingLocalizationId ? (object) Localization.Get(_filtered) : (object) _filtered))), true, false);
  }

  public void QuestShareServer(
    int _questCode,
    string _questID,
    string _poiName,
    Vector3 _position,
    Vector3 _size,
    Vector3 _returnPos,
    int _sharedByEntityID,
    int _sharedWithEntityID,
    int _questGiverID)
  {
    if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageSharedQuest>().Setup(_questCode, _questID, _poiName, _position, _size, _returnPos, _sharedByEntityID, _sharedWithEntityID, _questGiverID));
    else if (this.m_World.IsLocalPlayer(_sharedWithEntityID))
      this.QuestShareClient(_questCode, _questID, _poiName, _position, _size, _returnPos, _sharedByEntityID, _questGiverID);
    else
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageSharedQuest>().Setup(_questCode, _questID, _poiName, _position, _size, _returnPos, _sharedByEntityID, _sharedWithEntityID, _questGiverID), _attachedToEntityId: _sharedWithEntityID);
  }

  public void QuestShareClient(
    int _questCode,
    string _questID,
    string _poiName,
    Vector3 _position,
    Vector3 _size,
    Vector3 _returnPos,
    int _SharedByEntityID,
    int _questGiverID,
    EntityPlayerLocal _player = null)
  {
    if (Object.op_Equality((Object) _player, (Object) null))
      _player = this.myEntityPlayerLocal;
    if (Object.op_Equality((Object) _player, (Object) null))
      return;
    if (_player.QuestJournal.HasActiveQuestByQuestCode(_questCode))
    {
      if (!PartyQuests.AutoAccept)
        return;
      Log.Out($"Ignoring received quest, already have one active with the quest code {_questCode}:");
      for (int index = 0; index < _player.QuestJournal.quests.Count; ++index)
      {
        Quest quest = _player.QuestJournal.quests[index];
        Log.Out($"  {index}.: id={quest.ID}, code={quest.QuestCode}, name={quest.QuestClass.Name}, POI={quest.GetParsedText("{poi.name}")}, state={quest.CurrentState}, owner={quest.SharedOwnerID}");
      }
    }
    else
    {
      if (!_player.QuestJournal.AddSharedQuestEntry(_questCode, _questID, _poiName, _position, _size, _returnPos, _SharedByEntityID, _questGiverID) || PartyQuests.AutoAccept)
        return;
      string str = "?";
      EntityPlayer entity = this.m_World.GetEntity(_SharedByEntityID) as EntityPlayer;
      if (Object.op_Inequality((Object) entity, (Object) null))
        str = entity.PlayerDisplayName;
      GameManager.ShowTooltip(_player, string.Format(Localization.Get("ttQuestShared"), (object) str, (object) QuestClass.GetQuest(_questID).Name), string.Empty, "ui_quest_invite");
    }
  }

  public void SharedKillServer(int _entityID, int _killerID, float _xpModifier = 1f)
  {
    if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
    {
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageSharedPartyKill>().Setup(_entityID, _killerID));
    }
    else
    {
      EntityPlayer entity1 = (EntityPlayer) this.m_World.GetEntity(_killerID);
      EntityAlive entity2 = this.m_World.GetEntity(_entityID) as EntityAlive;
      if (Object.op_Equality((Object) entity1, (Object) null) || Object.op_Equality((Object) entity2, (Object) null))
        return;
      int experienceValue = EntityClass.list[entity2.entityClass].ExperienceValue;
      int _xp = (int) EffectManager.GetValue(PassiveEffects.ExperienceGain, entity2.inventory.holdingItemItemValue, (float) experienceValue, entity2);
      if ((double) _xpModifier != 1.0)
        _xp = (int) ((double) _xp * (double) _xpModifier + 0.5);
      if (entity1.IsInParty())
      {
        int num = entity1.Party.MemberCountInRange(entity1);
        _xp = (int) ((double) _xp * (1.0 - 0.10000000149011612 * (double) num));
      }
      if (entity1.Party == null)
        return;
      for (int index = 0; index < entity1.Party.MemberList.Count; ++index)
      {
        EntityPlayer member = entity1.Party.MemberList[index];
        if (!Object.op_Equality((Object) member, (Object) entity1) && (double) Vector3.Distance(entity1.position, member.position) < (double) GameStats.GetInt(EnumGameStats.PartySharedKillRange))
        {
          if (this.m_World.IsLocalPlayer(member.entityId))
            this.SharedKillClient(entity2.entityClass, _xp, _entityID: entity2.entityId);
          else
            SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageSharedPartyKill>().Setup(entity2.entityClass, _xp, _killerID, entity2.entityId), _attachedToEntityId: member.entityId);
        }
      }
    }
  }

  public void SharedKillClient(
    int _entityTypeID,
    int _xp,
    EntityPlayerLocal _player = null,
    int _entityID = -1)
  {
    if (Object.op_Equality((Object) _player, (Object) null))
      _player = this.myEntityPlayerLocal;
    if (Object.op_Equality((Object) _player, (Object) null))
      return;
    string entityClassName = EntityClass.list[_entityTypeID].entityClassName;
    _xp = _player.Progression.AddLevelExp(_xp, "_xpFromParty", Progression.XPTypes.Kill);
    _player.bPlayerStatsChanged = true;
    if (_xp > 0)
      GameManager.ShowTooltip(_player, string.Format(Localization.Get("ttPartySharedXPReceived"), (object) _xp));
    QuestEventManager.Current.EntityKilled((EntityAlive) _player, _entityID == -1 ? (EntityAlive) null : this.m_World.GetEntity(_entityID) as EntityAlive);
  }

  public IEnumerator ShowExitingGameUICoroutine()
  {
    bool flag = this.windowManager.IsWindowOpen(XUiC_ExitingGame.ID);
    this.windowManager.Open(XUiC_ExitingGame.ID, false, true);
    if (!flag)
    {
      yield return (object) null;
      yield return (object) null;
    }
  }

  public static void ShowTooltipMP(EntityPlayer _player, string _text, string _alertSound = "")
  {
    if (_player is EntityPlayerLocal)
      GameManager.ShowTooltip(_player as EntityPlayerLocal, _text, string.Empty, _alertSound);
    else
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageShowToolbeltMessage>().Setup(_text, _alertSound), _attachedToEntityId: _player.entityId);
  }

  public static void ShowTooltip(
    EntityPlayerLocal _player,
    string _text,
    bool _showImmediately = false,
    bool _pinTooltip = false,
    float _timeout = 0.0f)
  {
    GameManager.ShowTooltip(_player, _text, (string[]) null, _showImmediately: _showImmediately, _pinTooltip: _pinTooltip, _timeout: _timeout);
  }

  public static void ShowTooltip(
    EntityPlayerLocal _player,
    string _text,
    string _arg,
    string _alertSound = null,
    ToolTipEvent _handler = null,
    bool _showImmediately = false,
    bool _pinTooltip = false,
    float _timeout = 0.0f)
  {
    GameManager.ShowTooltip(_player, _text, new string[1]
    {
      _arg
    }, _alertSound, _handler, (_showImmediately ? 1 : 0) != 0, _timeout: _timeout);
  }

  public static void ShowTooltip(
    EntityPlayerLocal _player,
    string _text,
    string[] _args,
    string _alertSound = null,
    ToolTipEvent _handler = null,
    bool _showImmediately = false,
    bool _pinTooltip = false,
    float _timeout = 0.0f)
  {
    if (GameManager.IsDedicatedServer || Object.op_Equality((Object) _player, (Object) null))
      return;
    XUiC_PopupToolTip.QueueTooltip(LocalPlayerUI.GetUIForPlayer(_player).nguiWindowManager.WindowManager.playerUI.xui, _text, _args, _alertSound, _handler, _showImmediately, _pinTooltip, _timeout);
  }

  public static void RemovePinnedTooltip(EntityPlayerLocal _player, string _key)
  {
    if (GameManager.IsDedicatedServer || Object.op_Equality((Object) _player, (Object) null))
      return;
    XUiC_PopupToolTip.RemovePinnedTooltip(LocalPlayerUI.GetUIForPlayer(_player).nguiWindowManager.WindowManager.playerUI.xui, _key);
  }

  public void ClearTooltips(NGUIWindowManager _nguiWindowManager)
  {
    if (GameManager.IsDedicatedServer)
      return;
    XUiC_PopupToolTip.ClearTooltips(_nguiWindowManager.WindowManager.playerUI.xui);
  }

  public void ClearCurrentTooltip(NGUIWindowManager _nguiWindowManager)
  {
    if (GameManager.IsDedicatedServer)
      return;
    XUiC_PopupToolTip.ClearCurrentTooltip(_nguiWindowManager.WindowManager.playerUI.xui);
  }

  public void SetToolTipPause(NGUIWindowManager _nguiWindowManager, bool _isPaused)
  {
    if (GameManager.IsDedicatedServer)
      return;
    XUiC_PopupToolTip.SetToolTipPause(_nguiWindowManager.WindowManager.playerUI.xui, _isPaused);
  }

  public static void ShowSubtitle(
    XUi _xui,
    string speaker,
    string content,
    float duration,
    bool centerAlign = false)
  {
    XUiC_SubtitlesDisplay.DisplaySubtitle(_xui.playerUI, speaker, content, duration, centerAlign);
  }

  public static void PlayVideo(
    string id,
    bool skippable,
    XUiC_VideoPlayer.DelegateOnVideoFinished callback = null)
  {
    XUiC_VideoPlayer.PlayVideo(LocalPlayerUI.primaryUI.xui, VideoManager.GetVideoData(id), skippable, callback);
  }

  public static bool IsVideoPlaying() => XUiC_VideoPlayer.IsVideoPlaying;

  public void ClearTileEntityLockForClient(int _entityId)
  {
    foreach (KeyValuePair<ITileEntity, int> lockedTileEntity in this.lockedTileEntities)
    {
      if (_entityId == lockedTileEntity.Value)
      {
        this.lockedTileEntities.Remove(lockedTileEntity.Key);
        break;
      }
    }
  }

  public int GetEntityIDForLockedTileEntity(TileEntity te)
  {
    return this.lockedTileEntities.ContainsKey((ITileEntity) te) ? this.lockedTileEntities[(ITileEntity) te] : -1;
  }

  public IEnumerator ResetWindowsAndLocksByPlayer(int _playerId)
  {
    if (_playerId != -1 && SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
    {
      List<KeyValuePair<ITileEntity, int>> _locksToClear = new List<KeyValuePair<ITileEntity, int>>();
      foreach (KeyValuePair<ITileEntity, int> lockedTileEntity in this.lockedTileEntities)
      {
        if (lockedTileEntity.Value == _playerId)
          _locksToClear.Add(lockedTileEntity);
      }
      if (_locksToClear.Count > 0)
        yield return (object) this.ResetWindowsAndLocks(_locksToClear);
    }
  }

  public IEnumerator ResetWindowsAndLocksByChunks(HashSetLong chunks)
  {
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
    {
      List<KeyValuePair<ITileEntity, int>> _locksToClear = new List<KeyValuePair<ITileEntity, int>>();
      foreach (long chunk1 in chunks)
      {
        foreach (KeyValuePair<ITileEntity, int> lockedTileEntity in this.lockedTileEntities)
        {
          Chunk chunk2 = lockedTileEntity.Key.GetChunk();
          if (chunk2 == null || chunk2.Key == chunk1)
            _locksToClear.Add(lockedTileEntity);
        }
      }
      if (_locksToClear.Count > 0)
        yield return (object) this.ResetWindowsAndLocks(_locksToClear);
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public IEnumerator ResetWindowsAndLocks(List<KeyValuePair<ITileEntity, int>> _locksToClear)
  {
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
    {
      HashSet<int> idsToClose = new HashSet<int>();
      foreach (KeyValuePair<ITileEntity, int> keyValuePair in _locksToClear)
      {
        ITileEntity key = keyValuePair.Key;
        if (key.GetChunk() == null)
        {
          Log.Error("[ResetWindowsAndLocks] Failed to find chunk for tile entity. There may be issues unlocking this tile entity");
          this.lockedTileEntities.Remove(key);
        }
        else
        {
          int _playerId = keyValuePair.Value;
          Vector3i worldPos = key.ToWorldPos();
          key.SetUserAccessing(false);
          key.SetModified();
          this.TEUnlockServer(key.GetClrIdx(), worldPos, key.EntityId);
          EntityPlayerLocal localPlayerFromId = this.m_World.GetLocalPlayerFromID(_playerId);
          if (Object.op_Inequality((Object) localPlayerFromId, (Object) null))
            localPlayerFromId.PlayerUI.windowManager.CloseAllOpenWindows();
          else
            idsToClose.Add(_playerId);
        }
      }
      this.m_World.TickEntitiesFlush();
      yield return (object) null;
      yield return (object) null;
      foreach (int num in idsToClose)
        SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageCloseAllWindows>().Setup(num), true, num);
      idsToClose = (HashSet<int>) null;
    }
  }

  public void TELockServer(
    int _clrIdx,
    Vector3i _blockPos,
    int _lootEntityId,
    int _entityIdThatOpenedIt,
    string _customUi = null)
  {
    if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
    {
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageTELock>().Setup(NetPackageTELock.TELockType.LockServer, _clrIdx, _blockPos, _lootEntityId, _entityIdThatOpenedIt, _customUi));
    }
    else
    {
      foreach (KeyValuePair<ITileEntity, int> lockedTileEntity in this.lockedTileEntities)
      {
        if (_entityIdThatOpenedIt == lockedTileEntity.Value)
          return;
      }
      TileEntity tileEntity = _lootEntityId != -1 ? this.m_World.GetTileEntity(_lootEntityId) : this.m_World.GetTileEntity(_blockPos);
      if (tileEntity == null || !this.OpenTileEntityAllowed(_entityIdThatOpenedIt, tileEntity, _customUi))
        return;
      EntityAlive entity;
      if (this.lockedTileEntities.ContainsKey((ITileEntity) tileEntity) && Object.op_Inequality((Object) (entity = (EntityAlive) this.m_World.GetEntity(this.lockedTileEntities[(ITileEntity) tileEntity])), (Object) null) && !entity.IsDead())
      {
        if (Object.op_Equality((Object) (this.m_World.GetEntity(_entityIdThatOpenedIt) as EntityPlayerLocal), (Object) null))
          SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageTELock>().Setup(NetPackageTELock.TELockType.DeniedAccess, _clrIdx, _blockPos, _lootEntityId, _entityIdThatOpenedIt, _customUi), _attachedToEntityId: _entityIdThatOpenedIt);
        else
          this.TEDeniedAccessClient(_clrIdx, _blockPos, _lootEntityId, _entityIdThatOpenedIt);
      }
      else
      {
        this.lockedTileEntities[(ITileEntity) tileEntity] = _entityIdThatOpenedIt;
        if (tileEntity == null)
          return;
        this.OpenTileEntityUi(_entityIdThatOpenedIt, (ITileEntity) tileEntity, _customUi);
        SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageTELock>().Setup(NetPackageTELock.TELockType.AccessClient, _clrIdx, _blockPos, _lootEntityId, _entityIdThatOpenedIt, _customUi), true);
      }
    }
  }

  public void TEUnlockServer(
    int _clrIdx,
    Vector3i _blockPos,
    int _lootEntityId,
    bool _allowContainerDestroy = true)
  {
    if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
    {
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageTELock>().Setup(NetPackageTELock.TELockType.UnlockServer, _clrIdx, _blockPos, _lootEntityId, -1, _allowEmptyDestroy: _allowContainerDestroy));
    }
    else
    {
      TileEntity tileEntity;
      if (_lootEntityId == -1)
      {
        tileEntity = this.m_World.GetTileEntity(_blockPos);
      }
      else
      {
        tileEntity = this.m_World.GetTileEntity(_lootEntityId);
        if (tileEntity == null)
        {
          foreach (KeyValuePair<ITileEntity, int> lockedTileEntity in this.lockedTileEntities)
          {
            if (lockedTileEntity.Key.EntityId == _lootEntityId)
            {
              this.lockedTileEntities.Remove(lockedTileEntity.Key);
              break;
            }
          }
        }
      }
      if (tileEntity == null)
        return;
      this.lockedTileEntities.Remove((ITileEntity) tileEntity);
      if (!_allowContainerDestroy)
        return;
      this.DestroyLootOnClose(tileEntity, _blockPos, _lootEntityId);
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void DestroyLootOnClose(TileEntity _te, Vector3i _blockPos, int _lootEntityId)
  {
    if (!(_te is ITileEntityLootable tileEntityLootable))
      return;
    switch (LootContainer.GetLootContainer(tileEntityLootable.lootListName).destroyOnClose)
    {
      case LootContainer.DestroyOnClose.True:
        if (tileEntityLootable.bPlayerBackpack)
        {
          if (!tileEntityLootable.IsEmpty())
            break;
          if (_lootEntityId == -1)
          {
            BlockValue block = this.m_World.GetBlock(_blockPos);
            block.Block.DamageBlock((WorldBase) this.m_World, 0, _blockPos, block, block.Block.MaxDamage, -1);
            break;
          }
          Entity entity = this.m_World.GetEntity(_lootEntityId);
          if (!Object.op_Inequality((Object) entity, (Object) null))
            break;
          entity.KillLootContainer();
          break;
        }
        if (_lootEntityId == -1)
        {
          BlockValue block = this.m_World.GetBlock(_blockPos);
          this.DropContentOfLootContainerServer(block, _blockPos, _lootEntityId, (ITileEntityLootable) null);
          block.Block.DamageBlock((WorldBase) this.m_World, 0, _blockPos, block, block.Block.MaxDamage, -1);
          break;
        }
        this.DropContentOfLootContainerServer(BlockValue.Air, _blockPos, _lootEntityId, (ITileEntityLootable) null);
        Entity entity1 = this.m_World.GetEntity(_lootEntityId);
        if (!Object.op_Inequality((Object) entity1, (Object) null))
          break;
        entity1.KillLootContainer();
        break;
      case LootContainer.DestroyOnClose.Empty:
        if (!tileEntityLootable.IsEmpty())
          break;
        goto case LootContainer.DestroyOnClose.True;
    }
  }

  public void TEAccessClient(
    int _clrIdx,
    Vector3i _blockPos,
    int _lootEntityId,
    int _entityIdThatOpenedIt,
    string _customUi = null)
  {
    if (this.m_World == null)
      return;
    TileEntity _te = _lootEntityId != -1 ? this.m_World.GetTileEntity(_lootEntityId) : this.m_World.GetTileEntity(_blockPos);
    if (_te == null)
      return;
    int playerId = this.myPlayerId;
    this.OpenTileEntityUi(_entityIdThatOpenedIt, (ITileEntity) _te, _customUi);
  }

  public void FreeAllTileEntityLocks() => this.lockedTileEntities.Clear();

  [PublicizedFrom(EAccessModifier.Private)]
  public bool OpenTileEntityAllowed(int _entityIdThatOpenedIt, TileEntity _te, string _customUi)
  {
    ITileEntityLootable _typedTe;
    return !_te.TryGetSelfOrFeature<ITileEntityLootable>(out _typedTe) || this.lootContainerCanOpen(_typedTe, _entityIdThatOpenedIt);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void OpenTileEntityUi(int _entityIdThatOpenedIt, ITileEntity _te, string _customUi)
  {
    EntityPlayerLocal entity = this.m_World.GetEntity(_entityIdThatOpenedIt) as EntityPlayerLocal;
    LocalPlayerUI uiForPlayer = LocalPlayerUI.GetUIForPlayer(entity);
    if (!string.IsNullOrEmpty(_customUi))
    {
      switch (_customUi)
      {
        case "sign":
          this.signOpened(_te, uiForPlayer);
          break;
        case "lockpick":
          this.lockpickOpened(_te, entity);
          break;
        case "container":
          this.lootContainerOpened(_te, uiForPlayer, _entityIdThatOpenedIt);
          break;
      }
    }
    else
    {
      switch (_te)
      {
        case TileEntityLootContainer _te1:
          this.lootContainerOpened((ITileEntity) _te1, uiForPlayer, _entityIdThatOpenedIt);
          break;
        case TileEntityDewCollector _te2:
          this.dewCollectorOpened(_te2, uiForPlayer, _entityIdThatOpenedIt);
          break;
        case TileEntityWorkstation _te3:
          this.workstationOpened(_te3, uiForPlayer);
          break;
        case TileEntityTrader _te4:
          this.traderOpened(_te4, uiForPlayer);
          break;
        case ITileEntitySignable _:
          this.signOpened(_te, uiForPlayer);
          break;
        case TileEntityPowerSource _te5:
          this.generatorOpened(_te5, uiForPlayer);
          break;
        case TileEntityPoweredTrigger _te6:
          this.triggerOpened(_te6, uiForPlayer);
          break;
        case TileEntityPoweredRangedTrap _te7:
          this.rangedTrapOpened(_te7, uiForPlayer);
          break;
        case TileEntityPowered _te8:
          this.poweredGenericOpened(_te8, uiForPlayer);
          break;
      }
    }
  }

  public void TEDeniedAccessClient(
    int _clrIdx,
    Vector3i _blockPos,
    int _lootEntityId,
    int _entityIdThatOpenedIt)
  {
    if (this.m_World == null)
      return;
    LocalPlayerUI uiForPlayer = LocalPlayerUI.GetUIForPlayer(this.m_World.GetEntity(_entityIdThatOpenedIt) as EntityPlayerLocal);
    if (Object.op_Equality((Object) uiForPlayer, (Object) null))
      return;
    TileEntity tileEntity = _lootEntityId != -1 ? this.m_World.GetTileEntity(_lootEntityId) : this.m_World.GetTileEntity(_blockPos);
    if (tileEntity == null)
      return;
    if (tileEntity is TileEntityTrader)
    {
      if (tileEntity is TileEntityVendingMachine)
        GameManager.ShowTooltip(uiForPlayer.entityPlayer, Localization.Get("ttNoInteractItem"), string.Empty, "ui_denied");
      else
        GameManager.ShowTooltip(uiForPlayer.entityPlayer, Localization.Get("ttNoInteractPerson"), string.Empty, "ui_denied");
    }
    else
      GameManager.ShowTooltip(uiForPlayer.entityPlayer, Localization.Get("ttNoInteractItem"), string.Empty, "ui_denied");
    uiForPlayer.entityPlayer.OverrideFOV = -1f;
    uiForPlayer.xui.Dialog.keepZoomOnClose = false;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void workstationOpened(TileEntityWorkstation _te, LocalPlayerUI _playerUI)
  {
    if (!Object.op_Inequality((Object) _playerUI, (Object) null))
      return;
    string blockName = this.m_World.GetBlock(_te.ToWorldPos()).Block.GetBlockName();
    WorkstationData workstationData = CraftingManager.GetWorkstationData(blockName);
    if (workstationData == null)
      return;
    string str = workstationData.WorkstationWindow != "" ? workstationData.WorkstationWindow : $"workstation_{blockName}";
    if (_playerUI.windowManager.HasWindow(str))
    {
      ((XUiC_WorkstationWindowGroup) ((XUiWindowGroup) _playerUI.windowManager.GetWindow(str)).Controller).SetTileEntity(_te);
      _playerUI.windowManager.Open(str, true);
    }
    else
      Log.Warning("Window '{0}' not found in XUI!", new object[1]
      {
        (object) str
      });
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void traderOpened(TileEntityTrader _te, LocalPlayerUI _playerUI)
  {
    if (!Object.op_Inequality((Object) _playerUI, (Object) null))
      return;
    _playerUI.xui.Trader.TraderTileEntity = _te;
    _playerUI.xui.Trader.Trader = _te.TraderData;
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && GameManager.Instance.traderManager.TraderInventoryRequested(_te.TraderData, XUiM_Player.GetPlayer().entityId))
      _te.SetModified();
    _playerUI.windowManager.CloseAllOpenWindows();
    _playerUI.windowManager.Open("trader", true);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void signOpened(ITileEntity _te, LocalPlayerUI _playerUI)
  {
    ITileEntitySignable selfOrFeature = _te.GetSelfOrFeature<ITileEntitySignable>();
    if (selfOrFeature == null)
    {
      Log.Error($"Can not open sign UI for TE {_te}");
    }
    else
    {
      if (!Object.op_Inequality((Object) _playerUI, (Object) null))
        return;
      ((XUiWindowGroup) _playerUI.windowManager.GetWindow("signMultiline")).Controller.GetChildByType<XUiC_SignWindow>().SetTileEntitySign(selfOrFeature);
      _playerUI.windowManager.Open("signMultiline", true);
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void lockpickOpened(ITileEntity _te, EntityPlayerLocal _player)
  {
    ILockPickable selfOrFeature = _te.GetSelfOrFeature<ILockPickable>();
    if (selfOrFeature == null)
      Log.Error($"Can not open lockpick UI for TE {_te}");
    else
      selfOrFeature.ShowLockpickUi(_player);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void generatorOpened(TileEntityPowerSource _te, LocalPlayerUI _playerUI)
  {
    if (!Object.op_Inequality((Object) _playerUI, (Object) null))
      return;
    ((XUiC_PowerSourceWindowGroup) ((XUiWindowGroup) _playerUI.windowManager.GetWindow("powersource")).Controller).TileEntity = _te;
    _playerUI.windowManager.Open("powersource", true);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void triggerOpened(TileEntityPoweredTrigger _te, LocalPlayerUI _playerUI)
  {
    if (!Object.op_Inequality((Object) _playerUI, (Object) null))
      return;
    ((XUiC_PowerTriggerWindowGroup) ((XUiWindowGroup) _playerUI.windowManager.GetWindow("powertrigger")).Controller).TileEntity = _te;
    _playerUI.windowManager.Open("powertrigger", true);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void rangedTrapOpened(TileEntityPoweredRangedTrap _te, LocalPlayerUI _playerUI)
  {
    if (!Object.op_Inequality((Object) _playerUI, (Object) null))
      return;
    ((XUiC_PowerRangedTrapWindowGroup) ((XUiWindowGroup) _playerUI.windowManager.GetWindow("powerrangedtrap")).Controller).TileEntity = _te;
    _playerUI.windowManager.Open("powerrangedtrap", true);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void poweredGenericOpened(TileEntityPowered _te, LocalPlayerUI _playerUI)
  {
    if (!Object.op_Inequality((Object) _playerUI, (Object) null) || !(_te.WindowGroupToOpen != string.Empty))
      return;
    ((XUiC_PoweredGenericWindowGroup) ((XUiWindowGroup) _playerUI.windowManager.GetWindow(_te.WindowGroupToOpen)).Controller).TileEntity = _te;
    _playerUI.windowManager.Open(_te.WindowGroupToOpen, true);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public bool lootContainerCanOpen(ITileEntityLootable _te, int _entityIdThatOpenedIt)
  {
    if (_te.EntityId != -1)
    {
      Entity entity1 = this.m_World.GetEntity(_te.EntityId);
      if (Object.op_Inequality((Object) entity1, (Object) null) && entity1.spawnById > 0 && entity1.spawnById != _entityIdThatOpenedIt && !entity1.spawnByAllowShare)
      {
        if (TwitchManager.Current.DeniedCrateEvent != "")
        {
          EntityPlayer entity2 = this.m_World.GetEntity(_entityIdThatOpenedIt) as EntityPlayer;
          GameEventManager.Current.HandleAction(TwitchManager.Current.DeniedCrateEvent, entity2, (Entity) entity2, false);
        }
        return false;
      }
    }
    return true;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void lootContainerOpened(
    ITileEntity _te,
    LocalPlayerUI _playerUI,
    int _entityIdThatOpenedIt)
  {
    ITileEntityLootable selfOrFeature = _te.GetSelfOrFeature<ITileEntityLootable>();
    if (selfOrFeature == null)
    {
      Log.Error($"Can not open container UI for TE {_te}");
    }
    else
    {
      FastTags<TagGroup.Global> _containerTags = FastTags<TagGroup.Global>.none;
      if (Object.op_Inequality((Object) _playerUI, (Object) null))
      {
        bool flag = true;
        string _lootContainerName = string.Empty;
        if (selfOrFeature.EntityId != -1)
        {
          Entity entity = this.m_World.GetEntity(selfOrFeature.EntityId);
          if (Object.op_Inequality((Object) entity, (Object) null))
          {
            if (entity.spawnById > 0 && entity.spawnById != _playerUI.entityPlayer.entityId && TwitchManager.Current.StealingCrateEvent != "")
              GameEventManager.Current.HandleAction(TwitchManager.Current.StealingCrateEvent, (EntityPlayer) _playerUI.entityPlayer, (Entity) _playerUI.entityPlayer, false);
            _containerTags = entity.EntityTags;
            _lootContainerName = Localization.Get(entity.EntityName);
            if (entity is EntityVehicle)
              flag = false;
          }
        }
        else
        {
          BlockValue block = this.m_World.GetBlock(selfOrFeature.ToWorldPos());
          _containerTags = block.Block.Tags;
          _lootContainerName = block.Block.GetLocalizedBlockName();
        }
        if (flag)
        {
          ((XUiC_LootWindowGroup) ((XUiWindowGroup) _playerUI.windowManager.GetWindow("looting")).Controller).SetTileEntityChest(_lootContainerName, selfOrFeature);
          _playerUI.windowManager.Open("looting", true);
        }
        LootContainer lootContainer = LootContainer.GetLootContainer(selfOrFeature.lootListName);
        if (lootContainer != null && Object.op_Inequality((Object) _playerUI.entityPlayer, (Object) null))
          lootContainer.ExecuteBuffActions(selfOrFeature.EntityId, (EntityAlive) _playerUI.entityPlayer);
      }
      if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
      {
        this.lootManager.LootContainerOpened(selfOrFeature, _entityIdThatOpenedIt, _containerTags);
        selfOrFeature.bTouched = true;
        selfOrFeature.SetModified();
      }
      else
        this.lootManager.LootContainerOpenedClient(selfOrFeature, _entityIdThatOpenedIt, _containerTags);
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void dewCollectorOpened(
    TileEntityDewCollector _te,
    LocalPlayerUI _playerUI,
    int _entityIdThatOpenedIt)
  {
    if (!Object.op_Inequality((Object) _playerUI, (Object) null))
      return;
    ((XUiC_DewCollectorWindowGroup) ((XUiWindowGroup) _playerUI.windowManager.GetWindow("dewcollector")).Controller).SetTileEntity(_te);
    _playerUI.windowManager.Open("dewcollector", true);
  }

  public void DropContentOfLootContainerServer(
    BlockValue _bvOld,
    Vector3i _worldPos,
    int _lootEntityId,
    ITileEntityLootable _teOld = null)
  {
    if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
    {
      Log.Warning("DropContentOfLootContainerServer can not be called on clients! From:\n" + StackTraceUtility.ExtractStackTrace());
    }
    else
    {
      string str = "DroppedLootContainer";
      FastTags<TagGroup.Global> none = FastTags<TagGroup.Global>.none;
      ITileEntityLootable tileEntityLootable;
      Vector3 vector3;
      if (_lootEntityId == -1)
      {
        tileEntityLootable = _teOld ?? this.m_World.GetTileEntity(_worldPos).GetSelfOrFeature<ITileEntityLootable>();
        if (tileEntityLootable == null || this.lockedTileEntities.ContainsKey((ITileEntity) tileEntityLootable))
          return;
        vector3 = Vector3.op_Addition(tileEntityLootable.ToWorldPos().ToVector3(), new Vector3(0.5f, 0.75f, 0.5f));
        if (_bvOld.Block.Properties.Values.ContainsKey("DroppedEntityClass"))
        {
          FastTags<TagGroup.Global> tags = _bvOld.Block.Tags;
          str = _bvOld.Block.Properties.Values["DroppedEntityClass"];
        }
      }
      else
      {
        Entity entity1 = this.m_World.GetEntity(_lootEntityId);
        if (!Object.op_Implicit((Object) entity1))
          return;
        FastTags<TagGroup.Global> entityTags = entity1.EntityTags;
        vector3 = entity1.GetPosition();
        vector3.y += 0.9f;
        if ((double) entity1.lootDropProb != 0.0)
        {
          EntityLootContainer entity2 = EntityFactory.CreateEntity(EntityClass.list[entity1.entityClass].lootDropEntityClass, vector3, Vector3.zero) as EntityLootContainer;
          this.m_World.SpawnEntityInWorld((Entity) entity2);
          ((Component) entity2).transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
          Manager.BroadcastPlay(vector3, "zpack_spawn");
          return;
        }
        tileEntityLootable = this.m_World.GetTileEntity(_lootEntityId).GetSelfOrFeature<ITileEntityLootable>();
        if (tileEntityLootable == null)
          return;
      }
      if (!tileEntityLootable.bTouched)
        this.lootManager.LootContainerOpened(tileEntityLootable, -1, _bvOld.Block.Tags);
      if (!tileEntityLootable.IsEmpty())
      {
        EntityLootContainer entity = EntityFactory.CreateEntity(str.GetHashCode(), vector3, Vector3.zero) as EntityLootContainer;
        if (Object.op_Inequality((Object) entity, (Object) null))
          entity.SetContent(ItemStack.Clone((IList<ItemStack>) tileEntityLootable.items));
        this.m_World.SpawnEntityInWorld((Entity) entity);
      }
      tileEntityLootable.SetEmpty();
    }
  }

  public EntityLootContainer DropContentInLootContainerServer(
    int _droppedByID,
    string _containerEntity,
    Vector3 _pos,
    ItemStack[] _items,
    bool _skipIfEmpty = false)
  {
    if (_skipIfEmpty && ItemStack.IsEmpty(_items))
      return (EntityLootContainer) null;
    if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
    {
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageDropItemsContainer>().Setup(_droppedByID, _containerEntity, _pos, _items));
      return (EntityLootContainer) null;
    }
    _pos.y += 0.25f;
    EntityLootContainer entity = EntityFactory.CreateEntity(_containerEntity.GetHashCode(), _pos, Vector3.zero) as EntityLootContainer;
    if (Object.op_Implicit((Object) entity))
    {
      entity.SetContent(ItemStack.Clone((IList<ItemStack>) _items));
      entity.spawnById = _droppedByID;
    }
    this.m_World.SpawnEntityInWorld((Entity) entity);
    return entity;
  }

  public GameStateManager GetGameStateManager() => this.gameStateManager;

  public void IdMappingReceived(string _name, byte[] _data)
  {
    Log.Out("Received mapping data for: " + _name);
    switch (_name)
    {
      case "blocks":
        Block.nameIdMapping = new NameIdMapping((string) null, Block.MAX_BLOCKS);
        Block.nameIdMapping.LoadFromArray(_data);
        break;
      case "items":
        ItemClass.nameIdMapping = new NameIdMapping((string) null, ItemClass.MAX_ITEMS);
        ItemClass.nameIdMapping.LoadFromArray(_data);
        break;
      default:
        Log.Warning("Unknown mapping received for: " + _name);
        break;
    }
  }

  public void SetSpawnPointList(SpawnPointList _startPoints)
  {
    this.StartCoroutine(this.setSpawnPointListCo(_startPoints));
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public IEnumerator setSpawnPointListCo(SpawnPointList _startPoints)
  {
    while (!this.chunkClusterLoaded && SingletonMonoBehaviour<ConnectionManager>.Instance.IsConnected)
      yield return (object) null;
    if (this.chunkClusterLoaded)
      this.m_World.ChunkCache.ChunkProvider.SetSpawnPointList(_startPoints);
  }

  public void RequestToSpawnEntityServer(EntityCreationData _ecd)
  {
    if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
    {
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageRequestToSpawnEntity>().Setup(_ecd));
    }
    else
    {
      if (_ecd.entityClass == "fallingTree".GetHashCode())
      {
        for (int index = 0; index < this.m_World.Entities.list.Count; ++index)
        {
          if (this.m_World.Entities.list[index] is EntityFallingTree && ((EntityFallingTree) this.m_World.Entities.list[index]).GetBlockPos() == _ecd.blockPos)
            return;
        }
      }
      Entity entity = EntityFactory.CreateEntity(_ecd);
      if (entity is EntityBackpack entityBackpack)
      {
        foreach (PersistentPlayerData persistentPlayerData in (IEnumerable<PersistentPlayerData>) this.persistentPlayers.Players.Values)
        {
          if (persistentPlayerData.EntityId == entityBackpack.RefPlayerId)
          {
            uint totalMinutes = GameUtils.WorldTimeToTotalMinutes(this.m_World.worldTime);
            persistentPlayerData.AddDroppedBackpack(entity.entityId, new Vector3i(_ecd.pos), totalMinutes);
            break;
          }
        }
      }
      this.m_World.SpawnEntityInWorld(entity);
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void LocalPlayerInventoryChanged()
  {
    this.countdownSendPlayerInventoryToServer.ResetAndRestart();
  }

  public void TriggerSendOfLocalPlayerDataFile(float _sendItInSeconds)
  {
    this.countdownSendPlayerDataFileToServer.SetPassedIn(_sendItInSeconds);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void doSendLocalInventory(EntityPlayerLocal _player)
  {
    if (Object.op_Equality((Object) _player, (Object) null))
      return;
    SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackagePlayerInventory>().Setup(_player, this.sendPlayerToolbelt, this.sendPlayerBag, this.sendPlayerEquipment, this.sendDragAndDropItem));
    this.sendPlayerToolbelt = false;
    this.sendPlayerBag = false;
    this.sendPlayerEquipment = false;
    this.sendDragAndDropItem = false;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void doSendLocalPlayerData(EntityPlayerLocal _player)
  {
    if (Object.op_Equality((Object) _player, (Object) null))
      return;
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
    {
      this.SaveLocalPlayerData();
    }
    else
    {
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackagePlayerData>().Setup((EntityPlayer) _player));
      this.sendPlayerToolbelt = false;
      this.sendPlayerBag = false;
      this.sendPlayerEquipment = false;
      this.sendDragAndDropItem = false;
    }
  }

  public void SetPauseWindowEffects(bool _bOn)
  {
    if (!_bOn || GameModeSurvivalSP.TypeName.Equals(GamePrefs.GetString(EnumGamePrefs.GameMode)))
      return;
    foreach (EntityPlayerLocal localPlayer in this.m_World.GetLocalPlayers())
    {
      if (localPlayer.AimingGun)
        localPlayer.AimingGun = false;
    }
  }

  public static bool ReportUnusedAssets(bool bStart = false)
  {
    if (bStart)
    {
      if (GameManager.materialsBefore == null)
        GameManager.materialsBefore = new List<string>();
      else
        GameManager.materialsBefore.Clear();
      foreach (Object @object in Resources.FindObjectsOfTypeAll<Material>())
        GameManager.materialsBefore.Add(@object.name);
      Resources.UnloadUnusedAssets();
      GC.Collect();
      GameManager.runningAssetsUnused = true;
      GameManager.unusedAssetsTimer = Time.realtimeSinceStartup;
      GameManager.Instance.Pause(true);
    }
    else
    {
      if (GameManager.materialsBefore == null || !GameManager.runningAssetsUnused)
        return true;
      if ((double) Time.realtimeSinceStartup < (double) GameManager.unusedAssetsTimer + 5.0)
        return false;
      Material[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll<Material>();
      if (GameManager.materialsBefore.Count == objectsOfTypeAll.Length)
      {
        Log.Out($"No unused assets found. ( {GameManager.materialsBefore.Count.ToString()} materials found. )");
      }
      else
      {
        Log.Out("Material before: " + GameManager.materialsBefore.Count.ToString());
        Log.Out("Material after: " + objectsOfTypeAll.Length.ToString());
        string str = "Material Diff: ";
        Dictionary<string, int> dictionary = new Dictionary<string, int>();
        for (int index = 0; index < objectsOfTypeAll.Length; ++index)
        {
          int num;
          if (dictionary.TryGetValue(((Object) objectsOfTypeAll[index]).name, out num))
            ++num;
          else
            dictionary.Add(((Object) objectsOfTypeAll[index]).name, 1);
        }
        for (int index = 0; index < GameManager.materialsBefore.Count; ++index)
        {
          if (!dictionary.ContainsKey(GameManager.materialsBefore[index]))
            str = $"{str}{GameManager.materialsBefore[index]}, ";
        }
        Log.Out(str);
      }
      GameManager.Instance.Pause(false);
      GameManager.runningAssetsUnused = false;
    }
    return true;
  }

  public bool IsPaused() => this.gamePaused;

  public void Pause(bool _bOn)
  {
    if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsSinglePlayer || GameModeEditWorld.TypeName.Equals(GamePrefs.GetString(EnumGamePrefs.GameMode)))
      _bOn = false;
    this.SetPauseWindowEffects(_bOn);
    if (_bOn)
    {
      GameStats.Set(EnumGameStats.GameState, 2);
      if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
      {
        this.SaveLocalPlayerData();
        this.SaveWorld();
      }
      Time.timeScale = 0.0f;
      if (Object.op_Inequality((Object) this.World.GetPrimaryPlayer(), (Object) null))
        this.triggerEffectManager.StopGamepadVibration();
    }
    else
    {
      if (GameStats.GetInt(EnumGameStats.GameState) != 0)
        GameStats.Set(EnumGameStats.GameState, 1);
      Time.timeScale = 1f;
    }
    if (this.gamePaused != _bOn)
    {
      if (_bOn)
      {
        Manager.PauseGameplayAudio();
        EnvironmentAudioManager.Instance.Pause();
        this.m_World.dmsConductor.OnPauseGame();
      }
      else
      {
        Manager.UnPauseGameplayAudio();
        EnvironmentAudioManager.Instance.UnPause();
        this.m_World.dmsConductor.OnUnPauseGame();
      }
    }
    this.gamePaused = _bOn;
  }

  public void AddLMPPersistentPlayerData(EntityPlayerLocal _playerEntity)
  {
  }

  public void SetBlockTextureServer(
    Vector3i _blockPos,
    BlockFace _blockFace,
    int _idx,
    int _playerIdThatChanged,
    byte _channel = 255 /*0xFF*/)
  {
    this.SetBlockTextureClient(_blockPos, _blockFace, _idx, _channel);
    NetPackageSetBlockTexture _package = NetPackageManager.GetPackage<NetPackageSetBlockTexture>().Setup(_blockPos, _blockFace, _idx, GameManager.IsDedicatedServer ? -1 : this.myPlayerId, _channel);
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) _package);
    else
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) _package);
  }

  public void SetBlockTextureClient(
    Vector3i _blockPos,
    BlockFace _blockFace,
    int _idx,
    byte _channel)
  {
    DynamicMeshManager.ChunkChanged(_blockPos, -1, 1);
    int num1;
    int num2;
    if (_channel == byte.MaxValue)
    {
      num1 = 0;
      num2 = 0;
    }
    else if (_channel < (byte) 1)
    {
      num1 = num2 = (int) _channel;
    }
    else
    {
      Log.Error($"Specified texture channel \"{_channel}\" is out of range of the project channel count of \"{1}\".");
      return;
    }
    for (int index = num1; index <= num2; ++index)
    {
      if (_blockFace != BlockFace.None)
      {
        this.m_World.ChunkCache.SetBlockFaceTexture(_blockPos, _blockFace, _idx, index);
      }
      else
      {
        long num3 = (long) _idx;
        long _textureFull = num3 | num3 << 8 | num3 << 16 /*0x10*/ | num3 << 24 | num3 << 32 /*0x20*/ | num3 << 40;
        this.m_World.ChunkCache.SetTextureFull(_blockPos, _textureFull, index);
      }
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void handleGlobalActions()
  {
    if (GameManager.IsDedicatedServer)
      return;
    if (((OneAxisInputControl) PlayerActionsGlobal.Instance.Console).WasPressed && !this.m_GUIConsole.isShowing)
      this.windowManager.Open((GUIWindow) this.m_GUIConsole, false);
    if (((OneAxisInputControl) PlayerActionsGlobal.Instance.Fullscreen).WasPressed)
      Screen.fullScreen = !Screen.fullScreen;
    if (((OneAxisInputControl) PlayerActionsGlobal.Instance.Screenshot).WasPressed)
    {
      Manager.PlayButtonClick();
      GameUtils.TakeScreenShot(GameUtils.EScreenshotMode.Both, _isSaveTGA: InputUtils.ControlKeyPressed);
    }
    if (((OneAxisInputControl) PlayerActionsGlobal.Instance.DebugScreenshot).WasPressed)
    {
      Manager.PlayButtonClick();
      LocalPlayerUI.primaryUI.windowManager.Open(GUIWindowScreenshotText.ID, false);
    }
    bool? nullable1;
    if (Object.op_Inequality((Object) LocalPlayerUI.primaryUI, (Object) null))
    {
      nullable1 = ((OneAxisInputControl) PlatformManager.NativePlatform?.Input.PrimaryPlayer?.GUIActions?.FocusSearch)?.WasPressed;
      if (nullable1.GetValueOrDefault())
        XUiC_TextInput.SelectCurrentSearchField(LocalPlayerUI.primaryUI);
    }
    LocalPlayerUI forPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
    if (!Object.op_Inequality((Object) forPrimaryPlayer, (Object) null))
      return;
    PlayerActionsLocal playerInput = forPrimaryPlayer.playerInput;
    bool? nullable2;
    if (playerInput == null)
    {
      nullable1 = new bool?();
      nullable2 = nullable1;
    }
    else
    {
      PlayerActionsGUI guiActions = playerInput.GUIActions;
      if (guiActions == null)
      {
        nullable1 = new bool?();
        nullable2 = nullable1;
      }
      else
      {
        PlayerAction focusSearch = guiActions.FocusSearch;
        if (focusSearch == null)
        {
          nullable1 = new bool?();
          nullable2 = nullable1;
        }
        else
          nullable2 = new bool?(((OneAxisInputControl) focusSearch).WasPressed);
      }
    }
    nullable1 = nullable2;
    if (!nullable1.GetValueOrDefault())
      return;
    XUiC_TextInput.SelectCurrentSearchField(forPrimaryPlayer);
  }

  public void SetConsoleWindowVisible(bool _b)
  {
    if (_b)
    {
      if (this.m_GUIConsole.isShowing)
        return;
      this.windowManager.Open((GUIWindow) this.m_GUIConsole, false);
    }
    else
      this.windowManager.Close((GUIWindow) this.m_GUIConsole);
  }

  public static bool IsSplatMapAvailable()
  {
    string str = GamePrefs.GetString(EnumGamePrefs.GameWorld);
    return !(str == "Empty") && !(str == "Playtesting");
  }

  public static bool UpdatingRemoteResources
  {
    get => GameManager.\u003CUpdatingRemoteResources\u003Ek__BackingField;
    [PublicizedFrom(EAccessModifier.Private)] set
    {
      GameManager.\u003CUpdatingRemoteResources\u003Ek__BackingField = value;
    }
  }

  public static bool RemoteResourcesLoaded
  {
    get => GameManager.\u003CRemoteResourcesLoaded\u003Ek__BackingField;
    [PublicizedFrom(EAccessModifier.Private)] set
    {
      GameManager.\u003CRemoteResourcesLoaded\u003Ek__BackingField = value;
    }
  }

  public static void LoadRemoteResources(
    GameManager.RemoteResourcesCompleteHandler _callback = null)
  {
    if (GameManager.UpdatingRemoteResources)
      return;
    NewsManager.Instance.UpdateNews();
    if (BlockedPlayerList.Instance != null)
      GameManager.Instance.StartCoroutine(BlockedPlayerList.Instance.ReadStorageAndResolve());
    DLCTitleStorageManager.Instance.FetchFromSource();
    if (PlatformManager.NativePlatform.User.UserStatus == EUserStatus.LoggedIn)
    {
      GameManager.Instance.StartCoroutine(GameManager.Instance.UpdateRemoteResourcesRoutine(_callback));
    }
    else
    {
      GameManager.UpdatingRemoteResources = false;
      GameManager.RemoteResourcesLoaded = true;
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public IEnumerator UpdateRemoteResourcesRoutine(
    GameManager.RemoteResourcesCompleteHandler _callback)
  {
    GameManager gameManager = this;
    IRemoteFileStorage storage = PlatformManager.MultiPlatform.RemoteFileStorage;
    if (storage == null)
    {
      GameManager.RemoteResourcesLoaded = true;
    }
    else
    {
      GameManager.UpdatingRemoteResources = true;
      float readyTime = Time.time;
      while (!storage.IsReady)
      {
        yield return (object) null;
        if ((double) Time.time - (double) readyTime > 3.0)
        {
          Log.Warning("Waiting for remote resources timed out");
          GameManager.UpdatingRemoteResources = false;
          GameManager.RemoteResourcesLoaded = true;
          yield break;
        }
      }
      gameManager.retrievingEula = true;
      storage.GetFile($"eula_{Localization.language.ToLower()}", new IRemoteFileStorage.FileDownloadCompleteCallback(gameManager.EulaProviderCallback));
      while (gameManager.retrievingEula)
        yield return (object) null;
      if (BacktraceUtils.Initialized)
      {
        gameManager.retrievingBacktraceConfig = true;
        storage.GetFile("backtraceconfig.xml", new IRemoteFileStorage.FileDownloadCompleteCallback(gameManager.BacktraceConfigProviderCallback));
        while (gameManager.retrievingBacktraceConfig)
          yield return (object) null;
      }
      GameManager.UpdatingRemoteResources = false;
      GameManager.RemoteResourcesLoaded = true;
      if (_callback != null)
        _callback();
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void EulaProviderCallback(
    IRemoteFileStorage.EFileDownloadResult _result,
    string _errorDetails,
    byte[] _data)
  {
    this.retrievingEula = false;
    if (_result != IRemoteFileStorage.EFileDownloadResult.Ok)
    {
      Log.Warning($"Retrieving EULA file failed: {_result.ToStringCached<IRemoteFileStorage.EFileDownloadResult>()} ({_errorDetails})");
    }
    else
    {
      string contents;
      if (!this.LoadEulaXML(_data, out contents))
        return;
      XUiC_EulaWindow.retrievedEula = contents;
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public bool LoadEulaXML(byte[] _data, out string contents)
  {
    contents = "";
    XmlFile xmlFile;
    try
    {
      xmlFile = new XmlFile(_data, true);
    }
    catch (Exception ex)
    {
      Log.Error("Failed loading EULA XML: {0}", new object[1]
      {
        (object) ex.Message
      });
      return false;
    }
    XElement root = xmlFile.XmlDoc.Root;
    if (root == null)
      return false;
    int num = int.Parse(root.GetAttribute((XName) "version").Trim());
    contents = root.Value;
    if (num > GamePrefs.GetInt(EnumGamePrefs.EulaLatestVersion))
      GamePrefs.Set(EnumGamePrefs.EulaLatestVersion, num);
    return true;
  }

  public static bool HasAcceptedLatestEula()
  {
    return GamePrefs.GetInt(EnumGamePrefs.EulaVersionAccepted) >= GamePrefs.GetInt(EnumGamePrefs.EulaLatestVersion);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void BacktraceConfigProviderCallback(
    IRemoteFileStorage.EFileDownloadResult _result,
    string _errorDetails,
    byte[] _data)
  {
    this.retrievingBacktraceConfig = false;
    if (_result != IRemoteFileStorage.EFileDownloadResult.Ok)
    {
      Log.Warning($"Retrieving Backtrace config file failed: {_result.ToStringCached<IRemoteFileStorage.EFileDownloadResult>()} ({_errorDetails})");
    }
    else
    {
      try
      {
        BacktraceUtils.UpdateConfig(new XmlFile(_data, true));
      }
      catch (Exception ex)
      {
        Log.Error("Failed loading Backtrace config XML: {0}", new object[1]
        {
          (object) ex.Message
        });
      }
    }
  }

  public bool IsGoreCensored() => GameManager.DebugCensorship;

  public int persistentPlayerCount
  {
    [PublicizedFrom(EAccessModifier.Private)] get => this.persistentPlayerIds.Count;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void CalculatePersistentPlayerCount(string worldName, string saveName)
  {
    this.persistentPlayerIds = new List<string>();
    string path = GameIO.GetSaveGameDir(worldName, saveName) + "/Player";
    if (!SdDirectory.Exists(path))
    {
      Log.Warning("save folder does not exist");
    }
    else
    {
      foreach (SdFileSystemInfo fileSystemInfo in new SdDirectoryInfo(path).GetFileSystemInfos())
      {
        int length;
        string str = (length = fileSystemInfo.Name.IndexOf('.')) == -1 ? fileSystemInfo.Name : fileSystemInfo.Name.Substring(0, length);
        if (!this.persistentPlayerIds.Contains(str))
          this.persistentPlayerIds.Add(str);
      }
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  static GameManager()
  {
  }

  [CompilerGenerated]
  [PublicizedFrom(EAccessModifier.Private)]
  public void \u003CloadStaticData\u003Eb__129_0(string _progressText, float _percentage)
  {
    this.CurrentLoadAction = _progressText;
  }

  [CompilerGenerated]
  [PublicizedFrom(EAccessModifier.Private)]
  public void \u003CsetLocalPlayerEntity\u003Eb__159_0() => this.sendPlayerToolbelt = true;

  [CompilerGenerated]
  [PublicizedFrom(EAccessModifier.Private)]
  public void \u003CsetLocalPlayerEntity\u003Eb__159_1() => this.sendPlayerBag = true;

  [CompilerGenerated]
  [PublicizedFrom(EAccessModifier.Private)]
  public void \u003CsetLocalPlayerEntity\u003Eb__159_2() => this.sendPlayerEquipment = true;

  [CompilerGenerated]
  [PublicizedFrom(EAccessModifier.Private)]
  public void \u003CsetLocalPlayerEntity\u003Eb__159_3() => this.sendDragAndDropItem = true;

  [CompilerGenerated]
  [PublicizedFrom(EAccessModifier.Private)]
  public IMapChunkDatabase.DirectoryPlayerId \u003CPlayerId\u003Eb__205_0()
  {
    string combinedString = this.getPersistentPlayerID((ClientInfo) null).CombinedString;
    return new IMapChunkDatabase.DirectoryPlayerId(GameIO.GetPlayerDataLocalDir(), combinedString);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public struct BlockParticleCreationData(Vector3i _blockPos, ParticleEffect _particleEffect)
  {
    public Vector3i blockPos = _blockPos;
    public ParticleEffect particleEffect = _particleEffect;
  }

  public delegate void OnWorldChangedEvent(World _world);

  public delegate void OnLocalPlayerChangedEvent(EntityPlayerLocal _localPlayer);

  [PublicizedFrom(EAccessModifier.Private)]
  public enum EMultiShutReason
  {
    AppNoNetwork,
    AppSuspended,
    PermMissingMultiplayer,
    PermMissingCrossplay,
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public class ExplodeGroup
  {
    public Vector3 pos;
    public float radius;
    public int delay;
    public List<GameManager.ExplodeGroup.Falling> fallings = new List<GameManager.ExplodeGroup.Falling>();

    public struct Falling
    {
      public Vector3i pos;
      public BlockValue bv;
    }
  }

  public class EntityItemLifetimeComparer : IComparer<EntityItem>
  {
    public int Compare(EntityItem _obj1, EntityItem _obj2)
    {
      return (int) ((double) _obj2.lifetime - (double) _obj1.lifetime);
    }
  }

  public delegate void RemoteResourcesCompleteHandler();
}
