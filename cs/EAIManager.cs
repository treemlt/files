// Decompiled with JetBrains decompiler
// Type: EAIManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using GamePath;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EAIManager
{
  public const float cInterestDistanceMax = 10f;
  public float interestDistance;
  public float lookTime;
  public const float cSenseScaleMax = 1.6f;
  public float feralSense;
  public float groupCircle;
  public float noiseSeekDist;
  public float pathCostScale;
  public float partialPathHeightScale;
  public float seeOffset;
  [PublicizedFrom(EAccessModifier.Private)]
  public EntityAlive entity;
  public GameRandom random;
  [PublicizedFrom(EAccessModifier.Private)]
  public EAITaskList tasks;
  [PublicizedFrom(EAccessModifier.Private)]
  public EAITaskList targetTasks;
  [PublicizedFrom(EAccessModifier.Private)]
  public List<Entity> allies = new List<Entity>();
  public static bool isAnimFreeze;

  public EAIManager(EntityAlive _entity)
  {
    this.entity = _entity;
    this.random = _entity.world.aiDirector.random;
    this.entity.rand = this.random;
    this.tasks = new EAITaskList(this);
    this.targetTasks = new EAITaskList(this);
    this.interestDistance = 10f;
  }

  public void CopyPropertiesFromEntityClass(EntityClass ec)
  {
    ec.Properties.ParseFloat(EntityClass.PropAIFeralSense, ref this.feralSense);
    ec.Properties.ParseFloat(EntityClass.PropAIGroupCircle, ref this.groupCircle);
    ec.Properties.ParseFloat(EntityClass.PropAINoiseSeekDist, ref this.noiseSeekDist);
    ec.Properties.ParseFloat(EntityClass.PropAISeeOffset, ref this.seeOffset);
    Vector2 optionalValue;
    // ISSUE: explicit constructor call
    ((Vector2) ref optionalValue).\u002Ector(1f, 1f);
    ec.Properties.ParseVec(EntityClass.PropAIPathCostScale, ref optionalValue);
    this.pathCostScale = this.random.RandomRange(optionalValue.x, optionalValue.y);
    this.partialPathHeightScale = 1f - this.pathCostScale;
    string _str1 = ec.Properties.GetString("AITask");
    if (_str1.Length > 0)
    {
      this.ParseTasks(_str1, this.tasks);
    }
    else
    {
      int _priority = 1;
      string _className;
      while (true)
      {
        string str = EntityClass.PropAITask + _priority.ToString();
        if (ec.Properties.Values.TryGetValue(str, out _className) && _className.Length != 0)
        {
          EAIBase instance = EAIManager.CreateInstance(_className);
          if (instance != null)
          {
            instance.Init(this.entity);
            DictionarySave<string, string> keyData = ec.Properties.ParseKeyData(str);
            if (keyData != null)
            {
              try
              {
                instance.SetData(keyData);
              }
              catch (Exception ex)
              {
                Log.Error("EAIManager {0} SetData error {1}", new object[2]
                {
                  (object) _className,
                  (object) ex
                });
              }
            }
            this.tasks.AddTask(_priority, instance);
            ++_priority;
          }
          else
            break;
        }
        else
          goto label_10;
      }
      throw new Exception($"Class '{_className}' not found!");
    }
label_10:
    string _str2 = ec.Properties.GetString("AITarget");
    if (_str2.Length > 0)
    {
      this.ParseTasks(_str2, this.targetTasks);
    }
    else
    {
      int _priority = 1;
      string _className;
      while (true)
      {
        string str = EntityClass.PropAITargetTask + _priority.ToString();
        if (ec.Properties.Values.TryGetValue(str, out _className) && _className.Length != 0)
        {
          EAIBase instance = EAIManager.CreateInstance(_className);
          if (instance != null)
          {
            instance.Init(this.entity);
            DictionarySave<string, string> keyData = ec.Properties.ParseKeyData(str);
            if (keyData != null)
            {
              try
              {
                instance.SetData(keyData);
              }
              catch (Exception ex)
              {
                Log.Error("EAIManager {0} SetData error {1}", new object[2]
                {
                  (object) _className,
                  (object) ex
                });
              }
            }
            this.targetTasks.AddTask(_priority, instance);
            ++_priority;
          }
          else
            goto label_15;
        }
        else
          break;
      }
      return;
label_15:
      throw new Exception($"Class '{_className}' not found!");
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void ParseTasks(string _str, EAITaskList _list)
  {
    int _priority = 1;
    for (int index = 0; index < _str.Length; ++index)
    {
      if (char.IsLetter(_str[index]))
      {
        int num = _str.IndexOf('|', index + 1);
        if (num < 0)
          num = _str.Length;
        string str = _str.Substring(index, num - index);
        string _className = str;
        string data1 = (string) null;
        int length = str.IndexOf(' ');
        if (length >= 0)
        {
          _className = str.Substring(0, length);
          data1 = str.Substring(length + 1);
        }
        EAIBase instance = EAIManager.CreateInstance(_className);
        if (instance == null)
          throw new Exception($"Class '{_className}' not found!");
        instance.Init(this.entity);
        if (data1 != null)
        {
          DictionarySave<string, string> data2 = DynamicProperties.ParseData(data1);
          if (data2 != null)
          {
            try
            {
              instance.SetData(data2);
            }
            catch (Exception ex)
            {
              Log.Error("EAIManager {0} SetData error {1}", new object[2]
              {
                (object) _className,
                (object) ex
              });
            }
          }
        }
        _list.AddTask(_priority, instance);
        ++_priority;
        index = num;
      }
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static EAIBase CreateInstance(string _className)
  {
    return (EAIBase) Activator.CreateInstance(EAIManager.GetType(_className));
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static System.Type GetType(string _className)
  {
    switch (_className)
    {
      case "ApproachAndAttackTarget":
        return typeof (EAIApproachAndAttackTarget);
      case "ApproachDistraction":
        return typeof (EAIApproachDistraction);
      case "ApproachSpot":
        return typeof (EAIApproachSpot);
      case "BlockIf":
        return typeof (EAIBlockIf);
      case "BlockingTargetTask":
        return typeof (EAIBlockingTargetTask);
      case "BreakBlock":
        return typeof (EAIBreakBlock);
      case "DestroyArea":
        return typeof (EAIDestroyArea);
      case "Dodge":
        return typeof (EAIDodge);
      case "Leap":
        return typeof (EAILeap);
      case "Look":
        return typeof (EAILook);
      case "RangedAttackTarget":
        return typeof (EAIRangedAttackTarget);
      case "RunawayFromEntity":
        return typeof (EAIRunawayFromEntity);
      case "RunawayWhenHurt":
        return typeof (EAIRunawayWhenHurt);
      case "SetAsTargetIfHurt":
        return typeof (EAISetAsTargetIfHurt);
      case "SetNearestCorpseAsTarget":
        return typeof (EAISetNearestCorpseAsTarget);
      case "SetNearestEntityAsTarget":
        return typeof (EAISetNearestEntityAsTarget);
      case "TakeCover":
        return typeof (EAITakeCover);
      case "Territorial":
        return typeof (EAITerritorial);
      case "Wander":
        return typeof (EAIWander);
      default:
        Log.Warning("EAIManager GetType slow lookup for {0}", new object[1]
        {
          (object) _className
        });
        return System.Type.GetType("EAI" + _className);
    }
  }

  public void Update()
  {
    this.interestDistance = Mathf.MoveTowards(this.interestDistance, 10f, 0.004166667f);
    this.targetTasks.OnUpdateTasks();
    this.tasks.OnUpdateTasks();
    this.UpdateDebugName();
  }

  public void UpdateDebugName()
  {
    if (!GamePrefs.GetBool(EnumGamePrefs.DebugMenuShowTasks))
      return;
    this.entity.DebugNameInfo = this.MakeDebugName((EntityPlayer) GameManager.Instance.World.GetPrimaryPlayer());
  }

  public string MakeDebugName(EntityPlayer player)
  {
    EntityMoveHelper moveHelper = this.entity.moveHelper;
    string empty = string.Empty;
    if (this.entity.IsSleeper)
      empty += $"\nSleeper {(this.entity.IsSleeping ? (object) "Sleep " : (object) "")}{(this.entity.IsSleeperPassive ? (object) "Passive" : (object) "")}";
    string str1 = empty + $"\nHealth {this.entity.Health} / {this.entity.GetMaxHealth()}, PCost {this.pathCostScale.ToCultureInvariantString(".00")}, InterestD {this.interestDistance.ToCultureInvariantString("0.000")}";
    string str2 = $"\n{(this.entity.IsAlert ? (object) $"Alert {((float) this.entity.GetAlertTicks() / 20f).ToCultureInvariantString("0.00")}, " : (object) "")}{(this.entity.HasInvestigatePosition ? (object) $"Investigate {((float) this.entity.GetInvestigatePositionTicks() / 20f).ToCultureInvariantString("0.00")}, " : (object) "")}";
    if (str2.Length > 1)
      str1 += str2;
    string str3 = $"\n{(moveHelper.IsActive ? (object) $"Move {this.entity.GetMoveSpeedAggro().ToCultureInvariantString(".00")} {this.entity.GetSpeedModifier().ToCultureInvariantString(".00")}," : (object) "")}{(moveHelper.BlockedFlags > 0 ? (object) $"Blocked {moveHelper.BlockedFlags}, {moveHelper.BlockedTime.ToCultureInvariantString("0.00")}" : (object) "")}{(moveHelper.CanBreakBlocks ? (object) "CanBrk, " : (object) "")}{(moveHelper.IsUnreachableAbove ? (object) "UnreachAbove, " : (object) "")}{(moveHelper.IsUnreachableSide ? (object) "UnreachSide, " : (object) "")}{(moveHelper.IsUnreachableSideJump ? (object) "UnreachSideJump" : (object) "")}";
    if (str3.Length > 1)
      str1 += str3;
    if (this.entity.bodyDamage.CurrentStun != EnumEntityStunType.None)
      str1 += $"\nStun {this.entity.bodyDamage.CurrentStun.ToStringCached<EnumEntityStunType>()}, {this.entity.bodyDamage.StunDuration.ToCultureInvariantString("0.00")}";
    if (Object.op_Implicit((Object) this.entity.emodel) && this.entity.emodel.IsRagdollActive)
      str1 = $"{str1}\nRagdoll {this.entity.emodel.GetRagdollDebugInfo()}";
    for (int index = 0; index < this.tasks.GetExecutingTasks().Count; ++index)
    {
      EAITaskEntry executingTask = this.tasks.GetExecutingTasks()[index];
      str1 = $"{str1}\n1 {executingTask.action.ToString()}";
    }
    for (int index = 0; index < this.targetTasks.GetExecutingTasks().Count; ++index)
    {
      EAITaskEntry executingTask = this.targetTasks.GetExecutingTasks()[index];
      str1 = $"{str1}\n2 {executingTask.action.ToString()}";
    }
    string str4;
    if (this.entity.IsSleeping)
    {
      float wake;
      float groan;
      this.entity.GetSleeperDebugScale(this.entity.GetDistance((Entity) player), out wake, out groan);
      string str5 = $"\nLight {player.Stealth.lightLevel.ToCultureInvariantString():0} groan{groan.ToCultureInvariantString():0} wake{wake.ToCultureInvariantString():0}, Noise {this.entity.noisePlayerVolume.ToCultureInvariantString():0} groan{this.entity.sleeperNoiseToSense.ToCultureInvariantString():0} wake{this.entity.sleeperNoiseToWake.ToCultureInvariantString():0}";
      str4 = str1 + str5;
    }
    else
    {
      float stealthDebugScale = this.entity.GetSeeStealthDebugScale(this.GetSeeDistance((Entity) player));
      string str6 = $"\nLight {player.Stealth.lightLevel.ToCultureInvariantString():0} sight {stealthDebugScale.ToCultureInvariantString():0}, noise {this.entity.noisePlayerVolume.ToCultureInvariantString():0} dist {this.entity.noisePlayerDistance.ToCultureInvariantString():0}";
      str4 = str1 + str6;
    }
    return str4 + this.entity.MakeDebugNameInfo();
  }

  public bool CheckPath(PathInfo pathInfo)
  {
    List<EAITaskEntry> executingTasks = this.tasks.GetExecutingTasks();
    for (int index = 0; index < executingTasks.Count; ++index)
    {
      if (executingTasks[index].action.IsPathUsageBlocked(pathInfo.path))
        return false;
    }
    return true;
  }

  public void DamagedByEntity()
  {
    EntityMoveHelper moveHelper = this.entity.moveHelper;
    if (moveHelper != null)
      moveHelper.IsDestroyAreaTryUnreachable = false;
    this.tasks.GetTask<EAIDestroyArea>()?.Stop();
  }

  public void SleeperWokeUp()
  {
    for (int index = 0; index < this.targetTasks.Tasks.Count; ++index)
      this.targetTasks.Tasks[index].executeTime = 0.0f;
  }

  public void FallHitGround(float distance)
  {
    if ((double) distance >= 0.800000011920929)
      this.entity.ConditionalTriggerSleeperWakeUp();
    if ((double) distance < 2.5)
      return;
    EntityMoveHelper moveHelper = this.entity.moveHelper;
    if (!moveHelper.IsActive || !moveHelper.IsUnreachableSide && !moveHelper.IsMoveToAbove())
      return;
    this.ClearTaskDelay<EAIDestroyArea>(this.tasks);
    moveHelper.UnreachablePercent += 0.3f;
    moveHelper.IsDestroyAreaTryUnreachable = true;
    Bounds _bb;
    // ISSUE: explicit constructor call
    ((Bounds) ref _bb).\u002Ector(this.entity.position, new Vector3(20f, 10f, 20f));
    this.entity.world.GetEntitiesInBounds(typeof (EntityHuman), _bb, this.allies);
    if (this.allies.Count >= 3)
    {
      for (int index = 0; index < 2; ++index)
      {
        EntityHuman ally = (EntityHuman) this.allies[this.entity.rand.RandomRange(this.allies.Count)];
        ally.moveHelper.UnreachablePercent += 0.12f;
        ally.moveHelper.IsDestroyAreaTryUnreachable = true;
      }
    }
    this.allies.Clear();
  }

  public float GetSeeDistance(Entity _seeEntity)
  {
    return this.entity.GetDistance(_seeEntity) - this.seeOffset;
  }

  public static float CalcSenseScale()
  {
    switch (GamePrefs.GetInt(EnumGamePrefs.ZombieFeralSense))
    {
      case 1:
        if (GameManager.Instance.World.IsDaytime())
          return 1f;
        break;
      case 2:
        if (GameManager.Instance.World.IsDark())
          return 1f;
        break;
      case 3:
        return 1f;
    }
    return 0.0f;
  }

  public void SetTargetOnlyPlayers(float _distance)
  {
    List<EAITaskEntry> tasks1 = this.tasks.Tasks;
    for (int index = 0; index < tasks1.Count; ++index)
    {
      if (tasks1[index].action is EAIApproachAndAttackTarget action)
        action.SetTargetOnlyPlayers();
    }
    List<EAITaskEntry> tasks2 = this.targetTasks.Tasks;
    for (int index = 0; index < tasks2.Count; ++index)
    {
      if (tasks2[index].action is EAISetNearestEntityAsTarget action)
        action.SetTargetOnlyPlayers(_distance);
    }
  }

  public List<T> GetTasks<T>() where T : class => this.getTaskTypes<T>(this.tasks);

  public List<T> GetTargetTasks<T>() where T : class => this.getTaskTypes<T>(this.targetTasks);

  [PublicizedFrom(EAccessModifier.Private)]
  public List<T> getTaskTypes<T>(EAITaskList taskList) where T : class
  {
    List<T> objList = new List<T>();
    for (int index = 0; index < taskList.Tasks.Count; ++index)
    {
      EAITaskEntry task = taskList.Tasks[index];
      if (task.action is T)
        objList.Add(task.action as T);
    }
    return objList.Count > 0 ? objList : (List<T>) null;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void ClearTaskDelay<T>(EAITaskList taskList) where T : class
  {
    for (int index = 0; index < taskList.Tasks.Count; ++index)
    {
      EAITaskEntry task = taskList.Tasks[index];
      if (task.action is T)
        task.executeTime = 0.0f;
    }
  }

  public static void ToggleAnimFreeze()
  {
    World world = GameManager.Instance.World;
    if (world == null)
      return;
    EAIManager.isAnimFreeze = !EAIManager.isAnimFreeze;
    List<Entity> list = world.Entities.list;
    for (int index = 0; index < list.Count; ++index)
    {
      EntityAlive entityAlive = list[index] as EntityAlive;
      if (Object.op_Implicit((Object) entityAlive) && entityAlive.aiManager != null && !entityAlive.emodel.IsRagdollActive && Object.op_Implicit((Object) entityAlive.emodel.avatarController))
      {
        Animator animator = entityAlive.emodel.avatarController.GetAnimator();
        if (Object.op_Implicit((Object) animator))
          ((Behaviour) animator).enabled = !EAIManager.isAnimFreeze;
      }
    }
  }
}
