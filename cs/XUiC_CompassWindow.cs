// Decompiled with JetBrains decompiler
// Type: XUiC_CompassWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
[Preserve]
public class XUiC_CompassWindow : XUiController
{
  public static string ID = "";
  [PublicizedFrom(EAccessModifier.Private)]
  public EntityPlayerLocal localPlayer;
  public List<UISprite> waypointSpriteList = new List<UISprite>();
  [PublicizedFrom(EAccessModifier.Private)]
  public readonly CachedStringFormatter<ulong> daytimeFormatter = new CachedStringFormatter<ulong>((Func<ulong, string>) ([PublicizedFrom(EAccessModifier.Internal)] (_worldTime) => ValueDisplayFormatters.WorldTime(_worldTime, Localization.Get("xuiDayTimeLong"))));
  [PublicizedFrom(EAccessModifier.Private)]
  public readonly CachedStringFormatterInt dayFormatter = new CachedStringFormatterInt();
  [PublicizedFrom(EAccessModifier.Private)]
  public readonly CachedStringFormatter<int, int> timeFormatter = new CachedStringFormatter<int, int>((Func<int, int, string>) ([PublicizedFrom(EAccessModifier.Internal)] (_hour, _min) => $"{_hour:00}:{_min:00}"));
  [PublicizedFrom(EAccessModifier.Private)]
  public bool showSleeperVolumes;

  public override void Init()
  {
    base.Init();
    XUiC_CompassWindow.ID = this.WindowGroup.ID;
    for (int index = 0; index < 50; ++index)
    {
      UISprite uiSprite = new GameObject()
      {
        transform = {
          parent = this.ViewComponent.UiTransform
        }
      }.AddComponent<UISprite>();
      this.waypointSpriteList.Add(uiSprite);
      uiSprite.atlas = (INGUIAtlas) this.xui.GetAtlasByName("UIAtlas", "menu_empty");
      ((Component) uiSprite).transform.localScale = Vector3.one;
      uiSprite.spriteName = "menu_empty";
      ((UIWidget) uiSprite).SetDimensions(20, 20);
      ((UIWidget) uiSprite).color = Color.clear;
      ((UIWidget) uiSprite).pivot = (UIWidget.Pivot) 4;
      ((UIWidget) uiSprite).depth = 12;
      ((Component) uiSprite).gameObject.layer = 12;
    }
  }

  public override void Update(float _dt)
  {
    base.Update(_dt);
    if (!Object.op_Implicit((Object) this.localPlayer))
    {
      this.localPlayer = this.xui.playerUI.entityPlayer;
      if (!Object.op_Implicit((Object) this.localPlayer))
        return;
    }
    this.ViewComponent.IsVisible = !this.localPlayer.IsDead() && this.xui.playerUI.windowManager.IsHUDEnabled();
    if (Object.op_Inequality((Object) this.localPlayer.playerCamera, (Object) null))
    {
      World world = GameManager.Instance.World;
      this.showSleeperVolumes = true;
      int waypointSpriteIndex = 0;
      this.updateNavObjects(this.localPlayer, ref waypointSpriteIndex);
      this.updateMarkers(this.localPlayer, ref waypointSpriteIndex, world.GetObjectOnMapList(EnumMapObjectType.SleepingBag));
      this.updateMarkers(this.localPlayer, ref waypointSpriteIndex, world.GetObjectOnMapList(EnumMapObjectType.LandClaim));
      this.updateMarkers(this.localPlayer, ref waypointSpriteIndex, world.GetObjectOnMapList(EnumMapObjectType.MapMarker));
      this.updateMarkers(this.localPlayer, ref waypointSpriteIndex, world.GetObjectOnMapList(EnumMapObjectType.MapQuickMarker));
      this.updateMarkers(this.localPlayer, ref waypointSpriteIndex, world.GetObjectOnMapList(EnumMapObjectType.Backpack));
      if (this.showSleeperVolumes)
        this.updateMarkers(this.localPlayer, ref waypointSpriteIndex, world.GetObjectOnMapList(EnumMapObjectType.Quest));
      this.updateMarkers(this.localPlayer, ref waypointSpriteIndex, world.GetObjectOnMapList(EnumMapObjectType.TreasureChest));
      this.updateMarkers(this.localPlayer, ref waypointSpriteIndex, world.GetObjectOnMapList(EnumMapObjectType.FetchItem));
      this.updateMarkers(this.localPlayer, ref waypointSpriteIndex, world.GetObjectOnMapList(EnumMapObjectType.HiddenCache));
      this.updateMarkers(this.localPlayer, ref waypointSpriteIndex, world.GetObjectOnMapList(EnumMapObjectType.RestorePower));
      if (this.showSleeperVolumes)
        this.updateMarkers(this.localPlayer, ref waypointSpriteIndex, world.GetObjectOnMapList(EnumMapObjectType.SleeperVolume));
      this.updateMarkers(this.localPlayer, ref waypointSpriteIndex, world.GetObjectOnMapList(EnumMapObjectType.VendingMachine));
      if (GameStats.GetBool(EnumGameStats.AirDropMarker))
        this.updateMarkers(this.localPlayer, ref waypointSpriteIndex, world.GetObjectOnMapList(EnumMapObjectType.SupplyDrop));
      Color clear = Color.clear;
      for (int index = waypointSpriteIndex; index < this.waypointSpriteList.Count; ++index)
        ((UIWidget) this.waypointSpriteList[index]).color = clear;
    }
    if (!XUi.IsGameRunning())
      return;
    this.RefreshBindings();
  }

