// Decompiled with JetBrains decompiler
// Type: SleeperVolume
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

#nullable disable
public class SleeperVolume
{
  [PublicizedFrom(EAccessModifier.Private)]
  public const byte cVersion = 21;
  [PublicizedFrom(EAccessModifier.Private)]
  public const int cSpawnDelay = 2;
  [PublicizedFrom(EAccessModifier.Private)]
  public const int cDespawnDelay = 900;
  [PublicizedFrom(EAccessModifier.Private)]
  public const int cDespawnPassiveDelay = 200;
  [PublicizedFrom(EAccessModifier.Private)]
  public const int cBedrollClearTime = 24000;
  [PublicizedFrom(EAccessModifier.Private)]
  public const int cPlayerInsideDelayTime = 1000;
  [PublicizedFrom(EAccessModifier.Private)]
  public const float cPlayerYOffset = 0.8f;
  [PublicizedFrom(EAccessModifier.Private)]
  public const float cAttackPaddingXZ = -0.1f;
  [PublicizedFrom(EAccessModifier.Private)]
  public const float cPassivePaddingXZ = -0.3f;
  [PublicizedFrom(EAccessModifier.Private)]
  public const float cPassiveNoisePadding = 0.9f;
  public static Vector3i chunkPadding = new Vector3i(12, 1, 12);
  [PublicizedFrom(EAccessModifier.Private)]
  public static Vector3i triggerPaddingMin = new Vector3i(8f, 0.7f, 8f);
  [PublicizedFrom(EAccessModifier.Private)]
  public static Vector3i triggerPaddingMax = new Vector3i(8f, 0.7f, 8f);
  [PublicizedFrom(EAccessModifier.Private)]
  public static Vector3i unpadding = new Vector3i(14, 16 /*0x10*/, 14);
  [PublicizedFrom(EAccessModifier.Private)]
  public PrefabInstance prefabInstance;
  [PublicizedFrom(EAccessModifier.Private)]
  public short groupId;
  [PublicizedFrom(EAccessModifier.Private)]
  public short spawnCountMin;
  [PublicizedFrom(EAccessModifier.Private)]
  public short spawnCountMax;
  public const int cTriggerFlagsMask = 7;
  public const int cFlagsHasScript = 16 /*0x10*/;
  [PublicizedFrom(EAccessModifier.Private)]
  public int flags;
  [PublicizedFrom(EAccessModifier.Private)]
  public List<SleeperVolume.SpawnPoint> spawnPointList = new List<SleeperVolume.SpawnPoint>();
  [PublicizedFrom(EAccessModifier.Private)]
  public List<int> spawnsAvailable;
  [PublicizedFrom(EAccessModifier.Private)]
  public string groupName;
  [PublicizedFrom(EAccessModifier.Private)]
  public List<SleeperVolume.GroupCount> groupCountList;
  [PublicizedFrom(EAccessModifier.Private)]
  public Dictionary<int, SleeperVolume.RespawnData> respawnMap = new Dictionary<int, SleeperVolume.RespawnData>();
  [PublicizedFrom(EAccessModifier.Private)]
  public List<int> respawnList;
  [PublicizedFrom(EAccessModifier.Private)]
  public int gameStage;
  [PublicizedFrom(EAccessModifier.Private)]
  public int lastClassId;
  [PublicizedFrom(EAccessModifier.Private)]
  public ulong respawnTime = ulong.MaxValue;
  [PublicizedFrom(EAccessModifier.Private)]
  public int numSpawned;
  [PublicizedFrom(EAccessModifier.Private)]
  public bool isSpawned;
  [PublicizedFrom(EAccessModifier.Private)]
  public bool isSpawning;
  [PublicizedFrom(EAccessModifier.Private)]
  public int spawnDelay;
  [PublicizedFrom(EAccessModifier.Private)]
  public int ticksUntilDespawn;
  [PublicizedFrom(EAccessModifier.Private)]
  public EntityPlayer playerTouchedToUpdate;
  [PublicizedFrom(EAccessModifier.Private)]
  public EntityPlayer playerTouchedTrigger;
  [PublicizedFrom(EAccessModifier.Private)]
  public bool hasPassives;
  [PublicizedFrom(EAccessModifier.Private)]
  public SleeperVolume.ETriggerType triggerState = SleeperVolume.ETriggerType.Passive;
  public bool wasCleared;
  public bool isQuestExclude;
  public bool isPriority;
  public Vector3i BoxMin;
  public Vector3i BoxMax;
  public Vector3 Center;
  public List<byte> TriggeredByIndices = new List<byte>();
  [PublicizedFrom(EAccessModifier.Private)]
  public MinScript minScript;
  [PublicizedFrom(EAccessModifier.Private)]
  public const int cWanderingCountdown = 10;
  [PublicizedFrom(EAccessModifier.Private)]
  public static int wanderingCountdown = 5;
  [PublicizedFrom(EAccessModifier.Private)]
  public static GameRandom sleeperRandom;
  public static int TickSpawnCount;
  [PublicizedFrom(EAccessModifier.Private)]
  public const int cSpawnPerTickMax = 2;
  [PublicizedFrom(EAccessModifier.Private)]
  public static float[] difficultyTierScale = new float[7]
  {
    1f,
    1f,
    1f,
    0.9f,
    0.9f,
    0.9f,
    0.9f
  };
  [PublicizedFrom(EAccessModifier.Private)]
  public static float[][] isHiddenOffsets = new float[2][]
  {
    new float[12]
    {
      -0.7f,
      0.3f,
      0.0f,
      0.3f,
      0.7f,
      0.3f,
      -0.7f,
      0.8f,
      0.0f,
      0.8f,
      0.7f,
      0.8f
    },
    new float[12]
    {
      -0.4f,
      0.5f,
      0.0f,
      0.5f,
      0.4f,
      0.5f,
      -0.4f,
      1.5f,
      0.0f,
      1.5f,
      0.4f,
      1.5f
    }
  };
  [PublicizedFrom(EAccessModifier.Private)]
  public const ushort cFlagsQuestExclude = 1;
  [PublicizedFrom(EAccessModifier.Private)]
  public const ushort cFlagsPriority = 2;
  [PublicizedFrom(EAccessModifier.Private)]
  public const ushort cFlagsSpawning = 4;
  [PublicizedFrom(EAccessModifier.Private)]
  public const ushort cFlagsCleared = 8;

  public bool IsTrigger
  {
    [PublicizedFrom(EAccessModifier.Private)] get => this.TriggeredByIndices.Count > 0;
  }

