// Decompiled with JetBrains decompiler
// Type: World
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using Audio;
using DynamicMusic;
using DynamicMusic.Factories;
using GamePath;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml;
using UnityEngine;

#nullable disable
public class World : WorldBase, IBlockAccess, IChunkAccess, IChunkCallback
{
  public const int cCollisionBlocks = 5;
  public ulong worldTime;
  public int DawnHour;
  public int DuskHour;
  public float Gravity = 0.08f;
  public DictionaryList<int, Entity> Entities = new DictionaryList<int, Entity>();
  public DictionaryList<int, EntityPlayer> Players = new DictionaryList<int, EntityPlayer>();
  public List<EntityAlive> EntityAlives = new List<EntityAlive>();
  public NetEntityDistribution entityDistributer;
  public AIDirector aiDirector;
  public Manager audioManager;
  public Conductor dmsConductor;
  public IGameManager gameManager;
  public int Seed;
  public WorldBiomes Biomes;
  public SpawnManagerBiomes biomeSpawnManager;
  public BiomeIntensity LocalPlayerBiomeIntensityStandingOn = BiomeIntensity.Default;
  public WorldCreationData wcd;
  [PublicizedFrom(EAccessModifier.Private)]
  public WorldState worldState;
  public ChunkManager m_ChunkManager;
  public SharedChunkObserverCache m_SharedChunkObserverCache;
  public WorldEnvironment m_WorldEnvironment;
  public BiomeAtmosphereEffects BiomeAtmosphereEffects;
  public FlatAreaManager FlatAreaManager;
  public static bool IsSplatMapAvailable;
  public List<SSpawnedEntity> Last4Spawned = new List<SSpawnedEntity>();
  public int playerEntityUpdateCount;
  public int clientLastEntityId;
  public Transform EntitiesTransform;
  [PublicizedFrom(EAccessModifier.Private)]
  public GameRandom rand;
  [PublicizedFrom(EAccessModifier.Private)]
  public bool isUnityTerrainConfigured;
  [PublicizedFrom(EAccessModifier.Private)]
  public List<EntityPlayerLocal> m_LocalPlayerEntities = new List<EntityPlayerLocal>();
  [PublicizedFrom(EAccessModifier.Private)]
  public EntityPlayerLocal m_LocalPlayerEntity;
  [PublicizedFrom(EAccessModifier.Private)]
  public List<Entity> entitiesWithinAABBExcludingEntity = new List<Entity>();
  [PublicizedFrom(EAccessModifier.Private)]
  public List<EntityAlive> livingEntitiesWithinAABBExcludingEntity = new List<EntityAlive>();
  [PublicizedFrom(EAccessModifier.Private)]
  public MapObjectManager objectsOnMap;
  [PublicizedFrom(EAccessModifier.Private)]
  public SpawnManagerDynamic dynamicSpawnManager;
  [PublicizedFrom(EAccessModifier.Private)]
  public WorldBlockTicker worldBlockTicker;
  [PublicizedFrom(EAccessModifier.Private)]
  public List<SleeperVolume> sleeperVolumes = new List<SleeperVolume>();
  [PublicizedFrom(EAccessModifier.Private)]
  public Dictionary<Vector3i, List<int>> sleeperVolumeMap = new Dictionary<Vector3i, List<int>>();
  [PublicizedFrom(EAccessModifier.Private)]
  public List<TriggerVolume> triggerVolumes = new List<TriggerVolume>();
  [PublicizedFrom(EAccessModifier.Private)]
  public Dictionary<Vector3i, List<int>> triggerVolumeMap = new Dictionary<Vector3i, List<int>>();
  [PublicizedFrom(EAccessModifier.Private)]
  public List<WallVolume> wallVolumes = new List<WallVolume>();
  [PublicizedFrom(EAccessModifier.Private)]
  public Dictionary<Vector3i, List<int>> wallVolumeMap = new Dictionary<Vector3i, List<int>>();
  [PublicizedFrom(EAccessModifier.Private)]
  public Dictionary<Vector3i, object> blockData = new Dictionary<Vector3i, object>();
  [PublicizedFrom(EAccessModifier.Private)]
  public List<long> newlyLoadedChunksThisUpdate = new List<long>();
  [PublicizedFrom(EAccessModifier.Private)]
  public Dictionary<long, ulong> areaMasterChunksToLock = new Dictionary<long, ulong>();
  public TriggerManager triggerManager;
  [PublicizedFrom(EAccessModifier.Private)]
  public static int[][] supportOrder = new int[8][]
  {
    new int[8]{ 0, 7, 1, 6, 2, 4, 3, 5 },
    new int[8]{ 0, 2, 1, 7, 3, 4, 6, 5 },
    new int[8]{ 2, 1, 3, 0, 4, 6, 5, 7 },
    new int[8]{ 2, 4, 3, 1, 5, 6, 0, 7 },
    new int[8]{ 4, 3, 5, 2, 6, 0, 7, 1 },
    new int[8]{ 4, 6, 5, 3, 7, 0, 2, 1 },
    new int[8]{ 6, 5, 7, 4, 0, 2, 1, 3 },
    new int[8]{ 6, 0, 7, 5, 1, 2, 4, 3 }
  };
  [PublicizedFrom(EAccessModifier.Private)]
  public static int[] supportOffsets = new int[16 /*0x10*/]
  {
    0,
    1,
    1,
    1,
    1,
    0,
    1,
    -1,
    0,
    -1,
    -1,
    -1,
    -1,
    0,
    -1,
    1
  };
  [PublicizedFrom(EAccessModifier.Private)]
  public MicroStopwatch msUnculling = new MicroStopwatch();
  [PublicizedFrom(EAccessModifier.Private)]
  public HashSetList<Chunk> chunksToUncull = new HashSetList<Chunk>();
  [PublicizedFrom(EAccessModifier.Private)]
  public HashSetList<Chunk> chunksToRegenerate = new HashSetList<Chunk>();
  [PublicizedFrom(EAccessModifier.Private)]
  public World.ClipBlock[] _clipBlocks = new World.ClipBlock[32 /*0x20*/];
  [PublicizedFrom(EAccessModifier.Private)]
  public Bounds[] _clipBounds = new Bounds[16 /*0x10*/];
  [PublicizedFrom(EAccessModifier.Private)]
  public const int cCollCacheSize = 50;
  [PublicizedFrom(EAccessModifier.Private)]
  public BlockValue[,,] collBlockCache = new BlockValue[50, 50, 50];
  [PublicizedFrom(EAccessModifier.Private)]
  public sbyte[,,] collDensityCache = new sbyte[50, 50, 50];
  [PublicizedFrom(EAccessModifier.Private)]
  public int tickEntityFrameCount;
  [PublicizedFrom(EAccessModifier.Private)]
  public float tickEntityFrameCountAverage = 1f;
  [PublicizedFrom(EAccessModifier.Private)]
  public List<Entity> tickEntityList = new List<Entity>();
  [PublicizedFrom(EAccessModifier.Private)]
  public float tickEntityPartialTicks;
  [PublicizedFrom(EAccessModifier.Private)]
  public int tickEntityIndex;
  [PublicizedFrom(EAccessModifier.Private)]
  public int tickEntitySliceCount;
  [PublicizedFrom(EAccessModifier.Protected)]
  public Queue<Vector3i> fallingBlocks = new Queue<Vector3i>();
  [PublicizedFrom(EAccessModifier.Protected)]
  public Dictionary<Vector3i, float> fallingBlocksMap = new Dictionary<Vector3i, float>();
  [PublicizedFrom(EAccessModifier.Private)]
  public List<Chunk> m_lpChunkList = new List<Chunk>();
  public const float cEdgeHard = 50f;
  public const float cEdgeSoft = 80f;
  [PublicizedFrom(EAccessModifier.Private)]
  public const int cEdgeMinWorldSize = 1024 /*0x0400*/;
  [PublicizedFrom(EAccessModifier.Private)]
  public static List<TraderArea> traderAreas;
  [PublicizedFrom(EAccessModifier.Private)]
  public const int cTraderPlacingProtection = 2;
  public bool isEventBloodMoon;
  public ulong eventWorldTime;
  public int WorldDay;
  public int WorldHour;
  [PublicizedFrom(EAccessModifier.Protected)]
  public HashSet<Vector3i> pendingUpgradeDowngradeBlocks = new HashSet<Vector3i>();

  public event World.OnEntityLoadedDelegate EntityLoadedDelegates;

  public event World.OnEntityUnloadedDelegate EntityUnloadedDelegates;

  public event World.OnWorldChangedEvent OnWorldChanged;

  public virtual void Init(IGameManager _gameManager, WorldBiomes _biomes)
  {
    this.gameManager = _gameManager;
    this.m_ChunkManager = new ChunkManager();
    this.m_ChunkManager.Init(this);
    this.m_SharedChunkObserverCache = new SharedChunkObserverCache(this.m_ChunkManager, 3, (IThreadingSemantics) new NoThreadingSemantics());
    LightManager.Init();
    this.triggerManager = new TriggerManager();
    this.Biomes = _biomes;
    if (_biomes != null)
      this.biomeSpawnManager = new SpawnManagerBiomes(this);
    this.audioManager = Manager.Instance;
    this.BiomeAtmosphereEffects = new BiomeAtmosphereEffects();
    this.BiomeAtmosphereEffects.Init(this);
  }

