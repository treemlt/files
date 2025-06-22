// Decompiled with JetBrains decompiler
// Type: EntityVulture
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
[Preserve]
public class EntityVulture : EntityFlying
{
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public const float cFlyingMinimumSpeed = 0.02f;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public const float cTargetDistanceClose = 0.9f;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public const float cTargetDistanceMax = 80f;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public const float cTargetAttackOffsetY = -0.1f;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public const float cVomitMinRange = 3f;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public const int cAttackDelay = 18;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public const int cCollisionMask = 1082195968 /*0x40810000*/;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public const float cBattleFatigueMin = 30f;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public const float cBattleFatigueMax = 60f;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public const float cBattleFatigueCooldownMin = 80f;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public const float cBattleFatigueCooldownMax = 180f;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int moveUpdateDelay;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float motionReverseScale = 1f;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public Vector3 waypoint;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool isCircling;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public Vector3 circleCenter;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float circleReverseScale;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float glidingCurrentPercent;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float glidingPercent;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float accel;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public Vector2 wanderHeightRange = new Vector2(10f, 30f);
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public EntityAlive currentTarget;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public float targetAttackHealthPercent = 0.8f;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int targetSwitchDelay;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int homeCheckDelay;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int homeSeekDelay;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int wanderChangeDelay;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int checkBlockedDelay;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float battleDuration;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float battleFatigueSeconds;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool isBattleFatigued;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int attackDelay;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int attackCount;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int attack2Delay;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool isAttack2On;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public EAISetNearestEntityAsTargetSorter sorter;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public static List<Entity> list = new List<Entity>();
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public EntityVulture.State state;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float stateTime;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float stateMaxTime;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public List<Bounds> collBB = new List<Bounds>();

  [PublicizedFrom(EAccessModifier.Protected)]
  public override void Awake()
  {
    BoxCollider component = ((Component) this).gameObject.GetComponent<BoxCollider>();
    if (Object.op_Implicit((Object) component))
    {
      component.center = new Vector3(0.0f, 0.35f, 0.0f);
      component.size = new Vector3(0.4f, 0.4f, 0.4f);
    }
    base.Awake();
    this.state = EntityVulture.State.WanderStart;
  }

  public override void Init(int _entityClass)
  {
    base.Init(_entityClass);
    this.Init();
  }

