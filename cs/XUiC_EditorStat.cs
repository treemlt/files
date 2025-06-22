// Decompiled with JetBrains decompiler
// Type: XUiC_EditorStat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
[Preserve]
public class XUiC_EditorStat : XUiController
{
  [PublicizedFrom(EAccessModifier.Private)]
  public float lastDirtyTime;
  [PublicizedFrom(EAccessModifier.Private)]
  public bool hasLootStat;
  [PublicizedFrom(EAccessModifier.Private)]
  public int lootContainers;
  [PublicizedFrom(EAccessModifier.Private)]
  public int fetchLootContainers;
  [PublicizedFrom(EAccessModifier.Private)]
  public int restorePowerNodes;
  [PublicizedFrom(EAccessModifier.Private)]
  public bool hasBlockEntitiesStat;
  [PublicizedFrom(EAccessModifier.Private)]
  public int totalBlockEntities;
  [PublicizedFrom(EAccessModifier.Private)]
  public bool hasSelection;
  [PublicizedFrom(EAccessModifier.Private)]
  public Vector3i selectionSize;
  [PublicizedFrom(EAccessModifier.Private)]
  public const int DC_AVERAGE_FRAMES = 20;
  [PublicizedFrom(EAccessModifier.Private)]
  public readonly int[] drawcallsBuf = new int[20];
  [PublicizedFrom(EAccessModifier.Private)]
  public int drawcallsBufIndex;
  [PublicizedFrom(EAccessModifier.Private)]
  public int drawcallsSum;
  [PublicizedFrom(EAccessModifier.Private)]
  public static WorldStats manualStats;
  [PublicizedFrom(EAccessModifier.Private)]
  public static DateTime ManualStatsUpdateTime;
  [PublicizedFrom(EAccessModifier.Private)]
  public readonly CachedStringFormatter<Vector3i> prefabSizeFormatter = new CachedStringFormatter<Vector3i>((Func<Vector3i, string>) ([PublicizedFrom(EAccessModifier.Internal)] (_i) => _i.ToString()));
  [PublicizedFrom(EAccessModifier.Private)]
  public readonly CachedStringFormatter<Vector3i> selectionSizeFormatter = new CachedStringFormatter<Vector3i>((Func<Vector3i, string>) ([PublicizedFrom(EAccessModifier.Internal)] (_i) => _i.ToString()));
  [PublicizedFrom(EAccessModifier.Private)]
  public readonly CachedStringFormatterInt lootFormatter = new CachedStringFormatterInt();
  [PublicizedFrom(EAccessModifier.Private)]
  public readonly CachedStringFormatterInt fetchlootFormatter = new CachedStringFormatterInt();
  [PublicizedFrom(EAccessModifier.Private)]
  public readonly CachedStringFormatterInt restorepowerFormatter = new CachedStringFormatterInt();
  [PublicizedFrom(EAccessModifier.Private)]
  public readonly CachedStringFormatterInt blockentitiesFormatter = new CachedStringFormatterInt();
  [PublicizedFrom(EAccessModifier.Private)]
  public readonly CachedStringFormatter<int> vertsFormatter = new CachedStringFormatter<int>((Func<int, string>) ([PublicizedFrom(EAccessModifier.Internal)] (_i) => ((double) _i).FormatNumberWithMetricPrefix()));
  [PublicizedFrom(EAccessModifier.Private)]
  public readonly CachedStringFormatter<int> trisFormatter = new CachedStringFormatter<int>((Func<int, string>) ([PublicizedFrom(EAccessModifier.Internal)] (_i) => ((double) _i).FormatNumberWithMetricPrefix()));
  [PublicizedFrom(EAccessModifier.Private)]
  public readonly CachedStringFormatterInt batchesFormatter = new CachedStringFormatterInt();
  [PublicizedFrom(EAccessModifier.Private)]
  public readonly CachedStringFormatterInt statsVertsFormatter = new CachedStringFormatterInt();
  [PublicizedFrom(EAccessModifier.Private)]
  public readonly CachedStringFormatterInt statsTrisFormatter = new CachedStringFormatterInt();
  [PublicizedFrom(EAccessModifier.Private)]
  public readonly CachedStringFormatter<DateTime> statsManualUpdateTimeFormatter = new CachedStringFormatter<DateTime>((Func<DateTime, string>) ([PublicizedFrom(EAccessModifier.Internal)] (_dt) => _dt.ToString((IFormatProvider) Utils.StandardCulture)));
  [PublicizedFrom(EAccessModifier.Private)]
  public readonly CachedStringFormatterInt statsManualVertsFormatter = new CachedStringFormatterInt();
  [PublicizedFrom(EAccessModifier.Private)]
  public readonly CachedStringFormatterInt statsManualTrisFormatter = new CachedStringFormatterInt();

