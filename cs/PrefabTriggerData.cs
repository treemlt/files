// Decompiled with JetBrains decompiler
// Type: PrefabTriggerData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
[Preserve]
public class PrefabTriggerData
{
  public Dictionary<int, List<BlockTrigger>> TriggeredByDictionary = new Dictionary<int, List<BlockTrigger>>();
  public Dictionary<int, List<SleeperVolume>> TriggeredByVolumes = new Dictionary<int, List<SleeperVolume>>();
  public PrefabInstance PrefabInstance;
  [PublicizedFrom(EAccessModifier.Protected)]
  public World world;
  public List<int> PlayersInArea = new List<int>();
  public List<byte> TriggeredLayers;
  public List<byte> TriggeredByLayers;
  public List<BlockTrigger> Triggers = new List<BlockTrigger>();
  public TriggerManager Owner;
  [PublicizedFrom(EAccessModifier.Private)]
  public float needsTriggerTimer = -1f;

  public bool NeedsTriggerUpdate
  {
    get => (double) this.needsTriggerTimer != -1.0;
    set
    {
      if (this.Owner == null)
        this.Owner = this.world.triggerManager;
      if (value)
      {
        this.Owner.AddToUpdateList(this);
        this.needsTriggerTimer = 3f;
      }
      else
      {
        this.Owner.RemoveFromUpdateList(this);
        this.needsTriggerTimer = -1f;
      }
    }
  }

  public PrefabTriggerData(PrefabInstance instance)
  {
    this.PrefabInstance = instance;
    this.world = GameManager.Instance.World;
    this.SetupData();
  }

