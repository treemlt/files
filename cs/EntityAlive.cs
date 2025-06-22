// Decompiled with JetBrains decompiler
// Type: EntityAlive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using Audio;
using GamePath;
using Platform;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UAI;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
[Preserve]
public abstract class EntityAlive : Entity
{
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public const float cTraderTeleportCheckTime = 0.1f;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public const float cDamageImmunityOnRespawnSeconds = 1f;
  public static readonly FastTags<TagGroup.Global> DistractionResistanceWithTargetTags = FastTags<TagGroup.Global>.GetTag("with_target");
  public static readonly int FeralTagBit = FastTags<TagGroup.Global>.GetBit("feral");
  public static readonly int FallingBuffTagBit = FastTags<TagGroup.Global>.GetBit("buffPlayerFallingDamage");
  public static readonly FastTags<TagGroup.Global> StanceTagCrouching = FastTags<TagGroup.Global>.GetTag("crouching");
  public static readonly FastTags<TagGroup.Global> StanceTagStanding = FastTags<TagGroup.Global>.GetTag("standing");
  public static readonly FastTags<TagGroup.Global> MovementTagIdle = FastTags<TagGroup.Global>.GetTag("idle");
  public static readonly FastTags<TagGroup.Global> MovementTagWalking = FastTags<TagGroup.Global>.GetTag("walking");
  public static readonly FastTags<TagGroup.Global> MovementTagRunning = FastTags<TagGroup.Global>.GetTag("running");
  public static readonly FastTags<TagGroup.Global> MovementTagFloating = FastTags<TagGroup.Global>.GetTag("floating");
  public static readonly FastTags<TagGroup.Global> MovementTagSwimming = FastTags<TagGroup.Global>.GetTag("swimming");
  public static readonly FastTags<TagGroup.Global> MovementTagSwimmingRun = FastTags<TagGroup.Global>.GetTag("swimmingRun");
  public static readonly FastTags<TagGroup.Global> MovementTagJumping = FastTags<TagGroup.Global>.GetTag("jumping");
  public static readonly FastTags<TagGroup.Global> MovementTagFalling = FastTags<TagGroup.Global>.GetTag("falling");
  public static readonly FastTags<TagGroup.Global> MovementTagClimbing = FastTags<TagGroup.Global>.GetTag("climbing");
  public static readonly FastTags<TagGroup.Global> MovementTagDriving = FastTags<TagGroup.Global>.GetTag("driving");
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public static readonly float[] moveSpeedRandomness = new float[6]
  {
    0.2f,
    1f,
    1.1f,
    1.2f,
    1.35f,
    1.5f
  };
  public const float CLIMB_LADDER_SPEED = 1234f;
  public static ulong HitDelay = 11000;
  public static float HitSoundDistance = 10f;
  public MinEventParams MinEventContext = new MinEventParams();
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int equippingCount;
  public bool IsSleeper;
  public bool IsSleeping;
  public bool IsSleeperPassive;
  public bool SleeperSupressLivingSounds;
  public Vector3 SleeperSpawnPosition;
  public Vector3 SleeperSpawnLookDir;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public float accumulatedDamageResisted;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int pendingSleepTrigger = -1;
  public int lastSleeperPose;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public Vector3 sleeperLookDir;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public float sleeperSightRange;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float sleeperViewAngle;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public Vector2 sightLightThreshold;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public Vector2 sightWakeThresholdAtRange;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public Vector2 sightGroanThresholdAtRange;
  public float sleeperNoiseToSense;
  public float sleeperNoiseToWake;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool isSnore;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool isGroan;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool isGroanSilent;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float sleeperNoiseToSenseSoundChance;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int snoreGroanCD;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public const int kSnoreGroanMinCD = 20;
  public float noisePlayerDistance;
  public float noisePlayerVolume;
  public EntityPlayer noisePlayer;
  public EntityItem pendingDistraction;
  public float pendingDistractionDistanceSq;
  public EntityItem distraction;
  public float distractionResistance;
  public float distractionResistanceWithTarget;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public const float cSwimGravityPer = 0.025f;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public const float cSwimDragY = 0.91f;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public const float cSwimDrag = 0.91f;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public const float cSwimAnimDelay = 6f;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public int jumpTicks;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public EntityAlive.JumpState jumpState;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public int jumpStateTicks;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public float jumpDistance;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public float jumpHeightDiff;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public float jumpSwimDurationTicks;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public Vector3 jumpSwimMotion;
  public float jumpDelay;
  public float jumpMaxDistance;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool jumpIsMoving;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int ticksNoPlayerAdjacent;
  public int hasBeenAttackedTime;
  public float painHitsFelt;
  public float painResistPercent;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public int attackingTime;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public EntityAlive revengeEntity;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int revengeTimer;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool targetAlertChanged;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float lastAliveTime;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public bool alertEnabled = true;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int alertTicks;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public static string notAlertedId = "_notAlerted";
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int notAlertDelayTicks;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool isAlert;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public Vector3 investigatePos;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int investigatePositionTicks;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool isInvestigateAlert;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool hasAI;
  public EAIManager aiManager;
  public List<string> AIPackages;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public Context utilityAIContext;
  public EntityPlayer aiClosestPlayer;
  public float aiClosestPlayerDistSq;
  public float aiActiveScale;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float aiActiveDelay;
  public bool IsBloodMoon;
  public bool IsFeral;
  public bool IsBreakingDoors;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool m_isBreakingBlocks;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool m_isEating;
  public Vector3 ChaseReturnLocation;
  public bool IsScoutZombie;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public EntityLookHelper lookHelper;
  public EntityMoveHelper moveHelper;
  public PathNavigate navigator;
  public bool bCanClimbLadders;
  public bool bCanClimbVertical;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public Vector3 lastTargetPos;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public EntityAlive damagedTarget;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public EntityAlive attackTarget;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int attackTargetTime;
  public EntityAlive attackTargetClient;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public EntityAlive attackTargetLast;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public EntitySeeCache seeCache;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public ChunkCoordinates homePosition;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int maximumHomeDistance;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float jumpMovementFactor = 0.02f;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public float landMovementFactor = 0.1f;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public float jumpMotionYValue = 0.419f;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float stepSoundDistanceRemaining;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float stepSoundRotYRemaining;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float nextSwimDistance;
  public Inventory inventory;
  public Inventory saveInventory;
  public Equipment equipment;
  public Bag bag;
  public ChallengeJournal challengeJournal;
  public int ExperienceValue;
  public int deathUpdateTime;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public EntityAlive entityThatKilledMe;
  public bool bPlayerStatsChanged;
  public bool bEntityAliveFlagsChanged;
  public bool bPlayerTwitchChanged;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public Dictionary<EnumDamageSource, ulong> damageSourceTimeouts = (Dictionary<EnumDamageSource, ulong>) new EnumDictionary<EnumDamageSource, ulong>();
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public int traderTeleportStreak = 1;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public bool bJetpackWearing;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public bool bJetpackActive;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool bParachuteWearing;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool bAimingGun;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public bool bMovementRunning;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public bool bCrouching;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public bool bJumping;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool bClimbing;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public int died;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public int score;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public int killedZombies;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public int killedPlayers;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public int teamNumber;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public string entityName = string.Empty;
  public string DebugNameInfo = string.Empty;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public int damageLocationBits;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public bool bSpawned;
  public bool bReplicatedAlertFlag;
  public int vehiclePoseMode = -1;
  public byte factionId;
  public byte factionRank;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int ticksToCheckSeenByPlayer;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool wasSeenByPlayer;
  public DamageResponse RecordedDamage;
  public float moveSpeed;
  public float moveSpeedNight;
  public float moveSpeedAggro;
  public float moveSpeedAggroMax;
  public float moveSpeedPanic;
  public float moveSpeedPanicMax;
  public float swimSpeed;
  public Vector2 swimStrokeRate;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public ItemValue handItem;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public string soundSpawn;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public string soundSleeperGroan;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public string soundSleeperSnore;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public string soundDeath;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public string soundAlert;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public string soundAttack;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public string soundLiving;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public string soundRandom;
  public string soundSense;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public string soundGiveUp;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public string soundStepType;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public string soundStamina;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public string soundJump;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public string soundLand;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public string soundHurt;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public string soundHurtSmall;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public string soundDrownPain;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public string soundDrownDeath;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public string soundWaterSurface;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int soundDelayTicks;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int soundLivingID = -1;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public const float cSoundRandomMaxDist = 20f;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int soundAlertTicks;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int soundRandomTicks;
  public int classMaxHealth;
  public int classMaxStamina;
  public int classMaxFood;
  public int classMaxWater;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float weight;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float pushFactor;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float maxViewAngle;
  public float sightRangeBase;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public float sightRange;
  public float senseScale;
  public int timeStayAfterDeath;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public BlockValue corpseBlockValue;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float corpseBlockChance;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int attackTimeoutDay;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int attackTimeoutNight;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public string particleOnDeath;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public string particleOnDestroy;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public EntityBedrollPositionList spawnPoints;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public List<Vector3i> droppedBackpackPositions;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public float speedModifier = 1f;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public Vector3 accumulatedRootMotion;
  public Vector3 moveDirection;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool isMoveDirAbsolute;
  public Vector3 lookAtPosition;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public Vector3i blockPosStandingOn;
  public BlockValue blockValueStandingOn;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool blockStandingOnChanged;
  public BiomeDefinition biomeStandingOn;
  public bool IsMale;
  public int crouchType;
  public float crouchBendPer;
  public float crouchBendPerTarget;
  public const int cWalkTypeSwim = -1;
  public const int cWalkTypeFat = 1;
  public const int cWalkTypeCripple = 5;
  public const int cWalkTypeCrouch = 8;
  public const int cWalkTypeBandit = 15;
  public const int cWalkTypeCrawlFirst = 20;
  public const int cWalkTypeCrawler = 21;
  public const int cWalkTypeSpider = 22;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public int walkType;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public int walkTypeBeforeCrouch;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public string rightHandTransformName;
  public int pingToServer;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public List<ItemStack> itemsOnEnterGame = new List<ItemStack>();
  public Utils.EnumHitDirection lastHitDirection = Utils.EnumHitDirection.None;
  public Vector3 lastHitImpactDir = Vector3.zero;
  public Vector3 lastHitEntityFwd = Vector3.zero;
  public bool lastHitRanged;
  public float lastHitForce;
  public DamageResponse lastDamageResponse;
  public bool canDisintegrate;
  public bool isDisintegrated;
  public float CreationTimeSinceLevelLoad;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public EntityStats entityStats;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float proneRefillRate;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float kneelRefillRate;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float proneRefillCounter;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float kneelRefillCounter;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int deathHealth;
  public BodyDamage bodyDamage;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool stompsSpikes;
  public float OverrideSize = 1f;
  public float OverrideHeadSize = 1f;
  public float OverrideHeadDismemberScaleTime = 1.5f;
  public float OverridePitch;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool isDancing;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float lastTimeTraderStationChecked;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool lerpForwardSpeed;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float speedForwardTarget;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float speedForwardTargetStep = 1f;
  public EntityBuffs Buffs;
  public Progression Progression;
  public FastTags<TagGroup.Global> CurrentStanceTag = EntityAlive.StanceTagStanding;
  public FastTags<TagGroup.Global> CurrentMovementTag = FastTags<TagGroup.Global>.none;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float renderFade;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float renderFadeTarget;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public readonly List<EntityAlive.FallBehavior> fallBehaviors = new List<EntityAlive.FallBehavior>();
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool disableFallBehaviorUntilOnGround;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public readonly List<EntityAlive.DestroyBlockBehavior> _destroyBlockBehaviors = new List<EntityAlive.DestroyBlockBehavior>();
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public DynamicRagdollFlags _dynamicRagdoll;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float _dynamicRagdollStunTime;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public Vector3 _dynamicRagdollRootMotion;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public readonly List<Vector3> _ragdollPositionsPrev = new List<Vector3>();
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public readonly List<Vector3> _ragdollPositionsCur = new List<Vector3>();
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool isFirstTimeEquipmentReassigned = true;
  public bool CrouchingLocked;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public EModelBase.HeadStates currentHeadState;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public static readonly List<EntityAlive.WeightBehavior> weightBehaviorTemp = new List<EntityAlive.WeightBehavior>();
  public static bool ShowDebugDisplayHit = false;
  public static float DebugDisplayHitSize = 0.005f;
  public static float DebugDisplayHitTime = 10f;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool bPlayHurtSound;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public bool bBeenWounded;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public int woundedStrength;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public DamageSource woundedDamageSource;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int despawnDelayCounter;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool isDespawnWhenPlayerFar;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool wasOnGround = true;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float landWaterLevel;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool m_addedToWorld;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int saveHoldingItemIdxBeforeAttach;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float impactSoundTime;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public Dictionary<Transform, EntityAlive.ImpactData> impacts = new Dictionary<Transform, EntityAlive.ImpactData>();
  public const string cParticlePrefix = "Ptl_";
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public Dictionary<string, Transform> particles = new Dictionary<string, Transform>();
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public Dictionary<string, Transform> parts = new Dictionary<string, Transform>();
  [SerializeField]
  [PublicizedFrom(EAccessModifier.Private)]
  public List<OwnedEntityData> ownedEntities = new List<OwnedEntityData>();
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public List<EntityAlive.NetworkStatChange> networkStatsUpdateQueue = new List<EntityAlive.NetworkStatChange>();

  public bool IsEquipping
  {
    get => this.equippingCount > 0;
    set
    {
      if (value)
      {
        ++this.equippingCount;
      }
      else
      {
        if (this.equippingCount <= 0)
          return;
        --this.equippingCount;
      }
    }
  }

  public bool IsDancing
  {
    get => this.isDancing;
    set
    {
      this.isDancing = value;
      if (value)
      {
        if (!Object.op_Inequality((Object) this.emodel, (Object) null) || !Object.op_Inequality((Object) this.emodel.avatarController, (Object) null))
          return;
        this.emodel.avatarController.UpdateInt(nameof (IsDancing), this.EntityClass.DanceTypeID);
      }
      else
      {
        if (!Object.op_Inequality((Object) this.emodel, (Object) null) || !Object.op_Inequality((Object) this.emodel.avatarController, (Object) null))
          return;
        this.emodel.avatarController.UpdateInt(nameof (IsDancing), 0);
      }
    }
  }

  public void BeginDynamicRagdoll(DynamicRagdollFlags flags, FloatRange stunTime)
  {
    this._dynamicRagdoll = flags;
    this._dynamicRagdollRootMotion = Vector3.zero;
    this._dynamicRagdollStunTime = stunTime.Random(this.rand);
  }

  public void ActivateDynamicRagdoll()
  {
    if (!this._dynamicRagdoll.HasFlag((Enum) DynamicRagdollFlags.Active))
      return;
    DynamicRagdollFlags dynamicRagdoll = this._dynamicRagdoll;
    this._dynamicRagdoll = DynamicRagdollFlags.None;
    Vector3 forceVec = Vector3.op_Multiply(this._dynamicRagdollRootMotion, 20f);
    this.bodyDamage.StunDuration = this._dynamicRagdollStunTime;
    this.emodel.DoRagdoll(this._dynamicRagdollStunTime, EnumBodyPartHit.None, forceVec, Vector3.zero, true);
    if (!dynamicRagdoll.HasFlag((Enum) DynamicRagdollFlags.UseBoneVelocities) || this._ragdollPositionsPrev.Count != this._ragdollPositionsCur.Count)
      return;
    List<Vector3> velocities = new List<Vector3>();
    for (int index = 0; index < this._ragdollPositionsPrev.Count; ++index)
    {
      Vector3 vector3 = Vector3.op_Subtraction(this._ragdollPositionsCur[index], this._ragdollPositionsPrev[index]);
      velocities.Add(Vector3.op_Multiply(vector3, 20f));
    }
    this.emodel.ApplyRagdollVelocities(velocities);
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public override void Awake()
  {
    base.Awake();
    this.entityName = ((object) this).GetType().Name;
    this.MinEventContext.Self = this;
    this.seeCache = new EntitySeeCache(this);
    this.maximumHomeDistance = -1;
    this.homePosition = new ChunkCoordinates(0, 0, 0);
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && !(this is EntityPlayer))
    {
      this.hasAI = true;
      this.navigator = AstarManager.CreateNavigator(this);
      this.aiManager = new EAIManager(this);
      this.lookHelper = new EntityLookHelper(this);
      this.moveHelper = new EntityMoveHelper(this);
    }
    this.equipment = new Equipment(this);
    this.InitInventory();
    if (this.bag == null)
      this.bag = new Bag(this);
    this.stepHeight = 0.52f;
    this.soundDelayTicks = this.GetSoundRandomTicks() / 3 - 5;
    this.spawnPoints = new EntityBedrollPositionList(this);
    this.CreationTimeSinceLevelLoad = Time.timeSinceLevelLoad;
    this.Buffs = new EntityBuffs(this);
    this.droppedBackpackPositions = new List<Vector3i>();
  }

  public override void Init(int _entityClass)
  {
    base.Init(_entityClass);
    this.InitStats();
    this.switchModelView(EnumEntityModelView.ThirdPerson);
    this.InitPostCommon();
  }