  public override bool GetBindingValue(ref string value, string bindingName)
  {
    switch (bindingName)
    {
      case "compass_language":
        value = !GamePrefs.GetBool(EnumGamePrefs.OptionsUiCompassUseEnglishCardinalDirections) ? Localization.language : Localization.DefaultLanguage;
        return true;
      case "compass_rotation":
        value = !Object.op_Inequality((Object) this.localPlayer, (Object) null) || !Object.op_Inequality((Object) this.localPlayer.playerCamera, (Object) null) ? "0.0" : ((Component) this.localPlayer.playerCamera).transform.eulerAngles.y.ToString();
        return true;
      case "day":
        value = "0";
        if (XUi.IsGameRunning())
        {
          int days = GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime);
          value = this.dayFormatter.Format(days);
        }
        return true;
      case "daycolor":
        value = "FFFFFF";
        if (XUi.IsGameRunning())
        {
          long worldTime = (long) GameManager.Instance.World.worldTime;
          int num = GameStats.GetInt(EnumGameStats.BloodMoonWarning);
          (int Days, int Hours, int _) = GameUtils.WorldTimeToElements((ulong) worldTime);
          if (num != -1 && GameStats.GetInt(EnumGameStats.BloodMoonDay) == Days && num <= Hours)
            value = "FF0000";
        }
        return true;
      case "daytime":
        value = "";
        if (XUi.IsGameRunning())
          value = this.daytimeFormatter.Format(GameManager.Instance.World.worldTime);
        return true;
      case "daytitle":
        value = Localization.Get("xuiDay");
        return true;
      case "showtime":
        value = !Object.op_Inequality((Object) this.localPlayer, (Object) null) ? "true" : ((double) EffectManager.GetValue(PassiveEffects.NoTimeDisplay, _entity: (EntityAlive) this.localPlayer) == 0.0).ToString();
        return true;
      case "time":
        value = "";
        if (XUi.IsGameRunning())
        {
          (int _, int num1, int num2) = GameUtils.WorldTimeToElements(GameManager.Instance.World.worldTime);
          value = this.timeFormatter.Format(num1, num2);
        }
        return true;
      case "timetitle":
        value = Localization.Get("xuiTime");
        return true;
      default:
        return base.GetBindingValue(ref value, bindingName);
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void updateMarkers(
    EntityPlayerLocal localPlayer,
    ref int waypointSpriteIndex,
    List<MapObject> _mapObjectList)
  {
    int count = _mapObjectList.Count;
    if (count == 0)
      return;
    float num1 = (float) this.ViewComponent.Size.x * 0.5f;
    float num2 = num1 * 1.15f;
    Transform cameraTransform = localPlayer.cameraTransform;
    Entity entity = Object.op_Inequality((Object) localPlayer.AttachedToEntity, (Object) null) ? localPlayer.AttachedToEntity : (Entity) localPlayer;
    Vector3 position1 = entity.GetPosition();
    Vector2 vector2_1;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2_1).\u002Ector(position1.x, position1.z);
    Vector3 forward = cameraTransform.forward;
    Vector2 vector2_2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2_2).\u002Ector(forward.x, forward.z);
    ((Vector2) ref vector2_2).Normalize();
    Vector3 right = cameraTransform.right;
    Vector2 vector2_3;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2_3).\u002Ector(right.x, right.z);
    ((Vector2) ref vector2_3).Normalize();
    for (int index = 0; index < count; ++index)
    {
      MapObject mapObject = _mapObjectList[index];
      mapObject.RefreshData();
      if (waypointSpriteIndex >= this.waypointSpriteList.Count)
        break;
      if (mapObject.IsOnCompass())
      {
        if (mapObject is MapObjectZombie)
          this.showSleeperVolumes = false;
        Vector3 position2 = mapObject.GetPosition();
        Vector2 vector2_4 = Vector2.op_Subtraction(new Vector2(position2.x, position2.z), vector2_1);
        float magnitude = ((Vector2) ref vector2_4).magnitude;
        bool flag = true;
        if (mapObject.type == EnumMapObjectType.TreasureChest)
        {
          float defaultRadius = (float) (mapObject as MapObjectTreasureChest).DefaultRadius;
          float num3 = Utils.FastClamp(EffectManager.GetValue(PassiveEffects.TreasureRadius, _originalValue: defaultRadius, _entity: (EntityAlive) localPlayer), 0.0f, defaultRadius);
          if ((double) magnitude < (double) num3)
          {
            float num4 = Mathf.PingPong(Time.time, 0.25f);
            float num5 = 1.25f + num4;
            this.waypointSpriteList[waypointSpriteIndex].atlas = (INGUIAtlas) this.xui.GetAtlasByName("UIAtlas", mapObject.GetMapIcon());
            this.waypointSpriteList[waypointSpriteIndex].spriteName = mapObject.GetMapIcon();
            ((UIWidget) this.waypointSpriteList[waypointSpriteIndex]).SetDimensions((int) (25.0 * (double) num5), (int) (25.0 * (double) num5));
            ((Component) this.waypointSpriteList[waypointSpriteIndex]).transform.localPosition = new Vector3(num1, -24f);
            Color mapIconColor = mapObject.GetMapIconColor();
            ((UIWidget) this.waypointSpriteList[waypointSpriteIndex]).color = Color.Lerp(mapIconColor, Color.red, num4 * 4f);
            ++waypointSpriteIndex;
            flag = false;
          }
        }
        string _spriteName = mapObject.GetCompassIcon();
        Vector3 vector3;
        if (mapObject.type == EnumMapObjectType.HiddenCache)
        {
          ((UIBasicSprite) this.waypointSpriteList[waypointSpriteIndex]).flip = (UIBasicSprite.Flip) 0;
          if ((double) position2.y < (double) localPlayer.GetPosition().y - 2.0)
            _spriteName = mapObject.GetCompassDownIcon();
          else if ((double) position2.y > (double) localPlayer.GetPosition().y + 2.0)
            _spriteName = mapObject.GetCompassUpIcon();
          ((UIWidget) this.waypointSpriteList[waypointSpriteIndex]).depth = 100;
          this.waypointSpriteList[waypointSpriteIndex].atlas = (INGUIAtlas) this.xui.GetAtlasByName("UIAtlas", _spriteName);
          this.waypointSpriteList[waypointSpriteIndex].spriteName = _spriteName;
          vector3 = Vector3.op_Subtraction(position2, entity.GetPosition());
          if ((double) ((Vector3) ref vector3).magnitude < 10.0)
          {
            float num6 = Mathf.PingPong(Time.time, 0.25f);
            float num7 = 1.25f + num6;
            ((UIWidget) this.waypointSpriteList[waypointSpriteIndex]).SetDimensions((int) (25.0 * (double) num7), (int) (25.0 * (double) num7));
            ((Component) this.waypointSpriteList[waypointSpriteIndex]).transform.localPosition = new Vector3(num1, -24f);
            Color mapIconColor = mapObject.GetMapIconColor();
            ((UIWidget) this.waypointSpriteList[waypointSpriteIndex]).color = Color.Lerp(mapIconColor, Color.grey, num6 * 4f);
            ++waypointSpriteIndex;
            flag = false;
          }
        }
        if (mapObject.UseUpDownCompassIcons())
        {
          ((UIBasicSprite) this.waypointSpriteList[waypointSpriteIndex]).flip = (UIBasicSprite.Flip) 0;
          if ((double) position2.y < (double) localPlayer.GetPosition().y - 2.0)
            _spriteName = mapObject.GetCompassDownIcon();
          else if ((double) position2.y > (double) localPlayer.GetPosition().y + 3.0)
            _spriteName = mapObject.GetCompassUpIcon();
          ((UIWidget) this.waypointSpriteList[waypointSpriteIndex]).depth = 100;
          this.waypointSpriteList[waypointSpriteIndex].atlas = (INGUIAtlas) this.xui.GetAtlasByName("UIAtlas", _spriteName);
          this.waypointSpriteList[waypointSpriteIndex].spriteName = _spriteName;
        }
        if (flag)
        {
          Vector2 normalized = ((Vector2) ref vector2_4).normalized;
          if (!mapObject.IsCompassIconClamped() && (double) Vector2.Dot(normalized, vector2_2) < 0.75)
          {
            ((UIWidget) this.waypointSpriteList[waypointSpriteIndex]).color = Color.clear;
          }
          else
          {
            float compassIconScale = mapObject.GetCompassIconScale(magnitude);
            ((UIWidget) this.waypointSpriteList[waypointSpriteIndex]).color = mapObject.GetMapIconColor();
            if (mapObject.IsTracked() && mapObject.NearbyCompassBlink())
            {
              vector3 = Vector3.op_Subtraction(position2, entity.GetPosition());
              if ((double) ((Vector3) ref vector3).magnitude <= 6.0)
              {
                Color mapIconColor = mapObject.GetMapIconColor();
                float num8 = Mathf.PingPong(Time.time, 0.5f);
                ((UIWidget) this.waypointSpriteList[waypointSpriteIndex]).color = Color.Lerp(Color.grey, mapIconColor, num8 * 4f);
                if ((double) num8 > 0.25)
                  compassIconScale += num8 - 0.25f;
              }
            }
            this.waypointSpriteList[waypointSpriteIndex].atlas = (INGUIAtlas) this.xui.GetAtlasByName("UIAtlas", _spriteName);
            this.waypointSpriteList[waypointSpriteIndex].spriteName = _spriteName;
            ((UIWidget) this.waypointSpriteList[waypointSpriteIndex]).SetDimensions((int) (25.0 * (double) compassIconScale), (int) (25.0 * (double) compassIconScale));
            if ((double) Vector2.Dot(normalized, vector2_2) >= 0.75)
              ((Component) this.waypointSpriteList[waypointSpriteIndex]).transform.localPosition = new Vector3(num1 + Vector2.Dot(normalized, vector2_3) * num2, -16f);
            else
              ((Component) this.waypointSpriteList[waypointSpriteIndex]).transform.localPosition = new Vector3(num1 + ((double) Vector2.Dot(normalized, vector2_3) < 0.0 ? -0.675f : 0.675f) * num2, -16f);
            if (mapObject.type == EnumMapObjectType.Entity)
              ((UIWidget) this.waypointSpriteList[waypointSpriteIndex]).depth = 12 + (int) ((double) compassIconScale * 100.0);
            if (!mapObject.IsTracked())
            {
              Color mapIconColor = mapObject.GetMapIconColor();
              ((UIWidget) this.waypointSpriteList[waypointSpriteIndex]).color = Color.op_Multiply(new Color(mapIconColor.r * 0.75f, mapIconColor.g * 0.75f, mapIconColor.b * 0.75f), compassIconScale);
            }
            ++waypointSpriteIndex;
          }
        }
      }
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void updateNavObjects(EntityPlayerLocal localPlayer, ref int waypointSpriteIndex)
  {
    float num1 = (float) this.ViewComponent.Size.x * 0.5f;
    float num2 = num1 * 1.15f;
    Transform cameraTransform = localPlayer.cameraTransform;
    Entity entity = Object.op_Inequality((Object) localPlayer.AttachedToEntity, (Object) null) ? localPlayer.AttachedToEntity : (Entity) localPlayer;
    Vector3 position1 = entity.GetPosition();
    Vector2 vector2_1;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2_1).\u002Ector(position1.x, position1.z);
    Vector3 forward = cameraTransform.forward;
    Vector2 vector2_2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2_2).\u002Ector(forward.x, forward.z);
    ((Vector2) ref vector2_2).Normalize();
    Vector3 right = cameraTransform.right;
    Vector2 vector2_3;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2_3).\u002Ector(right.x, right.z);
    ((Vector2) ref vector2_3).Normalize();
    List<NavObject> navObjectList = NavObjectManager.Instance.NavObjectList;
    for (int index = 0; index < navObjectList.Count; ++index)
    {
      NavObject navObject = navObjectList[index];
      if (!navObject.hiddenOnCompass && navObject.IsValid())
      {
        if (waypointSpriteIndex >= this.waypointSpriteList.Count)
          break;
        NavObjectCompassSettings currentCompassSettings = navObject.CurrentCompassSettings;
        if (currentCompassSettings != null)
        {
          Vector3 position2 = navObject.GetPosition();
          Vector2 vector2_4 = Vector2.op_Subtraction(new Vector2(position2.x + Origin.position.x, position2.z + Origin.position.z), vector2_1);
          float magnitude = ((Vector2) ref vector2_4).magnitude;
          if ((double) magnitude >= (double) currentCompassSettings.MinDistance)
          {
            float maxDistance = navObject.GetMaxDistance((NavObjectSettings) currentCompassSettings, (EntityPlayer) localPlayer);
            if ((double) maxDistance == -1.0 || (double) magnitude <= (double) maxDistance)
            {
              bool flag = true;
              string _spriteName = navObject.GetSpriteName((NavObjectSettings) currentCompassSettings);
              ((UIWidget) this.waypointSpriteList[waypointSpriteIndex]).depth = 12 + currentCompassSettings.DepthOffset;
              if (currentCompassSettings.HotZone != null)
              {
                float num3 = 1f;
                if (currentCompassSettings.HotZone.HotZoneType == NavObjectCompassSettings.HotZoneSettings.HotZoneTypes.Treasure)
                {
                  float extraData = navObject.ExtraData;
                  num3 = Utils.FastClamp(EffectManager.GetValue(PassiveEffects.TreasureRadius, _originalValue: extraData, _entity: (EntityAlive) localPlayer), 0.0f, extraData);
                }
                else if (currentCompassSettings.HotZone.HotZoneType == NavObjectCompassSettings.HotZoneSettings.HotZoneTypes.Custom)
                  num3 = currentCompassSettings.HotZone.CustomDistance;
                if ((double) magnitude < (double) num3)
                {
                  float num4 = Mathf.PingPong(Time.time, 0.25f);
                  float num5 = 1.25f + num4;
                  this.waypointSpriteList[waypointSpriteIndex].atlas = (INGUIAtlas) this.xui.GetAtlasByName("UIAtlas", currentCompassSettings.HotZone.SpriteName);
                  this.waypointSpriteList[waypointSpriteIndex].spriteName = currentCompassSettings.HotZone.SpriteName;
                  ((UIWidget) this.waypointSpriteList[waypointSpriteIndex]).SetDimensions((int) (25.0 * (double) num5), (int) (25.0 * (double) num5));
                  ((Component) this.waypointSpriteList[waypointSpriteIndex]).transform.localPosition = new Vector3(num1, -24f);
                  Color color = currentCompassSettings.HotZone.Color;
                  ((UIWidget) this.waypointSpriteList[waypointSpriteIndex]).color = Color.Lerp(color, Color.red, num4 * 4f);
                  ++waypointSpriteIndex;
                  flag = false;
                }
              }
              if (currentCompassSettings.ShowVerticalCompassIcons)
              {
                ((UIBasicSprite) this.waypointSpriteList[waypointSpriteIndex]).flip = (UIBasicSprite.Flip) 0;
                float num6 = localPlayer.GetPosition().y - Origin.position.y;
                if ((double) position2.y < (double) num6 + (double) currentCompassSettings.ShowDownOffset)
                  _spriteName = currentCompassSettings.DownSpriteName;
                else if ((double) position2.y > (double) num6 + (double) currentCompassSettings.ShowUpOffset)
                  _spriteName = currentCompassSettings.UpSpriteName;
                ((UIWidget) this.waypointSpriteList[waypointSpriteIndex]).depth = 100;
                this.waypointSpriteList[waypointSpriteIndex].atlas = (INGUIAtlas) this.xui.GetAtlasByName("UIAtlas", _spriteName);
                this.waypointSpriteList[waypointSpriteIndex].spriteName = _spriteName;
              }
              if (flag)
              {
                Vector2 normalized = ((Vector2) ref vector2_4).normalized;
                if (!currentCompassSettings.IconClamped && (double) Vector2.Dot(normalized, vector2_2) < 0.75)
                {
                  ((UIWidget) this.waypointSpriteList[waypointSpriteIndex]).color = Color.clear;
                }
                else
                {
                  float num7 = navObject.GetCompassIconScale(magnitude);
                  ((UIWidget) this.waypointSpriteList[waypointSpriteIndex]).color = navObject.UseOverrideColor ? navObject.OverrideColor : currentCompassSettings.Color;
                  if (currentCompassSettings.HasPulse)
                  {
                    Vector3 vector3 = Vector3.op_Subtraction(position2, entity.GetPosition());
                    if ((double) ((Vector3) ref vector3).magnitude <= 6.0)
                    {
                      Color color = navObject.UseOverrideColor ? navObject.OverrideColor : currentCompassSettings.Color;
                      float num8 = Mathf.PingPong(Time.time, 0.5f);
                      ((UIWidget) this.waypointSpriteList[waypointSpriteIndex]).color = Color.Lerp(Color.grey, color, num8 * 4f);
                      if ((double) num8 > 0.25)
                        num7 += num8 - 0.25f;
                    }
                  }
                  this.waypointSpriteList[waypointSpriteIndex].atlas = (INGUIAtlas) this.xui.GetAtlasByName("UIAtlas", _spriteName);
                  this.waypointSpriteList[waypointSpriteIndex].spriteName = _spriteName;
                  ((UIWidget) this.waypointSpriteList[waypointSpriteIndex]).SetDimensions((int) (25.0 * (double) num7), (int) (25.0 * (double) num7));
                  if ((double) Vector2.Dot(normalized, vector2_2) >= 0.75)
                    ((Component) this.waypointSpriteList[waypointSpriteIndex]).transform.localPosition = new Vector3(num1 + Vector2.Dot(normalized, vector2_3) * num2, -16f);
                  else
                    ((Component) this.waypointSpriteList[waypointSpriteIndex]).transform.localPosition = new Vector3(num1 + ((double) Vector2.Dot(normalized, vector2_3) < 0.0 ? -0.675f : 0.675f) * num2, -16f);
                  if (!navObject.IsActive)
                  {
                    Color color = navObject.UseOverrideColor ? navObject.OverrideColor : currentCompassSettings.Color;
                    if ((double) currentCompassSettings.MinFadePercent != -1.0)
                    {
                      if ((double) currentCompassSettings.MinFadePercent > (double) num7)
                        num7 = currentCompassSettings.MinFadePercent;
                      ((UIWidget) this.waypointSpriteList[waypointSpriteIndex]).color = Color.op_Multiply(color, num7);
                    }
                    else
                      ((UIWidget) this.waypointSpriteList[waypointSpriteIndex]).color = color;
                  }
                  ++waypointSpriteIndex;
                }
              }
            }
          }
        }
      }
    }
  }

  public override void OnOpen()
  {
    base.OnOpen();
    this.xui.playerUI.windowManager.CloseIfOpen("windowpaging");
  }

  [PublicizedFrom(EAccessModifier.Private)]
  static XUiC_CompassWindow()
  {
  }
}
