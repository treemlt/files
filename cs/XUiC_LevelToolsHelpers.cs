// Decompiled with JetBrains decompiler
// Type: XUiC_LevelToolsHelpers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

#nullable disable
public static class XUiC_LevelToolsHelpers
{
  [PublicizedFrom(EAccessModifier.Private)]
  public static readonly string[] goNamesUnpaintable = new string[5]
  {
    "_BlockEntities",
    "models",
    "modelsCollider",
    "cutout",
    "cutoutCollider"
  };
  [PublicizedFrom(EAccessModifier.Private)]
  public static readonly string[] goNamesPaintable = new string[2]
  {
    "opaque",
    "opaqueCollider"
  };
  [PublicizedFrom(EAccessModifier.Private)]
  public static readonly string[] goNamesTerrain = new string[2]
  {
    "terrain",
    "terrainCollider"
  };
  [PublicizedFrom(EAccessModifier.Private)]
  public static bool wasShowingImposterBeforeUpdate;
  [PublicizedFrom(EAccessModifier.Private)]
  public const float screenshotBorderPercentage = 0.15f;
  [PublicizedFrom(EAccessModifier.Private)]
  public const bool screenshot4To3 = true;
  [PublicizedFrom(EAccessModifier.Private)]
  public static bool drawingScreenshotGuide;

