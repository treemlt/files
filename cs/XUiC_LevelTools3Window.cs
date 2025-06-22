// Decompiled with JetBrains decompiler
// Type: XUiC_LevelTools3Window
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
[Preserve]
public class XUiC_LevelTools3Window : XUiController
{
  public static string ID = "";
  [PublicizedFrom(EAccessModifier.Private)]
  public XUiC_SimpleButton buttonCopySleeperVolume;
  [PublicizedFrom(EAccessModifier.Private)]
  public readonly List<XUiC_LevelTools3Window.VolumeTypeDefinition> volumeTypeDefinitions = new List<XUiC_LevelTools3Window.VolumeTypeDefinition>()
  {
    new XUiC_LevelTools3Window.VolumeTypeDefinition("SleeperVolume", new Action<Vector3i, Vector3i>(PrefabSleeperVolumeManager.Instance.AddSleeperVolumeServer)),
    new XUiC_LevelTools3Window.VolumeTypeDefinition("TriggerVolume", new Action<Vector3i, Vector3i>(PrefabTriggerVolumeManager.Instance.AddTriggerVolumeServer)),
    new XUiC_LevelTools3Window.VolumeTypeDefinition("InfoVolume", new Action<Vector3i, Vector3i>(PrefabVolumeManager.Instance.AddInfoVolumeServer)),
    new XUiC_LevelTools3Window.VolumeTypeDefinition("TraderTeleport", new Action<Vector3i, Vector3i>(PrefabVolumeManager.Instance.AddTeleportVolumeServer)),
    new XUiC_LevelTools3Window.VolumeTypeDefinition("WallVolume", new Action<Vector3i, Vector3i>(PrefabVolumeManager.Instance.AddWallVolumeServer))
  };
  [PublicizedFrom(EAccessModifier.Private)]
  public XUiC_CategoryList volumeTypeSelector;
  [PublicizedFrom(EAccessModifier.Private)]
  public XUiC_SimpleButton btnVolumesCreate;
  [PublicizedFrom(EAccessModifier.Private)]
  public XUiC_SimpleButton btnVolumesCreateFromSelection;
  [PublicizedFrom(EAccessModifier.Private)]
  public XUiC_ToggleButton toggleVolumesShow;
  [PublicizedFrom(EAccessModifier.Private)]
  public XUiC_SimpleButton btnVolumesDupe;

  public override void Init()
  {
    base.Init();
    XUiC_LevelTools3Window.ID = this.WindowGroup.ID;
    this.buttonCopySleeperVolume = this.GetChildById("buttonCopySleeperVolume")?.GetChildByType<XUiC_SimpleButton>();
    if (this.buttonCopySleeperVolume != null && GameManager.Instance.GetActiveBlockTool() is BlockToolSelection activeBlockTool)
    {
      NGuiAction action;
      if (activeBlockTool.GetActions().TryGetValue("copySleeperVolume", out action))
      {
        string str = $"{action.GetText()} {action.GetHotkey().GetBindingXuiMarkupString(_displayStyle: XUiUtils.DisplayStyle.KeyboardWithParentheses)}";
        string tooltip = action.GetTooltip();
        this.buttonCopySleeperVolume.Text = str;
        this.buttonCopySleeperVolume.OnPressed += (XUiEvent_OnPressEventHandler) ([PublicizedFrom(EAccessModifier.Internal)] (_1, _2) => action.OnClick());
        this.buttonCopySleeperVolume.Tooltip = tooltip;
      }
    }
    this.volumeTypeSelector = this.GetChildById("volumeTypeSelector")?.GetChildByType<XUiC_CategoryList>();
    if (this.volumeTypeSelector != null)
      this.volumeTypeSelector.SetCategoryToFirst();
    this.btnVolumesCreate = this.GetChildById("btnVolumesCreate")?.GetChildByType<XUiC_SimpleButton>();
    if (this.btnVolumesCreate != null)
      this.btnVolumesCreate.OnPressed += new XUiEvent_OnPressEventHandler(this.BtnVolumesCreateOnOnPressed);
    this.btnVolumesCreateFromSelection = this.GetChildById("btnVolumesCreateFromSelection")?.GetChildByType<XUiC_SimpleButton>();
    if (this.btnVolumesCreateFromSelection != null)
      this.btnVolumesCreateFromSelection.OnPressed += new XUiEvent_OnPressEventHandler(this.BtnVolumesCreateFromSelectionOnOnPressed);
    this.toggleVolumesShow = this.GetChildById("toggleVolumesShow")?.GetChildByType<XUiC_ToggleButton>();
    if (this.toggleVolumesShow != null)
      this.toggleVolumesShow.OnValueChanged += new XUiEvent_ToggleButtonValueChanged(this.ToggleVolumesShowOnOnValueChanged);
    this.btnVolumesDupe = this.GetChildById("btnVolumesDupe")?.GetChildByType<XUiC_SimpleButton>();
    if (this.btnVolumesDupe == null)
      return;
    this.btnVolumesDupe.OnPressed += new XUiEvent_OnPressEventHandler(this.BtnVolumesDupeOnOnPressed);
  }

