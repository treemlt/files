// Decompiled with JetBrains decompiler
// Type: SelectionBoxManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using Audio;
using InControl;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SelectionBoxManager : MonoBehaviour
{
  public const string CategoryDynamicPrefab = "DynamicPrefabs";
  public const string CategoryStartPoint = "StartPoint";
  public const string CategorySelection = "Selection";
  public const string CategoryTraderTeleport = "TraderTeleport";
  public const string CategoryInfoVolume = "InfoVolume";
  public const string CategoryWallVolume = "WallVolume";
  public const string CategoryTriggerVolume = "TriggerVolume";
  public const string CategorySleeperVolume = "SleeperVolume";
  public const string CategoryPOIMarker = "POIMarker";
  public const string CategoryPrefabFacingVolume = "PrefabFacing";
  public static Color ColDynamicPrefabInactive = new Color(0.0f, 0.4f, 0.0f, 0.6f);
  public static Color ColDynamicPrefabActive = new Color(0.6f, 1f, 0.0f, 0.15f);
  public static Color ColDynamicPrefabFaceSel = new Color(0.0f, 1f, 0.0f, 0.6f);
  public static Color ColEntitySpawnerInactive = new Color(0.6f, 0.0f, 0.0f, 0.6f);
  public static Color ColEntitySpawnerActive = new Color(1f, 0.0f, 0.0f, 0.4f);
  public static Color ColEntitySpawnerFaceSel = new Color(1f, 1f, 0.0f, 0.3f);
  public static Color ColEntitySpawnerTrigger = new Color(1f, 1f, 0.0f, 0.3f);
  public static Color ColStartPointInactive = new Color(1f, 1f, 1f, 0.5f);
  public static Color ColStartPointActive = new Color(1f, 1f, 0.0f, 0.8f);
  public static Color ColSelectionActive = new Color(0.0f, 0.0f, 1f, 0.5f);
  public static Color ColSelectionInactive = new Color(0.0f, 0.0f, 1f, 0.5f);
  public static Color ColSelectionFaceSel = new Color(1f, 1f, 0.0f, 0.4f);
  public static Color ColTraderTeleportInactive = new Color(0.5f, 0.0f, 0.5f, 0.6f);
  public static Color ColTraderTeleport = new Color(1f, 0.0f, 1f, 0.3f);
  public static Color ColSleeperVolume = new Color(0.7f, 0.75f, 1f, 0.3f);
  public static Color ColSleeperVolumeInactive = new Color(0.25f, 0.25f, 0.5f, 0.6f);
  public static Color ColTriggerVolume = new Color(1f, 0.0f, 0.0f, 0.4f);
  public static Color ColTriggerVolumeInactive = new Color(0.6f, 0.0f, 0.0f, 0.6f);
  public static Color ColInfoVolume = new Color(0.0f, 1f, 1f, 0.4f);
  public static Color ColInfoVolumeInactive = new Color(0.0f, 0.6f, 0.6f, 0.6f);
  public static Color ColWallVolume = new Color(0.5f, 1f, 1f, 0.4f);
  public static Color ColWallVolumeInactive = new Color(0.5f, 0.6f, 0.6f, 0.6f);
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public readonly Dictionary<string, SelectionCategory> categories = new Dictionary<string, SelectionCategory>();
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public (SelectionCategory category, SelectionBox box)? selection;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public float alphaMultiplier = 1f;
  public static SelectionBoxManager Instance;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool bMousedPressed;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public Vector3 highlightedAxisScreenDir;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public Vector3i highlightedAxis = Vector3i.zero;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public Vector3 mouseMoveDir = Vector3.zero;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int lastSelOpMode;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool bWaitForRelease;

  public (SelectionCategory category, SelectionBox box)? Selection
  {
    get
    {
      if (!this.selection.HasValue)
        return new (SelectionCategory, SelectionBox)?();
      return Object.op_Equality((Object) this.selection.Value.box, (Object) null) ? new (SelectionCategory, SelectionBox)?() : new (SelectionCategory, SelectionBox)?(this.selection.Value);
    }
    [PublicizedFrom(EAccessModifier.Private)] set => this.selection = value;
  }

  public float AlphaMultiplier
  {
    get => this.alphaMultiplier;
    set
    {
      this.alphaMultiplier = Mathf.Clamp01(value);
      GamePrefs.Set(EnumGamePrefs.OptionsSelectionBoxAlphaMultiplier, this.alphaMultiplier);
      this.UpdateAllColors();
    }
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public void Awake()
  {
    SelectionBoxManager.Instance = this;
    Origin.Add(((Component) this).transform, 1);
    this.alphaMultiplier = GamePrefs.GetFloat(EnumGamePrefs.OptionsSelectionBoxAlphaMultiplier);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void OnDestroy() => Origin.Remove(((Component) this).transform);

  public Dictionary<string, SelectionCategory> GetCategories() => this.categories;

  public SelectionCategory GetCategory(string _name)
  {
    SelectionCategory category;
    this.categories.TryGetValue(_name, out category);
    return category;
  }

  public bool TryGetSelectionBox(string _category, string _name, out SelectionBox _selectionBox)
  {
    _selectionBox = this.GetCategory(_category)?.GetBox(_name);
    return Object.op_Inequality((Object) _selectionBox, (Object) null);
  }

  public void CreateCategory(
    string _name,
    Color _colSelected,
    Color _colUnselected,
    Color _colFaceSelected,
    bool _bCollider,
    string _tag,
    int _layer = 0)
  {
    Transform transform = new GameObject(_name).transform;
    transform.parent = ((Component) this).transform;
    SelectionCategory selectionCategory = new SelectionCategory(_name, transform, _colSelected, _colUnselected, _colFaceSelected, _bCollider, _tag, (ISelectionBoxCallback) null, _layer);
    selectionCategory.SetVisible(false);
    this.categories[_name] = selectionCategory;
  }

  public void SetUserData(string _category, string _name, object _data)
  {
    SelectionBox _selectionBox;
    if (this.TryGetSelectionBox(_category, _name, out _selectionBox))
      _selectionBox.UserData = _data;
    this.UpdateSleepersAndMarkers();
  }

  public bool IsActive(string _category, string _name)
  {
    SelectionBox _selectionBox;
    if (!this.TryGetSelectionBox(_category, _name, out _selectionBox))
      return false;
    (SelectionCategory, SelectionBox)? selection = this.Selection;
    ref (SelectionCategory, SelectionBox)? local = ref selection;
    return Object.op_Equality(local.HasValue ? (Object) local.GetValueOrDefault().Item2 : (Object) null, (Object) _selectionBox);
  }

  public void SetActive(string _category, string _name, bool _bActive)
  {
    SelectionCategory category = this.GetCategory(_category);
    if (!category.IsVisible())
      category.SetVisible(true);
    SelectionBox _selectionBox;
    if (!this.TryGetSelectionBox(_category, _name, out _selectionBox))
      return;
    this.activate(this.categories[_category], _bActive ? _selectionBox : (SelectionBox) null);
  }

  public void SetFacingDirection(string _category, string _name, float _facing)
  {
    SelectionCategory category = this.GetCategory(_category);
    if (!category.IsVisible())
      category.SetVisible(true);
    SelectionBox _selectionBox;
    if (!this.TryGetSelectionBox(_category, _name, out _selectionBox))
      return;
    _selectionBox.facingDirection = _facing;
  }

  public void Deactivate() => this.activate((SelectionCategory) null, (SelectionBox) null);

  public bool GetSelected(out string _selectedCategory, out string _selectedName)
  {
    if (this.Selection.HasValue)
    {
      _selectedCategory = this.Selection.Value.category.name;
      _selectedName = ((Object) this.Selection.Value.box).name;
      return true;
    }
    _selectedCategory = (string) null;
    _selectedName = (string) null;
    return false;
  }

  public void Unselect()
  {
    this.Selection = new (SelectionCategory, SelectionBox)?();
    this.UpdateSleepersAndMarkers();
  }

  public bool Select(WorldRayHitInfo _hitInfo)
  {
    if (_hitInfo.tag == null)
      return false;
    foreach (KeyValuePair<string, SelectionCategory> category in this.categories)
    {
      if (_hitInfo.tag.Equals(category.Value.tag))
      {
        foreach (KeyValuePair<string, SelectionBox> box in category.Value.boxes)
        {
          if (Object.op_Equality((Object) box.Value.GetBoxTransform(), (Object) _hitInfo.transform))
          {
            if (category.Value.name != "SleeperVolume")
              SleeperVolumeToolManager.ShowSleepers(false);
            Manager.PlayButtonClick();
            return this.activate(category.Value, box.Value);
          }
        }
      }
    }
    return false;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public bool activate(SelectionCategory _cat, SelectionBox _box)
  {
    (SelectionCategory category, SelectionBox box)? selection = this.Selection;
    bool flag = true;
    (SelectionCategory, SelectionBox)? nullable = this.Selection;
    ref (SelectionCategory, SelectionBox)? local = ref nullable;
    if (Object.op_Equality(local.HasValue ? (Object) local.GetValueOrDefault().Item2 : (Object) null, (Object) _box) || Object.op_Equality((Object) _box, (Object) null))
    {
      nullable = new (SelectionCategory, SelectionBox)?();
      this.Selection = nullable;
    }
    else
    {
      this.Selection = new (SelectionCategory, SelectionBox)?((_cat, _box));
      _box.SetFrameActive(true);
      _box.SetAllFacesColor(_cat.colActive);
    }
    if (selection.HasValue)
    {
      selection.Value.box.SetFrameActive(false);
      selection.Value.box.SetAllFacesColor(selection.Value.category.colInactive);
      selection.Value.category.callback?.OnSelectionBoxActivated(selection.Value.category.name, ((Object) selection.Value.box).name, false);
    }
    nullable = this.Selection;
    if (nullable.HasValue)
    {
      nullable = this.Selection;
      if (nullable.Value.Item1.callback != null)
      {
        nullable = this.Selection;
        ISelectionBoxCallback callback = nullable.Value.Item1.callback;
        nullable = this.Selection;
        string name1 = nullable.Value.Item1.name;
        nullable = this.Selection;
        string name2 = ((Object) nullable.Value.Item2).name;
        flag = callback.OnSelectionBoxActivated(name1, name2, true);
      }
    }
    this.UpdateSleepersAndMarkers();
    return flag;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void UpdateSleepersAndMarkers()
  {
    if (!this.Selection.HasValue)
    {
      SleeperVolumeToolManager.SelectionChanged((SelectionBox) null);
      POIMarkerToolManager.SelectionChanged((SelectionBox) null);
    }
    else
    {
      (SelectionCategory category, SelectionBox selectionBox) = this.Selection.Value;
      if (category.name.Equals("SleeperVolume"))
      {
        SleeperVolumeToolManager.SelectionChanged(selectionBox);
      }
      else
      {
        if (!category.name.Equals("POIMarker"))
          return;
        POIMarkerToolManager.SelectionChanged(selectionBox);
      }
    }
  }

  public void UpdateAllColors()
  {
    foreach (KeyValuePair<string, SelectionCategory> category in this.categories)
    {
      foreach (KeyValuePair<string, SelectionBox> box in category.Value.boxes)
      {
        (SelectionCategory, SelectionBox)? selection = this.Selection;
        ref (SelectionCategory, SelectionBox)? local = ref selection;
        Color _c = Object.op_Equality(local.HasValue ? (Object) local.GetValueOrDefault().Item2 : (Object) null, (Object) box.Value) ? category.Value.colActive : category.Value.colInactive;
        box.Value.SetAllFacesColor(_c);
      }
    }
  }

  public void Clear()
  {
    foreach (KeyValuePair<string, SelectionCategory> category in this.categories)
      category.Value.Clear();
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public Vector3i createBlockMoveVector(Vector3 _relPlayerAxis)
  {
    return (double) Math.Abs(_relPlayerAxis.x) <= (double) Math.Abs(_relPlayerAxis.z) ? new Vector3i(0.0f, 0.0f, Mathf.Sign(_relPlayerAxis.z)) : new Vector3i(Mathf.Sign(_relPlayerAxis.x), 0.0f, 0.0f);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void moveSelection(Vector3i _deltaVec)
  {
    (SelectionCategory, SelectionBox)? selection = this.Selection;
    ref (SelectionCategory, SelectionBox)? local = ref selection;
    if ((local.HasValue ? local.GetValueOrDefault().Item1.callback : (ISelectionBoxCallback) null) == null)
      return;
    selection = this.Selection;
    ISelectionBoxCallback callback = selection.Value.Item1.callback;
    selection = this.Selection;
    string name1 = selection.Value.Item1.name;
    selection = this.Selection;
    string name2 = ((Object) selection.Value.Item2).name;
    Vector3 vector3 = _deltaVec.ToVector3();
    callback.OnSelectionBoxMoved(name1, name2, vector3);
    this.UpdateSleepersAndMarkers();
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void incSelection(
    int _dTop,
    int _dBottom,
    int _dNorth,
    int _dSouth,
    int _dEast,
    int _dWest)
  {
    (SelectionCategory, SelectionBox)? selection = this.Selection;
    ref (SelectionCategory, SelectionBox)? local = ref selection;
    if ((local.HasValue ? local.GetValueOrDefault().Item1.callback : (ISelectionBoxCallback) null) == null)
      return;
    selection = this.Selection;
    ISelectionBoxCallback callback1 = selection.Value.Item1.callback;
    selection = this.Selection;
    string name1 = selection.Value.Item1.name;
    if (!callback1.OnSelectionBoxIsAvailable(name1, EnumSelectionBoxAvailabilities.CanResize))
      return;
    selection = this.Selection;
    ISelectionBoxCallback callback2 = selection.Value.Item1.callback;
    selection = this.Selection;
    string name2 = selection.Value.Item1.name;
    selection = this.Selection;
    string name3 = ((Object) selection.Value.Item2).name;
    int _dTop1 = _dTop;
    int _dBottom1 = _dBottom;
    int _dNorth1 = _dNorth;
    int _dSouth1 = _dSouth;
    int _dEast1 = _dEast;
    int _dWest1 = _dWest;
    callback2.OnSelectionBoxSized(name2, name3, _dTop1, _dBottom1, _dNorth1, _dSouth1, _dEast1, _dWest1);
    this.UpdateSleepersAndMarkers();
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void mirrorSelection(Vector3i _axis)
  {
    (SelectionCategory, SelectionBox)? selection = this.Selection;
    ref (SelectionCategory, SelectionBox)? local = ref selection;
    if ((local.HasValue ? local.GetValueOrDefault().Item1.callback : (ISelectionBoxCallback) null) == null)
      return;
    selection = this.Selection;
    ISelectionBoxCallback callback = selection.Value.Item1.callback;
    selection = this.Selection;
    string name = selection.Value.Item1.name;
    if (!callback.OnSelectionBoxIsAvailable(name, EnumSelectionBoxAvailabilities.CanMirror))
      return;
    selection = this.Selection;
    selection.Value.Item1.callback.OnSelectionBoxMirrored(_axis);
    this.UpdateSleepersAndMarkers();
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void ShowThroughWalls(string _categoryName, bool _isThroughWalls, bool _isAll)
  {
    if (_isAll)
    {
      foreach (KeyValuePair<string, SelectionBox> box in this.categories[_categoryName].boxes)
        box.Value.ShowThroughWalls(_isThroughWalls);
    }
    else
    {
      (SelectionCategory, SelectionBox)? selection = this.Selection;
      ref (SelectionCategory, SelectionBox)? local = ref selection;
      if (!local.HasValue)
        return;
      local.GetValueOrDefault().Item2.ShowThroughWalls(_isThroughWalls);
    }
  }

  public bool ConsumeScrollWheel(float _scrollWheel, PlayerActionsLocal _playerActions)
  {
    (SelectionCategory, SelectionBox)? selection = this.Selection;
    ref (SelectionCategory, SelectionBox)? local = ref selection;
    if ((local.HasValue ? local.GetValueOrDefault().Item1.callback : (ISelectionBoxCallback) null) == null || (double) Mathf.Abs(_scrollWheel) < 0.10000000149011612)
      return false;
    int num = Mathf.RoundToInt(_scrollWheel * 10f);
    bool flag = false;
    bool controlKeyPressed = InputUtils.ControlKeyPressed;
    if (GamePrefs.GetInt(EnumGamePrefs.SelectionOperationMode) == 2)
    {
      if (((OneAxisInputControl) _playerActions.Jump).IsPressed)
      {
        this.incSelection(num, 0, 0, 0, 0, 0);
        flag = true;
      }
      if (((OneAxisInputControl) _playerActions.Crouch).IsPressed && !controlKeyPressed)
      {
        this.incSelection(0, num, 0, 0, 0, 0);
        flag = true;
      }
      if (((OneAxisInputControl) _playerActions.MoveLeft).IsPressed)
      {
        this.incSelection(0, 0, 0, 0, num, 0);
        flag = true;
      }
      if (((OneAxisInputControl) _playerActions.MoveRight).IsPressed)
      {
        this.incSelection(0, 0, 0, 0, 0, num);
        flag = true;
      }
      if (((OneAxisInputControl) _playerActions.MoveForward).IsPressed)
      {
        this.incSelection(0, 0, num, 0, 0, 0);
        flag = true;
      }
      if (((OneAxisInputControl) _playerActions.MoveBack).IsPressed)
      {
        this.incSelection(0, 0, 0, num, 0, 0);
        flag = true;
      }
    }
    return flag;
  }

  public void CheckKeys(
    GameManager _gameManager,
    PlayerActionsLocal _playerActions,
    WorldRayHitInfo _hitInfo)
  {
    bool altKeyPressed = InputUtils.AltKeyPressed;
    GameManager.bVolumeBlocksEditing = !altKeyPressed;
    foreach (KeyValuePair<string, SelectionCategory> category in this.categories)
    {
      string str;
      SelectionCategory selectionCategory;
      category.Deconstruct(ref str, ref selectionCategory);
      string _categoryName = str;
      if (selectionCategory.IsVisible())
        this.ShowThroughWalls(_categoryName, altKeyPressed, true);
    }
    if (((OneAxisInputControl) _playerActions.SelectionRotate).WasPressed && !Input.GetKey((KeyCode) 9))
    {
      (SelectionCategory, SelectionBox)? selection = this.Selection;
      ref (SelectionCategory, SelectionBox)? local = ref selection;
      if ((local.HasValue ? local.GetValueOrDefault().Item1.callback : (ISelectionBoxCallback) null) == null)
      {
        BlockToolSelection.Instance.RotateFocusedBlock(_hitInfo, _playerActions);
      }
      else
      {
        (SelectionCategory category, SelectionBox box) = this.Selection.Value;
        category.callback.OnSelectionBoxRotated(category.name, ((Object) box).name);
      }
    }
    (SelectionCategory, SelectionBox)? selection1 = this.Selection;
    ref (SelectionCategory, SelectionBox)? local1 = ref selection1;
    if ((local1.HasValue ? local1.GetValueOrDefault().Item1.callback : (ISelectionBoxCallback) null) == null)
      return;
    (SelectionCategory category1, SelectionBox box1) = this.Selection.Value;
    bool controlKeyPressed = InputUtils.ControlKeyPressed;
    if (GamePrefs.GetInt(EnumGamePrefs.SelectionOperationMode) == 1 && GamePrefs.GetInt(EnumGamePrefs.SelectionContextMode) == 1)
    {
      if (((OneAxisInputControl) _playerActions.MoveBack).WasPressed)
        this.moveSelection(-1 * this.createBlockMoveVector(((Component) _gameManager.World.GetPrimaryPlayer()).transform.forward));
      if (((OneAxisInputControl) _playerActions.MoveForward).WasPressed)
        this.moveSelection(this.createBlockMoveVector(((Component) _gameManager.World.GetPrimaryPlayer()).transform.forward));
      if (((OneAxisInputControl) _playerActions.MoveLeft).WasPressed)
        this.moveSelection(-1 * this.createBlockMoveVector(((Component) _gameManager.World.GetPrimaryPlayer()).transform.right));
      if (((OneAxisInputControl) _playerActions.MoveRight).WasPressed)
        this.moveSelection(this.createBlockMoveVector(((Component) _gameManager.World.GetPrimaryPlayer()).transform.right));
    }
    else if (GamePrefs.GetInt(EnumGamePrefs.SelectionOperationMode) == 1 && GamePrefs.GetInt(EnumGamePrefs.SelectionContextMode) == 0)
    {
      if (((OneAxisInputControl) _playerActions.MoveBack).WasPressed)
        this.moveSelection(-1 * Vector3i.forward);
      if (((OneAxisInputControl) _playerActions.MoveForward).WasPressed)
        this.moveSelection(Vector3i.forward);
      if (((OneAxisInputControl) _playerActions.MoveLeft).WasPressed)
        this.moveSelection(-1 * Vector3i.right);
      if (((OneAxisInputControl) _playerActions.MoveRight).WasPressed)
        this.moveSelection(Vector3i.right);
    }
    if (GamePrefs.GetInt(EnumGamePrefs.SelectionOperationMode) == 1)
    {
      if (((OneAxisInputControl) _playerActions.Jump).WasPressed)
        this.moveSelection(new Vector3i(0, 1, 0));
      if (((OneAxisInputControl) _playerActions.Crouch).WasPressed && !controlKeyPressed)
        this.moveSelection(new Vector3i(0, -1, 0));
    }
    if (((OneAxisInputControl) _playerActions.SelectionMoveMode).WasPressed)
      GamePrefs.Set(EnumGamePrefs.SelectionContextMode, (GamePrefs.GetInt(EnumGamePrefs.SelectionContextMode) + 1) % 2);
    if (GamePrefs.GetInt(EnumGamePrefs.SelectionOperationMode) == 2)
    {
      Color colFaceSelected = category1.colFaceSelected;
      if (((OneAxisInputControl) _playerActions.Jump).WasPressed)
        box1.SetFaceColor(BlockFace.Top, colFaceSelected);
      else if (((OneAxisInputControl) _playerActions.Jump).WasReleased)
        box1.ResetAllFacesColor();
      if (((OneAxisInputControl) _playerActions.Crouch).WasPressed && !controlKeyPressed)
        box1.SetFaceColor(BlockFace.Bottom, colFaceSelected);
      else if (((OneAxisInputControl) _playerActions.Crouch).WasReleased)
        box1.ResetAllFacesColor();
      if (((OneAxisInputControl) _playerActions.MoveLeft).WasPressed)
        box1.SetFaceColor(BlockFace.East, colFaceSelected);
      else if (((OneAxisInputControl) _playerActions.MoveLeft).WasReleased)
        box1.ResetAllFacesColor();
      if (((OneAxisInputControl) _playerActions.MoveRight).WasPressed)
        box1.SetFaceColor(BlockFace.West, colFaceSelected);
      else if (((OneAxisInputControl) _playerActions.MoveRight).WasReleased)
        box1.ResetAllFacesColor();
      if (((OneAxisInputControl) _playerActions.MoveForward).WasPressed)
        box1.SetFaceColor(BlockFace.North, colFaceSelected);
      else if (((OneAxisInputControl) _playerActions.MoveForward).WasReleased)
        box1.ResetAllFacesColor();
      if (((OneAxisInputControl) _playerActions.MoveBack).WasPressed)
        box1.SetFaceColor(BlockFace.South, colFaceSelected);
      else if (((OneAxisInputControl) _playerActions.MoveBack).WasReleased)
        box1.ResetAllFacesColor();
    }
    if (((OneAxisInputControl) _playerActions.SelectionDelete).WasPressed)
    {
      this.SetActive(category1.name, ((Object) box1).name, false);
      if (category1.callback.OnSelectionBoxDelete(category1.name, ((Object) box1).name))
      {
        Manager.PlayButtonClick();
        category1.RemoveBox(((Object) box1).name);
        this.Selection = new (SelectionCategory, SelectionBox)?();
      }
    }
    (SelectionCategory, SelectionBox)? selection2 = this.Selection;
    ref (SelectionCategory, SelectionBox)? local2 = ref selection2;
    if ((local2.HasValue ? (local2.GetValueOrDefault().Item1.name.Equals("SleeperVolume") ? 1 : 0) : 0) == 0)
      return;
    SleeperVolumeToolManager.CheckKeys();
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public void Update()
  {
    EntityPlayerLocal primaryPlayer;
    if (!this.Selection.HasValue || Object.op_Equality((Object) GameManager.Instance, (Object) null) || GameManager.Instance.World == null || Object.op_Equality((Object) (primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer()), (Object) null))
      return;
    Camera finalCamera = primaryPlayer.finalCamera;
    if (Object.op_Equality((Object) finalCamera, (Object) null))
      return;
    SelectionBox box = this.Selection.Value.box;
    if (box.Axises.Count == 0)
      return;
    if (this.lastSelOpMode != GamePrefs.GetInt(EnumGamePrefs.SelectionOperationMode))
    {
      this.lastSelOpMode = GamePrefs.GetInt(EnumGamePrefs.SelectionOperationMode);
      this.mouseMoveDir = Vector3.zero;
    }
    Vector3 mousePosition = Input.mousePosition;
    mousePosition.z = 0.0f;
    Vector3 screenPoint1 = finalCamera.WorldToScreenPoint(box.AxisOrigin);
    screenPoint1.z = 0.0f;
    bool flag = true;
    Vector3 vector3;
    if (!this.bMousedPressed)
    {
      for (int index = 0; index < box.Axises.Count; ++index)
      {
        Vector3 screenPoint2 = finalCamera.WorldToScreenPoint(box.Axises[index]);
        screenPoint2.z = 0.0f;
        if ((double) this.GetLineDistanceSq(screenPoint1, screenPoint2, mousePosition) < 225.0)
        {
          this.highlightedAxis = box.AxisesI[index];
          vector3 = Vector3.op_Subtraction(screenPoint1, screenPoint2);
          this.highlightedAxisScreenDir = ((Vector3) ref vector3).normalized;
          flag = false;
          break;
        }
      }
    }
    if (!this.bMousedPressed & flag)
      this.highlightedAxis = Vector3i.zero;
    box.HighlightAxis(this.highlightedAxis);
    if (this.bWaitForRelease && Input.GetMouseButton(0))
      return;
    this.bWaitForRelease = false;
    if (!this.highlightedAxis.Equals(Vector3i.zero))
    {
      if (!this.bMousedPressed && Input.GetMouseButtonDown(0))
        this.bMousedPressed = true;
      vector3 = Vector3.op_Subtraction(primaryPlayer.cameraTransform.position, box.AxisOrigin);
      float num1 = Math.Max(0.5f, ((Vector3) ref vector3).magnitude / 35f);
      this.mouseMoveDir = Vector3.op_Addition(this.mouseMoveDir, Vector3.op_Multiply(new Vector3((float) (-(double) Input.GetAxis("Mouse X") * 5.0), (float) (-(double) Input.GetAxis("Mouse Y") * 5.0), 0.0f), num1));
      float magnitude = ((Vector3) ref this.mouseMoveDir).magnitude;
      if (this.bMousedPressed && !Input.GetMouseButtonUp(0) && GamePrefs.GetInt(EnumGamePrefs.SelectionOperationMode) == 3)
      {
        this.mirrorSelection(this.highlightedAxis);
        this.bWaitForRelease = true;
      }
      if (this.bMousedPressed && (double) magnitude > 5.0)
      {
        float num2 = (!this.highlightedAxis.Equals(Vector3i.one) ? Vector3.Dot(this.highlightedAxisScreenDir, this.mouseMoveDir) : -1f * Mathf.Sign(this.mouseMoveDir.y)) * (magnitude * 0.05f);
        if ((double) Mathf.Abs(num2) > 1.0)
        {
          this.mouseMoveDir = Vector3.zero;
          int num3 = (int) Mathf.Sign(num2);
          if (GamePrefs.GetInt(EnumGamePrefs.SelectionOperationMode) == 1)
            this.moveSelection(this.highlightedAxis * num3);
          else if (GamePrefs.GetInt(EnumGamePrefs.SelectionOperationMode) == 2)
            this.incSelection(this.highlightedAxis.y > 0 ? this.highlightedAxis.y * num3 : 0, this.highlightedAxis.y < 0 ? -1 * this.highlightedAxis.y * num3 : 0, this.highlightedAxis.z > 0 ? this.highlightedAxis.z * num3 : 0, this.highlightedAxis.z < 0 ? -1 * this.highlightedAxis.z * num3 : 0, this.highlightedAxis.x > 0 ? this.highlightedAxis.x * num3 : 0, this.highlightedAxis.x < 0 ? -1 * this.highlightedAxis.x * num3 : 0);
        }
      }
    }
    if (Input.GetMouseButton(0))
      return;
    this.bMousedPressed = false;
    this.mouseMoveDir = Vector3.zero;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public float GetLineDistanceSq(Vector3 _lineStart, Vector3 _lineEnd, Vector3 _point)
  {
    Vector3 vector3_1 = Vector3.op_Subtraction(_lineEnd, _lineStart);
    float sqrMagnitude = ((Vector3) ref vector3_1).sqrMagnitude;
    if ((double) sqrMagnitude < 9.9999999747524271E-07)
    {
      Vector3 vector3_2 = Vector3.op_Subtraction(_point, _lineStart);
      return ((Vector3) ref vector3_2).sqrMagnitude;
    }
    float num = Mathf.Clamp01(Vector3.Dot(Vector3.op_Subtraction(_point, _lineStart), vector3_1) / sqrMagnitude);
    Vector3 vector3_3 = Vector3.op_Subtraction(Vector3.op_Addition(_lineStart, Vector3.op_Multiply(vector3_1, num)), _point);
    return ((Vector3) ref vector3_3).sqrMagnitude;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  static SelectionBoxManager()
  {
  }
}
