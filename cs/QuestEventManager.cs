// Decompiled with JetBrains decompiler
// Type: QuestEventManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using Challenges;
using Quests;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

#nullable disable
public class QuestEventManager
{
  [PublicizedFrom(EAccessModifier.Private)]
  public static QuestEventManager instance = (QuestEventManager) null;
  [PublicizedFrom(EAccessModifier.Private)]
  public List<BaseObjective> objectivesToUpdate = new List<BaseObjective>();
  [PublicizedFrom(EAccessModifier.Private)]
  public List<BaseChallengeObjective> challengeObjectivesToUpdate = new List<BaseChallengeObjective>();
  [PublicizedFrom(EAccessModifier.Private)]
  public List<TrackingHandler> questTrackersToUpdate = new List<TrackingHandler>();
  [PublicizedFrom(EAccessModifier.Private)]
  public ChallengeTrackingHandler challengeTrackerToUpdate;
  [PublicizedFrom(EAccessModifier.Private)]
  public List<Vector3> removeSleeperDataList = new List<Vector3>();
  public Dictionary<int, NPCQuestData> npcQuestData = new Dictionary<int, NPCQuestData>();
  public List<QuestTierReward> questTierRewards = new List<QuestTierReward>();
  public Dictionary<Vector3, SleeperEventData> SleeperVolumeUpdateDictionary = new Dictionary<Vector3, SleeperEventData>();
  public List<Vector3> SleeperVolumeLocationList = new List<Vector3>();
  public Dictionary<int, TreasureQuestData> TreasureQuestDictionary = new Dictionary<int, TreasureQuestData>();
  public Dictionary<int, RestorePowerQuestData> BlockActivateQuestDictionary = new Dictionary<int, RestorePowerQuestData>();
  public Dictionary<int, List<PrefabInstance>> tierPrefabList = new Dictionary<int, List<PrefabInstance>>();
  public Dictionary<TraderArea, List<QuestEventManager.PrefabListData>> TraderPrefabList = new Dictionary<TraderArea, List<QuestEventManager.PrefabListData>>();
  public Rect QuestBounds;
  public List<Vector3i> ActiveQuestBlocks = new List<Vector3i>();
  public Dictionary<int, int> ForceResetQuestTrader = new Dictionary<int, int>();
  public static FastTags<TagGroup.Global> manualResetTag = FastTags<TagGroup.Global>.Parse("manual");
  public static FastTags<TagGroup.Global> traderTag = FastTags<TagGroup.Global>.Parse("trader");
  public static FastTags<TagGroup.Global> clearTag = FastTags<TagGroup.Global>.Parse("clear");
  public static FastTags<TagGroup.Global> treasureTag = FastTags<TagGroup.Global>.Parse("treasure");
  public static FastTags<TagGroup.Global> fetchTag = FastTags<TagGroup.Global>.Parse("fetch");
  public static FastTags<TagGroup.Global> craftingTag = FastTags<TagGroup.Global>.Parse("crafting");
  public static FastTags<TagGroup.Global> restorePowerTag = FastTags<TagGroup.Global>.Parse("restore_power");
  public static FastTags<TagGroup.Global> infestedTag = FastTags<TagGroup.Global>.Parse("infested");
  public static FastTags<TagGroup.Global> banditTag = FastTags<TagGroup.Global>.Parse("bandit");
  public static FastTags<TagGroup.Global> allQuestTags = FastTags<TagGroup.Global>.CombineTags(FastTags<TagGroup.Global>.CombineTags(QuestEventManager.traderTag, QuestEventManager.clearTag, QuestEventManager.treasureTag, QuestEventManager.fetchTag), FastTags<TagGroup.Global>.CombineTags(QuestEventManager.craftingTag, QuestEventManager.restorePowerTag), FastTags<TagGroup.Global>.CombineTags(QuestEventManager.infestedTag, QuestEventManager.banditTag));
  [PublicizedFrom(EAccessModifier.Private)]
  public const int cTreasurePointAttempts = 5;
  [PublicizedFrom(EAccessModifier.Private)]
  public const float cTreasurePointDistanceAdd = 50f;
  [PublicizedFrom(EAccessModifier.Private)]
  public const float cTreasurePointMaxDistanceAdd = 500f;

  public static QuestEventManager Current
  {
    get
    {
      if (QuestEventManager.instance == null)
        QuestEventManager.instance = new QuestEventManager();
      return QuestEventManager.instance;
    }
  }

  public static bool HasInstance => QuestEventManager.instance != null;

  public void SetupTraderPrefabList(TraderArea area)
  {
    if (this.TraderPrefabList.ContainsKey(area))
      return;
    Vector3 vector3 = area.Position.ToVector3();
    List<PrefabInstance> poiPrefabs = GameManager.Instance.GetDynamicPrefabDecorator().GetPOIPrefabs();
    List<QuestEventManager.PrefabListData> prefabListDataList = new List<QuestEventManager.PrefabListData>();
    QuestEventManager.PrefabListData prefabListData1 = new QuestEventManager.PrefabListData();
    QuestEventManager.PrefabListData prefabListData2 = new QuestEventManager.PrefabListData();
    QuestEventManager.PrefabListData prefabListData3 = new QuestEventManager.PrefabListData();
    prefabListDataList.Add(prefabListData1);
    prefabListDataList.Add(prefabListData2);
    prefabListDataList.Add(prefabListData3);
    for (int index = 0; index < poiPrefabs.Count; ++index)
    {
      float num = Vector3.Distance(vector3, (Vector3) poiPrefabs[index].boundingBoxPosition);
      if ((double) num <= 500.0)
        prefabListData1.AddPOI(poiPrefabs[index]);
      else if ((double) num <= 1500.0)
        prefabListData2.AddPOI(poiPrefabs[index]);
      else
        prefabListData3.AddPOI(poiPrefabs[index]);
    }
    this.TraderPrefabList.Add(area, prefabListDataList);
  }

  public List<PrefabInstance> GetPrefabsForTrader(
    TraderArea traderArea,
    int difficulty,
    int index,
    GameRandom gameRandom)
  {
    if (traderArea == null)
      return (List<PrefabInstance>) null;
    if (!this.TraderPrefabList.ContainsKey(traderArea))
      this.SetupTraderPrefabList(traderArea);
    QuestEventManager.PrefabListData prefabListData = this.TraderPrefabList[traderArea][index];
    prefabListData.ShuffleDifficulty(difficulty, gameRandom);
    return prefabListData.TierData.ContainsKey(difficulty) ? prefabListData.TierData[difficulty] : (List<PrefabInstance>) null;
  }

