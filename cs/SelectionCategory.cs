// Decompiled with JetBrains decompiler
// Type: SelectionCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class SelectionCategory
{
  public readonly string name;
  [PublicizedFrom(EAccessModifier.Private)]
  public readonly Transform transform;
  public readonly Color colActive;
  public readonly Color colInactive;
  public readonly Color colFaceSelected;
  [PublicizedFrom(EAccessModifier.Private)]
  public readonly bool bCollider;
  public readonly string tag;
  [CompilerGenerated]
  [PublicizedFrom(EAccessModifier.Private)]
  public ISelectionBoxCallback \u003Ccallback\u003Ek__BackingField;
  [PublicizedFrom(EAccessModifier.Private)]
  public readonly int layer;
  public readonly Dictionary<string, SelectionBox> boxes = new Dictionary<string, SelectionBox>();

  public ISelectionBoxCallback callback
  {
    get => this.\u003Ccallback\u003Ek__BackingField;
    [PublicizedFrom(EAccessModifier.Private)] set
    {
      this.\u003Ccallback\u003Ek__BackingField = value;
    }
  }

  public SelectionCategory(
    string _name,
    Transform _transform,
    Color _colActive,
    Color _colInactive,
    Color _colFaceSelected,
    bool _bCollider,
    string _tag,
    ISelectionBoxCallback _callback,
    int _layer = 0)
  {
    this.name = _name;
    this.transform = _transform;
    this.colActive = _colActive;
    this.colInactive = _colInactive;
    this.colFaceSelected = _colFaceSelected;
    this.bCollider = _bCollider;
    this.tag = _tag;
    this.callback = _callback;
    this.layer = _layer;
  }

  public void SetCallback(ISelectionBoxCallback _callback) => this.callback = _callback;

  public bool IsVisible() => ((Component) this.transform).gameObject.activeSelf;

  public void SetVisible(bool _bVisible)
  {
    ((Component) this.transform).gameObject.SetActive(_bVisible);
    switch (this.name)
    {
      case "SleeperVolume":
        SleeperVolumeToolManager.SetVisible(_bVisible);
        break;
      case "POIMarker":
        POIMarkerToolManager.UpdateAllColors();
        int num;
        if (_bVisible)
        {
          (SelectionCategory, SelectionBox)? selection = SelectionBoxManager.Instance.Selection;
          ref (SelectionCategory, SelectionBox)? local = ref selection;
          num = (local.HasValue ? local.GetValueOrDefault().Item1 : (SelectionCategory) null) != null ? 1 : 0;
        }
        else
          num = 0;
        POIMarkerToolManager.ShowPOIMarkers(num != 0);
        break;
    }
    if (_bVisible)
      return;
    (SelectionCategory, SelectionBox)? selection1 = SelectionBoxManager.Instance.Selection;
    ref (SelectionCategory, SelectionBox)? local1 = ref selection1;
    if ((local1.HasValue ? local1.GetValueOrDefault().Item1 : (SelectionCategory) null) != this)
      return;
    SelectionBoxManager.Instance.Deactivate();
  }

  public void SetCaptionVisibility(bool _visible)
  {
    foreach (KeyValuePair<string, SelectionBox> box in this.boxes)
      box.Value.SetCaptionVisibility(_visible);
  }

  public void Clear()
  {
    foreach (KeyValuePair<string, SelectionBox> box in this.boxes)
      Object.Destroy((Object) ((Component) box.Value).gameObject);
    this.boxes.Clear();
    if (!(this.name == "SleeperVolume"))
      return;
    SleeperVolumeToolManager.ClearSleeperVolumes();
  }

  public SelectionBox AddBox(
    string _name,
    Vector3 _pos,
    Vector3i _size,
    bool _bDrawDirection = false,
    bool _bAlwaysDrawDirection = false)
  {
    if (this.boxes.TryGetValue(_name, out SelectionBox _))
      this.RemoveBox(_name);
    Transform transform = new GameObject(_name).transform;
    transform.parent = this.transform;
    SelectionBox _selBox = ((Component) transform).gameObject.AddComponent<SelectionBox>();
    _selBox.SetOwner(this);
    _selBox.SetAllFacesColor(this.colInactive);
    _selBox.bDrawDirection = _bDrawDirection;
    _selBox.bAlwaysDrawDirection = _bAlwaysDrawDirection;
    _selBox.SetPositionAndSize(_pos, _size);
    if (this.bCollider)
      _selBox.EnableCollider(this.tag, this.layer);
    this.boxes[_name] = _selBox;
    if (this.name == "SleeperVolume")
      SleeperVolumeToolManager.RegisterSleeperVolume(_selBox);
    return _selBox;
  }

  public SelectionBox GetBox(string _name)
  {
    SelectionBox box;
    this.boxes.TryGetValue(_name, out box);
    return box;
  }

  public void RenameBox(string _name, string _newName)
  {
    SelectionBox selectionBox;
    if (_name.Equals(_newName) || !this.boxes.TryGetValue(_name, out selectionBox))
      return;
    ((Object) selectionBox).name = _newName;
    this.boxes[_newName] = selectionBox;
    this.boxes.Remove(_name);
  }

  public void RemoveBox(string _name)
  {
    SelectionBox _selBox;
    if (!this.boxes.TryGetValue(_name, out _selBox))
      return;
    (SelectionCategory, SelectionBox)? selection = SelectionBoxManager.Instance.Selection;
    ref (SelectionCategory, SelectionBox)? local = ref selection;
    if (Object.op_Equality(local.HasValue ? (Object) local.GetValueOrDefault().Item2 : (Object) null, (Object) _selBox))
      SelectionBoxManager.Instance.Deactivate();
    if (this.name == "SleeperVolume")
      SleeperVolumeToolManager.UnRegisterSleeperVolume(_selBox);
    Object.Destroy((Object) ((Component) _selBox).gameObject);
    this.boxes.Remove(_name);
  }
}
