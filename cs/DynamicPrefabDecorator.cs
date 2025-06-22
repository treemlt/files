// Decompiled with JetBrains decompiler
// Type: DynamicPrefabDecorator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

#nullable disable
public class DynamicPrefabDecorator : IDynamicDecorator, ISelectionBoxCallback
{
  [PublicizedFrom(EAccessModifier.Private)]
  public const int cPrefabMaxRadius = 200;
  [PublicizedFrom(EAccessModifier.Private)]
  public Dictionary<string, Prefab> prefabCache = new Dictionary<string, Prefab>();
  [PublicizedFrom(EAccessModifier.Private)]
  public Dictionary<string, Prefab[]> prefabCacheRotations = new Dictionary<string, Prefab[]>();
  public List<PrefabInstance> allPrefabs = new List<PrefabInstance>();
  [PublicizedFrom(EAccessModifier.Private)]
  public List<PrefabInstance> poiPrefabs = new List<PrefabInstance>();
  [PublicizedFrom(EAccessModifier.Private)]
  public bool isSortNeeded = true;
  [PublicizedFrom(EAccessModifier.Private)]
  public List<PrefabInstance> allPrefabsSorted = new List<PrefabInstance>();
  [PublicizedFrom(EAccessModifier.Private)]
  public List<TraderArea> traderAreas = new List<TraderArea>();
  [PublicizedFrom(EAccessModifier.Private)]
  public int id;
  public PrefabInstance ActivePrefab;
  [PublicizedFrom(EAccessModifier.Private)]
  public Dictionary<string, bool> prefabMeshExisting = new Dictionary<string, bool>();
  [PublicizedFrom(EAccessModifier.Private)]
  public FastTags<TagGroup.Poi> streetTileTag = FastTags<TagGroup.Poi>.Parse("streettile");
  public int ProtectSizeXMax;
  [PublicizedFrom(EAccessModifier.Private)]
  public DynamicPrefabDecorator.TraderComparer traderComparer = new DynamicPrefabDecorator.TraderComparer();
  [PublicizedFrom(EAccessModifier.Private)]
  public List<PrefabInstance> decorateChunkPIs = new List<PrefabInstance>();
  public static int PrefabPreviewLimit;
  [PublicizedFrom(EAccessModifier.Private)]
  public readonly Vector3 boundsPad = new Vector3(1f / 1000f, 1f / 1000f, 1f / 1000f);
  [PublicizedFrom(EAccessModifier.Private)]
  public BlockValue blockValueTerrainFiller;
  [PublicizedFrom(EAccessModifier.Private)]
  public BlockValue blockValueTerrainFiller2;

  public event Action<PrefabInstance> OnPrefabLoaded;

  public event Action<PrefabInstance> OnPrefabChanged;

  public event Action<PrefabInstance> OnPrefabRemoved;