  public IEnumerator LoadWorld(string _sWorldName, bool _fixedSizeCC = false)
  {
    World _world = this;
    Log.Out("World.Load: " + _sWorldName);
    GamePrefs.Set(EnumGamePrefs.GameWorld, _sWorldName);
    World.IsSplatMapAvailable = GameManager.IsSplatMapAvailable();
    _world.DuskDawnInit();
    _world.wcd = new WorldCreationData(GameIO.GetWorldDir());
    _world.worldState = new WorldState();
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && _sWorldName != null)
    {
      string str;
      if (_world.IsEditor())
      {
        str = PathAbstractions.WorldsSearchPaths.GetLocation(_sWorldName).FullPath + "/main.ttw";
      }
      else
      {
        str = GameIO.GetSaveGameDir() + "/main.ttw";
        if (!SdFile.Exists(str))
        {
          if (!SdDirectory.Exists(GameIO.GetSaveGameDir()))
            SdDirectory.CreateDirectory(GameIO.GetSaveGameDir());
          Log.Out("Loading base world file header...");
          _world.worldState.Load(GameIO.GetWorldDir() + "/main.ttw", false, true);
          _world.worldState.GenerateNewGuid();
          _world.Seed = GamePrefs.GetString(EnumGamePrefs.GameName).GetHashCode();
          _world.worldState.SetFrom(_world, _world.worldState.providerId);
          _world.worldState.worldTime = 7000UL;
          _world.worldState.saveDataLimit = SaveDataLimit.GetLimitFromPref();
          _world.worldState.Save(str);
        }
      }
      if (!_world.worldState.Load(str, _makeExtraBackupOnSuccess: !_world.IsEditor()))
        Log.Error($"Could not load file '{str}'!");
      else
        _world.Seed = _world.worldState.seed;
    }
    _world.wcd.Apply(_world, _world.worldState);
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
      _world.Seed = GamePrefs.GetString(EnumGamePrefs.GameNameClient).GetHashCode();
    GameRandomManager.Instance.SetBaseSeed(_world.Seed);
    _world.rand = GameRandomManager.Instance.CreateGameRandom();
    _world.rand.SetLock();
    _world.worldTime = !_world.IsEditor() ? _world.worldState.worldTime : 12000UL;
    GameTimer.Instance.ticks = _world.worldState.timeInTicks;
    EntityFactory.nextEntityID = _world.worldState.nextEntityID;
    if (PlatformOptimizations.LimitedSaveData && _world.worldState.saveDataLimit < 0L)
      SaveDataLimit.SetLimitToPref(SaveDataLimitType.VeryLong.CalculateTotalSize(GameUtils.WorldInfo.LoadWorldInfo(PathAbstractions.WorldsSearchPaths.GetLocation(_sWorldName)).WorldSize));
    else
      SaveDataLimit.SetLimitToPref(_world.worldState.saveDataLimit);
    _world.clientLastEntityId = -2;
    if (_sWorldName != null)
    {
      _world.EntitiesTransform = GameObject.Find("/Entities").transform;
      EntityFactory.Init(_world.EntitiesTransform);
    }
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
    {
      _world.dynamicSpawnManager = new SpawnManagerDynamic(_world, (XmlDocument) null);
      if (_world.worldState.dynamicSpawnerState != null && _world.worldState.dynamicSpawnerState.Length > 0L)
      {
        using (PooledBinaryReader _br = MemoryPools.poolBinaryReader.AllocSync(false))
        {
          _br.SetBaseStream((Stream) _world.worldState.dynamicSpawnerState);
          _world.dynamicSpawnManager.Read((BinaryReader) _br);
        }
      }
      _world.entityDistributer = new NetEntityDistribution(_world, 0);
      _world.worldBlockTicker = new WorldBlockTicker(_world);
      _world.aiDirector = new AIDirector(_world);
      if (_world.worldState.aiDirectorState != null)
      {
        using (PooledBinaryReader stream = MemoryPools.poolBinaryReader.AllocSync(false))
        {
          stream.SetBaseStream((Stream) _world.worldState.aiDirectorState);
          _world.aiDirector.Load((BinaryReader) stream);
        }
      }
      if (_world.worldState.sleeperVolumeState != null)
      {
        using (PooledBinaryReader _br = MemoryPools.poolBinaryReader.AllocSync(false))
        {
          _br.SetBaseStream((Stream) _world.worldState.sleeperVolumeState);
          _world.ReadSleeperVolumes((BinaryReader) _br);
        }
      }
      else
        _world.sleeperVolumes.Clear();
      if (_world.worldState.triggerVolumeState != null)
      {
        using (PooledBinaryReader _br = MemoryPools.poolBinaryReader.AllocSync(false))
        {
          _br.SetBaseStream((Stream) _world.worldState.triggerVolumeState);
          _world.ReadTriggerVolumes((BinaryReader) _br);
        }
      }
      else
        _world.triggerVolumes.Clear();
      if (_world.worldState.wallVolumeState != null)
      {
        using (PooledBinaryReader _br = MemoryPools.poolBinaryReader.AllocSync(false))
        {
          _br.SetBaseStream((Stream) _world.worldState.wallVolumeState);
          _world.ReadWallVolumes((BinaryReader) _br);
        }
      }
      else
        _world.wallVolumes.Clear();
      SleeperVolume.WorldInit();
    }
    DecoManager.Instance.IsEnabled = _sWorldName != "Empty";
    yield return (object) null;
    ChunkCluster cc = (ChunkCluster) null;
    yield return (object) _world.CreateChunkCluster(SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? _world.worldState.providerId : EnumChunkProviderId.NetworkClient, GamePrefs.GetString(EnumGamePrefs.GameWorld), 0, _fixedSizeCC, (Action<ChunkCluster>) ([PublicizedFrom(EAccessModifier.Internal)] (_cluster) => cc = _cluster));
    yield return (object) null;
    XUiC_ProgressWindow.SetText(LocalPlayerUI.primaryUI, Localization.Get("uiLoadWorldEnvironment"));
    string typeName = "WorldEnvironment";
    if (_world.wcd.Properties.Values.ContainsKey("WorldEnvironment.Class"))
      typeName = _world.wcd.Properties.Values["WorldEnvironment.Class"];
    GameObject gameObject = new GameObject("WorldEnvironment");
    _world.m_WorldEnvironment = gameObject.AddComponent(System.Type.GetType(typeName)) as WorldEnvironment;
    _world.m_WorldEnvironment.Init(_world.wcd, _world);
    DynamicPrefabDecorator dynamicPrefabDecorator = cc.ChunkProvider.GetDynamicPrefabDecorator();
    SelectionBoxManager.Instance.GetCategory("DynamicPrefabs").SetCallback((ISelectionBoxCallback) dynamicPrefabDecorator);
    if (GameManager.Instance.IsEditMode() && !PrefabEditModeManager.Instance.IsActive())
      SelectionBoxManager.Instance.GetCategory("DynamicPrefabs").SetVisible(true);
    if (DecoManager.Instance.IsEnabled)
    {
      IChunkProvider chunkProvider = _world.ChunkCache.ChunkProvider;
      yield return (object) DecoManager.Instance.OnWorldLoaded(chunkProvider.GetWorldSize().x, chunkProvider.GetWorldSize().y, _world, chunkProvider);
      _world.m_WorldEnvironment.CreateUnityTerrain();
    }
    if (!_world.IsEditor())
    {
      (_world.dmsConductor = Factory.CreateConductor()).Init(true);
      if (!GameManager.IsDedicatedServer)
        yield return (object) _world.dmsConductor.PreloadRoutine();
    }
    _world.SetupTraders();
    _world.SetupSleeperVolumes();
    _world.SetupTriggerVolumes();
    _world.SetupWallVolumes();
    if (!GameManager.IsDedicatedServer && GameManager.IsSplatMapAvailable())
    {
      if (UnityDistantTerrainTest.Instance == null)
        UnityDistantTerrainTest.Create();
      if (!_world.isUnityTerrainConfigured)
      {
        _world.isUnityTerrainConfigured = true;
        ChunkProviderGenerateWorldFromRaw chunkProvider = _world.ChunkCache.ChunkProvider as ChunkProviderGenerateWorldFromRaw;
        UnityDistantTerrainTest instance = UnityDistantTerrainTest.Instance;
        if (chunkProvider != null)
        {
          instance.HeightMap = chunkProvider.heightData;
          instance.hmWidth = chunkProvider.GetWorldSize().x;
          instance.hmHeight = chunkProvider.GetWorldSize().y;
          instance.TerrainMaterial = MeshDescription.meshes[5].materialDistant;
          instance.TerrainMaterial.renderQueue = 2490;
          instance.WaterMaterial = MeshDescription.meshes[1].materialDistant;
          instance.WaterMaterial.SetVector("_WorldDim", new Vector4((float) chunkProvider.GetWorldSize().x, (float) chunkProvider.GetWorldSize().y, 0.0f, 0.0f));
          chunkProvider.GetWaterChunks16x16(out instance.WaterChunks16x16Width, out instance.WaterChunks16x16);
          instance.LoadTerrain();
        }
      }
    }
    if (_world.OnWorldChanged != null)
      _world.OnWorldChanged(_sWorldName);
  }

  public void Save()
  {
    this.worldState.SetFrom(this, this.ChunkCache.ChunkProvider.GetProviderId());
    if (this.IsEditor())
    {
      this.worldState.ResetDynamicData();
      this.worldState.nextEntityID = 171;
      this.worldState.Save(PathAbstractions.WorldsSearchPaths.GetLocation(GamePrefs.GetString(EnumGamePrefs.GameWorld)).FullPath + "/main.ttw");
    }
    else
      this.worldState.Save(GameIO.GetSaveGameDir() + "/main.ttw");
    for (int _idx = 0; _idx < this.ChunkClusters.Count; ++_idx)
      this.ChunkClusters[_idx]?.Save();
    this.SaveDecorations();
    SaveDataUtils.SaveDataManager.CommitAsync();
  }

  public void SaveDecorations() => DecoManager.Instance.Save();

  public void SaveWorldState()
  {
    this.worldState.SetFrom(this, this.ChunkCache.ChunkProvider.GetProviderId());
    this.worldState.Save(GameIO.GetSaveGameDir() + "/main.ttw");
  }

  public virtual void UnloadWorld(bool _bUnloadRespawnableEntities)
  {
    Log.Out("World.Unload");
    if (Object.op_Inequality((Object) this.m_WorldEnvironment, (Object) null))
    {
      this.m_WorldEnvironment.Cleanup();
      Object.Destroy((Object) ((Component) this.m_WorldEnvironment).gameObject);
      this.m_WorldEnvironment = (WorldEnvironment) null;
    }
    this.ChunkCache = (ChunkCluster) null;
    this.ChunkClusters.Cleanup();
    this.UnloadEntities(this.Entities.list, true);
    EntityFactory.Cleanup();
    if (BlockToolSelection.Instance != null)
      BlockToolSelection.Instance.SelectionActive = false;
    SelectionBoxManager.Instance.GetCategory("DynamicPrefabs").Clear();
    SelectionBoxManager.Instance.GetCategory("TraderTeleport").Clear();
    SelectionBoxManager.Instance.GetCategory("SleeperVolume").Clear();
    SelectionBoxManager.Instance.GetCategory("TriggerVolume").Clear();
    DecoManager.Instance.OnWorldUnloaded();
    Block.OnWorldUnloaded();
    if (UnityDistantTerrainTest.Instance == null)
      return;
    UnityDistantTerrainTest.Instance.Cleanup();
    this.isUnityTerrainConfigured = false;
  }

  public virtual void Cleanup()
  {
    Log.Out("World.Cleanup");
    if (this.m_ChunkManager != null)
    {
      this.m_ChunkManager.Cleanup();
      this.m_ChunkManager = (ChunkManager) null;
    }
    if (this.audioManager != null)
    {
      this.audioManager.Dispose();
      Manager.CleanUp();
      this.audioManager = (Manager) null;
    }
    if (this.dmsConductor != null)
    {
      this.dmsConductor.CleanUp();
      this.dmsConductor.OnWorldExit();
      this.dmsConductor = (Conductor) null;
    }
    LightManager.Dispose();
    for (int index = 0; index < this.Entities.list.Count; ++index)
      Object.Destroy((Object) ((Component) this.Entities.list[index].RootTransform).gameObject);
    this.Entities.Clear();
    this.EntityAlives.Clear();
    if (this.Biomes != null)
      this.Biomes.Cleanup();
    if (this.entityDistributer != null)
    {
      this.entityDistributer.Cleanup();
      this.entityDistributer = (NetEntityDistribution) null;
    }
    if (this.biomeSpawnManager != null)
    {
      this.biomeSpawnManager.Cleanup();
      this.biomeSpawnManager = (SpawnManagerBiomes) null;
    }
    this.dynamicSpawnManager = (SpawnManagerDynamic) null;
    if (this.worldBlockTicker != null)
    {
      this.worldBlockTicker.Cleanup();
      this.worldBlockTicker = (WorldBlockTicker) null;
    }
    BlockShapeNew.Cleanup();
    this.Biomes = (WorldBiomes) null;
    if (this.objectsOnMap != null)
      this.objectsOnMap.Clear();
    this.m_LocalPlayerEntity = (EntityPlayerLocal) null;
    this.aiDirector = (AIDirector) null;
    PathFinderThread.Instance = (PathFinderThread) null;
    this.wcd = (WorldCreationData) null;
    this.worldState = (WorldState) null;
    this.BiomeAtmosphereEffects = (BiomeAtmosphereEffects) null;
    DynamicMeshUnity.ClearCachedDynamicMeshChunksList();
    if (this.FlatAreaManager == null)
      return;
    this.FlatAreaManager.Cleanup();
  }

  public void ClearCaches()
  {
    this.m_ChunkManager.FreePools();
    PathPoint.CompactPool();
    for (int _idx = 0; _idx < this.ChunkClusters.Count; ++_idx)
      this.ChunkClusters[_idx]?.ChunkProvider.ClearCaches();
  }

  public string Guid => this.worldState != null ? this.worldState.Guid : (string) null;

  public long GetNextChunkToProvide() => this.m_ChunkManager.GetNextChunkToProvide();

  public virtual IEnumerator CreateChunkCluster(
    EnumChunkProviderId _chunkProviderId,
    string _clusterName,
    int _forceClrIdx,
    bool _bFixedSize,
    Action<ChunkCluster> _resultHandler)
  {
    World world = this;
    ChunkCluster cc = new ChunkCluster(world, _clusterName, world.ChunkClusters.LayerMappingTable[0]);
    if (_forceClrIdx != -1)
    {
      world.ChunkClusters.AddFixed(cc, _forceClrIdx);
      world.ChunkCache = world.ChunkClusters.Cluster0;
    }
    cc.IsFixedSize = _bFixedSize;
    cc.AddChunkCallback((IChunkCallback) world);
    WaterSimulationNative.Instance.Init(cc);
    yield return (object) cc.Init(_chunkProviderId);
    _resultHandler(cc);
  }

  public override void AddLocalPlayer(EntityPlayerLocal _localPlayer)
  {
    if (!this.m_LocalPlayerEntities.Contains(_localPlayer))
      this.m_LocalPlayerEntities.Add(_localPlayer);
    if (this.objectsOnMap != null)
      return;
    this.objectsOnMap = new MapObjectManager();
  }

  public override void RemoveLocalPlayer(EntityPlayerLocal _localPlayer)
  {
    this.m_LocalPlayerEntities.Remove(_localPlayer);
  }

  public override List<EntityPlayerLocal> GetLocalPlayers() => this.m_LocalPlayerEntities;

  public override bool IsLocalPlayer(int _playerId)
  {
    Entity entity = this.GetEntity(_playerId);
    return Object.op_Inequality((Object) entity, (Object) null) && entity is EntityPlayerLocal;
  }

  public override EntityPlayerLocal GetLocalPlayerFromID(int _playerId)
  {
    return this.GetEntity(_playerId) as EntityPlayerLocal;
  }

  public override EntityPlayerLocal GetClosestLocalPlayer(Vector3 _position)
  {
    EntityPlayerLocal closestLocalPlayer = this.GetPrimaryPlayer();
    if (this.m_LocalPlayerEntities.Count > 1)
    {
      float num = float.MaxValue;
      for (int index = 0; index < this.m_LocalPlayerEntities.Count; ++index)
      {
        Vector3 vector3 = Vector3.op_Subtraction(this.m_LocalPlayerEntities[index].GetPosition(), _position);
        float sqrMagnitude = ((Vector3) ref vector3).sqrMagnitude;
        if ((double) sqrMagnitude < (double) num)
        {
          num = sqrMagnitude;
          closestLocalPlayer = this.m_LocalPlayerEntities[index];
        }
      }
    }
    return closestLocalPlayer;
  }

  public override Vector3 GetVectorToClosestLocalPlayer(Vector3 _position)
  {
    return Vector3.op_Subtraction(this.GetClosestLocalPlayer(_position).GetPosition(), _position);
  }

  public override float GetSquaredDistanceToClosestLocalPlayer(Vector3 _position)
  {
    Vector3 closestLocalPlayer = this.GetVectorToClosestLocalPlayer(_position);
    return ((Vector3) ref closestLocalPlayer).sqrMagnitude;
  }

  public override float GetDistanceToClosestLocalPlayer(Vector3 _position)
  {
    Vector3 closestLocalPlayer = this.GetVectorToClosestLocalPlayer(_position);
    return ((Vector3) ref closestLocalPlayer).magnitude;
  }

  public void SetLocalPlayer(EntityPlayerLocal _thePlayer)
  {
    this.m_LocalPlayerEntity = _thePlayer;
    this.audioManager.AttachLocalPlayer(_thePlayer, this);
    LightManager.AttachLocalPlayer(_thePlayer, this);
    OcclusionManager.Instance.SetSourceDepthCamera(_thePlayer.playerCamera);
  }

  public override EntityPlayerLocal GetPrimaryPlayer() => this.m_LocalPlayerEntity;

  public int GetPrimaryPlayerId()
  {
    return !Object.op_Inequality((Object) this.m_LocalPlayerEntity, (Object) null) ? -1 : this.m_LocalPlayerEntity.entityId;
  }

  public override List<EntityPlayer> GetPlayers() => this.Players.list;

  public void GetSunAndBlockColors(Vector3i _worldBlockPos, out byte sunLight, out byte blockLight)
  {
    sunLight = (byte) 0;
    blockLight = (byte) 0;
    IChunk chunkFromWorldPos = this.GetChunkFromWorldPos(_worldBlockPos);
    if (chunkFromWorldPos == null)
      return;
    int blockXz1 = World.toBlockXZ(_worldBlockPos.x);
    int blockY = World.toBlockY(_worldBlockPos.y);
    int blockXz2 = World.toBlockXZ(_worldBlockPos.z);
    sunLight = chunkFromWorldPos.GetLight(blockXz1, blockY, blockXz2, Chunk.LIGHT_TYPE.SUN);
    blockLight = chunkFromWorldPos.GetLight(blockXz1, blockY, blockXz2, Chunk.LIGHT_TYPE.BLOCK);
  }

  public override float GetLightBrightness(Vector3i blockPos)
  {
    IChunk chunkFromWorldPos = this.GetChunkFromWorldPos(blockPos);
    if (chunkFromWorldPos != null)
    {
      int blockXz1 = World.toBlockXZ(blockPos.x);
      int blockY = World.toBlockY(blockPos.y);
      int blockXz2 = World.toBlockXZ(blockPos.z);
      return chunkFromWorldPos.GetLightBrightness(blockXz1, blockY, blockXz2, 0);
    }
    return !this.IsDaytime() ? 0.1f : 0.65f;
  }

  public override int GetBlockLightValue(int _clrIdx, Vector3i blockPos)
  {
    ChunkCluster chunkCluster = this.ChunkClusters[_clrIdx];
    if (chunkCluster == null)
      return (int) MarchingCubes.DensityAir;
    IChunk chunkFromWorldPos = chunkCluster.GetChunkFromWorldPos(blockPos);
    if (chunkFromWorldPos == null)
      return 0;
    int blockXz1 = World.toBlockXZ(blockPos.x);
    int blockY = World.toBlockY(blockPos.y);
    int blockXz2 = World.toBlockXZ(blockPos.z);
    return chunkFromWorldPos.GetLightValue(blockXz1, blockY, blockXz2, 0);
  }

  public override sbyte GetDensity(int _clrIdx, Vector3i _blockPos)
  {
    return this.GetDensity(_clrIdx, _blockPos.x, _blockPos.y, _blockPos.z);
  }

  public override sbyte GetDensity(int _clrIdx, int _x, int _y, int _z)
  {
    ChunkCluster chunkCluster = this.ChunkClusters[_clrIdx];
    if (chunkCluster == null)
      return MarchingCubes.DensityAir;
    IChunk chunkFromWorldPos = chunkCluster.GetChunkFromWorldPos(_x, _y, _z);
    return chunkFromWorldPos != null ? chunkFromWorldPos.GetDensity(World.toBlockXZ(_x), World.toBlockY(_y), World.toBlockXZ(_z)) : MarchingCubes.DensityAir;
  }

  public void SetDensity(int _clrIdx, Vector3i _pos, sbyte _density, bool _bFoceDensity = false)
  {
    this.ChunkClusters[_clrIdx]?.SetDensity(_pos, _density, _bFoceDensity);
  }

  public long GetTexture(int _x, int _y, int _z, int channel = 0)
  {
    Chunk chunkFromWorldPos = (Chunk) this.ChunkCache.GetChunkFromWorldPos(_x, _y, _z);
    return chunkFromWorldPos != null ? chunkFromWorldPos.GetTextureFull(World.toBlockXZ(_x), World.toBlockY(_y), World.toBlockXZ(_z), channel) : 0L;
  }

  public TextureFullArray GetTextureFullArray(int _x, int _y, int _z)
  {
    Chunk chunkFromWorldPos = (Chunk) this.ChunkCache.GetChunkFromWorldPos(_x, _y, _z);
    return chunkFromWorldPos != null ? chunkFromWorldPos.GetTextureFullArray(World.toBlockXZ(_x), World.toBlockY(_y), World.toBlockXZ(_z), true) : new TextureFullArray(0L);
  }

  public void SetTexture(int _clrIdx, int _x, int _y, int _z, long _tex, int channel = 0)
  {
    this.ChunkClusters[_clrIdx].SetTextureFull(new Vector3i(_x, _y, _z), _tex, channel);
  }

  public override byte GetStability(int worldX, int worldY, int worldZ)
  {
    IChunk chunkSync = this.GetChunkSync(World.toChunkXZ(worldX), World.toChunkXZ(worldZ));
    return chunkSync != null ? chunkSync.GetStability(World.toBlockXZ(worldX), World.toBlockY(worldY), World.toBlockXZ(worldZ)) : (byte) 0;
  }

  public override byte GetStability(Vector3i _pos) => this.GetStability(_pos.x, _pos.y, _pos.z);

  public override void SetStability(int worldX, int worldY, int worldZ, byte stab)
  {
    this.GetChunkSync(World.toChunkXZ(worldX), World.toChunkXZ(worldZ))?.SetStability(World.toBlockXZ(worldX), World.toBlockY(worldY), World.toBlockXZ(worldZ), stab);
  }

  public override void SetStability(Vector3i _pos, byte stab)
  {
    this.SetStability(_pos.x, _pos.y, _pos.z, stab);
  }

  public override byte GetHeight(int worldX, int worldZ)
  {
    IChunk chunkSync = this.GetChunkSync(World.toChunkXZ(worldX), World.toChunkXZ(worldZ));
    return chunkSync != null ? chunkSync.GetHeight(World.toBlockXZ(worldX), World.toBlockXZ(worldZ)) : (byte) 0;
  }

  public byte GetTerrainHeight(int worldX, int worldZ)
  {
    Chunk chunkSync = (Chunk) this.GetChunkSync(World.toChunkXZ(worldX), World.toChunkXZ(worldZ));
    return chunkSync != null ? chunkSync.GetTerrainHeight(World.toBlockXZ(worldX), World.toBlockXZ(worldZ)) : (byte) 0;
  }

  public float GetHeightAt(float worldX, float worldZ)
  {
    ITerrainGenerator terrainGenerator = this.ChunkCache.ChunkProvider?.GetTerrainGenerator();
    return terrainGenerator != null ? terrainGenerator.GetTerrainHeightAt((int) worldX, (int) worldZ) : 0.0f;
  }

  public bool GetWaterAt(float worldX, float worldZ)
  {
    if (!(this.ChunkCache.ChunkProvider is ChunkProviderGenerateWorldFromRaw chunkProvider))
      return false;
    WorldDecoratorPOIFromImage poiFromImage = chunkProvider.poiFromImage;
    if (poiFromImage == null || !poiFromImage.m_Poi.Contains((int) worldX, (int) worldZ))
      return false;
    byte data = poiFromImage.m_Poi.GetData((int) worldX, (int) worldZ);
    if (data == (byte) 0)
      return false;
    PoiMapElement poiForColor = this.Biomes.getPoiForColor((uint) data);
    return poiForColor != null && poiForColor.m_BlockValue.type == 240 /*0xF0*/;
  }

  public override bool IsWater(int _x, int _y, int _z)
  {
    if ((uint) _y < 256U /*0x0100*/)
    {
      IChunk chunkFromWorldPos = this.GetChunkFromWorldPos(_x, _y, _z);
      if (chunkFromWorldPos != null)
      {
        _x &= 15;
        _y &= (int) byte.MaxValue;
        _z &= 15;
        return chunkFromWorldPos.IsWater(_x, _y, _z);
      }
    }
    return false;
  }

  public override bool IsWater(Vector3i _pos) => this.IsWater(_pos.x, _pos.y, _pos.z);

  public override bool IsWater(Vector3 _pos) => this.IsWater(World.worldToBlockPos(_pos));

  public override bool IsAir(int _x, int _y, int _z)
  {
    if ((uint) _y < 256U /*0x0100*/)
    {
      IChunk chunkFromWorldPos = this.GetChunkFromWorldPos(_x, _y, _z);
      if (chunkFromWorldPos != null)
      {
        _x &= 15;
        _y &= (int) byte.MaxValue;
        _z &= 15;
        return chunkFromWorldPos.IsAir(_x, _y, _z);
      }
    }
    return true;
  }

  public bool CheckForLevelNearbyHeights(float worldX, float worldZ, int distance)
  {
    ITerrainGenerator terrainGenerator = this.ChunkCache.ChunkProvider?.GetTerrainGenerator();
    if (terrainGenerator == null)
      return false;
    float terrainHeightAt1 = terrainGenerator.GetTerrainHeightAt((int) worldX, (int) worldZ);
    float num1 = terrainHeightAt1;
    float num2 = terrainHeightAt1;
    float terrainHeightAt2 = terrainGenerator.GetTerrainHeightAt((int) worldX + distance, (int) worldZ);
    if ((double) terrainHeightAt2 < (double) num1)
      num1 = terrainHeightAt2;
    else if ((double) terrainHeightAt2 > (double) num2)
      num2 = terrainHeightAt2;
    float terrainHeightAt3 = terrainGenerator.GetTerrainHeightAt((int) worldX - distance, (int) worldZ);
    if ((double) terrainHeightAt3 < (double) num1)
      num1 = terrainHeightAt3;
    else if ((double) terrainHeightAt3 > (double) num2)
      num2 = terrainHeightAt3;
    float terrainHeightAt4 = terrainGenerator.GetTerrainHeightAt((int) worldX, (int) worldZ + distance);
    if ((double) terrainHeightAt4 < (double) num1)
      num1 = terrainHeightAt4;
    else if ((double) terrainHeightAt4 > (double) num2)
      num2 = terrainHeightAt4;
    float terrainHeightAt5 = terrainGenerator.GetTerrainHeightAt((int) worldX, (int) worldZ - distance);
    if ((double) terrainHeightAt5 < (double) num1)
      num1 = terrainHeightAt5;
    else if ((double) terrainHeightAt5 > (double) num2)
      num2 = terrainHeightAt5;
    return (double) Mathf.Abs(num2 - num1) <= 2.0;
  }

  public bool FindRandomSpawnPointNearRandomPlayer(
    int maxLightValue,
    out int x,
    out int y,
    out int z)
  {
    if (this.Players.list.Count == 0)
    {
      x = y = z = 0;
      return false;
    }
    Entity _entityPlayer = (Entity) null;
    int num = this.GetGameRandom().RandomRange(this.Players.list.Count);
    for (int index = 0; index < this.Players.list.Count; ++index)
    {
      _entityPlayer = (Entity) this.Players.list[index];
      if (num-- == 0)
        break;
    }
    return this.FindRandomSpawnPointNearPlayer(_entityPlayer, maxLightValue, out x, out y, out z, 32 /*0x20*/);
  }

  public bool FindRandomSpawnPointNearPlayer(
    Entity _entityPlayer,
    int maxLightValue,
    out int x,
    out int y,
    out int z,
    int maxDistance)
  {
    return this.FindRandomSpawnPointNearPosition(_entityPlayer.GetPosition(), maxLightValue, out x, out y, out z, new Vector3((float) maxDistance, (float) maxDistance, (float) maxDistance), true);
  }

  public bool FindRandomSpawnPointNearPositionUnderground(
    Vector3 _pos,
    int maxLightValue,
    out int x,
    out int y,
    out int z,
    Vector3 maxDistance)
  {
    x = y = z = 0;
    for (int index = 0; index < 5; ++index)
    {
      x = Utils.Fastfloor(_pos.x + this.RandomRange((float) (-(double) maxDistance.x / 2.0), maxDistance.x / 2f));
      z = Utils.Fastfloor(_pos.z + this.RandomRange((float) (-(double) maxDistance.z / 2.0), maxDistance.z / 2f));
      Chunk chunkFromWorldPos = (Chunk) this.GetChunkFromWorldPos(x, z);
      if (chunkFromWorldPos != null && this.IsInPlayfield(chunkFromWorldPos))
      {
        int blockXz1 = World.toBlockXZ(x);
        int blockXz2 = World.toBlockXZ(z);
        int startY = Utils.Fastfloor(_pos.y - maxDistance.y / 2f);
        int endY = Utils.Fastfloor(_pos.y + maxDistance.y / 2f);
        int y1 = (int) _pos.y;
        if (y1 >= startY && y1 <= endY && chunkFromWorldPos.CanMobsSpawnAtPos(blockXz1, y1, blockXz2))
        {
          y = y1;
          return true;
        }
        if (chunkFromWorldPos.FindSpawnPointAtXZ(blockXz1, blockXz2, out y, maxLightValue, 0, startY, endY))
          return true;
      }
    }
    return false;
  }

  public bool FindRandomSpawnPointNearPosition(
    Vector3 _pos,
    int maxLightValue,
    out int x,
    out int y,
    out int z,
    Vector3 maxDistance,
    bool _bOnGround,
    bool _bIgnoreCanMobsSpawnOn = false)
  {
    x = y = z = 0;
    for (int index = 0; index < 5; ++index)
    {
      x = Utils.Fastfloor(_pos.x + this.RandomRange((float) (-(double) maxDistance.x / 2.0), maxDistance.x / 2f));
      z = Utils.Fastfloor(_pos.z + this.RandomRange((float) (-(double) maxDistance.z / 2.0), maxDistance.z / 2f));
      Chunk chunkFromWorldPos = (Chunk) this.GetChunkFromWorldPos(x, z);
      if (chunkFromWorldPos != null && this.IsInPlayfield(chunkFromWorldPos))
      {
        if (_bOnGround)
        {
          int blockXz1 = World.toBlockXZ(x);
          int blockXz2 = World.toBlockXZ(z);
          int startY = Utils.Fastfloor(_pos.y - maxDistance.y / 2f);
          int endY = Utils.Fastfloor(_pos.y + maxDistance.y / 2f);
          int _y = (int) chunkFromWorldPos.GetHeight(blockXz1, blockXz2) + 1;
          if (_y >= startY && _y <= endY && chunkFromWorldPos.CanMobsSpawnAtPos(blockXz1, _y, blockXz2, _bIgnoreCanMobsSpawnOn))
          {
            y = _y;
            return true;
          }
          if (chunkFromWorldPos.FindSpawnPointAtXZ(blockXz1, blockXz2, out y, maxLightValue, 0, startY, endY, _bIgnoreCanMobsSpawnOn))
            return true;
        }
        else
        {
          y = Utils.Fastfloor(_pos.y + this.RandomRange((float) (-(double) maxDistance.y / 2.0), maxDistance.y / 2f));
          return true;
        }
      }
    }
    return false;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public bool isPositionInRangeOfBedrolls(Vector3 _position)
  {
    int num1 = GamePrefs.GetInt(EnumGamePrefs.BedrollDeadZoneSize);
    int num2 = num1 * num1;
    for (int index = 0; index < this.Players.list.Count; ++index)
    {
      EntityBedrollPositionList spawnPoints = this.Players.list[index].SpawnPoints;
      int count = spawnPoints.Count;
      for (int _idx = 0; _idx < count; ++_idx)
      {
        Vector3 vector3 = Vector3.op_Subtraction(spawnPoints[_idx].ToVector3(), _position);
        if ((double) ((Vector3) ref vector3).sqrMagnitude < (double) num2)
          return true;
      }
    }
    return false;
  }

  public bool GetRandomSpawnPositionMinMaxToRandomPlayer(
    int _minRange,
    int _maxRange,
    bool _bConsiderBedrolls,
    out EntityPlayer _player,
    out Vector3 _position)
  {
    _position = Vector3.zero;
    _player = (EntityPlayer) null;
    if (this.Players.list.Count == 0 || _maxRange - _minRange <= 0)
      return false;
    int num1 = this.rand.RandomRange(this.Players.list.Count);
    for (int index = 0; index < this.Players.list.Count; ++index)
    {
      if (num1-- == 0)
      {
        _player = this.Players.list[index];
        break;
      }
    }
    int num2 = _minRange * _minRange;
    for (int index1 = 0; index1 < 10; ++index1)
    {
      Vector2 zero = Vector2.zero;
      Vector2 vector2_1;
      do
      {
        vector2_1 = Vector2.op_Multiply(this.rand.RandomInsideUnitCircle, (float) (_maxRange - _minRange));
      }
      while ((double) ((Vector2) ref vector2_1).sqrMagnitude < 0.01);
      Vector2 vector2_2 = Vector2.op_Addition(vector2_1, Vector2.op_Multiply(vector2_1, (float) _minRange / ((Vector2) ref vector2_1).magnitude));
      _position = Vector3.op_Addition(_player.GetPosition(), new Vector3(vector2_2.x, 0.0f, vector2_2.y));
      Vector3i blockPos = World.worldToBlockPos(_position);
      Chunk chunkFromWorldPos = (Chunk) this.GetChunkFromWorldPos(blockPos);
      if (chunkFromWorldPos != null)
      {
        int blockXz1 = World.toBlockXZ(blockPos.x);
        int blockXz2 = World.toBlockXZ(blockPos.z);
        blockPos.y = (int) chunkFromWorldPos.GetHeight(blockXz1, blockXz2) + 1;
        _position.y = (float) blockPos.y;
        if ((!_bConsiderBedrolls || !this.isPositionInRangeOfBedrolls(blockPos.ToVector3())) && chunkFromWorldPos.CanMobsSpawnAtPos(blockXz1, Utils.Fastfloor(_position.y), blockXz2))
        {
          bool flag = true;
          for (int index2 = 0; index2 < this.Players.list.Count; ++index2)
          {
            EntityPlayer entityPlayer = this.Players.list[index2];
            if ((double) entityPlayer.GetDistanceSq(_position) < (double) num2)
            {
              flag = false;
              break;
            }
            if (entityPlayer.CanSee(_position))
            {
              flag = false;
              break;
            }
          }
          if (flag)
          {
            _position = Vector3.op_Addition(blockPos.ToVector3(), new Vector3(0.5f, this.GetTerrainOffset(0, blockPos), 0.5f));
            return true;
          }
        }
      }
    }
    return false;
  }

  public bool GetMobRandomSpawnPosWithWater(
    Vector3 _targetPos,
    int _minRange,
    int _maxRange,
    int _minPlayerRange,
    bool _checkBedrolls,
    out Vector3 _position)
  {
    return this.GetRandomSpawnPositionMinMaxToPosition(_targetPos, _minRange, _maxRange, _minPlayerRange, _checkBedrolls, out _position, _retryCount: 20) || this.GetRandomSpawnPositionMinMaxToPosition(_targetPos, _minRange, _maxRange, _minPlayerRange, _checkBedrolls, out _position, _checkWater: false, _retryCount: 20);
  }

  public bool GetRandomSpawnPositionMinMaxToPosition(
    Vector3 _targetPos,
    int _minRange,
    int _maxRange,
    int _minPlayerRange,
    bool _checkBedrolls,
    out Vector3 _position,
    int _forPlayerEntityId = -1,
    bool _checkWater = true,
    int _retryCount = 50,
    bool _checkLandClaim = false,
    EnumLandClaimOwner _maxLandClaimType = EnumLandClaimOwner.None)
  {
    _position = Vector3.zero;
    int num = _maxRange - _minRange;
    if (num <= 0)
      return false;
    PersistentPlayerData dataFromEntityId = _checkLandClaim ? GameManager.Instance.persistentPlayers.GetPlayerDataFromEntityID(_forPlayerEntityId) : (PersistentPlayerData) null;
    for (int index = 0; index < _retryCount; ++index)
    {
      Vector2 vector2_1;
      do
      {
        vector2_1 = Vector2.op_Multiply(this.rand.RandomInsideUnitCircle, (float) num);
      }
      while ((double) ((Vector2) ref vector2_1).sqrMagnitude < 0.0099999997764825821);
      Vector2 vector2_2 = Vector2.op_Addition(vector2_1, Vector2.op_Multiply(vector2_1, (float) _minRange / ((Vector2) ref vector2_1).magnitude));
      _position.x = _targetPos.x + vector2_2.x;
      _position.y = _targetPos.y;
      _position.z = _targetPos.z + vector2_2.y;
      Vector3i blockPos = World.worldToBlockPos(_position);
      Chunk chunkFromWorldPos = (Chunk) this.GetChunkFromWorldPos(blockPos);
      if (chunkFromWorldPos != null)
      {
        int blockXz1 = World.toBlockXZ(blockPos.x);
        int blockXz2 = World.toBlockXZ(blockPos.z);
        blockPos.y = (int) chunkFromWorldPos.GetHeight(blockXz1, blockXz2) + 1;
        _position.y = (float) blockPos.y;
        if (!_checkBedrolls || !this.isPositionInRangeOfBedrolls(blockPos.ToVector3()))
        {
          if (_forPlayerEntityId == -1)
          {
            if (!chunkFromWorldPos.CanMobsSpawnAtPos(blockXz1, Utils.Fastfloor(_position.y), blockXz2, _checkWater: _checkWater))
              continue;
          }
          else if (!chunkFromWorldPos.CanPlayersSpawnAtPos(blockXz1, Utils.Fastfloor(_position.y), blockXz2) || !chunkFromWorldPos.IsPositionOnTerrain(blockXz1, blockPos.y, blockXz2) || this.GetPOIAtPosition(_position) != null || _checkWater && chunkFromWorldPos.IsWater(blockXz1, blockPos.y - 1, blockXz2) || _checkLandClaim && this.GetLandClaimOwner(blockPos, dataFromEntityId) > _maxLandClaimType)
            continue;
          if (this.isPositionFarFromPlayers(_position, _minPlayerRange))
          {
            _position = Vector3.op_Addition(blockPos.ToVector3(), new Vector3(0.5f, this.GetTerrainOffset(0, blockPos), 0.5f));
            return true;
          }
        }
      }
    }
    _position = Vector3.zero;
    return false;
  }

  public bool GetRandomSpawnPositionInAreaMinMaxToPlayers(
    Rect _area,
    int _minDistance,
    int UNUSED_maxDistance,
    bool _checkBedrolls,
    out Vector3 _position)
  {
    _position = Vector3.zero;
    if (this.Players.list.Count == 0)
      return false;
    for (int index1 = 0; index1 < 10; ++index1)
    {
      _position.x = ((Rect) ref _area).x + this.RandomRange(0.0f, ((Rect) ref _area).width - 1f);
      _position.y = 0.0f;
      _position.z = ((Rect) ref _area).y + this.RandomRange(0.0f, ((Rect) ref _area).height - 1f);
      Vector3i blockPos = World.worldToBlockPos(_position);
      Chunk chunkFromWorldPos = (Chunk) this.GetChunkFromWorldPos(blockPos);
      if (chunkFromWorldPos != null)
      {
        int blockXz1 = World.toBlockXZ(blockPos.x);
        int blockXz2 = World.toBlockXZ(blockPos.z);
        blockPos.y = (int) chunkFromWorldPos.GetHeight(blockXz1, blockXz2) + 1;
        _position.y = (float) blockPos.y;
        if ((!_checkBedrolls || !this.isPositionInRangeOfBedrolls(blockPos.ToVector3())) && chunkFromWorldPos.CanMobsSpawnAtPos(blockXz1, Utils.Fastfloor(_position.y), blockXz2))
        {
          bool flag = this.isPositionFarFromPlayers(_position, _minDistance);
          if (flag)
          {
            for (int index2 = 0; index2 < this.Players.list.Count; ++index2)
            {
              EntityPlayer entityPlayer = this.Players.list[index2];
              Vector3 vector3 = Vector3.op_Subtraction(_position, entityPlayer.position);
              if ((double) ((Vector3) ref vector3).sqrMagnitude < 2500.0 && entityPlayer.IsInViewCone(_position))
              {
                flag = false;
                break;
              }
            }
            if (flag)
            {
              _position = Vector3.op_Addition(blockPos.ToVector3(), new Vector3(0.5f, this.GetTerrainOffset(0, blockPos), 0.5f));
              return true;
            }
          }
        }
      }
    }
    return false;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public bool isPositionFarFromPlayers(Vector3 _position, int _minDistance)
  {
    int num = _minDistance * _minDistance;
    for (int index = 0; index < this.Players.list.Count; ++index)
    {
      if ((double) this.Players.list[index].GetDistanceSq(_position) < (double) num)
        return false;
    }
    return true;
  }

  public Vector3 FindSupportingBlockPos(Vector3 pos)
  {
    Vector3i blockPos = World.worldToBlockPos(pos);
    BlockValue block1 = this.GetBlock(blockPos);
    Block block2 = block1.Block;
    if (block2.IsMovementBlocked((IBlockAccess) this, blockPos, block1, BlockFace.Top) || block2.IsElevator())
      return pos;
    ++blockPos.y;
    BlockValue block3 = this.GetBlock(blockPos);
    if (block3.Block.IsElevator((int) block3.rotation))
      return pos;
    blockPos.y -= 2;
    BlockValue block4 = this.GetBlock(blockPos);
    Block block5 = block4.Block;
    if (!block5.IsElevator() && !block5.IsMovementBlocked((IBlockAccess) this, blockPos, block4, BlockFace.Top))
    {
      Vector3 vector3_1;
      // ISSUE: explicit constructor call
      ((Vector3) ref vector3_1).\u002Ector((float) blockPos.x + 0.5f, pos.y, (float) blockPos.z + 0.5f);
      Vector3 vector3_2 = Vector3.op_Subtraction(pos, vector3_1);
      int index1 = Mathf.RoundToInt((float) (((double) Mathf.Atan2(vector3_2.x, vector3_2.z) * 57.295780181884766 + 22.5) / 45.0)) & 7;
      int[] numArray = World.supportOrder[index1];
      Vector3i vector3i;
      vector3i.y = blockPos.y;
      for (int index2 = 0; index2 < 8; ++index2)
      {
        int index3 = numArray[index2] * 2;
        vector3i.x = blockPos.x + World.supportOffsets[index3];
        vector3i.z = blockPos.z + World.supportOffsets[index3 + 1];
        BlockValue block6 = this.GetBlock(vector3i);
        if (block6.Block.IsMovementBlocked((IBlockAccess) this, vector3i, block6, BlockFace.Top))
        {
          pos.x = (float) vector3i.x + 0.5f;
          pos.z = (float) vector3i.z + 0.5f;
          break;
        }
      }
    }
    return pos;
  }

  public float GetTerrainOffset(int _clrIdx, Vector3i _blockPos)
  {
    float terrainOffset = 0.0f;
    if (this.GetBlock(_clrIdx, _blockPos - Vector3i.up).Block.shape.IsTerrain())
      terrainOffset = MarchingCubes.GetDecorationOffsetY(this.GetDensity(_clrIdx, _blockPos), this.GetDensity(_clrIdx, _blockPos - Vector3i.up));
    return terrainOffset;
  }

  public bool IsInPlayfield(Chunk _c)
  {
    ChunkCluster chunkCache = this.ChunkCache;
    if (!chunkCache.IsFixedSize || this.IsEditor())
      return true;
    return _c.X > chunkCache.ChunkMinPos.x && _c.Z > chunkCache.ChunkMinPos.y && _c.X < chunkCache.ChunkMaxPos.x && _c.Z < chunkCache.ChunkMaxPos.y;
  }

  public override BlockValue GetBlock(Vector3i _pos)
  {
    ChunkCluster chunkCache = this.ChunkCache;
    return chunkCache == null ? BlockValue.Air : chunkCache.GetBlock(_pos);
  }

  public override BlockValue GetBlock(int _clrIdx, Vector3i _pos)
  {
    ChunkCluster chunkCluster = this.ChunkClusters[_clrIdx];
    return chunkCluster == null ? BlockValue.Air : chunkCluster.GetBlock(_pos);
  }

  public override BlockValue GetBlock(int _x, int _y, int _z)
  {
    return this.GetBlock(new Vector3i(_x, _y, _z));
  }

  public override BlockValue GetBlock(int _clrIdx, int _x, int _y, int _z)
  {
    ChunkCluster chunkCluster = this.ChunkClusters[_clrIdx];
    return chunkCluster == null ? BlockValue.Air : chunkCluster.GetBlock(new Vector3i(_x, _y, _z));
  }

  public WaterValue GetWater(int _x, int _y, int _z) => this.GetWater(new Vector3i(_x, _y, _z));

  public WaterValue GetWater(Vector3i _pos)
  {
    ChunkCluster chunkCache = this.ChunkCache;
    return chunkCache == null ? WaterValue.Empty : chunkCache.GetWater(_pos);
  }

  public float GetWaterPercent(Vector3i _pos)
  {
    ChunkCluster chunkCache = this.ChunkCache;
    return chunkCache == null ? 0.0f : chunkCache.GetWater(_pos).GetMassPercent();
  }

  public void HandleWaterLevelChanged(Vector3i _pos, float _waterPercent)
  {
    GameLightManager.Instance?.HandleWaterLevelChanged();
  }

  public BiomeDefinition GetBiome(string _name) => this.Biomes.GetBiome(_name);

  public BiomeDefinition GetBiome(int _x, int _z)
  {
    Chunk chunkFromWorldPos = (Chunk) this.GetChunkFromWorldPos(_x, _z);
    return chunkFromWorldPos != null ? this.Biomes.GetBiome(chunkFromWorldPos.GetBiomeId(World.toBlockXZ(_x), World.toBlockXZ(_z))) : (BiomeDefinition) null;
  }

  public BiomeDefinition GetBiomeInWorld(int _x, int _z)
  {
    IChunkProvider chunkProvider = this.ChunkCache?.ChunkProvider;
    if (chunkProvider != null)
    {
      IBiomeProvider biomeProvider = chunkProvider.GetBiomeProvider();
      if (biomeProvider != null)
        return biomeProvider.GetBiomeAt(_x, _z);
    }
    return (BiomeDefinition) null;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public IChunk GetChunkFromWorldPos(int x, int z)
  {
    return this.GetChunkSync(World.toChunkXZ(x), World.toChunkXZ(z));
  }

  public override IChunk GetChunkFromWorldPos(int x, int y, int z)
  {
    return this.GetChunkSync(World.toChunkXZ(x), World.toChunkXZ(z));
  }

  public override IChunk GetChunkFromWorldPos(Vector3i _blockPos)
  {
    return this.GetChunkSync(World.toChunkXZ(_blockPos.x), World.toChunkXZ(_blockPos.z));
  }

  public override void GetChunkFromWorldPos(int _blockX, int _blockZ, ref IChunk _chunk)
  {
    _blockX >>= 4;
    _blockZ >>= 4;
    if (_chunk != null && _chunk.X == _blockX && _chunk.Z == _blockZ)
      return;
    _chunk = this.GetChunkSync(_blockX, _blockZ);
  }

  public override bool GetChunkFromWorldPos(Vector3i _blockPos, ref IChunk _chunk)
  {
    Vector3i chunkXyz = World.toChunkXYZ(_blockPos);
    if (_chunk == null || _chunk.ChunkPos != chunkXyz)
      _chunk = this.GetChunkSync(chunkXyz);
    return _chunk != null;
  }

  public override IChunk GetChunkSync(Vector3i chunkPos)
  {
    return this.GetChunkSync(chunkPos.x, chunkPos.z);
  }

  public IChunk GetChunkSync(int chunkX, int chunkZ)
  {
    ChunkCluster chunkCache = this.ChunkCache;
    return chunkCache == null ? (IChunk) null : (IChunk) chunkCache.GetChunkSync(chunkX, chunkZ);
  }

  public IChunk GetChunkSync(long _key)
  {
    ChunkCluster chunkCache = this.ChunkCache;
    return chunkCache == null ? (IChunk) null : (IChunk) chunkCache.GetChunkSync(_key);
  }

  public bool IsChunkAreaLoaded(Vector3 _position)
  {
    return this.IsChunkAreaLoaded(Utils.Fastfloor(_position.x), 0, Utils.Fastfloor(_position.z));
  }

  public bool IsChunkAreaLoaded(int _blockPosX, int _, int _blockPosZ)
  {
    int chunkXz1 = World.toChunkXZ(_blockPosX - 8);
    int chunkXz2 = World.toChunkXZ(_blockPosZ - 8);
    int chunkXz3 = World.toChunkXZ(_blockPosX + 8);
    int chunkXz4 = World.toChunkXZ(_blockPosZ + 8);
    for (int chunkX = chunkXz1; chunkX <= chunkXz3; ++chunkX)
    {
      for (int chunkZ = chunkXz2; chunkZ <= chunkXz4; ++chunkZ)
      {
        if (this.GetChunkSync(chunkX, chunkZ) == null)
          return false;
      }
    }
    return true;
  }

  public bool IsChunkAreaCollidersLoaded(Vector3 _position)
  {
    return this.IsChunkAreaCollidersLoaded(Utils.Fastfloor(_position.x), Utils.Fastfloor(_position.z));
  }

  public bool IsChunkAreaCollidersLoaded(int _blockPosX, int _blockPosZ)
  {
    int chunkXz1 = World.toChunkXZ(_blockPosX - 8);
    int chunkXz2 = World.toChunkXZ(_blockPosZ - 8);
    int chunkXz3 = World.toChunkXZ(_blockPosX + 8);
    int chunkXz4 = World.toChunkXZ(_blockPosZ + 8);
    for (int chunkX = chunkXz1; chunkX <= chunkXz3; ++chunkX)
    {
      for (int chunkZ = chunkXz2; chunkZ <= chunkXz4; ++chunkZ)
      {
        Chunk chunkSync = (Chunk) this.GetChunkSync(chunkX, chunkZ);
        if (chunkSync == null || !chunkSync.IsCollisionMeshGenerated)
          return false;
      }
    }
    return true;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int toChunkXZ(int _v) => _v >> 4;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector2i toChunkXZ(Vector2i _v) => new Vector2i(_v.x >> 4, _v.y >> 4);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector2i toChunkXZ(Vector3 _v)
  {
    return new Vector2i(Utils.Fastfloor(_v.x) >> 4, Utils.Fastfloor(_v.z) >> 4);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector2i toChunkXZ(Vector3i _v) => new Vector2i(_v.x >> 4, _v.z >> 4);

  public static Vector3i toChunkXYZCube(Vector3 _v)
  {
    return new Vector3i(Utils.Fastfloor(_v.x) >> 4, Utils.Fastfloor(_v.y) >> 4, Utils.Fastfloor(_v.z) >> 4);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3i toChunkXYZ(Vector3i _v) => new Vector3i(_v.x >> 4, _v.y >> 8, _v.z >> 4);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int toChunkY(int _v) => _v >> 8;

  public static Vector3 toChunkXyzWorldPos(Vector3i _v)
  {
    return new Vector3((float) (_v.x & -16), (float) (_v.y & -256), (float) (_v.z & -16));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3i toBlock(Vector3i _p)
  {
    _p.x &= 15;
    _p.y &= (int) byte.MaxValue;
    _p.z &= 15;
    return _p;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3i toBlock(int _x, int _y, int _z)
  {
    return new Vector3i(_x & 15, _y & (int) byte.MaxValue, _z & 15);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int toBlockXZ(int _v) => _v & 15;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int toBlockY(int _v) => _v & (int) byte.MaxValue;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3 blockToTransformPos(Vector3i _blockPos)
  {
    return new Vector3((float) _blockPos.x + 0.5f, (float) _blockPos.y, (float) _blockPos.z + 0.5f);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3i worldToBlockPos(Vector3 _worldPos)
  {
    return new Vector3i(Utils.Fastfloor(_worldPos.x), Utils.Fastfloor(_worldPos.y), Utils.Fastfloor(_worldPos.z));
  }

  public override void SetBlockRPC(int _clrIdx, Vector3i _blockPos, BlockValue _blockValue)
  {
    this.gameManager.SetBlocksRPC(new List<BlockChangeInfo>()
    {
      new BlockChangeInfo(_clrIdx, _blockPos, _blockValue, true)
    });
  }

  public override void SetBlockRPC(Vector3i _blockPos, BlockValue _blockValue)
  {
    this.gameManager.SetBlocksRPC(new List<BlockChangeInfo>()
    {
      new BlockChangeInfo(_blockPos, _blockValue, true)
    });
  }

  public override void SetBlockRPC(Vector3i _blockPos, BlockValue _blockValue, sbyte _density)
  {
    this.gameManager.SetBlocksRPC(new List<BlockChangeInfo>()
    {
      new BlockChangeInfo(_blockPos, _blockValue, _density)
    });
  }

  public override void SetBlockRPC(
    int _clrIdx,
    Vector3i _blockPos,
    BlockValue _blockValue,
    sbyte _density)
  {
    this.gameManager.SetBlocksRPC(new List<BlockChangeInfo>()
    {
      new BlockChangeInfo(_clrIdx, _blockPos, _blockValue, _density)
    });
  }

  public override void SetBlockRPC(Vector3i _blockPos, sbyte _density)
  {
    this.gameManager.SetBlocksRPC(new List<BlockChangeInfo>()
    {
      new BlockChangeInfo(0, _blockPos, _density)
    });
  }

  public override void SetBlocksRPC(List<BlockChangeInfo> _blockChangeInfo)
  {
    this.gameManager.SetBlocksRPC(_blockChangeInfo);
  }

  public override void SetBlockRPC(
    int _clrIdx,
    Vector3i _blockPos,
    BlockValue _blockValue,
    sbyte _density,
    int _changingEntityId)
  {
    this.gameManager.SetBlocksRPC(new List<BlockChangeInfo>()
    {
      new BlockChangeInfo(_clrIdx, _blockPos, _blockValue, _density, _changingEntityId)
    });
  }

  public override void SetBlockRPC(
    int _clrIdx,
    Vector3i _blockPos,
    BlockValue _blockValue,
    int _changingEntityId)
  {
    this.gameManager.SetBlocksRPC(new List<BlockChangeInfo>()
    {
      new BlockChangeInfo(_clrIdx, _blockPos, _blockValue, true, _changingEntityId)
    });
  }

  public BlockValue SetBlock(
    int _clrIdx,
    Vector3i _blockPos,
    BlockValue _blockValue,
    bool bNotify,
    bool updateLight)
  {
    return this.ChunkClusters[_clrIdx].SetBlock(_blockPos, _blockValue, bNotify, updateLight);
  }

  public bool IsDaytime() => !this.IsDark();

  public bool IsDark()
  {
    float num = (float) (this.worldTime % 24000UL) / 1000f;
    return (double) num < (double) this.DawnHour || (double) num > (double) this.DuskHour;
  }

  public override TileEntity GetTileEntity(int _clrIdx, Vector3i _pos) => this.GetTileEntity(_pos);

  public override TileEntity GetTileEntity(Vector3i _pos)
  {
    ChunkCluster chunkCache = this.ChunkCache;
    if (chunkCache == null)
      return (TileEntity) null;
    Chunk chunkFromWorldPos = (Chunk) chunkCache.GetChunkFromWorldPos(_pos);
    if (chunkFromWorldPos == null)
      return (TileEntity) null;
    Vector3i _blockPosInChunk = new Vector3i(World.toBlockXZ(_pos.x), World.toBlockY(_pos.y), World.toBlockXZ(_pos.z));
    return chunkFromWorldPos.GetTileEntity(_blockPosInChunk);
  }

  public TileEntity GetTileEntity(int _entityId)
  {
    Entity entity = this.GetEntity(_entityId);
    if (Object.op_Equality((Object) entity, (Object) null))
      return (TileEntity) null;
    if (entity is EntityTrader && entity.IsAlive())
      return (TileEntity) ((EntityTrader) entity).TileEntityTrader;
    if (entity.lootContainer == null)
    {
      string lootList = entity.GetLootList();
      if (!string.IsNullOrEmpty(lootList))
      {
        entity.lootContainer = new TileEntityLootContainer((Chunk) null);
        entity.lootContainer.entityId = entity.entityId;
        entity.lootContainer.lootListName = lootList;
        entity.lootContainer.SetContainerSize(LootContainer.GetLootContainer(lootList).size, true);
      }
    }
    return (TileEntity) entity.lootContainer;
  }

  public void RemoveTileEntity(TileEntity _te)
  {
    Chunk chunk = _te.GetChunk();
    if (chunk != null)
      chunk.RemoveTileEntity(this, _te);
    else
      Log.Error("RemoveTileEntity: chunk not found!");
  }

  public BlockTrigger GetBlockTrigger(int _clrIdx, Vector3i _pos)
  {
    ChunkCluster chunkCluster = this.ChunkClusters[_clrIdx];
    if (chunkCluster == null)
      return (BlockTrigger) null;
    Chunk chunkFromWorldPos = (Chunk) chunkCluster.GetChunkFromWorldPos(_pos);
    if (chunkFromWorldPos == null)
      return (BlockTrigger) null;
    Vector3i _blockPosInChunk = new Vector3i(World.toBlockXZ(_pos.x), World.toBlockY(_pos.y), World.toBlockXZ(_pos.z));
    return chunkFromWorldPos.GetBlockTrigger(_blockPosInChunk);
  }

  public void OnUpdateTick(float _partialTicks, ArraySegment<long> _activeChunks)
  {
    this.updateChunkAddedRemovedCallbacks();
    this.WorldEventUpdateTime();
    WaterSplashCubes.Update();
    DecoManager.Instance.UpdateTick(this);
    MultiBlockManager.Instance.MainThreadUpdate();
    if (!this.IsEditor())
      this.dmsConductor.Update();
    this.checkPOIUnculling();
    this.updateChunksToUncull();
    if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
      return;
    this.worldBlockTicker.Tick(_activeChunks, (EntityPlayer) this.m_LocalPlayerEntity, this.rand);
    if (GameTimer.Instance.ticks % 20UL == 0UL)
    {
      bool flag1 = GameStats.GetBool(EnumGameStats.IsSpawnEnemies);
      int _idx = 0;
      ChunkCluster _cc = this.ChunkClusters[_idx];
      bool flag2 = GameTimer.Instance.ticks % 40UL == 0UL;
      for (int offset = _activeChunks.Offset; offset < _activeChunks.Count; ++offset)
      {
        long num = _activeChunks.Array[offset];
        if ((!flag2 || num % 2L == 0L) && (flag2 || num % 2L != 0L))
        {
          int clrIdx = WorldChunkCache.extractClrIdx(num);
          if (clrIdx != _idx)
          {
            ChunkCluster chunkCluster = this.ChunkClusters[clrIdx];
            if (chunkCluster != null)
            {
              _cc = chunkCluster;
              _idx = clrIdx;
            }
            else
              continue;
          }
          Chunk chunkSync = _cc?.GetChunkSync(num);
          if (chunkSync != null)
          {
            if (chunkSync.NeedsTicking)
              chunkSync.UpdateTick(this, flag1);
            if (!this.IsEditor() && chunkSync.IsAreaMaster() && chunkSync.IsAreaMasterDominantBiomeInitialized(_cc))
            {
              ChunkAreaBiomeSpawnData chunkBiomeSpawnData = chunkSync.GetChunkBiomeSpawnData();
              if (chunkBiomeSpawnData != null && chunkBiomeSpawnData.IsSpawnNeeded(this.Biomes, this.worldTime) && chunkSync.IsAreaMasterCornerChunksLoaded(_cc))
              {
                if (this.areaMasterChunksToLock.ContainsKey(chunkSync.Key))
                {
                  chunkSync.isModified |= chunkBiomeSpawnData.DelayAllEnemySpawningUntil(this.areaMasterChunksToLock[chunkSync.Key], this.Biomes);
                  this.areaMasterChunksToLock.Remove(chunkSync.Key);
                }
                else
                  this.biomeSpawnManager.Update(string.Empty, flag1, (object) chunkBiomeSpawnData);
              }
            }
          }
        }
      }
    }
    if (GameTimer.Instance.ticks % 16UL /*0x10*/ == 0UL && GamePrefs.GetString(EnumGamePrefs.DynamicSpawner).Length > 0)
      this.dynamicSpawnManager.Update(GamePrefs.GetString(EnumGamePrefs.DynamicSpawner), GameStats.GetBool(EnumGameStats.IsSpawnEnemies), (object) null);
    this.aiDirector.Tick((double) _partialTicks / 20.0);
    this.TickSleeperVolumes();
  }

  public bool UncullPOI(PrefabInstance _pi)
  {
    if (!_pi.AddChunksToUncull(this, this.chunksToUncull))
      return false;
    Log.Out("Unculling POI {0} {1}", new object[2]
    {
      (object) _pi.location.Name,
      (object) _pi.boundingBoxPosition
    });
    return true;
  }

  public void UncullChunk(Chunk _c)
  {
    if (!_c.IsInternalBlocksCulled)
      return;
    this.chunksToUncull.Add(_c);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void checkPOIUnculling()
  {
    if (GameTimer.Instance.ticks % 38UL != 0UL || GameStats.GetInt(EnumGameStats.OptionsPOICulling) == 0)
      return;
    List<EntityPlayer> list = GameManager.Instance.World.Players.list;
    for (int index = 0; index < list.Count; ++index)
    {
      EntityPlayer entityPlayer = list[index];
      if (entityPlayer.Spawned)
      {
        Dictionary<int, PrefabInstance> prefabsAroundNear = entityPlayer.GetPrefabsAroundNear();
        if (prefabsAroundNear != null)
        {
          foreach (KeyValuePair<int, PrefabInstance> keyValuePair in prefabsAroundNear)
          {
            PrefabInstance _pi = keyValuePair.Value;
            if (_pi.Overlaps(entityPlayer.position, 6f))
              this.UncullPOI(_pi);
          }
        }
      }
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void updateChunksToUncull()
  {
    if (this.chunksToUncull.list.Count == 0)
      return;
    this.msUnculling.ResetAndRestart();
    this.chunksToRegenerate.Clear();
    for (int index = this.chunksToUncull.list.Count - 1; index >= 0; --index)
    {
      Chunk chunk = this.chunksToUncull.list[index];
      if (chunk.InProgressUnloading)
      {
        this.chunksToUncull.Remove(chunk);
      }
      else
      {
        int num = (int) chunk.RestoreCulledBlocks(this);
        this.chunksToUncull.Remove(chunk);
        if (!this.chunksToRegenerate.hashSet.Contains(chunk))
          this.chunksToRegenerate.Add(chunk);
        Chunk chunkSync1;
        if ((num & 8) != 0 && (chunkSync1 = (Chunk) this.GetChunkSync(chunk.X - 1, chunk.Z)) != null && !this.chunksToRegenerate.hashSet.Contains(chunkSync1))
          this.chunksToRegenerate.Add(chunkSync1);
        Chunk chunkSync2;
        if ((num & 32 /*0x20*/) != 0 && (chunkSync2 = (Chunk) this.GetChunkSync(chunk.X + 1, chunk.Z)) != null && !this.chunksToRegenerate.hashSet.Contains(chunkSync2))
          this.chunksToRegenerate.Add(chunkSync2);
        Chunk chunkSync3;
        if ((num & 4) != 0 && (chunkSync3 = (Chunk) this.GetChunkSync(chunk.X, chunk.Z + 1)) != null && !this.chunksToRegenerate.hashSet.Contains(chunkSync3))
          this.chunksToRegenerate.Add(chunkSync3);
        Chunk chunkSync4;
        if ((num & 16 /*0x10*/) != 0 && (chunkSync4 = (Chunk) this.GetChunkSync(chunk.X, chunk.Z - 1)) != null && !this.chunksToRegenerate.hashSet.Contains(chunkSync4))
          this.chunksToRegenerate.Add(chunkSync4);
        if (((Stopwatch) this.msUnculling).ElapsedMilliseconds > 5L)
          break;
      }
    }
    for (int index = this.chunksToRegenerate.list.Count - 1; index >= 0; --index)
      this.chunksToRegenerate.list[index].NeedsRegeneration = true;
  }

  public Vector3[] GetRandomSpawnPointPositions(int _count)
  {
    Vector3[] spawnPointPositions = new Vector3[_count];
    List<Chunk> chunkArrayCopySync = this.ChunkCache.GetChunkArrayCopySync();
    int count = chunkArrayCopySync.Count;
    while (_count > 0)
    {
      for (int index = 0; index < chunkArrayCopySync.Count; ++index)
      {
        Chunk _chunk = chunkArrayCopySync[index];
        if (this.GetGameRandom().RandomRange(count) == 1)
        {
          Chunk[] neighbours = new Chunk[8];
          if (this.ChunkCache.GetNeighborChunks(_chunk, neighbours))
          {
            int x;
            int y;
            int z;
            if (_chunk.FindRandomTopSoilPoint(this, out x, out y, out z, 5))
            {
              spawnPointPositions[spawnPointPositions.Length - _count] = new Vector3((float) x, (float) y, (float) z);
              --_count;
            }
            if (_count == 0)
              break;
          }
        }
      }
    }
    return spawnPointPositions;
  }

  public Vector3 ClipBoundsMove(
    Entity _entity,
    Bounds _aabb,
    Vector3 move,
    Vector3 expandDir,
    float stepHeight)
  {
    if ((double) stepHeight > 0.0)
      move.y = stepHeight;
    Bounds bounds = BoundsUtils.ExpandDirectional(_aabb, expandDir);
    int num1 = Utils.Fastfloor(((Bounds) ref bounds).min.x - 0.5f);
    int num2 = Utils.Fastfloor(((Bounds) ref bounds).max.x + 1.5f);
    int num3 = Utils.Fastfloor(((Bounds) ref bounds).min.y - 0.5f);
    int num4 = Utils.Fastfloor(((Bounds) ref bounds).max.y + 1f);
    int num5 = Utils.Fastfloor(((Bounds) ref bounds).min.z - 0.5f);
    int num6 = Utils.Fastfloor(((Bounds) ref bounds).max.z + 1.5f);
    World.ClipBlock.ResetStorage();
    int num7 = 0;
    int numColliders = 0;
    Chunk _c = (Chunk) null;
    Vector3 _blockPos = new Vector3();
    for (int index1 = num1; index1 < num2; ++index1)
    {
      _blockPos.x = (float) index1;
      for (int index2 = num5; index2 < num6; ++index2)
      {
        _blockPos.z = (float) index2;
        if (_c == null || _c.X != World.toChunkXZ(index1) || _c.Z != World.toChunkXZ(index2))
        {
          _c = (Chunk) this.GetChunkFromWorldPos(index1, index2);
          if (_c != null)
          {
            if (!this.IsInPlayfield(_c))
              this._clipBounds[numColliders++] = _c.GetAABB();
          }
          else
            continue;
        }
        for (int _y = num3; _y < num4; ++_y)
        {
          if (_y > 0 && _y < 256 /*0x0100*/)
          {
            BlockValue block1 = this.GetBlock(index1, _y, index2);
            Block block2 = block1.Block;
            if (block2.IsCollideMovement)
            {
              float _yDistort = 0.0f;
              _blockPos.y = (float) _y;
              this._clipBlocks[num7++] = World.ClipBlock.New(block1, block2, _yDistort, _blockPos, _aabb);
            }
          }
        }
      }
    }
    Vector3 min = ((Bounds) ref _aabb).min;
    Vector3 max = ((Bounds) ref _aabb).max;
    if ((double) move.y != 0.0 && num7 > 0)
    {
      for (int index = 0; index < num7; ++index)
      {
        World.ClipBlock clipBlock = this._clipBlocks[index];
        IList<Bounds> clipBoundsList = clipBlock.block.GetClipBoundsList(clipBlock.value, clipBlock.pos);
        move.y = BoundsUtils.ClipBoundsMoveY(clipBlock.bmins, clipBlock.bmaxs, move.y, clipBoundsList, clipBoundsList.Count);
        if ((double) move.y == 0.0)
          break;
      }
    }
    if ((double) move.y != 0.0)
    {
      if (numColliders > 0)
        move.y = BoundsUtils.ClipBoundsMoveY(min, max, move.y, (IList<Bounds>) this._clipBounds, numColliders);
      min.y += move.y;
      max.y += move.y;
      for (int index = 0; index < num7; ++index)
      {
        World.ClipBlock clipBlock = this._clipBlocks[index];
        clipBlock.bmins.y += move.y;
        clipBlock.bmaxs.y += move.y;
      }
    }
    if ((double) move.x != 0.0 && num7 > 0)
    {
      for (int index = 0; index < num7; ++index)
      {
        World.ClipBlock clipBlock = this._clipBlocks[index];
        IList<Bounds> clipBoundsList = clipBlock.block.GetClipBoundsList(clipBlock.value, clipBlock.pos);
        move.x = BoundsUtils.ClipBoundsMoveX(clipBlock.bmins, clipBlock.bmaxs, move.x, clipBoundsList, clipBoundsList.Count);
        if ((double) move.x == 0.0)
          break;
      }
    }
    if ((double) move.x != 0.0)
    {
      if (numColliders > 0)
        move.x = BoundsUtils.ClipBoundsMoveX(min, max, move.x, (IList<Bounds>) this._clipBounds, numColliders);
      min.x += move.x;
      max.x += move.x;
      for (int index = 0; index < num7; ++index)
      {
        World.ClipBlock clipBlock = this._clipBlocks[index];
        clipBlock.bmins.x += move.x;
        clipBlock.bmaxs.x += move.x;
      }
    }
    if ((double) move.z != 0.0 && num7 > 0)
    {
      for (int index = 0; index < num7; ++index)
      {
        World.ClipBlock clipBlock = this._clipBlocks[index];
        IList<Bounds> clipBoundsList = clipBlock.block.GetClipBoundsList(clipBlock.value, clipBlock.pos);
        move.z = BoundsUtils.ClipBoundsMoveZ(clipBlock.bmins, clipBlock.bmaxs, move.z, clipBoundsList, clipBoundsList.Count);
        if ((double) move.z == 0.0)
          break;
      }
    }
    if ((double) move.z != 0.0)
    {
      if (numColliders > 0)
        move.z = BoundsUtils.ClipBoundsMoveZ(min, max, move.z, (IList<Bounds>) this._clipBounds, numColliders);
      min.z += move.z;
      max.z += move.z;
      for (int index = 0; index < num7; ++index)
      {
        World.ClipBlock clipBlock = this._clipBlocks[index];
        clipBlock.bmins.z += move.z;
        clipBlock.bmaxs.z += move.z;
      }
    }
    if ((double) stepHeight > 0.0)
    {
      stepHeight = -stepHeight;
      if (num7 > 0)
      {
        for (int index = 0; index < num7; ++index)
        {
          World.ClipBlock clipBlock = this._clipBlocks[index];
          IList<Bounds> clipBoundsList = clipBlock.block.GetClipBoundsList(clipBlock.value, clipBlock.pos);
          stepHeight = BoundsUtils.ClipBoundsMoveY(clipBlock.bmins, clipBlock.bmaxs, stepHeight, clipBoundsList, clipBoundsList.Count);
          if ((double) stepHeight == 0.0)
            break;
        }
      }
      if ((double) stepHeight != 0.0 && numColliders > 0)
        stepHeight = BoundsUtils.ClipBoundsMoveY(min, max, stepHeight, (IList<Bounds>) this._clipBounds, numColliders);
      move.y += stepHeight;
    }
    return move;
  }

  public List<Bounds> GetCollidingBounds(
    Entity _entity,
    Bounds _aabb,
    List<Bounds> collidingBoundingBoxes)
  {
    int num1 = Utils.Fastfloor(((Bounds) ref _aabb).min.x - 0.5f);
    int num2 = Utils.Fastfloor(((Bounds) ref _aabb).max.x + 0.5f);
    int num3 = Utils.Fastfloor(((Bounds) ref _aabb).min.y - 1f);
    int num4 = Utils.Fastfloor(((Bounds) ref _aabb).max.y + 1f);
    int num5 = Utils.Fastfloor(((Bounds) ref _aabb).min.z - 0.5f);
    int num6 = Utils.Fastfloor(((Bounds) ref _aabb).max.z + 0.5f);
    Chunk _c = (Chunk) null;
    int num7 = num1 - 1;
    int index1 = 0;
    while (num7 <= num2 + 1)
    {
      if (index1 >= 50)
      {
        Log.Warning($"1BB exceeded size {50}: BB={_aabb.ToCultureInvariantString()}");
        return collidingBoundingBoxes;
      }
      int num8 = num5 - 1;
      int index2 = 0;
      while (num8 <= num6 + 1)
      {
        if (index2 >= 50)
        {
          Log.Warning($"2BB exceeded size {50}: BB={_aabb.ToCultureInvariantString()}");
          return collidingBoundingBoxes;
        }
        if (_c == null || _c.X != World.toChunkXZ(num7) || _c.Z != World.toChunkXZ(num8))
        {
          _c = (Chunk) this.GetChunkFromWorldPos(num7, num8);
          if (_c != null)
          {
            if (!this.IsInPlayfield(_c))
              collidingBoundingBoxes.Add(_c.GetAABB());
          }
          else
            goto label_17;
        }
        int blockXz1 = World.toBlockXZ(num7);
        int blockXz2 = World.toBlockXZ(num8);
        int _y = num3;
        int index3 = 0;
        while (_y < num4)
        {
          if (_y > 0 && _y < (int) byte.MaxValue)
          {
            BlockValue block = _c.GetBlock(blockXz1, _y, blockXz2);
            if (index3 >= 50)
            {
              Log.Warning($"3BB exceeded size {50}: BB={_aabb.ToCultureInvariantString()}");
              return collidingBoundingBoxes;
            }
            this.collBlockCache[index1, index3, index2] = block;
            this.collDensityCache[index1, index3, index2] = _c.GetDensity(blockXz1, _y, blockXz2);
          }
          ++_y;
          ++index3;
        }
label_17:
        ++num8;
        ++index2;
      }
      ++num7;
      ++index1;
    }
    int _x = num1;
    int num9 = 0;
    while (_x <= num2)
    {
      if (num9 >= 50)
      {
        Log.Warning($"4BB exceeded size {50}: BB={_aabb.ToCultureInvariantString()}");
        return collidingBoundingBoxes;
      }
      int _z = num5;
      int num10 = 0;
      while (_z <= num6)
      {
        if (num10 >= 50)
        {
          Log.Warning($"5BB exceeded size {50}: BB={_aabb.ToCultureInvariantString()}");
          return collidingBoundingBoxes;
        }
        int _y = num3;
        int index4 = 0;
        while (_y < num4)
        {
          if (_y > 0 && _y < (int) byte.MaxValue)
          {
            if (index4 >= 50)
            {
              Log.Warning($"6BB exceeded size {50}: BB={_aabb.ToCultureInvariantString()}");
              return collidingBoundingBoxes;
            }
            BlockValue _blockValue = this.collBlockCache[num9 + 1, index4, num10 + 1];
            Block block = _blockValue.Block;
            if (block.IsCollideMovement)
            {
              float _distortedAddY = 0.0f;
              if (block.shape.IsTerrain())
                _distortedAddY = MarchingCubes.GetDecorationOffsetY(this.collDensityCache[num9 + 1, index4 + 1, num10 + 1], this.collDensityCache[num9 + 1, index4, num10 + 1]);
              block.GetCollidingAABB(_blockValue, _x, _y, _z, _distortedAddY, _aabb, collidingBoundingBoxes);
            }
          }
          ++_y;
          ++index4;
        }
        ++_z;
        ++num10;
      }
      ++_x;
      ++num9;
    }
    Bounds _aabbOfEntity = _aabb;
    ((Bounds) ref _aabbOfEntity).Expand(0.25f);
    List<Entity> entitiesInBounds = this.GetEntitiesInBounds(_entity, _aabbOfEntity);
    for (int index5 = 0; index5 < entitiesInBounds.Count; ++index5)
    {
      Bounds boundingBox1 = entitiesInBounds[index5].getBoundingBox();
      if (((Bounds) ref boundingBox1).Intersects(_aabb))
        collidingBoundingBoxes.Add(boundingBox1);
      Bounds boundingBox2 = _entity.getBoundingBox();
      if (((Bounds) ref boundingBox2).Intersects(_aabb))
        collidingBoundingBoxes.Add(boundingBox2);
    }
    return collidingBoundingBoxes;
  }

  public List<Entity> GetEntitiesInBounds(Entity _excludeEntity, Bounds _aabbOfEntity)
  {
    this.entitiesWithinAABBExcludingEntity.Clear();
    int num1 = Utils.Fastfloor((float) (((double) ((Bounds) ref _aabbOfEntity).min.x - 5.0) / 16.0));
    int num2 = Utils.Fastfloor((float) (((double) ((Bounds) ref _aabbOfEntity).max.x + 5.0) / 16.0));
    int num3 = Utils.Fastfloor((float) (((double) ((Bounds) ref _aabbOfEntity).min.z - 5.0) / 16.0));
    int num4 = Utils.Fastfloor((float) (((double) ((Bounds) ref _aabbOfEntity).max.z + 5.0) / 16.0));
    for (int _x = num1; _x <= num2; ++_x)
    {
      for (int _y = num3; _y <= num4; ++_y)
        this.ChunkCache.GetChunkSync(_x, _y)?.GetEntitiesInBounds(_excludeEntity, _aabbOfEntity, this.entitiesWithinAABBExcludingEntity, true);
    }
    return this.entitiesWithinAABBExcludingEntity;
  }

  public List<Entity> GetEntitiesInBounds(
    Entity _excludeEntity,
    Bounds _aabbOfEntity,
    bool _isAlive)
  {
    this.entitiesWithinAABBExcludingEntity.Clear();
    int num1 = Utils.Fastfloor((float) (((double) ((Bounds) ref _aabbOfEntity).min.x - 5.0) / 16.0));
    int num2 = Utils.Fastfloor((float) (((double) ((Bounds) ref _aabbOfEntity).max.x + 5.0) / 16.0));
    int num3 = Utils.Fastfloor((float) (((double) ((Bounds) ref _aabbOfEntity).min.z - 5.0) / 16.0));
    int num4 = Utils.Fastfloor((float) (((double) ((Bounds) ref _aabbOfEntity).max.z + 5.0) / 16.0));
    for (int _x = num1; _x <= num2; ++_x)
    {
      for (int _y = num3; _y <= num4; ++_y)
        this.ChunkCache.GetChunkSync(_x, _y)?.GetEntitiesInBounds(_excludeEntity, _aabbOfEntity, this.entitiesWithinAABBExcludingEntity, _isAlive);
    }
    return this.entitiesWithinAABBExcludingEntity;
  }

  public List<EntityAlive> GetLivingEntitiesInBounds(
    EntityAlive _excludeEntity,
    Bounds _aabbOfEntity)
  {
    this.livingEntitiesWithinAABBExcludingEntity.Clear();
    int num1 = Utils.Fastfloor((float) (((double) ((Bounds) ref _aabbOfEntity).min.x - 5.0) / 16.0));
    int num2 = Utils.Fastfloor((float) (((double) ((Bounds) ref _aabbOfEntity).max.x + 5.0) / 16.0));
    int num3 = Utils.Fastfloor((float) (((double) ((Bounds) ref _aabbOfEntity).min.z - 5.0) / 16.0));
    int num4 = Utils.Fastfloor((float) (((double) ((Bounds) ref _aabbOfEntity).max.z + 5.0) / 16.0));
    for (int _x = num1; _x <= num2; ++_x)
    {
      for (int _y = num3; _y <= num4; ++_y)
        this.ChunkCache.GetChunkSync(_x, _y)?.GetLivingEntitiesInBounds(_excludeEntity, _aabbOfEntity, this.livingEntitiesWithinAABBExcludingEntity);
    }
    return this.livingEntitiesWithinAABBExcludingEntity;
  }

  public void GetEntitiesInBounds(FastTags<TagGroup.Global> _tags, Bounds _bb, List<Entity> _list)
  {
    int num1 = Utils.Fastfloor((float) (((double) ((Bounds) ref _bb).min.x - 5.0) / 16.0));
    int num2 = Utils.Fastfloor((float) (((double) ((Bounds) ref _bb).max.x + 5.0) / 16.0));
    int num3 = Utils.Fastfloor((float) (((double) ((Bounds) ref _bb).min.z - 5.0) / 16.0));
    int num4 = Utils.Fastfloor((float) (((double) ((Bounds) ref _bb).max.z + 5.0) / 16.0));
    for (int chunkZ = num3; chunkZ <= num4; ++chunkZ)
    {
      for (int chunkX = num1; chunkX <= num2; ++chunkX)
        ((Chunk) this.GetChunkSync(chunkX, chunkZ))?.GetEntitiesInBounds(_tags, _bb, _list);
    }
  }

  public List<Entity> GetEntitiesInBounds(System.Type _class, Bounds _bb, List<Entity> _list)
  {
    int num1 = Utils.Fastfloor((float) (((double) ((Bounds) ref _bb).min.x - 5.0) / 16.0));
    int num2 = Utils.Fastfloor((float) (((double) ((Bounds) ref _bb).max.x + 5.0) / 16.0));
    int num3 = Utils.Fastfloor((float) (((double) ((Bounds) ref _bb).min.z - 5.0) / 16.0));
    int num4 = Utils.Fastfloor((float) (((double) ((Bounds) ref _bb).max.z + 5.0) / 16.0));
    for (int chunkZ = num3; chunkZ <= num4; ++chunkZ)
    {
      for (int chunkX = num1; chunkX <= num2; ++chunkX)
        ((Chunk) this.GetChunkSync(chunkX, chunkZ))?.GetEntitiesInBounds(_class, _bb, _list);
    }
    return _list;
  }

  public void GetEntitiesAround(
    EntityFlags _mask,
    Vector3 _pos,
    float _radius,
    List<Entity> _list)
  {
    int num1 = Utils.Fastfloor((float) (((double) _pos.x - (double) _radius) / 16.0));
    int num2 = Utils.Fastfloor((float) (((double) _pos.x + (double) _radius) / 16.0));
    int num3 = Utils.Fastfloor((float) (((double) _pos.z - (double) _radius) / 16.0));
    int num4 = Utils.Fastfloor((float) (((double) _pos.z + (double) _radius) / 16.0));
    for (int chunkZ = num3; chunkZ <= num4; ++chunkZ)
    {
      for (int chunkX = num1; chunkX <= num2; ++chunkX)
        ((Chunk) this.GetChunkSync(chunkX, chunkZ))?.GetEntitiesAround(_mask, _pos, _radius, _list);
    }
  }

  public void GetEntitiesAround(
    EntityFlags _flags,
    EntityFlags _mask,
    Vector3 _pos,
    float _radius,
    List<Entity> _list)
  {
    _flags &= _mask;
    int num1 = Utils.Fastfloor((float) (((double) _pos.x - (double) _radius) / 16.0));
    int num2 = Utils.Fastfloor((float) (((double) _pos.x + (double) _radius) / 16.0));
    int num3 = Utils.Fastfloor((float) (((double) _pos.z - (double) _radius) / 16.0));
    int num4 = Utils.Fastfloor((float) (((double) _pos.z + (double) _radius) / 16.0));
    for (int chunkZ = num3; chunkZ <= num4; ++chunkZ)
    {
      for (int chunkX = num1; chunkX <= num2; ++chunkX)
        ((Chunk) this.GetChunkSync(chunkX, chunkZ))?.GetEntitiesAround(_flags, _mask, _pos, _radius, _list);
    }
  }

  public int GetEntityAliveCount(EntityFlags _flags, EntityFlags _mask)
  {
    int entityAliveCount = 0;
    int count = this.EntityAlives.Count;
    for (int index = 0; index < count; ++index)
    {
      if ((this.EntityAlives[index].entityFlags & _mask) == _flags)
        ++entityAliveCount;
    }
    return entityAliveCount;
  }

  public void GetPlayersAround(Vector3 _pos, float _radius, List<EntityPlayer> _list)
  {
    float num = _radius * _radius;
    for (int index = this.Players.list.Count - 1; index >= 0; --index)
    {
      EntityPlayer entityPlayer = this.Players.list[index];
      Vector3 vector3 = Vector3.op_Subtraction(entityPlayer.position, _pos);
      if ((double) ((Vector3) ref vector3).sqrMagnitude <= (double) num)
        _list.Add(entityPlayer);
    }
  }

  public void SetEntitiesVisibleNearToLocalPlayer()
  {
    EntityPlayerLocal primaryPlayer = this.GetPrimaryPlayer();
    if (Object.op_Equality((Object) primaryPlayer, (Object) null))
      return;
    bool aimingGun = primaryPlayer.AimingGun;
    Vector3 vector3_1 = Vector3.op_Addition(primaryPlayer.cameraTransform.position, Origin.position);
    for (int index = this.Entities.list.Count - 1; index >= 0; --index)
    {
      Entity entity = this.Entities.list[index];
      if (Object.op_Inequality((Object) entity, (Object) primaryPlayer))
      {
        Vector3 vector3_2 = Vector3.op_Subtraction(entity.position, vector3_1);
        entity.VisiblityCheck(((Vector3) ref vector3_2).sqrMagnitude, aimingGun);
      }
    }
  }

  public void TickEntities(float _partialTicks)
  {
    int frameCount = Time.frameCount;
    int num1 = frameCount - this.tickEntityFrameCount;
    if (num1 <= 0)
      num1 = 1;
    this.tickEntityFrameCount = frameCount;
    this.tickEntityFrameCountAverage = (float) ((double) this.tickEntityFrameCountAverage * 0.800000011920929 + (double) num1 * 0.20000000298023224);
    this.tickEntityPartialTicks = _partialTicks;
    this.tickEntityIndex = 0;
    this.tickEntityList.Clear();
    Entity primaryPlayer = (Entity) this.GetPrimaryPlayer();
    int count = this.Entities.list.Count;
    for (int index = 0; index < count; ++index)
    {
      Entity entity = this.Entities.list[index];
      if (Object.op_Inequality((Object) entity, (Object) primaryPlayer))
        this.tickEntityList.Add(entity);
    }
    if (Object.op_Implicit((Object) primaryPlayer))
      this.TickEntity(primaryPlayer, _partialTicks);
    this.EntityActivityUpdate();
    int num2 = (int) ((double) this.tickEntityFrameCountAverage + 0.40000000596046448) - 1;
    if (num2 <= 0)
    {
      this.TickEntitiesFlush();
    }
    else
    {
      int num3 = (this.tickEntityList.Count - 25) / (num2 + 1);
      if (num3 < 0)
        num3 = 0;
      this.tickEntitySliceCount = (this.tickEntityList.Count - num3) / num2 + 1;
    }
  }

  public void TickEntitiesFlush() => this.TickEntitiesSlice(this.tickEntityList.Count);

  public void TickEntitiesSlice() => this.TickEntitiesSlice(this.tickEntitySliceCount);

  [PublicizedFrom(EAccessModifier.Private)]
  public void TickEntitiesSlice(int count)
  {
    int num = Utils.FastMin(this.tickEntityIndex + count, this.tickEntityList.Count);
    for (int tickEntityIndex = this.tickEntityIndex; tickEntityIndex < num; ++tickEntityIndex)
    {
      Entity tickEntity = this.tickEntityList[tickEntityIndex];
      if (Object.op_Implicit((Object) tickEntity))
        this.TickEntity(tickEntity, this.tickEntityPartialTicks);
    }
    this.tickEntityIndex = num;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void TickEntity(Entity e, float _partialTicks)
  {
    e.SetLastTickPos(e.position);
    e.OnUpdatePosition(_partialTicks);
    e.CheckPosition();
    if (e.IsSpawned() && !e.IsMarkedForUnload())
    {
      Chunk chunkSync1 = (Chunk) this.GetChunkSync(e.chunkPosAddedEntityTo.x, e.chunkPosAddedEntityTo.z);
      bool flag = false;
      if (chunkSync1 != null)
      {
        if (!chunkSync1.hasEntities)
          flag = true;
        else
          chunkSync1.AdJustEntityTracking(e);
      }
      int chunkXz1 = World.toChunkXZ(Utils.Fastfloor(e.position.x));
      int chunkXz2 = World.toChunkXZ(Utils.Fastfloor(e.position.z));
      if (flag || !e.addedToChunk || e.chunkPosAddedEntityTo.x != chunkXz1 || e.chunkPosAddedEntityTo.z != chunkXz2)
      {
        if (e.addedToChunk && chunkSync1 != null)
          chunkSync1.RemoveEntityFromChunk(e);
        Chunk chunkSync2 = (Chunk) this.GetChunkSync(chunkXz1, chunkXz2);
        if (chunkSync2 != null)
        {
          e.addedToChunk = true;
          chunkSync2.AddEntityToChunk(e);
        }
        else
          e.addedToChunk = false;
      }
      if (e is EntityPlayer || this.IsChunkAreaLoaded(e.position))
      {
        if (e.CanUpdateEntity())
          e.OnUpdateEntity();
        else if (e is EntityAlive)
          ((EntityAlive) e).CheckDespawn();
      }
      else
      {
        EntityAlive entityAlive = e as EntityAlive;
        if (Object.op_Inequality((Object) entityAlive, (Object) null))
        {
          entityAlive.SetAttackTarget((EntityAlive) null, 0);
          entityAlive.CheckDespawn();
        }
      }
    }
    if (!e.IsMarkedForUnload() || e.isEntityRemote || e.bWillRespawn)
      return;
    this.unloadEntity(e, e.IsDespawned ? EnumRemoveEntityReason.Despawned : EnumRemoveEntityReason.Killed);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void TickEntityRemove(Entity e)
  {
    int index = this.tickEntityList.IndexOf(e);
    if (index < this.tickEntityIndex)
      return;
    this.tickEntityList[index] = (Entity) null;
  }

  public void EntityActivityUpdate()
  {
    List<EntityPlayer> list = this.Players.list;
    if (list.Count == 0)
      return;
    for (int index = list.Count - 1; index >= 0; --index)
      list[index].aiClosest.Clear();
    int count = this.EntityAlives.Count;
    Vector3 vector3_1;
    for (int index = 0; index < count; ++index)
    {
      EntityAlive entityAlive1 = this.EntityAlives[index];
      EntityPlayer closestPlayer = this.GetClosestPlayer(entityAlive1.position, -1f, false);
      if (Object.op_Implicit((Object) closestPlayer))
      {
        closestPlayer.aiClosest.Add(entityAlive1);
        entityAlive1.aiClosestPlayer = closestPlayer;
        EntityAlive entityAlive2 = entityAlive1;
        vector3_1 = Vector3.op_Subtraction(closestPlayer.position, entityAlive1.position);
        double sqrMagnitude = (double) ((Vector3) ref vector3_1).sqrMagnitude;
        entityAlive2.aiClosestPlayerDistSq = (float) sqrMagnitude;
      }
      else
      {
        entityAlive1.aiClosestPlayer = (EntityPlayer) null;
        entityAlive1.aiClosestPlayerDistSq = float.MaxValue;
      }
    }
    Vector3 vector3_2 = Vector3.zero;
    float num1 = 0.0f;
    if (Object.op_Implicit((Object) this.m_LocalPlayerEntity))
    {
      vector3_2 = Vector3.op_Addition(this.m_LocalPlayerEntity.cameraTransform.position, Origin.position);
      this.m_LocalPlayerEntity.emodel.ClothSimOn(!Object.op_Implicit((Object) this.m_LocalPlayerEntity.AttachedToEntity));
      num1 = 625f;
      if (this.m_LocalPlayerEntity.AimingGun)
        num1 = 3025f;
    }
    int num2 = Utils.FastClamp(60 / list.Count, 4, 20);
    for (int index1 = list.Count - 1; index1 >= 0; --index1)
    {
      EntityPlayer entityPlayer = list[index1];
      entityPlayer.aiClosest.Sort((Comparison<EntityAlive>) ([PublicizedFrom(EAccessModifier.Internal)] (e1, e2) => e1.aiClosestPlayerDistSq.CompareTo(e2.aiClosestPlayerDistSq)));
      for (int index2 = 0; index2 < entityPlayer.aiClosest.Count; ++index2)
      {
        EntityAlive entityAlive = entityPlayer.aiClosest[index2];
        if (index2 < num2 || (double) entityAlive.aiClosestPlayerDistSq < 64.0)
        {
          entityAlive.aiActiveScale = 1f;
          bool _on = (double) entityAlive.aiClosestPlayerDistSq < 36.0;
          entityAlive.emodel.JiggleOn(_on);
        }
        else
        {
          float num3 = (double) entityAlive.aiClosestPlayerDistSq < 225.0 ? 0.3f : 0.1f;
          entityAlive.aiActiveScale = num3;
          entityAlive.emodel.JiggleOn(false);
        }
      }
      if (Object.op_Inequality((Object) entityPlayer, (Object) this.m_LocalPlayerEntity))
      {
        int num4;
        if (!Object.op_Implicit((Object) entityPlayer.AttachedToEntity))
        {
          vector3_1 = Vector3.op_Subtraction(entityPlayer.position, vector3_2);
          num4 = (double) ((Vector3) ref vector3_1).sqrMagnitude < (double) num1 ? 1 : 0;
        }
        else
          num4 = 0;
        bool _on = num4 != 0;
        entityPlayer.emodel.ClothSimOn(_on);
      }
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void addToChunk(Entity e)
  {
    if (e.addedToChunk)
      return;
    ((Chunk) this.GetChunkFromWorldPos(e.GetBlockPosition()))?.AddEntityToChunk(e);
  }

  public override void UnloadEntities(List<Entity> _entityList, bool _forceUnload = false)
  {
    for (int index = _entityList.Count - 1; index >= 0; --index)
    {
      Entity entity = _entityList[index];
      if (_forceUnload || !entity.bWillRespawn && (!Object.op_Inequality((Object) entity.AttachedMainEntity, (Object) null) || !entity.AttachedMainEntity.bWillRespawn))
        this.unloadEntity(entity, EnumRemoveEntityReason.Unloaded);
    }
  }

  public override Entity RemoveEntity(int _entityId, EnumRemoveEntityReason _reason)
  {
    Entity entity = this.GetEntity(_entityId);
    if (Object.op_Inequality((Object) entity, (Object) null))
    {
      entity.MarkToUnload();
      this.unloadEntity(entity, _reason);
    }
    return entity;
  }

  public void unloadEntity(Entity _e, EnumRemoveEntityReason _reason)
  {
    EnumRemoveEntityReason unloadReason = _e.unloadReason;
    _e.unloadReason = _reason;
    if (!this.Entities.dict.ContainsKey(_e.entityId))
    {
      Log.Warning("{0} World unloadEntity !dict {1}, {2}, was {3}", new object[4]
      {
        (object) GameManager.frameCount,
        (object) _e,
        (object) _reason,
        (object) unloadReason
      });
    }
    else
    {
      if (this.EntityUnloadedDelegates != null)
        this.EntityUnloadedDelegates(_e, _reason);
      if (_e.NavObject != null)
      {
        if (_reason == EnumRemoveEntityReason.Unloaded && _e is EntitySupplyCrate)
          _e.NavObject.PauseEntityUpdate();
        else
          NavObjectManager.Instance.UnRegisterNavObject(_e.NavObject);
      }
      _e.OnEntityUnload();
      this.Entities.Remove(_e.entityId);
      this.TickEntityRemove(_e);
      EntityAlive entityAlive = _e as EntityAlive;
      if (Object.op_Implicit((Object) entityAlive))
        this.EntityAlives.Remove(entityAlive);
      this.RemoveEntityFromMap(_e, _reason);
      if (_e.addedToChunk && _e.IsMarkedForUnload())
      {
        Chunk chunkSync = (Chunk) this.GetChunkSync(_e.chunkPosAddedEntityTo.x, _e.chunkPosAddedEntityTo.z);
        if (chunkSync != null && !chunkSync.InProgressUnloading)
          chunkSync.RemoveEntityFromChunk(_e);
      }
      if (!this.IsRemote())
      {
        if (VehicleManager.Instance != null)
        {
          EntityVehicle _vehicle = _e as EntityVehicle;
          if (Object.op_Implicit((Object) _vehicle))
            VehicleManager.Instance.RemoveTrackedVehicle(_vehicle, _reason);
        }
        if (DroneManager.Instance != null)
        {
          EntityDrone _drone = _e as EntityDrone;
          if (Object.op_Implicit((Object) _drone))
            DroneManager.Instance.RemoveTrackedDrone(_drone, _reason);
        }
        if (TurretTracker.Instance != null)
        {
          EntityTurret _turret = _e as EntityTurret;
          if (Object.op_Implicit((Object) _turret))
            TurretTracker.Instance.RemoveTrackedTurret(_turret, _reason);
        }
      }
      if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
        this.entityDistributer.Remove(_e, _reason);
      if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && _e is EntityAlive && PathFinderThread.Instance != null)
        PathFinderThread.Instance.RemovePathsFor(_e.entityId);
      if (_e is EntityPlayer)
      {
        this.Players.Remove(_e.entityId);
        this.gameManager.HandlePersistentPlayerDisconnected(_e.entityId);
        ++this.playerEntityUpdateCount;
        NavObjectManager.Instance.UnRegisterNavObjectByOwnerEntity(_e, "sleeping_bag");
      }
      if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
        this.aiDirector.RemoveEntity(_e);
      this.audioManager.EntityRemovedFromWorld(_e, this);
      WeatherManager.EntityRemovedFromWorld(_e);
      LightManager.EntityRemovedFromWorld(_e, this);
    }
  }

  public override Entity GetEntity(int _entityId)
  {
    Entity entity;
    this.Entities.dict.TryGetValue(_entityId, out entity);
    return entity;
  }

  public override void ChangeClientEntityIdToServer(int _clientEntityId, int _serverEntityId)
  {
    Entity entity = this.GetEntity(_clientEntityId);
    if (!Object.op_Implicit((Object) entity))
      return;
    this.Entities.Remove(_clientEntityId);
    entity.entityId = _serverEntityId;
    entity.clientEntityId = 0;
    this.Entities.Add(_serverEntityId, entity);
  }

  public void SpawnEntityInWorld(Entity _entity)
  {
    if (Object.op_Equality((Object) _entity, (Object) null))
    {
      Log.Warning("Ignore spawning of empty entity");
    }
    else
    {
      if (this.EntityLoadedDelegates != null)
        this.EntityLoadedDelegates(_entity);
      this.AddEntityToMap(_entity);
      this.Entities.Add(_entity.entityId, _entity);
      this.addToChunk(_entity);
      EntityPlayer entityPlayer = _entity as EntityPlayer;
      EntityAlive alive = !Object.op_Implicit((Object) entityPlayer) ? _entity as EntityAlive : (EntityAlive) null;
      if (Object.op_Implicit((Object) alive))
        this.EntityAlives.Add(alive);
      if (!this.IsRemote())
      {
        if (_entity is EntityVehicle _vehicle && VehicleManager.Instance != null)
          VehicleManager.Instance.AddTrackedVehicle(_vehicle);
        if (_entity is EntityDrone _drone)
        {
          if (DroneManager.Instance != null)
            DroneManager.Instance.AddTrackedDrone(_drone);
          if (_drone.OriginalItemValue == null)
            _drone.InitDynamicSpawn();
        }
        if (_entity is EntityTurret _turret)
        {
          if (TurretTracker.Instance != null)
            TurretTracker.Instance.AddTrackedTurret(_turret);
          if (_turret.OriginalItemValue.ItemClass == null)
            _turret.InitDynamicSpawn();
        }
      }
      if (this.audioManager != null)
        this.audioManager.EntityAddedToWorld(_entity, this);
      WeatherManager.EntityAddedToWorld(_entity);
      LightManager.EntityAddedToWorld(_entity, this);
      _entity.OnAddedToWorld();
      if ((double) _entity.position.y < 1.0)
        Log.Warning($"Spawned entity with wrong pos: {((object) _entity)?.ToString()} id={_entity.entityId.ToString()} pos={_entity.position.ToCultureInvariantString()}");
      if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
        this.entityDistributer.Add(_entity);
      if (Object.op_Implicit((Object) entityPlayer))
      {
        this.Players.Add(_entity.entityId, entityPlayer);
        ++this.playerEntityUpdateCount;
      }
      else if (Object.op_Implicit((Object) alive))
      {
        alive.Spawned = true;
        GameEventManager.Current.HandleSpawnModifier(alive);
      }
      if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
        return;
      this.aiDirector.AddEntity(_entity);
    }
  }

  public void AddEntityToMap(Entity _entity)
  {
    if (Object.op_Equality((Object) _entity, (Object) null) || !_entity.HasUIIcon() || _entity.GetMapObjectType() != EnumMapObjectType.Entity)
      return;
    switch (_entity)
    {
      case EntityVehicle _vehicle:
        EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
        if (!Object.op_Inequality((Object) primaryPlayer, (Object) null))
          break;
        LocalPlayerUI uiForPlayer = LocalPlayerUI.GetUIForPlayer(primaryPlayer);
        if (!Object.op_Inequality((Object) uiForPlayer, (Object) null) || !Object.op_Inequality((Object) uiForPlayer.xui, (Object) null) || uiForPlayer.xui.GetWindow("mapArea") == null)
          break;
        ((XUiC_MapArea) uiForPlayer.xui.GetWindow("mapArea").Controller).RefreshVehiclePositionWaypoint(_vehicle, false);
        break;
      case EntityEnemy _:
      case EntityEnemyAnimal _:
        this.ObjectOnMapAdd((MapObject) new MapObjectZombie(_entity));
        break;
      case EntityAnimal _:
        this.ObjectOnMapAdd((MapObject) new MapObjectAnimal(_entity));
        break;
      default:
        this.ObjectOnMapAdd(new MapObject(EnumMapObjectType.Entity, Vector3.zero, (long) _entity.entityId, _entity, false));
        break;
    }
  }

  public void RemoveEntityFromMap(Entity _entity, EnumRemoveEntityReason _reason)
  {
    if (Object.op_Equality((Object) _entity, (Object) null))
      return;
    EnumMapObjectType mapObjectType = _entity.GetMapObjectType();
    if (mapObjectType == EnumMapObjectType.SupplyDrop)
    {
      if (_reason != EnumRemoveEntityReason.Killed)
        return;
      this.ObjectOnMapRemove(_entity.GetMapObjectType(), _entity.entityId);
    }
    else
    {
      if (_entity is EntityVehicle)
      {
        EntityVehicle _vehicle = _entity as EntityVehicle;
        EntityPlayerLocal primaryPlayer = this.GetPrimaryPlayer();
        if (Object.op_Inequality((Object) primaryPlayer, (Object) null))
        {
          LocalPlayerUI uiForPlayer = LocalPlayerUI.GetUIForPlayer(primaryPlayer);
          if (Object.op_Inequality((Object) uiForPlayer, (Object) null))
          {
            switch (_reason)
            {
              case EnumRemoveEntityReason.Unloaded:
                if (_vehicle.GetOwner() != null && _vehicle.LocalPlayerIsOwner())
                {
                  ((XUiC_MapArea) uiForPlayer.xui.GetWindow("mapArea").Controller).RefreshVehiclePositionWaypoint(_vehicle, true);
                  break;
                }
                break;
              case EnumRemoveEntityReason.Killed:
                ((XUiC_MapArea) uiForPlayer.xui.GetWindow("mapArea").Controller).RemoveVehicleLastKnownWaypoint(_vehicle);
                break;
            }
          }
        }
      }
      this.ObjectOnMapRemove(mapObjectType, _entity.entityId);
    }
  }

  public void RefreshEntitiesOnMap()
  {
    foreach (Entity _entity in this.Entities.list)
    {
      this.RemoveEntityFromMap(_entity, EnumRemoveEntityReason.Undef);
      this.AddEntityToMap(_entity);
    }
  }

  public void LockAreaMasterChunksAround(Vector3i _blockPos, ulong _worldTimeToLock)
  {
    for (int index1 = -2; index1 <= 2; ++index1)
    {
      for (int index2 = -2; index2 <= 2; ++index2)
      {
        Vector3i areaMasterChunkPos = Chunk.ToAreaMasterChunkPos(new Vector3i(_blockPos.x + index1 * 80 /*0x50*/, 0, _blockPos.z + index2 * 80 /*0x50*/));
        Chunk chunkSync = (Chunk) this.GetChunkSync(areaMasterChunkPos.x, areaMasterChunkPos.z);
        if (chunkSync != null && chunkSync.GetChunkBiomeSpawnData() != null)
          chunkSync.isModified |= chunkSync.GetChunkBiomeSpawnData().DelayAllEnemySpawningUntil(_worldTimeToLock, this.Biomes);
        else
          this.areaMasterChunksToLock[WorldChunkCache.MakeChunkKey(areaMasterChunkPos.x, areaMasterChunkPos.z)] = _worldTimeToLock;
      }
    }
  }

  public bool IsWaterInBounds(Bounds _aabb)
  {
    Vector3 min = ((Bounds) ref _aabb).min;
    Vector3 max = ((Bounds) ref _aabb).max;
    int num1 = Utils.Fastfloor(min.x);
    int num2 = Utils.Fastfloor(max.x + 1f);
    int num3 = Utils.Fastfloor(min.y);
    int num4 = Utils.Fastfloor(max.y + 1f);
    int num5 = Utils.Fastfloor(min.z);
    int num6 = Utils.Fastfloor(max.z + 1f);
    for (int _x = num1; _x < num2; ++_x)
    {
      for (int _y = num3; _y < num4; ++_y)
      {
        for (int _z = num5; _z < num6; ++_z)
        {
          if (this.IsWater(_x, _y, _z))
            return true;
        }
      }
    }
    return false;
  }

  public bool IsMaterialInBounds(Bounds _aabb, MaterialBlock _material)
  {
    int num1 = Utils.Fastfloor(((Bounds) ref _aabb).min.x);
    int num2 = Utils.Fastfloor(((Bounds) ref _aabb).max.x + 1f);
    int num3 = Utils.Fastfloor(((Bounds) ref _aabb).min.y);
    int num4 = Utils.Fastfloor(((Bounds) ref _aabb).max.y + 1f);
    int num5 = Utils.Fastfloor(((Bounds) ref _aabb).min.z);
    int num6 = Utils.Fastfloor(((Bounds) ref _aabb).max.z + 1f);
    for (int _x = num1; _x < num2; ++_x)
    {
      for (int _y = num3; _y < num4; ++_y)
      {
        for (int _z = num5; _z < num6; ++_z)
        {
          if (this.GetBlock(_x, _y, _z).Block.blockMaterial == _material)
            return true;
        }
      }
    }
    return false;
  }

  public Dictionary<Vector3i, float> fallingBlocksHashSet => this.fallingBlocksMap;

  public override void AddFallingBlocks(IList<Vector3i> _list)
  {
    for (int index = 0; index < _list.Count; ++index)
      this.AddFallingBlock(_list[index]);
  }

  public void AddFallingBlock(Vector3i _blockPos, bool includeOversized = false)
  {
    if (this.fallingBlocksMap.ContainsKey(_blockPos))
      return;
    BlockValue block = this.GetBlock(_blockPos);
    if (block.ischild || block.Block.StabilityIgnore || block.isair || !includeOversized && block.Block.isOversized)
      return;
    DynamicMeshManager.AddFallingBlockObserver(_blockPos);
    this.fallingBlocks.Enqueue(_blockPos);
    this.fallingBlocksMap[_blockPos] = Time.time;
  }

  public void LetBlocksFall()
  {
    if (this.fallingBlocks.Count == 0)
      return;
    int num = 0;
    Vector3i zero = Vector3i.zero;
    while (this.fallingBlocks.Count > 0 && num < 2)
    {
      Vector3i vector3i = this.fallingBlocks.Dequeue();
      if (zero.Equals(vector3i))
      {
        this.fallingBlocks.Enqueue(vector3i);
        break;
      }
      this.fallingBlocksMap.Remove(vector3i);
      BlockValue block1 = this.GetBlock(vector3i.x, vector3i.y, vector3i.z);
      if (!block1.isair)
      {
        TextureFullArray textureFullArray = this.GetTextureFullArray(vector3i.x, vector3i.y, vector3i.z);
        Block block2 = block1.Block;
        block2.OnBlockStartsToFall((WorldBase) this, vector3i, block1);
        DynamicMeshManager.ChunkChanged(vector3i, -1, block1.type);
        if (block2.ShowModelOnFall())
        {
          Vector3 _transformPos;
          // ISSUE: explicit constructor call
          ((Vector3) ref _transformPos).\u002Ector((float) vector3i.x + 0.5f + this.RandomRange(-0.1f, 0.1f), (float) vector3i.y + 0.5f, (float) vector3i.z + 0.5f + this.RandomRange(-0.1f, 0.1f));
          this.SpawnEntityInWorld(EntityFactory.CreateEntity(EntityClass.FromString("fallingBlock"), -1, block1, textureFullArray, 1, _transformPos, Vector3.zero, -1f, -1, (string) null));
          ++num;
        }
      }
    }
  }

  public override IGameManager GetGameManager() => this.gameManager;

  public override Manager GetAudioManager() => this.audioManager;

  public override AIDirector GetAIDirector() => this.aiDirector;

  public override bool IsRemote() => !SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer;

  public EntityPlayer GetClosestPlayer(
    float _x,
    float _y,
    float _z,
    int _notFromThisTeam,
    double _maxDistance)
  {
    float num = -1f;
    EntityPlayer closestPlayer = (EntityPlayer) null;
    for (int index = 0; index < this.Players.list.Count; ++index)
    {
      EntityPlayer entityPlayer = this.Players.list[index];
      if (!entityPlayer.IsDead() && entityPlayer.Spawned && (_notFromThisTeam == 0 || entityPlayer.TeamNumber != _notFromThisTeam))
      {
        float distanceSq = entityPlayer.GetDistanceSq(new Vector3(_x, _y, _z));
        if ((_maxDistance < 0.0 || (double) distanceSq < _maxDistance * _maxDistance) && ((double) num == -1.0 || (double) distanceSq < (double) num))
        {
          num = distanceSq;
          closestPlayer = entityPlayer;
        }
      }
    }
    return closestPlayer;
  }

  public EntityPlayer GetClosestPlayer(Entity _entity, float _distMax, bool _isDead)
  {
    return this.GetClosestPlayer(_entity.position, _distMax, _isDead);
  }

  public EntityPlayer GetClosestPlayer(Vector3 _pos, float _distMax, bool _isDead)
  {
    if ((double) _distMax < 0.0)
      _distMax = float.MaxValue;
    float num1 = _distMax * _distMax;
    EntityPlayer closestPlayer = (EntityPlayer) null;
    float num2 = float.MaxValue;
    for (int index = this.Players.list.Count - 1; index >= 0; --index)
    {
      EntityPlayer entityPlayer = this.Players.list[index];
      if (entityPlayer.IsDead() == _isDead && entityPlayer.Spawned)
      {
        float distanceSq = entityPlayer.GetDistanceSq(_pos);
        if ((double) distanceSq < (double) num2 && (double) distanceSq <= (double) num1)
        {
          num2 = distanceSq;
          closestPlayer = entityPlayer;
        }
      }
    }
    return closestPlayer;
  }

  public EntityPlayer GetClosestPlayerSeen(EntityAlive _entity, float _distMax, float lightMin)
  {
    Vector3 position = _entity.position;
    if ((double) _distMax < 0.0)
      _distMax = float.MaxValue;
    float num1 = _distMax * _distMax;
    EntityPlayer closestPlayerSeen = (EntityPlayer) null;
    float num2 = float.MaxValue;
    for (int index = this.Players.list.Count - 1; index >= 0; --index)
    {
      EntityPlayer _other = this.Players.list[index];
      if (!_other.IsDead() && _other.Spawned)
      {
        float distanceSq = _other.GetDistanceSq(position);
        if ((double) distanceSq < (double) num2 && (double) distanceSq <= (double) num1 && (double) _other.Stealth.lightLevel >= (double) lightMin && _entity.CanSee((EntityAlive) _other))
        {
          num2 = distanceSq;
          closestPlayerSeen = _other;
        }
      }
    }
    return closestPlayerSeen;
  }

  public bool IsPlayerAliveAndNear(Vector3 _pos, float _distMax)
  {
    float num = _distMax * _distMax;
    for (int index = this.Players.list.Count - 1; index >= 0; --index)
    {
      EntityPlayer entityPlayer = this.Players.list[index];
      if (!entityPlayer.IsDead() && entityPlayer.Spawned)
      {
        Vector3 vector3 = Vector3.op_Subtraction(entityPlayer.position, _pos);
        if ((double) ((Vector3) ref vector3).sqrMagnitude <= (double) num)
          return true;
      }
    }
    return false;
  }

  public override WorldBlockTicker GetWBT() => this.worldBlockTicker;

  public override bool IsOpenSkyAbove(int _clrIdx, int _x, int _y, int _z)
  {
    return this.ChunkClusters[_clrIdx] == null || ((Chunk) this.GetChunkSync(_x >> 4, _z >> 4)).IsOpenSkyAbove(_x & 15, _y, _z & 15);
  }

  public override bool IsEditor() => GameManager.Instance.IsEditMode();

  public int GetGameMode() => GameStats.GetInt(EnumGameStats.GameModeId);

  public SpawnManagerDynamic GetDynamiceSpawnManager() => this.dynamicSpawnManager;

  public override bool CanPlaceLandProtectionBlockAt(
    Vector3i blockPos,
    PersistentPlayerData lpRelative)
  {
    if (GameStats.GetInt(EnumGameStats.GameModeId) != 1)
      return true;
    if ((double) this.InBoundsForPlayersPercent(blockPos.ToVector3CenterXZ()) < 0.5)
      return false;
    this.m_lpChunkList.Clear();
    int claimSize = GameStats.GetInt(EnumGameStats.LandClaimSize) - 1;
    int deadZone = GameStats.GetInt(EnumGameStats.LandClaimDeadZone) + claimSize;
    int num1 = deadZone / 16 /*0x10*/ + 1;
    int num2 = deadZone / 16 /*0x10*/ + 1;
    for (int index1 = -num1; index1 <= num1; ++index1)
    {
      int _x = blockPos.x + index1 * 16 /*0x10*/;
      for (int index2 = -num2; index2 <= num2; ++index2)
      {
        int _z = blockPos.z + index2 * 16 /*0x10*/;
        Chunk chunkFromWorldPos = (Chunk) this.GetChunkFromWorldPos(new Vector3i(_x, blockPos.y, _z));
        if (chunkFromWorldPos != null && !this.m_lpChunkList.Contains(chunkFromWorldPos))
        {
          this.m_lpChunkList.Add(chunkFromWorldPos);
          if (this.IsLandProtectedBlock(chunkFromWorldPos, blockPos, lpRelative, claimSize, deadZone, true))
          {
            this.m_lpChunkList.Clear();
            return false;
          }
        }
      }
    }
    int num3 = deadZone / 2;
    Vector3i vector3i = new Vector3i(num3, num3, num3);
    if (this.IsWithinTraderArea(blockPos - vector3i, blockPos + vector3i))
      return false;
    this.m_lpChunkList.Clear();
    return true;
  }

  public bool IsEmptyPosition(Vector3i blockPos)
  {
    if (this.IsWithinTraderArea(blockPos))
      return false;
    if (GameStats.GetInt(EnumGameStats.GameModeId) != 1)
      return true;
    this.m_lpChunkList.Clear();
    int num1 = GameStats.GetInt(EnumGameStats.LandClaimSize);
    int num2 = (num1 - 1) / 2;
    int num3 = num1 / 16 /*0x10*/ + 1;
    int num4 = num1 / 16 /*0x10*/ + 1;
    int num5 = blockPos.x - num2;
    int num6 = blockPos.z - num2;
    for (int index1 = -num3; index1 <= num3; ++index1)
    {
      int _x = num5 + index1 * 16 /*0x10*/;
      for (int index2 = -num4; index2 <= num4; ++index2)
      {
        int _z = num6 + index2 * 16 /*0x10*/;
        Chunk chunkFromWorldPos = (Chunk) this.GetChunkFromWorldPos(new Vector3i(_x, blockPos.y, _z));
        if (chunkFromWorldPos != null && !this.m_lpChunkList.Contains(chunkFromWorldPos))
        {
          this.m_lpChunkList.Add(chunkFromWorldPos);
          if (this.IsLandProtectedBlock(chunkFromWorldPos, blockPos, (PersistentPlayerData) null, num2, num2, false))
          {
            this.m_lpChunkList.Clear();
            return false;
          }
        }
      }
    }
    this.m_lpChunkList.Clear();
    return true;
  }

  public override bool CanPickupBlockAt(Vector3i blockPos, PersistentPlayerData lpRelative)
  {
    return !this.IsWithinTraderArea(blockPos) && this.CanPlaceBlockAt(blockPos, lpRelative, false);
  }

  public override bool CanPlaceBlockAt(
    Vector3i blockPos,
    PersistentPlayerData lpRelative,
    bool traderAllowed = false)
  {
    if (!traderAllowed && this.IsWithinTraderArea(blockPos) || (double) this.InBoundsForPlayersPercent(blockPos.ToVector3CenterXZ()) < 0.5)
      return false;
    if (GameStats.GetInt(EnumGameStats.GameModeId) != 1)
      return true;
    this.m_lpChunkList.Clear();
    int num1 = GameStats.GetInt(EnumGameStats.LandClaimSize);
    int num2 = (num1 - 1) / 2;
    int num3 = num1 / 16 /*0x10*/ + 1;
    int num4 = num1 / 16 /*0x10*/ + 1;
    int num5 = blockPos.x - num2;
    int num6 = blockPos.z - num2;
    for (int index1 = -num3; index1 <= num3; ++index1)
    {
      int _x = num5 + index1 * 16 /*0x10*/;
      for (int index2 = -num4; index2 <= num4; ++index2)
      {
        int _z = num6 + index2 * 16 /*0x10*/;
        Chunk chunkFromWorldPos = (Chunk) this.GetChunkFromWorldPos(new Vector3i(_x, blockPos.y, _z));
        if (chunkFromWorldPos != null && !this.m_lpChunkList.Contains(chunkFromWorldPos))
        {
          this.m_lpChunkList.Add(chunkFromWorldPos);
          if (this.IsLandProtectedBlock(chunkFromWorldPos, blockPos, lpRelative, num2, num2, false))
          {
            this.m_lpChunkList.Clear();
            return false;
          }
        }
      }
    }
    this.m_lpChunkList.Clear();
    return true;
  }

  public override float GetLandProtectionHardnessModifier(
    Vector3i blockPos,
    EntityAlive lpRelative,
    PersistentPlayerData ppData)
  {
    if (GameStats.GetInt(EnumGameStats.GameModeId) != 1 || lpRelative is EntityEnemy || Object.op_Equality((Object) lpRelative, (Object) null))
      return 1f;
    float num1 = 1f;
    BlockValue block = this.GetBlock(blockPos);
    if (!block.Equals(BlockValue.Air))
    {
      num1 = block.Block.LPHardnessScale;
      if ((double) num1 == 0.0)
        return 1f;
    }
    this.m_lpChunkList.Clear();
    int num2 = GameStats.GetInt(EnumGameStats.LandClaimSize);
    int halfClaimSize = (num2 - 1) / 2;
    int num3 = num2 / 16 /*0x10*/ + 1;
    int num4 = num2 / 16 /*0x10*/ + 1;
    int num5 = blockPos.x - halfClaimSize;
    int num6 = blockPos.z - halfClaimSize;
    float val1 = 1f;
    for (int index1 = -num3; index1 <= num3; ++index1)
    {
      int _x = num5 + index1 * 16 /*0x10*/;
      for (int index2 = -num4; index2 <= num4; ++index2)
      {
        int _z = num6 + index2 * 16 /*0x10*/;
        Chunk chunkFromWorldPos = (Chunk) this.GetChunkFromWorldPos(new Vector3i(_x, blockPos.y, _z));
        if (chunkFromWorldPos != null && !this.m_lpChunkList.Contains(chunkFromWorldPos))
        {
          this.m_lpChunkList.Add(chunkFromWorldPos);
          float hardnessModifier = this.GetLandProtectionHardnessModifier(chunkFromWorldPos, blockPos, ppData, halfClaimSize);
          if ((double) hardnessModifier < 1.0)
          {
            this.m_lpChunkList.Clear();
            return hardnessModifier;
          }
          val1 = Math.Max(val1, hardnessModifier);
        }
      }
    }
    this.m_lpChunkList.Clear();
    if ((double) val1 <= 1.0)
      return val1;
    if (lpRelative is EntityVehicle)
      val1 *= 2f;
    return val1 * num1;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public float GetLandProtectionHardnessModifier(
    Chunk chunk,
    Vector3i blockPos,
    PersistentPlayerData lpRelative,
    int halfClaimSize)
  {
    float _originalValue = 1f;
    PersistentPlayerList persistentPlayerList = this.gameManager.GetPersistentPlayerList();
    List<Vector3i> indexedBlock = chunk.IndexedBlocks["lpblock"];
    if (indexedBlock != null)
    {
      Vector3i worldPos = chunk.GetWorldPos();
      for (int index = 0; index < indexedBlock.Count; ++index)
      {
        Vector3i pos = indexedBlock[index] + worldPos;
        if (BlockLandClaim.IsPrimary(chunk.GetBlock(indexedBlock[index])))
        {
          PersistentPlayerData protectionBlockOwner = persistentPlayerList.GetLandProtectionBlockOwner(pos);
          if (protectionBlockOwner != null && (lpRelative == null || protectionBlockOwner != lpRelative && (blockPos == pos || protectionBlockOwner.ACL == null || !protectionBlockOwner.ACL.Contains(lpRelative.PrimaryId))))
          {
            int num1 = Math.Abs(pos.x - blockPos.x);
            int num2 = Math.Abs(pos.z - blockPos.z);
            int num3 = halfClaimSize;
            if (num1 <= num3 && num2 <= halfClaimSize)
            {
              float modifierForPlayer = this.GetLandProtectionHardnessModifierForPlayer(protectionBlockOwner);
              if ((double) modifierForPlayer < 1.0)
                return modifierForPlayer;
              _originalValue = Mathf.Max(_originalValue, modifierForPlayer);
              if (lpRelative != null)
              {
                EntityPlayer entity = this.GetEntity(lpRelative.EntityId) as EntityPlayer;
                _originalValue = EffectManager.GetValue(PassiveEffects.LandClaimDamageModifier, entity.inventory.holdingItemItemValue, _originalValue, (EntityAlive) entity);
              }
            }
          }
        }
      }
    }
    return _originalValue;
  }

  public override bool IsMyLandProtectedBlock(
    Vector3i worldBlockPos,
    PersistentPlayerData lpRelative,
    bool traderAllowed = false)
  {
    if (GameStats.GetInt(EnumGameStats.GameModeId) != 1)
      return true;
    if (!traderAllowed && this.IsWithinTraderArea(worldBlockPos))
      return false;
    this.m_lpChunkList.Clear();
    int num1 = GameStats.GetInt(EnumGameStats.LandClaimSize);
    int num2 = (num1 - 1) / 2;
    int num3 = num1 / 16 /*0x10*/ + 1;
    int num4 = num1 / 16 /*0x10*/ + 1;
    int num5 = worldBlockPos.x - num2;
    int num6 = worldBlockPos.z - num2;
    for (int index1 = -num3; index1 <= num3; ++index1)
    {
      int _x = num5 + index1 * 16 /*0x10*/;
      for (int index2 = -num4; index2 <= num4; ++index2)
      {
        int _z = num6 + index2 * 16 /*0x10*/;
        Chunk chunkFromWorldPos = (Chunk) this.GetChunkFromWorldPos(new Vector3i(_x, worldBlockPos.y, _z));
        if (chunkFromWorldPos != null && !this.m_lpChunkList.Contains(chunkFromWorldPos))
        {
          this.m_lpChunkList.Add(chunkFromWorldPos);
          if (this.IsMyLandClaimInChunk(chunkFromWorldPos, worldBlockPos, lpRelative, num2, num2, false))
          {
            this.m_lpChunkList.Clear();
            return true;
          }
        }
      }
    }
    this.m_lpChunkList.Clear();
    return false;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public bool IsLandProtectedBlock(
    Chunk chunk,
    Vector3i blockPos,
    PersistentPlayerData lpRelative,
    int claimSize,
    int deadZone,
    bool forKeystone)
  {
    PersistentPlayerList persistentPlayerList = this.gameManager.GetPersistentPlayerList();
    List<Vector3i> indexedBlock = chunk.IndexedBlocks["lpblock"];
    if (indexedBlock != null)
    {
      Vector3i worldPos = chunk.GetWorldPos();
      for (int index = 0; index < indexedBlock.Count; ++index)
      {
        Vector3i pos = indexedBlock[index] + worldPos;
        if (BlockLandClaim.IsPrimary(chunk.GetBlock(indexedBlock[index])))
        {
          int num1 = Math.Abs(pos.x - blockPos.x);
          int num2 = Math.Abs(pos.z - blockPos.z);
          if (num1 <= deadZone && num2 <= deadZone)
          {
            PersistentPlayerData protectionBlockOwner = persistentPlayerList.GetLandProtectionBlockOwner(pos);
            if (protectionBlockOwner != null)
            {
              bool flag = this.IsLandProtectionValidForPlayer(protectionBlockOwner);
              if (flag && lpRelative != null)
              {
                if (lpRelative == protectionBlockOwner)
                  flag = false;
                else if (protectionBlockOwner.ACL != null && protectionBlockOwner.ACL.Contains(lpRelative.PrimaryId))
                  flag = ((num1 > claimSize ? 0 : (num2 <= claimSize ? 1 : 0)) & (forKeystone ? 1 : 0)) != 0;
              }
              if (flag)
                return true;
            }
          }
        }
      }
    }
    return false;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public bool IsMyLandClaimInChunk(
    Chunk chunk,
    Vector3i blockPos,
    PersistentPlayerData lpRelative,
    int claimSize,
    int deadZone,
    bool forKeystone)
  {
    PersistentPlayerList persistentPlayerList = this.gameManager.GetPersistentPlayerList();
    List<Vector3i> indexedBlock = chunk.IndexedBlocks["lpblock"];
    if (indexedBlock != null)
    {
      Vector3i worldPos = chunk.GetWorldPos();
      for (int index = 0; index < indexedBlock.Count; ++index)
      {
        Vector3i pos = indexedBlock[index] + worldPos;
        if (BlockLandClaim.IsPrimary(chunk.GetBlock(indexedBlock[index])))
        {
          int num1 = Math.Abs(pos.x - blockPos.x);
          int num2 = Math.Abs(pos.z - blockPos.z);
          if (num1 <= deadZone && num2 <= deadZone)
          {
            PersistentPlayerData protectionBlockOwner = persistentPlayerList.GetLandProtectionBlockOwner(pos);
            if (protectionBlockOwner != null)
            {
              bool flag = this.IsLandProtectionValidForPlayer(protectionBlockOwner);
              if (flag && lpRelative != null)
                flag = lpRelative != protectionBlockOwner ? protectionBlockOwner.ACL != null && protectionBlockOwner.ACL.Contains(lpRelative.PrimaryId) && ((num1 > claimSize ? 0 : (num2 <= claimSize ? 1 : 0)) & (forKeystone ? 1 : 0)) != 0 : num1 <= claimSize && num2 <= claimSize;
              if (flag)
                return true;
            }
          }
        }
      }
    }
    return false;
  }

  public override EnumLandClaimOwner GetLandClaimOwner(
    Vector3i worldBlockPos,
    PersistentPlayerData lpRelative)
  {
    if (GameStats.GetInt(EnumGameStats.GameModeId) != 1)
      return EnumLandClaimOwner.Self;
    if (this.IsWithinTraderArea(worldBlockPos))
      return EnumLandClaimOwner.None;
    this.m_lpChunkList.Clear();
    int num1 = GameStats.GetInt(EnumGameStats.LandClaimSize);
    int num2 = (num1 - 1) / 2;
    int num3 = num1 / 16 /*0x10*/ + 1;
    int num4 = num1 / 16 /*0x10*/ + 1;
    int num5 = worldBlockPos.x - num2;
    int num6 = worldBlockPos.z - num2;
    for (int index1 = -num3; index1 <= num3; ++index1)
    {
      int _x = num5 + index1 * 16 /*0x10*/;
      for (int index2 = -num4; index2 <= num4; ++index2)
      {
        int _z = num6 + index2 * 16 /*0x10*/;
        Chunk chunkFromWorldPos = (Chunk) this.GetChunkFromWorldPos(new Vector3i(_x, worldBlockPos.y, _z));
        if (chunkFromWorldPos != null && !this.m_lpChunkList.Contains(chunkFromWorldPos))
        {
          this.m_lpChunkList.Add(chunkFromWorldPos);
          EnumLandClaimOwner landClaimOwner = this.GetLandClaimOwner(chunkFromWorldPos, worldBlockPos, lpRelative, num2, num2, false);
          if (landClaimOwner != EnumLandClaimOwner.None)
          {
            this.m_lpChunkList.Clear();
            return landClaimOwner;
          }
        }
      }
    }
    this.m_lpChunkList.Clear();
    return EnumLandClaimOwner.None;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public EnumLandClaimOwner GetLandClaimOwner(
    Chunk chunk,
    Vector3i blockPos,
    PersistentPlayerData lpRelative,
    int claimSize,
    int deadZone,
    bool forKeystone)
  {
    PersistentPlayerList persistentPlayerList = this.gameManager.GetPersistentPlayerList();
    List<Vector3i> indexedBlock = chunk.IndexedBlocks["lpblock"];
    if (indexedBlock != null)
    {
      Vector3i worldPos = chunk.GetWorldPos();
      for (int index = 0; index < indexedBlock.Count; ++index)
      {
        Vector3i pos = indexedBlock[index] + worldPos;
        if (BlockLandClaim.IsPrimary(chunk.GetBlock(indexedBlock[index])))
        {
          int num1 = Math.Abs(pos.x - blockPos.x);
          int num2 = Math.Abs(pos.z - blockPos.z);
          int num3 = deadZone;
          if (num1 <= num3 && num2 <= deadZone)
          {
            PersistentPlayerData protectionBlockOwner = persistentPlayerList.GetLandProtectionBlockOwner(pos);
            if (protectionBlockOwner != null && this.IsLandProtectionValidForPlayer(protectionBlockOwner))
            {
              if (lpRelative == null)
                return EnumLandClaimOwner.Other;
              if (lpRelative == protectionBlockOwner)
                return EnumLandClaimOwner.Self;
              return protectionBlockOwner.ACL != null && protectionBlockOwner.ACL.Contains(lpRelative.PrimaryId) ? EnumLandClaimOwner.Ally : EnumLandClaimOwner.Other;
            }
          }
        }
      }
    }
    return EnumLandClaimOwner.None;
  }

  public bool GetLandClaimOwnerInParty(EntityPlayer player, PersistentPlayerData lpRelative)
  {
    if (GameStats.GetInt(EnumGameStats.GameModeId) != 1)
      return false;
    Vector3i blockPosition = player.GetBlockPosition();
    if (this.IsWithinTraderArea(blockPosition))
      return false;
    this.m_lpChunkList.Clear();
    int num1 = GameStats.GetInt(EnumGameStats.LandClaimSize);
    int num2 = (num1 - 1) / 2;
    int num3 = num1 / 16 /*0x10*/ + 1;
    int num4 = num1 / 16 /*0x10*/ + 1;
    int num5 = blockPosition.x - num2;
    int num6 = blockPosition.z - num2;
    for (int index1 = -num3; index1 <= num3; ++index1)
    {
      int _x = num5 + index1 * 16 /*0x10*/;
      for (int index2 = -num4; index2 <= num4; ++index2)
      {
        int _z = num6 + index2 * 16 /*0x10*/;
        Chunk chunkFromWorldPos = (Chunk) this.GetChunkFromWorldPos(new Vector3i(_x, blockPosition.y, _z));
        if (chunkFromWorldPos != null && !this.m_lpChunkList.Contains(chunkFromWorldPos))
        {
          this.m_lpChunkList.Add(chunkFromWorldPos);
          if (this.GetLandClaimOwnerInParty(chunkFromWorldPos, player, blockPosition, lpRelative, num2, num2))
          {
            this.m_lpChunkList.Clear();
            return true;
          }
        }
      }
    }
    this.m_lpChunkList.Clear();
    return false;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public bool GetLandClaimOwnerInParty(
    Chunk chunk,
    EntityPlayer player,
    Vector3i blockPos,
    PersistentPlayerData lpRelative,
    int claimSize,
    int deadZone)
  {
    PersistentPlayerList persistentPlayerList = this.gameManager.GetPersistentPlayerList();
    bool flag = player.Party != null;
    List<Vector3i> indexedBlock = chunk.IndexedBlocks["lpblock"];
    if (indexedBlock != null)
    {
      Vector3i worldPos = chunk.GetWorldPos();
      for (int index = 0; index < indexedBlock.Count; ++index)
      {
        Vector3i pos = indexedBlock[index] + worldPos;
        if (BlockLandClaim.IsPrimary(chunk.GetBlock(indexedBlock[index])))
        {
          int num1 = Math.Abs(pos.x - blockPos.x);
          int num2 = Math.Abs(pos.z - blockPos.z);
          if (num1 <= deadZone && num2 <= deadZone)
          {
            PersistentPlayerData protectionBlockOwner = persistentPlayerList.GetLandProtectionBlockOwner(pos);
            if (protectionBlockOwner != null)
            {
              if (lpRelative == null && Object.op_Inequality((Object) player, (Object) null) && protectionBlockOwner.EntityId == player.entityId)
                lpRelative = protectionBlockOwner;
              if (this.IsLandProtectionValidForPlayer(protectionBlockOwner) && lpRelative != null)
              {
                if (lpRelative == protectionBlockOwner)
                {
                  if (num1 <= claimSize && num2 <= claimSize)
                    return true;
                }
                else if (flag && protectionBlockOwner.ACL != null && protectionBlockOwner.ACL.Contains(lpRelative.PrimaryId) && player.Party.ContainsMember(protectionBlockOwner.EntityId) && num1 <= claimSize && num2 <= claimSize)
                  return true;
              }
              return false;
            }
          }
        }
      }
    }
    return false;
  }

  public bool IsLandProtectionValidForPlayer(PersistentPlayerData ppData)
  {
    double num = (double) GameStats.GetInt(EnumGameStats.LandClaimExpiryTime) * 24.0;
    return ppData.OfflineHours <= num;
  }

  public float GetLandProtectionHardnessModifierForPlayer(PersistentPlayerData ppData)
  {
    float modifierForPlayer1 = (float) GameStats.GetInt(EnumGameStats.LandClaimOnlineDurabilityModifier);
    if (ppData.EntityId != -1)
      return modifierForPlayer1;
    double offlineHours = ppData.OfflineHours;
    double offlineMinutes = ppData.OfflineMinutes;
    float num1 = (float) GameStats.GetInt(EnumGameStats.LandClaimOfflineDelay);
    if ((double) num1 != 0.0 && offlineMinutes <= (double) num1)
      return modifierForPlayer1;
    double num2 = (double) GameStats.GetInt(EnumGameStats.LandClaimExpiryTime) * 24.0;
    if (offlineHours > num2)
      return 1f;
    EnumLandClaimDecayMode landClaimDecayMode = (EnumLandClaimDecayMode) GameStats.GetInt(EnumGameStats.LandClaimDecayMode);
    float modifierForPlayer2 = (float) GameStats.GetInt(EnumGameStats.LandClaimOfflineDurabilityModifier);
    if ((double) modifierForPlayer2 == 0.0)
      return 0.0f;
    if (landClaimDecayMode == EnumLandClaimDecayMode.DecaySlowly)
      return Mathf.Max(1f, (float) (1.0 - (offlineHours - 24.0) / (num2 - 24.0)) * modifierForPlayer2);
    if (landClaimDecayMode == EnumLandClaimDecayMode.BuffedUntilExpired)
      return modifierForPlayer2;
    double num3 = (offlineHours - 24.0) / (num2 - 24.0);
    return Mathf.Max(1f, (float) ((1.0 - num3) * (1.0 - num3)) * modifierForPlayer2);
  }

  public float GetDecorationOffsetY(Vector3i _blockPos)
  {
    return MarchingCubes.GetDecorationOffsetY(this.GetDensity(0, _blockPos), this.GetDensity(0, _blockPos - Vector3i.up));
  }

  public EnumDecoAllowed GetDecoAllowedAt(int _x, int _z)
  {
    Chunk chunkFromWorldPos = (Chunk) this.GetChunkFromWorldPos(_x, _z);
    return chunkFromWorldPos != null ? chunkFromWorldPos.GetDecoAllowedAt(World.toBlockXZ(_x), World.toBlockXZ(_z)) : EnumDecoAllowed.Nothing;
  }

  public void SetDecoAllowedAt(int _x, int _z, EnumDecoAllowed _decoAllowed)
  {
    ((Chunk) this.GetChunkFromWorldPos(_x, _z))?.SetDecoAllowedAt(World.toBlockXZ(_x), World.toBlockXZ(_z), _decoAllowed);
  }

  public Vector3 GetTerrainNormalAt(int _x, int _z)
  {
    Chunk chunkFromWorldPos = (Chunk) this.GetChunkFromWorldPos(_x, _z);
    return chunkFromWorldPos != null ? chunkFromWorldPos.GetTerrainNormal(World.toBlockXZ(_x), World.toBlockXZ(_z)) : Vector3.zero;
  }

  public bool GetWorldExtent(out Vector3i _minSize, out Vector3i _maxSize)
  {
    return this.ChunkCache.ChunkProvider.GetWorldExtent(out _minSize, out _maxSize);
  }

  public virtual bool IsPositionAvailable(int _clrIdx, Vector3 _position)
  {
    ChunkCluster chunkCluster = this.ChunkClusters[_clrIdx];
    if (chunkCluster == null)
      return false;
    Vector3i blockPos = World.worldToBlockPos(_position);
    for (int index = 0; index < Vector3i.MIDDLE_AND_HORIZONTAL_DIRECTIONS_DIAGONAL.Length; ++index)
    {
      Vector3i vector3i = Vector3i.MIDDLE_AND_HORIZONTAL_DIRECTIONS_DIAGONAL[index] * 16 /*0x10*/;
      IChunk chunkFromWorldPos = chunkCluster.GetChunkFromWorldPos(blockPos + vector3i);
      if (chunkFromWorldPos == null || !chunkFromWorldPos.GetAvailable())
        return false;
    }
    return true;
  }

  public bool GetBiomeIntensity(Vector3i _position, out BiomeIntensity _biomeIntensity)
  {
    Chunk chunkFromWorldPos = (Chunk) this.GetChunkFromWorldPos(_position);
    if (chunkFromWorldPos != null && !chunkFromWorldPos.NeedsLightCalculation)
    {
      _biomeIntensity = chunkFromWorldPos.GetBiomeIntensity(World.toBlockXZ(_position.x), World.toBlockXZ(_position.z));
      return true;
    }
    _biomeIntensity = BiomeIntensity.Default;
    return false;
  }

  public bool CanMobsSpawnAtPos(Vector3 _pos)
  {
    Vector3i blockPos = World.worldToBlockPos(_pos);
    Chunk chunkFromWorldPos = (Chunk) this.GetChunkFromWorldPos(blockPos);
    return chunkFromWorldPos != null && chunkFromWorldPos.CanMobsSpawnAtPos(World.toBlockXZ(blockPos.x), World.toBlockY(blockPos.y), World.toBlockXZ(blockPos.z));
  }

  public bool CanSleeperSpawnAtPos(Vector3 _pos, bool _checkBelow)
  {
    Vector3i blockPos = World.worldToBlockPos(_pos);
    Chunk chunkFromWorldPos = (Chunk) this.GetChunkFromWorldPos(blockPos);
    return chunkFromWorldPos != null && chunkFromWorldPos.CanSleeperSpawnAtPos(World.toBlockXZ(blockPos.x), World.toBlockY(blockPos.y), World.toBlockXZ(blockPos.z), _checkBelow);
  }

  public bool CanPlayersSpawnAtPos(Vector3 _pos, bool _bAllowToSpawnOnAirPos = false)
  {
    Vector3i blockPos = World.worldToBlockPos(_pos);
    Chunk chunkFromWorldPos = (Chunk) this.GetChunkFromWorldPos(blockPos);
    return chunkFromWorldPos != null && chunkFromWorldPos.CanPlayersSpawnAtPos(World.toBlockXZ(blockPos.x), World.toBlockY(blockPos.y), World.toBlockXZ(blockPos.z), _bAllowToSpawnOnAirPos);
  }

  public void CheckEntityCollisionWithBlocks(Entity _entity)
  {
    if (!_entity.CanCollideWithBlocks())
      return;
    for (int _idx = 0; _idx < this.ChunkClusters.Count; ++_idx)
    {
      ChunkCluster chunkCluster = this.ChunkClusters[_idx];
      if (chunkCluster != null && chunkCluster.Overlaps(_entity.boundingBox))
        chunkCluster.CheckCollisionWithBlocks(_entity);
    }
  }

  public void OnChunkAdded(Chunk _c)
  {
    lock (this.newlyLoadedChunksThisUpdate)
      this.newlyLoadedChunksThisUpdate.Add(_c.Key);
  }

  public void OnChunkBeforeRemove(Chunk _c)
  {
    lock (this.newlyLoadedChunksThisUpdate)
      this.newlyLoadedChunksThisUpdate.Remove(_c.Key);
    if (this.worldBlockTicker != null)
      this.worldBlockTicker.OnChunkRemoved(_c);
    GameManager.Instance.prefabLODManager.TriggerUpdate();
  }

  public void OnChunkBeforeSave(Chunk _c)
  {
    if (this.worldBlockTicker == null)
      return;
    this.worldBlockTicker.OnChunkBeforeSave(_c);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void updateChunkAddedRemovedCallbacks()
  {
    lock (this.newlyLoadedChunksThisUpdate)
    {
      int _idx = 0;
      ChunkCluster chunkCluster = this.ChunkClusters[_idx];
      for (int index = this.newlyLoadedChunksThisUpdate.Count - 1; index >= 0; --index)
      {
        long num = this.newlyLoadedChunksThisUpdate[index];
        int clrIdx = WorldChunkCache.extractClrIdx(num);
        if (clrIdx != _idx)
        {
          _idx = clrIdx;
          chunkCluster = this.ChunkClusters[_idx];
        }
        if (chunkCluster != null)
        {
          Chunk chunkSync = chunkCluster.GetChunkSync(num);
          if (chunkSync != null && !chunkSync.NeedsDecoration)
          {
            chunkSync.OnLoad(this);
            if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
              this.worldBlockTicker.OnChunkAdded((WorldBase) this, chunkSync, this.rand);
            this.newlyLoadedChunksThisUpdate.RemoveAt(index);
          }
        }
      }
    }
  }

  public override ulong GetWorldTime() => this.worldTime;

  public override WorldCreationData GetWorldCreationData() => this.wcd;

  public bool IsEntityInRange(int _entityId, int _refEntity, int _range)
  {
    if (_entityId == _refEntity)
      return true;
    Entity entity;
    Entity _other;
    return this.Entities.dict.TryGetValue(_entityId, out entity) && this.Entities.dict.TryGetValue(_refEntity, out _other) && (double) entity.GetDistanceSq(_other) <= (double) (_range * _range);
  }

  public bool IsEntityInRange(int _entityId, Vector3 _position, int _range)
  {
    Entity entity;
    return this.Entities.dict.TryGetValue(_entityId, out entity) && (double) entity.GetDistanceSq(_position) <= (double) (_range * _range);
  }

  public bool IsPositionInBounds(Vector3 position)
  {
    Vector3i _minSize;
    Vector3i _maxSize;
    this.GetWorldExtent(out _minSize, out _maxSize);
    if (GamePrefs.GetString(EnumGamePrefs.GameWorld) == "Navezgane")
    {
      _minSize = new Vector3i(-2400, _minSize.y, -2400);
      _maxSize = new Vector3i(2400, _maxSize.y, 2400);
    }
    else if (!GameUtils.IsPlaytesting())
    {
      _minSize = new Vector3i(_minSize.x + 320, _minSize.y, _minSize.z + 320);
      _maxSize = new Vector3i(_maxSize.x - 320, _maxSize.y, _maxSize.z - 320);
    }
    Vector3Int vector3Int1 = (Vector3Int) _minSize;
    Vector3Int vector3Int2 = (Vector3Int) _maxSize;
    BoundsInt boundsInt;
    // ISSUE: explicit constructor call
    ((BoundsInt) ref boundsInt).\u002Ector(vector3Int1, Vector3Int.op_Subtraction(vector3Int2, vector3Int1));
    return ((BoundsInt) ref boundsInt).Contains(Vector3Int.RoundToInt(position));
  }

  public float InBoundsForPlayersPercent(Vector3 _pos)
  {
    Vector3i _minSize;
    Vector3i _maxSize;
    this.GetWorldExtent(out _minSize, out _maxSize);
    if (_maxSize.x - _minSize.x < 1024 /*0x0400*/)
      return 1f;
    Vector2 vector2;
    vector2.x = (float) (_minSize.x + _maxSize.x) * 0.5f;
    vector2.y = (float) (_minSize.z + _maxSize.z) * 0.5f;
    return Utils.FastMin(Utils.FastClamp01((double) _pos.x >= (double) vector2.x ? (float) (((double) _maxSize.x - 50.0 - (double) _pos.x) / 80.0) : (float) (((double) _pos.x - ((double) _minSize.x + 50.0)) / 80.0)), Utils.FastClamp01((double) _pos.z >= (double) vector2.y ? (float) (((double) _maxSize.z - 50.0 - (double) _pos.z) / 80.0) : (float) (((double) _pos.z - ((double) _minSize.z + 50.0)) / 80.0)));
  }

  public bool AdjustBoundsForPlayers(ref Vector3 _pos, float _padPercent)
  {
    Vector3i _minSize;
    Vector3i _maxSize;
    this.GetWorldExtent(out _minSize, out _maxSize);
    if (_maxSize.x - _minSize.x < 1024 /*0x0400*/ || _maxSize.x == 0)
      return false;
    int num = (int) (50.0 + 80.0 * (double) _padPercent);
    _minSize.x += num;
    _minSize.z += num;
    _maxSize.x -= num;
    _maxSize.z -= num;
    bool flag = false;
    if ((double) _pos.x < (double) _minSize.x)
    {
      _pos.x = (float) _minSize.x;
      flag = true;
    }
    else if ((double) _pos.x > (double) _maxSize.x)
    {
      _pos.x = (float) _maxSize.x;
      flag = true;
    }
    if ((double) _pos.z < (double) _minSize.z)
    {
      _pos.z = (float) _minSize.z;
      flag = true;
    }
    else if ((double) _pos.z > (double) _maxSize.z)
    {
      _pos.z = (float) _maxSize.z;
      flag = true;
    }
    return flag;
  }

  public bool IsPositionRadiated(Vector3 position)
  {
    IChunkProvider chunkProvider = this.ChunkCache.ChunkProvider;
    IBiomeProvider biomeProvider;
    return chunkProvider != null && (biomeProvider = chunkProvider.GetBiomeProvider()) != null && (double) biomeProvider.GetRadiationAt((int) position.x, (int) position.z) > 0.0;
  }

  public bool IsPositionWithinPOI(Vector3 position, int offset)
  {
    return this.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator().GetPrefabFromWorldPosInsideWithOffset((int) position.x, (int) position.z, offset) != null;
  }

  public PrefabInstance GetPOIAtPosition(Vector3 _position, bool _checkTags = true)
  {
    return this.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator()?.GetPrefabAtPosition(_position, _checkTags);
  }

  public void GetPOIsAtXZ(int _xMin, int _xMax, int _zMin, int _zMax, List<PrefabInstance> _list)
  {
    this.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator()?.GetPrefabsAtXZ(_xMin, _xMax, _zMin, _zMax, _list);
  }

  public Vector3 ClampToValidWorldPos(Vector3 position)
  {
    Vector3i _minSize;
    Vector3i _maxSize;
    this.GetWorldExtent(out _minSize, out _maxSize);
    if (GamePrefs.GetString(EnumGamePrefs.GameWorld) == "Navezgane")
    {
      _minSize = new Vector3i(-2400, _minSize.y, -2400);
      _maxSize = new Vector3i(2400, _maxSize.y, 2400);
    }
    else if (!GameUtils.IsPlaytesting())
    {
      _minSize = new Vector3i(_minSize.x + 320, _minSize.y, _minSize.z + 320);
      _maxSize = new Vector3i(_maxSize.x - 320, _maxSize.y, _maxSize.z - 320);
    }
    double num1 = (double) Mathf.Clamp(position.x, (float) _minSize.x, (float) _maxSize.x);
    float num2 = Mathf.Clamp(position.y, (float) _minSize.y, (float) _maxSize.y);
    float num3 = Mathf.Clamp(position.z, (float) _minSize.z, (float) _maxSize.z);
    double num4 = (double) num2;
    double num5 = (double) num3;
    return new Vector3((float) num1, (float) num4, (float) num5);
  }

  public Vector3 ClampToValidWorldPosForMap(Vector2 position)
  {
    Vector3i _minSize;
    Vector3i _maxSize;
    this.GetWorldExtent(out _minSize, out _maxSize);
    if (GamePrefs.GetString(EnumGamePrefs.GameWorld) == "Navezgane")
    {
      _minSize = new Vector3i(-2550, _minSize.y, -2550);
      _maxSize = new Vector3i(2550, _maxSize.y, 2550);
    }
    return Vector2.op_Implicit(new Vector2(Mathf.Clamp(position.x, (float) _minSize.x, (float) _maxSize.x), Mathf.Clamp(position.y, (float) _minSize.z, (float) _maxSize.z)));
  }

  public void ObjectOnMapAdd(MapObject _mo)
  {
    if (this.objectsOnMap == null)
      return;
    this.objectsOnMap.Add(_mo);
  }

  public void ObjectOnMapRemove(EnumMapObjectType _type, int _key)
  {
    if (this.objectsOnMap == null)
      return;
    this.objectsOnMap.Remove(_type, _key);
  }

  public void ObjectOnMapRemove(EnumMapObjectType _type, Vector3 _position)
  {
    if (this.objectsOnMap == null)
      return;
    this.objectsOnMap.RemoveByPosition(_type, _position);
  }

  public void ObjectOnMapRemove(EnumMapObjectType _type)
  {
    if (this.objectsOnMap == null)
      return;
    this.objectsOnMap.RemoveByType(_type);
  }

  public List<MapObject> GetObjectOnMapList(EnumMapObjectType _type)
  {
    return this.objectsOnMap != null ? this.objectsOnMap.GetList(_type) : new List<MapObject>();
  }

  public void DebugAddSpawnedEntity(Entity entity)
  {
    if (Object.op_Equality((Object) this.GetPrimaryPlayer(), (Object) null) || !(entity is EntityAlive))
      return;
    EntityAlive entityAlive = (EntityAlive) entity;
    SSpawnedEntity sspawnedEntity = new SSpawnedEntity();
    ref SSpawnedEntity local = ref sspawnedEntity;
    Vector3 vector3 = Vector3.op_Subtraction(entityAlive.GetPosition(), this.GetPrimaryPlayer().GetPosition());
    double magnitude = (double) ((Vector3) ref vector3).magnitude;
    local.distanceToLocalPlayer = (float) magnitude;
    sspawnedEntity.name = entityAlive.EntityName;
    sspawnedEntity.pos = entityAlive.GetPosition();
    sspawnedEntity.timeSpawned = Time.time;
    this.Last4Spawned.Add(sspawnedEntity);
    if (this.Last4Spawned.Count <= 4)
      return;
    this.Last4Spawned.RemoveAt(0);
  }

  public static void SetWorldAreas(List<TraderArea> _traders) => World.traderAreas = _traders;

  [PublicizedFrom(EAccessModifier.Private)]
  public void SetupTraders()
  {
    if (World.traderAreas == null)
      return;
    DynamicPrefabDecorator dynamicPrefabDecorator = this.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator();
    if (dynamicPrefabDecorator != null)
    {
      dynamicPrefabDecorator.ClearTraders();
      for (int index = 0; index < World.traderAreas.Count; ++index)
        dynamicPrefabDecorator.AddTrader(World.traderAreas[index]);
    }
    World.traderAreas = (List<TraderArea>) null;
  }

  public List<TraderArea> TraderAreas
  {
    get => this.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator()?.GetTraderAreas();
  }

  public bool IsWithinTraderArea(Vector3i _worldBlockPos)
  {
    return this.GetTraderAreaAt(_worldBlockPos) != null;
  }

  public bool IsWithinTraderPlacingProtection(Vector3i _worldBlockPos)
  {
    DynamicPrefabDecorator dynamicPrefabDecorator = this.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator();
    return dynamicPrefabDecorator != null && dynamicPrefabDecorator.GetTraderAtPosition(_worldBlockPos, 2) != null;
  }

  public bool IsWithinTraderPlacingProtection(Bounds _bounds)
  {
    DynamicPrefabDecorator dynamicPrefabDecorator = this.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator();
    if (dynamicPrefabDecorator == null)
      return false;
    ((Bounds) ref _bounds).Expand(4f);
    Vector3i blockPos1 = World.worldToBlockPos(((Bounds) ref _bounds).min);
    Vector3i blockPos2 = World.worldToBlockPos(((Bounds) ref _bounds).max);
    return dynamicPrefabDecorator.IsWithinTraderArea(blockPos1, blockPos2);
  }

  public bool IsWithinTraderArea(Vector3i _minPos, Vector3i _maxPos)
  {
    DynamicPrefabDecorator dynamicPrefabDecorator = this.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator();
    return dynamicPrefabDecorator != null && dynamicPrefabDecorator.IsWithinTraderArea(_minPos, _maxPos);
  }

  public TraderArea GetTraderAreaAt(Vector3i _pos)
  {
    return this.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator()?.GetTraderAtPosition(_pos, 0);
  }

  public override int AddSleeperVolume(SleeperVolume _sleeperVolume)
  {
    lock (this.sleeperVolumes)
    {
      this.sleeperVolumes.Add(_sleeperVolume);
      List<int> intList;
      if (!this.sleeperVolumeMap.TryGetValue(_sleeperVolume.BoxMin, out intList))
      {
        intList = new List<int>();
        this.sleeperVolumeMap.Add(_sleeperVolume.BoxMin, intList);
      }
      intList.Add(this.sleeperVolumes.Count - 1);
      return this.sleeperVolumes.Count - 1;
    }
  }

  public override int FindSleeperVolume(Vector3i mins, Vector3i maxs)
  {
    List<int> intList;
    if (this.sleeperVolumeMap.TryGetValue(mins, out intList))
    {
      for (int index1 = 0; index1 < intList.Count; ++index1)
      {
        int index2 = intList[index1];
        if (this.sleeperVolumes[index2].BoxMax == maxs)
          return index2;
      }
    }
    return -1;
  }

  public override int GetSleeperVolumeCount() => this.sleeperVolumes.Count;

  public override SleeperVolume GetSleeperVolume(int index) => this.sleeperVolumes[index];

  public void CheckSleeperVolumeTouching(EntityPlayer _player)
  {
    if (!GameStats.GetBool(EnumGameStats.IsSpawnEnemies))
      return;
    Chunk chunkFromWorldPos = (Chunk) this.GetChunkFromWorldPos(_player.GetBlockPosition());
    if (chunkFromWorldPos == null)
      return;
    List<int> sleeperVolumes = chunkFromWorldPos.GetSleeperVolumes();
    lock (this.sleeperVolumes)
    {
      for (int index1 = 0; index1 < sleeperVolumes.Count; ++index1)
      {
        int index2 = sleeperVolumes[index1];
        if (index2 < this.sleeperVolumes.Count)
          this.sleeperVolumes[index2].CheckTouching(this, _player);
      }
    }
  }

  public void CheckSleeperVolumeNoise(Vector3 position)
  {
    if (!GameStats.GetBool(EnumGameStats.IsSpawnEnemies))
      return;
    position.y += 0.1f;
    Chunk chunkFromWorldPos = (Chunk) this.GetChunkFromWorldPos(World.worldToBlockPos(position));
    if (chunkFromWorldPos == null)
      return;
    List<int> sleeperVolumes = chunkFromWorldPos.GetSleeperVolumes();
    lock (this.sleeperVolumes)
    {
      for (int index1 = 0; index1 < sleeperVolumes.Count; ++index1)
      {
        int index2 = sleeperVolumes[index1];
        if (index2 < this.sleeperVolumes.Count)
          this.sleeperVolumes[index2].CheckNoise(this, position);
      }
    }
  }

  public void WriteSleeperVolumes(BinaryWriter _bw)
  {
    _bw.Write(this.sleeperVolumes.Count);
    for (int index = 0; index < this.sleeperVolumes.Count; ++index)
      this.sleeperVolumes[index].Write(_bw);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void ReadSleeperVolumes(BinaryReader _br)
  {
    this.sleeperVolumes.Clear();
    this.sleeperVolumeMap.Clear();
    int num = _br.ReadInt32();
    for (int index = 0; index < num; ++index)
    {
      SleeperVolume sleeperVolume = SleeperVolume.Read(_br);
      this.sleeperVolumes.Add(sleeperVolume);
      List<int> intList;
      if (!this.sleeperVolumeMap.TryGetValue(sleeperVolume.BoxMin, out intList))
      {
        intList = new List<int>();
        this.sleeperVolumeMap.Add(sleeperVolume.BoxMin, intList);
      }
      intList.Add(this.sleeperVolumes.Count - 1);
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void SetupSleeperVolumes()
  {
    for (int index = 0; index < this.sleeperVolumes.Count; ++index)
      this.sleeperVolumes[index].AddToPrefabInstance();
  }

  public void NotifySleeperVolumesEntityDied(EntityAlive entity)
  {
    lock (this.sleeperVolumes)
    {
      for (int index = 0; index < this.sleeperVolumes.Count; ++index)
        this.sleeperVolumes[index].EntityDied(entity);
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void TickSleeperVolumes()
  {
    if (!GameStats.GetBool(EnumGameStats.IsSpawnEnemies))
      return;
    lock (this.sleeperVolumes)
    {
      SleeperVolume.TickSpawnCount = 0;
      for (int index = 0; index < this.sleeperVolumes.Count; ++index)
        this.sleeperVolumes[index].Tick(this);
    }
  }

  public override int AddTriggerVolume(TriggerVolume _triggerVolume)
  {
    lock (this.triggerVolumes)
    {
      this.triggerVolumes.Add(_triggerVolume);
      List<int> intList;
      if (!this.triggerVolumeMap.TryGetValue(_triggerVolume.BoxMin, out intList))
      {
        intList = new List<int>();
        this.triggerVolumeMap.Add(_triggerVolume.BoxMin, intList);
      }
      intList.Add(this.triggerVolumes.Count - 1);
      return this.triggerVolumes.Count - 1;
    }
  }

  public override void ResetTriggerVolumes(long chunkKey)
  {
    Vector2i xz = WorldChunkCache.extractXZ(chunkKey);
    Bounds aabb = Chunk.CalculateAABB(xz.x, 0, xz.y);
    foreach (TriggerVolume triggerVolume in this.triggerVolumes)
    {
      if (triggerVolume.Intersects(aabb))
        triggerVolume.Reset();
    }
  }

  public override void ResetSleeperVolumes(long chunkKey)
  {
    Vector2i xz = WorldChunkCache.extractXZ(chunkKey);
    Bounds aabb = Chunk.CalculateAABB(xz.x, 0, xz.y);
    foreach (SleeperVolume sleeperVolume in this.sleeperVolumes)
    {
      if (sleeperVolume.Intersects(aabb))
        sleeperVolume.DespawnAndReset(this);
    }
  }

  public override int FindTriggerVolume(Vector3i mins, Vector3i maxs)
  {
    List<int> intList;
    if (this.triggerVolumeMap.TryGetValue(mins, out intList))
    {
      for (int index1 = 0; index1 < intList.Count; ++index1)
      {
        int index2 = intList[index1];
        if (this.triggerVolumes[index2].BoxMax == maxs)
          return index2;
      }
    }
    return -1;
  }

  public override int GetTriggerVolumeCount() => this.triggerVolumes.Count;

  public override TriggerVolume GetTriggerVolume(int index) => this.triggerVolumes[index];

  public void CheckTriggerVolumeTrigger(EntityPlayer _player)
  {
    Chunk chunkFromWorldPos = (Chunk) this.GetChunkFromWorldPos(_player.GetBlockPosition());
    if (chunkFromWorldPos == null)
      return;
    List<int> triggerVolumes = chunkFromWorldPos.GetTriggerVolumes();
    lock (this.triggerVolumes)
    {
      for (int index1 = 0; index1 < triggerVolumes.Count; ++index1)
      {
        int index2 = triggerVolumes[index1];
        if (index2 < this.triggerVolumes.Count)
          this.triggerVolumes[index2].CheckTouching(this, _player);
      }
    }
  }

  public void WriteTriggerVolumes(BinaryWriter _bw)
  {
    _bw.Write(this.triggerVolumes.Count);
    for (int index = 0; index < this.triggerVolumes.Count; ++index)
      this.triggerVolumes[index].Write(_bw);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void ReadTriggerVolumes(BinaryReader _br)
  {
    this.triggerVolumes.Clear();
    this.triggerVolumeMap.Clear();
    int num = _br.ReadInt32();
    for (int index = 0; index < num; ++index)
    {
      TriggerVolume triggerVolume = TriggerVolume.Read(_br);
      this.triggerVolumes.Add(triggerVolume);
      List<int> intList;
      if (!this.triggerVolumeMap.TryGetValue(triggerVolume.BoxMin, out intList))
      {
        intList = new List<int>();
        this.triggerVolumeMap.Add(triggerVolume.BoxMin, intList);
      }
      intList.Add(this.triggerVolumes.Count - 1);
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void SetupTriggerVolumes()
  {
    for (int index = 0; index < this.triggerVolumes.Count; ++index)
      this.triggerVolumes[index].AddToPrefabInstance();
  }

  public override int AddWallVolume(WallVolume _wallVolume)
  {
    lock (this.wallVolumes)
    {
      this.wallVolumes.Add(_wallVolume);
      List<int> intList;
      if (!this.wallVolumeMap.TryGetValue(_wallVolume.BoxMin, out intList))
      {
        intList = new List<int>();
        this.wallVolumeMap.Add(_wallVolume.BoxMin, intList);
      }
      intList.Add(this.wallVolumes.Count - 1);
      if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
      {
        NetPackageWallVolume package = NetPackageManager.GetPackage<NetPackageWallVolume>();
        SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) package.Setup(_wallVolume));
      }
      return this.wallVolumes.Count - 1;
    }
  }

  public override int FindWallVolume(Vector3i mins, Vector3i maxs)
  {
    List<int> intList;
    if (this.wallVolumeMap.TryGetValue(mins, out intList))
    {
      for (int index1 = 0; index1 < intList.Count; ++index1)
      {
        int index2 = intList[index1];
        if (this.wallVolumes[index2].BoxMax == maxs)
          return index2;
      }
    }
    return -1;
  }

  public override int GetWallVolumeCount() => this.wallVolumes.Count;

  public override WallVolume GetWallVolume(int index)
  {
    if (index >= this.wallVolumes.Count)
      Debug.LogWarning((object) $"Wall Volume Error: Index {index} | wallVolumeCount: {this.wallVolumes.Count}");
    return this.wallVolumes[index];
  }

  public override List<WallVolume> GetAllWallVolumes() => this.wallVolumes;

  public void WriteWallVolumes(BinaryWriter _bw)
  {
    _bw.Write(this.wallVolumes.Count);
    for (int index = 0; index < this.wallVolumes.Count; ++index)
      this.wallVolumes[index].Write(_bw);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void ReadWallVolumes(BinaryReader _br)
  {
    this.wallVolumes.Clear();
    this.wallVolumeMap.Clear();
    int num = _br.ReadInt32();
    for (int index = 0; index < num; ++index)
    {
      WallVolume wallVolume = WallVolume.Read(_br);
      this.wallVolumes.Add(wallVolume);
      List<int> intList;
      if (!this.wallVolumeMap.TryGetValue(wallVolume.BoxMin, out intList))
      {
        intList = new List<int>();
        this.wallVolumeMap.Add(wallVolume.BoxMin, intList);
      }
      intList.Add(this.wallVolumes.Count - 1);
    }
  }

  public void SetWallVolumesForClient(List<WallVolume> wallVolumeData)
  {
    this.wallVolumes.Clear();
    this.wallVolumeMap.Clear();
    foreach (WallVolume wallVolume in wallVolumeData)
    {
      this.wallVolumes.Add(wallVolume);
      List<int> intList;
      if (!this.wallVolumeMap.TryGetValue(wallVolume.BoxMin, out intList))
      {
        intList = new List<int>();
        this.wallVolumeMap.Add(wallVolume.BoxMin, intList);
      }
      intList.Add(this.wallVolumes.Count - 1);
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void SetupWallVolumes()
  {
    for (int index = 0; index < this.wallVolumes.Count; ++index)
      this.wallVolumes[index].AddToPrefabInstance();
  }

  public void AddBlockData(Vector3i v3i, object bd) => this.blockData.Add(v3i, bd);

  public object GetBlockData(Vector3i v3i)
  {
    object blockData;
    if (!this.blockData.TryGetValue(v3i, out blockData))
      blockData = (object) null;
    return blockData;
  }

  public void ClearBlockData(Vector3i v3i) => this.blockData.Remove(v3i);

  public void RebuildTerrain(
    HashSetLong _chunks,
    Vector3i _areaStart,
    Vector3i _areaSize,
    bool _bStopStabilityUpdate,
    bool _bRegenerateChunk,
    bool _bFillEmptyBlocks,
    bool _isReset = false)
  {
    this.ChunkCache.ChunkProvider.RebuildTerrain(_chunks, _areaStart, _areaSize, _bStopStabilityUpdate, _bRegenerateChunk, _bFillEmptyBlocks, _isReset);
  }

  public override GameRandom GetGameRandom() => this.rand;

  public float RandomRange(float _min, float _max) => this.rand.RandomFloat * (_max - _min) + _min;

  [PublicizedFrom(EAccessModifier.Private)]
  public void DuskDawnInit()
  {
    (this.DuskHour, this.DawnHour) = GameUtils.CalcDuskDawnHours(GameStats.GetInt(EnumGameStats.DayLightLength));
  }

  public void SetTime(ulong _time)
  {
    this.worldTime = _time;
    if (!Object.op_Implicit((Object) this.m_WorldEnvironment))
      return;
    this.m_WorldEnvironment.WorldTimeChanged();
  }

  public void SetTimeJump(ulong _time, bool _isSeek = false)
  {
    this.SetTime(_time);
    SkyManager.bUpdateSunMoonNow = true;
    if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
      return;
    this.aiDirector.BloodMoonComponent.TimeChanged(_isSeek);
  }

  public bool IsWorldEvent(World.WorldEvent _event)
  {
    return _event == World.WorldEvent.BloodMoon && this.isEventBloodMoon;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void WorldEventUpdateTime()
  {
    this.WorldDay = GameUtils.WorldTimeToDays(this.worldTime);
    this.WorldHour = GameUtils.WorldTimeToHours(this.worldTime);
    int days = GameUtils.WorldTimeToDays(this.eventWorldTime);
    int hours = GameUtils.WorldTimeToHours(this.eventWorldTime);
    int worldDay = this.WorldDay;
    if (days == worldDay && hours == this.WorldHour)
      return;
    this.eventWorldTime = this.worldTime;
    (int duskHour, int dawnHour) = GameUtils.CalcDuskDawnHours(GameStats.GetInt(EnumGameStats.DayLightLength));
    int num1 = this.isEventBloodMoon ? 1 : 0;
    this.isEventBloodMoon = false;
    int num2 = GameStats.GetInt(EnumGameStats.BloodMoonDay);
    if (this.WorldDay == num2)
    {
      if (this.WorldHour >= duskHour)
        this.isEventBloodMoon = true;
    }
    else if (this.WorldDay > 1 && this.WorldDay == num2 + 1 && this.WorldHour < dawnHour)
      this.isEventBloodMoon = true;
    int num3 = this.isEventBloodMoon ? 1 : 0;
    if (num1 == num3)
      return;
    EntityPlayerLocal primaryPlayer = this.GetPrimaryPlayer();
    if (!Object.op_Implicit((Object) primaryPlayer))
      return;
    if (this.isEventBloodMoon && this.WorldHour == duskHour)
    {
      primaryPlayer.BloodMoonParticipation = true;
    }
    else
    {
      if (this.isEventBloodMoon || this.WorldHour != dawnHour || !primaryPlayer.BloodMoonParticipation)
        return;
      QuestEventManager.Current.BloodMoonSurvived();
      primaryPlayer.BloodMoonParticipation = false;
    }
  }

  public override void AddPendingDowngradeBlock(Vector3i _blockPos)
  {
    this.pendingUpgradeDowngradeBlocks.Add(_blockPos);
  }

  public override bool TryRetrieveAndRemovePendingDowngradeBlock(Vector3i _blockPos)
  {
    if (!this.pendingUpgradeDowngradeBlocks.Contains(_blockPos))
      return false;
    this.pendingUpgradeDowngradeBlocks.Remove(_blockPos);
    return true;
  }

  public IEnumerator ResetPOIS(
    List<PrefabInstance> prefabInstances,
    FastTags<TagGroup.Global> questTags,
    int entityID,
    int[] sharedWith,
    QuestClass questClass)
  {
    World _world = this;
    for (int k = 0; k < prefabInstances.Count; ++k)
      yield return (object) prefabInstances[k].ResetTerrain(_world);
    for (int index1 = 0; index1 < prefabInstances.Count; ++index1)
    {
      PrefabInstance prefabInstance = prefabInstances[index1];
      _world.triggerManager.RemoveFromUpdateList(prefabInstance);
      prefabInstance.LastQuestClass = questClass;
      prefabInstance.ResetBlocksAndRebuild(_world, questTags);
      for (int index2 = 0; index2 < prefabInstance.prefab.SleeperVolumes.Count; ++index2)
      {
        Vector3i startPos = prefabInstance.prefab.SleeperVolumes[index2].startPos;
        Vector3i size = prefabInstance.prefab.SleeperVolumes[index2].size;
        int sleeperVolume = GameManager.Instance.World.FindSleeperVolume(prefabInstance.boundingBoxPosition + startPos, prefabInstance.boundingBoxPosition + startPos + size);
        if (sleeperVolume != -1)
          _world.GetSleeperVolume(sleeperVolume).DespawnAndReset(_world);
      }
      for (int index3 = 0; index3 < prefabInstance.prefab.TriggerVolumes.Count; ++index3)
      {
        Vector3i startPos = prefabInstance.prefab.TriggerVolumes[index3].startPos;
        Vector3i size = prefabInstance.prefab.TriggerVolumes[index3].size;
        int triggerVolume = GameManager.Instance.World.FindTriggerVolume(prefabInstance.boundingBoxPosition + startPos, prefabInstance.boundingBoxPosition + startPos + size);
        if (triggerVolume != -1)
          _world.GetTriggerVolume(triggerVolume).Reset();
      }
      _world.triggerManager.RefreshTriggers(prefabInstance, questTags);
      if (prefabInstance.prefab.GetQuestTag(questTags) && (prefabInstance.lockInstance == null || prefabInstance.lockInstance.CheckQuestLock()))
      {
        prefabInstance.lockInstance = new QuestLockInstance(entityID);
        if (sharedWith != null)
          prefabInstance.lockInstance.AddQuesters(sharedWith);
      }
    }
    bool finished = false;
    while (!finished)
    {
      int index = 0;
      while (index < prefabInstances.Count && prefabInstances[index].bPrefabCopiedIntoWorld)
        ++index;
      finished = index >= prefabInstances.Count;
      if (!finished)
        yield return (object) null;
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  static World()
  {
  }

  public delegate void OnEntityLoadedDelegate(Entity _entity);

  public delegate void OnEntityUnloadedDelegate(Entity _entity, EnumRemoveEntityReason _reason);

  public delegate void OnWorldChangedEvent(string _sWorldName);

  [PublicizedFrom(EAccessModifier.Private)]
  public class ClipBlock
  {
    public const int kMaxBlocks = 32 /*0x20*/;
    public BlockValue value;
    public Vector3 pos;
    public Block block;
    public Vector3 bmins;
    public Vector3 bmaxs;
    [PublicizedFrom(EAccessModifier.Private)]
    public static int _storageIndex = 0;
    [PublicizedFrom(EAccessModifier.Private)]
    public static World.ClipBlock[] _storage = new World.ClipBlock[32 /*0x20*/];

    public static void ResetStorage() => World.ClipBlock._storageIndex = 0;

    public static World.ClipBlock New(
      BlockValue _value,
      Block _block,
      float _yDistort,
      Vector3 _blockPos,
      Bounds _bounds)
    {
      World.ClipBlock clipBlock = World.ClipBlock._storage[World.ClipBlock._storageIndex];
      if (clipBlock == null)
      {
        clipBlock = new World.ClipBlock();
        World.ClipBlock._storage[World.ClipBlock._storageIndex] = clipBlock;
      }
      clipBlock.Init(_value, _block, _yDistort, _blockPos, _bounds);
      ++World.ClipBlock._storageIndex;
      return clipBlock;
    }

    [PublicizedFrom(EAccessModifier.Private)]
    public void Init(
      BlockValue _value,
      Block _block,
      float _yDistort,
      Vector3 _blockPos,
      Bounds _bounds)
    {
      this.value = _value;
      this.block = _block;
      this.pos = _blockPos;
      Bounds bounds = _bounds;
      ref Bounds local1 = ref bounds;
      ((Bounds) ref local1).center = Vector3.op_Subtraction(((Bounds) ref local1).center, _blockPos);
      ref Bounds local2 = ref bounds;
      ((Bounds) ref local2).min = Vector3.op_Subtraction(((Bounds) ref local2).min, new Vector3(0.0f, _yDistort, 0.0f));
      this.bmins = ((Bounds) ref bounds).min;
      this.bmaxs = ((Bounds) ref bounds).max;
    }

    [PublicizedFrom(EAccessModifier.Private)]
    static ClipBlock()
    {
    }
  }

  public enum WorldEvent
  {
    BloodMoon,
  }
}
