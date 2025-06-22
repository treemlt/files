// Decompiled with JetBrains decompiler
// Type: NetPackageQuestEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
[Preserve]
public class NetPackageQuestEvent : NetPackage
{
  public int entityID;
  public Vector3 prefabPos;
  public FastTags<TagGroup.Global> questTags;
  public NetPackageQuestEvent.QuestEventTypes eventType;
  public ObjectiveFetchFromContainer.FetchModeTypes FetchModeType;
  public bool SubscribeTo;
  public int PartyCount;
  public int questCode;
  public int factionPointOverride;
  public string blockIndex = "";
  public string eventName = "";
  public string questID;
  [PublicizedFrom(EAccessModifier.Private)]
  public ulong extraData;
  public List<Vector3i> activateList;
  public int[] SharedWithList;

  public NetPackageQuestEvent Setup(NetPackageQuestEvent.QuestEventTypes _eventType, int _entityID)
  {
    this.eventType = _eventType;
    this.entityID = _entityID;
    return this;
  }

  public NetPackageQuestEvent Setup(
    NetPackageQuestEvent.QuestEventTypes _eventType,
    int _entityID,
    int _traderID,
    int _overrideFactionPoints)
  {
    this.eventType = _eventType;
    this.entityID = _entityID;
    this.questCode = _traderID;
    this.factionPointOverride = _overrideFactionPoints;
    return this;
  }

  public NetPackageQuestEvent Setup(
    NetPackageQuestEvent.QuestEventTypes _eventType,
    int _entityID,
    Vector3 _prefabPos,
    int _questCode)
  {
    this.entityID = _entityID;
    this.prefabPos = _prefabPos;
    this.eventType = _eventType;
    this.questCode = _questCode;
    return this;
  }

  public NetPackageQuestEvent Setup(
    NetPackageQuestEvent.QuestEventTypes _eventType,
    int _entityID,
    Vector3 _prefabPos,
    int _questCode,
    ulong _extraData)
  {
    this.entityID = _entityID;
    this.prefabPos = _prefabPos;
    this.eventType = _eventType;
    this.questCode = _questCode;
    this.extraData = _extraData;
    return this;
  }

  public NetPackageQuestEvent Setup(
    NetPackageQuestEvent.QuestEventTypes _eventType,
    int _entityID,
    Vector3 _prefabPos)
  {
    this.entityID = _entityID;
    this.prefabPos = _prefabPos;
    this.eventType = _eventType;
    return this;
  }

  public NetPackageQuestEvent Setup(
    NetPackageQuestEvent.QuestEventTypes _eventType,
    int _entityID,
    string _questID,
    FastTags<TagGroup.Global> _questTags,
    Vector3 _prefabPos,
    int[] _sharedWithList)
  {
    this.entityID = _entityID;
    this.prefabPos = _prefabPos;
    this.eventType = _eventType;
    this.questTags = _questTags;
    this.questID = _questID;
    this.SharedWithList = _sharedWithList;
    return this;
  }

  public NetPackageQuestEvent Setup(
    NetPackageQuestEvent.QuestEventTypes _eventType,
    int _entityID,
    Vector3 _prefabPos,
    ObjectiveFetchFromContainer.FetchModeTypes _fetchModeType)
  {
    this.entityID = _entityID;
    this.prefabPos = _prefabPos;
    this.eventType = _eventType;
    this.FetchModeType = _fetchModeType;
    return this;
  }

  public NetPackageQuestEvent Setup(
    NetPackageQuestEvent.QuestEventTypes _eventType,
    int _entityID,
    Vector3 _prefabPos,
    ObjectiveFetchFromContainer.FetchModeTypes _fetchModeType,
    int[] _sharedWithList)
  {
    this.entityID = _entityID;
    this.prefabPos = _prefabPos;
    this.eventType = _eventType;
    this.FetchModeType = _fetchModeType;
    this.SharedWithList = _sharedWithList;
    return this;
  }

