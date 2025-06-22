// Decompiled with JetBrains decompiler
// Type: EntityClass
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
[Preserve]
public class EntityClass
{
  public static string PropEntityFlags = "EntityFlags";
  public static string PropEntityType = "EntityType";
  public static string PropClass = "Class";
  public static string PropCensor = "Censor";
  public static string PropMesh = "Mesh";
  public static string PropMeshFP = "MeshFP";
  public static string PropPrefab = "Prefab";
  public static string PropPrefabCombined = "PrefabCombined";
  public static string PropParent = "Parent";
  public static string PropAvatarController = "AvatarController";
  public static string PropLocalAvatarController = "LocalAvatarController";
  public static string PropSkinTexture = "SkinTexture";
  public static string PropAltMats = "AltMats";
  public static string PropSwapMats = "SwapMats";
  public static string PropMatColor = "MatColor";
  public static string PropRightHandJointName = "RightHandJointName";
  public static string PropHandItem = "HandItem";
  public static string PropHandItemCrawler = "HandItemCrawler";
  public static string PropMaxHealth = "MaxHealth";
  public static string PropMaxStamina = "MaxStamina";
  public static string PropSickness = "Sickness";
  public static string PropGassiness = "Gassiness";
  public static string PropWellness = "Wellness";
  public static string PropFood = "Food";
  public static string PropWater = "Water";
  public static string PropMaxViewAngle = "MaxViewAngle";
  public static string PropWeight = "Weight";
  public static string PropPushFactor = "PushFactor";
  public static string PropTimeStayAfterDeath = "TimeStayAfterDeath";
  public static string PropImmunity = "Immunity";
  public static string PropIsMale = "IsMale";
  public static string PropIsChunkObserver = "IsChunkObserver";
  public static string PropAIFeralSense = "AIFeralSense";
  public static string PropAIGroupCircle = "AIGroupCircle";
  public static string PropAINoiseSeekDist = "AINoiseSeekDist";
  public static string PropAISeeOffset = "AISeeOffset";
  public static string PropAIPathCostScale = "AIPathCostScale";
  public static string PropAITask = "AITask-";
  public static string PropAITargetTask = "AITarget-";
  public static string PropMoveSpeed = "MoveSpeed";
  public static string PropMoveSpeedNight = "MoveSpeedNight";
  public static string PropMoveSpeedAggro = "MoveSpeedAggro";
  public static string PropMoveSpeedRand = "MoveSpeedRand";
  public static string PropMoveSpeedPanic = "MoveSpeedPanic";
  public static string PropMoveSpeedPattern = "MoveSpeedPattern";
  public static string PropSwimSpeed = "SwimSpeed";
  public static string PropSwimStrokeRate = "SwimStrokeRate";
  public static string PropCrouchType = "CrouchType";
  public static string PropDanceType = "DanceType";
  public static string PropWalkType = "WalkType";
  public static string PropCanClimbVertical = "CanClimbVertical";
  public static string PropCanClimbLadders = "CanClimbLadders";
  public static string PropJumpDelay = "JumpDelay";
  public static string PropJumpMaxDistance = "JumpMaxDistance";
  public static string PropIsEnemyEntity = "IsEnemyEntity";
  public static string PropIsAnimalEntity = "IsAnimalEntity";
  public static string PropSoundRandomTime = "SoundRandomTime";
  public static string PropSoundAlertTime = "SoundAlertTime";
  public static string PropSoundRandom = "SoundRandom";
  public static string PropSoundHurt = "SoundHurt";
  public static string PropSoundJump = "SoundJump";
  public static string PropSoundHurtSmall = "SoundHurtSmall";
  public static string PropSoundDrownPain = "SoundDrownPain";
  public static string PropSoundDrownDeath = "SoundDrownDeath";
  public static string PropSoundWaterSurface = "SoundWaterSurface";
  public static string PropSoundDeath = "SoundDeath";
  public static string PropSoundAttack = "SoundAttack";
  public static string PropSoundAlert = "SoundAlert";
  public static string PropSoundSense = "SoundSense";
  public static string PropSoundStamina = "SoundStamina";
  public static string PropSoundLiving = "SoundLiving";
  public static string PropSoundSpawn = "SoundSpawn";
  public static string PropSoundLand = "SoundLanding";
  public static string PropSoundStepType = "SoundStepType";
  public static string PropSoundGiveUp = "SoundGiveUp";
  public static string PropSoundExplodeWarn = "SoundExplodeWarn";
  public static string PropSoundTick = "SoundTick";
  public static string PropExplodeDelay = "ExplodeDelay";
  public static string PropExplodeHealthThreshold = "ExplodeHealthThreshold";
  public static string PropLootListOnDeath = "LootListOnDeath";
  public static string PropLootListAlive = "LootListAlive";
  public static string PropLootDropProb = "LootDropProb";
  public static string PropLootDropEntityClass = "LootDropEntityClass";
  public static string PropAttackTimeoutDay = "AttackTimeoutDay";
  public static string PropAttackTimeoutNight = "AttackTimeoutNight";
  public static string PropMapIcon = "MapIcon";
  public static string PropCompassIcon = "CompassIcon";
  public static string PropTrackerIcon = "TrackerIcon";
  public static string PropCompassUpIcon = "CompassUpIcon";
  public static string PropCompassDownIcon = "CompassDownIcon";
  public static string PropParticleOnSpawn = "ParticleOnSpawn";
  public static string PropParticleOnDeath = "ParticleOnDeath";
  public static string PropParticleOnDestroy = "ParticleOnDestroy";
  public static string PropItemsOnEnterGame = "ItemsOnEnterGame";
  public static string PropFallLandBehavior = "FallLandBehavior";
  public static string PropDestroyBlockBehavior = "DestroyBlockBehavior";
  public static string PropDropInventoryBlock = "DropInventoryBlock";
  public static string PropModelType = "ModelType";
  public static string PropRagdollOnDeathChance = nameof (RagdollOnDeathChance);
  public static string PropHasRagdoll = nameof (HasRagdoll);
  public static string PropMass = "Mass";
  public static string PropSizeScale = nameof (SizeScale);
  public static string PropPhysicsBody = nameof (PhysicsBody);
  public static string PropColliders = "Colliders";
  public static string PropLookAtAngle = nameof (LookAtAngle);
  public static string PropCrouchYOffsetFP = "CrouchYOffsetFP";
  public static string PropRotateToGround = "RotateToGround";
  public static string PropCorpseBlock = "CorpseBlock";
  public static string PropCorpseBlockChance = nameof (CorpseBlockChance);
  public static string PropCorpseBlockDensity = nameof (CorpseBlockDensity);
  public static string PropRootMotion = nameof (RootMotion);
  public static string PropExperienceGain = "ExperienceGain";
  public static string PropHasDeathAnim = nameof (HasDeathAnim);
  public static string PropLegCrippleScale = nameof (LegCrippleScale);
  public static string PropLegCrawlerThreshold = nameof (LegCrawlerThreshold);
  public static string PropDismemberMultiplierHead = nameof (DismemberMultiplierHead);
  public static string PropDismemberMultiplierArms = nameof (DismemberMultiplierArms);
  public static string PropDismemberMultiplierLegs = nameof (DismemberMultiplierLegs);
  public static string PropKnockdownKneelDamageThreshold = nameof (KnockdownKneelDamageThreshold);
  public static string PropKnockdownKneelStunDuration = nameof (KnockdownKneelStunDuration);
  public static string PropKnockdownProneDamageThreshold = nameof (KnockdownProneDamageThreshold);
  public static string PropKnockdownProneStunDuration = nameof (KnockdownProneStunDuration);
  public static string PropKnockdownProneRefillRate = nameof (KnockdownProneRefillRate);
  public static string PropKnockdownKneelRefillRate = nameof (KnockdownKneelRefillRate);
  public static string PropArmsExplosionDamageMultiplier = nameof (ArmsExplosionDamageMultiplier);
  public static string PropLegsExplosionDamageMultiplier = nameof (LegsExplosionDamageMultiplier);
  public static string PropChestExplosionDamageMultiplier = nameof (ChestExplosionDamageMultiplier);
  public static string PropHeadExplosionDamageMultiplier = nameof (HeadExplosionDamageMultiplier);
  public static string PropPainResistPerHit = nameof (PainResistPerHit);
  public static string PropArchetype = "Archetype";
  public static string PropSwimOffset = nameof (SwimOffset);
  public static string PropUMARace = nameof (UMARace);
  public static string PropUMAGeneratedModelName = nameof (UMAGeneratedModelName);
  public static string PropNPCID = "NPCID";
  public static string PropModelTransformAdjust = nameof (ModelTransformAdjust);
  public static string PropAIPackages = nameof (AIPackages);
  public static string PropBuffs = nameof (Buffs);
  public static string PropStealthSoundDecayRate = "StealthSoundDecayRate";
  public static string PropSightRange = nameof (SightRange);
  public static string PropSightLightThreshold = "SightLightThreshold";
  public static string PropSleeperSightToSenseMin = nameof (SleeperSightToSenseMin);
  public static string PropSleeperSightToSenseMax = nameof (SleeperSightToSenseMax);
  public static string PropSleeperSightToWakeMin = nameof (SleeperSightToWakeMin);
  public static string PropSleeperSightToWakeMax = nameof (SleeperSightToWakeMax);
  public static string PropSleeperNoiseToSense = nameof (SleeperNoiseToSense);
  public static string PropSleeperNoiseToSenseSoundChance = nameof (SleeperNoiseToSenseSoundChance);
  public static string PropSleeperNoiseToWake = nameof (SleeperNoiseToWake);
  public static string PropSoundSleeperSense = "SoundSleeperSense";
  public static string PropSoundSleeperSnore = "SoundSleeperBackToSleep";
  public static string PropMaxTurnSpeed = nameof (MaxTurnSpeed);
  public static string PropSearchRadius = nameof (SearchRadius);
  public static string PropTags = nameof (Tags);
  public static string PropNavObject = nameof (NavObject);
  public static string PropNavObjectHeadOffset = nameof (NavObjectHeadOffset);
  public static string PropStompsSpikes = "StompsSpikes";
  public static string PropUserSpawnType = "UserSpawnType";
  public static string PropHideInSpawnMenu = "HideInSpawnMenu";
  public static string PropCanBigHead = nameof (CanBigHead);
  public static string PropOnActivateEvent = "ActivateEvent";
  public static readonly int itemClass = EntityClass.FromString("item");
  public static readonly int fallingBlockClass = EntityClass.FromString("fallingBlock");
  public static readonly int fallingTreeClass = EntityClass.FromString("fallingTree");
  public static readonly int playerMaleClass = EntityClass.FromString("playerMale");
  public static readonly int playerFemaleClass = EntityClass.FromString("playerFemale");
  public static readonly int playerNewMaleClass = EntityClass.FromString("playerNewMale");
  public static Dictionary<string, Color> sColors = new Dictionary<string, Color>();
  public static DictionarySave<int, EntityClass> list = new DictionarySave<int, EntityClass>();
  public DynamicProperties Properties = new DynamicProperties();
  public System.Type classname;
  public int censorMode;
  public EntityFlags entityFlags;
  public int censorType;
  public string prefabPath;
  public Transform prefabT;
  public bool IsPrefabCombined;
  public string meshPath;
  public Transform mesh;
  public Transform meshFP;
  public string skinTexture;
  public string parentGameObjectName;
  public string entityClassName;
  public EntityClass.UserSpawnType userSpawnType = EntityClass.UserSpawnType.Menu;
  public bool bIsEnemyEntity;
  public bool bIsAnimalEntity;
  public ExplosionData explosionData;
  public System.Type modelType;
  public float MassKg;
  public float SizeScale;
  public float RagdollOnDeathChance;
  public bool HasRagdoll;
  public string CollidersRagdollAsset;
  public float LookAtAngle;
  public float crouchYOffsetFP;
  public string CorpseBlockId;
  public float CorpseBlockChance;
  public int CorpseBlockDensity;
  public float MaxTurnSpeed;
  public bool RootMotion;
  public bool HasDeathAnim;
  public bool bIsMale;
  public bool bIsChunkObserver;
  public int ExperienceValue;
  public int lootDropEntityClass;
  public PhysicsBodyLayout PhysicsBody;
  public int DeadBodyHitPoints;
  public float LegCrippleScale;
  public float LegCrawlerThreshold;
  public float DismemberMultiplierHead;
  public float DismemberMultiplierArms;
  public float DismemberMultiplierLegs;
  public float LowerLegDismemberThreshold;
  public float LowerLegDismemberBonusChance;
  public float LowerLegDismemberBaseChance;
  public float UpperLegDismemberThreshold;
  public float UpperLegDismemberBonusChance;
  public float UpperLegDismemberBaseChance;
  public float LowerArmDismemberThreshold;
  public float LowerArmDismemberBonusChance;
  public float LowerArmDismemberBaseChance;
  public float UpperArmDismemberThreshold;
  public float UpperArmDismemberBonusChance;
  public float UpperArmDismemberBaseChance;
  public float KnockdownKneelDamageThreshold;
  public float LegsExplosionDamageMultiplier;
  public float ArmsExplosionDamageMultiplier;
  public float ChestExplosionDamageMultiplier;
  public float HeadExplosionDamageMultiplier;
  public float PainResistPerHit;
  public float PainResistPerHitLowHealth;
  public float PainResistPerHitLowHealthPercent;
  public float SearchRadius;
  public float SwimOffset;
  public float SightRange;
  public Vector2 SleeperSightToSenseMin;
  public Vector2 SleeperSightToSenseMax;
  public Vector2 SleeperSightToWakeMin;
  public Vector2 SleeperSightToWakeMax;
  public Vector2 sightLightThreshold;
  public Vector2 NoiseAlert;
  public Vector2 SleeperNoiseToSense;
  public float SleeperNoiseToSenseSoundChance;
  public Vector2 SleeperNoiseToWake;
  public string UMARace;
  public string UMAGeneratedModelName;
  public string[] AltMatNames;
  public string[] MatSwap;
  public EntityClass.ParticleData particleOnSpawn;
  public Vector2 KnockdownKneelStunDuration;
  public float KnockdownProneDamageThreshold;
  public Vector2 KnockdownProneStunDuration;
  public Vector2 KnockdownProneRefillRate;
  public Vector2 KnockdownKneelRefillRate;
  public Vector3 ModelTransformAdjust;
  public string ArchetypeName;
  public string[] AIPackages;
  public bool UseAIPackages;
  public Dictionary<EnumDropEvent, List<Block.SItemDropProb>> itemsToDrop = (Dictionary<EnumDropEvent, List<Block.SItemDropProb>>) new EnumDictionary<EnumDropEvent, List<Block.SItemDropProb>>();
  public List<string> Buffs;
  public FastTags<TagGroup.Global> Tags;
  public string NavObject = "";
  public Vector3 NavObjectHeadOffset = Vector3.zero;
  public bool CanBigHead = true;
  public int DanceTypeID;
  public MinEffectController Effects;
  [PublicizedFrom(EAccessModifier.Private)]
  public static readonly char[] commaSeparator = new char[1]
  {
    ','
  };
  public string onActivateEvent = "";

