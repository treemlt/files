// Decompiled with JetBrains decompiler
// Type: Chunk
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using Platform;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using UnityEngine;

#nullable disable
public class Chunk : IChunk, IBlockAccess, IMemoryPoolableObject
{
  public static uint CurrentSaveVersion = 47;
  public const int cAreaMasterSizeChunks = 5;
  public const int cAreaMasterSizeBlocks = 80 /*0x50*/;
  public const int cTextureChannelCount = 1;
  [PublicizedFrom(EAccessModifier.Private)]
  public ChunkBlockLayer[] m_BlockLayers;
  [PublicizedFrom(EAccessModifier.Private)]
  public ChunkBlockChannel chnStability;
  [PublicizedFrom(EAccessModifier.Private)]
  public ChunkBlockChannel chnDensity;
  [PublicizedFrom(EAccessModifier.Private)]
  public ChunkBlockChannel chnLight;
  [PublicizedFrom(EAccessModifier.Private)]
  public ChunkBlockChannel chnDamage;
  [PublicizedFrom(EAccessModifier.Private)]
  public ChunkBlockChannel[] chnTextures;
  [PublicizedFrom(EAccessModifier.Private)]
  public ChunkBlockChannel chnWater;
  [PublicizedFrom(EAccessModifier.Private)]
  public int m_X;
  [PublicizedFrom(EAccessModifier.Private)]
  public int m_Y;
  [PublicizedFrom(EAccessModifier.Private)]
  public int m_Z;
  public Vector3i worldPosIMin;
  public Vector3i worldPosIMax;
  [PublicizedFrom(EAccessModifier.Private)]
  public const double cEntityListHeight = 16.0;
  [PublicizedFrom(EAccessModifier.Private)]
  public const int cEntityListCount = 16 /*0x10*/;
  public List<Entity>[] entityLists = new List<Entity>[16 /*0x10*/];
  [PublicizedFrom(EAccessModifier.Private)]
  public DictionaryList<Vector3i, TileEntity> tileEntities = new DictionaryList<Vector3i, TileEntity>();
  [PublicizedFrom(EAccessModifier.Private)]
  public List<int> sleeperVolumes = new List<int>();
  [PublicizedFrom(EAccessModifier.Private)]
  public List<int> triggerVolumes = new List<int>();
  [PublicizedFrom(EAccessModifier.Private)]
  public List<int> wallVolumes = new List<int>();
  [PublicizedFrom(EAccessModifier.Private)]
  public byte[] m_HeightMap;
  [PublicizedFrom(EAccessModifier.Private)]
  public byte[] m_bTopSoilBroken;
  [PublicizedFrom(EAccessModifier.Private)]
  public byte[] m_Biomes;
  [PublicizedFrom(EAccessModifier.Private)]
  public byte[] m_BiomeIntensities;
  public byte DominantBiome;
  public byte AreaMasterDominantBiome = byte.MaxValue;
  [PublicizedFrom(EAccessModifier.Private)]
  public byte[] m_NormalX;
  [PublicizedFrom(EAccessModifier.Private)]
  public byte[] m_NormalY;
  [PublicizedFrom(EAccessModifier.Private)]
  public byte[] m_NormalZ;
  [PublicizedFrom(EAccessModifier.Private)]
  public byte[] m_TerrainHeight;
  [PublicizedFrom(EAccessModifier.Private)]
  public List<EntityCreationData> entityStubs = new List<EntityCreationData>();
  public DictionaryKeyValueList<string, global::ChunkCustomData> ChunkCustomData = new DictionaryKeyValueList<string, global::ChunkCustomData>();
  public ulong SavedInWorldTicks;
  public ulong LastTimeRandomTicked;
  [PublicizedFrom(EAccessModifier.Private)]
  public List<Vector3b> insideDevices = new List<Vector3b>();
  [PublicizedFrom(EAccessModifier.Private)]
  public HashSet<int> insideDevicesHashSet = new HashSet<int>();
  [PublicizedFrom(EAccessModifier.Private)]
  public DictionaryList<Vector3i, BlockTrigger> triggerData = new DictionaryList<Vector3i, BlockTrigger>();
  [PublicizedFrom(EAccessModifier.Private)]
  public DictionaryList<ulong, BlockEntityData> blockEntityStubs = new DictionaryList<ulong, BlockEntityData>();
  [PublicizedFrom(EAccessModifier.Private)]
  public List<BlockEntityData> blockEntityStubsToRemove = new List<BlockEntityData>();
  [PublicizedFrom(EAccessModifier.Private)]
  public ChunkAreaBiomeSpawnData biomeSpawnData;
  [PublicizedFrom(EAccessModifier.Private)]
  public Queue<int> m_layerIndexQueue = new Queue<int>();
  [PublicizedFrom(EAccessModifier.Private)]
  public VoxelMeshLayer[] m_meshLayers = new VoxelMeshLayer[16 /*0x10*/];
  public volatile bool hasEntities;
  public bool isModified;
  [PublicizedFrom(EAccessModifier.Private)]
  public Bounds boundingBox;
  public DictionarySave<string, List<Vector3i>> IndexedBlocks = new DictionarySave<string, List<Vector3i>>();
  [PublicizedFrom(EAccessModifier.Private)]
  public volatile int m_NeedsRegenerationAtY;
  [PublicizedFrom(EAccessModifier.Private)]
  public EnumDecoAllowed[] m_DecoBiomeArray;
  [PublicizedFrom(EAccessModifier.Private)]
  public ushort[] mapColors;
  [PublicizedFrom(EAccessModifier.Private)]
  public bool bMapDirty;
  [PublicizedFrom(EAccessModifier.Private)]
  public bool bEmpty;
  [PublicizedFrom(EAccessModifier.Private)]
  public bool bEmptyDirty = true;
  [PublicizedFrom(EAccessModifier.Private)]
  public DictionaryKeyList<Vector3i, int> tickedBlocks = new DictionaryKeyList<Vector3i, int>();
  public bool IsInternalBlocksCulled;
  public bool StopStabilityCalculation;
  public OcclusionManager.OccludeeZone occludeeZone;
  public readonly ReaderWriterLockSlim sync = new ReaderWriterLockSlim();
  [PublicizedFrom(EAccessModifier.Private)]
  public WaterSimulationNative.ChunkHandle waterSimHandle;
  public static int InstanceCount;
  public int TotalMemory;
  [PublicizedFrom(EAccessModifier.Private)]
  public int totalTris;
  [PublicizedFrom(EAccessModifier.Private)]
  public int[][] trisInMesh = new int[16 /*0x10*/][];
  [PublicizedFrom(EAccessModifier.Private)]
  public int[][] sizeOfMesh = new int[16 /*0x10*/][];
  [PublicizedFrom(EAccessModifier.Private)]
  public WaterDebugManager.RendererHandle waterDebugHandle;
  public readonly int ClrIdx;
  public volatile bool InProgressCopying;
  public volatile bool InProgressDecorating;
  public volatile bool InProgressLighting;
  public volatile bool InProgressRegeneration;
  public volatile bool InProgressUnloading;
  public volatile bool InProgressSaving;
  public volatile bool InProgressNetworking;
  public volatile bool InProgressWaterSim;
  public volatile bool IsDisplayed;
  public volatile bool IsCollisionMeshGenerated;
  public volatile bool NeedsOnlyCollisionMesh;
  public int NeedsRegenerationDebug;
  public volatile bool NeedsDecoration;
  public volatile bool NeedsLightDecoration;
  public volatile bool NeedsLightCalculation;
  [PublicizedFrom(EAccessModifier.Private)]
  public static BlockValue bvPOIFiller;
  public static bool IgnorePaintTextures = false;
  [PublicizedFrom(EAccessModifier.Private)]
  public bool spawnedBiomeParticles;
  [PublicizedFrom(EAccessModifier.Private)]
  public List<GameObject> biomeParticles;
  [PublicizedFrom(EAccessModifier.Private)]
  public static readonly List<Transform> occlusionTs = new List<Transform>(200);
  public Chunk.DisplayState displayState;
  [PublicizedFrom(EAccessModifier.Private)]
  public int blockEntitiesIndex;
  [PublicizedFrom(EAccessModifier.Private)]
  public static List<MeshRenderer> tempMeshRenderers = new List<MeshRenderer>();
  public int MeshLayerCount;
  [PublicizedFrom(EAccessModifier.Private)]
  public static int[] biomeCnt = new int[50];
  [PublicizedFrom(EAccessModifier.Private)]
  public string cachedToString;
  [PublicizedFrom(EAccessModifier.Private)]
  public const int dbChunkX = 136;
  [PublicizedFrom(EAccessModifier.Private)]
  public const int dbChunkZ = 25;

  public void AssignWaterSimHandle(WaterSimulationNative.ChunkHandle handle)
  {
    this.waterSimHandle = handle;
  }

  public void ResetWaterSimHandle() => this.waterSimHandle.Reset();

  public void AssignWaterDebugRenderer(WaterDebugManager.RendererHandle handle)
  {
    this.waterDebugHandle = handle;
  }

  public void ResetWaterDebugHandle()
  {
  }

  public byte[] GetTopSoil() => this.m_bTopSoilBroken;

  public void SetTopSoil(IList<byte> soil)
  {
    for (int index = 0; index < this.m_bTopSoilBroken.Length; ++index)
      this.m_bTopSoilBroken[index] = soil[index];
  }

  public Chunk()
  {
    this.m_X = 0;
    this.m_Y = 0;
    this.Z = 0;
    for (int index = 0; index < this.trisInMesh.GetLength(0); ++index)
    {
      this.trisInMesh[index] = new int[MeshDescription.meshes.Length];
      this.sizeOfMesh[index] = new int[MeshDescription.meshes.Length];
    }
    for (int index = 0; index < 16 /*0x10*/; ++index)
      this.entityLists[index] = new List<Entity>();
    this.NeedsLightCalculation = true;
    this.NeedsDecoration = true;
    this.hasEntities = false;
    this.isModified = false;
    this.m_BlockLayers = new ChunkBlockLayer[64 /*0x40*/];
    this.chnLight = new ChunkBlockChannel(0L);
    this.chnDensity = new ChunkBlockChannel((long) (byte) MarchingCubes.DensityAir);
    this.chnStability = new ChunkBlockChannel(0L);
    this.chnDamage = new ChunkBlockChannel(0L, 2);
    this.chnTextures = new ChunkBlockChannel[1];
    for (int index = 0; index < 1; ++index)
      this.chnTextures[index] = new ChunkBlockChannel(0L, 6);
    this.chnWater = new ChunkBlockChannel(0L, 2);
    this.m_HeightMap = new byte[256 /*0x0100*/];
    this.m_TerrainHeight = new byte[256 /*0x0100*/];
    this.m_bTopSoilBroken = new byte[32 /*0x20*/];
    this.m_Biomes = new byte[256 /*0x0100*/];
    this.m_BiomeIntensities = new byte[1536 /*0x0600*/];
    this.m_NormalX = new byte[256 /*0x0100*/];
    this.m_NormalY = new byte[256 /*0x0100*/];
    this.m_NormalZ = new byte[256 /*0x0100*/];
    ++Chunk.InstanceCount;
  }

  public Chunk(int _x, int _z)
    : this()
  {
    this.m_X = _x;
    this.m_Y = 0;
    this.m_Z = _z;
    this.ResetStability();
    this.RefreshSunlight();
    this.NeedsLightCalculation = true;
    this.NeedsDecoration = false;
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  void object.Finalize()
  {
    try
    {
      --Chunk.InstanceCount;
    }
    finally
    {
      // ISSUE: explicit finalizer call
      base.Finalize();
    }
  }

  public void ResetLights(byte _lightValue = 0) => this.chnLight.Clear((long) _lightValue);

  public void Reset()
  {
    if (this.InProgressSaving)
      Log.Warning("Unloading: chunk while saving " + this?.ToString());
    this.cachedToString = (string) null;
    this.m_X = 0;
    this.m_Y = 0;
    this.Z = 0;
    this.MeshLayerCount = 0;
    for (int index = 0; index < 16 /*0x10*/; ++index)
      this.entityLists[index].Clear();
    this.entityStubs.Clear();
    this.blockEntityStubs.Clear();
    this.sleeperVolumes.Clear();
    this.triggerVolumes.Clear();
    this.tileEntities.Clear();
    this.IndexedBlocks.Clear();
    this.triggerData.Clear();
    this.insideDevices.Clear();
    this.insideDevicesHashSet.Clear();
    this.NeedsRegeneration = false;
    this.NeedsDecoration = true;
    this.NeedsLightDecoration = false;
    this.NeedsLightCalculation = true;
    this.hasEntities = false;
    this.isModified = false;
    this.InProgressRegeneration = false;
    this.InProgressSaving = false;
    this.InProgressCopying = false;
    this.InProgressDecorating = false;
    this.InProgressLighting = false;
    this.InProgressUnloading = false;
    this.NeedsOnlyCollisionMesh = false;
    this.IsCollisionMeshGenerated = false;
    this.SavedInWorldTicks = 0UL;
    MemoryPools.poolCBL.FreeSync((IList<ChunkBlockLayer>) this.m_BlockLayers);
    this.chnDensity.FreeLayers();
    this.chnStability.FreeLayers();
    this.chnLight.FreeLayers();
    this.chnDamage.FreeLayers();
    for (int index = 0; index < 1; ++index)
      this.chnTextures[index].FreeLayers();
    this.chnWater.FreeLayers();
    this.ResetLights();
    Array.Clear((Array) this.m_HeightMap, 0, this.m_HeightMap.GetLength(0));
    Array.Clear((Array) this.m_TerrainHeight, 0, this.m_TerrainHeight.GetLength(0));
    Array.Clear((Array) this.m_bTopSoilBroken, 0, this.m_bTopSoilBroken.GetLength(0));
    Array.Clear((Array) this.m_Biomes, 0, this.m_Biomes.GetLength(0));
    Array.Clear((Array) this.m_NormalX, 0, this.m_NormalX.GetLength(0));
    Array.Clear((Array) this.m_NormalY, 0, this.m_NormalY.GetLength(0));
    Array.Clear((Array) this.m_NormalZ, 0, this.m_NormalZ.GetLength(0));
    this.ResetBiomeIntensity(BiomeIntensity.Default);
    this.DominantBiome = (byte) 0;
    this.AreaMasterDominantBiome = byte.MaxValue;
    this.biomeSpawnData = (ChunkAreaBiomeSpawnData) null;
    if (this.m_DecoBiomeArray != null)
      Array.Clear((Array) this.m_DecoBiomeArray, 0, this.m_DecoBiomeArray.GetLength(0));
    this.ChunkCustomData.Clear();
    this.bMapDirty = true;
    lock (this.tickedBlocks)
      this.tickedBlocks.Clear();
    this.bEmptyDirty = true;
    this.StopStabilityCalculation = true;
    this.waterSimHandle.Reset();
  }

  public void Cleanup() => this.waterSimHandle.Reset();

  public int X
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get => this.m_X;
    set
    {
      this.cachedToString = (string) null;
      this.m_X = value;
      this.updateBounds();
    }
  }