  public override void InitFromPrefab(int _entityClass)
  {
    base.InitFromPrefab(_entityClass);
    this.Init();
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public virtual void Init()
  {
    if (this.navigator != null)
      this.navigator.setCanDrown(true);
    this.battleFatigueSeconds = this.rand.RandomRange(30f, 60f);
  }

  public override void SetSleeper()
  {
    base.SetSleeper();
    this.sorter = new EAISetNearestEntityAsTargetSorter((Entity) this);
    this.setHomeArea(new Vector3i(this.position), (int) this.sleeperSightRange + 1);
    this.battleFatigueSeconds = float.MaxValue;
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public override void updateTasks()
  {
    if (GamePrefs.GetBool(EnumGamePrefs.DebugStopEnemiesMoving))
    {
      this.aiManager.UpdateDebugName();
    }
    else
    {
      if (GameStats.GetInt(EnumGameStats.GameState) == 2)
        return;
      this.CheckDespawn();
      this.GetEntitySenses().ClearIfExpired();
      if (this.IsSleeperPassive)
        return;
      if (this.IsSleeping)
      {
        float seeDistance = this.GetSeeDistance();
        this.world.GetEntitiesInBounds(typeof (EntityPlayer), BoundsUtils.ExpandBounds(this.boundingBox, seeDistance, seeDistance, seeDistance), EntityVulture.list);
        EntityVulture.list.Sort((IComparer<Entity>) this.sorter);
        EntityPlayer _attackTarget = (EntityPlayer) null;
        float num = float.MaxValue;
        if (Object.op_Inequality((Object) this.noisePlayer, (Object) null) && (double) this.noisePlayerVolume >= (double) this.sleeperNoiseToWake)
        {
          _attackTarget = this.noisePlayer;
          num = this.noisePlayerDistance;
        }
        for (int index = 0; index < EntityVulture.list.Count; ++index)
        {
          EntityPlayer _other = (EntityPlayer) EntityVulture.list[index];
          if (this.CanSee((EntityAlive) _other))
          {
            float distance = this.GetDistance((Entity) _other);
            if (this.GetSleeperDisturbedLevel(distance, _other.Stealth.lightLevel) >= 2 && (double) distance < (double) num)
            {
              _attackTarget = _other;
              num = distance;
            }
          }
        }
        EntityVulture.list.Clear();
        if (Object.op_Equality((Object) _attackTarget, (Object) null))
          return;
        this.ConditionalTriggerSleeperWakeUp();
        this.SetAttackTarget((EntityAlive) _attackTarget, 1200);
      }
      bool flag1 = this.Buffs.HasBuff("buffShocked");
      if (flag1)
      {
        this.SetState(EntityVulture.State.Stun);
      }
      else
      {
        EntityAlive revengeTarget = this.GetRevengeTarget();
        if (Object.op_Implicit((Object) revengeTarget))
        {
          this.battleDuration = 0.0f;
          this.isBattleFatigued = false;
          this.SetRevengeTarget((EntityAlive) null);
          if (Object.op_Inequality((Object) revengeTarget, (Object) this.attackTarget) && (!Object.op_Implicit((Object) this.attackTarget) || (double) this.rand.RandomFloat < 0.5))
            this.SetAttackTarget(revengeTarget, 1200);
        }
        if (Object.op_Inequality((Object) this.attackTarget, (Object) this.currentTarget))
        {
          this.currentTarget = this.attackTarget;
          if (Object.op_Implicit((Object) this.currentTarget))
          {
            this.SetState(EntityVulture.State.Attack);
            this.waypoint = this.position;
            this.moveUpdateDelay = 0;
            this.homeCheckDelay = 400;
          }
          else
            this.SetState(EntityVulture.State.AttackStop);
        }
      }
      Vector3 vector3_1 = Vector3.op_Subtraction(this.waypoint, this.position);
      float sqrMagnitude1 = ((Vector3) ref vector3_1).sqrMagnitude;
      this.stateTime += 0.05f;
      switch (this.state)
      {
        case EntityVulture.State.Attack:
          this.battleDuration += 0.05f;
          break;
        case EntityVulture.State.AttackReposition:
          if ((double) sqrMagnitude1 < 2.25 || (double) this.stateTime >= (double) this.stateMaxTime)
          {
            this.SetState(EntityVulture.State.Attack);
            this.motion = Vector3.op_Multiply(this.motion, -0.2f);
            this.motion.y = 0.0f;
            break;
          }
          break;
        case EntityVulture.State.AttackStop:
          this.ClearTarget();
          this.SetState(EntityVulture.State.WanderStart);
          break;
        case EntityVulture.State.Home:
          if ((double) sqrMagnitude1 < 4.0 || (double) this.stateTime > 30.0)
          {
            this.SetState(EntityVulture.State.WanderStart);
            break;
          }
          if (--this.homeSeekDelay <= 0)
          {
            this.homeSeekDelay = 40;
            int _minXZ = 10;
            if ((double) this.stateTime > 20.0)
              _minXZ = -20;
            int maximumHomeDistance = this.getMaximumHomeDistance();
            Vector3 vector3_2 = RandomPositionGenerator.CalcTowards((EntityAlive) this, _minXZ, 30, maximumHomeDistance / 2, this.getHomePosition().position.ToVector3());
            if (!((Vector3) ref vector3_2).Equals(Vector3.zero))
            {
              this.waypoint = vector3_2;
              this.AdjustWaypoint();
              break;
            }
            break;
          }
          break;
        case EntityVulture.State.Stun:
          Animator componentInChildren = ((Component) this.ModelTransform).GetComponentInChildren<Animator>();
          if (flag1)
          {
            this.motion = Vector3.op_Multiply(this.rand.RandomOnUnitSphere, -0.075f);
            this.motion.y += -0.0600000024f;
            if (!Object.op_Implicit((Object) componentInChildren))
              return;
            ((Behaviour) componentInChildren).enabled = false;
            return;
          }
          if (Object.op_Implicit((Object) componentInChildren))
            ((Behaviour) componentInChildren).enabled = true;
          this.SetState(EntityVulture.State.WanderStart);
          break;
        case EntityVulture.State.WanderStart:
          this.homeCheckDelay = 60;
          if (!this.isWithinHomeDistanceCurrentPosition())
          {
            this.StartHome(this.getHomePosition().position.ToVector3());
            break;
          }
          this.SetState(EntityVulture.State.Wander);
          this.isCircling = !this.IsSleeper && (double) this.rand.RandomFloat < 0.40000000596046448;
          float y = this.position.y;
          RaycastHit raycastHit;
          if (Physics.Raycast(Vector3.op_Subtraction(this.position, Origin.position), Vector3.down, ref raycastHit, 999f, 65536 /*0x010000*/))
          {
            float num = this.rand.RandomRange(this.wanderHeightRange.x, this.wanderHeightRange.y);
            if (this.IsSleeper)
              num *= 0.4f;
            y += -((RaycastHit) ref raycastHit).distance + num;
          }
          else
            this.isCircling = false;
          bool flag2 = false;
          EntityPlayer _other1 = (EntityPlayer) null;
          if (!this.isBattleFatigued)
          {
            _other1 = this.world.GetClosestPlayerSeen((EntityAlive) this, 80f, 1f);
            if (Object.op_Implicit((Object) _other1) && (double) this.GetDistanceSq((Entity) _other1) > 400.0)
              flag2 = true;
          }
          if (this.isCircling)
          {
            this.wanderChangeDelay = 120;
            Vector3 right = ((Component) this).transform.right;
            right.y = 0.0f;
            this.circleReverseScale = 1f;
            if ((double) this.rand.RandomFloat < 0.5)
            {
              this.circleReverseScale = -1f;
              right.x = -right.x;
              right.z = -right.z;
            }
            this.circleCenter = Vector3.op_Addition(this.position, Vector3.op_Multiply(right, (float) (3.0 + (double) this.rand.RandomFloat * 7.0)));
            this.circleCenter.y = y;
            if (flag2)
            {
              this.circleCenter.x = (float) ((double) this.circleCenter.x * 0.60000002384185791 + (double) _other1.position.x * 0.40000000596046448);
              this.circleCenter.z = (float) ((double) this.circleCenter.z * 0.60000002384185791 + (double) _other1.position.z * 0.40000000596046448);
              break;
            }
            break;
          }
          this.wanderChangeDelay = 400;
          this.waypoint = this.position;
          this.waypoint.x += (float) ((double) this.rand.RandomFloat * 16.0 - 8.0);
          this.waypoint.y = y;
          this.waypoint.z += (float) ((double) this.rand.RandomFloat * 16.0 - 8.0);
          if (flag2)
          {
            this.waypoint.x = (float) ((double) this.waypoint.x * 0.60000002384185791 + (double) _other1.position.x * 0.40000000596046448);
            this.waypoint.z = (float) ((double) this.waypoint.z * 0.60000002384185791 + (double) _other1.position.z * 0.40000000596046448);
          }
          this.AdjustWaypoint();
          break;
        case EntityVulture.State.Wander:
          if (this.isBattleFatigued)
          {
            this.battleDuration -= 0.05f;
            if ((double) this.battleDuration <= 0.0)
              this.isBattleFatigued = false;
          }
          if (--this.wanderChangeDelay <= 0)
            this.SetState(EntityVulture.State.WanderStart);
          if (this.isCircling)
          {
            Vector3 vector3_3 = Vector3.op_Subtraction(this.circleCenter, this.position);
            float x = vector3_3.x;
            vector3_3.x = -vector3_3.z * this.circleReverseScale;
            vector3_3.z = x * this.circleReverseScale;
            vector3_3.y = 0.0f;
            this.waypoint = Vector3.op_Addition(this.position, vector3_3);
          }
          else if ((double) sqrMagnitude1 < 1.0)
            this.SetState(EntityVulture.State.WanderStart);
          if (--this.targetSwitchDelay <= 0)
          {
            this.targetSwitchDelay = 40;
            if (this.IsSleeper || (double) this.rand.RandomFloat >= 0.5)
            {
              EntityPlayer target = this.FindTarget();
              if (Object.op_Implicit((Object) target))
              {
                this.SetAttackTarget((EntityAlive) target, 1200);
                break;
              }
              break;
            }
            break;
          }
          break;
      }
      if (this.state != EntityVulture.State.Home && --this.homeCheckDelay <= 0)
      {
        this.homeCheckDelay = 60;
        if (!this.isWithinHomeDistanceCurrentPosition())
          this.SetState(EntityVulture.State.AttackStop);
      }
      if (--this.moveUpdateDelay <= 0)
      {
        this.moveUpdateDelay = 4 + this.rand.RandomRange(5);
        if (Object.op_Implicit((Object) this.currentTarget) && this.state == EntityVulture.State.Attack)
        {
          this.waypoint = this.currentTarget.getHeadPosition();
          this.waypoint.y += -0.1f;
          this.waypoint = !Object.op_Implicit((Object) this.currentTarget.AttachedToEntity) ? Vector3.op_Addition(this.waypoint, Vector3.op_Multiply(this.currentTarget.GetVelocityPerSecond(), 0.1f)) : Vector3.op_Addition(this.waypoint, Vector3.op_Multiply(this.currentTarget.GetVelocityPerSecond(), 0.3f));
          Vector3 vector3_4 = Vector3.op_Subtraction(this.waypoint, this.position);
          vector3_4.y = 0.0f;
          ((Vector3) ref vector3_4).Normalize();
          this.waypoint = Vector3.op_Addition(this.waypoint, Vector3.op_Multiply(vector3_4, -0.6f));
        }
        if (!this.IsCourseTraversable(this.waypoint, out float _))
        {
          this.waypoint.y += 2f;
          if (this.state == EntityVulture.State.Attack)
          {
            if ((double) this.rand.RandomFloat < 0.10000000149011612)
              this.StartAttackReposition();
          }
          else if (this.state != EntityVulture.State.Home && this.state != EntityVulture.State.AttackReposition)
            this.SetState(EntityVulture.State.WanderStart);
        }
      }
      Vector3 vector3_5 = Vector3.op_Subtraction(this.waypoint, this.position);
      float magnitude1 = ((Vector3) ref vector3_5).magnitude;
      Vector3 vector3_6 = Vector3.op_Multiply(vector3_5, 1f / magnitude1);
      this.glidingPercent = 0.0f;
      if ((double) vector3_6.y > 0.56999999284744263)
        this.accel = 0.35f;
      else if ((double) vector3_6.y < -0.34000000357627869)
      {
        this.accel = 0.95f;
        this.glidingPercent = 1f;
      }
      else
      {
        this.accel = 0.55f;
        if (this.state == EntityVulture.State.Home || this.state == EntityVulture.State.Wander)
        {
          this.accel = 0.8f;
          if (this.isCircling)
            this.glidingPercent = 1f;
        }
      }
      if (this.attackDelay > 0)
        this.glidingPercent = 0.0f;
      if (Object.op_Implicit((Object) this.currentTarget) && Object.op_Implicit((Object) this.currentTarget.AttachedToEntity))
      {
        if (this.IsBloodMoon && (double) this.accel > 0.5)
          this.accel = 2.5f;
        this.accel *= this.moveSpeedAggro;
      }
      else
        this.accel *= this.moveSpeed;
      this.motion = Vector3.op_Addition(Vector3.op_Multiply(this.motion, 0.9f), Vector3.op_Multiply(vector3_6, this.accel * 0.1f));
      if (Object.op_Implicit((Object) this.emodel.avatarController))
      {
        this.glidingCurrentPercent = Mathf.MoveTowards(this.glidingCurrentPercent, this.glidingPercent, 0.0600000024f);
        this.emodel.avatarController.UpdateFloat("Gliding", this.glidingCurrentPercent);
      }
      if (this.attackDelay > 0)
        --this.attackDelay;
      if (this.attack2Delay > 0)
        --this.attack2Delay;
      float num1 = Mathf.Atan2(this.motion.x * this.motionReverseScale, this.motion.z * this.motionReverseScale) * 57.29578f;
      if (Object.op_Implicit((Object) this.currentTarget) && --this.targetSwitchDelay <= 0)
      {
        this.targetSwitchDelay = 60;
        if (this.state != EntityVulture.State.AttackStop)
        {
          EntityPlayer target = this.FindTarget();
          if (Object.op_Implicit((Object) target) && Object.op_Inequality((Object) target, (Object) this.attackTarget))
            this.SetAttackTarget((EntityAlive) target, 400);
        }
        float num2 = Object.op_Implicit((Object) this.currentTarget.AttachedToEntity) ? 0.1f : 0.25f;
        if (this.state != EntityVulture.State.AttackReposition && (double) this.rand.RandomFloat < (double) num2)
          this.StartAttackReposition();
      }
      if (Object.op_Implicit((Object) this.currentTarget))
      {
        Vector3 _lookPos = Vector3.op_Addition(this.currentTarget.getHeadPosition(), Vector3.op_Multiply(this.currentTarget.GetVelocityPerSecond(), 0.1f));
        Vector3 vector3_7 = Vector3.op_Subtraction(_lookPos, this.position);
        float sqrMagnitude2 = ((Vector3) ref vector3_7).sqrMagnitude;
        if ((double) sqrMagnitude2 > 6400.0 && !this.IsBloodMoon || this.currentTarget.IsDead())
          this.SetState(EntityVulture.State.AttackStop);
        else if (this.state != EntityVulture.State.AttackReposition)
        {
          if ((double) sqrMagnitude2 < 4.0)
            num1 = Mathf.Atan2(vector3_7.x, vector3_7.z) * 57.29578f;
          if (this.attackDelay <= 0 && !this.isAttack2On)
          {
            if ((double) sqrMagnitude2 < 0.809999942779541 && (double) this.position.y >= (double) this.currentTarget.position.y && (double) this.position.y < (double) _lookPos.y + 0.10000000149011612)
              this.AttackAndAdjust(false);
            else if (this.checkBlockedDelay > 0)
            {
              --this.checkBlockedDelay;
            }
            else
            {
              this.checkBlockedDelay = 6;
              Vector3 normalized = ((Vector3) ref vector3_7).normalized;
              Ray ray;
              // ISSUE: explicit constructor call
              ((Ray) ref ray).\u002Ector(Vector3.op_Subtraction(Vector3.op_Addition(this.position, new Vector3(0.0f, 0.22f, 0.0f)), Vector3.op_Multiply(normalized, 0.13f)), normalized);
              if (Voxel.Raycast(this.world, ray, 0.83f, 1082195968 /*0x40810000*/, 128 /*0x80*/, 0.13f))
                this.AttackAndAdjust(true);
            }
          }
          bool flag3 = false;
          if (this.inventory.holdingItemData.actionData[1] is ItemActionVomit.ItemActionDataVomit _actionData && this.attack2Delay <= 0 && (double) sqrMagnitude2 >= 9.0)
          {
            float range = ((ItemActionRanged) this.inventory.holdingItem.Actions[1]).GetRange((ItemActionData) _actionData);
            if ((double) sqrMagnitude2 < (double) range * (double) range && (double) Utils.FastAbs(Utils.DeltaAngle(num1, this.rotation.y)) < 20.0 && (double) Utils.FastAbs(Vector3.SignedAngle(vector3_7, ((Component) this).transform.forward, Vector3.right)) < 25.0)
              flag3 = true;
          }
          if (!this.isAttack2On & flag3)
          {
            this.isAttack2On = true;
            _actionData.muzzle = this.emodel.GetHeadTransform();
            _actionData.numWarningsPlayed = 999;
          }
          if (this.isAttack2On)
          {
            if (!flag3)
            {
              this.isAttack2On = false;
            }
            else
            {
              this.motion = Vector3.op_Multiply(this.motion, 0.7f);
              this.SetLookPosition(_lookPos);
              this.UseHoldingItem(1, false);
              if (!_actionData.isActive)
                this.isAttack2On = false;
            }
            if (!this.isAttack2On)
            {
              if (_actionData.numVomits > 0)
                this.StartAttackReposition();
              this.UseHoldingItem(1, true);
              this.attack2Delay = 60;
              this.SetLookPosition(Vector3.zero);
            }
          }
        }
      }
      float magnitude2 = ((Vector3) ref this.motion).magnitude;
      if ((double) magnitude2 < 0.019999999552965164)
        this.motion = Vector3.op_Multiply(this.motion, (float) (1.0 / (double) magnitude2 * 0.019999999552965164));
      double num3 = (double) this.SeekYaw(num1, 0.0f, 20f);
      this.aiManager.UpdateDebugName();
    }
  }

  public override string MakeDebugNameInfo()
  {
    return $"\n{this.state.ToStringCached<EntityVulture.State>()} {this.stateTime.ToCultureInvariantString("0.00")}\nWaypoint {this.waypoint.ToCultureInvariantString()}\nTarget {(Object.op_Implicit((Object) this.currentTarget) ? (object) ((Object) this.currentTarget).name : (object) "")}, AtkDelay {this.attackDelay}, BtlTime {this.battleDuration.ToCultureInvariantString("0.00")}\nSpeed {((Vector3) ref this.motion).magnitude.ToCultureInvariantString("0.000")}, Motion {this.motion.ToCultureInvariantString("0.000")}, Accel {this.accel.ToCultureInvariantString("0.000")}";
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public void SetState(EntityVulture.State newState)
  {
    this.state = newState;
    this.stateTime = 0.0f;
    this.motionReverseScale = 1f;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void AdjustWaypoint()
  {
    int maxValue = (int) byte.MaxValue;
    Vector3i _pos = new Vector3i(this.waypoint);
    while (!this.world.GetBlock(_pos).isair && --maxValue >= 0)
    {
      ++this.waypoint.y;
      ++_pos.y;
    }
    this.waypoint.y = Mathf.Min(this.waypoint.y, 250f);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void StartAttackReposition()
  {
    if (!this.IsBloodMoon && (double) this.battleDuration >= (double) this.battleFatigueSeconds)
    {
      this.ClearTarget();
      this.battleDuration = this.rand.RandomRange(80f, 180f);
      this.isBattleFatigued = true;
      this.SetState(EntityVulture.State.Wander);
    }
    else
    {
      this.SetState(EntityVulture.State.AttackReposition);
      this.stateMaxTime = this.rand.RandomRange(0.8f, 5f);
      this.attackCount = 0;
      this.waypoint = this.position;
      this.waypoint.x += (float) ((double) this.rand.RandomFloat * 8.0 - 4.0);
      this.waypoint.y += (float) ((double) this.rand.RandomFloat * 4.0 + 3.0);
      this.waypoint.z += (float) ((double) this.rand.RandomFloat * 8.0 - 4.0);
      this.moveUpdateDelay = 0;
      this.motion = Vector3.op_UnaryNegation(this.motion);
      if ((double) this.rand.RandomFloat >= 0.5)
        return;
      this.motionReverseScale = -1f;
      this.motion.y = 0.2f;
    }
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public void StartHome(Vector3 _homePos)
  {
    this.SetState(EntityVulture.State.Home);
    this.homeSeekDelay = 0;
    this.waypoint = _homePos;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void ClearTarget()
  {
    this.SetAttackTarget((EntityAlive) null, 0);
    this.SetRevengeTarget((EntityAlive) null);
    this.currentTarget = (EntityAlive) null;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public EntityPlayer FindTarget()
  {
    if (this.IsBloodMoon)
    {
      EntityPlayer target = this.world.GetClosestPlayerSeen((EntityAlive) this, -1f, 0.0f);
      if (!Object.op_Implicit((Object) target))
        target = this.world.GetClosestPlayer((Entity) this, -1f, false);
      return target;
    }
    EntityPlayer target1 = this.world.GetClosestPlayerSeen((EntityAlive) this, 80f, 26f);
    if (!Object.op_Implicit((Object) target1) || (double) target1.inWaterPercent >= 0.60000002384185791)
      target1 = this.noisePlayer;
    if (Object.op_Implicit((Object) target1))
    {
      if (this.isBattleFatigued)
        return (EntityPlayer) null;
      float num = (float) target1.Health / target1.Stats.Health.ModifiedMax;
      if (this.IsSleeper || (double) num <= (double) this.targetAttackHealthPercent)
        return target1;
    }
    return (EntityPlayer) null;
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public override void fallHitGround(float _v, Vector3 _fallMotion)
  {
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public override bool isDetailedHeadBodyColliders() => true;

  [PublicizedFrom(EAccessModifier.Protected)]
  public override bool isRadiationSensitive() => false;

  public override float GetEyeHeight() => 0.3f;

  public override Vector3 GetLookVector()
  {
    return ((Vector3) ref this.lookAtPosition).Equals(Vector3.zero) ? base.GetLookVector() : Vector3.op_Subtraction(this.lookAtPosition, this.getHeadPosition());
  }

  public override bool CanDamageEntity(int _sourceEntityId)
  {
    Entity entity = this.world.GetEntity(_sourceEntityId);
    return !Object.op_Implicit((Object) entity) || entity.entityClass != this.entityClass;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void AttackAndAdjust(bool isBlock)
  {
    if (!this.UseHoldingItem(0, false))
      return;
    this.UseHoldingItem(0, true);
    this.attackDelay = 18;
    this.isCircling = false;
    if (Object.op_Implicit((Object) this.currentTarget.AttachedToEntity))
      this.motion = Vector3.op_Multiply(this.motion, 0.7f);
    else
      this.motion = Vector3.op_Multiply(this.motion, 0.6f);
    if (++this.attackCount < 5 && (double) this.rand.RandomFloat >= 0.25)
      return;
    this.StartAttackReposition();
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public bool IsCourseTraversable(Vector3 _pos, out float _distance)
  {
    float num1 = _pos.x - this.position.x;
    float num2 = _pos.y - this.position.y;
    float num3 = _pos.z - this.position.z;
    _distance = Mathf.Sqrt((float) ((double) num1 * (double) num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3));
    if ((double) _distance < 1.5)
      return true;
    float num4 = num1 / _distance;
    float num5 = num2 / _distance;
    float num6 = num3 / _distance;
    Bounds boundingBox = this.boundingBox;
    this.collBB.Clear();
    for (int index = 1; (double) index < (double) _distance - 1.0; ++index)
    {
      ref Bounds local = ref boundingBox;
      ((Bounds) ref local).center = Vector3.op_Addition(((Bounds) ref local).center, new Vector3(num4, num5, num6));
      this.world.GetCollidingBounds((Entity) this, boundingBox, this.collBB);
      if (this.collBB.Count > 0)
        return false;
    }
    return true;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  static EntityVulture()
  {
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public enum State
  {
    Attack,
    AttackReposition,
    AttackStop,
    Home,
    Stun,
    WanderStart,
    Wander,
  }
}