  public static void Add(string _entityClassname, EntityClass _entityClass)
  {
    _entityClass.entityClassName = _entityClassname;
    EntityClass.list[_entityClassname.GetHashCode()] = _entityClass;
  }

  public static EntityClass GetEntityClass(int entityClass)
  {
    EntityClass entityClass1;
    EntityClass.list.TryGetValue(entityClass, out entityClass1);
    return entityClass1;
  }

  public static string GetEntityClassName(int entityClass)
  {
    EntityClass entityClass1;
    return EntityClass.list.TryGetValue(entityClass, out entityClass1) ? entityClass1.entityClassName : "null";
  }

  public static int GetId(string _name)
  {
    foreach (KeyValuePair<int, EntityClass> keyValuePair in EntityClass.list.Dict)
    {
      if (keyValuePair.Value.entityClassName == _name)
        return keyValuePair.Key;
    }
    return -1;
  }

  public static int FromString(string _s) => _s.GetHashCode();

  public EntityClass Init()
  {
    this.censorType = 1;
    string str1 = "";
    if (this.Properties.Contains(EntityClass.PropCensor))
      str1 = this.Properties.GetStringValue(EntityClass.PropCensor);
    if (!string.IsNullOrEmpty(str1) && str1.Contains(","))
    {
      string[] strArray = str1.Split(",", StringSplitOptions.None);
      if (strArray.Length > 1)
      {
        StringParsers.TryParseSInt32(strArray[0], out this.censorMode);
        StringParsers.TryParseSInt32(strArray[1], out this.censorType);
      }
    }
    else
      this.Properties.ParseInt(EntityClass.PropCensor, ref this.censorMode);
    if (!this.Properties.Values.TryGetValue(EntityClass.PropPrefab, out this.prefabPath) || this.prefabPath.Length == 0)
      throw new Exception($"Mandatory property 'prefab' missing in entity_class '{this.entityClassName}'");
    string str2;
    bool result;
    if (((!this.Properties.Values.TryGetValue(EntityClass.PropPrefabCombined, out str2) ? 0 : (bool.TryParse(str2, out result) ? 1 : 0)) & (result ? 1 : 0)) != 0)
      this.IsPrefabCombined = true;
    else if (this.prefabPath[0] == '/')
    {
      this.prefabPath = this.prefabPath.Substring(1);
      this.IsPrefabCombined = true;
    }
    else if (DataLoader.IsInResources(this.prefabPath))
      this.prefabPath = "Prefabs/prefabEntity" + this.prefabPath;
    string _uri1;
    if (this.Properties.Values.TryGetValue(EntityClass.PropMesh, out _uri1) && _uri1.Length > 0)
    {
      if (this.censorMode != 0 && (this.censorType == 1 || this.censorType == 3) && Object.op_Implicit((Object) GameManager.Instance) && GameManager.Instance.IsGoreCensored())
        _uri1 = _uri1.Replace(".", "_CGore.");
      if (DataLoader.IsInResources(_uri1))
        _uri1 = "Entities/" + _uri1;
      this.meshPath = _uri1;
    }
    if (this.Properties.Values.ContainsKey(EntityClass.PropMeshFP))
    {
      string _uri2 = this.Properties.Values[EntityClass.PropMeshFP];
      if (DataLoader.IsInResources(_uri2))
        _uri2 = "Entities/" + _uri2;
      this.meshFP = DataLoader.LoadAsset<Transform>(_uri2);
      if (Object.op_Equality((Object) this.meshFP, (Object) null))
        Log.Error($"Could not load file '{_uri2}' for entity_class '{this.entityClassName}'");
    }
    this.entityFlags = EntityFlags.None;
    EntityClass.ParseEntityFlags(this.Properties.GetString(EntityClass.PropEntityFlags), ref this.entityFlags);
    if (this.Properties.Values.ContainsKey(EntityClass.PropClass))
    {
      this.classname = System.Type.GetType(this.Properties.Values[EntityClass.PropClass]);
      if (this.classname == (System.Type) null)
        Log.Error($"Could not instantiate class{this.Properties.Values[EntityClass.PropClass]}' for entity_class '{this.entityClassName}'");
    }
    this.modelType = typeof (EModelCustom);
    string _name1 = this.Properties.GetString(EntityClass.PropModelType);
    if (_name1.Length > 0)
    {
      this.modelType = ReflectionHelpers.GetTypeWithPrefix("EModel", _name1);
      if (this.modelType == (System.Type) null)
        throw new Exception($"Model class '{_name1}' not found!");
    }
    string str3 = this.Properties.GetString(EntityClass.PropAltMats);
    if (str3.Length > 0)
      this.AltMatNames = str3.Split(',', StringSplitOptions.None);
    string str4 = this.Properties.GetString(EntityClass.PropSwapMats);
    if (str4.Length > 0)
      this.MatSwap = str4.Split(",", StringSplitOptions.None);
    if (this.Properties.Values.ContainsKey(EntityClass.PropParticleOnSpawn))
    {
      this.particleOnSpawn.fileName = this.Properties.Values[EntityClass.PropParticleOnSpawn];
      this.particleOnSpawn.shapeMesh = this.Properties.Params1[EntityClass.PropParticleOnSpawn];
      DataLoader.PreloadBundle(this.particleOnSpawn.fileName);
    }
    this.RagdollOnDeathChance = 0.5f;
    if (this.Properties.Values.ContainsKey(EntityClass.PropRagdollOnDeathChance))
      this.RagdollOnDeathChance = StringParsers.ParseFloat(this.Properties.Values[EntityClass.PropRagdollOnDeathChance]);
    if (this.Properties.Values.ContainsKey(EntityClass.PropHasRagdoll))
      this.HasRagdoll = StringParsers.ParseBool(this.Properties.Values[EntityClass.PropHasRagdoll]);
    if (this.Properties.Values.ContainsKey(EntityClass.PropColliders))
    {
      this.CollidersRagdollAsset = this.Properties.Values[EntityClass.PropColliders];
      DataLoader.PreloadBundle(this.CollidersRagdollAsset);
    }
    this.Properties.ParseFloat(EntityClass.PropLookAtAngle, ref this.LookAtAngle);
    if (this.Properties.Values.ContainsKey(EntityClass.PropCrouchYOffsetFP))
      this.crouchYOffsetFP = StringParsers.ParseFloat(this.Properties.Values[EntityClass.PropCrouchYOffsetFP]);
    if (this.Properties.Values.ContainsKey(EntityClass.PropParent))
      this.parentGameObjectName = this.Properties.Values[EntityClass.PropParent];
    if (this.Properties.Values.ContainsKey(EntityClass.PropSkinTexture))
    {
      this.skinTexture = this.Properties.Values[EntityClass.PropSkinTexture];
      DataLoader.PreloadBundle(this.skinTexture);
    }
    this.bIsEnemyEntity = false;
    if (this.Properties.Values.ContainsKey(EntityClass.PropIsEnemyEntity))
      this.bIsEnemyEntity = StringParsers.ParseBool(this.Properties.Values[EntityClass.PropIsEnemyEntity]);
    this.bIsAnimalEntity = false;
    if (this.Properties.Values.ContainsKey(EntityClass.PropIsAnimalEntity))
      this.bIsAnimalEntity = StringParsers.ParseBool(this.Properties.Values[EntityClass.PropIsAnimalEntity]);
    this.CorpseBlockId = (string) null;
    if (this.Properties.Values.ContainsKey(EntityClass.PropCorpseBlock))
      this.CorpseBlockId = this.Properties.Values[EntityClass.PropCorpseBlock];
    this.CorpseBlockChance = 1f;
    if (this.Properties.Values.ContainsKey(EntityClass.PropCorpseBlockChance))
      this.CorpseBlockChance = StringParsers.ParseFloat(this.Properties.Values[EntityClass.PropCorpseBlockChance]);
    this.CorpseBlockDensity = (int) MarchingCubes.DensityTerrain;
    if (this.Properties.Values.ContainsKey(EntityClass.PropCorpseBlockDensity))
    {
      this.CorpseBlockDensity = int.Parse(this.Properties.Values[EntityClass.PropCorpseBlockDensity]);
      this.CorpseBlockDensity = Math.Max((int) sbyte.MinValue, Math.Min((int) sbyte.MaxValue, this.CorpseBlockDensity));
    }
    this.RootMotion = false;
    if (this.Properties.Values.ContainsKey(EntityClass.PropRootMotion))
      this.RootMotion = StringParsers.ParseBool(this.Properties.Values[EntityClass.PropRootMotion]);
    this.HasDeathAnim = false;
    if (this.Properties.Values.ContainsKey(EntityClass.PropHasDeathAnim))
      this.HasDeathAnim = StringParsers.ParseBool(this.Properties.Values[EntityClass.PropHasDeathAnim]);
    this.ExperienceValue = 100;
    if (this.Properties.Values.ContainsKey(EntityClass.PropExperienceGain))
      this.ExperienceValue = (int) StringParsers.ParseFloat(this.Properties.Values[EntityClass.PropExperienceGain]);
    string _s = this.Properties.GetString(EntityClass.PropLootDropEntityClass);
    if (_s.Length > 0)
      this.lootDropEntityClass = EntityClass.FromString(_s);
    this.bIsMale = false;
    if (this.Properties.Values.ContainsKey(EntityClass.PropIsMale))
      this.bIsMale = StringParsers.ParseBool(this.Properties.Values[EntityClass.PropIsMale]);
    this.bIsChunkObserver = false;
    if (this.Properties.Values.ContainsKey(EntityClass.PropIsChunkObserver))
      this.bIsChunkObserver = StringParsers.ParseBool(this.Properties.Values[EntityClass.PropIsChunkObserver]);
    this.SightRange = Constants.cDefaultMonsterSeeDistance;
    if (this.Properties.Values.ContainsKey(EntityClass.PropSightRange))
      this.SightRange = StringParsers.ParseFloat(this.Properties.Values[EntityClass.PropSightRange]);
    this.sightLightThreshold = !this.Properties.Values.ContainsKey(EntityClass.PropSightLightThreshold) ? new Vector2(30f, 100f) : StringParsers.ParseMinMaxCount(this.Properties.Values[EntityClass.PropSightLightThreshold]);
    this.SleeperNoiseToSense = new Vector2(15f, 15f);
    this.Properties.ParseVec(EntityClass.PropSleeperNoiseToSense, ref this.SleeperNoiseToSense);
    this.SleeperNoiseToSenseSoundChance = 1f;
    this.Properties.ParseFloat(EntityClass.PropSleeperNoiseToSenseSoundChance, ref this.SleeperNoiseToSenseSoundChance);
    this.SleeperNoiseToWake = new Vector2(15f, 15f);
    this.Properties.ParseVec(EntityClass.PropSleeperNoiseToWake, ref this.SleeperNoiseToWake);
    this.SleeperSightToSenseMin = new Vector2(25f, 25f);
    this.Properties.ParseVec(EntityClass.PropSleeperSightToSenseMin, ref this.SleeperSightToSenseMin);
    this.SleeperSightToSenseMax = new Vector2(200f, 200f);
    this.Properties.ParseVec(EntityClass.PropSleeperSightToSenseMax, ref this.SleeperSightToSenseMax);
    this.SleeperSightToWakeMin = new Vector2(15f, 15f);
    this.Properties.ParseVec(EntityClass.PropSleeperSightToWakeMin, ref this.SleeperSightToWakeMin);
    this.SleeperSightToWakeMax = new Vector2(200f, 200f);
    this.Properties.ParseVec(EntityClass.PropSleeperSightToWakeMax, ref this.SleeperSightToWakeMax);
    this.MassKg = 10f;
    if (this.Properties.Values.ContainsKey(EntityClass.PropMass))
      this.MassKg = StringParsers.ParseFloat(this.Properties.Values[EntityClass.PropMass]);
    this.MassKg *= 0.454f;
    this.SizeScale = 1f;
    if (this.Properties.Values.ContainsKey(EntityClass.PropSizeScale))
      this.SizeScale = StringParsers.ParseFloat(this.Properties.Values[EntityClass.PropSizeScale]);
    string _name2 = this.Properties.GetString(EntityClass.PropPhysicsBody);
    if (_name2.Length > 0)
      this.PhysicsBody = PhysicsBodyLayout.Find(_name2);
    if (this.Properties.Values.ContainsKey("DeadBodyHitPoints"))
      this.DeadBodyHitPoints = int.Parse(this.Properties.Values["DeadBodyHitPoints"]);
    this.Properties.ParseFloat(EntityClass.PropLegCrippleScale, ref this.LegCrippleScale);
    this.Properties.ParseFloat(EntityClass.PropLegCrawlerThreshold, ref this.LegCrawlerThreshold);
    this.DismemberMultiplierHead = 1f;
    this.Properties.ParseFloat(EntityClass.PropDismemberMultiplierHead, ref this.DismemberMultiplierHead);
    this.DismemberMultiplierArms = 1f;
    this.Properties.ParseFloat(EntityClass.PropDismemberMultiplierArms, ref this.DismemberMultiplierArms);
    this.DismemberMultiplierLegs = 1f;
    this.Properties.ParseFloat(EntityClass.PropDismemberMultiplierLegs, ref this.DismemberMultiplierLegs);
    if (this.Properties.Values.ContainsKey(EntityClass.PropKnockdownKneelDamageThreshold))
      this.KnockdownKneelDamageThreshold = StringParsers.ParseFloat(this.Properties.Values[EntityClass.PropKnockdownKneelDamageThreshold]);
    if (this.Properties.Values.ContainsKey(EntityClass.PropKnockdownKneelStunDuration))
      this.KnockdownKneelStunDuration = StringParsers.ParseMinMaxCount(this.Properties.Values[EntityClass.PropKnockdownKneelStunDuration]);
    if (this.Properties.Values.ContainsKey(EntityClass.PropKnockdownProneDamageThreshold))
      this.KnockdownProneDamageThreshold = StringParsers.ParseFloat(this.Properties.Values[EntityClass.PropKnockdownProneDamageThreshold]);
    if (this.Properties.Values.ContainsKey(EntityClass.PropKnockdownProneStunDuration))
      this.KnockdownProneStunDuration = StringParsers.ParseMinMaxCount(this.Properties.Values[EntityClass.PropKnockdownProneStunDuration]);
    if (this.Properties.Values.ContainsKey(EntityClass.PropKnockdownKneelRefillRate))
      this.KnockdownKneelRefillRate = StringParsers.ParseMinMaxCount(this.Properties.Values[EntityClass.PropKnockdownKneelRefillRate]);
    if (this.Properties.Values.ContainsKey(EntityClass.PropKnockdownProneRefillRate))
      this.KnockdownProneRefillRate = StringParsers.ParseMinMaxCount(this.Properties.Values[EntityClass.PropKnockdownProneRefillRate]);
    this.LegsExplosionDamageMultiplier = 1f;
    if (this.Properties.Values.ContainsKey(EntityClass.PropLegsExplosionDamageMultiplier))
      this.LegsExplosionDamageMultiplier = StringParsers.ParseFloat(this.Properties.Values[EntityClass.PropLegsExplosionDamageMultiplier]);
    this.ArmsExplosionDamageMultiplier = 1f;
    if (this.Properties.Values.ContainsKey(EntityClass.PropArmsExplosionDamageMultiplier))
      this.ArmsExplosionDamageMultiplier = StringParsers.ParseFloat(this.Properties.Values[EntityClass.PropArmsExplosionDamageMultiplier]);
    this.HeadExplosionDamageMultiplier = 1f;
    if (this.Properties.Values.ContainsKey(EntityClass.PropHeadExplosionDamageMultiplier))
      this.HeadExplosionDamageMultiplier = StringParsers.ParseFloat(this.Properties.Values[EntityClass.PropHeadExplosionDamageMultiplier]);
    this.ChestExplosionDamageMultiplier = 1f;
    if (this.Properties.Values.ContainsKey(EntityClass.PropChestExplosionDamageMultiplier))
      this.ChestExplosionDamageMultiplier = StringParsers.ParseFloat(this.Properties.Values[EntityClass.PropChestExplosionDamageMultiplier]);
    Vector3 zero = Vector3.zero;
    this.Properties.ParseVec(EntityClass.PropPainResistPerHit, ref zero, 0.0f);
    this.PainResistPerHit = zero.x;
    this.PainResistPerHitLowHealth = zero.y;
    this.PainResistPerHitLowHealthPercent = zero.z;
    if (this.Properties.Values.ContainsKey(EntityClass.PropArchetype))
      this.ArchetypeName = this.Properties.Values[EntityClass.PropArchetype];
    this.SwimOffset = 0.9f;
    if (this.Properties.Values.ContainsKey(EntityClass.PropSwimOffset))
      this.SwimOffset = StringParsers.ParseFloat(this.Properties.Values[EntityClass.PropSwimOffset]);
    this.SearchRadius = 6f;
    this.Properties.ParseFloat(EntityClass.PropSearchRadius, ref this.SearchRadius);
    if (this.Properties.Values.ContainsKey(EntityClass.PropUMARace))
      this.UMARace = this.Properties.Values[EntityClass.PropUMARace];
    if (this.Properties.Values.ContainsKey(EntityClass.PropUMAGeneratedModelName))
      this.UMAGeneratedModelName = this.Properties.Values[EntityClass.PropUMAGeneratedModelName];
    if (this.Properties.Values.ContainsKey(EntityClass.PropModelTransformAdjust))
      this.ModelTransformAdjust = StringParsers.ParseVector3(this.Properties.Values[EntityClass.PropModelTransformAdjust]);
    if (this.Properties.Values.ContainsKey(EntityClass.PropAIPackages))
    {
      this.AIPackages = this.Properties.Values[EntityClass.PropAIPackages].Split(',', StringSplitOptions.None);
      for (int index = 0; index < this.AIPackages.Length; ++index)
        this.AIPackages[index] = this.AIPackages[index].Trim();
      this.UseAIPackages = true;
    }
    if (this.Properties.Values.ContainsKey(EntityClass.PropBuffs))
    {
      string[] collection = this.Properties.Values[EntityClass.PropBuffs].Split(new char[1]
      {
        ';'
      }, StringSplitOptions.RemoveEmptyEntries);
      if (collection.Length != 0)
        this.Buffs = new List<string>((IEnumerable<string>) collection);
    }
    if (this.Properties.Values.ContainsKey(EntityClass.PropMaxTurnSpeed))
      this.MaxTurnSpeed = StringParsers.ParseFloat(this.Properties.Values[EntityClass.PropMaxTurnSpeed]);
    if (this.Properties.Values.ContainsKey(EntityClass.PropTags))
      this.Tags = FastTags<TagGroup.Global>.Parse(this.Properties.Values[EntityClass.PropTags]);
    if (this.Properties.Values.ContainsKey(EntityClass.PropNavObject))
      this.NavObject = this.Properties.Values[EntityClass.PropNavObject];
    this.Properties.ParseVec(EntityClass.PropNavObjectHeadOffset, ref this.NavObjectHeadOffset);
    this.explosionData = new ExplosionData(this.Properties, this.Effects);
    bool optionalValue = false;
    this.Properties.ParseBool(EntityClass.PropHideInSpawnMenu, ref optionalValue);
    if (optionalValue)
      this.userSpawnType = EntityClass.UserSpawnType.Console;
    this.Properties.ParseEnum<EntityClass.UserSpawnType>(EntityClass.PropUserSpawnType, ref this.userSpawnType);
    this.Properties.ParseBool(EntityClass.PropCanBigHead, ref this.CanBigHead);
    this.Properties.ParseInt(EntityClass.PropDanceType, ref this.DanceTypeID);
    this.Properties.ParseString(EntityClass.PropOnActivateEvent, ref this.onActivateEvent);
    return this;
  }

