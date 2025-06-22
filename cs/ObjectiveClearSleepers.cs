// Decompiled with JetBrains decompiler
// Type: ObjectiveClearSleepers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
[Preserve]
public class ObjectiveClearSleepers : BaseObjective
{
  [PublicizedFrom(EAccessModifier.Private)]
  public Vector3 position;
  [PublicizedFrom(EAccessModifier.Private)]
  public float distanceOffset;
  [PublicizedFrom(EAccessModifier.Private)]
  public string icon = "ui_game_symbol_quest";
  [PublicizedFrom(EAccessModifier.Private)]
  public string locationVariable = "gotolocation";
  public Dictionary<Vector3, MapObjectSleeperVolume> SleeperMapObjectList = new Dictionary<Vector3, MapObjectSleeperVolume>();
  public Dictionary<Vector3, NavObject> SleeperNavObjectList = new Dictionary<Vector3, NavObject>();

  public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
  {
    get => BaseObjective.ObjectiveValueTypes.Boolean;
  }

  public override bool RequiresZombies => true;

  public override void SetupQuestTag() => this.OwnerQuest.AddQuestTag(QuestEventManager.clearTag);

  public override void SetupObjective()
  {
    this.keyword = Localization.Get("ObjectiveClearAreas_keyword");
    this.SetupIcon();
  }

  public override bool UpdateUI => this.ObjectiveState != BaseObjective.ObjectiveStates.Failed;

  public override void SetupDisplay()
  {
    this.Description = this.keyword;
    this.StatusText = "";
  }

  public override void AddHooks()
  {
    this.GetPosition();
    Vector3 pos1 = Vector3.zero;
    Vector3 pos2 = Vector3.zero;
    this.OwnerQuest.GetPositionData(out pos1, Quest.PositionDataTypes.POIPosition);
    this.OwnerQuest.GetPositionData(out pos2, Quest.PositionDataTypes.POISize);
    QuestEventManager.Current.SleepersCleared += new QuestEvent_SleepersCleared(this.Current_SleepersCleared);
    QuestEventManager.Current.SleeperVolumePositionAdd += new QuestEvent_SleeperVolumePositionChanged(this.Current_SleeperVolumePositionAdd);
    QuestEventManager.Current.SleeperVolumePositionRemove += new QuestEvent_SleeperVolumePositionChanged(this.Current_SleeperVolumePositionRemove);
    QuestEventManager.Current.SubscribeToUpdateEvent(this.OwnerQuest.OwnerJournal.OwnerPlayer.entityId, pos1);
    this.SetupZombieCompassBounds(pos1, pos2);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void Current_SleeperVolumePositionAdd(Vector3 position)
  {
    if (this.NavObjectName == "")
    {
      if (this.SleeperMapObjectList.ContainsKey(position))
        return;
      MapObjectSleeperVolume _mo = new MapObjectSleeperVolume(position);
      GameManager.Instance.World.ObjectOnMapAdd((MapObject) _mo);
      this.SleeperMapObjectList.Add(position, _mo);
    }
    else
    {
      if (this.SleeperNavObjectList.ContainsKey(position))
        return;
      NavObject navObject = NavObjectManager.Instance.RegisterNavObject(this.NavObjectName, position);
      this.SleeperNavObjectList.Add(position, navObject);
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void Current_SleeperVolumePositionRemove(Vector3 position)
  {
    if (this.NavObjectName == "")
    {
      if (!this.SleeperMapObjectList.ContainsKey(position))
        return;
      MapObject sleeperMapObject = (MapObject) this.SleeperMapObjectList[position];
      GameManager.Instance.World.ObjectOnMapRemove(sleeperMapObject.type, (int) sleeperMapObject.key);
      this.SleeperMapObjectList.Remove(position);
    }
    else
    {
      if (!this.SleeperNavObjectList.ContainsKey(position))
        return;
      NavObjectManager.Instance.UnRegisterNavObject(this.SleeperNavObjectList[position]);
      this.SleeperNavObjectList.Remove(position);
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void RemoveSleeperVolumeMapObjects()
  {
    if (this.NavObjectName == "")
    {
      GameManager.Instance.World.ObjectOnMapRemove(EnumMapObjectType.SleeperVolume);
      this.SleeperMapObjectList.Clear();
    }
    else
    {
      NavObjectManager.Instance.UnRegisterNavObjectByClass(this.NavObjectName);
      this.SleeperNavObjectList.Clear();
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void SetupZombieCompassBounds(Vector3 poiPos, Vector3 poiSize)
  {
    this.OwnerQuest.OwnerJournal.OwnerPlayer.ZombieCompassBounds = new Rect(poiPos.x, poiPos.z, poiSize.x, poiSize.z);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void Current_SleepersCleared(Vector3 prefabPos)
  {
    Vector3 pos = Vector3.zero;
    this.OwnerQuest.GetPositionData(out pos, Quest.PositionDataTypes.POIPosition);
    if ((double) pos.x != (double) prefabPos.x || (double) pos.z != (double) prefabPos.z || !this.OwnerQuest.CheckRequirements())
      return;
    this.Complete = true;
    this.OwnerQuest.RefreshQuestCompletion();
  }

  public override void RemoveHooks()
  {
    QuestEventManager.Current.SleepersCleared -= new QuestEvent_SleepersCleared(this.Current_SleepersCleared);
    Vector3 pos = Vector3.zero;
    this.OwnerQuest.GetPositionData(out pos, Quest.PositionDataTypes.POIPosition);
    QuestEventManager.Current.UnSubscribeToUpdateEvent(this.OwnerQuest.OwnerJournal.OwnerPlayer.entityId, pos);
    if (this.OwnerQuest.OwnerJournal.ActiveQuest == this.OwnerQuest)
      this.OwnerQuest.OwnerJournal.OwnerPlayer.ZombieCompassBounds = new Rect();
    this.RemoveSleeperVolumeMapObjects();
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void SetupIcon()
  {
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public Vector3 GetPosition()
  {
    if (this.OwnerQuest.GetPositionData(out this.position, Quest.PositionDataTypes.POIPosition))
      this.OwnerQuest.Position = this.position;
    return Vector3.zero;
  }

  public void FinalizePoint(float offset, float x, float y, float z)
  {
    this.distanceOffset = offset;
    this.position = new Vector3(x, y, z);
    this.OwnerQuest.DataVariables.Add(this.locationVariable, $"{offset.ToCultureInvariantString()},{x.ToCultureInvariantString()},{y.ToCultureInvariantString()},{z.ToCultureInvariantString()}");
    this.OwnerQuest.Position = this.position;
    this.CurrentValue = (byte) 1;
  }

  public override void Refresh()
  {
    if (!this.Complete)
      return;
    this.OwnerQuest.RefreshQuestCompletion();
  }

  public override BaseObjective Clone()
  {
    ObjectiveClearSleepers objective = new ObjectiveClearSleepers();
    this.CopyValues((BaseObjective) objective);
    return (BaseObjective) objective;
  }

  public override bool SetLocation(Vector3 pos, Vector3 size)
  {
    this.FinalizePoint(this.distanceOffset, pos.x, pos.y, pos.z);
    return true;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public enum GotoStates
  {
    NoPosition,
    TryRefresh,
    TryComplete,
    Completed,
  }
}
