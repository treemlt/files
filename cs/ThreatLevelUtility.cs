// Decompiled with JetBrains decompiler
// Type: DynamicMusic.ThreatLevelUtility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using MusicUtils.Enums;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
namespace DynamicMusic;

public static class ThreatLevelUtility
{
  [PublicizedFrom(EAccessModifier.Private)]
  public const int ZOMBIE_COMBAT_QUANTITY = 4;
  [PublicizedFrom(EAccessModifier.Private)]
  public const float PLAYER_HOME_MINIMUM_DISTANCE = 50f;
  [PublicizedFrom(EAccessModifier.Private)]
  public const float THREAT_PER_ENEMY = 0.0333333351f;
  [PublicizedFrom(EAccessModifier.Private)]
  public static List<Entity> enemies = new List<Entity>();
  [PublicizedFrom(EAccessModifier.Private)]
  public static Vector3 boundingBoxRange = new Vector3(50f, 50f, 50f);
  public static int Zombies;
  public static int Targeting;
  [PublicizedFrom(EAccessModifier.Private)]
  public static Queue<float> threatLevels = new Queue<float>();
  [PublicizedFrom(EAccessModifier.Private)]
  public const int LOOKBACK = 300;

  public static float GetThreatLevelOn(EntityPlayerLocal _player)
  {
    GameManager.Instance.World.GetEntitiesInBounds(typeof (EntityEnemy), new Bounds(_player.position, ThreatLevelUtility.boundingBoxRange), ThreatLevelUtility.enemies);
    float num1 = 0.0f;
    int num2 = ThreatLevelUtility.Zombies = ThreatLevelUtility.zombiesContributingThreat();
    int num3 = ThreatLevelUtility.Targeting = ThreatLevelUtility.EnemiesTargeting();
    if (num3 > 0)
      _player.LastTargetEventTime = Time.time;
    float threatLevelOn;
    if (num2 >= 4 && num3 > 0 || _player.ThreatLevel.Category == ThreatLevelType.Panicked && num2 > 0 && (double) Time.time - (double) _player.LastTargetEventTime < 15.0 || GameUtils.IsBloodMoonTime(GameManager.Instance.World.worldTime, GameUtils.CalcDuskDawnHours(GameStats.GetInt(EnumGameStats.DayLightLength)), GameStats.GetInt(EnumGameStats.BloodMoonDay)))
    {
      int num4 = GamePrefs.GetInt(EnumGamePrefs.BloodMoonEnemyCount) * (_player.Party != null ? _player.Party.MemberList.Count : 1);
      float num5 = 0.3f / (float) num4;
      float num6 = 0.15f / (float) num4;
      float num7 = MathUtils.Clamp((float) (0.699999988079071 + (double) num5 * (double) num3 + (double) num6 * (double) (num2 - num3)), 0.7f, 1f);
      ThreatLevelUtility.threatLevels.Enqueue(num7);
      if (ThreatLevelUtility.threatLevels.Count > 300)
      {
        double num8 = (double) ThreatLevelUtility.threatLevels.Dequeue();
      }
      threatLevelOn = MathUtils.Clamp(ThreatLevelUtility.threatLevels.Average(), 0.7f, 1f);
    }
    else
      threatLevelOn = (float) ((double) num1 + (GameManager.Instance.World.IsDark() ? 0.10000000149011612 : 0.0) + (ThreatLevelUtility.isPlayerInUnclearedPOI(_player) ? 0.20000000298023224 : 0.0) + (ThreatLevelUtility.IsPlayerHome(_player) ? 0.0 : 0.20000000298023224) + (ThreatLevelUtility.IsPlayerInSpookyBiome(_player) ? 0.10000000149011612 : 0.0)) + (float) num2 * 0.0333333351f;
    ThreatLevelUtility.enemies.Clear();
    return threatLevelOn;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static bool isPlayerInUnclearedPOI(EntityPlayerLocal _player)
  {
    if ((double) _player.PlayerStats.LightInsidePer > 0.20000000298023224)
    {
      if (GamePrefs.GetString(EnumGamePrefs.GameWorld).Equals("Playtesting"))
        return true;
      if (_player.prefab != null)
      {
        foreach (SleeperVolume sleeperVolume in _player.prefab.sleeperVolumes)
        {
          if (!sleeperVolume.wasCleared)
            return true;
        }
      }
    }
    return false;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static int zombiesContributingThreat()
  {
    int num = 0;
    for (int index = 0; index < ThreatLevelUtility.enemies.Count; ++index)
    {
      EntityEnemy enemy = ThreatLevelUtility.enemies[index] as EntityEnemy;
      if (enemy.IsAlive() && !enemy.IsSleeping)
        ++num;
    }
    return num;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static int EnemiesTargeting()
  {
    int num = 0;
    for (int index = 0; index < ThreatLevelUtility.enemies.Count; ++index)
    {
      EntityEnemy enemy = ThreatLevelUtility.enemies[index] as EntityEnemy;
      if (Object.op_Inequality((Object) enemy, (Object) null) && enemy.IsAlive() && Object.op_Inequality((Object) (enemy.GetAttackTargetLocal() as EntityPlayer), (Object) null))
        ++num;
    }
    return num;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static bool IsPlayerHome(EntityPlayerLocal _player)
  {
    SpawnPosition spawnPoint = _player.GetSpawnPoint();
    if (spawnPoint.IsUndef())
      return false;
    Vector3 vector3 = Vector3.op_Subtraction(spawnPoint.position, _player.position);
    return (double) ((Vector3) ref vector3).magnitude <= 50.0;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static bool IsPlayerInSpookyBiome(EntityPlayerLocal _player)
  {
    if (_player.biomeStandingOn == null)
      return false;
    return _player.biomeStandingOn.m_sBiomeName.Equals("burnt_forest") || _player.biomeStandingOn.m_sBiomeName.Equals("wasteland");
  }

  [PublicizedFrom(EAccessModifier.Private)]
  static ThreatLevelUtility()
  {
  }
}