  public bool hasPrefabLoaded
  {
    [PublicizedFrom(EAccessModifier.Private)] get
    {
      return PrefabEditModeManager.Instance.IsActive() && PrefabEditModeManager.Instance.VoxelPrefab != null && PrefabEditModeManager.Instance.VoxelPrefab.location.Type != PathAbstractions.EAbstractedLocationType.None;
    }
  }

  public Prefab selectedPrefab
  {
    [PublicizedFrom(EAccessModifier.Private)] get
    {
      return GameManager.Instance.GetDynamicPrefabDecorator()?.ActivePrefab?.prefab;
    }
  }

  public bool hasPrefabSelected
  {
    [PublicizedFrom(EAccessModifier.Private)] get => this.selectedPrefab != null;
  }

  public static WorldStats ManualStats
  {
    get => XUiC_EditorStat.manualStats;
    set
    {
      XUiC_EditorStat.ManualStatsUpdateTime = DateTime.Now;
      XUiC_EditorStat.manualStats = value;
    }
  }

  public override void OnOpen()
  {
    base.OnOpen();
    this.IsDirty = true;
  }

  public override void Update(float _dt)
  {
    base.Update(_dt);
    if (!this.IsDirty && (double) Time.time - (double) this.lastDirtyTime < 1.0)
      return;
    this.lootContainers = 0;
    this.fetchLootContainers = 0;
    this.restorePowerNodes = 0;
    this.totalBlockEntities = 0;
    this.hasSelection = false;
    this.selectionSize = new Vector3i();
    if (this.hasPrefabLoaded)
    {
      (SelectionCategory, SelectionBox)? selection = SelectionBoxManager.Instance.Selection;
      ref (SelectionCategory, SelectionBox)? local = ref selection;
      SelectionBox selectionBox = local.HasValue ? local.GetValueOrDefault().Item2 : (SelectionBox) null;
      if (Object.op_Inequality((Object) selectionBox, (Object) null))
      {
        this.selectionSize = selectionBox.GetScale();
        this.hasSelection = true;
      }
      if (this.hasLootStat)
        PrefabEditModeManager.Instance.GetLootAndFetchLootContainerCount(out this.lootContainers, out this.fetchLootContainers, out this.restorePowerNodes);
      if (this.hasBlockEntitiesStat)
      {
        foreach (Component usedChunkGameObject in GameManager.Instance.World.m_ChunkManager.GetUsedChunkGameObjects())
          this.totalBlockEntities += this.countBlockEntities(usedChunkGameObject.transform);
      }
    }
    this.RefreshBindings();
    this.IsDirty = false;
    this.lastDirtyTime = Time.time;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public int countBlockEntities(Transform _t)
  {
    int num = 0;
    for (int index = 0; index < _t.childCount; ++index)
    {
      Transform child = _t.GetChild(index);
      if (((Object) child).name == "_BlockEntities")
        num += child.childCount;
      else
        num += this.countBlockEntities(child);
    }
    return num;
  }

  public override bool GetBindingValue(ref string _value, string _bindingName)
  {
    Prefab prefab = this.hasPrefabLoaded ? PrefabEditModeManager.Instance.VoxelPrefab : this.selectedPrefab;
    bool flag1 = prefab != null;
    bool flag2 = prefab?.RenderingCostStats != null;
    bool flag3 = XUiC_EditorStat.ManualStats != null;
    switch (_bindingName)
    {
      case "block_entities":
        this.hasBlockEntitiesStat = true;
        _value = this.blockentitiesFormatter.Format(this.totalBlockEntities);
        return true;
      case "difficulty_tier":
        _value = prefab?.DifficultyTier.ToString() ?? "";
        return true;
      case "drawcalls":
        _value = this.batchesFormatter.Format(this.drawcallsSum / 20);
        return true;
      case "fetchloot_containers":
        this.hasLootStat = true;
        _value = this.fetchlootFormatter.Format(this.fetchLootContainers);
        return true;
      case "has_loaded_prefab":
        _value = this.hasPrefabLoaded.ToString();
        return true;
      case "has_selected_prefab":
        _value = this.hasPrefabSelected.ToString();
        return true;
      case "has_selection":
        _value = this.hasSelection.ToString();
        return true;
      case "loaded_prefab_changed":
        _value = !this.hasPrefabLoaded || !PrefabEditModeManager.Instance.NeedsSaving ? "" : "*";
        return true;
      case "loaded_prefab_name":
        _value = prefab?.PrefabName ?? "";
        return true;
      case "loot_containers":
        this.hasLootStat = true;
        _value = this.lootFormatter.Format(this.lootContainers);
        return true;
      case "prefab_size":
        _value = flag1 ? this.prefabSizeFormatter.Format(prefab.size) : "";
        return true;
      case "prefab_volume":
        _value = flag1 ? prefab.size.Volume().ToString() : "";
        return true;
      case "restorepower_nodes":
        this.hasLootStat = true;
        _value = this.restorepowerFormatter.Format(this.restorePowerNodes);
        return true;
      case "selection_size":
        _value = this.hasSelection ? this.selectionSizeFormatter.Format(this.selectionSize) : "-";
        return true;
      case "show_quest_clear_count":
        _value = prefab?.ShowQuestClearCount.ToString() ?? "";
        return true;
      case "sleeper_info":
        _value = prefab?.CalcSleeperInfo() ?? "";
        return true;
      case "statsLightsVolume":
        if (!flag2)
        {
          _value = "-";
          return true;
        }
        float lightsVolume1 = prefab.RenderingCostStats.LightsVolume;
        int num1 = prefab.size.Volume();
        _value = $"{lightsVolume1:F0} ({(ValueType) (float) ((double) lightsVolume1 / (double) num1):P1})";
        return true;
      case "statsManualLightsVolume":
        if (!flag3)
        {
          _value = "-";
          return true;
        }
        float lightsVolume2 = XUiC_EditorStat.ManualStats.LightsVolume;
        int num2 = prefab != null ? prefab.size.Volume() : 0;
        _value = $"{lightsVolume2:F0} ({(ValueType) (float) ((double) lightsVolume2 / (double) num2):P1})";
        return true;
      case "statsManualTriangles":
        _value = flag3 ? this.statsManualTrisFormatter.Format(XUiC_EditorStat.ManualStats.TotalTriangles) : "-";
        return true;
      case "statsManualUpdateTime":
        _value = flag3 ? this.statsManualUpdateTimeFormatter.Format(XUiC_EditorStat.ManualStatsUpdateTime.ToLocalTime()) : "<not captured>";
        return true;
      case "statsManualVertices":
        _value = flag3 ? this.statsManualVertsFormatter.Format(XUiC_EditorStat.ManualStats.TotalVertices) : "-";
        return true;
      case "statsTriangles":
        _value = flag2 ? this.statsTrisFormatter.Format(prefab.RenderingCostStats.TotalTriangles) : "-";
        return true;
      case "statsVertices":
        _value = flag2 ? this.statsVertsFormatter.Format(prefab.RenderingCostStats.TotalVertices) : "-";
        return true;
      case "tris":
        _value = "";
        return true;
      case "verts":
        _value = "";
        return true;
      default:
        return base.GetBindingValue(ref _value, _bindingName);
    }
  }
}
