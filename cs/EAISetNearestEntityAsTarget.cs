// Decompiled with JetBrains decompiler
// Type: EAISetNearestEntityAsTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
[Preserve]
public class EAISetNearestEntityAsTarget : EAITarget
{
  [PublicizedFrom(EAccessModifier.Private)]
  public const float cHearDistMax = 50f;
  [PublicizedFrom(EAccessModifier.Private)]
  public List<EAISetNearestEntityAsTarget.TargetClass> targetClasses;
  [PublicizedFrom(EAccessModifier.Private)]
  public int playerTargetClassIndex = -1;
  [PublicizedFrom(EAccessModifier.Private)]
  public float closeTargetDist;
  [PublicizedFrom(EAccessModifier.Private)]
  public EntityAlive closeTargetEntity;
  [PublicizedFrom(EAccessModifier.Private)]
  public EntityAlive targetEntity;
  [PublicizedFrom(EAccessModifier.Private)]
  public EntityPlayer targetPlayer;
  [PublicizedFrom(EAccessModifier.Private)]
  public Vector3 lastSeenPos;
  [PublicizedFrom(EAccessModifier.Private)]
  public float findTime;
  [PublicizedFrom(EAccessModifier.Private)]
  public float senseSoundTime;
  [PublicizedFrom(EAccessModifier.Private)]
  public EAISetNearestEntityAsTargetSorter sorter;
  [PublicizedFrom(EAccessModifier.Private)]
  public static List<Entity> list = new List<Entity>();

  public override void Init(EntityAlive _theEntity)
  {
    this.Init(_theEntity, 25f, true);
    this.MutexBits = 1;
    this.sorter = new EAISetNearestEntityAsTargetSorter((Entity) _theEntity);
  }

  public override void SetData(DictionarySave<string, string> data)
  {
    base.SetData(data);
    this.targetClasses = new List<EAISetNearestEntityAsTarget.TargetClass>();
    string str;
    if (!data.TryGetValue("class", out str))
      return;
    string[] strArray = str.Split(',', StringSplitOptions.None);
    for (int index = 0; index < strArray.Length; index += 3)
    {
      EAISetNearestEntityAsTarget.TargetClass targetClass;
      targetClass.type = EntityFactory.GetEntityType(strArray[index]);
      targetClass.hearDistMax = 0.0f;
      if (index + 1 < strArray.Length)
        targetClass.hearDistMax = StringParsers.ParseFloat(strArray[index + 1]);
      if ((double) targetClass.hearDistMax == 0.0)
        targetClass.hearDistMax = 50f;
      targetClass.seeDistMax = 0.0f;
      if (index + 2 < strArray.Length)
        targetClass.seeDistMax = StringParsers.ParseFloat(strArray[index + 2]);
      if (targetClass.type == typeof (EntityPlayer))
        this.playerTargetClassIndex = this.targetClasses.Count;
      this.targetClasses.Add(targetClass);
    }
  }

  public void SetTargetOnlyPlayers(float _distance)
  {
    this.targetClasses.Clear();
    this.targetClasses.Add(new EAISetNearestEntityAsTarget.TargetClass()
    {
      type = typeof (EntityPlayer),
      hearDistMax = _distance,
      seeDistMax = -_distance
    });
    this.playerTargetClassIndex = 0;
  }

