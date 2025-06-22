// Decompiled with JetBrains decompiler
// Type: PrefabEditModeManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class PrefabEditModeManager
{
  public const int cGroundGridYDefault = 1;
  [PublicizedFrom(EAccessModifier.Private)]
  public const string SingleFacingBoxName = "single";
  public static PrefabEditModeManager Instance;
  public PathAbstractions.AbstractedLocation LoadedPrefab;
  public Prefab VoxelPrefab;
  public GameObject ImposterPrefab;
  public bool NeedsSaving;
  public Vector3i minPos;
  public Vector3i maxPos;
  [PublicizedFrom(EAccessModifier.Private)]
  public readonly Dictionary<PathAbstractions.AbstractedLocation, Prefab> loadedPrefabHeaders = new Dictionary<PathAbstractions.AbstractedLocation, Prefab>();
  [PublicizedFrom(EAccessModifier.Private)]
  public int curGridYPos;
  [PublicizedFrom(EAccessModifier.Private)]
  public GameObject groundGrid;
  [PublicizedFrom(EAccessModifier.Private)]
  public int prefabInstanceId = -1;
  [PublicizedFrom(EAccessModifier.Private)]
  public FileSystemWatcher xmlWatcher;
  [PublicizedFrom(EAccessModifier.Private)]
  public SelectionBox boxShowFacing;
  [PublicizedFrom(EAccessModifier.Private)]
  public bool bShowFacing;
  [PublicizedFrom(EAccessModifier.Private)]
  public bool bShowQuestLoot;
  [PublicizedFrom(EAccessModifier.Private)]
  public bool bShowBlockTriggers;
  public List<byte> TriggerLayers = new List<byte>();
  [PublicizedFrom(EAccessModifier.Private)]
  public bool showCompositionGrid;
  [CompilerGenerated]
  [PublicizedFrom(EAccessModifier.Private)]
  public bool \u003CHighlightingBlocks\u003Ek__BackingField;
  [PublicizedFrom(EAccessModifier.Private)]
  public int highlightBlockId;

  public event Action<PrefabInstance> OnPrefabChanged;

  public PrefabEditModeManager() => PrefabEditModeManager.Instance = this;

  public void Init()
  {
    this.ReloadAllXmls();
    this.InitXmlWatcher();
    if (this.IsActive())
      SelectionBoxManager.Instance.GetCategory("DynamicPrefabs").SetVisible(false);
    this.NeedsSaving = false;
    GameManager.Instance.World.ChunkClusters[0].OnBlockChangedDelegates += new ChunkCluster.OnBlockChangedDelegate(this.blockChangeDelegate);
  }

  public void Update()
  {
    if (!this.IsActive())
      return;
    this.updateCompositionGrid();
  }

  public bool IsActive()
  {
    return GameManager.Instance.IsEditMode() && GamePrefs.GetString(EnumGamePrefs.GameWorld) == "Empty";
  }

  public void LoadRecentlyUsedOrCreateNew()
  {
    if (this.VoxelPrefab != null)
      return;
    string _name = GamePrefs.GetString(EnumGamePrefs.LastLoadedPrefab);
    PathAbstractions.AbstractedLocation abstractedLocation = PathAbstractions.AbstractedLocation.None;
    if (!string.IsNullOrEmpty(_name))
      abstractedLocation = PathAbstractions.PrefabsSearchPaths.GetLocation(_name);
    if (abstractedLocation.Exists())
      ThreadManager.StartCoroutine(this.loadLastUsedPrefabLater());
    else
      this.NewVoxelPrefab();
  }

  public void LoadRecentlyUsed()
  {
    string str = GamePrefs.GetString(EnumGamePrefs.LastLoadedPrefab);
    if (string.IsNullOrEmpty(str) || this.VoxelPrefab != null && !(str != this.VoxelPrefab.PrefabName))
      return;
    ThreadManager.StartCoroutine(this.loadLastUsedPrefabLater());
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public IEnumerator loadLastUsedPrefabLater()
  {
    yield return (object) new WaitForSeconds(1f);
    ChunkCluster cc = GameManager.Instance.World.ChunkCache;
    foreach (Chunk chunk in cc.GetChunkArrayCopySync())
    {
      Chunk c = chunk;
      if (!cc.IsOnBorder(c) && !c.IsEmpty())
      {
        while (c.NeedsRegeneration || c.NeedsCopying)
          yield return (object) new WaitForSeconds(1f);
        c = (Chunk) null;
      }
    }
    this.LoadVoxelPrefab(PathAbstractions.PrefabsSearchPaths.GetLocation(GamePrefs.GetString(EnumGamePrefs.LastLoadedPrefab)));
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void InitXmlWatcher()
  {
    string gameDir = GameIO.GetGameDir("Data/Prefabs");
    Log.Out("Watching prefabs folder for XML changes: " + gameDir);
    this.xmlWatcher = new FileSystemWatcher(gameDir, "*.xml");
    this.xmlWatcher.IncludeSubdirectories = true;
    this.xmlWatcher.Changed += new FileSystemEventHandler(this.OnXmlFileChanged);
    this.xmlWatcher.Created += new FileSystemEventHandler(this.OnXmlFileChanged);
    this.xmlWatcher.Deleted += new FileSystemEventHandler(this.OnXmlFileChanged);
    this.xmlWatcher.EnableRaisingEvents = true;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void OnXmlFileChanged(object _sender, FileSystemEventArgs _e)
  {
    Log.Out($"Prefab XML {_e.ChangeType}: {_e.Name}");
    PathAbstractions.AbstractedLocation abstractedLocation = new PathAbstractions.AbstractedLocation(PathAbstractions.EAbstractedLocationType.GameData, Path.GetFileNameWithoutExtension(_e.Name), Path.ChangeExtension(_e.FullPath, ".tts"), (string) null, false);
    if (_e.ChangeType == WatcherChangeTypes.Deleted)
    {
      lock (this.loadedPrefabHeaders)
        this.loadedPrefabHeaders.Remove(abstractedLocation);
    }
    else
    {
      this.LoadXml(abstractedLocation);
      if (this.VoxelPrefab != null && this.VoxelPrefab.location == abstractedLocation)
      {
        Log.Out("Applying XML changes to loaded prefab");
        this.VoxelPrefab.LoadXMLData(this.VoxelPrefab.location);
      }
      else
      {
        if (this.VoxelPrefab == null)
          return;
        Log.Out($"XML changed not related to loaded prefab. (Loaded: {this.VoxelPrefab.location}, FP {this.VoxelPrefab.location.FullPath}; Changed: {abstractedLocation}, FP {abstractedLocation.FullPath})");
      }
    }
  }

  public void ReloadAllXmls()
  {
    lock (this.loadedPrefabHeaders)
    {
      this.loadedPrefabHeaders.Clear();
      foreach (PathAbstractions.AbstractedLocation availablePaths in PathAbstractions.PrefabsSearchPaths.GetAvailablePathsList())
        this.LoadXml(availablePaths);
    }
  }

  public void LoadXml(PathAbstractions.AbstractedLocation _location)
  {
    Prefab prefab = new Prefab();
    prefab.LoadXMLData(_location);
    lock (this.loadedPrefabHeaders)
      this.loadedPrefabHeaders[_location] = prefab;
  }

  public void Cleanup()
  {
    if (this.xmlWatcher != null)
    {
      this.xmlWatcher.EnableRaisingEvents = false;
      this.xmlWatcher = (FileSystemWatcher) null;
    }
    lock (this.loadedPrefabHeaders)
      this.loadedPrefabHeaders.Clear();
    if (Object.op_Implicit((Object) this.groundGrid))
    {
      Object.Destroy((Object) this.groundGrid);
      this.groundGrid = (GameObject) null;
    }
    if (this.prefabInstanceId != -1)
    {
      DynamicPrefabDecorator dynamicPrefabDecorator = GameManager.Instance.GetDynamicPrefabDecorator();
      dynamicPrefabDecorator.RemovePrefabAndSelection(GameManager.Instance.World, dynamicPrefabDecorator.GetPrefab(this.prefabInstanceId), false);
      this.prefabInstanceId = -1;
    }
    GameManager.Instance.World.ChunkClusters[0].OnBlockChangedDelegates -= new ChunkCluster.OnBlockChangedDelegate(this.blockChangeDelegate);
    this.ClearImposterPrefab();
    this.ClearVoxelPrefab();
    this.OnPrefabChanged = (Action<PrefabInstance>) null;
    this.HighlightBlocks((Block) null);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void blockChangeDelegate(
    Vector3i pos,
    BlockValue bvOld,
    sbyte oldDens,
    TextureFullArray oldTex,
    BlockValue bvNew)
  {
    this.NeedsSaving = true;
  }

  public void FindPrefabs(string _group, List<PathAbstractions.AbstractedLocation> _result)
  {
    lock (this.loadedPrefabHeaders)
    {
      foreach (KeyValuePair<PathAbstractions.AbstractedLocation, Prefab> loadedPrefabHeader in this.loadedPrefabHeaders)
      {
        switch (_group)
        {
          case null:
            _result.Add(loadedPrefabHeader.Key);
            continue;
          case "":
            if (loadedPrefabHeader.Value.editorGroups == null || loadedPrefabHeader.Value.editorGroups.Count == 0)
            {
              _result.Add(loadedPrefabHeader.Key);
              continue;
            }
            continue;
          default:
            if (loadedPrefabHeader.Value.editorGroups != null)
            {
              for (int index = 0; index < loadedPrefabHeader.Value.editorGroups.Count; ++index)
              {
                if (string.Compare(loadedPrefabHeader.Value.editorGroups[index], _group, StringComparison.OrdinalIgnoreCase) == 0)
                {
                  _result.Add(loadedPrefabHeader.Key);
                  break;
                }
              }
              continue;
            }
            continue;
        }
      }
    }
  }

  public void GetAllTags(List<string> _result, Prefab _considerLoadedPrefab = null)
  {
    lock (this.loadedPrefabHeaders)
    {
      foreach (KeyValuePair<PathAbstractions.AbstractedLocation, Prefab> loadedPrefabHeader in this.loadedPrefabHeaders)
      {
        FastTags<TagGroup.Poi> tags = loadedPrefabHeader.Value.Tags;
        if (!tags.IsEmpty)
        {
          tags = loadedPrefabHeader.Value.Tags;
          foreach (string tagName in tags.GetTagNames())
          {
            if (!_result.ContainsCaseInsensitive(tagName))
              _result.Add(tagName);
          }
        }
      }
    }
    if ((_considerLoadedPrefab != null ? (_considerLoadedPrefab.Tags.IsEmpty ? 1 : 0) : 1) != 0)
      return;
    foreach (string tagName in _considerLoadedPrefab.Tags.GetTagNames())
    {
      if (!_result.ContainsCaseInsensitive(tagName))
        _result.Add(tagName);
    }
  }

  public void GetAllThemeTags(List<string> _result, Prefab _considerLoadedPrefab = null)
  {
    lock (this.loadedPrefabHeaders)
    {
      foreach (KeyValuePair<PathAbstractions.AbstractedLocation, Prefab> loadedPrefabHeader in this.loadedPrefabHeaders)
      {
        FastTags<TagGroup.Poi> themeTags = loadedPrefabHeader.Value.ThemeTags;
        if (!themeTags.IsEmpty)
        {
          themeTags = loadedPrefabHeader.Value.ThemeTags;
          foreach (string tagName in themeTags.GetTagNames())
          {
            if (!_result.ContainsCaseInsensitive(tagName))
              _result.Add(tagName);
          }
        }
      }
    }
    if ((_considerLoadedPrefab != null ? (_considerLoadedPrefab.ThemeTags.IsEmpty ? 1 : 0) : 1) != 0)
      return;
    foreach (string tagName in _considerLoadedPrefab.ThemeTags.GetTagNames())
    {
      if (!_result.ContainsCaseInsensitive(tagName))
        _result.Add(tagName);
    }
  }

  public void GetAllGroups(List<string> _result, Prefab _considerLoadedPrefab = null)
  {
    lock (this.loadedPrefabHeaders)
    {
      foreach (KeyValuePair<PathAbstractions.AbstractedLocation, Prefab> loadedPrefabHeader in this.loadedPrefabHeaders)
      {
        if (loadedPrefabHeader.Value.editorGroups != null)
        {
          foreach (string editorGroup in loadedPrefabHeader.Value.editorGroups)
          {
            if (!_result.ContainsCaseInsensitive(editorGroup))
              _result.Add(editorGroup);
          }
        }
      }
    }
    if (_considerLoadedPrefab?.editorGroups == null)
      return;
    foreach (string editorGroup in _considerLoadedPrefab.editorGroups)
    {
      if (!_result.ContainsCaseInsensitive(editorGroup))
        _result.Add(editorGroup);
    }
  }

  public void GetAllZones(List<string> _result, Prefab _considerLoadedPrefab = null)
  {
    lock (this.loadedPrefabHeaders)
    {
      foreach (KeyValuePair<PathAbstractions.AbstractedLocation, Prefab> loadedPrefabHeader in this.loadedPrefabHeaders)
      {
        string[] allowedZones = loadedPrefabHeader.Value.GetAllowedZones();
        if (allowedZones != null)
        {
          foreach (string str in allowedZones)
          {
            if (!_result.ContainsCaseInsensitive(str))
              _result.Add(str);
          }
        }
      }
    }
    if (_considerLoadedPrefab == null)
      return;
    string[] allowedZones1 = _considerLoadedPrefab.GetAllowedZones();
    if (allowedZones1 == null)
      return;
    foreach (string str in allowedZones1)
    {
      if (!_result.ContainsCaseInsensitive(str))
        _result.Add(str);
    }
  }

  public void GetAllQuestTags(List<string> _result, Prefab _considerLoadedPrefab = null)
  {
    if (_considerLoadedPrefab != null)
    {
      string[] array = _considerLoadedPrefab.GetQuestTags().ToString().Split(',', StringSplitOptions.None);
      Array.Sort<string>(array);
      for (int index = 0; index < array.Length; ++index)
      {
        string str = array[index].Trim();
        if (!_result.ContainsCaseInsensitive(str))
          _result.Add(str);
      }
    }
    string[] array1 = QuestEventManager.allQuestTags.ToString().Split(',', StringSplitOptions.None);
    Array.Sort<string>(array1);
    for (int index = 0; index < array1.Length; ++index)
    {
      string str = array1[index].Trim();
      if (!_result.ContainsCaseInsensitive(str))
        _result.Add(str);
    }
  }

  public bool HasPrefabImposter(PathAbstractions.AbstractedLocation _location)
  {
    return SdFile.Exists(_location.FullPathNoExtension + ".mesh");
  }

  public void ClearImposterPrefab()
  {
    Object.Destroy((Object) this.ImposterPrefab);
    this.ImposterPrefab = (GameObject) null;
  }

  public bool LoadImposterPrefab(PathAbstractions.AbstractedLocation _location)
  {
    this.ClearImposterPrefab();
    this.ClearVoxelPrefab();
    if (!SdFile.Exists(_location.FullPathNoExtension + ".mesh"))
      return false;
    this.LoadedPrefab = _location;
    bool bTextureArray = MeshDescription.meshes[0].bTextureArray;
    this.ImposterPrefab = SimpleMeshFile.ReadGameObject(_location.FullPathNoExtension + ".mesh", _bTextureArray: bTextureArray);
    ((Object) this.ImposterPrefab.transform).name = _location.Name;
    this.ImposterPrefab.transform.position = new Vector3(0.0f, -3f, 0.0f);
    return true;
  }

  public bool IsShowingImposterPrefab()
  {
    return Object.op_Inequality((Object) this.ImposterPrefab, (Object) null);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void removeAllChunks()
  {
    GameManager.Instance.World.m_ChunkManager.RemoveAllChunks();
    GameManager.Instance.World.ChunkCache.Clear();
    WaterSimulationNative.Instance.Clear();
  }

  public void ClearVoxelPrefab()
  {
    SelectionBoxManager.Instance.Unselect();
    SelectionBoxManager.Instance.GetCategory("DynamicPrefabs").Clear();
    SelectionBoxManager.Instance.GetCategory("TraderTeleport").Clear();
    SelectionBoxManager.Instance.GetCategory("SleeperVolume").Clear();
    SelectionBoxManager.Instance.GetCategory("InfoVolume").Clear();
    SelectionBoxManager.Instance.GetCategory("WallVolume").Clear();
    SelectionBoxManager.Instance.GetCategory("TriggerVolume").Clear();
    SelectionBoxManager.Instance.GetCategory("POIMarker").Clear();
    SleeperVolumeToolManager.CleanUp();
    this.prefabInstanceId = -1;
    this.LoadedPrefab = PathAbstractions.AbstractedLocation.None;
    this.VoxelPrefab = (Prefab) null;
    this.removeAllChunks();
    DecoManager.Instance.OnWorldUnloaded();
    ThreadManager.RunCoroutineSync(DecoManager.Instance.OnWorldLoaded(1024 /*0x0400*/, 1024 /*0x0400*/, GameManager.Instance.World, (IChunkProvider) null));
    this.TogglePrefabFacing(false);
    this.HighlightQuestLoot = this.HighlightQuestLoot;
    this.HighlightBlockTriggers = this.HighlightBlockTriggers;
    this.showCompositionGrid = false;
  }

  public bool NewVoxelPrefab()
  {
    this.ClearImposterPrefab();
    this.ClearVoxelPrefab();
    this.VoxelPrefab = new Prefab();
    DynamicPrefabDecorator dynamicPrefabDecorator = GameManager.Instance.GetDynamicPrefabDecorator();
    if (this.prefabInstanceId != -1)
      dynamicPrefabDecorator.RemovePrefabAndSelection(GameManager.Instance.World, dynamicPrefabDecorator.GetPrefab(this.prefabInstanceId), false);
    dynamicPrefabDecorator.ClearAllPrefabs();
    this.prefabInstanceId = dynamicPrefabDecorator.CreateNewPrefabAndActivate(Prefab.LocationForNewPrefab("New Prefab"), Vector3i.zero, this.VoxelPrefab).id;
    SelectionBoxManager.Instance.GetCategory("DynamicPrefabs").SetVisible(false);
    this.curGridYPos = this.VoxelPrefab.yOffset;
    if (Object.op_Implicit((Object) this.groundGrid))
      this.groundGrid.transform.position = new Vector3(0.0f, (float) (1 - this.curGridYPos) - 0.01f, 0.0f);
    GameManager.Instance.prefabEditModeManager.ToggleGroundGrid(true);
    for (int index1 = -6; index1 <= 6; ++index1)
    {
      for (int index2 = -6; index2 <= 6; ++index2)
      {
        Chunk chunk = MemoryPools.PoolChunks.AllocSync(true);
        chunk.X = index1;
        chunk.Z = index2;
        chunk.ResetBiomeIntensity(BiomeIntensity.Default);
        chunk.NeedsRegeneration = true;
        chunk.NeedsLightCalculation = false;
        chunk.NeedsDecoration = false;
        chunk.ResetLights(byte.MaxValue);
        GameManager.Instance.World.ChunkCache.AddChunkSync(chunk, false);
        WaterSimulationNative.Instance.InitializeChunk(chunk);
      }
    }
    this.NeedsSaving = false;
    GamePrefs.Set(EnumGamePrefs.LastLoadedPrefab, string.Empty);
    GameManager.Instance.World.m_ChunkManager.RemoveAllChunksOnAllClients();
    WaterSimulationNative.Instance.SetPaused(true);
    return true;
  }

  public bool LoadVoxelPrefab(
    PathAbstractions.AbstractedLocation _location,
    bool _bBulk = false,
    bool _bIgnoreExcludeImposterCheck = false)
  {
    this.ClearImposterPrefab();
    this.ClearVoxelPrefab();
    this.highlightBlocks(0);
    if (_location.Type == PathAbstractions.EAbstractedLocationType.None)
    {
      Log.Out("No prefab found to load!");
      return false;
    }
    this.VoxelPrefab = new Prefab();
    if (!this.VoxelPrefab.Load(_location, _allowMissingBlocks: true))
    {
      Log.Out($"Error loading prefab {_location}");
      this.VoxelPrefab = (Prefab) null;
      return false;
    }
    if (!_bIgnoreExcludeImposterCheck & _bBulk && this.VoxelPrefab.bExcludeDistantPOIMesh)
    {
      this.VoxelPrefab = (Prefab) null;
      return false;
    }
    int num1 = this.VoxelPrefab.size.x * this.VoxelPrefab.size.y * this.VoxelPrefab.size.z;
    if (!_bIgnoreExcludeImposterCheck & _bBulk && (this.VoxelPrefab.size.y <= 6 && num1 < 1500 || this.VoxelPrefab.size.y > 6 && num1 < 100))
    {
      this.VoxelPrefab = (Prefab) null;
      return false;
    }
    this.LoadedPrefab = _location;
    this.curGridYPos = this.VoxelPrefab.yOffset;
    if (Object.op_Implicit((Object) this.groundGrid))
      this.groundGrid.transform.position = new Vector3(0.0f, (float) (1 - this.curGridYPos) - 0.01f, 0.0f);
    ChunkCluster chunkCache = GameManager.Instance.World.ChunkCache;
    this.removeAllChunks();
    int _x = -1 * this.VoxelPrefab.size.x / 2;
    int _z = -1 * this.VoxelPrefab.size.z / 2;
    int num2 = _x + this.VoxelPrefab.size.x;
    int num3 = _z + this.VoxelPrefab.size.z;
    chunkCache.ChunkMinPos = new Vector2i((_x - 1) / 16 /*0x10*/ - 1, (_z - 1) / 16 /*0x10*/ - 1);
    ChunkCluster chunkCluster1 = chunkCache;
    chunkCluster1.ChunkMinPos = chunkCluster1.ChunkMinPos - new Vector2i(2, 2);
    chunkCache.ChunkMaxPos = new Vector2i(num2 / 16 /*0x10*/ + 1, num3 / 16 /*0x10*/ + 1);
    ChunkCluster chunkCluster2 = chunkCache;
    chunkCluster2.ChunkMaxPos = chunkCluster2.ChunkMaxPos + new Vector2i(2, 2);
    List<Chunk> chunkList = new List<Chunk>();
    for (int x = chunkCache.ChunkMinPos.x; x <= chunkCache.ChunkMaxPos.x; ++x)
    {
      for (int y = chunkCache.ChunkMinPos.y; y <= chunkCache.ChunkMaxPos.y; ++y)
      {
        Chunk _chunk = MemoryPools.PoolChunks.AllocSync(true);
        _chunk.X = x;
        _chunk.Z = y;
        _chunk.SetFullSunlight();
        _chunk.NeedsLightCalculation = false;
        _chunk.NeedsDecoration = false;
        _chunk.NeedsRegeneration = false;
        chunkCache.AddChunkSync(_chunk, true);
        chunkList.Add(_chunk);
      }
    }
    Vector3i vector3i = new Vector3i(_x, 1, _z);
    this.VoxelPrefab.CopyIntoLocal(chunkCache, vector3i, true, false, FastTags<TagGroup.Global>.none);
    for (int index = 0; index < chunkList.Count; ++index)
    {
      Chunk _c = chunkList[index];
      _c.NeedsLightCalculation = false;
      _c.NeedsRegeneration = true;
      WaterSimulationNative.Instance.InitializeChunk(_c);
    }
    DynamicPrefabDecorator dynamicPrefabDecorator = GameManager.Instance.GetDynamicPrefabDecorator();
    if (this.prefabInstanceId != -1)
      dynamicPrefabDecorator.RemovePrefabAndSelection(GameManager.Instance.World, dynamicPrefabDecorator.GetPrefab(this.prefabInstanceId), false);
    dynamicPrefabDecorator.ClearAllPrefabs();
    this.prefabInstanceId = dynamicPrefabDecorator.CreateNewPrefabAndActivate(this.VoxelPrefab.location, vector3i, this.VoxelPrefab, false).id;
    this.NeedsSaving = false;
    GamePrefs.Set(EnumGamePrefs.LastLoadedPrefab, _location.Name);
    GameManager.Instance.World.m_ChunkManager.RemoveAllChunksOnAllClients();
    this.HighlightQuestLoot = this.HighlightQuestLoot;
    this.HighlightBlockTriggers = this.HighlightBlockTriggers;
    WaterSimulationNative.Instance.SetPaused(true);
    this.highlightBlocks(this.highlightBlockId);
    return true;
  }

  public bool SaveVoxelPrefab()
  {
    if (this.VoxelPrefab == null)
      return false;
    int num1 = Chunk.IgnorePaintTextures ? 1 : 0;
    Chunk.IgnorePaintTextures = false;
    this.updatePrefabBounds();
    Chunk.IgnorePaintTextures = num1 != 0;
    this.VoxelPrefab.RecalcInsideDevices(this.VoxelPrefab.UpdateInsideOutside(this.minPos, this.maxPos));
    int num2 = this.VoxelPrefab.Save(this.VoxelPrefab.location) ? 1 : 0;
    if (num2 != 0)
    {
      this.LoadedPrefab = this.VoxelPrefab.location;
      this.LoadXml(this.VoxelPrefab.location);
      GamePrefs.Set(EnumGamePrefs.LastLoadedPrefab, this.VoxelPrefab.PrefabName);
    }
    this.NeedsSaving = false;
    return num2 != 0;
  }

  public void UpdateMinMax()
  {
    if (this.VoxelPrefab == null)
      return;
    Vector3i vector3i1 = new Vector3i(int.MaxValue, int.MaxValue, int.MaxValue);
    Vector3i vector3i2 = new Vector3i(int.MinValue, int.MinValue, int.MinValue);
    foreach (Chunk chunk in GameManager.Instance.World.ChunkCache.GetChunkArrayCopySync())
    {
      for (int _x = 0; _x < 16 /*0x10*/; ++_x)
      {
        for (int _z = 0; _z < 16 /*0x10*/; ++_z)
        {
          for (int _y = 0; _y < 256 /*0x0100*/; ++_y)
          {
            int blockId = chunk.GetBlockId(_x, _y, _z);
            WaterValue water = chunk.GetWater(_x, _y, _z);
            if (blockId != 0 || water.HasMass() || chunk.GetDensity(_x, _y, _z) < (sbyte) 0)
            {
              Vector3i worldPos = chunk.ToWorldPos(new Vector3i(_x, _y, _z));
              if (vector3i1.x > worldPos.x)
                vector3i1.x = worldPos.x;
              if (vector3i1.y > worldPos.y)
                vector3i1.y = worldPos.y;
              if (vector3i1.z > worldPos.z)
                vector3i1.z = worldPos.z;
              if (vector3i2.x < worldPos.x)
                vector3i2.x = worldPos.x;
              if (vector3i2.y < worldPos.y)
                vector3i2.y = worldPos.y;
              if (vector3i2.z < worldPos.z)
                vector3i2.z = worldPos.z;
            }
          }
        }
      }
    }
    if (vector3i1.x == int.MaxValue)
    {
      vector3i1 = Vector3i.zero;
      vector3i2 = Vector3i.zero;
    }
    this.minPos = vector3i1;
    this.maxPos = vector3i2;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void updatePrefabBounds()
  {
    if (this.VoxelPrefab == null)
      return;
    this.VoxelPrefab.yOffset = this.curGridYPos;
    this.UpdateMinMax();
    this.VoxelPrefab.CopyFromWorldWithEntities(GameManager.Instance.World, this.minPos, this.maxPos, (ICollection<int>) new List<int>());
    if (this.prefabInstanceId == -1)
      return;
    GameManager.Instance.GetDynamicPrefabDecorator().GetPrefab(this.prefabInstanceId).UpdateBoundingBoxPosAndScale(this.minPos, this.VoxelPrefab.size);
  }

  public void SetGroundLevel(int _yOffset)
  {
    this.curGridYPos = _yOffset;
    if (!Object.op_Implicit((Object) this.groundGrid))
      return;
    this.groundGrid.transform.position = new Vector3(0.0f, (float) (1 - this.curGridYPos) - 0.01f, 0.0f);
  }

  public void ToggleGroundGrid(bool _bForceOn = false)
  {
    if (Object.op_Equality((Object) this.groundGrid, (Object) null))
    {
      this.groundGrid = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/GroundGrid/GroundGrid"));
      this.groundGrid.transform.position = new Vector3(0.0f, (float) (1 - this.curGridYPos) + 0.01f, 0.0f);
      for (int index = 0; index < this.groundGrid.transform.childCount; ++index)
        ((Component) this.groundGrid.transform.GetChild(index)).gameObject.tag = "B_Mesh";
    }
    else
      this.groundGrid.SetActive(_bForceOn || !this.groundGrid.activeSelf);
  }

  public bool IsGroundGrid()
  {
    return Object.op_Inequality((Object) this.groundGrid, (Object) null) && this.groundGrid.activeSelf;
  }

  public void ToggleCompositionGrid()
  {
    this.showCompositionGrid = !this.showCompositionGrid;
    if (!this.showCompositionGrid)
      return;
    SelectionCategory category = SelectionBoxManager.Instance.GetCategory("DynamicPrefabs");
    int num = category.IsVisible() ? 1 : 0;
    this.UpdatePrefabBounds();
    if (num != 0)
      return;
    category.SetVisible(false);
  }

  public bool IsCompositionGrid() => this.IsActive() && this.showCompositionGrid;

  [PublicizedFrom(EAccessModifier.Private)]
  public void updateCompositionGrid()
  {
    Color color1 = Color32.op_Implicit(new Color32((byte) 0, byte.MaxValue, (byte) 40, byte.MaxValue));
    Color color2 = Color32.op_Implicit(new Color32((byte) 0, byte.MaxValue, (byte) 200, byte.MaxValue));
    Color color3 = Color32.op_Implicit(new Color32((byte) 0, byte.MaxValue, (byte) 200, byte.MaxValue));
    if (!this.showCompositionGrid)
      return;
    Vector3i vector3i1 = this.maxPos + Vector3i.one;
    Vector3i vector3i2 = vector3i1 - this.minPos;
    if (vector3i2 == Vector3i.one)
      return;
    EntityPlayerLocal primaryPlayer = GameManager.Instance.World?.GetPrimaryPlayer();
    if (Object.op_Equality((Object) primaryPlayer, (Object) null))
      return;
    float num1 = (float) (1 - this.curGridYPos) + 0.05f;
    float num2 = (float) vector3i2.x / 1.618034f;
    int num3 = Mathf.RoundToInt(num2);
    int num4 = Mathf.RoundToInt((float) (((double) vector3i2.x - (double) num2) / 2.0));
    float num5 = (float) vector3i2.z / 1.618034f;
    int num6 = Mathf.RoundToInt(num5);
    int num7 = Mathf.RoundToInt((float) (((double) vector3i2.z - (double) num5) / 2.0));
    float num8 = (float) vector3i2.x / 2f;
    float num9 = (float) vector3i2.z / 2f;
    Vector3 _pos1;
    // ISSUE: explicit constructor call
    ((Vector3) ref _pos1).\u002Ector((float) (this.minPos.x + num3), num1, (float) this.minPos.z);
    Vector3 _pos2;
    // ISSUE: explicit constructor call
    ((Vector3) ref _pos2).\u002Ector((float) (this.minPos.x + num3), num1, (float) vector3i1.z);
    DebugLines.Create("GoldenRatio_X1", primaryPlayer.RootTransform, _pos1, _pos2, color1, color1, 0.1f, 0.1f, 0.1f);
    // ISSUE: explicit constructor call
    ((Vector3) ref _pos1).\u002Ector((float) (vector3i1.x - num3), num1, (float) this.minPos.z);
    // ISSUE: explicit constructor call
    ((Vector3) ref _pos2).\u002Ector((float) (vector3i1.x - num3), num1, (float) vector3i1.z);
    DebugLines.Create("GoldenRatio_X2", primaryPlayer.RootTransform, _pos1, _pos2, color1, color1, 0.1f, 0.1f, 0.1f);
    // ISSUE: explicit constructor call
    ((Vector3) ref _pos1).\u002Ector((float) this.minPos.x, num1, (float) (this.minPos.z + num6));
    // ISSUE: explicit constructor call
    ((Vector3) ref _pos2).\u002Ector((float) vector3i1.x, num1, (float) (this.minPos.z + num6));
    DebugLines.Create("GoldenRatio_Z1", primaryPlayer.RootTransform, _pos1, _pos2, color1, color1, 0.1f, 0.1f, 0.1f);
    // ISSUE: explicit constructor call
    ((Vector3) ref _pos1).\u002Ector((float) this.minPos.x, num1, (float) (vector3i1.z - num6));
    // ISSUE: explicit constructor call
    ((Vector3) ref _pos2).\u002Ector((float) vector3i1.x, num1, (float) (vector3i1.z - num6));
    DebugLines.Create("GoldenRatio_Z2", primaryPlayer.RootTransform, _pos1, _pos2, color1, color1, 0.1f, 0.1f, 0.1f);
    // ISSUE: explicit constructor call
    ((Vector3) ref _pos1).\u002Ector((float) this.minPos.x + num8, num1, (float) this.minPos.z);
    // ISSUE: explicit constructor call
    ((Vector3) ref _pos2).\u002Ector((float) this.minPos.x + num8, num1, (float) vector3i1.z);
    DebugLines.Create("GoldenRatio_InnerX", primaryPlayer.RootTransform, _pos1, _pos2, color2, color2, 0.03f, 0.03f, 0.1f);
    // ISSUE: explicit constructor call
    ((Vector3) ref _pos1).\u002Ector((float) this.minPos.x, num1, (float) this.minPos.z + num9);
    // ISSUE: explicit constructor call
    ((Vector3) ref _pos2).\u002Ector((float) vector3i1.x, num1, (float) this.minPos.z + num9);
    DebugLines.Create("GoldenRatio_InnerZ", primaryPlayer.RootTransform, _pos1, _pos2, color2, color2, 0.03f, 0.03f, 0.1f);
    // ISSUE: explicit constructor call
    ((Vector3) ref _pos1).\u002Ector((float) (this.minPos.x + num4), num1, (float) this.minPos.z);
    // ISSUE: explicit constructor call
    ((Vector3) ref _pos2).\u002Ector((float) (this.minPos.x + num4), num1, (float) vector3i1.z);
    DebugLines.Create("GoldenRatio_OuterX1", primaryPlayer.RootTransform, _pos1, _pos2, color3, color3, 0.03f, 0.03f, 0.1f);
    // ISSUE: explicit constructor call
    ((Vector3) ref _pos1).\u002Ector((float) (vector3i1.x - num4), num1, (float) this.minPos.z);
    // ISSUE: explicit constructor call
    ((Vector3) ref _pos2).\u002Ector((float) (vector3i1.x - num4), num1, (float) vector3i1.z);
    DebugLines.Create("GoldenRatio_OuterX2", primaryPlayer.RootTransform, _pos1, _pos2, color3, color3, 0.03f, 0.03f, 0.1f);
    // ISSUE: explicit constructor call
    ((Vector3) ref _pos1).\u002Ector((float) this.minPos.x, num1, (float) (this.minPos.z + num7));
    // ISSUE: explicit constructor call
    ((Vector3) ref _pos2).\u002Ector((float) vector3i1.x, num1, (float) (this.minPos.z + num7));
    DebugLines.Create("GoldenRatio_OuterZ1", primaryPlayer.RootTransform, _pos1, _pos2, color3, color3, 0.03f, 0.03f, 0.1f);
    // ISSUE: explicit constructor call
    ((Vector3) ref _pos1).\u002Ector((float) this.minPos.x, num1, (float) (vector3i1.z - num7));
    // ISSUE: explicit constructor call
    ((Vector3) ref _pos2).\u002Ector((float) vector3i1.x, num1, (float) (vector3i1.z - num7));
    DebugLines.Create("GoldenRatio_OuterZ2", primaryPlayer.RootTransform, _pos1, _pos2, color3, color3, 0.03f, 0.03f, 0.1f);
  }

  public void UpdatePrefabBounds()
  {
    DynamicPrefabDecorator dynamicPrefabDecorator = GameManager.Instance.GetDynamicPrefabDecorator();
    if (this.prefabInstanceId == -1)
    {
      this.VoxelPrefab = new Prefab();
      this.prefabInstanceId = dynamicPrefabDecorator.CreateNewPrefabAndActivate(this.VoxelPrefab.location, Vector3i.zero, this.VoxelPrefab).id;
    }
    if (this.prefabInstanceId != -1)
    {
      SelectionBoxManager.Instance.GetCategory("DynamicPrefabs").GetBox(dynamicPrefabDecorator.GetPrefab(this.prefabInstanceId).name).SetVisible(true);
      SelectionBoxManager.Instance.GetCategory("DynamicPrefabs").SetVisible(true);
    }
    this.updatePrefabBounds();
  }

  public bool IsPrefabFacing() => this.bShowFacing;

  public void TogglePrefabFacing(bool _bShow)
  {
    if (this.VoxelPrefab == null)
      return;
    if (_bShow && Object.op_Equality((Object) this.boxShowFacing, (Object) null))
      this.boxShowFacing = SelectionBoxManager.Instance.GetCategory("PrefabFacing").AddBox("single", (Vector3) Vector3i.zero, Vector3i.one, true);
    this.bShowFacing = _bShow;
    this.updateFacing();
    if (!Object.op_Inequality((Object) this.boxShowFacing, (Object) null))
      return;
    if (this.bShowFacing)
      this.boxShowFacing.SetPositionAndSize(new Vector3(0.0f, 2f, (float) (-this.VoxelPrefab.size.z / 2 - 3)), Vector3i.one);
    SelectionBoxManager.Instance.SetActive("PrefabFacing", "single", this.bShowFacing);
    SelectionBoxManager.Instance.GetCategory("PrefabFacing").GetBox("single").SetVisible(this.bShowFacing);
    SelectionBoxManager.Instance.GetCategory("PrefabFacing").SetVisible(this.bShowFacing);
  }

  public void RotatePrefabFacing()
  {
    if (this.VoxelPrefab == null)
      return;
    ++this.VoxelPrefab.rotationToFaceNorth;
    this.VoxelPrefab.rotationToFaceNorth &= 3;
    this.updateFacing();
    this.NeedsSaving = true;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void updateFacing()
  {
    if (this.VoxelPrefab == null)
      return;
    float _facing = 0.0f;
    switch (this.VoxelPrefab.rotationToFaceNorth)
    {
      case 1:
        _facing = 90f;
        break;
      case 2:
        _facing = 180f;
        break;
      case 3:
        _facing = 270f;
        break;
    }
    SelectionBoxManager.Instance.SetFacingDirection("PrefabFacing", "single", _facing);
  }

  public void MoveGroundGridUpOrDown(int _deltaY)
  {
    if (Object.op_Implicit((Object) this.groundGrid) && this.groundGrid.activeSelf)
    {
      this.curGridYPos = Utils.FastClamp(this.curGridYPos - _deltaY, -200, 0);
      if (this.VoxelPrefab != null)
      {
        this.VoxelPrefab.yOffset = this.curGridYPos;
        if (this.prefabInstanceId != -1 && this.OnPrefabChanged != null)
          this.OnPrefabChanged(GameManager.Instance.GetDynamicPrefabDecorator().GetPrefab(this.prefabInstanceId));
      }
      if (Object.op_Implicit((Object) this.groundGrid))
        this.groundGrid.transform.position = new Vector3(0.0f, (float) (1 - this.curGridYPos) - 0.01f, 0.0f);
    }
    this.NeedsSaving = true;
  }

  public void MovePrefabUpOrDown(int _deltaY)
  {
    this.updatePrefabBounds();
    Vector3i _destinationPos = this.minPos + _deltaY * Vector3i.up;
    if (_destinationPos.y < 1 || this.maxPos.y + _deltaY > 250)
      return;
    ChunkCluster chunkCache = GameManager.Instance.World.ChunkCache;
    List<Chunk> chunkArrayCopySync = chunkCache.GetChunkArrayCopySync();
    foreach (Chunk chunk in chunkArrayCopySync)
    {
      chunk.RemoveAllTileEntities();
      if (!chunk.IsEmpty())
      {
        for (int _x = 0; _x < 16 /*0x10*/; ++_x)
        {
          for (int _z = 0; _z < 16 /*0x10*/; ++_z)
          {
            for (int _y = 0; _y < 254; ++_y)
            {
              chunk.SetWater(_x, _y, _z, WaterValue.Empty);
              BlockValue block = chunk.GetBlock(_x, _y, _z);
              if (!block.isair && !block.ischild)
              {
                chunkCache.SetBlock(chunk.ToWorldPos(new Vector3i(_x, _y, _z)), BlockValue.Air, true, false);
                chunk.SetDensity(_x, _y, _z, MarchingCubes.DensityAir);
                chunk.SetTextureFull(_x, _y, _z, 0L);
              }
            }
          }
        }
      }
    }
    this.VoxelPrefab.CopyIntoLocal(chunkCache, _destinationPos, true, false, FastTags<TagGroup.Global>.none);
    foreach (Chunk chunk in chunkArrayCopySync)
      chunk.NeedsRegeneration = true;
    GameManager.Instance.World.m_ChunkManager.RemoveAllChunksOnAllClients();
    this.UpdateMinMax();
    if (this.prefabInstanceId == -1)
      return;
    GameManager.Instance.GetDynamicPrefabDecorator().GetPrefab(this.prefabInstanceId).UpdateBoundingBoxPosAndScale(this.minPos, this.VoxelPrefab.size, false);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public bool checkLayerEmpty(int _y, List<Chunk> chunks)
  {
    bool bAllEmpty = true;
    for (int index = 0; index < chunks.Count; ++index)
      chunks[index].LoopOverAllBlocks((ChunkBlockLayer.LoopBlocksDelegate) ([PublicizedFrom(EAccessModifier.Internal)] (x, y, z, bv) =>
      {
        if (y != _y)
          return;
        bAllEmpty = false;
      }));
    return bAllEmpty;
  }

  public void StripTextures()
  {
    HashSet<Chunk> changedChunks = new HashSet<Chunk>();
    List<Chunk> chunkArrayCopySync = GameManager.Instance.World.ChunkCache.GetChunkArrayCopySync();
    bool bUseSelection = BlockToolSelection.Instance.SelectionActive;
    Vector3i selStart = BlockToolSelection.Instance.SelectionMin;
    Vector3i selEnd = selStart + BlockToolSelection.Instance.SelectionSize - Vector3i.one;
    for (int index = 0; index < chunkArrayCopySync.Count; ++index)
    {
      Chunk chunk = chunkArrayCopySync[index];
      chunk.LoopOverAllBlocks((ChunkBlockLayer.LoopBlocksDelegate) ([PublicizedFrom(EAccessModifier.Internal)] (x, y, z, bv) =>
      {
        if (bUseSelection)
        {
          Vector3i worldPos = chunk.ToWorldPos(new Vector3i(x, y, z));
          if (worldPos.x < selStart.x || worldPos.x > selEnd.x || worldPos.y < selStart.y || worldPos.y > selEnd.y || worldPos.z < selStart.z || worldPos.z > selEnd.z)
            return;
        }
        if (chunk.GetTextureFull(x, y, z, 0) == 0L)
          return;
        chunk.SetTextureFull(x, y, z, 0L);
        changedChunks.Add(chunk);
      }));
    }
    foreach (Chunk chunk in changedChunks)
      chunk.NeedsRegeneration = true;
    if (changedChunks.Count > 0)
      this.NeedsSaving = true;
    GameManager.Instance.World.m_ChunkManager.RemoveAllChunksOnAllClients();
  }

  public void StripInternalTextures()
  {
    HashSet<Chunk> changedChunks = new HashSet<Chunk>();
    World world = GameManager.Instance.World;
    List<Chunk> chunkArrayCopySync = world.ChunkCache.GetChunkArrayCopySync();
    bool bUseSelection = BlockToolSelection.Instance.SelectionActive;
    Vector3i selStart = BlockToolSelection.Instance.SelectionMin;
    Vector3i selEnd = selStart + BlockToolSelection.Instance.SelectionSize - Vector3i.one;
    for (int index1 = 0; index1 < chunkArrayCopySync.Count; ++index1)
    {
      Chunk chunk = chunkArrayCopySync[index1];
      chunk.LoopOverAllBlocks((ChunkBlockLayer.LoopBlocksDelegate) ([PublicizedFrom(EAccessModifier.Internal)] (x, y, z, bv) =>
      {
        if (bUseSelection)
        {
          Vector3i worldPos = chunk.ToWorldPos(new Vector3i(x, y, z));
          if (worldPos.x < selStart.x || worldPos.x > selEnd.x || worldPos.y < selStart.y || worldPos.y > selEnd.y || worldPos.z < selStart.z || worldPos.z > selEnd.z)
            return;
        }
        if (chunk.GetTextureFull(x, y, z, 0) == 0L || !(bv.Block.shape is BlockShapeNew shape3))
          return;
        for (int index2 = 0; index2 < 6; ++index2)
        {
          BlockFace blockFace = (BlockFace) index2;
          Vector3i vector3i = new Vector3i(BlockFaceFlags.OffsetForFace(blockFace));
          BlockValue block = world.GetBlock(chunk.ToWorldPos(new Vector3i(x, y, z)) + vector3i);
          if (!block.isair && block.Block.shape is BlockShapeNew shape4)
          {
            BlockFace _face = BlockFaceFlags.OppositeFace(blockFace);
            BlockShapeNew.EnumFaceOcclusionInfo faceInfo1 = shape3.GetFaceInfo(bv, blockFace);
            BlockShapeNew.EnumFaceOcclusionInfo faceInfo2 = shape4.GetFaceInfo(block, _face);
            if ((faceInfo1 == BlockShapeNew.EnumFaceOcclusionInfo.Full && faceInfo2 == BlockShapeNew.EnumFaceOcclusionInfo.Full || faceInfo1 == BlockShapeNew.EnumFaceOcclusionInfo.Part && faceInfo2 == BlockShapeNew.EnumFaceOcclusionInfo.Full ? 1 : (faceInfo1 != BlockShapeNew.EnumFaceOcclusionInfo.Part || faceInfo2 != BlockShapeNew.EnumFaceOcclusionInfo.Part || shape3 != shape4 ? 0 : ((int) bv.rotation == (int) block.rotation ? 1 : 0))) != 0)
            {
              BlockFace rotatedBlockFace = shape3.GetRotatedBlockFace(bv, blockFace);
              for (int _channel = 0; _channel < 1; ++_channel)
                world.ChunkCache.SetBlockFaceTexture(chunk.ToWorldPos(new Vector3i(x, y, z)), rotatedBlockFace, 0, _channel);
              changedChunks.Add(chunk);
            }
          }
        }
      }));
    }
    foreach (Chunk chunk in changedChunks)
      chunk.NeedsRegeneration = true;
    if (changedChunks.Count > 0)
      this.NeedsSaving = true;
    GameManager.Instance.World.m_ChunkManager.RemoveAllChunksOnAllClients();
  }

  public void GetLootAndFetchLootContainerCount(
    out int _loot,
    out int _fetchLoot,
    out int _restorePower)
  {
    int tempLoot = 0;
    int tempFetchLoot = 0;
    int tempRestorePower = 0;
    List<Chunk> chunkArrayCopySync = GameManager.Instance.World.ChunkCache.GetChunkArrayCopySync();
    for (int index = 0; index < chunkArrayCopySync.Count; ++index)
      chunkArrayCopySync[index].LoopOverAllBlocks((ChunkBlockLayer.LoopBlocksDelegate) ([PublicizedFrom(EAccessModifier.Internal)] (_1, _2, _3, bv) =>
      {
        Block block = bv.Block;
        if (block == null)
          return;
        bool flag = block.IndexName != null;
        if (flag && block.IndexName == Constants.cQuestLootFetchContainerIndexName)
          ++tempFetchLoot;
        else if (flag && block.IndexName == Constants.cQuestRestorePowerIndexName)
        {
          ++tempRestorePower;
        }
        else
        {
          switch (block)
          {
            case BlockLoot _:
              ++tempLoot;
              break;
            case BlockCompositeTileEntity compositeTileEntity2:
              if (!compositeTileEntity2.CompositeData.HasFeature<ITileEntityLootable>())
                break;
              ++tempLoot;
              break;
          }
        }
      }));
    _loot = tempLoot;
    _fetchLoot = tempFetchLoot;
    _restorePower = tempRestorePower;
  }

  public bool HighlightingBlocks
  {
    get => this.\u003CHighlightingBlocks\u003Ek__BackingField;
    [PublicizedFrom(EAccessModifier.Private)] set
    {
      this.\u003CHighlightingBlocks\u003Ek__BackingField = value;
    }
  }

  public void HighlightBlocks(Block _blockClass)
  {
    this.highlightBlockId = _blockClass != null ? _blockClass.blockID : 0;
    this.HighlightingBlocks = this.highlightBlockId > 0;
    this.highlightBlocks(this.highlightBlockId);
  }

  public void ToggleHighlightBlocks()
  {
    this.HighlightingBlocks = !this.HighlightingBlocks;
    this.highlightBlocks(this.HighlightingBlocks ? this.highlightBlockId : 0);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void highlightBlocks(int _blockId)
  {
    BlockHighlighter.Cleanup();
    if (_blockId <= 0)
      return;
    List<Chunk> chunkArrayCopySync = GameManager.Instance.World.ChunkCache.GetChunkArrayCopySync();
    for (int index = 0; index < chunkArrayCopySync.Count; ++index)
    {
      Chunk chunk = chunkArrayCopySync[index];
      chunk.LoopOverAllBlocks((ChunkBlockLayer.LoopBlocksDelegate) ([PublicizedFrom(EAccessModifier.Internal)] (_x, _y, _z, _bv) =>
      {
        if (_bv.type != _blockId)
          return;
        BlockHighlighter.AddBlock(chunk.worldPosIMin + new Vector3i(_x, _y, _z));
      }));
    }
  }

  public bool HighlightQuestLoot
  {
    get => this.bShowQuestLoot;
    set
    {
      this.bShowQuestLoot = value;
      NavObjectManager.Instance.UnRegisterNavObjectByClass("editor_quest_loot_container");
      if (!value)
        return;
      foreach (Chunk chunk in GameManager.Instance.World.ChunkCache.GetChunkArrayCopySync())
      {
        List<Vector3i> vector3iList;
        if (chunk.IndexedBlocks.TryGetValue(Constants.cQuestLootFetchContainerIndexName, out vector3iList))
        {
          Vector3i worldPos = chunk.GetWorldPos();
          foreach (Vector3i vector3i in vector3iList)
            NavObjectManager.Instance.RegisterNavObject("editor_quest_loot_container", (worldPos + vector3i).ToVector3Center());
        }
      }
    }
  }

  public bool HighlightBlockTriggers
  {
    get => this.bShowBlockTriggers;
    set
    {
      this.bShowBlockTriggers = value;
      NavObjectManager.Instance.UnRegisterNavObjectByClass("editor_block_trigger");
      if (!value)
        return;
      foreach (Chunk chunk in GameManager.Instance.World.ChunkCache.GetChunkArrayCopySync())
      {
        List<BlockTrigger> list = chunk.GetBlockTriggers().list;
        Vector3i worldPos = chunk.GetWorldPos();
        for (int index = 0; index < list.Count; ++index)
        {
          NavObject navObject = NavObjectManager.Instance.RegisterNavObject("editor_block_trigger", (worldPos + list[index].LocalChunkPos).ToVector3Center());
          navObject.name = list[index].TriggerDisplay();
          navObject.OverrideColor = list[index].TriggeredByIndices.Count > 0 ? Color.blue : Color.red;
        }
      }
    }
  }
}
