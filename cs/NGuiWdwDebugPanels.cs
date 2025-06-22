// Decompiled with JetBrains decompiler
// Type: NGuiWdwDebugPanels
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using DynamicMusic;
using GamePath;
using MusicUtils.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

#nullable disable
public class NGuiWdwDebugPanels : MonoBehaviour
{
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public NGuiWdwDebugPanels.EDebugDataType debugData;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public NGuiWdwDebugPanels.EPerformanceDisplayType performanceType;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public static readonly GUIStyle guiStyleDebug = new GUIStyle();
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public static GUIStyle guiStyleToggleBox;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public static GUIStyle guiStyleTooltipLabel;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public static GUIStyle guiStyleLabelRightAligned;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public GUIFPS guiFPS;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public NetworkMonitor networkMonitorCh0;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public NetworkMonitor networkMonitorCh1;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public EntityPlayerLocal playerEntity;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public LocalPlayerUI playerUI;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public Vector2i lastResolution;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public GUIStyle boxStyle;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int boxAreaHeight;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int boxAreaWidth;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public const int cLineHeight = 16 /*0x10*/;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public const int cHeaderLabelWidth = 200;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public const int cHeaderLabelHeight = 25;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public Vector3 lastPlayerPos;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float lastPlayerTime;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float playerSpeed;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public static string filterCVar = "";
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public readonly List<NGuiWdwDebugPanels.PanelDefinition> Panels = new List<NGuiWdwDebugPanels.PanelDefinition>();

