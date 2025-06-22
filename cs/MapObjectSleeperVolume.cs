// Decompiled with JetBrains decompiler
// Type: MapObjectSleeperVolume
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MapObjectSleeperVolume(Vector3 _position) : MapObject(EnumMapObjectType.SleeperVolume, _position, (long) ++MapObjectSleeperVolume.newID, (Entity) null, false)
{
  public bool IsShowing = true;
  [PublicizedFrom(EAccessModifier.Private)]
  public static int newID;

  public override string GetMapIcon() => "ui_game_symbol_enemy_dot";

  public override string GetCompassIcon() => "ui_game_symbol_enemy_dot";

  public override string GetCompassDownIcon() => "ui_game_symbol_enemy_dot_down";

  public override string GetCompassUpIcon() => "ui_game_symbol_enemy_dot_up";

  public override bool UseUpDownCompassIcons() => true;

  public override bool IsOnCompass() => this.IsShowing;

  public override bool IsCompassIconClamped() => true;

  public override bool IsMapIconEnabled() => false;

  public override float GetMaxCompassIconScale() => 1f;

  public override float GetMinCompassIconScale() => 0.6f;

  public override float GetMaxCompassDistance() => 32f;

  public override Color GetMapIconColor()
  {
    return Color32.op_Implicit(new Color32(byte.MaxValue, (byte) 180, (byte) 0, byte.MaxValue));
  }
}