  public void CopyFrom(EntityClass _other, HashSet<string> _exclude = null)
  {
    foreach (KeyValuePair<string, string> keyValuePair in _other.Properties.Values.Dict)
    {
      if (_exclude == null || !_exclude.Contains(keyValuePair.Key))
        this.Properties.Values[keyValuePair.Key] = _other.Properties.Values[keyValuePair.Key];
    }
    foreach (KeyValuePair<string, string> keyValuePair in _other.Properties.Params1.Dict)
    {
      if (_exclude == null || !_exclude.Contains(keyValuePair.Key))
        this.Properties.Params1[keyValuePair.Key] = keyValuePair.Value;
    }
    foreach (KeyValuePair<string, string> keyValuePair in _other.Properties.Params2.Dict)
    {
      if (_exclude == null || !_exclude.Contains(keyValuePair.Key))
        this.Properties.Params2[keyValuePair.Key] = keyValuePair.Value;
    }
    foreach (KeyValuePair<string, string> keyValuePair in _other.Properties.Data.Dict)
    {
      if (_exclude == null || !_exclude.Contains(keyValuePair.Key))
        this.Properties.Data[keyValuePair.Key] = keyValuePair.Value;
    }
    foreach (KeyValuePair<string, DynamicProperties> keyValuePair in _other.Properties.Classes.Dict)
    {
      if (_exclude == null || !_exclude.Contains(keyValuePair.Key))
      {
        DynamicProperties dynamicProperties = new DynamicProperties();
        dynamicProperties.CopyFrom(keyValuePair.Value);
        this.Properties.Classes[keyValuePair.Key] = dynamicProperties;
      }
    }
  }