  public List<PrefabInstance> GetPrefabsByDifficultyTier(int difficulty)
  {
    if (this.tierPrefabList.Count == 0)
    {
      List<PrefabInstance> poiPrefabs = GameManager.Instance.GetDynamicPrefabDecorator().GetPOIPrefabs();
      for (int index = 0; index < poiPrefabs.Count; ++index)
      {
        if (!this.tierPrefabList.ContainsKey((int) poiPrefabs[index].prefab.DifficultyTier))
          this.tierPrefabList.Add((int) poiPrefabs[index].prefab.DifficultyTier, new List<PrefabInstance>());
        this.tierPrefabList[(int) poiPrefabs[index].prefab.DifficultyTier].Add(poiPrefabs[index]);
      }
    }
    return this.tierPrefabList.ContainsKey(difficulty) ? this.tierPrefabList[difficulty] : (List<PrefabInstance>) null;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public QuestEventManager()
  {
  }

  public event QuestEvent_BlockEvent BlockActivate;

  public void BlockActivated(string blockName, Vector3i blockPos)
  {
    if (this.BlockActivate == null)
      return;
    this.BlockActivate(blockName, blockPos);
  }

  public event QuestEvent_BlockChangedEvent BlockChange;

  public void BlockChanged(Block blockOld, Block blockNew, Vector3i blockPos)
  {
    if (this.BlockChange == null)
      return;
    this.BlockChange(blockOld, blockNew, blockPos);
  }

  public event QuestEvent_BlockDestroyEvent BlockDestroy;

  public void BlockDestroyed(Block block, Vector3i blockPos, Entity byEntity = null)
  {
    if (this.BlockDestroy != null)
      this.BlockDestroy(block, blockPos);
    if (!block.AllowBlockTriggers || !Object.op_Implicit((Object) byEntity))
      return;
    EntityPlayer _player = byEntity as EntityPlayer;
    if (!Object.op_Implicit((Object) _player))
      _player = byEntity.world.GetClosestPlayer(byEntity, 500f, false);
    if (!Object.op_Implicit((Object) _player))
      return;
    block.HandleTrigger(_player, _player.world, 0, blockPos, new BlockValue()
    {
      type = block.blockID
    });
  }

  public event QuestEvent_BlockEvent BlockPickup;

  public void BlockPickedUp(string blockName, Vector3i blockPos)
  {
    if (this.BlockPickup == null)
      return;
    this.BlockPickup(blockName, blockPos);
  }

  public event QuestEvent_BlockEvent BlockPlace;

  public void BlockPlaced(string blockName, Vector3i blockPos)
  {
    if (this.BlockPlace == null)
      return;
    this.BlockPlace(blockName, blockPos);
  }

  public event QuestEvent_BlockEvent BlockUpgrade;

  public void BlockUpgraded(string blockName, Vector3i blockPos)
  {
    if (this.BlockUpgrade == null)
      return;
    this.BlockUpgrade(blockName, blockPos);
  }

  public event QuestEvent_ItemStackActionEvent AddItem;

  public void ItemAdded(ItemStack newStack)
  {
    if (this.AddItem == null)
      return;
    this.AddItem(newStack);
  }

  public event QuestEvent_HarvestStackActionEvent HarvestItem;

  public void HarvestedItem(ItemValue heldItem, ItemStack newStack, BlockValue bv)
  {
    if (this.HarvestItem == null)
      return;
    this.HarvestItem(heldItem, newStack, bv);
  }

  public event QuestEvent_ItemStackActionEvent AssembleItem;

  public void AssembledItem(ItemStack newStack)
  {
    if (this.AssembleItem == null)
      return;
    this.AssembleItem(newStack);
  }

  public event QuestEvent_ItemStackActionEvent CraftItem;

  public void CraftedItem(ItemStack newStack)
  {
    if (this.CraftItem == null)
      return;
    this.CraftItem(newStack);
  }

  public event QuestEvent_ItemStackActionEvent ExchangeFromItem;

  public void ExchangedFromItem(ItemStack newStack)
  {
    if (this.ExchangeFromItem == null)
      return;
    this.ExchangeFromItem(newStack);
  }

  public event QuestEvent_ItemStackActionEvent ScrapItem;

  public void ScrappedItem(ItemStack newStack)
  {
    if (this.ScrapItem == null)
      return;
    this.ScrapItem(newStack);
  }

  public event QuestEvent_ItemValueActionEvent RepairItem;

  public void RepairedItem(ItemValue newValue)
  {
    if (this.RepairItem == null)
      return;
    this.RepairItem(newValue);
  }

  public event QuestEvent_SkillPointSpent SkillPointSpent;

  public event QuestEvent_ItemValueActionEvent HoldItem;

  public void HeldItem(ItemValue newValue)
  {
    if (this.HoldItem == null)
      return;
    this.HoldItem(newValue);
  }

  public event QuestEvent_ItemValueActionEvent WearItem;

  public void WoreItem(ItemValue newValue)
  {
    if (this.WearItem == null)
      return;
    this.WearItem(newValue);
  }

  public void SpendSkillPoint(ProgressionValue skill)
  {
    if (this.SkillPointSpent == null)
      return;
    this.SkillPointSpent(skill.ProgressionClass.Name);
  }

  public event QuestEvent_WindowChanged WindowChanged;

  public void ChangedWindow(string windowName)
  {
    if (this.WindowChanged == null)
      return;
    this.WindowChanged(windowName);
  }

  public event QuestEvent_OpenContainer ContainerOpened;

  public void OpenedContainer(
    int entityId,
    Vector3i containerLocation,
    ITileEntityLootable tileEntity)
  {
    if (this.ContainerOpened == null)
      return;
    this.ContainerOpened(entityId, containerLocation, tileEntity);
  }

  public event QuestEvent_OpenContainer ContainerClosed;

  public void ClosedContainer(
    int entityId,
    Vector3i containerLocation,
    ITileEntityLootable tileEntity)
  {
    if (this.ContainerClosed == null)
      return;
    this.ContainerClosed(entityId, containerLocation, tileEntity);
  }

  public event QuestEvent_EntityKillEvent EntityKill;

  public void EntityKilled(EntityAlive killedBy, EntityAlive killedEntity)
  {
    if (this.EntityKill == null || !Object.op_Inequality((Object) killedBy, (Object) null) || !Object.op_Inequality((Object) killedEntity, (Object) null))
      return;
    this.EntityKill(killedBy, killedEntity);
  }

  public event QuestEvent_NPCInteracted NPCInteract;

  public void NPCInteracted(EntityNPC entityNPC)
  {
    if (this.NPCInteract == null)
      return;
    this.NPCInteract(entityNPC);
  }

  public event QuestEvent_NPCInteracted NPCMeet;

  public void NPCMet(EntityNPC entityNPC)
  {
    if (this.NPCMeet == null)
      return;
    this.NPCMeet(entityNPC);
  }

  public event QuestEvent_SleepersCleared SleepersCleared;

  public void ClearedSleepers(Vector3 prefabPos)
  {
    if (this.SleepersCleared == null)
      return;
    this.SleepersCleared(prefabPos);
  }

  public event QuestEvent_Explosion ExplosionDetected;

  public void DetectedExplosion(Vector3 explosionPos, int entityID, float blockDamage)
  {
    if (this.ExplosionDetected == null)
      return;
    this.ExplosionDetected(explosionPos, entityID, blockDamage);
  }

  public event QuestEvent_PurchaseEvent BuyItems;

  public event QuestEvent_PurchaseEvent SellItems;

  public void BoughtItems(string traderName, int itemCount)
  {
    if (this.BuyItems == null)
      return;
    this.BuyItems(traderName, itemCount);
  }

  public void SoldItems(string traderName, int itemCount)
  {
    if (this.SellItems == null)
      return;
    this.SellItems(traderName, itemCount);
  }

  public event QuestEvent_ChallengeCompleteEvent ChallengeComplete;

  public void ChallengeCompleted(ChallengeClass challenge, bool isRedeemed)
  {
    if (this.ChallengeComplete == null)
      return;
    this.ChallengeComplete(challenge, isRedeemed);
  }

  public event QuestEvent_TwitchEvent TwitchEventReceive;

  public void TwitchEventReceived(TwitchObjectiveTypes actionType, string param)
  {
    if (this.TwitchEventReceive == null)
      return;
    this.TwitchEventReceive(actionType, param);
  }

  public event QuestEvent_QuestCompleteEvent QuestComplete;

  public void QuestCompleted(FastTags<TagGroup.Global> questTags, QuestClass questClass)
  {
    if (this.QuestComplete == null)
      return;
    this.QuestComplete(questTags, questClass);
  }

  public event QuestEvent_ChallengeAwardCredit ChallengeAwardCredit;

  public void ChallengeAwardCredited(string challengeStat, int creditAmount)
  {
    if (this.ChallengeAwardCredit == null)
      return;
    this.ChallengeAwardCredit(challengeStat, creditAmount);
  }

  public event QuestEvent_BiomeEvent BiomeEnter;

  public void BiomeEntered(BiomeDefinition biomeDef)
  {
    if (this.BiomeEnter == null)
      return;
    this.BiomeEnter(biomeDef);
  }

  public event QuestEvent_ItemValueActionEvent UseItem;

  public void UsedItem(ItemValue newValue)
  {
    if (this.UseItem == null)
      return;
    this.UseItem(newValue);
  }

  public event QuestEvent_FloatEvent TimeSurvive;

  public void TimeSurvived(float time)
  {
    if (this.TimeSurvive == null)
      return;
    this.TimeSurvive(time);
  }

  public event QuestEvent_Event BloodMoonSurvive;

  public void BloodMoonSurvived()
  {
    if (this.BloodMoonSurvive == null)
      return;
    this.BloodMoonSurvive();
  }

  public void Update()
  {
    ObjectiveRallyPoint.SetupFlags(this.objectivesToUpdate);
    float deltaTime = Time.deltaTime;
    for (int index = 0; index < this.objectivesToUpdate.Count; ++index)
      this.objectivesToUpdate[index].HandleUpdate(deltaTime);
    for (int index = 0; index < this.challengeObjectivesToUpdate.Count; ++index)
      this.challengeObjectivesToUpdate[index].HandleUpdate(deltaTime);
    for (int index = this.questTrackersToUpdate.Count - 1; index >= 0; --index)
    {
      if (!this.questTrackersToUpdate[index].Update(deltaTime))
        this.questTrackersToUpdate.RemoveAt(index);
    }
    if (this.challengeTrackerToUpdate != null && !this.challengeTrackerToUpdate.Update(deltaTime))
      this.challengeTrackerToUpdate = (ChallengeTrackingHandler) null;
    foreach (KeyValuePair<Vector3, SleeperEventData> sleeperVolumeUpdate in this.SleeperVolumeUpdateDictionary)
    {
      if (sleeperVolumeUpdate.Value.Update())
        this.removeSleeperDataList.Add(sleeperVolumeUpdate.Value.position);
    }
    for (int index = 0; index < this.removeSleeperDataList.Count; ++index)
      this.SleeperVolumeUpdateDictionary.Remove(this.removeSleeperDataList[index]);
    this.removeSleeperDataList.Clear();
  }

  public void HandlePlayerDisconnect(EntityPlayer player)
  {
    for (int index = 0; index < player.QuestJournal.quests.Count; ++index)
    {
      Quest quest = player.QuestJournal.quests[index];
      if (quest.CurrentState == Quest.QuestState.InProgress)
      {
        quest.HandleUnlockPOI(player);
        this.FinishTreasureQuest(quest.QuestCode, player);
      }
    }
  }

  public void HandleAllPlayersDisconnect()
  {
    foreach (int key in this.TreasureQuestDictionary.Keys)
      this.TreasureQuestDictionary[key].Remove();
    this.TreasureQuestDictionary.Clear();
  }

  [PublicizedFrom(EAccessModifier.Internal)]
  public void AddTraderResetQuestsForPlayer(int playerID, int traderID)
  {
    if (!this.ForceResetQuestTrader.ContainsKey(playerID))
      this.ForceResetQuestTrader.Add(playerID, traderID);
    else
      this.ForceResetQuestTrader[playerID] = traderID;
  }

  public void ClearTraderResetQuestsForPlayer(int playerID)
  {
    if (!this.ForceResetQuestTrader.ContainsKey(playerID))
      return;
    this.ForceResetQuestTrader.Remove(playerID);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public bool CheckResetQuestTrader(int playerEntityID, int npcEntityID)
  {
    if (!this.ForceResetQuestTrader.ContainsKey(playerEntityID))
      return false;
    Log.Out($"CheckResetQuestTrader {this.ForceResetQuestTrader[playerEntityID] == npcEntityID}");
    return this.ForceResetQuestTrader[playerEntityID] == npcEntityID;
  }

  public void AddObjectiveToBeUpdated(BaseObjective obj)
  {
    if (this.objectivesToUpdate.Contains(obj))
      return;
    this.objectivesToUpdate.Add(obj);
  }

  public void RemoveObjectiveToBeUpdated(BaseObjective obj)
  {
    if (!this.objectivesToUpdate.Contains(obj))
      return;
    this.objectivesToUpdate.Remove(obj);
  }

  public void AddObjectiveToBeUpdated(BaseChallengeObjective obj)
  {
    if (this.challengeObjectivesToUpdate.Contains(obj))
      return;
    this.challengeObjectivesToUpdate.Add(obj);
  }

  public void RemoveObjectiveToBeUpdated(BaseChallengeObjective obj)
  {
    if (!this.challengeObjectivesToUpdate.Contains(obj))
      return;
    this.challengeObjectivesToUpdate.Remove(obj);
  }

  public void AddTrackerToBeUpdated(TrackingHandler track)
  {
    if (this.questTrackersToUpdate.Contains(track))
      return;
    this.questTrackersToUpdate.Add(track);
  }

  public void RemoveTrackerToBeUpdated(TrackingHandler track)
  {
    if (!this.questTrackersToUpdate.Contains(track))
      return;
    this.questTrackersToUpdate.Remove(track);
  }

  public void AddTrackerToBeUpdated(ChallengeTrackingHandler track)
  {
    this.challengeTrackerToUpdate = track;
  }

  public void RemoveTrackerToBeUpdated(ChallengeTrackingHandler track)
  {
    this.challengeTrackerToUpdate = (ChallengeTrackingHandler) null;
  }

  public event QuestEvent_SleeperVolumePositionChanged SleeperVolumePositionAdd;

  public event QuestEvent_SleeperVolumePositionChanged SleeperVolumePositionRemove;

  public void SleeperVolumePositionAdded(Vector3 pos)
  {
    if (this.SleeperVolumePositionAdd == null)
      return;
    this.SleeperVolumePositionAdd(pos);
  }

  public void SleeperVolumePositionRemoved(Vector3 pos)
  {
    if (this.SleeperVolumePositionRemove == null)
      return;
    this.SleeperVolumePositionRemove(pos);
  }

  public void AddSleeperVolumeLocation(Vector3 newLocation)
  {
    this.SleeperVolumeLocationList.Add(newLocation);
  }

  public void SubscribeToUpdateEvent(int entityID, Vector3 prefabPos)
  {
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
    {
      if (!this.SleeperVolumeUpdateDictionary.ContainsKey(prefabPos))
      {
        SleeperEventData sleeperEventData = new SleeperEventData();
        sleeperEventData.SetupData(prefabPos);
        this.SleeperVolumeUpdateDictionary.Add(prefabPos, sleeperEventData);
      }
      SleeperEventData sleeperVolumeUpdate = this.SleeperVolumeUpdateDictionary[prefabPos];
      this.removeSleeperDataList.Remove(prefabPos);
      if (sleeperVolumeUpdate.EntityList.Contains(entityID))
        return;
      sleeperVolumeUpdate.EntityList.Add(entityID);
    }
    else
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageQuestEvent>().Setup(NetPackageQuestEvent.QuestEventTypes.ClearSleeper, entityID, prefabPos, true));
  }

  public void UnSubscribeToUpdateEvent(int entityID, Vector3 prefabPos)
  {
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
    {
      if (!this.SleeperVolumeUpdateDictionary.ContainsKey(prefabPos))
        return;
      SleeperEventData sleeperVolumeUpdate = this.SleeperVolumeUpdateDictionary[prefabPos];
      if (!sleeperVolumeUpdate.EntityList.Contains(entityID))
        return;
      sleeperVolumeUpdate.EntityList.Remove(entityID);
      if (sleeperVolumeUpdate.EntityList.Count == 0)
        this.removeSleeperDataList.Add(prefabPos);
      foreach (SleeperVolume sleeperVolume in sleeperVolumeUpdate.SleeperVolumes)
        QuestEventManager.Current.SleeperVolumePositionRemoved(sleeperVolume.Center);
    }
    else
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageQuestEvent>().Setup(NetPackageQuestEvent.QuestEventTypes.ClearSleeper, entityID, prefabPos, false));
  }

