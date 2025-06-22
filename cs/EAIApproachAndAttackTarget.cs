// Decompiled with JetBrains decompiler
// Type: EAIApproachAndAttackTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using GamePath;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
[Preserve]
public class EAIApproachAndAttackTarget : EAIBase
{
  [PublicizedFrom(EAccessModifier.Private)]
  public const float cSleeperChaseTime = 90f;
  [PublicizedFrom(EAccessModifier.Private)]
  public List<EAIApproachAndAttackTarget.TargetClass> targetClasses;
  [PublicizedFrom(EAccessModifier.Private)]
  public float chaseTimeMax;
  [PublicizedFrom(EAccessModifier.Private)]
  public bool hasHome;
  [PublicizedFrom(EAccessModifier.Private)]
  public bool isGoingHome;
  [PublicizedFrom(EAccessModifier.Private)]
  public float homeTimeout;
  [PublicizedFrom(EAccessModifier.Private)]
  public EntityAlive entityTarget;
  [PublicizedFrom(EAccessModifier.Private)]
  public Vector3 entityTargetPos;
  [PublicizedFrom(EAccessModifier.Private)]
  public Vector3 entityTargetVel;
  [PublicizedFrom(EAccessModifier.Private)]
  public int attackTimeout;
  [PublicizedFrom(EAccessModifier.Private)]
  public int pathCounter;
  [PublicizedFrom(EAccessModifier.Private)]
  public Vector2 seekPosOffset;
  [PublicizedFrom(EAccessModifier.Private)]
  public bool isTargetToEat;
  [PublicizedFrom(EAccessModifier.Private)]
  public bool isEating;
  [PublicizedFrom(EAccessModifier.Private)]
  public int eatCount;
  [PublicizedFrom(EAccessModifier.Private)]
  public EAIBlockingTargetTask blockTargetTask;
  [PublicizedFrom(EAccessModifier.Private)]
  public int relocateTicks;

  public override void Init(EntityAlive _theEntity)
  {
    base.Init(_theEntity);
    this.MutexBits = 3;
    this.executeDelay = 0.1f;
  }

  public override void SetData(DictionarySave<string, string> data)
  {
    base.SetData(data);
    this.targetClasses = new List<EAIApproachAndAttackTarget.TargetClass>();
    string str;
    if (!data.TryGetValue("class", out str))
      return;
    string[] strArray = str.Split(',', StringSplitOptions.None);
    for (int index = 0; index < strArray.Length; index += 2)
    {
      EAIApproachAndAttackTarget.TargetClass targetClass = new EAIApproachAndAttackTarget.TargetClass();
      targetClass.type = EntityFactory.GetEntityType(strArray[index]);
      targetClass.chaseTimeMax = 0.0f;
      if (index + 1 < strArray.Length)
        targetClass.chaseTimeMax = StringParsers.ParseFloat(strArray[index + 1]);
      this.targetClasses.Add(targetClass);
      if (targetClass.type == typeof (EntityEnemyAnimal))
      {
        targetClass.type = typeof (EntityAnimalSnake);
        this.targetClasses.Add(targetClass);
      }
    }
  }

  public void SetTargetOnlyPlayers()
  {
    this.targetClasses.Clear();
    this.targetClasses.Add(new EAIApproachAndAttackTarget.TargetClass()
    {
      type = typeof (EntityPlayer)
    });
  }

  public override bool CanExecute()
  {
    if (this.theEntity.sleepingOrWakingUp || this.theEntity.bodyDamage.CurrentStun != EnumEntityStunType.None || this.theEntity.Jumping && !this.theEntity.isSwimming)
      return false;
    this.entityTarget = this.theEntity.GetAttackTarget();
    if (Object.op_Equality((Object) this.entityTarget, (Object) null))
      return false;
    System.Type type = ((object) this.entityTarget).GetType();
    for (int index = 0; index < this.targetClasses.Count; ++index)
    {
      EAIApproachAndAttackTarget.TargetClass targetClass = this.targetClasses[index];
      if (targetClass.type != (System.Type) null && targetClass.type.IsAssignableFrom(type))
      {
        this.chaseTimeMax = targetClass.chaseTimeMax;
        return true;
      }
    }
    return false;
  }