  public static void ParseEntityFlags(string _names, ref EntityFlags optionalValue)
  {
    if (_names.Length <= 0)
      return;
    if (_names.IndexOf(',') >= 0)
    {
      foreach (string _name in _names.Split(EntityClass.commaSeparator, StringSplitOptions.RemoveEmptyEntries))
      {
        EntityFlags _result;
        if (EnumUtils.TryParse<EntityFlags>(_name, out _result, true))
          optionalValue |= _result;
      }
    }
    else
    {
      EntityFlags _result;
      if (!EnumUtils.TryParse<EntityFlags>(_names, out _result, true))
        return;
      optionalValue = _result;
    }
  }

  public static void Cleanup() => EntityClass.list.Clear();

  public void AddDroppedId(
    EnumDropEvent _eEvent,
    string _name,
    int _minCount,
    int _maxCount,
    float _prob,
    float _stickChance,
    string _toolCategory,
    string _tag)
  {
    List<Block.SItemDropProb> sitemDropProbList = this.itemsToDrop.ContainsKey(_eEvent) ? this.itemsToDrop[_eEvent] : (List<Block.SItemDropProb>) null;
    if (sitemDropProbList == null)
    {
      sitemDropProbList = new List<Block.SItemDropProb>();
      this.itemsToDrop[_eEvent] = sitemDropProbList;
    }
    sitemDropProbList.Add(new Block.SItemDropProb(_name, _minCount, _maxCount, _prob, 1f, _stickChance, _toolCategory, _tag));
  }

  [PublicizedFrom(EAccessModifier.Private)]
  static EntityClass()
  {
  }

  public enum CensorModeType
  {
    None,
    ZPrefab,
    Dismemberment,
    ZPrefabAndDismemberment,
  }

  public enum UserSpawnType
  {
    None,
    Console,
    Menu,
  }

  public struct ParticleData
  {
    public string fileName;
    public string shapeMesh;
  }
}