  public IEnumerator QuestLockPOI(
    int entityID,
    QuestClass questClass,
    Vector3 prefabPos,
    FastTags<TagGroup.Global> questTags,
    int[] sharedWithList,
    Action completionCallback)
  {
    List<PrefabInstance> fromWorldPosInside = GameManager.Instance.GetDynamicPrefabDecorator().GetPrefabsFromWorldPosInside(prefabPos, questTags);
    yield return (object) GameManager.Instance.World.ResetPOIS(fromWorldPosInside, questTags, entityID, sharedWithList, questClass);
    if (completionCallback != null)
      completionCallback();
  }

  public void QuestUnlockPOI(int entityID, Vector3 prefabPos)
  {
    PrefabInstance prefabFromWorldPos = GameManager.Instance.GetDynamicPrefabDecorator().GetPrefabFromWorldPos((int) prefabPos.x, (int) prefabPos.z);
    if (prefabFromWorldPos.lockInstance == null)
      return;
    prefabFromWorldPos.lockInstance.RemoveQuester(entityID);
  }

  public QuestEventManager.POILockoutReasonTypes CheckForPOILockouts(
    int entityId,
    Vector2 prefabPos,
    out ulong extraData)
  {
    World world = GameManager.Instance.World;
    PrefabInstance prefabFromWorldPos = GameManager.Instance.GetDynamicPrefabDecorator().GetPrefabFromWorldPos((int) prefabPos.x, (int) prefabPos.y);
    Rect rect;
    // ISSUE: explicit constructor call
    ((Rect) ref rect).\u002Ector((float) prefabFromWorldPos.boundingBoxPosition.x, (float) prefabFromWorldPos.boundingBoxPosition.z, (float) prefabFromWorldPos.boundingBoxSize.x, (float) prefabFromWorldPos.boundingBoxSize.z);
    if (prefabFromWorldPos.lockInstance != null && prefabFromWorldPos.lockInstance.CheckQuestLock())
      prefabFromWorldPos.lockInstance = (QuestLockInstance) null;
    if (prefabFromWorldPos.lockInstance != null)
    {
      extraData = prefabFromWorldPos.lockInstance.LockedOutUntil;
      return QuestEventManager.POILockoutReasonTypes.QuestLock;
    }
    extraData = 0UL;
    EntityPlayer entity = (EntityPlayer) world.GetEntity(entityId);
    if (Object.op_Inequality((Object) entity, (Object) null))
    {
      for (int index = 0; index < world.Players.list.Count; ++index)
      {
        Vector3 position = world.Players.list[index].GetPosition();
        EntityPlayer entityPlayer = world.Players.list[index];
        if (Object.op_Inequality((Object) entity, (Object) entityPlayer) && (!entity.IsInParty() || !entity.Party.MemberList.Contains(entityPlayer)) && ((Rect) ref rect).Contains(new Vector2(position.x, position.z)))
          return QuestEventManager.POILockoutReasonTypes.PlayerInside;
      }
    }
    switch (prefabFromWorldPos.CheckForAnyPlayerHome(world))
    {
      case GameUtils.EPlayerHomeType.Landclaim:
        return QuestEventManager.POILockoutReasonTypes.LandClaim;
      case GameUtils.EPlayerHomeType.Bedroll:
        return QuestEventManager.POILockoutReasonTypes.Bedroll;
      default:
        return QuestEventManager.POILockoutReasonTypes.None;
    }
  }

