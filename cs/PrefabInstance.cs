// Decompiled with JetBrains decompiler
// Type: PrefabInstance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PrefabInstance
{
  public int id;
  public byte rotation;
  public byte imposterBaseRotation;
  public Prefab prefab;
  public byte lastCopiedRotation;
  public Vector3i lastCopiedPrefabPosition;
  public bool bPrefabCopiedIntoWorld;
  public Vector3i boundingBoxPosition;
  public Vector3i boundingBoxSize;
  public string name;
  public PathAbstractions.AbstractedLocation location;
  [PublicizedFrom(EAccessModifier.Private)]
  public bool imposterLookupDone;
  [PublicizedFrom(EAccessModifier.Private)]
  public PathAbstractions.AbstractedLocation imposterLocation = PathAbstractions.AbstractedLocation.None;
  public int standaloneBlockSize;
  public float yOffsetOfPrefab;
  public QuestLockInstance lockInstance;
  public List<SleeperVolume> sleeperVolumes = new List<SleeperVolume>();
  public List<TriggerVolume> triggerVolumes = new List<TriggerVolume>();
  public List<WallVolume> wallVolumes = new List<WallVolume>();
  [PublicizedFrom(EAccessModifier.Private)]
  public List<int> entityInstanceIds = new List<int>();
  public FastTags<TagGroup.Global> LastRefreshType = FastTags<TagGroup.Global>.none;
  public QuestClass LastQuestClass;
  [PublicizedFrom(EAccessModifier.Private)]
  public HashSetLong occupiedChunks;

  public PrefabInstance(
    int _id,
    PathAbstractions.AbstractedLocation _location,
    Vector3i _position,
    byte _rotation,
    Prefab _bad,
    int _standaloneBlockSize)
  {
    this.id = _id;
    if (_bad != null)
    {
      _bad.location = _location;
      this.boundingBoxSize = _bad.size;
    }
    this.name = $"{_location.Name}.{_id.ToString()}";
    this.location = _location;
    this.boundingBoxPosition = _position;
    this.lastCopiedPrefabPosition = Vector3i.zero;
    this.bPrefabCopiedIntoWorld = false;
    this.rotation = this.lastCopiedRotation = _rotation;
    this.prefab = _bad;
    this.standaloneBlockSize = _standaloneBlockSize;
  }

  public Bounds GetAABB()
  {
    return BoundsUtils.BoundsForMinMax((float) this.boundingBoxPosition.x, (float) this.boundingBoxPosition.y, (float) this.boundingBoxPosition.z, (float) (this.boundingBoxPosition.x + this.boundingBoxSize.x), (float) (this.boundingBoxPosition.y + this.boundingBoxSize.y), (float) (this.boundingBoxPosition.z + this.boundingBoxSize.z));
  }

  public Vector2 GetCenterXZ()
  {
    return new Vector2((float) this.boundingBoxPosition.x + (float) this.boundingBoxSize.x * 0.5f, (float) this.boundingBoxPosition.z + (float) this.boundingBoxSize.z * 0.5f);
  }

  public bool IsBBInSyncWithPrefab()
  {
    return this.bPrefabCopiedIntoWorld && this.lastCopiedPrefabPosition.Equals(this.boundingBoxPosition) && this.prefab.size.Equals(this.boundingBoxSize) && (int) this.lastCopiedRotation == (int) this.rotation;
  }

  public void CopyIntoWorld(
    World _world,
    bool _CopyEntities,
    bool _bOverwriteExistingBlocks,
    FastTags<TagGroup.Global> _tags)
  {
    if ((int) this.lastCopiedRotation != (int) this.rotation)
    {
      if ((int) this.lastCopiedRotation < (int) this.rotation)
        this.prefab.RotateY(true, (int) this.rotation - (int) this.lastCopiedRotation);
      else
        this.prefab.RotateY(false, (int) this.lastCopiedRotation - (int) this.rotation);
      this.lastCopiedRotation = this.rotation;
      this.UpdateBoundingBoxPosAndScale(this.boundingBoxPosition, this.prefab.size);
    }
    this.prefab.CopyIntoLocal(_world.ChunkClusters[0], this.boundingBoxPosition, _bOverwriteExistingBlocks, true, _tags);
    if (_CopyEntities)
    {
      bool _bSpawnEnemies = _world.IsEditor() || GameStats.GetBool(EnumGameStats.IsSpawnEnemies);
      this.entityInstanceIds.Clear();
      this.prefab.CopyEntitiesIntoWorld(_world, this.boundingBoxPosition, (ICollection<int>) this.entityInstanceIds, _bSpawnEnemies);
    }
    this.lastCopiedPrefabPosition = this.boundingBoxPosition;
    this.bPrefabCopiedIntoWorld = true;
  }

  public static void RefreshSwitchesInContainingPoi(Quest q)
  {
    Vector3 pos;
    if (GameManager.Instance.World.IsEditor() || !q.GetPositionData(out pos, Quest.PositionDataTypes.POIPosition))
      return;
    World world = GameManager.Instance.World;
    PrefabInstance prefabAtPosition = GameManager.Instance.GetDynamicPrefabDecorator().GetPrefabAtPosition(pos);
    if (prefabAtPosition == null)
      return;
    for (int _z = 0; _z < prefabAtPosition.boundingBoxSize.z; ++_z)
    {
      for (int _x = 0; _x < prefabAtPosition.boundingBoxSize.x; ++_x)
      {
        for (int _y = 0; _y < prefabAtPosition.boundingBoxSize.y; ++_y)
        {
          Vector3i blockPos = World.worldToBlockPos((Vector3) (prefabAtPosition.boundingBoxPosition + new Vector3i(_x, _y, _z)));
          BlockValue block = world.GetBlock(blockPos);
          if (block.Block is BlockActivateSwitch || block.Block is BlockActivateSingle)
            block.Block.Refresh((WorldBase) world, (Chunk) null, 0, blockPos, block);
        }
      }
    }
  }

  public static void RefreshTriggersInContainingPoi(Vector3 v)
  {
    if (GameManager.Instance.World.IsEditor())
      return;
    PrefabInstance prefabAtPosition = GameManager.Instance.GetDynamicPrefabDecorator().GetPrefabAtPosition(v);
    if (prefabAtPosition == null)
      return;
    GameManager.Instance.World.triggerManager.RefreshTriggers(prefabAtPosition, prefabAtPosition.LastRefreshType);
  }

  public void CleanFromWorld(World _world, bool _bRemoveEntities)
  {
    if (!this.bPrefabCopiedIntoWorld)
      return;
    BlockTools.ClearRPC(_world, 0, this.lastCopiedPrefabPosition, this.prefab.size.x, this.prefab.size.y, this.prefab.size.z, true);
    if (_bRemoveEntities)
    {
      List<int> intList = new List<int>();
      for (int index = 0; index < this.entityInstanceIds.Count; ++index)
      {
        int entityInstanceId = this.entityInstanceIds[index];
        Entity entity = _world.GetEntity(entityInstanceId);
        if (Object.op_Inequality((Object) entity, (Object) null) && !entity.IsDead())
          _world.RemoveEntity(entityInstanceId, EnumRemoveEntityReason.Unloaded);
        else
          intList.Add(entityInstanceId);
      }
      for (int index = 0; index < intList.Count; ++index)
        this.entityInstanceIds.Remove(intList[index]);
    }
    this.lastCopiedPrefabPosition = Vector3i.zero;
    this.bPrefabCopiedIntoWorld = false;
  }

  public void AddSleeperVolume(SleeperVolume _volume)
  {
    if (this.sleeperVolumes.Contains(_volume))
      return;
    this.sleeperVolumes.Add(_volume);
  }

  public void AddTriggerVolume(TriggerVolume _volume)
  {
    if (this.triggerVolumes.Contains(_volume))
      return;
    this.triggerVolumes.Add(_volume);
  }

  public void AddWallVolume(WallVolume _volume)
  {
    if (this.wallVolumes.Contains(_volume))
      return;
    this.wallVolumes.Add(_volume);
  }

  public void ResizeBoundingBox(Vector3i _deltaVec)
  {
    Vector3i _size = this.boundingBoxSize + _deltaVec;
    if (_size.x <= 1)
      _size.x = 1;
    if (_size.y <= 1)
      _size.y = 1;
    if (_size.z <= 1)
      _size.z = 1;
    this.UpdateBoundingBoxPosAndScale(this.boundingBoxPosition, _size);
  }

  public void MoveBoundingBox(Vector3i _deltaVec)
  {
    this.UpdateBoundingBoxPosAndScale(this.boundingBoxPosition + _deltaVec, this.boundingBoxSize);
  }

  public void SetBoundingBoxPosition(Vector3i _position)
  {
    this.UpdateBoundingBoxPosAndScale(_position, this.boundingBoxSize);
  }

  public void SetBoundingBoxSize(World _world, Vector3i _size)
  {
    this.UpdateBoundingBoxPosAndScale(this.boundingBoxPosition, _size);
  }

  public void CreateBoundingBox(bool _alsoCreateOtherBoxes = true)
  {
    SelectionBox selectionBox = SelectionBoxManager.Instance.GetCategory("DynamicPrefabs").AddBox(this.name, (Vector3) this.boundingBoxPosition, this.boundingBoxSize, true, true);
    selectionBox.facingDirection = (float) (-((this.prefab.rotationToFaceNorth + (int) this.rotation) % 4) * 90);
    selectionBox.UserData = (object) this;
    selectionBox.SetCaption(this.prefab.PrefabName);
    if (!_alsoCreateOtherBoxes)
      return;
    if (this.prefab.bTraderArea)
    {
      for (int index = 0; index < this.prefab.TeleportVolumes.Count; ++index)
      {
        Prefab.PrefabTeleportVolume teleportVolume = this.prefab.TeleportVolumes[index];
        this.prefab.AddTeleportVolumeSelectionBox(teleportVolume, $"{this.name}_{index.ToString()}", this.boundingBoxPosition + teleportVolume.startPos);
      }
    }
    if (this.prefab.bSleeperVolumes)
    {
      for (int index = 0; index < this.prefab.SleeperVolumes.Count; ++index)
      {
        Prefab.PrefabSleeperVolume sleeperVolume = this.prefab.SleeperVolumes[index];
        this.prefab.AddSleeperVolumeSelectionBox(sleeperVolume, $"{this.name}_{index.ToString()}", this.boundingBoxPosition + sleeperVolume.startPos);
      }
    }
    if (this.prefab.bInfoVolumes)
    {
      for (int index = 0; index < this.prefab.InfoVolumes.Count; ++index)
      {
        Prefab.PrefabInfoVolume infoVolume = this.prefab.InfoVolumes[index];
        this.prefab.AddInfoVolumeSelectionBox(infoVolume, $"{this.name}_{index.ToString()}", this.boundingBoxPosition + infoVolume.startPos);
      }
    }
    if (this.prefab.bWallVolumes)
    {
      for (int index = 0; index < this.prefab.WallVolumes.Count; ++index)
      {
        Prefab.PrefabWallVolume wallVolume = this.prefab.WallVolumes[index];
        this.prefab.AddWallVolumeSelectionBox(wallVolume, $"{this.name}_{index.ToString()}", this.boundingBoxPosition + wallVolume.startPos);
      }
    }
    if (this.prefab.bTriggerVolumes)
    {
      for (int index = 0; index < this.prefab.TriggerVolumes.Count; ++index)
      {
        Prefab.PrefabTriggerVolume triggerVolume = this.prefab.TriggerVolumes[index];
        this.prefab.AddTriggerVolumeSelectionBox(triggerVolume, $"{this.name}_{index.ToString()}", this.boundingBoxPosition + triggerVolume.startPos);
      }
    }
    if (!this.prefab.bPOIMarkers)
      return;
    for (int index = 0; index < this.prefab.POIMarkers.Count; ++index)
    {
      Prefab.Marker poiMarker = this.prefab.POIMarkers[index];
      this.prefab.AddPOIMarker($"{poiMarker.GroupName}_{index.ToString()}", this.boundingBoxPosition, poiMarker.Start, poiMarker.Size, poiMarker.GroupName, poiMarker.Tags, poiMarker.MarkerType, index);
    }
  }

  public void UpdateBoundingBoxPosAndScale(Vector3i _pos, Vector3i _size, bool _moveSleepers = true)
  {
    if (_moveSleepers)
      this.prefab.MoveVolumes(this.boundingBoxPosition - _pos);
    this.boundingBoxPosition = _pos;
    this.boundingBoxSize = _size;
    SelectionBox box1 = SelectionBoxManager.Instance.GetCategory("DynamicPrefabs").GetBox(this.name);
    box1.SetPositionAndSize((Vector3) this.boundingBoxPosition, this.boundingBoxSize);
    box1.facingDirection = (float) ((this.prefab.rotationToFaceNorth + (int) this.rotation) % 4 * 90);
    if ((double) box1.facingDirection == 90.0)
      box1.facingDirection = 270f;
    else if ((double) box1.facingDirection == 270.0)
      box1.facingDirection = 90f;
    if (this.prefab.bSleeperVolumes)
    {
      SelectionCategory category = SelectionBoxManager.Instance.GetCategory("SleeperVolume");
      bool _visible = category.IsVisible();
      for (int index = 0; index < this.prefab.SleeperVolumes.Count; ++index)
      {
        Prefab.PrefabSleeperVolume sleeperVolume = this.prefab.SleeperVolumes[index];
        if (sleeperVolume.used)
        {
          string _name = $"{this.name}_{index.ToString()}";
          SelectionBox box2 = category.GetBox(_name);
          if (Object.op_Inequality((Object) box2, (Object) null))
          {
            box2.SetPositionAndSize((Vector3) (this.boundingBoxPosition + sleeperVolume.startPos), sleeperVolume.size);
            box2.SetVisible(_visible);
          }
        }
      }
    }
    if (this.prefab.bTraderArea)
    {
      SelectionCategory category = SelectionBoxManager.Instance.GetCategory("TraderTeleport");
      for (int index = 0; index < this.prefab.TeleportVolumes.Count; ++index)
      {
        if (this.prefab.TeleportVolumes[index].used)
        {
          SelectionBox box3 = category.GetBox($"{this.name}_{index.ToString()}");
          if (Object.op_Inequality((Object) box3, (Object) null))
          {
            box3.SetPositionAndSize((Vector3) (this.boundingBoxPosition + this.prefab.TeleportVolumes[index].startPos), this.prefab.TeleportVolumes[index].size);
            box3.SetVisible(category.IsVisible());
          }
        }
      }
    }
    if (this.prefab.bTriggerVolumes)
    {
      SelectionCategory category = SelectionBoxManager.Instance.GetCategory("TriggerVolume");
      for (int index = 0; index < this.prefab.TriggerVolumes.Count; ++index)
      {
        if (this.prefab.TriggerVolumes[index].used)
        {
          SelectionBox box4 = category.GetBox($"{this.name}_{index.ToString()}");
          if (Object.op_Inequality((Object) box4, (Object) null))
          {
            box4.SetPositionAndSize((Vector3) (this.boundingBoxPosition + this.prefab.TriggerVolumes[index].startPos), this.prefab.TriggerVolumes[index].size);
            box4.SetVisible(category.IsVisible());
          }
        }
      }
    }
    if (this.prefab.bInfoVolumes)
    {
      SelectionCategory category = SelectionBoxManager.Instance.GetCategory("InfoVolume");
      for (int index = 0; index < this.prefab.InfoVolumes.Count; ++index)
      {
        if (this.prefab.InfoVolumes[index].used)
        {
          SelectionBox box5 = category.GetBox($"{this.name}_{index.ToString()}");
          if (Object.op_Inequality((Object) box5, (Object) null))
          {
            box5.SetPositionAndSize((Vector3) (this.boundingBoxPosition + this.prefab.InfoVolumes[index].startPos), this.prefab.InfoVolumes[index].size);
            box5.SetVisible(category.IsVisible());
          }
        }
      }
    }
    if (this.prefab.bWallVolumes)
    {
      SelectionCategory category = SelectionBoxManager.Instance.GetCategory("WallVolume");
      for (int index = 0; index < this.prefab.WallVolumes.Count; ++index)
      {
        SelectionBox box6 = category.GetBox($"{this.name}_{index.ToString()}");
        if (Object.op_Inequality((Object) box6, (Object) null))
        {
          box6.SetPositionAndSize((Vector3) (this.boundingBoxPosition + this.prefab.WallVolumes[index].startPos), this.prefab.WallVolumes[index].size);
          box6.SetVisible(category.IsVisible());
        }
      }
    }
    if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
      return;
    GameManager.Instance.GetDynamicPrefabDecorator()?.CallPrefabChangedEvent(this);
  }

  public SelectionBox GetBox()
  {
    return SelectionBoxManager.Instance.GetCategory("DynamicPrefabs").GetBox(this.name);
  }

  public void RotateAroundY()
  {
    this.rotation = (byte) (((int) this.rotation + 1) % 4);
    MathUtils.Swap(ref this.boundingBoxSize.x, ref this.boundingBoxSize.z);
    this.UpdateBoundingBoxPosAndScale(this.boundingBoxPosition, this.boundingBoxSize);
  }

  public void SetRotation(byte _rotation)
  {
    while ((int) this.rotation != (int) (byte) ((uint) _rotation & 3U))
    {
      this.rotation = (byte) ((int) this.rotation + 1 & 3);
      this.prefab.RotateY(false, 1);
      this.boundingBoxSize = this.prefab.size;
    }
  }

  public bool Overlaps(Chunk _chunk)
  {
    Vector3i worldPosImax = _chunk.worldPosIMax;
    if (worldPosImax.x < this.boundingBoxPosition.x || worldPosImax.y < this.boundingBoxPosition.y || worldPosImax.z < this.boundingBoxPosition.z)
      return false;
    Vector3i worldPosImin = _chunk.worldPosIMin;
    Vector3i vector3i = this.boundingBoxPosition + this.boundingBoxSize;
    return worldPosImin.x < vector3i.x && worldPosImin.y < vector3i.y && worldPosImin.z < vector3i.z;
  }

  public bool Overlaps(Vector3 _pos, float _expandBounds = 0.0f)
  {
    Bounds aabb = this.GetAABB();
    ((Bounds) ref aabb).Expand(_expandBounds);
    Vector3 max = ((Bounds) ref aabb).max;
    Vector3 min = ((Bounds) ref aabb).min;
    return (double) _pos.x <= (double) max.x && (double) _pos.x >= (double) min.x && (double) _pos.y <= (double) max.y && (double) _pos.y >= (double) min.y && (double) _pos.z <= (double) max.z && (double) _pos.z >= (double) min.z;
  }

  public bool IsWithinInfoArea(Vector3 _pos)
  {
    if (this.prefab.InfoVolumes.Count == 0)
      return true;
    foreach (Prefab.PrefabInfoVolume infoVolume in this.prefab.InfoVolumes)
    {
      Vector3i vector3i = this.boundingBoxPosition + infoVolume.startPos;
      if (((double) (vector3i.x - 1) > (double) _pos.x || (double) _pos.x > (double) (vector3i.x + infoVolume.size.x + 1) || (double) (vector3i.y - 1) > (double) _pos.y || (double) _pos.y > (double) (vector3i.y + infoVolume.size.y + 1) || (double) (vector3i.z - 1) > (double) _pos.z ? 0 : ((double) _pos.z <= (double) (vector3i.z + infoVolume.size.z + 1) ? 1 : 0)) != 0)
        return true;
    }
    return false;
  }

  public void CopyIntoChunk(World _world, Chunk _chunk, bool _bForceOverwriteBlocks = false)
  {
    this.prefab.CopyBlocksIntoChunkNoEntities(_world, _chunk, this.boundingBoxPosition, _bForceOverwriteBlocks);
    bool _bSpawnEnemies = _world.IsEditor() || GameStats.GetBool(EnumGameStats.IsSpawnEnemies);
    this.prefab.CopyEntitiesIntoChunkStub(_chunk, this.boundingBoxPosition, (ICollection<int>) this.entityInstanceIds, _bSpawnEnemies);
    this.lastCopiedPrefabPosition = this.boundingBoxPosition;
    this.bPrefabCopiedIntoWorld = true;
  }

  public HashSetLong GetOccupiedChunks()
  {
    if (this.occupiedChunks != null)
      return this.occupiedChunks;
    this.occupiedChunks = new HashSetLong();
    int chunkXz1 = World.toChunkXZ(this.boundingBoxPosition.x);
    int chunkXz2 = World.toChunkXZ(this.boundingBoxPosition.x + this.boundingBoxSize.x);
    int chunkXz3 = World.toChunkXZ(this.boundingBoxPosition.z);
    int chunkXz4 = World.toChunkXZ(this.boundingBoxPosition.z + this.boundingBoxSize.z);
    for (int x = chunkXz1; x <= chunkXz2; ++x)
    {
      for (int y = chunkXz3; y <= chunkXz4; ++y)
        this.occupiedChunks.Add(WorldChunkCache.MakeChunkKey(x, y));
    }
    return this.occupiedChunks;
  }

  public IEnumerator ResetTerrain(World _world)
  {
    HashSetLong chunks = this.GetOccupiedChunks();
    yield return (object) GameManager.Instance.ResetWindowsAndLocksByChunks(chunks);
    ChunkCluster chunkCluster = _world.ChunkClusters[0];
    foreach (long _key in chunks)
    {
      Chunk chunkSync = chunkCluster.GetChunkSync(_key);
      if (chunkSync != null)
      {
        chunkSync.ResetWaterDebugHandle();
        chunkSync.ResetWaterSimHandle();
      }
    }
    _world.RebuildTerrain(chunks, this.boundingBoxPosition, this.boundingBoxSize, true, false, false, true);
  }

  public void ResetBlocksAndRebuild(World _world, FastTags<TagGroup.Global> questTags)
  {
    this.LastRefreshType = questTags;
    ChunkCluster chunkCluster = _world.ChunkClusters[0];
    chunkCluster.ChunkPosNeedsRegeneration_DelayedStart();
    HashSetLong occupiedChunks = this.GetOccupiedChunks();
    foreach (long _key in occupiedChunks)
    {
      Chunk chunkSync = chunkCluster.GetChunkSync(_key);
      if (chunkSync != null)
      {
        chunkSync.StopStabilityCalculation = true;
        chunkSync.ResetWaterDebugHandle();
        chunkSync.ResetWaterSimHandle();
      }
    }
    this.CopyIntoWorld(_world, false, true, questTags);
    foreach (long _key in occupiedChunks)
    {
      Chunk chunkSync = chunkCluster.GetChunkSync(_key);
      if (chunkSync != null)
      {
        chunkSync.NeedsDecoration = true;
        chunkSync.NeedsLightDecoration = true;
        chunkSync.NeedsLightCalculation = true;
      }
    }
    List<TileEntity> tileEntityList = new List<TileEntity>(10);
    foreach (long _key in occupiedChunks)
    {
      Chunk chunkSync = chunkCluster.GetChunkSync(_key);
      if (chunkSync != null)
      {
        tileEntityList.Clear();
        List<TileEntity> list = chunkSync.GetTileEntities().list;
        for (int index = list.Count - 1; index >= 0; --index)
        {
          if (!chunkSync.GetBlock(list[index].localChunkPos).Block.HasTileEntity)
          {
            tileEntityList.Add(list[index]);
          }
          else
          {
            Vector3i worldPos = list[index].ToWorldPos();
            if (this.boundingBoxPosition.x <= worldPos.x && this.boundingBoxPosition.y <= worldPos.y && this.boundingBoxPosition.z <= worldPos.z && this.boundingBoxPosition.x + this.boundingBoxSize.x > worldPos.x && this.boundingBoxPosition.y + this.boundingBoxSize.y > worldPos.y && this.boundingBoxPosition.z + this.boundingBoxSize.z > worldPos.z)
              list[index].Reset(questTags);
          }
        }
        foreach (TileEntity _te in tileEntityList)
          chunkSync.RemoveTileEntity(_world, _te);
      }
    }
    chunkCluster.ChunkPosNeedsRegeneration_DelayedStop();
    _world.m_ChunkManager.ResendChunksToClients(occupiedChunks);
  }

  public GameUtils.EPlayerHomeType CheckForAnyPlayerHome(World _world)
  {
    return GameUtils.CheckForAnyPlayerHome(_world, this.boundingBoxPosition, this.boundingBoxPosition + this.boundingBoxSize);
  }

  public bool AddChunksToUncull(World _world, HashSetList<Chunk> _chunksToUncull)
  {
    bool uncull = false;
    foreach (long occupiedChunk in this.GetOccupiedChunks())
    {
      Chunk chunkSync = _world.ChunkCache.GetChunkSync(occupiedChunk);
      if (chunkSync != null && chunkSync.IsInternalBlocksCulled && !_chunksToUncull.hashSet.Contains(chunkSync))
      {
        _chunksToUncull.Add(chunkSync);
        uncull = true;
      }
    }
    return uncull;
  }

  public PathAbstractions.AbstractedLocation GetImposterLocation()
  {
    if (this.imposterLookupDone)
      return this.imposterLocation;
    string _name = this.prefab?.distantPOIOverride ?? this.location.Name;
    this.imposterLocation = PathAbstractions.PrefabImpostersSearchPaths.GetLocation(_name);
    this.imposterLookupDone = true;
    return this.imposterLocation;
  }

  public bool Contains(int _entityId) => this.entityInstanceIds.Contains(_entityId);

  public override bool Equals(object obj)
  {
    return obj is PrefabInstance && this.boundingBoxPosition == ((PrefabInstance) obj).boundingBoxPosition;
  }

  public override int GetHashCode() => this.boundingBoxPosition.GetHashCode();

  public override string ToString()
  {
    return $"[DynamicPrefabDecorator {this.id.ToString()} {(this.prefab != null ? this.prefab.PrefabName : string.Empty)}]";
  }

  public void UpdateImposterView()
  {
    if (!GameManager.Instance.IsEditMode() || PrefabEditModeManager.Instance.IsActive())
      return;
    SelectionBox box = this.GetBox();
    if (Object.op_Equality((Object) box, (Object) null))
    {
      Log.Error("PrefabInstance has not SelectionBox! (UIV)");
    }
    else
    {
      Transform transform = ((Component) box).transform.Find("PrefabImposter");
      if (Object.op_Equality((Object) transform, (Object) null))
      {
        ThreadManager.RunCoroutineSync(this.prefab.ToTransform(true, true, true, false, ((Component) box).transform, "PrefabImposter", new Vector3((float) -this.boundingBoxSize.x / 2f, 0.15f, (float) -this.boundingBoxSize.z / 2f), DynamicPrefabDecorator.PrefabPreviewLimit));
        transform = ((Component) box).transform.Find("PrefabImposter");
        this.imposterBaseRotation = this.lastCopiedRotation;
      }
      int num = MathUtils.Mod((int) this.rotation - (int) this.imposterBaseRotation, 4);
      Vector3 localEulerAngles = transform.localEulerAngles;
      localEulerAngles.y = -90f * (float) num;
      transform.localEulerAngles = localEulerAngles;
      Vector3 localPosition = transform.localPosition;
      localPosition.x = (float) ((double) this.boundingBoxSize.x / 2.0 * (num % 3 == 0 ? -1.0 : 1.0));
      localPosition.z = (float) ((double) this.boundingBoxSize.z / 2.0 * (num < 2 ? -1.0 : 1.0));
      transform.localPosition = localPosition;
      ((Component) transform).gameObject.SetActive(!this.IsBBInSyncWithPrefab());
    }
  }

  public void DestroyImposterView()
  {
    SelectionBox box = this.GetBox();
    if (Object.op_Equality((Object) box, (Object) null))
    {
      Log.Error("PrefabInstance has not SelectionBox! (DIV)");
    }
    else
    {
      Transform transform = ((Component) box).transform.Find("PrefabImposter");
      if (!Object.op_Inequality((Object) transform, (Object) null))
        return;
      Object.DestroyImmediate((Object) ((Component) transform).gameObject);
    }
  }

  public Vector3i GetPositionRelativeToPoi(Vector3i _pos)
  {
    Vector3i vector3i = _pos - this.boundingBoxPosition;
    if (((int) this.rotation & 1) != 0)
      MathUtils.Swap(ref vector3i.x, ref vector3i.z);
    Vector3i positionRelativeToPoi;
    switch ((int) this.rotation & 3)
    {
      case 0:
        positionRelativeToPoi = vector3i;
        break;
      case 1:
        positionRelativeToPoi = new Vector3i(vector3i.x, vector3i.y, this.boundingBoxSize.z - 1 - vector3i.z);
        break;
      case 2:
        positionRelativeToPoi = new Vector3i(this.boundingBoxSize.x - 1 - vector3i.x, vector3i.y, this.boundingBoxSize.z - 1 - vector3i.z);
        break;
      case 3:
        positionRelativeToPoi = new Vector3i(this.boundingBoxSize.x - 1 - vector3i.x, vector3i.y, vector3i.z);
        break;
      default:
        positionRelativeToPoi = vector3i;
        break;
    }
    return positionRelativeToPoi;
  }

  public Vector3i GetWorldPositionOfPoiOffset(Vector3i _offset)
  {
    Vector3i boundingBoxSize = this.boundingBoxSize;
    Vector3i vector3i;
    switch ((int) this.rotation & 3)
    {
      case 1:
        vector3i = new Vector3i(_offset.x, _offset.y, boundingBoxSize.z - 1 - _offset.z);
        break;
      case 2:
        vector3i = new Vector3i(boundingBoxSize.x - 1 - _offset.x, _offset.y, boundingBoxSize.z - 1 - _offset.z);
        break;
      case 3:
        vector3i = new Vector3i(boundingBoxSize.x - 1 - _offset.x, _offset.y, _offset.z);
        break;
      default:
        vector3i = _offset;
        break;
    }
    _offset = vector3i;
    if (((int) this.rotation & 1) != 0)
      MathUtils.Swap(ref _offset.x, ref _offset.z);
    _offset += this.boundingBoxPosition;
    return _offset;
  }
}