  public int Y
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get => this.m_Y;
  }

  public int Z
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get => this.m_Z;
    set
    {
      this.cachedToString = (string) null;
      this.m_Z = value;
      this.updateBounds();
    }
  }

  public Vector3i ChunkPos
  {
    get => new Vector3i(this.m_X, this.m_Y, this.m_Z);
    set
    {
      this.cachedToString = (string) null;
      this.m_X = value.x;
      this.m_Z = value.z;
      this.updateBounds();
    }
  }

  public long Key => WorldChunkCache.MakeChunkKey(this.m_X, this.m_Z);

  public bool IsLocked
  {
    get
    {
      return this.InProgressCopying || this.InProgressDecorating || this.InProgressLighting || this.InProgressRegeneration || this.InProgressUnloading || this.InProgressSaving || this.InProgressNetworking || this.InProgressWaterSim;
    }
  }

  public bool IsLockedExceptUnloading
  {
    get
    {
      return this.InProgressCopying || this.InProgressDecorating || this.InProgressLighting || this.InProgressRegeneration || this.InProgressSaving || this.InProgressNetworking || this.InProgressWaterSim;
    }
  }

  public bool IsInitialized
  {
    get => !this.NeedsLightCalculation && !this.InProgressDecorating && !this.InProgressUnloading;
  }

  public bool GetAvailable() => this.IsCollisionMeshGenerated;

  public bool NeedsRegeneration
  {
    get
    {
      lock (this)
        return this.m_NeedsRegenerationAtY != 0;
    }
    set
    {
      lock (this.m_layerIndexQueue)
      {
        this.MeshLayerCount = 0;
        this.m_layerIndexQueue.Clear();
        MemoryPools.poolVML.FreeSync((IList<VoxelMeshLayer>) this.m_meshLayers);
      }
      lock (this)
        this.m_NeedsRegenerationAtY = !value ? 0 : (int) ushort.MaxValue;
      this.NeedsRegenerationDebug = this.m_NeedsRegenerationAtY;
    }
  }

  public void ClearNeedsRegenerationAt(int _idx)
  {
    lock (this)
    {
      this.m_NeedsRegenerationAtY &= ~(1 << _idx);
      this.NeedsRegenerationDebug = this.m_NeedsRegenerationAtY;
    }
  }

  public bool NeedsCopying => this.HasMeshLayer();

  public int NeedsRegenerationAt
  {
    get
    {
      lock (this)
        return this.m_NeedsRegenerationAtY;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)] set
    {
      lock (this)
        this.m_NeedsRegenerationAtY |= 1 << (value >> 4);
    }
  }

  public void SetNeedsRegenerationRaw(int _v) => this.m_NeedsRegenerationAtY = _v;

  public bool NeedsSaving
  {
    get
    {
      return this.isModified || this.hasEntities || this.tileEntities.Count > 0 || this.triggerData.Count > 0;
    }
  }

  public void load(PooledBinaryReader stream, uint _version)
  {
    this.read(stream, _version, false);
    this.isModified = false;
  }

  public void read(PooledBinaryReader stream, uint _version) => this.read(stream, _version, true);

  [PublicizedFrom(EAccessModifier.Private)]
  public void read(PooledBinaryReader _br, uint _version, bool _bNetworkRead)
  {
    this.cachedToString = (string) null;
    this.m_X = _br.ReadInt32();
    this.m_Y = _br.ReadInt32();
    this.Z = _br.ReadInt32();
    if (_version > 30U)
      this.SavedInWorldTicks = _br.ReadUInt64();
    this.LastTimeRandomTicked = this.SavedInWorldTicks;
    MemoryPools.poolCBL.FreeSync((IList<ChunkBlockLayer>) this.m_BlockLayers);
    Array.Clear((Array) this.m_HeightMap, 0, 256 /*0x0100*/);
    if (_version < 28U)
      throw new Exception($"Chunk version {_version.ToString()} not supported any more!");
    for (int index = 0; index < 64 /*0x40*/; ++index)
    {
      if (_br.ReadBoolean())
      {
        ChunkBlockLayer chunkBlockLayer = MemoryPools.poolCBL.AllocSync(false);
        chunkBlockLayer.Read((BinaryReader) _br, _version, _bNetworkRead);
        this.m_BlockLayers[index] = chunkBlockLayer;
        this.bEmptyDirty = true;
      }
    }
    if (_version < 28U)
      this.chnStability.Convert(new ChunkBlockLayerLegacy[256 /*0x0100*/]);
    else if (!_bNetworkRead)
      this.chnStability.Read((BinaryReader) _br, _version, _bNetworkRead);
    _br.Flush();
    this.recalcIndexedBlocks();
    BinaryFormatter binaryFormatter = (BinaryFormatter) null;
    if (_version < 10U)
    {
      binaryFormatter = new BinaryFormatter();
      byte[,] numArray = (byte[,]) binaryFormatter.Deserialize(_br.BaseStream);
      for (int index1 = 0; index1 < 16 /*0x10*/; ++index1)
      {
        for (int index2 = 0; index2 < 16 /*0x10*/; ++index2)
          this.m_HeightMap[index1 + index2 * 16 /*0x10*/] = numArray[index1, index2];
      }
    }
    else
      _br.Read(this.m_HeightMap, 0, 256 /*0x0100*/);
    if (_version >= 7U && _version < 8U)
    {
      if (binaryFormatter == null)
        binaryFormatter = new BinaryFormatter();
      byte[,] numArray = (byte[,]) binaryFormatter.Deserialize(_br.BaseStream);
      this.m_TerrainHeight = new byte[numArray.GetLength(0) * numArray.GetLength(1)];
      for (int _x = 0; _x < numArray.GetLength(0); ++_x)
      {
        for (int _z = 0; _z < numArray.GetLength(1); ++_z)
          this.SetTerrainHeight(_x, _z, numArray[_x, _z]);
      }
    }
    else if (_version > 21U)
      _br.Read(this.m_TerrainHeight, 0, this.m_TerrainHeight.Length);
    if (_version > 41U)
      _br.Read(this.m_bTopSoilBroken, 0, 32 /*0x20*/);
    if (_version > 8U && _version < 15U)
    {
      if (binaryFormatter == null)
        binaryFormatter = new BinaryFormatter();
      byte[,] numArray = (byte[,]) binaryFormatter.Deserialize(_br.BaseStream);
      this.m_Biomes = new byte[numArray.GetLength(0) * numArray.GetLength(1)];
      for (int _x = 0; _x < numArray.GetLength(0); ++_x)
      {
        for (int _z = 0; _z < numArray.GetLength(1); ++_z)
          this.SetBiomeId(_x, _z, numArray[_x, _z]);
      }
    }
    else
      _br.Read(this.m_Biomes, 0, 256 /*0x0100*/);
    if (_version > 19U)
    {
      _br.Read(this.m_BiomeIntensities, 0, 1536 /*0x0600*/);
    }
    else
    {
      for (int offs = 0; offs < this.m_BiomeIntensities.Length; offs += 6)
        BiomeIntensity.Default.ToArray(this.m_BiomeIntensities, offs);
    }
    if (_version > 23U)
      this.DominantBiome = _br.ReadByte();
    if (_version > 24U)
      this.AreaMasterDominantBiome = _br.ReadByte();
    if (_version > 25U)
    {
      int num = (int) _br.ReadUInt16();
      this.ChunkCustomData.Clear();
      for (int index = 0; index < num; ++index)
      {
        global::ChunkCustomData chunkCustomData = new global::ChunkCustomData();
        chunkCustomData.Read((BinaryReader) _br);
        this.ChunkCustomData.Set(chunkCustomData.key, chunkCustomData);
      }
    }
    if (_version > 22U)
      _br.Read(this.m_NormalX, 0, 256 /*0x0100*/);
    if (_version > 20U)
      _br.Read(this.m_NormalY, 0, 256 /*0x0100*/);
    if (_version > 22U)
      _br.Read(this.m_NormalZ, 0, 256 /*0x0100*/);
    if (_version > 12U && _version < 27U)
      throw new Exception($"Chunk version {_version.ToString()} not supported any more!");
    this.chnDensity.Read((BinaryReader) _br, _version, _bNetworkRead);
    if (_version < 27U)
    {
      SmartArray _sa1 = new SmartArray(4, 8, 4);
      _sa1.read((BinaryReader) _br);
      SmartArray _sa2 = new SmartArray(4, 8, 4);
      _sa2.read((BinaryReader) _br);
      this.chnLight.Convert(_sa1, 0);
      this.chnLight.Convert(_sa2, 4);
    }
    else
      this.chnLight.Read((BinaryReader) _br, _version, _bNetworkRead);
    if (_version >= 33U && _version < 36U)
    {
      ChunkBlockChannel chunkBlockChannel = new ChunkBlockChannel(0L);
      chunkBlockChannel.Read((BinaryReader) _br, _version, _bNetworkRead);
      chunkBlockChannel.Read((BinaryReader) _br, _version, _bNetworkRead);
    }
    if (_version >= 36U)
      this.chnDamage.Read((BinaryReader) _br, _version, _bNetworkRead);
    if (_version >= 47U)
    {
      for (int index = 0; index < 1; ++index)
        this.chnTextures[index].Read((BinaryReader) _br, _version, _bNetworkRead);
    }
    else if (_version >= 35U)
      this.chnTextures[0].Read((BinaryReader) _br, _version, _bNetworkRead);
    if (_version >= 46U)
      this.chnWater.Read((BinaryReader) _br, _version, _bNetworkRead);
    else if (WaterSimulationNative.Instance.IsInitialized)
      throw new Exception("Serialized data incompatible with new water simulation");
    this.NeedsDecoration = false;
    this.NeedsLightCalculation = false;
    if (_version >= 6U)
      this.NeedsLightCalculation = _br.ReadBoolean();
    int num1 = _br.ReadInt32();
    for (int index = 0; index < 16 /*0x10*/; ++index)
      this.entityLists[index].Clear();
    this.entityStubs.Clear();
    for (int index = 0; index < num1; ++index)
    {
      EntityCreationData entityCreationData = new EntityCreationData();
      entityCreationData.read(_br, _bNetworkRead);
      this.entityStubs.Add(entityCreationData);
    }
    this.hasEntities = this.entityStubs.Count > 0;
    if (_version > 13U && _version < 32U /*0x20*/)
      _br.ReadInt32();
    int num2 = _br.ReadInt32();
    this.tileEntities.Clear();
    for (int index = 0; index < num2; ++index)
    {
      TileEntity tileEntity = TileEntity.Instantiate((TileEntityType) _br.ReadInt32(), this);
      if (tileEntity != null)
      {
        tileEntity.read(_br, _bNetworkRead ? TileEntity.StreamModeRead.FromServer : TileEntity.StreamModeRead.Persistency);
        tileEntity.OnReadComplete();
        this.tileEntities.Set(tileEntity.localChunkPos, tileEntity);
      }
    }
    if (_version > 10U && _version < 43U && !_bNetworkRead)
    {
      int num3 = (int) _br.ReadUInt16();
      int num4 = (int) _br.ReadByte();
    }
    if (_version > 33U && _br.ReadBoolean())
    {
      for (int index = 0; index < 16 /*0x10*/; ++index)
      {
        int num5 = (int) _br.ReadUInt16();
      }
    }
    if (!_bNetworkRead && _version == 37U)
    {
      byte num6 = _br.ReadByte();
      for (int index = 0; index < (int) num6; ++index)
        SleeperVolume.Read((BinaryReader) _br);
    }
    if (!_bNetworkRead && _version > 37U)
    {
      this.sleeperVolumes.Clear();
      int num7 = (int) _br.ReadByte();
      for (int index = 0; index < num7; ++index)
      {
        int id = _br.ReadInt32();
        if (id < 0)
          Log.Error("chunk sleeper volumeId invalid {0}", new object[1]
          {
            (object) id
          });
        else
          this.AddSleeperVolumeId(id);
      }
    }
    if (!_bNetworkRead && _version >= 44U)
    {
      this.triggerVolumes.Clear();
      int num8 = (int) _br.ReadByte();
      for (int index = 0; index < num8; ++index)
      {
        int id = _br.ReadInt32();
        if (id < 0)
          Log.Error("chunk trigger volumeId invalid {0}", new object[1]
          {
            (object) id
          });
        else
          this.AddTriggerVolumeId(id);
      }
    }
    if (_version >= 45U)
    {
      this.wallVolumes.Clear();
      int num9 = (int) _br.ReadByte();
      for (int index = 0; index < num9; ++index)
      {
        int id = _br.ReadInt32();
        if (id < 0)
          Log.Error("chunk wall volumeId invalid {0}", new object[1]
          {
            (object) id
          });
        else
          this.AddWallVolumeId(id);
      }
    }
    if (_bNetworkRead)
      _br.ReadBoolean();
    lock (this.tickedBlocks)
    {
      this.tickedBlocks.Clear();
      for (int index = 0; index < 64 /*0x40*/; ++index)
      {
        ChunkBlockLayer blockLayer = this.m_BlockLayers[index];
        if (blockLayer != null)
        {
          for (int offs = 0; offs < 1024 /*0x0400*/; ++offs)
          {
            int idAt = blockLayer.GetIdAt(offs);
            if (idAt != 0 && Block.BlocksLoaded && idAt < Block.list.Length && Block.list[idAt] != null && Block.list[idAt].IsRandomlyTick && !blockLayer.GetAt(offs).ischild)
              this.tickedBlocks.Add(this.ToWorldPos(offs % 256 /*0x0100*/ % 16 /*0x10*/, index * 4 + offs / 256 /*0x0100*/, offs % 256 /*0x0100*/ / 16 /*0x10*/), 0);
          }
        }
      }
    }
    this.insideDevices.Clear();
    if (_version > 39U)
    {
      int num10 = (int) _br.ReadInt16();
      this.insideDevices.Capacity = num10;
      byte _x = 0;
      byte _z = 0;
      int num11 = 0;
      for (int index = 0; index < num10; ++index)
      {
        if (num11 == 0)
        {
          _x = _br.ReadByte();
          _z = _br.ReadByte();
          num11 = (int) _br.ReadByte();
        }
        Vector3b vector3b = new Vector3b(_x, _br.ReadByte(), _z);
        this.insideDevices.Add(vector3b);
        this.insideDevicesHashSet.Add(vector3b.GetHashCode());
        --num11;
      }
    }
    if (_version > 40U)
      this.IsInternalBlocksCulled = _br.ReadBoolean();
    if (_version > 42U && !_bNetworkRead)
    {
      this.triggerData.Clear();
      int num12 = (int) _br.ReadInt16();
      for (int index = 0; index < num12; ++index)
      {
        Vector3i _key = StreamUtils.ReadVector3i((BinaryReader) _br);
        BlockTrigger blockTrigger = new BlockTrigger(this);
        blockTrigger.LocalChunkPos = _key;
        blockTrigger.Read(_br);
        this.triggerData.Add(_key, blockTrigger);
      }
    }
    if (_bNetworkRead)
    {
      this.ResetStabilityToBottomMost();
      this.NeedsLightCalculation = true;
    }
    this.bMapDirty = true;
    this.StopStabilityCalculation = false;
  }

  public void save(PooledBinaryWriter stream)
  {
    this.saveBlockIds();
    this.write(stream, false);
    this.isModified = false;
    this.SavedInWorldTicks = GameTimer.Instance.ticks;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void saveBlockIds()
  {
    if (Block.nameIdMapping == null)
      return;
    NameIdMapping nameIdMapping = Block.nameIdMapping;
    lock (nameIdMapping)
    {
      for (int index = 0; index < 256 /*0x0100*/; index += 4)
      {
        ChunkBlockLayer blockLayer = this.m_BlockLayers[index >> 2];
        if (blockLayer == null)
        {
          Block block = BlockValue.Air.Block;
          nameIdMapping.AddMapping(block.blockID, block.GetBlockName());
        }
        else
          blockLayer.SaveBlockMappings(nameIdMapping);
      }
    }
  }

  public void write(PooledBinaryWriter stream) => this.write(stream, true);

  [PublicizedFrom(EAccessModifier.Private)]
  public void write(PooledBinaryWriter _bw, bool _bNetworkWrite)
  {
    byte[] numArray = MemoryPools.poolByte.Alloc(256 /*0x0100*/);
    _bw.Write(this.m_X);
    _bw.Write(this.m_Y);
    _bw.Write(this.m_Z);
    _bw.Write(this.SavedInWorldTicks);
    for (int index = 0; index < 64 /*0x40*/; ++index)
    {
      bool flag = this.m_BlockLayers[index] != null;
      _bw.Write(flag);
      if (flag)
        this.m_BlockLayers[index].Write((BinaryWriter) _bw, _bNetworkWrite);
    }
    if (!_bNetworkWrite)
      this.chnStability.Write((BinaryWriter) _bw, _bNetworkWrite, numArray);
    _bw.Write(this.m_HeightMap);
    _bw.Write(this.m_TerrainHeight);
    _bw.Write(this.m_bTopSoilBroken);
    _bw.Write(this.m_Biomes);
    _bw.Write(this.m_BiomeIntensities);
    _bw.Write(this.DominantBiome);
    _bw.Write(this.AreaMasterDominantBiome);
    int num1 = 0;
    if (_bNetworkWrite)
    {
      for (int index = 0; index < this.ChunkCustomData.valueList.Count; ++index)
      {
        if (this.ChunkCustomData.valueList[index].isSavedToNetwork)
          ++num1;
      }
    }
    else
      num1 = this.ChunkCustomData.valueList.Count;
    _bw.Write((ushort) num1);
    for (int index = 0; index < this.ChunkCustomData.valueList.Count; ++index)
    {
      if (!_bNetworkWrite || this.ChunkCustomData.valueList[index].isSavedToNetwork)
        this.ChunkCustomData.valueList[index].Write((BinaryWriter) _bw);
    }
    _bw.Write(this.m_NormalX);
    _bw.Write(this.m_NormalY);
    _bw.Write(this.m_NormalZ);
    this.chnDensity.Write((BinaryWriter) _bw, _bNetworkWrite, numArray);
    this.chnLight.Write((BinaryWriter) _bw, _bNetworkWrite, numArray);
    this.chnDamage.Write((BinaryWriter) _bw, _bNetworkWrite, numArray);
    for (int index = 0; index < 1; ++index)
      this.chnTextures[index].Write((BinaryWriter) _bw, _bNetworkWrite, numArray);
    this.chnWater.Write((BinaryWriter) _bw, _bNetworkWrite, numArray);
    _bw.Write(this.NeedsLightCalculation);
    int num2 = 0;
    for (int index1 = 0; index1 < 16 /*0x10*/; ++index1)
    {
      List<Entity> entityList = this.entityLists[index1];
      for (int index2 = 0; index2 < entityList.Count; ++index2)
      {
        Entity entity = entityList[index2];
        if (!(entity is EntityVehicle) && !(entity is EntityDrone) && (!_bNetworkWrite && entity.IsSavedToFile() || _bNetworkWrite && entity.IsSavedToNetwork()))
          ++num2;
      }
    }
    _bw.Write(num2);
    for (int index3 = 0; index3 < 16 /*0x10*/; ++index3)
    {
      List<Entity> entityList = this.entityLists[index3];
      for (int index4 = 0; index4 < entityList.Count; ++index4)
      {
        Entity _e = entityList[index4];
        if (!(_e is EntityVehicle) && !(_e is EntityDrone) && (!_bNetworkWrite && _e.IsSavedToFile() || _bNetworkWrite && _e.IsSavedToNetwork()))
          new EntityCreationData(_e).write(_bw, _bNetworkWrite);
      }
    }
    _bw.Write(this.tileEntities.Count);
    for (int index = 0; index < this.tileEntities.list.Count; ++index)
    {
      _bw.Write((int) this.tileEntities.list[index].GetTileEntityType());
      this.tileEntities.list[index].write(_bw, _bNetworkWrite ? TileEntity.StreamModeWrite.ToClient : TileEntity.StreamModeWrite.Persistency);
    }
    _bw.Write(false);
    if (!_bNetworkWrite)
    {
      int count = this.sleeperVolumes.Count;
      _bw.Write((byte) count);
      for (int index = 0; index < count; ++index)
        _bw.Write(this.sleeperVolumes[index]);
    }
    if (!_bNetworkWrite)
    {
      int count = this.triggerVolumes.Count;
      _bw.Write((byte) count);
      for (int index = 0; index < count; ++index)
        _bw.Write(this.triggerVolumes[index]);
    }
    int count1 = this.wallVolumes.Count;
    _bw.Write((byte) count1);
    for (int index = 0; index < count1; ++index)
      _bw.Write(this.wallVolumes[index]);
    if (_bNetworkWrite)
      _bw.Write(false);
    List<byte> byteList = new List<byte>();
    int num3 = int.MaxValue;
    int num4 = int.MaxValue;
    _bw.Write((short) this.insideDevices.Count);
    foreach (Vector3b insideDevice in this.insideDevices)
    {
      if (byteList.Count > 254 || num3 != (int) insideDevice.x || num4 != (int) insideDevice.z)
      {
        if (byteList.Count > 0)
        {
          _bw.Write((byte) num3);
          _bw.Write((byte) num4);
          _bw.Write((byte) byteList.Count);
          for (int index = 0; index < byteList.Count; ++index)
            _bw.Write(byteList[index]);
          byteList.Clear();
        }
        num3 = (int) insideDevice.x;
        num4 = (int) insideDevice.z;
      }
      byteList.Add(insideDevice.y);
    }
    if (byteList.Count > 0)
    {
      _bw.Write((byte) num3);
      _bw.Write((byte) num4);
      _bw.Write((byte) byteList.Count);
      for (int index = 0; index < byteList.Count; ++index)
        _bw.Write(byteList[index]);
    }
    _bw.Write(this.IsInternalBlocksCulled);
    if (!_bNetworkWrite)
    {
      int count2 = this.triggerData.Count;
      _bw.Write((short) count2);
      for (int index = 0; index < count2; ++index)
      {
        StreamUtils.Write((BinaryWriter) _bw, this.triggerData.list[index].LocalChunkPos);
        this.triggerData.list[index].Write(_bw);
      }
    }
    MemoryPools.poolByte.Free(numArray);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void recalcIndexedBlocks()
  {
    this.IndexedBlocks.Clear();
    for (int _curLayerIdx = 0; _curLayerIdx < 64 /*0x40*/; ++_curLayerIdx)
      this.m_BlockLayers[_curLayerIdx]?.AddIndexedBlocks(_curLayerIdx, this.IndexedBlocks);
  }

  public void AddEntityStub(EntityCreationData _ecd) => this.entityStubs.Add(_ecd);

  public BlockEntityData GetBlockEntity(Vector3i _worldPos)
  {
    BlockEntityData blockEntity;
    this.blockEntityStubs.dict.TryGetValue(GameUtils.Vector3iToUInt64(_worldPos), out blockEntity);
    return blockEntity;
  }

  public BlockEntityData GetBlockEntity(Transform _transform)
  {
    for (int index = 0; index < this.blockEntityStubs.list.Count; ++index)
    {
      if (Object.op_Equality((Object) this.blockEntityStubs.list[index].transform, (Object) _transform))
        return this.blockEntityStubs.list[index];
    }
    return (BlockEntityData) null;
  }

  public void AddEntityBlockStub(BlockEntityData _ecd)
  {
    ulong uint64 = GameUtils.Vector3iToUInt64(_ecd.pos);
    BlockEntityData blockEntityData;
    if (this.blockEntityStubs.dict.TryGetValue(uint64, out blockEntityData))
      this.blockEntityStubsToRemove.Add(blockEntityData);
    this.blockEntityStubs.Set(uint64, _ecd);
  }

  public void RemoveEntityBlockStub(Vector3i _pos)
  {
    ulong uint64 = GameUtils.Vector3iToUInt64(_pos);
    BlockEntityData blockEntityData;
    if (this.blockEntityStubs.dict.TryGetValue(uint64, out blockEntityData))
    {
      this.blockEntityStubsToRemove.Add(blockEntityData);
      this.blockEntityStubs.Remove(uint64);
    }
    else
      Log.Warning($"Entity block on pos {_pos.ToString()} not found!");
  }

  public int EnableEntityBlocks(bool _on, string _name)
  {
    _name = _name.ToLower();
    int num = 0;
    for (int index = 0; index < this.blockEntityStubs.list.Count; ++index)
    {
      BlockEntityData blockEntityData = this.blockEntityStubs.list[index];
      if (Object.op_Implicit((Object) blockEntityData.transform))
      {
        string lower = ((Object) blockEntityData.transform).name.ToLower();
        if (_name.Length == 0 || lower.Contains(_name))
        {
          ((Component) blockEntityData.transform).gameObject.SetActive(_on);
          ++num;
        }
      }
    }
    return num;
  }

  public void AddInsideDevicePosition(int _blockX, int _blockY, int _blockZ, BlockValue _bv)
  {
    Vector3b vector3b = new Vector3b(_blockX, _blockY, _blockZ);
    this.insideDevices.Add(vector3b);
    this.insideDevicesHashSet.Add(vector3b.GetHashCode());
    this.IsInternalBlocksCulled = true;
  }

  public int EnableInsideBlockEntities(bool _bOn)
  {
    int num = 0;
    foreach (Vector3b insideDevice in this.insideDevices)
    {
      BlockEntityData blockEntityData;
      if (this.blockEntityStubs.dict.TryGetValue(GameUtils.Vector3iToUInt64(this.ToWorldPos(insideDevice.ToVector3i())), out blockEntityData) && blockEntityData.bHasTransform)
      {
        ((Component) blockEntityData.transform).gameObject.SetActive(_bOn);
        ++num;
      }
    }
    return num;
  }

  public void ResetStability()
  {
    this.chnStability.Clear();
    for (int _x = 0; _x < 16 /*0x10*/; ++_x)
    {
      for (int _z = 0; _z < 16 /*0x10*/; ++_z)
      {
        for (int _y = 0; _y < 256 /*0x0100*/; ++_y)
        {
          int blockId = this.GetBlockId(_x, _y, _z);
          if (blockId != 0)
          {
            if (!Block.list[blockId].StabilitySupport)
            {
              this.chnStability.Set(_x, _y, _z, 1L);
              break;
            }
            this.chnStability.Set(_x, _y, _z, 15L);
          }
          else
            break;
        }
      }
    }
  }

  public void ResetStabilityToBottomMost()
  {
    this.chnStability.Clear();
    for (int _z = 0; _z < 16 /*0x10*/; ++_z)
    {
      for (int _x = 0; _x < 16 /*0x10*/; ++_x)
      {
        int _y;
        for (_y = 0; _y < 256 /*0x0100*/; ++_y)
        {
          int blockId = this.GetBlockId(_x, _y, _z);
          if (blockId != 0 && Block.list[blockId].StabilitySupport)
            break;
        }
        for (; _y < 256 /*0x0100*/; ++_y)
        {
          int blockId = this.GetBlockId(_x, _y, _z);
          if (blockId != 0)
          {
            if (!Block.list[blockId].StabilitySupport)
            {
              this.chnStability.Set(_x, _y, _z, 1L);
              break;
            }
            this.chnStability.Set(_x, _y, _z, 15L);
          }
          else
            break;
        }
      }
    }
  }

  public void RefreshSunlight()
  {
    this.chnLight.SetHalf(false, (byte) 15);
    for (int _x = 0; _x < 16 /*0x10*/; ++_x)
    {
      for (int _z = 0; _z < 16 /*0x10*/; ++_z)
      {
        int num = 15;
        bool flag1 = true;
        int maxValue;
        for (maxValue = (int) byte.MaxValue; maxValue >= 0; --maxValue)
        {
          int blockId = this.GetBlockId(_x, maxValue, _z);
          if (flag1)
          {
            if (blockId != 0)
              flag1 = false;
            else
              continue;
          }
          Block block = Block.list[blockId];
          bool flag2 = block.shape.IsTerrain();
          if (!flag2)
          {
            num -= block.lightOpacity;
            if (num <= 0)
              break;
          }
          this.chnLight.Set(_x, maxValue, _z, (long) (byte) num);
          if (flag2)
          {
            num -= block.lightOpacity;
            if (num <= 0)
              break;
          }
        }
        for (int _y = maxValue - 1; _y >= 0; --_y)
          this.chnLight.Set(_x, _y, _z, 0L);
      }
    }
    this.isModified = true;
  }

  public void SetFullSunlight() => this.chnLight.SetHalf(false, (byte) 15);

  public void CopyLightsFrom(Chunk _other)
  {
    this.chnLight.CopyFrom(_other.chnLight);
    this.isModified = true;
  }

  public bool CanMobsSpawnAtPos(
    int _x,
    int _y,
    int _z,
    bool _ignoreCanMobsSpawnOn = false,
    bool _checkWater = true)
  {
    if (_y < 2 || _y > 251 || this.IsTraderArea(_x, _z))
      return false;
    if (_checkWater || !this.IsWater(_x, _y - 1, _z))
    {
      Block block = this.GetBlockNoDamage(_x, _y - 1, _z).Block;
      if (!_ignoreCanMobsSpawnOn && !block.CanMobsSpawnOn || !block.IsCollideMovement)
        return false;
    }
    Block block1 = this.GetBlockNoDamage(_x, _y, _z).Block;
    if (!block1.IsCollideMovement || !block1.shape.IsSolidSpace)
    {
      Block block2 = this.GetBlockNoDamage(_x, _y + 1, _z).Block;
      if ((!block2.IsCollideMovement || !block2.shape.IsSolidSpace) && (!_checkWater || !this.IsWater(_x, _y, _z)))
        return true;
    }
    return false;
  }

  public bool CanSleeperSpawnAtPos(int _x, int _y, int _z, bool _checkBelow)
  {
    if (_checkBelow && !this.GetBlockNoDamage(_x, _y - 1, _z).Block.IsCollideMovement)
      return false;
    Block block = this.GetBlockNoDamage(_x, _y, _z).Block;
    return !block.IsCollideMovement && !block.shape.IsSolidSpace;
  }

  public bool CanPlayersSpawnAtPos(int _x, int _y, int _z, bool _allowOnAirPos = false)
  {
    if (_y < 2 || _y > 251)
      return false;
    BlockValue blockNoDamage = this.GetBlockNoDamage(_x, _y - 1, _z);
    Block block1 = blockNoDamage.Block;
    if (!block1.CanPlayersSpawnOn)
      return false;
    blockNoDamage = this.GetBlockNoDamage(_x, _y, _z);
    Block block2 = blockNoDamage.Block;
    blockNoDamage = this.GetBlockNoDamage(_x, _y + 1, _z);
    Block block3 = blockNoDamage.Block;
    return (_allowOnAirPos && block1.blockID == 0 || block1.IsCollideMovement) && (!block2.IsCollideMovement || !block2.shape.IsSolidSpace) && !this.IsWater(_x, _y, _z) && (!block3.IsCollideMovement || !block3.shape.IsSolidSpace);
  }

  public bool IsPositionOnTerrain(int _x, int _y, int _z)
  {
    return _y >= 1 && this.GetBlockNoDamage(_x, _y - 1, _z).Block.shape.IsTerrain();
  }

  public bool FindRandomTopSoilPoint(World _world, out int x, out int y, out int z, int numTrys)
  {
    x = 0;
    y = 0;
    z = 0;
    while (numTrys-- > 0)
    {
      x = _world.GetGameRandom().RandomRange(15);
      z = _world.GetGameRandom().RandomRange(15);
      y = (int) this.GetHeight(x, z);
      if (y >= 2 && this.CanMobsSpawnAtPos(x, y, z))
      {
        x += this.m_X * 16 /*0x10*/;
        ++y;
        z += this.m_Z * 16 /*0x10*/;
        return true;
      }
    }
    return false;
  }

  public bool FindRandomCavePoint(
    World _world,
    out int x,
    out int y,
    out int z,
    int numTrys,
    int relMinY)
  {
    x = 0;
    y = 0;
    z = 0;
label_6:
    while (numTrys-- > 0)
    {
      x = _world.GetGameRandom().RandomRange(15);
      z = _world.GetGameRandom().RandomRange(15);
      int height = (int) this.GetHeight(x, z);
      y = height;
      while (true)
      {
        if (y > height - relMinY && y > 2)
        {
          if (!this.CanMobsSpawnAtPos(x, y, z))
            --y;
          else
            break;
        }
        else
          goto label_6;
      }
      x += this.m_X * 16 /*0x10*/;
      ++y;
      z += this.m_Z * 16 /*0x10*/;
      return true;
    }
    return false;
  }

  public bool FindSpawnPointAtXZ(
    int x,
    int z,
    out int y,
    int _maxLightV,
    int _darknessV,
    int startY,
    int endY,
    bool _bIgnoreCanMobsSpawnOn = false)
  {
    endY = Utils.FastClamp(endY, 1, (int) byte.MaxValue);
    startY = Utils.FastClamp(startY - 1, 1, (int) byte.MaxValue);
    y = endY;
    while (y > startY)
    {
      if (this.GetLightValue(x, y, z, _darknessV) <= _maxLightV)
      {
        if (this.CanMobsSpawnAtPos(x, y, z, _bIgnoreCanMobsSpawnOn))
        {
          ++y;
          return true;
        }
        --y;
      }
    }
    return false;
  }

  public float GetLightBrightness(int x, int y, int z, int _ss)
  {
    return (float) this.GetLightValue(x, y, z, _ss) / 15f;
  }

  public int GetLightValue(int x, int y, int z, int _darknessValue)
  {
    int lightValue = (int) this.GetLight(x, y, z, Chunk.LIGHT_TYPE.SUN) - _darknessValue;
    if (lightValue == 15)
      return lightValue;
    int light = (int) this.GetLight(x, y, z, Chunk.LIGHT_TYPE.BLOCK);
    return lightValue > light ? lightValue : light;
  }

  public byte GetLight(int x, int y, int z, Chunk.LIGHT_TYPE type)
  {
    x &= 15;
    z &= 15;
    int num = (int) this.chnLight.GetByte(x, y, z);
    return type == Chunk.LIGHT_TYPE.SUN ? (byte) (num & 15) : (byte) (num >> 4);
  }

  public void SetLight(int x, int y, int z, byte intensity, Chunk.LIGHT_TYPE type)
  {
    x &= 15;
    z &= 15;
    int num1 = (int) this.chnLight.GetByte(x, y, z);
    int num2 = (int) intensity;
    switch (type)
    {
      case Chunk.LIGHT_TYPE.BLOCK:
        num2 = num2 << 4 | num1 & 15;
        break;
      case Chunk.LIGHT_TYPE.SUN:
        num2 |= num1 & 240 /*0xF0*/;
        break;
    }
    if (num2 != num1)
    {
      this.chnLight.Set(x, y, z, (long) (byte) num2);
      this.NeedsRegenerationAt = y;
    }
    this.isModified = true;
  }

  public void CheckSameLight() => this.chnLight.CheckSameValue();

  public void CheckSameStability() => this.chnStability.CheckSameValue();

  public static bool IsNeighbourChunksDecorated(Chunk[] _neighbours)
  {
    for (int index = 0; index < _neighbours.Length; ++index)
    {
      Chunk neighbour = _neighbours[index];
      if (neighbour == null || neighbour.NeedsDecoration)
        return false;
    }
    return true;
  }

  public static bool IsNeighbourChunksLit(Chunk[] _neighbours)
  {
    for (int index = 0; index < _neighbours.Length; ++index)
    {
      Chunk neighbour = _neighbours[index];
      if (neighbour == null || neighbour.NeedsLightCalculation)
        return false;
    }
    return true;
  }

  public Vector3i GetWorldPos() => new Vector3i(this.m_X << 4, this.m_Y << 8, this.m_Z << 4);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public int GetBlockWorldPosX(int _x) => (this.m_X << 4) + _x;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public int GetBlockWorldPosZ(int _z) => (this.m_Z << 4) + _z;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public byte GetHeight(int _x, int _z) => this.m_HeightMap[_x + _z * 16 /*0x10*/];

  public void SetHeight(int _x, int _z, byte _h) => this.m_HeightMap[_x + _z * 16 /*0x10*/] = _h;

  public byte GetMaxHeight()
  {
    byte maxHeight = 0;
    for (int index = this.m_HeightMap.Length - 1; index >= 0; --index)
    {
      byte height = this.m_HeightMap[index];
      if ((int) height > (int) maxHeight)
        maxHeight = height;
    }
    return maxHeight;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public byte GetTerrainHeight(int _x, int _z) => this.m_TerrainHeight[_x + _z * 16 /*0x10*/];

  public void SetTerrainHeight(int _x, int _z, byte _h)
  {
    this.m_TerrainHeight[_x + _z * 16 /*0x10*/] = _h;
  }

  public byte GetTopMostTerrainHeight()
  {
    byte mostTerrainHeight = 0;
    for (int index = 0; index < this.m_TerrainHeight.Length; ++index)
    {
      if ((int) this.m_TerrainHeight[index] > (int) mostTerrainHeight)
        mostTerrainHeight = this.m_TerrainHeight[index];
    }
    return mostTerrainHeight;
  }

  public bool IsTopSoil(int _x, int _z)
  {
    return ((uint) this.m_bTopSoilBroken[(_x + _z * 16 /*0x10*/) / 8] & (uint) (1 << (_x + _z * 16 /*0x10*/) % 8)) <= 0U;
  }

  public void SetTopSoilBroken(int _x, int _z)
  {
    int index = (_x + _z * 16 /*0x10*/) / 8;
    int num1 = (_x + _z * 16 /*0x10*/) % 8;
    int num2 = (int) this.m_bTopSoilBroken[index] | 1 << num1;
    this.m_bTopSoilBroken[index] = (byte) num2;
  }

  public BlockValue GetBlock(Vector3i _pos)
  {
    BlockValue block = BlockValue.Air;
    try
    {
      ChunkBlockLayer blockLayer = this.m_BlockLayers[_pos.y >> 2];
      if (blockLayer != null)
        block = blockLayer.GetAt(_pos.x, _pos.y, _pos.z);
    }
    catch (IndexOutOfRangeException ex)
    {
      Log.Error($"GetBlock failed: _y = {_pos.y.ToString()}, len = {this.m_BlockLayers.Length.ToString()} (chunk {this.m_X.ToString()}/{this.m_Z.ToString()})");
      throw;
    }
    block.damage = this.GetDamage(_pos.x, _pos.y, _pos.z);
    return block;
  }

  public BlockValue GetBlock(int _x, int _y, int _z)
  {
    if (this.IsInternalBlocksCulled && this.isInside(_x, _y, _z))
    {
      if (Chunk.bvPOIFiller.isair)
        Chunk.bvPOIFiller = new BlockValue((uint) Block.GetBlockByName(Constants.cPOIFillerBlock).blockID);
      return Chunk.bvPOIFiller;
    }
    BlockValue block = BlockValue.Air;
    try
    {
      ChunkBlockLayer blockLayer = this.m_BlockLayers[_y >> 2];
      if (blockLayer != null)
        block = blockLayer.GetAt(_x, _y, _z);
    }
    catch (IndexOutOfRangeException ex)
    {
      Log.Error($"GetBlock failed: _y = {_y.ToString()}, len = {this.m_BlockLayers.Length.ToString()} (chunk {this.m_X.ToString()}/{this.m_Z.ToString()})");
      throw;
    }
    block.damage = this.GetDamage(_x, _y, _z);
    return block;
  }

  public BlockValue GetBlockNoDamage(int _x, int _y, int _z)
  {
    BlockValue blockNoDamage = BlockValue.Air;
    try
    {
      ChunkBlockLayer blockLayer = this.m_BlockLayers[_y >> 2];
      if (blockLayer != null)
        blockNoDamage = blockLayer.GetAt(_x, _y, _z);
    }
    catch (IndexOutOfRangeException ex)
    {
      Log.Error($"GetBlockNoDamage failed: _y = {_y.ToString()}, len = {this.m_BlockLayers.Length.ToString()} (chunk {this.m_X.ToString()}/{this.m_Z.ToString()})");
      throw;
    }
    return blockNoDamage;
  }

  public int GetBlockId(int _x, int _y, int _z)
  {
    int blockId = 0;
    ChunkBlockLayer blockLayer = this.m_BlockLayers[_y >> 2];
    if (blockLayer != null)
      blockId = blockLayer.GetIdAt(_x, _y, _z);
    return blockId;
  }

  public void CopyMeshDataFrom(Chunk _other)
  {
    for (int index = 0; index < this.m_BlockLayers.Length; ++index)
    {
      if (_other.m_BlockLayers[index] == null)
      {
        if (this.m_BlockLayers[index] != null)
        {
          MemoryPools.poolCBL.FreeSync(this.m_BlockLayers[index]);
          this.m_BlockLayers[index] = (ChunkBlockLayer) null;
        }
      }
      else
      {
        if (this.m_BlockLayers[index] == null)
          this.m_BlockLayers[index] = MemoryPools.poolCBL.AllocSync(true);
        this.m_BlockLayers[index].CopyFrom(_other.m_BlockLayers[index]);
      }
    }
    this.chnDensity.CopyFrom(_other.chnDensity);
    this.chnDamage.CopyFrom(_other.chnDamage);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public byte GetBiomeId(int _x, int _z) => this.m_Biomes[_x + _z * 16 /*0x10*/];

  public void SetBiomeId(int _x, int _z, byte _biomeId)
  {
    this.m_Biomes[_x + _z * 16 /*0x10*/] = _biomeId;
  }

  public void FillBiomeId(byte _biomeId)
  {
    for (int index = 0; index < this.m_Biomes.Length; ++index)
      this.m_Biomes[index] = _biomeId;
  }

  public BiomeIntensity GetBiomeIntensity(int _x, int _z)
  {
    return this.m_BiomeIntensities == null ? BiomeIntensity.Default : new BiomeIntensity(this.m_BiomeIntensities, (_x + _z * 16 /*0x10*/) * 6);
  }

  public void CalcBiomeIntensity(Chunk[] _neighbours)
  {
    int[] _unsortedBiomeIdArray = new int[50];
    for (int index1 = 0; index1 < 16 /*0x10*/; ++index1)
    {
      for (int index2 = 0; index2 < 16 /*0x10*/; ++index2)
      {
        Array.Clear((Array) _unsortedBiomeIdArray, 0, _unsortedBiomeIdArray.Length);
        for (int index3 = -16; index3 < 16 /*0x10*/; ++index3)
        {
          int _v1 = index1 + index3;
          int _v2 = index2 + index3;
          Chunk chunk = this;
          if (_v1 < 0)
            chunk = _v2 >= 0 ? (_v2 < 16 /*0x10*/ ? _neighbours[1] : _neighbours[6]) : _neighbours[5];
          else if (_v1 >= 16 /*0x10*/)
            chunk = _v2 >= 0 ? (_v2 < 16 /*0x10*/ ? _neighbours[0] : _neighbours[4]) : _neighbours[3];
          else if (_v2 >= 16 /*0x10*/)
            chunk = _neighbours[2];
          else if (_v2 < 0)
            chunk = _neighbours[3];
          int biomeId = (int) chunk.GetBiomeId(World.toBlockXZ(_v1), World.toBlockXZ(_v2));
          if (biomeId >= 0 && biomeId < _unsortedBiomeIdArray.Length)
            ++_unsortedBiomeIdArray[biomeId];
        }
        BiomeIntensity.FromArray(_unsortedBiomeIdArray).ToArray(this.m_BiomeIntensities, (index1 + index2 * 16 /*0x10*/) * 6);
      }
    }
  }

  public void CalcDominantBiome()
  {
    int[] numArray = new int[50];
    for (int index = 0; index < this.m_Biomes.Length; ++index)
      ++numArray[(int) this.m_Biomes[index]];
    int num = 0;
    for (int index = 0; index < numArray.Length; ++index)
    {
      if (numArray[index] > num)
      {
        this.DominantBiome = (byte) index;
        num = numArray[index];
      }
    }
  }

  public void ResetBiomeIntensity(BiomeIntensity _v)
  {
    for (int offs = 0; offs < this.m_BiomeIntensities.Length; offs += 6)
      _v.ToArray(this.m_BiomeIntensities, offs);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public byte GetStability(int _x, int _y, int _z) => (byte) this.chnStability.Get(_x, _y, _z);

  public void SetStability(int _x, int _y, int _z, byte _v)
  {
    this.chnStability.Set(_x, _y, _z, (long) _v);
  }

  public void SetDensity(int _x, int _y, int _z, sbyte _density)
  {
    this.chnDensity.Set(_x, _y, _z, (long) (byte) _density);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public sbyte GetDensity(int _x, int _y, int _z) => (sbyte) this.chnDensity.Get(_x, _y, _z);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool HasSameDensityValue(int _y) => this.chnDensity.HasSameValue(_y);

  public sbyte GetSameDensityValue(int _y)
  {
    if (_y < 0)
      return MarchingCubes.DensityTerrain;
    return _y >= 256 /*0x0100*/ ? MarchingCubes.DensityAir : (sbyte) this.chnDensity.GetSameValue(_y);
  }

  public void CheckSameDensity() => this.chnDensity.CheckSameValue();

  public bool IsOnlyTerrain(int _y) => this.IsOnlyTerrainLayer(_y >> 2);

  public bool IsOnlyTerrainLayer(int _idx)
  {
    if (_idx < 0 || _idx >= this.m_BlockLayers.Length)
      return true;
    return this.m_BlockLayers[_idx] != null && this.m_BlockLayers[_idx].IsOnlyTerrain();
  }

  public void CheckOnlyTerrain()
  {
    for (int index = 0; index < this.m_BlockLayers.Length; ++index)
      this.m_BlockLayers[index]?.CheckOnlyTerrain();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public long GetTextureFull(int _x, int _y, int _z, int channel = 0)
  {
    return !Chunk.IgnorePaintTextures ? this.chnTextures[channel].Get(_x, _y, _z) : 0L;
  }

  public TextureFullArray GetTextureFullArray(int _x, int _y, int _z, bool applyIgnore = true)
  {
    TextureFullArray textureFullArray;
    for (int index = 0; index < 1; ++index)
      textureFullArray[index] = !applyIgnore || !Chunk.IgnorePaintTextures ? this.chnTextures[index].Get(_x, _y, _z) : 0L;
    return textureFullArray;
  }

  public void SetTextureFull(int _x, int _y, int _z, long _texturefull, int channel = 0)
  {
    this.chnTextures[channel].Set(_x, _y, _z, _texturefull);
    this.isModified = true;
  }

  public TextureFullArray GetSetTextureFullArray(
    int _x,
    int _y,
    int _z,
    TextureFullArray _texturefullArray)
  {
    TextureFullArray textureFullArray;
    for (int index = 0; index < 1; ++index)
      textureFullArray[index] = this.chnTextures[index].GetSet(_x, _y, _z, _texturefullArray[index]);
    this.isModified = true;
    return textureFullArray;
  }

  public int GetBlockFaceTexture(int _x, int _y, int _z, BlockFace _face, int channel)
  {
    return (int) (this.chnTextures[channel].Get(_x, _y, _z) >> (int) _face * 8 & (long) byte.MaxValue);
  }

  public long SetBlockFaceTexture(
    int _x,
    int _y,
    int _z,
    BlockFace _face,
    int _texture,
    int channel = 0)
  {
    long num1;
    long num2 = num1 = this.chnTextures[channel].Get(_x, _y, _z);
    int num3 = (int) _face * 8;
    long num4 = num2 & ~((long) byte.MaxValue << num3) | (long) (_texture & (int) byte.MaxValue) << num3;
    this.chnTextures[channel].Set(_x, _y, _z, num4);
    this.isModified = true;
    return num1;
  }

  public static int Value64FullToIndex(long _valueFull, BlockFace _blockFace)
  {
    return (int) (_valueFull >> (int) _blockFace * 8 & (long) byte.MaxValue);
  }

  public static long TextureIdxToTextureFullValue64(int _idx)
  {
    long num = (long) _idx;
    return (num & (long) byte.MaxValue) << 40 | (num & (long) byte.MaxValue) << 32 /*0x20*/ | (num & (long) byte.MaxValue) << 24 | (num & (long) byte.MaxValue) << 16 /*0x10*/ | (num & (long) byte.MaxValue) << 8 | num & (long) byte.MaxValue;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void SetDamage(int _x, int _y, int _z, int _damage)
  {
    this.chnDamage.Set(_x, _y, _z, (long) _damage);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public int GetDamage(int _x, int _y, int _z) => (int) this.chnDamage.Get(_x, _y, _z);

  public bool IsAir(int _x, int _y, int _z)
  {
    return !this.IsWater(_x, _y, _z) && this.GetBlockNoDamage(_x, _y, _z).isair;
  }

  public void ClearWater() => this.chnWater.Clear(0L);

  public bool IsWater(int _x, int _y, int _z) => this.GetWater(_x, _y, _z).HasMass();

  public WaterValue GetWater(int _x, int _y, int _z)
  {
    return WaterValue.FromRawData(this.chnWater.Get(_x, _y, _z));
  }

  public void SetWater(int _x, int _y, int _z, WaterValue _data)
  {
    this.SetWaterRaw(_x, _y, _z, _data);
    this.waterSimHandle.WakeNeighbours(_x, _y, _z);
  }

  public void SetWaterRaw(int _x, int _y, int _z, WaterValue _data)
  {
    if (!WaterUtils.CanWaterFlowThrough(this.GetBlock(_x, _y, _z)))
      _data.SetMass(0);
    this.chnWater.Set(_x, _y, _z, _data.RawData);
    this.bEmptyDirty = true;
    this.bMapDirty = true;
    this.isModified = true;
    this.waterSimHandle.SetWaterMass(_x, _y, _z, _data.GetMass());
    if (!_data.HasMass())
      return;
    int index = ChunkBlockLayerLegacy.CalcOffset(_x, _z);
    if ((int) this.m_HeightMap[index] >= _y)
      return;
    this.m_HeightMap[index] = (byte) _y;
  }

  public void SetWaterSimUpdate(
    int _x,
    int _y,
    int _z,
    WaterValue _data,
    out WaterValue _lastData)
  {
    if (!WaterUtils.CanWaterFlowThrough(this.GetBlockNoDamage(_x, _y, _z)))
    {
      _lastData = WaterValue.FromRawData(this.chnWater.Get(_x, _y, _z));
    }
    else
    {
      long set = this.chnWater.GetSet(_x, _y, _z, _data.RawData);
      _lastData = WaterValue.FromRawData(set);
      if (_lastData.GetMass() == _data.GetMass())
        return;
      GameManager.Instance.World.HandleWaterLevelChanged(this.ToWorldPos(_x, _y, _z), _data.GetMassPercent());
      this.bEmptyDirty = true;
      this.bMapDirty = true;
      this.isModified = true;
      if (!_data.HasMass())
        return;
      int index = ChunkBlockLayerLegacy.CalcOffset(_x, _z);
      if ((int) this.m_HeightMap[index] >= _y)
        return;
      this.m_HeightMap[index] = (byte) _y;
    }
  }

  public bool IsEmpty()
  {
    if (this.bEmptyDirty)
    {
      this.bEmpty = true;
      for (int index = 0; index < this.m_BlockLayers.Length; ++index)
      {
        if (this.m_BlockLayers[index] != null)
        {
          this.bEmpty = false;
          break;
        }
      }
      if (this.bEmpty)
        this.bEmpty = this.chnWater.IsDefault();
      this.bEmptyDirty = false;
    }
    return this.bEmpty;
  }

  public bool IsEmpty(int _y) => this.IsEmptyLayer(_y >> 2);

  public bool IsEmptyLayer(int _idx)
  {
    if ((long) (uint) _idx >= (long) this.m_BlockLayers.Length)
      return true;
    return this.m_BlockLayers[_idx] == null && this.chnWater.IsDefaultLayer(_idx);
  }

  public int RecalcHeights()
  {
    int v1 = 0;
    for (int _z = 0; _z < 16 /*0x10*/; ++_z)
    {
      for (int _x = 0; _x < 16 /*0x10*/; ++_x)
      {
        int index = ChunkBlockLayerLegacy.CalcOffset(_x, _z);
        this.m_HeightMap[index] = (byte) 0;
        for (int maxValue = (int) byte.MaxValue; maxValue >= 0; --maxValue)
        {
          ChunkBlockLayer blockLayer = this.m_BlockLayers[maxValue >> 2];
          if (((blockLayer == null ? 0 : (!blockLayer.GetAt(_x, maxValue, _z).isair ? 1 : 0)) != 0 ? 1 : (this.IsWater(_x, maxValue, _z) ? 1 : 0)) != 0)
          {
            this.m_HeightMap[index] = (byte) maxValue;
            v1 = Utils.FastMax(v1, maxValue);
            break;
          }
        }
      }
    }
    return v1;
  }

  public byte RecalcHeightAt(int _x, int _yMaxStart, int _z)
  {
    int index = ChunkBlockLayerLegacy.CalcOffset(_x, _z);
    for (int _y = _yMaxStart; _y >= 0; --_y)
    {
      ChunkBlockLayer blockLayer = this.m_BlockLayers[_y >> 2];
      if (((blockLayer == null ? 0 : (!blockLayer.GetAt(_x, _y, _z).isair ? 1 : 0)) != 0 ? 1 : (this.IsWater(_x, _y, _z) ? 1 : 0)) != 0)
      {
        this.m_HeightMap[index] = (byte) _y;
        return (byte) _y;
      }
    }
    return 0;
  }

  public BlockValue SetBlock(
    WorldBase _world,
    int x,
    int y,
    int z,
    BlockValue _blockValue,
    bool _notifyAddChange = true,
    bool _notifyRemove = true,
    bool _fromReset = false,
    bool _poiOwned = false,
    int _changedByEntityId = -1)
  {
    Vector3i vector3i = new Vector3i((this.m_X << 4) + x, y, (this.m_Z << 4) + z);
    BlockValue blockValue = this.SetBlockRaw(x, y, z, _blockValue);
    bool flag = !blockValue.isair && _blockValue.isair;
    if (flag)
    {
      this.waterSimHandle.WakeNeighbours(x, y, z);
      if (blockValue.Block.StabilitySupport)
        MultiBlockManager.Instance.SetOversizedStabilityDirty(vector3i);
    }
    if (!_blockValue.ischild)
      MultiBlockManager.Instance.UpdateTrackedBlockData(vector3i, _blockValue, _poiOwned);
    _blockValue = this.GetBlock(x, y, z);
    if (_notifyRemove && !blockValue.isair && blockValue.type != _blockValue.type)
      blockValue.Block?.OnBlockRemoved(_world, this, vector3i, blockValue);
    if (_notifyAddChange)
    {
      Block block = _blockValue.Block;
      if (block != null)
      {
        if (blockValue.type != _blockValue.type)
        {
          if (!_blockValue.isair)
          {
            PlatformUserIdentifierAbs _addedByPlayer = (PlatformUserIdentifierAbs) null;
            if (_changedByEntityId != -1)
              _addedByPlayer = GameManager.Instance.persistentPlayers.GetPlayerDataFromEntityID(_changedByEntityId).PrimaryId;
            block.OnBlockAdded(_world, this, vector3i, _blockValue, _addedByPlayer);
          }
        }
        else
        {
          block.OnBlockValueChanged(_world, this, 0, vector3i, blockValue, _blockValue);
          if (_fromReset)
            block.OnBlockReset(_world, this, vector3i, _blockValue);
        }
      }
    }
    if (flag)
    {
      this.RemoveBlockTrigger(new Vector3i(x, y, z));
      GameEventManager.Current.BlockRemoved(vector3i);
    }
    if ((DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent() && SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && !GameManager.Instance.IsEditMode() && BlockLimitTracker.instance != null && !blockValue.Equals(_blockValue))
    {
      BlockLimitTracker.instance.TryRemoveOrReplaceBlock(blockValue, _blockValue, vector3i);
      if (!flag)
        BlockLimitTracker.instance.TryAddTrackedBlock(_blockValue, vector3i, _changedByEntityId);
      BlockLimitTracker.instance.ServerUpdateClients();
    }
    return blockValue;
  }

  public BlockValue SetBlockRaw(int _x, int _y, int _z, BlockValue _blockValue)
  {
    if ((uint) _y >= (uint) byte.MaxValue)
      return BlockValue.Air;
    Block block1 = _blockValue.Block;
    if (block1 == null)
      return BlockValue.Air;
    if (_blockValue.isWater)
    {
      ChunkBlockLayer blockLayer = this.m_BlockLayers[_y >> 2];
      BlockValue _bv = blockLayer != null ? blockLayer.GetAt(_x, _y, _z) : BlockValue.Air;
      if (!WaterUtils.CanWaterFlowThrough(_bv))
        this.SetBlockRaw(_x, _y, _z, BlockValue.Air);
      this.SetWater(_x, _y, _z, WaterValue.Full);
      return _bv;
    }
    if (!WaterUtils.CanWaterFlowThrough(_blockValue))
      this.SetWater(_x, _y, _z, WaterValue.Empty);
    this.waterSimHandle.SetVoxelSolid(_x, _y, _z, BlockFaceFlags.RotateFlags(block1.WaterFlowMask, _blockValue.rotation));
    BlockValue blockValue = BlockValue.Air;
    int index1 = _y >> 2;
    ChunkBlockLayer blockLayer1 = this.m_BlockLayers[index1];
    if (blockLayer1 != null)
    {
      int offs = ChunkBlockLayer.CalcOffset(_x, _y, _z);
      blockValue = blockLayer1.GetAt(offs);
      blockLayer1.SetAt(offs, _blockValue.rawData);
      if (!blockValue.ischild)
        blockValue.damage = this.GetDamage(_x, _y, _z);
    }
    else if (!_blockValue.isair)
    {
      ChunkBlockLayer chunkBlockLayer = MemoryPools.poolCBL.AllocSync(true);
      this.m_BlockLayers[index1] = chunkBlockLayer;
      chunkBlockLayer.SetAt(_x, _y, _z, _blockValue.rawData);
    }
    if (!_blockValue.ischild)
      this.SetDamage(_x, _y, _z, _blockValue.damage);
    Block block2 = blockValue.Block;
    if (blockValue.type != _blockValue.type)
    {
      if (!blockValue.ischild && block2.IndexName != null && this.IndexedBlocks.ContainsKey(block2.IndexName))
      {
        this.IndexedBlocks[block2.IndexName].Remove(new Vector3i(_x, _y, _z));
        if (this.IndexedBlocks[block2.IndexName].Count == 0)
          this.IndexedBlocks.Remove(block2.IndexName);
      }
      if (!_blockValue.ischild && block1.IndexName != null && block1.FilterIndexType(_blockValue))
      {
        if (!this.IndexedBlocks.ContainsKey(block1.IndexName))
          this.IndexedBlocks[block1.IndexName] = new List<Vector3i>();
        this.IndexedBlocks[block1.IndexName].Add(new Vector3i(_x, _y, _z));
      }
    }
    int index2 = ChunkBlockLayerLegacy.CalcOffset(_x, _z);
    if (_blockValue.isair)
    {
      if ((int) this.m_HeightMap[index2] == _y)
      {
        int num = (int) this.RecalcHeightAt(_x, _y - 1, _z);
      }
    }
    else if ((int) this.m_HeightMap[index2] < _y)
      this.m_HeightMap[index2] = (byte) _y;
    if (blockValue.isair && !_blockValue.isair && !_blockValue.ischild)
    {
      if (block1.IsRandomlyTick)
      {
        lock (this.tickedBlocks)
          this.tickedBlocks.Replace(this.ToWorldPos(_x, _y, _z), 0);
      }
    }
    else if (!blockValue.isair && _blockValue.isair && !blockValue.ischild)
    {
      if (block2.IsRandomlyTick)
      {
        lock (this.tickedBlocks)
          this.tickedBlocks.Remove(this.ToWorldPos(_x, _y, _z));
      }
    }
    else if (block2.IsRandomlyTick && !block1.IsRandomlyTick && !blockValue.ischild)
    {
      lock (this.tickedBlocks)
        this.tickedBlocks.Remove(this.ToWorldPos(_x, _y, _z));
    }
    else if (!block2.IsRandomlyTick && block1.IsRandomlyTick && !_blockValue.ischild)
    {
      lock (this.tickedBlocks)
        this.tickedBlocks.Replace(this.ToWorldPos(_x, _y, _z), 0);
    }
    this.bMapDirty = true;
    this.isModified = true;
    this.bEmptyDirty = true;
    return blockValue;
  }

  public bool FillBlockRaw(int _heightIncl, BlockValue _blockValue)
  {
    if (_heightIncl >= (int) byte.MaxValue || _blockValue.isair || _blockValue.ischild)
      return false;
    Block block = _blockValue.Block;
    if (block == null || _blockValue.isWater || !this.IsEmpty())
      return false;
    uint rawData = _blockValue.rawData;
    sbyte _density = block.shape.IsTerrain() ? MarchingCubes.DensityTerrain : MarchingCubes.DensityAir;
    int damage = _blockValue.damage;
    int _y1;
    for (_y1 = 0; _y1 <= _heightIncl - 4; _y1 += 4)
    {
      int index = _y1 >> 2;
      if (this.m_BlockLayers[index] == null)
        this.m_BlockLayers[index] = MemoryPools.poolCBL.AllocSync(true);
      this.m_BlockLayers[index].Fill(rawData);
    }
    for (; _y1 <= _heightIncl; ++_y1)
    {
      for (int _x = 0; _x < 16 /*0x10*/; ++_x)
      {
        for (int _z = 0; _z < 16 /*0x10*/; ++_z)
        {
          int index = _y1 >> 2;
          if (this.m_BlockLayers[index] == null)
            this.m_BlockLayers[index] = MemoryPools.poolCBL.AllocSync(true);
          this.m_BlockLayers[index].SetAt(_x, _y1, _z, rawData);
        }
      }
    }
    List<Vector3i> vector3iList = (List<Vector3i>) null;
    if (block.IndexName != null)
    {
      if (!this.IndexedBlocks.ContainsKey(block.IndexName))
        this.IndexedBlocks[block.IndexName] = new List<Vector3i>();
      vector3iList = this.IndexedBlocks[block.IndexName];
      vector3iList.Clear();
    }
    lock (this.tickedBlocks)
    {
      this.tickedBlocks.Clear();
      for (int _y2 = 0; _y2 <= _heightIncl; ++_y2)
      {
        for (int _x = 0; _x < 16 /*0x10*/; ++_x)
        {
          for (int _z = 0; _z < 16 /*0x10*/; ++_z)
          {
            this.SetDensity(_x, _y2, _z, _density);
            this.SetDamage(_x, _y2, _z, damage);
            vector3iList?.Add(new Vector3i(_x, _y2, _z));
            if (block.IsRandomlyTick)
              this.tickedBlocks.Replace(this.ToWorldPos(_x, _y2, _z), 0);
          }
        }
      }
    }
    for (int _x = 0; _x < 16 /*0x10*/; ++_x)
    {
      for (int _z = 0; _z < 16 /*0x10*/; ++_z)
        this.m_HeightMap[ChunkBlockLayerLegacy.CalcOffset(_x, _z)] = (byte) _heightIncl;
    }
    this.bMapDirty = true;
    this.isModified = true;
    this.bEmptyDirty = true;
    return true;
  }

  public DictionaryKeyList<Vector3i, int> GetTickedBlocks() => this.tickedBlocks;

  public void RemoveTileEntityAt<T>(World world, Vector3i _posInChunk)
  {
    TileEntity tileEntity;
    if (this.tileEntities.dict.TryGetValue(_posInChunk, out tileEntity) && tileEntity is T)
    {
      tileEntity.IsRemoving = true;
      tileEntity.OnRemove(world);
      this.tileEntities.Remove(_posInChunk);
      tileEntity.IsRemoving = false;
    }
    this.isModified = true;
  }

  public void RemoveAllTileEntities()
  {
    this.isModified = this.tileEntities.Count > 0;
    this.tileEntities.Clear();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public byte GetHeight(int _blockOffset) => this.m_HeightMap[_blockOffset];

  public void AddTileEntity(TileEntity _te) => this.tileEntities.Set(_te.localChunkPos, _te);

  public void RemoveTileEntity(World world, TileEntity _te)
  {
    TileEntity tileEntity;
    if (!this.tileEntities.dict.TryGetValue(_te.localChunkPos, out tileEntity) || tileEntity == null)
      return;
    tileEntity.IsRemoving = true;
    tileEntity.OnRemove(world);
    this.tileEntities.Remove(_te.localChunkPos);
    tileEntity.IsRemoving = false;
    this.isModified = true;
  }

  public TileEntity GetTileEntity(Vector3i _blockPosInChunk)
  {
    TileEntity tileEntity;
    return !this.tileEntities.dict.TryGetValue(_blockPosInChunk, out tileEntity) ? (TileEntity) null : tileEntity;
  }

  public DictionaryList<Vector3i, TileEntity> GetTileEntities() => this.tileEntities;

  public void AddSleeperVolumeId(int id)
  {
    if (this.sleeperVolumes.Contains(id))
      return;
    if (this.sleeperVolumes.Count < (int) byte.MaxValue)
      this.sleeperVolumes.Add(id);
    else
      Log.Error("Chunk AddSleeperVolumeId at max");
  }

  public List<int> GetSleeperVolumes() => this.sleeperVolumes;

  public void AddTriggerVolumeId(int id)
  {
    if (this.triggerVolumes.Contains(id))
      return;
    if (this.triggerVolumes.Count < (int) byte.MaxValue)
      this.triggerVolumes.Add(id);
    else
      Log.Error("Chunk AddTriggerVolumeId at max");
  }

  public List<int> GetTriggerVolumes() => this.triggerVolumes;

  public void AddWallVolumeId(int id)
  {
    if (this.wallVolumes.Contains(id))
      return;
    if (this.wallVolumes.Count < (int) byte.MaxValue)
      this.wallVolumes.Add(id);
    else
      Log.Error("Chunk AddWallVolume at max");
  }

  public List<int> GetWallVolumes() => this.wallVolumes;

  public int GetTickRefCount(int _layerIdx)
  {
    return this.m_BlockLayers[_layerIdx] == null ? 0 : this.m_BlockLayers[_layerIdx].GetTickRefCount();
  }

  public DictionaryList<Vector3i, BlockTrigger> GetBlockTriggers() => this.triggerData;

  public void AddBlockTrigger(BlockTrigger _td)
  {
    this.triggerData.Set(_td.LocalChunkPos, _td);
    this.isModified = true;
  }

  public void RemoveBlockTrigger(BlockTrigger _td)
  {
    BlockTrigger blockTrigger;
    if (!this.triggerData.dict.TryGetValue(_td.LocalChunkPos, out blockTrigger) || blockTrigger == null)
      return;
    this.triggerData.Remove(_td.LocalChunkPos);
    this.isModified = true;
  }

  public void RemoveBlockTrigger(Vector3i _blockPos)
  {
    if (!this.triggerData.dict.ContainsKey(_blockPos))
      return;
    this.triggerData.Remove(_blockPos);
    this.isModified = true;
  }

  public BlockTrigger GetBlockTrigger(Vector3i _blockPosInChunk)
  {
    BlockTrigger blockTrigger;
    this.triggerData.dict.TryGetValue(_blockPosInChunk, out blockTrigger);
    return blockTrigger;
  }

  public void UpdateTick(World _world, bool _bSpawnEnemies)
  {
    this.ProfilerBegin("TeTick");
    for (int index = 0; index < this.tileEntities.list.Count; ++index)
      this.tileEntities.list[index].UpdateTick(_world);
    this.ProfilerEnd();
  }

  public bool NeedsTicking => this.tileEntities.Count > 0 || this.sleeperVolumes.Count > 0;

  public bool IsOpenSkyAbove(int _x, int _y, int _z) => _y >= (int) this.GetHeight(_x, _z);

  public void GetLivingEntitiesInBounds(
    EntityAlive _excludeEntity,
    Bounds _aabb,
    List<EntityAlive> _entityOutputList)
  {
    int num1 = Utils.Fastfloor(((double) ((Bounds) ref _aabb).min.y - 5.0) / 16.0);
    int num2 = Utils.Fastfloor(((double) ((Bounds) ref _aabb).max.y + 5.0) / 16.0);
    if (num1 < 0)
      num1 = 0;
    if (num2 >= 16 /*0x10*/)
      num2 = 15;
    for (int index1 = num1; index1 <= num2; ++index1)
    {
      List<Entity> entityList = this.entityLists[index1];
      for (int index2 = 0; index2 < entityList.Count; ++index2)
      {
        EntityAlive _other = entityList[index2] as EntityAlive;
        if (!Object.op_Equality((Object) _other, (Object) null) && !Object.op_Equality((Object) _other, (Object) _excludeEntity) && !_other.IsDead() && ((Bounds) ref _other.boundingBox).Intersects(_aabb) && (!Object.op_Inequality((Object) _excludeEntity, (Object) null) || _excludeEntity.CanCollideWith((Entity) _other)))
          _entityOutputList.Add(_other);
      }
    }
  }

  public void GetEntitiesInBounds(
    Entity _excludeEntity,
    Bounds _aabb,
    List<Entity> _entityOutputList,
    bool isAlive)
  {
    int num1 = Utils.Fastfloor(((double) ((Bounds) ref _aabb).min.y - 5.0) / 16.0);
    int num2 = Utils.Fastfloor(((double) ((Bounds) ref _aabb).max.y + 5.0) / 16.0);
    if (num1 < 0)
      num1 = 0;
    if (num2 >= 16 /*0x10*/)
      num2 = 15;
    for (int index1 = num1; index1 <= num2; ++index1)
    {
      List<Entity> entityList = this.entityLists[index1];
      for (int index2 = 0; index2 < entityList.Count; ++index2)
      {
        Entity _other = entityList[index2];
        if (!Object.op_Equality((Object) _other, (Object) _excludeEntity) && isAlive == _other.IsAlive() && ((Bounds) ref _other.boundingBox).Intersects(_aabb) && (!Object.op_Inequality((Object) _excludeEntity, (Object) null) || _excludeEntity.CanCollideWith(_other)))
          _entityOutputList.Add(_other);
      }
    }
  }

  public void GetEntitiesInBounds(FastTags<TagGroup.Global> _tags, Bounds _bb, List<Entity> _list)
  {
    int num1 = Utils.Fastfloor(((double) ((Bounds) ref _bb).min.y - 5.0) / 16.0);
    int num2 = Utils.Fastfloor(((double) ((Bounds) ref _bb).max.y + 5.0) / 16.0);
    if (num1 < 0)
      num1 = 0;
    else if (num1 >= 16 /*0x10*/)
      num1 = 15;
    if (num2 >= 16 /*0x10*/)
      num2 = 15;
    else if (num2 < 0)
      num2 = 0;
    for (int index1 = num1; index1 <= num2; ++index1)
    {
      List<Entity> entityList = this.entityLists[index1];
      for (int index2 = 0; index2 < entityList.Count; ++index2)
      {
        Entity entity = entityList[index2];
        if (entity.HasAnyTags(_tags) && ((Bounds) ref entity.boundingBox).Intersects(_bb))
          _list.Add(entity);
      }
    }
  }

  public void GetEntitiesInBounds(System.Type _class, Bounds _bb, List<Entity> _list)
  {
    int num1 = Utils.Fastfloor(((double) ((Bounds) ref _bb).min.y - 5.0) / 16.0);
    int num2 = Utils.Fastfloor(((double) ((Bounds) ref _bb).max.y + 5.0) / 16.0);
    if (num1 < 0)
      num1 = 0;
    else if (num1 >= 16 /*0x10*/)
      num1 = 15;
    if (num2 >= 16 /*0x10*/)
      num2 = 15;
    else if (num2 < 0)
      num2 = 0;
    for (int index1 = num1; index1 <= num2; ++index1)
    {
      List<Entity> entityList = this.entityLists[index1];
      for (int index2 = 0; index2 < entityList.Count; ++index2)
      {
        Entity entity = entityList[index2];
        if (_class.IsAssignableFrom(((object) entity).GetType()) && ((Bounds) ref entity.boundingBox).Intersects(_bb))
          _list.Add(entity);
      }
    }
  }

  public void GetEntitiesAround(
    EntityFlags _mask,
    Vector3 _pos,
    float _radius,
    List<Entity> _list)
  {
    int num1 = Utils.Fastfloor(((double) _pos.y - (double) _radius) / 16.0);
    int num2 = Utils.Fastfloor(((double) _pos.y + (double) _radius) / 16.0);
    if (num1 < 0)
      num1 = 0;
    else if (num1 >= 16 /*0x10*/)
      num1 = 15;
    if (num2 >= 16 /*0x10*/)
      num2 = 15;
    else if (num2 < 0)
      num2 = 0;
    float num3 = _radius * _radius;
    for (int index1 = num1; index1 <= num2; ++index1)
    {
      List<Entity> entityList = this.entityLists[index1];
      for (int index2 = 0; index2 < entityList.Count; ++index2)
      {
        Entity entity = entityList[index2];
        if ((entity.entityFlags & _mask) != EntityFlags.None)
        {
          Vector3 vector3 = Vector3.op_Subtraction(entity.position, _pos);
          if ((double) ((Vector3) ref vector3).sqrMagnitude <= (double) num3)
            _list.Add(entity);
        }
      }
    }
  }

  public void GetEntitiesAround(
    EntityFlags _flags,
    EntityFlags _mask,
    Vector3 _pos,
    float _radius,
    List<Entity> _list)
  {
    int num1 = Utils.Fastfloor(((double) _pos.y - (double) _radius) / 16.0);
    int num2 = Utils.Fastfloor(((double) _pos.y + (double) _radius) / 16.0);
    if (num1 < 0)
      num1 = 0;
    else if (num1 >= 16 /*0x10*/)
      num1 = 15;
    if (num2 >= 16 /*0x10*/)
      num2 = 15;
    else if (num2 < 0)
      num2 = 0;
    float num3 = _radius * _radius;
    for (int index1 = num1; index1 <= num2; ++index1)
    {
      List<Entity> entityList = this.entityLists[index1];
      for (int index2 = 0; index2 < entityList.Count; ++index2)
      {
        Entity entity = entityList[index2];
        if ((entity.entityFlags & _mask) == _flags)
        {
          Vector3 vector3 = Vector3.op_Subtraction(entity.position, _pos);
          if ((double) ((Vector3) ref vector3).sqrMagnitude <= (double) num3)
            _list.Add(entity);
        }
      }
    }
  }

  public void RemoveEntityFromChunk(Entity _entity)
  {
    this.entityLists[_entity.chunkPosAddedEntityTo.y].Remove(_entity);
    this.isModified = true;
    bool flag = false;
    for (int index = 0; index < 16 /*0x10*/; ++index)
    {
      if (this.entityLists[index].Count > 0)
      {
        flag = true;
        break;
      }
    }
    this.hasEntities = flag;
  }

  public void AddEntityToChunk(Entity _entity)
  {
    this.hasEntities = true;
    int chunkXz1 = World.toChunkXZ(Utils.Fastfloor(_entity.position.x));
    int chunkXz2 = World.toChunkXZ(Utils.Fastfloor(_entity.position.z));
    if (chunkXz1 != this.m_X || chunkXz2 != this.m_Z)
      Log.Error($"Wrong entity chunk position! {((object) _entity)?.ToString()} x={chunkXz1.ToString()} z={chunkXz2.ToString()}/{this?.ToString()}");
    int index = Utils.Fastfloor((double) _entity.position.y / 16.0);
    if (index < 0)
      index = 0;
    if (index >= 16 /*0x10*/)
      index = 15;
    _entity.addedToChunk = true;
    _entity.chunkPosAddedEntityTo.x = this.m_X;
    _entity.chunkPosAddedEntityTo.y = index;
    _entity.chunkPosAddedEntityTo.z = this.m_Z;
    this.entityLists[index].Add(_entity);
  }

  public void AdJustEntityTracking(Entity _entity)
  {
    if (!_entity.addedToChunk)
      return;
    int index = Utils.Fastfloor((double) _entity.position.y / 16.0);
    if (index < 0)
      index = 0;
    if (index >= 16 /*0x10*/)
      index = 15;
    if (_entity.chunkPosAddedEntityTo.y == index)
      return;
    this.entityLists[_entity.chunkPosAddedEntityTo.y].Remove(_entity);
    _entity.chunkPosAddedEntityTo.y = index;
    this.entityLists[index].Add(_entity);
    this.isModified = true;
  }

  public Bounds GetAABB() => this.boundingBox;

  public static Bounds CalculateAABB(int _chunkX, int _chunkY, int _chunkZ)
  {
    return BoundsUtils.BoundsForMinMax((float) (_chunkX * 16 /*0x10*/), (float) (_chunkY * 256 /*0x0100*/), (float) (_chunkZ * 16 /*0x10*/), (float) (_chunkX * 16 /*0x10*/ + 16 /*0x10*/), (float) (_chunkY * 256 /*0x0100*/ + 256 /*0x0100*/), (float) (_chunkZ * 16 /*0x10*/ + 16 /*0x10*/));
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void updateBounds()
  {
    this.boundingBox = Chunk.CalculateAABB(this.m_X, this.m_Y, this.m_Z);
    this.worldPosIMin.x = this.m_X << 4;
    this.worldPosIMin.y = this.m_Y << 8;
    this.worldPosIMin.z = this.m_Z << 4;
    this.worldPosIMax.x = this.worldPosIMin.x + 15;
    this.worldPosIMax.y = this.worldPosIMin.y + (int) byte.MaxValue;
    this.worldPosIMax.z = this.worldPosIMin.z + 15;
  }

  public int GetTris() => this.totalTris;

  public int GetTrisInMesh(int _idx)
  {
    int trisInMesh = 0;
    for (int index = 0; index < this.trisInMesh.GetLength(0); ++index)
      trisInMesh += this.trisInMesh[index][_idx];
    return trisInMesh;
  }

  public int GetSizeOfMesh(int _idx)
  {
    int sizeOfMesh = 0;
    for (int index = 0; index < this.trisInMesh.GetLength(0); ++index)
      sizeOfMesh += this.sizeOfMesh[index][_idx];
    return sizeOfMesh;
  }

  public int GetUsedMem()
  {
    this.TotalMemory = 0;
    for (int index = 0; index < this.m_BlockLayers.Length; ++index)
      this.TotalMemory += this.m_BlockLayers[index] != null ? this.m_BlockLayers[index].GetUsedMem() : 0;
    this.TotalMemory += 12;
    this.TotalMemory += this.m_TerrainHeight.Length;
    this.TotalMemory += this.m_HeightMap.Length;
    this.TotalMemory += this.m_Biomes.Length;
    this.TotalMemory += this.m_BiomeIntensities.Length;
    this.TotalMemory += this.m_NormalX.Length;
    this.TotalMemory += this.m_NormalY.Length;
    this.TotalMemory += this.m_NormalZ.Length;
    this.TotalMemory += this.chnStability.GetUsedMem();
    this.TotalMemory += this.chnLight.GetUsedMem();
    this.TotalMemory += this.chnDensity.GetUsedMem();
    this.TotalMemory += this.chnDamage.GetUsedMem();
    for (int index = 0; index < 1; ++index)
      this.TotalMemory += this.chnTextures[index].GetUsedMem();
    this.TotalMemory += this.chnWater.GetUsedMem();
    return this.TotalMemory;
  }

  public void GetTextureChannelMemory(out int[] texMem)
  {
    texMem = new int[1];
    for (int index = 0; index < 1; ++index)
      texMem[index] = this.chnTextures[index].GetUsedMem();
  }

  public void OnLoadedFromCache()
  {
    this.NeedsRegeneration = true;
    this.isModified = true;
    this.InProgressRegeneration = false;
    this.InProgressSaving = false;
    this.InProgressCopying = false;
    this.InProgressDecorating = false;
    this.InProgressLighting = false;
    this.InProgressUnloading = false;
    this.NeedsOnlyCollisionMesh = false;
    this.IsCollisionMeshGenerated = false;
    this.entityStubs.Clear();
    for (int index1 = 0; index1 < 16 /*0x10*/; ++index1)
    {
      for (int index2 = 0; index2 < this.entityLists[index1].Count; ++index2)
      {
        if (this.entityLists[index1][index2].IsSavedToFile())
          this.entityStubs.Add(new EntityCreationData(this.entityLists[index1][index2]));
      }
      this.entityLists[index1].Clear();
    }
  }

  public void OnLoad(World _world)
  {
    if (!_world.IsRemote())
    {
      for (int index = 0; index < this.entityStubs.Count; ++index)
      {
        EntityCreationData entityStub = this.entityStubs[index];
        if (!Object.op_Inequality((Object) _world.GetEntity(entityStub.id), (Object) null))
          _world.SpawnEntityInWorld(EntityFactory.CreateEntity(entityStub));
      }
      this.removeExpiredCustomChunkDataEntries(_world.GetWorldTime());
    }
    if (!_world.IsEditor())
      GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled);
    for (int index = 0; index < this.m_BlockLayers.Length; ++index)
    {
      if (this.m_BlockLayers[index] != null)
        this.m_BlockLayers[index].OnLoad((WorldBase) _world, 0, this.X * 16 /*0x10*/, index * 4, this.Z * 16 /*0x10*/);
    }
  }

  public void OnUnload(WorldBase _world)
  {
    this.ProfilerBegin("Chunk OnUnload");
    this.InProgressUnloading = true;
    if (this.biomeParticles != null)
    {
      this.ProfilerBegin("biome particles");
      for (int index = 0; index < this.biomeParticles.Count; ++index)
        Object.Destroy((Object) this.biomeParticles[index]);
      this.biomeParticles = (List<GameObject>) null;
      this.ProfilerEnd();
    }
    this.spawnedBiomeParticles = false;
    if (!_world.IsRemote())
    {
      this.ProfilerBegin("enities");
      for (int index = 0; index < 16 /*0x10*/; ++index)
      {
        if (this.entityLists[index].Count != 0)
          _world.UnloadEntities(this.entityLists[index]);
      }
      this.ProfilerEnd();
      this.removeExpiredCustomChunkDataEntries(_world.GetWorldTime());
    }
    this.ProfilerBegin("tile entities");
    for (int index = 0; index < this.tileEntities.list.Count; ++index)
      this.tileEntities.list[index].OnUnload(GameManager.Instance.World);
    this.ProfilerEnd();
    this.RemoveBlockEntityTransforms();
    this.ProfilerBegin("block layers");
    for (int index = 0; index < this.m_BlockLayers.Length; ++index)
    {
      if (this.m_BlockLayers[index] != null)
        this.m_BlockLayers[index].OnUnload(_world, 0, this.X * 16 /*0x10*/, index * 4, this.Z * 16 /*0x10*/);
    }
    this.ProfilerEnd();
    this.ProfilerBegin("water");
    this.waterSimHandle.Reset();
    this.ProfilerEnd();
    this.ProfilerEnd();
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void SpawnBiomeParticles(Transform _parentForEntityBlocks)
  {
    if (this.spawnedBiomeParticles)
      return;
    this.biomeParticles = BiomeParticleManager.SpawnParticles(this, _parentForEntityBlocks);
    this.spawnedBiomeParticles = true;
  }

  public void OnDisplay(World _world, Transform _entityBlocksParentT, ChunkCluster _chunkCluster)
  {
    this.ProfilerBegin("Chunk OnDisplay");
    this.SpawnBiomeParticles(_entityBlocksParentT);
    this.displayState = Chunk.DisplayState.BlockEntities;
    this.blockEntitiesIndex = 0;
    this.blockEntityStubs.list.Sort((Comparison<BlockEntityData>) ([PublicizedFrom(EAccessModifier.Internal)] (a, b) => a.pos.y.CompareTo(b.pos.y)));
    this.ProfilerEnd();
  }

  public void OnDisplayBlockEntities(
    World _world,
    Transform _entityBlocksParentT,
    ChunkCluster _chunkCluster)
  {
    this.ProfilerBegin("Chunk OnDisplayBlockEntities");
    Vector3 vector3_1;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3_1).\u002Ector((float) (this.X * 16 /*0x10*/), 0.0f, (float) (this.Z * 16 /*0x10*/));
    int num1 = _chunkCluster.LayerMappingTable["nocollision"];
    int num2 = _chunkCluster.LayerMappingTable["terraincollision"];
    int num3 = 0;
    int num4 = Utils.FastMax(50, this.blockEntityStubs.list.Count / 3 + 8);
    for (; this.blockEntitiesIndex < this.blockEntityStubs.list.Count; ++this.blockEntitiesIndex)
    {
      BlockEntityData blockEntityData = this.blockEntityStubs.list[this.blockEntitiesIndex];
      if (blockEntityData.bHasTransform)
      {
        if (!this.NeedsOnlyCollisionMesh && !blockEntityData.bRenderingOn)
          this.SetBlockEntityRendering(blockEntityData, true);
      }
      else
      {
        if (++num3 > num4)
        {
          this.ProfilerEnd();
          return;
        }
        BlockValue block1 = _chunkCluster.GetBlock(blockEntityData.pos);
        if (!this.IsInternalBlocksCulled || block1.type == blockEntityData.blockValue.type)
        {
          Block block2 = blockEntityData.blockValue.Block;
          if (!(block2.shape is BlockShapeModelEntity shape))
          {
            this.RemoveEntityBlockStub(blockEntityData.pos);
          }
          else
          {
            float num5 = 0.0f;
            if (block2.IsTerrainDecoration && _world.GetBlock(blockEntityData.pos - Vector3i.up).Block.shape.IsTerrain())
              num5 = _world.GetDecorationOffsetY(blockEntityData.pos);
            Quaternion rotation = shape.GetRotation(block1);
            Vector3 rotatedOffset = shape.GetRotatedOffset(block2, rotation);
            rotatedOffset.x += 0.5f;
            rotatedOffset.z += 0.5f;
            rotatedOffset.y += num5;
            Vector3 vector3_2 = Vector3.op_Addition(blockEntityData.pos.ToVector3(), rotatedOffset);
            GameObject objectForType = GameObjectPool.Instance.GetObjectForType(shape.modelName, out block2.defaultTintColor);
            if (Object.op_Implicit((Object) objectForType))
            {
              this.ProfilerBegin("BE setup");
              Transform transform = objectForType.transform;
              blockEntityData.transform = transform;
              blockEntityData.bHasTransform = true;
              transform.SetParent(_entityBlocksParentT, false);
              transform.localScale = Vector3.one;
              transform.SetLocalPositionAndRotation(Vector3.op_Subtraction(vector3_2, vector3_1), rotation);
              int num6 = block2.IsCollideMovement ? 1 : 0;
              int newLayer = num1;
              if (num6 != 0)
              {
                switch (objectForType.layer)
                {
                  case 4:
                    break;
                  case 30:
                    newLayer = _chunkCluster.LayerMappingTable["Glass"];
                    break;
                  default:
                    newLayer = num2;
                    break;
                }
              }
              Utils.SetColliderLayerRecursively(objectForType, newLayer);
              Vector3i localPosition = Chunk.ToLocalPosition(blockEntityData.pos);
              this.ProfilerBegin("BE TBA");
              block2.OnBlockEntityTransformBeforeActivated((WorldBase) _world, blockEntityData.pos, this.GetBlock(localPosition.x, localPosition.y, localPosition.z), blockEntityData);
              this.ProfilerEnd();
              objectForType.SetActive(true);
              this.ProfilerBegin("BE TAA");
              block2.OnBlockEntityTransformAfterActivated((WorldBase) _world, blockEntityData.pos, 0, this.GetBlock(localPosition.x, localPosition.y, localPosition.z), blockEntityData);
              this.ProfilerEnd();
              if (this.NeedsOnlyCollisionMesh)
                this.SetBlockEntityRendering(blockEntityData, false);
              else
                Chunk.occlusionTs.Add(blockEntityData.transform);
              this.ProfilerEnd();
            }
          }
        }
      }
    }
    if (Chunk.occlusionTs.Count > 0)
    {
      if (OcclusionManager.Instance.cullChunkEntities)
      {
        this.ProfilerBegin("BE occlusion");
        OcclusionManager.Instance.AddChunkTransforms(this, Chunk.occlusionTs);
        this.ProfilerEnd();
      }
      Chunk.occlusionTs.Clear();
    }
    this.removeBlockEntitesMarkedForRemoval();
    AstarManager.AddBoundsToUpdate(this.boundingBox);
    this.displayState = Chunk.DisplayState.Done;
    DynamicMeshThread.AddChunkGameObject(this);
    this.ProfilerEnd();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3i ToLocalPosition(Vector3i _pos)
  {
    _pos.x &= 15;
    _pos.y &= (int) byte.MaxValue;
    _pos.z &= 15;
    return _pos;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void removeBlockEntitesMarkedForRemoval()
  {
    if (OcclusionManager.Instance.cullChunkEntities)
    {
      for (int index = 0; index < this.blockEntityStubsToRemove.Count; ++index)
      {
        BlockEntityData blockEntityData = this.blockEntityStubsToRemove[index];
        if (blockEntityData.bHasTransform)
          Chunk.occlusionTs.Add(blockEntityData.transform);
      }
      if (Chunk.occlusionTs.Count > 0)
      {
        OcclusionManager.Instance.RemoveChunkTransforms(this, Chunk.occlusionTs);
        Chunk.occlusionTs.Clear();
      }
    }
    for (int index = 0; index < this.blockEntityStubsToRemove.Count; ++index)
    {
      BlockEntityData _bed = this.blockEntityStubsToRemove[index];
      _bed.Cleanup();
      if (_bed.bHasTransform)
        this.poolBlockEntityTransform(_bed);
    }
    this.blockEntityStubsToRemove.Clear();
  }

  public void OnHide()
  {
    this.RemoveBlockEntityTransforms();
    AstarManager.AddBoundsToUpdate(this.boundingBox);
  }

  public void RemoveBlockEntityTransforms()
  {
    this.ProfilerBegin(nameof (RemoveBlockEntityTransforms));
    if (OcclusionManager.Instance.cullChunkEntities)
    {
      this.ProfilerBegin("OcclusionManager RemoveChunk");
      OcclusionManager.Instance.RemoveChunk(this);
      this.ProfilerEnd();
    }
    for (int index = 0; index < this.blockEntityStubs.list.Count; ++index)
    {
      BlockEntityData _bed = this.blockEntityStubs.list[index];
      if (_bed.bHasTransform)
        this.poolBlockEntityTransform(_bed);
    }
    this.ProfilerEnd();
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void poolBlockEntityTransform(BlockEntityData _bed)
  {
    if (!_bed.bRenderingOn)
      this.SetBlockEntityRendering(_bed, true);
    if (Object.op_Implicit((Object) _bed.transform))
      GameObjectPool.Instance.PoolObject(((Component) _bed.transform).gameObject);
    else
      Log.Error("BlockEntity {0} at pos {1} null transform!", new object[2]
      {
        (object) _bed.ToString(),
        (object) _bed.pos
      });
    _bed.bHasTransform = false;
    _bed.transform = (Transform) null;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void SetBlockEntityRendering(BlockEntityData _bed, bool _bOn)
  {
    _bed.bRenderingOn = _bOn;
    if (!Object.op_Implicit((Object) _bed.transform))
    {
      Log.Error($"2: {_bed.ToString()} on pos {_bed.pos} with empty transform/gameobject!");
    }
    else
    {
      this.ProfilerBegin("SetBlockEntityRendering set enable");
      ((Component) _bed.transform).GetComponentsInChildren<MeshRenderer>(Chunk.tempMeshRenderers);
      for (int index = 0; index < Chunk.tempMeshRenderers.Count; ++index)
        ((Renderer) Chunk.tempMeshRenderers[index]).enabled = _bOn;
      Chunk.tempMeshRenderers.Clear();
      this.ProfilerEnd();
      this.ProfilerBegin("SetBlockEntityRendering BroadcastMessage");
      if (_bOn)
        ((Component) _bed.transform).BroadcastMessage("SetRenderingOn", (SendMessageOptions) 1);
      else
        ((Component) _bed.transform).BroadcastMessage("SetRenderingOff", (SendMessageOptions) 1);
      this.ProfilerEnd();
    }
  }

  public static void ToTerrain(Chunk _chunk, Chunk _terrainChunk)
  {
    for (int _x = 0; _x < 16 /*0x10*/; ++_x)
    {
      for (int _z = 0; _z < 16 /*0x10*/; ++_z)
      {
        byte height = _chunk.GetHeight(_x, _z);
        for (int _y = 0; _y <= (int) height; ++_y)
        {
          if (!_chunk.GetBlock(_x, _y, _z).isair)
            _terrainChunk.SetBlockRaw(_x, _y, _z, Constants.cTerrainBlockValue);
        }
        for (int _y = 0; _y < 256 /*0x0100*/; ++_y)
          _terrainChunk.SetDensity(_x, _y, _z, _chunk.GetDensity(_x, _y, _z));
        _terrainChunk.SetHeight(_x, _z, height);
        _terrainChunk.SetTerrainHeight(_x, _z, height);
      }
    }
    _terrainChunk.CopyLightsFrom(_chunk);
    _terrainChunk.isModified = true;
    _terrainChunk.NeedsLightCalculation = false;
  }

  public void AddMeshLayer(VoxelMeshLayer _vml)
  {
    for (int _idx = 0; _idx < MeshDescription.meshes.Length; ++_idx)
    {
      this.trisInMesh[_vml.idx][_idx] = _vml.GetTrisInMesh(_idx);
      this.sizeOfMesh[_vml.idx][_idx] = _vml.GetSizeOfMesh(_idx);
    }
    this.totalTris = 0;
    for (int index1 = 0; index1 < this.trisInMesh.GetLength(0); ++index1)
    {
      for (int index2 = 0; index2 < MeshDescription.meshes.Length; ++index2)
        this.totalTris += this.trisInMesh[index1][index2];
    }
    lock (this.m_layerIndexQueue)
    {
      VoxelMeshLayer meshLayer = this.m_meshLayers[_vml.idx];
      if (meshLayer == null)
      {
        ++this.MeshLayerCount;
        this.m_layerIndexQueue.Enqueue(_vml.idx);
      }
      else
        MemoryPools.poolVML.FreeSync(meshLayer);
      this.m_meshLayers[_vml.idx] = _vml;
    }
  }

  public bool HasMeshLayer()
  {
    lock (this.m_layerIndexQueue)
      return this.m_layerIndexQueue.Count > 0;
  }

  public VoxelMeshLayer GetMeshLayer()
  {
    lock (this.m_layerIndexQueue)
    {
      if (this.m_layerIndexQueue.Count <= 0)
        return (VoxelMeshLayer) null;
      --this.MeshLayerCount;
      int index = this.m_layerIndexQueue.Dequeue();
      VoxelMeshLayer meshLayer = this.m_meshLayers[index];
      this.m_meshLayers[index] = (VoxelMeshLayer) null;
      return meshLayer;
    }
  }

  public EnumDecoAllowed GetDecoAllowedAt(int x, int z)
  {
    EnumDecoAllowed decoAllowed = EnumDecoAllowed.Everything;
    if (this.m_DecoBiomeArray != null)
      decoAllowed = this.m_DecoBiomeArray[x + z * 16 /*0x10*/];
    if (decoAllowed.GetSize() < EnumDecoAllowedSize.NoBigOnlySmall)
    {
      switch (DecoManager.Instance.GetDecoOccupiedAt(x + this.m_X * 16 /*0x10*/, z + this.m_Z * 16 /*0x10*/))
      {
        case EnumDecoOccupied.Free:
        case EnumDecoOccupied.SmallSlope:
        case EnumDecoOccupied.Stop_BigDeco:
        case EnumDecoOccupied.Perimeter:
        case EnumDecoOccupied.POI:
          break;
        default:
          decoAllowed = decoAllowed.WithSize(EnumDecoAllowedSize.NoBigNoSmall);
          break;
      }
    }
    return decoAllowed;
  }

  public EnumDecoAllowedSlope GetDecoAllowedSlopeAt(int x, int z)
  {
    return this.GetDecoAllowedAt(x, z).GetSlope();
  }

  public EnumDecoAllowedSize GetDecoAllowedSizeAt(int x, int z)
  {
    return this.GetDecoAllowedAt(x, z).GetSize();
  }

  public bool GetDecoAllowedStreetOnlyAt(int x, int z)
  {
    return this.GetDecoAllowedAt(x, z).GetStreetOnly();
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void EnsureDecoBiomeArray()
  {
    if (this.m_DecoBiomeArray != null)
      return;
    this.m_DecoBiomeArray = new EnumDecoAllowed[256 /*0x0100*/];
  }

  public void SetDecoAllowedAt(int x, int z, EnumDecoAllowed _newVal)
  {
    this.EnsureDecoBiomeArray();
    int index = x + z * 16 /*0x10*/;
    int decoBiome = (int) this.m_DecoBiomeArray[index];
    EnumDecoAllowedSlope slope = ((EnumDecoAllowed) decoBiome).GetSlope();
    if (slope > _newVal.GetSlope())
      _newVal = _newVal.WithSlope(slope);
    EnumDecoAllowedSize size = ((EnumDecoAllowed) decoBiome).GetSize();
    if (size > _newVal.GetSize())
      _newVal = _newVal.WithSize(size);
    if (((EnumDecoAllowed) decoBiome).GetStreetOnly() && !_newVal.GetStreetOnly())
      _newVal = _newVal.WithStreetOnly(true);
    this.m_DecoBiomeArray[index] = _newVal;
  }

  public void SetDecoAllowedSlopeAt(int x, int z, EnumDecoAllowedSlope _newVal)
  {
    this.EnsureDecoBiomeArray();
    int index = x + z * 16 /*0x10*/;
    this.SetDecoAllowedAt(x, z, this.m_DecoBiomeArray[index].WithSlope(_newVal));
  }

  public void SetDecoAllowedSizeAt(int x, int z, EnumDecoAllowedSize _newVal)
  {
    this.EnsureDecoBiomeArray();
    int index = x + z * 16 /*0x10*/;
    this.SetDecoAllowedAt(x, z, this.m_DecoBiomeArray[index].WithSize(_newVal));
  }

  public void SetDecoAllowedStreetOnlyAt(int x, int z, bool _newVal)
  {
    this.EnsureDecoBiomeArray();
    int index = x + z * 16 /*0x10*/;
    this.SetDecoAllowedAt(x, z, this.m_DecoBiomeArray[index].WithStreetOnly(_newVal));
  }

  public Vector3 GetTerrainNormal(int _x, int _z)
  {
    int index = _x + _z * 16 /*0x10*/;
    Vector3 terrainNormal;
    terrainNormal.x = (float) (sbyte) this.m_NormalX[index] / (float) sbyte.MaxValue;
    terrainNormal.y = (float) (sbyte) this.m_NormalY[index] / (float) sbyte.MaxValue;
    terrainNormal.z = (float) (sbyte) this.m_NormalZ[index] / (float) sbyte.MaxValue;
    return terrainNormal;
  }

  public float GetTerrainNormalY(int _x, int _z)
  {
    return (float) (sbyte) this.m_NormalY[_x + _z * 16 /*0x10*/] / (float) sbyte.MaxValue;
  }

  public void SetTerrainNormal(int x, int z, Vector3 _v)
  {
    int index = x + z * 16 /*0x10*/;
    this.m_NormalX[index] = (byte) Utils.FastClamp(_v.x * (float) sbyte.MaxValue, (float) sbyte.MinValue, (float) sbyte.MaxValue);
    this.m_NormalY[index] = (byte) Utils.FastClamp(_v.y * (float) sbyte.MaxValue, (float) sbyte.MinValue, (float) sbyte.MaxValue);
    this.m_NormalZ[index] = (byte) Utils.FastClamp(_v.z * (float) sbyte.MaxValue, (float) sbyte.MinValue, (float) sbyte.MaxValue);
  }

  public Vector3i ToWorldPos()
  {
    return new Vector3i(this.m_X * 16 /*0x10*/, this.m_Y * 256 /*0x0100*/, this.m_Z * 16 /*0x10*/);
  }

  public Vector3i ToWorldPos(int _x, int _y, int _z)
  {
    return new Vector3i(this.m_X * 16 /*0x10*/ + _x, this.m_Y * 256 /*0x0100*/ + _y, this.m_Z * 16 /*0x10*/ + _z);
  }

  public Vector3i ToWorldPos(Vector3i _pos)
  {
    return new Vector3i(this.m_X * 16 /*0x10*/, this.m_Y * 256 /*0x0100*/, this.m_Z * 16 /*0x10*/) + _pos;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void updateFullMap()
  {
    if (this.mapColors == null)
      this.mapColors = new ushort[256 /*0x0100*/];
    for (int _x = 0; _x < 16 /*0x10*/; ++_x)
    {
      for (int _z = 0; _z < 16 /*0x10*/; ++_z)
      {
        int index1 = _x + _z * 16 /*0x10*/;
        int height = (int) this.m_HeightMap[index1];
        int index2 = height >> 2;
        BlockValue _blockValue = this.m_BlockLayers[index2] != null ? this.m_BlockLayers[index2].GetAt(_x, height, _z) : BlockValue.Air;
        WaterValue water;
        for (water = this.GetWater(_x, height, _z); height > 0 && (_blockValue.isair || _blockValue.Block.IsTerrainDecoration) && !water.HasMass(); water = this.GetWater(_x, height, _z))
        {
          --height;
          _blockValue = this.m_BlockLayers[index2] != null ? this.m_BlockLayers[index2].GetAt(_x, height, _z) : BlockValue.Air;
        }
        Color col = BlockLiquidv2.Color;
        if (!water.HasMass())
        {
          float num1 = (float) (sbyte) this.m_NormalX[index1] / (float) sbyte.MaxValue;
          float num2 = (float) (sbyte) this.m_NormalY[index1] / (float) sbyte.MaxValue;
          float num3 = (float) (sbyte) this.m_NormalZ[index1] / (float) sbyte.MaxValue;
          col = _blockValue.Block.GetMapColor(_blockValue, new Vector3(num1, num2, num3), height);
        }
        this.mapColors[index1] = Utils.ToColor5(col);
      }
    }
    this.bMapDirty = false;
    ModEvents.SCalcChunkColorsDoneData _data = new ModEvents.SCalcChunkColorsDoneData(this);
    ModEvents.CalcChunkColorsDone.Invoke(ref _data);
  }

  public ushort[] GetMapColors()
  {
    if (this.mapColors == null || this.bMapDirty)
      this.updateFullMap();
    return this.mapColors;
  }

  public void OnDecorated()
  {
    this.CheckSameDensity();
    this.CheckOnlyTerrain();
  }

  public bool IsAreaMaster() => this.m_X % 5 == 0 && this.m_Z % 5 == 0;

  public bool IsAreaMasterCornerChunksLoaded(ChunkCluster _cc)
  {
    return _cc.GetChunkSync(this.m_X - 2, this.m_Z) != null && _cc.GetChunkSync(this.m_X, this.m_Z + 2) != null && _cc.GetChunkSync(this.m_X + 2, this.m_Z + 2) != null && _cc.GetChunkSync(this.m_X - 2, this.m_Z - 2) != null;
  }

  public static Vector3i ToAreaMasterChunkPos(Vector3i _worldBlockPos)
  {
    return new Vector3i(World.toChunkXZ(_worldBlockPos.x) / 5 * 5, World.toChunkY(_worldBlockPos.y), World.toChunkXZ(_worldBlockPos.z) / 5 * 5);
  }

  public bool IsAreaMasterDominantBiomeInitialized(ChunkCluster _cc)
  {
    if (this.AreaMasterDominantBiome != byte.MaxValue)
      return true;
    if (_cc == null)
      return false;
    for (int index = 0; index < 50; ++index)
      Chunk.biomeCnt[index] = 0;
    for (int _x = this.m_X - 2; _x < this.m_X + 2; ++_x)
    {
      for (int _y = this.m_Z - 2; _y < this.m_Z + 2; ++_y)
      {
        Chunk chunkSync = _cc.GetChunkSync(_x, _y);
        if (chunkSync == null)
          return false;
        if (chunkSync.DominantBiome > (byte) 0)
          ++Chunk.biomeCnt[(int) chunkSync.DominantBiome];
      }
    }
    int num = 0;
    for (int index = 1; index < Chunk.biomeCnt.Length; ++index)
    {
      if (Chunk.biomeCnt[index] > num)
      {
        this.AreaMasterDominantBiome = (byte) index;
        num = Chunk.biomeCnt[index];
      }
    }
    return true;
  }

  public ChunkAreaBiomeSpawnData GetChunkBiomeSpawnData()
  {
    if (this.AreaMasterDominantBiome == byte.MaxValue)
      return (ChunkAreaBiomeSpawnData) null;
    if (this.biomeSpawnData == null)
    {
      global::ChunkCustomData _ccd;
      if (!this.ChunkCustomData.dict.TryGetValue("bspd.main", out _ccd) || _ccd == null)
      {
        _ccd = new global::ChunkCustomData("bspd.main", ulong.MaxValue, false);
        this.ChunkCustomData.Set(_ccd.key, _ccd);
      }
      this.biomeSpawnData = new ChunkAreaBiomeSpawnData(this, this.AreaMasterDominantBiome, _ccd);
    }
    return this.biomeSpawnData;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void removeExpiredCustomChunkDataEntries(ulong _worldTime)
  {
    List<string> stringList = (List<string>) null;
    for (int index = 0; index < this.ChunkCustomData.valueList.Count; ++index)
    {
      if (this.ChunkCustomData.valueList[index].expiresInWorldTime <= _worldTime)
      {
        if (stringList == null)
          stringList = new List<string>();
        stringList.Add(this.ChunkCustomData.keyList[index]);
        this.ChunkCustomData.valueList[index].OnRemove(this);
      }
    }
    if (stringList == null)
      return;
    for (int index = 0; index < stringList.Count; ++index)
      this.ChunkCustomData.Remove(stringList[index]);
  }

  public bool IsTraderArea(int _x, int _z)
  {
    Vector3i worldPosImin = this.worldPosIMin;
    worldPosImin.x += _x;
    worldPosImin.z += _z;
    return GameManager.Instance.World.IsWithinTraderArea(worldPosImin);
  }

  public override int GetHashCode() => 31 /*0x1F*/ * this.m_X + this.m_Z;

  public void EnterReadLock() => this.sync.EnterReadLock();

  public void EnterWriteLock() => this.sync.EnterWriteLock();

  public void ExitReadLock() => this.sync.ExitReadLock();

  public void ExitWriteLock() => this.sync.ExitWriteLock();

  public override bool Equals(object obj)
  {
    return base.Equals(obj) && obj.GetHashCode() == this.GetHashCode();
  }

  public override string ToString()
  {
    if (this.cachedToString == null)
      this.cachedToString = $"Chunk_{this.m_X},{this.m_Z}";
    return this.cachedToString;
  }

  public List<Chunk.DensityMismatchInformation> CheckDensities(bool _logAllMismatches = false)
  {
    Vector3i vector3i1 = new Vector3i(0, 0, 0);
    Vector3i vector3i2 = new Vector3i(16 /*0x10*/, 256 /*0x0100*/, 16 /*0x10*/);
    int num1 = this.m_X << 4;
    int num2 = this.m_Y << 8;
    int num3 = this.m_Z << 4;
    bool flag = true;
    List<Chunk.DensityMismatchInformation> mismatchInformationList = new List<Chunk.DensityMismatchInformation>();
    for (int x = vector3i1.x; x < vector3i2.x; ++x)
    {
      for (int z = vector3i1.z; z < vector3i2.z; ++z)
      {
        for (int y = vector3i1.y; y < vector3i2.y; ++y)
        {
          sbyte density = this.GetDensity(x, y, z);
          BlockValue block = this.GetBlock(x, y, z);
          bool _isTerrain = block.Block.shape.IsTerrain();
          if (!(!_isTerrain ? density >= (sbyte) 0 : density < (sbyte) 0))
          {
            Chunk.DensityMismatchInformation mismatchInformation = new Chunk.DensityMismatchInformation(num1 + x, num2 + y, num3 + z, density, block.type, _isTerrain);
            mismatchInformationList.Add(mismatchInformation);
            if (flag | _logAllMismatches)
            {
              Log.Warning(mismatchInformation.ToString());
              flag = false;
            }
          }
        }
      }
    }
    return mismatchInformationList;
  }

  public bool RepairDensities()
  {
    Vector3i vector3i1 = new Vector3i(0, 0, 0);
    Vector3i vector3i2 = new Vector3i(16 /*0x10*/, 256 /*0x0100*/, 16 /*0x10*/);
    bool flag = false;
    for (int x = vector3i1.x; x < vector3i2.x; ++x)
    {
      for (int z = vector3i1.z; z < vector3i2.z; ++z)
      {
        for (int y = vector3i1.y; y < vector3i2.y; ++y)
        {
          Block block = this.GetBlock(x, y, z).Block;
          sbyte density = this.GetDensity(x, y, z);
          if (block.shape.IsTerrain())
          {
            if (density >= (sbyte) 0)
            {
              this.SetDensity(x, y, z, (sbyte) -1);
              flag = true;
            }
          }
          else if (density < (sbyte) 0)
          {
            this.SetDensity(x, y, z, (sbyte) 1);
            flag = true;
          }
        }
      }
    }
    return flag;
  }

  public void LoopOverAllBlocks(
    ChunkBlockLayer.LoopBlocksDelegate _delegate,
    bool _bIncludeChilds = false,
    bool _bIncludeAirBlocks = false)
  {
    for (int index = 0; index < this.m_BlockLayers.Length; ++index)
      this.m_BlockLayers[index]?.LoopOverAllBlocks(this, index << 2, _delegate, _bIncludeChilds, _bIncludeAirBlocks);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public bool isInside(int _x, int _y, int _z)
  {
    return this.insideDevicesHashSet.Contains(new Vector3b(_x, _y, _z).GetHashCode());
  }

  public BlockFaceFlag RestoreCulledBlocks(World _world)
  {
    BlockFaceFlag blockFaceFlag = BlockFaceFlag.None;
    for (int index = this.insideDevices.Count - 1; index >= 0; --index)
    {
      Vector3b insideDevice = this.insideDevices[index];
      if (insideDevice.x == (byte) 0)
        blockFaceFlag |= BlockFaceFlag.West;
      else if (insideDevice.x == (byte) 15)
        blockFaceFlag |= BlockFaceFlag.East;
      if (insideDevice.z == (byte) 0)
        blockFaceFlag |= BlockFaceFlag.North;
      else if (insideDevice.z == (byte) 15)
        blockFaceFlag |= BlockFaceFlag.South;
    }
    this.IsInternalBlocksCulled = false;
    return blockFaceFlag;
  }

  public bool HasFallingBlocks()
  {
    foreach (List<Entity> entityList in this.entityLists)
    {
      for (int index = 0; index < entityList.Count; ++index)
      {
        if (entityList[index] is EntityFallingBlock)
          return true;
      }
    }
    return false;
  }

  [Conditional("DEBUG_CHUNK_PROFILE")]
  [PublicizedFrom(EAccessModifier.Private)]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void ProfilerBegin(string _name)
  {
  }

  [Conditional("DEBUG_CHUNK_PROFILE")]
  [PublicizedFrom(EAccessModifier.Private)]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void ProfilerEnd()
  {
  }

  [Conditional("DEBUG_CHUNK_RWCHECK")]
  [PublicizedFrom(EAccessModifier.Private)]
  public void RWCheck(PooledBinaryReader stream)
  {
    if (stream.ReadInt32() == 1431655765 /*0x55555555*/)
      return;
    Log.Error("Chunk !RWCheck");
  }

  [Conditional("DEBUG_CHUNK_RWCHECK")]
  [PublicizedFrom(EAccessModifier.Private)]
  public void RWCheck(PooledBinaryWriter stream) => stream.Write(1431655765 /*0x55555555*/);

  [Conditional("DEBUG_CHUNK_TRIGGERLOG")]
  public void LogTrigger(string _format = "", params object[] _args)
  {
    _format = $"{GameManager.frameCount} Chunk {this.ChunkPos} trigger {_format}";
    Log.Warning(_format, _args);
  }

  [Conditional("DEBUG_CHUNK_CHUNK")]
  public static void LogChunk(long _key, string _format = "", params object[] _args)
  {
    int x = WorldChunkCache.extractX(_key);
    int z = WorldChunkCache.extractZ(_key);
    if (x != 136 || z != 25)
      return;
    _format = $"{GameManager.frameCount} Chunk pos {x} {z}, {_format}";
    Log.Warning(_format, _args);
  }

  [Conditional("DEBUG_CHUNK_ENTITY")]
  public void LogEntity(string _format = "", params object[] _args)
  {
    if (this.m_X != 136 || this.m_Z != 25)
      return;
    _format = $"{GameManager.frameCount} Chunk {this.ChunkPos} entity {_format}";
    Log.Warning(_format, _args);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  static Chunk()
  {
  }

  public enum LIGHT_TYPE
  {
    BLOCK,
    SUN,
  }

  public enum DisplayState
  {
    Start,
    BlockEntities,
    Done,
  }

  public struct DensityMismatchInformation(
    int _x,
    int _y,
    int _z,
    sbyte _density,
    int _bvType,
    bool _isTerrain)
  {
    public int x = _x;
    public int y = _y;
    public int z = _z;
    public sbyte density = _density;
    public int bvType = _bvType;
    public bool isTerrain = _isTerrain;

    public string ToJsonString()
    {
      return $"{{\"x\":{this.x}, \"y\":{this.y}, \"z\":{this.z}, \"density\":{this.density}, \"bvtype\":{this.bvType}, \"terrain\":{this.isTerrain.ToString().ToLower()}}}";
    }

    public override string ToString()
    {
      return $"DENSITYMISMATCH;{this.x};{this.y};{this.z};{this.density};{this.isTerrain};{this.bvType}";
    }
  }
}