  public override void Start()
  {
    this.entityTargetPos = this.entityTarget.position;
    this.entityTargetVel = Vector3.zero;
    this.isTargetToEat = this.entityTarget.IsDead();
    this.isEating = false;
    this.theEntity.IsEating = false;
    this.homeTimeout = this.theEntity.IsSleeper ? 90f : this.chaseTimeMax;
    this.hasHome = (double) this.homeTimeout > 0.0;
    this.isGoingHome = false;
    if (Vector3.op_Equality(this.theEntity.ChaseReturnLocation, Vector3.zero))
      this.theEntity.ChaseReturnLocation = this.theEntity.IsSleeper ? this.theEntity.SleeperSpawnPosition : this.theEntity.position;
    this.pathCounter = 0;
    this.relocateTicks = 0;
    this.attackTimeout = 5;
  }

  public override bool Continue()
  {
    if (this.theEntity.sleepingOrWakingUp || this.theEntity.bodyDamage.CurrentStun != EnumEntityStunType.None)
      return false;
    EntityAlive attackTarget = this.theEntity.GetAttackTarget();
    return this.isGoingHome ? !Object.op_Implicit((Object) attackTarget) && Vector3.op_Inequality(this.theEntity.ChaseReturnLocation, Vector3.zero) : Object.op_Implicit((Object) attackTarget) && !Object.op_Inequality((Object) attackTarget, (Object) this.entityTarget) && attackTarget.IsDead() == this.isTargetToEat;
  }

  public override void Reset()
  {
    this.theEntity.IsEating = false;
    this.theEntity.moveHelper.Stop();
    if (this.blockTargetTask == null)
      return;
    this.blockTargetTask.canExecute = false;
  }