  public override void Update(float _dt)
  {
    base.Update(_dt);
    string _name = this.volumeTypeSelector.CurrentCategory?.CategoryName ?? "";
    SelectionCategory category = SelectionBoxManager.Instance.GetCategory(_name);
    XUiC_SimpleButton createFromSelection = this.btnVolumesCreateFromSelection;
    BlockToolSelection instance = BlockToolSelection.Instance;
    int num = instance != null ? (instance.SelectionActive ? 1 : 0) : 0;
    createFromSelection.Enabled = num != 0;
    this.toggleVolumesShow.Value = category != null && category.IsVisible();
    this.btnVolumesDupe.Enabled = !SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && false;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public bool volumeTypeByName(
    string _name,
    out XUiC_LevelTools3Window.VolumeTypeDefinition _result)
  {
    _result = this.volumeTypeDefinitions.Find((Predicate<XUiC_LevelTools3Window.VolumeTypeDefinition>) ([PublicizedFrom(EAccessModifier.Internal)] (_vtd) => _vtd.SelectionCategory.name == _name));
    return _result != null;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void BtnVolumesCreateOnOnPressed(XUiController _sender, int _mouseButton)
  {
    XUiC_LevelTools3Window.VolumeTypeDefinition _result;
    if (!this.volumeTypeByName(this.volumeTypeSelector.CurrentCategory?.CategoryName ?? "", out _result))
      return;
    XUiC_LevelTools3Window.addVolume(_result.AddVolumeHandler);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void BtnVolumesCreateFromSelectionOnOnPressed(XUiController _sender, int _mouseButton)
  {
    XUiC_LevelTools3Window.VolumeTypeDefinition _result;
    if (!this.volumeTypeByName(this.volumeTypeSelector.CurrentCategory?.CategoryName ?? "", out _result))
      return;
    XUiC_LevelTools3Window.addVolumeFromSelection(_result.AddVolumeHandler);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void ToggleVolumesShowOnOnValueChanged(XUiC_ToggleButton _sender, bool _newValue)
  {
    XUiC_LevelTools3Window.VolumeTypeDefinition _result;
    if (!this.volumeTypeByName(this.volumeTypeSelector.CurrentCategory?.CategoryName ?? "", out _result))
      return;
    SelectionCategory selectionCategory = _result.SelectionCategory;
    selectionCategory.SetVisible(!selectionCategory.IsVisible());
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void BtnVolumesDupeOnOnPressed(XUiController _sender, int _mouseButton)
  {
    throw new NotImplementedException();
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static void addVolume(Action<Vector3i, Vector3i> _addVolumeCallback)
  {
    Vector3 raycastHitPoint = XUiC_LevelTools3Window.getRaycastHitPoint();
    if (((Vector3) ref raycastHitPoint).Equals(Vector3.zero))
      return;
    Vector3i vector3i1 = new Vector3i(5, 4, 5);
    Vector3i vector3i2 = World.worldToBlockPos(raycastHitPoint) - new Vector3i(vector3i1.x / 2, 0, vector3i1.z / 2);
    _addVolumeCallback(vector3i2, vector3i1);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static void addVolumeFromSelection(Action<Vector3i, Vector3i> _addVolumeCallback)
  {
    BlockToolSelection instance = BlockToolSelection.Instance;
    if (instance == null || !instance.SelectionActive)
      return;
    _addVolumeCallback(instance.SelectionMin, instance.SelectionSize);
  }

  public static Vector3 getRaycastHitPoint(float _maxDistance = 100f, float _offsetUp = 0.0f)
  {
    Camera finalCamera = GameManager.Instance.World.GetPrimaryPlayer().finalCamera;
    Ray ray = finalCamera.ScreenPointToRay(new Vector3((float) Screen.width * 0.5f, (float) Screen.height * 0.5f, 0.0f));
    ref Ray local1 = ref ray;
    ((Ray) ref local1).origin = Vector3.op_Addition(((Ray) ref local1).origin, Origin.position);
    Transform transform = ((Component) finalCamera).transform;
    ref Ray local2 = ref ray;
    ((Ray) ref local2).origin = Vector3.op_Addition(((Ray) ref local2).origin, Vector3.op_Multiply(transform.forward, 0.1f));
    ref Ray local3 = ref ray;
    ((Ray) ref local3).origin = Vector3.op_Addition(((Ray) ref local3).origin, Vector3.op_Multiply(transform.up, _offsetUp));
    return Voxel.Raycast(GameManager.Instance.World, ray, _maxDistance, 4095 /*0x0FFF*/, 0.0f) ? Vector3.op_Subtraction(Voxel.voxelRayHitInfo.hit.pos, Vector3.op_Multiply(((Ray) ref ray).direction, 0.05f)) : Vector3.zero;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  static XUiC_LevelTools3Window()
  {
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public class VolumeTypeDefinition
  {
    public readonly SelectionCategory SelectionCategory;
    public readonly Action<Vector3i, Vector3i> AddVolumeHandler;

    public VolumeTypeDefinition(
      string _selectionCategoryName,
      Action<Vector3i, Vector3i> _addVolumeHandler)
    {
      this.SelectionCategory = SelectionBoxManager.Instance.GetCategory(_selectionCategoryName);
      this.AddVolumeHandler = _addVolumeHandler;
    }
  }
}