  public void SetupRepairForMP(
    List<Vector3i> repairBlockList,
    List<bool> repairStates,
    World _world,
    Vector3 prefabPos)
  {
    PrefabInstance prefabFromWorldPos = GameManager.Instance.GetDynamicPrefabDecorator().GetPrefabFromWorldPos((int) prefabPos.x, (int) prefabPos.z);
    Vector3i vector3i = new Vector3i(prefabPos);
    Vector3i size = prefabFromWorldPos.prefab.size;
    int chunkXz1 = World.toChunkXZ(vector3i.x - 1);
    int chunkXz2 = World.toChunkXZ(vector3i.x + size.x + 1);
    int chunkXz3 = World.toChunkXZ(vector3i.z - 1);
    int chunkXz4 = World.toChunkXZ(vector3i.z + size.z + 1);
    repairBlockList.Clear();
    repairStates.Clear();
    Rect rect;
    // ISSUE: explicit constructor call
    ((Rect) ref rect).\u002Ector((float) vector3i.x, (float) vector3i.z, (float) size.x, (float) size.z);
    for (int chunkX = chunkXz1; chunkX <= chunkXz2; ++chunkX)
    {
      for (int chunkZ = chunkXz3; chunkZ <= chunkXz4; ++chunkZ)
      {
        if (_world.GetChunkSync(chunkX, chunkZ) is Chunk chunkSync)
        {
          List<Vector3i> indexedBlock = chunkSync.IndexedBlocks[Constants.cQuestRestorePowerIndexName];
          if (indexedBlock != null)
          {
            for (int index = 0; index < indexedBlock.Count; ++index)
            {
              BlockValue block = chunkSync.GetBlock(indexedBlock[index]);
              if (!block.ischild)
              {
                Vector3i worldPos = chunkSync.ToWorldPos(indexedBlock[index]);
                if (((Rect) ref rect).Contains(new Vector2((float) worldPos.x, (float) worldPos.z)))
                {
                  repairStates.Add(!block.Block.UpgradeBlock.isair);
                  repairBlockList.Add(worldPos);
                }
              }
            }
          }
        }
      }
    }
  }