  public override void Update()
  {
    if (this.hasHome && !this.isTargetToEat)
    {
      if (this.isGoingHome)
      {
        Vector3 vector3 = Vector3.op_Subtraction(this.theEntity.ChaseReturnLocation, this.theEntity.position);
        float y = vector3.y;
        vector3.y = 0.0f;
        if ((double) ((Vector3) ref vector3).sqrMagnitude <= 0.16000001132488251 && (double) Utils.FastAbs(y) < 2.0)
        {
          Vector3 chaseReturnLocation = this.theEntity.ChaseReturnLocation;
          chaseReturnLocation.y = this.theEntity.position.y;
          this.theEntity.SetPosition(chaseReturnLocation);
          this.theEntity.ChaseReturnLocation = Vector3.zero;
          if (!this.theEntity.IsSleeper)
            return;
          this.theEntity.ResumeSleeperPose();
          return;
        }
        if (--this.pathCounter > 0 || PathFinderThread.Instance.IsCalculatingPath(this.theEntity.entityId))
          return;
        this.pathCounter = 60;
        this.theEntity.FindPath(this.theEntity.ChaseReturnLocation, this.theEntity.GetMoveSpeedAggro() * 0.8f, false, (EAIBase) this);
        return;
      }
      this.homeTimeout -= 0.05f;
      if ((double) this.homeTimeout <= 0.0)
      {
        if (this.blockTargetTask == null)
        {
          List<EAIBlockingTargetTask> targetTasks = this.manager.GetTargetTasks<EAIBlockingTargetTask>();
          if (targetTasks != null)
            this.blockTargetTask = targetTasks[0];
        }
        if (this.blockTargetTask != null)
          this.blockTargetTask.canExecute = true;
        this.theEntity.SetAttackTarget((EntityAlive) null, 0);
        this.theEntity.SetLookPosition(Vector3.zero);
        this.theEntity.PlayGiveUpSound();
        this.pathCounter = 0;
        this.isGoingHome = true;
        return;
      }
    }
    if (Object.op_Equality((Object) this.entityTarget, (Object) null))
      return;
    if (this.relocateTicks > 0)
    {
      if (this.theEntity.navigator.noPathAndNotPlanningOne())
      {
        this.relocateTicks = 0;
      }
      else
      {
        --this.relocateTicks;
        this.theEntity.moveHelper.SetFocusPos(this.entityTarget.position);
        return;
      }
    }
    Vector3 vector3_1 = this.entityTarget.position;
    if (this.isTargetToEat)
      vector3_1 = this.entityTarget.getBellyPosition();
    Vector3 vector3_2 = Vector3.op_Subtraction(vector3_1, this.entityTargetPos);
    if ((double) ((Vector3) ref vector3_2).sqrMagnitude < 1.0)
      this.entityTargetVel = Vector3.op_Addition(Vector3.op_Multiply(this.entityTargetVel, 0.7f), Vector3.op_Multiply(vector3_2, 0.3f));
    this.entityTargetPos = vector3_1;
    --this.attackTimeout;
    if (this.isEating)
    {
      if (this.theEntity.bodyDamage.HasLimbs)
        this.theEntity.RotateTo(vector3_1.x, vector3_1.y, vector3_1.z, 8f, 5f);
      if (this.attackTimeout > 0)
        return;
      this.attackTimeout = 25 + this.GetRandom(10);
      if ((this.eatCount & 1) == 0)
      {
        this.theEntity.PlayOneShot("eat_player");
        this.entityTarget.DamageEntity(DamageSource.eat, 35, false, 1f);
      }
      Vector3 _pos;
      // ISSUE: explicit constructor call
      ((Vector3) ref _pos).\u002Ector(0.0f, 0.04f, 0.08f);
      ParticleEffect _pe = new ParticleEffect("blood_eat", _pos, 1f, Color.white, (string) null, this.theEntity.entityId, ParticleEffect.Attachment.Head);
      GameManager.Instance.SpawnParticleEffectServer(_pe, this.theEntity.entityId, false, false);
      ++this.eatCount;
    }
    else
    {
      this.theEntity.moveHelper.CalcIfUnreachablePos();
      float num1;
      float maxDist;
      if (!this.isTargetToEat)
      {
        ItemValue holdingItemItemValue = this.theEntity.inventory.holdingItemItemValue;
        int holdingItemIdx = this.theEntity.inventory.holdingItemIdx;
        ItemAction action = holdingItemItemValue.ItemClass.Actions[holdingItemIdx];
        num1 = 1.095f;
        if (action != null)
        {
          num1 = action.Range;
          if ((double) num1 == 0.0)
            num1 = EffectManager.GetItemValue(PassiveEffects.MaxRange, holdingItemItemValue);
        }
        maxDist = Utils.FastMax(0.7f, num1 - 0.35f);
      }
      else
      {
        num1 = this.theEntity.GetHeight() * 0.9f;
        maxDist = num1 - 0.05f;
      }
      float num2 = maxDist * maxDist;
      float num3 = 4f;
      if (this.theEntity.IsFeral)
        num3 = 8f;
      float targetXzDistanceSq = this.GetTargetXZDistanceSq(this.RandomFloat * num3);
      float _x = vector3_1.y - this.theEntity.position.y;
      float num4 = Utils.FastAbs(_x);
      bool flag = (double) targetXzDistanceSq <= (double) num2 && (double) num4 < 1.0;
      if (!flag)
      {
        if ((double) num4 < 3.0 && !PathFinderThread.Instance.IsCalculatingPath(this.theEntity.entityId))
        {
          PathEntity path = this.theEntity.navigator.getPath();
          if (path != null && path.NodeCountRemaining() <= 2)
            this.pathCounter = 0;
        }
        if (--this.pathCounter <= 0 && this.theEntity.CanNavigatePath() && !PathFinderThread.Instance.IsCalculatingPath(this.theEntity.entityId))
        {
          this.pathCounter = 6 + this.GetRandom(10);
          Vector3 moveToLocation = this.GetMoveToLocation(maxDist);
          if ((double) moveToLocation.y - (double) this.theEntity.position.y < -8.0)
          {
            this.pathCounter += 40;
            if ((double) this.RandomFloat < 0.20000000298023224)
            {
              this.seekPosOffset.x += (float) ((double) this.RandomFloat * 0.60000002384185791 - 0.30000001192092896);
              this.seekPosOffset.y += (float) ((double) this.RandomFloat * 0.60000002384185791 - 0.30000001192092896);
            }
            moveToLocation.x += this.seekPosOffset.x;
            moveToLocation.z += this.seekPosOffset.y;
          }
          else
          {
            Vector3 vector3_3 = Vector3.op_Subtraction(moveToLocation, this.theEntity.position);
            float num5 = ((Vector3) ref vector3_3).magnitude - 5f;
            if ((double) num5 > 0.0)
            {
              if ((double) num5 > 60.0)
                num5 = 60f;
              this.pathCounter += (int) ((double) num5 / 20.0 * 20.0);
            }
          }
          this.theEntity.FindPath(moveToLocation, this.theEntity.GetMoveSpeedAggro(), true, (EAIBase) this);
        }
      }
      if (this.theEntity.Climbing)
        return;
      this.theEntity.SetLookPosition(!this.theEntity.CanSee(this.entityTarget) || this.theEntity.IsBreakingBlocks ? Vector3.zero : this.entityTarget.getHeadPosition());
      if (!flag)
      {
        if (this.theEntity.navigator.noPathAndNotPlanningOne() && this.pathCounter > 0 && (double) _x < 2.0999999046325684)
          this.theEntity.moveHelper.SetMoveTo(this.GetMoveToLocation(maxDist), true);
      }
      else
      {
        this.theEntity.moveHelper.Stop();
        this.pathCounter = 0;
      }
      float num6 = this.isTargetToEat ? num1 : num1 - 0.1f;
      float num7 = num6 * num6;
      if ((double) targetXzDistanceSq > (double) num7 || (double) _x < -1.25 || (double) _x - (double) this.theEntity.GetHeight() > 0.64999997615814209)
        return;
      this.theEntity.IsBreakingBlocks = false;
      this.theEntity.IsBreakingDoors = false;
      if (this.theEntity.bodyDamage.HasLimbs && !this.theEntity.Electrocuted)
        this.theEntity.RotateTo(vector3_1.x, vector3_1.y, vector3_1.z, 30f, 30f);
      if (this.isTargetToEat)
      {
        this.isEating = true;
        this.theEntity.IsEating = true;
        this.attackTimeout = 20;
        this.eatCount = 0;
      }
      else
      {
        if ((Object.op_Equality((Object) this.theEntity.GetDamagedTarget(), (Object) this.entityTarget) ? 1 : (!Object.op_Inequality((Object) this.entityTarget, (Object) null) ? 0 : (Object.op_Equality((Object) this.entityTarget.GetDamagedTarget(), (Object) this.theEntity) ? 1 : 0))) != 0)
        {
          this.homeTimeout = this.theEntity.IsSleeper ? 90f : this.chaseTimeMax;
          if (this.blockTargetTask != null)
            this.blockTargetTask.canExecute = false;
          this.theEntity.ClearDamagedTarget();
          if (Object.op_Implicit((Object) this.entityTarget))
            this.entityTarget.ClearDamagedTarget();
        }
        if (this.attackTimeout > 0)
          return;
        if ((double) this.manager.groupCircle > 0.0)
        {
          Entity targetIfAttackedNow = this.theEntity.GetTargetIfAttackedNow();
          if (Object.op_Inequality((Object) targetIfAttackedNow, (Object) this.entityTarget) && (!Object.op_Implicit((Object) this.entityTarget.AttachedToEntity) || Object.op_Inequality((Object) this.entityTarget.AttachedToEntity, (Object) targetIfAttackedNow)))
          {
            if (!Object.op_Inequality((Object) targetIfAttackedNow, (Object) null))
              return;
            this.relocateTicks = 46;
            Vector3 vector3_4 = Vector3.op_Subtraction(this.theEntity.position, vector3_1);
            Vector3 vector3_5 = Vector3.op_Multiply(((Vector3) ref vector3_4).normalized, num6 + 1.1f);
            float num8 = (float) ((double) this.RandomFloat * 28.0 + 18.0);
            if ((double) this.RandomFloat < 0.5)
              num8 = -num8;
            Vector3 vector3_6 = Quaternion.op_Multiply(Quaternion.Euler(0.0f, num8, 0.0f), vector3_5);
            this.theEntity.FindPath(Vector3.op_Addition(vector3_1, vector3_6), this.theEntity.GetMoveSpeedAggro(), true, (EAIBase) this);
            return;
          }
        }
        this.theEntity.SleeperSupressLivingSounds = false;
        if (!this.theEntity.Attack(false))
          return;
        this.attackTimeout = this.theEntity.GetAttackTimeoutTicks();
        this.theEntity.Attack(true);
      }
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public float GetTargetXZDistanceSq(float estimatedTicks)
  {
    Vector3 vector3_1 = Vector3.op_Addition(this.entityTarget.position, Vector3.op_Multiply(this.entityTargetVel, estimatedTicks));
    if (this.isTargetToEat)
      vector3_1 = this.entityTarget.getBellyPosition();
    Vector3 vector3_2 = Vector3.op_Subtraction(Vector3.op_Addition(this.theEntity.position, Vector3.op_Multiply(this.theEntity.motion, estimatedTicks)), vector3_1);
    vector3_2.y = 0.0f;
    return ((Vector3) ref vector3_2).sqrMagnitude;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public Vector3 GetMoveToLocation(float maxDist)
  {
    Vector3 pos = Vector3.op_Addition(this.entityTarget.position, Vector3.op_Multiply(this.entityTargetVel, 6f));
    if (this.isTargetToEat)
      pos = this.entityTarget.getBellyPosition();
    Vector3 supportingBlockPos = this.entityTarget.world.FindSupportingBlockPos(pos);
    if ((double) maxDist > 0.0)
    {
      Vector3 vector3_1;
      // ISSUE: explicit constructor call
      ((Vector3) ref vector3_1).\u002Ector(this.theEntity.position.x, supportingBlockPos.y, this.theEntity.position.z);
      Vector3 vector3_2 = Vector3.op_Subtraction(supportingBlockPos, vector3_1);
      float magnitude = ((Vector3) ref vector3_2).magnitude;
      if ((double) magnitude < 3.0)
      {
        if ((double) magnitude <= (double) maxDist)
        {
          float num = supportingBlockPos.y - this.theEntity.position.y;
          return (double) num < -3.0 || (double) num > 1.5 ? supportingBlockPos : vector3_1;
        }
        Vector3 vector3_3 = Vector3.op_Multiply(vector3_2, maxDist / magnitude);
        Vector3 _worldPos = Vector3.op_Subtraction(supportingBlockPos, vector3_3);
        _worldPos.y += 0.51f;
        BlockValue block1 = this.entityTarget.world.GetBlock(World.worldToBlockPos(_worldPos));
        Block block2 = block1.Block;
        if (block2.PathType <= 0)
        {
          RaycastHit raycastHit;
          if (Physics.Raycast(Vector3.op_Subtraction(_worldPos, Origin.position), Vector3.down, ref raycastHit, 1.02f, 1082195968 /*0x40810000*/))
          {
            _worldPos.y = ((RaycastHit) ref raycastHit).point.y + Origin.position.y;
            return _worldPos;
          }
          if (block2.IsElevator((int) block1.rotation))
          {
            _worldPos.y = supportingBlockPos.y;
            return _worldPos;
          }
        }
      }
    }
    return supportingBlockPos;
  }

  public override string ToString()
  {
    ItemValue holdingItemItemValue = this.theEntity.inventory.holdingItemItemValue;
    int holdingItemIdx = this.theEntity.inventory.holdingItemIdx;
    ItemAction action = holdingItemItemValue.ItemClass.Actions[holdingItemIdx];
    float num1 = 1.095f;
    if (!this.isTargetToEat && action != null)
    {
      num1 = action.Range;
      if ((double) num1 == 0.0)
        num1 = EffectManager.GetItemValue(PassiveEffects.MaxRange, holdingItemItemValue);
    }
    float num2 = this.isTargetToEat ? num1 : num1 - 0.1f;
    float targetXzDistanceSq = this.GetTargetXZDistanceSq(0.0f);
    return $"{base.ToString()}, {(Object.op_Implicit((Object) this.entityTarget) ? (object) this.entityTarget.EntityName : (object) "")}{(this.theEntity.CanSee(this.entityTarget) ? (object) "(see)" : (object) "")}{(this.theEntity.navigator.noPathAndNotPlanningOne() ? (object) "(-path)" : (this.theEntity.navigator.noPath() ? (object) "(!path)" : (object) ""))}{(this.isTargetToEat ? (object) "(eat)" : (object) "")}{(this.isGoingHome ? (object) "(home)" : (object) "")} dist {Mathf.Sqrt(targetXzDistanceSq).ToCultureInvariantString("0.000")} rng {num2.ToCultureInvariantString("0.000")} timeout {this.homeTimeout.ToCultureInvariantString("0.00")}";
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public struct TargetClass
  {
    public System.Type type;
    public float chaseTimeMax;
  }
}
