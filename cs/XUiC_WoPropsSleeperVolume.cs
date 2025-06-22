// Decompiled with JetBrains decompiler
// Type: XUiC_WoPropsSleeperVolume
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
[Preserve]
public class XUiC_WoPropsSleeperVolume : XUiController, ISelectionBoxCallback
{
  public static string ID = "";
  [PublicizedFrom(EAccessModifier.Private)]
  public XUiView triggersBox;
  [PublicizedFrom(EAccessModifier.Private)]
  public XUiV_Label labelIndex;
  [PublicizedFrom(EAccessModifier.Private)]
  public XUiV_Label labelPosition;
  [PublicizedFrom(EAccessModifier.Private)]
  public XUiV_Label labelSize;
  [PublicizedFrom(EAccessModifier.Private)]
  public XUiV_Label labelSleeperCount;
  [PublicizedFrom(EAccessModifier.Private)]
  public XUiV_Label labelGroup;
  [PublicizedFrom(EAccessModifier.Private)]
  public XUiC_TextInput txtGroupId;
  [PublicizedFrom(EAccessModifier.Private)]
  public XUiC_ComboBoxBool cbxPriority;
  [PublicizedFrom(EAccessModifier.Private)]
  public XUiC_ComboBoxBool cbxQuestExclude;
  [PublicizedFrom(EAccessModifier.Private)]
  public XUiC_ComboBoxList<XUiC_WoPropsSleeperVolume.CountPreset> cbxCountPreset;
  [PublicizedFrom(EAccessModifier.Private)]
  public XUiC_TextInput txtSpawnMin;
  [PublicizedFrom(EAccessModifier.Private)]
  public XUiC_TextInput txtSpawnMax;
  [PublicizedFrom(EAccessModifier.Private)]
  public XUiC_ComboBoxEnum<SleeperVolume.ETriggerType> cbxTrigger;
  [PublicizedFrom(EAccessModifier.Private)]
  public XUiC_TextInput txtMinScript;
  [PublicizedFrom(EAccessModifier.Private)]
  public XUiC_SpawnersList spawnersList;
  [PublicizedFrom(EAccessModifier.Private)]
  public PrefabInstance m_selectedPrefabInstance;
  [PublicizedFrom(EAccessModifier.Private)]
  public int selIdx;
  [PublicizedFrom(EAccessModifier.Private)]
  public bool bSleeperVolumeChanged;
  [PublicizedFrom(EAccessModifier.Private)]
  public XUiC_PrefabTriggerEditorList triggeredByList;
  [PublicizedFrom(EAccessModifier.Private)]
  public bool showTriggeredBy;
  [PublicizedFrom(EAccessModifier.Private)]
  public static XUiC_WoPropsSleeperVolume instance;

  public static int selectedVolumeIndex
  {
    get
    {
      return XUiC_WoPropsSleeperVolume.instance != null && XUiC_WoPropsSleeperVolume.instance.m_selectedPrefabInstance != null ? XUiC_WoPropsSleeperVolume.instance.selIdx : -1;
    }
  }

  public static PrefabInstance selectedPrefabInstance
  {
    get
    {
      return XUiC_WoPropsSleeperVolume.instance != null ? XUiC_WoPropsSleeperVolume.instance.m_selectedPrefabInstance : (PrefabInstance) null;
    }
  }

  public List<byte> TriggeredByIndices
  {
    get
    {
      return this.m_selectedPrefabInstance != null ? this.m_selectedPrefabInstance.prefab.SleeperVolumes[this.selIdx].triggeredByIndices : (List<byte>) null;
    }
  }