  public NetPackageQuestEvent Setup(
    NetPackageQuestEvent.QuestEventTypes _eventType,
    int _entityID,
    int _questCode,
    string _completeEvent,
    Vector3 _prefabPos,
    string _blockIndex,
    int[] _sharedWithList)
  {
    this.entityID = _entityID;
    this.questCode = _questCode;
    this.eventName = _completeEvent;
    this.prefabPos = _prefabPos;
    this.eventType = _eventType;
    this.blockIndex = _blockIndex;
    this.SharedWithList = _sharedWithList;
    return this;
  }

  public NetPackageQuestEvent Setup(
    NetPackageQuestEvent.QuestEventTypes _eventType,
    int _entityID,
    Vector3 _prefabPos,
    bool _subscribeTo)
  {
    this.entityID = _entityID;
    this.prefabPos = _prefabPos;
    this.eventType = _eventType;
    this.SubscribeTo = _subscribeTo;
    return this;
  }

  public NetPackageQuestEvent Setup(
    NetPackageQuestEvent.QuestEventTypes _eventType,
    int _entityID,
    int _questCode,
    string _completeEvent,
    Vector3 _prefabPos,
    List<Vector3i> _activateList)
  {
    this.entityID = _entityID;
    this.questCode = _questCode;
    this.eventName = _completeEvent;
    this.prefabPos = _prefabPos;
    this.eventType = _eventType;
    this.activateList = _activateList;
    return this;
  }

  public NetPackageQuestEvent Setup(
    NetPackageQuestEvent.QuestEventTypes _eventType,
    int _entityID,
    int _questCode,
    string _questID,
    Vector3 _prefabPos,
    int[] _sharedWithList)
  {
    this.entityID = _entityID;
    this.questCode = _questCode;
    this.prefabPos = _prefabPos;
    this.eventType = _eventType;
    this.questID = _questID;
    this.SharedWithList = _sharedWithList;
    return this;
  }

  public override void read(PooledBinaryReader _reader)
  {
    this.entityID = _reader.ReadInt32();
    this.prefabPos = StreamUtils.ReadVector3((BinaryReader) _reader);
    this.eventType = (NetPackageQuestEvent.QuestEventTypes) _reader.ReadByte();
    this.questTags = FastTags<TagGroup.Global>.Parse(_reader.ReadString());
    this.questCode = _reader.ReadInt32();
    switch (this.eventType)
    {
      case NetPackageQuestEvent.QuestEventTypes.RallyMarkerLocked:
        this.extraData = _reader.ReadUInt64();
        break;
      case NetPackageQuestEvent.QuestEventTypes.LockPOI:
        this.questID = _reader.ReadString();
        int length1 = (int) _reader.ReadByte();
        if (length1 > 0)
        {
          this.SharedWithList = new int[length1];
          for (int index = 0; index < length1; ++index)
            this.SharedWithList[index] = _reader.ReadInt32();
          break;
        }
        this.SharedWithList = (int[]) null;
        break;
      case NetPackageQuestEvent.QuestEventTypes.ClearSleeper:
        this.SubscribeTo = _reader.ReadBoolean();
        break;
      case NetPackageQuestEvent.QuestEventTypes.SetupFetch:
        this.FetchModeType = (ObjectiveFetchFromContainer.FetchModeTypes) _reader.ReadByte();
        int length2 = (int) _reader.ReadByte();
        if (length2 > 0)
        {
          this.SharedWithList = new int[length2];
          for (int index = 0; index < length2; ++index)
            this.SharedWithList[index] = _reader.ReadInt32();
          break;
        }
        this.SharedWithList = (int[]) null;
        break;
      case NetPackageQuestEvent.QuestEventTypes.SetupRestorePower:
        this.blockIndex = _reader.ReadString();
        this.eventName = _reader.ReadString();
        int length3 = (int) _reader.ReadByte();
        if (length3 > 0)
        {
          this.SharedWithList = new int[length3];
          for (int index = 0; index < length3; ++index)
            this.SharedWithList[index] = _reader.ReadInt32();
        }
        else
          this.SharedWithList = (int[]) null;
        int num = (int) _reader.ReadByte();
        this.activateList = new List<Vector3i>();
        if (num <= 0)
          break;
        for (int index = 0; index < num; ++index)
          this.activateList.Add(StreamUtils.ReadVector3i((BinaryReader) _reader));
        break;
      case NetPackageQuestEvent.QuestEventTypes.ResetTraderQuests:
        this.factionPointOverride = _reader.ReadInt32();
        break;
    }
  }

