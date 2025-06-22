// Decompiled with JetBrains decompiler
// Type: TileEntity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public abstract class TileEntity : ITileEntity
{
  [PublicizedFrom(EAccessModifier.Protected)]
  public const int CurrentSaveVersion = 11;
  public int entityId = -1;
  [PublicizedFrom(EAccessModifier.Private)]
  public Vector3i chunkPos;
  [PublicizedFrom(EAccessModifier.Protected)]
  public int readVersion;
  [PublicizedFrom(EAccessModifier.Protected)]
  public Chunk chunk;
  [PublicizedFrom(EAccessModifier.Private)]
  public ulong heapMapLastTime;
  [PublicizedFrom(EAccessModifier.Private)]
  public ulong heapMapUpdateTime;
  [PublicizedFrom(EAccessModifier.Protected)]
  public bool bDisableModifiedCheck;
  [PublicizedFrom(EAccessModifier.Protected)]
  public bool bUserAccessing;
  [CompilerGenerated]
  [PublicizedFrom(EAccessModifier.Private)]
  public bool \u003CIsRemoving\u003Ek__BackingField;
  [PublicizedFrom(EAccessModifier.Private)]
  public readonly List<ITileEntityChangedListener> _listeners = new List<ITileEntityChangedListener>();
  [PublicizedFrom(EAccessModifier.Private)]
  public byte handleCounter;
  [PublicizedFrom(EAccessModifier.Private)]
  public byte lockHandleWaitingFor = byte.MaxValue;

  public int EntityId => this.entityId;

  public Vector3i localChunkPos
  {
    get => this.chunkPos;
    set
    {
      this.chunkPos = value;
      this.OnSetLocalChunkPosition();
    }
  }

  public BlockValue blockValue => this.chunk.GetBlock(this.localChunkPos);

  public bool IsRemoving
  {
    get => this.\u003CIsRemoving\u003Ek__BackingField;
    set => this.\u003CIsRemoving\u003Ek__BackingField = value;
  }

  public event XUiEvent_TileEntityDestroyed Destroyed;

  public List<ITileEntityChangedListener> listeners => this._listeners;

  public bool bWaitingForServerResponse => this.lockHandleWaitingFor != byte.MaxValue;

  public TileEntity(Chunk _chunk) => this.chunk = _chunk;

  public virtual TileEntity Clone()
  {
    throw new NotImplementedException("Clone() not implemented yet");
  }

  public virtual void CopyFrom(TileEntity _other)
  {
    throw new NotImplementedException("CopyFrom() not implemented yet");
  }

  public virtual void UpdateTick(World world)
  {
  }

  public abstract TileEntityType GetTileEntityType();

  public virtual void OnSetLocalChunkPosition()
  {
  }

  public Vector3i ToWorldPos()
  {
    return this.chunk != null ? new Vector3i(this.chunk.X * 16 /*0x10*/, this.chunk.Y * 256 /*0x0100*/, this.chunk.Z * 16 /*0x10*/) + this.localChunkPos : Vector3i.zero;
  }

  public Vector3 ToWorldCenterPos()
  {
    if (this.entityId != -1)
    {
      Entity entity = GameManager.Instance.World.GetEntity(this.entityId);
      if (Object.op_Implicit((Object) entity))
        return entity.position;
    }
    if (this.chunk == null)
      return Vector3.zero;
    BlockValue blockNoDamage = this.chunk.GetBlockNoDamage(this.chunkPos.x, this.chunkPos.y, this.chunkPos.z);
    Block block = blockNoDamage.Block;
    Vector3 worldCenterPos;
    worldCenterPos.x = (float) (this.chunk.X * 16 /*0x10*/ + this.chunkPos.x);
    worldCenterPos.y = (float) (this.chunk.Y * 256 /*0x0100*/ + this.chunkPos.y);
    worldCenterPos.z = (float) (this.chunk.Z * 16 /*0x10*/ + this.chunkPos.z);
    if (!block.isMultiBlock)
    {
      worldCenterPos.x += 0.5f;
      worldCenterPos.y += 0.5f;
      worldCenterPos.z += 0.5f;
    }
    else if (block.shape is BlockShapeModelEntity shape)
    {
      Quaternion rotation = shape.GetRotation(blockNoDamage);
      worldCenterPos = Vector3.op_Addition(worldCenterPos, shape.GetRotatedOffset(block, rotation));
      worldCenterPos.x += 0.5f;
      worldCenterPos.z += 0.5f;
    }
    return worldCenterPos;
  }

  public int GetClrIdx() => this.chunk == null ? 0 : this.chunk.ClrIdx;

  public Chunk GetChunk() => this.chunk;

  public void SetChunk(Chunk _chunk) => this.chunk = _chunk;

  public static TileEntity Instantiate(TileEntityType type, Chunk _chunk)
  {
    switch (type)
    {
      case TileEntityType.DewCollector:
        return (TileEntity) new TileEntityDewCollector(_chunk);
      case TileEntityType.LandClaim:
        return (TileEntity) new TileEntityLandClaim(_chunk);
      case TileEntityType.Loot:
        return (TileEntity) new TileEntityLootContainer(_chunk);
      case TileEntityType.Trader:
        return (TileEntity) new TileEntityTrader(_chunk);
      case TileEntityType.VendingMachine:
        return (TileEntity) new TileEntityVendingMachine(_chunk);
      case TileEntityType.Forge:
        return (TileEntity) new TileEntityForge(_chunk);
      case TileEntityType.SecureLoot:
        return (TileEntity) new TileEntitySecureLootContainer(_chunk);
      case TileEntityType.SecureDoor:
        return (TileEntity) new TileEntitySecureDoor(_chunk);
      case TileEntityType.Workstation:
        return (TileEntity) new TileEntityWorkstation(_chunk);
      case TileEntityType.Sign:
        return (TileEntity) new TileEntitySign(_chunk);
      case TileEntityType.GoreBlock:
        return (TileEntity) new TileEntityGoreBlock(_chunk);
      case TileEntityType.Powered:
        return (TileEntity) new TileEntityPoweredBlock(_chunk);
      case TileEntityType.PowerSource:
        return (TileEntity) new TileEntityPowerSource(_chunk);
      case TileEntityType.PowerRangeTrap:
        return (TileEntity) new TileEntityPoweredRangedTrap(_chunk);
      case TileEntityType.Light:
        return (TileEntity) new TileEntityLight(_chunk);
      case TileEntityType.Trigger:
        return (TileEntity) new TileEntityPoweredTrigger(_chunk);
      case TileEntityType.Sleeper:
        return (TileEntity) new TileEntitySleeper(_chunk);
      case TileEntityType.PowerMeleeTrap:
        return (TileEntity) new TileEntityPoweredMeleeTrap(_chunk);
      case TileEntityType.SecureLootSigned:
        return (TileEntity) new TileEntitySecureLootContainerSigned(_chunk);
      case TileEntityType.Composite:
        return (TileEntity) new TileEntityComposite(_chunk);
      default:
        Log.Warning("Dropping TE with unknown type: " + type.ToStringCached<TileEntityType>());
        return (TileEntity) null;
    }
  }

  public virtual void read(PooledBinaryReader _br, TileEntity.StreamModeRead _eStreamMode)
  {
    if (_eStreamMode == TileEntity.StreamModeRead.Persistency)
    {
      this.readVersion = (int) _br.ReadUInt16();
      this.chunkPos = StreamUtils.ReadVector3i((BinaryReader) _br);
      this.entityId = _br.ReadInt32();
      if (this.readVersion <= 1)
        return;
      this.heapMapUpdateTime = _br.ReadUInt64();
      this.heapMapLastTime = this.heapMapUpdateTime - AIDirector.GetActivityWorldTimeDelay();
    }
    else
    {
      this.chunkPos = StreamUtils.ReadVector3i((BinaryReader) _br);
      this.entityId = _br.ReadInt32();
    }
  }

  public virtual void write(PooledBinaryWriter _bw, TileEntity.StreamModeWrite _eStreamMode)
  {
    if (_eStreamMode == TileEntity.StreamModeWrite.Persistency)
    {
      _bw.Write((ushort) 11);
      StreamUtils.Write((BinaryWriter) _bw, this.chunkPos);
      _bw.Write(this.entityId);
      _bw.Write(this.heapMapUpdateTime);
    }
    else
    {
      StreamUtils.Write((BinaryWriter) _bw, this.chunkPos);
      _bw.Write(this.entityId);
    }
  }

  public override string ToString()
  {
    return string.Format($"[TE] {this.GetTileEntityType().ToStringCached<TileEntityType>()}/{this.ToWorldPos().ToString()}/{this.entityId.ToString()}");
  }

  public virtual void OnRemove(World world) => this.OnDestroy();

  public virtual void OnUnload(World world)
  {
  }

  public virtual void OnReadComplete()
  {
  }

  public void SetDisableModifiedCheck(bool _b) => this.bDisableModifiedCheck = _b;

  public void SetModified() => this.setModified();

  public void SetChunkModified()
  {
    if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer || this.chunk == null)
      return;
    this.chunk.isModified = true;
  }

  public virtual bool IsActive(World world) => false;

  [PublicizedFrom(EAccessModifier.Protected)]
  public bool IsByWater(World _world, Vector3i _blockPos)
  {
    return _world.IsWater(_blockPos.x, _blockPos.y + 1, _blockPos.z) | _world.IsWater(_blockPos.x + 1, _blockPos.y, _blockPos.z) | _world.IsWater(_blockPos.x - 1, _blockPos.y, _blockPos.z) | _world.IsWater(_blockPos.x, _blockPos.y, _blockPos.z + 1) | _world.IsWater(_blockPos.x, _blockPos.y, _blockPos.z - 1);
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public void emitHeatMapEvent(World world, EnumAIDirectorChunkEvent eventType)
  {
    if (world.worldTime < this.heapMapLastTime)
      this.heapMapUpdateTime = 0UL;
    if (world.worldTime < this.heapMapUpdateTime || world.aiDirector == null)
      return;
    Vector3i worldPos = this.ToWorldPos();
    Block block = world.GetBlock(worldPos).Block;
    if (block == null)
      return;
    world.aiDirector.NotifyActivity(eventType, worldPos, block.HeatMapStrength);
    this.heapMapLastTime = world.worldTime;
    this.heapMapUpdateTime = world.worldTime + AIDirector.GetActivityWorldTimeDelay();
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual void setModified()
  {
    if (this.bDisableModifiedCheck)
      return;
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
    {
      this.SetChunkModified();
      Vector3? _entitiesInRangeOfWorldPos = new Vector3?(this.ToWorldCenterPos());
      if (Vector3.op_Equality(_entitiesInRangeOfWorldPos.Value, Vector3.zero))
        _entitiesInRangeOfWorldPos = new Vector3?();
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageTileEntity>().Setup(this, TileEntity.StreamModeWrite.ToClient, byte.MaxValue), true, _entitiesInRangeOfWorldPos: _entitiesInRangeOfWorldPos);
    }
    else
    {
      if (++this.handleCounter == byte.MaxValue)
        this.handleCounter = (byte) 0;
      this.lockHandleWaitingFor = this.handleCounter;
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageTileEntity>().Setup(this, TileEntity.StreamModeWrite.ToServer, this.lockHandleWaitingFor));
    }
    this.NotifyListeners();
  }

  public override int GetHashCode()
  {
    return this.entityId != -1 ? this.entityId | 134217728 /*0x08000000*/ : this.ToWorldPos().GetHashCode() & int.MaxValue;
  }

  public override bool Equals(object obj)
  {
    return base.Equals(obj) && obj.GetHashCode() == this.GetHashCode();
  }

  public void NotifyListeners()
  {
    for (int index = 0; index < this.listeners.Count; ++index)
      this.listeners[index].OnTileEntityChanged((ITileEntity) this);
  }

  public virtual void UpgradeDowngradeFrom(TileEntity _other) => _other.OnDestroy();

  public virtual void ReplacedBy(BlockValue _bvOld, BlockValue _bvNew, TileEntity _teNew)
  {
  }

  public virtual void SetUserAccessing(bool _bUserAccessing)
  {
    this.bUserAccessing = _bUserAccessing;
  }

  public bool IsUserAccessing() => this.bUserAccessing;

  public virtual void SetHandle(byte _handle)
  {
    if (this.lockHandleWaitingFor == byte.MaxValue || (int) this.lockHandleWaitingFor != (int) _handle)
      return;
    this.lockHandleWaitingFor = byte.MaxValue;
  }

  public virtual void OnDestroy()
  {
    if (this.Destroyed == null)
      return;
    this.Destroyed((ITileEntity) this);
  }

  public virtual void Reset(FastTags<TagGroup.Global> questTags)
  {
  }

  public enum StreamModeRead
  {
    Persistency,
    FromServer,
    FromClient,
  }

  public enum StreamModeWrite
  {
    Persistency,
    ToServer,
    ToClient,
  }
}
