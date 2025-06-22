// Decompiled with JetBrains decompiler
// Type: XUiC_PrefabTriggerEditorList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

#nullable disable
[Preserve]
public class XUiC_PrefabTriggerEditorList : 
  XUiC_List<XUiC_PrefabTriggerEditorList.PrefabTriggerEntry>
{
  public Prefab EditPrefab;
  public XUiC_TriggerProperties Owner;
  public XUiC_WoPropsSleeperVolume SleeperOwner;
  public bool IsTriggers;
  [PublicizedFrom(EAccessModifier.Private)]
  public readonly List<string> groupsResult = new List<string>();

  public override void OnOpen()
  {
    base.OnOpen();
    this.RebuildList(false);
  }

  public override void RebuildList(bool _resetFilter = false)
  {
    this.allEntries.Clear();
    this.groupsResult.Clear();
    if (this.EditPrefab != null)
    {
      List<byte> triggerLayers = this.EditPrefab.TriggerLayers;
      for (int index = 0; index < triggerLayers.Count; ++index)
        this.allEntries.Add(new XUiC_PrefabTriggerEditorList.PrefabTriggerEntry(this, triggerLayers[index]));
    }
    this.allEntries.Sort();
    base.RebuildList(_resetFilter);
  }

  [Preserve]
  public class PrefabTriggerEntry : XUiListEntry<XUiC_PrefabTriggerEditorList.PrefabTriggerEntry>
  {
    [PublicizedFrom(EAccessModifier.Private)]
    public readonly XUiC_PrefabTriggerEditorList parentList;
    public readonly string name;
    public byte TriggerLayer;

    public PrefabTriggerEntry(XUiC_PrefabTriggerEditorList _parentList, byte _triggerLayer)
    {
      this.parentList = _parentList;
      this.TriggerLayer = _triggerLayer;
      this.name = _triggerLayer.ToString();
    }

    public override int CompareTo(
      XUiC_PrefabTriggerEditorList.PrefabTriggerEntry _otherEntry)
    {
      return _otherEntry == null ? 1 : this.TriggerLayer.CompareTo(_otherEntry.TriggerLayer);
    }

    [PublicizedFrom(EAccessModifier.Private)]
    public bool GetSelected()
    {
      bool selected = false;
      if (this.parentList.Owner != null)
      {
        if (this.parentList.Owner.blockTrigger != null || this.parentList.Owner.TriggerVolume != null)
        {
          if (this.parentList.IsTriggers)
          {
            if (this.parentList.Owner.TriggersIndices != null)
              selected = this.parentList.Owner.TriggersIndices.Contains(StringParsers.ParseUInt8(this.name));
          }
          else if (this.parentList.Owner != null)
          {
            if (this.parentList.Owner.TriggeredByIndices != null)
              selected = this.parentList.Owner.TriggeredByIndices.Contains(StringParsers.ParseUInt8(this.name));
          }
          else if (this.parentList.SleeperOwner != null && this.parentList.SleeperOwner.TriggeredByIndices != null)
            selected = this.parentList.SleeperOwner.TriggeredByIndices.Contains(StringParsers.ParseUInt8(this.name));
        }
      }
      else if (this.parentList.SleeperOwner != null && !this.parentList.IsTriggers && this.parentList.SleeperOwner != null && this.parentList.SleeperOwner.TriggeredByIndices != null)
        selected = this.parentList.SleeperOwner.TriggeredByIndices.Contains(StringParsers.ParseUInt8(this.name));
      return selected;
    }

    public override bool GetBindingValue(ref string _value, string _bindingName)
    {
      switch (_bindingName)
      {
        case "name":
          _value = this.name;
          return true;
        case "selected":
          _value = this.GetSelected() ? "true" : "false";
          return true;
        case "assigned":
          _value = "true";
          return true;
        default:
          return false;
      }
    }

    public override bool MatchesSearch(string _searchString)
    {
      return this.name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0;
    }

    public static bool GetNullBindingValues(ref string _value, string _bindingName)
    {
      switch (_bindingName)
      {
        case "name":
          _value = string.Empty;
          return true;
        case "selected":
          _value = "false";
          return true;
        case "assigned":
          _value = "false";
          return true;
        default:
          return false;
      }
    }
  }
}