  public override void Init()
  {
    base.Init();
    XUiC_WoPropsSleeperVolume.ID = this.WindowGroup.ID;
    XUiC_WoPropsSleeperVolume.instance = this;
    this.labelIndex = this.GetChildById("labelIndex").ViewComponent as XUiV_Label;
    this.labelPosition = this.GetChildById("labelPosition").ViewComponent as XUiV_Label;
    this.labelSize = this.GetChildById("labelSize").ViewComponent as XUiV_Label;
    this.labelSleeperCount = this.GetChildById("labelSleeperCount").ViewComponent as XUiV_Label;
    this.labelGroup = this.GetChildById("labelGroup").ViewComponent as XUiV_Label;
    this.txtGroupId = (XUiC_TextInput) this.GetChildById("groupId");
    this.txtGroupId.OnChangeHandler += new XUiEvent_InputOnChangedEventHandler(this.TxtGroupId_OnChangeHandler);
    this.cbxPriority = (XUiC_ComboBoxBool) this.GetChildById("cbxPriority");
    this.cbxPriority.OnValueChanged += new XUiC_ComboBox<bool>.XUiEvent_ValueChanged(this.CbxPriority_OnValueChanged);
    this.cbxQuestExclude = (XUiC_ComboBoxBool) this.GetChildById("cbxQuestExclude");
    this.cbxQuestExclude.OnValueChanged += new XUiC_ComboBox<bool>.XUiEvent_ValueChanged(this.CbxQuestExclude_OnValueChanged);
    this.cbxCountPreset = (XUiC_ComboBoxList<XUiC_WoPropsSleeperVolume.CountPreset>) this.GetChildById("cbxCountPreset");
    this.cbxCountPreset.OnValueChanged += new XUiC_ComboBox<XUiC_WoPropsSleeperVolume.CountPreset>.XUiEvent_ValueChanged(this.CbxCountPreset_OnValueChanged);
    this.cbxCountPreset.Elements.Add(new XUiC_WoPropsSleeperVolume.CountPreset((short) -1, (short) -1, "Custom"));
    this.cbxCountPreset.Elements.Add(new XUiC_WoPropsSleeperVolume.CountPreset((short) 1, (short) 2, "12"));
    this.cbxCountPreset.Elements.Add(new XUiC_WoPropsSleeperVolume.CountPreset((short) 2, (short) 3, "23"));
    this.cbxCountPreset.Elements.Add(new XUiC_WoPropsSleeperVolume.CountPreset((short) 3, (short) 4, "34"));
    this.cbxCountPreset.Elements.Add(new XUiC_WoPropsSleeperVolume.CountPreset((short) 4, (short) 5, "45"));
    this.cbxCountPreset.Elements.Add(new XUiC_WoPropsSleeperVolume.CountPreset((short) 5, (short) 6, "56"));
    this.cbxCountPreset.Elements.Add(new XUiC_WoPropsSleeperVolume.CountPreset((short) 6, (short) 7, "67"));
    this.cbxCountPreset.Elements.Add(new XUiC_WoPropsSleeperVolume.CountPreset((short) 7, (short) 8, "78"));
    this.cbxCountPreset.Elements.Add(new XUiC_WoPropsSleeperVolume.CountPreset((short) 8, (short) 9, "89"));
    this.cbxCountPreset.Elements.Add(new XUiC_WoPropsSleeperVolume.CountPreset((short) 9, (short) 10, "910"));
    this.cbxCountPreset.MinIndex = 1;
    this.txtSpawnMin = (XUiC_TextInput) this.GetChildById("spawnMin");
    this.txtSpawnMin.OnChangeHandler += new XUiEvent_InputOnChangedEventHandler(this.TxtSpawnMin_OnChangeHandler);
    this.txtSpawnMax = (XUiC_TextInput) this.GetChildById("spawnMax");
    this.txtSpawnMax.OnChangeHandler += new XUiEvent_InputOnChangedEventHandler(this.TxtSpawnMax_OnChangeHandler);
    this.cbxTrigger = (XUiC_ComboBoxEnum<SleeperVolume.ETriggerType>) this.GetChildById("cbxTrigger");
    this.cbxTrigger.OnValueChanged += new XUiC_ComboBox<SleeperVolume.ETriggerType>.XUiEvent_ValueChanged(this.CbxTrigger_OnValueChanged);
    this.txtMinScript = (XUiC_TextInput) this.GetChildById("script");
    this.txtMinScript.OnChangeHandler += new XUiEvent_InputOnChangedEventHandler(this.TxtMinScript_OnChangeHandler);
    this.spawnersList = (XUiC_SpawnersList) this.GetChildById("spawners");
    this.spawnersList.SelectionChanged += new XUiEvent_ListSelectionChangedEventHandler<XUiC_SpawnersList.SpawnerEntry>(this.SpawnersList_SelectionChanged);
    this.spawnersList.SelectableEntries = false;
    this.triggersBox = this.GetChildById("triggersBox").ViewComponent;
    this.triggeredByList = this.GetChildById("triggeredBy") as XUiC_PrefabTriggerEditorList;
    if (this.triggeredByList != null)
      this.triggeredByList.SelectionChanged += new XUiEvent_ListSelectionChangedEventHandler<XUiC_PrefabTriggerEditorList.PrefabTriggerEntry>(this.TriggeredByList_SelectionChanged);
    XUiController childById = this.GetChildById("addTriggeredByButton");
    if (childById != null)
      childById.OnPress += new XUiEvent_OnPressEventHandler(this.HandleAddTriggeredByEntry);
    if (!Object.op_Inequality((Object) SelectionBoxManager.Instance, (Object) null))
      return;
    SelectionBoxManager.Instance.GetCategory("SleeperVolume").SetCallback((ISelectionBoxCallback) this);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void HandleAddTriggeredByEntry(XUiController _sender, int _mouseButton)
  {
    this.TriggerOnAddTriggersPressed();
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void TxtGroupId_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
  {
    if (_changeFromCode || _text.Length <= 0 || this.m_selectedPrefabInstance == null)
      return;
    PrefabSleeperVolumeManager.Instance.UpdateSleeperPropertiesServer(this.m_selectedPrefabInstance.id, this.selIdx, new Prefab.PrefabSleeperVolume(this.m_selectedPrefabInstance.prefab.SleeperVolumes[this.selIdx])
    {
      groupId = StringParsers.ParseSInt16(_text)
    });
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void CbxPriority_OnValueChanged(XUiController _sender, bool _oldValue, bool _newValue)
  {
    if (this.m_selectedPrefabInstance == null)
      return;
    PrefabSleeperVolumeManager.Instance.UpdateSleeperPropertiesServer(this.m_selectedPrefabInstance.id, this.selIdx, new Prefab.PrefabSleeperVolume(this.m_selectedPrefabInstance.prefab.SleeperVolumes[this.selIdx])
    {
      isPriority = _newValue
    });
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void CbxQuestExclude_OnValueChanged(XUiController _sender, bool _oldValue, bool _newValue)
  {
    if (this.m_selectedPrefabInstance == null)
      return;
    PrefabSleeperVolumeManager.Instance.UpdateSleeperPropertiesServer(this.m_selectedPrefabInstance.id, this.selIdx, new Prefab.PrefabSleeperVolume(this.m_selectedPrefabInstance.prefab.SleeperVolumes[this.selIdx])
    {
      isQuestExclude = _newValue
    });
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public int FindCountPresetIndex(int _min, int _max)
  {
    for (int index = 0; index < this.cbxCountPreset.Elements.Count; ++index)
    {
      if ((int) this.cbxCountPreset.Elements[index].min == _min && (int) this.cbxCountPreset.Elements[index].max == _max)
        return index;
    }
    return -1;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void UpdateCountPresetLabel()
  {
    if (this.m_selectedPrefabInstance == null)
      return;
    Prefab.PrefabSleeperVolume sleeperVolume = this.m_selectedPrefabInstance.prefab.SleeperVolumes[this.selIdx];
    int countPresetIndex = this.FindCountPresetIndex((int) sleeperVolume.spawnCountMin, (int) sleeperVolume.spawnCountMax);
    if (countPresetIndex < 0)
    {
      this.cbxCountPreset.MinIndex = 0;
      this.cbxCountPreset.SelectedIndex = 0;
    }
    else
    {
      this.cbxCountPreset.MinIndex = 1;
      this.cbxCountPreset.SelectedIndex = countPresetIndex;
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void CbxCountPreset_OnValueChanged(
    XUiController _sender,
    XUiC_WoPropsSleeperVolume.CountPreset _oldvalue,
    XUiC_WoPropsSleeperVolume.CountPreset _newvalue)
  {
    this.cbxCountPreset.MinIndex = 1;
    if (this.m_selectedPrefabInstance != null)
      PrefabSleeperVolumeManager.Instance.UpdateSleeperPropertiesServer(this.m_selectedPrefabInstance.id, this.selIdx, new Prefab.PrefabSleeperVolume(this.m_selectedPrefabInstance.prefab.SleeperVolumes[this.selIdx])
      {
        spawnCountMin = _newvalue.min,
        spawnCountMax = _newvalue.max
      });
    this.UpdateCountPresetLabel();
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void TxtSpawnMin_OnChangeHandler(
    XUiController _sender,
    string _text,
    bool _changeFromCode)
  {
    if (_changeFromCode || _text.Length <= 0)
      return;
    short sint16 = StringParsers.ParseSInt16(_text);
    if (this.m_selectedPrefabInstance == null)
      return;
    PrefabSleeperVolumeManager.Instance.UpdateSleeperPropertiesServer(this.m_selectedPrefabInstance.id, this.selIdx, new Prefab.PrefabSleeperVolume(this.m_selectedPrefabInstance.prefab.SleeperVolumes[this.selIdx])
    {
      spawnCountMin = sint16
    });
    this.UpdateCountPresetLabel();
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void TxtSpawnMax_OnChangeHandler(
    XUiController _sender,
    string _text,
    bool _changeFromCode)
  {
    if (_changeFromCode || _text.Length <= 0)
      return;
    short sint16 = StringParsers.ParseSInt16(_text);
    if (this.m_selectedPrefabInstance == null)
      return;
    PrefabSleeperVolumeManager.Instance.UpdateSleeperPropertiesServer(this.m_selectedPrefabInstance.id, this.selIdx, new Prefab.PrefabSleeperVolume(this.m_selectedPrefabInstance.prefab.SleeperVolumes[this.selIdx])
    {
      spawnCountMax = sint16
    });
    this.UpdateCountPresetLabel();
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void CbxTrigger_OnValueChanged(
    XUiController _sender,
    SleeperVolume.ETriggerType _oldValue,
    SleeperVolume.ETriggerType _newValue)
  {
    if (this.m_selectedPrefabInstance == null)
      return;
    Prefab.PrefabSleeperVolume _volumeSettings = new Prefab.PrefabSleeperVolume(this.m_selectedPrefabInstance.prefab.SleeperVolumes[this.selIdx]);
    _volumeSettings.SetTrigger(_newValue);
    PrefabSleeperVolumeManager.Instance.UpdateSleeperPropertiesServer(this.m_selectedPrefabInstance.id, this.selIdx, _volumeSettings);
    this.triggersBox.IsVisible = true;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void TxtMinScript_OnChangeHandler(
    XUiController _sender,
    string _text,
    bool _changeFromCode)
  {
    if (_changeFromCode || this.m_selectedPrefabInstance == null)
      return;
    PrefabSleeperVolumeManager.Instance.UpdateSleeperPropertiesServer(this.m_selectedPrefabInstance.id, this.selIdx, new Prefab.PrefabSleeperVolume(this.m_selectedPrefabInstance.prefab.SleeperVolumes[this.selIdx])
    {
      minScript = MinScript.ConvertFromUIText(_text)
    });
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void SpawnersList_SelectionChanged(
    XUiC_ListEntry<XUiC_SpawnersList.SpawnerEntry> _previousEntry,
    XUiC_ListEntry<XUiC_SpawnersList.SpawnerEntry> _newEntry)
  {
    string str = (string) null;
    if (_newEntry != null)
      str = _newEntry.GetEntry().name;
    if (this.m_selectedPrefabInstance == null)
      return;
    PrefabSleeperVolumeManager.Instance.UpdateSleeperPropertiesServer(this.m_selectedPrefabInstance.id, this.selIdx, new Prefab.PrefabSleeperVolume(this.m_selectedPrefabInstance.prefab.SleeperVolumes[this.selIdx])
    {
      groupName = str
    });
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void TriggeredByList_SelectionChanged(
    XUiC_ListEntry<XUiC_PrefabTriggerEditorList.PrefabTriggerEntry> _previousEntry,
    XUiC_ListEntry<XUiC_PrefabTriggerEditorList.PrefabTriggerEntry> _newEntry)
  {
    if (_newEntry == null)
      return;
    byte _result = 0;
    if (StringParsers.TryParseUInt8(_newEntry.GetEntry().name, out _result))
    {
      Prefab.PrefabSleeperVolume prefabSleeperVolume = new Prefab.PrefabSleeperVolume(this.m_selectedPrefabInstance.prefab.SleeperVolumes[this.selIdx]);
      if (prefabSleeperVolume != null)
        this.HandleTriggersSetting(prefabSleeperVolume, _result, false, GameManager.Instance.World);
      PrefabSleeperVolumeManager.Instance.UpdateSleeperPropertiesServer(this.m_selectedPrefabInstance.id, this.selIdx, prefabSleeperVolume);
    }
    _newEntry.IsDirty = true;
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public void HandleTriggersSetting(
    Prefab.PrefabSleeperVolume psv,
    byte triggerLayer,
    bool isTriggers,
    World _world)
  {
    if (!_world.IsEditor() || isTriggers)
      return;
    if (psv.HasTriggeredBy(triggerLayer))
      psv.RemoveTriggeredByFlag(triggerLayer);
    else
      psv.SetTriggeredByFlag(triggerLayer);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void TriggerOnAddTriggersPressed()
  {
    if (this.m_selectedPrefabInstance == null)
      return;
    this.m_selectedPrefabInstance.prefab.AddNewTriggerLayer();
    this.triggeredByList.RebuildList(false);
  }

  public override void OnOpen()
  {
    base.OnOpen();
    this.UpdateCountPresetLabel();
  }

  public override void Update(float _dt)
  {
    base.Update(_dt);
    if (this.m_selectedPrefabInstance != null)
    {
      if (this.bSleeperVolumeChanged)
      {
        this.bSleeperVolumeChanged = false;
        this.m_selectedPrefabInstance.prefab.CountSleeperSpawnsInVolume(GameManager.Instance.World, this.m_selectedPrefabInstance.boundingBoxPosition, this.selIdx);
        this.UpdateCountPresetLabel();
      }
      Prefab.PrefabSleeperVolume sleeperVolume = this.m_selectedPrefabInstance.prefab.SleeperVolumes[this.selIdx];
      this.txtGroupId.Text = sleeperVolume.groupId.ToString();
      this.cbxPriority.Value = sleeperVolume.isPriority;
      this.cbxQuestExclude.Value = sleeperVolume.isQuestExclude;
      this.labelIndex.Text = this.selIdx.ToString();
      this.labelPosition.Text = sleeperVolume.startPos.ToString();
      this.labelSize.Text = sleeperVolume.size.ToString();
      this.labelSleeperCount.Text = this.m_selectedPrefabInstance.prefab.Transient_NumSleeperSpawns.ToString();
      this.labelGroup.Text = GameStageGroup.MakeDisplayName(sleeperVolume.groupName);
      this.txtSpawnMin.Text = sleeperVolume.spawnCountMin.ToString();
      this.txtSpawnMax.Text = sleeperVolume.spawnCountMax.ToString();
      this.cbxTrigger.Value = (SleeperVolume.ETriggerType) (sleeperVolume.flags & 7);
      this.txtMinScript.Text = MinScript.ConvertToUIText(sleeperVolume.minScript);
    }
    else
    {
      this.txtGroupId.Text = string.Empty;
      this.cbxPriority.Value = false;
      this.cbxQuestExclude.Value = false;
      this.labelIndex.Text = string.Empty;
      this.labelPosition.Text = string.Empty;
      this.labelSize.Text = string.Empty;
      this.labelSleeperCount.Text = string.Empty;
      this.labelGroup.Text = string.Empty;
      this.txtSpawnMin.Text = string.Empty;
      this.txtSpawnMax.Text = string.Empty;
      this.cbxTrigger.Value = SleeperVolume.ETriggerType.Active;
      this.txtMinScript.Text = string.Empty;
    }
    this.triggersBox.IsVisible = true;
  }

  public override void Cleanup()
  {
    base.Cleanup();
    if (Object.op_Inequality((Object) SelectionBoxManager.Instance, (Object) null))
      SelectionBoxManager.Instance.GetCategory("SleeperVolume").SetCallback((ISelectionBoxCallback) null);
    XUiC_WoPropsSleeperVolume.instance = (XUiC_WoPropsSleeperVolume) null;
  }

  public bool OnSelectionBoxActivated(string _category, string _name, bool _bActivated)
  {
    if (_bActivated)
    {
      int _volumeId;
      if (this.getPrefabIdAndVolumeId(_name, out int _, out _volumeId))
        this.selIdx = _volumeId;
    }
    else
      this.m_selectedPrefabInstance = (PrefabInstance) null;
    return true;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public bool getPrefabIdAndVolumeId(string _name, out int _prefabInstanceId, out int _volumeId)
  {
    _prefabInstanceId = _volumeId = 0;
    string[] strArray1 = _name.Split('.', StringSplitOptions.None);
    if (strArray1.Length > 1)
    {
      string[] strArray2 = strArray1[1].Split('_', StringSplitOptions.None);
      if (strArray2.Length > 1 && int.TryParse(strArray2[1], out _volumeId) && int.TryParse(strArray2[0], out _prefabInstanceId))
      {
        this.m_selectedPrefabInstance = PrefabSleeperVolumeManager.Instance.GetPrefabInstance(_prefabInstanceId);
        this.bSleeperVolumeChanged = true;
        Prefab prefab = this.m_selectedPrefabInstance.prefab;
        this.triggeredByList.EditPrefab = prefab;
        this.triggeredByList.SleeperOwner = this;
        this.triggeredByList.IsTriggers = false;
        if (prefab.TriggerLayers.Count == 0)
          prefab.AddInitialTriggerLayers();
        return true;
      }
    }
    return false;
  }

  public static void SleeperVolumeChanged(int _prefabInstanceId, int _volumeId)
  {
    if (XUiC_WoPropsSleeperVolume.selectedPrefabInstance == null || XUiC_WoPropsSleeperVolume.selectedPrefabInstance.id != _prefabInstanceId || XUiC_WoPropsSleeperVolume.selectedVolumeIndex != _volumeId)
      return;
    XUiC_WoPropsSleeperVolume.instance.bSleeperVolumeChanged = true;
  }

  public void OnSelectionBoxMoved(string _category, string _name, Vector3 _moveVector)
  {
    if (this.m_selectedPrefabInstance == null)
      return;
    Prefab.PrefabSleeperVolume _volumeSettings = new Prefab.PrefabSleeperVolume(this.m_selectedPrefabInstance.prefab.SleeperVolumes[this.selIdx]);
    _volumeSettings.startPos += new Vector3i(_moveVector);
    PrefabSleeperVolumeManager.Instance.UpdateSleeperPropertiesServer(this.m_selectedPrefabInstance.id, this.selIdx, _volumeSettings);
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
    if (this.m_selectedPrefabInstance == null)
      return;
    Prefab.PrefabSleeperVolume _volumeSettings = new Prefab.PrefabSleeperVolume(this.m_selectedPrefabInstance.prefab.SleeperVolumes[this.selIdx]);
    _volumeSettings.size += new Vector3i(_dEast + _dWest, _dTop + _dBottom, _dNorth + _dSouth);
    _volumeSettings.startPos += new Vector3i(-_dWest, -_dBottom, -_dSouth);
    Vector3i vector3i = _volumeSettings.size;
    if (vector3i.x < 2)
      vector3i = new Vector3i(1, vector3i.y, vector3i.z);
    if (vector3i.y < 2)
      vector3i = new Vector3i(vector3i.x, 1, vector3i.z);
    if (vector3i.z < 2)
      vector3i = new Vector3i(vector3i.x, vector3i.y, 1);
    _volumeSettings.size = vector3i;
    PrefabSleeperVolumeManager.Instance.UpdateSleeperPropertiesServer(this.m_selectedPrefabInstance.id, this.selIdx, _volumeSettings);
  }

  public void OnSelectionBoxMirrored(Vector3i _axis)
  {
  }

  public bool OnSelectionBoxDelete(string _category, string _name)
  {
    foreach (LocalPlayerUI playerUi in LocalPlayerUI.PlayerUIs)
    {
      if (playerUi.windowManager.IsModalWindowOpen())
      {
        SelectionBoxManager.Instance.SetActive(_category, _name, true);
        return false;
      }
    }
    int _volumeId;
    if (!this.getPrefabIdAndVolumeId(_name, out int _, out _volumeId))
      return false;
    PrefabSleeperVolumeManager.Instance.UpdateSleeperPropertiesServer(this.m_selectedPrefabInstance.id, _volumeId, new Prefab.PrefabSleeperVolume(this.m_selectedPrefabInstance.prefab.SleeperVolumes[_volumeId])
    {
      used = false
    });
    return true;
  }

  public bool OnSelectionBoxIsAvailable(string _category, EnumSelectionBoxAvailabilities _criteria)
  {
    return _criteria == EnumSelectionBoxAvailabilities.CanShowProperties || _criteria == EnumSelectionBoxAvailabilities.CanResize;
  }

  public void OnSelectionBoxShowProperties(bool _bVisible, GUIWindowManager _windowManager)
  {
    string _selectedCategory;
    if (!SelectionBoxManager.Instance.GetSelected(out _selectedCategory, out string _) || !_selectedCategory.Equals("SleeperVolume"))
      return;
    _windowManager.SwitchVisible(XUiC_WoPropsSleeperVolume.ID);
  }

  public void OnSelectionBoxRotated(string _category, string _name)
  {
  }

  public static bool GetSelectedVolumeStats(out XUiC_WoPropsSleeperVolume.VolumeStats _stats)
  {
    _stats = new XUiC_WoPropsSleeperVolume.VolumeStats();
    int selectedVolumeIndex = XUiC_WoPropsSleeperVolume.selectedVolumeIndex;
    if (selectedVolumeIndex < 0)
      return false;
    if (XUiC_WoPropsSleeperVolume.instance.bSleeperVolumeChanged)
    {
      XUiC_WoPropsSleeperVolume.instance.bSleeperVolumeChanged = false;
      XUiC_WoPropsSleeperVolume.selectedPrefabInstance.prefab.CountSleeperSpawnsInVolume(GameManager.Instance.World, XUiC_WoPropsSleeperVolume.selectedPrefabInstance.boundingBoxPosition, selectedVolumeIndex);
      XUiC_WoPropsSleeperVolume.instance.UpdateCountPresetLabel();
    }
    Prefab.PrefabSleeperVolume sleeperVolume = XUiC_WoPropsSleeperVolume.selectedPrefabInstance.prefab.SleeperVolumes[selectedVolumeIndex];
    _stats.index = selectedVolumeIndex;
    _stats.pos = XUiC_WoPropsSleeperVolume.selectedPrefabInstance.boundingBoxPosition + sleeperVolume.startPos;
    _stats.size = sleeperVolume.size;
    _stats.groupName = GameStageGroup.MakeDisplayName(sleeperVolume.groupName);
    _stats.isPriority = sleeperVolume.isPriority;
    _stats.isQuestExclude = sleeperVolume.isQuestExclude;
    _stats.sleeperCount = XUiC_WoPropsSleeperVolume.selectedPrefabInstance.prefab.Transient_NumSleeperSpawns;
    _stats.spawnCountMin = (int) sleeperVolume.spawnCountMin;
    _stats.spawnCountMax = (int) sleeperVolume.spawnCountMax;
    return true;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  static XUiC_WoPropsSleeperVolume()
  {
  }

  public struct VolumeStats
  {
    public int index;
    public Vector3i pos;
    public Vector3i size;
    public string groupName;
    public int sleeperCount;
    public int spawnCountMin;
    public int spawnCountMax;
    public bool isPriority;
    public bool isQuestExclude;
  }

  public struct CountPreset(short _min, short _max, string _name)
  {
    public readonly short min = _min;
    public readonly short max = _max;
    [PublicizedFrom(EAccessModifier.Private)]
    public readonly string name = _name;

    public override string ToString() => this.name;
  }
}
