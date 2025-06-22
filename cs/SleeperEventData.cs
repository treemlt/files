// Decompiled with JetBrains decompiler
// Type: SleeperEventData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SleeperEventData
{
  public List<SleeperVolume> SleeperVolumes = new List<SleeperVolume>();
  public List<int> EntityList = new List<int>();
  public bool hasRefreshed;
  public int ShowQuestClearCount = 1;
  public Vector3 position;

  public void SetupData(Vector3 _position)
  {
    this.position = _position;
    PrefabInstance prefabFromWorldPos = GameManager.Instance.GetDynamicPrefabDecorator().GetPrefabFromWorldPos((int) this.position.x, (int) this.position.z);
    this.ShowQuestClearCount = prefabFromWorldPos.prefab.ShowQuestClearCount;
    if (prefabFromWorldPos == null)
      return;
    for (int index = 0; index < prefabFromWorldPos.prefab.SleeperVolumes.Count; ++index)
    {
      Vector3i startPos = prefabFromWorldPos.prefab.SleeperVolumes[index].startPos;
      Vector3i size = prefabFromWorldPos.prefab.SleeperVolumes[index].size;
      int sleeperVolume1 = GameManager.Instance.World.FindSleeperVolume(prefabFromWorldPos.boundingBoxPosition + startPos, prefabFromWorldPos.boundingBoxPosition + startPos + size);
      if (sleeperVolume1 != -1)
      {
        SleeperVolume sleeperVolume2 = GameManager.Instance.World.GetSleeperVolume(sleeperVolume1);
        if (!sleeperVolume2.isQuestExclude && !this.SleeperVolumes.Contains(sleeperVolume2))
          this.SleeperVolumes.Add(sleeperVolume2);
      }
    }
  }

  public bool Update()
  {
    if (!this.hasRefreshed)
    {
      World world = GameManager.Instance.World;
      for (int index = 0; index < this.SleeperVolumes.Count; ++index)
        this.SleeperVolumes[index].DespawnAndReset(world);
      this.hasRefreshed = true;
    }
    if (this.SleeperVolumes.Count <= 0)
      return false;
    bool flag1 = false;
    bool flag2 = false;
    for (int index1 = this.SleeperVolumes.Count - 1; index1 >= 0; --index1)
    {
      SleeperVolume sleeperVolume = this.SleeperVolumes[index1];
      if (sleeperVolume.wasCleared)
      {
        for (int index2 = 0; index2 < this.EntityList.Count; ++index2)
        {
          if (GameManager.Instance.World.GetEntity(this.EntityList[index2]) as EntityPlayer is EntityPlayerLocal)
            QuestEventManager.Current.SleeperVolumePositionRemoved(sleeperVolume.Center);
          else
            SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageQuestEvent>().Setup(NetPackageQuestEvent.QuestEventTypes.HideSleeperVolume, this.EntityList[index2], sleeperVolume.Center));
        }
        this.SleeperVolumes.RemoveAt(index1);
        flag1 = true;
      }
      else
        flag2 = true;
    }
    if (flag1)
    {
      if (this.SleeperVolumes.Count <= this.ShowQuestClearCount)
      {
        for (int index3 = 0; index3 < this.EntityList.Count; ++index3)
        {
          EntityPlayer entity = GameManager.Instance.World.GetEntity(this.EntityList[index3]) as EntityPlayer;
          for (int index4 = 0; index4 < this.SleeperVolumes.Count; ++index4)
          {
            if (entity is EntityPlayerLocal)
              QuestEventManager.Current.SleeperVolumePositionAdded(this.SleeperVolumes[index4].Center);
            else
              SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageQuestEvent>().Setup(NetPackageQuestEvent.QuestEventTypes.ShowSleeperVolume, this.EntityList[index3], this.SleeperVolumes[index4].Center), _attachedToEntityId: this.EntityList[index3]);
          }
        }
      }
    }
    if (flag2)
      return false;
    bool flag3 = false;
    for (int index = 0; index < this.EntityList.Count; ++index)
    {
      if (GameManager.Instance.World.GetEntity(this.EntityList[index]) as EntityPlayer is EntityPlayerLocal)
        flag3 = true;
      else
        SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageQuestEvent>().Setup(NetPackageQuestEvent.QuestEventTypes.ClearSleeper, this.EntityList[index], this.position));
    }
    if (flag3)
      QuestEventManager.Current.ClearedSleepers(this.position);
    return true;
  }
}
