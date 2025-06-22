// Decompiled with JetBrains decompiler
// Type: PlayerStealth
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#nullable disable
public struct PlayerStealth
{
  public const float cLightLevelMax = 200f;
  [PublicizedFrom(EAccessModifier.Private)]
  public const float cLightMpyBase = 0.32f;
  [PublicizedFrom(EAccessModifier.Private)]
  public const int cVersion = 3;
  [PublicizedFrom(EAccessModifier.Private)]
  public const float cNextSoundPercent = 0.6f;
  [PublicizedFrom(EAccessModifier.Private)]
  public const float cSleeperNoiseDecay = 50f;
  [PublicizedFrom(EAccessModifier.Private)]
  public const float cSleeperNoiseHear = 360f;
  [PublicizedFrom(EAccessModifier.Private)]
  public const int cSleeperNoiseWaitTicks = 20;
  public float lightLevel;
  [PublicizedFrom(EAccessModifier.Private)]
  public float lightAttackPercent;
  public float noiseVolume;
  public int smell;
  [PublicizedFrom(EAccessModifier.Private)]
  public float speedAverage;
  [PublicizedFrom(EAccessModifier.Private)]
  public EntityPlayer player;
  [PublicizedFrom(EAccessModifier.Private)]
  public int sendTickDelay;
  [PublicizedFrom(EAccessModifier.Private)]
  public int lightLevelSent;
  [PublicizedFrom(EAccessModifier.Private)]
  public int noiseVolumeSent;
  [PublicizedFrom(EAccessModifier.Private)]
  public bool alertEnemySent;
  [PublicizedFrom(EAccessModifier.Private)]
  public int sleeperNoiseWaitTicks;
  [PublicizedFrom(EAccessModifier.Private)]
  public float sleeperNoiseVolume;
  [PublicizedFrom(EAccessModifier.Private)]
  public List<PlayerStealth.NoiseData> noises;
  [PublicizedFrom(EAccessModifier.Private)]
  public int alertEnemiesTicks;
  [PublicizedFrom(EAccessModifier.Private)]
  public bool alertEnemy;
  [PublicizedFrom(EAccessModifier.Private)]
  public Color32 barColorUI;
  [PublicizedFrom(EAccessModifier.Private)]
  public static List<Entity> entityTempList = new List<Entity>();

  public void Init(EntityPlayer _player)
  {
    this.player = _player;
    this.noises = new List<PlayerStealth.NoiseData>();
    this.barColorUI = new Color32((byte) 0, (byte) 0, (byte) 0, byte.MaxValue);
  }