  public void SetupActivateForMP(
    int entityID,
    int questCode,
    string completeEvent,
    List<Vector3i> activateBlockList,
    World _world,
    Vector3 prefabPos,
    string indexName,
    int[] sharedWithList)
  {
    PrefabInstance prefabFromWorldPos = GameManager.Instance.GetDynamicPrefabDecorator().GetPrefabFromWorldPos((int) prefabPos.x, (int) prefabPos.z);
    Vector3i targetPosition = new Vector3i(prefabPos);
    Vector3i size = prefabFromWorldPos.prefab.size;
    EntityPlayer entity1 = _world.GetEntity(entityID) as EntityPlayer;
    int chunkXz1 = World.toChunkXZ(targetPosition.x - 1);
    int chunkXz2 = World.toChunkXZ(targetPosition.x + size.x + 1);
    int chunkXz3 = World.toChunkXZ(targetPosition.z - 1);
    int chunkXz4 = World.toChunkXZ(targetPosition.z + size.z + 1);
    activateBlockList.Clear();
    Rect rect;
    // ISSUE: explicit constructor call
    ((Rect) ref rect).\u002Ector((float) targetPosition.x, (float) targetPosition.z, (float) size.x, (float) size.z);
    BlockChangeInfo blockChangeInfo = new BlockChangeInfo();
    List<BlockChangeInfo> blockChanges = new List<BlockChangeInfo>();
    for (int chunkX = chunkXz1; chunkX <= chunkXz2; ++chunkX)
    {
      for (int chunkZ = chunkXz3; chunkZ <= chunkXz4; ++chunkZ)
      {
        if (_world.GetChunkSync(chunkX, chunkZ) is Chunk chunkSync)
        {
          List<Vector3i> indexedBlock = chunkSync.IndexedBlocks[indexName];
          if (indexedBlock != null)
          {
            for (int index = 0; index < indexedBlock.Count; ++index)
            {
              BlockValue block = chunkSync.GetBlock(indexedBlock[index]);
              if (!block.ischild)
              {
                Vector3i worldPos = chunkSync.ToWorldPos(indexedBlock[index]);
                if (((Rect) ref rect).Contains(new Vector2((float) worldPos.x, (float) worldPos.z)))
                {
                  activateBlockList.Add(worldPos);
                  if (block.Block is BlockQuestActivate)
                    (block.Block as BlockQuestActivate).SetupForQuest((WorldBase) _world, chunkSync, worldPos, block, blockChanges);
                }
              }
            }
          }
        }
      }
    }
    if (entity1 is EntityPlayerLocal)
      entity1.QuestJournal.HandleRestorePowerReceived(prefabPos, activateBlockList);
    else
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageQuestEvent>().Setup(NetPackageQuestEvent.QuestEventTypes.SetupRestorePower, entity1.entityId, questCode, completeEvent, prefabPos, activateBlockList), _attachedToEntityId: entity1.entityId);
    QuestEventManager.Current.AddRestorePowerQuest(questCode, entityID, new Vector3i(prefabPos), completeEvent);
    if (entity1.IsInParty() && sharedWithList != null)
    {
      Party party = entity1.Party;
      for (int index = 0; index < sharedWithList.Length; ++index)
      {
        EntityPlayer entity2 = _world.GetEntity(sharedWithList[index]) as EntityPlayer;
        if (entity2 is EntityPlayerLocal)
          entity2.QuestJournal.HandleRestorePowerReceived(prefabPos, activateBlockList);
        else
          SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageQuestEvent>().Setup(NetPackageQuestEvent.QuestEventTypes.SetupRestorePower, entity2.entityId, questCode, completeEvent, prefabPos, activateBlockList), _attachedToEntityId: entity2.entityId);
        QuestEventManager.Current.AddRestorePowerQuest(questCode, sharedWithList[index], new Vector3i(prefabPos), completeEvent);
      }
    }
    if (blockChanges.Count > 0)
      GameManager.Instance.StartCoroutine(this.UpdateBlocks(blockChanges));
    GameEventManager.Current.HandleAction("quest_poi_lights_off", (EntityPlayer) null, (Entity) entity1, false, (Vector3) targetPosition);
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public IEnumerator UpdateBlocks(List<BlockChangeInfo> blockChanges)
  {
    yield return (object) new WaitForSeconds(1f);
    if (Object.op_Inequality((Object) GameManager.Instance, (Object) null) && GameManager.Instance.World != null)
      GameManager.Instance.World.SetBlocksRPC(blockChanges);
  }

  public void SetupFetchForMP(
    int entityID,
    Vector3 prefabPos,
    ObjectiveFetchFromContainer.FetchModeTypes fetchMode,
    int[] sharedWithList)
  {
    PrefabInstance prefabFromWorldPos = GameManager.Instance.GetDynamicPrefabDecorator().GetPrefabFromWorldPos((int) prefabPos.x, (int) prefabPos.z);
    this.HandleContainerPositions(GameManager.Instance.World, entityID, new Vector3i(prefabPos), prefabFromWorldPos.prefab.size, fetchMode, sharedWithList);
  }

  public void HandleContainerPositions(
    World _world,
    int _entityID,
    Vector3i _prefabPosition,
    Vector3i _prefabSize,
    ObjectiveFetchFromContainer.FetchModeTypes fetchMode,
    int[] sharedWithList)
  {
    int chunkXz1 = World.toChunkXZ(_prefabPosition.x - 1);
    int chunkXz2 = World.toChunkXZ(_prefabPosition.x + _prefabSize.x + 1);
    int chunkXz3 = World.toChunkXZ(_prefabPosition.z - 1);
    int chunkXz4 = World.toChunkXZ(_prefabPosition.z + _prefabSize.z + 1);
    List<Vector3i> vector3iList = new List<Vector3i>();
    Rect rect;
    // ISSUE: explicit constructor call
    ((Rect) ref rect).\u002Ector((float) _prefabPosition.x, (float) _prefabPosition.z, (float) _prefabSize.x, (float) _prefabSize.z);
    for (int chunkX = chunkXz1; chunkX <= chunkXz2; ++chunkX)
    {
      for (int chunkZ = chunkXz3; chunkZ <= chunkXz4; ++chunkZ)
      {
        if (_world.GetChunkSync(chunkX, chunkZ) is Chunk chunkSync)
        {
          List<Vector3i> indexedBlock = chunkSync.IndexedBlocks[Constants.cQuestLootFetchContainerIndexName];
          if (indexedBlock != null)
          {
            for (int index = 0; index < indexedBlock.Count; ++index)
            {
              if (!chunkSync.GetBlock(indexedBlock[index]).ischild)
              {
                Vector3i worldPos = chunkSync.ToWorldPos(indexedBlock[index]);
                if (((Rect) ref rect).Contains(new Vector2((float) worldPos.x, (float) worldPos.z)))
                  vector3iList.Add(worldPos);
              }
            }
          }
        }
      }
    }
    if (vector3iList.Count == 0)
    {
      Log.Error("Valid container not found for fetch loot.");
    }
    else
    {
      List<int> intList = new List<int>();
      EntityPlayer entity1 = _world.GetEntity(_entityID) as EntityPlayer;
      Quest.PositionDataTypes dataType = fetchMode == ObjectiveFetchFromContainer.FetchModeTypes.Standard ? Quest.PositionDataTypes.FetchContainer : Quest.PositionDataTypes.HiddenCache;
      int index1 = _world.GetGameRandom().RandomRange(vector3iList.Count);
      if (entity1 is EntityPlayerLocal)
        entity1.QuestJournal.SetActivePositionData(dataType, vector3iList[index1]);
      else
        SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageQuestEvent>().Setup(NetPackageQuestEvent.QuestEventTypes.SetupFetch, _entityID, vector3iList[index1].ToVector3(), fetchMode));
      intList.Add(index1);
      if (entity1.IsInParty() && sharedWithList != null)
      {
        Party party = entity1.Party;
        for (int index2 = 0; index2 < sharedWithList.Length; ++index2)
        {
          EntityPlayer entity2 = _world.GetEntity(sharedWithList[index2]) as EntityPlayer;
          int index3 = _world.GetGameRandom().RandomRange(vector3iList.Count);
          if (entity2 is EntityPlayerLocal)
            entity2.QuestJournal.SetActivePositionData(dataType, vector3iList[index3]);
          else
            SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageQuestEvent>().Setup(NetPackageQuestEvent.QuestEventTypes.SetupFetch, entity2.entityId, vector3iList[index3].ToVector3(), fetchMode));
          if (!intList.Contains(index3))
            intList.Add(index3);
        }
      }
      List<BlockChangeInfo> blockChanges = new List<BlockChangeInfo>();
      GameRandom gameRandom = GameManager.Instance.World.GetGameRandom();
      for (int index4 = 0; index4 < vector3iList.Count; ++index4)
      {
        if (!intList.Contains(index4))
        {
          Chunk chunkFromWorldPos = (Chunk) _world.GetChunkFromWorldPos(vector3iList[index4]);
          BlockValue _blockValue = BlockPlaceholderMap.Instance.Replace(Block.GetBlockValue("cntQuestRandomLootHelper"), gameRandom, chunkFromWorldPos, vector3iList[index4].x, 0, vector3iList[index4].z, FastTags<TagGroup.Global>.none);
          blockChanges.Add(new BlockChangeInfo(chunkFromWorldPos.ClrIdx, vector3iList[index4], _blockValue));
        }
      }
      if (blockChanges.Count <= 0)
        return;
      GameManager.Instance.StartCoroutine(this.UpdateBlocks(blockChanges));
    }
  }

  public void Cleanup()
  {
    this.BlockPickup = (QuestEvent_BlockEvent) null;
    this.BlockPlace = (QuestEvent_BlockEvent) null;
    this.BlockUpgrade = (QuestEvent_BlockEvent) null;
    this.AddItem = (QuestEvent_ItemStackActionEvent) null;
    this.AssembleItem = (QuestEvent_ItemStackActionEvent) null;
    this.CraftItem = (QuestEvent_ItemStackActionEvent) null;
    this.ExchangeFromItem = (QuestEvent_ItemStackActionEvent) null;
    this.ScrapItem = (QuestEvent_ItemStackActionEvent) null;
    this.RepairItem = (QuestEvent_ItemValueActionEvent) null;
    this.SkillPointSpent = (QuestEvent_SkillPointSpent) null;
    this.WearItem = (QuestEvent_ItemValueActionEvent) null;
    this.WindowChanged = (QuestEvent_WindowChanged) null;
    this.ContainerOpened = (QuestEvent_OpenContainer) null;
    this.EntityKill = (QuestEvent_EntityKillEvent) null;
    this.HarvestItem = (QuestEvent_HarvestStackActionEvent) null;
    this.SellItems = (QuestEvent_PurchaseEvent) null;
    this.BuyItems = (QuestEvent_PurchaseEvent) null;
    this.ExplosionDetected = (QuestEvent_Explosion) null;
    this.ChallengeComplete = (QuestEvent_ChallengeCompleteEvent) null;
    this.BiomeEnter = (QuestEvent_BiomeEvent) null;
    this.UseItem = (QuestEvent_ItemValueActionEvent) null;
    this.TimeSurvive = (QuestEvent_FloatEvent) null;
    this.BloodMoonSurvive = (QuestEvent_Event) null;
    this.objectivesToUpdate = (List<BaseObjective>) null;
    this.npcQuestData.Clear();
    this.npcQuestData = (Dictionary<int, NPCQuestData>) null;
    this.questTierRewards.Clear();
    this.questTierRewards = (List<QuestTierReward>) null;
    QuestEventManager.instance = (QuestEventManager) null;
  }

  public void SetupQuestList(EntityTrader npc, int playerEntityID, List<Quest> questList)
  {
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
      return;
    if (!this.npcQuestData.ContainsKey(npc.entityId))
      this.npcQuestData.Add(npc.entityId, new NPCQuestData());
    if (!this.npcQuestData[npc.entityId].PlayerQuestList.ContainsKey(playerEntityID))
      this.npcQuestData[npc.entityId].PlayerQuestList.Add(playerEntityID, new NPCQuestData.PlayerQuestData(questList));
    else
      this.npcQuestData[npc.entityId].PlayerQuestList[playerEntityID].QuestList = questList;
    if (GameManager.Instance.World.GetEntity(playerEntityID) is EntityPlayerLocal)
      return;
    NetPackageNPCQuestList.SendQuestPacketsToPlayer(npc, playerEntityID);
  }

  public List<Quest> GetQuestList(World world, int npcEntityID, int playerEntityID)
  {
    if (this.npcQuestData.ContainsKey(npcEntityID))
    {
      NPCQuestData npcQuestData = this.npcQuestData[npcEntityID];
      if (npcQuestData.PlayerQuestList.ContainsKey(playerEntityID))
      {
        NPCQuestData.PlayerQuestData playerQuest = npcQuestData.PlayerQuestList[playerEntityID];
        if (QuestEventManager.Current.CheckResetQuestTrader(playerEntityID, npcEntityID))
        {
          playerQuest.QuestList.Clear();
          playerQuest.QuestList = (List<Quest>) null;
          QuestEventManager.Current.ClearTraderResetQuestsForPlayer(playerEntityID);
        }
        else if ((int) ((long) world.GetWorldTime() - (long) playerQuest.LastUpdate) > 24000)
        {
          playerQuest.QuestList.Clear();
          playerQuest.QuestList = (List<Quest>) null;
        }
        return playerQuest.QuestList;
      }
    }
    return (List<Quest>) null;
  }

  public void ClearQuestList(int npcEntityID)
  {
    if (!this.npcQuestData.ContainsKey(npcEntityID))
      return;
    this.npcQuestData[npcEntityID].PlayerQuestList.Clear();
  }

  public void ClearQuestListForPlayer(int npcEntityID, int playerID)
  {
    if (!this.npcQuestData.ContainsKey(npcEntityID))
      return;
    NPCQuestData npcQuestData = this.npcQuestData[npcEntityID];
    if (!npcQuestData.PlayerQuestList.ContainsKey(playerID))
      return;
    npcQuestData.PlayerQuestList.Remove(playerID);
  }

  public void AddQuestTierReward(QuestTierReward reward)
  {
    if (this.questTierRewards == null)
      this.questTierRewards = new List<QuestTierReward>();
    this.questTierRewards.Add(reward);
  }

  public void HandleNewCompletedQuest(
    EntityPlayer player,
    byte questFaction,
    int completedQuestTier,
    bool addsToTierComplete)
  {
    if (!addsToTierComplete)
      return;
    int currentFactionTier1 = player.QuestJournal.GetCurrentFactionTier(questFaction, allowExtraTierOverMax: true);
    int currentFactionTier2 = player.QuestJournal.GetCurrentFactionTier(questFaction, completedQuestTier, true);
    int num = currentFactionTier2;
    if (currentFactionTier1 == num)
      return;
    for (int index = 0; index < this.questTierRewards.Count; ++index)
    {
      if (this.questTierRewards[index].Tier == currentFactionTier2)
        this.questTierRewards[index].GiveRewards(player);
    }
  }

  public void HandleRallyMarkerActivate(
    EntityPlayerLocal _player,
    Vector3i blockPos,
    BlockValue blockValue)
  {
    Quest quest = _player.QuestJournal.HasQuestAtRallyPosition(blockPos.ToVector3());
    if (quest == null)
      return;
    Action _onOk = (Action) ([PublicizedFrom(EAccessModifier.Internal)] () => QuestEventManager.Current.BlockActivated(blockValue.Block.GetBlockName(), blockPos));
    if (_player.IsInParty())
    {
      List<EntityPlayer> withListNotInRange = quest.GetSharedWithListNotInRange();
      if (withListNotInRange != null && withListNotInRange.Count > 0)
      {
        StringBuilder stringBuilder = new StringBuilder();
        for (int index = 0; index < withListNotInRange.Count; ++index)
        {
          stringBuilder.Append(withListNotInRange[index].PlayerDisplayName);
          if (index < withListNotInRange.Count - 1)
            stringBuilder.Append(", ");
        }
        XUiC_MessageBoxWindowGroup.ShowMessageBox(_player.PlayerUI.xui, "Rally Activate", string.Format(Localization.Get("xuiQuestRallyOutOfRange"), (object) stringBuilder.ToString().Trim(',')), XUiC_MessageBoxWindowGroup.MessageBoxTypes.OkCancel, _onOk);
      }
      else
        _onOk();
    }
    else
      _onOk();
  }

  public void AddTreasureQuest(
    int _questCode,
    int _entityID,
    int _blocksPerReduction,
    Vector3i _position,
    Vector3 _treasureOffset)
  {
    if (this.TreasureQuestDictionary.ContainsKey(_questCode))
      return;
    TreasureQuestData treasureQuestData = new TreasureQuestData(_questCode, _entityID, _blocksPerReduction, _position, _treasureOffset);
    this.TreasureQuestDictionary.Add(_questCode, treasureQuestData);
  }

  public void SetTreasureContainerPosition(int _questCode, Vector3i _updatedPosition)
  {
    if (!this.TreasureQuestDictionary.ContainsKey(_questCode))
      return;
    this.TreasureQuestDictionary[_questCode].UpdatePosition(_updatedPosition);
  }

  public bool GetTreasureContainerPosition(
    int _questCode,
    float _distance,
    int _offset,
    float _treasureRadius,
    Vector3 _startPosition,
    int _entityID,
    bool _useNearby,
    int _currentBlocksPerReduction,
    out int _blocksPerReduction,
    out Vector3i _position,
    out Vector3 _treasureOffset)
  {
    _position = Vector3i.zero;
    _treasureOffset = Vector3.zero;
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
    {
      if (this.TreasureQuestDictionary.ContainsKey(_questCode))
      {
        _position = this.TreasureQuestDictionary[_questCode].Position;
        _treasureOffset = this.TreasureQuestDictionary[_questCode].TreasureOffset;
        this.TreasureQuestDictionary[_questCode].AddSharedQuester(_entityID, _currentBlocksPerReduction);
        _blocksPerReduction = this.TreasureQuestDictionary[_questCode].BlocksPerReduction;
        return true;
      }
      _blocksPerReduction = _currentBlocksPerReduction;
      float num = _distance + 500f;
      for (float distance = _distance; (double) distance < (double) num; distance += 50f)
      {
        for (int index = 0; index < 5; ++index)
        {
          if (ObjectiveTreasureChest.CalculateTreasurePoint(_startPosition, distance, _offset, _treasureRadius - 1f, _useNearby, out _position, out _treasureOffset))
          {
            this.AddTreasureQuest(_questCode, _entityID, _blocksPerReduction, _position, _treasureOffset);
            return true;
          }
        }
      }
      return false;
    }
    SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageQuestTreasurePoint>().Setup(_questCode, _distance, _offset, _treasureRadius, _startPosition, _entityID, _useNearby, _currentBlocksPerReduction));
    _position = Vector3i.zero;
    _treasureOffset = Vector3.zero;
    _blocksPerReduction = _currentBlocksPerReduction;
    return true;
  }

  public void UpdateTreasureBlocksPerReduction(int _questCode, int _newBlocksPerReduction)
  {
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
    {
      if (!this.TreasureQuestDictionary.ContainsKey(_questCode))
        return;
      this.TreasureQuestDictionary[_questCode].SendBlocksPerReductionUpdate(_newBlocksPerReduction);
    }
    else
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageQuestTreasurePoint>().Setup(_questCode, _newBlocksPerReduction));
  }

  public void FinishTreasureQuest(int _questCode, EntityPlayer _player)
  {
    TreasureQuestData treasureQuestData;
    if (!this.TreasureQuestDictionary.TryGetValue(_questCode, out treasureQuestData))
      return;
    treasureQuestData.RemoveSharedQuester(_player);
    if (!(GameManager.Instance.World.ChunkCache.ChunkProvider is ChunkProviderGenerateWorld chunkProvider))
      return;
    Debug.Log((object) $"[FinishTreasureQuest] Requesting reset at world position: {treasureQuestData.Position}");
    Vector2i chunkXz = World.toChunkXZ(treasureQuestData.Position);
    for (int x = chunkXz.x - 1; x <= chunkXz.x + 1; ++x)
    {
      for (int y = chunkXz.y - 1; y <= chunkXz.y + 1; ++y)
      {
        long _chunkKey = WorldChunkCache.MakeChunkKey(x, y);
        chunkProvider.RequestChunkReset(_chunkKey);
      }
    }
  }

  public void AddRestorePowerQuest(
    int _questCode,
    int _entityID,
    Vector3i _position,
    string _completeEvent)
  {
    if (!this.BlockActivateQuestDictionary.ContainsKey(_questCode))
    {
      RestorePowerQuestData restorePowerQuestData = new RestorePowerQuestData(_questCode, _entityID, _position, _completeEvent);
      this.BlockActivateQuestDictionary.Add(_questCode, restorePowerQuestData);
    }
    else
      this.BlockActivateQuestDictionary[_questCode].AddSharedQuester(_entityID);
  }

  public void FinishManagedQuest(int _questCode, EntityPlayer _player)
  {
    if (!this.BlockActivateQuestDictionary.ContainsKey(_questCode))
      return;
    this.BlockActivateQuestDictionary[_questCode].RemoveSharedQuester(_player);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  static QuestEventManager()
  {
  }

  public enum POILockoutReasonTypes
  {
    None,
    PlayerInside,
    Bedroll,
    LandClaim,
    QuestLock,
  }

  public class PrefabListData
  {
    public Dictionary<int, List<PrefabInstance>> TierData = new Dictionary<int, List<PrefabInstance>>();

    public void AddPOI(PrefabInstance poi)
    {
      int difficultyTier = (int) poi.prefab.DifficultyTier;
      if (!this.TierData.ContainsKey(difficultyTier))
        this.TierData.Add(difficultyTier, new List<PrefabInstance>());
      this.TierData[difficultyTier].Add(poi);
    }

    public void ShuffleDifficulty(int difficulty, GameRandom gameRandom)
    {
      if (!this.TierData.ContainsKey(difficulty))
        return;
      List<PrefabInstance> prefabInstanceList = this.TierData[difficulty];
      for (int index1 = 0; index1 < prefabInstanceList.Count * 2; ++index1)
      {
        int index2 = gameRandom.RandomRange(prefabInstanceList.Count);
        int index3 = gameRandom.RandomRange(prefabInstanceList.Count);
        PrefabInstance prefabInstance = prefabInstanceList[index3];
        prefabInstanceList[index3] = prefabInstanceList[index2];
        prefabInstanceList[index2] = prefabInstance;
      }
    }
  }
}