  [PublicizedFrom(EAccessModifier.Protected)]
  public void Awake()
  {
    this.playerUI = ((Component) this).GetComponentInParent<LocalPlayerUI>();
    if (this.playerUI.IsCleanCopy || LocalPlayerUI.CreatingCleanCopy)
      return;
    NGuiWdwDebugPanels.guiStyleDebug.fontSize = 12;
    NGuiWdwDebugPanels.guiStyleDebug.fontStyle = (FontStyle) 1;
    this.debugData = NGuiWdwDebugPanels.EDebugDataType.Off;
    this.guiFPS = ((Component) ((Component) this).transform).GetComponentInChildren<GUIFPS>();
    NGuiAction _action1 = new NGuiAction("Show Debug Data", PlayerActionsGlobal.Instance.ShowDebugData);
    _action1.SetClickActionDelegate((NGuiAction.OnClickActionDelegate) ([PublicizedFrom(EAccessModifier.Private)] () => this.debugData = this.debugData.CycleEnum<NGuiWdwDebugPanels.EDebugDataType>()));
    _action1.SetIsEnabledDelegate((NGuiAction.IsEnabledDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled)));
    NGuiAction _action2 = new NGuiAction("ShowFPS", PlayerActionsGlobal.Instance.ShowFPS);
    _action2.SetClickActionDelegate((NGuiAction.OnClickActionDelegate) ([PublicizedFrom(EAccessModifier.Private)] () =>
    {
      this.performanceType = this.performanceType.CycleEnum<NGuiWdwDebugPanels.EPerformanceDisplayType>(NGuiWdwDebugPanels.EPerformanceDisplayType.Off, GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled) ? NGuiWdwDebugPanels.EPerformanceDisplayType.FpsAndNetGraphs : NGuiWdwDebugPanels.EPerformanceDisplayType.Fps);
      this.guiFPS.Enabled = this.performanceType != 0;
      this.guiFPS.ShowGraph = this.performanceType == NGuiWdwDebugPanels.EPerformanceDisplayType.FpsAndFpsGraph;
      this.networkMonitorCh0.Enabled = this.performanceType == NGuiWdwDebugPanels.EPerformanceDisplayType.FpsAndNetGraphs;
      this.networkMonitorCh1.Enabled = this.performanceType == NGuiWdwDebugPanels.EPerformanceDisplayType.FpsAndNetGraphs;
    }));
    this.playerUI.windowManager.AddGlobalAction(_action1);
    this.playerUI.windowManager.AddGlobalAction(_action2);
    GameManager.Instance.OnWorldChanged += new GameManager.OnWorldChangedEvent(this.HandleWorldChanged);
    GameObject gameObject = GameObject.Find("NetworkMonitor");
    this.networkMonitorCh0 = new NetworkMonitor(0, ((Component) gameObject.transform.Find("Ch0")).transform);
    this.networkMonitorCh1 = new NetworkMonitor(1, ((Component) gameObject.transform.Find("Ch1")).transform);
    string _enabledPanels = GamePrefs.GetString(EnumGamePrefs.DebugPanelsEnabled);
    if (_enabledPanels == null || _enabledPanels == "-")
    {
      _enabledPanels = ",Ge,Fo,Pr,";
      if (!GameManager.Instance.IsEditMode())
        _enabledPanels += "Ply,";
      if (!GameManager.Instance.IsEditMode())
        _enabledPanels += "Sp,";
      if (GameManager.Instance.IsEditMode())
        _enabledPanels += "Se,";
    }
    this.Panels.Add(new NGuiWdwDebugPanels.PanelDefinition("Player", "Ply", new Func<int, int, int>(this.showDebugPanel_Player), _enabledPanels));
    this.Panels.Add(new NGuiWdwDebugPanels.PanelDefinition("General", "Ge", new Func<int, int, int>(this.showDebugPanel_General), _enabledPanels));
    this.Panels.Add(new NGuiWdwDebugPanels.PanelDefinition("Spawning", "Sp", new Func<int, int, int>(this.showDebugPanel_Spawning), _enabledPanels, SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer));
    this.Panels.Add(new NGuiWdwDebugPanels.PanelDefinition("Chunk", "Ch", new Func<int, int, int>(this.showDebugPanel_Chunk), _enabledPanels));
    this.Panels.Add(new NGuiWdwDebugPanels.PanelDefinition("Cache", "Ca", new Func<int, int, int>(this.showDebugPanel_Cache), _enabledPanels));
    this.Panels.Add(new NGuiWdwDebugPanels.PanelDefinition("Focused Block", "Fo", new Func<int, int, int>(this.showDebugPanel_FocusedBlock), _enabledPanels));
    this.Panels.Add(new NGuiWdwDebugPanels.PanelDefinition("Network", "Ne", new Func<int, int, int>(this.showDebugPanel_Network), _enabledPanels));
    this.Panels.Add(new NGuiWdwDebugPanels.PanelDefinition("Selection", "Se", new Func<int, int, int>(this.showDebugPanel_Selection), _enabledPanels));
    this.Panels.Add(new NGuiWdwDebugPanels.PanelDefinition("Prefab", "Pr", new Func<int, int, int>(this.showDebugPanel_Prefab), _enabledPanels));
    this.Panels.Add(new NGuiWdwDebugPanels.PanelDefinition("Stealth", "St", new Func<int, int, int>(this.showDebugPanel_Stealth), _enabledPanels));
    this.Panels.Add(new NGuiWdwDebugPanels.PanelDefinition("Player Extended - Buffs and CVars", "Plx", new Func<int, int, int>(this.showDebugPanel_PlayerEffectInfo), _enabledPanels));
    this.Panels.Add(new NGuiWdwDebugPanels.PanelDefinition("Texture", "Te", new Func<int, int, int>(this.showDebugPanel_Texture), _enabledPanels));
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void OnEnable()
  {
    this.playerEntity = this.playerUI.entityPlayer;
    this.playerUI.OnEntityPlayerLocalAssigned += new Action<EntityPlayerLocal>(this.HandleEntityPlayerLocalAssigned);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void OnDisable()
  {
    this.playerUI.OnEntityPlayerLocalAssigned -= new Action<EntityPlayerLocal>(this.HandleEntityPlayerLocalAssigned);
    string str = ",";
    foreach (NGuiWdwDebugPanels.PanelDefinition panel in this.Panels)
    {
      if (panel.Active)
        str = $"{str}{panel.ButtonCaption},";
    }
    GamePrefs.Set(EnumGamePrefs.DebugPanelsEnabled, str);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void OnDestroy()
  {
    if (Object.op_Inequality((Object) GameManager.Instance, (Object) null))
      GameManager.Instance.OnWorldChanged -= new GameManager.OnWorldChangedEvent(this.HandleWorldChanged);
    this.networkMonitorCh0.Cleanup();
    this.networkMonitorCh1.Cleanup();
  }

  public void ToggleDisplay()
  {
    if (this.debugData == NGuiWdwDebugPanels.EDebugDataType.Off)
      this.debugData = NGuiWdwDebugPanels.EDebugDataType.General;
    else
      this.debugData = NGuiWdwDebugPanels.EDebugDataType.Off;
  }

  public void ShowGeneralData() => this.debugData = NGuiWdwDebugPanels.EDebugDataType.General;

  [PublicizedFrom(EAccessModifier.Private)]
  public void HandleWorldChanged(World _world)
  {
    this.debugData = NGuiWdwDebugPanels.EDebugDataType.Off;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void HandleEntityPlayerLocalAssigned(EntityPlayerLocal _entity)
  {
    this.playerEntity = _entity;
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public void OnGUI()
  {
    if (!GameManager.Instance.gameStateManager.IsGameStarted() || !this.playerUI.windowManager.IsHUDEnabled())
      return;
    if (NGuiWdwDebugPanels.guiStyleToggleBox == null)
    {
      NGuiWdwDebugPanels.guiStyleToggleBox = new GUIStyle(GUI.skin.toggle)
      {
        wordWrap = false,
        padding = new RectOffset(17, 0, 3, 0)
      };
      NGuiWdwDebugPanels.guiStyleTooltipLabel = new GUIStyle(GUI.skin.label)
      {
        wordWrap = false,
        clipping = (TextClipping) 0
      };
      NGuiWdwDebugPanels.guiStyleLabelRightAligned = new GUIStyle(GUI.skin.label)
      {
        alignment = (TextAnchor) 2
      };
    }
    if (GameStats.GetInt(EnumGameStats.GameState) != 1)
      return;
    GUI.color = Color.white;
    if (this.debugData != NGuiWdwDebugPanels.EDebugDataType.Off)
      this.panelManager();
    if (this.performanceType != NGuiWdwDebugPanels.EPerformanceDisplayType.FpsAndHeat)
      return;
    this.debugShowHeatValue();
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public void Update()
  {
    if (GameStats.GetInt(EnumGameStats.GameState) == 0 || GameManager.IsDedicatedServer)
      return;
    this.networkMonitorCh0.Update();
    this.networkMonitorCh1.Update();
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void debugShowChunkCache()
  {
    float middleX = (float) Screen.width / 2f;
    float middleY = (float) Screen.height / 2f;
    for (int _idx = 0; _idx < GameManager.Instance.World.ChunkClusters.Count; ++_idx)
      GameManager.Instance.World.ChunkClusters[_idx]?.DebugOnGUI(middleX + (float) (100 * _idx), middleY, 8f);
    GameManager.Instance.World.m_ChunkManager.DebugOnGUI(middleX, middleY, 8);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void debugShowHeatValue()
  {
    if (!GameStats.GetBool(EnumGameStats.ZombieHordeMeter) || Object.op_Equality((Object) GameManager.Instance, (Object) null) || GameManager.Instance.World?.aiDirector == null)
      return;
    Vector2i vector2i = new Vector2i(Screen.width, Screen.height);
    if (this.lastResolution != vector2i)
    {
      this.lastResolution = vector2i;
      this.boxStyle = new GUIStyle(GUI.skin.box);
      this.boxStyle.alignment = (TextAnchor) 3;
      int num = 13;
      if (vector2i.y > 1200)
        num = vector2i.y / 90;
      this.boxStyle.fontSize = num;
      this.boxAreaHeight = num + 10;
      this.boxAreaWidth = num * 22;
    }
    Vector3i blockPos = World.worldToBlockPos(this.playerEntity.GetPosition());
    int chunkXz1 = World.toChunkXZ(blockPos.x);
    int chunkXz2 = World.toChunkXZ(blockPos.z);
    AIDirectorChunkEventComponent component = GameManager.Instance.World.aiDirector.GetComponent<AIDirectorChunkEventComponent>();
    AIDirectorChunkData dataFromPosition = component.GetChunkDataFromPosition(blockPos, false);
    string str = $"Heat act {component.GetActiveCount()}";
    float num1 = 0.0f;
    if (dataFromPosition != null)
    {
      num1 = dataFromPosition.ActivityLevel;
      str += $", ch {chunkXz1} {chunkXz2}, {num1:F2}%, {dataFromPosition.EventCount} evs";
      if ((double) dataFromPosition.cooldownDelay > 0.0)
        str += $", {dataFromPosition.cooldownDelay} cd";
    }
    GUI.color = (double) num1 >= 90.0 ? new Color(1f, 0.5f, 0.5f) : ((double) num1 >= 50.0 ? Color.yellow : Color.green);
    float num2 = (float) (Screen.height / 2 + 48 /*0x30*/) + 18f * GamePrefs.GetFloat(EnumGamePrefs.OptionsUiFpsScaling);
    Rect rect;
    // ISSUE: explicit constructor call
    ((Rect) ref rect).\u002Ector(14f, num2, (float) this.boxAreaWidth, (float) this.boxAreaHeight);
    GUI.Box(rect, str, this.boxStyle);
    if (dataFromPosition == null)
      return;
    GUI.color = new Color(0.9f, 0.9f, 0.9f);
    int num3 = Utils.FastMin(10, dataFromPosition.EventCount);
    for (int _index = 0; _index < num3; ++_index)
    {
      ref Rect local = ref rect;
      ((Rect) ref local).y = ((Rect) ref local).y + (float) (this.boxAreaHeight + 1);
      AIDirectorChunkEvent directorChunkEvent = dataFromPosition.GetEvent(_index);
      GUI.Box(rect, $"{_index + 1} {directorChunkEvent.EventType} ({directorChunkEvent.Position}) {directorChunkEvent.Value:F2} {directorChunkEvent.Duration}", this.boxStyle);
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public int showDebugPanel_EnablePanels(int x, int y)
  {
    int num1 = 6;
    int num2 = this.Panels.Count == 0 ? 0 : (this.Panels.Count - 1) / num1 + 1;
    GUI.Box(new Rect((float) x, (float) (y - 1), 250f, (float) (21 * num2 + 4)), "");
    x += 5;
    int num3 = x;
    GUI.color = Color.yellow;
    for (int index = 0; index < this.Panels.Count; ++index)
    {
      NGuiWdwDebugPanels.PanelDefinition panel = this.Panels[index];
      if (!panel.Enabled)
        GUI.enabled = false;
      panel.Active = GUI.Toggle(new Rect((float) x, (float) (y + 1), 38f, 20f), panel.Active, new GUIContent(panel.ButtonCaption, panel.Name), NGuiWdwDebugPanels.guiStyleToggleBox);
      GUI.enabled = true;
      x += 40;
      if (index % num1 == 5)
      {
        y += 21;
        x = num3;
      }
    }
    if (this.Panels.Count % num1 != 0)
      y += 21;
    GUI.color = Color.white;
    return y + 10;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void PanelBoxWithHeader(
    NGuiWdwDebugPanels.EGuiState _guiState,
    int _x,
    ref int _y,
    int _boxWidth,
    int _boxHeight,
    string _boxCaption)
  {
    if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
    {
      GUI.Box(new Rect((float) _x, (float) _y, (float) _boxWidth, (float) _boxHeight), "");
      this.HeaderLabel(_guiState, _x, _y, _boxCaption);
    }
    _y += 21;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void HeaderLabel(
    NGuiWdwDebugPanels.EGuiState _guiState,
    int _x,
    int _y,
    string _text,
    int _labelWidth = 200,
    int _labelHeight = 25)
  {
    if (_guiState != NGuiWdwDebugPanels.EGuiState.Draw)
      return;
    GUI.color = Color.yellow;
    GUI.Label(new Rect((float) (_x + 5), (float) _y, (float) _labelWidth, (float) _labelHeight), _text);
    GUI.color = Color.white;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void LabelWithOutline(
    int _x,
    int _y,
    string _text,
    int _labelWidth = 200,
    int _labelHeight = 25,
    int _xOffset = 5)
  {
    Utils.DrawOutline(new Rect((float) (_x + _xOffset), (float) _y, (float) _labelWidth, (float) _labelHeight), _text, NGuiWdwDebugPanels.guiStyleDebug, Color.black, Color.white);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void FakeTextField(int _x, int _y, int _width, int _height, string _text)
  {
    GUI.Box(new Rect((float) _x, (float) _y, (float) _width, (float) _height), "", GUI.skin.textField);
    GUI.Label(new Rect((float) (_x + 3), (float) (_y + 3), (float) _width, (float) _height), _text);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public int showDebugPanel_Player(int x, int y)
  {
    int num1 = 0;
    int num2 = y;
    int _boxWidth = 340;
    for (NGuiWdwDebugPanels.EGuiState _guiState = NGuiWdwDebugPanels.EGuiState.CalcSize; _guiState < NGuiWdwDebugPanels.EGuiState.Count; ++_guiState)
    {
      y = num2;
      this.PanelBoxWithHeader(_guiState, x, ref y, _boxWidth, num1 - num2 + 5, "Player");
      EntityPlayer playerEntity = (EntityPlayer) this.playerEntity;
      if (Object.op_Equality((Object) playerEntity, (Object) null))
        return y;
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
      {
        float num3 = Time.time - this.lastPlayerTime;
        if ((double) num3 >= 0.5)
        {
          Vector3 vector3 = Vector3.op_Subtraction(playerEntity.position, this.lastPlayerPos);
          this.playerSpeed = ((Vector3) ref vector3).magnitude / num3;
          this.lastPlayerPos = playerEntity.position;
          this.lastPlayerTime = Time.time;
        }
        this.LabelWithOutline(x, y, $"X/Y/Z: {playerEntity.position.x:F1}/{playerEntity.position.y:F1}/{playerEntity.position.z:F1}, Speed {this.playerSpeed:F3}");
      }
      y += 16 /*0x10*/;
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
        this.LabelWithOutline(x, y, $"Rot: {playerEntity.rotation.x:F1}/{playerEntity.rotation.y:F1}/{playerEntity.rotation.z:F1}");
      y += 16 /*0x10*/;
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
      {
        string str1 = string.Empty;
        string str2 = string.Empty;
        BiomeDefinition biomeStandingOn = playerEntity.biomeStandingOn;
        if (biomeStandingOn != null)
        {
          str1 = biomeStandingOn.m_sBiomeName;
          IBiomeProvider biomeProvider = playerEntity.world.ChunkCache.ChunkProvider.GetBiomeProvider();
          if (biomeProvider != null)
          {
            Vector3i blockPosition = playerEntity.GetBlockPosition();
            int subBiomeIdxAt = biomeProvider.GetSubBiomeIdxAt(biomeStandingOn, blockPosition.x, blockPosition.y, blockPosition.z);
            if (subBiomeIdxAt >= 0)
              str2 = $", sub {subBiomeIdxAt}";
          }
        }
        this.LabelWithOutline(x, y, $"Biome: {str1}{str2}");
      }
      y += 16 /*0x10*/;
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
      {
        PrefabInstance poiAtPosition = playerEntity.world.GetPOIAtPosition(playerEntity.position, false);
        string str3;
        if (poiAtPosition != null)
          str3 = $"{poiAtPosition.name}, {poiAtPosition.boundingBoxPosition}, r {poiAtPosition.rotation}, sl {poiAtPosition.sleeperVolumes.Count}, tr {poiAtPosition.triggerVolumes.Count}";
        else
          str3 = string.Empty;
        string str4 = str3;
        this.LabelWithOutline(x, y, "POI: " + str4);
        y += 16 /*0x10*/;
        string str5 = poiAtPosition == null ? string.Empty : poiAtPosition.GetPositionRelativeToPoi(Vector3i.Floor(playerEntity.position)).ToString();
        this.LabelWithOutline(x, y, "X/Y/Z in prefab: " + str5);
        y += 16 /*0x10*/;
      }
      else
        y += 32 /*0x20*/;
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
        this.LabelWithOutline(x, y, $"DM Threat Lvl: {this.playerEntity.ThreatLevel.Category.ToStringCached<ThreatLevelType>()} : {this.playerEntity.ThreatLevel.Numeric:0.##}");
      y += 16 /*0x10*/;
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
        this.LabelWithOutline(x, y, $"DM Zeds: {ThreatLevelUtility.Zombies}, Targeting: {ThreatLevelUtility.Targeting}");
      y += 16 /*0x10*/;
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
        this.LabelWithOutline(x, y, $"Outside Temperature (F): {Mathf.FloorToInt(playerEntity.PlayerStats.GetOutsideTemperature())}");
      y += 16 /*0x10*/;
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
        this.LabelWithOutline(x, y, $"Feels Like Temperature (F): {Mathf.FloorToInt(playerEntity.Buffs.GetCustomVar("_coretemp")) + 70} ({Mathf.FloorToInt(playerEntity.Buffs.GetCustomVar("_coretemp"))})");
      y += 16 /*0x10*/;
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
        this.LabelWithOutline(x, y, $"Degrees Absorbed (F): {Mathf.FloorToInt(playerEntity.Buffs.GetCustomVar("_degreesabsorbed"))}");
      y += 16 /*0x10*/;
      num1 = y;
    }
    return y + 10;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public int showDebugPanel_PlayerEffectInfo(int x, int y)
  {
    int num1 = y;
    EntityAlive entityAlive = (EntityAlive) this.playerEntity;
    if (InputUtils.ShiftKeyPressed)
    {
      Ray ray = this.playerEntity.GetLookRay();
      if (GameManager.Instance.IsEditMode() && GamePrefs.GetInt(EnumGamePrefs.SelectionOperationMode) == 4)
      {
        ray = ((Component) this.playerEntity.cameraTransform).GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        ref Ray local = ref ray;
        ((Ray) ref local).origin = Vector3.op_Addition(((Ray) ref local).origin, Origin.position);
      }
      ref Ray local1 = ref ray;
      Vector3 origin = ((Ray) ref local1).origin;
      Vector3 direction = ((Ray) ref ray).direction;
      Vector3 vector3 = Vector3.op_Multiply(((Vector3) ref direction).normalized, 0.1f);
      ((Ray) ref local1).origin = Vector3.op_Addition(origin, vector3);
      float distance = Utils.FastMax(Utils.FastMax(Constants.cDigAndBuildDistance, Constants.cCollectItemDistance), 30f);
      int _hitMask = 69;
      if (Voxel.Raycast(GameManager.Instance.World, ray, distance, -555528213, _hitMask, 0.0f))
      {
        Transform hitRootTransform = GameUtils.GetHitRootTransform(Voxel.voxelRayHitInfo.tag, Voxel.voxelRayHitInfo.transform);
        if (Object.op_Inequality((Object) hitRootTransform, (Object) null))
          entityAlive = ((Component) hitRootTransform).gameObject.GetComponent<Entity>() as EntityAlive;
      }
    }
    if (Object.op_Equality((Object) entityAlive, (Object) null))
      return num1;
    int _boxHeight = entityAlive.Buffs.ActiveBuffs.Count + Mathf.Min(25, entityAlive.Buffs.CVars.Count) * 16 /*0x10*/ + 96 /*0x60*/ + 15;
    int _boxWidth = 440;
    x = (int) ((double) Screen.width / (double) Utils.FastClamp((float) Screen.height / 1080f * GameOptionsManager.GetActiveUiScale(), 0.4f, 2f)) - (_boxWidth + 16 /*0x10*/);
    y = 64 /*0x40*/;
    this.PanelBoxWithHeader(NGuiWdwDebugPanels.EGuiState.Draw, x, ref y, _boxWidth, _boxHeight, $"{entityAlive.EntityName} Buffs ({entityAlive.Buffs.ActiveBuffs.Count.ToString()})");
    for (int index = 0; index < entityAlive.Buffs.ActiveBuffs.Count; ++index)
    {
      BuffValue activeBuff = entityAlive.Buffs.ActiveBuffs[index];
      BuffClass buffClass = activeBuff.BuffClass;
      GUI.color = buffClass != null ? buffClass.IconColor : Color.magenta;
      Entity entity = GameManager.Instance.World.GetEntity(activeBuff.InstigatorId);
      string str = $"none (id {activeBuff.InstigatorId})";
      string _text = $"{activeBuff.BuffName} : From {(Object.op_Implicit((Object) entity) ? entity.GetDebugName() : str)} {(Object.op_Implicit((Object) entity) ? entity.entityId.ToString() : "")}";
      if (buffClass == null)
        _text += " : BuffClass Missing";
      this.LabelWithOutline(x, y, _text);
      GUI.color = Color.white;
      y += 16 /*0x10*/;
    }
    y += 21;
    this.HeaderLabel(NGuiWdwDebugPanels.EGuiState.Draw, x, y, $"{entityAlive.EntityName} CVars ({entityAlive.Buffs.CVars.Count.ToString()})");
    GUI.Label(new Rect((float) (x + 150), (float) y, 50f, 25f), "Filter:", NGuiWdwDebugPanels.guiStyleLabelRightAligned);
    if (Cursor.visible)
      NGuiWdwDebugPanels.filterCVar = GUI.TextField(new Rect((float) (x + 205), (float) y, 200f, 25f), NGuiWdwDebugPanels.filterCVar);
    else
      this.FakeTextField(x + 205, y, 200, 25, NGuiWdwDebugPanels.filterCVar);
    y += 21;
    int num2 = y;
    int num3 = 1;
    int num4 = -1;
    foreach (string key in entityAlive.Buffs.CVars.Keys)
    {
      if ((double) entityAlive.Buffs.CVars[key] != 0.0 && key.ContainsCaseInsensitive(NGuiWdwDebugPanels.filterCVar))
      {
        this.LabelWithOutline(x, y, $"{key} : {entityAlive.Buffs.CVars[key]}");
        if (num3 % 25 == 0)
        {
          x += 220;
          if (num4 == -1)
            num4 = y + 16 /*0x10*/ + 5;
          y = num2;
        }
        else
          y += 16 /*0x10*/;
        ++num3;
      }
    }
    return num1;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public int showDebugPanel_DynamicMusicInfo(int x, int y)
  {
    int num1 = 0;
    int num2 = y;
    int _boxWidth = 220;
    for (NGuiWdwDebugPanels.EGuiState _guiState = NGuiWdwDebugPanels.EGuiState.CalcSize; _guiState < NGuiWdwDebugPanels.EGuiState.Count; ++_guiState)
    {
      y = num2;
      this.PanelBoxWithHeader(_guiState, x, ref y, _boxWidth, num1 - num2 + 5, "Dynamic Music");
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
        this.LabelWithOutline(x, y, "SomeStringData");
      y += 16 /*0x10*/;
      num1 = y;
    }
    return y + 10;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public int showDebugPanel_Spawning(int x, int y)
  {
    int count = GameManager.Instance.World.Last4Spawned.Count;
    int _boxHeight = 21 + count * 16 /*0x10*/ + 5;
    this.PanelBoxWithHeader(NGuiWdwDebugPanels.EGuiState.Draw, x, ref y, 325, _boxHeight, "Spawning");
    for (int index = count - 1; index >= 0; --index)
    {
      SSpawnedEntity sspawnedEntity = GameManager.Instance.World.Last4Spawned[index];
      this.LabelWithOutline(x, y, $"{sspawnedEntity.name}:{sspawnedEntity.pos} - {sspawnedEntity.distanceToLocalPlayer:F1}m", 300);
      y += 16 /*0x10*/;
    }
    return y + 10;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public int showDebugPanel_Chunk(int x, int y)
  {
    EntityPlayer playerEntity = (EntityPlayer) this.playerEntity;
    if (Object.op_Equality((Object) playerEntity, (Object) null))
      return y;
    int x1 = playerEntity.chunkPosAddedEntityTo.x;
    int z = playerEntity.chunkPosAddedEntityTo.z;
    Chunk chunkSync1 = (Chunk) GameManager.Instance.World.GetChunkSync(x1, z);
    if (chunkSync1 == null)
      return y;
    Vector3i areaMasterChunkPos = Chunk.ToAreaMasterChunkPos(chunkSync1.ToWorldPos(Vector3i.zero));
    Chunk chunkSync2 = (Chunk) GameManager.Instance.World.GetChunkSync(areaMasterChunkPos.x, areaMasterChunkPos.z);
    int num1 = 0;
    int num2 = y;
    int _boxWidth = 550;
    for (NGuiWdwDebugPanels.EGuiState _guiState = NGuiWdwDebugPanels.EGuiState.CalcSize; _guiState < NGuiWdwDebugPanels.EGuiState.Count; ++_guiState)
    {
      y = num2;
      this.PanelBoxWithHeader(_guiState, x, ref y, _boxWidth, num1 - num2 + 5, "Chunk");
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
        this.LabelWithOutline(x, y, RegionFileManager.DebugUtil.GetLocationString(chunkSync1.X, chunkSync1.Z));
      y += 16 /*0x10*/;
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
      {
        string str = "";
        int num3 = 0;
        ChunkAreaBiomeSpawnData chunkBiomeSpawnData = chunkSync2?.GetChunkBiomeSpawnData();
        if (chunkBiomeSpawnData != null)
        {
          str = chunkBiomeSpawnData.poiTags.ToString();
          num3 = chunkBiomeSpawnData.groupsEnabledFlags;
        }
        this.LabelWithOutline(x, y, $"AreaMaster: {areaMasterChunkPos.x}/{areaMasterChunkPos.z} {str} {num3:x}");
      }
      y += 16 /*0x10*/;
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
      {
        ChunkAreaBiomeSpawnData chunkBiomeSpawnData = chunkSync2?.GetChunkBiomeSpawnData();
        this.LabelWithOutline(x, y, (chunkBiomeSpawnData != null ? chunkBiomeSpawnData.ToString() : string.Empty) ?? "");
      }
      y += 16 /*0x10*/;
      int num4 = chunkSync1.GetTris();
      string str1 = "Tris sum: " + num4.ToString();
      int num5 = 0;
      for (int _idx = 0; _guiState == NGuiWdwDebugPanels.EGuiState.Draw && _idx < MeshDescription.meshes.Length; ++_idx)
      {
        string[] strArray = new string[5]
        {
          str1,
          " [",
          _idx.ToString(),
          "]: ",
          null
        };
        num4 = chunkSync1.GetTrisInMesh(_idx);
        strArray[4] = num4.ToString();
        str1 = string.Concat(strArray);
        num5 += chunkSync1.GetSizeOfMesh(_idx);
      }
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
        this.LabelWithOutline(x, y, $"{str1} Size: {num5 / 1024 /*0x0400*/}kB", 300);
      y += 16 /*0x10*/;
      num1 = y;
    }
    return y + 10;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public int showDebugPanel_Cache(int x, int y)
  {
    int num1 = 0;
    int num2 = y;
    int _boxWidth = 550;
    for (NGuiWdwDebugPanels.EGuiState _guiState = NGuiWdwDebugPanels.EGuiState.CalcSize; _guiState < NGuiWdwDebugPanels.EGuiState.Count; ++_guiState)
    {
      y = num2;
      this.PanelBoxWithHeader(_guiState, x, ref y, _boxWidth, num1 - num2 + 5, "Cache");
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
      {
        int gameObjectsCount = GameManager.Instance.World.m_ChunkManager.GetDisplayedChunkGameObjectsCount();
        int count = GameManager.Instance.World.m_ChunkManager.GetFreeChunkGameObjects().Count;
        this.LabelWithOutline(x, y, $"CGO: {ChunkGameObject.InstanceCount} Displayed: {gameObjectsCount} Free: {count}");
      }
      y += 16 /*0x10*/;
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
        this.LabelWithOutline(x, y, MemoryPools.GetDebugInfo());
      y += 16 /*0x10*/;
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
        this.LabelWithOutline(x, y, MemoryPools.GetDebugInfoEx());
      y += 16 /*0x10*/;
      num1 = y;
    }
    return y + 10;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public int showDebugPanel_General(int x, int y)
  {
    World world = GameManager.Instance.World;
    if (world == null)
      return y;
    int num1 = 0;
    int num2 = y;
    int _boxWidth = 220;
    for (NGuiWdwDebugPanels.EGuiState _guiState = NGuiWdwDebugPanels.EGuiState.CalcSize; _guiState < NGuiWdwDebugPanels.EGuiState.Count; ++_guiState)
    {
      y = num2;
      this.PanelBoxWithHeader(_guiState, x, ref y, _boxWidth, num1 - num2 + 5, "General");
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
        this.LabelWithOutline(x, y, $"Seed='{(SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? GamePrefs.GetString(EnumGamePrefs.GameName) : GamePrefs.GetString(EnumGamePrefs.GameNameClient))}' '{GamePrefs.GetString(EnumGamePrefs.GameWorld)}'");
      y += 16 /*0x10*/;
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
        this.LabelWithOutline(x, y, $"Time scale: {Time.timeScale}");
      y += 16 /*0x10*/;
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
      {
        int entityAliveCount1 = world.GetEntityAliveCount(EntityFlags.Animal, EntityFlags.Animal);
        int entityAliveCount2 = world.GetEntityAliveCount(EntityFlags.Bandit, EntityFlags.Bandit);
        int entityAliveCount3 = world.GetEntityAliveCount(EntityFlags.Zombie, EntityFlags.Zombie);
        this.LabelWithOutline(x, y, $"World Ent: {world.Entities.Count} ({Entity.InstanceCount}) An: {entityAliveCount1} Ban: {entityAliveCount2} Zom: {entityAliveCount3}");
      }
      y += 16 /*0x10*/;
      PathFinderThread instance = PathFinderThread.Instance;
      if (instance != null)
      {
        if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
          this.LabelWithOutline(x, y, $"Paths: q {instance.GetQueueCount()}, finish {instance.GetFinishedCount()}");
        y += 16 /*0x10*/;
      }
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
        this.LabelWithOutline(x, y, $"Memory used: {GC.GetTotalMemory(false) / 1048576L /*0x100000*/}MB");
      y += 16 /*0x10*/;
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
        this.LabelWithOutline(x, y, $"Active threads: {ThreadManager.ActiveThreads.Count} tasks: {ThreadManager.QueuedCount}");
      y += 16 /*0x10*/;
      if (!world.IsRemote())
      {
        if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
          this.LabelWithOutline(x, y, "Ticked blocks: " + world.GetWBT().GetCount().ToString());
        y += 16 /*0x10*/;
      }
      num1 = y;
    }
    return y + 10;
  }

  public int showDebugPanel_FocusedBlock(int x, int y)
  {
    return this.showDebugPanel_FocusedBlock(x, y, false);
  }

  public int showDebugPanel_FocusedBlock(int x, int y, bool forceFocusedBlock = false)
  {
    EntityPlayer playerEntity = (EntityPlayer) this.playerEntity;
    if (Object.op_Equality((Object) playerEntity, (Object) null) || Object.op_Equality((Object) playerEntity, (Object) null) || playerEntity.inventory.holdingItemData == null || !playerEntity.inventory.holdingItemData.hitInfo.bHitValid)
      return y;
    WorldRayHitInfo hitInfo = playerEntity.inventory.holdingItemData.hitInfo;
    Vector3i vector3i = InputUtils.ShiftKeyPressed | forceFocusedBlock ? hitInfo.hit.blockPos : hitInfo.lastBlockPos;
    BlockFace blockFace = hitInfo.hit.blockFace;
    if (vector3i.y < 0 || vector3i.y >= 256 /*0x0100*/)
      return y;
    ChunkCluster chunkCluster = GameManager.Instance.World.ChunkClusters[hitInfo.hit.clrIdx];
    if (chunkCluster == null)
      return y;
    Chunk chunkFromWorldPos = (Chunk) chunkCluster.GetChunkFromWorldPos(vector3i);
    if (chunkFromWorldPos == null)
      return y;
    Vector3i block1 = World.toBlock(vector3i);
    int x1 = block1.x;
    int y1 = block1.y;
    int z = block1.z;
    BlockValue block2 = chunkFromWorldPos.GetBlock(block1);
    Block block3 = block2.Block;
    BlockShape shape = block3.shape;
    BlockFace rotatedBlockFace = shape.GetRotatedBlockFace(block2, blockFace);
    string[] strArray1 = new string[1];
    int[] numArray = new int[1];
    for (int _channel = 0; _channel < 1; ++_channel)
    {
      numArray[_channel] = GameManager.Instance.World.ChunkClusters[0].GetBlockFaceTexture(vector3i, rotatedBlockFace, _channel);
      if (numArray[_channel] == 0)
      {
        string _name;
        numArray[_channel] = GameUtils.FindPaintIdForBlockFace(block2, rotatedBlockFace, out _name, _channel);
        strArray1[_channel] = _name;
      }
      else
        strArray1[_channel] = numArray[_channel] < 0 || numArray[_channel] >= BlockTextureData.list.Length ? string.Empty : BlockTextureData.list[numArray[_channel]]?.Name ?? "N/A";
    }
    StringBuilder stringBuilder = new StringBuilder();
    for (int channel = 0; channel < 1; ++channel)
    {
      bool flag = false;
      if (numArray[0] >= 0 && numArray[0] < BlockTextureData.list.Length && block3.MeshIndex == (byte) 0)
      {
        BlockTextureData blockTextureData = BlockTextureData.list[numArray[0]];
        int textureId = blockTextureData != null ? (int) blockTextureData.TextureID : 0;
        int index = textureId == 0 ? block3.GetSideTextureId(block2, rotatedBlockFace, 0) : textureId;
        flag = MeshDescription.meshes[0].textureAtlas.uvMapping[index].bGlobalUV;
        if (rotatedBlockFace != BlockFace.None)
        {
          switch (block3.GetUVMode((int) rotatedBlockFace, channel))
          {
            case Block.UVMode.Global:
              flag = true;
              break;
            case Block.UVMode.Local:
              flag = false;
              break;
          }
        }
      }
      if (channel > 0)
        stringBuilder.Append(",");
      stringBuilder.Append(flag ? "G" : "L");
    }
    int num1 = 0;
    int num2 = y;
    int _boxWidth = 260;
    for (NGuiWdwDebugPanels.EGuiState _guiState = NGuiWdwDebugPanels.EGuiState.CalcSize; _guiState < NGuiWdwDebugPanels.EGuiState.Count; ++_guiState)
    {
      y = num2;
      this.PanelBoxWithHeader(_guiState, x, ref y, _boxWidth, num1 - num2 + 5, "Focused Block");
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
        this.LabelWithOutline(x, y, "Pos: " + vector3i.ToString());
      y += 16 /*0x10*/;
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
      {
        BlockValue blockValue1;
        if (block2.isair && playerEntity.inventory.holdingItemData is ItemClassBlock.ItemBlockInventoryData holdingItemData)
        {
          blockValue1 = holdingItemData.itemValue.ToBlockValue();
          if (!blockValue1.Block.shape.IsTerrain())
          {
            BlockValue blockValue2 = holdingItemData.itemValue.ToBlockValue() with
            {
              rotation = holdingItemData.rotation
            };
            int _x = x;
            int _y = y;
            blockValue1 = blockValue2;
            string _text = "Data: " + blockValue1.ToString();
            this.LabelWithOutline(_x, _y, _text);
            goto label_32;
          }
        }
        int _x1 = x;
        int _y1 = y;
        blockValue1 = block2;
        string _text1 = "Data: " + blockValue1.ToString();
        this.LabelWithOutline(_x1, _y1, _text1);
      }
label_32:
      y += 16 /*0x10*/;
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
        this.LabelWithOutline(x, y, $"Name: {block3.GetBlockName()} (W={blockFace.ToStringCached<BlockFace>()}->B={rotatedBlockFace.ToStringCached<BlockFace>()})");
      y += 16 /*0x10*/;
      for (int index = 0; index < 1; ++index)
      {
        if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
          this.LabelWithOutline(x, y, $"Paint Id: {numArray[index]} ({strArray1[index]})");
        y += 16 /*0x10*/;
      }
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
      {
        if (shape is BlockShapeModelEntity shapeModelEntity)
          this.LabelWithOutline(x, y, "Prefab: " + shapeModelEntity.modelName);
        else
          this.LabelWithOutline(x, y, $"Shape: {shape.GetName()} V={shape.GetVertexCount()} T={shape.GetTriangleCount()} UV: {stringBuilder.ToString()}");
      }
      y += 16 /*0x10*/;
      byte num3;
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
      {
        int _x = x;
        int _y = y;
        string[] strArray2 = new string[8];
        strArray2[0] = "Light: emit=";
        num3 = block3.GetLightValue(block2);
        strArray2[1] = num3.ToString();
        strArray2[2] = " opac=";
        strArray2[3] = block3.lightOpacity.ToString();
        strArray2[4] = " sun=";
        num3 = chunkFromWorldPos.GetLight(x1, y1, z, Chunk.LIGHT_TYPE.SUN);
        strArray2[5] = num3.ToString();
        strArray2[6] = " blk=";
        num3 = chunkFromWorldPos.GetLight(x1, y1, z, Chunk.LIGHT_TYPE.BLOCK);
        strArray2[7] = num3.ToString();
        string _text = string.Concat(strArray2);
        this.LabelWithOutline(_x, _y, _text);
      }
      y += 16 /*0x10*/;
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
      {
        int _x = x;
        int _y = y;
        string[] strArray3 = new string[5]
        {
          "Stability: ",
          null,
          null,
          null,
          null
        };
        num3 = chunkFromWorldPos.GetStability(x1, y1, z);
        strArray3[1] = num3.ToString();
        strArray3[2] = " Density: ";
        strArray3[3] = chunkFromWorldPos.GetDensity(x1, y1, z).ToString("0.00");
        strArray3[4] = " ";
        string _text = string.Concat(strArray3);
        this.LabelWithOutline(_x, _y, _text);
      }
      y += 16 /*0x10*/;
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
      {
        int _x = x;
        int _y = y;
        string[] strArray4 = new string[6];
        strArray4[0] = "Height: ";
        num3 = chunkFromWorldPos.GetHeight(x1, z);
        strArray4[1] = num3.ToString();
        strArray4[2] = " Terrain: ";
        num3 = chunkFromWorldPos.GetTerrainHeight(x1, z);
        strArray4[3] = num3.ToString();
        strArray4[4] = " Deco: ";
        strArray4[5] = chunkFromWorldPos.GetDecoAllowedAt(x1, z).ToStringFriendlyCached();
        string _text = string.Concat(strArray4);
        this.LabelWithOutline(_x, _y, _text);
      }
      y += 16 /*0x10*/;
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
      {
        string _text = "Normal: " + GameManager.Instance.World.GetTerrainNormalAt(vector3i.x, vector3i.z).ToCultureInvariantString();
        int mass = chunkFromWorldPos.GetWater(x1, y1, z).GetMass();
        if (mass > 0)
          _text = $"{_text} Water: {mass.ToString()}";
        this.LabelWithOutline(x, y, _text);
      }
      y += 16 /*0x10*/;
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
      {
        if (block3.HasTileEntity)
        {
          TileEntity tileEntity = chunkFromWorldPos.GetTileEntity(block1);
          ITileEntityLootable _typedTe;
          if (!(tileEntity is TileEntitySecureDoor) && tileEntity.TryGetSelfOrFeature<ITileEntityLootable>(out _typedTe))
            this.LabelWithOutline(x, y, "LootStage: " + playerEntity.GetLootStage(_typedTe.LootStageMod, _typedTe.LootStageBonus).ToString());
          else
            this.LabelWithOutline(x, y, "LootStage: " + playerEntity.GetLootStage(0.0f, 0.0f).ToString());
        }
        else
          this.LabelWithOutline(x, y, "LootStage: " + playerEntity.GetLootStage(0.0f, 0.0f).ToString());
      }
      y += 16 /*0x10*/;
      num1 = y;
    }
    return y + 10;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public int showDebugPanel_Network(int x, int y)
  {
    if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer || SingletonMonoBehaviour<ConnectionManager>.Instance.ClientCount() == 0)
      return y;
    int num1 = 0;
    int num2 = y;
    int _boxWidth = 220;
    int num3 = 0;
    int num4 = 0;
    int num5 = 0;
    int num6 = 0;
    for (NGuiWdwDebugPanels.EGuiState _guiState = NGuiWdwDebugPanels.EGuiState.CalcSize; _guiState < NGuiWdwDebugPanels.EGuiState.Count; ++_guiState)
    {
      y = num2;
      this.PanelBoxWithHeader(_guiState, x, ref y, _boxWidth, num1 - num2 + 5, "Network");
      if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
      {
        if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
          this.LabelWithOutline(x, y, "Clients: " + SingletonMonoBehaviour<ConnectionManager>.Instance.ClientCount().ToString());
        y += 16 /*0x10*/;
        if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
        {
          string str = $"{(num3 > 1024 /*0x0400*/ ? (object) ((float) num3 / 1024f).ToCultureInvariantString("0.0") : (object) num3.ToString())}{(num3 > 1024 /*0x0400*/ ? (object) "k" : (object) "")}B";
          this.LabelWithOutline(x, y, string.Format("   total sent: #{1,3}  {0}", (object) str, (object) num5));
        }
        y += 16 /*0x10*/;
        if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
        {
          string str = $"{(num4 > 1024 /*0x0400*/ ? (object) ((float) num4 / 1024f).ToCultureInvariantString("0.0") : (object) num4.ToString())}{(num4 > 1024 /*0x0400*/ ? (object) "k" : (object) "")}B";
          this.LabelWithOutline(x, y, string.Format("   total recv: #{1,3}  {0}", (object) str, (object) num6));
        }
        y += 16 /*0x10*/;
        int _bytesPerSecondSent = 0;
        int _bytesPerSecondReceived = 0;
        int _packagesPerSecondSent = 0;
        int _packagesPerSecondReceived = 0;
        foreach (ClientInfo clientInfo in SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.List)
        {
          if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
            this.LabelWithOutline(x, y, $"Client {clientInfo.InternalId.CombinedString,2}");
          y += 16 /*0x10*/;
          if (_guiState == NGuiWdwDebugPanels.EGuiState.CalcSize)
          {
            clientInfo.netConnection[0].GetStats().GetStats(0.5f, out _bytesPerSecondSent, out _packagesPerSecondSent, out _bytesPerSecondReceived, out _packagesPerSecondReceived);
            num3 += _bytesPerSecondSent;
            num4 += _bytesPerSecondReceived;
            num5 += _packagesPerSecondSent;
            num6 += _packagesPerSecondReceived;
          }
          if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
          {
            clientInfo.netConnection[0].GetStats().GetStats(0.5f, out _bytesPerSecondSent, out _packagesPerSecondSent, out _bytesPerSecondReceived, out _packagesPerSecondReceived);
            string str = $"{(_bytesPerSecondSent > 1024 /*0x0400*/ ? (object) ((float) _bytesPerSecondSent / 1024f).ToCultureInvariantString("0.0") : (object) _bytesPerSecondSent.ToString())}{(_bytesPerSecondSent > 1024 /*0x0400*/ ? (object) "k" : (object) "")}B";
            this.LabelWithOutline(x, y, string.Format("   stream0 sent: #{1,3}  {0}", (object) str, (object) _packagesPerSecondSent));
          }
          y += 16 /*0x10*/;
          if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
          {
            string str = $"{(_bytesPerSecondReceived > 1024 /*0x0400*/ ? (object) ((float) _bytesPerSecondSent / 1024f).ToCultureInvariantString("0.0") : (object) _bytesPerSecondReceived.ToString())}{(_bytesPerSecondReceived > 1024 /*0x0400*/ ? (object) "k" : (object) "")}B";
            this.LabelWithOutline(x, y, string.Format("   stream0 rcvd: #{1,3}  {0}", (object) str, (object) _packagesPerSecondReceived));
          }
          y += 16 /*0x10*/;
          if (_guiState == NGuiWdwDebugPanels.EGuiState.CalcSize)
          {
            clientInfo.netConnection[1].GetStats().GetStats(0.5f, out _bytesPerSecondSent, out _packagesPerSecondSent, out _bytesPerSecondReceived, out _packagesPerSecondReceived);
            num3 += _bytesPerSecondSent;
            num4 += _bytesPerSecondReceived;
            num5 += _packagesPerSecondSent;
            num6 += _packagesPerSecondReceived;
          }
          if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
          {
            clientInfo.netConnection[1].GetStats().GetStats(0.5f, out _bytesPerSecondSent, out _packagesPerSecondSent, out _bytesPerSecondReceived, out _packagesPerSecondReceived);
            string str = $"{(_bytesPerSecondSent > 1024 /*0x0400*/ ? (object) ((float) _bytesPerSecondSent / 1024f).ToCultureInvariantString("0.0") : (object) _bytesPerSecondSent.ToString())}{(_bytesPerSecondSent > 1024 /*0x0400*/ ? (object) "k" : (object) "")}B";
            this.LabelWithOutline(x, y, string.Format("   stream1 sent: #{1,3}  {0}", (object) str, (object) _packagesPerSecondSent));
          }
          y += 16 /*0x10*/;
          if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
          {
            string str = $"{(_bytesPerSecondReceived > 1024 /*0x0400*/ ? (object) ((float) _bytesPerSecondReceived / 1024f).ToCultureInvariantString("0.0") : (object) _bytesPerSecondReceived.ToString())}{(_bytesPerSecondReceived > 1024 /*0x0400*/ ? (object) "k" : (object) "")}B";
            this.LabelWithOutline(x, y, string.Format("   stream1 rcvd: #{1,3}  {0}", (object) str, (object) _packagesPerSecondReceived));
          }
          y += 16 /*0x10*/;
        }
      }
      num1 = y;
    }
    return y + 10;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public int showDebugPanel_Selection(int x, int y)
  {
    if (GameManager.Instance.GetActiveBlockTool() == null)
      return y;
    int num1 = 0;
    int num2 = y;
    int _boxWidth = 220;
    for (NGuiWdwDebugPanels.EGuiState _guiState = NGuiWdwDebugPanels.EGuiState.CalcSize; _guiState < NGuiWdwDebugPanels.EGuiState.Count; ++_guiState)
    {
      y = num2;
      this.PanelBoxWithHeader(_guiState, x, ref y, _boxWidth, num1 - num2 + 5, "Selection");
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
      {
        string debugOutput = GameManager.Instance.GetActiveBlockTool().GetDebugOutput();
        this.LabelWithOutline(x, y, debugOutput);
      }
      y += 16 /*0x10*/;
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
      {
        XUiC_WoPropsSleeperVolume.VolumeStats _stats;
        if (XUiC_WoPropsSleeperVolume.GetSelectedVolumeStats(out _stats))
        {
          this.LabelWithOutline(x, y, "Sleeper Volume");
          y += 16 /*0x10*/;
          this.LabelWithOutline(x, y, $"Index: {_stats.index}");
          y += 16 /*0x10*/;
          this.LabelWithOutline(x, y, $"Pos: {_stats.pos}");
          y += 16 /*0x10*/;
          this.LabelWithOutline(x, y, $"Size: {_stats.size}");
          y += 16 /*0x10*/;
          this.LabelWithOutline(x, y, "Group: " + _stats.groupName);
          y += 16 /*0x10*/;
          this.LabelWithOutline(x, y, $"Priority: {_stats.isPriority}   QuestExc: {_stats.isQuestExclude}");
          y += 16 /*0x10*/;
          this.LabelWithOutline(x, y, $"Sleepers: {_stats.sleeperCount}   MinMax: {_stats.spawnCountMin}-{_stats.spawnCountMax}");
          y += 16 /*0x10*/;
        }
      }
      else
        y += 112 /*0x70*/;
      num1 = y;
    }
    return y + 10;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public int showDebugPanel_Prefab(int x, int y)
  {
    PrefabInstance activePrefab = GameManager.Instance.GetDynamicPrefabDecorator()?.ActivePrefab;
    if (activePrefab == null)
      return y;
    Prefab.BlockStatistics blockStatistics = activePrefab.prefab.GetBlockStatistics();
    int num1 = 0;
    int num2 = y;
    int _boxWidth = 220;
    for (NGuiWdwDebugPanels.EGuiState _guiState = NGuiWdwDebugPanels.EGuiState.CalcSize; _guiState < NGuiWdwDebugPanels.EGuiState.Count; ++_guiState)
    {
      y = num2;
      this.PanelBoxWithHeader(_guiState, x, ref y, _boxWidth, num1 - num2 + 5, "Prefab");
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
        this.LabelWithOutline(x, y, "Name: " + activePrefab.prefab.PrefabName);
      y += 16 /*0x10*/;
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
        this.LabelWithOutline(x, y, "Pos: " + activePrefab.boundingBoxPosition.ToString());
      y += 16 /*0x10*/;
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
      {
        Vector3i vector3i = activePrefab.prefab.size;
        (SelectionCategory, SelectionBox)? selection = SelectionBoxManager.Instance.Selection;
        ref (SelectionCategory, SelectionBox)? local = ref selection;
        SelectionBox selectionBox = local.HasValue ? local.GetValueOrDefault().Item2 : (SelectionBox) null;
        if (Object.op_Inequality((Object) selectionBox, (Object) null))
          vector3i = selectionBox.GetScale();
        this.LabelWithOutline(x, y, "Size: " + vector3i.ToString());
      }
      y += 16 /*0x10*/;
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
      {
        this.LabelWithOutline(x, y, "Rot: " + activePrefab.rotation.ToString(), 70);
        this.LabelWithOutline(x, y, "RotToNorth: " + activePrefab.prefab.rotationToFaceNorth.ToString(), 130, _xOffset: 75);
      }
      y += 16 /*0x10*/;
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
        this.LabelWithOutline(x, y, $"BEnts: {blockStatistics.cntBlockEntities} BMods: {blockStatistics.cntBlockModels} Wdws: {blockStatistics.cntWindows}");
      y += 16 /*0x10*/;
      num1 = y;
    }
    return y + 10;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public int showDebugPanel_Stealth(int x, int y)
  {
    int num1 = 0;
    int num2 = y;
    int _boxWidth = 220;
    for (NGuiWdwDebugPanels.EGuiState _guiState = NGuiWdwDebugPanels.EGuiState.CalcSize; _guiState < NGuiWdwDebugPanels.EGuiState.Count; ++_guiState)
    {
      y = num2;
      this.PanelBoxWithHeader(_guiState, x, ref y, _boxWidth, num1 - num2 + 5, "Stealth");
      EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
      {
        float selfLight;
        float stealthLightLevel = LightManager.GetStealthLightLevel((EntityAlive) primaryPlayer, out selfLight);
        this.LabelWithOutline(x, y, $"Player light: {(int) ((double) stealthLightLevel * 100.0)} + {(int) ((double) selfLight * 100.0)}");
      }
      y += 16 /*0x10*/;
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
        this.LabelWithOutline(x, y, $"Light: {primaryPlayer.Stealth.lightLevel}");
      y += 16 /*0x10*/;
      if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
        this.LabelWithOutline(x, y, $"Noise: {primaryPlayer.Stealth.noiseVolume}");
      y += 16 /*0x10*/;
      num1 = y;
    }
    return y + 10;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public int showDebugPanel_Texture(int x, int y)
  {
    int num1 = 0;
    int num2 = y;
    int _boxWidth = 220;
    for (NGuiWdwDebugPanels.EGuiState _guiState = NGuiWdwDebugPanels.EGuiState.CalcSize; _guiState < NGuiWdwDebugPanels.EGuiState.Count; ++_guiState)
    {
      y = num2;
      this.PanelBoxWithHeader(_guiState, x, ref y, _boxWidth, num1 - num2 + 5, "Texture");
      bool streamingMipmapsActive = QualitySettings.streamingMipmapsActive;
      if (streamingMipmapsActive)
      {
        if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
          this.LabelWithOutline(x, y, $"Streaming mipmaps enabled: {streamingMipmapsActive}");
        y += 16 /*0x10*/;
        if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
          this.LabelWithOutline(x, y, $"Streaming budget: {QualitySettings.streamingMipmapsMemoryBudget} MB");
        y += 16 /*0x10*/;
        if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
          this.LabelWithOutline(x, y, $"Memory desired: {(double) Texture.desiredTextureMemory * 9.5367431640625E-07:F2} MB");
        y += 16 /*0x10*/;
        if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
          this.LabelWithOutline(x, y, $"Memory target: {(double) Texture.targetTextureMemory * 9.5367431640625E-07:F2} MB");
        y += 16 /*0x10*/;
        if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
          this.LabelWithOutline(x, y, $"Memory current: {(double) Texture.currentTextureMemory * 9.5367431640625E-07:F2} MB");
        y += 16 /*0x10*/;
        if (_guiState == NGuiWdwDebugPanels.EGuiState.Draw)
          this.LabelWithOutline(x, y, $"Non-streamed memory: {(double) Texture.nonStreamingTextureMemory * 9.5367431640625E-07:F2} MB");
        y += 16 /*0x10*/;
      }
      num1 = y;
    }
    return y + 10;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void panelManager()
  {
    double _v = (double) Screen.height / 1080.0 * (double) GameOptionsManager.GetActiveUiScale();
    float num1 = Utils.FastClamp((float) _v, 0.4f, 2f);
    Matrix4x4 matrix = GUI.matrix;
    GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(num1, num1, 1f));
    int num2 = this.showDebugPanel_EnablePanels(18, (int) ((double) ((GameManager.Instance.IsEditMode() ? 365 : 247) + 55) * (double) ((float) _v / num1)));
    for (int index = 0; index < this.Panels.Count; ++index)
    {
      NGuiWdwDebugPanels.PanelDefinition panel = this.Panels[index];
      if (panel.Enabled && panel.Active)
        num2 = panel.GuiHandler(18, num2);
    }
    if (!string.IsNullOrEmpty(GUI.tooltip))
    {
      Vector3 mousePosition = Input.mousePosition;
      mousePosition.y = (float) Screen.height - mousePosition.y;
      Vector3 vector3 = Vector3.op_Division(mousePosition, num1);
      vector3.y -= 20f;
      GUI.color = Color.white;
      GUI.Label(new Rect(vector3.x, vector3.y, 100f, 20f), GUI.tooltip ?? "", NGuiWdwDebugPanels.guiStyleTooltipLabel);
    }
    GUI.matrix = matrix;
  }

  public void SetActivePanels(params string[] panelCaptions)
  {
    foreach (NGuiWdwDebugPanels.PanelDefinition panel in this.Panels)
    {
      bool flag = false;
      foreach (string panelCaption in panelCaptions)
      {
        if (panel.ButtonCaption == panelCaption)
        {
          flag = true;
          break;
        }
      }
      panel.Active = flag;
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  static NGuiWdwDebugPanels()
  {
  }

  [CompilerGenerated]
  [PublicizedFrom(EAccessModifier.Private)]
  public void \u003CAwake\u003Eb__13_0()
  {
    this.debugData = this.debugData.CycleEnum<NGuiWdwDebugPanels.EDebugDataType>();
  }

  [CompilerGenerated]
  [PublicizedFrom(EAccessModifier.Private)]
  public void \u003CAwake\u003Eb__13_2()
  {
    this.performanceType = this.performanceType.CycleEnum<NGuiWdwDebugPanels.EPerformanceDisplayType>(NGuiWdwDebugPanels.EPerformanceDisplayType.Off, GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled) ? NGuiWdwDebugPanels.EPerformanceDisplayType.FpsAndNetGraphs : NGuiWdwDebugPanels.EPerformanceDisplayType.Fps);
    this.guiFPS.Enabled = this.performanceType != 0;
    this.guiFPS.ShowGraph = this.performanceType == NGuiWdwDebugPanels.EPerformanceDisplayType.FpsAndFpsGraph;
    this.networkMonitorCh0.Enabled = this.performanceType == NGuiWdwDebugPanels.EPerformanceDisplayType.FpsAndNetGraphs;
    this.networkMonitorCh1.Enabled = this.performanceType == NGuiWdwDebugPanels.EPerformanceDisplayType.FpsAndNetGraphs;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public enum EDebugDataType
  {
    Off,
    General,
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public enum EPerformanceDisplayType
  {
    Off,
    Fps,
    FpsAndHeat,
    FpsAndFpsGraph,
    FpsAndNetGraphs,
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public enum EGuiState
  {
    CalcSize,
    Draw,
    Count,
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public class PanelDefinition
  {
    public string Name;
    public string ButtonCaption;
    public Func<int, int, int> GuiHandler;
    public bool Enabled;
    public bool Active;

    public PanelDefinition(
      string _name,
      string _buttonCaption,
      Func<int, int, int> _guiHandler,
      string _enabledPanels,
      bool _enabled = true)
    {
      this.Name = _name;
      this.ButtonCaption = _buttonCaption;
      this.GuiHandler = _guiHandler;
      this.Enabled = _enabled;
      this.Active = _enabledPanels.Contains($",{_buttonCaption},");
    }
  }
}
