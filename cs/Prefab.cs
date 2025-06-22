// Decompiled with JetBrains decompiler
// Type: Prefab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using UniLinq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
public class Prefab : INeighborBlockCache, IChunkAccess
{
  [PublicizedFrom(EAccessModifier.Private)]
  public static int CurrentSaveVersion = 19;
  [PublicizedFrom(EAccessModifier.Private)]
  public const int cMinimumSupportedVersion = 13;
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_CopyAirBlocks = "CopyAirBlocks";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_AllowTopSoilDecorations = "AllowTopSoilDecorations";
  public const string cProp_YOffset = "YOffset";
  public const string cProp_RotationToFaceNorth = "RotationToFaceNorth";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_ExcludeDistantPOIMesh = "ExcludeDistantPOIMesh";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_ExcludePOICulling = "ExcludePOICulling";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_DistantPOIYOffset = "DistantPOIYOffset";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_DistantPOIOverride = "DistantPOIOverride";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_EditorGroups = "EditorGroups";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_IsTraderArea = "TraderArea";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_TraderAreaProtect = "TraderAreaProtect";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_SleeperVolumeStart = "SleeperVolumeStart";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_SleeperVolumeSize = "SleeperVolumeSize";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_SleeperVolumeGroup = "SleeperVolumeGroup";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_SleeperVolumeGroupId = "SleeperVolumeGroupId";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_SleeperIsPriorityVolume = "SleeperIsLootVolume";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_SleeperIsQuestExclude = "SleeperIsQuestExclude";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_SleeperVolumeFlags = "SleeperVolumeFlags";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_SleeperVolumeTriggeredBy = "SleeperVolumeTriggeredBy";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_SleeperVolumeScript = "SVS";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_TeleportVolumeStart = "TeleportVolumeStart";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_TeleportVolumeSize = "TeleportVolumeSize";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_InfoVolumeStart = "InfoVolumeStart";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_InfoVolumeSize = "InfoVolumeSize";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_WallVolumeStart = "WallVolumeStart";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_WallVolumeSize = "WallVolumeSize";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_TriggerVolumeStart = "TriggerVolumeStart";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_TriggerVolumeSize = "TriggerVolumeSize";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_TriggerVolumeTriggers = "TriggerVolumeTriggers";
  public const string cProp_POIMarkerStart = "POIMarkerStart";
  public const string cProp_POIMarkerSize = "POIMarkerSize";
  public const string cProp_POIMarkerGroup = "POIMarkerGroup";
  public const string cProp_POIMarkerTags = "POIMarkerTags";
  public const string cProp_POIMarkerType = "POIMarkerType";
  public const string cProp_POIMarkerPartToSpawn = "POIMarkerPartToSpawn";
  public const string cProp_POIMarkerPartRotations = "POIMarkerPartRotations";
  public const string cProp_POIMarkerPartSpawnChance = "POIMarkerPartSpawnChance";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_Zoning = "Zoning";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_AllowedBiomes = "AllowedBiomes";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_AllowedTownships = "AllowedTownships";
  public const string cProp_Tags = "Tags";
  public const string cProp_ThemeTags = "ThemeTags";
  public const string cProp_ThemeRepeatDist = "ThemeRepeatDistance";
  public const string cProp_DuplicateRepeatDist = "DuplicateRepeatDistance";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_StaticSpawnerClass = "StaticSpawner.Class";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_StaticSpawnerSize = "StaticSpawner.Size";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_StaticSpawnerTrigger = "StaticSpawner.Trigger";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_IndexedBlockOffsets = "IndexedBlockOffsets";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_QuestTags = "QuestTags";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cProp_ShowQuestClearCount = "ShowQuestClearCount";
  public const string cProp_DifficultyTier = "DifficultyTier";
  public const string cProp_PrefabSize = "PrefabSize";
  [PublicizedFrom(EAccessModifier.Private)]
  public static readonly string MISSING_BLOCK_NAME = "missingBlock";
  public Vector3i size;
  public PathAbstractions.AbstractedLocation location;
  public bool bCopyAirBlocks = true;
  public bool bExcludeDistantPOIMesh;
  public bool bExcludePOICulling;
  public float distantPOIYOffset;
  public string distantPOIOverride;
  public bool bAllowTopSoilDecorations;
  public bool bTraderArea;
  public Vector3i TraderAreaProtect;
  public List<Prefab.PrefabSleeperVolume> SleeperVolumes = new List<Prefab.PrefabSleeperVolume>();
  public List<Prefab.PrefabTeleportVolume> TeleportVolumes = new List<Prefab.PrefabTeleportVolume>();
  public List<Prefab.PrefabInfoVolume> InfoVolumes = new List<Prefab.PrefabInfoVolume>();
  public List<Prefab.PrefabWallVolume> WallVolumes = new List<Prefab.PrefabWallVolume>();
  public List<Prefab.PrefabTriggerVolume> TriggerVolumes = new List<Prefab.PrefabTriggerVolume>();
  public int yOffset;
  public int Transient_NumSleeperSpawns;
  public List<Prefab.Marker> POIMarkers = new List<Prefab.Marker>();
  public List<string> editorGroups = new List<string>();
  public int rotationToFaceNorth = 2;
  [PublicizedFrom(EAccessModifier.Private)]
  public List<string> allowedZones = new List<string>();
  [PublicizedFrom(EAccessModifier.Private)]
  public List<string> allowedBiomes = new List<string>();
  [PublicizedFrom(EAccessModifier.Private)]
  public List<string> allowedTownships = new List<string>();
  [PublicizedFrom(EAccessModifier.Private)]
  public FastTags<TagGroup.Poi> tags;
  [PublicizedFrom(EAccessModifier.Private)]
  public FastTags<TagGroup.Poi> themeTags;
  [PublicizedFrom(EAccessModifier.Private)]
  public int themeRepeatDistance = 300;
  [PublicizedFrom(EAccessModifier.Private)]
  public int duplicateRepeatDistance = 1000;
  [PublicizedFrom(EAccessModifier.Private)]
  public List<int> multiBlockParentIndices = new List<int>();
  [PublicizedFrom(EAccessModifier.Private)]
  public List<int> decoAllowedBlockIndices = new List<int>();
  public readonly Dictionary<string, List<Vector3i>> indexedBlockOffsets = (Dictionary<string, List<Vector3i>>) new CaseInsensitiveStringDictionary<List<Vector3i>>();
  [PublicizedFrom(EAccessModifier.Private)]
  public FastTags<TagGroup.Global> questTags = FastTags<TagGroup.Global>.none;
  [PublicizedFrom(EAccessModifier.Private)]
  public Prefab.BlockStatistics statistics;
  [PublicizedFrom(EAccessModifier.Private)]
  public WorldStats renderingCost;
  public string StaticSpawnerClass;
  public Vector3i StaticSpawnerSize;
  public int StaticSpawnerTrigger;
  public bool StaticSpawnerCreated;
  public int ShowQuestClearCount = 1;
  public byte DifficultyTier;
  [PublicizedFrom(EAccessModifier.Private)]
  public int localRotation;
  [PublicizedFrom(EAccessModifier.Private)]
  public readonly bool isCellsDataOwner = true;
  [PublicizedFrom(EAccessModifier.Private)]
  public Prefab.Cells<uint> blockCells;
  [PublicizedFrom(EAccessModifier.Private)]
  public Prefab.Cells<ushort> damageCells;
  [PublicizedFrom(EAccessModifier.Private)]
  public Prefab.Cells<sbyte> densityCells;
  [PublicizedFrom(EAccessModifier.Private)]
  public Prefab.Cells<TextureFullArray> textureCells;
  [PublicizedFrom(EAccessModifier.Private)]
  public Prefab.Cells<WaterValue> waterCells;
  [PublicizedFrom(EAccessModifier.Private)]
  public static Prefab.Data sharedData = new Prefab.Data();
  [PublicizedFrom(EAccessModifier.Private)]
  public List<EntityCreationData> entities = new List<EntityCreationData>();
  public DynamicProperties properties = new DynamicProperties();
  [PublicizedFrom(EAccessModifier.Private)]
  public int terrainFillerType;
  [PublicizedFrom(EAccessModifier.Private)]
  public int terrainFiller2Type;
  [PublicizedFrom(EAccessModifier.Private)]
  public int blockTypeMissingBlock = -1;
  [PublicizedFrom(EAccessModifier.Private)]
  public Dictionary<Vector3i, TileEntity> tileEntities = new Dictionary<Vector3i, TileEntity>();
  [PublicizedFrom(EAccessModifier.Private)]
  public PrefabInsideDataFile insidePos = new PrefabInsideDataFile();
  [PublicizedFrom(EAccessModifier.Private)]
  public Dictionary<Vector3i, BlockTrigger> triggerData = new Dictionary<Vector3i, BlockTrigger>();
  public List<byte> TriggerLayers = new List<byte>();
  [PublicizedFrom(EAccessModifier.Private)]
  public static byte[] tempBuf;
  [PublicizedFrom(EAccessModifier.Private)]
  public static SimpleBitStream simpleBitStreamReader = new SimpleBitStream();
  [PublicizedFrom(EAccessModifier.Private)]
  public int currX;
  [PublicizedFrom(EAccessModifier.Private)]
  public int currZ;
  [PublicizedFrom(EAccessModifier.Private)]
  public Dictionary<long, Prefab.PrefabChunk> dictChunks;

  public string PrefabName => this.location.FileNameNoExtension ?? "";

  public string LocalizedName => Localization.Get(this.PrefabName);

  public string LocalizedEnglishName => Localization.Get(this.PrefabName, "english");

  public float DensityScore
  {
    get => this.renderingCost != null ? (float) this.renderingCost.TotalVertices / 100000f : 0.0f;
  }

  public bool bSleeperVolumes => this.SleeperVolumes != null && this.SleeperVolumes.Count > 0;

  public bool bInfoVolumes => this.InfoVolumes != null && this.InfoVolumes.Count > 0;

  public bool bWallVolumes => this.WallVolumes != null && this.WallVolumes.Count > 0;

  public bool bTriggerVolumes => this.TriggerVolumes != null && this.TriggerVolumes.Count > 0;

  public bool bPOIMarkers => this.POIMarkers != null && this.POIMarkers.Count > 0;

  public WorldStats RenderingCostStats
  {
    get => this.renderingCost;
    set => this.renderingCost = value;
  }

  public FastTags<TagGroup.Poi> Tags
  {
    get => this.tags;
    set => this.tags = value;
  }

  public FastTags<TagGroup.Poi> ThemeTags
  {
    get => this.themeTags;
    set => this.themeTags = value;
  }

  public int ThemeRepeatDistance
  {
    get => this.themeRepeatDistance;
    set => this.themeRepeatDistance = value;
  }

  public int DuplicateRepeatDistance
  {
    get => this.duplicateRepeatDistance;
    set => this.duplicateRepeatDistance = value;
  }

  public static PathAbstractions.AbstractedLocation LocationForNewPrefab(
    string _name,
    string _prefabsSubfolder = null)
  {
    string launchArgument = GameUtils.GetLaunchArgument("newprefabsmod");
    if (!string.IsNullOrEmpty(launchArgument))
    {
      Mod mod = ModManager.GetMod(launchArgument, true);
      if (mod != null)
        return new PathAbstractions.AbstractedLocation(PathAbstractions.EAbstractedLocationType.Mods, _name, $"{mod.Path}/Prefabs{(_prefabsSubfolder != null ? "/" + _prefabsSubfolder : "")}", _prefabsSubfolder, _name, ".tts", false, mod);
      Log.Warning($"Argument -newprefabsmod given but mod with name '{launchArgument}' not found, ignoring!");
    }
    return new PathAbstractions.AbstractedLocation(PathAbstractions.EAbstractedLocationType.UserDataPath, _name, $"{GameIO.GetUserGameDataDir()}/LocalPrefabs{(_prefabsSubfolder != null ? "/" + _prefabsSubfolder : "")}", _prefabsSubfolder, _name, ".tts", false);
  }

  public static bool CanSaveIn(PathAbstractions.AbstractedLocation _location)
  {
    return _location.Type != PathAbstractions.EAbstractedLocationType.GameData;
  }

  public Prefab()
  {
  }

  public Prefab(Prefab _other, bool sharedData = false)
  {
    this.size = _other.size;
    if (sharedData)
    {
      this.isCellsDataOwner = false;
      this.blockCells = _other.blockCells;
      this.damageCells = _other.damageCells;
      this.densityCells = _other.densityCells;
      this.textureCells = _other.textureCells;
      this.waterCells = _other.waterCells;
    }
    else
    {
      this.blockCells = _other.blockCells.Clone();
      this.damageCells = _other.damageCells.Clone();
      this.densityCells = _other.densityCells.Clone();
      this.textureCells = _other.textureCells.Clone();
      this.waterCells = _other.waterCells.Clone();
    }
    if (sharedData)
    {
      this.multiBlockParentIndices = _other.multiBlockParentIndices;
      this.decoAllowedBlockIndices = _other.decoAllowedBlockIndices;
    }
    else
    {
      this.multiBlockParentIndices = new List<int>((IEnumerable<int>) _other.multiBlockParentIndices);
      this.decoAllowedBlockIndices = new List<int>((IEnumerable<int>) _other.decoAllowedBlockIndices);
    }
    this.location = _other.location;
    this.bCopyAirBlocks = _other.bCopyAirBlocks;
    this.bExcludeDistantPOIMesh = _other.bExcludeDistantPOIMesh;
    this.bExcludePOICulling = _other.bExcludePOICulling;
    this.distantPOIYOffset = _other.distantPOIYOffset;
    this.distantPOIOverride = _other.distantPOIOverride;
    this.bAllowTopSoilDecorations = _other.bAllowTopSoilDecorations;
    this.bTraderArea = _other.bTraderArea;
    this.TraderAreaProtect = _other.TraderAreaProtect;
    this.SleeperVolumes = _other.bSleeperVolumes ? _other.SleeperVolumes.ConvertAll<Prefab.PrefabSleeperVolume>((Converter<Prefab.PrefabSleeperVolume, Prefab.PrefabSleeperVolume>) ([PublicizedFrom(EAccessModifier.Internal)] (_input) => new Prefab.PrefabSleeperVolume(_input))) : new List<Prefab.PrefabSleeperVolume>();
    this.TeleportVolumes = _other.bTraderArea ? _other.TeleportVolumes.ConvertAll<Prefab.PrefabTeleportVolume>((Converter<Prefab.PrefabTeleportVolume, Prefab.PrefabTeleportVolume>) ([PublicizedFrom(EAccessModifier.Internal)] (_input) => new Prefab.PrefabTeleportVolume(_input))) : new List<Prefab.PrefabTeleportVolume>();
    this.InfoVolumes = _other.bInfoVolumes ? _other.InfoVolumes.ConvertAll<Prefab.PrefabInfoVolume>((Converter<Prefab.PrefabInfoVolume, Prefab.PrefabInfoVolume>) ([PublicizedFrom(EAccessModifier.Internal)] (_input) => new Prefab.PrefabInfoVolume(_input))) : new List<Prefab.PrefabInfoVolume>();
    this.WallVolumes = _other.bWallVolumes ? _other.WallVolumes.ConvertAll<Prefab.PrefabWallVolume>((Converter<Prefab.PrefabWallVolume, Prefab.PrefabWallVolume>) ([PublicizedFrom(EAccessModifier.Internal)] (_input) => new Prefab.PrefabWallVolume(_input))) : new List<Prefab.PrefabWallVolume>();
    this.TriggerVolumes = _other.bTriggerVolumes ? _other.TriggerVolumes.ConvertAll<Prefab.PrefabTriggerVolume>((Converter<Prefab.PrefabTriggerVolume, Prefab.PrefabTriggerVolume>) ([PublicizedFrom(EAccessModifier.Internal)] (_input) => new Prefab.PrefabTriggerVolume(_input))) : new List<Prefab.PrefabTriggerVolume>();
    this.yOffset = _other.yOffset;
    this.rotationToFaceNorth = _other.rotationToFaceNorth;
    this.allowedBiomes = new List<string>((IEnumerable<string>) _other.allowedBiomes);
    this.allowedTownships = new List<string>((IEnumerable<string>) _other.allowedTownships);
    this.allowedZones = new List<string>((IEnumerable<string>) _other.allowedZones);
    this.tags = new FastTags<TagGroup.Poi>(_other.tags);
    this.themeTags = new FastTags<TagGroup.Poi>(_other.themeTags);
    this.themeRepeatDistance = _other.themeRepeatDistance;
    this.duplicateRepeatDistance = _other.duplicateRepeatDistance;
    this.StaticSpawnerClass = _other.StaticSpawnerClass;
    this.StaticSpawnerSize = _other.StaticSpawnerSize;
    this.StaticSpawnerTrigger = _other.StaticSpawnerTrigger;
    this.questTags = _other.questTags;
    this.DifficultyTier = _other.DifficultyTier;
    this.ShowQuestClearCount = _other.ShowQuestClearCount;
    this.localRotation = _other.localRotation;
    for (int index = 0; index < _other.entities.Count; ++index)
      this.entities.Add(_other.entities[index].Clone());
    foreach (KeyValuePair<Vector3i, TileEntity> tileEntity in _other.tileEntities)
      this.tileEntities.Add(tileEntity.Key, tileEntity.Value);
    this.POIMarkers = new List<Prefab.Marker>();
    for (int index = 0; index < _other.POIMarkers.Count; ++index)
      this.POIMarkers.Add(new Prefab.Marker(_other.POIMarkers[index]));
    this.insidePos = _other.insidePos.Clone();
    foreach (KeyValuePair<Vector3i, BlockTrigger> keyValuePair in _other.triggerData)
      this.triggerData.Add(keyValuePair.Key, keyValuePair.Value);
    for (int index = 0; index < _other.TriggerLayers.Count; ++index)
      this.TriggerLayers.Add(_other.TriggerLayers[index]);
    this.renderingCost = _other.renderingCost;
  }

  public Prefab(Vector3i _size)
  {
    this.size = _size;
    this.localRotation = 0;
    this.InitData();
  }