  public bool IsTriggerAndNoRespawn
  {
    [PublicizedFrom(EAccessModifier.Private)] get
    {
      return (this.flags & 7) == 3 && this.respawnMap.Count == 0;
    }
  }

  public static void WorldInit()
  {
    SleeperVolume.sleeperRandom = GameRandomManager.Instance.CreateGameRandom();
  }

  public static SleeperVolume Create(
    Prefab.PrefabSleeperVolume psv,
    Vector3i _boxMin,
    Vector3i _boxMax)
  {
    SleeperVolume sleeperVolume = new SleeperVolume();
    sleeperVolume.SetMinMax(_boxMin, _boxMax);
    sleeperVolume.groupId = psv.groupId;
    sleeperVolume.isQuestExclude = psv.isQuestExclude;
    sleeperVolume.isPriority = psv.isPriority;
    sleeperVolume.spawnCountMin = psv.spawnCountMin;
    sleeperVolume.spawnCountMax = psv.spawnCountMax;
    sleeperVolume.flags = psv.flags;
    sleeperVolume.TriggeredByIndices = new List<byte>((IEnumerable<byte>) psv.triggeredByIndices);
    sleeperVolume.groupName = GameStageGroup.CleanName(psv.groupName);
    sleeperVolume.SetScript(psv.minScript);
    sleeperVolume.AddToPrefabInstance();
    return sleeperVolume;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void SetMinMax(Vector3i _boxMin, Vector3i _boxMax)
  {
    this.BoxMin = _boxMin;
    this.BoxMax = _boxMax;
    this.Center = Vector3.op_Multiply((this.BoxMin + this.BoxMax).ToVector3(), 0.5f);
  }

  public bool Intersects(Bounds bounds)
  {
    return BoundsUtils.Intersects(bounds, (Vector3) this.BoxMin, (Vector3) this.BoxMax);
  }

  public PrefabInstance PrefabInstance => this.prefabInstance;

  public void AddToPrefabInstance()
  {
    this.prefabInstance = GameManager.Instance.World.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator().GetPrefabAtPosition(this.Center);
    if (this.prefabInstance == null)
      return;
    this.prefabInstance.AddSleeperVolume(this);
  }

  public void AddSpawnPoint(int _x, int _y, int _z, BlockSleeper _block, BlockValue _blockValue)
  {
    if (this.spawnPointList.Count >= (int) byte.MaxValue)
      return;
    this.spawnPointList.Add(new SleeperVolume.SpawnPoint(new Vector3i(_x, _y, _z), _block.GetSleeperRotation(_blockValue), _blockValue.type));
  }

  public void SetScript(string _script)
  {
    if (string.IsNullOrEmpty(_script))
    {
      this.minScript = (MinScript) null;
    }
    else
    {
      this.minScript = new MinScript();
      this.minScript.SetText(_script);
    }
  }

  public void Tick(World _world)
  {
    if (this.isSpawning)
    {
      if (this.minScript != null && this.minScript.IsRunning())
      {
        foreach (KeyValuePair<int, SleeperVolume.RespawnData> respawn in this.respawnMap)
        {
          if (!Object.op_Implicit((Object) _world.GetEntity(respawn.Key)))
          {
            this.respawnMap.Clear();
            this.respawnList = (List<int>) null;
            this.minScript.Restart();
            break;
          }
        }
        this.minScript.Tick(this);
      }
      if (SleeperVolume.TickSpawnCount < 2)
        this.UpdateSpawn(_world);
    }
    if (this.isSpawning)
      return;
    if (this.isSpawned)
    {
      if (this.respawnMap.Count == 0)
        this.isSpawned = false;
      foreach (KeyValuePair<int, SleeperVolume.RespawnData> respawn in this.respawnMap)
      {
        if (!Object.op_Implicit((Object) _world.GetEntity(respawn.Key)))
        {
          this.isSpawned = false;
          break;
        }
      }
    }
    if (Object.op_Inequality((Object) this.playerTouchedToUpdate, (Object) null))
    {
      this.UpdatePlayerTouched(_world, this.playerTouchedToUpdate);
      this.playerTouchedToUpdate = (EntityPlayer) null;
    }
    else
    {
      if (--this.ticksUntilDespawn != 0)
        return;
      this.Despawn(_world);
    }
  }

  public int GetPlayerTouchedToUpdateId()
  {
    int touchedToUpdateId = -1;
    if (Object.op_Inequality((Object) this.playerTouchedToUpdate, (Object) null))
      touchedToUpdateId = this.playerTouchedToUpdate.entityId;
    return touchedToUpdateId;
  }

  public int GetPlayerTouchedTriggerId()
  {
    int touchedTriggerId = -1;
    if (Object.op_Inequality((Object) this.playerTouchedTrigger, (Object) null))
      touchedTriggerId = this.playerTouchedTrigger.entityId;
    return touchedTriggerId;
  }

  public void DespawnAndReset(World _world)
  {
    this.Despawn(_world);
    this.Reset();
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void Despawn(World _world)
  {
    this.triggerState = SleeperVolume.ETriggerType.Passive;
    this.playerTouchedTrigger = (EntityPlayer) null;
    int num = 0;
    foreach (KeyValuePair<int, SleeperVolume.RespawnData> respawn in this.respawnMap)
    {
      EntityAlive entity = _world.GetEntity(respawn.Key) as EntityAlive;
      if (Object.op_Implicit((Object) entity) && entity.IsSleeping)
      {
        entity.IsDespawned = true;
        entity.MarkToUnload();
        ++num;
      }
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void Reset()
  {
    this.playerTouchedToUpdate = (EntityPlayer) null;
    this.playerTouchedTrigger = (EntityPlayer) null;
    this.respawnTime = ulong.MaxValue;
    this.isSpawning = false;
    this.isSpawned = false;
    this.wasCleared = false;
    this.numSpawned = 0;
    this.respawnMap.Clear();
    this.respawnList = (List<int>) null;
    if (this.minScript == null)
      return;
    this.minScript.Reset();
  }

  public int GetAliveCount()
  {
    int num = 0;
    for (int index = 0; index < this.groupCountList.Count; ++index)
      num += this.groupCountList[index].count;
    return num - this.numSpawned + this.respawnMap.Count;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void UpdatePlayerTouched(World _world, EntityPlayer _playerTouched)
  {
    if (this.isSpawned || _world.worldTime < this.respawnTime && this.wasCleared)
      return;
    if (_world.worldTime >= this.respawnTime)
      this.Reset();
    this.isSpawning = true;
    this.isSpawned = true;
    float _countScale = 1f;
    if (this.prefabInstance != null)
    {
      float num = this.prefabInstance.LastQuestClass == null ? 1f : this.prefabInstance.LastQuestClass.SpawnMultiplier;
      byte difficultyTier = this.prefabInstance.prefab.DifficultyTier;
      _countScale = num * ((int) difficultyTier < SleeperVolume.difficultyTierScale.Length ? SleeperVolume.difficultyTierScale[(int) difficultyTier] : SleeperVolume.difficultyTierScale[SleeperVolume.difficultyTierScale.Length - 1]);
      if (this.prefabInstance.LastRefreshType.Test_AnySet(QuestEventManager.banditTag))
        _countScale = 0.2f;
    }
    if (this.spawnPointList.Count > 0)
    {
      int num = 0;
      this.gameStage = Mathf.Max(0, this.GetGameStageAround(_playerTouched) + num);
      if (this.respawnMap.Count > 0)
      {
        this.respawnList = new List<int>(this.respawnMap.Count);
        foreach (KeyValuePair<int, SleeperVolume.RespawnData> respawn in this.respawnMap)
          this.respawnList.Add(respawn.Key);
      }
      this.ResetSpawnsAvailable();
      if (this.groupCountList != null)
        this.groupCountList.Clear();
      if (this.spawnCountMin < (short) 0 || this.spawnCountMax < (short) 0)
      {
        this.spawnCountMin = (short) 5;
        this.spawnCountMax = (short) 6;
      }
      this.AddSpawnCount(this.groupName, (float) this.spawnCountMin * _countScale, (float) this.spawnCountMax * _countScale);
      this.spawnDelay = 0;
    }
    if (this.minScript == null)
      return;
    this.minScript.Run(this, _playerTouched, _countScale);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void ResetSpawnsAvailable()
  {
    bool flag = false;
    if (this.prefabInstance != null && this.prefabInstance.LastRefreshType.Test_AnySet(QuestEventManager.infestedTag))
      flag = true;
    this.spawnsAvailable = new List<int>(this.spawnPointList.Count);
    for (int index = 0; index < this.spawnPointList.Count; ++index)
    {
      if (flag || this.spawnPointList[index].GetBlock().spawnMode != BlockSleeper.eMode.Infested)
        this.spawnsAvailable.Add(index);
    }
  }

  public void AddSpawnCount(string _groupName, float _min, float _max)
  {
    if ((double) _max == 0.0)
      return;
    SleeperVolume.GroupCount groupCount;
    groupCount.groupName = _groupName;
    float num1 = SleeperVolume.sleeperRandom.RandomRange(_min, _max);
    int num2 = (int) num1;
    if ((double) SleeperVolume.sleeperRandom.RandomFloat < (double) num1 - (double) num2)
      ++num2;
    if ((double) _min > 0.0 && num2 == 0)
      num2 = 1;
    groupCount.count = num2;
    if (num2 <= 0)
      return;
    if (this.groupCountList == null)
      this.groupCountList = new List<SleeperVolume.GroupCount>();
    this.groupCountList.Add(groupCount);
  }

  public void CheckTouching(World _world, EntityPlayer _player)
  {
    if (this.IsTriggerAndNoRespawn || _player.IsSpectator)
      return;
    Vector3 position = _player.position;
    position.y += 0.8f;
    SleeperVolume.ETriggerType etriggerType = (SleeperVolume.ETriggerType) (this.flags & 7);
    if (this.hasPassives)
    {
      if ((double) position.x >= (double) this.BoxMin.x - -0.30000001192092896 && (double) position.x < (double) this.BoxMax.x - 0.30000001192092896 && (double) position.y >= (double) this.BoxMin.y && (double) position.y < (double) this.BoxMax.y && (double) position.z >= (double) this.BoxMin.z - -0.30000001192092896 && (double) position.z < (double) this.BoxMax.z - 0.30000001192092896 && etriggerType != SleeperVolume.ETriggerType.Passive)
        this.TouchGroup(_world, _player, true);
    }
    else if ((etriggerType == SleeperVolume.ETriggerType.Attack || etriggerType == SleeperVolume.ETriggerType.Trigger) && this.triggerState != etriggerType && (double) position.x >= (double) this.BoxMin.x - -0.10000000149011612 && (double) position.x < (double) this.BoxMax.x - 0.10000000149011612 && (double) position.y >= (double) this.BoxMin.y && (double) position.y < (double) this.BoxMax.y && (double) position.z >= (double) this.BoxMin.z - -0.10000000149011612 && (double) position.z < (double) this.BoxMax.z - 0.10000000149011612)
      this.TouchGroup(_world, _player, true);
    if (!Object.op_Equality((Object) this.playerTouchedToUpdate, (Object) null) || !this.CheckTrigger(_world, position))
      return;
    this.TouchGroup(_world, _player, false);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void TouchGroup(World _world, EntityPlayer _player, bool setActive)
  {
    SleeperVolume.ETriggerType trigger = (SleeperVolume.ETriggerType) (this.flags & 7);
    if (this.groupId == (short) 0 || this.prefabInstance == null)
    {
      this.Touch(_world, _player, setActive, trigger);
    }
    else
    {
      List<SleeperVolume> sleeperVolumes = this.prefabInstance.sleeperVolumes;
      for (int index = 0; index < sleeperVolumes.Count; ++index)
      {
        SleeperVolume sleeperVolume = sleeperVolumes[index];
        if ((int) sleeperVolume.groupId == (int) this.groupId && !sleeperVolume.IsTriggerAndNoRespawn)
          sleeperVolume.Touch(_world, _player, setActive, trigger);
      }
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void Touch(
    World _world,
    EntityPlayer _player,
    bool setActive,
    SleeperVolume.ETriggerType trigger)
  {
    if (setActive)
    {
      bool flag = (trigger == SleeperVolume.ETriggerType.Attack || trigger == SleeperVolume.ETriggerType.Trigger) && Object.op_Implicit((Object) _player);
      foreach (KeyValuePair<int, SleeperVolume.RespawnData> respawn in this.respawnMap)
      {
        int key = respawn.Key;
        EntityAlive entity = (EntityAlive) _world.GetEntity(key);
        if (Object.op_Implicit((Object) entity))
        {
          if (flag && _player.Stealth.CanSleeperAttackDetect(entity))
          {
            entity.ConditionalTriggerSleeperWakeUp();
            entity.SetAttackTarget((EntityAlive) _player, 400);
          }
          else if (trigger == SleeperVolume.ETriggerType.Wander)
            entity.ConditionalTriggerSleeperWakeUp();
          else if (--SleeperVolume.wanderingCountdown <= 0)
          {
            SleeperVolume.wanderingCountdown = 10;
            entity.ConditionalTriggerSleeperWakeUp();
          }
          else
            entity.SetSleeperActive();
        }
      }
      this.hasPassives = false;
      this.triggerState = trigger;
    }
    else
    {
      this.playerTouchedToUpdate = _player;
      this.ticksUntilDespawn = 900;
      if (this.hasPassives)
        this.ticksUntilDespawn = 200;
      if (!this.wasCleared || _world.worldTime >= this.respawnTime)
        return;
      this.respawnTime = Math.Max(this.respawnTime, _world.worldTime + 1000UL);
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public bool CheckTrigger(World _world, Vector3 playerPos)
  {
    if (this.isSpawned)
    {
      Vector3i vector3i1 = this.BoxMin - SleeperVolume.unpadding;
      Vector3i vector3i2 = this.BoxMax + SleeperVolume.unpadding;
      return (double) playerPos.x >= (double) vector3i1.x && (double) playerPos.x < (double) vector3i2.x && (double) playerPos.y >= (double) vector3i1.y && (double) playerPos.y < (double) vector3i2.y && (double) playerPos.z >= (double) vector3i1.z && (double) playerPos.z < (double) vector3i2.z;
    }
    Vector3i vector3i3 = this.BoxMin - SleeperVolume.triggerPaddingMin;
    Vector3i vector3i4 = this.BoxMax + SleeperVolume.triggerPaddingMax;
    if ((double) playerPos.x < (double) vector3i3.x || (double) playerPos.x >= (double) vector3i4.x || (double) playerPos.y < (double) vector3i3.y || (double) playerPos.y >= (double) vector3i4.y || (double) playerPos.z < (double) vector3i3.z || (double) playerPos.z >= (double) vector3i4.z)
      return false;
    if (this.wasCleared)
    {
      if (GameUtils.CheckForAnyPlayerHome(GameManager.Instance.World, this.BoxMin, this.BoxMax) == GameUtils.EPlayerHomeType.None)
        return true;
      this.respawnTime = Math.Max(this.respawnTime, _world.worldTime + 24000UL);
      return false;
    }
    if (this.prefabInstance != null)
      _world.UncullPOI(this.prefabInstance);
    return true;
  }

  public void CheckNoise(World _world, Vector3 pos)
  {
    if (!this.hasPassives || (double) pos.x < (double) this.BoxMin.x - 0.89999997615814209 || (double) pos.x >= (double) this.BoxMax.x + 0.89999997615814209 || (double) pos.y < (double) this.BoxMin.y - 0.89999997615814209 || (double) pos.y >= (double) this.BoxMax.y + 0.89999997615814209 || (double) pos.z < (double) this.BoxMin.z - 0.89999997615814209 || (double) pos.z >= (double) this.BoxMax.z + 0.89999997615814209 || (this.flags & 7) == 1)
      return;
    this.TouchGroup(_world, (EntityPlayer) null, true);
  }

  public void OnTriggered(EntityPlayer _player, World _world, int _triggerIndex)
  {
    this.triggerState = (SleeperVolume.ETriggerType) (this.flags & 7);
    this.playerTouchedTrigger = _player;
    this.UpdatePlayerTouched(_world, _player);
  }

  public void EntityDied(EntityAlive entity)
  {
    if (!this.respawnMap.Remove(entity.entityId))
      return;
    if (this.respawnList != null)
      this.respawnList.Remove(entity.entityId);
    int numSpawned = this.numSpawned;
    int count = this.respawnMap.Count;
    if (this.isSpawning)
      return;
    this.ClearedUpdate(entity.world);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void ClearedUpdate(World _world)
  {
    if (this.wasCleared || this.respawnMap.Count > 0)
      return;
    int num = GamePrefs.GetInt(EnumGamePrefs.LootRespawnDays);
    if (num <= 0)
      num = 30;
    this.respawnTime = _world.worldTime + (ulong) (uint) (num * 24000);
    this.wasCleared = true;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public int GetGameStageAround(EntityPlayer player)
  {
    return GameStageDefinition.CalcGameStageAround(player);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public bool SpawnPointIsHidden(World _world, int _index)
  {
    SleeperVolume.SpawnPoint spawnPoint = this.spawnPointList[_index];
    Vector3 vector3_1 = spawnPoint.pos.ToVector3();
    vector3_1.x += 0.5f;
    vector3_1.z += 0.5f;
    int index1 = 0;
    if (spawnPoint.GetBlock().pose == 5)
      index1 = 1;
    float[] isHiddenOffset = SleeperVolume.isHiddenOffsets[index1];
    for (int index2 = 0; index2 < _world.Players.list.Count; ++index2)
    {
      EntityPlayer entityPlayer = _world.Players.list[index2];
      Vector3 headPosition = entityPlayer.getHeadPosition();
      int modelLayer = entityPlayer.GetModelLayer();
      entityPlayer.SetModelLayer(2);
      Ray ray;
      // ISSUE: explicit constructor call
      ((Ray) ref ray).\u002Ector(headPosition, Vector3.one);
      Vector3 vector3_2 = Vector3.op_Subtraction(vector3_1, headPosition);
      Vector3 vector3_3 = Vector3.Cross(((Vector3) ref vector3_2).normalized, Vector3.up);
      for (int index3 = 0; index3 < isHiddenOffset.Length; index3 += 2)
      {
        Vector3 vector3_4 = Vector3.op_Addition(vector3_1, Vector3.op_Multiply(vector3_3, isHiddenOffset[index3]));
        vector3_4.y += isHiddenOffset[index3 + 1];
        Vector3 vector3_5 = Vector3.op_Subtraction(vector3_4, headPosition);
        ((Ray) ref ray).direction = vector3_5;
        if (!Voxel.Raycast(_world, ray, ((Vector3) ref vector3_5).magnitude, 71, 0.0f))
        {
          entityPlayer.SetModelLayer(modelLayer);
          return false;
        }
      }
      entityPlayer.SetModelLayer(modelLayer);
    }
    return true;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public int FindFathestSpawnFromPlayers(World _world)
  {
    int index1 = -1;
    float num1 = float.MinValue;
    for (int index2 = 0; index2 < this.spawnsAvailable.Count; ++index2)
    {
      Vector3i pos = this.spawnPointList[this.spawnsAvailable[index2]].pos;
      if (_world.CanSleeperSpawnAtPos((Vector3) pos, this.minScript == null))
      {
        Vector3 vector3_1 = pos.ToVector3();
        vector3_1.x += 0.5f;
        vector3_1.z += 0.5f;
        float num2 = float.MaxValue;
        for (int index3 = 0; index3 < _world.Players.list.Count; ++index3)
        {
          Vector3 position = _world.Players.list[index3].position;
          Vector3 vector3_2 = Vector3.op_Subtraction(vector3_1, position);
          float sqrMagnitude = ((Vector3) ref vector3_2).sqrMagnitude;
          if ((double) sqrMagnitude < (double) num2)
            num2 = sqrMagnitude;
        }
        if ((double) num2 > (double) num1)
        {
          num1 = num2;
          index1 = index2;
        }
      }
    }
    if (index1 < 0)
      return -1;
    int spawnFromPlayers = this.spawnsAvailable[index1];
    this.spawnsAvailable.RemoveAt(index1);
    return spawnFromPlayers;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void RemoveSpawnAvailable(int index)
  {
    for (int index1 = 0; index1 < this.spawnsAvailable.Count; ++index1)
    {
      if (this.spawnsAvailable[index1] == index)
      {
        this.spawnsAvailable.RemoveAt(index1);
        break;
      }
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void UpdateSpawn(World _world)
  {
    if (--this.spawnDelay > 0)
      return;
    this.spawnDelay = 2;
    bool flag1 = AIDirector.CanSpawn(2.1f);
    int num1 = GameStats.GetInt(EnumGameStats.EnemyCount);
    bool flag2 = false;
    if (this.minScript != null && this.minScript.IsRunning())
      flag2 = true;
    if (this.spawnsAvailable != null)
    {
      string cultureInvariantString = Time.time.ToCultureInvariantString();
      if (this.respawnList != null && this.respawnList.Count > 0)
      {
        int respawn = this.respawnList[this.respawnList.Count - 1];
        this.respawnList.RemoveAt(this.respawnList.Count - 1);
        Entity entity = _world.GetEntity(respawn);
        if (Object.op_Implicit((Object) entity))
        {
          this.hasPassives = true;
          flag2 = true;
          Log.Out("{0} SleeperVolume {1}: Still alive '{2}'", new object[3]
          {
            (object) cultureInvariantString,
            (object) this.BoxMin,
            (object) ((Object) entity).name
          });
        }
        else
        {
          int num2 = this.respawnMap[respawn].spawnPointIndex;
          if (num2 >= 0)
            this.RemoveSpawnAvailable(num2);
          else
            num2 = this.FindSpawnIndex(_world);
          if (num2 >= 0)
          {
            SleeperVolume.SpawnPoint spawnPoint = this.spawnPointList[num2];
            if (!this.CheckSpawnPos(_world, spawnPoint.pos))
            {
              this.respawnList.Add(respawn);
              this.spawnsAvailable.Add(num2);
              return;
            }
            string className = this.respawnMap[respawn].className;
            Log.Out("{0} SleeperVolume {1}: Restoring {2} ({3}) '{4}', count {5}", new object[6]
            {
              (object) cultureInvariantString,
              (object) this.BoxMin,
              (object) spawnPoint.pos,
              (object) World.toChunkXZ(spawnPoint.pos),
              (object) className,
              (object) num1
            });
            int entityClass = EntityClass.FromString(className);
            BlockSleeper block = spawnPoint.GetBlock();
            if (Object.op_Implicit((Object) this.Spawn(_world, entityClass, num2, block)))
              this.respawnMap.Remove(respawn);
            flag2 = true;
          }
        }
      }
      else if (flag1)
      {
        GameStageDefinition gameStageDefinition = (GameStageDefinition) null;
        if (this.groupCountList != null)
        {
          int num3 = 0;
          for (int index = 0; index < this.groupCountList.Count; ++index)
          {
            num3 += this.groupCountList[index].count;
            if (num3 > this.numSpawned)
            {
              GameStageGroup gameStageGroup = GameStageGroup.TryGet(this.groupCountList[index].groupName);
              if (gameStageGroup == null)
              {
                string str = this.prefabInstance?.name ?? "null";
                Log.Error("{0} SleeperVolume {1} {2}: Spawning group '{3}' missing", new object[4]
                {
                  (object) cultureInvariantString,
                  (object) str,
                  (object) this.BoxMin,
                  (object) this.groupCountList[index].groupName
                });
                gameStageGroup = GameStageGroup.TryGet("GroupGenericZombie");
              }
              gameStageDefinition = gameStageGroup.spawner;
              break;
            }
          }
        }
        if (gameStageDefinition != null)
        {
          GameStageDefinition.Stage stage = gameStageDefinition.GetStage(this.gameStage);
          if (stage != null)
          {
            int spawnIndex = this.FindSpawnIndex(_world);
            if (spawnIndex >= 0)
            {
              SleeperVolume.SpawnPoint spawnPoint = this.spawnPointList[spawnIndex];
              if (!this.CheckSpawnPos(_world, spawnPoint.pos))
              {
                this.spawnsAvailable.Add(spawnIndex);
                return;
              }
              BlockSleeper block = spawnPoint.GetBlock();
              if (block == null)
              {
                Log.Error("{0} BlockSleeper {1} null, type {2}", new object[3]
                {
                  (object) cultureInvariantString,
                  (object) spawnPoint.pos,
                  (object) spawnPoint.blockType
                });
              }
              else
              {
                string _sEntityGroupName = block.spawnGroup;
                if (string.IsNullOrEmpty(_sEntityGroupName))
                  _sEntityGroupName = stage.GetSpawnGroup(0).groupName;
                int randomFromGroup = EntityGroups.GetRandomFromGroup(_sEntityGroupName, ref this.lastClassId, SleeperVolume.sleeperRandom);
                EntityClass entityClass;
                EntityClass.list.TryGetValue(randomFromGroup, out entityClass);
                Log.Out("{0} SleeperVolume {1}: Spawning {2} ({3}), group '{4}', class {5}, count {6}", new object[7]
                {
                  (object) cultureInvariantString,
                  (object) this.BoxMin,
                  (object) spawnPoint.pos,
                  (object) World.toChunkXZ(spawnPoint.pos),
                  (object) _sEntityGroupName,
                  entityClass != null ? (object) entityClass.entityClassName : (object) "?",
                  (object) num1
                });
                if (Object.op_Implicit((Object) this.Spawn(_world, randomFromGroup, spawnIndex, block)))
                  ++this.numSpawned;
                flag2 = true;
              }
            }
          }
        }
      }
    }
    if (flag2)
      return;
    this.isSpawning = false;
    this.respawnList = (List<int>) null;
    if (this.numSpawned == 0)
    {
      if (this.respawnMap.Count == 0)
        this.wasCleared = true;
      Log.Out("{0} SleeperVolume {1}: None spawned, canSpawn {2}, respawnCnt {3}", new object[4]
      {
        (object) Time.time.ToCultureInvariantString(),
        (object) this.BoxMin,
        (object) flag1,
        (object) this.respawnMap.Count
      });
    }
    else
      this.ClearedUpdate(_world);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public int FindSpawnIndex(World _world)
  {
    if (this.spawnsAvailable.Count == 0)
      this.ResetSpawnsAvailable();
    int index = SleeperVolume.sleeperRandom.RandomRange(0, this.spawnsAvailable.Count);
    for (int count = this.spawnsAvailable.Count; count > 0; --count)
    {
      int spawnIndex = this.spawnsAvailable[index];
      Vector3i pos = this.spawnPointList[spawnIndex].pos;
      if (_world.CanSleeperSpawnAtPos((Vector3) pos, true) && this.SpawnPointIsHidden(_world, spawnIndex))
      {
        this.spawnsAvailable.RemoveAt(index);
        return spawnIndex;
      }
      if (++index >= this.spawnsAvailable.Count)
        index = 0;
    }
    return this.FindFathestSpawnFromPlayers(_world);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public bool CheckSpawnPos(World _world, Vector3i pos)
  {
    if (GameManager.bRecordNextSession || GameManager.bPlayRecordedSession)
      return true;
    Chunk chunkFromWorldPos = (Chunk) _world.GetChunkFromWorldPos(pos);
    return chunkFromWorldPos != null && !chunkFromWorldPos.IsInternalBlocksCulled && !chunkFromWorldPos.NeedsCopying && !chunkFromWorldPos.NeedsRegeneration;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public EntityAlive Spawn(World _world, int entityClass, int spawnIndex, BlockSleeper block)
  {
    SleeperVolume.SpawnPoint spawnPoint = this.spawnPointList[spawnIndex];
    Vector3 vector3 = spawnPoint.pos.ToVector3();
    vector3.x += 0.502f;
    vector3.z += 0.501f;
    EntityClass _entityClass;
    if (!EntityClass.list.TryGetValue(entityClass, out _entityClass))
    {
      Log.Warning("Spawn class {0} is missing", new object[1]
      {
        (object) entityClass
      });
      entityClass = EntityClass.FromString("zombieArlene");
    }
    else if (block != null && block.ExcludesWalkType(EntityAlive.GetSpawnWalkType(_entityClass)))
    {
      Log.Warning("Spawn {0} can't walk on block {1} with walkType {2}", new object[3]
      {
        (object) _entityClass.entityClassName,
        (object) block,
        (object) EntityAlive.GetSpawnWalkType(_entityClass)
      });
      return (EntityAlive) null;
    }
    EntityAlive entity = (EntityAlive) EntityFactory.CreateEntity(entityClass, vector3, new Vector3(0.0f, spawnPoint.rot, 0.0f));
    if (!Object.op_Implicit((Object) entity))
    {
      Log.Error("Spawn class {0} is null", new object[1]
      {
        (object) entityClass
      });
      return (EntityAlive) null;
    }
    ++SleeperVolume.TickSpawnCount;
    entity.SetSpawnerSource(EnumSpawnerSource.Dynamic);
    entity.IsSleeperPassive = true;
    entity.SleeperSpawnPosition = vector3;
    entity.SleeperSpawnLookDir = block.look;
    if (_world.GetTileEntity(0, spawnPoint.pos) is TileEntitySleeper tileEntity)
    {
      entity.SetSleeperSight((float) tileEntity.GetSightAngle(), (float) tileEntity.GetSightRange());
      entity.SetSleeperHearing(tileEntity.GetHearingPercent());
    }
    entity.SetSleeper();
    entity.TriggerSleeperPose(block.pose);
    _world.SpawnEntityInWorld((Entity) entity);
    SleeperVolume.RespawnData respawnData;
    respawnData.className = EntityClass.list[entityClass].entityClassName;
    respawnData.spawnPointIndex = spawnIndex;
    this.respawnMap.Add(entity.entityId, respawnData);
    this.hasPassives = true;
    this.SpawnParticle("sleeperSpawn", entity);
    if (Object.op_Implicit((Object) this.playerTouchedTrigger))
      GameManager.Instance.StartCoroutine(this.WakeAttackLater(entity, this.playerTouchedTrigger));
    return entity;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void SpawnParticle(string _particleName, EntityAlive _zombie)
  {
    World world = _zombie.world;
    Vector3 position = _zombie.position;
    position.y += 0.5f;
    Vector3i blockPos = World.worldToBlockPos(position);
    ++blockPos.y;
    if (world.GetBlock(blockPos).isair)
      return;
    --blockPos.y;
    float lightBrightness = world.GetLightBrightness(blockPos);
    ParticleEffect _pe = new ParticleEffect(_particleName, position, lightBrightness, Color.white, (string) null, (Transform) null, false);
    world.GetGameManager().SpawnParticleEffectServer(_pe, _zombie.entityId);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public IEnumerator WakeAttackLater(EntityAlive _ea, EntityPlayer _playerTouched)
  {
    yield return (object) new WaitForSeconds(1f);
    if (Object.op_Implicit((Object) _ea) && Object.op_Implicit((Object) _playerTouched))
    {
      _ea.ConditionalTriggerSleeperWakeUp();
      _ea.SetAttackTarget((EntityAlive) _playerTouched, 400);
    }
  }

  public List<SleeperVolume.SpawnPoint> GetSpawnPoints() => this.spawnPointList;

  public static SleeperVolume Read(BinaryReader _br)
  {
    SleeperVolume sleeperVolume = new SleeperVolume();
    int _version = (int) _br.ReadByte();
    string str = GameStageGroup.CleanName(_br.ReadString());
    if (_version >= 13)
    {
      if (_version >= 16 /*0x10*/)
        sleeperVolume.groupId = _br.ReadInt16();
      sleeperVolume.spawnCountMin = _br.ReadInt16();
      sleeperVolume.spawnCountMax = _br.ReadInt16();
    }
    sleeperVolume.groupName = str;
    sleeperVolume.SetMinMax(new Vector3i(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32()), new Vector3i(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32()));
    sleeperVolume.respawnTime = _br.ReadUInt64();
    if (_version <= 13)
    {
      long num1 = (long) _br.ReadUInt64();
    }
    sleeperVolume.numSpawned = _br.ReadInt32();
    if (_version > 7)
      _br.ReadInt32();
    sleeperVolume.gameStage = _br.ReadInt32();
    if (_version > 3)
    {
      if (_version > 4)
      {
        if (_version < 11)
          _br.ReadString();
      }
      else
        _br.ReadInt32();
    }
    if (_version >= 10)
    {
      _br.ReadString();
      _br.ReadInt32();
    }
    if (_version > 5)
      sleeperVolume.ticksUntilDespawn = _br.ReadInt32();
    if (_version >= 14)
    {
      ushort num2 = _br.ReadUInt16();
      sleeperVolume.isQuestExclude = ((int) num2 & 1) > 0;
      sleeperVolume.isPriority = ((int) num2 & 2) > 0;
      sleeperVolume.isSpawning = ((int) num2 & 4) > 0;
      sleeperVolume.wasCleared = ((int) num2 & 8) > 0;
      if (_version >= 18)
        sleeperVolume.flags = _br.ReadInt32();
    }
    else
    {
      sleeperVolume.isSpawning = _br.ReadBoolean();
      sleeperVolume.wasCleared = _br.ReadBoolean();
      if (_version >= 12)
        sleeperVolume.isQuestExclude = _br.ReadBoolean();
    }
    int num3 = (int) _br.ReadByte();
    if (num3 > 0)
    {
      for (int index = 0; index < num3; ++index)
        sleeperVolume.spawnPointList.Add(SleeperVolume.SpawnPoint.Read(_br, _version));
    }
    if (_version > 1)
    {
      int capacity = (int) _br.ReadByte();
      if (capacity > 0)
      {
        sleeperVolume.spawnsAvailable = new List<int>(capacity);
        for (int index = 0; index < capacity; ++index)
          sleeperVolume.spawnsAvailable.Add((int) _br.ReadByte());
      }
    }
    int num4 = (int) _br.ReadByte();
    if (num4 > 0)
    {
      for (int index = 0; index < num4; ++index)
        _br.ReadInt32();
      sleeperVolume.hasPassives = true;
    }
    if (_version >= 8)
    {
      int num5 = (int) _br.ReadByte();
      if (num5 > 0)
      {
        for (int index = 0; index < num5; ++index)
        {
          int key = _br.ReadInt32();
          SleeperVolume.RespawnData respawnData;
          respawnData.className = _br.ReadString();
          respawnData.spawnPointIndex = _version >= 17 ? (int) _br.ReadByte() : -1;
          sleeperVolume.respawnMap.Add(key, respawnData);
        }
      }
    }
    int capacity1 = (int) _br.ReadByte();
    if (capacity1 > 0)
    {
      sleeperVolume.groupCountList = new List<SleeperVolume.GroupCount>(capacity1);
      for (int index = 0; index < capacity1; ++index)
      {
        SleeperVolume.GroupCount groupCount;
        groupCount.groupName = str;
        if (_version >= 21)
          groupCount.groupName = _br.ReadString();
        groupCount.count = _br.ReadInt32();
        sleeperVolume.groupCountList.Add(groupCount);
      }
    }
    if (_version >= 19)
    {
      int num6 = (int) _br.ReadByte();
      sleeperVolume.TriggeredByIndices.Clear();
      if (num6 > 0)
      {
        for (int index = 0; index < num6; ++index)
          sleeperVolume.TriggeredByIndices.Add(_br.ReadByte());
      }
    }
    if ((sleeperVolume.flags & 16 /*0x10*/) > 0)
      sleeperVolume.minScript = MinScript.Read(_br);
    return sleeperVolume;
  }

  public void Write(BinaryWriter _bw)
  {
    _bw.Write((byte) 21);
    _bw.Write(this.groupName ?? string.Empty);
    _bw.Write(this.groupId);
    _bw.Write(this.spawnCountMin);
    _bw.Write(this.spawnCountMax);
    _bw.Write(this.BoxMin.x);
    _bw.Write(this.BoxMin.y);
    _bw.Write(this.BoxMin.z);
    _bw.Write(this.BoxMax.x);
    _bw.Write(this.BoxMax.y);
    _bw.Write(this.BoxMax.z);
    _bw.Write(this.respawnTime);
    _bw.Write(this.numSpawned);
    _bw.Write(0);
    _bw.Write(this.gameStage);
    _bw.Write(string.Empty);
    _bw.Write(0);
    _bw.Write(this.ticksUntilDespawn);
    ushort num = 0;
    if (this.isQuestExclude)
      num |= (ushort) 1;
    if (this.isPriority)
      num |= (ushort) 2;
    if (this.isSpawning)
      num |= (ushort) 4;
    if (this.wasCleared)
      num |= (ushort) 8;
    _bw.Write(num);
    this.flags &= -17;
    if (this.minScript != null && this.minScript.HasData())
      this.flags |= 16 /*0x10*/;
    _bw.Write(this.flags);
    int count1 = this.spawnPointList.Count;
    _bw.Write((byte) count1);
    for (int index = 0; index < count1; ++index)
      this.spawnPointList[index].Write(_bw);
    int count2 = this.spawnsAvailable != null ? this.spawnsAvailable.Count : 0;
    _bw.Write((byte) count2);
    for (int index = 0; index < count2; ++index)
      _bw.Write((byte) this.spawnsAvailable[index]);
    _bw.Write((byte) 0);
    _bw.Write(this.respawnMap != null ? (byte) this.respawnMap.Count : (byte) 0);
    if (this.respawnMap != null)
    {
      foreach (KeyValuePair<int, SleeperVolume.RespawnData> respawn in this.respawnMap)
      {
        _bw.Write(respawn.Key);
        _bw.Write(respawn.Value.className);
        _bw.Write((byte) respawn.Value.spawnPointIndex);
      }
    }
    _bw.Write(this.groupCountList != null ? (byte) this.groupCountList.Count : (byte) 0);
    if (this.groupCountList != null)
    {
      for (int index = 0; index < this.groupCountList.Count; ++index)
      {
        _bw.Write(this.groupCountList[index].groupName);
        _bw.Write(this.groupCountList[index].count);
      }
    }
    _bw.Write((byte) this.TriggeredByIndices.Count);
    for (int index = 0; index < this.TriggeredByIndices.Count; ++index)
      _bw.Write(this.TriggeredByIndices[index]);
    if ((this.flags & 16 /*0x10*/) <= 0)
      return;
    this.minScript.Write(_bw);
  }

  public override string ToString()
  {
    return $"{this.BoxMin} {(this.groupCountList == null || this.groupCountList.Count <= 0 ? "" : this.groupCountList[0].groupName)} G{this.groupId} Trig{(this.IsTrigger ? 1 : 0)} RespawnC{this.respawnMap.Count}";
  }

  [Conditional("DEBUG_SLEEPERLOG")]
  [PublicizedFrom(EAccessModifier.Private)]
  public static void LogSleeper(string format, params object[] args)
  {
    format = $"{GameManager.frameTime.ToCultureInvariantString()} {GameManager.frameCount} SleeperVolume {format}";
    Log.Warning(format, args);
  }

  public void Draw(float _duration)
  {
    Vector3 minPos = Vector3.op_Subtraction(this.BoxMin.ToVector3(), Origin.position);
    Vector3 vector3 = Vector3.op_Subtraction(this.BoxMax.ToVector3(), Origin.position);
    Color color1 = this.GetColor();
    Vector3 maxPos = vector3;
    Color color2 = color1;
    double duration = (double) _duration;
    Utils.DrawBoxLines(minPos, maxPos, color2, (float) duration);
  }

  public void DrawDebugLines(float _duration)
  {
    string _name = $"SleeperVolume{this.BoxMin},{this.BoxMax}";
    Color color = this.GetColor();
    Vector3 vector3_1 = this.BoxMin.ToVector3();
    Vector3 vector3_2 = this.BoxMax.ToVector3();
    Vector3 _cornerPos1 = Vector3.op_Addition(vector3_1, DebugLines.InsideOffsetV);
    Vector3 _cornerPos2 = Vector3.op_Subtraction(vector3_2, DebugLines.InsideOffsetV);
    Transform rootTransform = GameManager.Instance.World.GetPrimaryPlayer().RootTransform;
    Color _color1 = color;
    Color _color2 = color;
    double _duration1 = (double) _duration;
    DebugLines.Create(_name, rootTransform, _color1, _color2, 0.05f, 0.05f, (float) _duration1).AddCube(_cornerPos1, _cornerPos2);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public Color GetColor()
  {
    Color color = this.isQuestExclude ? Color.red : Color.green;
    if (this.respawnMap.Count > 0)
      color.b = 1f;
    if (this.IsTrigger)
    {
      // ISSUE: explicit constructor call
      ((Color) ref color).\u002Ector(0.25f, 0.25f, 0.25f);
    }
    if (this.wasCleared)
    {
      color.r *= 0.4f;
      color.g *= 0.4f;
      color.b *= 0.4f;
      color.a = 0.16f;
    }
    return color;
  }

  public string GetDescription()
  {
    long duration = (long) this.respawnTime - (long) GameManager.Instance.World.worldTime;
    if (duration < 0L)
      duration = 0L;
    int num1 = -1;
    int num2 = 0;
    if (this.groupCountList != null)
    {
      num1 = this.groupCountList.Count;
      for (int index = 0; index > this.groupCountList.Count; ++index)
        num2 += this.groupCountList[index].count;
    }
    return $"{this.BoxMin}, grpId {this.groupId}, {(Enum) (SleeperVolume.ETriggerType) (this.flags & 7)} ({this.triggerState}), cntList {num1}/{num2}, respawnCnt {this.respawnMap.Count}, spawned {this.numSpawned}, clear{this.wasCleared}, plHome {GameUtils.CheckForAnyPlayerHome(GameManager.Instance.World, this.BoxMin, this.BoxMax)}, respawnIn {this.DurationToString(duration)}, {(this.prefabInstance != null ? (object) $"{this.prefabInstance.name}, volumes {this.prefabInstance.sleeperVolumes.Count.ToString()}" : (object) "?")}";
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public string DurationToString(long duration)
  {
    string str = "";
    int num1 = (int) ((double) duration / 1000.0 / 24.0);
    if (num1 > 0)
      str += num1.ToString("0:");
    int num2 = (int) ((double) duration / 1000.0) % 24;
    if (num1 > 0 || num2 > 0)
      str += num2.ToString("00:");
    int num3 = (int) ((double) duration / 1000.0 * 60.0) % 60;
    if (num1 > 0 || num2 > 0 || num3 > 0)
      str += num3.ToString("00:");
    int num4 = (int) ((double) duration / 1000.0 * 60.0 * 60.0) % 60;
    return str + num4.ToString("00");
  }

  [PublicizedFrom(EAccessModifier.Private)]
  static SleeperVolume()
  {
  }

  public enum ETriggerType
  {
    Active,
    Passive,
    Attack,
    Trigger,
    Wander,
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public struct GroupCount
  {
    public string groupName;
    public int count;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public struct RespawnData
  {
    public string className;
    public int spawnPointIndex;
  }

  public struct SpawnPoint(Vector3i _pos, float _rot, int _blockType)
  {
    public readonly Vector3i pos = _pos;
    public readonly float rot = _rot;
    public readonly int blockType = _blockType;

    public BlockSleeper GetBlock()
    {
      if (!(Block.list[this.blockType] is BlockSleeper blockByName))
        blockByName = (BlockSleeper) Block.GetBlockByName("sleeperSit");
      return blockByName;
    }

    public static SleeperVolume.SpawnPoint Read(BinaryReader _br, int _version)
    {
      Vector3i _pos = new Vector3i(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32());
      if (_version >= 7 && _version < 20)
      {
        double num1 = (double) _br.ReadSingle();
        double num2 = (double) _br.ReadSingle();
        double num3 = (double) _br.ReadSingle();
      }
      float _rot = _br.ReadSingle();
      if (_version < 20)
      {
        int num = (int) _br.ReadByte();
      }
      int _blockType = 0;
      if (_version > 14)
      {
        string _blockname = _br.ReadString();
        Block blockByName = Block.GetBlockByName(_blockname);
        if (blockByName != null)
          _blockType = blockByName.blockID;
        else
          Log.Warning("SpawnPoint Read missing block {0}", new object[1]
          {
            (object) _blockname
          });
      }
      else if (_version >= 9)
        _blockType = (int) _br.ReadUInt16();
      return new SleeperVolume.SpawnPoint(_pos, _rot, _blockType);
    }

    public void Write(BinaryWriter _bw)
    {
      _bw.Write(this.pos.x);
      _bw.Write(this.pos.y);
      _bw.Write(this.pos.z);
      _bw.Write(this.rot);
      _bw.Write(this.GetBlock().GetBlockName());
    }
  }
}