  public static NGuiAction BuildAction(
    string _functionName,
    string _captionOverride,
    bool _forToggle)
  {
    if (_functionName.IndexOf(':') < 0)
      return (NGuiAction) null;
    NGuiAction nguiAction = (NGuiAction) null;
    if (_functionName.StartsWith("SBM:"))
      nguiAction = XUiC_LevelToolsHelpers.createSelectionBoxAction(_functionName.Substring("SBM:".Length));
    else if (_functionName.StartsWith("BTS:"))
      nguiAction = XUiC_LevelToolsHelpers.createBlockToolSelectionAction(_functionName.Substring("BTS:".Length));
    else if (_functionName.StartsWith("Special:"))
      nguiAction = XUiC_LevelToolsHelpers.createSpecialAction(_functionName.Substring("Special:".Length));
    if (nguiAction == null)
    {
      Log.Error($"Function {_functionName} for LevelTools UI not found");
      return (NGuiAction) null;
    }
    if (!string.IsNullOrEmpty(_captionOverride))
      nguiAction.SetText(_captionOverride);
    if (_forToggle == nguiAction.IsToggle())
      return nguiAction;
    Log.Error(_forToggle ? $"Function {_functionName} for LevelTools UI is not a toggle action, but bound to a toggle button" : $"Function {_functionName} for LevelTools UI is a toggle action, but bound to a regular button");
    return (NGuiAction) null;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static NGuiAction createSelectionBoxAction(string _categoryName)
  {
    SelectionCategory selectionCategory = SelectionBoxManager.Instance.GetCategory(_categoryName);
    if (selectionCategory == null)
      return (NGuiAction) null;
    NGuiAction selectionBoxAction = new NGuiAction(Localization.Get("selectionCategory" + _categoryName), (string) null, true);
    selectionBoxAction.SetDescription(_categoryName);
    selectionBoxAction.SetClickActionDelegate((NGuiAction.OnClickActionDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () =>
    {
      SelectionCategory selectionCategory1 = selectionCategory;
      selectionCategory1.SetVisible(!selectionCategory1.IsVisible());
    }));
    selectionBoxAction.SetIsCheckedDelegate((NGuiAction.IsCheckedDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => selectionCategory.IsVisible()));
    return selectionBoxAction;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static NGuiAction createBlockToolSelectionAction(string _actionName)
  {
    BlockToolSelection activeBlockTool = GameManager.Instance.GetActiveBlockTool() is BlockToolSelection ? (BlockToolSelection) GameManager.Instance.GetActiveBlockTool() : (BlockToolSelection) null;
    if (activeBlockTool == null)
      return (NGuiAction) null;
    NGuiAction toolSelectionAction;
    activeBlockTool.GetActions().TryGetValue(_actionName, out toolSelectionAction);
    return toolSelectionAction;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static NGuiAction createSpecialAction(string _substring)
  {
    switch (_substring)
    {
      case "BlockTriggers":
        NGuiAction specialAction1 = new NGuiAction(Localization.Get("leveltoolsShowBlockTriggers"), (string) null, true);
        specialAction1.SetClickActionDelegate((NGuiAction.OnClickActionDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => PrefabEditModeManager.Instance.HighlightBlockTriggers = !PrefabEditModeManager.Instance.HighlightBlockTriggers));
        specialAction1.SetIsCheckedDelegate((NGuiAction.IsCheckedDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => PrefabEditModeManager.Instance.HighlightBlockTriggers));
        return specialAction1;
      case "CapturePrefabStats":
        NGuiAction specialAction2 = new NGuiAction(Localization.Get("xuiCapturePrefabStats"), (string) null, false);
        specialAction2.SetClickActionDelegate((NGuiAction.OnClickActionDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () =>
        {
          if (PrefabEditModeManager.Instance.VoxelPrefab == null)
            GameManager.ShowTooltip(LocalPlayerUI.GetUIForPrimaryPlayer().entityPlayer, "[FF4444]" + Localization.Get("xuiPrefabStatsNoPrefabLoaded"));
          else
            XUiC_EditorStat.ManualStats = WorldStats.CaptureWorldStats();
        }));
        return specialAction2;
      case "CompositionGrid":
        NGuiAction specialAction3 = new NGuiAction(Localization.Get("leveltoolsShowCompositionGrid"), (string) null, true);
        specialAction3.SetClickActionDelegate((NGuiAction.OnClickActionDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => PrefabEditModeManager.Instance.ToggleCompositionGrid()));
        specialAction3.SetIsCheckedDelegate((NGuiAction.IsCheckedDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => PrefabEditModeManager.Instance.IsCompositionGrid()));
        return specialAction3;
      case "Decor":
        NGuiAction specialAction4 = new NGuiAction(Localization.Get("leveltoolsShowDecor"), (string) null, true);
        specialAction4.SetClickActionDelegate((NGuiAction.OnClickActionDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () =>
        {
          GameManager.bShowDecorBlocks = !GameManager.bShowDecorBlocks;
          foreach (Chunk chunk in GameManager.Instance.World.ChunkCache.GetChunkArrayCopySync())
            chunk.NeedsRegeneration = true;
        }));
        specialAction4.SetIsCheckedDelegate((NGuiAction.IsCheckedDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => GameManager.bShowDecorBlocks));
        return specialAction4;
      case "DensitiesClean":
        NGuiAction specialAction5 = new NGuiAction(Localization.Get("xuiCleanDensity"), (string) null, false);
        specialAction5.SetClickActionDelegate(new NGuiAction.OnClickActionDelegate(XUiC_LevelToolsHelpers.DensitiesClean));
        specialAction5.SetIsEnabledDelegate((NGuiAction.IsEnabledDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && PrefabEditModeManager.Instance.VoxelPrefab != null));
        return specialAction5;
      case "DensitiesSmoothAir":
        NGuiAction specialAction6 = new NGuiAction(Localization.Get("xuiSmoothPrefabAir"), (string) null, false);
        specialAction6.SetClickActionDelegate((NGuiAction.OnClickActionDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => PrefabHelpers.SmoothPOI(1, false)));
        specialAction6.SetIsEnabledDelegate((NGuiAction.IsEnabledDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && PrefabEditModeManager.Instance.VoxelPrefab != null));
        return specialAction6;
      case "DensitiesSmoothLand":
        NGuiAction specialAction7 = new NGuiAction(Localization.Get("xuiSmoothPrefabLand"), (string) null, false);
        specialAction7.SetClickActionDelegate((NGuiAction.OnClickActionDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => PrefabHelpers.SmoothPOI(1, true)));
        specialAction7.SetIsEnabledDelegate((NGuiAction.IsEnabledDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && PrefabEditModeManager.Instance.VoxelPrefab != null));
        return specialAction7;
      case "GroundGridMoveDown":
        NGuiAction specialAction8 = new NGuiAction(Localization.Get("xuiShowMoveGroundGridDown"), (string) null, false);
        specialAction8.SetClickActionDelegate((NGuiAction.OnClickActionDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => PrefabEditModeManager.Instance.MoveGroundGridUpOrDown(-1)));
        specialAction8.SetIsEnabledDelegate((NGuiAction.IsEnabledDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => PrefabEditModeManager.Instance.IsGroundGrid() && SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer));
        return specialAction8;
      case "GroundGridMoveUp":
        NGuiAction specialAction9 = new NGuiAction(Localization.Get("xuiShowMoveGroundGridUp"), (string) null, false);
        specialAction9.SetClickActionDelegate((NGuiAction.OnClickActionDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => PrefabEditModeManager.Instance.MoveGroundGridUpOrDown(1)));
        specialAction9.SetIsEnabledDelegate((NGuiAction.IsEnabledDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => PrefabEditModeManager.Instance.IsGroundGrid() && SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer));
        return specialAction9;
      case "GroundGridToggle":
        NGuiAction specialAction10 = new NGuiAction(Localization.Get("xuiShowGroundGrid"), (string) null, true);
        specialAction10.SetClickActionDelegate((NGuiAction.OnClickActionDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => PrefabEditModeManager.Instance.ToggleGroundGrid()));
        specialAction10.SetIsCheckedDelegate((NGuiAction.IsCheckedDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => PrefabEditModeManager.Instance.IsGroundGrid()));
        return specialAction10;
      case "HighlightBlocksToggle":
        NGuiAction specialAction11 = new NGuiAction(Localization.Get("xuiHighlightBlocks"), (string) null, true);
        specialAction11.SetClickActionDelegate((NGuiAction.OnClickActionDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => PrefabEditModeManager.Instance.ToggleHighlightBlocks()));
        specialAction11.SetIsCheckedDelegate((NGuiAction.IsCheckedDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => PrefabEditModeManager.Instance.HighlightingBlocks));
        return specialAction11;
      case "ImposterToggle":
        NGuiAction specialAction12 = new NGuiAction(Localization.Get("xuiShowImposter"), (string) null, true);
        specialAction12.SetClickActionDelegate(new NGuiAction.OnClickActionDelegate(XUiC_LevelToolsHelpers.imposterToggleShow));
        specialAction12.SetIsCheckedDelegate((NGuiAction.IsCheckedDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && PrefabEditModeManager.Instance.IsShowingImposterPrefab()));
        specialAction12.SetIsEnabledDelegate((NGuiAction.IsEnabledDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer));
        return specialAction12;
      case "ImposterUpdate":
        NGuiAction specialAction13 = new NGuiAction(Localization.Get("xuiUpdateImposter"), (string) null, false);
        specialAction13.SetClickActionDelegate((NGuiAction.OnClickActionDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => XUiC_SaveDirtyPrefab.Show(LocalPlayerUI.GetUIForPrimaryPlayer().xui, new Action<XUiC_SaveDirtyPrefab.ESelectedAction>(XUiC_LevelToolsHelpers.updateImposter))));
        specialAction13.SetIsEnabledDelegate((NGuiAction.IsEnabledDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && PrefabEditModeManager.Instance.LoadedPrefab.Type != PathAbstractions.EAbstractedLocationType.None));
        return specialAction13;
      case "LightPerformance":
        NGuiAction specialAction14 = new NGuiAction(Localization.Get("xuiDebugMenuShowLightPerf"), (string) null, true);
        specialAction14.SetClickActionDelegate((NGuiAction.OnClickActionDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => LightViewer.SetEnabled(!LightViewer.IsEnabled)));
        specialAction14.SetIsCheckedDelegate((NGuiAction.IsCheckedDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => LightViewer.IsEnabled));
        return specialAction14;
      case "Loot":
        NGuiAction specialAction15 = new NGuiAction(Localization.Get("leveltoolsShowLoot"), (string) null, true);
        specialAction15.SetClickActionDelegate((NGuiAction.OnClickActionDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () =>
        {
          GameManager.bShowLootBlocks = !GameManager.bShowLootBlocks;
          foreach (Chunk chunk in GameManager.Instance.World.ChunkCache.GetChunkArrayCopySync())
            chunk.NeedsRegeneration = true;
        }));
        specialAction15.SetIsCheckedDelegate((NGuiAction.IsCheckedDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => GameManager.bShowLootBlocks));
        return specialAction15;
      case "PaintTexturesToggle":
        NGuiAction specialAction16 = new NGuiAction(Localization.Get("xuiShowPaintTextures"), (string) null, true);
        specialAction16.SetClickActionDelegate((NGuiAction.OnClickActionDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () =>
        {
          Chunk.IgnorePaintTextures = !Chunk.IgnorePaintTextures;
          foreach (Chunk chunk in GameManager.Instance.World.ChunkCache.GetChunkArrayCopySync())
            chunk.NeedsRegeneration = true;
        }));
        specialAction16.SetIsCheckedDelegate((NGuiAction.IsCheckedDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => !Chunk.IgnorePaintTextures));
        return specialAction16;
      case "Paintable":
        NGuiAction specialAction17 = new NGuiAction(Localization.Get("leveltoolsShowPaintable"), (string) null, true);
        specialAction17.SetClickActionDelegate((NGuiAction.OnClickActionDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () =>
        {
          GameManager.bShowPaintables = !GameManager.bShowPaintables;
          XUiC_LevelToolsHelpers.setChunkPartVisible(XUiC_LevelToolsHelpers.goNamesPaintable, GameManager.bShowPaintables);
        }));
        specialAction17.SetIsCheckedDelegate((NGuiAction.IsCheckedDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => GameManager.bShowPaintables));
        return specialAction17;
      case "PrefabFacingToggle":
        NGuiAction specialAction18 = new NGuiAction(Localization.Get("xuiShowFacing"), (string) null, true);
        specialAction18.SetClickActionDelegate((NGuiAction.OnClickActionDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => PrefabEditModeManager.Instance.TogglePrefabFacing(!PrefabEditModeManager.Instance.IsPrefabFacing())));
        specialAction18.SetIsCheckedDelegate((NGuiAction.IsCheckedDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => PrefabEditModeManager.Instance.IsPrefabFacing()));
        return specialAction18;
      case "PrefabFacingUpdate":
        NGuiAction specialAction19 = new NGuiAction(Localization.Get("xuiUpdateFacing"), (string) null, false);
        specialAction19.SetClickActionDelegate((NGuiAction.OnClickActionDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => PrefabEditModeManager.Instance.RotatePrefabFacing()));
        specialAction19.SetIsEnabledDelegate((NGuiAction.IsEnabledDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => PrefabEditModeManager.Instance.IsPrefabFacing()));
        return specialAction19;
      case "PrefabMoveDown":
        NGuiAction specialAction20 = new NGuiAction(Localization.Get("xuiMovePrefabDown"), (string) null, false);
        specialAction20.SetClickActionDelegate((NGuiAction.OnClickActionDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => PrefabEditModeManager.Instance.MovePrefabUpOrDown(-1)));
        specialAction20.SetIsEnabledDelegate((NGuiAction.IsEnabledDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && PrefabEditModeManager.Instance.VoxelPrefab != null));
        return specialAction20;
      case "PrefabMoveUp":
        NGuiAction specialAction21 = new NGuiAction(Localization.Get("xuiMovePrefabUp"), (string) null, false);
        specialAction21.SetClickActionDelegate((NGuiAction.OnClickActionDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => PrefabEditModeManager.Instance.MovePrefabUpOrDown(1)));
        specialAction21.SetIsEnabledDelegate((NGuiAction.IsEnabledDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && PrefabEditModeManager.Instance.VoxelPrefab != null));
        return specialAction21;
      case "PrefabProperties":
        NGuiAction specialAction22 = new NGuiAction(Localization.Get("xuiPrefabProperties"), (string) null, false);
        specialAction22.SetClickActionDelegate((NGuiAction.OnClickActionDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => XUiC_PrefabPropertiesEditor.Show(LocalPlayerUI.GetUIForPrimaryPlayer().xui, XUiC_PrefabPropertiesEditor.EPropertiesFrom.LoadedPrefab, PathAbstractions.AbstractedLocation.None)));
        specialAction22.SetIsEnabledDelegate((NGuiAction.IsEnabledDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && PrefabEditModeManager.Instance.VoxelPrefab != null));
        return specialAction22;
      case "PrefabScreenshotTake":
        NGuiAction specialAction23 = new NGuiAction(Localization.Get("xuiTakeScreenshot"), (string) null, false);
        specialAction23.SetClickActionDelegate((NGuiAction.OnClickActionDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () =>
        {
          if (PrefabEditModeManager.Instance.VoxelPrefab == null)
            GameManager.ShowTooltip(LocalPlayerUI.GetUIForPrimaryPlayer().entityPlayer, "[FF4444]" + Localization.Get("xuiScreenshotNoPrefabLoaded"));
          else
            ThreadManager.StartCoroutine(XUiC_LevelToolsHelpers.screenshotCo(PrefabEditModeManager.Instance.LoadedPrefab.FullPathNoExtension));
        }));
        specialAction23.SetIsEnabledDelegate((NGuiAction.IsEnabledDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && PrefabEditModeManager.Instance.VoxelPrefab != null));
        return specialAction23;
      case "PrefabScreenshotToggleBounds":
        NGuiAction specialAction24 = new NGuiAction(Localization.Get("xuiShowScreenshotBounds"), (string) null, true);
        specialAction24.SetClickActionDelegate((NGuiAction.OnClickActionDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () =>
        {
          XUiC_LevelToolsHelpers.drawingScreenshotGuide = !XUiC_LevelToolsHelpers.drawingScreenshotGuide;
          if (!XUiC_LevelToolsHelpers.drawingScreenshotGuide)
            return;
          ThreadManager.StartCoroutine(XUiC_LevelToolsHelpers.drawScreenshotGuide());
        }));
        specialAction24.SetIsCheckedDelegate((NGuiAction.IsCheckedDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => XUiC_LevelToolsHelpers.drawingScreenshotGuide));
        return specialAction24;
      case "PrefabUpdateBounds":
        NGuiAction specialAction25 = new NGuiAction(Localization.Get("xuiUpdateBounds"), (string) null, false);
        specialAction25.SetClickActionDelegate((NGuiAction.OnClickActionDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => PrefabEditModeManager.Instance.UpdatePrefabBounds()));
        specialAction25.SetIsEnabledDelegate((NGuiAction.IsEnabledDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && PrefabEditModeManager.Instance.VoxelPrefab != null));
        return specialAction25;
      case "QuestLoot":
        NGuiAction specialAction26 = new NGuiAction(Localization.Get("leveltoolsShowQuestLoot"), (string) null, true);
        specialAction26.SetClickActionDelegate((NGuiAction.OnClickActionDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => PrefabEditModeManager.Instance.HighlightQuestLoot = !PrefabEditModeManager.Instance.HighlightQuestLoot));
        specialAction26.SetIsCheckedDelegate((NGuiAction.IsCheckedDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => PrefabEditModeManager.Instance.HighlightQuestLoot));
        return specialAction26;
      case "ShowChunkBorders":
        NGuiAction specialAction27 = new NGuiAction(Localization.Get("leveltoolsShowChunkBorders"), (string) null, true);
        specialAction27.SetClickActionDelegate((NGuiAction.OnClickActionDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () =>
        {
          PlayerMoveController moveController = LocalPlayerUI.GetUIForPrimaryPlayer().entityPlayer.MoveController;
          moveController.drawChunkMode = (moveController.drawChunkMode + 1) % 2;
        }));
        specialAction27.SetIsCheckedDelegate((NGuiAction.IsCheckedDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => LocalPlayerUI.GetUIForPrimaryPlayer().entityPlayer.MoveController.drawChunkMode > 0));
        return specialAction27;
      case "SleeperXRay":
        NGuiAction specialAction28 = new NGuiAction(Localization.Get("leveltoolsSleeperXRay"), (string) null, true);
        specialAction28.SetClickActionDelegate((NGuiAction.OnClickActionDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => SleeperVolumeToolManager.SetXRay(!SleeperVolumeToolManager.GetXRay())));
        specialAction28.SetIsCheckedDelegate(new NGuiAction.IsCheckedDelegate(SleeperVolumeToolManager.GetXRay));
        return specialAction28;
      case "Terrain":
        NGuiAction specialAction29 = new NGuiAction(Localization.Get("leveltoolsShowTerrain"), (string) null, true);
        specialAction29.SetClickActionDelegate((NGuiAction.OnClickActionDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () =>
        {
          GameManager.bShowTerrain = !GameManager.bShowTerrain;
          XUiC_LevelToolsHelpers.setChunkPartVisible(XUiC_LevelToolsHelpers.goNamesTerrain, GameManager.bShowTerrain);
        }));
        specialAction29.SetIsCheckedDelegate((NGuiAction.IsCheckedDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => GameManager.bShowTerrain));
        return specialAction29;
      case "TexturesStrip":
        NGuiAction specialAction30 = new NGuiAction(Localization.Get("xuiStripTextures"), (string) null, false);
        specialAction30.SetClickActionDelegate((NGuiAction.OnClickActionDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => PrefabEditModeManager.Instance.StripTextures()));
        specialAction30.SetIsEnabledDelegate((NGuiAction.IsEnabledDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && PrefabEditModeManager.Instance.VoxelPrefab != null));
        return specialAction30;
      case "TexturesStripInternal":
        NGuiAction specialAction31 = new NGuiAction(Localization.Get("xuiStripInternalTextures"), (string) null, false);
        specialAction31.SetClickActionDelegate((NGuiAction.OnClickActionDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => PrefabEditModeManager.Instance.StripInternalTextures()));
        return specialAction31;
      case "Unpaintable":
        NGuiAction specialAction32 = new NGuiAction(Localization.Get("leveltoolsShowUnpaintable"), (string) null, true);
        specialAction32.SetClickActionDelegate((NGuiAction.OnClickActionDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () =>
        {
          GameManager.bShowUnpaintables = !GameManager.bShowUnpaintables;
          XUiC_LevelToolsHelpers.setChunkPartVisible(XUiC_LevelToolsHelpers.goNamesUnpaintable, GameManager.bShowUnpaintables);
        }));
        specialAction32.SetIsCheckedDelegate((NGuiAction.IsCheckedDelegate) ([PublicizedFrom(EAccessModifier.Internal)] () => GameManager.bShowUnpaintables));
        return specialAction32;
      default:
        return (NGuiAction) null;
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static void setChunkPartVisible(
    string[] _matchedNames,
    bool _visible,
    List<ChunkGameObject> _cgos = null)
  {
    if (_cgos == null)
      _cgos = GameManager.Instance.World.m_ChunkManager.GetUsedChunkGameObjects();
    foreach (Component cgo in _cgos)
      XUiC_LevelToolsHelpers.setChunkPartVisible(cgo.transform, _matchedNames, _visible);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static void setChunkPartVisible(Transform _parent, string[] _matchedNames, bool _visible)
  {
    for (int index = 0; index < _parent.childCount; ++index)
    {
      Transform child = _parent.GetChild(index);
      string name = ((Object) child).name;
      if (((IList<string>) _matchedNames).ContainsCaseInsensitive(name))
        ((Component) child).gameObject.SetActive(_visible);
      else if (child.childCount > 0)
        XUiC_LevelToolsHelpers.setChunkPartVisible(child, _matchedNames, _visible);
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static void updateImposter(XUiC_SaveDirtyPrefab.ESelectedAction _action)
  {
    LocalPlayerUI.GetUIForPrimaryPlayer().windowManager.Open(XUiC_InGameMenuWindow.ID, true);
    if (_action == XUiC_SaveDirtyPrefab.ESelectedAction.Cancel)
      return;
    LocalPlayerUI.GetUIForPrimaryPlayer().windowManager.TempHUDDisable();
    XUiC_LevelToolsHelpers.wasShowingImposterBeforeUpdate = PrefabEditModeManager.Instance.IsShowingImposterPrefab();
    PrefabHelpers.convert(new Action(XUiC_LevelToolsHelpers.waitForUpdateImposter));
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static void waitForUpdateImposter()
  {
    PrefabHelpers.Cleanup();
    if (XUiC_LevelToolsHelpers.wasShowingImposterBeforeUpdate)
      PrefabEditModeManager.Instance.LoadImposterPrefab(PrefabEditModeManager.Instance.LoadedPrefab);
    else
      PrefabEditModeManager.Instance.LoadVoxelPrefab(PrefabEditModeManager.Instance.LoadedPrefab);
    LocalPlayerUI.GetUIForPrimaryPlayer().windowManager.ReEnableHUD();
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static void imposterToggleShow()
  {
    if (PrefabEditModeManager.Instance.IsShowingImposterPrefab())
      XUiC_LevelToolsHelpers.showPrefab();
    else
      XUiC_SaveDirtyPrefab.Show(LocalPlayerUI.GetUIForPrimaryPlayer().xui, new Action<XUiC_SaveDirtyPrefab.ESelectedAction>(XUiC_LevelToolsHelpers.showImposter));
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static void showImposter(XUiC_SaveDirtyPrefab.ESelectedAction _action)
  {
    LocalPlayerUI.GetUIForPrimaryPlayer().windowManager.Open(XUiC_InGameMenuWindow.ID, true);
    if (_action == XUiC_SaveDirtyPrefab.ESelectedAction.Cancel)
      return;
    PathAbstractions.AbstractedLocation loadedPrefab = PrefabEditModeManager.Instance.LoadedPrefab;
    PrefabEditModeManager.Instance.ClearImposterPrefab();
    if (PrefabEditModeManager.Instance.HasPrefabImposter(loadedPrefab))
      PrefabEditModeManager.Instance.LoadImposterPrefab(loadedPrefab);
    else
      GameManager.ShowTooltip(GameManager.Instance.World.GetLocalPlayers()[0], $"Prefab {loadedPrefab.Name} has no imposter yet");
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static void showPrefab()
  {
    if (PrefabEditModeManager.Instance.LoadedPrefab.Type == PathAbstractions.EAbstractedLocationType.None)
      return;
    PrefabEditModeManager.Instance.LoadVoxelPrefab(PrefabEditModeManager.Instance.LoadedPrefab);
  }

  public static bool IsShowImposter() => PrefabEditModeManager.Instance.IsShowingImposterPrefab();

  public static void SetShowImposter() => XUiC_LevelToolsHelpers.imposterToggleShow();

  [PublicizedFrom(EAccessModifier.Private)]
  public static IEnumerator drawScreenshotGuide()
  {
    while (XUiC_LevelToolsHelpers.drawingScreenshotGuide)
    {
      yield return (object) new WaitForEndOfFrame();
      Rect screenshotRect = GameUtils.GetScreenshotRect(0.15f, true);
      // ISSUE: explicit constructor call
      ((Rect) ref screenshotRect).\u002Ector(((Rect) ref screenshotRect).x - 2f, ((Rect) ref screenshotRect).y - 2f, ((Rect) ref screenshotRect).width + 4f, ((Rect) ref screenshotRect).height + 4f);
      GUIUtils.DrawRect(screenshotRect, Color.green);
      if (!GameManager.Instance.gameStateManager.IsGameStarted())
        XUiC_LevelToolsHelpers.drawingScreenshotGuide = false;
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static IEnumerator screenshotCo(string _filename)
  {
    LocalPlayerUI.GetUIForPrimaryPlayer().windowManager.TempHUDDisable();
    EntityPlayerLocal player = GameManager.Instance.World.GetPrimaryPlayer();
    bool isSpectator = player.IsSpectator;
    player.IsSpectator = true;
    SkyManager.SetSkyEnabled(false);
    yield return (object) null;
    try
    {
      GameUtils.TakeScreenShot(GameUtils.EScreenshotMode.File, _filename, 0.15f, true, 280, 210);
    }
    catch (Exception ex)
    {
      Log.Exception(ex);
    }
    yield return (object) null;
    player.IsSpectator = isSpectator;
    SkyManager.SetSkyEnabled(true);
    LocalPlayerUI.GetUIForPrimaryPlayer().windowManager.ReEnableHUD();
  }

  public static void ReplaceBlockId(Block _srcBlockClass, Block _dstBlockClass)
  {
    int sourceBlockId = _srcBlockClass.blockID;
    int targetBlockId = _dstBlockClass.blockID;
    HashSet<Chunk> changedChunks = new HashSet<Chunk>();
    bool bUseSelection = BlockToolSelection.Instance.SelectionActive;
    Vector3i selStart = BlockToolSelection.Instance.SelectionMin;
    Vector3i selEnd = selStart + BlockToolSelection.Instance.SelectionSize - Vector3i.one;
    List<Chunk> chunkArrayCopySync = GameManager.Instance.World.ChunkCache.GetChunkArrayCopySync();
    for (int index = 0; index < chunkArrayCopySync.Count; ++index)
    {
      Chunk curChunk = chunkArrayCopySync[index];
      curChunk.LoopOverAllBlocks((ChunkBlockLayer.LoopBlocksDelegate) ([PublicizedFrom(EAccessModifier.Internal)] (_x, _y, _z, _bv) =>
      {
        if (_bv.type != sourceBlockId)
          return;
        if (bUseSelection)
        {
          Vector3i worldPos = curChunk.ToWorldPos(new Vector3i(_x, _y, _z));
          if (worldPos.x < selStart.x || worldPos.x > selEnd.x || worldPos.y < selStart.y || worldPos.y > selEnd.y || worldPos.z < selStart.z || worldPos.z > selEnd.z)
            return;
        }
        if (_srcBlockClass.shape.IsTerrain() != _dstBlockClass.shape.IsTerrain())
        {
          sbyte _density = curChunk.GetDensity(_x, _y, _z);
          if (_dstBlockClass.shape.IsTerrain())
            _density = MarchingCubes.DensityTerrain;
          else if (_density != (sbyte) 0)
            _density = MarchingCubes.DensityAir;
          curChunk.SetDensity(_x, _y, _z, _density);
        }
        BlockValue _blockValue = new BlockValue((uint) targetBlockId)
        {
          rotation = _bv.rotation,
          meta = _bv.meta
        };
        curChunk.SetBlockRaw(_x, _y, _z, _blockValue);
        changedChunks.Add(curChunk);
      }), _bIncludeAirBlocks: true);
    }
    foreach (Chunk chunk in changedChunks)
      chunk.NeedsRegeneration = true;
    if (changedChunks.Count <= 0)
      return;
    PrefabEditModeManager.Instance.NeedsSaving = true;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static void ReplacePaint(int _sourcePaintId, int _targetPaintId)
  {
    HashSet<Chunk> changedChunks = new HashSet<Chunk>();
    bool bUseSelection = BlockToolSelection.Instance.SelectionActive;
    Vector3i selStart = BlockToolSelection.Instance.SelectionMin;
    Vector3i selEnd = selStart + BlockToolSelection.Instance.SelectionSize - Vector3i.one;
    List<Chunk> chunkArrayCopySync = GameManager.Instance.World.ChunkCache.GetChunkArrayCopySync();
    for (int index1 = 0; index1 < chunkArrayCopySync.Count; ++index1)
    {
      Chunk curChunk = chunkArrayCopySync[index1];
      curChunk.LoopOverAllBlocks((ChunkBlockLayer.LoopBlocksDelegate) ([PublicizedFrom(EAccessModifier.Internal)] (_x, _y, _z, _bv) =>
      {
        if (bUseSelection)
        {
          Vector3i worldPos = curChunk.ToWorldPos(new Vector3i(_x, _y, _z));
          if (worldPos.x < selStart.x || worldPos.x > selEnd.x || worldPos.y < selStart.y || worldPos.y > selEnd.y || worldPos.z < selStart.z || worldPos.z > selEnd.z)
            return;
        }
        bool flag = false;
        long _texturefull = curChunk.GetTextureFull(_x, _y, _z, 0);
        for (int index2 = 0; index2 < 6; ++index2)
        {
          if ((_texturefull >> index2 * 8 & (long) byte.MaxValue) == (long) _sourcePaintId)
          {
            _texturefull = _texturefull & ~((long) byte.MaxValue << index2 * 8) | (long) _targetPaintId << index2 * 8;
            flag = true;
          }
        }
        if (!flag)
          return;
        curChunk.SetTextureFull(_x, _y, _z, _texturefull);
        changedChunks.Add(curChunk);
      }));
    }
    foreach (Chunk chunk in changedChunks)
      chunk.NeedsRegeneration = true;
    if (changedChunks.Count <= 0)
      return;
    PrefabEditModeManager.Instance.NeedsSaving = true;
  }

  public static void ReplaceBlockShapeMaterials(string _oldMaterial, string _newMaterial)
  {
    HashSet<Chunk> changedChunks = new HashSet<Chunk>();
    Dictionary<int, int> blockReplaceCache = new Dictionary<int, int>();
    MicroStopwatch microStopwatch = new MicroStopwatch(true);
    int hits = 0;
    int misses = 0;
    int replaced = 0;
    bool bUseSelection = BlockToolSelection.Instance.SelectionActive;
    Vector3i selStart = BlockToolSelection.Instance.SelectionMin;
    Vector3i selEnd = selStart + BlockToolSelection.Instance.SelectionSize - Vector3i.one;
    List<Chunk> chunkArrayCopySync = GameManager.Instance.World.ChunkCache.GetChunkArrayCopySync();
    for (int index = 0; index < chunkArrayCopySync.Count; ++index)
    {
      Chunk curChunk = chunkArrayCopySync[index];
      curChunk.LoopOverAllBlocks((ChunkBlockLayer.LoopBlocksDelegate) ([PublicizedFrom(EAccessModifier.Internal)] (_x, _y, _z, _bv) =>
      {
        if (bUseSelection)
        {
          Vector3i worldPos = curChunk.ToWorldPos(new Vector3i(_x, _y, _z));
          if (worldPos.x < selStart.x || worldPos.x > selEnd.x || worldPos.y < selStart.y || worldPos.y > selEnd.y || worldPos.z < selStart.z || worldPos.z > selEnd.z)
            return;
        }
        int type = _bv.type;
        int blockId;
        if (!blockReplaceCache.TryGetValue(type, out blockId))
        {
          ++misses;
          Block block = _bv.Block;
          if (block.GetAutoShapeType() != EAutoShapeType.Shape)
          {
            blockReplaceCache[type] = -1;
            return;
          }
          if (!block.GetAutoShapeBlockName().Equals(_oldMaterial))
          {
            blockReplaceCache[type] = -1;
            return;
          }
          Block blockByName = Block.GetBlockByName($"{_newMaterial}:{block.GetAutoShapeShapeName()}", true);
          if (blockByName == null)
          {
            blockReplaceCache[type] = -1;
            return;
          }
          blockId = blockByName.blockID;
          blockReplaceCache[type] = blockId;
        }
        else
          ++hits;
        if (blockId < 0)
          return;
        ++replaced;
        BlockValue _blockValue = new BlockValue((uint) blockId)
        {
          rotation = _bv.rotation,
          meta = _bv.meta
        };
        curChunk.SetBlockRaw(_x, _y, _z, _blockValue);
        changedChunks.Add(curChunk);
      }), _bIncludeAirBlocks: true);
    }
    foreach (Chunk chunk in changedChunks)
      chunk.NeedsRegeneration = true;
    if (changedChunks.Count > 0)
      PrefabEditModeManager.Instance.NeedsSaving = true;
    Log.Out($"Replace material done in {((Stopwatch) microStopwatch).ElapsedMilliseconds} ms. Total checked blocks: {hits + misses}, replaced: {replaced}, cache hits: {hits}, misses: {misses}");
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static void DensitiesClean()
  {
    HashSet<Chunk> changedChunks = new HashSet<Chunk>();
    bool bUseSelection = BlockToolSelection.Instance.SelectionActive;
    Vector3i selStart = BlockToolSelection.Instance.SelectionMin;
    Vector3i selEnd = selStart + BlockToolSelection.Instance.SelectionSize - Vector3i.one;
    List<Chunk> chunkArrayCopySync = GameManager.Instance.World.ChunkCache.GetChunkArrayCopySync();
    for (int index = 0; index < chunkArrayCopySync.Count; ++index)
    {
      Chunk curChunk = chunkArrayCopySync[index];
      curChunk.LoopOverAllBlocks((ChunkBlockLayer.LoopBlocksDelegate) ([PublicizedFrom(EAccessModifier.Internal)] (_x, _y, _z, _bv) =>
      {
        if (bUseSelection)
        {
          Vector3i worldPos = curChunk.ToWorldPos(new Vector3i(_x, _y, _z));
          if (worldPos.x < selStart.x || worldPos.x > selEnd.x || worldPos.y < selStart.y || worldPos.y > selEnd.y || worldPos.z < selStart.z || worldPos.z > selEnd.z)
            return;
        }
        Block block = _bv.Block;
        sbyte density = curChunk.GetDensity(_x, _y, _z);
        sbyte _density = block.shape.IsTerrain() ? MarchingCubes.DensityTerrain : MarchingCubes.DensityAir;
        if ((int) _density == (int) density)
          return;
        curChunk.SetDensity(_x, _y, _z, _density);
        changedChunks.Add(curChunk);
      }), _bIncludeAirBlocks: true);
    }
    foreach (Chunk chunk in changedChunks)
      chunk.NeedsRegeneration = true;
    if (changedChunks.Count <= 0)
      return;
    PrefabEditModeManager.Instance.NeedsSaving = true;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  static XUiC_LevelToolsHelpers()
  {
  }
}