  public IEnumerator Load(string _path)
  {
    DynamicPrefabDecorator dynamicPrefabDecorator1 = this;
    if (SdFile.Exists(_path + "/prefabs.xml"))
    {
      MicroStopwatch msw = new MicroStopwatch(true);
      XmlFile xmlFile;
      try
      {
        dynamicPrefabDecorator1.id = 0;
        xmlFile = new XmlFile(_path, "prefabs");
      }
      catch (Exception ex)
      {
        Log.Error($"Loading prefabs xml file for level '{Path.GetFileName(_path)}': {ex.Message}");
        Log.Exception(ex);
        yield break;
      }
      int i = 0;
      int totalPrefabs = xmlFile.XmlDoc.Root.Elements((XName) "decoration").Count<XElement>();
      LocalPlayerUI ui = LocalPlayerUI.primaryUI;
      bool progressWindowOpen = Object.op_Implicit((Object) ui) && ui.windowManager.IsWindowOpen(XUiC_ProgressWindow.ID);
      foreach (XElement element in xmlFile.XmlDoc.Root.Elements((XName) "decoration"))
      {
        try
        {
          ++i;
          if (element.HasAttribute((XName) "name"))
          {
            string attribute = element.GetAttribute((XName) "name");
            Vector3i vector3i = Vector3i.Parse(element.GetAttribute((XName) "position"));
            bool _result;
            StringParsers.TryParseBool(element.GetAttribute((XName) "y_is_groundlevel"), out _result);
            byte _rotation = 0;
            if (element.HasAttribute((XName) "rotation"))
              _rotation = byte.Parse(element.GetAttribute((XName) "rotation"));
            Prefab prefabRotated = dynamicPrefabDecorator1.GetPrefabRotated(attribute, (int) _rotation);
            if (prefabRotated == null)
            {
              Log.Warning($"Could not load prefab '{attribute}'. Skipping it");
              continue;
            }
            if (_result)
              vector3i.y += prefabRotated.yOffset;
            if (prefabRotated.bTraderArea && SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
              dynamicPrefabDecorator1.AddTrader(new TraderArea(vector3i, prefabRotated.size, prefabRotated.TraderAreaProtect, prefabRotated.TeleportVolumes));
            DynamicPrefabDecorator dynamicPrefabDecorator2 = dynamicPrefabDecorator1;
            int id = dynamicPrefabDecorator1.id;
            int num = id + 1;
            dynamicPrefabDecorator2.id = num;
            PrefabInstance _pi = new PrefabInstance(id, prefabRotated.location, vector3i, _rotation, prefabRotated, 0);
            dynamicPrefabDecorator1.AddPrefab(_pi, _pi.prefab.HasQuestTag());
          }
        }
        catch (Exception ex)
        {
          Log.Error($"Loading prefabs xml file for level '{Path.GetFileName(_path)}': {ex.Message}");
          Log.Exception(ex);
        }
        if (((Stopwatch) msw).ElapsedMilliseconds > (long) Constants.cMaxLoadTimePerFrameMillis)
        {
          if (progressWindowOpen)
            XUiC_ProgressWindow.SetText(ui, string.Format(Localization.Get("uiLoadCreatingWorldPrefabs"), (object) Math.Min(100.0, 105.0 * (double) i / (double) totalPrefabs).ToString("0")));
          yield return (object) null;
          msw.ResetAndRestart();
        }
      }
      if (progressWindowOpen)
      {
        XUiC_ProgressWindow.SetText(ui, string.Format(Localization.Get("uiLoadCreatingWorldPrefabs"), (object) "100"));
        yield return (object) null;
      }
      dynamicPrefabDecorator1.SortPrefabs();
      XUiC_ProgressWindow.SetText(ui, Localization.Get("uiLoadCreatingWorld"));
      yield return (object) null;
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void SortPrefabs()
  {
    lock (this.allPrefabsSorted)
    {
      this.allPrefabsSorted.Clear();
      this.allPrefabsSorted.AddRange((IEnumerable<PrefabInstance>) this.allPrefabs);
      this.allPrefabsSorted.Sort((Comparison<PrefabInstance>) ([PublicizedFrom(EAccessModifier.Internal)] (a, b) => a.boundingBoxPosition.x.CompareTo(b.boundingBoxPosition.x)));
      this.isSortNeeded = false;
    }
  }

  public int GetNextId() => this.id++;

  public bool Save(string _path)
  {
    try
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.CreateXmlDeclaration();
      XmlElement _node = xmlDocument.AddXmlElement("prefabs");
      for (int index = 0; index < this.allPrefabs.Count; ++index)
      {
        PrefabInstance allPrefab = this.allPrefabs[index];
        if (allPrefab != null)
        {
          string str = "";
          Vector3i boundingBoxPosition = allPrefab.boundingBoxPosition;
          if (allPrefab.prefab != null && allPrefab.prefab.location.Type != PathAbstractions.EAbstractedLocationType.None)
          {
            str = allPrefab.prefab.PrefabName;
            boundingBoxPosition.y -= allPrefab.prefab.yOffset;
          }
          else if (allPrefab.location.Type != PathAbstractions.EAbstractedLocationType.None)
            str = allPrefab.location.Name;
          _node.AddXmlElement("decoration").SetAttrib("type", "model").SetAttrib("name", str).SetAttrib("position", boundingBoxPosition.ToStringNoBlanks()).SetAttrib("rotation", allPrefab.rotation.ToString()).SetAttrib("y_is_groundlevel", "true");
        }
      }
      xmlDocument.SdSave(_path + "/prefabs.xml");
      return true;
    }
    catch (Exception ex)
    {
      Log.Error(ex.ToString());
      Log.Error(ex.StackTrace);
      return false;
    }
  }

  public void Cleanup()
  {
    this.prefabCache.Clear();
    this.prefabCacheRotations.Clear();
    this.prefabMeshExisting.Clear();
  }

  public List<PrefabInstance> GetDynamicPrefabs() => this.allPrefabs;

  public void AddPrefab(PrefabInstance _pi, bool _isPOI = false)
  {
    this.allPrefabs.Add(_pi);
    if (_isPOI)
      this.poiPrefabs.Add(_pi);
    this.isSortNeeded = true;
  }

  public void RemovePrefab(PrefabInstance _pi)
  {
    this.allPrefabs.Remove(_pi);
    this.poiPrefabs.Remove(_pi);
    lock (this.allPrefabsSorted)
      this.allPrefabsSorted.Remove(_pi);
  }

  public List<PrefabInstance> GetPOIPrefabs() => this.poiPrefabs;

  public void ClearTraders() => this.traderAreas.Clear();

  public void AddTrader(TraderArea _ta)
  {
    this.ProtectSizeXMax = Utils.FastMax(this.ProtectSizeXMax, _ta.ProtectSize.x);
    this.traderAreas.Add(_ta);
    this.traderAreas.Sort((IComparer<TraderArea>) this.traderComparer);
  }

  public List<TraderArea> GetTraderAreas() => this.traderAreas;

  public bool IsWithinTraderArea(Vector3i _minPos, Vector3i _maxPos)
  {
    for (int index = 0; index < this.traderAreas.Count; ++index)
    {
      if (this.traderAreas[index].Overlaps(_minPos, _maxPos))
        return true;
    }
    return false;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public int TraderBinarySearch(int x)
  {
    int num1 = x - this.ProtectSizeXMax;
    int num2 = 0;
    int num3 = this.traderAreas.Count;
    while (num2 < num3)
    {
      int index = (num2 + num3) / 2;
      if (this.traderAreas[index].ProtectPosition.x < num1)
        num2 = index + 1;
      else
        num3 = index;
    }
    return num2;
  }

  public TraderArea GetTraderAtPosition(Vector3i _pos, int _padding)
  {
    TraderArea traderAtPosition = (TraderArea) null;
    int num1 = -_padding;
    int index = this.TraderBinarySearch(_pos.x - _padding);
    for (int count = this.traderAreas.Count; index < count; ++index)
    {
      TraderArea traderArea = this.traderAreas[index];
      int num2 = _pos.x - traderArea.ProtectPosition.x;
      if (num2 >= num1)
      {
        if (num2 < traderArea.ProtectSize.x + _padding)
        {
          int num3 = _pos.z - traderArea.ProtectPosition.z;
          if (num3 >= num1 && num3 < traderArea.ProtectSize.z + _padding)
          {
            traderAtPosition = traderArea;
            break;
          }
        }
      }
      else
        break;
    }
    return traderAtPosition;
  }

  public void CopyAllPrefabsIntoWorld(World _world, bool _bOverwriteExistingBlocks = false)
  {
    for (int index = 0; index < this.allPrefabs.Count; ++index)
    {
      if (this.allPrefabs[index].standaloneBlockSize == 0)
        this.allPrefabs[index].CopyIntoWorld(_world, true, _bOverwriteExistingBlocks, FastTags<TagGroup.Global>.none);
      else
        Log.Warning("Prefab with standaloneBlockSize={0} not supported", new object[1]
        {
          (object) this.allPrefabs[index].standaloneBlockSize
        });
    }
  }

  public void CleanAllPrefabsFromWorld(World _world)
  {
    for (int index = 0; index < this.allPrefabs.Count; ++index)
      this.allPrefabs[index].CleanFromWorld(_world, true);
  }

  public void ClearAllPrefabs()
  {
    foreach (PrefabInstance allPrefab in this.allPrefabs)
      this.CallPrefabRemovedEvent(allPrefab);
    this.allPrefabs.Clear();
    this.poiPrefabs.Clear();
    lock (this.allPrefabsSorted)
      this.allPrefabsSorted.Clear();
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void CallPrefabRemovedEvent(PrefabInstance _prefabInstance)
  {
    if (this.OnPrefabRemoved == null)
      return;
    this.OnPrefabRemoved(_prefabInstance);
  }

  public void CallPrefabChangedEvent(PrefabInstance _prefabInstance)
  {
    this.isSortNeeded = true;
    if (this.OnPrefabChanged == null)
      return;
    this.OnPrefabChanged(_prefabInstance);
  }

  public Prefab GetPrefab(
    string _name,
    bool _applyMapping = true,
    bool _fixChildblocks = true,
    bool _allowMissingBlocks = false)
  {
    lock (this.prefabCache)
    {
      if (this.prefabCache.ContainsKey(_name))
        return this.prefabCache[_name];
      Prefab prefab = new Prefab();
      if (!prefab.Load(_name, _applyMapping, _fixChildblocks, _allowMissingBlocks))
        return (Prefab) null;
      this.prefabCache[_name] = prefab;
      return prefab;
    }
  }

  public Prefab GetPrefabRotated(
    string _name,
    int _rotation,
    bool _applyMapping = true,
    bool _fixChildblocks = true,
    bool _allowMissingBlocks = false)
  {
    _rotation &= 3;
    lock (this.prefabCache)
    {
      Prefab[] prefabArray;
      if (this.prefabCacheRotations.TryGetValue(_name, out prefabArray))
      {
        if (prefabArray[_rotation] != null)
          return prefabArray[_rotation];
      }
      else
      {
        prefabArray = new Prefab[4];
        this.prefabCacheRotations[_name] = prefabArray;
      }
      Prefab prefabRotated = this.GetPrefab(_name, _applyMapping, _fixChildblocks && _rotation == 0, _allowMissingBlocks);
      if (prefabRotated == null)
        return (Prefab) null;
      if (_rotation > 0)
      {
        prefabRotated = prefabRotated.Clone(true);
        prefabRotated.RotateY(true, _rotation);
      }
      prefabArray[_rotation] = prefabRotated;
      return prefabRotated;
    }
  }

  public void CreateBoundingBoxes()
  {
    for (int index = 0; index < this.allPrefabs.Count; ++index)
      this.allPrefabs[index].CreateBoundingBox(false);
  }

  public PrefabInstance GetPrefab(int _id)
  {
    for (int index = 0; index < this.allPrefabs.Count; ++index)
    {
      if (this.allPrefabs[index].id == _id)
        return this.allPrefabs[index];
    }
    return (PrefabInstance) null;
  }

  public bool IsActivePrefab(int _id)
  {
    PrefabInstance prefab = this.GetPrefab(_id);
    return prefab != null && prefab == this.ActivePrefab;
  }

  public PrefabInstance CreateNewPrefabAndActivate(
    PathAbstractions.AbstractedLocation _location,
    Vector3i _position,
    Prefab _bad,
    bool _bSetActive = true)
  {
    if (_bad == null)
      _bad = new Prefab(new Vector3i(3, 3, 3));
    PrefabInstance _pi = new PrefabInstance(this.GetNextId(), _location, _position, (byte) 0, _bad, 0);
    _pi.CreateBoundingBox();
    this.AddPrefab(_pi);
    if (_bSetActive)
      SelectionBoxManager.Instance.SetActive("DynamicPrefabs", _pi.name, true);
    if (this.OnPrefabLoaded != null)
      this.OnPrefabLoaded(_pi);
    return _pi;
  }

  public PrefabInstance RemoveActivePrefab(World _world)
  {
    if (this.ActivePrefab == null)
      return (PrefabInstance) null;
    PrefabInstance activePrefab = this.ActivePrefab;
    this.RemovePrefabAndSelection(_world, activePrefab, true);
    this.ActivePrefab = (PrefabInstance) null;
    return activePrefab;
  }

  public void RemovePrefabAndSelection(World _world, PrefabInstance _prefab, bool _bCleanFromWorld)
  {
    if (_bCleanFromWorld)
      _prefab.CleanFromWorld(_world, true);
    this.RemovePrefab(_prefab);
    SelectionBoxManager.Instance.GetCategory("DynamicPrefabs").RemoveBox(_prefab.name);
    SelectionBoxManager.Instance.GetCategory("TraderTeleport").RemoveBox(_prefab.name);
    SelectionBoxManager.Instance.GetCategory("InfoVolume").RemoveBox(_prefab.name);
    SelectionBoxManager.Instance.GetCategory("WallVolume").RemoveBox(_prefab.name);
    SelectionBoxManager.Instance.GetCategory("TriggerVolume").RemoveBox(_prefab.name);
    for (int index = 0; index < _prefab.prefab.SleeperVolumes.Count; ++index)
    {
      if (_prefab.prefab.SleeperVolumes[index].used)
        SelectionBoxManager.Instance.GetCategory("SleeperVolume").RemoveBox($"{_prefab.name}_{index.ToString()}");
    }
    SelectionBoxManager.Instance.GetCategory("POIMarker").Clear();
    SelectionBoxManager.Instance.GetCategory("SleeperVolume").RemoveBox(_prefab.name);
    this.CallPrefabRemovedEvent(_prefab);
  }

  public virtual void DecorateChunk(World _world, Chunk _chunk)
  {
    this.DecorateChunk(_world, _chunk, false);
  }

  public void DecorateChunk(World _world, Chunk _chunk, bool _bForceOverwriteBlocks = false)
  {
    int blockWorldPosX = _chunk.GetBlockWorldPosX(0);
    int blockWorldPosZ = _chunk.GetBlockWorldPosZ(0);
    this.GetPrefabsAtXZ(blockWorldPosX, blockWorldPosX + 15, blockWorldPosZ, blockWorldPosZ + 15, this.decorateChunkPIs);
    this.decorateChunkPIs.Sort(new Comparison<PrefabInstance>(this.prefabInstanceSizeComparison));
    for (int index = 0; index < this.decorateChunkPIs.Count; ++index)
    {
      PrefabInstance decorateChunkPi = this.decorateChunkPIs[index];
      if (decorateChunkPi.Overlaps(_chunk))
        decorateChunkPi.CopyIntoChunk(_world, _chunk, _bForceOverwriteBlocks);
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public int prefabInstanceSizeComparison(PrefabInstance _a, PrefabInstance _b)
  {
    int num = _a.boundingBoxSize.x * _a.boundingBoxSize.z;
    return (_b.boundingBoxSize.x * _b.boundingBoxSize.z).CompareTo(num);
  }

  public bool IsEntityInPrefab(int _entityId)
  {
    for (int index = 0; index < this.allPrefabs.Count; ++index)
    {
      if (this.allPrefabs[index].Contains(_entityId))
        return true;
    }
    return false;
  }

  public bool OnSelectionBoxActivated(string _category, string _name, bool _bActivated)
  {
    if (_bActivated)
    {
      (SelectionCategory, SelectionBox)? selection = SelectionBoxManager.Instance.Selection;
      ref (SelectionCategory, SelectionBox)? local = ref selection;
      SelectionBox selectionBox = local.HasValue ? local.GetValueOrDefault().Item2 : (SelectionBox) null;
      if (Object.op_Equality((Object) selectionBox, (Object) null))
      {
        Log.Error("Prefab SelectionBox selected but no prefab defined (OSBA)!");
        return true;
      }
      if (selectionBox.UserData is PrefabInstance userData)
      {
        this.ActivePrefab = userData;
        this.ActivePrefab.UpdateImposterView();
      }
      else
      {
        Log.Error("Selected prefab SelectionBox has no PrefabInstance assigned");
        StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(_name, '.', 1);
        int _result = 0;
        if (separatorPositions.TotalFound >= 1 && StringParsers.TryParseSInt32(_name, out _result, separatorPositions.Sep1 + 1, separatorPositions.Sep2 - 1))
          this.ActivePrefab = this.GetPrefab(_result);
      }
    }
    else
      this.ActivePrefab = (PrefabInstance) null;
    return true;
  }

  public void OnSelectionBoxMoved(string _category, string _name, Vector3 _moveVector)
  {
    if (this.ActivePrefab == null)
      return;
    (SelectionCategory, SelectionBox)? selection = SelectionBoxManager.Instance.Selection;
    ref (SelectionCategory, SelectionBox)? local = ref selection;
    if (Object.op_Equality(local.HasValue ? (Object) local.GetValueOrDefault().Item2 : (Object) null, (Object) null))
    {
      Log.Error("Prefab SelectionBox selected but no prefab defined (OSBM)!");
    }
    else
    {
      this.ActivePrefab.MoveBoundingBox(new Vector3i(_moveVector));
      this.ActivePrefab.UpdateImposterView();
    }
  }

  public void OnSelectionBoxSized(
    string _category,
    string _name,
    int _dTop,
    int _dBottom,
    int _dNorth,
    int _dSouth,
    int _dEast,
    int _dWest)
  {
    if (GameManager.Instance.IsEditMode() && !PrefabEditModeManager.Instance.IsActive() || this.ActivePrefab == null)
      return;
    this.ActivePrefab.ResizeBoundingBox(new Vector3i(_dEast + _dWest, _dTop + _dBottom, _dNorth + _dSouth));
    this.ActivePrefab.MoveBoundingBox(new Vector3i(-_dWest, -_dBottom, -_dSouth));
  }

  public void OnSelectionBoxMirrored(Vector3i _axis)
  {
  }

  public bool OnSelectionBoxDelete(string _category, string _name)
  {
    SelectionBox box = SelectionBoxManager.Instance.GetCategory(_category)?.GetBox(_name);
    if (Object.op_Equality((Object) box, (Object) null))
    {
      Log.Error("SelectionBox null (OSBD)");
      return false;
    }
    if (box.UserData is PrefabInstance userData)
      userData.DestroyImposterView();
    return false;
  }

  public bool OnSelectionBoxIsAvailable(string _category, EnumSelectionBoxAvailabilities _criteria)
  {
    return _criteria == EnumSelectionBoxAvailabilities.CanResize ? PrefabEditModeManager.Instance.IsActive() : _criteria == EnumSelectionBoxAvailabilities.CanShowProperties;
  }

  public void OnSelectionBoxShowProperties(bool _bVisible, GUIWindowManager _windowManager)
  {
    XUiC_EditorPanelSelector childByType = _windowManager.playerUI.xui.FindWindowGroupByName(XUiC_EditorPanelSelector.ID).GetChildByType<XUiC_EditorPanelSelector>();
    if (childByType == null)
      return;
    childByType.SetSelected("prefabList");
    _windowManager.SwitchVisible(XUiC_InGameMenuWindow.ID);
  }

  public void OnSelectionBoxRotated(string _category, string _name)
  {
    (SelectionCategory, SelectionBox)? selection = SelectionBoxManager.Instance.Selection;
    ref (SelectionCategory, SelectionBox)? local = ref selection;
    if (Object.op_Equality(local.HasValue ? (Object) local.GetValueOrDefault().Item2 : (Object) null, (Object) null))
    {
      Log.Error("Prefab SelectionBox selected but no prefab defined (OSBR)!");
    }
    else
    {
      this.ActivePrefab.RotateAroundY();
      this.ActivePrefab.UpdateImposterView();
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public int PrefabBinarySearch(float x)
  {
    if (this.isSortNeeded)
      this.SortPrefabs();
    int num1 = (int) x - 200;
    int num2 = 0;
    int num3 = this.allPrefabsSorted.Count;
    while (num2 < num3)
    {
      int index = (num2 + num3) / 2;
      if (this.allPrefabsSorted[index].boundingBoxPosition.x < num1)
        num2 = index + 1;
      else
        num3 = index;
    }
    return num2;
  }

  public PrefabInstance GetPrefabAtPosition(Vector3 _position, bool _checkTags = true)
  {
    PrefabInstance prefabAtPosition = (PrefabInstance) null;
    Vector3i vector3i = Vector3i.Floor(_position);
    int index1 = this.PrefabBinarySearch((float) vector3i.x);
    for (int count = this.allPrefabsSorted.Count; index1 < count; ++index1)
    {
      PrefabInstance prefabInstance1 = this.allPrefabsSorted[index1];
      int num1 = vector3i.x - prefabInstance1.boundingBoxPosition.x;
      if (num1 >= 0)
      {
        if (num1 < prefabInstance1.boundingBoxSize.x)
        {
          int num2 = vector3i.z - prefabInstance1.boundingBoxPosition.z;
          if (num2 >= 0 && num2 < prefabInstance1.boundingBoxSize.z)
          {
            int num3 = vector3i.y - prefabInstance1.boundingBoxPosition.y;
            if (num3 >= 0 && num3 < prefabInstance1.boundingBoxSize.y)
            {
              FastTags<TagGroup.Poi> tags;
              if (_checkTags)
              {
                tags = prefabInstance1.prefab.Tags;
                if (tags.Test_AnySet(this.streetTileTag))
                  continue;
              }
              prefabAtPosition = prefabInstance1;
              for (int index2 = index1 + 1; index2 < count; ++index2)
              {
                PrefabInstance prefabInstance2 = this.allPrefabsSorted[index2];
                int num4 = vector3i.x - prefabInstance2.boundingBoxPosition.x;
                if (num4 >= 0)
                {
                  if (num4 < prefabInstance2.boundingBoxSize.x)
                  {
                    int num5 = vector3i.z - prefabInstance2.boundingBoxPosition.z;
                    if (num5 >= 0 && num5 < prefabInstance2.boundingBoxSize.z)
                    {
                      int num6 = vector3i.y - prefabInstance2.boundingBoxPosition.y;
                      if (num6 >= 0 && num6 < prefabInstance2.boundingBoxSize.y)
                      {
                        if (_checkTags)
                        {
                          tags = prefabInstance2.prefab.Tags;
                          if (tags.Test_AnySet(this.streetTileTag))
                            continue;
                        }
                        if (prefabAtPosition.boundingBoxPosition.x != prefabInstance2.boundingBoxPosition.x || prefabAtPosition.boundingBoxSize.x >= prefabInstance2.boundingBoxPosition.x)
                        {
                          prefabAtPosition = prefabInstance2;
                          break;
                        }
                        break;
                      }
                    }
                  }
                }
                else
                  break;
              }
              break;
            }
          }
        }
      }
      else
        break;
    }
    return prefabAtPosition;
  }

  public void GetPrefabsAtXZ(
    int _xMin,
    int _xMax,
    int _zMin,
    int _zMax,
    List<PrefabInstance> _list)
  {
    lock (this.allPrefabsSorted)
    {
      _list.Clear();
      if (this.isSortNeeded)
      {
        _list.AddRange((IEnumerable<PrefabInstance>) this.allPrefabsSorted);
      }
      else
      {
        int count = this.allPrefabsSorted.Count;
        int num1 = Utils.Fastfloor((float) _xMin) - 200;
        int num2 = 0;
        int num3 = count;
        while (num2 < num3)
        {
          int index = (num2 + num3) / 2;
          if (this.allPrefabsSorted[index].boundingBoxPosition.x < num1)
            num2 = index + 1;
          else
            num3 = index;
        }
        for (int index = num2; index < count; ++index)
        {
          PrefabInstance prefabInstance = this.allPrefabsSorted[index];
          if (prefabInstance.boundingBoxPosition.x > _xMax)
            break;
          if (prefabInstance.boundingBoxPosition.x + prefabInstance.boundingBoxSize.x > _xMin && prefabInstance.boundingBoxPosition.z <= _zMax && prefabInstance.boundingBoxPosition.z + prefabInstance.boundingBoxSize.z > _zMin)
            _list.Add(prefabInstance);
        }
      }
    }
  }

  public virtual void GetPrefabsAround(
    Vector3 _position,
    float _distance,
    Dictionary<int, PrefabInstance> _prefabs)
  {
    float num1 = _distance * _distance;
    for (int index = 0; index < this.allPrefabs.Count; ++index)
    {
      PrefabInstance allPrefab = this.allPrefabs[index];
      double num2 = (double) _position.x - ((double) allPrefab.boundingBoxPosition.x + (double) allPrefab.boundingBoxSize.x * 0.5);
      float num3 = _position.z - ((float) allPrefab.boundingBoxPosition.z + (float) allPrefab.boundingBoxSize.z * 0.5f);
      if (num2 * num2 + (double) num3 * (double) num3 <= (double) num1)
      {
        string str = allPrefab.prefab.distantPOIOverride == null ? allPrefab.prefab.PrefabName : allPrefab.prefab.distantPOIOverride;
        bool flag;
        if (!this.prefabMeshExisting.TryGetValue(str, out flag))
        {
          flag = PathAbstractions.PrefabImpostersSearchPaths.GetLocation(str).Type != PathAbstractions.EAbstractedLocationType.None;
          this.prefabMeshExisting[str] = flag;
        }
        if (flag)
          _prefabs.Add(allPrefab.id, allPrefab);
      }
    }
  }

  public virtual void GetPrefabsAround(
    Vector3 _position,
    float _nearDistance,
    float _farDistance,
    Dictionary<int, PrefabInstance> _prefabsFar,
    Dictionary<int, PrefabInstance> _prefabsNear)
  {
    Vector2 vector2_1;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2_1).\u002Ector(_position.x, _position.z);
    float num1 = _nearDistance;
    float num2 = _farDistance * _farDistance;
    for (int index = this.PrefabBinarySearch(vector2_1.x - _farDistance); index < this.allPrefabsSorted.Count; ++index)
    {
      PrefabInstance prefabInstance = this.allPrefabsSorted[index];
      if ((double) _position.x - (double) prefabInstance.boundingBoxPosition.x < -(double) _farDistance)
        break;
      double num3 = (double) _position.x - ((double) prefabInstance.boundingBoxPosition.x + (double) prefabInstance.boundingBoxSize.x * 0.5);
      float num4 = _position.z - ((float) prefabInstance.boundingBoxPosition.z + (float) prefabInstance.boundingBoxSize.z * 0.5f);
      if (num3 * num3 + (double) num4 * (double) num4 <= (double) num2)
      {
        Vector2 vector2_2;
        vector2_2.x = (float) prefabInstance.boundingBoxPosition.x;
        vector2_2.y = (float) prefabInstance.boundingBoxPosition.z;
        Vector2 vector2_3;
        vector2_3.x = vector2_2.x + (float) prefabInstance.boundingBoxSize.x;
        vector2_3.y = vector2_2.y;
        Vector2 vector2_4;
        vector2_4.x = vector2_2.x;
        vector2_4.y = vector2_2.y + (float) prefabInstance.boundingBoxSize.z;
        Vector2 vector2_5;
        vector2_5.x = vector2_3.x;
        vector2_5.y = vector2_4.y;
        if (!DynamicMeshManager.IsOutsideDistantTerrain(vector2_2.x, vector2_3.x, vector2_2.y, vector2_4.y))
        {
          Vector2 vector2_6 = Vector2.op_Subtraction(vector2_2, vector2_1);
          if ((double) Utils.FastMax(Utils.FastAbs(vector2_6.x), Utils.FastAbs(vector2_6.y)) < (double) num1)
          {
            Vector2 vector2_7 = Vector2.op_Subtraction(vector2_3, vector2_1);
            if ((double) Utils.FastMax(Utils.FastAbs(vector2_7.x), Utils.FastAbs(vector2_7.y)) < (double) num1)
            {
              Vector2 vector2_8 = Vector2.op_Subtraction(vector2_4, vector2_1);
              if ((double) Utils.FastMax(Utils.FastAbs(vector2_8.x), Utils.FastAbs(vector2_8.y)) < (double) num1)
              {
                Vector2 vector2_9 = Vector2.op_Subtraction(vector2_5, vector2_1);
                if ((double) Utils.FastMax(Utils.FastAbs(vector2_9.x), Utils.FastAbs(vector2_9.y)) < (double) num1)
                {
                  _prefabsNear.Add(prefabInstance.id, prefabInstance);
                  continue;
                }
              }
            }
          }
          string str = prefabInstance.prefab.distantPOIOverride == null ? prefabInstance.prefab.PrefabName : prefabInstance.prefab.distantPOIOverride;
          bool flag;
          if (!this.prefabMeshExisting.TryGetValue(str, out flag))
          {
            flag = PathAbstractions.PrefabImpostersSearchPaths.GetLocation(str).Type != PathAbstractions.EAbstractedLocationType.None;
            this.prefabMeshExisting[str] = flag;
          }
          if (flag)
            _prefabsFar.Add(prefabInstance.id, prefabInstance);
        }
      }
    }
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public bool ValidPrefabForQuest(
    EntityTrader trader,
    PrefabInstance prefab,
    FastTags<TagGroup.Global> questTag,
    List<Vector2> usedPOILocations = null,
    int entityIDforQuests = -1,
    BiomeFilterTypes biomeFilterType = BiomeFilterTypes.SameBiome,
    string biomeFilter = "")
  {
    if (!prefab.prefab.bSleeperVolumes || !prefab.prefab.GetQuestTag(questTag))
      return false;
    Vector2 prefabPos;
    // ISSUE: explicit constructor call
    ((Vector2) ref prefabPos).\u002Ector((float) prefab.boundingBoxPosition.x, (float) prefab.boundingBoxPosition.z);
    if (usedPOILocations != null && usedPOILocations.Contains(prefabPos) || QuestEventManager.Current.CheckForPOILockouts(entityIDforQuests, prefabPos, out ulong _) != QuestEventManager.POILockoutReasonTypes.None)
      return false;
    Vector2 vector2 = new Vector2((float) prefab.boundingBoxPosition.x + (float) prefab.boundingBoxSize.x / 2f, (float) prefab.boundingBoxPosition.z + (float) prefab.boundingBoxSize.z / 2f);
    if (biomeFilterType != BiomeFilterTypes.AnyBiome)
    {
      string[] strArray = (string[]) null;
      BiomeDefinition biomeAt1 = GameManager.Instance.World.ChunkCache.ChunkProvider.GetBiomeProvider().GetBiomeAt((int) prefabPos.x, (int) prefabPos.y);
      if (biomeFilterType == BiomeFilterTypes.OnlyBiome)
      {
        if (biomeAt1.m_sBiomeName != biomeFilter)
          return false;
      }
      else if (biomeFilterType == BiomeFilterTypes.ExcludeBiome)
      {
        if (strArray == null)
          strArray = biomeFilter.Split(',', StringSplitOptions.None);
        bool flag = false;
        for (int index = 0; index < strArray.Length; ++index)
        {
          if (biomeAt1.m_sBiomeName == strArray[index])
          {
            flag = true;
            break;
          }
        }
        if (flag)
          return false;
      }
      else if (biomeFilterType == BiomeFilterTypes.SameBiome && Object.op_Inequality((Object) trader, (Object) null))
      {
        BiomeDefinition biomeAt2 = GameManager.Instance.World.ChunkCache.ChunkProvider.GetBiomeProvider().GetBiomeAt((int) trader.position.x, (int) trader.position.z);
        if (biomeAt1 != biomeAt2)
          return false;
      }
    }
    return true;
  }

  public virtual PrefabInstance GetRandomPOINearTrader(
    EntityTrader trader,
    FastTags<TagGroup.Global> questTag,
    byte difficulty,
    List<Vector2> usedPOILocations = null,
    int entityIDforQuests = -1,
    BiomeFilterTypes biomeFilterType = BiomeFilterTypes.SameBiome,
    string biomeFilter = "")
  {
    World world = GameManager.Instance.World;
    for (int index1 = 0; index1 < 3; ++index1)
    {
      List<PrefabInstance> prefabsForTrader = QuestEventManager.Current.GetPrefabsForTrader(trader.traderArea, (int) difficulty, index1, world.GetGameRandom());
      if (prefabsForTrader != null)
      {
        for (int index2 = 0; index2 < prefabsForTrader.Count; ++index2)
        {
          PrefabInstance prefab = prefabsForTrader[index2];
          if (this.ValidPrefabForQuest(trader, prefab, questTag, usedPOILocations, entityIDforQuests, biomeFilterType, biomeFilter))
            return prefab;
        }
      }
    }
    return (PrefabInstance) null;
  }

  public virtual PrefabInstance GetRandomPOINearWorldPos(
    Vector2 worldPos,
    int minSearchDistance,
    int maxSearchDistance,
    FastTags<TagGroup.Global> questTag,
    byte difficulty,
    List<Vector2> usedPOILocations = null,
    int entityIDforQuests = -1,
    BiomeFilterTypes biomeFilterType = BiomeFilterTypes.SameBiome,
    string biomeFilter = "")
  {
    List<PrefabInstance> byDifficultyTier = QuestEventManager.Current.GetPrefabsByDifficultyTier((int) difficulty);
    if (byDifficultyTier == null)
      return (PrefabInstance) null;
    string[] strArray = (string[]) null;
    BiomeDefinition biomeAt1 = GameManager.Instance.World.ChunkCache.ChunkProvider.GetBiomeProvider().GetBiomeAt((int) worldPos.x, (int) worldPos.y);
    World world = GameManager.Instance.World;
    for (int index1 = 0; index1 < 50; ++index1)
    {
      int index2 = world.GetGameRandom().RandomRange(byDifficultyTier.Count);
      PrefabInstance randomPoiNearWorldPos = byDifficultyTier[index2];
      if (randomPoiNearWorldPos.prefab.bSleeperVolumes && randomPoiNearWorldPos.prefab.GetQuestTag(questTag) && (int) randomPoiNearWorldPos.prefab.DifficultyTier == (int) difficulty)
      {
        Vector2 prefabPos;
        // ISSUE: explicit constructor call
        ((Vector2) ref prefabPos).\u002Ector((float) randomPoiNearWorldPos.boundingBoxPosition.x, (float) randomPoiNearWorldPos.boundingBoxPosition.z);
        if ((usedPOILocations == null || !usedPOILocations.Contains(prefabPos)) && QuestEventManager.Current.CheckForPOILockouts(entityIDforQuests, prefabPos, out ulong _) == QuestEventManager.POILockoutReasonTypes.None)
        {
          Vector2 vector2_1;
          // ISSUE: explicit constructor call
          ((Vector2) ref vector2_1).\u002Ector((float) randomPoiNearWorldPos.boundingBoxPosition.x + (float) randomPoiNearWorldPos.boundingBoxSize.x / 2f, (float) randomPoiNearWorldPos.boundingBoxPosition.z + (float) randomPoiNearWorldPos.boundingBoxSize.z / 2f);
          if (biomeFilterType != BiomeFilterTypes.AnyBiome)
          {
            BiomeDefinition biomeAt2 = GameManager.Instance.World.ChunkCache.ChunkProvider.GetBiomeProvider().GetBiomeAt((int) prefabPos.x, (int) prefabPos.y);
            if (biomeFilterType == BiomeFilterTypes.OnlyBiome)
            {
              if (biomeAt2.m_sBiomeName != biomeFilter)
                continue;
            }
            else if (biomeFilterType == BiomeFilterTypes.ExcludeBiome)
            {
              if (strArray == null)
                strArray = biomeFilter.Split(',', StringSplitOptions.None);
              bool flag = false;
              for (int index3 = 0; index3 < strArray.Length; ++index3)
              {
                if (biomeAt2.m_sBiomeName == strArray[index3])
                {
                  flag = true;
                  break;
                }
              }
              if (flag)
                continue;
            }
            else if (biomeFilterType == BiomeFilterTypes.SameBiome && biomeAt2 != biomeAt1)
              continue;
          }
          Vector2 vector2_2 = Vector2.op_Subtraction(worldPos, vector2_1);
          float sqrMagnitude = ((Vector2) ref vector2_2).sqrMagnitude;
          if ((double) sqrMagnitude < (double) maxSearchDistance && (double) sqrMagnitude > (double) minSearchDistance)
            return randomPoiNearWorldPos;
        }
      }
    }
    return (PrefabInstance) null;
  }

  public virtual PrefabInstance GetClosestPOIToWorldPos(
    FastTags<TagGroup.Global> questTag,
    Vector2 worldPos,
    List<Vector2> excludeList = null,
    int maxSearchDist = -1,
    bool ignoreCurrentPOI = false,
    BiomeFilterTypes biomeFilterType = BiomeFilterTypes.SameBiome,
    string biomeFilter = "")
  {
    PrefabInstance closestPoiToWorldPos = (PrefabInstance) null;
    string[] strArray = (string[]) null;
    Vector3 _pos;
    // ISSUE: explicit constructor call
    ((Vector3) ref _pos).\u002Ector(worldPos.x, 0.0f, worldPos.y);
    float num = maxSearchDist < 0 ? float.MaxValue : (float) maxSearchDist;
    IBiomeProvider biomeProvider = GameManager.Instance.World.ChunkCache.ChunkProvider.GetBiomeProvider();
    BiomeDefinition biomeAt1 = biomeProvider?.GetBiomeAt((int) worldPos.x, (int) worldPos.y);
    for (int index1 = 0; index1 < this.poiPrefabs.Count; ++index1)
    {
      PrefabInstance poiPrefab = this.poiPrefabs[index1];
      if (!poiPrefab.prefab.PrefabName.Contains("rwg_tile") && (poiPrefab.prefab.GetQuestTag(questTag) || questTag.IsEmpty))
      {
        if (ignoreCurrentPOI)
        {
          _pos.y = (float) poiPrefab.boundingBoxPosition.y;
          if (poiPrefab.Overlaps(_pos))
            continue;
        }
        Vector2 vector2_1;
        // ISSUE: explicit constructor call
        ((Vector2) ref vector2_1).\u002Ector((float) poiPrefab.boundingBoxPosition.x + (float) poiPrefab.boundingBoxSize.x / 2f, (float) poiPrefab.boundingBoxPosition.z + (float) poiPrefab.boundingBoxSize.z / 2f);
        if (excludeList == null || !excludeList.Contains(new Vector2((float) poiPrefab.boundingBoxPosition.x, (float) poiPrefab.boundingBoxPosition.z)))
        {
          if (biomeFilterType != BiomeFilterTypes.AnyBiome)
          {
            BiomeDefinition biomeAt2 = biomeProvider?.GetBiomeAt((int) vector2_1.x, (int) vector2_1.y);
            switch (biomeFilterType)
            {
              case BiomeFilterTypes.OnlyBiome:
                if (!(biomeAt2.m_sBiomeName != biomeFilter))
                  break;
                continue;
              case BiomeFilterTypes.ExcludeBiome:
                if (strArray == null)
                  strArray = biomeFilter.Split(',', StringSplitOptions.None);
                bool flag = false;
                for (int index2 = 0; index2 < strArray.Length; ++index2)
                {
                  if (biomeAt2.m_sBiomeName == strArray[index2])
                  {
                    flag = true;
                    break;
                  }
                }
                if (!flag)
                  break;
                continue;
              case BiomeFilterTypes.SameBiome:
                if (biomeAt2 != biomeAt1)
                  continue;
                break;
            }
          }
          Vector2 vector2_2 = Vector2.op_Subtraction(worldPos, vector2_1);
          float sqrMagnitude = ((Vector2) ref vector2_2).sqrMagnitude;
          if ((double) sqrMagnitude < (double) num)
          {
            num = sqrMagnitude;
            closestPoiToWorldPos = poiPrefab;
          }
        }
      }
    }
    return closestPoiToWorldPos;
  }

  public virtual PrefabInstance GetPrefabFromWorldPos(int x, int z)
  {
    for (int index = 0; index < this.allPrefabs.Count; ++index)
    {
      if (this.allPrefabs[index].boundingBoxPosition.x == x && this.allPrefabs[index].boundingBoxPosition.z == z && !this.allPrefabs[index].prefab.PrefabName.Contains("rwg_tile") && !this.allPrefabs[index].prefab.PrefabName.Contains("part_"))
        return this.allPrefabs[index];
    }
    return (PrefabInstance) null;
  }

  public virtual PrefabInstance GetPrefabFromWorldPosInside(int _x, int _z)
  {
    for (int index = 0; index < this.allPrefabs.Count; ++index)
    {
      PrefabInstance allPrefab = this.allPrefabs[index];
      int x = allPrefab.boundingBoxPosition.x;
      int z = allPrefab.boundingBoxPosition.z;
      if (x <= _x && z <= _z && x + allPrefab.boundingBoxSize.x >= _x && z + allPrefab.boundingBoxSize.z >= _z)
        return this.allPrefabs[index];
    }
    return (PrefabInstance) null;
  }

  public virtual PrefabInstance GetPrefabFromWorldPosInsideWithOffset(int _x, int _z, int _offset)
  {
    for (int index = 0; index < this.allPrefabs.Count; ++index)
    {
      PrefabInstance allPrefab = this.allPrefabs[index];
      int num1 = allPrefab.boundingBoxPosition.x - _offset;
      int num2 = allPrefab.boundingBoxPosition.z - _offset;
      int num3 = allPrefab.boundingBoxPosition.x + allPrefab.boundingBoxSize.x + _offset;
      int num4 = allPrefab.boundingBoxPosition.z + allPrefab.boundingBoxSize.z + _offset;
      int num5 = _x;
      if (num1 <= num5 && num2 <= _z && num3 >= _x && num4 >= _z)
        return this.allPrefabs[index];
    }
    return (PrefabInstance) null;
  }

  public virtual PrefabInstance GetPrefabFromWorldPosInside(int _x, int _y, int _z)
  {
    for (int index = 0; index < this.allPrefabs.Count; ++index)
    {
      PrefabInstance allPrefab = this.allPrefabs[index];
      int x = allPrefab.boundingBoxPosition.x;
      int y = allPrefab.boundingBoxPosition.y;
      int z = allPrefab.boundingBoxPosition.z;
      if (x <= _x && y <= _y && z <= _z && x + allPrefab.boundingBoxSize.x >= _x && y + allPrefab.boundingBoxSize.y >= _y && z + allPrefab.boundingBoxSize.z >= _z)
        return this.allPrefabs[index];
    }
    return (PrefabInstance) null;
  }

  public virtual List<PrefabInstance> GetPrefabsFromWorldPosInside(
    Vector3 _pos,
    FastTags<TagGroup.Global> _questTags)
  {
    _pos = Vector3.op_Addition(_pos, this.boundsPad);
    List<PrefabInstance> source = new List<PrefabInstance>();
    Bounds bounds = new Bounds();
    for (int index = 0; index < this.allPrefabs.Count; ++index)
    {
      PrefabInstance allPrefab = this.allPrefabs[index];
      if (allPrefab.prefab.GetQuestTag(_questTags))
      {
        ((Bounds) ref bounds).SetMinMax((Vector3) allPrefab.boundingBoxPosition, Vector3.op_Subtraction((Vector3) (allPrefab.boundingBoxPosition + allPrefab.boundingBoxSize), this.boundsPad));
        if (((Bounds) ref bounds).Contains(_pos))
          source.AddRange((IEnumerable<PrefabInstance>) this.GetPrefabsIntersecting(allPrefab));
      }
    }
    return source.OrderByDescending<PrefabInstance, int>((Func<PrefabInstance, int>) ([PublicizedFrom(EAccessModifier.Internal)] (pi) => pi.boundingBoxSize.x * pi.boundingBoxSize.z)).ToList<PrefabInstance>();
  }

  public virtual List<PrefabInstance> GetPrefabsIntersecting(PrefabInstance parentPI)
  {
    List<PrefabInstance> source = new List<PrefabInstance>();
    source.Add(parentPI);
    Bounds bounds1 = new Bounds();
    ((Bounds) ref bounds1).SetMinMax((Vector3) parentPI.boundingBoxPosition, Vector3.op_Subtraction((Vector3) (parentPI.boundingBoxPosition + parentPI.boundingBoxSize), this.boundsPad));
    float num = ((Bounds) ref bounds1).size.x * ((Bounds) ref bounds1).size.z;
    Bounds bounds2 = new Bounds();
    for (int index = 0; index < this.allPrefabs.Count; ++index)
    {
      PrefabInstance allPrefab = this.allPrefabs[index];
      if (parentPI != allPrefab)
      {
        ((Bounds) ref bounds2).SetMinMax((Vector3) allPrefab.boundingBoxPosition, Vector3.op_Subtraction((Vector3) (allPrefab.boundingBoxPosition + allPrefab.boundingBoxSize), this.boundsPad));
        if (((Bounds) ref bounds1).Intersects(bounds2) && (double) ((Bounds) ref bounds2).size.x * (double) ((Bounds) ref bounds2).size.z < (double) num && !source.Contains(allPrefab))
          source.Add(allPrefab);
      }
    }
    return source.OrderByDescending<PrefabInstance, int>((Func<PrefabInstance, int>) ([PublicizedFrom(EAccessModifier.Internal)] (pi) => pi.boundingBoxSize.x * pi.boundingBoxSize.z)).ToList<PrefabInstance>();
  }

  public IEnumerator CopyPrefabHeightsIntoHeightMap(
    int _heightMapWidth,
    int _heightMapHeight,
    IBackedArray<ushort> _heightData,
    int _heightMapScale = 1,
    ushort[] _topTextures = null)
  {
    MicroStopwatch yieldMs = new MicroStopwatch(true);
    if (this.blockValueTerrainFiller.isair)
    {
      this.blockValueTerrainFiller = Block.GetBlockValue(Constants.cTerrainFillerBlockName);
      this.blockValueTerrainFiller2 = Block.GetBlockValue(Constants.cTerrainFiller2BlockName);
    }
    List<PrefabInstance> allPrefabs = this.GetDynamicPrefabs();
    for (int i = 0; i < allPrefabs.Count; ++i)
    {
      PrefabInstance _pi = allPrefabs[i];
      if (_pi.prefab != null)
      {
        this.copyPrefabsIntoHeightMap(_pi, _heightMapWidth, _heightMapHeight, _heightData, _heightMapScale, _topTextures);
        if (((Stopwatch) yieldMs).ElapsedMilliseconds > (long) Constants.cMaxLoadTimePerFrameMillis)
        {
          yield return (object) null;
          yieldMs.ResetAndRestart();
        }
      }
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void copyPrefabsIntoHeightMap(
    PrefabInstance _pi,
    int _heightMapWidth,
    int _heightMapHeight,
    IBackedArray<ushort> _heightData,
    int _heightMapScale,
    ushort[] _topTextures = null)
  {
    using (IBackedArrayView<ushort> singleView = BackedArrays.CreateSingleView<ushort>(_heightData, BackedArrayHandleMode.ReadWrite))
    {
      int rotation = (int) _pi.rotation;
      Prefab prefab = _pi.prefab;
      int yOffset = prefab.yOffset;
      Vector3i size = prefab.size;
      int x = _pi.boundingBoxPosition.x;
      int y = _pi.boundingBoxPosition.y;
      int z = _pi.boundingBoxPosition.z;
      if (_pi.boundingBoxPosition.x < -_heightMapWidth / 2 || _pi.boundingBoxPosition.x + size.x > _heightMapWidth / 2 || _pi.boundingBoxPosition.z < -_heightMapHeight / 2 || _pi.boundingBoxPosition.z + size.z > _heightMapHeight / 2)
        Log.Warning($"Prefab {_pi.name} outside of the world bounds (position {_pi.boundingBoxPosition})");
      bool flag = _pi.name.Contains("rwg_tile");
      for (int _z = (size.z + _heightMapScale - 1) % _heightMapScale; _z < size.z; _z += _heightMapScale)
      {
        int num1 = _z + z;
        int num2 = (num1 / _heightMapScale + _heightMapHeight / 2) * _heightMapWidth;
        int num3 = (num1 + _heightMapHeight / 2) * _heightMapScale * _heightMapWidth;
        for (int _x = (size.x + _heightMapScale - 1) % _heightMapScale; _x < size.x; _x += _heightMapScale)
        {
          int num4 = _x + x;
          int i = (num4 / _heightMapScale + _heightMapWidth / 2 + num2) % _heightData.Length;
          int index = (num4 + _heightMapWidth / 2) * _heightMapScale + num3;
          for (int _y = 0; _y < size.y; ++_y)
          {
            BlockValue blockNoDamage = prefab.GetBlockNoDamage(rotation, _x, _y, _z);
            WaterValue water = prefab.GetWater(_x, _y, _z);
            Block block = blockNoDamage.Block;
            if ((blockNoDamage.isair || block == null || !block.shape.IsTerrain() ? 1 : (water.HasMass() ? 1 : 0)) != 0)
            {
              if (_y <= -yOffset)
              {
                if (!flag || _y > 0)
                  continue;
              }
              else
                break;
            }
            sbyte density = prefab.GetDensity(rotation, _x, _y, _z);
            float num5 = (float) (y + _y + yOffset) + (float) ((double) -density / 128.0 - 1.0);
            if (flag)
              num5 -= (float) yOffset;
            if ((double) num5 > 0.0)
            {
              ushort num6 = (ushort) ((double) num5 / (1.0 / 257.0));
              if (blockNoDamage.type != this.blockValueTerrainFiller2.type || (int) num6 <= (int) singleView[i])
              {
                if (i >= 0 && i < _heightData.Length && (flag || (int) num6 > (int) singleView[i]))
                  singleView[i] = num6;
                if (block != null && _topTextures != null && !blockNoDamage.isair && blockNoDamage.type != this.blockValueTerrainFiller.type && blockNoDamage.type != this.blockValueTerrainFiller2.type)
                {
                  int sideTextureId = block.GetSideTextureId(blockNoDamage, BlockFace.Top, 0);
                  if (index >= 0 && index < _topTextures.Length)
                    _topTextures[index] = (ushort) sideTextureId;
                }
              }
            }
          }
        }
      }
    }
  }

  public void CalculateStats(
    out int basePrefabCount,
    out int rotatedPrefabsCount,
    out int activePrefabCount,
    out int basePrefabBytes,
    out int rotatedPrefabBytes,
    out int activePrefabBytes)
  {
    ChunkCluster chunkCache = GameManager.Instance.World.ChunkCache;
    lock (this.prefabCache)
    {
      basePrefabCount = this.prefabCache.Count;
      basePrefabBytes = 0;
      foreach (Prefab prefab in this.prefabCache.Values)
        basePrefabBytes += prefab.EstimateOwnedBytes();
      rotatedPrefabsCount = 0;
      rotatedPrefabBytes = 0;
      foreach (Prefab[] prefabArray in this.prefabCacheRotations.Values)
      {
        for (int index = 1; index < prefabArray.Length; ++index)
        {
          Prefab prefab = prefabArray[index];
          if (prefab != null)
          {
            ++rotatedPrefabsCount;
            rotatedPrefabBytes += prefab.EstimateOwnedBytes();
          }
        }
      }
    }
    if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
    {
      activePrefabCount = -1;
      activePrefabBytes = -1;
    }
    else
    {
      List<EntityPlayer> list = GameManager.Instance.World.Players.list;
      HashSet<Prefab> prefabSet = new HashSet<Prefab>();
      List<PrefabInstance> _list = new List<PrefabInstance>();
      foreach (EntityPlayer entityPlayer in list)
      {
        foreach (long _key in entityPlayer.ChunkObserver.chunksAround.list)
        {
          IChunk chunkSync = (IChunk) chunkCache.GetChunkSync(_key);
          if (chunkSync != null)
          {
            Vector3i worldPos = chunkSync.GetWorldPos();
            this.GetPrefabsAtXZ(worldPos.x, worldPos.x + 15, worldPos.z, worldPos.z + 15, _list);
            foreach (PrefabInstance prefabInstance in _list)
              prefabSet.Add(prefabInstance.prefab);
          }
        }
      }
      activePrefabCount = prefabSet.Count;
      activePrefabBytes = 0;
      foreach (Prefab prefab in prefabSet)
        activePrefabBytes += prefab.EstimateOwnedBytes();
    }
  }

  public class TraderComparer : IComparer<TraderArea>
  {
    public int Compare(TraderArea _ta1, TraderArea _ta2)
    {
      return _ta1.ProtectPosition.x - _ta2.ProtectPosition.x;
    }
  }
}