  public override void write(PooledBinaryWriter _writer)
  {
    base.write(_writer);
    _writer.Write(this.entityID);
    StreamUtils.Write((BinaryWriter) _writer, this.prefabPos);
    _writer.Write((byte) this.eventType);
    _writer.Write(this.questTags.ToString());
    _writer.Write(this.questCode);
    switch (this.eventType)
    {
      case NetPackageQuestEvent.QuestEventTypes.RallyMarkerLocked:
        _writer.Write(this.extraData);
        break;
      case NetPackageQuestEvent.QuestEventTypes.LockPOI:
        _writer.Write(this.questID);
        if (this.SharedWithList == null)
        {
          _writer.Write((byte) 0);
          break;
        }
        _writer.Write((byte) this.SharedWithList.Length);
        for (int index = 0; index < this.SharedWithList.Length; ++index)
          _writer.Write(this.SharedWithList[index]);
        break;
      case NetPackageQuestEvent.QuestEventTypes.ClearSleeper:
        _writer.Write(this.SubscribeTo);
        break;
      case NetPackageQuestEvent.QuestEventTypes.SetupFetch:
        _writer.Write((byte) this.FetchModeType);
        if (this.SharedWithList == null)
        {
          _writer.Write((byte) 0);
          break;
        }
        _writer.Write((byte) this.SharedWithList.Length);
        for (int index = 0; index < this.SharedWithList.Length; ++index)
          _writer.Write(this.SharedWithList[index]);
        break;
      case NetPackageQuestEvent.QuestEventTypes.SetupRestorePower:
        _writer.Write(this.blockIndex);
        _writer.Write(this.eventName);
        if (this.SharedWithList == null)
        {
          _writer.Write((byte) 0);
        }
        else
        {
          _writer.Write((byte) this.SharedWithList.Length);
          for (int index = 0; index < this.SharedWithList.Length; ++index)
            _writer.Write(this.SharedWithList[index]);
        }
        if (this.activateList == null)
        {
          _writer.Write((byte) 0);
          break;
        }
        _writer.Write((byte) this.activateList.Count);
        for (int index = 0; index < this.activateList.Count; ++index)
          StreamUtils.Write((BinaryWriter) _writer, this.activateList[index]);
        break;
      case NetPackageQuestEvent.QuestEventTypes.ResetTraderQuests:
        _writer.Write(this.factionPointOverride);
        break;
    }
  }