  public override bool CanExecute()
  {
    if (Object.op_Inequality((Object) this.theEntity.distraction, (Object) null))
      return false;
    this.FindTarget();
    if (!Object.op_Implicit((Object) this.closeTargetEntity))
      return false;
    this.targetEntity = this.closeTargetEntity;
    this.targetPlayer = this.closeTargetEntity as EntityPlayer;
    return true;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void FindTarget()
  {
    this.closeTargetDist = float.MaxValue;
    this.closeTargetEntity = (EntityAlive) null;
    float seeDistance = this.theEntity.GetSeeDistance();
    for (int index1 = 0; index1 < this.targetClasses.Count; ++index1)
    {
      EAISetNearestEntityAsTarget.TargetClass targetClass = this.targetClasses[index1];
      float num = seeDistance;
      if ((double) targetClass.seeDistMax != 0.0)
      {
        float v2 = (double) targetClass.seeDistMax < 0.0 ? -targetClass.seeDistMax : targetClass.seeDistMax * this.theEntity.senseScale;
        num = Utils.FastMin(num, v2);
      }
      if (targetClass.type == typeof (EntityPlayer))
      {
        this.FindTargetPlayer(num);
        if (Object.op_Implicit((Object) this.theEntity.noisePlayer) && Object.op_Inequality((Object) this.theEntity.noisePlayer, (Object) this.closeTargetEntity))
        {
          if (Object.op_Implicit((Object) this.closeTargetEntity))
          {
            if ((double) this.theEntity.noisePlayerVolume >= (double) this.theEntity.sleeperNoiseToWake)
            {
              Vector3 vector3 = Vector3.op_Subtraction(this.theEntity.position, this.theEntity.noisePlayer.position);
              float magnitude = ((Vector3) ref vector3).magnitude;
              if ((double) magnitude < (double) this.closeTargetDist)
              {
                this.closeTargetDist = magnitude;
                this.closeTargetEntity = (EntityAlive) this.theEntity.noisePlayer;
              }
            }
          }
          else if (!this.theEntity.IsSleeping)
            this.SeekNoise(this.theEntity.noisePlayer);
        }
        if (Object.op_Implicit((Object) this.closeTargetEntity))
        {
          EntityPlayer closeTargetEntity = (EntityPlayer) this.closeTargetEntity;
          if (closeTargetEntity.IsBloodMoonDead && (double) closeTargetEntity.currentLife >= 0.5)
          {
            Log.Out("Player {0}, living {1}, lost BM immunity", new object[2]
            {
              (object) closeTargetEntity.GetDebugName(),
              (object) (float) ((double) closeTargetEntity.currentLife * 60.0)
            });
            closeTargetEntity.IsBloodMoonDead = false;
          }
        }
      }
      else if (!this.theEntity.IsSleeping && !this.theEntity.HasInvestigatePosition)
      {
        this.theEntity.world.GetEntitiesInBounds(targetClass.type, BoundsUtils.ExpandBounds(this.theEntity.boundingBox, num, 4f, num), EAISetNearestEntityAsTarget.list);
        EAISetNearestEntityAsTarget.list.Sort((IComparer<Entity>) this.sorter);
        for (int index2 = 0; index2 < EAISetNearestEntityAsTarget.list.Count; ++index2)
        {
          EntityAlive entityAlive = (EntityAlive) EAISetNearestEntityAsTarget.list[index2];
          if (!(entityAlive is EntityDrone) && this.check(entityAlive))
          {
            float distance = this.theEntity.GetDistance((Entity) entityAlive);
            if ((double) distance < (double) this.closeTargetDist)
            {
              this.closeTargetDist = distance;
              this.closeTargetEntity = entityAlive;
              this.lastSeenPos = entityAlive.position;
              break;
            }
            break;
          }
        }
        EAISetNearestEntityAsTarget.list.Clear();
      }
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void SeekNoise(EntityPlayer player)
  {
    Vector3 vector3 = Vector3.op_Subtraction(player.position, this.theEntity.position);
    float magnitude = ((Vector3) ref vector3).magnitude;
    if (this.playerTargetClassIndex >= 0)
    {
      float num = this.targetClasses[this.playerTargetClassIndex].hearDistMax * this.theEntity.senseScale * player.DetectUsScale(this.theEntity);
      if ((double) magnitude > (double) num)
        return;
    }
    float num1 = magnitude * 0.9f;
    if ((double) num1 > (double) this.manager.noiseSeekDist)
      num1 = this.manager.noiseSeekDist;
    if (this.theEntity.IsBloodMoon)
      num1 = this.manager.noiseSeekDist * 0.25f;
    this.theEntity.SetInvestigatePosition(player.GetBreadcrumbPos(num1 * this.RandomFloat), this.theEntity.CalcInvestigateTicks((int) (30.0 + (double) this.RandomFloat * 30.0) * 20, (EntityAlive) player));
    this.PlaySoundSenseNoise();
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void PlaySoundSenseNoise()
  {
    float time = Time.time;
    if ((double) this.senseSoundTime - (double) time >= 0.0)
      return;
    this.senseSoundTime = (float) ((double) time + 10.0 + (double) this.RandomFloat * 10.0);
    this.theEntity.PlayOneShot(this.theEntity.soundSense);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void FindTargetPlayer(float seeDist)
  {
    if (this.theEntity.IsSleeperPassive)
      return;
    this.theEntity.world.GetEntitiesInBounds(typeof (EntityPlayer), BoundsUtils.ExpandBounds(this.theEntity.boundingBox, seeDist, seeDist, seeDist), EAISetNearestEntityAsTarget.list);
    if (this.theEntity.IsSleeping)
    {
      EAISetNearestEntityAsTarget.list.Sort((IComparer<Entity>) this.sorter);
      EntityPlayer entityPlayer = (EntityPlayer) null;
      float num = float.MaxValue;
      bool flag = false;
      if (Object.op_Inequality((Object) this.theEntity.noisePlayer, (Object) null))
      {
        if ((double) this.theEntity.noisePlayerVolume >= (double) this.theEntity.sleeperNoiseToWake)
        {
          entityPlayer = this.theEntity.noisePlayer;
          num = this.theEntity.noisePlayerDistance;
        }
        else if ((double) this.theEntity.noisePlayerVolume >= (double) this.theEntity.sleeperNoiseToSense)
          flag = true;
      }
      for (int index = 0; index < EAISetNearestEntityAsTarget.list.Count; ++index)
      {
        EntityPlayer _other = (EntityPlayer) EAISetNearestEntityAsTarget.list[index];
        if (this.theEntity.CanSee((EntityAlive) _other) && !_other.IsIgnoredByAI())
        {
          float distance = this.theEntity.GetDistance((Entity) _other);
          int sleeperDisturbedLevel = this.theEntity.GetSleeperDisturbedLevel(distance, _other.Stealth.lightLevel);
          if (sleeperDisturbedLevel >= 2)
          {
            if ((double) distance < (double) num)
            {
              entityPlayer = _other;
              num = distance;
            }
          }
          else if (sleeperDisturbedLevel >= 1)
            flag = true;
        }
      }
      EAISetNearestEntityAsTarget.list.Clear();
      if (Object.op_Inequality((Object) entityPlayer, (Object) null))
      {
        this.closeTargetDist = num;
        this.closeTargetEntity = (EntityAlive) entityPlayer;
      }
      else if (flag)
        this.theEntity.Groan();
      else
        this.theEntity.Snore();
    }
    else
    {
      for (int index = 0; index < EAISetNearestEntityAsTarget.list.Count; ++index)
      {
        EntityPlayer entityPlayer = (EntityPlayer) EAISetNearestEntityAsTarget.list[index];
        if (entityPlayer.IsAlive() && !entityPlayer.IsIgnoredByAI())
        {
          float seeDistance = this.manager.GetSeeDistance((Entity) entityPlayer);
          if ((double) seeDistance < (double) this.closeTargetDist && this.theEntity.CanSee((EntityAlive) entityPlayer) && this.theEntity.CanSeeStealth(seeDistance, entityPlayer.Stealth.lightLevel))
          {
            this.closeTargetDist = seeDistance;
            this.closeTargetEntity = (EntityAlive) entityPlayer;
          }
        }
      }
      EAISetNearestEntityAsTarget.list.Clear();
    }
  }

  public override void Start()
  {
    this.theEntity.SetAttackTarget(this.targetEntity, 200);
    this.theEntity.ConditionalTriggerSleeperWakeUp();
    this.PlaySoundSenseNoise();
    base.Start();
  }

  public override bool Continue()
  {
    if (this.targetEntity.IsDead() || Object.op_Inequality((Object) this.theEntity.distraction, (Object) null))
    {
      if (Object.op_Equality((Object) this.theEntity.GetAttackTarget(), (Object) this.targetEntity))
        this.theEntity.SetAttackTarget((EntityAlive) null, 0);
      return false;
    }
    this.findTime += 0.05f;
    if ((double) this.findTime > 2.0)
    {
      this.findTime = 0.0f;
      this.FindTarget();
      if (Object.op_Implicit((Object) this.closeTargetEntity) && Object.op_Inequality((Object) this.closeTargetEntity, (Object) this.targetEntity))
        return false;
    }
    if (Object.op_Inequality((Object) this.theEntity.GetAttackTarget(), (Object) this.targetEntity))
      return false;
    if (this.check(this.targetEntity) && (Object.op_Equality((Object) this.targetPlayer, (Object) null) || this.theEntity.CanSeeStealth(this.manager.GetSeeDistance((Entity) this.targetEntity), this.targetPlayer.Stealth.lightLevel)))
    {
      this.theEntity.SetAttackTarget(this.targetEntity, 600);
      this.lastSeenPos = this.targetEntity.position;
      return true;
    }
    if ((double) this.theEntity.GetDistanceSq(this.lastSeenPos) < 2.25)
      this.lastSeenPos = Vector3.zero;
    this.theEntity.SetAttackTarget((EntityAlive) null, 0);
    int ticks = this.theEntity.CalcInvestigateTicks(Constants.cEnemySenseMemory * 20, this.targetEntity);
    if (Vector3.op_Inequality(this.lastSeenPos, Vector3.zero))
      this.theEntity.SetInvestigatePosition(this.lastSeenPos, ticks);
    return false;
  }

  public override void Reset()
  {
    this.targetEntity = (EntityAlive) null;
    this.targetPlayer = (EntityPlayer) null;
  }

  public override string ToString()
  {
    return $"{base.ToString()}, {(Object.op_Implicit((Object) this.targetEntity) ? (object) this.targetEntity.EntityName : (object) "")}";
  }

  [PublicizedFrom(EAccessModifier.Private)]
  static EAISetNearestEntityAsTarget()
  {
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public struct TargetClass
  {
    public System.Type type;
    public float hearDistMax;
    public float seeDistMax;
  }
}