  public int EstimateOwnedBytes()
  {
    int num1 = 0;
    if (this.isCellsDataOwner)
    {
      int num2 = num1 + IntPtr.Size;
      int _arrayCount;
      int _arraySize;
      int _cellsCount;
      int _cellsSize;
      int _usedCount;
      if (this.blockCells != null)
      {
        this.blockCells.Stats(out _arrayCount, out _arraySize, out _cellsCount, out _cellsSize, out _usedCount);
        num2 += _cellsSize + _arrayCount * IntPtr.Size;
      }
      int num3 = num2 + IntPtr.Size;
      if (this.damageCells != null)
      {
        this.damageCells.Stats(out _arrayCount, out _arraySize, out _cellsCount, out _cellsSize, out _usedCount);
        num3 += _cellsSize + _arrayCount * IntPtr.Size;
      }
      int num4 = num3 + IntPtr.Size;
      if (this.densityCells != null)
      {
        this.densityCells.Stats(out _arrayCount, out _arraySize, out _cellsCount, out _cellsSize, out _usedCount);
        num4 += _cellsSize + _arrayCount * IntPtr.Size;
      }
      int num5 = num4 + IntPtr.Size;
      if (this.textureCells != null)
      {
        this.textureCells.Stats(out _arrayCount, out _arraySize, out _cellsCount, out _cellsSize, out _usedCount);
        num5 += _cellsSize + _arrayCount * IntPtr.Size;
      }
      num1 = num5 + IntPtr.Size;
      if (this.waterCells != null)
      {
        this.waterCells.Stats(out _arrayCount, out _arraySize, out _cellsCount, out _cellsSize, out _usedCount);
        num1 += _cellsSize + _arrayCount * IntPtr.Size;
      }
    }
    return num1 + MemoryTracker.GetSize<byte>(this.TriggerLayers);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void InitData()
  {
    if (!this.isCellsDataOwner)
    {
      Log.Error("InitData failed: Cannot set block data on non-owning Prefab instance.");
    }
    else
    {
      this.blockCells = new Prefab.Cells<uint>(this.size.y, 0U);
      this.damageCells = new Prefab.Cells<ushort>(this.size.y, (ushort) 0);
      this.densityCells = new Prefab.Cells<sbyte>(this.size.y, MarchingCubes.DensityAir);
      this.textureCells = new Prefab.Cells<TextureFullArray>(this.size.y, TextureFullArray.Default);
      this.waterCells = new Prefab.Cells<WaterValue>(this.size.y, WaterValue.Empty);
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void InitTerrainFillers()
  {
    this.terrainFillerType = Block.GetBlockValue(Constants.cTerrainFillerBlockName).type;
    this.terrainFiller2Type = Block.GetBlockValue(Constants.cTerrainFiller2BlockName).type;
  }

  public Prefab Clone(bool sharedData = false) => new Prefab(this, sharedData);

  public int GetLocalRotation() => this.localRotation;

  public void SetLocalRotation(int _rot) => this.localRotation = _rot;

  [PublicizedFrom(EAccessModifier.Private)]
  public int CoordToOffset(int _localRotation, int _x, int _y, int _z)
  {
    int offset;
    switch (_localRotation)
    {
      case 1:
        offset = _z + _y * this.size.z + (this.size.x - _x - 1) * this.size.z * this.size.y;
        break;
      case 2:
        offset = this.size.x - _x - 1 + _y * this.size.x + (this.size.z - _z - 1) * this.size.x * this.size.y;
        break;
      case 3:
        offset = this.size.z - _z - 1 + _y * this.size.z + _x * this.size.z * this.size.y;
        break;
      default:
        offset = _x + _y * this.size.x + _z * this.size.x * this.size.y;
        break;
    }
    return offset;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void offsetToCoord(int _offset, out int _x, out int _y, out int _z)
  {
    int num = this.size.x * this.size.y;
    _z = _offset / num;
    _offset %= num;
    _y = _offset / this.size.x;
    _x = _offset % this.size.x;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void offsetToCoordRotated(int _offset, out int _x, out int _y, out int _z)
  {
    switch (this.localRotation)
    {
      case 1:
        _x = -(_offset / (this.size.z * this.size.y) - this.size.x + 1);
        _offset %= this.size.z * this.size.y;
        _y = _offset / this.size.z;
        _z = _offset % this.size.z;
        break;
      case 2:
        _z = -(_offset / (this.size.x * this.size.y) - this.size.z + 1);
        _offset %= this.size.x * this.size.y;
        _y = _offset / this.size.x;
        _offset %= this.size.x;
        _x = -(_offset - this.size.x + 1);
        break;
      case 3:
        _x = _offset / (this.size.z * this.size.y);
        _offset %= this.size.z * this.size.y;
        _y = _offset / this.size.z;
        _offset %= this.size.z;
        _z = -(_offset - this.size.z + 1);
        break;
      default:
        int num = this.size.x * this.size.y;
        _z = _offset / num;
        _offset %= num;
        _y = _offset / this.size.x;
        _x = _offset % this.size.x;
        break;
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void RotateCoords(ref int _x, ref int _z)
  {
    switch (this.localRotation)
    {
      case 1:
        int num1 = _x;
        _x = _z;
        _z = this.size.x - num1 - 1;
        break;
      case 2:
        _x = this.size.x - _x - 1;
        _z = this.size.z - _z - 1;
        break;
      case 3:
        int num2 = _x;
        _x = this.size.z - _z - 1;
        _z = num2;
        break;
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void RotateCoords(int _rot, ref int _x, ref int _z)
  {
    switch (_rot)
    {
      case 1:
        int num1 = _x;
        _x = _z;
        _z = this.size.x - num1 - 1;
        break;
      case 2:
        _x = this.size.x - _x - 1;
        _z = this.size.z - _z - 1;
        break;
      case 3:
        int num2 = _x;
        _x = this.size.z - _z - 1;
        _z = num2;
        break;
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void InverseRotateRelative(ref int _x, ref int _z)
  {
    switch (this.localRotation)
    {
      case 1:
        int num1 = _x;
        _x = -_z;
        _z = num1;
        break;
      case 2:
        _x = -_x;
        _z = -_z;
        break;
      case 3:
        int num2 = _x;
        _x = _z;
        _z = -num2;
        break;
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void RotateRelative(ref int _x, ref int _z)
  {
    switch (this.localRotation)
    {
      case 1:
        int num1 = _x;
        _x = _z;
        _z = -num1;
        break;
      case 2:
        _x = -_x;
        _z = -_z;
        break;
      case 3:
        int num2 = _x;
        _x = -_z;
        _z = num2;
        break;
    }
  }

  public void SetBlock(int _x, int _y, int _z, BlockValue _bv)
  {
    if (_bv.isWater)
    {
      Log.Warning("Prefabs should no longer store water blocks. Please use SetWater instead");
      this.SetWater(_x, _y, _z, WaterValue.Full);
    }
    else if (!this.isCellsDataOwner)
    {
      Log.Error("SetBlock failed: Cannot set block data on non-owning Prefab instance.");
    }
    else
    {
      if ((long) (uint) _x >= (long) this.size.x || (long) (uint) _y >= (long) this.size.y || (long) (uint) _z >= (long) this.size.z)
        return;
      this.RotateCoords(ref _x, ref _z);
      this.blockCells.SetData(_x, _y, _z, _bv.rawData);
      this.damageCells.SetData(_x, _y, _z, (ushort) _bv.damage);
    }
  }

  public float GetHeight(int _x, int _z, bool _terrainOnly = true)
  {
    for (int y = this.size.y; y >= 0; --y)
    {
      BlockValue block = this.GetBlock(_x, y, _z);
      if (!block.isair && !(!block.Block.shape.IsTerrain() & _terrainOnly))
      {
        float num = (float) (1.0 - (double) (byte) block.Block.Density / (double) byte.MaxValue);
        return (float) (y - 1) + num;
      }
    }
    return 0.0f;
  }

  public BlockValue GetBlock(int _x, int _y, int _z)
  {
    if ((long) (uint) _x >= (long) this.size.x || (long) (uint) _y >= (long) this.size.y || (long) (uint) _z >= (long) this.size.z)
      return BlockValue.Air;
    BlockValue air = BlockValue.Air;
    this.RotateCoords(ref _x, ref _z);
    Prefab.Cells<uint>.Cell cell1 = this.blockCells.GetCell(_x, _y, _z);
    if (cell1.a != null)
    {
      air.rawData = cell1.Get(_x, _z);
      Prefab.Cells<ushort>.Cell cell2 = this.damageCells.GetCell(_x, _y, _z);
      if (cell2.a != null)
        air.damage = (int) cell2.Get(_x, _z);
      if (!this.isCellsDataOwner && this.localRotation != 0)
        this.ApplyRotation(ref air);
    }
    return air;
  }

  public BlockValue GetBlockNoDamage(int _localRotation, int _x, int _y, int _z)
  {
    BlockValue air = BlockValue.Air;
    this.RotateCoords(_localRotation, ref _x, ref _z);
    Prefab.Cells<uint>.Cell cell = this.blockCells.GetCell(_x, _y, _z);
    if (cell.a != null)
    {
      air.rawData = cell.Get(_x, _z);
      if (!this.isCellsDataOwner && this.localRotation != 0)
        this.ApplyRotation(ref air);
    }
    return air;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void ApplyRotation(ref BlockValue bv)
  {
    if (bv.ischild)
    {
      int parentx = bv.parentx;
      int parentz = bv.parentz;
      if (parentx == 0 && parentz == 0)
        return;
      this.InverseRotateRelative(ref parentx, ref parentz);
      bv.parentx = parentx;
      bv.parentz = parentz;
    }
    else
      bv = bv.Block.shape.RotateY(true, bv, this.localRotation);
  }

  public BlockValue GetBlockNoDamage(int _offs) => BlockValue.Air;

  public int GetBlockCount() => this.size.x * this.size.y * this.size.z;

  public WaterValue GetWater(int _x, int _y, int _z)
  {
    this.RotateCoords(ref _x, ref _z);
    return this.waterCells.GetData(_x, _y, _z);
  }

  public void SetWater(int _x, int _y, int _z, WaterValue _wv)
  {
    if ((long) (uint) _x >= (long) this.size.x || (long) (uint) _y >= (long) this.size.y || (long) (uint) _z >= (long) this.size.z)
      return;
    this.RotateCoords(ref _x, ref _z);
    this.waterCells.SetData(_x, _y, _z, _wv);
  }

  public byte GetStab(int relx, int absy, int relz) => 0;

  public void SetDensity(int _x, int _y, int _z, sbyte _density)
  {
    this.RotateCoords(ref _x, ref _z);
    this.densityCells.SetData(_x, _y, _z, _density);
  }

  public sbyte GetDensity(int _x, int _y, int _z)
  {
    this.RotateCoords(ref _x, ref _z);
    return this.densityCells.GetData(_x, _y, _z);
  }

  public sbyte GetDensity(int _localRotation, int _x, int _y, int _z)
  {
    this.RotateCoords(_localRotation, ref _x, ref _z);
    return this.densityCells.GetData(_x, _y, _z);
  }

  public void SetTexture(int _x, int _y, int _z, TextureFullArray _fulltexture)
  {
    this.RotateCoords(ref _x, ref _z);
    this.textureCells.SetData(_x, _y, _z, _fulltexture);
  }

  public TextureFullArray GetTexture(int _x, int _y, int _z)
  {
    this.RotateCoords(ref _x, ref _z);
    return this.textureCells.GetData(_x, _y, _z);
  }

  public bool IsInsidePrefab(int _x, int _y, int _z)
  {
    int x;
    int y;
    int z;
    switch (this.localRotation)
    {
      case 1:
        x = _z;
        y = _y;
        z = this.size.x - _x - 1;
        break;
      case 2:
        x = this.size.x - _x - 1;
        y = _y;
        z = this.size.z - _z - 1;
        break;
      case 3:
        x = this.size.z - _z - 1;
        y = _y;
        z = _x;
        break;
      default:
        x = _x;
        y = _y;
        z = _z;
        break;
    }
    return this.insidePos.Contains(x, y, z);
  }

  public void ToggleQuestTag(FastTags<TagGroup.Global> questTag)
  {
    if (this.GetQuestTag(questTag))
      this.questTags = this.questTags.Remove(questTag);
    else
      this.questTags |= questTag;
  }

  public FastTags<TagGroup.Global> GetQuestTags() => new FastTags<TagGroup.Global>(this.questTags);

  public bool GetQuestTag(FastTags<TagGroup.Global> questTag)
  {
    return this.questTags.Test_AllSet(questTag);
  }

  public bool HasAnyQuestTag(FastTags<TagGroup.Global> questTag)
  {
    return this.questTags.Test_AnySet(questTag);
  }

  public bool HasQuestTag() => !this.questTags.IsEmpty;

  public TileEntity GetTileEntity(Vector3i _blockPos)
  {
    switch (this.localRotation)
    {
      case 1:
        int x1 = _blockPos.x;
        _blockPos.x = _blockPos.z;
        _blockPos.z = this.size.x - x1 - 1;
        break;
      case 2:
        _blockPos.x = this.size.x - _blockPos.x - 1;
        _blockPos.z = this.size.z - _blockPos.z - 1;
        break;
      case 3:
        int x2 = _blockPos.x;
        _blockPos.x = this.size.z - _blockPos.z - 1;
        _blockPos.z = x2;
        break;
    }
    TileEntity tileEntity;
    return this.tileEntities.TryGetValue(_blockPos, out tileEntity) ? tileEntity : (TileEntity) null;
  }

  public BlockTrigger GetBlockTrigger(Vector3i _blockPos)
  {
    switch (this.localRotation)
    {
      case 1:
        int x1 = _blockPos.x;
        _blockPos.x = _blockPos.z;
        _blockPos.z = this.size.x - x1 - 1;
        break;
      case 2:
        _blockPos.x = this.size.x - _blockPos.x - 1;
        _blockPos.z = this.size.z - _blockPos.z - 1;
        break;
      case 3:
        int x2 = _blockPos.x;
        _blockPos.x = this.size.z - _blockPos.z - 1;
        _blockPos.z = x2;
        break;
    }
    BlockTrigger blockTrigger;
    return this.triggerData.TryGetValue(_blockPos, out blockTrigger) ? blockTrigger : (BlockTrigger) null;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void ReadFromProperties()
  {
    this.bCopyAirBlocks = this.properties.GetBool("CopyAirBlocks");
    this.bExcludeDistantPOIMesh = this.properties.GetBool("ExcludeDistantPOIMesh");
    this.bExcludePOICulling = this.properties.GetBool("ExcludePOICulling");
    this.distantPOIYOffset = this.properties.GetFloat("DistantPOIYOffset");
    this.properties.ParseString("DistantPOIOverride", ref this.distantPOIOverride);
    this.bAllowTopSoilDecorations = this.properties.GetBool("AllowTopSoilDecorations");
    this.editorGroups.Clear();
    if (this.properties.Values.ContainsKey("EditorGroups"))
    {
      this.editorGroups.AddRange((IEnumerable<string>) this.properties.GetStringValue("EditorGroups").Split(',', StringSplitOptions.None));
      for (int index = 0; index < this.editorGroups.Count; ++index)
        this.editorGroups[index] = this.editorGroups[index].Trim();
    }
    if (this.properties.Values.ContainsKey("DifficultyTier"))
      this.DifficultyTier = (byte) this.properties.GetInt("DifficultyTier");
    this.properties.ParseInt("ShowQuestClearCount", ref this.ShowQuestClearCount);
    this.bTraderArea = this.properties.GetBool("TraderArea");
    this.TraderAreaProtect = this.properties.Values.ContainsKey("TraderAreaProtect") ? StringParsers.ParseVector3i(this.properties.Values["TraderAreaProtect"]) : Vector3i.zero;
    this.SleeperVolumes = new List<Prefab.PrefabSleeperVolume>();
    DictionarySave<string, string> values = this.properties.Values;
    if (values.ContainsKey("SleeperVolumeSize") && values.ContainsKey("SleeperVolumeStart"))
    {
      List<Vector3i> list1 = StringParsers.ParseList<Vector3i>(values["SleeperVolumeSize"], '#', (Func<string, int, int, Vector3i>) ([PublicizedFrom(EAccessModifier.Internal)] (_s, _start, _end) => StringParsers.ParseVector3i(_s, _start, _end)));
      List<Vector3i> list2 = StringParsers.ParseList<Vector3i>(values["SleeperVolumeStart"], '#', (Func<string, int, int, Vector3i>) ([PublicizedFrom(EAccessModifier.Internal)] (_s, _start, _end) => StringParsers.ParseVector3i(_s, _start, _end)));
      List<string> stringList1 = (List<string>) null;
      string _input;
      if (values.TryGetValue("SleeperVolumeGroupId", out _input))
        stringList1 = new List<string>((IEnumerable<string>) _input.Split(',', StringSplitOptions.None));
      List<string> stringList2 = (List<string>) null;
      if (values.TryGetValue("SleeperVolumeGroup", out _input))
        stringList2 = new List<string>((IEnumerable<string>) _input.Split(',', StringSplitOptions.None));
      List<bool> boolList1 = values.ContainsKey("SleeperIsLootVolume") ? StringParsers.ParseList<bool>(values["SleeperIsLootVolume"], ',', (Func<string, int, int, bool>) ([PublicizedFrom(EAccessModifier.Internal)] (_s, _start, _end) => StringParsers.ParseBool(_s, _start, _end))) : new List<bool>();
      List<bool> boolList2 = values.ContainsKey("SleeperIsQuestExclude") ? StringParsers.ParseList<bool>(values["SleeperIsQuestExclude"], ',', (Func<string, int, int, bool>) ([PublicizedFrom(EAccessModifier.Internal)] (_s, _start, _end) => StringParsers.ParseBool(_s, _start, _end))) : new List<bool>();
      List<int> intList = (List<int>) null;
      if (values.TryGetValue("SleeperVolumeFlags", out _input))
        intList = StringParsers.ParseList<int>(_input, ',', (Func<string, int, int, int>) ([PublicizedFrom(EAccessModifier.Internal)] (_s, _start, _end) => StringParsers.ParseSInt32(_s, _start, _end, NumberStyles.HexNumber)));
      List<string> stringList3 = (List<string>) null;
      if (values.TryGetValue("SleeperVolumeTriggeredBy", out _input))
        stringList3 = StringParsers.ParseList<string>(_input, '#', (Func<string, int, int, string>) ([PublicizedFrom(EAccessModifier.Internal)] (_s, _start, _end) => _s.Substring(_start, _end == -1 ? _s.Length - _start : _end + 1 - _start)));
      for (int index1 = 0; index1 < list2.Count; ++index1)
      {
        Vector3i _startPos = list2[index1];
        Vector3i _size = index1 < list1.Count ? list1[index1] : Vector3i.one;
        short _groupId = 0;
        string str1 = "???";
        short _spawnMin = 5;
        short _spawnMax = 5;
        if (stringList1 != null)
          _groupId = StringParsers.ParseSInt16(stringList1[index1]);
        if (stringList2 != null)
        {
          if (stringList2.Count == list2.Count)
            str1 = stringList2[index1];
          else if (stringList2.Count == list2.Count * 3)
          {
            int index2 = index1 * 3;
            str1 = stringList2[index2];
            _spawnMin = StringParsers.ParseSInt16(stringList2[index2 + 1]);
            _spawnMax = StringParsers.ParseSInt16(stringList2[index2 + 2]);
          }
          str1 = GameStageGroup.CleanName(str1);
        }
        bool _isPriority = index1 < boolList1.Count && boolList1[index1];
        bool _isQuestExclude = index1 < boolList2.Count && boolList2[index1];
        int _flags = 0;
        if (intList != null && index1 < intList.Count)
          _flags = intList[index1];
        Prefab.PrefabSleeperVolume prefabSleeperVolume = new Prefab.PrefabSleeperVolume();
        prefabSleeperVolume.Use(_startPos, _size, _groupId, str1, _isPriority, _isQuestExclude, (int) _spawnMin, (int) _spawnMax, _flags);
        string str2 = this.properties.GetString("SVS" + index1.ToString());
        if (str2.Length > 0)
          prefabSleeperVolume.minScript = str2;
        if (stringList3 != null && stringList3[index1].Trim() != "")
          prefabSleeperVolume.triggeredByIndices = StringParsers.ParseList<byte>(stringList3[index1], ',', (Func<string, int, int, byte>) ([PublicizedFrom(EAccessModifier.Internal)] (_s, _start, _end) => StringParsers.ParseUInt8(_s, _start, _end)));
        this.SleeperVolumes.Add(prefabSleeperVolume);
      }
    }
    this.TeleportVolumes = new List<Prefab.PrefabTeleportVolume>();
    if (values.ContainsKey("TeleportVolumeSize") && values.ContainsKey("TeleportVolumeStart"))
    {
      List<Vector3i> list3 = StringParsers.ParseList<Vector3i>(values["TeleportVolumeSize"], '#', (Func<string, int, int, Vector3i>) ([PublicizedFrom(EAccessModifier.Internal)] (_s, _start, _end) => StringParsers.ParseVector3i(_s, _start, _end)));
      List<Vector3i> list4 = StringParsers.ParseList<Vector3i>(values["TeleportVolumeStart"], '#', (Func<string, int, int, Vector3i>) ([PublicizedFrom(EAccessModifier.Internal)] (_s, _start, _end) => StringParsers.ParseVector3i(_s, _start, _end)));
      for (int index = 0; index < list4.Count; ++index)
      {
        Vector3i _startPos = list4[index];
        Vector3i _size = index < list3.Count ? list3[index] : Vector3i.one;
        Prefab.PrefabTeleportVolume prefabTeleportVolume = new Prefab.PrefabTeleportVolume();
        prefabTeleportVolume.Use(_startPos, _size);
        this.TeleportVolumes.Add(prefabTeleportVolume);
      }
    }
    this.InfoVolumes = new List<Prefab.PrefabInfoVolume>();
    if (values.ContainsKey("InfoVolumeSize") && values.ContainsKey("InfoVolumeStart"))
    {
      List<Vector3i> list5 = StringParsers.ParseList<Vector3i>(values["InfoVolumeSize"], '#', (Func<string, int, int, Vector3i>) ([PublicizedFrom(EAccessModifier.Internal)] (_s, _start, _end) => StringParsers.ParseVector3i(_s, _start, _end)));
      List<Vector3i> list6 = StringParsers.ParseList<Vector3i>(values["InfoVolumeStart"], '#', (Func<string, int, int, Vector3i>) ([PublicizedFrom(EAccessModifier.Internal)] (_s, _start, _end) => StringParsers.ParseVector3i(_s, _start, _end)));
      for (int index = 0; index < list6.Count; ++index)
      {
        Vector3i _startPos = list6[index];
        Vector3i _size = index < list5.Count ? list5[index] : Vector3i.one;
        Prefab.PrefabInfoVolume prefabInfoVolume = new Prefab.PrefabInfoVolume();
        prefabInfoVolume.Use(_startPos, _size);
        this.InfoVolumes.Add(prefabInfoVolume);
      }
    }
    this.WallVolumes = new List<Prefab.PrefabWallVolume>();
    if (values.ContainsKey("WallVolumeSize") && values.ContainsKey("WallVolumeStart"))
    {
      List<Vector3i> list7 = StringParsers.ParseList<Vector3i>(values["WallVolumeSize"], '#', (Func<string, int, int, Vector3i>) ([PublicizedFrom(EAccessModifier.Internal)] (_s, _start, _end) => StringParsers.ParseVector3i(_s, _start, _end)));
      List<Vector3i> list8 = StringParsers.ParseList<Vector3i>(values["WallVolumeStart"], '#', (Func<string, int, int, Vector3i>) ([PublicizedFrom(EAccessModifier.Internal)] (_s, _start, _end) => StringParsers.ParseVector3i(_s, _start, _end)));
      for (int index = 0; index < list8.Count; ++index)
      {
        Vector3i _startPos = list8[index];
        Vector3i _size = index < list7.Count ? list7[index] : Vector3i.one;
        Prefab.PrefabWallVolume prefabWallVolume = new Prefab.PrefabWallVolume();
        prefabWallVolume.Use(_startPos, _size);
        this.WallVolumes.Add(prefabWallVolume);
      }
    }
    this.TriggerVolumes = new List<Prefab.PrefabTriggerVolume>();
    if (values.ContainsKey("TriggerVolumeSize") && values.ContainsKey("TriggerVolumeStart"))
    {
      List<Vector3i> list9 = StringParsers.ParseList<Vector3i>(values["TriggerVolumeSize"], '#', (Func<string, int, int, Vector3i>) ([PublicizedFrom(EAccessModifier.Internal)] (_s, _start, _end) => StringParsers.ParseVector3i(_s, _start, _end)));
      List<Vector3i> list10 = StringParsers.ParseList<Vector3i>(values["TriggerVolumeStart"], '#', (Func<string, int, int, Vector3i>) ([PublicizedFrom(EAccessModifier.Internal)] (_s, _start, _end) => StringParsers.ParseVector3i(_s, _start, _end)));
      List<string> list11 = StringParsers.ParseList<string>(values["TriggerVolumeTriggers"], '#', (Func<string, int, int, string>) ([PublicizedFrom(EAccessModifier.Internal)] (_s, _start, _end) => _s.Substring(_start, _end == -1 ? _s.Length - _start : _end + 1 - _start)));
      for (int index = 0; index < list10.Count; ++index)
      {
        Vector3i _startPos = list10[index];
        Vector3i _size = index < list9.Count ? list9[index] : Vector3i.one;
        Prefab.PrefabTriggerVolume trigger = new Prefab.PrefabTriggerVolume();
        trigger.Use(_startPos, _size);
        if (list11[index].Trim() != "")
          trigger.TriggersIndices = StringParsers.ParseList<byte>(list11[index], ',', (Func<string, int, int, byte>) ([PublicizedFrom(EAccessModifier.Internal)] (_s, _start, _end) => StringParsers.ParseUInt8(_s, _start, _end)));
        this.TriggerVolumes.Add(trigger);
        this.HandleAddingTriggerLayers(trigger);
      }
    }
    if (values.ContainsKey("POIMarkerSize") && values.ContainsKey("POIMarkerStart"))
    {
      this.POIMarkers.Clear();
      List<Vector3i> list12 = StringParsers.ParseList<Vector3i>(values["POIMarkerSize"], '#', (Func<string, int, int, Vector3i>) ([PublicizedFrom(EAccessModifier.Internal)] (_s, _start, _end) => StringParsers.ParseVector3i(_s, _start, _end)));
      List<Vector3i> list13 = StringParsers.ParseList<Vector3i>(values["POIMarkerStart"], '#', (Func<string, int, int, Vector3i>) ([PublicizedFrom(EAccessModifier.Internal)] (_s, _start, _end) => StringParsers.ParseVector3i(_s, _start, _end)));
      List<Prefab.Marker.MarkerTypes> markerTypesList = new List<Prefab.Marker.MarkerTypes>();
      if (values.ContainsKey("POIMarkerType"))
      {
        foreach (string str in values["POIMarkerType"].Split(',', StringSplitOptions.None))
        {
          Prefab.Marker.MarkerTypes result;
          if (Enum.TryParse<Prefab.Marker.MarkerTypes>(str, true, out result))
            markerTypesList.Add(result);
        }
      }
      List<FastTags<TagGroup.Poi>> fastTagsList = new List<FastTags<TagGroup.Poi>>();
      if (values.ContainsKey("POIMarkerTags"))
      {
        string[] strArray = values["POIMarkerTags"].Split('#', StringSplitOptions.None);
        for (int index = 0; index < strArray.Length; ++index)
        {
          if (strArray[index].Length > 0)
            fastTagsList.Add(FastTags<TagGroup.Poi>.Parse(strArray[index]));
          else
            fastTagsList.Add(FastTags<TagGroup.Poi>.none);
        }
      }
      List<string> stringList4 = new List<string>();
      if (values.ContainsKey("POIMarkerGroup"))
        stringList4.AddRange((IEnumerable<string>) values["POIMarkerGroup"].Split(',', StringSplitOptions.None));
      List<string> stringList5 = new List<string>();
      if (values.ContainsKey("POIMarkerPartToSpawn"))
        stringList5.AddRange((IEnumerable<string>) values["POIMarkerPartToSpawn"].Split(',', StringSplitOptions.None));
      List<int> intList = new List<int>();
      if (values.ContainsKey("POIMarkerPartRotations"))
      {
        foreach (string _input in values["POIMarkerPartRotations"].Split(',', StringSplitOptions.None))
        {
          int _result;
          if (StringParsers.TryParseSInt32(_input, out _result))
            intList.Add(_result);
          else
            intList.Add(0);
        }
      }
      List<float> floatList = new List<float>();
      if (values.ContainsKey("POIMarkerPartSpawnChance"))
      {
        foreach (string _input in values["POIMarkerPartSpawnChance"].Split(',', StringSplitOptions.None))
        {
          float _result;
          if (StringParsers.TryParseFloat(_input, out _result))
            floatList.Add(_result);
          else
            floatList.Add(0.0f);
        }
      }
      for (int index = 0; index < list13.Count; ++index)
      {
        Prefab.Marker marker = new Prefab.Marker();
        marker.Start = list13[index];
        if (index < list12.Count)
          marker.Size = list12[index];
        if (index < markerTypesList.Count)
          marker.MarkerType = markerTypesList[index];
        if (index < stringList4.Count)
          marker.GroupName = stringList4[index];
        if (index < fastTagsList.Count)
          marker.Tags = fastTagsList[index];
        if (index < stringList5.Count)
          marker.PartToSpawn = stringList5[index];
        if (index < intList.Count)
          marker.Rotations = (byte) intList[index];
        if (index < floatList.Count)
          marker.PartChanceToSpawn = floatList[index];
        this.POIMarkers.Add(marker);
      }
    }
    this.yOffset = this.properties.GetInt("YOffset");
    if (this.size == Vector3i.zero && values.ContainsKey("PrefabSize"))
      this.size = StringParsers.ParseVector3i(this.properties.Values["PrefabSize"]);
    this.rotationToFaceNorth = this.properties.GetInt("RotationToFaceNorth");
    if (this.properties.Values.ContainsKey("Tags"))
      this.tags = FastTags<TagGroup.Poi>.Parse(this.properties.Values["Tags"].Replace(" ", ""));
    if (this.properties.Values.ContainsKey("ThemeTags"))
      this.themeTags = FastTags<TagGroup.Poi>.Parse(this.properties.Values["ThemeTags"].Replace(" ", ""));
    if (this.properties.Values.ContainsKey("ThemeRepeatDistance"))
      this.themeRepeatDistance = StringParsers.ParseSInt32(this.properties.Values["ThemeRepeatDistance"]);
    if (this.properties.Values.ContainsKey("DuplicateRepeatDistance"))
      this.duplicateRepeatDistance = StringParsers.ParseSInt32(this.properties.Values["DuplicateRepeatDistance"]);
    this.indexedBlockOffsets.Clear();
    if (this.properties.Classes.ContainsKey("IndexedBlockOffsets"))
    {
      foreach (KeyValuePair<string, DynamicProperties> keyValuePair1 in this.properties.Classes["IndexedBlockOffsets"].Classes.Dict)
      {
        if (keyValuePair1.Value.Values.Dict.Count > 0)
        {
          List<Vector3i> vector3iList = new List<Vector3i>();
          this.indexedBlockOffsets[keyValuePair1.Key] = vector3iList;
          foreach (KeyValuePair<string, string> keyValuePair2 in keyValuePair1.Value.Values.Dict)
            vector3iList.Add(StringParsers.ParseVector3i(keyValuePair1.Value.Values[keyValuePair2.Key]));
        }
      }
    }
    if (this.properties.Values.ContainsKey("QuestTags"))
      this.questTags = FastTags<TagGroup.Global>.Parse(this.properties.Values["QuestTags"]);
    this.properties.ParseString("StaticSpawner.Class", ref this.StaticSpawnerClass);
    if (this.properties.Values.ContainsKey("StaticSpawner.Size"))
    {
      string[] strArray = this.properties.Values["StaticSpawner.Size"].Replace(" ", "").Split(',', StringSplitOptions.None);
      this.StaticSpawnerSize = new Vector3i(int.Parse(strArray[0]), int.Parse(strArray[1]), int.Parse(strArray[2]));
    }
    this.properties.ParseInt("StaticSpawner.Trigger", ref this.StaticSpawnerTrigger);
    if (this.properties.Values.ContainsKey("AllowedTownships"))
    {
      this.allowedTownships.Clear();
      foreach (string str in this.properties.Values["AllowedTownships"].Replace(" ", "").Split(',', StringSplitOptions.None))
        this.allowedTownships.Add(str.ToLower());
    }
    if (this.properties.Values.ContainsKey("AllowedBiomes"))
    {
      this.allowedBiomes.Clear();
      foreach (string str in this.properties.Values["AllowedBiomes"].Replace(" ", "").Split(',', StringSplitOptions.None))
        this.allowedBiomes.Add(str.ToLower());
    }
    if (this.properties.Values.ContainsKey("Zoning"))
    {
      this.allowedZones.Clear();
      foreach (string str in this.properties.Values["Zoning"].Split(',', StringSplitOptions.None))
        this.AddAllowedZone(str.Trim());
    }
    else
      this.allowedZones.Add("none");
    if (!this.properties.Classes.ContainsKey("Stats"))
      return;
    this.renderingCost = WorldStats.FromProperties(this.properties.Classes["Stats"]);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void writeToProperties()
  {
    this.properties.Values["CopyAirBlocks"] = this.bCopyAirBlocks.ToString();
    this.properties.Values["ExcludeDistantPOIMesh"] = this.bExcludeDistantPOIMesh.ToString();
    this.properties.Values["ExcludePOICulling"] = this.bExcludePOICulling.ToString();
    this.properties.Values["DistantPOIYOffset"] = this.distantPOIYOffset.ToCultureInvariantString();
    if (this.distantPOIOverride != null)
      this.properties.Values["DistantPOIOverride"] = this.distantPOIOverride;
    this.properties.Values.Remove("EditorGroups");
    if (this.editorGroups.Count > 0)
    {
      string str = string.Empty;
      for (int index = 0; index < this.editorGroups.Count; ++index)
        str = str + this.editorGroups[index] + (index < this.editorGroups.Count - 1 ? ", " : string.Empty);
      this.properties.Values["EditorGroups"] = str;
    }
    this.properties.Values["AllowTopSoilDecorations"] = this.bAllowTopSoilDecorations.ToString();
    this.properties.Values["DifficultyTier"] = this.DifficultyTier.ToString();
    this.properties.Values["ShowQuestClearCount"] = this.ShowQuestClearCount.ToString();
    this.properties.Values["TraderArea"] = this.bTraderArea.ToString();
    if (this.bTraderArea)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      foreach (Prefab.PrefabTeleportVolume teleportVolume in this.TeleportVolumes)
      {
        if (stringBuilder1.Length > 0)
        {
          stringBuilder1.Append('#');
          stringBuilder2.Append('#');
        }
        stringBuilder1.Append(teleportVolume.size.ToString());
        stringBuilder2.Append(teleportVolume.startPos.ToString());
      }
      this.properties.Values["TeleportVolumeSize"] = stringBuilder1.ToString();
      this.properties.Values["TeleportVolumeStart"] = stringBuilder2.ToString();
    }
    else
    {
      this.properties.Values.Remove("TeleportVolumeSize");
      this.properties.Values.Remove("TeleportVolumeStart");
    }
    foreach (KeyValuePair<string, string> keyValuePair in this.properties.Values.Dict)
    {
      if (keyValuePair.Key.StartsWith("SVS"))
        this.properties.Values.MarkToRemove(keyValuePair.Key);
    }
    this.properties.Values.RemoveAllMarked((DictionarySave<string, string>.DictionaryRemoveCallback) ([PublicizedFrom(EAccessModifier.Private)] (_key) => this.properties.Values.Remove(_key)));
    bool flag = true;
    if (this.bSleeperVolumes)
    {
      StringBuilder stringBuilder3 = new StringBuilder();
      StringBuilder stringBuilder4 = new StringBuilder();
      StringBuilder stringBuilder5 = new StringBuilder();
      StringBuilder stringBuilder6 = new StringBuilder();
      StringBuilder stringBuilder7 = new StringBuilder();
      StringBuilder stringBuilder8 = new StringBuilder();
      StringBuilder stringBuilder9 = new StringBuilder();
      StringBuilder stringBuilder10 = new StringBuilder();
      foreach (Prefab.PrefabSleeperVolume sleeperVolume in this.SleeperVolumes)
      {
        if (sleeperVolume.used)
        {
          if (stringBuilder3.Length > 0)
          {
            stringBuilder3.Append('#');
            stringBuilder4.Append('#');
            stringBuilder5.Append(',');
            stringBuilder6.Append(',');
            stringBuilder7.Append(',');
            stringBuilder8.Append(',');
            stringBuilder9.Append(',');
            stringBuilder10.Append('#');
          }
          stringBuilder3.Append(sleeperVolume.size.ToString());
          stringBuilder4.Append(sleeperVolume.startPos.ToString());
          stringBuilder5.Append(sleeperVolume.groupId);
          stringBuilder6.Append(sleeperVolume.groupName);
          stringBuilder6.Append(',');
          stringBuilder6.Append(sleeperVolume.spawnCountMin.ToString());
          stringBuilder6.Append(',');
          stringBuilder6.Append(sleeperVolume.spawnCountMax.ToString());
          stringBuilder7.Append(sleeperVolume.isPriority.ToString());
          stringBuilder8.Append(sleeperVolume.isQuestExclude.ToString());
          stringBuilder9.Append(sleeperVolume.flags.ToString("x"));
          for (int index = 0; index < sleeperVolume.triggeredByIndices.Count; ++index)
          {
            if (index > 0)
              stringBuilder10.Append(',');
            stringBuilder10.Append(sleeperVolume.triggeredByIndices[index].ToString());
          }
          if (sleeperVolume.triggeredByIndices.Count == 0)
            stringBuilder10.Append(" ");
        }
      }
      if (stringBuilder3.Length > 0)
      {
        flag = false;
        this.properties.Values["SleeperVolumeSize"] = stringBuilder3.ToString();
        this.properties.Values["SleeperVolumeStart"] = stringBuilder4.ToString();
        this.properties.Values["SleeperVolumeGroupId"] = stringBuilder5.ToString();
        this.properties.Values["SleeperVolumeGroup"] = stringBuilder6.ToString();
        this.properties.Values["SleeperIsLootVolume"] = stringBuilder7.ToString();
        this.properties.Values["SleeperIsQuestExclude"] = stringBuilder8.ToString();
        this.properties.Values["SleeperVolumeFlags"] = stringBuilder9.ToString();
        this.properties.Values["SleeperVolumeTriggeredBy"] = stringBuilder10.ToString();
        int num = 0;
        for (int index = 0; index < this.SleeperVolumes.Count; ++index)
        {
          Prefab.PrefabSleeperVolume sleeperVolume = this.SleeperVolumes[index];
          if (sleeperVolume.used)
          {
            if (sleeperVolume.minScript != null)
              this.properties.Values["SVS" + num.ToString()] = sleeperVolume.minScript;
            ++num;
          }
        }
      }
    }
    if (flag)
    {
      this.properties.Values.Remove("SleeperVolumeSize");
      this.properties.Values.Remove("SleeperVolumeStart");
      this.properties.Values.Remove("SleeperVolumeGroupId");
      this.properties.Values.Remove("SleeperVolumeGroup");
      this.properties.Values.Remove("SleeperIsLootVolume");
      this.properties.Values.Remove("SleeperIsQuestExclude");
      this.properties.Values.Remove("SleeperVolumeFlags");
      this.properties.Values.Remove("SleeperVolumeTriggeredBy");
    }
    if (this.bInfoVolumes)
    {
      StringBuilder stringBuilder11 = new StringBuilder();
      StringBuilder stringBuilder12 = new StringBuilder();
      foreach (Prefab.PrefabInfoVolume infoVolume in this.InfoVolumes)
      {
        if (stringBuilder11.Length > 0)
        {
          stringBuilder11.Append('#');
          stringBuilder12.Append('#');
        }
        stringBuilder11.Append(infoVolume.size.ToString());
        stringBuilder12.Append(infoVolume.startPos.ToString());
      }
      this.properties.Values["InfoVolumeSize"] = stringBuilder11.ToString();
      this.properties.Values["InfoVolumeStart"] = stringBuilder12.ToString();
    }
    else
    {
      this.properties.Values.Remove("InfoVolumeSize");
      this.properties.Values.Remove("InfoVolumeStart");
    }
    if (this.bWallVolumes)
    {
      StringBuilder stringBuilder13 = new StringBuilder();
      StringBuilder stringBuilder14 = new StringBuilder();
      foreach (Prefab.PrefabWallVolume wallVolume in this.WallVolumes)
      {
        if (stringBuilder13.Length > 0)
        {
          stringBuilder13.Append('#');
          stringBuilder14.Append('#');
        }
        stringBuilder13.Append(wallVolume.size.ToString());
        stringBuilder14.Append(wallVolume.startPos.ToString());
      }
      this.properties.Values["WallVolumeSize"] = stringBuilder13.ToString();
      this.properties.Values["WallVolumeStart"] = stringBuilder14.ToString();
    }
    else
    {
      this.properties.Values.Remove("WallVolumeSize");
      this.properties.Values.Remove("WallVolumeStart");
    }
    if (this.bTriggerVolumes)
    {
      StringBuilder stringBuilder15 = new StringBuilder();
      StringBuilder stringBuilder16 = new StringBuilder();
      StringBuilder stringBuilder17 = new StringBuilder();
      foreach (Prefab.PrefabTriggerVolume triggerVolume in this.TriggerVolumes)
      {
        if (stringBuilder15.Length > 0)
        {
          stringBuilder15.Append('#');
          stringBuilder16.Append('#');
          stringBuilder17.Append('#');
        }
        for (int index = 0; index < triggerVolume.TriggersIndices.Count; ++index)
        {
          if (index > 0)
            stringBuilder17.Append(',');
          stringBuilder17.Append(triggerVolume.TriggersIndices[index].ToString());
        }
        if (triggerVolume.TriggersIndices.Count == 0)
          stringBuilder17.Append(" ");
        stringBuilder15.Append(triggerVolume.size.ToString());
        stringBuilder16.Append(triggerVolume.startPos.ToString());
      }
      this.properties.Values["TriggerVolumeSize"] = stringBuilder15.ToString();
      this.properties.Values["TriggerVolumeStart"] = stringBuilder16.ToString();
      this.properties.Values["TriggerVolumeTriggers"] = stringBuilder17.ToString();
    }
    else
    {
      this.properties.Values.Remove("TriggerVolumeSize");
      this.properties.Values.Remove("TriggerVolumeStart");
      this.properties.Values.Remove("TriggerVolumeTriggers");
    }
    if (this.bPOIMarkers)
    {
      StringBuilder stringBuilder18 = new StringBuilder();
      StringBuilder stringBuilder19 = new StringBuilder();
      StringBuilder stringBuilder20 = new StringBuilder();
      StringBuilder stringBuilder21 = new StringBuilder();
      StringBuilder stringBuilder22 = new StringBuilder();
      StringBuilder stringBuilder23 = new StringBuilder();
      StringBuilder stringBuilder24 = new StringBuilder();
      StringBuilder stringBuilder25 = new StringBuilder();
      foreach (Prefab.Marker poiMarker in this.POIMarkers)
      {
        if (stringBuilder19.Length > 0)
        {
          stringBuilder18.Append('#');
          stringBuilder19.Append('#');
          stringBuilder20.Append(',');
          stringBuilder21.Append('#');
          stringBuilder22.Append(',');
          stringBuilder23.Append(',');
          stringBuilder24.Append(',');
          stringBuilder25.Append(',');
        }
        stringBuilder18.Append(poiMarker.Size.ToString());
        stringBuilder19.Append(poiMarker.Start.ToString());
        stringBuilder20.Append(poiMarker.GroupName);
        stringBuilder21.Append(poiMarker.Tags.ToString());
        stringBuilder22.Append(poiMarker.MarkerType.ToString());
        stringBuilder23.Append(poiMarker.PartToSpawn);
        stringBuilder24.Append(poiMarker.Rotations.ToString());
        stringBuilder25.Append(poiMarker.PartChanceToSpawn.ToString());
      }
      this.properties.Values["POIMarkerSize"] = stringBuilder18.ToString();
      this.properties.Values["POIMarkerStart"] = stringBuilder19.ToString();
      this.properties.Values["POIMarkerGroup"] = stringBuilder20.ToString();
      this.properties.Values["POIMarkerTags"] = stringBuilder21.ToString();
      this.properties.Values["POIMarkerType"] = stringBuilder22.ToString();
      this.properties.Values["POIMarkerPartToSpawn"] = stringBuilder23.ToString();
      this.properties.Values["POIMarkerPartRotations"] = stringBuilder24.ToString();
      this.properties.Values["POIMarkerPartSpawnChance"] = stringBuilder25.ToString();
    }
    if (this.yOffset != 0)
      this.properties.Values["YOffset"] = this.yOffset.ToString();
    else
      this.properties.Values.Remove("YOffset");
    this.properties.Values["PrefabSize"] = this.size.ToString();
    this.properties.Values["RotationToFaceNorth"] = this.rotationToFaceNorth.ToString();
    if (this.StaticSpawnerClass != null)
      this.properties.Values["StaticSpawner.Class"] = this.StaticSpawnerClass;
    else
      this.properties.Values.Remove("StaticSpawner.Class");
    if (this.StaticSpawnerSize != Vector3i.zero)
      this.properties.Values["StaticSpawner.Size"] = $"{this.StaticSpawnerSize.x.ToString()},{this.StaticSpawnerSize.y.ToString()},{this.StaticSpawnerSize.z.ToString()}";
    else
      this.properties.Values.Remove("StaticSpawner.Size");
    if (this.StaticSpawnerTrigger > 0)
      this.properties.Values["StaticSpawner.Trigger"] = this.StaticSpawnerTrigger.ToString();
    else
      this.properties.Values.Remove("StaticSpawner.Trigger");
    string str1 = "";
    for (int index = 0; index < this.allowedTownships.Count; ++index)
      str1 = str1 + this.allowedTownships[index] + (index < this.allowedTownships.Count - 1 ? "," : "");
    if (str1.Length > 0)
      this.properties.Values["AllowedTownships"] = str1;
    else
      this.properties.Values.Remove("AllowedTownships");
    string str2 = "";
    for (int index = 0; index < this.allowedBiomes.Count; ++index)
      str2 = str2 + this.allowedBiomes[index] + (index < this.allowedBiomes.Count - 1 ? "," : "");
    if (str2.Length > 0)
      this.properties.Values["AllowedBiomes"] = str2;
    else
      this.properties.Values.Remove("AllowedBiomes");
    if (this.tags.ToString() != "")
      this.properties.Values["Tags"] = this.tags.ToString();
    else
      this.properties.Values.Remove("Tags");
    if (this.themeTags.ToString() != "")
      this.properties.Values["ThemeTags"] = this.themeTags.ToString();
    else
      this.properties.Values.Remove("ThemeTags");
    if (this.themeRepeatDistance != 300)
      this.properties.Values["ThemeRepeatDistance"] = this.themeRepeatDistance.ToString((IFormatProvider) NumberFormatInfo.InvariantInfo);
    else
      this.properties.Values.Remove("ThemeRepeatDistance");
    if (this.duplicateRepeatDistance != 1000)
      this.properties.Values["DuplicateRepeatDistance"] = this.duplicateRepeatDistance.ToString((IFormatProvider) NumberFormatInfo.InvariantInfo);
    else
      this.properties.Values.Remove("DuplicateRepeatDistance");
    if (this.indexedBlockOffsets.Any<KeyValuePair<string, List<Vector3i>>>((Func<KeyValuePair<string, List<Vector3i>>, bool>) ([PublicizedFrom(EAccessModifier.Internal)] (_pair) => _pair.Value.Count > 0)))
    {
      DynamicProperties dynamicProperties1 = new DynamicProperties();
      this.properties.Classes["IndexedBlockOffsets"] = dynamicProperties1;
      foreach (KeyValuePair<string, List<Vector3i>> indexedBlockOffset in this.indexedBlockOffsets)
      {
        if (indexedBlockOffset.Value.Count > 0)
        {
          DynamicProperties dynamicProperties2 = new DynamicProperties();
          dynamicProperties1.Classes[indexedBlockOffset.Key] = dynamicProperties2;
          for (int index = 0; index < indexedBlockOffset.Value.Count; ++index)
            dynamicProperties2.Values[index.ToString()] = indexedBlockOffset.Value[index].ToString();
        }
      }
    }
    else
      this.properties.Classes.Remove("IndexedBlockOffsets");
    if (!this.questTags.IsEmpty)
      str2 = this.questTags.ToString();
    if (str2.Length > 0)
      this.properties.Values["QuestTags"] = str2;
    else
      this.properties.Values.Remove("QuestTags");
    this.properties.Values.Remove("Zoning");
    if (this.allowedZones.Count > 0)
    {
      string str3 = string.Empty;
      for (int index = 0; index < this.allowedZones.Count; ++index)
        str3 = str3 + this.allowedZones[index] + (index < this.allowedZones.Count - 1 ? ", " : string.Empty);
      this.properties.Values["Zoning"] = str3;
    }
    if (this.renderingCost == null)
      return;
    this.properties.Classes["Stats"] = this.renderingCost.ToProperties();
  }

  public static bool PrefabExists(string _prefabFileName)
  {
    return PathAbstractions.PrefabsSearchPaths.GetLocation(_prefabFileName).Type != PathAbstractions.EAbstractedLocationType.None;
  }

  public bool Load(
    string _prefabName,
    bool _applyMapping = true,
    bool _fixChildblocks = true,
    bool _allowMissingBlocks = false,
    bool _skipLoadingBlockData = false)
  {
    return this.Load(PathAbstractions.PrefabsSearchPaths.GetLocation(_prefabName), _applyMapping, _fixChildblocks, _allowMissingBlocks, _skipLoadingBlockData);
  }

  public bool Load(
    PathAbstractions.AbstractedLocation _location,
    bool _applyMapping = true,
    bool _fixChildblocks = true,
    bool _allowMissingBlocks = false,
    bool _skipLoadingBlockData = false)
  {
    if (_location.Type == PathAbstractions.EAbstractedLocationType.None)
    {
      if (Object.op_Inequality((Object) SingletonMonoBehaviour<ConnectionManager>.Instance, (Object) null) && SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
        Log.Warning("Prefab loading failed. Prefab '{0}' does not exist!", new object[1]
        {
          (object) _location.Name
        });
      else
        Log.Error("Prefab loading failed. Prefab '{0}' does not exist!", new object[1]
        {
          (object) _location.Name
        });
      return false;
    }
    this.location = _location;
    return (!_skipLoadingBlockData || this.loadSizeDataOnly(_location, _applyMapping, _fixChildblocks, _allowMissingBlocks, _skipLoadingBlockData)) && this.loadBlockData(_location, _applyMapping, _fixChildblocks, _allowMissingBlocks, _skipLoadingBlockData) && this.LoadXMLData(_location);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public bool loadSizeDataOnly(
    PathAbstractions.AbstractedLocation _location,
    bool _applyMapping,
    bool _fixChildblocks,
    bool _allowMissingBlocks,
    bool _skipLoadingBlockData = false)
  {
    using (Stream _stream = SdFile.OpenRead(_location.FullPath))
    {
      using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
      {
        pooledBinaryReader.SetBaseStream(_stream);
        if (pooledBinaryReader.ReadChar() != 't' || pooledBinaryReader.ReadChar() != 't' || pooledBinaryReader.ReadChar() != 's' || pooledBinaryReader.ReadChar() != char.MinValue)
          return false;
        int num = (int) pooledBinaryReader.ReadUInt32();
        this.size = new Vector3i();
        this.size.x = (int) pooledBinaryReader.ReadInt16();
        this.size.y = (int) pooledBinaryReader.ReadInt16();
        this.size.z = (int) pooledBinaryReader.ReadInt16();
      }
    }
    return true;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public bool loadBlockData(
    PathAbstractions.AbstractedLocation _location,
    bool _applyMapping,
    bool _fixChildblocks,
    bool _allowMissingBlocks,
    bool _skipLoadingBlockData = false)
  {
    bool flag = true;
    ArrayListMP<int> arrayListMp = (ArrayListMP<int>) null;
    if (_applyMapping)
    {
      arrayListMp = this.loadIdMapping(_location.Folder, _location.FileNameNoExtension, _allowMissingBlocks);
      if (arrayListMp == null)
        return false;
    }
    try
    {
      using (Stream _stream = SdFile.OpenRead(_location.FullPath))
      {
        using (PooledBinaryReader _br = MemoryPools.poolBinaryReader.AllocSync(false))
        {
          _br.SetBaseStream(_stream);
          if (_br.ReadChar() != 't' || _br.ReadChar() != 't' || _br.ReadChar() != 's' || _br.ReadChar() != char.MinValue)
            return false;
          uint _version = _br.ReadUInt32();
          if (!this.readBlockData(_br, _version, arrayListMp?.Items, true))
            return false;
          if (_version > 12U)
            this.readTileEntities(_br);
          if (_version > 15U)
            this.readTriggerData(_br);
          this.insidePos.Load(_location.FullPathNoExtension + ".ins", this.size);
        }
      }
    }
    catch (Exception ex)
    {
      Log.Exception(ex);
      flag = false;
    }
    return flag;
  }

  public bool LoadXMLData(PathAbstractions.AbstractedLocation _location)
  {
    this.location = _location;
    if (!SdFile.Exists(_location.FullPathNoExtension + ".xml"))
      return true;
    if (!this.properties.Load(_location.Folder, _location.Name, false))
      return false;
    this.ReadFromProperties();
    return true;
  }

  public bool Save(string _prefabName, bool _createMapping = true)
  {
    return this.Save(PathAbstractions.PrefabsSearchPaths.GetLocation(_prefabName), _createMapping);
  }

  public bool Save(PathAbstractions.AbstractedLocation _location, bool _createMapping = true)
  {
    return this.saveBlockData(_location, _createMapping) && this.SaveXMLData(_location);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void AddAllChildBlocks()
  {
    if (!this.isCellsDataOwner || this.blockCells == null || this.blockCells.a == null)
      return;
    int length1 = this.blockCells.a.Length;
    for (int index1 = 0; index1 < length1; ++index1)
    {
      Prefab.Cells<uint>.CellsAtZ cellsAtZ = this.blockCells.a[index1];
      if (cellsAtZ != null)
      {
        int length2 = cellsAtZ.a.Length;
        for (int index2 = 0; index2 < length2; ++index2)
        {
          Prefab.Cells<uint>.CellsAtX cellsAtX = cellsAtZ.a[index2];
          if (cellsAtX != null)
          {
            int length3 = cellsAtX.a.Length;
            for (int index3 = 0; index3 < length3; ++index3)
            {
              Prefab.Cells<uint>.Cell cell = cellsAtX.a[index3];
              if (cell.a != null)
              {
                for (int index4 = 0; index4 < cell.a.Length; ++index4)
                {
                  uint _rawData = cell.a[index4];
                  if (((int) _rawData & (int) ushort.MaxValue) != 0)
                  {
                    BlockValue blockValue = new BlockValue(_rawData);
                    if (blockValue.rawData != 0U && !blockValue.ischild)
                    {
                      Block block = blockValue.Block;
                      if (block != null && block.isMultiBlock)
                      {
                        int num1 = (index3 << 2) + (index4 & 3);
                        int num2 = index1;
                        int num3 = (index2 << 2) + (index4 >> 2);
                        int rotation = (int) blockValue.rotation;
                        for (int _idx = block.multiBlockPos.Length - 1; _idx >= 0; --_idx)
                        {
                          Vector3i vector3i = block.multiBlockPos.Get(_idx, blockValue.type, rotation);
                          if (!(vector3i == Vector3i.zero))
                          {
                            int x = vector3i.x;
                            int y = vector3i.y;
                            int z = vector3i.z;
                            blockValue.ischild = true;
                            blockValue.parentx = -x;
                            blockValue.parenty = -y;
                            blockValue.parentz = -z;
                            this.RotateRelative(ref x, ref z);
                            int _x = num1 + x;
                            int _y = num2 + y;
                            int _z = num3 + z;
                            if ((long) (uint) _x < (long) this.size.x && (long) (uint) _y < (long) this.size.y && (long) (uint) _z < (long) this.size.z)
                              this.blockCells.SetData(_x, _y, _z, blockValue.rawData);
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void RemoveAllChildAndOldBlocks()
  {
    for (int _y = this.size.y - 1; _y >= 0; --_y)
    {
      for (int _z = this.size.z - 1; _z >= 0; --_z)
      {
        for (int _x = this.size.x - 1; _x >= 0; --_x)
        {
          BlockValue block1 = this.GetBlock(_x, _y, _z);
          Block block2 = block1.Block;
          if (block2 == null)
            this.SetBlock(_x, _y, _z, BlockValue.Air);
          else if (block1.ischild)
            this.SetBlock(_x, _y, _z, BlockValue.Air);
          else if (block2 is BlockModelTree && ((int) block1.meta & 1) != 0)
            this.SetBlock(_x, _y, _z, BlockValue.Air);
        }
      }
    }
  }

  public bool SaveXMLData(PathAbstractions.AbstractedLocation _location)
  {
    this.writeToProperties();
    return this.properties.Save("prefab", _location.Folder, _location.FileNameNoExtension);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public bool saveBlockData(PathAbstractions.AbstractedLocation _location, bool _createMapping)
  {
    this.RemoveAllChildAndOldBlocks();
    if (_createMapping)
    {
      NameIdMapping nameIdMapping = new NameIdMapping(_location.FullPathNoExtension + ".blocks.nim", Block.MAX_BLOCKS);
      for (int _offset = this.GetBlockCount() - 1; _offset >= 0; --_offset)
      {
        int _x;
        int _y;
        int _z;
        this.offsetToCoord(_offset, out _x, out _y, out _z);
        Block block = this.GetBlock(_x, _y, _z).Block;
        nameIdMapping.AddMapping(block.blockID, block.GetBlockName());
      }
      nameIdMapping.WriteToFile();
    }
    try
    {
      using (Stream _stream = SdFile.Open(_location.FullPath, FileMode.Create))
      {
        using (PooledBinaryWriter _bw = MemoryPools.poolBinaryWriter.AllocSync(false))
        {
          _bw.SetBaseStream(_stream);
          _bw.Write('t');
          _bw.Write('t');
          _bw.Write('s');
          _bw.Write((byte) 0);
          _bw.Write((uint) Prefab.CurrentSaveVersion);
          this.writeBlockData((BinaryWriter) _bw);
          this.writeTileEntities((BinaryWriter) _bw);
          this.writeTriggerData((BinaryWriter) _bw);
          if (this.IsCullThisPrefab())
            this.insidePos.Save(_location.FullPathNoExtension + ".ins");
          else
            SdFile.Delete(_location.FullPathNoExtension + ".ins");
        }
      }
      return true;
    }
    catch (Exception ex)
    {
      Log.Exception(ex);
    }
    return false;
  }

  public bool IsCullThisPrefab() => !this.bExcludePOICulling;

  [PublicizedFrom(EAccessModifier.Private)]
  public void writeBlockData(BinaryWriter _bw)
  {
    _bw.Write((short) this.size.x);
    _bw.Write((short) this.size.y);
    _bw.Write((short) this.size.z);
    Prefab.Data arrays = this.CellsToArrays();
    foreach (uint block in arrays.m_Blocks)
      _bw.Write(block);
    _bw.Write(arrays.m_Density);
    byte[] buffer = new byte[arrays.m_Damage.Length * 2];
    for (int index = 0; index < arrays.m_Damage.Length; ++index)
    {
      buffer[index * 2] = (byte) ((uint) arrays.m_Damage[index] & (uint) byte.MaxValue);
      buffer[index * 2 + 1] = (byte) ((int) arrays.m_Damage[index] >> 8 & (int) byte.MaxValue);
    }
    _bw.Write(buffer);
    SimpleBitStream simpleBitStream1 = new SimpleBitStream();
    for (int index = 0; index < arrays.m_Textures.Length; ++index)
    {
      bool _b = !arrays.m_Textures[index].IsDefault;
      simpleBitStream1.Add(_b);
    }
    simpleBitStream1.Write(_bw);
    for (int index = 0; index < arrays.m_Textures.Length; ++index)
    {
      if (!arrays.m_Textures[index].IsDefault)
        arrays.m_Textures[index].Write(_bw);
    }
    SimpleBitStream simpleBitStream2 = new SimpleBitStream();
    for (int index = 0; index < arrays.m_Water.Length; ++index)
      simpleBitStream2.Add(arrays.m_Water[index].HasMass());
    simpleBitStream2.Write(_bw);
    for (int index = 0; index < arrays.m_Water.Length; ++index)
    {
      WaterValue waterValue = arrays.m_Water[index];
      if (waterValue.HasMass())
        waterValue.Write(_bw);
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void writeTileEntities(BinaryWriter _bw)
  {
    _bw.Write((short) this.tileEntities.Count);
    foreach (KeyValuePair<Vector3i, TileEntity> tileEntity in this.tileEntities)
    {
      using (PooledExpandableMemoryStream _stream = MemoryPools.poolMemoryStream.AllocSync(true))
      {
        using (PooledBinaryWriter _bw1 = MemoryPools.poolBinaryWriter.AllocSync(true))
        {
          _bw1.SetBaseStream((Stream) _stream);
          tileEntity.Value.write(_bw1, TileEntity.StreamModeWrite.Persistency);
        }
        _bw.Write((short) _stream.Length);
        _bw.Write((byte) tileEntity.Value.GetTileEntityType());
        _stream.WriteTo(_bw.BaseStream);
      }
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void writeTriggerData(BinaryWriter _bw)
  {
    _bw.Write((short) this.triggerData.Count);
    foreach (KeyValuePair<Vector3i, BlockTrigger> keyValuePair in this.triggerData)
    {
      using (PooledExpandableMemoryStream _stream = MemoryPools.poolMemoryStream.AllocSync(true))
      {
        using (PooledBinaryWriter _bw1 = MemoryPools.poolBinaryWriter.AllocSync(true))
        {
          _bw1.SetBaseStream((Stream) _stream);
          keyValuePair.Value.Write(_bw1);
        }
        _bw.Write((short) _stream.Length);
        StreamUtils.Write(_bw, keyValuePair.Key);
        _stream.WriteTo(_bw.BaseStream);
      }
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public bool readBlockData(
    PooledBinaryReader _br,
    uint _version,
    int[] _blockIdMapping,
    bool _fixChildblocks)
  {
    this.statistics.Clear();
    this.multiBlockParentIndices.Clear();
    this.decoAllowedBlockIndices.Clear();
    this.localRotation = 0;
    this.size.x = (int) _br.ReadInt16();
    this.size.y = (int) _br.ReadInt16();
    this.size.z = (int) _br.ReadInt16();
    int blockCount = this.GetBlockCount();
    this.InitData();
    Prefab.sharedData.Expand(blockCount);
    Prefab.Data sharedData = Prefab.sharedData;
    if (_version >= 2U && _version < 7U)
      this.bCopyAirBlocks = _br.ReadBoolean();
    if (_version >= 3U && _version < 7U)
      this.bAllowTopSoilDecorations = _br.ReadBoolean();
    List<Vector3i> vector3iList = (List<Vector3i>) null;
    int typeMissingBlock = this.blockTypeMissingBlock;
    if (_blockIdMapping != null && typeMissingBlock >= 0)
      vector3iList = new List<Vector3i>();
    int v2 = blockCount * 4;
    if (Prefab.tempBuf == null || Prefab.tempBuf.Length < v2)
      Prefab.tempBuf = new byte[Utils.FastMax(200000, v2)];
    int index1 = 0;
    _br.Read(Prefab.tempBuf, 0, blockCount * 4);
    if (_version <= 4U)
    {
      for (int _x = 0; _x < this.size.x; ++_x)
      {
        for (int _z = 0; _z < this.size.z; ++_z)
        {
          for (int _y = 0; _y < this.size.y; ++_y)
          {
            BlockValue _bv = new BlockValue((uint) ((int) Prefab.tempBuf[index1] | (int) Prefab.tempBuf[index1 + 1] << 8 | (int) Prefab.tempBuf[index1 + 2] << 16 /*0x10*/ | (int) Prefab.tempBuf[index1 + 3] << 24));
            index1 += 4;
            if (_blockIdMapping != null)
            {
              int num = _blockIdMapping[_bv.type];
              if (num < 0)
              {
                Log.Error($"Loading prefab \"{this.location.ToString()}\" failed: Block {_bv.type.ToString()} used in prefab has no mapping.");
                return false;
              }
              _bv.type = num;
              if (typeMissingBlock >= 0 && _bv.type == this.blockTypeMissingBlock)
                vector3iList.Add(new Vector3i(_x, _y, _z));
            }
            if (_bv.isWater)
            {
              this.SetWater(_x, _y, _z, WaterValue.Full);
            }
            else
            {
              if (_fixChildblocks)
              {
                if (!_bv.ischild)
                {
                  Block block = _bv.Block;
                  if (block == null || ((int) _bv.meta & 1) != 0 && block is BlockModelTree)
                    continue;
                }
                else
                  continue;
              }
              this.SetBlock(_x, _y, _z, _bv);
            }
          }
        }
      }
    }
    else
    {
      for (int _offset = 0; _offset < blockCount; ++_offset)
      {
        uint _rawData = (uint) ((int) Prefab.tempBuf[index1] | (int) Prefab.tempBuf[index1 + 1] << 8 | (int) Prefab.tempBuf[index1 + 2] << 16 /*0x10*/ | (int) Prefab.tempBuf[index1 + 3] << 24);
        index1 += 4;
        sharedData.m_Blocks[_offset] = 0U;
        if (_rawData != 0U)
        {
          if (_version < 18U)
            _rawData = BlockValueV3.ConvertOldRawData(_rawData);
          BlockValue blockValue = new BlockValue(_rawData);
          if (_blockIdMapping != null)
          {
            int type = blockValue.type;
            if (type != 0)
            {
              int num = _blockIdMapping[type];
              if (num < 0)
              {
                int _x;
                int _y;
                int _z;
                this.offsetToCoord(_offset, out _x, out _y, out _z);
                Log.Error($"Loading prefab \"{this.location.ToString()}\" failed: Block {type.ToString()} used in prefab at {_x.ToString()} / {_y.ToString()} / {_z.ToString()} has no mapping.");
                return false;
              }
              blockValue.type = num;
              if (typeMissingBlock >= 0 && num == this.blockTypeMissingBlock)
              {
                int _x;
                int _y;
                int _z;
                this.offsetToCoord(_offset, out _x, out _y, out _z);
                vector3iList.Add(new Vector3i(_x, _y, _z));
              }
            }
          }
          if (_version < 17U && blockValue.isWater)
          {
            sharedData.m_Water[_offset] = WaterValue.Full;
          }
          else
          {
            Block block = blockValue.Block;
            this.updateBlockStatistics(blockValue, block);
            if (!_fixChildblocks || !blockValue.ischild && block != null && (((int) blockValue.meta & 1) == 0 || !(block is BlockModelTree)))
            {
              if (block.isMultiBlock && !blockValue.ischild)
                this.multiBlockParentIndices.Add(_offset);
              if (DecoUtils.HasDecoAllowed(blockValue))
                this.decoAllowedBlockIndices.Add(_offset);
              sharedData.m_Blocks[_offset] = blockValue.rawData;
            }
          }
        }
      }
      _br.Read(sharedData.m_Density, 0, this.size.x * this.size.y * this.size.z);
    }
    if (_blockIdMapping != null && typeMissingBlock >= 0)
    {
      foreach (Vector3i vector3i in vector3iList)
        this.SetDensity(vector3i.x, vector3i.y, vector3i.z, MarchingCubes.DensityAir);
    }
    if (_version > 8U)
    {
      _br.Read(Prefab.tempBuf, 0, blockCount * 2);
      for (int index2 = 0; index2 < blockCount; ++index2)
        sharedData.m_Damage[index2] = (ushort) ((uint) Prefab.tempBuf[index2 * 2] | (uint) Prefab.tempBuf[index2 * 2 + 1] << 8);
    }
    if (_version >= 10U)
    {
      Prefab.simpleBitStreamReader.Reset();
      Prefab.simpleBitStreamReader.Read((BinaryReader) _br);
      if (_version >= 19U)
      {
        int nextOffset;
        while ((nextOffset = Prefab.simpleBitStreamReader.GetNextOffset()) >= 0)
          sharedData.m_Textures[nextOffset].Read((BinaryReader) _br);
      }
      else
      {
        int nextOffset;
        while ((nextOffset = Prefab.simpleBitStreamReader.GetNextOffset()) >= 0)
          sharedData.m_Textures[nextOffset][0] = _br.ReadInt64();
      }
    }
    this.entities.Clear();
    if (_version >= 4U && _version < 12U)
    {
      int num = (int) _br.ReadInt16();
      for (int index3 = 0; index3 < num; ++index3)
      {
        EntityCreationData entityCreationData = new EntityCreationData();
        entityCreationData.read(_br, false);
        this.entities.Add(entityCreationData);
      }
    }
    if (_version >= 17U)
    {
      Prefab.simpleBitStreamReader.Reset();
      Prefab.simpleBitStreamReader.Read((BinaryReader) _br);
      int nextOffset;
      while ((nextOffset = Prefab.simpleBitStreamReader.GetNextOffset()) >= 0)
        sharedData.m_Water[nextOffset] = WaterValue.FromStream((BinaryReader) _br);
    }
    this.CellsFromArrays(ref sharedData);
    if (_fixChildblocks)
      this.AddAllChildBlocks();
    return true;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void CellsFromArrays(ref Prefab.Data _data)
  {
    BlockValue air = BlockValue.Air;
    for (int _y = 0; _y < this.size.y; ++_y)
    {
      for (int _z = this.size.z - 1; _z >= 0; --_z)
      {
        for (int _x = this.size.x - 1; _x >= 0; --_x)
        {
          int offset = this.CoordToOffset(0, _x, _y, _z);
          air.rawData = _data.m_Blocks[offset];
          if (!air.isair)
            this.blockCells.AllocCell(_x, _y, _z).Set(_x, _z, air.rawData);
          ushort num1 = _data.m_Damage[offset];
          if (num1 != (ushort) 0)
            this.damageCells.AllocCell(_x, _y, _z).Set(_x, _z, num1);
          sbyte num2 = (sbyte) _data.m_Density[offset];
          if ((int) num2 != (int) this.densityCells.defaultValue)
            this.densityCells.AllocCell(_x, _y, _z).Set(_x, _z, num2);
          TextureFullArray texture = _data.m_Textures[offset];
          if (!texture.IsDefault)
            this.textureCells.AllocCell(_x, _y, _z).Set(_x, _z, texture);
          WaterValue waterValue = _data.m_Water[offset];
          if (waterValue.HasMass())
            this.waterCells.AllocCell(_x, _y, _z).Set(_x, _z, waterValue);
        }
      }
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public Prefab.Data CellsToArrays()
  {
    Prefab.Data arrays;
    arrays.m_Blocks = this.blockCells.ToArray(this, this.size);
    arrays.m_Damage = this.damageCells.ToArray(this, this.size);
    arrays.m_Density = (byte[]) this.densityCells.ToArray(this, this.size);
    arrays.m_Textures = this.textureCells.ToArray(this, this.size);
    arrays.m_Water = this.waterCells.ToArray(this, this.size);
    return arrays;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void readTileEntities(PooledBinaryReader _br)
  {
    this.tileEntities.Clear();
    int num = (int) _br.ReadInt16();
    for (int index = 0; index < num; ++index)
    {
      int _length = (int) _br.ReadInt16();
      TileEntityType type = (TileEntityType) _br.ReadByte();
      try
      {
        TileEntity tileEntity = TileEntity.Instantiate(type, (Chunk) null);
        using (PooledExpandableMemoryStream expandableMemoryStream = MemoryPools.poolMemoryStream.AllocSync(true))
        {
          StreamUtils.StreamCopy(_br.BaseStream, (Stream) expandableMemoryStream, _length);
          expandableMemoryStream.Position = 0L;
          using (PooledBinaryReader _br1 = MemoryPools.poolBinaryReader.AllocSync(true))
          {
            _br1.SetBaseStream((Stream) expandableMemoryStream);
            tileEntity.read(_br1, TileEntity.StreamModeRead.Persistency);
          }
        }
        this.tileEntities.Add(tileEntity.localChunkPos, tileEntity);
      }
      catch (Exception ex)
      {
        Log.Error($"Skipping loading of active block data for {this.PrefabName} because of the following exception:");
        Log.Exception(ex);
      }
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void readTriggerData(PooledBinaryReader _br)
  {
    this.triggerData.Clear();
    this.TriggerLayers.Clear();
    int num = (int) _br.ReadInt16();
    for (int index = 0; index < num; ++index)
    {
      int _length = (int) _br.ReadInt16();
      Vector3i vector3i = StreamUtils.ReadVector3i((BinaryReader) _br);
      try
      {
        BlockTrigger trigger = new BlockTrigger((Chunk) null);
        trigger.LocalChunkPos = vector3i;
        using (PooledExpandableMemoryStream expandableMemoryStream = MemoryPools.poolMemoryStream.AllocSync(true))
        {
          StreamUtils.StreamCopy(_br.BaseStream, (Stream) expandableMemoryStream, _length);
          expandableMemoryStream.Position = 0L;
          using (PooledBinaryReader _br1 = MemoryPools.poolBinaryReader.AllocSync(true))
          {
            _br1.SetBaseStream((Stream) expandableMemoryStream);
            trigger.Read(_br1);
          }
        }
        if (Block.BlocksLoaded)
        {
          if (!this.GetBlock(vector3i.x, vector3i.y, vector3i.z).Block.AllowBlockTriggers)
            continue;
        }
        this.triggerData.Add(trigger.LocalChunkPos, trigger);
        this.HandleAddingTriggerLayers(trigger);
      }
      catch (Exception ex)
      {
        Log.Error($"Skipping loading of active block data for {this.PrefabName} because of the following exception:");
        Log.Exception(ex);
      }
    }
  }

  public void RotateY(bool _bLeft, int _rotCount)
  {
    if (_rotCount == 0)
      return;
    if (Block.BlocksLoaded && this.isCellsDataOwner)
    {
      int length1 = this.blockCells.a.Length;
      for (int index1 = 0; index1 < length1; ++index1)
      {
        Prefab.Cells<uint>.CellsAtZ cellsAtZ = this.blockCells.a[index1];
        if (cellsAtZ != null)
        {
          int length2 = cellsAtZ.a.Length;
          for (int index2 = 0; index2 < length2; ++index2)
          {
            Prefab.Cells<uint>.CellsAtX cellsAtX = cellsAtZ.a[index2];
            if (cellsAtX != null)
            {
              int length3 = cellsAtX.a.Length;
              for (int index3 = 0; index3 < length3; ++index3)
              {
                Prefab.Cells<uint>.Cell cell = cellsAtX.a[index3];
                if (cell.a != null)
                {
                  for (int index4 = 0; index4 < cell.a.Length; ++index4)
                  {
                    uint _rawData = cell.a[index4];
                    if (((int) _rawData & (int) ushort.MaxValue) != 0)
                    {
                      BlockValue _blockValue = new BlockValue(_rawData);
                      if (!_blockValue.ischild)
                      {
                        Block block = _blockValue.Block;
                        if (block == null || ((int) _blockValue.meta & 1) != 0 && block is BlockModelTree)
                        {
                          cell.a[index4] = 0U;
                        }
                        else
                        {
                          _blockValue = block.shape.RotateY(_bLeft, _blockValue, _rotCount);
                          cell.a[index4] = _blockValue.rawData;
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
    }
    for (int index5 = 0; index5 < _rotCount; ++index5)
    {
      this.localRotation += _bLeft ? 1 : -1;
      this.localRotation &= 3;
      for (int index6 = 0; index6 < this.entities.Count; ++index6)
      {
        EntityCreationData entity = this.entities[index6];
        if (_bLeft)
        {
          entity.pos = new Vector3((float) this.size.z - entity.pos.z, entity.pos.y, entity.pos.x);
          entity.rot = new Vector3(entity.rot.x, entity.rot.y - 90f, entity.rot.z);
        }
        else
        {
          entity.pos = new Vector3(entity.pos.z, entity.pos.y, (float) this.size.x - entity.pos.x);
          entity.rot = new Vector3(entity.rot.x, entity.rot.y + 90f, entity.rot.z);
        }
      }
      MathUtils.Swap(ref this.TraderAreaProtect.x, ref this.TraderAreaProtect.z);
      foreach (KeyValuePair<string, List<Vector3i>> indexedBlockOffset in this.indexedBlockOffsets)
      {
        for (int index7 = 0; index7 < indexedBlockOffset.Value.Count; ++index7)
        {
          Vector3i _center = indexedBlockOffset.Value[index7];
          Prefab.RotatePointOnY(_bLeft, ref _center);
          indexedBlockOffset.Value[index7] = _center;
        }
      }
      if (this.bTraderArea)
      {
        for (int index8 = 0; index8 < this.TeleportVolumes.Count; ++index8)
        {
          Vector3i size = this.TeleportVolumes[index8].size;
          Vector3i vector3i1 = this.TeleportVolumes[index8].startPos;
          Vector3i vector3i2 = vector3i1 + size;
          if (_bLeft)
          {
            vector3i1 = new Vector3i(this.size.z - vector3i1.z, vector3i1.y, vector3i1.x);
            vector3i2 = new Vector3i(this.size.z - vector3i2.z, vector3i2.y, vector3i2.x);
          }
          else
          {
            vector3i1 = new Vector3i(vector3i1.z, vector3i1.y, this.size.x - vector3i1.x);
            vector3i2 = new Vector3i(vector3i2.z, vector3i2.y, this.size.x - vector3i2.x);
          }
          if (vector3i1.x > vector3i2.x)
            MathUtils.Swap(ref vector3i1.x, ref vector3i2.x);
          if (vector3i1.z > vector3i2.z)
            MathUtils.Swap(ref vector3i1.z, ref vector3i2.z);
          this.TeleportVolumes[index8].startPos = vector3i1;
          MathUtils.Swap(ref size.x, ref size.z);
          this.TeleportVolumes[index8].size = size;
        }
      }
      if (this.bSleeperVolumes)
      {
        for (int index9 = 0; index9 < this.SleeperVolumes.Count; ++index9)
        {
          Vector3i size = this.SleeperVolumes[index9].size;
          Vector3i vector3i3 = this.SleeperVolumes[index9].startPos;
          Vector3i vector3i4 = vector3i3 + size;
          if (_bLeft)
          {
            vector3i3 = new Vector3i(this.size.z - vector3i3.z, vector3i3.y, vector3i3.x);
            vector3i4 = new Vector3i(this.size.z - vector3i4.z, vector3i4.y, vector3i4.x);
          }
          else
          {
            vector3i3 = new Vector3i(vector3i3.z, vector3i3.y, this.size.x - vector3i3.x);
            vector3i4 = new Vector3i(vector3i4.z, vector3i4.y, this.size.x - vector3i4.x);
          }
          if (vector3i3.x > vector3i4.x)
            MathUtils.Swap(ref vector3i3.x, ref vector3i4.x);
          if (vector3i3.z > vector3i4.z)
            MathUtils.Swap(ref vector3i3.z, ref vector3i4.z);
          this.SleeperVolumes[index9].startPos = vector3i3;
          MathUtils.Swap(ref size.x, ref size.z);
          this.SleeperVolumes[index9].size = size;
        }
      }
      if (this.bInfoVolumes)
      {
        for (int index10 = 0; index10 < this.InfoVolumes.Count; ++index10)
        {
          Vector3i size = this.InfoVolumes[index10].size;
          Vector3i vector3i5 = this.InfoVolumes[index10].startPos;
          Vector3i vector3i6 = vector3i5 + size;
          if (_bLeft)
          {
            vector3i5 = new Vector3i(this.size.z - vector3i5.z, vector3i5.y, vector3i5.x);
            vector3i6 = new Vector3i(this.size.z - vector3i6.z, vector3i6.y, vector3i6.x);
          }
          else
          {
            vector3i5 = new Vector3i(vector3i5.z, vector3i5.y, this.size.x - vector3i5.x);
            vector3i6 = new Vector3i(vector3i6.z, vector3i6.y, this.size.x - vector3i6.x);
          }
          if (vector3i5.x > vector3i6.x)
            MathUtils.Swap(ref vector3i5.x, ref vector3i6.x);
          if (vector3i5.z > vector3i6.z)
            MathUtils.Swap(ref vector3i5.z, ref vector3i6.z);
          this.InfoVolumes[index10].startPos = vector3i5;
          MathUtils.Swap(ref size.x, ref size.z);
          this.InfoVolumes[index10].size = size;
        }
      }
      if (this.bWallVolumes)
      {
        for (int index11 = 0; index11 < this.WallVolumes.Count; ++index11)
        {
          Vector3i size = this.WallVolumes[index11].size;
          Vector3i vector3i7 = this.WallVolumes[index11].startPos;
          Vector3i vector3i8 = vector3i7 + size;
          if (_bLeft)
          {
            vector3i7 = new Vector3i(this.size.z - vector3i7.z, vector3i7.y, vector3i7.x);
            vector3i8 = new Vector3i(this.size.z - vector3i8.z, vector3i8.y, vector3i8.x);
          }
          else
          {
            vector3i7 = new Vector3i(vector3i7.z, vector3i7.y, this.size.x - vector3i7.x);
            vector3i8 = new Vector3i(vector3i8.z, vector3i8.y, this.size.x - vector3i8.x);
          }
          if (vector3i7.x > vector3i8.x)
            MathUtils.Swap(ref vector3i7.x, ref vector3i8.x);
          if (vector3i7.z > vector3i8.z)
            MathUtils.Swap(ref vector3i7.z, ref vector3i8.z);
          this.WallVolumes[index11].startPos = vector3i7;
          MathUtils.Swap(ref size.x, ref size.z);
          this.WallVolumes[index11].size = size;
        }
      }
      if (this.bTriggerVolumes)
      {
        for (int index12 = 0; index12 < this.TriggerVolumes.Count; ++index12)
        {
          Vector3i size = this.TriggerVolumes[index12].size;
          Vector3i vector3i9 = this.TriggerVolumes[index12].startPos;
          Vector3i vector3i10 = vector3i9 + size;
          if (_bLeft)
          {
            vector3i9 = new Vector3i(this.size.z - vector3i9.z, vector3i9.y, vector3i9.x);
            vector3i10 = new Vector3i(this.size.z - vector3i10.z, vector3i10.y, vector3i10.x);
          }
          else
          {
            vector3i9 = new Vector3i(vector3i9.z, vector3i9.y, this.size.x - vector3i9.x);
            vector3i10 = new Vector3i(vector3i10.z, vector3i10.y, this.size.x - vector3i10.x);
          }
          if (vector3i9.x > vector3i10.x)
            MathUtils.Swap(ref vector3i9.x, ref vector3i10.x);
          if (vector3i9.z > vector3i10.z)
            MathUtils.Swap(ref vector3i9.z, ref vector3i10.z);
          this.TriggerVolumes[index12].startPos = vector3i9;
          MathUtils.Swap(ref size.x, ref size.z);
          this.TriggerVolumes[index12].size = size;
        }
      }
      for (int index13 = 0; index13 < this.POIMarkers.Count; ++index13)
      {
        Vector3i size = this.POIMarkers[index13].Size;
        Vector3i vector3i11 = this.POIMarkers[index13].Start;
        Vector3i vector3i12 = vector3i11 + size;
        if (_bLeft)
        {
          vector3i11 = new Vector3i(this.size.z - vector3i11.z, vector3i11.y, vector3i11.x);
          vector3i12 = new Vector3i(this.size.z - vector3i12.z, vector3i12.y, vector3i12.x);
        }
        else
        {
          vector3i11 = new Vector3i(vector3i11.z, vector3i11.y, this.size.x - vector3i11.x);
          vector3i12 = new Vector3i(vector3i12.z, vector3i12.y, this.size.x - vector3i12.x);
        }
        if (vector3i11.x > vector3i12.x)
          MathUtils.Swap(ref vector3i11.x, ref vector3i12.x);
        if (vector3i11.z > vector3i12.z)
          MathUtils.Swap(ref vector3i11.z, ref vector3i12.z);
        this.POIMarkers[index13].Start = vector3i11;
        MathUtils.Swap(ref size.x, ref size.z);
        this.POIMarkers[index13].Size = size;
      }
      MathUtils.Swap(ref this.size.x, ref this.size.z);
    }
    if (!Block.BlocksLoaded)
      return;
    this.AddAllChildBlocks();
  }

  public void RotatePOIMarkers(bool _bLeft, int _rotCount)
  {
    Vector3i size1 = this.size;
    for (int index1 = 0; index1 < _rotCount; ++index1)
    {
      for (int index2 = 0; index2 < this.POIMarkers.Count; ++index2)
      {
        Vector3i size2 = this.POIMarkers[index2].Size;
        Vector3i vector3i1 = this.POIMarkers[index2].Start;
        Vector3i vector3i2 = vector3i1 + size2;
        if (_bLeft)
        {
          vector3i1 = new Vector3i(size1.z - vector3i1.z, vector3i1.y, vector3i1.x);
          vector3i2 = new Vector3i(size1.z - vector3i2.z, vector3i2.y, vector3i2.x);
        }
        else
        {
          vector3i1 = new Vector3i(vector3i1.z, vector3i1.y, size1.x - vector3i1.x);
          vector3i2 = new Vector3i(vector3i2.z, vector3i2.y, size1.x - vector3i2.x);
        }
        if (vector3i1.x > vector3i2.x)
          MathUtils.Swap(ref vector3i1.x, ref vector3i2.x);
        if (vector3i1.z > vector3i2.z)
          MathUtils.Swap(ref vector3i1.z, ref vector3i2.z);
        this.POIMarkers[index2].Start = vector3i1;
        MathUtils.Swap(ref size2.x, ref size2.z);
        this.POIMarkers[index2].Size = size2;
      }
      MathUtils.Swap(ref size1.x, ref size1.z);
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static void RotatePointOnY(bool _bLeft, ref Vector3i _center)
  {
    Vector3 vector3 = !_bLeft ? Quaternion.op_Multiply(Quaternion.AngleAxis(90f, Vector3.up), _center.ToVector3()) : Quaternion.op_Multiply(Quaternion.AngleAxis(-90f, Vector3.up), _center.ToVector3());
    _center = new Vector3i(Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y), Mathf.RoundToInt(vector3.z));
  }

  public void Replace(
    BlockValue _src,
    BlockValue _dst,
    bool _bConsiderRotation,
    int _considerPaintId1 = -1,
    int _considerPaintId2 = -1)
  {
    for (int _x = 0; _x < this.size.x; ++_x)
    {
      for (int _z = 0; _z < this.size.z; ++_z)
      {
        for (int _y = 0; _y < this.size.y; ++_y)
        {
          BlockValue block1 = this.GetBlock(_x, _y, _z);
          if (!block1.ischild && block1.type == _src.type && (!_bConsiderRotation || (int) block1.rotation == (int) _src.rotation) && (_considerPaintId1 == -1 || this.hasTexture(this.GetTexture(_x, _y, _z), _considerPaintId1)) && (_considerPaintId2 == -1 || this.hasTexture(this.GetTexture(_x, _y, _z), _considerPaintId2)))
          {
            BlockValue _bv = _dst;
            if (!_bConsiderRotation)
              _bv.rotation = block1.rotation;
            _bv.meta = _dst.meta != (byte) 0 ? _dst.meta : block1.meta;
            this.SetBlock(_x, _y, _z, _bv);
            bool flag1 = _src.Block.shape.IsTerrain();
            Block block2 = _dst.Block;
            bool flag2 = block2 != null ? block2.shape.IsTerrain() : flag1;
            if (flag1 != flag2)
            {
              sbyte _density = this.GetDensity(_x, _y, _z);
              if (flag2)
                _density = MarchingCubes.DensityTerrain;
              else if (_density != (sbyte) 0)
                _density = MarchingCubes.DensityAir;
              this.SetDensity(_x, _y, _z, _density);
            }
          }
        }
      }
    }
  }

  public void Replace(int _searchPaintId, int _replacePaintId, int _blockId = -1)
  {
    for (int _x = 0; _x < this.size.x; ++_x)
    {
      for (int _z = 0; _z < this.size.z; ++_z)
      {
        for (int _y = 0; _y < this.size.y; ++_y)
        {
          BlockValue block = this.GetBlock(_x, _y, _z);
          if (!block.ischild && this.hasTexture(this.GetTexture(_x, _y, _z), _searchPaintId) && (_blockId == -1 || _blockId == block.type))
          {
            TextureFullArray texture = this.GetTexture(_x, _y, _z);
            for (int index1 = 0; index1 < 1; ++index1)
            {
              for (int index2 = 0; index2 < 6; ++index2)
              {
                if ((texture[index1] >> index2 * 8 & (long) byte.MaxValue) == (long) _searchPaintId)
                {
                  texture[index1] &= ~((long) byte.MaxValue << index2 * 8);
                  texture[index1] |= (long) _replacePaintId << index2 * 8;
                }
              }
            }
            this.SetTexture(_x, _y, _z, texture);
          }
        }
      }
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public bool hasTexture(TextureFullArray fulltexture, int textureIdx)
  {
    for (int index1 = 0; index1 < 1; ++index1)
    {
      for (int index2 = 0; index2 < 6; ++index2)
      {
        if ((fulltexture[index1] >> index2 * 8 & (long) byte.MaxValue) == (long) textureIdx)
          return true;
      }
    }
    return false;
  }

  public int Search(
    BlockValue _src,
    bool _bConsiderRotation,
    int _considerPaintId1 = -1,
    int _considerPaintId2 = -1)
  {
    int num = 0;
    for (int _x = 0; _x < this.size.x; ++_x)
    {
      for (int _z = 0; _z < this.size.z; ++_z)
      {
        for (int _y = 0; _y < this.size.y; ++_y)
        {
          BlockValue block = this.GetBlock(_x, _y, _z);
          if (!block.ischild && block.type == _src.type && (!_bConsiderRotation || (int) block.rotation == (int) _src.rotation) && (_considerPaintId1 == -1 || this.hasTexture(this.GetTexture(_x, _y, _z), _considerPaintId1)) && (_considerPaintId2 == -1 || this.hasTexture(this.GetTexture(_x, _y, _z), _considerPaintId2)))
            ++num;
        }
      }
    }
    return num;
  }

  public int Search(int _paintId, int _blockId = -1)
  {
    int num = 0;
    for (int _x = 0; _x < this.size.x; ++_x)
    {
      for (int _z = 0; _z < this.size.z; ++_z)
      {
        for (int _y = 0; _y < this.size.y; ++_y)
        {
          BlockValue block = this.GetBlock(_x, _y, _z);
          if (!block.ischild && this.hasTexture(this.GetTexture(_x, _y, _z), _paintId) && (_blockId == -1 || _blockId == block.type))
            ++num;
        }
      }
    }
    return num;
  }

  public void CopyIntoRPC(GameManager _gm, Vector3i _destinationPos, bool _pasteAirBlocks = false)
  {
    List<BlockChangeInfo> _changes = new List<BlockChangeInfo>();
    NetPackageWaterSet package1 = NetPackageManager.GetPackage<NetPackageWaterSet>();
    if (_pasteAirBlocks)
      this.AddAllChildBlocks();
    if (this.bCopyAirBlocks)
    {
      NetPackageWaterSet package2 = NetPackageManager.GetPackage<NetPackageWaterSet>();
      for (int index1 = 0; index1 < this.size.y; ++index1)
      {
        for (int index2 = 0; index2 < this.size.x; ++index2)
        {
          for (int index3 = 0; index3 < this.size.z; ++index3)
          {
            int num1 = index2 + _destinationPos.x;
            int num2 = index1 + _destinationPos.y;
            int num3 = index3 + _destinationPos.z;
            if (!_gm.World.GetBlock(num1, num2, num3).isair)
              _changes.Add(new BlockChangeInfo(0, new Vector3i(num1, num2, num3), BlockValue.Air));
            if (_gm.World.GetWater(num1, num2, num3).HasMass())
              package2.AddChange(num1, num2, num3, WaterValue.Empty);
          }
        }
      }
      _gm.SetWaterRPC(package2);
    }
    Dictionary<Vector3i, TileEntity> dictionary1 = new Dictionary<Vector3i, TileEntity>();
    Dictionary<Vector3i, BlockTrigger> dictionary2 = new Dictionary<Vector3i, BlockTrigger>();
    for (int _y = 0; _y < this.size.y; ++_y)
    {
      for (int _x = 0; _x < this.size.x; ++_x)
      {
        for (int _z = 0; _z < this.size.z; ++_z)
        {
          WaterValue water = this.GetWater(_x, _y, _z);
          if (water.HasMass())
            package1.AddChange(_x + _destinationPos.x, _y + _destinationPos.y, _z + _destinationPos.z, water);
          BlockValue block1 = this.GetBlock(_x, _y, _z);
          Block block2 = block1.Block;
          if (block2 != null && !block1.isair | _pasteAirBlocks && (!_pasteAirBlocks || !block1.ischild))
          {
            TextureFullArray texture = this.GetTexture(_x, _y, _z);
            sbyte density = this.GetDensity(_x, _y, _z);
            _changes.Add(new BlockChangeInfo(0, new Vector3i(_x + _destinationPos.x, _y + _destinationPos.y, _z + _destinationPos.z), block1, density, texture));
            Vector3i vector3i;
            if (block2.IsTileEntitySavedInPrefab())
            {
              vector3i = new Vector3i(_x, _y, _z);
              TileEntity tileEntity;
              if ((tileEntity = this.GetTileEntity(vector3i)) != null)
                dictionary1.Add(vector3i, tileEntity);
            }
            vector3i = new Vector3i(_x, _y, _z);
            BlockTrigger blockTrigger;
            if ((blockTrigger = this.GetBlockTrigger(vector3i)) != null)
              dictionary2.Add(vector3i, blockTrigger);
          }
        }
      }
    }
    _gm.SetBlocksRPC(_changes, (PlatformUserIdentifierAbs) null);
    _gm.SetWaterRPC(package1);
    if (_pasteAirBlocks)
      this.AddAllChildBlocks();
    bool flag = this.PrefabName.StartsWith("part_");
    foreach (KeyValuePair<Vector3i, TileEntity> keyValuePair in dictionary1)
    {
      Vector3i vector3i = keyValuePair.Key + _destinationPos;
      TileEntity _te = _gm.World.GetTileEntity(vector3i);
      if (_te == null | flag)
      {
        Chunk chunkFromWorldPos = (Chunk) _gm.World.GetChunkFromWorldPos(vector3i);
        Vector3i block = World.toBlock(vector3i);
        if (flag)
          chunkFromWorldPos.RemoveTileEntityAt<TileEntity>(_gm.World, block);
        _te = keyValuePair.Value.Clone();
        _te.SetChunk(chunkFromWorldPos);
        _te.localChunkPos = block;
        chunkFromWorldPos.AddTileEntity(_te);
      }
      Vector3i localChunkPos = _te.localChunkPos;
      _te.CopyFrom(keyValuePair.Value);
      _te.localChunkPos = localChunkPos;
      _te.SetModified();
    }
    foreach (KeyValuePair<Vector3i, BlockTrigger> keyValuePair in dictionary2)
    {
      Vector3i vector3i = keyValuePair.Key + _destinationPos;
      BlockTrigger _td = _gm.World.GetBlockTrigger(0, vector3i);
      if (_td == null | flag)
      {
        Chunk chunkFromWorldPos = (Chunk) _gm.World.GetChunkFromWorldPos(vector3i);
        Vector3i block = World.toBlock(vector3i);
        if (flag)
          chunkFromWorldPos.RemoveTileEntityAt<TileEntity>(_gm.World, block);
        _td = keyValuePair.Value.Clone();
        _td.Chunk = chunkFromWorldPos;
        _td.LocalChunkPos = block;
        chunkFromWorldPos.AddBlockTrigger(_td);
      }
      Vector3i localChunkPos = _td.LocalChunkPos;
      _td.CopyFrom(keyValuePair.Value);
      _td.LocalChunkPos = localChunkPos;
    }
  }

  public void CountSleeperSpawnsInVolume(World _world, Vector3i _offset, int index)
  {
    this.Transient_NumSleeperSpawns = 0;
    Prefab.PrefabSleeperVolume sleeperVolume = this.SleeperVolumes[index];
    Vector3i startPos = sleeperVolume.startPos;
    Vector3i size = sleeperVolume.size;
    Vector3i vector3i1 = startPos + size;
    Vector3i vector3i2 = startPos + _offset;
    Vector3i vector3i3 = vector3i1 + _offset;
    int x1 = vector3i2.x;
    int y1 = vector3i2.y;
    int z1 = vector3i2.z;
    int x2 = vector3i3.x;
    int y2 = vector3i3.y;
    int z2 = vector3i3.z;
    for (int _z = z1; _z < z2; ++_z)
    {
      for (int _y = y1; _y < y2; ++_y)
      {
        for (int _x = x1; _x < x2; ++_x)
        {
          if (!_world.GetBlock(_x, _y - 1, _z).Block.IsSleeperBlock && _world.GetBlock(_x, _y, _z).Block.IsSleeperBlock && !this.IsPosInSleeperPriorityVolume(new Vector3i(_x - _offset.x, _y - _offset.y, _z - _offset.z), index))
            ++this.Transient_NumSleeperSpawns;
        }
      }
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void CopySleeperBlocksContainedInVolume(
    int volumeIndex,
    Vector3i _offset,
    SleeperVolume _volume,
    Vector3i _volumeMins,
    Vector3i _volumeMaxs)
  {
    int num1 = Mathf.Max(_volumeMins.x, 0);
    int num2 = Mathf.Max(_volumeMins.y, 0);
    int num3 = Mathf.Max(_volumeMins.z, 0);
    int num4 = Mathf.Min(this.size.x, _volumeMaxs.x);
    int num5 = Mathf.Min(this.size.y, _volumeMaxs.y);
    int num6 = Mathf.Min(this.size.z, _volumeMaxs.z);
    for (int _x1 = num1; _x1 < num4; ++_x1)
    {
      int _x2 = _x1 + _offset.x;
      for (int _z1 = num3; _z1 < num6; ++_z1)
      {
        int _z2 = _z1 + _offset.z;
        for (int _y1 = num2; _y1 < num5; ++_y1)
        {
          if (_y1 <= 0 || !this.GetBlockNoDamage(this.localRotation, _x1, _y1 - 1, _z1).Block.IsSleeperBlock)
          {
            BlockValue block1 = this.GetBlock(_x1, _y1, _z1);
            Block block2 = block1.Block;
            if (block2.IsSleeperBlock)
            {
              int _y2 = _y1 + _offset.y;
              if (!this.IsPosInSleeperPriorityVolume(new Vector3i(_x1, _y1, _z1), volumeIndex))
                _volume.AddSpawnPoint(_x2, _y2, _z2, (BlockSleeper) block2, block1);
            }
          }
        }
      }
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void CopySleeperVolumes(World _world, Chunk _chunk, Vector3i _offset)
  {
    Vector3i vector3i1 = Vector3i.zero;
    Vector3i vector3i2 = Vector3i.zero;
    if (_chunk != null)
    {
      vector3i1 = _chunk.GetWorldPos();
      vector3i2 = vector3i1 + new Vector3i(16 /*0x10*/, 256 /*0x0100*/, 16 /*0x10*/);
    }
    for (int index = 0; index < this.SleeperVolumes.Count; ++index)
    {
      Prefab.PrefabSleeperVolume sleeperVolume = this.SleeperVolumes[index];
      if (sleeperVolume.used)
      {
        Vector3i startPos = sleeperVolume.startPos;
        Vector3i _volumeMaxs = startPos + sleeperVolume.size;
        Vector3i vector3i3 = startPos + _offset;
        Vector3i vector3i4 = vector3i3 + sleeperVolume.size;
        Vector3i vector3i5 = vector3i3 - SleeperVolume.chunkPadding;
        Vector3i vector3i6 = vector3i4 + SleeperVolume.chunkPadding;
        if (_chunk != null)
        {
          if ((vector3i5.x >= vector3i2.x || vector3i6.x <= vector3i1.x || vector3i5.y >= vector3i2.y || vector3i6.y <= vector3i1.y || vector3i5.z >= vector3i2.z ? 0 : (vector3i6.z > vector3i1.z ? 1 : 0)) != 0)
          {
            int id = _world.FindSleeperVolume(vector3i3, vector3i4);
            if (id < 0)
            {
              SleeperVolume _volume = SleeperVolume.Create(sleeperVolume, vector3i3, vector3i4);
              id = _world.AddSleeperVolume(_volume);
              this.CopySleeperBlocksContainedInVolume(index, _offset, _volume, startPos, _volumeMaxs);
            }
            _chunk.AddSleeperVolumeId(id);
          }
        }
        else
        {
          int id = _world.FindSleeperVolume(vector3i3, vector3i4);
          if (id < 0)
          {
            SleeperVolume _volume = SleeperVolume.Create(sleeperVolume, vector3i3, vector3i4);
            id = _world.AddSleeperVolume(_volume);
            this.CopySleeperBlocksContainedInVolume(index, _offset, _volume, startPos, _volumeMaxs);
          }
          int chunkXz1 = World.toChunkXZ(vector3i5.x);
          int chunkXz2 = World.toChunkXZ(vector3i6.x - 1);
          int chunkXz3 = World.toChunkXZ(vector3i5.z);
          int chunkXz4 = World.toChunkXZ(vector3i6.z - 1);
          for (int chunkX = chunkXz1; chunkX <= chunkXz2; ++chunkX)
          {
            for (int chunkZ = chunkXz3; chunkZ <= chunkXz4; ++chunkZ)
              ((Chunk) _world.GetChunkSync(chunkX, chunkZ))?.AddSleeperVolumeId(id);
          }
        }
      }
    }
    for (int index = 0; index < this.TriggerVolumes.Count; ++index)
    {
      Prefab.PrefabTriggerVolume triggerVolume = this.TriggerVolumes[index];
      Vector3i startPos = triggerVolume.startPos;
      Vector3i vector3i7 = startPos + triggerVolume.size;
      Vector3i vector3i8 = startPos + _offset;
      Vector3i vector3i9 = vector3i8 + triggerVolume.size;
      Vector3i vector3i10 = vector3i8 - SleeperVolume.chunkPadding;
      Vector3i vector3i11 = vector3i9 + SleeperVolume.chunkPadding;
      if (_chunk != null)
      {
        if ((vector3i10.x >= vector3i2.x || vector3i11.x <= vector3i1.x || vector3i10.y >= vector3i2.y || vector3i11.y <= vector3i1.y || vector3i10.z >= vector3i2.z ? 0 : (vector3i11.z > vector3i1.z ? 1 : 0)) != 0)
        {
          int id = _world.FindTriggerVolume(vector3i8, vector3i9);
          if (id < 0)
          {
            TriggerVolume _volume = TriggerVolume.Create(triggerVolume, vector3i8, vector3i9);
            id = _world.AddTriggerVolume(_volume);
          }
          _chunk.AddTriggerVolumeId(id);
        }
      }
      else
      {
        int id = _world.FindTriggerVolume(vector3i8, vector3i9);
        if (id < 0)
        {
          TriggerVolume _volume = TriggerVolume.Create(triggerVolume, vector3i8, vector3i9);
          id = _world.AddTriggerVolume(_volume);
        }
        int chunkXz5 = World.toChunkXZ(vector3i10.x);
        int chunkXz6 = World.toChunkXZ(vector3i11.x - 1);
        int chunkXz7 = World.toChunkXZ(vector3i10.z);
        int chunkXz8 = World.toChunkXZ(vector3i11.z - 1);
        for (int chunkX = chunkXz5; chunkX <= chunkXz6; ++chunkX)
        {
          for (int chunkZ = chunkXz7; chunkZ <= chunkXz8; ++chunkZ)
            ((Chunk) _world.GetChunkSync(chunkX, chunkZ))?.AddTriggerVolumeId(id);
        }
      }
    }
    for (int index = 0; index < this.WallVolumes.Count; ++index)
    {
      Prefab.PrefabWallVolume wallVolume = this.WallVolumes[index];
      Vector3i startPos = wallVolume.startPos;
      Vector3i vector3i12 = startPos + wallVolume.size;
      Vector3i vector3i13 = startPos + _offset;
      Vector3i vector3i14 = vector3i13 + wallVolume.size;
      Vector3i vector3i15 = vector3i13;
      Vector3i vector3i16 = vector3i14;
      if (_chunk != null)
      {
        if ((vector3i15.x >= vector3i2.x || vector3i16.x <= vector3i1.x || vector3i15.y >= vector3i2.y || vector3i16.y <= vector3i1.y || vector3i15.z >= vector3i2.z ? 0 : (vector3i16.z > vector3i1.z ? 1 : 0)) != 0)
        {
          int id = _world.FindWallVolume(vector3i13, vector3i14);
          if (id < 0)
          {
            WallVolume _volume = WallVolume.Create(wallVolume, vector3i13, vector3i14);
            id = _world.AddWallVolume(_volume);
          }
          _chunk.AddWallVolumeId(id);
        }
      }
      else
      {
        int id = _world.FindWallVolume(vector3i13, vector3i14);
        if (id < 0)
        {
          WallVolume _volume = WallVolume.Create(wallVolume, vector3i13, vector3i14);
          id = _world.AddWallVolume(_volume);
        }
        int chunkXz9 = World.toChunkXZ(vector3i15.x);
        int chunkXz10 = World.toChunkXZ(vector3i16.x - 1);
        int chunkXz11 = World.toChunkXZ(vector3i15.z);
        int chunkXz12 = World.toChunkXZ(vector3i16.z - 1);
        for (int chunkX = chunkXz9; chunkX <= chunkXz10; ++chunkX)
        {
          for (int chunkZ = chunkXz11; chunkZ <= chunkXz12; ++chunkZ)
            ((Chunk) _world.GetChunkSync(chunkX, chunkZ))?.AddWallVolumeId(id);
        }
      }
    }
  }

  public Prefab.PrefabSleeperVolume FindSleeperVolume(Vector3i _pos)
  {
    Prefab.PrefabSleeperVolume sleeperVolume1 = (Prefab.PrefabSleeperVolume) null;
    for (int index = 0; index < this.SleeperVolumes.Count; ++index)
    {
      Prefab.PrefabSleeperVolume sleeperVolume2 = this.SleeperVolumes[index];
      if (sleeperVolume2.used && this.IsPosInSleeperVolume(sleeperVolume2, _pos))
      {
        sleeperVolume1 = sleeperVolume2;
        if (sleeperVolume2.isPriority)
          break;
      }
    }
    return sleeperVolume1;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public bool IsPosInSleeperPriorityVolume(Vector3i _pos, int skipIndex)
  {
    for (int index = 0; index < this.SleeperVolumes.Count; ++index)
    {
      if (index != skipIndex)
      {
        Prefab.PrefabSleeperVolume sleeperVolume = this.SleeperVolumes[index];
        if (sleeperVolume.used && sleeperVolume.isPriority && this.IsPosInSleeperVolume(sleeperVolume, _pos))
          return true;
      }
    }
    return false;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public bool IsPosInSleeperVolume(Prefab.PrefabSleeperVolume volume, Vector3i _pos)
  {
    if (volume.used)
    {
      Vector3i startPos = volume.startPos;
      Vector3i vector3i = startPos + volume.size;
      if (_pos.x >= startPos.x && _pos.x < vector3i.x && _pos.y >= startPos.y && _pos.y < vector3i.y && _pos.z >= startPos.z && _pos.z < vector3i.z)
        return true;
    }
    return false;
  }

  public void MoveVolumes(Vector3i moveDistance)
  {
    for (int index = 0; index < this.SleeperVolumes.Count; ++index)
      this.SleeperVolumes[index].startPos += moveDistance;
    for (int index = 0; index < this.TeleportVolumes.Count; ++index)
      this.TeleportVolumes[index].startPos += moveDistance;
    for (int index = 0; index < this.TriggerVolumes.Count; ++index)
      this.TriggerVolumes[index].startPos += moveDistance;
    for (int index = 0; index < this.InfoVolumes.Count; ++index)
      this.InfoVolumes[index].startPos += moveDistance;
    for (int index = 0; index < this.WallVolumes.Count; ++index)
      this.WallVolumes[index].startPos += moveDistance;
  }

  public static void TransientSleeperBlockIncrement(Vector3i point, int c)
  {
    if (XUiC_WoPropsSleeperVolume.selectedVolumeIndex < 0)
      return;
    PrefabInstance selectedPrefabInstance = XUiC_WoPropsSleeperVolume.selectedPrefabInstance;
    Prefab prefab = selectedPrefabInstance.prefab;
    if (XUiC_WoPropsSleeperVolume.selectedVolumeIndex >= prefab.SleeperVolumes.Count || !prefab.IsPosInSleeperVolume(prefab.SleeperVolumes[XUiC_WoPropsSleeperVolume.selectedVolumeIndex], point - selectedPrefabInstance.boundingBoxPosition))
      return;
    prefab.Transient_NumSleeperSpawns += c;
  }

  public string CalcSleeperInfo()
  {
    int num1 = 0;
    int num2 = 0;
    bool flag = false;
    for (int index = 0; index < this.SleeperVolumes.Count; ++index)
    {
      Prefab.PrefabSleeperVolume sleeperVolume = this.SleeperVolumes[index];
      int spawnCountMin = (int) sleeperVolume.spawnCountMin;
      if (spawnCountMin < 0)
        flag = true;
      else
        num1 += spawnCountMin;
      int spawnCountMax = (int) sleeperVolume.spawnCountMax;
      if (spawnCountMax < 0)
        flag = true;
      else
        num2 += spawnCountMax;
    }
    string str = $"{this.SleeperVolumes.Count}, {num1}-{num2}";
    if (flag)
      str += "*";
    return str;
  }

  public void CopyIntoLocal(
    ChunkCluster _cluster,
    Vector3i _destinationPos,
    bool _bOverwriteExistingBlocks,
    bool _bSetChunkToRegenerate,
    FastTags<TagGroup.Global> _questTags)
  {
    World world = _cluster.GetWorld();
    bool flag1 = world.IsEditor();
    if (!flag1)
      this.CopySleeperVolumes(world, (Chunk) null, _destinationPos);
    Chunk chunkSync = _cluster.GetChunkSync(World.toChunkXZ(_destinationPos.x), World.toChunkXZ(_destinationPos.z));
    int seed = world.Seed;
    GameRandom _gameRandom = chunkSync != null ? Utils.RandomFromSeedOnPos(chunkSync.X, chunkSync.Z, seed) : (GameRandom) null;
    GameRandom gameRandom = GameRandomManager.Instance.CreateGameRandom((int) world.GetWorldTime());
    if (this.terrainFillerType == 0)
      this.InitTerrainFillers();
    for (int _v1 = this.size.y + _destinationPos.y; _v1 < (int) byte.MaxValue; ++_v1)
    {
      int blockY = World.toBlockY(_v1);
      bool flag2 = false;
      for (int index1 = 0; index1 < this.size.z; ++index1)
      {
        int _v2 = index1 + _destinationPos.z;
        int chunkXz1 = World.toChunkXZ(_v2);
        int blockXz1 = World.toBlockXZ(_v2);
        for (int index2 = 0; index2 < this.size.x; ++index2)
        {
          int _v3 = index2 + _destinationPos.x;
          int chunkXz2 = World.toChunkXZ(_v3);
          int blockXz2 = World.toBlockXZ(_v3);
          if (chunkSync == null || chunkSync.X != chunkXz2 || chunkSync.Z != chunkXz1)
          {
            chunkSync = _cluster.GetChunkSync(chunkXz2, chunkXz1);
            if (chunkSync == null)
              continue;
          }
          BlockValue block = chunkSync.GetBlock(blockXz2, blockY, blockXz1);
          if (!block.isair && !block.Block.shape.IsTerrain())
          {
            flag2 = true;
            if (!block.ischild)
              chunkSync.SetBlock((WorldBase) world, blockXz2, blockY, blockXz1, BlockValue.Air, _poiOwned: true);
          }
        }
      }
      if (!flag2)
        break;
    }
    if (_bOverwriteExistingBlocks)
    {
      for (int index3 = 0; index3 < this.size.z; ++index3)
      {
        int _v4 = index3 + _destinationPos.z;
        int chunkXz3 = World.toChunkXZ(_v4);
        int blockXz3 = World.toBlockXZ(_v4);
        for (int index4 = 0; index4 < this.size.x; ++index4)
        {
          int _v5 = index4 + _destinationPos.x;
          int chunkXz4 = World.toChunkXZ(_v5);
          int blockXz4 = World.toBlockXZ(_v5);
          if (chunkSync == null || chunkSync.X != chunkXz4 || chunkSync.Z != chunkXz3)
          {
            chunkSync = _cluster.GetChunkSync(chunkXz4, chunkXz3);
            if (chunkSync == null)
            {
              Debug.LogError((object) $"Chunk ({chunkXz4}, {chunkXz3}) unavailable during POI reset. Skipping reset for all POI blocks at XZ world position ({_v5},{_v4}).");
              continue;
            }
          }
          for (int index5 = 0; index5 < this.size.y; ++index5)
          {
            int blockY = World.toBlockY(index5 + _destinationPos.y);
            BlockValue block = chunkSync.GetBlock(blockXz4, blockY, blockXz3);
            if (block.Block.isMultiBlock && !block.ischild)
              chunkSync.SetBlock((WorldBase) world, blockXz4, blockY, blockXz3, BlockValue.Air);
          }
        }
      }
      foreach (int blockParentIndex in this.multiBlockParentIndices)
      {
        int _x;
        int _y;
        int _z;
        this.offsetToCoordRotated(blockParentIndex, out _x, out _y, out _z);
        MultiBlockManager.Instance.DeregisterTrackedBlockData(new Vector3i(_x, _y, _z) + _destinationPos);
      }
    }
    for (int _z = 0; _z < this.size.z; ++_z)
    {
      int _v6 = _z + _destinationPos.z;
      int chunkXz5 = World.toChunkXZ(_v6);
      int blockXz5 = World.toBlockXZ(_v6);
      for (int _x = 0; _x < this.size.x; ++_x)
      {
        int _v7 = _x + _destinationPos.x;
        int chunkXz6 = World.toChunkXZ(_v7);
        int blockXz6 = World.toBlockXZ(_v7);
        if (chunkSync == null || chunkSync.X != chunkXz6 || chunkSync.Z != chunkXz5)
        {
          chunkSync = _cluster.GetChunkSync(chunkXz6, chunkXz5);
          GameRandomManager.Instance.FreeGameRandom(_gameRandom);
          _gameRandom = (GameRandom) null;
          if (chunkSync == null)
          {
            Debug.LogError((object) $"Chunk ({chunkXz6}, {chunkXz5}) unavailable during POI reset. Skipping reset for all POI blocks at XZ world position ({_v7},{_v6}).");
            continue;
          }
        }
        if (_gameRandom == null)
          _gameRandom = Utils.RandomFromSeedOnPos(chunkXz6, chunkXz5, seed);
        int _h = -1;
        bool flag3 = false;
        for (int _y = 0; _y < this.size.y; ++_y)
        {
          WaterValue water = this.GetWater(_x, _y, _z);
          BlockValue targetBV = this.GetBlock(_x, _y, _z);
          if (this.bCopyAirBlocks || !targetBV.isair || water.HasMass())
          {
            int blockY = World.toBlockY(_y + _destinationPos.y);
            BlockValue block1 = chunkSync.GetBlock(blockXz6, blockY, blockXz5);
            BlockValue blockValue1 = targetBV;
            sbyte _density = this.GetDensity(_x, _y, _z);
            bool flag4 = false;
            if (!targetBV.isair && !flag1)
            {
              if (targetBV.Block.IsSleeperBlock)
              {
                flag4 = true;
                targetBV = BlockValue.Air;
              }
              if (targetBV.type == this.terrainFillerType)
              {
                BlockValue blockValue2 = block1;
                Block block2 = blockValue2.Block;
                if (blockValue2.isair || block2 == null || !block2.shape.IsTerrain())
                {
                  int terrainHeight = (int) chunkSync.GetTerrainHeight(blockXz6, blockXz5);
                  blockValue2 = chunkSync.GetBlock(blockXz6, terrainHeight, blockXz5);
                  Block block3 = blockValue2.Block;
                  if (blockValue2.isair || block3 == null || !block3.shape.IsTerrain())
                    continue;
                }
                targetBV = blockValue2;
                flag3 = true;
              }
              if (targetBV.type == this.terrainFiller2Type)
              {
                Block block4 = block1.Block;
                if (!block1.isair && block4 != null && block4.shape.IsTerrain())
                {
                  targetBV = block1;
                  _density = (sbyte) 0;
                }
                else
                {
                  targetBV = BlockValue.Air;
                  _density = MarchingCubes.DensityAir;
                }
              }
              if (targetBV.Block.isMultiBlock && MultiBlockManager.Instance.POIMBTrackingEnabled)
                this.ProcessMultiBlock(ref targetBV, chunkSync, new Vector3i(_x, _y, _z), new Vector3i(blockXz6, blockY, blockXz5), _questTags, _bOverwriteExistingBlocks);
              else if (BlockPlaceholderMap.Instance.IsReplaceableBlockType(targetBV))
              {
                byte meta = targetBV.meta;
                targetBV = BlockPlaceholderMap.Instance.Replace(targetBV, GameManager.Instance.World.GetGameRandom(), chunkSync, blockXz6, blockY, blockXz5, _questTags, _bOverwriteExistingBlocks) with
                {
                  meta = meta
                };
              }
            }
            if (_density == (sbyte) 0)
            {
              _density = MarchingCubes.DensityAir;
              if (targetBV.Block.shape.IsTerrain())
                _density = MarchingCubes.DensityTerrain;
            }
            if (block1.ischild || !_bOverwriteExistingBlocks && !block1.isair && !block1.Block.shape.IsTerrain())
            {
              chunkSync.SetDensity(blockXz6, blockY, blockXz5, _density);
            }
            else
            {
              chunkSync.SetDecoAllowedSizeAt(blockXz6, blockXz5, EnumDecoAllowedSize.NoBigOnlySmall);
              if (!flag4)
              {
                TextureFullArray texture = this.GetTexture(_x, _y, _z);
                chunkSync.GetSetTextureFullArray(blockXz6, blockY, blockXz5, texture);
              }
              chunkSync.SetBlock((WorldBase) world, blockXz6, blockY, blockXz5, targetBV, _fromReset: !_questTags.IsEmpty, _poiOwned: true);
              chunkSync.SetWater(blockXz6, blockY, blockXz5, water);
              Vector3i _blockPos = new Vector3i(_x, _y, _z);
              TileEntity tileEntity;
              if (blockValue1.Block.IsTileEntitySavedInPrefab() && (tileEntity = this.GetTileEntity(_blockPos)) != null)
              {
                Vector3i _blockPosInChunk = new Vector3i(blockXz6, blockY, blockXz5);
                TileEntity _te = chunkSync.GetTileEntity(_blockPosInChunk);
                if (_te == null)
                {
                  _te = tileEntity.Clone();
                  _te.localChunkPos = _blockPosInChunk;
                  _te.SetChunk(chunkSync);
                  chunkSync.AddTileEntity(_te);
                }
                _te.CopyFrom(tileEntity);
                _te.localChunkPos = _blockPosInChunk;
              }
              BlockTrigger blockTrigger1 = this.GetBlockTrigger(_blockPos);
              if (blockTrigger1 != null)
              {
                BlockTrigger blockTrigger2 = chunkSync.GetBlockTrigger(new Vector3i(blockXz6, blockY, blockXz5));
                if (blockTrigger2 == null)
                {
                  blockTrigger2 = blockTrigger1.Clone();
                  blockTrigger2.LocalChunkPos = new Vector3i(blockXz6, blockY, blockXz5);
                  blockTrigger2.Chunk = chunkSync;
                  chunkSync.AddBlockTrigger(blockTrigger2);
                }
                blockTrigger2.CopyFrom(blockTrigger1);
                blockTrigger2.LocalChunkPos = new Vector3i(blockXz6, blockY, blockXz5);
                targetBV.Block.OnTriggerAddedFromPrefab(blockTrigger2, blockTrigger2.LocalChunkPos, targetBV, FastTags<TagGroup.Global>.Parse(this.questTags.ToString()));
              }
              if (targetBV.Block.shape.IsTerrain())
                _h = blockY;
              chunkSync.SetDensity(blockXz6, blockY, blockXz5, _density);
            }
          }
        }
        if (_h >= 0)
          chunkSync.SetTerrainHeight(blockXz6, blockXz5, (byte) _h);
        if (!flag3)
          chunkSync.SetTopSoilBroken(blockXz6, blockXz5);
        chunkSync.SetDecoAllowedSizeAt(blockXz6, blockXz5, EnumDecoAllowedSize.NoBigOnlySmall);
        if (_bSetChunkToRegenerate)
          chunkSync.NeedsRegeneration = true;
      }
    }
    this.ApplyDecoAllowed(_cluster, _destinationPos);
    GameRandomManager.Instance.FreeGameRandom(_gameRandom);
    GameRandomManager.Instance.FreeGameRandom(gameRandom);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void ApplyDecoAllowed(ChunkCluster _cluster, Vector3i _prefabTargetPos)
  {
    int chunkXz1 = World.toChunkXZ(_prefabTargetPos.x);
    int chunkXz2 = World.toChunkXZ(_prefabTargetPos.z);
    int chunkXz3 = World.toChunkXZ(_prefabTargetPos.x + this.size.x - 1);
    int chunkXz4 = World.toChunkXZ(_prefabTargetPos.z + this.size.z - 1);
    for (int _y = chunkXz2; _y <= chunkXz4; ++_y)
    {
      for (int _x = chunkXz1; _x <= chunkXz3; ++_x)
      {
        Chunk chunkSync = _cluster.GetChunkSync(_x, _y);
        if (chunkSync != null)
          this.ApplyDecoAllowed(chunkSync, _prefabTargetPos);
      }
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void ProcessMultiBlock(
    ref BlockValue targetBV,
    Chunk chunk,
    Vector3i prefabRelPos,
    Vector3i chunkRelPos,
    FastTags<TagGroup.Global> questTags,
    bool overwriteExistingBlocks)
  {
    if (!targetBV.Block.isMultiBlock)
    {
      Debug.LogError((object) "[MultiBlockManager] BlockValue passed into ProcessMultiBlock is not a MultiBlock.");
    }
    else
    {
      Vector3i vector3i1 = chunk.GetWorldPos() + (chunkRelPos - prefabRelPos);
      Vector3i vector3i2 = prefabRelPos;
      if (targetBV.ischild)
        vector3i2 += new Vector3i(targetBV.parentx, targetBV.parenty, targetBV.parentz);
      Vector3i vector3i3 = vector3i1 + vector3i2;
      MultiBlockManager.TrackedBlockData poiMultiBlock;
      BlockValue blockValue;
      if (MultiBlockManager.Instance.TryGetPOIMultiBlock(vector3i3, out poiMultiBlock))
      {
        blockValue = new BlockValue(poiMultiBlock.rawData);
      }
      else
      {
        BlockValue _blockValue = !targetBV.ischild ? targetBV : this.GetBlock(vector3i2.x, vector3i2.y, vector3i2.z);
        if (BlockPlaceholderMap.Instance.IsReplaceableBlockType(_blockValue))
        {
          byte meta = _blockValue.meta;
          blockValue = BlockPlaceholderMap.Instance.Replace(_blockValue, GameManager.Instance.World.GetGameRandom(), chunk, chunkRelPos.x, chunkRelPos.y, chunkRelPos.z, questTags, overwriteExistingBlocks, false);
          if (blockValue.isair)
            blockValue = BlockValue.Air;
          else
            blockValue.meta = meta;
        }
        else
          blockValue = _blockValue;
        MultiBlockManager.Instance.DeregisterTrackedBlockData(vector3i3);
        if (!MultiBlockManager.Instance.TryRegisterPOIMultiBlock(vector3i3, blockValue))
          Debug.LogError((object) "[MultiBlockManager] Failed to register POI MultiBlock.");
      }
      if (blockValue.type == targetBV.type)
        return;
      if (blockValue.isair)
        targetBV = BlockValue.Air;
      else if (!blockValue.Block.isMultiBlock)
      {
        if (targetBV.ischild)
        {
          targetBV = BlockValue.Air;
        }
        else
        {
          targetBV.type = blockValue.type;
          targetBV.rotation = blockValue.rotation;
        }
      }
      else
      {
        Vector3i dim1 = targetBV.Block.multiBlockPos.dim;
        Vector3i dim2 = blockValue.Block.multiBlockPos.dim;
        if (dim2 != dim1)
        {
          if (dim2.x > dim1.x || dim2.y > dim1.y || dim2.z > dim1.z)
            Debug.LogWarning((object) $"[MultiBlockManager] The replacement block \"{blockValue.Block.GetBlockName()}\" is larger than the original block \"{targetBV.Block.GetBlockName()}\" in dimensions. \n{$"Replacement size: \"{dim2}\", Original size: \"{dim1}\". "}{$"Parent world position: {vector3i3}.\n"}Child blocks of the replacement will not be placed outside the original block's dimensions. \nNote: We expect to see this warning when single-block helpers are used to place MultiBlocks at 45-degree rotations. Many of these instances will be resolved by converting to the new oversized block format in the near future. \nIn situations where 45-degree rotations aren't needed, helper blocks should be set to the maximum dimensions of any possible replacements. Affected prefabs may need to be re-saved to implement these changes.");
          if (dim2.x < dim1.x || dim2.y < dim1.y || dim2.z < dim1.z)
          {
            Vector3i minPos;
            Vector3i maxPos;
            MultiBlockManager.GetMinMaxWorldPositions(vector3i3, blockValue, out minPos, out maxPos);
            Vector3i vector3i4 = vector3i1 + prefabRelPos;
            if (vector3i4.x < minPos.x || vector3i4.x > maxPos.x || vector3i4.y < minPos.y || vector3i4.y > maxPos.y || vector3i4.z < minPos.z || vector3i4.z > maxPos.z)
            {
              targetBV = BlockValue.Air;
              return;
            }
          }
        }
        targetBV.type = blockValue.type;
        if (targetBV.ischild)
          return;
        targetBV.rotation = blockValue.rotation;
      }
    }
  }

  public void SnapTerrainToArea(ChunkCluster _cluster, Vector3i _destinationPos)
  {
    for (int index1 = -1; index1 < this.size.x + 1; ++index1)
    {
      for (int index2 = -1; index2 < this.size.z + 1; ++index2)
      {
        bool _bUseHalfTerrainDensity = index1 == -1 || index2 == -1 || index1 == this.size.x || index2 == this.size.z;
        _cluster.SnapTerrainToPositionAtLocal(new Vector3i(_destinationPos.x + index1, _destinationPos.y - 1, _destinationPos.z + index2), true, _bUseHalfTerrainDensity);
      }
    }
  }

  public void CopyEntitiesIntoWorld(
    World _world,
    Vector3i _destinationPos,
    ICollection<int> _entityIds,
    bool _bSpawnEnemies)
  {
    _entityIds?.Clear();
    for (int index = 0; index < this.entities.Count; ++index)
    {
      EntityCreationData entity1 = this.entities[index];
      entity1.id = -1;
      if (_bSpawnEnemies || !EntityClass.list[entity1.entityClass].bIsEnemyEntity)
      {
        Entity entity2 = EntityFactory.CreateEntity(entity1);
        entity2.SetPosition(Vector3.op_Addition(entity2.position, _destinationPos.ToVector3()));
        _world.SpawnEntityInWorld(entity2);
        _entityIds?.Add(entity2.entityId);
      }
    }
  }

  public void CopyEntitiesIntoChunkStub(
    Chunk _chunk,
    Vector3i _destinationPos,
    ICollection<int> _entityIds,
    bool _bSpawnEnemies)
  {
    for (int index = 0; index < this.entities.Count; ++index)
    {
      EntityCreationData entity = this.entities[index];
      if (EntityClass.list.ContainsKey(entity.entityClass) && (_bSpawnEnemies || !EntityClass.list[entity.entityClass].bIsEnemyEntity))
      {
        int _v1 = Utils.Fastfloor(entity.pos.x) + _destinationPos.x;
        int _v2 = Utils.Fastfloor(entity.pos.z) + _destinationPos.z;
        if (_chunk.X == World.toChunkXZ(_v1) && _chunk.Z == World.toChunkXZ(_v2))
        {
          EntityCreationData _ecd = entity.Clone();
          EntityCreationData entityCreationData = _ecd;
          entityCreationData.pos = Vector3.op_Addition(entityCreationData.pos, Vector3.op_Addition(_destinationPos.ToVector3(), new Vector3(0.0f, 0.25f, 0.0f)));
          _ecd.id = EntityFactory.nextEntityID++;
          if (_ecd.lootContainer != null)
            _ecd.lootContainer.entityId = _ecd.id;
          _chunk.AddEntityStub(_ecd);
          _entityIds?.Add(_ecd.id);
        }
      }
    }
  }

  public static Vector3i SizeFromPositions(Vector3i _posStart, Vector3i _posEnd)
  {
    Vector3i vector3i1 = new Vector3i(Math.Min(_posStart.x, _posEnd.x), Math.Min(_posStart.y, _posEnd.y), Math.Min(_posStart.z, _posEnd.z));
    Vector3i vector3i2 = new Vector3i(Math.Max(_posStart.x, _posEnd.x), Math.Max(_posStart.y, _posEnd.y), Math.Max(_posStart.z, _posEnd.z));
    return new Vector3i(Math.Abs(vector3i2.x - vector3i1.x) + 1, Math.Abs(vector3i2.y - vector3i1.y) + 1, Math.Abs(vector3i2.z - vector3i1.z) + 1);
  }

  public Vector3i copyFromWorld(World _world, Vector3i _posStart, Vector3i _posEnd)
  {
    Vector3i vector3i1 = Vector3i.Min(_posStart, _posEnd);
    Vector3i vector3i2 = Vector3i.Max(_posStart, _posEnd);
    this.size.x = Math.Abs(vector3i2.x - vector3i1.x) + 1;
    this.size.y = Math.Abs(vector3i2.y - vector3i1.y) + 1;
    this.size.z = Math.Abs(vector3i2.z - vector3i1.z) + 1;
    this.localRotation = 0;
    this.InitData();
    this.tileEntities.Clear();
    int _y = 0;
    int y = vector3i1.y;
    while (y <= vector3i2.y)
    {
      int _x = 0;
      int x = vector3i1.x;
      while (x <= vector3i2.x)
      {
        int _z = 0;
        int z = vector3i1.z;
        while (z <= vector3i2.z)
        {
          BlockValue _bv = _world.GetBlock(x, y, z);
          if (_bv.isWater)
          {
            this.SetWater(x, y, z, WaterValue.Full);
            _bv = BlockValue.Air;
          }
          this.SetDensity(_x, _y, _z, _world.GetDensity(0, x, y, z));
          if (!_bv.ischild)
          {
            this.SetBlock(_x, _y, _z, _bv);
            this.SetWater(_x, _y, _z, _world.GetWater(x, y, z));
            this.SetTexture(_x, _y, _z, _world.GetTextureFullArray(x, y, z));
            if (_bv.Block.IsTileEntitySavedInPrefab())
            {
              Vector3i _blockPos = new Vector3i(x, y, z);
              TileEntity tileEntity1 = _world.GetTileEntity(_blockPos);
              if (tileEntity1 != null)
              {
                TileEntity tileEntity2 = tileEntity1.Clone();
                tileEntity2.localChunkPos = _blockPos - vector3i1;
                this.tileEntities.Add(tileEntity2.localChunkPos, tileEntity2);
              }
            }
          }
          ++z;
          ++_z;
        }
        ++x;
        ++_x;
      }
      ++y;
      ++_y;
    }
    return vector3i1;
  }

  public Vector3i CopyFromWorldWithEntities(
    World _world,
    Vector3i _posStart,
    Vector3i _posEnd,
    ICollection<int> _entityIds)
  {
    this.copyFromWorld(_world, _posStart, _posEnd);
    Vector3i vector3i1 = Vector3i.Min(_posStart, _posEnd);
    Vector3i vector3i2 = Vector3i.Max(_posStart, _posEnd);
    this.entities.Clear();
    int chunkXz1 = World.toChunkXZ(vector3i1.x);
    int chunkXz2 = World.toChunkXZ(vector3i1.z);
    int chunkXz3 = World.toChunkXZ(vector3i2.x);
    int chunkXz4 = World.toChunkXZ(vector3i2.z);
    Bounds _bb = BoundsUtils.BoundsForMinMax((float) vector3i1.x, (float) vector3i1.y, (float) vector3i1.z, (float) (vector3i2.x + 1), (float) (vector3i2.y + 1), (float) (vector3i2.z + 1));
    List<Entity> _list = new List<Entity>();
    for (int chunkX = chunkXz1; chunkX <= chunkXz3; ++chunkX)
    {
      for (int chunkZ = chunkXz2; chunkZ <= chunkXz4; ++chunkZ)
        ((Chunk) _world.GetChunkSync(chunkX, chunkZ))?.GetEntitiesInBounds(typeof (Entity), _bb, _list);
    }
    this.indexedBlockOffsets.Clear();
    this.triggerData.Clear();
    for (int chunkX = chunkXz1; chunkX <= chunkXz3; ++chunkX)
    {
      for (int chunkZ = chunkXz2; chunkZ <= chunkXz4; ++chunkZ)
      {
        Chunk chunkSync = (Chunk) _world.GetChunkSync(chunkX, chunkZ);
        if (chunkSync != null)
        {
          foreach (KeyValuePair<string, List<Vector3i>> keyValuePair in chunkSync.IndexedBlocks.Dict)
          {
            if (keyValuePair.Value != null && keyValuePair.Value.Count > 0)
            {
              List<Vector3i> vector3iList = new List<Vector3i>();
              this.indexedBlockOffsets[keyValuePair.Key] = vector3iList;
              foreach (Vector3i _pos in keyValuePair.Value)
              {
                Vector3i worldPos = chunkSync.ToWorldPos(_pos);
                Vector3 vector3 = worldPos.ToVector3();
                Vector3i vector3i3 = worldPos - vector3i1;
                if (((Bounds) ref _bb).Contains(vector3))
                  vector3iList.Add(vector3i3);
              }
            }
          }
          List<BlockTrigger> list = chunkSync.GetBlockTriggers().list;
          for (int index = 0; index < list.Count; ++index)
          {
            BlockTrigger blockTrigger = list[index].Clone();
            blockTrigger.LocalChunkPos = chunkSync.ToWorldPos(list[index].LocalChunkPos) - _posStart;
            this.triggerData.Add(blockTrigger.LocalChunkPos, blockTrigger);
          }
        }
      }
    }
    _entityIds?.Clear();
    for (int index = 0; index < _list.Count; ++index)
    {
      Entity _e = _list[index];
      if (!(_e is EntityPlayer))
      {
        EntityCreationData entityCreationData1 = new EntityCreationData(_e);
        EntityCreationData entityCreationData2 = entityCreationData1;
        entityCreationData2.pos = Vector3.op_Subtraction(entityCreationData2.pos, new Vector3(((Bounds) ref _bb).min.x, ((Bounds) ref _bb).min.y, ((Bounds) ref _bb).min.z));
        this.entities.Add(entityCreationData1);
        _entityIds?.Add(_e.entityId);
      }
    }
    return vector3i1;
  }

  public BlockValue Get(int relx, int absy, int relz)
  {
    int _x = this.currX + relx;
    int _y = absy;
    int _z = this.currZ + relz;
    return _x >= 0 && _x < this.size.x && _y >= 0 && _y < this.size.y && _z >= 0 && _z < this.size.z ? this.GetBlock(_x, _y, _z) : BlockValue.Air;
  }

  public IChunk GetChunk(int x, int z)
  {
    long key = WorldChunkCache.MakeChunkKey(x, z);
    Prefab.PrefabChunk chunk;
    if (!this.dictChunks.TryGetValue(key, out chunk))
    {
      chunk = new Prefab.PrefabChunk(this, x, z);
      this.dictChunks.Add(key, chunk);
    }
    return (IChunk) chunk;
  }

  public List<IChunk> GetChunks()
  {
    if (this.dictChunks.Count == 0)
    {
      int num1 = 0;
      int x = 0;
      while (num1 < this.size.x + 1)
      {
        int num2 = 0;
        int z = 0;
        while (num2 < this.size.z + 1)
        {
          this.GetChunk(x, z);
          num2 += 16 /*0x10*/;
          ++z;
        }
        num1 += 16 /*0x10*/;
        ++x;
      }
    }
    return ((IEnumerable<IChunk>) this.dictChunks.Values).ToList<IChunk>();
  }

  public IChunk GetNeighborChunk(int x, int z) => this.GetChunk(x, z);

  public bool IsWater(int relx, int absy, int relz)
  {
    int _x = this.currX + relx;
    int _z = this.currZ + relz;
    int _y = absy;
    return _x >= 0 && _x < this.size.x && _y >= 0 && _y < this.size.y && _z >= 0 && _z < this.size.z && this.GetWater(_x, _y, _z).HasMass();
  }

  public bool IsAir(int relx, int absy, int relz)
  {
    int _x = this.currX + relx;
    int _z = this.currZ + relz;
    int _y = absy;
    return _x >= 0 && _x < this.size.x && _y >= 0 && _y < this.size.y && _z >= 0 && _z < this.size.z && this.GetBlock(_x, _y, _z).isair && !this.GetWater(_x, _y, _z).HasMass();
  }

  public void Init(int _bX, int _bZ)
  {
    this.currX = _bX;
    this.currZ = _bZ;
    this.dictChunks = new Dictionary<long, Prefab.PrefabChunk>();
  }

  public void Clear()
  {
  }

  public void Cache()
  {
  }

  public void ToMesh(VoxelMesh[] _meshes)
  {
    new MeshGenerator((INeighborBlockCache) this).GenerateMesh(new Vector3i(-1, -1, -1), this.size + Vector3i.one, _meshes);
  }

  public void ToOptimizedColorCubeMesh(VoxelMesh _mesh)
  {
    new MeshGeneratorOptimizedMesh((INeighborBlockCache) this).GenerateColorCubeMesh(Vector3i.zero, this.size, _mesh);
  }

  public Transform ToTransform()
  {
    MeshFilter[][] meshFilterArray = new MeshFilter[MeshDescription.meshes.Length][];
    MeshRenderer[][] meshRendererArray = new MeshRenderer[MeshDescription.meshes.Length][];
    MeshCollider[][] meshColliderArray = new MeshCollider[MeshDescription.meshes.Length][];
    GameObject[] gameObjectArray = new GameObject[MeshDescription.meshes.Length];
    GameObject gameObject1 = new GameObject();
    gameObject1.transform.parent = (Transform) null;
    ((Object) gameObject1).name = "Prefab_" + this.PrefabName;
    GameObject gameObject2 = new GameObject("_BlockEntities");
    gameObject2.transform.parent = gameObject1.transform;
    GameObject gameObject3 = new GameObject("Meshes");
    gameObject3.transform.parent = gameObject1.transform;
    for (int _meshIndex = 0; _meshIndex < MeshDescription.meshes.Length; ++_meshIndex)
    {
      gameObjectArray[_meshIndex] = new GameObject(MeshDescription.meshes[_meshIndex].Name);
      gameObjectArray[_meshIndex].transform.parent = gameObject3.transform;
      VoxelMesh.CreateMeshFilter(_meshIndex, 0, gameObjectArray[_meshIndex], MeshDescription.meshes[_meshIndex].Tag, false, out meshFilterArray[_meshIndex], out meshRendererArray[_meshIndex], out meshColliderArray[_meshIndex]);
    }
    VoxelMesh[] _meshes = new VoxelMesh[6];
    for (int _meshIndex = 0; _meshIndex < _meshes.Length; ++_meshIndex)
      _meshes[_meshIndex] = new VoxelMesh(_meshIndex);
    new MeshGenerator((INeighborBlockCache) this).GenerateMesh(new Vector3i(-1, -1, -1), this.size + Vector3i.one, _meshes);
    for (int index = 0; index < _meshes.Length; ++index)
      _meshes[index].CopyToMesh(meshFilterArray[index], meshRendererArray[index], 0);
    for (int _x = 0; _x < this.size.x; ++_x)
    {
      for (int _z = 0; _z < this.size.z; ++_z)
      {
        for (int _y = 0; _y < this.size.y; ++_y)
        {
          Vector3i vector3i = new Vector3i(_x, _y, _z);
          BlockValue block1 = this.GetBlock(_x, _y, _z);
          Block block2 = block1.Block;
          if ((!block2.isMultiBlock || !block1.ischild) && block2.shape is BlockShapeModelEntity shape)
          {
            Quaternion rotation = shape.GetRotation(block1);
            Vector3 rotatedOffset = shape.GetRotatedOffset(block2, rotation);
            rotatedOffset.x += 0.5f;
            rotatedOffset.z += 0.5f;
            rotatedOffset.y += 0.0f;
            Vector3 vector3 = Vector3.op_Addition(vector3i.ToVector3(), rotatedOffset);
            GameObject objectForType = GameObjectPool.Instance.GetObjectForType(shape.modelName);
            if (!Object.op_Equality((Object) objectForType, (Object) null))
            {
              objectForType.SetActive(true);
              Transform transform = objectForType.transform;
              transform.parent = gameObject2.transform;
              transform.localScale = Vector3.one;
              transform.localPosition = vector3;
              transform.localRotation = rotation;
            }
          }
        }
      }
    }
    return gameObject1.transform;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public List<BiomeDefinition> toBiomeArray(WorldBiomes _biomes, List<string> _biomeStrList)
  {
    List<BiomeDefinition> biomeArray = new List<BiomeDefinition>();
    for (int index = 0; index < _biomeStrList.Count; ++index)
    {
      string biomeStr = _biomeStrList[index];
      BiomeDefinition biome;
      if ((biome = _biomes.GetBiome(biomeStr)) != null)
        biomeArray.Add(biome);
    }
    return biomeArray;
  }

  public string[] GetAllowedBiomes() => this.allowedBiomes.ToArray();

  public string[] GetAllowedZones() => this.allowedZones.ToArray();

  public bool IsAllowedZone(string _zone) => this.allowedZones.ContainsCaseInsensitive(_zone);

  public void AddAllowedZone(string _zone)
  {
    if (this.IsAllowedZone(_zone))
      return;
    this.allowedZones.Add(_zone);
  }

  public void RemoveAllowedZone(string _zone)
  {
    int index = this.allowedZones.FindIndex((Predicate<string>) ([PublicizedFrom(EAccessModifier.Internal)] (_s) => _s.EqualsCaseInsensitive(_zone)));
    if (index < 0)
      return;
    this.allowedZones.RemoveAt(index);
  }

  public string[] GetAllowedTownships() => this.allowedTownships.ToArray();

  public void SetAllowedBiomes(string[] _b)
  {
    this.allowedBiomes = new List<string>((IEnumerable<string>) _b);
  }

  public List<BiomeDefinition> GetAllowedBiomes(WorldBiomes _biomes)
  {
    return this.toBiomeArray(_biomes, this.allowedBiomes);
  }

  public void CopyBlocksIntoChunkNoEntities(
    World _world,
    Chunk _chunk,
    Vector3i _prefabTargetPos,
    bool _bForceOverwriteBlocks)
  {
    bool flag1 = this.IsCullThisPrefab() && GameStats.GetInt(EnumGameStats.OptionsPOICulling) > 1;
    bool flag2 = _world.IsEditor();
    if (this.terrainFillerType == 0)
      this.InitTerrainFillers();
    GameRandom gameRandom = GameManager.Instance.World.GetGameRandom();
    Bounds aabb = _chunk.GetAABB();
    int num1 = 0;
    int num2 = 0;
    int num3 = _prefabTargetPos.x - (int) ((Bounds) ref aabb).min.x;
    int num4;
    if (num3 >= 0)
    {
      num1 = num3;
      num4 = Utils.FastMin(16 /*0x10*/ - num3, this.size.x);
    }
    else
    {
      num2 = -1 * num3;
      num4 = Utils.FastMin(this.size.x + num3, 16 /*0x10*/);
    }
    int num5 = 0;
    int num6 = 0;
    int num7 = _prefabTargetPos.z - (int) ((Bounds) ref aabb).min.z;
    int num8;
    if (num7 >= 0)
    {
      num5 = num7;
      num8 = Utils.FastMin(16 /*0x10*/ - num7, this.size.z);
    }
    else
    {
      num6 = -1 * num7;
      num8 = Utils.FastMin(this.size.z + num7, 16 /*0x10*/);
    }
    for (int index1 = 0; index1 < num8; ++index1)
    {
      int num9 = index1 + num5;
      for (int index2 = 0; index2 < num4; ++index2)
      {
        int num10 = index2 + num1;
        int terrainHeight = (int) _chunk.GetTerrainHeight(num10, num9);
        BlockValue block1 = _chunk.GetBlock(num10, terrainHeight, num9);
        BlockValue blockValue1 = block1;
        int _h = terrainHeight;
        bool flag3 = false;
        for (int _y = 0; _y < this.size.y; ++_y)
        {
          BlockValue targetBV = this.GetBlock(index2 + num2, _y, index1 + num6);
          WaterValue water = this.GetWater(index2 + num2, _y, index1 + num6);
          Block block2 = targetBV.Block;
          bool flag4 = false;
          if (block2.IsSleeperBlock)
          {
            flag4 = true;
            targetBV = BlockValue.Air;
          }
          int num11 = _y + _prefabTargetPos.y;
          if (num11 >= 0 && num11 < (int) byte.MaxValue)
          {
            int num12 = this.bAllowTopSoilDecorations ? 1 : 0;
            if (targetBV.type == this.terrainFillerType)
            {
              if (!flag2)
              {
                BlockValue blockValue2 = _chunk.GetBlock(num10, num11, num9);
                block2 = blockValue2.Block;
                if (blockValue2.isair || block2 == null || !block2.shape.IsTerrain())
                {
                  blockValue2 = num11 >= terrainHeight ? block1 : blockValue1;
                  block2 = blockValue2.Block;
                  if (blockValue2.isair || block2 == null || !block2.shape.IsTerrain())
                    continue;
                }
                targetBV = blockValue2;
              }
              if (block2.multiBlockPos != null && block2.multiBlockPos.dim.x != 1 && block2.multiBlockPos.dim.y != 1)
              {
                int z = block2.multiBlockPos.dim.z;
              }
            }
            sbyte _density = this.GetDensity(index2 + num2, _y, index1 + num6);
            if (targetBV.type == this.terrainFiller2Type)
            {
              BlockValue block3 = _chunk.GetBlock(num10, num11, num9);
              Block block4 = block3.Block;
              if (!block3.isair && block4 != null && block4.shape.IsTerrain())
              {
                targetBV = block3;
                _density = _chunk.GetDensity(num10, num11, num9);
              }
              else
              {
                targetBV = BlockValue.Air;
                _density = MarchingCubes.DensityAir;
                if (num11 > 0 && _chunk.GetBlock(num10, num11 - 1, num9).Block.shape.IsTerrain())
                {
                  sbyte density = _chunk.GetDensity(num10, num11 - 1, num9);
                  _density = (sbyte) ((int) MarchingCubes.DensityAir + (int) density);
                }
              }
              block2 = targetBV.Block;
            }
            if (!flag2)
            {
              if (targetBV.Block.isMultiBlock && MultiBlockManager.Instance.POIMBTrackingEnabled)
              {
                this.ProcessMultiBlock(ref targetBV, _chunk, new Vector3i(index2 + num2, _y, index1 + num6), new Vector3i(num10, num11, num9), FastTags<TagGroup.Global>.none, _bForceOverwriteBlocks);
                block2 = targetBV.Block;
              }
              else if (BlockPlaceholderMap.Instance.IsReplaceableBlockType(targetBV))
              {
                targetBV = BlockPlaceholderMap.Instance.Replace(targetBV, gameRandom, _chunk, num10, num11, num9, FastTags<TagGroup.Global>.none, _bForceOverwriteBlocks);
                block2 = targetBV.Block;
              }
            }
            bool flag5 = block2.shape.IsTerrain();
            if (flag5)
            {
              blockValue1 = targetBV;
              if (num11 > _h)
              {
                _h = num11;
                flag3 = true;
              }
            }
            else if (num11 <= _h)
            {
              _h = num11 - 1;
              flag3 = true;
            }
            if (_density == (sbyte) 0)
              _density = !flag5 ? (!block2.shape.IsSolidCube || num11 > _h ? MarchingCubes.DensityAir : (sbyte) 1) : MarchingCubes.DensityTerrain;
            if (this.yOffset == 0)
            {
              sbyte density = _chunk.GetDensity(num10, num11, num9);
              if (_density >= (sbyte) 0 && density >= (sbyte) 0 && ((int) density != (int) MarchingCubes.DensityAir / 2 || block2.IsTerrainDecoration && !this.bCopyAirBlocks) || _density < (sbyte) 0 && density < (sbyte) 0 && (int) density != (int) MarchingCubes.DensityTerrain / 2)
                _density = density;
            }
            _chunk.SetDecoAllowedSizeAt(num10, num9, EnumDecoAllowedSize.NoBigOnlySmall);
            Vector3i _blockPos = new Vector3i(index2 + num2, _y, index1 + num6);
            if (flag1 && !block2.shape.IsTerrain() && this.IsInsidePrefab(_blockPos.x, _blockPos.y, _blockPos.z))
              _chunk.AddInsideDevicePosition(num10, num11, num9, targetBV);
            if (this.bCopyAirBlocks || !targetBV.isair || _y < -this.yOffset || water.HasMass())
            {
              BlockValue block5 = _chunk.GetBlock(num10, num11, num9);
              if (!_bForceOverwriteBlocks && !block5.Block.shape.IsTerrain() && !block5.isair && (block5.ischild || block5.type == targetBV.type))
              {
                _chunk.SetDensity(num10, num11, num9, _density);
              }
              else
              {
                if (!flag4)
                {
                  TextureFullArray texture = this.GetTexture(index2 + num2, _y, index1 + num6);
                  _chunk.GetSetTextureFullArray(num10, num11, num9, texture);
                }
                _chunk.SetBlock((WorldBase) _world, num10, num11, num9, targetBV, _poiOwned: true);
                _chunk.SetWater(num10, num11, num9, water);
                _chunk.SetDensity(num10, num11, num9, _density);
                TileEntity tileEntity;
                if (targetBV.Block.IsTileEntitySavedInPrefab() && (tileEntity = this.GetTileEntity(_blockPos)) != null)
                {
                  TileEntity _te = _chunk.GetTileEntity(new Vector3i(num10, num11, num9));
                  if (_te == null)
                  {
                    _te = tileEntity.Clone();
                    _te.localChunkPos = new Vector3i(num10, num11, num9);
                    _te.SetChunk(_chunk);
                    _chunk.AddTileEntity(_te);
                  }
                  _te.CopyFrom(tileEntity);
                  _te.localChunkPos = new Vector3i(num10, num11, num9);
                }
                BlockTrigger blockTrigger;
                if ((blockTrigger = this.GetBlockTrigger(_blockPos)) != null)
                {
                  BlockTrigger _td = _chunk.GetBlockTrigger(new Vector3i(num10, num11, num9));
                  if (_td == null)
                  {
                    _td = blockTrigger.Clone();
                    _td.LocalChunkPos = new Vector3i(num10, num11, num9);
                    _td.Chunk = _chunk;
                    _chunk.AddBlockTrigger(_td);
                  }
                  _td.CopyFrom(blockTrigger);
                  _td.LocalChunkPos = new Vector3i(num10, num11, num9);
                }
              }
            }
          }
        }
        if (flag3 && (_h > terrainHeight || _prefabTargetPos.y + this.size.y >= terrainHeight))
          _chunk.SetTerrainHeight(num10, num9, (byte) _h);
        _chunk.SetTopSoilBroken(num10, num9);
      }
    }
    this.CopySleeperVolumes(_world, _chunk, _prefabTargetPos);
    this.ApplyDecoAllowed(_chunk, _prefabTargetPos);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void ApplyDecoAllowed(Chunk _chunk, Vector3i _prefabTargetPos)
  {
    foreach (int allowedBlockIndex in this.decoAllowedBlockIndices)
    {
      int _x;
      int _y;
      int _z;
      this.offsetToCoordRotated(allowedBlockIndex, out _x, out _y, out _z);
      BlockValue block = this.GetBlock(_x, _y, _z);
      DecoUtils.ApplyDecoAllowed(_chunk, _prefabTargetPos + new Vector3i(_x, _y, _z), block);
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void updateBlockStatistics(BlockValue bv, Block b)
  {
    if (!Block.BlocksLoaded || b == null)
      return;
    this.statistics.cntWindows += b.BlockTag == BlockTags.Window ? 1 : 0;
    this.statistics.cntDoors += b.BlockTag == BlockTags.Door ? 1 : 0;
    this.statistics.cntBlockEntities += !(b.shape is BlockShapeModelEntity) || bv.ischild || b is BlockModelTree && bv.meta != (byte) 0 ? 0 : 1;
    this.statistics.cntBlockModels += !(b.shape is BlockShapeExt3dModel) || bv.ischild ? 0 : 1;
    this.statistics.cntSolid += !bv.isair ? 1 : 0;
  }

  public Prefab.BlockStatistics GetBlockStatistics() => this.statistics;

  public List<EntityCreationData> GetEntities() => this.entities;

  public void Mirror(EnumMirrorAlong _axis)
  {
    Prefab.Data arrays = this.CellsToArrays();
    Prefab.Data _data = new Prefab.Data();
    _data.Init(this.GetBlockCount());
    BlockValue air = BlockValue.Air;
    for (int _x = 0; _x < this.size.x; ++_x)
    {
      for (int _z = 0; _z < this.size.z; ++_z)
      {
        for (int _y = 0; _y < this.size.y; ++_y)
        {
          int offset1 = this.CoordToOffset(this.localRotation, _x, _y, _z);
          WaterValue waterValue = arrays.m_Water[offset1];
          air.rawData = arrays.m_Blocks[offset1];
          if (!air.ischild && (!air.isair || waterValue.HasMass()))
          {
            Block block = air.Block;
            BlockShape shape = block.shape;
            int num = (int) (byte) BlockShapeNew.MirrorStatic(_axis, (int) air.rotation, shape.SymmetryType);
            Vector3i _pos1 = new Vector3i(_x, _y, _z);
            Vector3i vector3i = GameUtils.Mirror(_axis, _pos1, this.size);
            if (block.isMultiBlock)
            {
              Vector3 vector3_1;
              // ISSUE: explicit constructor call
              ((Vector3) ref vector3_1).\u002Ector(block.multiBlockPos.dim.x % 2 == 0 ? -0.5f : 0.0f, block.multiBlockPos.dim.y % 2 == 0 ? -0.5f : 0.0f, block.multiBlockPos.dim.z % 2 == 0 ? -0.5f : 0.0f);
              Vector3 vector3_2 = Quaternion.op_Multiply(BlockShapeNew.GetRotationStatic((int) air.rotation), vector3_1);
              Vector3 _pos2 = Vector3.op_Addition(GameUtils.Mirror(_axis, Vector3.op_Addition(vector3i.ToVector3(), new Vector3(0.5f, 0.5f, 0.5f)), this.size), vector3_2);
              vector3i = World.worldToBlockPos(Vector3.op_Subtraction(GameUtils.Mirror(_axis, _pos2, this.size), Quaternion.op_Multiply(BlockShapeNew.GetRotationStatic(num), vector3_1)));
            }
            int offset2 = this.CoordToOffset(this.localRotation, vector3i.x, vector3i.y, vector3i.z);
            if (block.MirrorSibling != 0)
              air.type = block.MirrorSibling;
            air.rotation = (byte) num;
            _data.m_Blocks[offset2] = air.rawData;
            _data.m_Damage[offset2] = arrays.m_Damage[offset1];
            _data.m_Density[offset2] = arrays.m_Density[offset1];
            _data.m_Textures[offset2] = this.mirrorTexture(_axis, shape, (int) air.rotation, num, arrays.m_Textures[offset1]);
            _data.m_Water[offset2] = waterValue;
          }
        }
      }
    }
    this.CellsFromArrays(ref _data);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public TextureFullArray mirrorTexture(
    EnumMirrorAlong _axis,
    BlockShape _shape,
    int _sourceRot,
    int _targetRot,
    TextureFullArray _tex)
  {
    TextureFullArray textureFullArray = new TextureFullArray(0L);
    for (int index1 = 0; index1 < 6; ++index1)
    {
      BlockFace _face = (BlockFace) index1;
      BlockFace _sourceFace;
      BlockFace _targetFace;
      _shape.MirrorFace(_axis, _sourceRot, _targetRot, _face, out _sourceFace, out _targetFace);
      for (int index2 = 0; index2 < 1; ++index2)
      {
        long num = (_tex[index2] >> 8 * (int) _sourceFace & (long) byte.MaxValue) << 8 * (int) _targetFace;
        textureFullArray[index2] |= num;
      }
    }
    return textureFullArray;
  }

  public void CloneSleeperVolume(string name, Vector3i boundingBoxPosition, int idx)
  {
    Prefab.PrefabSleeperVolume sleeperVolume = this.SleeperVolumes[idx];
    this.AddSleeperVolume(name, boundingBoxPosition, sleeperVolume.startPos + new Vector3i(0, sleeperVolume.size.y + 1, 0), sleeperVolume.size, sleeperVolume.groupId, sleeperVolume.groupName, (int) sleeperVolume.spawnCountMin, (int) sleeperVolume.spawnCountMax);
  }

  public int AddSleeperVolume(
    string _prefabInstanceName,
    Vector3i bbPos,
    Vector3i startPos,
    Vector3i size,
    short groupId,
    string _groupName,
    int _spawnMin,
    int _spawnMax)
  {
    int num = -1;
    Prefab.PrefabSleeperVolume _volume = (Prefab.PrefabSleeperVolume) null;
    for (int index = 0; index < this.SleeperVolumes.Count; ++index)
    {
      if (!this.SleeperVolumes[index].used)
      {
        num = index;
        _volume = this.SleeperVolumes[index];
        break;
      }
    }
    if (_volume == null)
    {
      _volume = new Prefab.PrefabSleeperVolume();
      num = this.SleeperVolumes.Count;
      this.SleeperVolumes.Add(_volume);
    }
    _volume.Use(startPos, size, groupId, _groupName, false, false, _spawnMin, _spawnMax, 0);
    string _name = $"{_prefabInstanceName}_{num.ToString()}";
    this.AddSleeperVolumeSelectionBox(_volume, _name, bbPos + startPos);
    SelectionBoxManager.Instance.SetActive("SleeperVolume", _name, true);
    return num;
  }

  public void SetSleeperVolume(
    string _prefabInstanceName,
    Vector3i _prefabInstanceBoundingBox,
    int _index,
    Prefab.PrefabSleeperVolume _volumeSettings)
  {
    while (_index >= this.SleeperVolumes.Count)
      this.SleeperVolumes.Add(new Prefab.PrefabSleeperVolume());
    bool used = this.SleeperVolumes[_index].used;
    this.SleeperVolumes[_index] = _volumeSettings;
    string _name = $"{_prefabInstanceName}_{_index.ToString()}";
    if (_volumeSettings.used)
    {
      if (!used)
      {
        this.AddSleeperVolumeSelectionBox(_volumeSettings, _name, _prefabInstanceBoundingBox + _volumeSettings.startPos);
        SelectionBoxManager.Instance.SetActive("SleeperVolume", _name, true);
      }
      else
      {
        SelectionBoxManager.Instance.GetCategory("SleeperVolume").GetBox(_name).SetPositionAndSize((Vector3) (_prefabInstanceBoundingBox + _volumeSettings.startPos), _volumeSettings.size);
        SelectionBoxManager.Instance.SetUserData("SleeperVolume", _name, (object) _volumeSettings);
      }
    }
    else
    {
      if (!used)
        return;
      SelectionBoxManager.Instance.GetCategory("SleeperVolume").RemoveBox(_name);
    }
  }

  public void AddSleeperVolumeSelectionBox(
    Prefab.PrefabSleeperVolume _volume,
    string _name,
    Vector3i _pos)
  {
    SelectionBoxManager.Instance.GetCategory("SleeperVolume").AddBox(_name, (Vector3) _pos, _volume.size).UserData = (object) _volume;
  }

  public short FindSleeperVolumeFreeGroupId()
  {
    int num = 0;
    for (int index = 0; index < this.SleeperVolumes.Count; ++index)
    {
      Prefab.PrefabSleeperVolume sleeperVolume = this.SleeperVolumes[index];
      if ((int) sleeperVolume.groupId > num)
        num = (int) sleeperVolume.groupId;
    }
    return (short) (num + 1);
  }

  public int AddTeleportVolume(
    string _prefabInstanceName,
    Vector3i bbPos,
    Vector3i startPos,
    Vector3i size)
  {
    Prefab.PrefabTeleportVolume _volume = new Prefab.PrefabTeleportVolume();
    int count = this.TeleportVolumes.Count;
    this.TeleportVolumes.Add(_volume);
    _volume.Use(startPos, size);
    string _name = $"{_prefabInstanceName}_{count.ToString()}";
    this.AddTeleportVolumeSelectionBox(_volume, _name, bbPos + startPos);
    SelectionBoxManager.Instance.SetActive("TraderTeleport", _name, true);
    return count;
  }

  public void SetTeleportVolume(
    string _prefabInstanceName,
    Vector3i _prefabInstanceBoundingBox,
    int _index,
    Prefab.PrefabTeleportVolume _volumeSettings,
    bool remove = false)
  {
    while (_index >= this.TeleportVolumes.Count)
      this.TeleportVolumes.Add(new Prefab.PrefabTeleportVolume());
    if (!remove)
      this.TeleportVolumes[_index] = _volumeSettings;
    else
      this.TeleportVolumes.RemoveAt(_index);
    string _name = $"{_prefabInstanceName}_{_index.ToString()}";
    SelectionBoxManager.Instance.GetCategory("TraderTeleport").RemoveBox(_name);
    if (remove)
      return;
    this.AddTeleportVolumeSelectionBox(_volumeSettings, _name, _prefabInstanceBoundingBox + _volumeSettings.startPos);
    SelectionBoxManager.Instance.SetActive("TraderTeleport", _name, true);
  }

  public void AddTeleportVolumeSelectionBox(
    Prefab.PrefabTeleportVolume _volume,
    string _name,
    Vector3i _pos)
  {
    SelectionBoxManager.Instance.GetCategory("TraderTeleport").AddBox(_name, (Vector3) _pos, _volume.size).UserData = (object) _volume;
  }

  public int AddInfoVolume(
    string _prefabInstanceName,
    Vector3i bbPos,
    Vector3i startPos,
    Vector3i size)
  {
    Prefab.PrefabInfoVolume _volume = new Prefab.PrefabInfoVolume();
    int count = this.InfoVolumes.Count;
    this.InfoVolumes.Add(_volume);
    _volume.Use(startPos, size);
    string _name = $"{_prefabInstanceName}_{count.ToString()}";
    this.AddInfoVolumeSelectionBox(_volume, _name, bbPos + startPos);
    SelectionBoxManager.Instance.SetActive("InfoVolume", _name, true);
    return count;
  }

  public void SetInfoVolume(
    string _prefabInstanceName,
    Vector3i _prefabInstanceBoundingBox,
    int _index,
    Prefab.PrefabInfoVolume _volumeSettings,
    bool remove = false)
  {
    while (_index >= this.InfoVolumes.Count)
      this.InfoVolumes.Add(new Prefab.PrefabInfoVolume());
    if (!remove)
      this.InfoVolumes[_index] = _volumeSettings;
    else
      this.InfoVolumes.RemoveAt(_index);
    string _name = $"{_prefabInstanceName}_{_index.ToString()}";
    SelectionBoxManager.Instance.GetCategory("InfoVolume").RemoveBox(_name);
    if (remove)
      return;
    this.AddInfoVolumeSelectionBox(_volumeSettings, _name, _prefabInstanceBoundingBox + _volumeSettings.startPos);
    SelectionBoxManager.Instance.SetActive("InfoVolume", _name, true);
  }

  public void AddInfoVolumeSelectionBox(
    Prefab.PrefabInfoVolume _volume,
    string _name,
    Vector3i _pos)
  {
    SelectionBoxManager.Instance.GetCategory("InfoVolume").AddBox(_name, (Vector3) _pos, _volume.size).UserData = (object) _volume;
  }

  public int AddWallVolume(
    string _prefabInstanceName,
    Vector3i bbPos,
    Vector3i startPos,
    Vector3i size)
  {
    Prefab.PrefabWallVolume _volume = new Prefab.PrefabWallVolume();
    int count = this.WallVolumes.Count;
    this.WallVolumes.Add(_volume);
    _volume.Use(startPos, size);
    string _name = $"{_prefabInstanceName}_{count.ToString()}";
    this.AddWallVolumeSelectionBox(_volume, _name, bbPos + startPos);
    SelectionBoxManager.Instance.SetActive("WallVolume", _name, true);
    return count;
  }

  public void SetWallVolume(
    string _prefabInstanceName,
    Vector3i _prefabInstanceBoundingBox,
    int _index,
    Prefab.PrefabWallVolume _volumeSettings,
    bool remove = false)
  {
    while (_index >= this.WallVolumes.Count)
      this.WallVolumes.Add(new Prefab.PrefabWallVolume());
    if (!remove)
      this.WallVolumes[_index] = _volumeSettings;
    else
      this.WallVolumes.RemoveAt(_index);
    string _name = $"{_prefabInstanceName}_{_index.ToString()}";
    SelectionBoxManager.Instance.GetCategory("WallVolume").RemoveBox(_name);
    if (remove)
      return;
    this.AddWallVolumeSelectionBox(_volumeSettings, _name, _prefabInstanceBoundingBox + _volumeSettings.startPos);
    SelectionBoxManager.Instance.SetActive("WallVolume", _name, true);
  }

  public void AddWallVolumeSelectionBox(
    Prefab.PrefabWallVolume _volume,
    string _name,
    Vector3i _pos)
  {
    SelectionBoxManager.Instance.GetCategory("WallVolume").AddBox(_name, (Vector3) _pos, _volume.size).UserData = (object) _volume;
  }

  public int AddTriggerVolume(
    string _prefabInstanceName,
    Vector3i bbPos,
    Vector3i startPos,
    Vector3i size)
  {
    Prefab.PrefabTriggerVolume _volume = new Prefab.PrefabTriggerVolume();
    int count = this.TriggerVolumes.Count;
    this.TriggerVolumes.Add(_volume);
    _volume.Use(startPos, size);
    string _name = $"{_prefabInstanceName}_{count.ToString()}";
    this.AddTriggerVolumeSelectionBox(_volume, _name, bbPos + startPos);
    SelectionBoxManager.Instance.SetActive("TriggerVolume", _name, true);
    return count;
  }

  public void SetTriggerVolume(
    string _prefabInstanceName,
    Vector3i _prefabInstanceBoundingBox,
    int _index,
    Prefab.PrefabTriggerVolume _volumeSettings,
    bool remove = false)
  {
    while (_index >= this.TriggerVolumes.Count)
      this.TriggerVolumes.Add(new Prefab.PrefabTriggerVolume());
    if (!remove)
      this.TriggerVolumes[_index] = _volumeSettings;
    else
      this.TriggerVolumes.RemoveAt(_index);
    string _name = $"{_prefabInstanceName}_{_index.ToString()}";
    SelectionBoxManager.Instance.GetCategory("TriggerVolume").RemoveBox(_name);
    if (remove)
      return;
    this.AddTriggerVolumeSelectionBox(_volumeSettings, _name, _prefabInstanceBoundingBox + _volumeSettings.startPos);
    SelectionBoxManager.Instance.SetActive("TriggerVolume", _name, true);
  }

  public void AddTriggerVolumeSelectionBox(
    Prefab.PrefabTriggerVolume _volume,
    string _name,
    Vector3i _pos)
  {
    SelectionBoxManager.Instance.GetCategory("TriggerVolume").AddBox(_name, (Vector3) _pos, _volume.size).UserData = (object) _volume;
  }

  public void AddNewPOIMarker(
    string _prefabInstanceName,
    Vector3i bbPos,
    Vector3i _start,
    Vector3i _size,
    string _group,
    FastTags<TagGroup.Poi> _tags,
    Prefab.Marker.MarkerTypes _type,
    bool isSelected = false)
  {
    this.POIMarkers.Add(new Prefab.Marker(_start, _size, _type, _group, _tags));
    this.AddPOIMarker(_prefabInstanceName, bbPos, _start, _size, _group, _tags, _type, this.POIMarkers.Count - 1, isSelected);
  }

  public void AddPOIMarker(
    string _prefabInstanceName,
    Vector3i bbPos,
    Vector3i _start,
    Vector3i _size,
    string _group,
    FastTags<TagGroup.Poi> _tags,
    Prefab.Marker.MarkerTypes _type,
    int _index,
    bool isSelected = false)
  {
    this.AddPOIMarkerSelectionBox(this.POIMarkers[_index], _index, bbPos + _start, isSelected);
  }

  public void AddPOIMarkerSelectionBox(
    Prefab.Marker _marker,
    int _index,
    Vector3i _pos,
    bool isSelected = false)
  {
    string _name = "POIMarker_" + _index.ToString();
    _marker.Name = _name;
    SelectionBox selectionBox = SelectionBoxManager.Instance.GetCategory("POIMarker").AddBox(_name, (Vector3) _pos, _marker.Size);
    selectionBox.bDrawDirection = true;
    selectionBox.bAlwaysDrawDirection = true;
    SelectionBoxManager.Instance.SetUserData("POIMarker", _name, (object) _marker);
    SelectionBoxManager.Instance.SetActive("POIMarker", _name, true);
    float _facing = 0.0f;
    switch (_marker.Rotations)
    {
      case 1:
        _facing = _marker.MarkerType == Prefab.Marker.MarkerTypes.PartSpawn ? 90f : 270f;
        break;
      case 2:
        _facing = 180f;
        break;
      case 3:
        _facing = _marker.MarkerType == Prefab.Marker.MarkerTypes.PartSpawn ? 270f : 90f;
        break;
    }
    SelectionBoxManager.Instance.SetFacingDirection("POIMarker", _name, _facing);
    POIMarkerToolManager.RegisterPOIMarker(selectionBox);
    if (!isSelected)
      return;
    POIMarkerToolManager.SelectionChanged(selectionBox);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public ArrayListMP<int> loadIdMapping(
    string _directory,
    string _prefabFileName,
    bool _allowMissingBlocks)
  {
    if (!Block.BlocksLoaded)
    {
      Log.Error("Block data not loaded");
      return (ArrayListMP<int>) null;
    }
    string str = $"{_directory}/{_prefabFileName}.blocks.nim";
    if (!SdFile.Exists(str))
    {
      Log.Error($"Loading prefab \"{_prefabFileName}\" failed: Block name to ID mapping file missing.");
      return (ArrayListMP<int>) null;
    }
    using (NameIdMapping nameIdMapping = MemoryPools.poolNameIdMapping.AllocSync(true))
    {
      nameIdMapping.InitMapping(str, Block.MAX_BLOCKS);
      if (!nameIdMapping.LoadFromFile())
        return (ArrayListMP<int>) null;
      Block missingBlock = (Block) null;
      if (_allowMissingBlocks)
      {
        missingBlock = Block.GetBlockByName(Prefab.MISSING_BLOCK_NAME);
        this.blockTypeMissingBlock = missingBlock != null ? missingBlock.blockID : -1;
      }
      return nameIdMapping.createIdTranslationTable((Func<string, int>) ([PublicizedFrom(EAccessModifier.Internal)] (_blockName) =>
      {
        Block blockByName = Block.GetBlockByName(_blockName);
        return blockByName == null ? -1 : blockByName.blockID;
      }), (NameIdMapping.MissingEntryCallbackDelegate) ([PublicizedFrom(EAccessModifier.Internal)] (_name, _id) =>
      {
        if (!_allowMissingBlocks)
        {
          Log.Error($"Loading prefab \"{_prefabFileName}\" failed: Block \"{_name}\" ({_id}) used in prefab is unknown.");
          return -1;
        }
        if (missingBlock == null)
        {
          Log.Error($"Loading prefab \"{_prefabFileName}\" failed: Block \"{_name}\" ({_id}) used in prefab is unknown and the replacement block \"{Prefab.MISSING_BLOCK_NAME}\" was not found.");
          return -1;
        }
        Log.Warning($"Loading prefab \"{_prefabFileName}\": Block \"{_name}\" ({_id}) used in prefab is unknown and getting replaced by \"{Prefab.MISSING_BLOCK_NAME}\".");
        return missingBlock.blockID;
      }));
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public bool doRaycast(Ray ray, out RaycastHit hitInfo, Vector3i _min)
  {
    bool flag = Physics.Raycast(ray, ref hitInfo, (float) byte.MaxValue, 1073807360 /*0x40010000*/);
    if (!flag)
      return false;
    Vector3 vector3 = Vector3.op_Addition(((RaycastHit) ref hitInfo).point, Vector3.op_Multiply(((Ray) ref ray).direction, 0.01f));
    Vector3i vector3i = new Vector3i(Utils.Fastfloor(vector3.x), Utils.Fastfloor(vector3.y), Utils.Fastfloor(vector3.z));
    Block block = this.GetBlock(vector3i.x - _min.x, vector3i.y - _min.y, vector3i.z - _min.z).Block;
    if (block.bImposterDontBlock || block.bImposterExclude)
    {
      ((Ray) ref ray).origin = Vector3.op_Addition(((RaycastHit) ref hitInfo).point, Vector3.op_Multiply(((Ray) ref ray).direction, 0.01f));
      flag = Physics.Raycast(ray, ref hitInfo, (float) byte.MaxValue, 1073807360 /*0x40010000*/);
    }
    return flag;
  }

  public EnumInsideOutside[] UpdateInsideOutside(Vector3i _min, Vector3i _max)
  {
    EnumInsideOutside[] enumInsideOutsideArray = new EnumInsideOutside[this.GetBlockCount()];
    BlockValue air = BlockValue.Air;
    uint[] blocks = this.CellsToArrays().m_Blocks;
    Ray ray;
    RaycastHit hitInfo;
    for (int x = _min.x; x <= _max.x; ++x)
    {
      for (int z = _min.z; z <= _max.z; ++z)
      {
        int v1 = _max.y;
        // ISSUE: explicit constructor call
        ((Ray) ref ray).\u002Ector(Vector3.zero, Vector3.down);
        bool flag = false;
        for (float num1 = 0.0f; !flag && (double) num1 <= 1.0; num1 += 0.25f)
        {
          for (float num2 = 0.0f; !flag && (double) num2 <= 1.0; num2 += 0.25f)
          {
            ((Ray) ref ray).origin = new Vector3((float) x + num1, (float) (_max.y + 3), (float) z + num2);
            if (this.doRaycast(ray, out hitInfo, _min))
            {
              v1 = Utils.FastMin(v1, Utils.Fastfloor(((RaycastHit) ref hitInfo).point.y + ((Ray) ref ray).direction.y * 0.1f));
            }
            else
            {
              v1 = _min.y;
              flag = true;
            }
          }
        }
        int index1 = x - _min.x + (v1 - _min.y) * this.size.x + (z - _min.z) * this.size.x * this.size.y;
        if (index1 >= 0 && index1 < enumInsideOutsideArray.Length)
        {
          while (index1 > 0)
          {
            air.rawData = blocks[index1];
            if (air.isair)
            {
              index1 -= this.size.x;
              --v1;
            }
            else
              break;
          }
          if (index1 > 0)
          {
            air.rawData = blocks[index1];
            if (air.ischild)
            {
              int type = air.type;
              while (index1 > 0)
              {
                air.rawData = blocks[index1];
                if (air.type == type)
                {
                  index1 -= this.size.x;
                  --v1;
                }
                else
                  break;
              }
            }
          }
        }
        for (int y = _max.y; y >= v1; --y)
        {
          int index2 = x - _min.x + (y - _min.y) * this.size.x + (z - _min.z) * this.size.x * this.size.y;
          if (index2 >= 0 && index2 < enumInsideOutsideArray.Length)
            enumInsideOutsideArray[index2] = EnumInsideOutside.Outside;
        }
      }
    }
    for (int z = _min.z; z <= _max.z; ++z)
    {
      for (int y = _min.y; y <= _max.y; ++y)
      {
        int v1_1 = _min.x;
        // ISSUE: explicit constructor call
        ((Ray) ref ray).\u002Ector(Vector3.zero, Vector3.right);
        bool flag1 = false;
        for (float num3 = 0.0f; !flag1 && (double) num3 <= 1.0; num3 += 0.25f)
        {
          for (float num4 = 0.0f; !flag1 && (double) num4 <= 1.0; num4 += 0.25f)
          {
            ((Ray) ref ray).origin = new Vector3((float) (_min.x - 3), (float) y + num3, (float) z + num4);
            if (this.doRaycast(ray, out hitInfo, _min))
            {
              v1_1 = Utils.FastMax(v1_1, Utils.Fastfloor(((RaycastHit) ref hitInfo).point.x + ((Ray) ref ray).direction.x * 0.1f));
            }
            else
            {
              v1_1 = _max.x;
              flag1 = true;
            }
          }
        }
        int index3 = v1_1 - _min.x + (y - _min.y) * this.size.x + (z - _min.z) * this.size.x * this.size.y;
        if (index3 >= 0 && index3 < enumInsideOutsideArray.Length)
        {
          while (index3 < blocks.Length - 1)
          {
            air.rawData = blocks[index3];
            if (air.isair)
            {
              ++index3;
              ++v1_1;
            }
            else
              break;
          }
          if (index3 < enumInsideOutsideArray.Length)
          {
            air.rawData = blocks[index3];
            if (air.ischild)
            {
              int type = air.type;
              while (index3 > 0)
              {
                air.rawData = blocks[index3];
                if (air.type == type)
                {
                  ++index3;
                  ++v1_1;
                }
                else
                  break;
              }
            }
          }
        }
        for (int x = _min.x; x <= v1_1; ++x)
        {
          int index4 = x - _min.x + (y - _min.y) * this.size.x + (z - _min.z) * this.size.x * this.size.y;
          if (index4 >= 0 && index4 < enumInsideOutsideArray.Length)
            enumInsideOutsideArray[index4] = EnumInsideOutside.Outside;
        }
        int v1_2 = _max.x;
        // ISSUE: explicit constructor call
        ((Ray) ref ray).\u002Ector(Vector3.zero, Vector3.left);
        bool flag2 = false;
        for (float num5 = 0.0f; !flag2 && (double) num5 <= 1.0; num5 += 0.25f)
        {
          for (float num6 = 0.0f; !flag2 && (double) num6 <= 1.0; num6 += 0.25f)
          {
            ((Ray) ref ray).origin = new Vector3((float) (_max.x + 3), (float) y + num5, (float) z + num6);
            if (this.doRaycast(ray, out hitInfo, _min))
            {
              v1_2 = Utils.FastMin(v1_2, Utils.Fastfloor(((RaycastHit) ref hitInfo).point.x + ((Ray) ref ray).direction.x * 0.1f));
            }
            else
            {
              v1_2 = _min.x;
              flag2 = true;
            }
          }
        }
        int index5 = v1_2 - _min.x + (y - _min.y) * this.size.x + (z - _min.z) * this.size.x * this.size.y;
        if (index5 >= 0 && index5 < enumInsideOutsideArray.Length)
        {
          while (index5 > 0)
          {
            air.rawData = blocks[index5];
            if (air.isair)
            {
              --index5;
              --v1_2;
            }
            else
              break;
          }
          if (index5 > 0)
          {
            air.rawData = blocks[index5];
            if (air.ischild)
            {
              int type = air.type;
              while (index5 > 0)
              {
                air.rawData = blocks[index5];
                if (air.type == type)
                {
                  --index5;
                  --v1_2;
                }
                else
                  break;
              }
            }
          }
        }
        for (int x = _max.x; x >= v1_2; --x)
        {
          int index6 = x - _min.x + (y - _min.y) * this.size.x + (z - _min.z) * this.size.x * this.size.y;
          if (index6 >= 0 && index6 < enumInsideOutsideArray.Length)
            enumInsideOutsideArray[index6] = EnumInsideOutside.Outside;
        }
      }
    }
    for (int x = _min.x; x <= _max.x; ++x)
    {
      for (int y = _min.y; y <= _max.y; ++y)
      {
        int v1_3 = _min.z;
        // ISSUE: explicit constructor call
        ((Ray) ref ray).\u002Ector(Vector3.zero, Vector3.forward);
        bool flag3 = false;
        for (float num7 = 0.0f; !flag3 && (double) num7 <= 1.0; num7 += 0.25f)
        {
          for (float num8 = 0.0f; !flag3 && (double) num8 <= 1.0; num8 += 0.25f)
          {
            ((Ray) ref ray).origin = new Vector3((float) x + num7, (float) y + num8, (float) (_min.z - 3));
            if (this.doRaycast(ray, out hitInfo, _min))
            {
              v1_3 = Utils.FastMax(v1_3, Utils.Fastfloor(((RaycastHit) ref hitInfo).point.z + ((Ray) ref ray).direction.z * 0.1f));
            }
            else
            {
              v1_3 = _max.z;
              flag3 = true;
            }
          }
        }
        int index7 = x - _min.x + (y - _min.y) * this.size.x + (v1_3 - _min.z) * this.size.x * this.size.y;
        if (index7 >= 0 && index7 < enumInsideOutsideArray.Length)
        {
          while (index7 < blocks.Length - 1)
          {
            air.rawData = blocks[index7];
            if (air.isair)
            {
              index7 += this.size.x * this.size.y;
              ++v1_3;
            }
            else
              break;
          }
          if (index7 < enumInsideOutsideArray.Length)
          {
            air.rawData = blocks[index7];
            if (air.ischild)
            {
              int type = air.type;
              while (index7 > 0)
              {
                air.rawData = blocks[index7];
                if (air.type == type)
                {
                  index7 += this.size.x * this.size.y;
                  ++v1_3;
                }
                else
                  break;
              }
            }
          }
        }
        Debug.DrawLine(((Ray) ref ray).origin, new Vector3(((Ray) ref ray).origin.x, ((Ray) ref ray).origin.y, (float) v1_3), Color.blue, 10f);
        for (int z = _min.z; z <= v1_3; ++z)
        {
          int index8 = x - _min.x + (y - _min.y) * this.size.x + (z - _min.z) * this.size.x * this.size.y;
          if (index8 >= 0 && index8 < enumInsideOutsideArray.Length)
            enumInsideOutsideArray[index8] = EnumInsideOutside.Outside;
        }
        int v1_4 = _max.z;
        // ISSUE: explicit constructor call
        ((Ray) ref ray).\u002Ector(Vector3.zero, Vector3.back);
        bool flag4 = false;
        for (float num9 = 0.0f; !flag4 && (double) num9 <= 1.0; num9 += 0.25f)
        {
          for (float num10 = 0.0f; !flag4 && (double) num10 <= 1.0; num10 += 0.25f)
          {
            ((Ray) ref ray).origin = new Vector3((float) x + num9, (float) y + num10, (float) (_max.z + 3));
            if (this.doRaycast(ray, out hitInfo, _min))
            {
              v1_4 = Utils.FastMin(v1_4, Utils.Fastfloor(((RaycastHit) ref hitInfo).point.z + ((Ray) ref ray).direction.z * 0.1f));
            }
            else
            {
              v1_4 = _min.z;
              flag4 = true;
            }
          }
        }
        int index9 = x - _min.x + (y - _min.y) * this.size.x + (v1_4 - _min.z) * this.size.x * this.size.y;
        if (index9 >= 0 && index9 < enumInsideOutsideArray.Length)
        {
          while (index9 > 0)
          {
            air.rawData = blocks[index9];
            if (air.isair)
            {
              index9 -= this.size.x * this.size.y;
              --v1_3;
            }
            else
              break;
          }
          if (index9 > 0)
          {
            air.rawData = blocks[index9];
            if (air.ischild)
            {
              int type = air.type;
              while (index9 > 0)
              {
                air.rawData = blocks[index9];
                if (air.type == type)
                {
                  index9 -= this.size.x * this.size.y;
                  --v1_3;
                }
                else
                  break;
              }
            }
          }
        }
        for (int z = _max.z; z >= v1_4; --z)
        {
          int index10 = x - _min.x + (y - _min.y) * this.size.x + (z - _min.z) * this.size.x * this.size.y;
          if (index10 >= 0 && index10 < enumInsideOutsideArray.Length)
            enumInsideOutsideArray[index10] = EnumInsideOutside.Outside;
        }
      }
    }
    return enumInsideOutsideArray;
  }

  public void RecalcInsideDevices(EnumInsideOutside[] eInsideOutside)
  {
    this.insidePos.Init(this.size);
    if (!this.IsCullThisPrefab())
      return;
    int blockCount = this.GetBlockCount();
    for (int _offset = 0; _offset < blockCount; ++_offset)
    {
      int _x;
      int _y;
      int _z;
      this.offsetToCoord(_offset, out _x, out _y, out _z);
      if (!this.GetBlock(_x, _y, _z).Block.shape.IsTerrain() && eInsideOutside[_offset] == EnumInsideOutside.Inside)
        this.insidePos.Add(_offset);
    }
  }

  public Vector3i? GetFirstIndexedBlockOffsetOfType(string _indexName)
  {
    List<Vector3i> vector3iList;
    return this.indexedBlockOffsets.TryGetValue(_indexName, out vector3iList) && vector3iList.Count > 0 ? new Vector3i?(vector3iList[0]) : new Vector3i?();
  }

  public IChunk GetChunkSync(int chunkX, int chunkY, int chunkZ) => this.GetChunk(chunkX, chunkZ);

  public IChunk GetChunkFromWorldPos(int x, int y, int z)
  {
    return this.GetChunk(x / 16 /*0x10*/, z / 16 /*0x10*/);
  }

  public IChunk GetChunkFromWorldPos(Vector3i _blockPos)
  {
    return this.GetChunk(_blockPos.x / 16 /*0x10*/, _blockPos.z / 16 /*0x10*/);
  }

  public IEnumerator ToTransform(
    bool _genBlockModels,
    bool _genTerrain,
    bool _genBlockShapes,
    bool _fillEmptySpace,
    Transform _parent,
    string _name,
    Vector3 _position,
    int _heightLimit = 0)
  {
    Prefab _prefab = this;
    MicroStopwatch ms = new MicroStopwatch(true);
    GameObject _go = new GameObject();
    ((Object) _go).name = _name;
    _go.transform.SetParent(_parent);
    int ySize = 8;
    if (_heightLimit == 0)
      _heightLimit = _prefab.size.y;
    else if (_heightLimit < 0)
      _heightLimit = -_prefab.yOffset - _heightLimit;
    _heightLimit = Mathf.Clamp(_heightLimit, 0, _prefab.size.y);
    int y = 0;
    int y2 = 0;
    while (y < _heightLimit + 1)
    {
      int x = 0;
      int x2 = 0;
      while (x < _prefab.size.x + 1)
      {
        int z = 0;
        int z2 = 0;
        while (z < _prefab.size.z + 1 && !Object.op_Equality((Object) _go, (Object) null))
        {
          GameObject gameObject1 = new GameObject();
          gameObject1.transform.parent = _go.transform;
          ((Object) gameObject1).name = $"Chunk[{x2},{z2}]";
          MeshFilter[][] meshFilterArray = new MeshFilter[MeshDescription.meshes.Length][];
          MeshRenderer[][] meshRendererArray = new MeshRenderer[MeshDescription.meshes.Length][];
          MeshCollider[][] meshColliderArray = new MeshCollider[MeshDescription.meshes.Length][];
          GameObject[] gameObjectArray = new GameObject[MeshDescription.meshes.Length];
          GameObject gameObject2 = new GameObject("_BlockEntities");
          GameObject gameObject3 = new GameObject("Meshes");
          gameObject2.transform.parent = gameObject1.transform;
          gameObject3.transform.parent = gameObject1.transform;
          for (int _meshIndex = 0; _meshIndex < MeshDescription.meshes.Length; ++_meshIndex)
          {
            gameObjectArray[_meshIndex] = new GameObject(MeshDescription.meshes[_meshIndex].Name);
            gameObjectArray[_meshIndex].transform.parent = gameObject3.transform;
            VoxelMesh.CreateMeshFilter(_meshIndex, 0, gameObjectArray[_meshIndex], MeshDescription.meshes[_meshIndex].Tag, false, out meshFilterArray[_meshIndex], out meshRendererArray[_meshIndex], out meshColliderArray[_meshIndex]);
          }
          VoxelMesh[] _meshes = new VoxelMesh[6];
          for (int _meshIndex = 0; _meshIndex < _meshes.Length; ++_meshIndex)
          {
            if (_meshIndex == 5)
              _meshes[_meshIndex] = (VoxelMesh) new VoxelMeshTerrain(_meshIndex)
              {
                IsPreviewVoxelMesh = true
              };
            else
              _meshes[_meshIndex] = new VoxelMesh(_meshIndex);
          }
          MeshGeneratorPrefab meshGeneratorPrefab = new MeshGeneratorPrefab(_prefab);
          Vector3i _worldStartPos = new Vector3i(x, y, z);
          Vector3i _worldEndPos = new Vector3i(x + 15, y + ySize, z + 16 /*0x10*/);
          if (_genTerrain & _genBlockShapes)
            meshGeneratorPrefab.GenerateMeshOffset(_worldStartPos, _worldEndPos, _meshes);
          else if (!_genTerrain & _genBlockShapes)
            meshGeneratorPrefab.GenerateMeshNoTerrain(_worldStartPos, _worldEndPos, _meshes);
          else if (_genTerrain && !_genBlockShapes)
            meshGeneratorPrefab.GenerateMeshTerrainOnly(_worldStartPos, _worldEndPos, _meshes);
          for (int index = 0; index < _meshes.Length; ++index)
            _meshes[index].CopyToMesh(meshFilterArray[index], meshRendererArray[index], 0);
          if (_genBlockModels)
          {
            for (int _y = y; _y < y + ySize && _y < _prefab.size.y; ++_y)
            {
              for (int _x = x; _x < x + 16 /*0x10*/ && _x < _prefab.size.x; ++_x)
              {
                for (int _z = z; _z < z + 16 /*0x10*/ && _z < _prefab.size.z; ++_z)
                {
                  Vector3i vector3i = new Vector3i(_x, _y, _z);
                  BlockValue block1 = _prefab.GetBlock(_x, _y, _z);
                  if (!block1.ischild)
                  {
                    Block block2 = block1.Block;
                    if (block2.shape is BlockShapeModelEntity shape)
                    {
                      Quaternion rotation = shape.GetRotation(block1);
                      Vector3 rotatedOffset = shape.GetRotatedOffset(block2, rotation);
                      rotatedOffset.x += 0.5f;
                      rotatedOffset.z += 0.5f;
                      rotatedOffset.y += 0.0f;
                      Vector3 vector3 = Vector3.op_Addition(vector3i.ToVector3(), rotatedOffset);
                      GameObject objectForType = GameObjectPool.Instance.GetObjectForType(shape.modelName);
                      if (!Object.op_Equality((Object) objectForType, (Object) null))
                      {
                        Transform transform = objectForType.transform;
                        transform.parent = gameObject2.transform;
                        transform.localScale = Vector3.one;
                        transform.localPosition = vector3;
                        transform.localRotation = rotation;
                      }
                    }
                  }
                }
              }
            }
          }
          yield return (object) null;
          z += 16 /*0x10*/;
          ++z2;
        }
        x += 16 /*0x10*/;
        ++x2;
      }
      y += ySize;
      ++y2;
    }
    if (Object.op_Inequality((Object) _go, (Object) null))
      _go.transform.localPosition = new Vector3(_position.x * _go.transform.localScale.x, _position.y * _go.transform.localScale.y, _position.z * _go.transform.localScale.z);
    Log.Out($"Prefab preview generation took {(ValueType) (float) ((double) ((Stopwatch) ms).ElapsedMilliseconds / 1000.0)} seconds.");
  }

  public void HandleAddingTriggerLayers(BlockTrigger trigger)
  {
    for (int index = 0; index < trigger.TriggersIndices.Count; ++index)
    {
      if (!this.TriggerLayers.Contains(trigger.TriggersIndices[index]))
        this.TriggerLayers.Add(trigger.TriggersIndices[index]);
    }
    for (int index = 0; index < trigger.TriggeredByIndices.Count; ++index)
    {
      if (!this.TriggerLayers.Contains(trigger.TriggeredByIndices[index]))
        this.TriggerLayers.Add(trigger.TriggeredByIndices[index]);
    }
  }

  public void HandleAddingTriggerLayers(Prefab.PrefabTriggerVolume trigger)
  {
    for (int index = 0; index < trigger.TriggersIndices.Count; ++index)
    {
      if (!this.TriggerLayers.Contains(trigger.TriggersIndices[index]))
        this.TriggerLayers.Add(trigger.TriggersIndices[index]);
    }
  }

  public void AddInitialTriggerLayers()
  {
    for (byte index = 1; index < (byte) 6; ++index)
      this.TriggerLayers.Add(index);
  }

  public void AddNewTriggerLayer()
  {
    this.TriggerLayers = this.TriggerLayers.OrderBy<byte, byte>((Func<byte, byte>) ([PublicizedFrom(EAccessModifier.Internal)] (i) => i)).ToList<byte>();
    if (this.TriggerLayers.Count > 0)
    {
      int num = (int) this.TriggerLayers[this.TriggerLayers.Count - 1] + 1;
      if (num >= (int) byte.MaxValue || num <= 0)
        return;
      this.TriggerLayers.Add((byte) num);
    }
    else
      this.TriggerLayers.Add((byte) 1);
  }

  [Conditional("DEBUG_PREFABLOG")]
  [PublicizedFrom(EAccessModifier.Private)]
  public static void LogPrefab(string format, params object[] args)
  {
    format = $"{GameManager.frameCount} Prefab {format}";
    Log.Warning(format, args);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  static Prefab()
  {
  }

  [CompilerGenerated]
  [PublicizedFrom(EAccessModifier.Private)]
  public void \u003CwriteToProperties\u003Eb__194_0(string _key)
  {
    this.properties.Values.Remove(_key);
  }

  public struct BlockStatistics
  {
    public int cntWindows;
    public int cntDoors;
    public int cntBlockEntities;
    public int cntBlockModels;
    public int cntSolid;

    public void Clear()
    {
      this.cntWindows = 0;
      this.cntDoors = 0;
      this.cntBlockEntities = 0;
      this.cntSolid = 0;
      this.cntBlockModels = 0;
    }

    public override string ToString()
    {
      return $"Blocks: {this.cntSolid} BEnts: {this.cntBlockEntities} BMods: {this.cntBlockModels} Wdws: {this.cntWindows}";
    }
  }

  public class PrefabSleeperVolume
  {
    public bool used;
    public Vector3i startPos;
    public Vector3i size;
    public string groupName;
    public bool isPriority;
    public bool isQuestExclude;
    public short spawnCountMin;
    public short spawnCountMax;
    public short groupId;
    public int flags;
    public string minScript;
    public List<byte> triggeredByIndices = new List<byte>();

    public PrefabSleeperVolume()
    {
    }

    public PrefabSleeperVolume(Prefab.PrefabSleeperVolume _other)
    {
      this.used = _other.used;
      this.startPos = _other.startPos;
      this.size = _other.size;
      this.groupId = _other.groupId;
      this.groupName = _other.groupName;
      this.isPriority = _other.isPriority;
      this.isQuestExclude = _other.isQuestExclude;
      this.spawnCountMin = _other.spawnCountMin;
      this.spawnCountMax = _other.spawnCountMax;
      this.triggeredByIndices = _other.triggeredByIndices;
      this.flags = _other.flags;
      this.minScript = _other.minScript;
    }

    public void Use(
      Vector3i _startPos,
      Vector3i _size,
      short _groupId,
      string _groupName,
      bool _isPriority,
      bool _isQuestExclude,
      int _spawnMin,
      int _spawnMax,
      int _flags)
    {
      this.used = true;
      this.startPos = _startPos;
      this.size = _size;
      this.groupId = _groupId;
      this.groupName = _groupName;
      this.isPriority = _isPriority;
      this.isQuestExclude = _isQuestExclude;
      this.spawnCountMin = (short) _spawnMin;
      this.spawnCountMax = (short) _spawnMax;
      this.flags = _flags;
    }

    public void SetTrigger(SleeperVolume.ETriggerType type)
    {
      this.flags = (int) ((SleeperVolume.ETriggerType) (this.flags & -8) | type);
    }

    public void SetTriggeredByFlag(byte index)
    {
      if (this.triggeredByIndices.Contains(index))
        return;
      this.triggeredByIndices.Add(index);
    }

    public void ClearTriggeredBy() => this.triggeredByIndices.Clear();

    public void RemoveTriggeredByFlag(byte index) => this.triggeredByIndices.Remove(index);

    public bool HasTriggeredBy(byte index) => this.triggeredByIndices.Contains(index);

    public bool HasAnyTriggeredBy() => this.triggeredByIndices.Count > 0;
  }

  public class PrefabTeleportVolume
  {
    public Vector3i startPos;
    public Vector3i size;
    public bool used;

    public PrefabTeleportVolume()
    {
    }

    public PrefabTeleportVolume(Prefab.PrefabTeleportVolume _other)
    {
      this.startPos = _other.startPos;
      this.size = _other.size;
    }

    public void Use(Vector3i _startPos, Vector3i _size)
    {
      this.used = true;
      this.startPos = _startPos;
      this.size = _size;
    }
  }

  public class PrefabInfoVolume
  {
    public Vector3i startPos;
    public Vector3i size;
    public bool used;

    public PrefabInfoVolume()
    {
    }

    public PrefabInfoVolume(Prefab.PrefabInfoVolume _other)
    {
      this.startPos = _other.startPos;
      this.size = _other.size;
    }

    public void Use(Vector3i _startPos, Vector3i _size)
    {
      this.used = true;
      this.startPos = _startPos;
      this.size = _size;
    }
  }

  public class PrefabWallVolume
  {
    public Vector3i startPos;
    public Vector3i size;

    public PrefabWallVolume()
    {
    }

    public PrefabWallVolume(Prefab.PrefabWallVolume _other)
    {
      this.startPos = _other.startPos;
      this.size = _other.size;
    }

    public void Use(Vector3i _startPos, Vector3i _size)
    {
      this.startPos = _startPos;
      this.size = _size;
    }
  }

  [Preserve]
  public class PrefabTriggerVolume
  {
    public Vector3i startPos;
    public Vector3i size;
    public PrefabTriggerData TriggerDataOwner;
    public List<byte> TriggersIndices = new List<byte>();
    public bool used;

    public PrefabTriggerVolume()
    {
    }

    public PrefabTriggerVolume(Prefab.PrefabTriggerVolume _other)
    {
      this.startPos = _other.startPos;
      this.size = _other.size;
      this.TriggersIndices = _other.TriggersIndices;
    }

    public void Use(Vector3i _startPos, Vector3i _size)
    {
      this.startPos = _startPos;
      this.size = _size;
      this.used = true;
    }

    public void SetTriggersFlag(byte index)
    {
      if (this.TriggersIndices.Contains(index))
        return;
      this.TriggersIndices.Add(index);
    }

    public void RemoveTriggersFlag(byte index) => this.TriggersIndices.Remove(index);

    public void RemoveAllTriggersFlags() => this.TriggersIndices.Clear();

    public bool HasTriggers(byte index) => this.TriggersIndices.Contains(index);

    public bool HasAnyTriggers() => this.TriggersIndices.Count > 0;
  }

  public class Marker
  {
    [PublicizedFrom(EAccessModifier.Private)]
    public Vector3i start;
    [PublicizedFrom(EAccessModifier.Private)]
    public Vector3i size;
    [PublicizedFrom(EAccessModifier.Private)]
    public Prefab.Marker.MarkerTypes markerType;
    [PublicizedFrom(EAccessModifier.Private)]
    public string name;
    [PublicizedFrom(EAccessModifier.Private)]
    public FastTags<TagGroup.Poi> tags;
    [PublicizedFrom(EAccessModifier.Private)]
    public string partToSpawn;
    [PublicizedFrom(EAccessModifier.Private)]
    public byte rotations;
    [PublicizedFrom(EAccessModifier.Private)]
    public float partChanceToSpawn = 1f;
    public bool PartDirty = true;
    [PublicizedFrom(EAccessModifier.Private)]
    public int groupId = -1;
    [PublicizedFrom(EAccessModifier.Private)]
    public string groupName;
    [PublicizedFrom(EAccessModifier.Private)]
    public Color color;
    public static List<Vector3i> MarkerSizes = new List<Vector3i>()
    {
      Vector3i.one,
      new Vector3i(25, 0, 25),
      new Vector3i(42, 0, 42),
      new Vector3i(60, 0, 60),
      new Vector3i(100, 0, 100)
    };

    public string GroupName
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get => this.groupName;
      set
      {
        if (!(this.groupName != value))
          return;
        this.color = new Color();
        this.groupId = -1;
        this.groupName = value;
      }
    }

    public Color GroupColor
    {
      get
      {
        if (Color.op_Equality(this.color, new Color()))
        {
          GameRandom tempGameRandom = GameRandomManager.Instance.GetTempGameRandom(this.GroupId);
          this.color = Color32.op_Implicit(new Color32((byte) tempGameRandom.RandomRange(0, 256 /*0x0100*/), (byte) tempGameRandom.RandomRange(0, 256 /*0x0100*/), (byte) tempGameRandom.RandomRange(0, 256 /*0x0100*/), this.MarkerType == Prefab.Marker.MarkerTypes.PartSpawn ? (byte) 32 /*0x20*/ : (byte) 128 /*0x80*/));
        }
        return this.color;
      }
    }

    public int GroupId
    {
      get
      {
        if (this.groupId == -1)
          this.groupId = this.GroupName.GetHashCode();
        return this.groupId;
      }
    }

    public Vector3i Start
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get => this.start;
      set
      {
        if (!(this.start != value))
          return;
        this.start = value;
      }
    }

    public Vector3i Size
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get => this.size;
      set
      {
        if (!(this.size != value))
          return;
        this.size = value;
        this.PartDirty = true;
      }
    }

    public Prefab.Marker.MarkerTypes MarkerType
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get => this.markerType;
      set
      {
        if (this.markerType == value)
          return;
        this.markerType = value;
        this.PartDirty = true;
      }
    }

    public string Name
    {
      get => this.name;
      set
      {
        if (!(this.name != value))
          return;
        this.name = value;
      }
    }

    public FastTags<TagGroup.Poi> Tags
    {
      get => this.tags;
      set
      {
        if (this.tags.Equals(value))
          return;
        this.tags = value;
      }
    }

    public string PartToSpawn
    {
      get => this.partToSpawn;
      set
      {
        if (!(this.partToSpawn != value))
          return;
        this.partToSpawn = value;
        this.PartDirty = true;
      }
    }

    public byte Rotations
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get => this.rotations;
      set
      {
        if ((int) this.rotations == (int) value)
          return;
        this.rotations = value;
        this.PartDirty = true;
      }
    }

    public float PartChanceToSpawn
    {
      get => this.partChanceToSpawn;
      set
      {
        if ((double) this.rotations == (double) value)
          return;
        this.partChanceToSpawn = value;
        this.PartDirty = true;
      }
    }

    public Marker()
    {
    }

    public Marker(
      Vector3i _start,
      Vector3i _size,
      Prefab.Marker.MarkerTypes _type,
      string _group,
      FastTags<TagGroup.Poi> _tags)
    {
      this.Start = _start;
      this.Size = _size;
      this.MarkerType = _type;
      this.GroupName = _group;
      this.Tags = _tags;
    }

    public Marker(Prefab.Marker _other)
    {
      this.Start = _other.Start;
      this.Size = _other.Size;
      this.MarkerType = _other.MarkerType;
      this.GroupName = _other.GroupName;
      this.Tags = _other.Tags;
      this.Name = _other.Name;
      this.PartToSpawn = _other.PartToSpawn;
      this.Rotations = _other.Rotations;
      this.PartChanceToSpawn = _other.PartChanceToSpawn;
    }

    [PublicizedFrom(EAccessModifier.Private)]
    static Marker()
    {
    }

    public enum MarkerTypes : byte
    {
      None,
      POISpawn,
      RoadExit,
      PartSpawn,
    }

    public enum MarkerSize : byte
    {
      One,
      ExtraSmall,
      Small,
      Medium,
      Large,
      Custom,
    }
  }

  public struct Data
  {
    public uint[] m_Blocks;
    public ushort[] m_Damage;
    public byte[] m_Density;
    public TextureFullArray[] m_Textures;
    public WaterValue[] m_Water;

    public void Init(int _count)
    {
      this.m_Blocks = new uint[_count];
      this.m_Damage = new ushort[_count];
      this.m_Density = new byte[_count];
      this.m_Textures = new TextureFullArray[_count];
      this.m_Water = new WaterValue[_count];
    }

    public void Expand(int _count)
    {
      int length = this.m_Blocks != null ? this.m_Blocks.Length : 0;
      if (_count > length)
      {
        this.m_Blocks = new uint[_count];
        this.m_Damage = new ushort[_count];
        this.m_Density = new byte[_count];
        this.m_Textures = new TextureFullArray[_count];
        this.m_Water = new WaterValue[_count];
      }
      for (int index = 0; index < _count; ++index)
      {
        this.m_Textures[index].Fill(0L);
        this.m_Water[index] = WaterValue.Empty;
      }
    }
  }

  public class Cells<T> where T : unmanaged
  {
    public T defaultValue;
    [PublicizedFrom(EAccessModifier.Private)]
    public int sizeY;
    public Prefab.Cells<T>.CellsAtZ[] a;
    [PublicizedFrom(EAccessModifier.Private)]
    public static byte[] cellBytes = new byte[256 /*0x0100*/];

    public Cells(int _sizeY, T _defaultValue)
    {
      this.sizeY = _sizeY;
      this.defaultValue = _defaultValue;
    }

    [PublicizedFrom(EAccessModifier.Private)]
    public Cells(Prefab.Cells<T> _template)
    {
      this.sizeY = _template.sizeY;
      this.defaultValue = _template.defaultValue;
    }

    public Prefab.Cells<T>.Cell AllocCell(int _x, int _y, int _z)
    {
      if (this.a == null)
        this.a = new Prefab.Cells<T>.CellsAtZ[this.sizeY];
      Prefab.Cells<T>.CellsAtZ cellsAtZ = this.a[_y];
      if (cellsAtZ == null)
      {
        cellsAtZ = new Prefab.Cells<T>.CellsAtZ();
        this.a[_y] = cellsAtZ;
      }
      int index1 = _z >> 2;
      if (cellsAtZ.a == null || index1 >= cellsAtZ.a.Length)
        Array.Resize<Prefab.Cells<T>.CellsAtX>(ref cellsAtZ.a, index1 + 1);
      Prefab.Cells<T>.CellsAtX cellsAtX = cellsAtZ.a[index1];
      if (cellsAtX == null)
      {
        cellsAtX = new Prefab.Cells<T>.CellsAtX();
        cellsAtZ.a[index1] = cellsAtX;
      }
      int index2 = _x >> 2;
      if (cellsAtX.a == null || index2 >= cellsAtX.a.Length)
        Array.Resize<Prefab.Cells<T>.Cell>(ref cellsAtX.a, index2 + 1);
      Prefab.Cells<T>.Cell cell = cellsAtX.a[index2];
      if (cell.a == null)
      {
        cell = new Prefab.Cells<T>.Cell(this.defaultValue);
        cellsAtX.a[index2] = cell;
      }
      return cell;
    }

    public Prefab.Cells<T>.Cell GetCell(int _x, int _y, int _z)
    {
      if (this.a == null)
        return Prefab.Cells<T>.Cell.empty;
      Prefab.Cells<T>.CellsAtZ cellsAtZ = this.a[_y];
      if (cellsAtZ == null)
        return Prefab.Cells<T>.Cell.empty;
      int index1 = _z >> 2;
      if (cellsAtZ.a == null || index1 >= cellsAtZ.a.Length)
        return Prefab.Cells<T>.Cell.empty;
      Prefab.Cells<T>.CellsAtX cellsAtX = cellsAtZ.a[index1];
      if (cellsAtX == null)
        return Prefab.Cells<T>.Cell.empty;
      int index2 = _x >> 2;
      return cellsAtX.a == null || index2 >= cellsAtX.a.Length ? Prefab.Cells<T>.Cell.empty : cellsAtX.a[index2];
    }

    public T GetData(int _x, int _y, int _z)
    {
      Prefab.Cells<T>.Cell cell = this.GetCell(_x, _y, _z);
      return cell.a == null ? this.defaultValue : cell.Get(_x, _z);
    }

    public void SetData(int _x, int _y, int _z, T _data)
    {
      this.AllocCell(_x, _y, _z).Set(_x, _z, _data);
    }

    public Prefab.Cells<T> Clone()
    {
      Prefab.Cells<T> cells = new Prefab.Cells<T>(this.sizeY, this.defaultValue);
      if (this.a == null)
        return cells;
      cells.a = new Prefab.Cells<T>.CellsAtZ[this.sizeY];
      for (int index1 = 0; index1 < this.sizeY; ++index1)
      {
        Prefab.Cells<T>.CellsAtZ cellsAtZ1 = this.a[index1];
        if (cellsAtZ1 != null)
        {
          Prefab.Cells<T>.CellsAtZ cellsAtZ2 = new Prefab.Cells<T>.CellsAtZ();
          cellsAtZ2.a = new Prefab.Cells<T>.CellsAtX[cellsAtZ1.a.Length];
          cells.a[index1] = cellsAtZ2;
          for (int index2 = 0; index2 < cellsAtZ1.a.Length; ++index2)
          {
            Prefab.Cells<T>.CellsAtX cellsAtX1 = cellsAtZ1.a[index2];
            if (cellsAtX1 != null)
            {
              Prefab.Cells<T>.CellsAtX cellsAtX2 = new Prefab.Cells<T>.CellsAtX();
              cellsAtX2.a = new Prefab.Cells<T>.Cell[cellsAtX1.a.Length];
              cellsAtZ2.a[index2] = cellsAtX2;
              for (int index3 = 0; index3 < cellsAtX1.a.Length; ++index3)
              {
                Prefab.Cells<T>.Cell cell = cellsAtX1.a[index3];
                if (cell.a != null)
                  cellsAtX2.a[index3] = cell.Clone();
              }
            }
          }
        }
      }
      return cells;
    }

    public void Stats(
      out int _arrayCount,
      out int _arraySize,
      out int _cellsCount,
      out int _cellsSize,
      out int _usedCount)
    {
      _arrayCount = 0;
      _arraySize = 0;
      _cellsCount = 0;
      _cellsSize = 0;
      _usedCount = 0;
      int length = this.a != null ? this.a.Length : 0;
      _arrayCount += length;
      _arraySize += length * 8 + 8;
      for (int index1 = 0; index1 < length; ++index1)
      {
        Prefab.Cells<T>.CellsAtZ cellsAtZ = this.a[index1];
        if (cellsAtZ != null)
        {
          _arrayCount += cellsAtZ.a.Length;
          _arraySize += cellsAtZ.a.Length * 8 + 8;
          for (int index2 = 0; index2 < cellsAtZ.a.Length; ++index2)
          {
            Prefab.Cells<T>.CellsAtX cellsAtX = cellsAtZ.a[index2];
            if (cellsAtX != null)
            {
              _arrayCount += cellsAtX.a.Length;
              _arraySize += cellsAtX.a.Length * 8 + 8;
              for (int index3 = 0; index3 < cellsAtX.a.Length; ++index3)
              {
                Prefab.Cells<T>.Cell cell = cellsAtX.a[index3];
                if (cell.a != null)
                {
                  _cellsCount += 16 /*0x10*/;
                  _cellsSize += cell.Size();
                  _usedCount += cell.UsedCount(this.defaultValue);
                }
              }
            }
          }
        }
      }
    }

    public void Load(PooledBinaryReader _br)
    {
      Array.Clear((Array) this.a, 0, this.a.Length);
      if (_br.ReadByte() != (byte) 1 || _br.ReadUInt16() <= (ushort) 0)
        return;
      this.LoadData(_br);
    }

    [PublicizedFrom(EAccessModifier.Private)]
    public void LoadData(PooledBinaryReader _br)
    {
      while (true)
      {
        _br.Read(Prefab.Cells<T>.cellBytes, 0, 3);
        int cellByte = (int) Prefab.Cells<T>.cellBytes[0];
        if (cellByte != (int) byte.MaxValue)
        {
          int _x = (int) Prefab.Cells<T>.cellBytes[1] << 2;
          int _z = (int) Prefab.Cells<T>.cellBytes[2] << 2;
          this.AllocCell(_x, cellByte, _z).Load(_br);
        }
        else
          break;
      }
    }

    public void Save(BinaryWriter _bw)
    {
      ushort length = this.a != null ? (ushort) this.a.Length : (ushort) 0;
      if (length == (ushort) 0)
      {
        _bw.Write((byte) 0);
      }
      else
      {
        _bw.Write((byte) 1);
        _bw.Write(length);
        for (int index1 = 0; index1 < (int) length; ++index1)
        {
          Prefab.Cells<T>.CellsAtZ cellsAtZ = this.a[index1];
          if (cellsAtZ != null)
          {
            for (int index2 = 0; index2 < cellsAtZ.a.Length; ++index2)
            {
              Prefab.Cells<T>.CellsAtX cellsAtX = cellsAtZ.a[index2];
              if (cellsAtX != null)
              {
                for (int index3 = 0; index3 < cellsAtX.a.Length; ++index3)
                {
                  Prefab.Cells<T>.Cell cell = cellsAtX.a[index3];
                  if (cell.a != null)
                  {
                    Prefab.Cells<T>.cellBytes[0] = (byte) index1;
                    Prefab.Cells<T>.cellBytes[1] = (byte) index3;
                    Prefab.Cells<T>.cellBytes[2] = (byte) index2;
                    _bw.Write(Prefab.Cells<T>.cellBytes, 0, 3);
                    cell.Save(_bw);
                  }
                }
              }
            }
          }
        }
        Prefab.Cells<T>.cellBytes[0] = byte.MaxValue;
        _bw.Write(Prefab.Cells<T>.cellBytes, 0, 3);
      }
    }

    public T[] ToArray(Prefab prefab, Vector3i _size)
    {
      T[] array = new T[_size.x * _size.y * _size.z];
      int length = this.a != null ? this.a.Length : 0;
      for (int _y = 0; _y < length; ++_y)
      {
        Prefab.Cells<T>.CellsAtZ cellsAtZ = this.a[_y];
        if (cellsAtZ != null)
        {
          for (int index1 = 0; index1 < cellsAtZ.a.Length; ++index1)
          {
            Prefab.Cells<T>.CellsAtX cellsAtX = cellsAtZ.a[index1];
            if (cellsAtX != null)
            {
              for (int index2 = 0; index2 < cellsAtX.a.Length; ++index2)
              {
                Prefab.Cells<T>.Cell cell = cellsAtX.a[index2];
                if (cell.a != null)
                {
                  int num1 = index2 << 2;
                  int num2 = index1 << 2;
                  int num3 = Utils.FastMin(_size.x - num1, 4);
                  int num4 = Utils.FastMin(_size.z - num2, 4);
                  for (int _z = 0; _z < num4; ++_z)
                  {
                    for (int _x = 0; _x < num3; ++_x)
                    {
                      T obj = cell.Get(_x, _z);
                      int offset = prefab.CoordToOffset(0, num1 + _x, _y, num2 + _z);
                      array[offset] = obj;
                    }
                  }
                }
              }
            }
          }
        }
      }
      return array;
    }

    public unsafe void CompareTest(Vector3i size, PooledBinaryReader _br)
    {
      Prefab.Cells<T> cells = new Prefab.Cells<T>(this);
      cells.Load(_br);
      if (this.a.Length != cells.a.Length)
        Log.Error("Cells size");
      for (int _y = 0; _y < size.y; ++_y)
      {
        for (int _z1 = 0; _z1 < size.z; ++_z1)
        {
          for (int _x1 = 0; _x1 < size.x; ++_x1)
          {
            Prefab.Cells<T>.Cell cell1 = this.GetCell(_x1, _y, _z1);
            Prefab.Cells<T>.Cell cell2 = cells.GetCell(_x1, _y, _z1);
            if (cell1.a == null)
            {
              if (cell2.a != null)
                Log.Error("Cells one is null {0} {1} {2}", new object[3]
                {
                  (object) _x1,
                  (object) _y,
                  (object) _z1
                });
            }
            else if (cell2.a != null)
            {
              for (int _z2 = 0; _z2 < 4; ++_z2)
              {
                for (int _x2 = 0; _x2 < 4; ++_x2)
                {
                  T obj1 = cell1.Get(_x2, _z2);
                  T obj2 = cell2.Get(_x2, _z2);
                  byte* numPtr1 = (byte*) UnsafeUtility.AddressOf<T>(ref obj1);
                  byte* numPtr2 = (byte*) UnsafeUtility.AddressOf<T>(ref obj2);
                  for (int index = 0; index < sizeof (T); ++index)
                  {
                    if ((int) numPtr1[index] != (int) numPtr2[index])
                      Log.Error("Cells data {0} {1} {2}, {3} != {4}", new object[5]
                      {
                        (object) _x1,
                        (object) _y,
                        (object) _z1,
                        (object) numPtr1[index],
                        (object) numPtr2[index]
                      });
                  }
                }
              }
            }
          }
        }
      }
    }

    [PublicizedFrom(EAccessModifier.Private)]
    static Cells()
    {
    }

    public class CellsAtX
    {
      public Prefab.Cells<T>.Cell[] a;
    }

    public class CellsAtZ
    {
      public Prefab.Cells<T>.CellsAtX[] a;
    }

    public struct Cell
    {
      public const int cSizeXZ = 4;
      public const int cSizeArray = 16 /*0x10*/;
      public const int cSizeXZMask = 3;
      public const int cSizeXZShift = 2;
      public static Prefab.Cells<T>.Cell empty;
      public T[] a;

      public Cell(T _defaultValue)
      {
        this.a = new T[16 /*0x10*/];
        for (int index = 0; index < 16 /*0x10*/; ++index)
          this.a[index] = _defaultValue;
      }

      public Prefab.Cells<T>.Cell Clone()
      {
        Prefab.Cells<T>.Cell cell = new Prefab.Cells<T>.Cell();
        if (this.a != null)
        {
          cell.a = new T[16 /*0x10*/];
          for (int index = 0; index < 16 /*0x10*/; ++index)
            cell.a[index] = this.a[index];
        }
        return cell;
      }

      public override string ToString() => $"{(this.a != null ? this.a.Length : -1)}";

      public int Size() => 16 /*0x10*/ * sizeof (T);

      public unsafe int UsedCount(T _defaultValue)
      {
        int num = 0;
        byte* numPtr1 = (byte*) UnsafeUtility.AddressOf<T>(ref _defaultValue);
        for (int index1 = 0; index1 < 16 /*0x10*/; ++index1)
        {
          byte* numPtr2 = (byte*) UnsafeUtility.AddressOf<T>(ref this.a[index1]);
          for (int index2 = 0; index2 < sizeof (T); ++index2)
          {
            if ((int) numPtr1[index2] != (int) numPtr2[index2])
            {
              ++num;
              break;
            }
          }
        }
        return num;
      }

      public void Set(int _x, int _z, T _value) => this.a[(_x & 3) + ((_z & 3) << 2)] = _value;

      public T Get(int _x, int _z) => this.a[(_x & 3) + ((_z & 3) << 2)];

      public unsafe void Load(PooledBinaryReader _br)
      {
        int position = (int) _br.BaseStream.Position;
        int count = (int) _br.ReadUInt16();
        _br.Read(Prefab.Cells<T>.cellBytes, 0, count);
        Log.Warning("Cell Load at {0}, count{1}", new object[2]
        {
          (object) position,
          (object) count
        });
        int num1 = 0;
        byte* numPtr = (byte*) UnsafeUtility.AddressOf<T>(ref this.a[0]);
        int num2 = 0;
        while (num2 < count)
        {
          int cellByte1 = (int) (sbyte) Prefab.Cells<T>.cellBytes[num2++];
          if (cellByte1 >= 0)
          {
            for (int index = 0; index < cellByte1; ++index)
            {
              byte cellByte2 = Prefab.Cells<T>.cellBytes[num2++];
              numPtr[num1++] = cellByte2;
            }
          }
          else
          {
            int num3 = -cellByte1;
            byte cellByte3 = Prefab.Cells<T>.cellBytes[num2++];
            for (int index = 0; index < num3; ++index)
              numPtr[num1++] = cellByte3;
          }
        }
      }

      public unsafe void Save(BinaryWriter _bw)
      {
        byte* numPtr = (byte*) UnsafeUtility.AddressOf<T>(ref this.a[0]);
        int count = 0;
        int num1 = this.a.Length * sizeof (T);
        int index1 = 0;
        while (index1 < num1)
        {
          int num2 = 1;
          byte num3 = numPtr[index1];
          if (index1 + 1 < num1)
          {
            byte num4 = numPtr[index1 + 1];
            if ((int) num3 == (int) num4)
            {
              int num5 = 2;
              for (int index2 = index1 + 2; index2 < num1 && (int) numPtr[index2] == (int) num3; ++index2)
              {
                ++num5;
                if (num5 >= 128 /*0x80*/)
                  break;
              }
              if (num5 >= 3)
              {
                num2 = -num5;
                index1 += num5;
              }
              else
                num2 = num5;
            }
            if (num2 >= 0)
            {
              for (int index3 = index1 + num2; index3 < num1; ++index3)
              {
                byte num6 = numPtr[index3];
                if (index3 + 2 >= num1 || (int) num6 != (int) numPtr[index3 + 1] || (int) num6 != (int) numPtr[index3 + 2])
                {
                  ++num2;
                  if (num2 >= (int) sbyte.MaxValue)
                    break;
                }
                else
                  break;
              }
            }
          }
          Prefab.Cells<T>.cellBytes[count++] = (byte) num2;
          if (num2 >= 0)
          {
            for (int index4 = 0; index4 < num2; ++index4)
            {
              byte num7 = numPtr[index1++];
              Prefab.Cells<T>.cellBytes[count++] = num7;
            }
          }
          else
            Prefab.Cells<T>.cellBytes[count++] = num3;
        }
        _bw.Write((ushort) count);
        _bw.Write(Prefab.Cells<T>.cellBytes, 0, count);
      }

      [PublicizedFrom(EAccessModifier.Private)]
      static Cell()
      {
      }
    }

    [PublicizedFrom(EAccessModifier.Private)]
    public enum DataFormat
    {
      Empty,
      RLE,
    }
  }

  public class PrefabChunk : IChunk
  {
    [PublicizedFrom(EAccessModifier.Private)]
    public Prefab prefab;
    [CompilerGenerated]
    [PublicizedFrom(EAccessModifier.Private)]
    public int \u003CX\u003Ek__BackingField;
    [CompilerGenerated]
    [PublicizedFrom(EAccessModifier.Private)]
    public int \u003CY\u003Ek__BackingField;
    [CompilerGenerated]
    [PublicizedFrom(EAccessModifier.Private)]
    public int \u003CZ\u003Ek__BackingField;
    [CompilerGenerated]
    [PublicizedFrom(EAccessModifier.Private)]
    public Vector3i \u003CChunkPos\u003Ek__BackingField;

    public PrefabChunk(Prefab _prefab, int _x, int _z)
    {
      this.prefab = _prefab;
      this.X = _x;
      this.Z = _z;
      this.Y = 0;
    }

    public int X
    {
      get => this.\u003CX\u003Ek__BackingField;
      set => this.\u003CX\u003Ek__BackingField = value;
    }

    public int Y
    {
      get => this.\u003CY\u003Ek__BackingField;
      set => this.\u003CY\u003Ek__BackingField = value;
    }

    public int Z
    {
      get => this.\u003CZ\u003Ek__BackingField;
      set => this.\u003CZ\u003Ek__BackingField = value;
    }

    public Vector3i ChunkPos
    {
      get => this.\u003CChunkPos\u003Ek__BackingField;
      set => this.\u003CChunkPos\u003Ek__BackingField = value;
    }

    public bool GetAvailable() => true;

    [PublicizedFrom(EAccessModifier.Private)]
    public bool checkCoordinates(ref int _x, ref int _y, ref int _z)
    {
      _x = this.X * 16 /*0x10*/ + _x;
      _y = this.Y * 256 /*0x0100*/ + _y;
      _z = this.Z * 16 /*0x10*/ + _z;
      return _x >= 0 && _x < this.prefab.size.x && _y >= 0 && _y < this.prefab.size.y && _z >= 0 && _z < this.prefab.size.z;
    }

    public BlockValue GetBlock(int _x, int _y, int _z)
    {
      return !this.checkCoordinates(ref _x, ref _y, ref _z) ? BlockValue.Air : this.prefab.GetBlock(_x, _y, _z);
    }

    public BlockValue GetBlockNoDamage(int _x, int _y, int _z) => this.GetBlock(_x, _y, _z);

    public BlockValue GetBlock(int _bos, int _y)
    {
      return this.GetBlock(ChunkBlockLayerLegacy.OffsetX(_bos), _y, ChunkBlockLayerLegacy.OffsetX(_bos));
    }

    public bool IsAir(int _x, int _y, int _z)
    {
      return this.checkCoordinates(ref _x, ref _y, ref _z) && this.prefab.GetBlock(_x, _y, _z).isair && !this.prefab.GetWater(_x, _y, _z).HasMass();
    }

    public WaterValue GetWater(int _x, int _y, int _z)
    {
      return !this.checkCoordinates(ref _x, ref _y, ref _z) ? WaterValue.Empty : this.prefab.GetWater(_x, _y, _z);
    }

    public bool IsWater(int _x, int _y, int _z) => this.GetWater(_x, _y, _z).HasMass();

    public int GetBlockFaceTexture(int _x, int _y, int _z, BlockFace _blockFace, int channel)
    {
      return !this.checkCoordinates(ref _x, ref _y, ref _z) ? 0 : (int) (this.prefab.GetTexture(_x, _y, _z)[channel] >> (int) _blockFace * 6 & 63L /*0x3F*/);
    }

    public long GetTextureFull(int _x, int _y, int _z, int channel = 0)
    {
      return !this.checkCoordinates(ref _x, ref _y, ref _z) ? 0L : this.prefab.GetTexture(_x, _y, _z)[channel];
    }

    public bool IsOnlyTerrain(int _y) => false;

    public bool IsOnlyTerrainLayer(int _idx) => false;

    public bool IsEmpty() => false;

    public bool IsEmpty(int _y) => false;

    public bool IsEmptyLayer(int _y) => false;

    public byte GetStability(int _x, int _y, int _z) => 15;

    public byte GetStability(int _offs, int _y) => 15;

    public void SetStability(int _offs, int _y, byte _v)
    {
    }

    public void SetStability(int _x, int _y, int _z, byte _v)
    {
    }

    public byte GetLight(int x, int y, int z, Chunk.LIGHT_TYPE type) => 15;

    public int GetLightValue(int x, int y, int z, int _darknessV) => 15;

    public float GetLightBrightness(int x, int y, int z, int _darknessV) => 1f;

    public Vector3i GetWorldPos() => new Vector3i(this.X, this.Y, this.Z);

    public void SetVertexOffset(int x, int y, int z, Vector3 _vertexOffset)
    {
    }

    public bool GetVertexOffset(int _x, int _y, int _z, out Vector3 _vertexOffset)
    {
      _vertexOffset = Vector3.zero;
      return false;
    }

    public void SetVertexYOffset(int x, int y, int z, float _addYPos)
    {
    }

    public byte GetHeight(int _blockOffset) => (byte) this.prefab.size.y;

    public byte GetHeight(int _x, int _z) => (byte) this.prefab.size.y;

    public sbyte GetDensity(int _xzOffs, int _y)
    {
      return this.GetDensity(ChunkBlockLayerLegacy.OffsetX(_xzOffs), _y, ChunkBlockLayerLegacy.OffsetX(_xzOffs));
    }

    public sbyte GetDensity(int _x, int _y, int _z)
    {
      return !this.checkCoordinates(ref _x, ref _y, ref _z) ? sbyte.MaxValue : this.prefab.GetDensity(_x, _y, _z);
    }

    public sbyte SetDensity(int _xzOffs, int _y, sbyte _density) => 0;

    public bool HasSameDensityValue(int _y) => false;

    public sbyte GetSameDensityValue(int _y) => 0;

    public BlockEntityData GetBlockEntity(Vector3i _blockPos) => (BlockEntityData) null;

    public BlockEntityData GetBlockEntity(Transform _transform) => (BlockEntityData) null;

    public void SetTopSoilBroken(int _x, int _z)
    {
    }

    public bool IsTopSoil(int _x, int _z) => false;

    public byte GetTerrainHeight(int _x, int _z)
    {
      for (int _y = this.prefab.size.y - 1; _y >= 0; --_y)
      {
        if (this.GetBlock(_x, _y, _z).Block.shape.IsTerrain())
          return (byte) _y;
      }
      return 0;
    }
  }
}