  public void Tick()
  {
    float d = (float) ((double) this.player.speedForward * (double) this.player.speedForward + (double) this.player.speedStrafe * (double) this.player.speedStrafe);
    if ((double) d > 0.0099999997764825821)
      this.speedAverage = Utils.FastLerpUnclamped(this.speedAverage, (float) Math.Sqrt((double) d), 0.2f);
    else
      this.speedAverage *= 0.5f;
    float selfLight;
    float stealthLightLevel = LightManager.GetStealthLightLevel((EntityAlive) this.player, out selfLight);
    float num1 = Utils.FastClamp(selfLight / (stealthLightLevel + 0.05f), 0.5f, 3.2f);
    float num2 = stealthLightLevel + selfLight * num1;
    if (this.player.IsCrouching)
      num2 *= 0.6f;
    this.player.Buffs.SetCustomVar("_lightlevel", num2 * 100f);
    float num3 = num2 * (float) (1.0 + (double) this.speedAverage * 0.15000000596046448);
    float num4 = EffectManager.GetValue(PassiveEffects.LightMultiplier, _originalValue: 1f, _entity: (EntityAlive) this.player);
    this.lightAttackPercent = (double) selfLight < 0.10000000149011612 ? num4 : 1f;
    float num5 = (float) (0.31999999284744263 + 0.68000000715255737 * (double) num4);
    this.lightLevel = Utils.FastClamp((float) ((double) num3 * (double) num5 * 100.0), 0.0f, 200f);
    this.ProcNoiseCleanup();
    float num6 = this.CalcVolume();
    this.player.Buffs.SetCustomVar("_noiselevel", this.noiseVolume);
    if (--this.sleeperNoiseWaitTicks <= 0)
    {
      this.sleeperNoiseVolume -= 2.5f;
      if ((double) this.sleeperNoiseVolume < 0.0)
        this.sleeperNoiseVolume = 0.0f;
    }
    if ((double) num6 > 0.0)
    {
      float num7 = num6 * 1.2f;
      float num8 = EAIManager.CalcSenseScale();
      float num9 = num7 * (float) (1.0 + (double) num8 * 1.6000000238418579);
      float num10 = (float) (75.0 + 25.0 * (double) num8);
      if ((double) num9 > (double) num10)
        num9 = num10;
      Bounds _bb;
      // ISSUE: explicit constructor call
      ((Bounds) ref _bb).\u002Ector(this.player.position, new Vector3(num9, num9, num9));
      this.player.world.GetEntitiesInBounds(typeof (EntityEnemy), _bb, PlayerStealth.entityTempList);
      for (int index = 0; index < PlayerStealth.entityTempList.Count; ++index)
      {
        EntityAlive entityTemp = (EntityAlive) PlayerStealth.entityTempList[index];
        float distance = this.player.GetDistance((Entity) entityTemp);
        float num11 = this.noiseVolume * (float) (1.0 + (double) num8 * (double) entityTemp.aiManager.feralSense) / (float) ((double) distance * 0.60000002384185791 + 0.40000000596046448) * this.player.DetectUsScale(entityTemp);
        if ((double) num11 >= 1.0)
        {
          bool flag = true;
          if (Object.op_Implicit((Object) entityTemp.noisePlayer))
            flag = (double) num11 > (double) entityTemp.noisePlayerVolume;
          if (flag)
          {
            entityTemp.noisePlayer = this.player;
            entityTemp.noisePlayerDistance = distance;
            entityTemp.noisePlayerVolume = num11;
          }
        }
      }
      PlayerStealth.entityTempList.Clear();
    }
    if (--this.alertEnemiesTicks <= 0)
    {
      this.alertEnemiesTicks = 20;
      this.alertEnemy = false;
      this.player.world.GetEntitiesAround(EntityFlags.Zombie | EntityFlags.Animal | EntityFlags.Bandit, this.player.position, 12f, PlayerStealth.entityTempList);
      for (int index = 0; index < PlayerStealth.entityTempList.Count; ++index)
      {
        if (((EntityAlive) PlayerStealth.entityTempList[index]).IsAlert)
        {
          this.alertEnemy = true;
          break;
        }
      }
      PlayerStealth.entityTempList.Clear();
      this.SetBarColor(this.alertEnemy);
    }
    if (!this.player.isEntityRemote)
      return;
    if (this.sendTickDelay > 0)
      --this.sendTickDelay;
    if ((!this.player.IsCrouching || this.sendTickDelay != 0 || this.lightLevelSent == (int) this.lightLevel && this.noiseVolumeSent == (int) this.noiseVolume) && this.alertEnemySent == this.alertEnemy)
      return;
    this.sendTickDelay = 16 /*0x10*/;
    this.lightLevelSent = (int) this.lightLevel;
    this.noiseVolumeSent = (int) this.noiseVolume;
    this.alertEnemySent = this.alertEnemy;
    SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageEntityStealth>().Setup(this.player, this.lightLevelSent, this.noiseVolumeSent, this.alertEnemySent), _attachedToEntityId: this.player.entityId);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void SetBarColor(bool _isAlert)
  {
    this.barColorUI.r = (byte) 50;
    this.barColorUI.g = (byte) 135;
    if (!_isAlert)
      return;
    this.barColorUI.r = (byte) 180;
    this.barColorUI.g = (byte) 180;
  }