  public override void InitFromPrefab(int _entityClass)
  {
    base.InitFromPrefab(_entityClass);
    this.switchModelView(EnumEntityModelView.ThirdPerson);
    this.InitPostCommon();
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void InitPostCommon()
  {
    if (GameManager.IsDedicatedServer)
    {
      Transform modelTransform = this.emodel.GetModelTransform();
      if (Object.op_Implicit((Object) modelTransform))
        ServerHelper.SetupForServer(((Component) modelTransform).gameObject);
    }
    this.AddCharacterController();
    this.wasSeenByPlayer = false;
    this.ticksToCheckSeenByPlayer = 20;
    if (EntityClass.list[this.entityClass].UseAIPackages)
    {
      this.hasAI = true;
      this.AIPackages = new List<string>();
      this.AIPackages.AddRange((IEnumerable<string>) EntityClass.list[this.entityClass].AIPackages);
      this.utilityAIContext = new Context(this);
    }
    List<string> buffs = EntityClass.list[this.entityClass].Buffs;
    if (buffs != null)
    {
      for (int index = 0; index < buffs.Count; ++index)
      {
        string _name = buffs[index];
        if (!this.Buffs.HasBuff(_name))
        {
          int num = (int) this.Buffs.AddBuff(_name);
        }
      }
    }
    if ((this.entityFlags & (EntityFlags.Zombie | EntityFlags.Animal | EntityFlags.Bandit)) <= EntityFlags.None)
      return;
    this.emodel.SetVisible(false);
    this.emodel.SetFade(0.0f);
  }

  public override void PostInit()
  {
    base.PostInit();
    this.ApplySpawnState();
    LODGroup componentInChildren = ((Component) this.emodel.GetModelTransform()).GetComponentInChildren<LODGroup>();
    if (Object.op_Implicit((Object) componentInChildren))
    {
      LOD[] loDs = componentInChildren.GetLODs();
      loDs[loDs.Length - 1].screenRelativeTransitionHeight = 3f / 1000f;
      componentInChildren.SetLODs(loDs);
    }
    this.disableFallBehaviorUntilOnGround = true;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void ApplySpawnState()
  {
    if (this.Health <= 0 && this.isEntityRemote)
      this.ClientKill(DamageResponse.New(true));
    this.ExecuteDismember(true);
  }

  public virtual void InitInventory()
  {
    if (this.inventory != null)
      return;
    this.inventory = new Inventory((IGameManager) GameManager.Instance, this);
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual void switchModelView(EnumEntityModelView modelView)
  {
    this.emodel.SwitchModelAndView(modelView == EnumEntityModelView.FirstPerson, this.IsMale);
    this.ReassignEquipmentTransforms();
  }

  public virtual void ReassignEquipmentTransforms()
  {
    if (this.isFirstTimeEquipmentReassigned)
    {
      this.Buffs.SetCustomVar("_equipReload", 0.0f);
      this.isFirstTimeEquipmentReassigned = false;
    }
    else
      this.Buffs.SetCustomVar("_equipReload", 1f);
    this.equipment.InitializeEquipmentTransforms();
    this.Buffs.SetCustomVar("_equipReload", 0.0f);
  }

  public override void CopyPropertiesFromEntityClass()
  {
    base.CopyPropertiesFromEntityClass();
    EntityClass entityClass = EntityClass.list[this.entityClass];
    string _itemName = entityClass.Properties.GetString(EntityClass.PropHandItem);
    if (_itemName.Length > 0)
    {
      this.handItem = ItemClass.GetItem(_itemName);
      if (this.handItem.IsEmpty())
        throw new Exception($"Item with name '{_itemName}' not found!");
    }
    else
      this.handItem = ItemClass.GetItem("meleeHandPlayer").Clone();
    if (this.inventory != null)
      this.inventory.SetBareHandItem(this.handItem);
    this.rightHandTransformName = "Gunjoint";
    if (this.emodel is EModelSDCS)
      this.rightHandTransformName = "RightWeapon";
    entityClass.Properties.ParseString(EntityClass.PropRightHandJointName, ref this.rightHandTransformName);
    if (!(this is EntityPlayer))
    {
      this.factionId = (byte) 0;
      this.factionRank = (byte) 0;
      string _name = entityClass.Properties.GetString("Faction");
      if (_name.Length > 0)
      {
        Faction factionByName = FactionManager.Instance.GetFactionByName(_name);
        if (factionByName != null)
        {
          this.factionId = factionByName.ID;
          string _input = entityClass.Properties.GetString("FactionRank");
          if (_input.Length > 0)
            this.factionRank = StringParsers.ParseUInt8(_input);
        }
      }
    }
    else if (FactionManager.Instance.GetFaction(this.factionId).ID == (byte) 0)
    {
      this.factionId = FactionManager.Instance.CreateFaction(this.entityName).ID;
      this.factionRank = byte.MaxValue;
    }
    this.maxViewAngle = 180f;
    entityClass.Properties.ParseFloat(EntityClass.PropMaxViewAngle, ref this.maxViewAngle);
    this.sightRangeBase = entityClass.SightRange;
    this.sightLightThreshold = entityClass.sightLightThreshold;
    this.SetSleeperSight(-1f, -1f);
    this.sightWakeThresholdAtRange.x = this.rand.RandomRange(entityClass.SleeperSightToWakeMin.x, entityClass.SleeperSightToWakeMin.y);
    this.sightWakeThresholdAtRange.y = this.rand.RandomRange(entityClass.SleeperSightToWakeMax.y, entityClass.SleeperSightToWakeMax.y);
    this.sightGroanThresholdAtRange.x = this.rand.RandomRange(entityClass.SleeperSightToSenseMin.x, entityClass.SleeperSightToSenseMin.y);
    this.sightGroanThresholdAtRange.y = this.rand.RandomRange(entityClass.SleeperSightToSenseMax.y, entityClass.SleeperSightToSenseMax.y);
    this.sleeperNoiseToSense = this.rand.RandomRange(entityClass.SleeperNoiseToSense.x, entityClass.SleeperNoiseToSense.y);
    this.sleeperNoiseToSenseSoundChance = entityClass.SleeperNoiseToSenseSoundChance;
    this.sleeperNoiseToWake = this.rand.RandomRange(entityClass.SleeperNoiseToWake.x, entityClass.SleeperNoiseToWake.y);
    float optionalValue1 = 1f;
    entityClass.Properties.ParseFloat(EntityClass.PropAttackTimeoutDay, ref optionalValue1);
    this.attackTimeoutDay = (int) ((double) optionalValue1 * 20.0);
    entityClass.Properties.ParseFloat(EntityClass.PropAttackTimeoutNight, ref optionalValue1);
    this.attackTimeoutNight = (int) ((double) optionalValue1 * 20.0);
    entityClass.Properties.ParseBool(EntityClass.PropStompsSpikes, ref this.stompsSpikes);
    this.weight = 1f;
    entityClass.Properties.ParseFloat(EntityClass.PropWeight, ref this.weight);
    this.weight = Utils.FastMax(this.weight, 0.5f);
    this.pushFactor = 1f;
    entityClass.Properties.ParseFloat(EntityClass.PropPushFactor, ref this.pushFactor);
    float optionalValue2 = 5f;
    entityClass.Properties.ParseFloat(EntityClass.PropTimeStayAfterDeath, ref optionalValue2);
    this.timeStayAfterDeath = (int) ((double) optionalValue2 * 20.0);
    this.IsMale = true;
    entityClass.Properties.ParseBool(EntityClass.PropIsMale, ref this.IsMale);
    this.IsFeral = entityClass.Tags.Test_Bit(EntityAlive.FeralTagBit);
    this.proneRefillRate = this.rand.RandomRange(entityClass.KnockdownProneRefillRate.x, entityClass.KnockdownProneRefillRate.y);
    this.kneelRefillRate = this.rand.RandomRange(entityClass.KnockdownKneelRefillRate.x, entityClass.KnockdownKneelRefillRate.y);
    this.moveSpeed = 1f;
    entityClass.Properties.ParseFloat(EntityClass.PropMoveSpeed, ref this.moveSpeed);
    this.moveSpeedNight = this.moveSpeed;
    entityClass.Properties.ParseFloat(EntityClass.PropMoveSpeedNight, ref this.moveSpeedNight);
    this.moveSpeedAggro = this.moveSpeed;
    this.moveSpeedAggroMax = this.moveSpeed;
    entityClass.Properties.ParseVec(EntityClass.PropMoveSpeedAggro, ref this.moveSpeedAggro, ref this.moveSpeedAggroMax);
    this.moveSpeedPanic = 1f;
    this.moveSpeedPanicMax = 1f;
    entityClass.Properties.ParseFloat(EntityClass.PropMoveSpeedPanic, ref this.moveSpeedPanic);
    if ((double) this.moveSpeedPanic != 1.0)
      this.moveSpeedPanicMax = this.moveSpeedPanic;
    entityClass.Properties.ParseFloat(EntityClass.PropSwimSpeed, ref this.swimSpeed);
    entityClass.Properties.ParseVec(EntityClass.PropSwimStrokeRate, ref this.swimStrokeRate);
    Vector2 negativeInfinity = Vector2.negativeInfinity;
    entityClass.Properties.ParseVec(EntityClass.PropMoveSpeedRand, ref negativeInfinity);
    if ((double) negativeInfinity.x > -1.0)
    {
      float num1 = this.rand.RandomRange(negativeInfinity.x, negativeInfinity.y);
      int index = GameStats.GetInt(EnumGameStats.GameDifficulty);
      float num2 = num1 * EntityAlive.moveSpeedRandomness[index];
      if ((double) this.moveSpeedAggro < 1.0)
      {
        this.moveSpeedAggro += num2;
        if ((double) this.moveSpeedAggro < 0.10000000149011612)
          this.moveSpeedAggro = 0.1f;
        if ((double) this.moveSpeedAggro > (double) this.moveSpeedAggroMax)
          this.moveSpeedAggro = this.moveSpeedAggroMax;
      }
    }
    entityClass.Properties.ParseInt(EntityClass.PropCrouchType, ref this.crouchType);
    this.walkType = EntityAlive.GetSpawnWalkType(entityClass);
    entityClass.Properties.ParseBool(EntityClass.PropCanClimbLadders, ref this.bCanClimbLadders);
    entityClass.Properties.ParseBool(EntityClass.PropCanClimbVertical, ref this.bCanClimbVertical);
    Vector2 optionalValue3;
    // ISSUE: explicit constructor call
    ((Vector2) ref optionalValue3).\u002Ector(1.9f, 2.1f);
    entityClass.Properties.ParseVec(EntityClass.PropJumpMaxDistance, ref optionalValue3);
    this.jumpMaxDistance = this.rand.RandomRange(optionalValue3.x, optionalValue3.y);
    this.jumpDelay = 1f;
    entityClass.Properties.ParseFloat(EntityClass.PropJumpDelay, ref this.jumpDelay);
    this.jumpDelay *= 20f;
    this.ExperienceValue = 20;
    entityClass.Properties.ParseInt(EntityClass.PropExperienceGain, ref this.ExperienceValue);
    if (this.aiManager != null)
      this.aiManager.CopyPropertiesFromEntityClass(entityClass);
    entityClass.Properties.ParseString(EntityClass.PropSoundSpawn, ref this.soundSpawn);
    entityClass.Properties.ParseString(EntityClass.PropSoundSleeperSense, ref this.soundSleeperGroan);
    entityClass.Properties.ParseString(EntityClass.PropSoundSleeperSnore, ref this.soundSleeperSnore);
    entityClass.Properties.ParseString(EntityClass.PropSoundDeath, ref this.soundDeath);
    entityClass.Properties.ParseString(EntityClass.PropSoundAlert, ref this.soundAlert);
    entityClass.Properties.ParseString(EntityClass.PropSoundAttack, ref this.soundAttack);
    entityClass.Properties.ParseString(EntityClass.PropSoundLiving, ref this.soundLiving);
    entityClass.Properties.ParseString(EntityClass.PropSoundRandom, ref this.soundRandom);
    entityClass.Properties.ParseString(EntityClass.PropSoundSense, ref this.soundSense);
    entityClass.Properties.ParseString(EntityClass.PropSoundGiveUp, ref this.soundGiveUp);
    this.soundStepType = "step";
    entityClass.Properties.ParseString(EntityClass.PropSoundStepType, ref this.soundStepType);
    entityClass.Properties.ParseString(EntityClass.PropSoundStamina, ref this.soundStamina);
    entityClass.Properties.ParseString(EntityClass.PropSoundJump, ref this.soundJump);
    entityClass.Properties.ParseString(EntityClass.PropSoundLand, ref this.soundLand);
    entityClass.Properties.ParseString(EntityClass.PropSoundHurt, ref this.soundHurt);
    entityClass.Properties.ParseString(EntityClass.PropSoundHurtSmall, ref this.soundHurtSmall);
    entityClass.Properties.ParseString(EntityClass.PropSoundDrownPain, ref this.soundDrownPain);
    entityClass.Properties.ParseString(EntityClass.PropSoundDrownDeath, ref this.soundDrownDeath);
    entityClass.Properties.ParseString(EntityClass.PropSoundWaterSurface, ref this.soundWaterSurface);
    this.soundAlertTicks = 25;
    entityClass.Properties.ParseInt(EntityClass.PropSoundAlertTime, ref this.soundAlertTicks);
    this.soundAlertTicks *= 20;
    this.soundRandomTicks = 25;
    entityClass.Properties.ParseInt(EntityClass.PropSoundRandomTime, ref this.soundRandomTicks);
    this.soundRandomTicks *= 20;
    entityClass.Properties.ParseString(EntityClass.PropParticleOnDeath, ref this.particleOnDeath);
    entityClass.Properties.ParseString(EntityClass.PropParticleOnDestroy, ref this.particleOnDestroy);
    string _blockName = entityClass.Properties.GetString(EntityClass.PropCorpseBlock);
    if (_blockName.Length > 0)
      this.corpseBlockValue = Block.GetBlockValue(_blockName);
    this.corpseBlockChance = 1f;
    entityClass.Properties.ParseFloat(EntityClass.PropCorpseBlockChance, ref this.corpseBlockChance);
    GameMode gameModeForId = GameMode.GetGameModeForId(GameStats.GetInt(EnumGameStats.GameModeId));
    if (gameModeForId != null)
    {
      string str1 = entityClass.Properties.GetString($"{EntityClass.PropItemsOnEnterGame}.{gameModeForId.GetTypeName()}");
      if (str1.Length > 0)
      {
        foreach (string str2 in str1.Split(',', StringSplitOptions.None))
        {
          ItemStack itemStack = ItemStack.FromString(str2.Trim());
          if (itemStack.itemValue.IsEmpty())
            throw new Exception($"Item with name '{str2}' not found in class {EntityClass.list[this.entityClass].entityClassName}");
          if (itemStack.itemValue.ItemClass.CreativeMode != EnumCreativeMode.Console || (DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent())
            this.itemsOnEnterGame.Add(itemStack);
        }
      }
    }
    DynamicProperties dynamicProperties1 = entityClass.Properties.Classes[EntityClass.PropFallLandBehavior];
    if (dynamicProperties1 != null)
    {
      foreach (KeyValuePair<string, string> keyValuePair in dynamicProperties1.Data.Dict)
      {
        string key = keyValuePair.Key;
        DictionarySave<string, string> keyData = dynamicProperties1.ParseKeyData(key);
        if (keyData != null)
        {
          FloatRange floatRange = new FloatRange();
          FloatRange ragePer = new FloatRange();
          FloatRange rageTime = new FloatRange();
          IntRange difficulty = new IntRange(0, 10);
          string _input;
          EntityAlive.FallBehavior.Op result;
          if (!keyData.TryGetValue("anim", out _input) || !Enum.TryParse<EntityAlive.FallBehavior.Op>(_input, out result))
          {
            Log.Error($"Expected 'anim' parameter as float for FallBehavior {key}, skipping");
          }
          else
          {
            float _result1 = 0.0f;
            if (!keyData.TryGetValue("weight", out _input) || !StringParsers.TryParseFloat(_input, out _result1))
              Log.Error($"Expected 'weight' parameter as float for FallBehavior {key}, skipping");
            else if (keyData.TryGetValue("height", out _input))
            {
              FloatRange _result2;
              if (StringParsers.TryParseRange(_input, out _result2, new float?(float.MaxValue)))
              {
                FloatRange height = _result2;
                if (keyData.TryGetValue("ragePer", out _input))
                {
                  FloatRange _result3;
                  if (StringParsers.TryParseRange(_input, out _result3))
                  {
                    ragePer = _result3;
                  }
                  else
                  {
                    Log.Error($"Expected 'ragePer' parameter as range(min,min-max) {key}, skipping");
                    continue;
                  }
                }
                if (keyData.TryGetValue("rageTime", out _input))
                {
                  FloatRange _result4;
                  if (StringParsers.TryParseRange(_input, out _result4))
                  {
                    rageTime = _result4;
                  }
                  else
                  {
                    Log.Error($"Expected 'rageTime' parameter as range(min,min-max) {key}, skipping");
                    continue;
                  }
                }
                if (keyData.TryGetValue("difficulty", out _input))
                {
                  IntRange _result5;
                  if (StringParsers.TryParseRange(_input, out _result5))
                  {
                    difficulty = _result5;
                  }
                  else
                  {
                    Log.Error($"Expected 'difficulty' parameter as range(min,min-max) {key}, skipping");
                    continue;
                  }
                }
                this.fallBehaviors.Add(new EntityAlive.FallBehavior(key, result, height, _result1, ragePer, rageTime, difficulty));
              }
              else
                Log.Error($"Expected 'height' parameter as range(min,min-max) {key}, skipping");
            }
            else
              Log.Error($"Expected 'height' parameter for FallBehavior {key}, skipping");
          }
        }
      }
    }
    DynamicProperties dynamicProperties2 = entityClass.Properties.Classes[EntityClass.PropDestroyBlockBehavior];
    if (dynamicProperties2 != null)
    {
      EntityAlive.DestroyBlockBehavior.Op[] values = Enum.GetValues(typeof (EntityAlive.DestroyBlockBehavior.Op)) as EntityAlive.DestroyBlockBehavior.Op[];
      for (int index = 0; index < values.Length; ++index)
      {
        string stringCached = values[index].ToStringCached<EntityAlive.DestroyBlockBehavior.Op>();
        DictionarySave<string, string> keyData = dynamicProperties2.ParseKeyData(values[index].ToStringCached<EntityAlive.DestroyBlockBehavior.Op>());
        if (keyData != null)
        {
          FloatRange ragePer = new FloatRange();
          FloatRange rageTime = new FloatRange();
          IntRange difficulty = new IntRange(0, 10);
          string _input;
          float _result6;
          if (!keyData.TryGetValue("weight", out _input) || !StringParsers.TryParseFloat(_input, out _result6))
          {
            Log.Error($"Expected 'weight' parameter as float for FallBehavior {values[index]}, skipping");
          }
          else
          {
            if (keyData.TryGetValue("ragePer", out _input))
            {
              FloatRange _result7;
              if (StringParsers.TryParseRange(_input, out _result7))
              {
                ragePer = _result7;
              }
              else
              {
                Log.Error($"Expected 'ragePer' parameter as range(min,min-max) {values[index]}, skipping");
                continue;
              }
            }
            if (keyData.TryGetValue("rageTime", out _input))
            {
              FloatRange _result8;
              if (StringParsers.TryParseRange(_input, out _result8))
              {
                rageTime = _result8;
              }
              else
              {
                Log.Error($"Expected 'rageTime' parameter as range(min,min-max) {values[index]}, skipping");
                continue;
              }
            }
            if (keyData.TryGetValue("difficulty", out _input))
            {
              IntRange _result9;
              if (StringParsers.TryParseRange(_input, out _result9))
              {
                difficulty = _result9;
              }
              else
              {
                Log.Error($"Expected 'difficulty' parameter as range(min,min-max) {stringCached}, skipping");
                continue;
              }
            }
            this._destroyBlockBehaviors.Add(new EntityAlive.DestroyBlockBehavior(stringCached, values[index], _result6, ragePer, rageTime, difficulty));
          }
        }
      }
    }
    this.distractionResistance = EffectManager.GetValue(PassiveEffects.DistractionResistance, _entity: this);
    this.distractionResistanceWithTarget = EffectManager.GetValue(PassiveEffects.DistractionResistance, _entity: this, tags: EntityAlive.DistractionResistanceWithTargetTags);
  }

  public static int GetSpawnWalkType(EntityClass _entityClass)
  {
    int optionalValue = 0;
    _entityClass.Properties.ParseInt(EntityClass.PropWalkType, ref optionalValue);
    return optionalValue;
  }

  public override void VisiblityCheck(float _distanceSqr, bool _isZoom)
  {
    if ((this.entityFlags & (EntityFlags.Zombie | EntityFlags.Animal | EntityFlags.Bandit)) <= EntityFlags.None)
      return;
    if (GameManager.IsDedicatedServer)
      this.emodel.SetVisible(true);
    else if ((double) _distanceSqr < (_isZoom ? 14400.0 : 8100.0))
      this.renderFadeTarget = 1f;
    else
      this.renderFadeTarget = 0.0f;
  }

  public virtual void SetSleeper()
  {
    this.IsSleeper = true;
    this.aiManager.pathCostScale += 0.2f;
  }

  public void SetSleeperSight(float angle, float range)
  {
    if ((double) angle < 0.0)
      angle = this.maxViewAngle;
    this.sleeperViewAngle = angle;
    if ((double) range < 0.0)
      range = Utils.FastMax(3f, this.sightRangeBase * 0.2f);
    this.sleeperSightRange = range;
  }

  public void SetSleeperHearing(float percent)
  {
    if ((double) percent < 1.0 / 1000.0)
      percent = 1f / 1000f;
    percent = 1f / percent;
    this.sleeperNoiseToSense *= percent;
    this.sleeperNoiseToWake *= percent;
  }

  public int GetSleeperDisturbedLevel(float dist, float lightLevel)
  {
    float num1 = dist / this.sightRangeBase;
    if ((double) num1 <= 1.0)
    {
      float num2 = Mathf.Lerp(this.sightWakeThresholdAtRange.x, this.sightWakeThresholdAtRange.y, num1);
      if ((double) lightLevel > (double) num2)
        return 2;
      float num3 = Mathf.Lerp(this.sightGroanThresholdAtRange.x, this.sightGroanThresholdAtRange.y, num1);
      if ((double) lightLevel > (double) num3)
        return 1;
    }
    return 0;
  }

  public void GetSleeperDebugScale(float dist, out float wake, out float groan)
  {
    float num = dist / this.sightRangeBase;
    wake = Mathf.Lerp(this.sightWakeThresholdAtRange.x, this.sightWakeThresholdAtRange.y, num);
    groan = Mathf.Lerp(this.sightGroanThresholdAtRange.x, this.sightGroanThresholdAtRange.y, num);
  }

  public bool sleepingOrWakingUp => this.IsSleeping;

  public void TriggerSleeperPose(int _pose, bool _returningToSleep = false)
  {
    if (this.IsDead())
      return;
    if (Object.op_Implicit((Object) this.emodel) && Object.op_Implicit((Object) this.emodel.avatarController))
    {
      this.emodel.avatarController.TriggerSleeperPose(_pose, _returningToSleep);
      this.pendingSleepTrigger = -1;
      if (_pose != 5)
        this.physicsHeight = 0.85f;
    }
    else
      this.pendingSleepTrigger = _pose;
    this.lastSleeperPose = _pose;
    this.IsSleeping = true;
    this.SleeperSupressLivingSounds = true;
    this.sleeperLookDir = Quaternion.op_Multiply(Quaternion.AngleAxis(this.rotation.y, Vector3.up), this.SleeperSpawnLookDir);
  }

  public void ResumeSleeperPose() => this.TriggerSleeperPose(this.lastSleeperPose, true);

  public void ConditionalTriggerSleeperWakeUp()
  {
    if (!this.IsSleeping || this.IsDead())
      return;
    this.IsSleeping = false;
    this.IsSleeperPassive = false;
    this.emodel.avatarController.TriggerSleeperPose((double) this.physicsHeight >= 1.0 || this.IsWalkTypeACrawl() ? -1 : -2);
    if (this.aiManager != null)
      this.aiManager.SleeperWokeUp();
    if (this.world.IsRemote())
      return;
    SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageSleeperWakeup>().Setup(this.entityId));
  }

  public void SetSleeperActive()
  {
    if (!this.IsSleeperPassive)
      return;
    this.IsSleeperPassive = false;
    if (this.world.IsRemote())
      return;
    SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageSleeperPassiveChange>().Setup(this.entityId));
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual void InitStats() => this.entityStats = new EntityStats(this);

  public void SetStats(EntityStats _stats) => this.entityStats.CopyFrom(_stats);

  public EntityStats Stats
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get => this.entityStats;
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual ItemValue GetHandItem() => this.handItem;

  public bool IsHoldingLight() => this.inventory.IsFlashlightOn;

  public void CycleActivatableItems()
  {
  }

  public List<ItemValue> GetActivatableItemPool()
  {
    List<ItemValue> _pool = new List<ItemValue>();
    this.CollectActivatableItems(_pool);
    return _pool;
  }

  public void CollectActivatableItems(List<ItemValue> _pool)
  {
    if (this.inventory != null)
      EntityAlive.GetActivatableItems(this.inventory.holdingItemItemValue, _pool);
    if (this.equipment == null)
      return;
    int slotCount = this.equipment.GetSlotCount();
    for (int index = 0; index < slotCount; ++index)
      EntityAlive.GetActivatableItems(this.equipment.GetSlotItemOrNone(index), _pool);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static void GetActivatableItems(ItemValue _item, List<ItemValue> _itemPool)
  {
    ItemClass itemClass1 = _item.ItemClass;
    if (itemClass1 == null)
      return;
    if (itemClass1.HasTrigger(MinEventTypes.onSelfItemActivate))
      _itemPool.Add(_item);
    for (int index = 0; index < _item.Modifications.Length; ++index)
    {
      ItemValue modification = _item.Modifications[index];
      if (modification != null)
      {
        ItemClass itemClass2 = modification.ItemClass;
        if (itemClass2 != null && itemClass2.HasTrigger(MinEventTypes.onSelfItemActivate))
          _itemPool.Add(modification);
      }
    }
  }

  public override void OnUpdatePosition(float _partialTicks)
  {
    float _rotYDelta = Utils.DeltaAngle(this.rotation.y, this.prevRotation.y);
    base.OnUpdatePosition(_partialTicks);
    Vector3 zero = Vector3.zero;
    for (int index = 0; index < this.lastTickPos.Length - 1; ++index)
    {
      zero.x += this.lastTickPos[index].x - this.lastTickPos[index + 1].x;
      zero.z += this.lastTickPos[index].z - this.lastTickPos[index + 1].z;
    }
    Vector3 _dist = Vector3.op_Division(Vector3.op_Addition(zero, Vector3.op_Subtraction(this.position, this.lastTickPos[0])), (float) this.lastTickPos.Length);
    if (Object.op_Equality((Object) this.AttachedToEntity, (Object) null))
      this.updateStepSound(_dist.x, _dist.z, _rotYDelta);
    if (this.RootMotion || this.isEntityRemote)
      return;
    this.updateSpeedForwardAndStrafe(_dist, _partialTicks);
  }

  public void Snore()
  {
    if (this.isSnore || !this.isGroan || this.snoreGroanCD > 0)
      return;
    this.isSnore = true;
    this.isGroan = false;
    this.snoreGroanCD = this.rand.RandomRange(20, 21);
    if (this.soundSleeperSnore == null || this.isGroanSilent)
      return;
    Manager.BroadcastPlay((Entity) this, this.soundSleeperSnore);
  }

  public void Groan()
  {
    if (this.isGroan || this.snoreGroanCD > 0)
      return;
    this.isGroan = true;
    this.isSnore = false;
    this.snoreGroanCD = this.rand.RandomRange(20, 21);
    if ((double) this.sleeperNoiseToSenseSoundChance >= 1.0 || (double) this.rand.RandomFloat <= (double) this.sleeperNoiseToSenseSoundChance)
    {
      this.isGroanSilent = false;
      if (this.soundSleeperGroan == null)
        return;
      Manager.BroadcastPlay((Entity) this, this.soundSleeperGroan);
    }
    else
      this.isGroanSilent = true;
  }

  public override void OnUpdateEntity()
  {
    base.OnUpdateEntity();
    this.Buffs.SetCustomVar("_underwater", this.inWaterPercent);
    if (this.Buffs != null)
      this.Buffs.Update(Time.deltaTime);
    this.OnUpdateLive();
    if (!this.IsSleeping && (!this.isEntityRemote || !(this is EntityPlayer)))
    {
      this.bag.OnUpdate();
      if (this.inventory != null)
        this.inventory.OnUpdate();
    }
    if (this.Health <= 0 && !this.IsDead() && !this.isEntityRemote && !this.IsGodMode.Value)
    {
      if (this.Buffs.HasBuff("drowning"))
        this.DamageEntity(DamageSource.suffocating, 1, false, 1f);
      else
        this.DamageEntity(DamageSource.disease, 1, false, 1f);
    }
    if (this.IsAlive() && this.bPlayHurtSound)
    {
      string soundHurt = this.GetSoundHurt(this.woundedDamageSource, this.woundedStrength);
      if (soundHurt != null)
        this.PlayOneShot(soundHurt);
    }
    this.bPlayHurtSound = false;
    this.bBeenWounded = false;
    this.woundedStrength = 0;
    this.woundedDamageSource = (DamageSource) null;
    if (this.snoreGroanCD > 0)
      --this.snoreGroanCD;
    if (!this.IsDead() && !this.isEntityRemote)
    {
      if (this.isRadiationSensitive() && this.biomeStandingOn != null && this.biomeStandingOn.m_RadiationLevel > 0 && !this.IsGodMode.Value && this.world.worldTime % 20UL == 0UL)
        this.DamageEntity(DamageSource.radiation, this.biomeStandingOn.m_RadiationLevel, false, 1f);
      if (this.hasAI)
      {
        if (this.IsSleeping && this.pendingSleepTrigger > -1)
          this.TriggerSleeperPose(this.pendingSleepTrigger);
        --this.soundDelayTicks;
        if (this.attackingTime <= 0 && this.soundDelayTicks <= 0 && (double) this.aiClosestPlayerDistSq <= 400.0 && this.bodyDamage.CurrentStun == EnumEntityStunType.None && !this.SleeperSupressLivingSounds)
        {
          if (this.targetAlertChanged)
          {
            this.targetAlertChanged = false;
            this.soundDelayTicks = this.GetSoundAlertTicks();
            if (this.GetSoundAlert() != null && !this.IsScoutZombie)
              this.PlayOneShot(this.GetSoundAlert());
            this.OnEntityTargeted(this.attackTarget);
          }
          else
          {
            this.soundDelayTicks = this.GetSoundRandomTicks();
            this.attackTargetLast = (EntityAlive) null;
            if (this.GetSoundRandom() != null)
              this.PlayOneShot(this.GetSoundRandom());
          }
        }
      }
    }
    if (this.hasBeenAttackedTime > 0)
      --this.hasBeenAttackedTime;
    if ((double) this.painResistPercent > 0.0)
    {
      this.painResistPercent -= 0.0100000007f;
      if ((double) this.painResistPercent <= 0.0)
        this.painHitsFelt = 0.0f;
    }
    if (this.attackingTime > 0)
    {
      --this.attackingTime;
      if (this.attackingTime == 0 && Object.op_Inequality((Object) this.attackTarget, (Object) null))
        this.LastTargetPos = this.attackTarget.GetPosition();
    }
    if (this.investigatePositionTicks > 0 && --this.investigatePositionTicks == 0)
      this.ClearInvestigatePosition();
    bool flag = this.IsDead();
    if (this.alertEnabled)
    {
      this.isAlert = this.bReplicatedAlertFlag;
      if (!this.isEntityRemote)
      {
        if (this.alertTicks > 0)
          --this.alertTicks;
        this.isAlert = !flag && (this.alertTicks > 0 || Object.op_Implicit((Object) this.attackTarget) || this.HasInvestigatePosition && this.isInvestigateAlert);
        if (this.bReplicatedAlertFlag != this.isAlert)
        {
          this.bReplicatedAlertFlag = this.isAlert;
          this.bEntityAliveFlagsChanged = true;
        }
      }
      if (!this.isAlert && !flag)
      {
        this.Buffs.SetCustomVar(EntityAlive.notAlertedId, 1f);
        this.notAlertDelayTicks = 4;
      }
      else
      {
        if (this.notAlertDelayTicks > 0)
          --this.notAlertDelayTicks;
        if (this.notAlertDelayTicks == 0)
          this.Buffs.SetCustomVar(EntityAlive.notAlertedId, 0.0f);
      }
    }
    if (flag)
      this.OnDeathUpdate();
    if (!Object.op_Inequality((Object) this.revengeEntity, (Object) null))
      return;
    if (!this.revengeEntity.IsAlive())
      this.SetRevengeTarget((EntityAlive) null);
    else if (this.revengeTimer > 0)
      --this.revengeTimer;
    else
      this.SetRevengeTarget((EntityAlive) null);
  }

  public override void KillLootContainer()
  {
    if (!this.isEntityRemote && this.IsDead() && !this.corpseBlockValue.isair && this.deathUpdateTime < this.timeStayAfterDeath)
      this.deathUpdateTime = this.timeStayAfterDeath - 1;
    base.KillLootContainer();
  }

  public override void Kill(DamageResponse _dmResponse)
  {
    this.NotifySleeperDeath();
    if (Object.op_Inequality((Object) this.AttachedToEntity, (Object) null))
      this.Detach();
    if (this.deathUpdateTime == 0)
    {
      string soundDeath = this.GetSoundDeath(_dmResponse.Source);
      if (soundDeath != null)
        this.PlayOneShot(soundDeath);
    }
    if (this.IsDead())
    {
      this.SetDead();
    }
    else
    {
      this.ClientKill(_dmResponse);
      base.Kill(_dmResponse);
    }
  }

  public override void SetDead()
  {
    base.SetDead();
    this.Stats.Health.Value = 0.0f;
  }

  public void NotifySleeperDeath()
  {
    if (this.isEntityRemote || !this.IsSleeper)
      return;
    this.world.NotifySleeperVolumesEntityDied(this);
  }

  public void ClearEntityThatKilledMe() => this.entityThatKilledMe = (EntityAlive) null;

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual void ClientKill(DamageResponse _dmResponse)
  {
    this.lastHitDirection = Utils.EnumHitDirection.Back;
    if (Object.op_Equality((Object) this.entityThatKilledMe, (Object) null) && _dmResponse.Source != null)
    {
      Entity entity = _dmResponse.Source.getEntityId() != -1 ? this.world.GetEntity(_dmResponse.Source.getEntityId()) : (Entity) null;
      if (this.Spawned && entity is EntityAlive)
        this.entityThatKilledMe = (EntityAlive) entity;
    }
    if (this.IsDead())
      return;
    this.SetDead();
    if (this.Buffs != null)
      this.Buffs.OnDeath(this.entityThatKilledMe, _dmResponse.Source != null && _dmResponse.Source.damageType == EnumDamageTypes.Crushing, _dmResponse.Source == null ? FastTags<TagGroup.Global>.Parse("crushing") : _dmResponse.Source.DamageTypeTag);
    if (this.Progression != null)
      this.Progression.OnDeath();
    EntityPlayer entityPlayer = this as EntityPlayer;
    this.AnalyticsSendDeath(_dmResponse);
    if (Object.op_Equality((Object) entityPlayer, (Object) null) && this.entityThatKilledMe is EntityPlayer && (double) EffectManager.GetValue(PassiveEffects.CelebrationKill, _entity: this.entityThatKilledMe) > 0.0)
    {
      this.HandleClientDeath(_dmResponse.Source != null ? _dmResponse.Source.BlockPosition : this.GetBlockPosition());
      this.OnEntityDeath();
      float lightBrightness = this.world.GetLightBrightness(this.GetBlockPosition());
      this.world.GetGameManager().SpawnParticleEffectServer(new ParticleEffect("confetti", this.position, lightBrightness, Color.white, (string) null, (Transform) null, false), this.entityId, _worldSpawn: true);
      Manager.BroadcastPlayByLocalPlayer(this.position, "twitch_celebrate");
      GameManager.Instance.World.RemoveEntity(this.entityId, EnumRemoveEntityReason.Killed);
    }
    else
    {
      this.HandleClientDeath(_dmResponse.Source != null ? _dmResponse.Source.BlockPosition : this.GetBlockPosition());
      this.OnEntityDeath();
      this.emodel.OnDeath(_dmResponse, this.world.ChunkClusters[0]);
    }
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual void HandleClientDeath(Vector3i attackPos)
  {
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual void OnEntityTargeted(EntityAlive target)
  {
  }

  public void ForceHoldingWeaponUpdate()
  {
    if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsConnected)
      return;
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
    {
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageHoldingItem>().Setup(this), _allButAttachedToEntityId: this.entityId);
    }
    else
    {
      if (this.entityId <= 0 || !Object.op_Inequality((Object) (this as EntityPlayerLocal), (Object) null))
        return;
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageHoldingItem>().Setup(this));
    }
  }

  public virtual void SetHoldingItemTransform(Transform _transform)
  {
    this.emodel.SetInRightHand(_transform);
    this.ForceHoldingWeaponUpdate();
  }

  public virtual void OnHoldingItemChanged()
  {
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual void UpdateCameraFOV(bool _bLerpPosition)
  {
  }

  public virtual int GetCameraFOV() => GamePrefs.GetInt(EnumGamePrefs.OptionsGfxFOV);

  public float GetWetnessPercentage()
  {
    float v1 = this.inWaterPercent;
    if ((double) this.Stats.AmountEnclosed < (double) WeatherParams.EnclosureDetectionThreshold)
      v1 = Utils.FastMax(Utils.FastMax(v1, WeatherManager.Instance.GetCurrentRainfallValue()), WeatherManager.Instance.GetCurrentSnowfallValue());
    return v1;
  }

  public float GetAmountEnclosed()
  {
    Vector3 position = this.position;
    position.y += 0.5f;
    Vector3i blockPos = World.worldToBlockPos(position);
    if ((uint) blockPos.y < (uint) byte.MaxValue)
    {
      IChunk chunkFromWorldPos = this.world.GetChunkFromWorldPos(blockPos);
      if (chunkFromWorldPos != null)
        return 1f - Utils.FastMax((float) chunkFromWorldPos.GetLight(blockPos.x, blockPos.y, blockPos.z, Chunk.LIGHT_TYPE.SUN), (float) chunkFromWorldPos.GetLight(blockPos.x, blockPos.y + 1, blockPos.z, Chunk.LIGHT_TYPE.SUN)) / 15f;
    }
    return 1f;
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public override void OnHeadUnderwaterStateChanged(bool _bUnderwater)
  {
    base.OnHeadUnderwaterStateChanged(_bUnderwater);
    if (_bUnderwater)
      this.FireEvent(MinEventTypes.onSelfWaterSubmerge);
    else
      this.FireEvent(MinEventTypes.onSelfWaterSurface);
  }

  public virtual bool JetpackActive
  {
    get => this.bJetpackActive;
    set
    {
      if (value == this.bJetpackActive)
        return;
      this.bJetpackActive = value;
      this.bEntityAliveFlagsChanged |= !this.isEntityRemote;
    }
  }

  public virtual bool JetpackWearing
  {
    get => this.bJetpackWearing;
    set
    {
      if (value == this.bJetpackWearing)
        return;
      this.bJetpackWearing = value;
      this.bEntityAliveFlagsChanged |= !this.isEntityRemote;
    }
  }

  public virtual bool ParachuteWearing
  {
    get => this.bParachuteWearing;
    set
    {
      if (value == this.bParachuteWearing)
        return;
      this.bParachuteWearing = value;
      this.bEntityAliveFlagsChanged |= !this.isEntityRemote;
    }
  }

  public virtual bool AimingGun
  {
    get
    {
      return Object.op_Inequality((Object) this.emodel.avatarController, (Object) null) && this.emodel.avatarController.TryGetBool(AvatarController.isAimingHash, out this.bAimingGun) && this.bAimingGun;
    }
    set
    {
      bool aimingGun = this.AimingGun;
      if (value != aimingGun)
      {
        if (Object.op_Inequality((Object) this.emodel.avatarController, (Object) null))
          this.emodel.avatarController.UpdateBool(AvatarController.isAimingHash, value);
        this.UpdateCameraFOV(true);
      }
      if (!(this is EntityPlayerLocal) || this.inventory == null)
        return;
      this.inventory.holdingItem.Actions[1]?.AimingSet(this.inventory.holdingItemData.actionData[1], value, aimingGun);
    }
  }

  public virtual Vector3 GetChestTransformPosition()
  {
    return this.IsCrouching || this.bodyDamage.CurrentStun == EnumEntityStunType.Kneel || this.bodyDamage.CurrentStun == EnumEntityStunType.Prone ? Vector3.op_Addition(((Component) this).transform.position, new Vector3(0.0f, this.GetEyeHeight() * 0.25f, 0.0f)) : Vector3.op_Addition(((Component) this).transform.position, new Vector3(0.0f, this.GetEyeHeight() * 0.95f, 0.0f));
  }

  public virtual bool MovementRunning
  {
    get => this.bMovementRunning;
    set
    {
      if (value == this.bMovementRunning)
        return;
      this.bMovementRunning = value;
    }
  }

  public virtual bool Crouching
  {
    get => this.bCrouching;
    set
    {
      if (value == this.bCrouching)
        return;
      this.bCrouching = value;
      if (Object.op_Inequality((Object) this.emodel.avatarController, (Object) null))
        this.emodel.avatarController.SetCrouching(value);
      this.CurrentStanceTag = this.bCrouching ? EntityAlive.StanceTagCrouching : EntityAlive.StanceTagStanding;
      this.Buffs.SetCustomVar("_crouching", this.bCrouching ? 1f : 0.0f);
      this.bEntityAliveFlagsChanged |= !this.isEntityRemote;
    }
  }

  public bool IsCrouching => this.Crouching || this.CrouchingLocked;

  public virtual bool Jumping
  {
    get
    {
      return this.bJumping && (double) EffectManager.GetValue(PassiveEffects.JumpStrength, _originalValue: 1f, _entity: this) != 0.0;
    }
    set
    {
      if (value == this.bJumping)
        return;
      this.bJumping = value;
      if (this.Jumping)
      {
        this.StartJump();
        this.CurrentMovementTag &= EntityAlive.MovementTagIdle;
        this.CurrentMovementTag |= EntityAlive.MovementTagJumping;
      }
      else
      {
        this.EndJump();
        this.CurrentMovementTag &= EntityAlive.MovementTagJumping;
        this.bJumping = false;
      }
      this.bEntityAliveFlagsChanged |= !this.isEntityRemote;
    }
  }

  public bool Climbing
  {
    get => this.bClimbing;
    set
    {
      if (value == this.bClimbing)
        return;
      this.bClimbing = value;
      this.bPlayerStatsChanged |= !this.isEntityRemote;
      if (this.bClimbing)
      {
        this.CurrentMovementTag &= EntityAlive.MovementTagIdle;
        this.CurrentMovementTag |= EntityAlive.MovementTagClimbing;
      }
      else
        this.CurrentMovementTag &= EntityAlive.MovementTagClimbing;
    }
  }

  public virtual bool CanNavigatePath()
  {
    return this.onGround || this.isSwimming || this.bInElevator || this.Climbing;
  }

  public AvatarController.ActionState GetAnimActionState()
  {
    return Object.op_Implicit((Object) this.emodel.avatarController) ? this.emodel.avatarController.GetActionState() : AvatarController.ActionState.None;
  }

  public virtual void StartAnimAction(int _animType)
  {
    if (!Object.op_Implicit((Object) this.emodel.avatarController))
      return;
    int actionState = (int) this.emodel.avatarController.GetActionState();
    if (_animType != 9999)
    {
      if (this.emodel.avatarController.IsActionActive())
        return;
    }
    else if (!this.emodel.avatarController.IsActionActive())
      return;
    this.bPlayerStatsChanged |= !this.isEntityRemote;
    this.emodel.avatarController.StartAction(_animType);
  }

  public virtual void ContinueAnimAction(int _animType)
  {
    if (!Object.op_Implicit((Object) this.emodel.avatarController))
      return;
    this.bPlayerStatsChanged |= !this.isEntityRemote;
    this.emodel.avatarController.StartAction(_animType);
  }

  public virtual bool RightArmAnimationAttack
  {
    get
    {
      return Object.op_Inequality((Object) this.emodel.avatarController, (Object) null) && this.emodel.avatarController.IsAnimationAttackPlaying();
    }
    set
    {
      if (!Object.op_Inequality((Object) this.emodel.avatarController, (Object) null) || !value || this.emodel.avatarController.IsAnimationAttackPlaying())
        return;
      this.emodel.avatarController.StartAnimationAttack();
    }
  }

  public virtual bool RightArmAnimationUse
  {
    get
    {
      return Object.op_Inequality((Object) this.emodel.avatarController, (Object) null) && this.emodel.avatarController.IsAnimationUsePlaying();
    }
    set
    {
      if (!Object.op_Inequality((Object) this.emodel.avatarController, (Object) null) || value == this.emodel.avatarController.IsAnimationUsePlaying())
        return;
      this.emodel.avatarController.StartAnimationUse();
    }
  }

  public virtual bool SpecialAttack
  {
    get
    {
      return Object.op_Inequality((Object) this.emodel.avatarController, (Object) null) && this.emodel.avatarController.IsAnimationSpecialAttackPlaying();
    }
    set
    {
      if (!Object.op_Inequality((Object) this.emodel.avatarController, (Object) null) || value == this.emodel.avatarController.IsAnimationSpecialAttackPlaying())
        return;
      this.bPlayerStatsChanged |= !this.isEntityRemote;
      this.emodel.avatarController.StartAnimationSpecialAttack(value, 0);
    }
  }

  public virtual bool SpecialAttack2
  {
    get
    {
      return Object.op_Inequality((Object) this.emodel.avatarController, (Object) null) && this.emodel.avatarController.IsAnimationSpecialAttack2Playing();
    }
    set
    {
      if (!(Object.op_Inequality((Object) this.emodel.avatarController, (Object) null) & value))
        return;
      this.bPlayerStatsChanged |= !this.isEntityRemote;
      this.emodel.avatarController.StartAnimationSpecialAttack2();
    }
  }

  public virtual bool Raging
  {
    get
    {
      return Object.op_Inequality((Object) this.emodel.avatarController, (Object) null) && this.emodel.avatarController.IsAnimationRagingPlaying();
    }
    set
    {
      if (!Object.op_Inequality((Object) this.emodel.avatarController, (Object) null) || !value || this.emodel.avatarController.IsAnimationRagingPlaying())
        return;
      this.emodel.avatarController.StartAnimationRaging();
    }
  }

  public virtual bool Electrocuted
  {
    get
    {
      return Object.op_Inequality((Object) this.emodel, (Object) null) && Object.op_Inequality((Object) this.emodel.avatarController, (Object) null) && (double) this.emodel.avatarController.GetAnimationElectrocuteRemaining() > 0.0;
    }
    set
    {
      if (!Object.op_Inequality((Object) this.emodel, (Object) null) || !Object.op_Inequality((Object) this.emodel.avatarController, (Object) null) || value == (double) this.emodel.avatarController.GetAnimationElectrocuteRemaining() > 0.40000000596046448)
        return;
      this.bPlayerStatsChanged |= !this.isEntityRemote;
      if (!value)
        return;
      this.emodel.avatarController.StartAnimationElectrocute(0.6f);
      this.emodel.avatarController.Electrocute(true);
    }
  }

  public virtual bool HarvestingAnimation
  {
    get
    {
      return Object.op_Inequality((Object) this.emodel.avatarController, (Object) null) && this.emodel.avatarController.IsAnimationHarvestingPlaying();
    }
    set => this.emodel.avatarController.UpdateBool("Harvesting", value);
  }

  public virtual void StartHarvestingAnim(float _length, bool _weaponFireTrigger)
  {
    if (!Object.op_Inequality((Object) this.emodel, (Object) null) || !Object.op_Inequality((Object) this.emodel.avatarController, (Object) null))
      return;
    this.emodel.avatarController.StartAnimationHarvesting(_length, _weaponFireTrigger);
  }

  public bool IsEating
  {
    get => this.m_isEating;
    set
    {
      if (value == this.m_isEating)
        return;
      this.m_isEating = value;
      this.bPlayerStatsChanged |= !this.isEntityRemote;
      if (!Object.op_Inequality((Object) this.emodel, (Object) null) || !Object.op_Inequality((Object) this.emodel.avatarController, (Object) null))
        return;
      if (this.m_isEating)
        this.emodel.avatarController.StartEating();
      else
        this.emodel.avatarController.StopEating();
    }
  }

  public virtual void SetVehicleAnimation(int _animHash, int _pose)
  {
    if (!Object.op_Implicit((Object) this.emodel) || !Object.op_Implicit((Object) this.emodel.avatarController))
      return;
    this.emodel.avatarController.SetVehicleAnimation(_animHash, _pose);
    this.bPlayerStatsChanged = !this.isEntityRemote;
    if (_pose != -1 || !(this.emodel.avatarController is AvatarLocalPlayerController avatarController))
      return;
    avatarController.TPVResetAnimPose();
  }

  public virtual int GetVehicleAnimation()
  {
    return Object.op_Implicit((Object) this.emodel) && Object.op_Implicit((Object) this.emodel.avatarController) ? this.emodel.avatarController.GetVehicleAnimation() : -1;
  }

  public virtual int Died
  {
    get => this.died;
    set
    {
      if (value == this.died)
        return;
      this.died = value;
      this.bPlayerStatsChanged |= !this.isEntityRemote;
    }
  }

  public virtual int Score
  {
    get => this.score;
    set
    {
      if (value == this.score)
        return;
      this.score = value;
      this.bPlayerStatsChanged |= !this.isEntityRemote;
    }
  }

  public virtual int KilledZombies
  {
    get => this.killedZombies;
    set
    {
      if (value == this.killedZombies)
        return;
      this.killedZombies = value;
      this.bPlayerStatsChanged |= !this.isEntityRemote;
    }
  }

  public virtual int KilledPlayers
  {
    get => this.killedPlayers;
    set
    {
      if (value == this.killedPlayers)
        return;
      this.killedPlayers = value;
      this.bPlayerStatsChanged |= !this.isEntityRemote;
    }
  }

  public virtual int TeamNumber
  {
    get => this.teamNumber;
    set
    {
      if (value == this.teamNumber)
        return;
      this.teamNumber = value;
      this.bPlayerStatsChanged |= !this.isEntityRemote;
      if (this.isEntityRemote)
        return;
      GameManager.Instance.GameMessage(EnumGameMessages.ChangedTeam, this, (EntityAlive) null);
    }
  }

  public override string EntityName => this.entityName;

  public override void SetEntityName(string _name)
  {
    if (_name.Equals(this.entityName))
      return;
    this.entityName = _name;
    this.bPlayerStatsChanged |= !this.isEntityRemote;
    this.HandleSetNavName();
  }

  public virtual int DeathHealth
  {
    get => this.deathHealth;
    set
    {
      if (value == this.deathHealth)
        return;
      this.deathHealth = value;
      this.bPlayerStatsChanged |= !this.isEntityRemote;
    }
  }

  public virtual bool Spawned
  {
    get => this.bSpawned;
    set
    {
      if (value == this.bSpawned)
        return;
      this.bSpawned = value;
      this.onSpawnStateChanged();
      this.bEntityAliveFlagsChanged |= !this.isEntityRemote;
    }
  }

  public bool IsBreakingBlocks
  {
    get => this.m_isBreakingBlocks;
    set
    {
      if (value == this.m_isBreakingBlocks)
        return;
      this.m_isBreakingBlocks = value;
      this.bPlayerStatsChanged |= !this.isEntityRemote;
    }
  }

  public override bool IsSpawned() => this.bSpawned;

  public virtual EntityBedrollPositionList SpawnPoints => this.spawnPoints;

  public virtual void RemoveIKTargets() => this.emodel.RemoveIKController();

  public virtual void SetIKTargets(List<IKController.Target> targets)
  {
    IKController ikController = this.emodel.AddIKController();
    if (!Object.op_Implicit((Object) ikController))
      return;
    ikController.SetTargets(targets);
  }

  public virtual List<Vector3i> GetDroppedBackpackPositions() => this.droppedBackpackPositions;

  public virtual Vector3i GetLastDroppedBackpackPosition()
  {
    if (this.droppedBackpackPositions == null || this.droppedBackpackPositions.Count == 0)
      return Vector3i.zero;
    List<Vector3i> backpackPositions = this.droppedBackpackPositions;
    return backpackPositions[backpackPositions.Count - 1];
  }

  public virtual bool EqualsDroppedBackpackPositions(Vector3i position)
  {
    if (this.droppedBackpackPositions != null)
    {
      foreach (Vector3i backpackPosition in this.droppedBackpackPositions)
      {
        if (position.Equals(backpackPosition))
          return true;
      }
    }
    return false;
  }

  public virtual void SetDroppedBackpackPositions(List<Vector3i> positions)
  {
    this.droppedBackpackPositions.Clear();
    if (positions == null)
      return;
    this.droppedBackpackPositions.AddRange((IEnumerable<Vector3i>) positions);
  }

  public virtual void ClearDroppedBackpackPositions() => this.droppedBackpackPositions.Clear();

  public virtual int Health
  {
    get => (int) this.Stats.Health.Value;
    set => this.Stats.Health.Value = (float) value;
  }

  public virtual float Stamina
  {
    get => this.Stats.Stamina.Value;
    set => this.Stats.Stamina.Value = value;
  }

  public virtual float Water
  {
    get => this.Stats.Water.Value;
    set => this.Stats.Water.Value = value;
  }

  public virtual int GetMaxHealth() => (int) this.Stats.Health.Max;

  public virtual int GetMaxStamina() => (int) this.Stats.Stamina.Max;

  public virtual int GetMaxWater() => (int) this.Stats.Water.Max;

  public virtual bool IsValidAimAssistSlowdownTarget => true;

  public virtual bool IsValidAimAssistSnapTarget => true;

  public virtual EModelBase.HeadStates CurrentHeadState
  {
    get => this.currentHeadState;
    set
    {
      if (value != this.currentHeadState)
      {
        this.currentHeadState = value;
        this.bPlayerStatsChanged |= !this.isEntityRemote;
      }
      this.emodel.ForceHeadState(value);
    }
  }

  public virtual float GetStaminaMultiplier() => 1f;

  [PublicizedFrom(EAccessModifier.Protected)]
  public override void SetMovementState()
  {
    float num1 = this.speedStrafe;
    if ((double) num1 >= 1234.0)
      num1 = 0.0f;
    float num2 = (float) ((double) this.speedForward * (double) this.speedForward + (double) num1 * (double) num1);
    this.MovementState = (double) num2 > (double) this.moveSpeedAggro * (double) this.moveSpeedAggro ? 3 : ((double) num2 > (double) this.moveSpeed * (double) this.moveSpeed ? 2 : ((double) num2 > 1.0 / 1000.0 ? 1 : 0));
  }

  public virtual void OnUpdateLive()
  {
    this.Stats.Health.RegenerationAmount = 0.0f;
    if (!this.isEntityRemote && !this.IsDead())
      this.Stats.Tick(this.world.worldTime);
    if (this.jumpTicks > 0)
      --this.jumpTicks;
    if (this.attackTargetTime > 0)
    {
      --this.attackTargetTime;
      if (Object.op_Inequality((Object) this.attackTarget, (Object) null) && this.attackTargetTime == 0)
      {
        this.attackTarget = (EntityAlive) null;
        if (!this.isEntityRemote)
          this.world.entityDistributer.SendPacketToTrackedPlayersAndTrackedEntity(this.entityId, -1, (NetPackage) NetPackageManager.GetPackage<NetPackageSetAttackTarget>().Setup(this.entityId, -1));
      }
    }
    this.updateCurrentBlockPosAndValue();
    if (Object.op_Equality((Object) this.AttachedToEntity, (Object) null))
    {
      if (this.isEntityRemote)
      {
        if (this.RootMotion)
          this.MoveEntityHeaded(Vector3.zero, false);
      }
      else
      {
        if (this.Health <= 0)
        {
          this.bJumping = false;
          this.bClimbing = false;
          this.moveDirection = Vector3.zero;
        }
        else if (!this.world.IsRemote() && !this.IsDead() && !this.IsClientControlled() && this.hasAI)
          this.updateTasks();
        this.noisePlayer = (EntityPlayer) null;
        this.noisePlayerDistance = 0.0f;
        this.noisePlayerVolume = 0.0f;
        if (this.bJumping)
          this.UpdateJump();
        else
          this.jumpTicks = 0;
        float landMovementFactor = this.landMovementFactor;
        this.landMovementFactor *= this.GetSpeedModifier();
        this.MoveEntityHeaded(this.moveDirection, this.isMoveDirAbsolute);
        this.landMovementFactor = landMovementFactor;
      }
      this.CurrentMovementTag = (double) this.moveDirection.x > 0.0 || (double) this.moveDirection.z > 0.0 ? (!this.bMovementRunning ? EntityAlive.MovementTagWalking : EntityAlive.MovementTagRunning) : EntityAlive.MovementTagIdle;
    }
    if (this.bodyDamage.CurrentStun != EnumEntityStunType.None && !this.emodel.IsRagdollActive && !this.IsDead())
    {
      if (this.bodyDamage.CurrentStun == EnumEntityStunType.Getup)
      {
        if (!Object.op_Implicit((Object) this.emodel.avatarController) || !this.emodel.avatarController.IsAnimationStunRunning())
          this.ClearStun();
      }
      else
      {
        this.bodyDamage.StunDuration -= 0.05f;
        if ((double) this.bodyDamage.StunDuration <= 0.0)
        {
          this.SetStun(EnumEntityStunType.Getup);
          if (Object.op_Implicit((Object) this.emodel.avatarController))
            this.emodel.avatarController.EndStun();
        }
      }
    }
    for (this.proneRefillCounter += 0.05f * this.proneRefillRate; (double) this.proneRefillCounter >= 1.0; --this.proneRefillCounter)
      this.bodyDamage.StunProne = Mathf.Max(0, this.bodyDamage.StunProne - 1);
    for (this.kneelRefillCounter += 0.05f * this.kneelRefillRate; (double) this.kneelRefillCounter >= 1.0; --this.kneelRefillCounter)
      this.bodyDamage.StunKnee = Mathf.Max(0, this.bodyDamage.StunKnee - 1);
    EntityPlayer primaryPlayer = (EntityPlayer) this.world.GetPrimaryPlayer();
    if (Object.op_Inequality((Object) primaryPlayer, (Object) null) && Object.op_Inequality((Object) primaryPlayer, (Object) this))
    {
      if (--this.ticksToCheckSeenByPlayer <= 0)
      {
        this.wasSeenByPlayer = primaryPlayer.CanSee(this);
        this.ticksToCheckSeenByPlayer = !this.wasSeenByPlayer ? 20 : 200;
      }
      else if (this.wasSeenByPlayer)
        primaryPlayer.SetCanSee(this);
    }
    if (this.onGround)
      this.disableFallBehaviorUntilOnGround = false;
    this.UpdateDynamicRagdoll();
    this.checkForTeleportOutOfTraderArea();
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public void checkForTeleportOutOfTraderArea()
  {
    if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer || GameManager.Instance.IsEditMode() || this.IsGodMode.Value || !(this is EntityPlayer) || (double) Time.time - (double) this.lastTimeTraderStationChecked <= 0.10000000149011612)
      return;
    this.lastTimeTraderStationChecked = Time.time;
    Vector3 position = this.position;
    position.y += 0.5f;
    Vector3i blockPos = World.worldToBlockPos(position);
    TraderArea traderAreaAt = this.world.GetTraderAreaAt(blockPos);
    if (traderAreaAt != null && traderAreaAt.IsInitialized)
    {
      EntityPlayer requester = this as EntityPlayer;
      bool flag = false;
      Vector3 vector3_1 = (Vector3) (traderAreaAt.ProtectPosition + traderAreaAt.ProtectSize * 0.5f);
      if (Object.op_Implicit((Object) requester) && this.world.IsWorldEvent(World.WorldEvent.BloodMoon))
        flag = true;
      Prefab.PrefabTeleportVolume tpVolume;
      if ((Object.op_Implicit((Object) requester) || this is EntityHuman) && traderAreaAt.IsWithinTeleportArea(position, out tpVolume))
      {
        flag = traderAreaAt.IsClosed;
        if (!flag && Object.op_Implicit((Object) requester) && (double) EffectManager.GetValue(PassiveEffects.NoTrader, _entity: this) == 1.0)
        {
          flag = true;
          vector3_1 = (Vector3) (this.world.GetPOIAtPosition((Vector3) blockPos).boundingBoxPosition + tpVolume.startPos + tpVolume.size * 0.5f);
        }
      }
      if (!flag)
        return;
      ++this.traderTeleportStreak;
      Vector3 vector3_2 = Vector3.op_Subtraction(this.GetPosition(), vector3_1);
      Vector3 normalized = ((Vector3) ref vector3_2).normalized;
      normalized.y = 0.0f;
      Vector3 teleportPosition = this.GetTeleportPosition(Vector3.op_Addition(this.GetPosition(), Vector3.op_Multiply(Vector3.op_Multiply(normalized, 5f), (float) this.traderTeleportStreak)), normalized);
      if (this.isEntityRemote)
        SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.ForEntityId(this.entityId).SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageTeleportPlayer>().Setup(teleportPosition));
      else if (Object.op_Implicit((Object) requester))
        requester.Teleport(teleportPosition);
      else if (Object.op_Inequality((Object) this.AttachedToEntity, (Object) null))
        this.AttachedToEntity.SetPosition(teleportPosition);
      else
        this.SetPosition(teleportPosition);
      if (!Object.op_Implicit((Object) requester))
        return;
      GameEventManager.Current.HandleAction("game_on_trader_teleport", requester, (Entity) requester, false);
    }
    else
      this.traderTeleportStreak = 1;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public Vector3 GetTeleportPosition(
    Vector3 _position,
    Vector3 _direction,
    float _directionIncrease = 5f,
    int _maxAttempts = 20)
  {
    Vector3 teleportPosition = _position;
    Vector3 _position1 = _position;
    bool flag = false;
    for (int index = 0; !flag && index < _maxAttempts; ++index)
    {
      flag = this.world.GetRandomSpawnPositionMinMaxToPosition(_position, 5, 10, 1, false, out _position1, this.entityId, _retryCount: 20, _checkLandClaim: true, _maxLandClaimType: EnumLandClaimOwner.Ally);
      _position = Vector3.op_Addition(_position, Vector3.op_Multiply(_direction, _directionIncrease));
    }
    if (flag)
      return _position1;
    Log.Warning("Trader teleport: Could not find a valid teleport position, returning original position");
    return teleportPosition;
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual void StartJump()
  {
    this.jumpState = EntityAlive.JumpState.Leap;
    this.jumpStateTicks = 0;
    this.jumpDistance = 1f;
    this.jumpHeightDiff = 0.0f;
    this.disableFallBehaviorUntilOnGround = true;
    if (this.isSwimming)
    {
      this.jumpState = EntityAlive.JumpState.SwimStart;
      if (!Object.op_Inequality((Object) this.emodel.avatarController, (Object) null))
        return;
      this.emodel.avatarController.SetSwim(true);
    }
    else
    {
      if (!Object.op_Inequality((Object) this.emodel.avatarController, (Object) null))
        return;
      this.emodel.avatarController.StartAnimationJump(AnimJumpMode.Start);
    }
  }

  public virtual void SetJumpDistance(float _distance, float _heightDiff)
  {
    this.jumpDistance = _distance;
    this.jumpHeightDiff = _heightDiff;
  }

  public virtual void SetSwimValues(float _durationTicks, Vector3 _motion)
  {
    this.jumpSwimDurationTicks = Mathf.Clamp((float) ((double) _durationTicks / (double) this.swimSpeed - 6.0), 3f, 20f);
    this.jumpSwimMotion = _motion;
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual void UpdateJump()
  {
    if (this.IsFlyMode.Value)
    {
      this.Jumping = false;
    }
    else
    {
      ++this.jumpStateTicks;
      switch (this.jumpState)
      {
        case EntityAlive.JumpState.Leap:
          if ((double) this.accumulatedRootMotion.y <= 0.004999999888241291 && (double) this.jumpStateTicks < (double) this.jumpDelay)
            break;
          this.StartJumpMotion();
          this.jumpTicks = 200;
          this.jumpState = EntityAlive.JumpState.Air;
          this.jumpStateTicks = 0;
          this.jumpIsMoving = true;
          break;
        case EntityAlive.JumpState.Air:
          if (!this.onGround && ((double) this.motionMultiplier >= 0.44999998807907104 || this.jumpStateTicks <= 40))
            break;
          this.jumpState = EntityAlive.JumpState.Land;
          this.jumpStateTicks = 0;
          this.jumpIsMoving = false;
          break;
        case EntityAlive.JumpState.Land:
          if (this.jumpStateTicks <= 5)
            break;
          this.Jumping = false;
          break;
        case EntityAlive.JumpState.SwimStart:
          if ((double) this.jumpStateTicks <= 6.0)
            break;
          this.jumpTicks = 100;
          this.jumpState = EntityAlive.JumpState.Swim;
          this.jumpStateTicks = 0;
          this.jumpIsMoving = true;
          this.StartJumpSwimMotion();
          break;
        case EntityAlive.JumpState.Swim:
          if (this.isSwimming && (double) this.jumpStateTicks < (double) this.jumpSwimDurationTicks)
            break;
          this.Jumping = false;
          break;
      }
    }
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual void StartJumpSwimMotion()
  {
    if ((double) this.inWaterPercent > 0.64999997615814209)
    {
      float v2 = Mathf.Sqrt((float) ((double) this.jumpSwimMotion.x * (double) this.jumpSwimMotion.x + (double) this.jumpSwimMotion.z * (double) this.jumpSwimMotion.z)) + 1f / 1000f;
      this.jumpSwimMotion.y = Utils.FastClamp(this.jumpSwimMotion.y, Mathf.Lerp(-0.6f, -0.05f, v2 * 0.8f), 1f);
      float swimDurationTicks = this.jumpSwimDurationTicks;
      float num1 = (float) (((double) swimDurationTicks - 1.0) * (double) this.world.Gravity * 0.02500000037252903 * 0.49990001320838928) / Mathf.Pow(0.91f, (float) (((double) swimDurationTicks - 3.0) * 0.9100000262260437 * 0.11500000208616257));
      float num2 = Mathf.LerpUnclamped(0.46f, 0.418600023f, (float) (((double) swimDurationTicks - 1.0) / 15.0));
      float num3 = Mathf.Pow(0.91f, (swimDurationTicks - 1f) * num2);
      float num4 = 1f / swimDurationTicks / num3;
      float num5 = num1 + this.jumpSwimMotion.y * num4;
      float num6 = num4 / Utils.FastMax(1f, v2);
      this.motion.x = this.jumpSwimMotion.x * num6;
      this.motion.z = this.jumpSwimMotion.z * num6;
      this.motion.y = num5;
    }
    else
      this.motion.y = 0.0f;
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public void FaceJumpTo()
  {
    Vector3 vector3 = Vector3.op_Subtraction(this.moveHelper.JumpToPos, this.position);
    double num = (double) this.SeekYaw(Mathf.Round((float) ((double) Mathf.Atan2(vector3.x, vector3.z) * 57.295780181884766 / 90.0)) * 90f, 0.0f, 0.0f);
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual void StartJumpMotion()
  {
    this.SetAirBorne(true);
    float num1 = (float) (int) (5.0 + (double) Mathf.Pow(this.jumpDistance * 8f, 0.5f));
    this.motion = Vector3.op_Multiply(this.GetForwardVector(), this.jumpDistance / num1);
    float num2 = (float) ((double) num1 * (double) this.world.Gravity * 0.5);
    this.motion.y = Utils.FastMax(num2 * 0.5f, num2 + this.jumpHeightDiff / num1);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void JumpMove()
  {
    this.accumulatedRootMotion = Vector3.zero;
    Vector3 motion = this.motion;
    this.entityCollision(this.motion);
    this.motion.x = motion.x;
    this.motion.z = motion.z;
    if ((double) this.motion.y != 0.0)
      this.motion.y = motion.y;
    if (this.jumpState == EntityAlive.JumpState.Air)
    {
      this.motion.y -= this.world.Gravity;
    }
    else
    {
      this.motion.x *= 0.91f;
      this.motion.z *= 0.91f;
      this.motion.y -= this.world.Gravity * 0.025f;
      this.motion.y *= 0.91f;
    }
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual void EndJump()
  {
    this.jumpState = EntityAlive.JumpState.Off;
    this.jumpIsMoving = false;
    if (this.isEntityRemote || !Object.op_Inequality((Object) this.emodel.avatarController, (Object) null))
      return;
    this.emodel.avatarController.StartAnimationJump(AnimJumpMode.Land);
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public override bool CalcIfSwimming()
  {
    return (double) this.inWaterPercent >= (this.onGround || this.Jumping ? 0.699999988079071 : 0.5);
  }

  public override void SwimChanged()
  {
    if (!Object.op_Implicit((Object) this.emodel.avatarController))
      return;
    this.emodel.avatarController.SetSwim(this.isSwimming);
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public override void Update()
  {
    base.Update();
    this.updateNetworkStats();
    if (!this.isEntityRemote && this.RootMotion && this.lerpForwardSpeed)
    {
      float num1 = 0.06935714f;
      if ((double) this.speedForward > 0.019419999793171883)
        num1 = this.speedForwardTargetStep;
      float num2 = Utils.FastMoveTowards(this.speedForward, this.speedForwardTarget, num1 * Time.deltaTime);
      if ((double) this.speedForward > 0.019419999793171883 && (double) num2 <= 0.019419999793171883)
        num2 = 0.01942f;
      this.speedForward = num2;
    }
    if (this.isHeadUnderwater != ((double) this.Buffs.GetCustomVar("_underwater") == 1.0))
      this.Buffs.SetCustomVar("_underwater", this.isHeadUnderwater ? 1f : 0.0f);
    this.MinEventContext.Area = this.boundingBox;
    this.MinEventContext.Biome = this.biomeStandingOn;
    this.MinEventContext.ItemValue = this.inventory.holdingItemItemValue;
    this.MinEventContext.BlockValue = this.blockValueStandingOn;
    this.MinEventContext.ItemInventoryData = this.inventory.holdingItemData;
    this.MinEventContext.Position = this.position;
    this.MinEventContext.Seed = this.entityId + Mathf.Abs(GameManager.Instance.World.Seed);
    this.MinEventContext.Transform = ((Component) this).transform;
    FastTags<TagGroup.Global>.CombineTags(EntityClass.list[this.entityClass].Tags, this.inventory.holdingItem.ItemTags, this.CurrentStanceTag, this.CurrentMovementTag, ref this.MinEventContext.Tags);
    if (this.Progression != null)
      this.Progression.Update();
    if ((double) this.renderFade == (double) this.renderFadeTarget)
      return;
    this.renderFade = Mathf.MoveTowards(this.renderFade, this.renderFadeTarget, Time.deltaTime);
    this.emodel.SetFade(this.renderFade);
    bool _bVisible = (double) this.renderFade > 0.0099999997764825821;
    if (this.emodel.visible == _bVisible)
      return;
    this.emodel.SetVisible(_bVisible);
  }

  public virtual void OnDeathUpdate()
  {
    if (this.deathUpdateTime < this.timeStayAfterDeath)
      ++this.deathUpdateTime;
    int deadBodyHitPoints = EntityClass.list[this.entityClass].DeadBodyHitPoints;
    if (deadBodyHitPoints > 0 && this.DeathHealth <= -deadBodyHitPoints)
      this.deathUpdateTime = this.timeStayAfterDeath;
    if (this.deathUpdateTime < this.timeStayAfterDeath || this.isEntityRemote || this.markedForUnload)
      return;
    this.dropCorpseBlock();
    if (this.particleOnDestroy == null || this.particleOnDestroy.Length <= 0)
      return;
    float lightBrightness = this.world.GetLightBrightness(this.GetBlockPosition());
    this.world.GetGameManager().SpawnParticleEffectServer(new ParticleEffect(this.particleOnDestroy, this.getHeadPosition(), lightBrightness, Color.white, (string) null, (Transform) null, false), this.entityId);
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual Vector3i dropCorpseBlock()
  {
    if (this.corpseBlockValue.isair || (double) this.rand.RandomFloat > (double) this.corpseBlockChance)
      return Vector3i.zero;
    Vector3i blockPos = World.worldToBlockPos(this.position);
    while (blockPos.y < 254 && (double) blockPos.y - (double) this.position.y < 3.0 && !this.corpseBlockValue.Block.CanPlaceBlockAt((WorldBase) this.world, 0, blockPos, this.corpseBlockValue))
      blockPos += Vector3i.up;
    if (blockPos.y >= 254 || (double) blockPos.y - (double) this.position.y >= 2.0999999046325684)
      return Vector3i.zero;
    this.world.SetBlockRPC(blockPos, this.corpseBlockValue);
    return blockPos;
  }

  public void NotifyRootMotion(Animator animator)
  {
    this.accumulatedRootMotion = Vector3.op_Addition(this.accumulatedRootMotion, animator.deltaPosition);
  }

  public virtual float MaxVelocity => 5f;

  [PublicizedFrom(EAccessModifier.Protected)]
  public void DefaultMoveEntity(Vector3 _direction, bool _isDirAbsolute)
  {
    float num1 = 0.91f;
    if (AIDirector.debugFreezePos && this.aiManager != null)
      this.motion = Vector3.zero;
    if (this.onGround)
    {
      num1 = 0.546f;
      if (!this.IsDead() && this is EntityPlayer)
      {
        BlockValue block = this.world.GetBlock(Utils.Fastfloor(this.position.x), Utils.Fastfloor(((Bounds) ref this.boundingBox).min.y), Utils.Fastfloor(this.position.z));
        if (block.isair || block.Block.blockMaterial.IsGroundCover)
          block = this.world.GetBlock(Utils.Fastfloor(this.position.x), Utils.Fastfloor(((Bounds) ref this.boundingBox).min.y - 1f), Utils.Fastfloor(this.position.z));
        if (!block.isair)
          num1 = Mathf.Clamp(1f - block.Block.blockMaterial.Friction, 0.01f, 1f);
      }
    }
    if (!this.RootMotion || !this.onGround && this.jumpTicks > 0)
    {
      float _velocity = !this.onGround ? this.jumpMovementFactor : this.landMovementFactor * (float) (0.16300000250339508 / ((double) num1 * (double) num1 * (double) num1));
      this.Move(_direction, _isDirAbsolute, _velocity, this.MaxVelocity);
    }
    if (this.Climbing)
    {
      this.fallDistance = 0.0f;
      this.entityCollision(this.motion);
      this.distanceClimbed += ((Vector3) ref this.motion).magnitude;
      if ((double) this.distanceClimbed > 0.5)
      {
        this.internalPlayStepSound(1f);
        this.distanceClimbed = 0.0f;
      }
    }
    else
    {
      if (this.IsInElevator())
      {
        if (!this.RootMotion)
        {
          float num2 = 0.15f;
          if ((double) this.motion.x < -(double) num2)
            this.motion.x = -num2;
          if ((double) this.motion.x > (double) num2)
            this.motion.x = num2;
          if ((double) this.motion.z < -(double) num2)
            this.motion.z = -num2;
          if ((double) this.motion.z > (double) num2)
            this.motion.z = num2;
        }
        this.fallDistance = 0.0f;
      }
      if (this.IsSleeping)
      {
        this.motion.x = 0.0f;
        this.motion.z = 0.0f;
      }
      this.entityCollision(this.motion);
    }
    if (this.isSwimming)
    {
      this.motion.x *= 0.91f;
      this.motion.z *= 0.91f;
      this.motion.y -= this.world.Gravity * 0.025f;
      this.motion.y *= 0.91f;
    }
    else
    {
      this.motion.x *= num1;
      this.motion.z *= num1;
      if (!this.bInElevator)
        this.motion.y -= this.world.Gravity;
      this.motion.y *= 0.98f;
    }
  }

  public virtual void MoveEntityHeaded(Vector3 _direction, bool _isDirAbsolute)
  {
    if (Object.op_Inequality((Object) this.AttachedToEntity, (Object) null))
      return;
    if (this.jumpIsMoving)
    {
      this.JumpMove();
    }
    else
    {
      if (this.RootMotion)
      {
        if (!this.isEntityRemote || this.bodyDamage.CurrentStun != EnumEntityStunType.None || this.IsDead() || Object.op_Inequality((Object) this.emodel, (Object) null) && Object.op_Inequality((Object) this.emodel.avatarController, (Object) null) && this.emodel.avatarController.IsAnimationHitRunning())
        {
          bool flag = Object.op_Implicit((Object) this.emodel) && this.emodel.IsRagdollActive;
          if (this.isSwimming && !flag)
            this.motion = Vector3.op_Addition(this.motion, Vector3.op_Multiply(this.accumulatedRootMotion, 1f / 1000f));
          else if (this.onGround || this.jumpTicks > 0)
          {
            if (flag)
            {
              this.motion.x = 0.0f;
              this.motion.z = 0.0f;
            }
            else
            {
              float y = this.motion.y;
              this.motion = this.accumulatedRootMotion;
              this.motion.y += y;
            }
          }
          this.accumulatedRootMotion = Vector3.zero;
        }
        else
        {
          this.accumulatedRootMotion = Vector3.zero;
          return;
        }
      }
      if (this.IsFlyMode.Value)
      {
        EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
        float num1 = Object.op_Inequality((Object) primaryPlayer, (Object) null) ? primaryPlayer.GodModeSpeedModifier : 1f;
        float num2 = (float) (2.0 * (this.MovementRunning ? 0.34999999403953552 : 0.11999999731779099)) * num1;
        if (!this.RootMotion)
          this.Move(_direction, _isDirAbsolute, this.GetPassiveEffectSpeedModifier() * num2, this.GetPassiveEffectSpeedModifier() * num2);
        if (!this.IsNoCollisionMode.Value)
        {
          this.entityCollision(this.motion);
          this.motion = Vector3.op_Multiply(this.motion, this.ConditionalScalePhysicsMulConstant(0.546f));
        }
        else
        {
          this.SetPosition(Vector3.op_Addition(this.position, this.motion));
          this.motion = Vector3.zero;
        }
      }
      else
        this.DefaultMoveEntity(_direction, _isDirAbsolute);
      if (this.isEntityRemote || !this.RootMotion)
        return;
      float num3 = this.landMovementFactor * 2.5f;
      if ((double) this.inWaterPercent > 0.30000001192092896)
      {
        if ((double) num3 > 0.0099999997764825821)
        {
          float num4 = (float) (((double) this.inWaterPercent - 0.30000001192092896) * 1.4285714626312256);
          num3 = Mathf.Lerp(num3, (float) (0.0099999997764825821 + ((double) num3 - 0.0099999997764825821) * 0.10000000149011612), num4);
        }
        if (this.isSwimming)
          num3 = this.landMovementFactor * 5f;
      }
      float magnitude = ((Vector3) ref _direction).magnitude;
      if ((double) magnitude > 1.0)
        num3 /= magnitude;
      float num5 = _direction.z * num3;
      if (this.lerpForwardSpeed)
      {
        if ((double) Utils.FastAbs(this.speedForwardTarget - num5) > 0.05000000074505806)
          this.speedForwardTargetStep = Utils.FastAbs(num5 - this.speedForward) / 0.18f;
        this.speedForwardTarget = num5;
      }
      else
        this.speedForward = num5;
      this.speedStrafe = _direction.x * num3;
      this.SetMovementState();
      this.ReplicateSpeeds();
    }
  }

  public float GetPassiveEffectSpeedModifier()
  {
    return this.IsCrouching ? (this.MovementRunning ? EffectManager.GetValue(PassiveEffects.WalkSpeed, _originalValue: Constants.cPlayerSpeedModifierWalking, _entity: this) : EffectManager.GetValue(PassiveEffects.CrouchSpeed, _originalValue: Constants.cPlayerSpeedModifierCrouching, _entity: this)) : (this.MovementRunning ? EffectManager.GetValue(PassiveEffects.RunSpeed, _originalValue: Constants.cPlayerSpeedModifierRunning, _entity: this) : EffectManager.GetValue(PassiveEffects.WalkSpeed, _originalValue: Constants.cPlayerSpeedModifierWalking, _entity: this));
  }

  public void SetMoveForward(float _moveForward)
  {
    this.moveDirection.x = 0.0f;
    this.moveDirection.z = _moveForward;
    this.isMoveDirAbsolute = false;
    this.Climbing = false;
    this.lerpForwardSpeed = true;
    this.motion.x = 0.0f;
    this.motion.z = 0.0f;
    this.accumulatedRootMotion.x = 0.0f;
    this.accumulatedRootMotion.z = 0.0f;
    if (!this.bInElevator)
      return;
    this.motion.y = 0.0f;
  }

  public void SetMoveForwardWithModifiers(float _speedModifier, float _speedScale, bool _climb)
  {
    this.moveDirection.x = 0.0f;
    this.moveDirection.z = 1f;
    this.isMoveDirAbsolute = false;
    this.Climbing = _climb;
    this.lerpForwardSpeed = true;
    float speedModifier = this.speedModifier;
    this.speedModifier = _speedModifier * _speedScale;
    if ((double) speedModifier <= 0.20000000298023224)
      return;
    float num = this.speedModifier / speedModifier;
    this.accumulatedRootMotion.x *= num;
    this.accumulatedRootMotion.z *= num;
  }

  public void AddMotion(float dir, float speed)
  {
    float num = dir * ((float) Math.PI / 180f);
    this.accumulatedRootMotion.x += Mathf.Sin(num) * speed;
    this.accumulatedRootMotion.z += Mathf.Cos(num) * speed;
  }

  public void MakeMotionMoveToward(float x, float z, float minMotion, float maxMotion)
  {
    if (this.RootMotion)
    {
      float num1 = Mathf.Sqrt((float) ((double) x * (double) x + (double) z * (double) z));
      if ((double) num1 > 0.0)
      {
        float num2 = Utils.FastClamp(Mathf.Sqrt((float) ((double) this.accumulatedRootMotion.x * (double) this.accumulatedRootMotion.x + (double) this.accumulatedRootMotion.z * (double) this.accumulatedRootMotion.z)), minMotion, maxMotion) / num1;
        if ((double) num2 < 1.0)
        {
          x *= num2;
          z *= num2;
        }
      }
      this.accumulatedRootMotion.x = x;
      this.accumulatedRootMotion.z = z;
    }
    else
    {
      this.moveDirection.x = x;
      this.moveDirection.z = z;
      this.isMoveDirAbsolute = true;
    }
  }

  public bool IsInFrontOfMe(Vector3 _position)
  {
    Vector3 headPosition = this.getHeadPosition();
    float angleBetween = Utils.GetAngleBetween(Vector3.op_Subtraction(_position, headPosition), this.GetForwardVector());
    float num = this.GetMaxViewAngle() * 0.5f;
    return (double) angleBetween >= -(double) num && (double) angleBetween <= (double) num;
  }

  public bool IsInViewCone(Vector3 _position)
  {
    Vector3 headPosition = this.getHeadPosition();
    Vector3 _dir1 = Vector3.op_Subtraction(_position, headPosition);
    Vector3 vector3;
    float num1;
    if (this.IsSleeping)
    {
      vector3 = this.sleeperLookDir;
      num1 = this.sleeperViewAngle;
    }
    else
    {
      vector3 = this.GetLookVector();
      num1 = this.GetMaxViewAngle();
    }
    float num2 = num1 * 0.5f;
    Vector3 _dir2 = vector3;
    float angleBetween = Utils.GetAngleBetween(_dir1, _dir2);
    return (double) angleBetween >= -(double) num2 && (double) angleBetween <= (double) num2;
  }

  public void DrawViewCone()
  {
    Vector3 vector3_1;
    float num1;
    if (this.IsSleeping)
    {
      vector3_1 = this.sleeperLookDir;
      num1 = this.sleeperViewAngle;
    }
    else
    {
      vector3_1 = this.GetLookVector();
      num1 = this.GetMaxViewAngle();
    }
    Vector3 vector3_2 = Vector3.op_Multiply(vector3_1, this.GetSeeDistance());
    float num2 = num1 * 0.5f;
    Vector3 vector3_3 = Vector3.op_Subtraction(this.getHeadPosition(), Origin.position);
    Debug.DrawRay(vector3_3, vector3_2, new Color(0.9f, 0.9f, 0.5f), 0.1f);
    Debug.DrawRay(vector3_3, Quaternion.op_Multiply(Quaternion.Euler(0.0f, -num2, 0.0f), vector3_2), new Color(0.6f, 0.6f, 0.3f), 0.1f);
    Debug.DrawRay(vector3_3, Quaternion.op_Multiply(Quaternion.Euler(0.0f, num2, 0.0f), vector3_2), new Color(0.6f, 0.6f, 0.3f), 0.1f);
  }

  public bool CanSee(Vector3 _pos)
  {
    Vector3 headPosition = this.getHeadPosition();
    Vector3 vector3 = Vector3.op_Subtraction(_pos, headPosition);
    float seeDistance = this.GetSeeDistance();
    if ((double) ((Vector3) ref vector3).magnitude > (double) seeDistance || !this.IsInViewCone(_pos))
      return false;
    Ray ray;
    // ISSUE: explicit constructor call
    ((Ray) ref ray).\u002Ector(headPosition, vector3);
    ref Ray local = ref ray;
    ((Ray) ref local).origin = Vector3.op_Addition(((Ray) ref local).origin, Vector3.op_Multiply(((Vector3) ref vector3).normalized, 0.2f));
    int modelLayer = this.GetModelLayer();
    this.SetModelLayer(2);
    bool flag = true;
    if (Voxel.Raycast(this.world, ray, seeDistance, false, false))
      flag = false;
    this.SetModelLayer(modelLayer);
    return flag;
  }

  public bool CanEntityBeSeen(Entity _other)
  {
    Vector3 headPosition1 = this.getHeadPosition();
    Vector3 headPosition2 = _other.getHeadPosition();
    Vector3 vector3 = Vector3.op_Subtraction(headPosition2, headPosition1);
    double magnitude = (double) ((Vector3) ref vector3).magnitude;
    float seeDistance = this.GetSeeDistance();
    if (_other is EntityPlayer entityPlayer)
      seeDistance *= entityPlayer.DetectUsScale(this);
    double num = (double) seeDistance;
    if (magnitude > num || !this.IsInViewCone(headPosition2))
      return false;
    bool flag = false;
    Ray ray;
    // ISSUE: explicit constructor call
    ((Ray) ref ray).\u002Ector(headPosition1, vector3);
    ref Ray local = ref ray;
    ((Ray) ref local).origin = Vector3.op_Addition(((Ray) ref local).origin, Vector3.op_Multiply(((Vector3) ref vector3).normalized, -0.1f));
    int modelLayer = this.GetModelLayer();
    this.SetModelLayer(2);
    if (Voxel.Raycast(this.world, ray, seeDistance, -1612492821, 64 /*0x40*/, 0.0f))
    {
      if (Voxel.voxelRayHitInfo.tag == "E_Vehicle")
      {
        EntityVehicle collisionEntity = EntityVehicle.FindCollisionEntity(Voxel.voxelRayHitInfo.transform);
        if (Object.op_Implicit((Object) collisionEntity) && collisionEntity.IsAttached(_other))
          flag = true;
      }
      else
      {
        if (Voxel.voxelRayHitInfo.tag.StartsWith("E_BP_"))
          Voxel.voxelRayHitInfo.transform = GameUtils.GetHitRootTransform(Voxel.voxelRayHitInfo.tag, Voxel.voxelRayHitInfo.transform);
        if (Object.op_Equality((Object) ((Component) _other).transform, (Object) Voxel.voxelRayHitInfo.transform))
          flag = true;
      }
    }
    this.SetModelLayer(modelLayer);
    return flag;
  }

  public virtual float GetSeeDistance()
  {
    this.senseScale = 1f;
    if (this.IsSleeping)
    {
      this.sightRange = this.sleeperSightRange;
      return this.sleeperSightRange;
    }
    this.sightRange = this.sightRangeBase;
    if (this.aiManager != null)
    {
      this.senseScale = (float) (1.0 + (double) EAIManager.CalcSenseScale() * (double) this.aiManager.feralSense);
      this.sightRange = this.sightRangeBase * this.senseScale;
    }
    return this.sightRange;
  }

  public bool CanSeeStealth(float dist, float lightLevel)
  {
    float num = Utils.FastLerp(this.sightLightThreshold.x, this.sightLightThreshold.y, dist / this.sightRange);
    return (double) lightLevel > (double) num;
  }

  public float GetSeeStealthDebugScale(float dist)
  {
    return Utils.FastLerp(this.sightLightThreshold.x, this.sightLightThreshold.y, dist / this.sightRange);
  }

  public override void SetAlive()
  {
    if (this.IsDead())
      this.lastAliveTime = Time.time;
    base.SetAlive();
    if (!this.isEntityRemote)
      this.Stats.ResetStats();
    this.Stats.Health.MaxModifier = 0.0f;
    this.Health = (int) this.Stats.Health.ModifiedMax;
    this.Stamina = this.Stats.Stamina.ModifiedMax;
    this.deathUpdateTime = 0;
    this.bDead = false;
    this.RecordedDamage.Fatal = false;
    this.emodel.SetAlive();
  }

  public float YawForTarget(Entity _otherEntity) => this.YawForTarget(_otherEntity.GetPosition());

  public float YawForTarget(Vector3 target)
  {
    float x = target.x - this.position.x;
    return (float) (-(Math.Atan2((double) target.z - (double) this.position.z, (double) x) * 180.0 / Math.PI) + 90.0);
  }

  public void RotateTo(Entity _otherEntity, float _dYaw, float _dPitch)
  {
    float x1 = _otherEntity.position.x - this.position.x;
    float y1 = _otherEntity.position.z - this.position.z;
    float y2;
    if (_otherEntity is EntityAlive)
    {
      EntityAlive entityAlive = (EntityAlive) _otherEntity;
      y2 = (float) ((double) this.position.y + (double) this.GetEyeHeight() - ((double) entityAlive.position.y + (double) entityAlive.GetEyeHeight()));
    }
    else
      y2 = (float) (((double) ((Bounds) ref _otherEntity.boundingBox).min.y + (double) ((Bounds) ref _otherEntity.boundingBox).max.y) / 2.0 - ((double) this.position.y + (double) this.GetEyeHeight()));
    float x2 = Mathf.Sqrt((float) ((double) x1 * (double) x1 + (double) y1 * (double) y1));
    float _intendedRotation = (float) (-(Math.Atan2((double) y1, (double) x1) * 180.0 / Math.PI) + 90.0);
    this.rotation.x = EntityAlive.UpdateRotation(this.rotation.x, (float) -(Math.Atan2((double) y2, (double) x2) * 180.0 / Math.PI), _dPitch);
    this.rotation.y = EntityAlive.UpdateRotation(this.rotation.y, _intendedRotation, _dYaw);
  }

  public void RotateTo(float _x, float _y, float _z, float _dYaw, float _dPitch)
  {
    float x1 = _x - this.position.x;
    float y = _z - this.position.z;
    float x2 = Mathf.Sqrt((float) ((double) x1 * (double) x1 + (double) y * (double) y));
    this.rotation.y = EntityAlive.UpdateRotation(this.rotation.y, (float) (-(Math.Atan2((double) y, (double) x1) * 180.0 / Math.PI) + 90.0), _dYaw);
    if ((double) _dPitch <= 0.0)
      return;
    this.rotation.x = -EntityAlive.UpdateRotation(this.rotation.x, (float) -(Math.Atan2((double) _y - (double) this.position.y, (double) x2) * 180.0 / Math.PI), _dPitch);
  }

  public static float UpdateRotation(float _curRotation, float _intendedRotation, float _maxIncr)
  {
    float num = _intendedRotation - _curRotation;
    while ((double) num < -180.0)
      num += 360f;
    while ((double) num >= 180.0)
      num -= 360f;
    if ((double) num > (double) _maxIncr)
      num = _maxIncr;
    if ((double) num < -(double) _maxIncr)
      num = -_maxIncr;
    return _curRotation + num;
  }

  public override float GetEyeHeight()
  {
    if (this.walkType == 21)
      return 0.15f;
    if (this.walkType == 22)
      return 0.6f;
    return !this.IsCrouching ? this.height * 0.8f : this.height * 0.5f;
  }

  public virtual float GetSpeedModifier() => this.speedModifier;

  [PublicizedFrom(EAccessModifier.Protected)]
  public override void fallHitGround(float _distance, Vector3 _fallMotion)
  {
    base.fallHitGround(_distance, _fallMotion);
    if ((double) _distance > 2.0)
    {
      int _strength = (int) ((-(double) _fallMotion.y - 0.85000002384185791) * 160.0);
      if (_strength > 0)
        this.DamageEntity(DamageSource.fall, _strength, false, 1f);
      this.PlayHitGroundSound();
    }
    if (!this.IsDead() && !this.emodel.IsRagdollActive && (this.disableFallBehaviorUntilOnGround || !this.ChooseFallBehavior(_distance, _fallMotion)) && Object.op_Implicit((Object) this.emodel) && Object.op_Implicit((Object) this.emodel.avatarController))
      this.emodel.avatarController.StartAnimationJump(AnimJumpMode.Land);
    if (this.aiManager == null)
      return;
    this.aiManager.FallHitGround(_distance);
  }

  public bool NotifyDestroyedBlock(ItemActionAttack.AttackHitInfo attackHitInfo)
  {
    if (attackHitInfo == null || this.moveHelper == null || this.moveHelper.BlockedFlags <= 0)
      return false;
    if (this.moveHelper.HitInfo.hit.blockPos == attackHitInfo.hitPosition)
      this.moveHelper.ClearBlocked();
    if (this._destroyBlockBehaviors.Count == 0)
      return false;
    float num1 = 0.0f;
    EntityAlive.weightBehaviorTemp.Clear();
    int num2 = GameStats.GetInt(EnumGameStats.GameDifficulty);
    for (int index = 0; index < this._destroyBlockBehaviors.Count; ++index)
    {
      EntityAlive.DestroyBlockBehavior destroyBlockBehavior = this._destroyBlockBehaviors[index];
      if (num2 >= destroyBlockBehavior.Difficulty.min && num2 <= destroyBlockBehavior.Difficulty.max)
      {
        EntityAlive.WeightBehavior weightBehavior;
        weightBehavior.weight = destroyBlockBehavior.Weight + num1;
        weightBehavior.index = index;
        EntityAlive.weightBehaviorTemp.Add(weightBehavior);
        num1 += destroyBlockBehavior.Weight;
      }
    }
    bool flag = false;
    if ((double) num1 > 0.0)
    {
      EntityAlive.DestroyBlockBehavior behavior = (EntityAlive.DestroyBlockBehavior) null;
      float num3 = this.rand.RandomFloat * num1;
      for (int index = 0; index < EntityAlive.weightBehaviorTemp.Count; ++index)
      {
        if ((double) num3 <= (double) EntityAlive.weightBehaviorTemp[index].weight)
        {
          behavior = this._destroyBlockBehaviors[EntityAlive.weightBehaviorTemp[index].index];
          break;
        }
      }
      if (behavior != null)
        flag = this.ExecuteDestroyBlockBehavior(behavior, attackHitInfo);
    }
    return flag;
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual bool ExecuteDestroyBlockBehavior(
    EntityAlive.DestroyBlockBehavior behavior,
    ItemActionAttack.AttackHitInfo attackHitInfo)
  {
    return false;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public bool ChooseFallBehavior(float _distance, Vector3 _fallMotion)
  {
    if (this.fallBehaviors.Count == 0)
      return false;
    float num1 = 0.0f;
    EntityAlive.weightBehaviorTemp.Clear();
    int num2 = GameStats.GetInt(EnumGameStats.GameDifficulty);
    for (int index = 0; index < this.fallBehaviors.Count; ++index)
    {
      EntityAlive.FallBehavior fallBehavior = this.fallBehaviors[index];
      if ((double) _distance >= (double) fallBehavior.Height.min && (double) _distance <= (double) fallBehavior.Height.max && num2 >= fallBehavior.Difficulty.min && num2 <= fallBehavior.Difficulty.max)
      {
        EntityAlive.WeightBehavior weightBehavior;
        weightBehavior.weight = fallBehavior.Weight + num1;
        weightBehavior.index = index;
        EntityAlive.weightBehaviorTemp.Add(weightBehavior);
        num1 += fallBehavior.Weight;
      }
    }
    bool flag = false;
    if ((double) num1 > 0.0)
    {
      EntityAlive.FallBehavior behavior = (EntityAlive.FallBehavior) null;
      float num3 = this.rand.RandomFloat * num1;
      for (int index = 0; index < EntityAlive.weightBehaviorTemp.Count; ++index)
      {
        if ((double) num3 <= (double) EntityAlive.weightBehaviorTemp[index].weight)
        {
          behavior = this.fallBehaviors[EntityAlive.weightBehaviorTemp[index].index];
          break;
        }
      }
      if (behavior != null)
        flag = this.ExecuteFallBehavior(behavior, _distance, _fallMotion);
    }
    return flag;
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual bool ExecuteFallBehavior(
    EntityAlive.FallBehavior behavior,
    float _distance,
    Vector3 _fallMotion)
  {
    return false;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void PlayHitGroundSound()
  {
    if (this.soundLand == null || this.soundLand.Length == 0)
      this.PlayOneShot("entityhitsground");
    else
      this.PlayOneShot(this.soundLand);
  }

  public virtual bool FriendlyFireCheck(EntityAlive other) => true;

  public virtual bool HasImmunity(BuffClass _buffClass) => false;

  public int CalculateBlockDamage(
    BlockDamage block,
    int defaultBlockDamage,
    out bool bypassMaxDamage)
  {
    if (this.stompsSpikes && block.HasTag(BlockTags.Spike))
    {
      bypassMaxDamage = true;
      return 999;
    }
    bypassMaxDamage = false;
    return defaultBlockDamage;
  }

  public override int DamageEntity(
    DamageSource _damageSource,
    int _strength,
    bool _criticalHit,
    float _impulseScale = 1f)
  {
    if (_damageSource.damageType == EnumDamageTypes.Suicide && Object.op_Implicit((Object) this.emodel) && this.emodel.avatarController is AvatarZombieController)
      (this.emodel.avatarController as AvatarZombieController).CleanupDismemberedLimbs();
    EnumDamageSource source = _damageSource.GetSource();
    if (_damageSource.IsIgnoreConsecutiveDamages() && source != EnumDamageSource.Internal)
    {
      if (this.damageSourceTimeouts.ContainsKey(source) && GameTimer.Instance.ticks - this.damageSourceTimeouts[source] < 30UL)
        return -1;
      this.damageSourceTimeouts[source] = GameTimer.Instance.ticks;
    }
    EntityAlive entity1 = this.world.GetEntity(_damageSource.getEntityId()) as EntityAlive;
    if (!this.FriendlyFireCheck(entity1))
      return -1;
    bool flag = _damageSource.GetDamageType() == EnumDamageTypes.Heat;
    if (!flag && Object.op_Implicit((Object) entity1) && (this.entityFlags & entity1.entityFlags & EntityFlags.Zombie) > EntityFlags.None || this.IsGodMode.Value)
      return -1;
    if (!this.IsDead() && Object.op_Implicit((Object) entity1))
    {
      float num = EffectManager.GetValue(PassiveEffects.DamageBonus, _entity: entity1);
      if ((double) num > 0.0)
      {
        _damageSource.DamageMultiplier = num;
        _damageSource.BonusDamageType = EnumDamageBonusType.Sneak;
      }
    }
    this.MinEventContext.Other = entity1;
    float num1 = Utils.FastMin(1f, EffectManager.GetValue(PassiveEffects.GeneralDamageResist, _entity: this));
    float v2 = (float) _strength * num1 + this.accumulatedDamageResisted;
    int num2 = Utils.FastMin(_strength, (int) v2);
    this.accumulatedDamageResisted = v2 - (float) num2;
    _strength -= num2;
    DamageResponse _dmResponse = this.damageEntityLocal(_damageSource, _strength, _criticalHit, _impulseScale);
    NetPackage _package = (NetPackage) NetPackageManager.GetPackage<NetPackageDamageEntity>().Setup(this.entityId, _dmResponse);
    if (this.world.IsRemote())
    {
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(_package);
    }
    else
    {
      int _excludePlayer = -1;
      if (!flag && _damageSource.CreatorEntityId != -2)
      {
        _excludePlayer = _damageSource.getEntityId();
        if (_damageSource.CreatorEntityId != -1)
        {
          Entity entity2 = this.world.GetEntity(_damageSource.CreatorEntityId);
          if (Object.op_Implicit((Object) entity2) && !entity2.isEntityRemote)
            _excludePlayer = -1;
        }
      }
      this.world.entityDistributer.SendPacketToTrackedPlayersAndTrackedEntity(this.entityId, _excludePlayer, _package);
    }
    return _dmResponse.ModStrength;
  }

  public virtual void SetDamagedTarget(EntityAlive _attackTarget)
  {
    this.damagedTarget = _attackTarget;
  }

  public virtual void ClearDamagedTarget() => this.damagedTarget = (EntityAlive) null;

  public EntityAlive GetDamagedTarget() => this.damagedTarget;

  public override bool IsDead() => base.IsDead() || this.RecordedDamage.Fatal;

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual DamageResponse damageEntityLocal(
    DamageSource _damageSource,
    int _strength,
    bool _criticalHit,
    float impulseScale)
  {
    DamageResponse _dmResponse = new DamageResponse();
    _dmResponse.Source = _damageSource;
    _dmResponse.Strength = _strength;
    _dmResponse.Critical = _criticalHit;
    _dmResponse.HitDirection = Utils.EnumHitDirection.None;
    _dmResponse.MovementState = this.MovementState;
    _dmResponse.Random = this.rand.RandomFloat;
    _dmResponse.ImpulseScale = impulseScale;
    _dmResponse.HitBodyPart = _damageSource.GetEntityDamageBodyPart((Entity) this);
    _dmResponse.ArmorSlot = _damageSource.GetEntityDamageEquipmentSlot((Entity) this);
    _dmResponse.ArmorSlotGroup = _damageSource.GetEntityDamageEquipmentSlotGroup((Entity) this);
    if (_strength > 0)
      _dmResponse.HitDirection = _damageSource.Equals((object) DamageSource.fall) ? Utils.EnumHitDirection.Back : (Utils.EnumHitDirection) Utils.Get4HitDirectionAsInt(_damageSource.getDirection(), this.GetLookVector());
    if (_damageSource.AffectedByArmor())
      this.equipment.CalcDamage(ref _dmResponse.Strength, ref _dmResponse.ArmorDamage, _dmResponse.Source.DamageTypeTag, this.MinEventContext.Other, _dmResponse.Source.AttackingItem);
    float damagePer = this.GetDamageFraction((float) _dmResponse.Strength);
    if (_dmResponse.Fatal || _dmResponse.Strength >= this.Health)
    {
      if ((_dmResponse.HitBodyPart & EnumBodyPartHit.Head) > EnumBodyPartHit.None)
      {
        if ((double) damagePer >= 0.20000000298023224)
          _dmResponse.Source.DismemberChance = Utils.FastMax(_dmResponse.Source.DismemberChance * 0.5f, 0.3f);
      }
      else if ((double) damagePer >= 0.11999999731779099)
        _dmResponse.Source.DismemberChance = Utils.FastMax(_dmResponse.Source.DismemberChance * 0.5f, 0.5f);
      damagePer = 1f;
      if (this.canDisintegrate)
        this.Disintegrate();
    }
    this.CheckDismember(ref _dmResponse, damagePer);
    int stunKnee = this.bodyDamage.StunKnee;
    int stunProne = this.bodyDamage.StunProne;
    if ((_dmResponse.HitBodyPart & EnumBodyPartHit.Head) > EnumBodyPartHit.None && _dmResponse.Dismember)
    {
      if (this.Health > 0)
        _dmResponse.Strength = this.Health;
    }
    else if (_damageSource.CanStun && this.GetWalkType() != 21 && this.bodyDamage.CurrentStun != EnumEntityStunType.Prone)
    {
      if ((_dmResponse.HitBodyPart & (EnumBodyPartHit.Arms | EnumBodyPartHit.Torso | EnumBodyPartHit.Head)) > EnumBodyPartHit.None)
        stunProne += _strength;
      else if (_dmResponse.HitBodyPart.IsLeg())
        stunKnee += _strength * (_criticalHit ? 2 : 1);
    }
    if ((!_dmResponse.HitBodyPart.IsLeg() || !_dmResponse.Dismember) && this.GetWalkType() != 21 && !this.sleepingOrWakingUp)
    {
      EntityClass entityClass = EntityClass.list[this.entityClass];
      if ((double) this.GetDamageFraction((float) stunProne) >= (double) entityClass.KnockdownProneDamageThreshold && (double) entityClass.KnockdownProneDamageThreshold > 0.0)
      {
        if (this.bodyDamage.CurrentStun != EnumEntityStunType.Prone)
        {
          _dmResponse.Stun = EnumEntityStunType.Prone;
          _dmResponse.StunDuration = this.rand.RandomRange(entityClass.KnockdownProneStunDuration.x, entityClass.KnockdownProneStunDuration.y);
        }
      }
      else if ((double) this.GetDamageFraction((float) stunKnee) >= (double) entityClass.KnockdownKneelDamageThreshold && (double) entityClass.KnockdownKneelDamageThreshold > 0.0 && this.bodyDamage.CurrentStun != EnumEntityStunType.Prone)
      {
        _dmResponse.Stun = EnumEntityStunType.Kneel;
        _dmResponse.StunDuration = this.rand.RandomRange(entityClass.KnockdownKneelStunDuration.x, entityClass.KnockdownKneelStunDuration.y);
      }
    }
    bool flag = false;
    int num1 = _dmResponse.Strength + _dmResponse.ArmorDamage / 2;
    if (num1 > 0 && !this.IsGodMode.Value && _dmResponse.Stun == EnumEntityStunType.None && !this.sleepingOrWakingUp)
    {
      flag = _dmResponse.Strength < this.Health;
      if (flag)
        flag = this.GetWalkType() == 21 || !_dmResponse.Dismember || !_dmResponse.HitBodyPart.IsLeg();
      if (flag && _dmResponse.Source.GetDamageType() != EnumDamageTypes.Bashing)
        flag = num1 >= 6;
      if (_dmResponse.Source.GetDamageType() == EnumDamageTypes.BarbedWire)
        flag = true;
    }
    _dmResponse.PainHit = flag;
    if (_dmResponse.Strength >= this.Health)
      _dmResponse.Fatal = true;
    if (_dmResponse.Fatal)
      _dmResponse.Stun = EnumEntityStunType.None;
    if (this.isEntityRemote)
    {
      _dmResponse.ModStrength = 0;
    }
    else
    {
      if (this.Health <= _dmResponse.Strength)
        _strength -= this.Health;
      _dmResponse.ModStrength = _strength;
    }
    if (_dmResponse.Dismember)
    {
      EntityAlive entity = this.world.GetEntity(_dmResponse.Source.getEntityId()) as EntityAlive;
      if (Object.op_Inequality((Object) entity, (Object) null))
        entity.FireEvent(MinEventTypes.onDismember);
    }
    if (Object.op_Inequality((Object) this.MinEventContext.Other, (Object) null))
    {
      this.MinEventContext.Other.MinEventContext.DamageResponse = _dmResponse;
      float num2 = EffectManager.GetValue(PassiveEffects.HealthSteal, _entity: this.MinEventContext.Other);
      if ((double) num2 != 0.0)
      {
        int _v = (int) ((double) num1 * (double) num2);
        if (_v + this.MinEventContext.Other.Health <= 0)
          _v = (this.MinEventContext.Other.Health - 1) * -1;
        this.MinEventContext.Other.AddHealth(_v);
        if (_v < 0 && this.MinEventContext.Other is EntityPlayerLocal)
          ((EntityPlayerLocal) this.MinEventContext.Other).ForceBloodSplatter();
      }
    }
    if (_dmResponse.Source.BuffClass == null || this.Progression != null)
    {
      this.MinEventContext.DamageResponse = _dmResponse;
      EntityAlive entity = this.world.GetEntity(_damageSource.getEntityId()) as EntityAlive;
      if (Object.op_Implicit((Object) entity) && !entity.isEntityRemote)
        this.MinEventContext.IsLocal = this is EntityPlayer && this.isEntityRemote;
      if (_dmResponse.Source.BuffClass == null)
        this.FireEvent(MinEventTypes.onOtherAttackedSelf);
      else if (this.Progression != null)
        this.Progression.FireEvent(MinEventTypes.onOtherAttackedSelf, this.MinEventContext);
      this.MinEventContext.IsLocal = false;
    }
    this.ProcessDamageResponseLocal(_dmResponse);
    return _dmResponse;
  }

  public virtual bool IsImmuneToLegDamage
  {
    get
    {
      EntityClass entityClass = EntityClass.list[this.entityClass];
      if (this.GetWalkType() == 21 || !this.bodyDamage.HasLeftLeg || !this.bodyDamage.HasRightLeg)
        return true;
      return (double) entityClass.LowerLegDismemberThreshold <= 0.0 && (double) entityClass.UpperLegDismemberThreshold <= 0.0;
    }
  }

  public override void ProcessDamageResponse(DamageResponse _dmResponse)
  {
    if ((double) Time.time - (double) this.lastAliveTime < 1.0)
      return;
    base.ProcessDamageResponse(_dmResponse);
    this.ProcessDamageResponseLocal(_dmResponse);
    if (this.world.IsRemote())
      return;
    Entity entity = this.world.GetEntity(_dmResponse.Source.getEntityId());
    if (Object.op_Implicit((Object) entity) && !entity.isEntityRemote && this.isEntityRemote && this is EntityPlayer)
      this.world.entityDistributer.SendPacketToTrackedPlayers(this.entityId, this.entityId, (NetPackage) NetPackageManager.GetPackage<NetPackageDamageEntity>().Setup(this.entityId, _dmResponse));
    else if (_dmResponse.Source.BuffClass != null)
      this.world.entityDistributer.SendPacketToTrackedPlayers(this.entityId, this.entityId, (NetPackage) NetPackageManager.GetPackage<NetPackageDamageEntity>().Setup(this.entityId, _dmResponse));
    else
      this.world.entityDistributer.SendPacketToTrackedPlayersAndTrackedEntity(this.entityId, _dmResponse.Source.getEntityId(), (NetPackage) NetPackageManager.GetPackage<NetPackageDamageEntity>().Setup(this.entityId, _dmResponse));
  }

  public virtual void ProcessDamageResponseLocal(DamageResponse _dmResponse)
  {
    if (Object.op_Equality((Object) this.emodel, (Object) null))
      return;
    if (_dmResponse.Source.BonusDamageType != EnumDamageBonusType.None)
    {
      EntityPlayerLocal primaryPlayer = this.world.GetPrimaryPlayer();
      if (Object.op_Implicit((Object) primaryPlayer) && primaryPlayer.entityId == _dmResponse.Source.getEntityId())
      {
        switch (_dmResponse.Source.BonusDamageType)
        {
          case EnumDamageBonusType.Sneak:
            primaryPlayer.NotifySneakDamage(_dmResponse.Source.DamageMultiplier);
            break;
          case EnumDamageBonusType.Stun:
            primaryPlayer.NotifyDamageMultiplier(_dmResponse.Source.DamageMultiplier);
            break;
        }
      }
    }
    EntityAlive entity1 = this.world.GetEntity(_dmResponse.Source.getEntityId()) as EntityAlive;
    if (Object.op_Inequality((Object) entity1, (Object) null))
      entity1.SetDamagedTarget(this);
    if (this.IsSleeperPassive)
      this.world.CheckSleeperVolumeNoise(this.position);
    this.ConditionalTriggerSleeperWakeUp();
    this.SleeperSupressLivingSounds = false;
    this.bPlayHurtSound = false;
    if (Object.op_Inequality((Object) this.AttachedToEntity, (Object) null) && !_dmResponse.Source.bIsDamageTransfer && _dmResponse.Source.GetSource() != EnumDamageSource.Internal)
    {
      _dmResponse.Source.bIsDamageTransfer = true;
      this.AttachedToEntity.DamageEntity(_dmResponse.Source, _dmResponse.Strength, _dmResponse.Critical, _dmResponse.ImpulseScale);
    }
    else
    {
      if (this.equipment != null && _dmResponse.ArmorDamage > 0)
      {
        List<ItemValue> armor = this.equipment.GetArmor();
        if (armor.Count > 0)
        {
          float _originalValue = (float) _dmResponse.ArmorDamage / (float) armor.Count;
          if ((double) _originalValue < 1.0 && (double) _originalValue != 0.0)
            _originalValue = 1f;
          for (int index = 0; index < armor.Count; ++index)
            armor[index].UseTimes += EffectManager.GetValue(PassiveEffects.DegradationPerUse, armor[index], _originalValue, this, tags: armor[index].ItemClass.ItemTags);
        }
      }
      this.ApplyLocalBodyDamage(_dmResponse);
      this.lastHitRanged = false;
      this.lastDamageResponse = _dmResponse;
      bool flag1 = (double) EffectManager.GetValue(PassiveEffects.NegateDamageSelf, _entity: this, tags: FastTags<TagGroup.Global>.Parse(_dmResponse.HitBodyPart.ToString())) > 0.0 || (double) EffectManager.GetValue(PassiveEffects.NegateDamageOther, Object.op_Inequality((Object) entity1, (Object) null) ? entity1.inventory.holdingItemItemValue : (ItemValue) null, _entity: entity1) > 0.0;
      if (_dmResponse.Dismember && !flag1)
      {
        this.lastHitImpactDir = _dmResponse.Source.getDirection();
        if (Object.op_Inequality((Object) entity1, (Object) null))
          this.lastHitEntityFwd = entity1.GetForwardVector();
        if (_dmResponse.Source.ItemClass != null && _dmResponse.Source.ItemClass.HasAnyTags(DismembermentManager.rangedTags))
          this.lastHitRanged = true;
        if (_dmResponse.Source.ItemClass != null)
        {
          float strength = (float) _dmResponse.ModStrength / (float) this.GetMaxHealth();
          this.lastHitForce = DismembermentManager.GetImpactForce(_dmResponse.Source.ItemClass, strength);
        }
        this.ExecuteDismember(false);
      }
      bool flag2 = _dmResponse.Stun != 0;
      bool flag3 = this.bodyDamage.CurrentStun != 0;
      if (!flag1 && _dmResponse.Fatal && this.isEntityRemote)
        this.ClientKill(_dmResponse);
      else if (flag2 && Object.op_Implicit((Object) this.emodel.avatarController))
      {
        if (_dmResponse.Stun == EnumEntityStunType.Prone)
        {
          if (this.bodyDamage.CurrentStun == EnumEntityStunType.None)
          {
            if (_dmResponse.Critical && _dmResponse.Source.damageType == EnumDamageTypes.Bashing || (double) this.rand.RandomFloat < 0.60000002384185791)
              this.DoRagdoll(_dmResponse);
            else
              this.emodel.avatarController.BeginStun(EnumEntityStunType.Prone, _dmResponse.HitBodyPart, _dmResponse.HitDirection, _dmResponse.Critical, _dmResponse.Random);
            this.SetStun(EnumEntityStunType.Prone);
            this.bodyDamage.StunDuration = _dmResponse.StunDuration;
          }
          else if (this.bodyDamage.CurrentStun != EnumEntityStunType.Prone)
          {
            this.DoRagdoll(_dmResponse);
            this.SetStun(EnumEntityStunType.Prone);
            this.bodyDamage.StunDuration = _dmResponse.StunDuration * 0.5f;
          }
        }
        else if (_dmResponse.Stun == EnumEntityStunType.Kneel)
        {
          bool flag4 = false;
          if (this.bodyDamage.CurrentStun == EnumEntityStunType.None)
          {
            if (_dmResponse.Critical || (double) this.rand.RandomFloat < 0.25)
            {
              flag4 = true;
            }
            else
            {
              this.SetStun(EnumEntityStunType.Kneel);
              this.emodel.avatarController.BeginStun(EnumEntityStunType.Kneel, _dmResponse.HitBodyPart, _dmResponse.HitDirection, _dmResponse.Critical, _dmResponse.Random);
            }
          }
          else if (this.bodyDamage.CurrentStun == EnumEntityStunType.Kneel)
            flag4 = true;
          if (flag4)
          {
            this.DoRagdoll(_dmResponse);
            this.SetStun(EnumEntityStunType.Prone);
          }
          this.bodyDamage.StunDuration = _dmResponse.StunDuration;
        }
      }
      else if (_dmResponse.PainHit && !flag3 && Object.op_Implicit((Object) this.emodel.avatarController))
      {
        EntityClass entityClass = EntityClass.list[this.entityClass];
        float num = entityClass.PainResistPerHit;
        if ((double) num >= 0.0)
        {
          float maxHealth = (float) this.GetMaxHealth();
          if ((double) this.Health / (double) maxHealth < (double) entityClass.PainResistPerHitLowHealthPercent)
            num = entityClass.PainResistPerHitLowHealth;
          this.painResistPercent = Utils.FastMin(this.painResistPercent + num, 3f);
          float _duration = float.MaxValue;
          if ((double) this.painResistPercent >= 3.0 && (double) num >= 1.0)
          {
            _duration = 0.0f;
            this.painHitsFelt += 0.15f;
          }
          else if ((double) this.painResistPercent >= 1.0)
          {
            _duration = Utils.FastLerp(0.5f, 0.15f, (float) (((double) this.painResistPercent - 1.0) * 0.75));
            this.painHitsFelt += 0.3f;
          }
          else
            this.painHitsFelt += Utils.FastLerp(1f, 0.3f, this.painResistPercent);
          this.emodel.avatarController.StartAnimationHit(_dmResponse.HitBodyPart, (int) _dmResponse.HitDirection, (int) ((double) _dmResponse.Strength * 100.0 / (double) maxHealth), _dmResponse.Critical, _dmResponse.MovementState, _dmResponse.Random, _duration);
        }
      }
      if (this.bodyDamage.CurrentStun == EnumEntityStunType.None)
      {
        if (_dmResponse.Source.CanStun)
        {
          if ((_dmResponse.HitBodyPart & (EnumBodyPartHit.Arms | EnumBodyPartHit.Torso | EnumBodyPartHit.Head)) > EnumBodyPartHit.None)
            this.bodyDamage.StunProne += _dmResponse.Strength;
          else if (_dmResponse.HitBodyPart.IsLeg())
            this.bodyDamage.StunKnee += _dmResponse.Strength;
        }
      }
      else
      {
        this.bodyDamage.StunProne = 0;
        this.bodyDamage.StunKnee = 0;
      }
      bool flag5 = this.Health <= 0;
      if (this.Health <= 0 && this.deathUpdateTime > 0)
        this.DeathHealth -= _dmResponse.Strength;
      int num1 = _dmResponse.Strength;
      if ((double) EffectManager.GetValue(PassiveEffects.HeadShotOnly, _entity: GameManager.Instance.World.GetEntity(_dmResponse.Source.getEntityId()) as EntityAlive) > 0.0 && (_dmResponse.HitBodyPart & EnumBodyPartHit.Head) == EnumBodyPartHit.None)
      {
        num1 = 0;
        _dmResponse.Fatal = false;
      }
      if (flag1)
      {
        num1 = 0;
        _dmResponse.Fatal = false;
      }
      if (this.isEntityRemote)
      {
        this.Health -= num1;
        this.RecordedDamage = _dmResponse;
      }
      else
      {
        if (!this.IsGodMode.Value)
        {
          this.Health -= num1;
          if (_dmResponse.Fatal && this.Health > 0)
            this.Health = 0;
          this.hasBeenAttackedTime = 0;
          if (_dmResponse.PainHit)
            this.hasBeenAttackedTime = this.GetMaxAttackTime();
        }
        this.bPlayHurtSound = this.bBeenWounded = num1 > 0;
        if (this.bBeenWounded)
        {
          this.setBeenAttacked();
          this.MinEventContext.Other = GameManager.Instance.World.GetEntity(_dmResponse.Source.getEntityId()) as EntityAlive;
          this.FireEvent(MinEventTypes.onOtherDamagedSelf);
        }
        if (num1 > this.woundedStrength)
        {
          this.woundedStrength = _dmResponse.Strength;
          this.woundedDamageSource = _dmResponse.Source;
        }
        this.lastHitDirection = _dmResponse.HitDirection;
        if (this.Health <= 0)
        {
          _dmResponse.Source.getDirection();
          _dmResponse.Strength += this.Health;
          Entity entity2 = _dmResponse.Source.getEntityId() != -1 ? this.world.GetEntity(_dmResponse.Source.getEntityId()) : (Entity) null;
          if (this.Spawned && !flag5)
            this.entityThatKilledMe = !(entity2 is EntityAlive) ? (EntityAlive) null : (EntityAlive) entity2;
          this.Kill(_dmResponse);
          if (!_dmResponse.Fatal && this.world.IsRemote())
            this.DamageEntity(DamageSource.disease, 1, false, 1f);
        }
      }
      Entity entity3 = _dmResponse.Source.getEntityId() != -1 ? this.world.GetEntity(_dmResponse.Source.getEntityId()) : (Entity) null;
      if (Object.op_Inequality((Object) entity3, (Object) null) && Object.op_Inequality((Object) entity3, (Object) this))
      {
        if (entity3 is EntityAlive && !this.isEntityRemote && !entity3.IsIgnoredByAI())
        {
          this.SetRevengeTarget((EntityAlive) entity3);
          if (this.aiManager != null)
            this.aiManager.DamagedByEntity();
        }
        if (entity3 is EntityPlayer)
          ((EntityAlive) entity3).FireEvent(MinEventTypes.onCombatEntered);
        this.FireEvent(MinEventTypes.onCombatEntered);
      }
      if (_dmResponse.Strength > 0 && _dmResponse.Source.GetDamageType() == EnumDamageTypes.Electrical)
        this.Electrocuted = true;
      this.RecordedDamage = _dmResponse;
    }
  }

  public bool CanUseHeavyArmorSound()
  {
    foreach (ItemValue itemValue in this.equipment.GetArmor())
    {
      if (itemValue.ItemClass.MadeOfMaterial.id == "MarmorHeavy")
        return true;
    }
    return false;
  }

  public EntityAlive GetRevengeTarget() => this.revengeEntity;

  public void SetRevengeTarget(EntityAlive _other)
  {
    this.revengeEntity = _other;
    this.revengeTimer = Object.op_Equality((Object) this.revengeEntity, (Object) null) ? 0 : 500;
  }

  public void SetRevengeTimer(int ticks) => this.revengeTimer = ticks;

  public override bool CanBePushed() => !this.IsDead();

  public override bool CanCollideWith(Entity _other)
  {
    return !this.IsDead() && !(_other is EntityItem) && !(_other is EntitySupplyCrate);
  }

  public override bool CanCollideWithBlocks() => !this.IsSleeping;

  public void DoRagdoll(DamageResponse _dmResponse)
  {
    this.emodel.DoRagdoll(_dmResponse, _dmResponse.StunDuration);
  }

  public void AddScore(
    int _diedMySelfTimes,
    int _zombieKills,
    int _playerKills,
    int _otherTeamnumber,
    int _conditions)
  {
    this.KilledZombies += _zombieKills;
    this.KilledPlayers += _playerKills;
    this.Died += _diedMySelfTimes;
    this.Score += _zombieKills * GameStats.GetInt(EnumGameStats.ScoreZombieKillMultiplier) + _playerKills * GameStats.GetInt(EnumGameStats.ScorePlayerKillMultiplier) + _diedMySelfTimes * GameStats.GetInt(EnumGameStats.ScoreDiedMultiplier);
    if (this.Score < 0)
      this.Score = 0;
    if (!(this is EntityPlayerLocal))
      return;
    if (_diedMySelfTimes > 0)
      PlatformManager.NativePlatform.AchievementManager?.SetAchievementStat(EnumAchievementDataStat.Deaths, _diedMySelfTimes);
    if (_zombieKills > 0)
      PlatformManager.NativePlatform.AchievementManager?.SetAchievementStat(EnumAchievementDataStat.ZombiesKilled, _zombieKills);
    if (_playerKills > 0)
      PlatformManager.NativePlatform.AchievementManager?.SetAchievementStat(EnumAchievementDataStat.PlayersKilled, _playerKills);
    if ((_conditions & 2) == 0)
      return;
    PlatformManager.NativePlatform.AchievementManager?.SetAchievementStat(EnumAchievementDataStat.KilledWith44Magnum, 1);
  }

  public virtual void AwardKill(EntityAlive killer)
  {
    if (!Object.op_Inequality((Object) killer, (Object) null) || !Object.op_Inequality((Object) killer, (Object) this))
      return;
    int _zombieKills = 0;
    int _playerKills = 0;
    int _conditions = 0;
    switch (this.entityType)
    {
      case EntityType.Player:
        ++_playerKills;
        break;
      case EntityType.Zombie:
        ++_zombieKills;
        break;
    }
    EntityPlayer entityPlayer = killer as EntityPlayer;
    if (!Object.op_Implicit((Object) entityPlayer))
      return;
    GameManager.Instance.AwardKill(killer, this);
    if (entityPlayer.inventory.IsHoldingGun() && entityPlayer.inventory.holdingItem.Name.Equals("gunHandgunT2Magnum44"))
      _conditions = 2;
    GameManager.Instance.AddScoreServer(killer.entityId, _zombieKills, _playerKills, this.TeamNumber, _conditions);
  }

  public virtual void OnEntityDeath()
  {
    if (this.deathUpdateTime != 0)
      return;
    this.AddScore(1, 0, 0, -1, 0);
    if (this.soundLiving != null && this.soundLivingID >= 0)
    {
      Manager.Stop(this.entityId, this.soundLiving);
      this.soundLivingID = -1;
    }
    if (Object.op_Implicit((Object) this.AttachedToEntity))
      this.Detach();
    if (this.isEntityRemote)
      return;
    this.AwardKill(this.entityThatKilledMe);
    if (this.particleOnDeath != null && this.particleOnDeath.Length > 0)
    {
      float lightBrightness = this.world.GetLightBrightness(this.GetBlockPosition());
      this.world.GetGameManager().SpawnParticleEffectServer(new ParticleEffect(this.particleOnDeath, this.getHeadPosition(), lightBrightness, Color.white, (string) null, (Transform) null, false), this.entityId);
    }
    if (this.isGameMessageOnDeath())
      GameManager.Instance.GameMessage(EnumGameMessages.EntityWasKilled, this, this.entityThatKilledMe);
    if (Object.op_Inequality((Object) this.entityThatKilledMe, (Object) null))
      Log.Out("Entity {0} {1} killed by {2} {3}", new object[4]
      {
        (object) this.GetDebugName(),
        (object) this.entityId,
        (object) this.entityThatKilledMe.GetDebugName(),
        (object) this.entityThatKilledMe.entityId
      });
    else
      Log.Out("Entity {0} {1} killed", new object[2]
      {
        (object) this.GetDebugName(),
        (object) this.entityId
      });
    ModEvents.SEntityKilledData _data = new ModEvents.SEntityKilledData((Entity) this, (Entity) this.entityThatKilledMe);
    ModEvents.EntityKilled.Invoke(ref _data);
    this.dropItemOnDeath();
    this.entityThatKilledMe = (EntityAlive) null;
  }

  public void Disintegrate()
  {
    this.timeStayAfterDeath = 0;
    this.isDisintegrated = true;
  }

  public virtual void PlayGiveUpSound()
  {
    if (this.soundGiveUp == null)
      return;
    this.PlayOneShot(this.soundGiveUp);
  }

  public virtual Vector3 GetCameraLook(float _t) => this.GetLookVector();

  public Vector3 GetForwardVector()
  {
    float num1 = Mathf.Cos((float) ((double) this.rotation.y * (7.0 / 400.0) - 3.1415927410125732));
    double num2 = (double) Mathf.Sin((float) ((double) this.rotation.y * (7.0 / 400.0) - 3.1415927410125732));
    float num3 = -Mathf.Cos(0.0f);
    float num4 = Mathf.Sin(0.0f);
    double num5 = (double) num3;
    return new Vector3((float) (num2 * num5), num4, num1 * num3);
  }

  public Vector2 GetForwardVector2()
  {
    double num1 = (double) this.rotation.y * (Math.PI / 180.0);
    float num2 = Mathf.Cos((float) num1);
    return new Vector2(Mathf.Sin((float) num1), num2);
  }

  public virtual Vector3 GetLookVector()
  {
    float num1 = Mathf.Cos((float) ((double) this.rotation.y * (7.0 / 400.0) - 3.1415927410125732));
    double num2 = (double) Mathf.Sin((float) ((double) this.rotation.y * (7.0 / 400.0) - 3.1415927410125732));
    float num3 = -Mathf.Cos(this.rotation.x * (7f / 400f));
    float num4 = Mathf.Sin(this.rotation.x * (7f / 400f));
    double num5 = (double) num3;
    return new Vector3((float) (num2 * num5), num4, num1 * num3);
  }

  public virtual Vector3 GetLookVector(Vector3 _altLookVector) => this.GetLookVector();

  [PublicizedFrom(EAccessModifier.Protected)]
  public int GetSoundRandomTicks()
  {
    return this.rand.RandomRange(this.soundRandomTicks / 2, this.soundRandomTicks);
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public int GetSoundAlertTicks()
  {
    return this.rand.RandomRange(this.soundAlertTicks / 2, this.soundAlertTicks);
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public string GetSoundRandom() => this.soundRandom;

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual string GetSoundJump() => this.soundJump;

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual string GetSoundHurt(DamageSource _damageSource, int _damageStrength)
  {
    return this.soundHurt;
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public string GetSoundHurtSmall() => this.soundHurtSmall;

  [PublicizedFrom(EAccessModifier.Protected)]
  public string GetSoundHurt() => this.soundHurt;

  [PublicizedFrom(EAccessModifier.Protected)]
  public string GetSoundDrownPain() => this.soundDrownPain;

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual string GetSoundDeath(DamageSource _damageSource) => this.soundDeath;

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual string GetSoundAttack() => this.soundAttack;

  public virtual string GetSoundAlert() => this.soundAlert;

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual string GetSoundStamina() => this.soundStamina;

  public virtual Ray GetLookRay()
  {
    return new Ray(Vector3.op_Addition(this.position, new Vector3(0.0f, this.GetEyeHeight(), 0.0f)), this.GetLookVector());
  }

  public virtual Ray GetMeleeRay() => this.GetLookRay();

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual void dropItemOnDeath()
  {
    for (int _idx = 0; _idx < this.inventory.GetItemCount(); ++_idx)
    {
      ItemStack _itemStack = this.inventory.GetItem(_idx);
      ItemClass forId = ItemClass.GetForId(_itemStack.itemValue.type);
      if (forId != null && forId.CanDrop())
      {
        this.world.GetGameManager().ItemDropServer(_itemStack, this.position, new Vector3(0.5f, 0.0f, 0.5f), _lifetime: Constants.cItemDroppedOnDeathLifetime);
        this.inventory.SetItem(_idx, ItemValue.None.Clone(), 0);
      }
    }
    this.inventory.SetFlashlight(false);
    this.equipment.DropItems();
    if (this.world.IsDark())
      this.lootDropProb *= 1f;
    if (Object.op_Implicit((Object) this.entityThatKilledMe))
      this.lootDropProb = EffectManager.GetValue(PassiveEffects.LootDropProb, this.entityThatKilledMe.inventory.holdingItemItemValue, this.lootDropProb, this.entityThatKilledMe);
    if ((double) this.lootDropProb <= (double) this.rand.RandomFloat)
      return;
    GameManager.Instance.DropContentOfLootContainerServer(BlockValue.Air, new Vector3i(this.position), this.entityId, (ITileEntityLootable) null);
  }

  public virtual Vector3 GetDropPosition()
  {
    return this.ParachuteWearing || this.JetpackWearing ? Vector3.op_Addition(Vector3.op_Subtraction(Vector3.op_Addition(((Component) this).transform.position, ((Component) this).transform.forward), Vector3.op_Multiply(Vector3.up, 0.3f)), Origin.position) : Vector3.op_Addition(Vector3.op_Addition(Vector3.op_Addition(((Component) this).transform.position, ((Component) this).transform.forward), Vector3.up), Origin.position);
  }

  public virtual void OnFired()
  {
    if (!Object.op_Inequality((Object) this.emodel.avatarController, (Object) null))
      return;
    this.emodel.avatarController.StartAnimationFiring();
  }

  public virtual void OnReloadStart()
  {
    if (!Object.op_Inequality((Object) this.emodel.avatarController, (Object) null))
      return;
    this.emodel.avatarController.StartAnimationReloading();
  }

  public virtual void OnReloadEnd()
  {
  }

  public virtual bool WillForceToFollow(EntityAlive _other) => false;

  public void AddHealth(int _v)
  {
    if (this.Health <= 0)
      return;
    this.Health += _v;
  }

  public void AddStamina(float _v)
  {
    if (this.entityStats.Stamina == null || this.Health <= 0)
      return;
    this.entityStats.Stamina.Value += _v;
  }

  public void AddWater(float _v) => this.Stats.Water.Value += _v;

  public int GetTicksNoPlayerAdjacent() => this.ticksNoPlayerAdjacent;

  public bool CanSee(EntityAlive _other) => this.seeCache.CanSee((Entity) _other);

  public void SetCanSee(EntityAlive _other) => this.seeCache.SetCanSee((Entity) _other);

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual void updateTasks()
  {
    if (GamePrefs.GetBool(EnumGamePrefs.DebugStopEnemiesMoving))
    {
      this.SetMoveForwardWithModifiers(0.0f, 0.0f, false);
      if (this.aiManager == null)
        return;
      this.aiManager.UpdateDebugName();
    }
    else
    {
      this.CheckDespawn();
      this.seeCache.ClearIfExpired();
      bool useAiPackages = EntityClass.list[this.entityClass].UseAIPackages;
      this.aiActiveDelay -= this.aiActiveScale;
      if ((double) this.aiActiveDelay <= 0.0)
      {
        this.aiActiveDelay = 1f;
        if (!useAiPackages)
          this.aiManager.Update();
        else
          UAIBase.Update(this.utilityAIContext);
      }
      PathInfo path = PathFinderThread.Instance.GetPath(this.entityId);
      if (path.path != null)
      {
        bool flag = true;
        if (!useAiPackages)
          flag = this.aiManager.CheckPath(path);
        if (flag)
          this.navigator.SetPath(path, path.speed);
      }
      this.navigator.UpdateNavigation();
      this.moveHelper.UpdateMoveHelper();
      this.lookHelper.onUpdateLook();
      if (Object.op_Inequality((Object) this.distraction, (Object) null) && (this.distraction.IsDead() || this.distraction.IsMarkedForUnload()))
        this.distraction = (EntityItem) null;
      if (!Object.op_Inequality((Object) this.pendingDistraction, (Object) null) || !this.pendingDistraction.IsDead() && !this.pendingDistraction.IsMarkedForUnload())
        return;
      this.pendingDistraction = (EntityItem) null;
    }
  }

  public PathNavigate getNavigator() => this.navigator;

  public void FindPath(Vector3 targetPos, float moveSpeed, bool canBreak, EAIBase behavior)
  {
    Vector3 vector3 = Vector3.op_Subtraction(targetPos, this.position);
    if ((double) vector3.x * (double) vector3.x + (double) vector3.z * (double) vector3.z > 1225.0)
    {
      if ((double) vector3.y > 45.0)
        targetPos.y = this.position.y + 45f;
      else if ((double) vector3.y < -45.0)
        targetPos.y = this.position.y - 45f;
    }
    PathFinderThread.Instance.FindPath(this, targetPos, moveSpeed, canBreak, behavior);
  }

  public bool isWithinHomeDistanceCurrentPosition()
  {
    return this.isWithinHomeDistance(Utils.Fastfloor(this.position.x), Utils.Fastfloor(this.position.y), Utils.Fastfloor(this.position.z));
  }

  public bool isWithinHomeDistance(int _x, int _y, int _z)
  {
    return this.maximumHomeDistance < 0 || (double) this.homePosition.getDistanceSquared(_x, _y, _z) < (double) (this.maximumHomeDistance * this.maximumHomeDistance);
  }

  public void setHomeArea(Vector3i _pos, int _maxDistance)
  {
    this.homePosition.position = _pos;
    this.maximumHomeDistance = _maxDistance;
  }

  public ChunkCoordinates getHomePosition() => this.homePosition;

  public int getMaximumHomeDistance() => this.maximumHomeDistance;

  public void detachHome() => this.maximumHomeDistance = -1;

  public bool hasHome() => this.maximumHomeDistance >= 0;

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual bool canDespawn()
  {
    return !this.IsClientControlled() && this.GetSpawnerSource() != EnumSpawnerSource.StaticSpawner && !this.IsSleeping;
  }

  public void ResetDespawnTime()
  {
    this.ticksNoPlayerAdjacent = 0;
    this.seeCache.SetLastTimePlayerSeen();
  }

  public void CheckDespawn()
  {
    if (this.isEntityRemote)
      return;
    if (!this.CanUpdateEntity() && this.bIsChunkObserver && Object.op_Equality((Object) this.world.GetClosestPlayer((Entity) this, -1f, false), (Object) null))
    {
      this.MarkToUnload();
    }
    else
    {
      if (!this.canDespawn() || ++this.despawnDelayCounter < 20)
        return;
      this.despawnDelayCounter = 0;
      this.ticksNoPlayerAdjacent += 20;
      EnumSpawnerSource spawnerSource = this.GetSpawnerSource();
      EntityPlayer closestPlayer = this.world.GetClosestPlayer((Entity) this, -1f, false);
      switch (spawnerSource)
      {
        case EnumSpawnerSource.Biome:
          if (!Object.op_Implicit((Object) this.world.GetClosestPlayer((Entity) this, 130f, false)))
          {
            if (Object.op_Implicit((Object) this.world.GetClosestPlayer((Entity) this, 20f, true)))
            {
              this.isDespawnWhenPlayerFar = true;
              break;
            }
            if (this.isDespawnWhenPlayerFar)
            {
              this.Despawn();
              break;
            }
            break;
          }
          break;
        case EnumSpawnerSource.Dynamic:
          if (!Object.op_Implicit((Object) closestPlayer))
          {
            if (Object.op_Implicit((Object) this.world.GetClosestPlayer((Entity) this, -1f, true)))
              return;
            this.Despawn();
            return;
          }
          break;
      }
      if (!Object.op_Implicit((Object) closestPlayer))
        return;
      Vector3 vector3 = Vector3.op_Subtraction(closestPlayer.position, this.position);
      float sqrMagnitude = ((Vector3) ref vector3).sqrMagnitude;
      if ((double) sqrMagnitude < 6400.0)
        this.ticksNoPlayerAdjacent = 0;
      int num = int.MaxValue;
      float lastTimePlayerSeen = this.seeCache.GetLastTimePlayerSeen();
      if ((double) lastTimePlayerSeen > 0.0)
        num = (int) ((double) Time.time - (double) lastTimePlayerSeen);
      switch (spawnerSource)
      {
        case EnumSpawnerSource.Biome:
          if (this.ticksNoPlayerAdjacent > 100 && (double) sqrMagnitude > 16384.0)
          {
            this.Despawn();
            break;
          }
          if (this.ticksNoPlayerAdjacent <= 1800)
            break;
          this.Despawn();
          break;
        case EnumSpawnerSource.Dynamic:
          if (Object.op_Implicit((Object) this.attackTarget))
            num = 0;
          if (this.IsSleeper && !this.IsSleeping)
          {
            if ((double) sqrMagnitude <= 9216.0 || num <= 80 /*0x50*/)
              break;
            this.Despawn();
            break;
          }
          if ((double) sqrMagnitude > 2304.0 && num > 60 && !this.HasInvestigatePosition)
          {
            this.Despawn();
            break;
          }
          if (this.ticksNoPlayerAdjacent <= 1800)
            break;
          this.Despawn();
          break;
      }
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void Despawn()
  {
    this.IsDespawned = true;
    this.MarkToUnload();
  }

  public void ForceDespawn() => this.Despawn();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public EntityAlive GetAttackTarget() => this.attackTarget;

  public virtual Vector3 GetAttackTargetHitPosition() => this.attackTarget.getChestPosition();

  public EntityAlive GetAttackTargetLocal()
  {
    return this.isEntityRemote ? this.attackTargetClient : this.attackTarget;
  }

  public void SetAttackTarget(EntityAlive _attackTarget, int _attackTargetTime)
  {
    if (Object.op_Equality((Object) _attackTarget, (Object) this.attackTarget))
    {
      this.attackTargetTime = _attackTargetTime;
    }
    else
    {
      if (Object.op_Implicit((Object) this.attackTarget))
        this.attackTargetLast = this.attackTarget;
      this.targetAlertChanged = false;
      if (Object.op_Implicit((Object) _attackTarget))
      {
        if (Object.op_Inequality((Object) _attackTarget, (Object) this.attackTargetLast))
        {
          this.targetAlertChanged = true;
          this.soundDelayTicks = this.rand.RandomRange(5, 20);
        }
        this.investigatePositionTicks = 0;
      }
      if (!this.isEntityRemote)
        this.world.entityDistributer.SendPacketToTrackedPlayersAndTrackedEntity(this.entityId, -1, (NetPackage) NetPackageManager.GetPackage<NetPackageSetAttackTarget>().Setup(this.entityId, Object.op_Implicit((Object) _attackTarget) ? _attackTarget.entityId : -1));
      this.attackTarget = _attackTarget;
      this.attackTargetTime = _attackTargetTime;
    }
  }

  public void SetAttackTargetClient(EntityAlive _attackTarget)
  {
    this.attackTargetClient = _attackTarget;
  }

  public bool HasInvestigatePosition => this.investigatePositionTicks > 0;

  public Vector3 InvestigatePosition => this.investigatePos;

  public int GetInvestigatePositionTicks() => this.investigatePositionTicks;

  public void ClearInvestigatePosition()
  {
    this.investigatePos = Vector3.zero;
    this.investigatePositionTicks = 0;
    this.ResetDespawnTime();
    int ticks = this.rand.RandomRange(12, 25) * 20;
    if (this.entityType == EntityType.Zombie)
      ticks /= 2;
    this.SetAlertTicks(ticks);
  }

  public int CalcInvestigateTicks(int _ticks, EntityAlive _investigateEntity)
  {
    float num = EffectManager.GetValue(PassiveEffects.EnemySearchDuration, _originalValue: 1f, _entity: _investigateEntity, tags: EntityClass.list[this.entityClass].Tags);
    return (int) ((double) _ticks / (double) num);
  }

  public void SetInvestigatePosition(Vector3 pos, int ticks, bool isAlert = true)
  {
    this.investigatePos = pos;
    this.investigatePositionTicks = ticks;
    this.isInvestigateAlert = isAlert;
  }

  public int GetAlertTicks() => this.alertTicks;

  public void SetAlertTicks(int ticks) => this.alertTicks = ticks;

  public virtual bool IsAlert => this.isEntityRemote ? this.bReplicatedAlertFlag : this.isAlert;

  public Vector3 LastTargetPos
  {
    get => this.lastTargetPos;
    set => this.lastTargetPos = value;
  }

  public EntitySeeCache GetEntitySenses() => this.seeCache;

  public virtual bool IsRunning => this.IsBloodMoon || this.world.IsDark();

  public virtual float GetMoveSpeed()
  {
    return this.IsBloodMoon || this.world.IsDark() ? EffectManager.GetValue(PassiveEffects.WalkSpeed, _originalValue: this.moveSpeedNight, _entity: this) : EffectManager.GetValue(PassiveEffects.CrouchSpeed, _originalValue: this.moveSpeed, _entity: this);
  }

  public virtual float GetMoveSpeedAggro()
  {
    return this.IsBloodMoon || this.world.IsDark() ? EffectManager.GetValue(PassiveEffects.RunSpeed, _originalValue: this.moveSpeedAggroMax, _entity: this) : EffectManager.GetValue(PassiveEffects.WalkSpeed, _originalValue: this.moveSpeedAggro, _entity: this);
  }

  public float GetMoveSpeedPanic()
  {
    return EffectManager.GetValue(PassiveEffects.RunSpeed, _originalValue: this.moveSpeedPanic, _entity: this);
  }

  public override float GetWeight() => this.weight;

  public override float GetPushFactor() => this.pushFactor;

  public virtual bool CanEntityJump() => true;

  public void SetMaxViewAngle(float _angle) => this.maxViewAngle = _angle;

  public virtual float GetMaxViewAngle() => this.maxViewAngle;

  public void SetSightLightThreshold(Vector2 _threshold) => this.sightLightThreshold = _threshold;

  public int GetModelLayer() => ((Component) this.emodel.GetModelTransform()).gameObject.layer;

  public virtual void SetModelLayer(int _layerId, bool force = false, string[] excludeTags = null)
  {
    Utils.SetLayerRecursively(((Component) this.emodel.GetModelTransform()).gameObject, _layerId);
  }

  public virtual void SetColliderLayer(int _layerId, bool _force = false)
  {
    Utils.SetColliderLayerRecursively(((Component) this.emodel.GetModelTransform()).gameObject, _layerId);
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual int GetMaxAttackTime() => 10;

  public int GetAttackTimeoutTicks()
  {
    return !this.world.IsDark() ? this.attackTimeoutDay : this.attackTimeoutNight;
  }

  public override string GetLootList()
  {
    return !string.IsNullOrEmpty(this.lootListOnDeath) && this.IsDead() ? this.lootListOnDeath : base.GetLootList();
  }

  public override void MarkToUnload()
  {
    base.MarkToUnload();
    this.deathUpdateTime = this.timeStayAfterDeath;
  }

  public override bool IsMarkedForUnload()
  {
    return base.IsMarkedForUnload() && this.deathUpdateTime >= this.timeStayAfterDeath;
  }

  public virtual bool IsAttackValid()
  {
    if (!(this is EntityPlayer) && (this.Electrocuted || this.bodyDamage.CurrentStun == EnumEntityStunType.Kneel || this.bodyDamage.CurrentStun == EnumEntityStunType.Prone) || Object.op_Inequality((Object) this.emodel, (Object) null) && Object.op_Inequality((Object) this.emodel.avatarController, (Object) null) && this.emodel.avatarController.IsAttackPrevented() || this.IsDead())
      return false;
    if ((double) this.painResistPercent >= 1.0)
      return true;
    if (this.hasBeenAttackedTime > 0)
      return false;
    return Object.op_Equality((Object) this.emodel.avatarController, (Object) null) || !this.emodel.avatarController.IsAnimationHitRunning();
  }

  public virtual bool IsAttackImpact()
  {
    return Object.op_Implicit((Object) this.emodel) && Object.op_Implicit((Object) this.emodel.avatarController) && this.emodel.avatarController.IsAttackImpact();
  }

  public virtual void ShowHoldingItem(bool _show) => this.inventory.ShowRightHand(_show);

  public virtual bool IsHoldingItemInUse(int _actionIndex)
  {
    ItemAction action = this.inventory.holdingItem.Actions[_actionIndex];
    return action != null && action.IsActionRunning(this.inventory.holdingItemData.actionData[_actionIndex]);
  }

  public virtual bool UseHoldingItem(int _actionIndex, bool _isReleased)
  {
    if (!_isReleased && (_actionIndex == 0 && Object.op_Implicit((Object) this.emodel) && Object.op_Implicit((Object) this.emodel.avatarController) && this.emodel.avatarController.IsAnimationAttackPlaying() || !this.IsAttackValid()))
      return false;
    if (_actionIndex == 0 && _isReleased && this.GetSoundAttack() != null)
      this.PlayOneShot(this.GetSoundAttack());
    this.attackingTime = 60;
    this.inventory.holdingItem.Actions[_actionIndex]?.ExecuteAction(this.inventory.holdingItemData.actionData[_actionIndex], _isReleased);
    return true;
  }

  public bool Attack(bool _isReleased) => this.UseHoldingItem(0, _isReleased);

  public Entity GetTargetIfAttackedNow()
  {
    if (!this.IsAttackValid())
      return (Entity) null;
    ItemClass holdingItem = this.inventory.holdingItem;
    if (holdingItem != null)
    {
      int holdingItemIdx = this.inventory.holdingItemIdx;
      ItemAction action = holdingItem.Actions[holdingItemIdx];
      if (action != null)
      {
        WorldRayHitInfo executeActionTarget = action.GetExecuteActionTarget(this.inventory.holdingItemData.actionData[holdingItemIdx]);
        if (executeActionTarget != null && executeActionTarget.bHitValid && Object.op_Implicit((Object) executeActionTarget.transform))
        {
          float num1 = action.Range;
          if ((double) num1 == 0.0)
            num1 = EffectManager.GetItemValue(PassiveEffects.MaxRange, this.inventory.holdingItemItemValue);
          float num2 = num1 + 0.3f;
          if ((double) executeActionTarget.hit.distanceSq <= (double) num2 * (double) num2)
          {
            Transform t = executeActionTarget.transform;
            if (executeActionTarget.tag.StartsWith("E_BP_"))
              t = GameUtils.GetHitRootTransform(Voxel.voxelRayHitInfo.tag, executeActionTarget.transform);
            if (Object.op_Inequality((Object) t, (Object) null))
            {
              Entity component = ((Component) t).GetComponent<Entity>();
              if (Object.op_Implicit((Object) component))
                return component;
            }
            if (executeActionTarget.tag == "E_Vehicle")
              return (Entity) EntityVehicle.FindCollisionEntity(t);
          }
        }
      }
    }
    return (Entity) null;
  }

  public virtual float GetBlockDamageScale()
  {
    EnumGamePrefs _eProperty = EnumGamePrefs.BlockDamageAI;
    if (this.IsBloodMoon)
      _eProperty = EnumGamePrefs.BlockDamageAIBM;
    return (float) GamePrefs.GetInt(_eProperty) * 0.01f;
  }

  public virtual void PlayStepSound(float _volume) => this.internalPlayStepSound(_volume);

  [PublicizedFrom(EAccessModifier.Protected)]
  public void internalPlayStepSound(float _volume)
  {
    if (this.blockValueStandingOn.isair)
      return;
    if (!this.onGround && !this.IsInElevator() || this.isHeadUnderwater)
    {
      if (this is EntityPlayerLocal || !this.isHeadUnderwater && !this.world.IsWater(this.blockPosStandingOn))
        return;
      Manager.Play((Entity) this, "player_swim");
    }
    else
    {
      BlockValue _blockValue = this.blockValueStandingOn;
      Vector3i blockPosStandingOn = this.blockPosStandingOn;
      ++blockPosStandingOn.y;
      BlockValue block1 = this.world.GetBlock(blockPosStandingOn);
      if (block1.Block.blockMaterial.stepSound != null)
      {
        _blockValue = block1;
      }
      else
      {
        BlockValue block2;
        if (!(block2 = this.world.GetBlock(blockPosStandingOn + Vector3i.right)).isair && block2.Block.blockMaterial.stepSound != null)
        {
          _blockValue = block2;
        }
        else
        {
          BlockValue block3;
          if (!(block3 = this.world.GetBlock(blockPosStandingOn - Vector3i.right)).isair && block3.Block.blockMaterial.stepSound != null)
          {
            _blockValue = block3;
          }
          else
          {
            BlockValue block4;
            if (!(block4 = this.world.GetBlock(blockPosStandingOn + Vector3i.forward)).isair && block4.Block.blockMaterial.stepSound != null)
            {
              _blockValue = block4;
            }
            else
            {
              BlockValue block5;
              if (!(block5 = this.world.GetBlock(blockPosStandingOn - Vector3i.forward)).isair && block5.Block.blockMaterial.stepSound != null)
                _blockValue = block5;
            }
          }
        }
      }
      if (_blockValue.isair)
        return;
      Block block6 = _blockValue.Block;
      if ((double) EffectManager.GetValue(PassiveEffects.SilenceBlockSteps, _entity: this, tags: block6.Tags) > 0.0)
        return;
      MaterialBlock materialForSide = block6.GetMaterialForSide(_blockValue, BlockFace.Top);
      if (materialForSide == null || materialForSide.stepSound == null)
        return;
      string name = materialForSide.stepSound.name;
      if (name.Length <= 0)
        return;
      this.PlayStepSound(this.soundStepType + name, _volume);
    }
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual void updateStepSound(float _distX, float _distZ, float _rotYDelta)
  {
    if (this.blockValueStandingOn.isair)
      return;
    float num = Mathf.Sqrt((float) ((double) _distX * (double) _distX + (double) _distZ * (double) _distZ));
    if (!this.onGround || this.isHeadUnderwater)
    {
      this.distanceSwam += num;
      if ((double) this.distanceSwam <= (double) this.nextSwimDistance)
        return;
      ++this.nextSwimDistance;
      if ((double) this.nextSwimDistance < (double) this.distanceSwam || (double) this.nextSwimDistance > (double) this.distanceSwam + 1.0)
        this.nextSwimDistance = this.distanceSwam + 1f;
      this.internalPlayStepSound(1f);
    }
    else
    {
      this.distanceWalked += num;
      if ((double) num == 0.0)
      {
        this.stepSoundDistanceRemaining = 0.25f;
      }
      else
      {
        this.stepSoundDistanceRemaining -= num;
        if ((double) this.stepSoundDistanceRemaining <= 0.0)
        {
          this.stepSoundDistanceRemaining = this.getNextStepSoundDistance();
          this.internalPlayStepSound(1f);
        }
      }
      this.stepSoundRotYRemaining -= Utils.FastAbs(_rotYDelta);
      if ((double) this.stepSoundRotYRemaining > 0.0)
        return;
      this.stepSoundRotYRemaining = 90f;
      this.internalPlayStepSound(1f);
    }
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public void updatePlayerLandSound(float _distXZ, float _diffY)
  {
    if (this.blockValueStandingOn.isair)
      return;
    if ((double) _distXZ >= 0.02500000037252903 || (double) Utils.FastAbs(_diffY) >= 0.014999999664723873)
    {
      float num = this.inWaterPercent * 2f;
      double _x = (double) num - (double) this.landWaterLevel;
      this.landWaterLevel = num;
      float v1 = Utils.FastAbs((float) _x);
      if ((double) num > 0.0)
        v1 = Utils.FastMax(v1, _distXZ);
      if ((double) v1 >= 0.019999999552965164)
        Manager.Play((Entity) this, "player_swim", Utils.FastMin((float) ((double) v1 * 2.2000000476837158 + 0.0099999997764825821), 1f));
    }
    if (this.isHeadUnderwater)
      this.wasOnGround = true;
    else
      this.wasOnGround = this.onGround;
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public void updateCurrentBlockPosAndValue()
  {
    Vector3i blockPosition = this.GetBlockPosition();
    BlockValue block1 = this.world.GetBlock(blockPosition);
    if (block1.isair)
    {
      --blockPosition.y;
      block1 = this.world.GetBlock(blockPosition);
    }
    if (block1.ischild)
    {
      blockPosition += block1.parent;
      block1 = this.world.GetBlock(blockPosition);
    }
    if (this.blockPosStandingOn != blockPosition || !this.blockValueStandingOn.Equals(block1) || this.onGround && !this.wasOnGround)
    {
      this.blockPosStandingOn = blockPosition;
      this.blockValueStandingOn = block1;
      this.blockStandingOnChanged = !this.world.IsRemote();
      BiomeDefinition biome = this.world.GetBiome(this.blockPosStandingOn.x, this.blockPosStandingOn.z);
      if (biome != null && this.biomeStandingOn != biome && (this.biomeStandingOn == null || (int) this.biomeStandingOn.m_Id != (int) biome.m_Id))
        this.onNewBiomeEntered(biome);
    }
    this.CalcIfInElevator();
    Block block2 = this.blockValueStandingOn.Block;
    if (block2.BuffsWhenWalkedOn != null && block2.UseBuffsWhenWalkedOn(this.world, this.blockPosStandingOn, this.blockValueStandingOn))
    {
      bool flag = true;
      if (this.world.GetTileEntity(0, this.blockPosStandingOn) is TileEntityWorkstation tileEntity)
        flag = tileEntity.IsBurning;
      if (flag)
      {
        for (int index = 0; index < block2.BuffsWhenWalkedOn.Length; ++index)
        {
          BuffValue buff = this.Buffs.GetBuff(block2.BuffsWhenWalkedOn[index]);
          if (buff == null || (double) buff.DurationInSeconds >= 1.0)
          {
            int num = (int) this.Buffs.AddBuff(block2.BuffsWhenWalkedOn[index], blockPosition);
          }
        }
      }
    }
    if (this.onGround && !this.IsFlyMode.Value)
    {
      if ((double) block2.MovementFactor != 1.0 && block2.HasCollidingAABB(this.blockValueStandingOn, this.blockPosStandingOn.x, this.blockPosStandingOn.y, this.blockPosStandingOn.z, 0.0f, this.boundingBox))
        this.SetMotionMultiplier(EffectManager.GetValue(PassiveEffects.MovementFactorMultiplier, _originalValue: block2.MovementFactor, _entity: this));
      if (this.blockStandingOnChanged)
      {
        this.blockStandingOnChanged = false;
        if (!this.blockValueStandingOn.isair)
        {
          block2.OnEntityWalking((WorldBase) this.world, this.blockPosStandingOn.x, this.blockPosStandingOn.y, this.blockPosStandingOn.z, this.blockValueStandingOn, (Entity) this);
          if (GameManager.bPhysicsActive && !this.blockValueStandingOn.ischild && !this.blockValueStandingOn.Block.isOversized && this.world.GetStability(this.blockPosStandingOn) == (byte) 0 && Block.CanFallBelow((WorldBase) this.world, this.blockPosStandingOn.x, this.blockPosStandingOn.y, this.blockPosStandingOn.z))
          {
            Log.Warning("EntityAlive {0} AddFallingBlock stab 0 happens?", new object[1]
            {
              (object) this.EntityName
            });
            this.world.AddFallingBlock(this.blockPosStandingOn);
          }
        }
        BlockValue block3 = this.world.GetBlock(this.blockPosStandingOn + Vector3i.up);
        if (!block3.isair)
          block3.Block.OnEntityWalking((WorldBase) this.world, this.blockPosStandingOn.x, this.blockPosStandingOn.y + 1, this.blockPosStandingOn.z, block3, (Entity) this);
      }
    }
    this.HandleLootStageMaxCheck();
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual void HandleLootStageMaxCheck()
  {
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void CalcIfInElevator()
  {
    ChunkCluster chunkCache = this.world.ChunkCache;
    Vector3i _pos = new Vector3i(this.blockPosStandingOn.x, Utils.Fastfloor(((Bounds) ref this.boundingBox).min.y), this.blockPosStandingOn.z);
    BlockValue block1 = chunkCache.GetBlock(_pos);
    this.bInElevator = block1.Block.IsElevator((int) block1.rotation);
    ++_pos.y;
    BlockValue block2 = chunkCache.GetBlock(_pos);
    this.bInElevator |= block2.Block.IsElevator((int) block2.rotation);
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual float getNextStepSoundDistance() => 1.5f;

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual void onNewBiomeEntered(BiomeDefinition _biome) => this.biomeStandingOn = _biome;

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual void updateSpeedForwardAndStrafe(Vector3 _dist, float _partialTicks)
  {
    if (this.isEntityRemote && (double) _partialTicks > 1.0)
      _dist = Vector3.op_Division(_dist, _partialTicks);
    this.speedForward *= 0.5f;
    this.speedStrafe *= 0.5f;
    this.speedVertical *= 0.5f;
    if ((double) Mathf.Abs(_dist.x) > 1.0 / 1000.0 || (double) Mathf.Abs(_dist.z) > 1.0 / 1000.0)
    {
      float num1 = Mathf.Sin((float) (-(double) this.rotation.y * 3.1415927410125732 / 180.0));
      float num2 = Mathf.Cos((float) (-(double) this.rotation.y * 3.1415927410125732 / 180.0));
      this.speedForward += (float) ((double) num2 * (double) _dist.z - (double) num1 * (double) _dist.x);
      this.speedStrafe += (float) ((double) num2 * (double) _dist.x + (double) num1 * (double) _dist.z);
    }
    if ((double) Mathf.Abs(_dist.y) > 1.0 / 1000.0)
      this.speedVertical += _dist.y;
    this.SetMovementState();
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual void PlayStepSound(string stepSound, float _volume)
  {
    if (this is EntityPlayerLocal)
      Manager.BroadcastPlay((Entity) this, stepSound);
    else
      Manager.Play((Entity) this, stepSound);
  }

  public virtual void SetLookPosition(Vector3 _lookPos)
  {
    Vector3 vector3 = Vector3.op_Subtraction(this.lookAtPosition, _lookPos);
    if ((double) ((Vector3) ref vector3).sqrMagnitude < 1.0 / 625.0)
      return;
    this.lookAtPosition = _lookPos;
    if (this.world.entityDistributer != null)
      this.world.entityDistributer.SendPacketToTrackedPlayers(this.entityId, Object.op_Inequality((Object) this.world.GetPrimaryPlayer(), (Object) null) ? this.world.GetPrimaryPlayer().entityId : -1, (NetPackage) NetPackageManager.GetPackage<NetPackageEntityLookAt>().Setup(this.entityId, _lookPos));
    if (!Object.op_Inequality((Object) this.emodel.avatarController, (Object) null))
      return;
    this.emodel.avatarController.SetLookPosition(_lookPos);
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual bool isRadiationSensitive() => true;

  public virtual bool IsAimingGunPossible() => true;

  public int GetDeathTime() => this.deathUpdateTime;

  public void SetDeathTime(int _deathTime) => this.deathUpdateTime = _deathTime;

  public int GetTimeStayAfterDeath() => this.timeStayAfterDeath;

  public bool IsCorpse()
  {
    return Object.op_Implicit((Object) this.emodel) && this.emodel.IsRagdollDead && (double) this.deathUpdateTime > 70.0;
  }

  public override void OnAddedToWorld()
  {
    if (!(this is EntityPlayerLocal))
      OcclusionManager.AddEntity(this, 7f);
    this.m_addedToWorld = true;
    if (!this.isEntityRemote)
      this.bSpawned = true;
    if (Object.op_Equality((Object) (this as EntityPlayer), (Object) null))
      this.FireEvent(MinEventTypes.onSelfFirstSpawn);
    this.StartStopLivingSound();
  }

  public override void OnEntityUnload()
  {
    if (!(this is EntityPlayerLocal))
      OcclusionManager.RemoveEntity(this);
    if (this.navigator != null)
    {
      this.navigator.SetPath((PathInfo) null, 0.0f);
      this.navigator = (PathNavigate) null;
    }
    base.OnEntityUnload();
    this.lookHelper = (EntityLookHelper) null;
    this.moveHelper = (EntityMoveHelper) null;
    this.seeCache = (EntitySeeCache) null;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public float GetDamageFraction(float _damage) => _damage / (float) this.GetMaxHealth();

  [PublicizedFrom(EAccessModifier.Private)]
  public float GetDismemberChance(ref DamageResponse _dmResponse, float damagePer)
  {
    EnumBodyPartHit hitBodyPart = _dmResponse.HitBodyPart;
    EntityClass entityClass = EntityClass.list[this.entityClass];
    float _originalValue = 0.0f;
    switch (hitBodyPart.ToPrimary())
    {
      case BodyPrimaryHit.Head:
        _originalValue = entityClass.DismemberMultiplierHead;
        break;
      case BodyPrimaryHit.LeftUpperArm:
      case BodyPrimaryHit.RightUpperArm:
      case BodyPrimaryHit.LeftLowerArm:
      case BodyPrimaryHit.RightLowerArm:
        _originalValue = entityClass.DismemberMultiplierArms;
        break;
      case BodyPrimaryHit.LeftUpperLeg:
      case BodyPrimaryHit.RightUpperLeg:
      case BodyPrimaryHit.LeftLowerLeg:
      case BodyPrimaryHit.RightLowerLeg:
        _originalValue = entityClass.DismemberMultiplierLegs;
        break;
    }
    EntityAlive _entity = this;
    float num = EffectManager.GetValue(PassiveEffects.DismemberSelfChance, _originalValue: _originalValue, _entity: _entity);
    float dismemberChance1 = _dmResponse.Source.DismemberChance;
    float dismemberChance2 = (double) dismemberChance1 < 100.0 ? dismemberChance1 * damagePer * num : 100f;
    EntityPlayerLocal entity = this.world.GetEntity(_dmResponse.Source.getEntityId()) as EntityPlayerLocal;
    if (Object.op_Implicit((Object) entity) && entity.DebugDismembermentChance)
      dismemberChance2 = 1f;
    if (DismembermentManager.DebugLogEnabled && (double) dismemberChance2 > 0.0)
      Log.Out("[EntityAlive.GetDismemberChance] - {0}, primary {1}, damage {2}, chance {3} * damage% {4} * multiplier {5} = {6}", new object[7]
      {
        (object) hitBodyPart,
        (object) hitBodyPart.ToPrimary(),
        (object) _dmResponse.Strength,
        (object) dismemberChance1.ToCultureInvariantString(),
        (object) damagePer.ToCultureInvariantString(),
        (object) num.ToCultureInvariantString(),
        (object) dismemberChance2.ToCultureInvariantString()
      });
    return dismemberChance2;
  }

  public virtual void CheckDismember(ref DamageResponse _dmResponse, float damagePer)
  {
    bool flag = _dmResponse.HitBodyPart.IsLeg();
    if (!flag || !this.IsAlive() || this.bodyDamage.CurrentStun == EnumEntityStunType.None && !this.sleepingOrWakingUp)
    {
      float dismemberChance = this.GetDismemberChance(ref _dmResponse, damagePer);
      if ((double) dismemberChance > 0.0 && (double) this.rand.RandomFloat <= (double) dismemberChance)
      {
        _dmResponse.Dismember = true;
        if (!flag)
          return;
        _dmResponse.TurnIntoCrawler = true;
      }
      else
      {
        if (!flag)
          return;
        EntityClass entityClass = EntityClass.list[this.entityClass];
        if ((double) entityClass.LegCrawlerThreshold > 0.0 && (double) this.GetDamageFraction((float) _dmResponse.Strength) >= (double) entityClass.LegCrawlerThreshold)
          _dmResponse.TurnIntoCrawler = true;
        if ((this.bodyDamage.ShouldBeCrawler ? 1 : (_dmResponse.TurnIntoCrawler ? 1 : 0)) != 0 || (double) entityClass.LegCrippleScale <= 0.0)
          return;
        float num = this.GetDamageFraction((float) _dmResponse.Strength) * entityClass.LegCrippleScale;
        if ((double) num < 0.05000000074505806)
          return;
        if (((int) this.bodyDamage.Flags & 4096 /*0x1000*/) == 0 && _dmResponse.HitBodyPart.IsLeftLeg() && (double) this.rand.RandomFloat < (double) num)
          _dmResponse.CrippleLegs = true;
        if (((int) this.bodyDamage.Flags & 8192 /*0x2000*/) != 0 || !_dmResponse.HitBodyPart.IsRightLeg() || (double) this.rand.RandomFloat >= (double) num)
          return;
        _dmResponse.CrippleLegs = true;
      }
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void ApplyLocalBodyDamage(DamageResponse _dmResponse)
  {
    EnumBodyPartHit enumBodyPartHit = _dmResponse.HitBodyPart;
    this.bodyDamage.bodyPartHit = enumBodyPartHit;
    this.bodyDamage.damageType = _dmResponse.Source.damageType;
    if (_dmResponse.Dismember)
    {
      if (DismembermentManager.DebugBodyPartHit != EnumBodyPartHit.None)
        enumBodyPartHit = DismembermentManager.DebugBodyPartHit;
      if ((enumBodyPartHit & EnumBodyPartHit.Head) > EnumBodyPartHit.None)
        this.bodyDamage.Flags |= 1U;
      if ((enumBodyPartHit & EnumBodyPartHit.LeftUpperArm) > EnumBodyPartHit.None)
        this.bodyDamage.Flags |= 2U;
      if ((enumBodyPartHit & EnumBodyPartHit.LeftLowerArm) > EnumBodyPartHit.None)
        this.bodyDamage.Flags |= 4U;
      if ((enumBodyPartHit & EnumBodyPartHit.RightUpperArm) > EnumBodyPartHit.None)
        this.bodyDamage.Flags |= 8U;
      if ((enumBodyPartHit & EnumBodyPartHit.RightLowerArm) > EnumBodyPartHit.None)
        this.bodyDamage.Flags |= 16U /*0x10*/;
      if ((enumBodyPartHit & EnumBodyPartHit.LeftUpperLeg) > EnumBodyPartHit.None)
      {
        this.bodyDamage.Flags |= 32U /*0x20*/;
        this.bodyDamage.ShouldBeCrawler = true;
      }
      if ((enumBodyPartHit & EnumBodyPartHit.LeftLowerLeg) > EnumBodyPartHit.None)
      {
        this.bodyDamage.Flags |= 64U /*0x40*/;
        this.bodyDamage.ShouldBeCrawler = true;
      }
      if ((enumBodyPartHit & EnumBodyPartHit.RightUpperLeg) > EnumBodyPartHit.None)
      {
        this.bodyDamage.Flags |= 128U /*0x80*/;
        this.bodyDamage.ShouldBeCrawler = true;
      }
      if ((enumBodyPartHit & EnumBodyPartHit.RightLowerLeg) > EnumBodyPartHit.None)
      {
        this.bodyDamage.Flags |= 256U /*0x0100*/;
        this.bodyDamage.ShouldBeCrawler = true;
      }
    }
    if (_dmResponse.TurnIntoCrawler)
      this.bodyDamage.ShouldBeCrawler = true;
    if (!_dmResponse.CrippleLegs)
      return;
    if (_dmResponse.HitBodyPart.IsLeftLeg())
      this.bodyDamage.Flags |= 4096U /*0x1000*/;
    if (!_dmResponse.HitBodyPart.IsRightLeg())
      return;
    this.bodyDamage.Flags |= 8192U /*0x2000*/;
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public void ExecuteDismember(bool restoreState)
  {
    if (Object.op_Equality((Object) this.emodel, (Object) null) || Object.op_Equality((Object) this.emodel.avatarController, (Object) null))
      return;
    this.emodel.avatarController.DismemberLimb(this.bodyDamage, restoreState);
    if (!this.bodyDamage.ShouldBeCrawler)
      return;
    this.SetupCrawlerState(restoreState);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void SetupCrawlerState(bool restoreState)
  {
    if (!this.IsDead())
    {
      this.emodel.avatarController.TurnIntoCrawler(restoreState);
      this.SetMaxHeight(0.5f);
      ItemValue _bareHandItemValue = (ItemValue) null;
      if (EntityClass.list[this.entityClass].Properties.Values.ContainsKey(EntityClass.PropHandItemCrawler))
      {
        _bareHandItemValue = ItemClass.GetItem(EntityClass.list[this.entityClass].Properties.Values[EntityClass.PropHandItemCrawler]);
        if (_bareHandItemValue.IsEmpty())
          _bareHandItemValue = (ItemValue) null;
      }
      if (_bareHandItemValue == null)
        _bareHandItemValue = ItemClass.GetItem("meleeHandZombie02");
      this.inventory.SetBareHandItem(_bareHandItemValue);
      this.TurnIntoCrawler();
    }
    this.walkType = 21;
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual void TurnIntoCrawler()
  {
  }

  public void ClearStun()
  {
    this.bodyDamage.CurrentStun = EnumEntityStunType.None;
    this.bodyDamage.StunDuration = 0.0f;
    this.SetCVar("_stunned", 0.0f);
  }

  public void SetStun(EnumEntityStunType stun)
  {
    this.bodyDamage.CurrentStun = stun;
    this.SetCVar("_stunned", 1f);
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual void onSpawnStateChanged()
  {
    if (!this.m_addedToWorld)
      return;
    this.StartStopLivingSound();
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void StartStopLivingSound()
  {
    if (this.soundLiving != null)
    {
      if (this.Spawned)
      {
        if (!this.IsDead() && this.Health > 0)
        {
          Manager.Play((Entity) this, this.soundLiving);
          this.soundLivingID = 0;
        }
      }
      else if (this.soundLivingID >= 0)
      {
        Manager.Stop(this.entityId, this.soundLiving);
        this.soundLivingID = -1;
      }
    }
    if (!this.Spawned || this.soundSpawn == null || this.SleeperSupressLivingSounds)
      return;
    this.PlayOneShot(this.soundSpawn);
  }

  public void CrouchHeightFixedUpdate()
  {
    if (this.crouchType == 0 || (double) this.physicsBaseHeight <= 1.2999999523162842)
      return;
    float num1 = this.physicsBaseHeight;
    if (this.IsInElevator())
      num1 *= 1.06f;
    if (this.emodel.IsRagdollMovement || this.bodyDamage.CurrentStun == EnumEntityStunType.Prone)
      num1 = this.physicsBaseHeight * 0.08f;
    float num2 = this.m_characterController.GetRadius() * 0.9f;
    float num3 = num2 + 0.3f;
    float num4 = num1 + 0.01f - num3 - num2;
    Vector3 vector3 = this.PhysicsTransform.position;
    vector3.y += num3;
    if (this.moveHelper != null && (this.moveHelper.BlockedFlags & 3) == 2)
      vector3 = Vector3.op_Addition(vector3, Vector3.op_Multiply(this.ModelTransform.forward, 0.15f));
    RaycastHit raycastHit;
    if (Physics.SphereCast(vector3, num2, Vector3.up, ref raycastHit, num4, 1083277312))
    {
      Transform transform = ((RaycastHit) ref raycastHit).transform;
      if (Object.op_Implicit((Object) transform) && ((Component) transform).CompareTag("Physics"))
      {
        Entity component = ((Component) transform).GetComponent<Entity>();
        if (!Object.op_Implicit((Object) component))
          return;
        component.PhysicsPush(Vector3.op_Multiply(transform.forward, 0.1f * Time.fixedDeltaTime), ((RaycastHit) ref raycastHit).point, true);
        return;
      }
      if ((double) this.world.GetBlock(new Vector3i(Vector3.op_Addition(((RaycastHit) ref raycastHit).point, Origin.position))).Block.Damage <= 0.0)
        num1 = (float) ((double) ((RaycastHit) ref raycastHit).point.y - ((double) vector3.y - (double) num3) - 0.20999999344348907);
    }
    float _height;
    if ((double) num1 < (double) this.physicsHeight)
    {
      if (this.IsInElevator())
        return;
      _height = Mathf.MoveTowards(this.physicsHeight, num1, 0.099999994f);
    }
    else
      _height = Mathf.MoveTowards(this.physicsHeight, num1, 0.0166666657f);
    this.SetHeight(_height);
    float num5 = this.physicsBaseHeight * 0.7f;
    if ((double) _height <= (double) num5)
    {
      this.crouchBendPerTarget = 0.0f;
      int _walkType = 8;
      if (this.walkType != _walkType && this.walkType != 21)
      {
        this.walkTypeBeforeCrouch = this.walkType;
        this.SetWalkType(_walkType);
      }
    }
    else
    {
      this.crouchBendPerTarget = (float) (1.0 - ((double) _height - (double) num5) / ((double) this.physicsBaseHeight - (double) num5));
      if (this.walkTypeBeforeCrouch != 0)
      {
        this.SetWalkType(this.walkTypeBeforeCrouch);
        this.walkTypeBeforeCrouch = 0;
      }
    }
    this.crouchBendPer = Mathf.MoveTowards(this.crouchBendPer, this.crouchBendPerTarget, 0.099999994f);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void SetWalkType(int _walkType)
  {
    this.walkType = _walkType;
    this.emodel.avatarController.SetWalkType(_walkType, true);
  }

  public int GetWalkType() => this.walkType;

  public bool IsWalkTypeACrawl() => this.walkType >= 20;

  public string GetRightHandTransformName() => this.rightHandTransformName;

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual bool isGameMessageOnDeath() => true;

  public override float GetLightBrightness()
  {
    Vector3i blockPosition = this.GetBlockPosition();
    Vector3i blockPos = blockPosition;
    blockPos.y += Mathf.RoundToInt(this.height + 0.5f);
    return Utils.FastMax(this.world.GetLightBrightness(blockPosition), this.world.GetLightBrightness(blockPos));
  }

  public virtual float GetLightLevel()
  {
    EntityAlive attachedToEntity = this.AttachedToEntity as EntityAlive;
    return Object.op_Implicit((Object) attachedToEntity) ? attachedToEntity.GetLightLevel() : this.inventory.GetLightLevel();
  }

  public override int AttachToEntity(Entity _other, int slot = -1)
  {
    slot = base.AttachToEntity(_other, slot);
    if (slot >= 0)
    {
      this.CurrentMovementTag = EntityAlive.MovementTagIdle;
      this.Crouching = false;
      this.saveInventory = (Inventory) null;
      if (_other is EntityAlive && _other.GetAttachedToInfo(slot).bReplaceLocalInventory)
      {
        this.saveInventory = this.inventory;
        this.saveHoldingItemIdxBeforeAttach = this.inventory.holdingItemIdx;
        this.inventory.SetHoldingItemIdxNoHolsterTime(this.inventory.DUMMY_SLOT_IDX);
        this.inventory = ((EntityAlive) _other).inventory;
      }
      this.bPlayerStatsChanged |= !this.isEntityRemote;
    }
    return slot;
  }

  public override void Detach()
  {
    if (this.saveInventory != null)
    {
      this.inventory = this.saveInventory;
      this.inventory.SetHoldingItemIdxNoHolsterTime(this.saveHoldingItemIdxBeforeAttach);
      this.saveInventory = (Inventory) null;
    }
    base.Detach();
    this.bPlayerStatsChanged |= !this.isEntityRemote;
  }

  public override void Write(BinaryWriter _bw, bool _bNetworkWrite)
  {
    base.Write(_bw, _bNetworkWrite);
    _bw.Write(this.deathHealth);
  }

  public override void Read(byte _version, BinaryReader _br)
  {
    base.Read(_version, _br);
    if (_version <= (byte) 24)
      return;
    this.deathHealth = _br.ReadInt32();
  }

  public virtual string ToString()
  {
    return $"[type={((object) this).GetType().Name}, name={GameUtils.SafeStringFormat(this.EntityName)}, id={this.entityId}]";
  }

  public virtual void FireEvent(MinEventTypes _eventType, bool useInventory = true)
  {
    EntityClass.list[this.entityClass].Effects?.FireEvent(_eventType, this.MinEventContext);
    if (this.Progression != null)
      this.Progression.FireEvent(_eventType, this.MinEventContext);
    if (this.challengeJournal != null)
      this.challengeJournal.FireEvent(_eventType, this.MinEventContext);
    if (this.inventory != null & useInventory)
      this.inventory.FireEvent(_eventType, this.MinEventContext);
    this.equipment.FireEvent(_eventType, this.MinEventContext);
    this.Buffs.FireEvent(_eventType, this.MinEventContext);
  }

  public float GetCVar(string _varName)
  {
    return this.Buffs == null ? 0.0f : this.Buffs.GetCustomVar(_varName);
  }

  public void SetCVar(string _varName, float _value)
  {
    if (this.Buffs == null)
      return;
    this.Buffs.SetCustomVar(_varName, _value);
  }

  public virtual void BuffAdded(BuffValue _buff)
  {
  }

  public override void OnCollisionForward(Transform t, Collision collision, bool isStay)
  {
    if (!this.emodel.IsRagdollActive)
      return;
    Vector3 vector3_1 = collision.relativeVelocity;
    if ((double) ((Vector3) ref vector3_1).sqrMagnitude < 1.0 / 16.0)
      return;
    vector3_1 = collision.impulse;
    float sqrMagnitude = ((Vector3) ref vector3_1).sqrMagnitude;
    if ((double) sqrMagnitude < 400.0)
      return;
    if (this.IsDead())
    {
      EntityAlive.ImpactData impactData;
      this.impacts.TryGetValue(t, out impactData);
      ++impactData.count;
      this.impacts[t] = impactData;
      if (impactData.count >= 10)
      {
        if (impactData.count == 10)
        {
          Rigidbody component1 = ((Component) t).GetComponent<Rigidbody>();
          if (Object.op_Implicit((Object) component1))
          {
            component1.velocity = Vector3.zero;
            component1.angularVelocity = Vector3.zero;
            component1.drag = 0.5f;
            component1.angularDrag = 0.5f;
          }
          CharacterJoint component2 = ((Component) t).GetComponent<CharacterJoint>();
          if (Object.op_Implicit((Object) component2))
            component2.enableProjection = false;
        }
        if (impactData.count != 25 || ((Component) t).gameObject.CompareTag("E_BP_Body"))
          return;
        ((Component) t).GetComponent<Collider>().enabled = false;
        return;
      }
    }
    if ((double) Time.time - (double) this.impactSoundTime < 0.25)
      return;
    this.impactSoundTime = Time.time;
    if ((double) t.lossyScale.x == 0.0)
      return;
    string soundGroupName = "impactbodylight";
    if ((double) sqrMagnitude >= 3600.0)
      soundGroupName = "impactbodyheavy";
    Vector3 vector3_2 = Vector3.zero;
    int contactCount = collision.contactCount;
    for (int index = 0; index < contactCount; ++index)
    {
      ContactPoint contact = collision.GetContact(index);
      vector3_2 = Vector3.op_Addition(vector3_2, ((ContactPoint) ref contact).point);
    }
    Manager.BroadcastPlay(Vector3.op_Addition(Vector3.op_Multiply(vector3_2, 1f / (float) contactCount), Origin.position), soundGroupName);
  }

  public void AddParticle(string _name, Transform _t)
  {
    if (this.particles.ContainsKey(_name))
      this.particles[_name] = _t;
    else
      this.particles.Add(_name, _t);
  }

  public bool RemoveParticle(string _name)
  {
    Transform transform;
    if (!this.particles.Remove(_name, ref transform))
      return false;
    if (Object.op_Implicit((Object) transform))
      Object.Destroy((Object) ((Component) transform).gameObject);
    return true;
  }

  public bool HasParticle(string _name) => this.particles.TryGetValue(_name, out Transform _);

  public void AddPart(string _name, Transform _t)
  {
    if (this.parts.ContainsKey(_name))
      this.parts[_name] = _t;
    else
      this.parts.Add(_name, _t);
  }

  public void RemovePart(string _name)
  {
    Transform transform;
    if (!this.parts.TryGetValue(_name, out transform))
      return;
    this.parts.Remove(_name);
    if (!Object.op_Implicit((Object) transform))
      return;
    ((Object) ((Component) transform).gameObject).name = ".";
    Object.Destroy((Object) ((Component) transform).gameObject);
  }

  public void SetPartActive(string _name, bool isActive)
  {
    Transform transform;
    if (!this.parts.TryGetValue(_name, out transform) || !Object.op_Implicit((Object) transform))
      return;
    bool flag = true;
    for (int index = transform.childCount - 1; index >= 0; --index)
    {
      Transform child = transform.GetChild(index);
      if (((Component) child).CompareTag("ModOn"))
      {
        ((Component) child).gameObject.SetActive(isActive);
        flag = false;
      }
      else if (((Component) child).CompareTag("ModMesh"))
      {
        if (((Object) transform.parent).name == "CameraNode")
          ((Component) child).gameObject.SetActive(false);
        flag = false;
      }
    }
    if (!flag)
      return;
    ((Component) transform).gameObject.SetActive(isActive);
  }

  public void AddOwnedEntity(OwnedEntityData _entityData)
  {
    if (_entityData == null)
      return;
    this.ownedEntities.Add(_entityData);
  }

  public void AddOwnedEntity(Entity _entity)
  {
    if (this.ownedEntities.Find((Predicate<OwnedEntityData>) ([PublicizedFrom(EAccessModifier.Internal)] (e) => e.Id == _entity.entityId)) != null)
      return;
    this.AddOwnedEntity(new OwnedEntityData(_entity));
  }

  public void RemoveOwnedEntity(OwnedEntityData _entityData)
  {
    if (_entityData == null)
      return;
    this.ownedEntities.Remove(_entityData);
  }

  public void RemoveOwnedEntity(int _entityId)
  {
    this.RemoveOwnedEntity(this.ownedEntities.Find((Predicate<OwnedEntityData>) ([PublicizedFrom(EAccessModifier.Internal)] (e) => e.Id == _entityId)));
  }

  public void RemoveOwnedEntity(Entity _entity) => this.RemoveOwnedEntity(_entity.entityId);

  public OwnedEntityData GetOwnedEntity(int _entityId)
  {
    return this.ownedEntities.Find((Predicate<OwnedEntityData>) ([PublicizedFrom(EAccessModifier.Internal)] (e) => e.Id == _entityId));
  }

  public OwnedEntityData[] GetOwnedEntityClass(string name)
  {
    List<OwnedEntityData> ownedEntityDataList = new List<OwnedEntityData>();
    for (int index = 0; index < this.ownedEntities.Count; ++index)
    {
      OwnedEntityData ownedEntity = this.ownedEntities[index];
      if (EntityClass.list[ownedEntity.ClassId].entityClassName.ContainsCaseInsensitive(name))
        ownedEntityDataList.Add(ownedEntity);
    }
    return ownedEntityDataList.ToArray();
  }

  public bool HasOwnedEntity(int _entityId) => this.GetOwnedEntity(_entityId) != null;

  public OwnedEntityData[] GetOwnedEntities() => this.ownedEntities.ToArray();

  public int OwnedEntityCount => this.ownedEntities.Count;

  public void ClearOwnedEntities() => this.ownedEntities.Clear();

  public void HandleSetNavName()
  {
    if (this.NavObject == null)
      return;
    this.NavObject.name = this.entityName;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void UpdateDynamicRagdoll()
  {
    if (!this._dynamicRagdoll.HasFlag((Enum) DynamicRagdollFlags.Active))
      return;
    if (Vector3.op_Inequality(this.accumulatedRootMotion, Vector3.zero))
      this._dynamicRagdollRootMotion = this.accumulatedRootMotion;
    if (this._dynamicRagdoll.HasFlag((Enum) DynamicRagdollFlags.UseBoneVelocities))
    {
      this._ragdollPositionsPrev.Clear();
      this._ragdollPositionsCur.CopyTo<Vector3>((IList<Vector3>) this._ragdollPositionsPrev);
      this.emodel.CaptureRagdollPositions(this._ragdollPositionsCur);
    }
    if (!this._dynamicRagdoll.HasFlag((Enum) DynamicRagdollFlags.RagdollOnFall) || this.onGround)
      return;
    this.ActivateDynamicRagdoll();
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual void AnalyticsSendDeath(DamageResponse _dmResponse)
  {
  }

  public virtual string MakeDebugNameInfo() => string.Empty;

  public static void SetupAllDebugNameHUDs(bool _isAdd)
  {
    List<Entity> list = GameManager.Instance.World.Entities.list;
    for (int index = 0; index < list.Count; ++index)
    {
      EntityAlive entityAlive = list[index] as EntityAlive;
      if (Object.op_Implicit((Object) entityAlive))
        entityAlive.SetupDebugNameHUD(_isAdd);
    }
  }

  public void SetupDebugNameHUD(bool _isAdd)
  {
    if (this is EntityPlayer)
      return;
    GUIHUDEntityName component = ((Component) this.ModelTransform).GetComponent<GUIHUDEntityName>();
    if (_isAdd)
    {
      if (Object.op_Implicit((Object) component))
        return;
      ((Component) this.ModelTransform).gameObject.AddComponent<GUIHUDEntityName>();
    }
    else
    {
      if (!Object.op_Implicit((Object) component))
        return;
      Object.Destroy((Object) component);
    }
  }

  public EModelBase.HeadStates GetHeadState()
  {
    return this.EntityClass.CanBigHead ? this.emodel.HeadState : EModelBase.HeadStates.Standard;
  }

  public void SetBigHead()
  {
    switch (this)
    {
      case EntityAnimal _:
      case EntityEnemy _:
      case EntityTrader _:
        if (!this.EntityClass.CanBigHead || this.emodel.HeadState != EModelBase.HeadStates.Standard)
          break;
        this.emodel.HeadState = EModelBase.HeadStates.Growing;
        Manager.BroadcastPlayByLocalPlayer(this.position, "twitch_bighead_inflate");
        break;
    }
  }

  public void ResetHead()
  {
    switch (this)
    {
      case EntityAnimal _:
      case EntityEnemy _:
      case EntityTrader _:
        if (!this.EntityClass.CanBigHead || this.emodel.HeadState != EModelBase.HeadStates.BigHead && this.emodel.HeadState != EModelBase.HeadStates.Growing)
          break;
        this.StartCoroutine(this.resetHeadLater(this.emodel));
        break;
    }
  }

  public void SetDancing(bool enabled)
  {
    if (this.EntityClass.DanceTypeID != 0)
      this.IsDancing = enabled;
    else
      this.IsDancing = false;
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public IEnumerator resetHeadLater(EModelBase model)
  {
    EntityAlive entityAlive = this;
    yield return (object) new WaitForSeconds(0.25f);
    if (Object.op_Inequality((Object) entityAlive.emodel, (Object) null) && Object.op_Inequality((Object) entityAlive.emodel.GetHeadTransform(), (Object) null) && (double) entityAlive.emodel.GetHeadTransform().localScale.x > 1.0)
    {
      entityAlive.emodel.HeadState = EModelBase.HeadStates.Shrinking;
      Manager.BroadcastPlayByLocalPlayer(entityAlive.position, "twitch_bighead_deflate");
    }
  }

  public void SetSpawnByData(int newSpawnByID, string newSpawnByName)
  {
    this.spawnById = newSpawnByID;
    this.spawnByName = newSpawnByName;
    this.bPlayerStatsChanged |= !this.isEntityRemote;
  }

  [PublicizedFrom(EAccessModifier.Internal)]
  public void SetHeadSize(float overrideHeadSize)
  {
    this.OverrideHeadSize = overrideHeadSize;
    this.emodel.SetHeadScale(overrideHeadSize);
  }

  public void SetVehiclePoseMode(int _pose)
  {
    this.vehiclePoseMode = _pose;
    if (_pose == this.GetVehicleAnimation())
      return;
    this.Crouching = false;
    this.SetVehicleAnimation(AvatarController.vehiclePoseHash, _pose);
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public void updateNetworkStats()
  {
    if (this.networkStatsUpdateQueue.Count <= 0)
      return;
    EntityAlive.NetworkStatChange networkStatsUpdate = this.networkStatsUpdateQueue[0];
    this.networkStatsUpdateQueue.RemoveAt(0);
    if (networkStatsUpdate.m_NetworkStats != null)
    {
      networkStatsUpdate.m_NetworkStats.ToEntity(this);
    }
    else
    {
      EntityAlive.EntityNetworkHoldingData holdingData = networkStatsUpdate.m_HoldingData;
      if (holdingData == null)
        return;
      ItemStack holdingItemStack = holdingData.m_HoldingItemStack;
      byte holdingItemIndex = holdingData.m_HoldingItemIndex;
      if (!this.inventory.GetItem((int) holdingItemIndex).Equals((object) holdingItemStack))
        this.inventory.SetItem((int) holdingItemIndex, holdingItemStack);
      if (this.inventory.holdingItemIdx == (int) holdingItemIndex)
        return;
      this.inventory.SetHoldingItemIdxNoHolsterTime((int) holdingItemIndex);
    }
  }

  public void EnqueueNetworkStats(EntityAlive.EntityNetworkStats netStats)
  {
    this.networkStatsUpdateQueue.Add(new EntityAlive.NetworkStatChange()
    {
      m_NetworkStats = netStats
    });
  }

  public void EnqueueNetworkHoldingData(ItemStack holdingItemStack, byte holdingItemIndex)
  {
    this.networkStatsUpdateQueue.Add(new EntityAlive.NetworkStatChange()
    {
      m_HoldingData = new EntityAlive.EntityNetworkHoldingData()
      {
        m_HoldingItemStack = holdingItemStack,
        m_HoldingItemIndex = holdingItemIndex
      }
    });
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public EntityAlive()
  {
  }

  [PublicizedFrom(EAccessModifier.Private)]
  static EntityAlive()
  {
  }

  public enum JumpState
  {
    Off,
    Climb,
    Leap,
    Air,
    Land,
    SwimStart,
    Swim,
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public class FallBehavior
  {
    public string Name;
    public readonly EntityAlive.FallBehavior.Op ResponseOp;
    public readonly FloatRange Height;
    public readonly float Weight;
    public readonly FloatRange RagePer;
    public readonly FloatRange RageTime;
    public readonly IntRange Difficulty;

    public FallBehavior(
      string name,
      EntityAlive.FallBehavior.Op type,
      FloatRange height,
      float weight,
      FloatRange ragePer,
      FloatRange rageTime,
      IntRange difficulty)
    {
      this.Name = name;
      this.ResponseOp = type;
      this.Height = height;
      this.Weight = weight;
      this.RagePer = ragePer;
      this.RageTime = rageTime;
      this.Difficulty = difficulty;
    }

    public enum Op
    {
      None,
      Land,
      LandLow,
      LandHard,
      Stumble,
      Ragdoll,
    }
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public class DestroyBlockBehavior
  {
    public string Name;
    public readonly EntityAlive.DestroyBlockBehavior.Op ResponseOp;
    public readonly float Weight;
    public readonly FloatRange RagePer;
    public readonly FloatRange RageTime;
    public readonly IntRange Difficulty = new IntRange(int.MinValue, int.MaxValue);

    public DestroyBlockBehavior(
      string name,
      EntityAlive.DestroyBlockBehavior.Op type,
      float weight,
      FloatRange ragePer,
      FloatRange rageTime,
      IntRange difficulty)
    {
      this.Name = name;
      this.ResponseOp = type;
      this.Weight = weight;
      this.RagePer = ragePer;
      this.RageTime = rageTime;
      this.Difficulty = difficulty;
    }

    public enum Op
    {
      None,
      Ragdoll,
      Stumble,
    }
  }

  public enum EnumApproachState
  {
    Ok,
    TooFarAway,
    BlockedByWorldMesh,
    BlockedByEntity,
    Unknown,
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public struct WeightBehavior
  {
    public float weight;
    public int index;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public struct ImpactData
  {
    public int count;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public class NetworkStatChange
  {
    public EntityAlive.EntityNetworkStats m_NetworkStats;
    public EntityAlive.EntityNetworkHoldingData m_HoldingData;
  }

  public class EntityNetworkHoldingData
  {
    public ItemStack m_HoldingItemStack;
    public byte m_HoldingItemIndex;
  }

  public class EntityNetworkStats
  {
    [PublicizedFrom(EAccessModifier.Private)]
    public int experience;
    [PublicizedFrom(EAccessModifier.Private)]
    public int level;
    [PublicizedFrom(EAccessModifier.Private)]
    public int killed;
    [PublicizedFrom(EAccessModifier.Private)]
    public int killedZombies;
    [PublicizedFrom(EAccessModifier.Private)]
    public int killedPlayers;
    [PublicizedFrom(EAccessModifier.Private)]
    public ItemStack holdingItemStack;
    [PublicizedFrom(EAccessModifier.Private)]
    public byte holdingItemIndex;
    [PublicizedFrom(EAccessModifier.Private)]
    public int deathHealth;
    [PublicizedFrom(EAccessModifier.Private)]
    public int teamNumber;
    [PublicizedFrom(EAccessModifier.Private)]
    public Equipment equipment;
    [PublicizedFrom(EAccessModifier.Private)]
    public bool hasProgression;
    [PublicizedFrom(EAccessModifier.Private)]
    public byte[] progressionsData;
    [PublicizedFrom(EAccessModifier.Private)]
    public int attachedToEntityId;
    [PublicizedFrom(EAccessModifier.Private)]
    public string entityName;
    [PublicizedFrom(EAccessModifier.Private)]
    public float distanceWalked;
    [PublicizedFrom(EAccessModifier.Private)]
    public uint totalItemsCrafted;
    [PublicizedFrom(EAccessModifier.Private)]
    public float longestLife;
    [PublicizedFrom(EAccessModifier.Private)]
    public float currentLife;
    [PublicizedFrom(EAccessModifier.Private)]
    public float totalTimePlayed;
    [PublicizedFrom(EAccessModifier.Private)]
    public int vehiclePose;
    [PublicizedFrom(EAccessModifier.Private)]
    public bool isSpectator;
    [PublicizedFrom(EAccessModifier.Private)]
    public bool isPlayer;

    public void FillFromEntity(EntityAlive _entity)
    {
      this.killed = _entity.Died;
      this.holdingItemStack = _entity.inventory.holdingItemStack;
      this.holdingItemIndex = (byte) _entity.inventory.holdingItemIdx;
      this.deathHealth = _entity.DeathHealth;
      this.teamNumber = _entity.TeamNumber;
      this.equipment = _entity.equipment;
      if (Object.op_Equality((Object) GameManager.Instance.World.GetPrimaryPlayer(), (Object) _entity))
        _entity.inventory.TurnOffLightFlares();
      if (_entity.Progression != null && _entity.Progression.bProgressionStatsChanged)
      {
        _entity.Progression.bProgressionStatsChanged = false;
        this.hasProgression = true;
        this.progressionsData = _entity.Progression.ToBytes();
      }
      this.attachedToEntityId = Object.op_Inequality((Object) _entity.AttachedToEntity, (Object) null) ? _entity.AttachedToEntity.entityId : -1;
      this.entityName = _entity.EntityName;
      EntityPlayer entityPlayer = _entity as EntityPlayer;
      if (Object.op_Inequality((Object) entityPlayer, (Object) null))
      {
        this.isPlayer = true;
        this.killedPlayers = _entity.KilledPlayers;
        this.killedZombies = _entity.KilledZombies;
        this.experience = entityPlayer.Progression.ExpToNextLevel;
        this.level = entityPlayer.Progression.Level;
        this.totalItemsCrafted = entityPlayer.totalItemsCrafted;
        this.distanceWalked = entityPlayer.distanceWalked;
        this.longestLife = entityPlayer.longestLife;
        this.currentLife = entityPlayer.currentLife;
        this.totalTimePlayed = entityPlayer.totalTimePlayed;
        this.vehiclePose = entityPlayer.GetVehicleAnimation();
        this.isSpectator = entityPlayer.IsSpectator;
      }
      else
      {
        this.isPlayer = false;
        this.experience = 0;
        this.level = 1;
        this.distanceWalked = 0.0f;
        this.totalItemsCrafted = 0U;
        this.longestLife = 0.0f;
        this.currentLife = 0.0f;
        this.totalTimePlayed = 0.0f;
      }
    }

    public void ToEntity(EntityAlive _entity)
    {
      _entity.Died = this.killed;
      _entity.DeathHealth = this.deathHealth;
      _entity.TeamNumber = this.teamNumber;
      _entity.inventory.bResetLightLevelWhenChanged = true;
      if (!_entity.inventory.GetItem((int) this.holdingItemIndex).Equals((object) this.holdingItemStack))
      {
        _entity.inventory.SetItem((int) this.holdingItemIndex, this.holdingItemStack);
        _entity.inventory.ForceHoldingItemUpdate();
      }
      if (_entity.inventory.holdingItemIdx != (int) this.holdingItemIndex)
        _entity.inventory.SetHoldingItemIdxNoHolsterTime((int) this.holdingItemIndex);
      _entity.equipment.Apply(this.equipment, false);
      if (this.hasProgression)
      {
        _entity.Progression = Progression.FromBytes(this.progressionsData, _entity);
        if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && _entity.Progression != null)
          _entity.Progression.bProgressionStatsChanged = true;
      }
      _entity.SetEntityName(this.entityName);
      EntityPlayer entityPlayer = _entity as EntityPlayer;
      if (!Object.op_Inequality((Object) entityPlayer, (Object) null) || !this.isPlayer)
        return;
      if (_entity.NavObject != null)
        _entity.NavObject.name = this.entityName;
      _entity.KilledZombies = this.killedZombies;
      _entity.KilledPlayers = this.killedPlayers;
      entityPlayer.Progression.ExpToNextLevel = this.experience;
      entityPlayer.Progression.Level = this.level;
      entityPlayer.totalItemsCrafted = this.totalItemsCrafted;
      entityPlayer.distanceWalked = this.distanceWalked;
      entityPlayer.longestLife = this.longestLife;
      entityPlayer.currentLife = this.currentLife;
      entityPlayer.totalTimePlayed = this.totalTimePlayed;
      entityPlayer.SetVehiclePoseMode(this.vehiclePose);
      entityPlayer.IsSpectator = this.isSpectator;
    }

    public void read(PooledBinaryReader _reader)
    {
      this.killed = _reader.ReadInt32();
      this.holdingItemStack = new ItemStack();
      this.holdingItemStack.Read((BinaryReader) _reader);
      this.holdingItemIndex = _reader.ReadByte();
      this.deathHealth = _reader.ReadInt32();
      this.teamNumber = (int) _reader.ReadByte();
      this.equipment = Equipment.Read((BinaryReader) _reader);
      this.attachedToEntityId = _reader.ReadInt32();
      this.entityName = _reader.ReadString();
      this.isPlayer = _reader.ReadBoolean();
      if (this.isPlayer)
      {
        this.killedZombies = _reader.ReadInt32();
        this.killedPlayers = _reader.ReadInt32();
        this.experience = _reader.ReadInt32();
        this.level = _reader.ReadInt32();
        this.totalItemsCrafted = _reader.ReadUInt32();
        this.distanceWalked = _reader.ReadSingle();
        this.longestLife = _reader.ReadSingle();
        this.currentLife = _reader.ReadSingle();
        this.totalTimePlayed = _reader.ReadSingle();
        this.vehiclePose = _reader.ReadInt32();
        this.isSpectator = _reader.ReadBoolean();
      }
      this.hasProgression = _reader.ReadBoolean();
      if (!this.hasProgression)
        return;
      int count = (int) _reader.ReadInt16();
      this.progressionsData = new byte[count];
      _reader.Read(this.progressionsData, 0, count);
    }

    public void write(PooledBinaryWriter _writer)
    {
      _writer.Write(this.killed);
      this.holdingItemStack.Write((BinaryWriter) _writer);
      _writer.Write(this.holdingItemIndex);
      _writer.Write(this.deathHealth);
      _writer.Write((byte) this.teamNumber);
      this.equipment.Write((BinaryWriter) _writer);
      _writer.Write(this.attachedToEntityId);
      _writer.Write(this.entityName);
      _writer.Write(this.isPlayer);
      if (this.isPlayer)
      {
        _writer.Write(this.killedZombies);
        _writer.Write(this.killedPlayers);
        _writer.Write(this.experience);
        _writer.Write(this.level);
        _writer.Write(this.totalItemsCrafted);
        _writer.Write(this.distanceWalked);
        _writer.Write(this.longestLife);
        _writer.Write(this.currentLife);
        _writer.Write(this.totalTimePlayed);
        _writer.Write(this.vehiclePose);
        _writer.Write(this.isSpectator);
      }
      _writer.Write(this.hasProgression);
      if (!this.hasProgression)
        return;
      _writer.Write((short) this.progressionsData.Length);
      _writer.Write(this.progressionsData, 0, this.progressionsData.Length);
    }

    public void SetName(string name) => this.entityName = name;
  }
}