  public void ResetData()
  {
    if (this.TriggeredLayers != null)
      this.TriggeredLayers.Clear();
    if (this.TriggeredByLayers != null)
      this.TriggeredByLayers.Clear();
    this.TriggeredByDictionary.Clear();
    this.TriggeredByVolumes.Clear();
    this.Triggers.Clear();
    this.SetupData();
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public void SetupData()
  {
    bool flag = GameManager.Instance.World.IsEditor();
    HashSetLong occupiedChunks = this.PrefabInstance.GetOccupiedChunks();
    Vector3i boundingBoxSize = this.PrefabInstance.boundingBoxSize;
    Vector3i boundingBoxPosition = this.PrefabInstance.boundingBoxPosition;
    foreach (long _key in occupiedChunks)
    {
      Chunk chunkSync = this.world.ChunkCache.GetChunkSync(_key);
      if (chunkSync != null)
      {
        foreach (BlockTrigger blockTrigger in chunkSync.GetBlockTriggers().list)
        {
          Vector3i worldPos = blockTrigger.ToWorldPos();
          if (boundingBoxPosition.x <= worldPos.x && boundingBoxPosition.y <= worldPos.y && boundingBoxPosition.z <= worldPos.z && boundingBoxPosition.x + boundingBoxSize.x > worldPos.x && boundingBoxPosition.y + boundingBoxSize.y > worldPos.y && boundingBoxPosition.z + boundingBoxSize.z > worldPos.z)
          {
            foreach (int triggeredByIndex in blockTrigger.TriggeredByIndices)
            {
              List<BlockTrigger> blockTriggerList;
              if (!this.TriggeredByDictionary.TryGetValue(triggeredByIndex, out blockTriggerList))
              {
                blockTriggerList = new List<BlockTrigger>();
                this.TriggeredByDictionary[triggeredByIndex] = blockTriggerList;
              }
              blockTriggerList.Add(blockTrigger);
              if (flag)
              {
                if (this.TriggeredByLayers == null)
                  this.TriggeredByLayers = new List<byte>();
                if (!this.TriggeredByLayers.Contains((byte) triggeredByIndex))
                  this.TriggeredByLayers.Add((byte) triggeredByIndex);
              }
            }
            foreach (int triggersIndex in blockTrigger.TriggersIndices)
            {
              if (this.TriggeredLayers == null)
                this.TriggeredLayers = new List<byte>();
              if (!this.TriggeredLayers.Contains((byte) triggersIndex))
                this.TriggeredLayers.Add((byte) triggersIndex);
            }
            this.Triggers.Add(blockTrigger);
            blockTrigger.TriggerDataOwner = this;
          }
        }
      }
    }
    List<SleeperVolume> sleeperVolumes = this.PrefabInstance.sleeperVolumes;
    for (int index1 = 0; index1 < sleeperVolumes.Count; ++index1)
    {
      SleeperVolume triggeredVolume = sleeperVolumes[index1];
      for (int index2 = 0; index2 < triggeredVolume.TriggeredByIndices.Count; ++index2)
      {
        this.AddTriggeredBy(triggeredVolume);
        if (flag)
        {
          if (this.TriggeredByLayers == null)
            this.TriggeredByLayers = new List<byte>();
          byte triggeredByIndex = triggeredVolume.TriggeredByIndices[index2];
          if (!this.TriggeredByLayers.Contains(triggeredByIndex))
            this.TriggeredByLayers.Add(triggeredByIndex);
        }
      }
    }
    this.RefreshTriggers();
  }

  public void Update(float deltaTime)
  {
    if ((double) this.needsTriggerTimer == -1.0)
      return;
    this.needsTriggerTimer -= deltaTime;
    if ((double) this.needsTriggerTimer > 0.0)
      return;
    this.HandleNeedTriggers();
    this.NeedsTriggerUpdate = false;
  }

  public void HandleNeedTriggers()
  {
    for (int index = 0; index < this.Triggers.Count; ++index)
    {
      if (this.Triggers[index].NeedsTriggered == BlockTrigger.TriggeredStates.NeedsTriggered)
      {
        this.Trigger((EntityPlayer) null, this.Triggers[index]);
        this.Triggers[index].NeedsTriggered = BlockTrigger.TriggeredStates.HasTriggered;
      }
    }
  }

  public void RefreshTriggers()
  {
    if (GameManager.Instance.IsEditMode())
      return;
    for (int index = 0; index < this.Triggers.Count; ++index)
      this.Triggers[index].Refresh(FastTags<TagGroup.Global>.none);
  }

  public void RefreshTriggersForQuest(FastTags<TagGroup.Global> questTags)
  {
    if (GameManager.Instance.IsEditMode())
      return;
    for (int index = 0; index < this.Triggers.Count; ++index)
      this.Triggers[index].Refresh(questTags);
  }

  public void ResetTriggers()
  {
    if (GameManager.Instance.IsEditMode())
      return;
    for (int index = 0; index < this.Triggers.Count; ++index)
      this.Triggers[index].NeedsTriggered = BlockTrigger.TriggeredStates.NotTriggered;
  }

  public void AddPlayerInArea(int entityID)
  {
    if (this.PlayersInArea.Contains(entityID))
      return;
    this.PlayersInArea.Add(entityID);
  }

  public void RemovePlayerInArea(int entityID)
  {
    if (!this.PlayersInArea.Contains(entityID))
      return;
    this.PlayersInArea.Remove(entityID);
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public void AddTriggeredBy(SleeperVolume triggeredVolume)
  {
    for (int index = 0; index < triggeredVolume.TriggeredByIndices.Count; ++index)
    {
      byte triggeredByIndex = triggeredVolume.TriggeredByIndices[index];
      if (!this.TriggeredByVolumes.ContainsKey((int) triggeredByIndex))
        this.TriggeredByVolumes.Add((int) triggeredByIndex, new List<SleeperVolume>());
      this.TriggeredByVolumes[(int) triggeredByIndex].Add(triggeredVolume);
    }
  }

  public void Trigger(EntityPlayer player, byte index)
  {
    List<BlockChangeInfo> blockChangeInfoList = new List<BlockChangeInfo>();
    World world = GameManager.Instance.World;
    List<BlockTrigger> blockTriggerList;
    if (this.TriggeredByDictionary.TryGetValue((int) index, out blockTriggerList))
    {
      for (int index1 = 0; index1 < blockTriggerList.Count; ++index1)
        blockTriggerList[index1].OnTriggered(player, world, (int) index, blockChangeInfoList);
    }
    List<SleeperVolume> sleeperVolumeList;
    if (this.TriggeredByVolumes.TryGetValue((int) index, out sleeperVolumeList))
    {
      foreach (SleeperVolume sleeperVolume in sleeperVolumeList)
        sleeperVolume.OnTriggered(player, world, (int) index);
    }
    if (blockChangeInfoList.Count <= 0)
      return;
    this.UpdateBlocks(blockChangeInfoList);
  }

  public void Trigger(EntityPlayer player, BlockTrigger trigger)
  {
    List<BlockChangeInfo> blockChangeInfoList = new List<BlockChangeInfo>();
    World world = GameManager.Instance.World;
    foreach (int triggersIndex in trigger.TriggersIndices)
    {
      List<BlockTrigger> blockTriggerList;
      if (this.TriggeredByDictionary.TryGetValue(triggersIndex, out blockTriggerList))
      {
        foreach (BlockTrigger blockTrigger in blockTriggerList)
          blockTrigger.OnTriggered(player, world, triggersIndex, blockChangeInfoList, trigger);
      }
      if (Object.op_Inequality((Object) player, (Object) null) && this.TriggeredByVolumes.ContainsKey(triggersIndex))
      {
        foreach (SleeperVolume sleeperVolume in this.TriggeredByVolumes[triggersIndex])
          sleeperVolume.OnTriggered(player, world, triggersIndex);
      }
    }
    if (blockChangeInfoList.Count <= 0)
      return;
    this.UpdateBlocks(blockChangeInfoList);
  }

  public void Trigger(EntityPlayer player, TriggerVolume trigger)
  {
    List<BlockChangeInfo> blockChangeInfoList = new List<BlockChangeInfo>();
    World world = GameManager.Instance.World;
    for (int index1 = 0; index1 < trigger.TriggersIndices.Count; ++index1)
    {
      int triggersIndex = (int) trigger.TriggersIndices[index1];
      if (this.TriggeredByDictionary.ContainsKey(triggersIndex))
      {
        for (int index2 = 0; index2 < this.TriggeredByDictionary[triggersIndex].Count; ++index2)
          this.TriggeredByDictionary[triggersIndex][index2].OnTriggered(player, world, triggersIndex, blockChangeInfoList);
      }
      if (this.TriggeredByVolumes.ContainsKey(triggersIndex))
      {
        for (int index3 = 0; index3 < this.TriggeredByVolumes[triggersIndex].Count; ++index3)
          this.TriggeredByVolumes[triggersIndex][index3].OnTriggered(player, world, triggersIndex);
      }
    }
    if (blockChangeInfoList.Count <= 0)
      return;
    this.UpdateBlocks(blockChangeInfoList);
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public void UpdateBlocks(List<BlockChangeInfo> blockChanges)
  {
    if (GameManager.Instance.World == null || blockChanges == null)
      return;
    GameManager.Instance.World.SetBlocksRPC(blockChanges);
  }

  public void SetupTriggerTestNavObjects()
  {
    this.RemoveTriggerTestNavObjects();
    for (int index = 0; index < this.Triggers.Count; ++index)
    {
      NavObject navObject = NavObjectManager.Instance.RegisterNavObject("editor_block_trigger", this.Triggers[index].ToWorldPos().ToVector3Center());
      navObject.name = this.Triggers[index].TriggerDisplay();
      navObject.OverrideColor = this.Triggers[index].TriggeredByIndices.Count > 0 ? Color.blue : Color.red;
    }
  }

  public void RemoveTriggerTestNavObjects()
  {
    for (int index = 0; index < this.Triggers.Count; ++index)
      NavObjectManager.Instance.UnRegisterNavObjectByPosition(this.Triggers[index].ToWorldPos().ToVector3Center(), "editor_block_trigger");
  }
}
