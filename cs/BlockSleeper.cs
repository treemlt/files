// Decompiled with JetBrains decompiler
// Type: BlockSleeper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
[Preserve]
public class BlockSleeper : Block
{
  [PublicizedFrom(EAccessModifier.Private)]
  public static readonly string PropPose = "Pose";
  [PublicizedFrom(EAccessModifier.Private)]
  public static readonly string PropLookIdentity = "LookIdentity";
  [PublicizedFrom(EAccessModifier.Private)]
  public static readonly string PropExcludeWalkType = "ExcludeWalkType";
  [PublicizedFrom(EAccessModifier.Private)]
  public static readonly string PropSpawnGroup = "SpawnGroup";
  [PublicizedFrom(EAccessModifier.Private)]
  public static readonly string PropSpawnMode = "SpawnMode";
  public int pose;
  public Vector3 look;
  public string spawnGroup;
  public BlockSleeper.eMode spawnMode;
  [PublicizedFrom(EAccessModifier.Private)]
  public List<int> excludedWalkTypes;
  [PublicizedFrom(EAccessModifier.Private)]
  public new BlockActivationCommand[] cmds = new BlockActivationCommand[1]
  {
    new BlockActivationCommand("open", "dummy", true)
  };

  public BlockSleeper()
  {
    this.IsSleeperBlock = true;
    this.HasTileEntity = true;
  }

  public override void Init()
  {
    base.Init();
    this.Properties.ParseInt(BlockSleeper.PropPose, ref this.pose);
    this.look = Vector3.forward;
    this.Properties.ParseVec(BlockSleeper.PropLookIdentity, ref this.look);
    string str = this.Properties.GetString(BlockSleeper.PropExcludeWalkType);
    if (str.Length > 0)
    {
      string[] strArray = str.Split(',', StringSplitOptions.None);
      this.excludedWalkTypes = new List<int>();
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (strArray[index] == "Crawler")
          this.excludedWalkTypes.Add(21);
        else
          Log.Warning("Block {0}, invalid ExcludeWalkType {1}", new object[2]
          {
            (object) this.GetBlockName(),
            (object) strArray[index]
          });
      }
    }
    this.Properties.ParseString(BlockSleeper.PropSpawnGroup, ref this.spawnGroup);
    this.Properties.ParseEnum<BlockSleeper.eMode>(BlockSleeper.PropSpawnMode, ref this.spawnMode);
  }

  public override bool CanPlaceBlockAt(
    WorldBase _world,
    int _clrIdx,
    Vector3i _blockPos,
    BlockValue _blockValue,
    bool _bOmitCollideCheck = false)
  {
    return _world.IsEditor() || base.CanPlaceBlockAt(_world, _clrIdx, _blockPos, _blockValue, _bOmitCollideCheck);
  }

  public float GetSleeperRotation(BlockValue _blockValue)
  {
    switch (_blockValue.rotation)
    {
      case 1:
        return 90f;
      case 2:
        return 180f;
      case 3:
        return 270f;
      case 24:
        return 45f;
      case 25:
        return 135f;
      case 26:
        return 225f;
      case 27:
        return 315f;
      default:
        return 0.0f;
    }
  }

  public bool ExcludesWalkType(int _walkType)
  {
    return this.excludedWalkTypes != null && this.excludedWalkTypes.Contains(_walkType);
  }

  public override void OnBlockAdded(
    WorldBase _world,
    Chunk _chunk,
    Vector3i _blockPos,
    BlockValue _blockValue,
    PlatformUserIdentifierAbs _addedByPlayer)
  {
    base.OnBlockAdded(_world, _chunk, _blockPos, _blockValue, _addedByPlayer);
    TileEntitySleeper _te = new TileEntitySleeper(_chunk);
    _te.localChunkPos = World.toBlock(_blockPos);
    _chunk.AddTileEntity((TileEntity) _te);
  }

  public override void OnBlockRemoved(
    WorldBase _world,
    Chunk _chunk,
    Vector3i _blockPos,
    BlockValue _blockValue)
  {
    base.OnBlockRemoved(_world, _chunk, _blockPos, _blockValue);
    _chunk.RemoveTileEntityAt<TileEntitySleeper>((World) _world, World.toBlock(_blockPos));
  }

  public override string GetActivationText(
    WorldBase _world,
    BlockValue _blockValue,
    int _clrIdx,
    Vector3i _blockPos,
    EntityAlive _entityFocusing)
  {
    return _world.IsEditor() ? "Configure Sleeper" : (string) null;
  }

  public override bool OnBlockActivated(
    string _commandName,
    WorldBase _world,
    int _cIdx,
    Vector3i _blockPos,
    BlockValue _blockValue,
    EntityPlayerLocal _player)
  {
    if (!(_world.GetTileEntity(_cIdx, _blockPos) is TileEntitySleeper tileEntity) || !Object.op_Inequality((Object) _player, (Object) null))
      return false;
    XUiC_WoPropsSleeperBlock.Open(_player.PlayerUI, tileEntity);
    return true;
  }

  public override bool HasBlockActivationCommands(
    WorldBase _world,
    BlockValue _blockValue,
    int _clrIdx,
    Vector3i _blockPos,
    EntityAlive _entityFocusing)
  {
    return true;
  }

  public override BlockActivationCommand[] GetBlockActivationCommands(
    WorldBase _world,
    BlockValue _blockValue,
    int _clrIdx,
    Vector3i _blockPos,
    EntityAlive _entityFocusing)
  {
    this.cmds[0].enabled = true;
    return this.cmds;
  }

  public override bool IsTileEntitySavedInPrefab() => true;

  [PublicizedFrom(EAccessModifier.Private)]
  static BlockSleeper()
  {
  }

  public enum eMode
  {
    Normal,
    Bandit,
    Infested,
  }
}