  public override void ProcessPackage(World _world, GameManager _callbacks)
  {
    if (_world == null)
      return;
    switch (this.eventType)
    {
      case NetPackageQuestEvent.QuestEventTypes.TryRallyMarker:
        if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
          break;
        Vector2 prefabPos;
        // ISSUE: explicit constructor call
        ((Vector2) ref prefabPos).\u002Ector(this.prefabPos.x, this.prefabPos.z);
        NetPackageQuestEvent.QuestEventTypes _eventType = NetPackageQuestEvent.QuestEventTypes.RallyMarkerActivated;
        ulong extraData;
        switch (QuestEventManager.Current.CheckForPOILockouts(this.entityID, prefabPos, out extraData))
        {
          case QuestEventManager.POILockoutReasonTypes.PlayerInside:
            _eventType = NetPackageQuestEvent.QuestEventTypes.RallyMarker_PlayerLocked;
            break;
          case QuestEventManager.POILockoutReasonTypes.Bedroll:
            _eventType = NetPackageQuestEvent.QuestEventTypes.RallyMarker_BedrollLocked;
            break;
          case QuestEventManager.POILockoutReasonTypes.LandClaim:
            _eventType = NetPackageQuestEvent.QuestEventTypes.RallyMarker_LandClaimLocked;
            break;
          case QuestEventManager.POILockoutReasonTypes.QuestLock:
            _eventType = NetPackageQuestEvent.QuestEventTypes.RallyMarkerLocked;
            break;
        }
        SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageQuestEvent>().Setup(_eventType, this.entityID, this.prefabPos, this.questCode, extraData));
        break;
      case NetPackageQuestEvent.QuestEventTypes.RallyMarkerActivated:
        EntityPlayer entity1 = _world.GetEntity(this.entityID) as EntityPlayer;
        if (!Object.op_Inequality((Object) entity1, (Object) null))
          break;
        entity1.QuestJournal.HandleRallyMarkerActivation(this.questCode, this.prefabPos, true, QuestEventManager.POILockoutReasonTypes.None);
        break;
      case NetPackageQuestEvent.QuestEventTypes.RallyMarkerLocked:
        EntityPlayer entity2 = _world.GetEntity(this.entityID) as EntityPlayer;
        if (!Object.op_Inequality((Object) entity2, (Object) null))
          break;
        entity2.QuestJournal.HandleRallyMarkerActivation(this.questCode, this.prefabPos, false, QuestEventManager.POILockoutReasonTypes.QuestLock, this.extraData);
        break;
      case NetPackageQuestEvent.QuestEventTypes.RallyMarker_PlayerLocked:
        EntityPlayer entity3 = _world.GetEntity(this.entityID) as EntityPlayer;
        if (!Object.op_Inequality((Object) entity3, (Object) null))
          break;
        entity3.QuestJournal.HandleRallyMarkerActivation(this.questCode, this.prefabPos, false, QuestEventManager.POILockoutReasonTypes.PlayerInside);
        break;
      case NetPackageQuestEvent.QuestEventTypes.RallyMarker_BedrollLocked:
        EntityPlayer entity4 = _world.GetEntity(this.entityID) as EntityPlayer;
        if (!Object.op_Inequality((Object) entity4, (Object) null))
          break;
        entity4.QuestJournal.HandleRallyMarkerActivation(this.questCode, this.prefabPos, false, QuestEventManager.POILockoutReasonTypes.Bedroll);
        break;
      case NetPackageQuestEvent.QuestEventTypes.RallyMarker_LandClaimLocked:
        EntityPlayer entity5 = _world.GetEntity(this.entityID) as EntityPlayer;
        if (!Object.op_Inequality((Object) entity5, (Object) null))
          break;
        entity5.QuestJournal.HandleRallyMarkerActivation(this.questCode, this.prefabPos, false, QuestEventManager.POILockoutReasonTypes.LandClaim);
        break;
      case NetPackageQuestEvent.QuestEventTypes.LockPOI:
        GameManager.Instance.StartCoroutine(QuestEventManager.Current.QuestLockPOI(this.entityID, QuestClass.GetQuest(this.questID), this.prefabPos, this.questTags, this.SharedWithList, (Action) ([PublicizedFrom(EAccessModifier.Private)] () => SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageQuestEvent>().Setup(NetPackageQuestEvent.QuestEventTypes.POILocked, this.entityID), _attachedToEntityId: this.entityID))));
        break;
      case NetPackageQuestEvent.QuestEventTypes.UnlockPOI:
        QuestEventManager.Current.QuestUnlockPOI(this.entityID, this.prefabPos);
        break;
      case NetPackageQuestEvent.QuestEventTypes.ClearSleeper:
        if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
        {
          if (this.SubscribeTo)
          {
            QuestEventManager.Current.SubscribeToUpdateEvent(this.entityID, this.prefabPos);
            break;
          }
          QuestEventManager.Current.UnSubscribeToUpdateEvent(this.entityID, this.prefabPos);
          break;
        }
        QuestEventManager.Current.ClearedSleepers(this.prefabPos);
        break;
      case NetPackageQuestEvent.QuestEventTypes.ShowSleeperVolume:
        QuestEventManager.Current.SleeperVolumePositionAdded(this.prefabPos);
        break;
      case NetPackageQuestEvent.QuestEventTypes.HideSleeperVolume:
        QuestEventManager.Current.SleeperVolumePositionRemoved(this.prefabPos);
        break;
      case NetPackageQuestEvent.QuestEventTypes.SetupFetch:
        if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
        {
          QuestEventManager.Current.SetupFetchForMP(this.entityID, this.prefabPos, this.FetchModeType, this.SharedWithList);
          break;
        }
        EntityPlayer entity6 = _world.GetEntity(this.entityID) as EntityPlayer;
        Quest.PositionDataTypes dataType = this.FetchModeType == ObjectiveFetchFromContainer.FetchModeTypes.Standard ? Quest.PositionDataTypes.FetchContainer : Quest.PositionDataTypes.HiddenCache;
        if (!Object.op_Inequality((Object) entity6, (Object) null))
          break;
        entity6.QuestJournal.SetActivePositionData(dataType, new Vector3i(this.prefabPos));
        break;
      case NetPackageQuestEvent.QuestEventTypes.SetupRestorePower:
        if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
        {
          QuestEventManager.Current.SetupActivateForMP(this.entityID, this.questCode, this.eventName, new List<Vector3i>(), GameManager.Instance.World, this.prefabPos, this.blockIndex, this.SharedWithList);
          break;
        }
        EntityPlayer entity7 = _world.GetEntity(this.entityID) as EntityPlayer;
        if (!Object.op_Inequality((Object) entity7, (Object) null))
          break;
        entity7.QuestJournal.HandleRestorePowerReceived(this.prefabPos, this.activateList);
        break;
      case NetPackageQuestEvent.QuestEventTypes.FinishManagedQuest:
        QuestEventManager.Current.FinishManagedQuest(this.questCode, _world.GetEntity(this.entityID) as EntityPlayer);
        break;
      case NetPackageQuestEvent.QuestEventTypes.POILocked:
        if (ObjectiveRallyPoint.OutstandingRallyPoint == null)
          break;
        ObjectiveRallyPoint.OutstandingRallyPoint.RallyPointActivated();
        break;
      case NetPackageQuestEvent.QuestEventTypes.ResetTraderQuests:
        QuestEventManager.Current.AddTraderResetQuestsForPlayer(this.entityID, this.questCode);
        if (this.factionPointOverride == -1)
          break;
        EntityPlayer entity8 = _world.GetEntity(this.entityID) as EntityPlayer;
        if (!Object.op_Inequality((Object) entity8, (Object) null) || !(_world.GetEntity(this.questCode) is EntityTrader entity9))
          break;
        entity9.ClearActiveQuests(entity8.entityId);
        entity9.SetupActiveQuestsForPlayer(entity8, this.factionPointOverride);
        break;
    }
  }

  public override int GetLength() => 20;

  [CompilerGenerated]
  [PublicizedFrom(EAccessModifier.Private)]
  public void \u003CProcessPackage\u003Eb__30_0()
  {
    SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageQuestEvent>().Setup(NetPackageQuestEvent.QuestEventTypes.POILocked, this.entityID), _attachedToEntityId: this.entityID);
  }

  public enum QuestEventTypes
  {
    TryRallyMarker,
    ConfirmRallyMarker,
    RallyMarkerActivated,
    RallyMarkerLocked,
    RallyMarker_PlayerLocked,
    RallyMarker_BedrollLocked,
    RallyMarker_LandClaimLocked,
    LockPOI,
    UnlockPOI,
    ClearSleeper,
    ShowSleeperVolume,
    HideSleeperVolume,
    SetupFetch,
    SetupRestorePower,
    FinishManagedQuest,
    POILocked,
    ResetTraderQuests,
  }
}