  public void ProcNoiseCleanup()
  {
    for (int index = 0; index < this.noises.Count; ++index)
    {
      PlayerStealth.NoiseData noise = this.noises[index];
      if (noise.ticks > 1)
      {
        --noise.ticks;
        this.noises[index] = noise;
      }
      else
      {
        this.noises.RemoveAt(index);
        --index;
      }
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public float CalcVolume()
  {
    float num1 = 0.0f;
    float num2 = 1f;
    for (int index = 0; index < this.noises.Count; ++index)
    {
      num1 += this.noises[index].volume * num2;
      num2 *= 0.6f;
    }
    this.noiseVolume = Mathf.Pow(num1 * 2.35f, 0.86f);
    this.noiseVolume *= 1.5f;
    this.noiseVolume *= EffectManager.GetValue(PassiveEffects.NoiseMultiplier, _originalValue: 1f, _entity: (EntityAlive) this.player);
    return num1;
  }

  public bool CanSleeperAttackDetect(EntityAlive _e)
  {
    if (this.player.IsCrouching)
    {
      float num = Utils.FastLerp(3f, 15f, this.lightAttackPercent);
      if ((double) _e.GetDistance((Entity) this.player) > (double) num)
        return false;
    }
    return true;
  }

  public void SetClientLevels(float _lightLevel, float _noiseVolume, bool _isAlert)
  {
    this.lightLevel = _lightLevel;
    this.noiseVolume = _noiseVolume;
    this.alertEnemy = _isAlert;
    this.SetBarColor(_isAlert);
  }

  public bool NotifyNoise(float volume, float duration)
  {
    if ((double) volume <= 0.0)
      return false;
    this.AddNoise(this.noises, volume, (int) ((double) duration * 20.0));
    if ((double) volume >= 11.0)
      this.sleeperNoiseWaitTicks = 20;
    float num = volume;
    if ((double) volume > 60.0)
      num = 60f + Mathf.Pow(volume - 60f, 1.4f);
    this.sleeperNoiseVolume += num * EffectManager.GetValue(PassiveEffects.NoiseMultiplier, _originalValue: 1f, _entity: (EntityAlive) this.player);
    if ((double) this.sleeperNoiseVolume < 360.0)
      return false;
    this.sleeperNoiseVolume = 360f;
    return true;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void AddNoise(List<PlayerStealth.NoiseData> list, float volume, int ticks)
  {
    PlayerStealth.NoiseData noiseData = new PlayerStealth.NoiseData(volume, ticks);
    for (int index = 0; index < list.Count; ++index)
    {
      PlayerStealth.NoiseData noise = this.noises[index];
      if ((double) volume >= (double) noise.volume)
      {
        list.Insert(index, noiseData);
        return;
      }
    }
    list.Insert(list.Count, noiseData);
  }

  public static PlayerStealth Read(EntityPlayer _player, BinaryReader br)
  {
    int num1 = br.ReadInt32();
    PlayerStealth playerStealth = new PlayerStealth();
    playerStealth.Init(_player);
    playerStealth.lightLevel = (float) br.ReadInt32();
    int num2 = br.ReadInt32();
    if (num2 > 0)
    {
      if (num1 >= 3)
      {
        for (int index = 0; index < num2; ++index)
        {
          double num3 = (double) br.ReadSingle();
          float volume = br.ReadSingle();
          int ticks = br.ReadInt32();
          playerStealth.AddNoise(playerStealth.noises, volume, ticks);
        }
      }
      else if (num1 >= 2)
      {
        for (int index = 0; index < num2; ++index)
        {
          double num4 = (double) br.ReadSingle();
          double num5 = (double) br.ReadSingle();
          br.ReadInt32();
        }
      }
      else
      {
        for (int index = 0; index < num2; ++index)
        {
          br.ReadInt32();
          br.ReadInt32();
        }
      }
    }
    return playerStealth;
  }

  public void Write(BinaryWriter bw)
  {
    bw.Write(3);
    bw.Write(this.lightLevel);
    bw.Write(this.noises != null ? this.noises.Count : 0);
    if (this.noises == null)
      return;
    for (int index = 0; index < this.noises.Count; ++index)
    {
      PlayerStealth.NoiseData noise = this.noises[index];
      bw.Write(0.0f);
      bw.Write(noise.volume);
      bw.Write(noise.ticks);
    }
  }

  public Color32 ValueColorUI => this.barColorUI;

  public float ValuePercentUI
  {
    get
    {
      return Utils.FastClamp01((float) (((double) this.lightLevel + (double) this.noiseVolume * 0.5 + (this.alertEnemy ? 5.0 : 0.0)) * 0.0099999997764825821 + 0.004999999888241291));
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  static PlayerStealth()
  {
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public struct NoiseData(float _volume, int _ticks)
  {
    public float volume = _volume;
    public int ticks = _ticks;
  }
}
