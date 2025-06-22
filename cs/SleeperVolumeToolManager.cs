// Decompiled with JetBrains decompiler
// Type: SleeperVolumeToolManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SleeperVolumeToolManager
{
  [PublicizedFrom(EAccessModifier.Private)]
  public static GameObject GroupGameObject = (GameObject) null;
  [PublicizedFrom(EAccessModifier.Private)]
  public static Dictionary<Vector3i, List<SleeperVolumeToolManager.BlockData>> sleepers;
  [PublicizedFrom(EAccessModifier.Private)]
  public static List<SelectionBox> registeredSleeperVolumes = new List<SelectionBox>();
  [PublicizedFrom(EAccessModifier.Private)]
  public static bool xRayOn = true;
  [PublicizedFrom(EAccessModifier.Private)]
  public const int cActiveIndex = 0;
  [PublicizedFrom(EAccessModifier.Private)]
  public const int cPriorityIndex = 1;
  [PublicizedFrom(EAccessModifier.Private)]
  public const int cNoVolumeIndex = 2;
  [PublicizedFrom(EAccessModifier.Private)]
  public const int cDarkIndex = 3;
  [PublicizedFrom(EAccessModifier.Private)]
  public const int cBanditIndex = 4;
  [PublicizedFrom(EAccessModifier.Private)]
  public const int cInfestedIndex = 5;
  [PublicizedFrom(EAccessModifier.Private)]
  public static readonly Color[] typeColors = new Color[6]
  {
    new Color(1f, 0.6f, 0.1f),
    new Color(0.7f, 0.7f, 0.7f),
    new Color(1f, 0.1f, 1f),
    new Color(0.02f, 0.02f, 0.02f),
    new Color(0.1f, 1f, 0.1f),
    new Color(1f, 0.1f, 0.1f)
  };
  [PublicizedFrom(EAccessModifier.Private)]
  public static readonly List<Material> typeMats = new List<Material>();
  [PublicizedFrom(EAccessModifier.Private)]
  public static Color groupSelectedColor = new Color(0.9f, 0.9f, 1f, 0.4f);
  [PublicizedFrom(EAccessModifier.Private)]
  public static Color[] groupColors = new Color[8]
  {
    new Color(1f, 0.2f, 0.2f, 0.4f),
    new Color(1f, 0.6f, 0.2f, 0.4f),
    new Color(1f, 1f, 0.2f, 0.4f),
    new Color(0.6f, 1f, 0.2f, 0.4f),
    new Color(0.2f, 1f, 0.2f, 0.4f),
    new Color(0.2f, 1f, 0.6f, 0.4f),
    new Color(0.2f, 1f, 1f, 0.4f),
    new Color(0.2f, 0.6f, 1f, 0.4f)
  };
  [PublicizedFrom(EAccessModifier.Private)]
  public static SelectionBox currentSelectionBox;
  [PublicizedFrom(EAccessModifier.Private)]
  public static SelectionBox previousSelectionBox;

  public static void RegisterSleeperBlock(BlockValue _bv, Transform prefabTrans, Vector3i position)
  {
    if (!(_bv.Block is BlockSleeper block))
    {
      Log.Warning("SleeperVolumeToolManager RegisterSleeperBlock not sleeper {0}", new object[1]
      {
        (object) _bv
      });
    }
    else
    {
      if (SleeperVolumeToolManager.sleepers == null)
      {
        SleeperVolumeToolManager.sleepers = new Dictionary<Vector3i, List<SleeperVolumeToolManager.BlockData>>();
        Shader shader = Shader.Find("Game/UI/Sleeper");
        for (int index = 0; index < SleeperVolumeToolManager.typeColors.Length; ++index)
          SleeperVolumeToolManager.typeMats.Add(new Material(shader)
          {
            renderQueue = 4001,
            color = SleeperVolumeToolManager.typeColors[index]
          });
      }
      Vector3i worldPos = GameManager.Instance.World.ChunkClusters[0].GetChunkFromWorldPos(position).GetWorldPos();
      SleeperVolumeToolManager.BlockData data = new SleeperVolumeToolManager.BlockData();
      data.block = block;
      data.prefabT = prefabTrans;
      data.position = position;
      prefabTrans.position = Vector3.op_Addition(Vector3.op_Addition(Vector3.op_Addition(prefabTrans.position, position.ToVector3()), Vector3.op_Multiply(Vector3.one, 0.5f)), Vector3.op_Multiply(Vector3.up, 0.01f));
      if (Object.op_Equality((Object) SleeperVolumeToolManager.GroupGameObject, (Object) null))
      {
        SleeperVolumeToolManager.GroupGameObject = new GameObject();
        ((Object) SleeperVolumeToolManager.GroupGameObject).name = "SleeperVolumeToolManagerPrefabs";
      }
      prefabTrans.parent = SleeperVolumeToolManager.GroupGameObject.transform;
      List<SleeperVolumeToolManager.BlockData> blockDataList;
      if (!SleeperVolumeToolManager.sleepers.TryGetValue(worldPos, out blockDataList))
      {
        blockDataList = new List<SleeperVolumeToolManager.BlockData>();
        SleeperVolumeToolManager.sleepers.Add(worldPos, blockDataList);
      }
      blockDataList.Add(data);
      SleeperVolumeToolManager.UpdateSleeperVisuals(data);
    }
  }

  public static void UnRegisterSleeperBlock(Vector3i position)
  {
    if (SleeperVolumeToolManager.sleepers == null)
      return;
    Vector3i worldPos = GameManager.Instance.World.ChunkClusters[0].GetChunkFromWorldPos(position).GetWorldPos();
    List<SleeperVolumeToolManager.BlockData> blockDataList;
    if (!SleeperVolumeToolManager.sleepers.TryGetValue(worldPos, out blockDataList))
      return;
    for (int index = 0; index < blockDataList.Count; ++index)
    {
      if (blockDataList[index].position == position)
      {
        Object.Destroy((Object) ((Component) blockDataList[index].prefabT).gameObject);
        blockDataList.RemoveAt(index);
        break;
      }
    }
  }

  public static void CleanUp()
  {
    if (SleeperVolumeToolManager.sleepers != null)
    {
      foreach (KeyValuePair<Vector3i, List<SleeperVolumeToolManager.BlockData>> sleeper in SleeperVolumeToolManager.sleepers)
      {
        for (int index = 0; index < sleeper.Value.Count; ++index)
        {
          Transform prefabT = sleeper.Value[index].prefabT;
          if (Object.op_Implicit((Object) prefabT))
            Object.Destroy((Object) ((Component) prefabT).gameObject);
        }
      }
      SleeperVolumeToolManager.sleepers.Clear();
    }
    SleeperVolumeToolManager.ClearSleeperVolumes();
  }

  public static void RegisterSleeperVolume(SelectionBox _selBox)
  {
    if (SleeperVolumeToolManager.registeredSleeperVolumes.Contains(_selBox))
      return;
    SleeperVolumeToolManager.registeredSleeperVolumes.Add(_selBox);
  }

  public static void UnRegisterSleeperVolume(SelectionBox _selBox)
  {
    if (!SleeperVolumeToolManager.registeredSleeperVolumes.Contains(_selBox))
      return;
    SleeperVolumeToolManager.registeredSleeperVolumes.Remove(_selBox);
  }

  public static void ClearSleeperVolumes()
  {
    SleeperVolumeToolManager.registeredSleeperVolumes.Clear();
  }

  public static void CheckKeys()
  {
    if (!Input.GetKeyDown((KeyCode) 93) || !Object.op_Implicit((Object) SleeperVolumeToolManager.currentSelectionBox))
      return;
    Prefab.PrefabSleeperVolume userData1 = (Prefab.PrefabSleeperVolume) SleeperVolumeToolManager.currentSelectionBox.UserData;
    if (userData1 == null)
      return;
    short num = -1;
    if (InputUtils.ShiftKeyPressed)
      num = (short) 0;
    else if (Object.op_Implicit((Object) SleeperVolumeToolManager.previousSelectionBox))
    {
      Prefab.PrefabSleeperVolume userData2 = (Prefab.PrefabSleeperVolume) SleeperVolumeToolManager.previousSelectionBox.UserData;
      if (userData2 != null)
      {
        num = userData2.groupId;
        if (num == (short) 0)
        {
          PrefabInstance selectedPrefabInstance = XUiC_WoPropsSleeperVolume.selectedPrefabInstance;
          if (selectedPrefabInstance != null)
          {
            num = selectedPrefabInstance.prefab.FindSleeperVolumeFreeGroupId();
            userData2.groupId = num;
            Log.Out("Set sleeper volume {0} to new group ID {1}", new object[2]
            {
              (object) userData2.startPos,
              (object) num
            });
          }
        }
      }
    }
    if (num < (short) 0)
      return;
    userData1.groupId = num;
    SleeperVolumeToolManager.SelectionChanged(SleeperVolumeToolManager.currentSelectionBox);
    Log.Out("Set sleeper volume {0} to group ID {1}", new object[2]
    {
      (object) userData1.startPos,
      (object) num
    });
  }

  public static void SetVisible(bool _visible)
  {
    if (_visible)
      SleeperVolumeToolManager.SelectionChanged((SelectionBox) null);
    else
      SleeperVolumeToolManager.ShowSleepers(false);
  }

  public static void SelectionChanged(SelectionBox selBox)
  {
    if (Object.op_Implicit((Object) selBox) && Object.op_Inequality((Object) selBox, (Object) SleeperVolumeToolManager.currentSelectionBox))
      SleeperVolumeToolManager.previousSelectionBox = SleeperVolumeToolManager.currentSelectionBox;
    SleeperVolumeToolManager.currentSelectionBox = selBox;
    SleeperVolumeToolManager.UpdateSleeperVisuals();
    SleeperVolumeToolManager.UpdateVolumeColors();
  }

  public static void UpdateVolumeColors()
  {
    int num = 0;
    if (Object.op_Implicit((Object) SleeperVolumeToolManager.currentSelectionBox))
      num = (int) ((Prefab.PrefabSleeperVolume) SleeperVolumeToolManager.currentSelectionBox.UserData).groupId;
    for (int index = 0; index < SleeperVolumeToolManager.registeredSleeperVolumes.Count; ++index)
    {
      SelectionBox registeredSleeperVolume = SleeperVolumeToolManager.registeredSleeperVolumes[index];
      Prefab.PrefabSleeperVolume userData = (Prefab.PrefabSleeperVolume) registeredSleeperVolume.UserData;
      if (userData.groupId != (short) 0)
      {
        if ((int) userData.groupId == num)
          registeredSleeperVolume.SetAllFacesColor(SleeperVolumeToolManager.groupSelectedColor);
        else
          registeredSleeperVolume.SetAllFacesColor(SleeperVolumeToolManager.groupColors[(int) userData.groupId % SleeperVolumeToolManager.groupColors.Length]);
      }
      else
        registeredSleeperVolume.SetAllFacesColor(SelectionBoxManager.ColSleeperVolumeInactive);
    }
    if (!Object.op_Implicit((Object) SleeperVolumeToolManager.currentSelectionBox))
      return;
    SleeperVolumeToolManager.currentSelectionBox.SetAllFacesColor(SelectionBoxManager.ColSleeperVolume);
  }

  public static void ShowSleepers(bool bShow = true)
  {
    if (SleeperVolumeToolManager.sleepers == null)
      return;
    foreach (KeyValuePair<Vector3i, List<SleeperVolumeToolManager.BlockData>> sleeper in SleeperVolumeToolManager.sleepers)
    {
      for (int index = 0; index < sleeper.Value.Count; ++index)
      {
        Transform prefabT = sleeper.Value[index].prefabT;
        if (Object.op_Implicit((Object) prefabT))
          ((Component) prefabT).gameObject.SetActive(bShow);
      }
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static void UpdateSleeperVisuals()
  {
    if (SleeperVolumeToolManager.sleepers == null)
      return;
    foreach (KeyValuePair<Vector3i, List<SleeperVolumeToolManager.BlockData>> sleeper in SleeperVolumeToolManager.sleepers)
    {
      List<SleeperVolumeToolManager.BlockData> blockDataList = sleeper.Value;
      for (int index = 0; index < blockDataList.Count; ++index)
        SleeperVolumeToolManager.UpdateSleeperVisuals(blockDataList[index]);
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static void UpdateSleeperVisuals(SleeperVolumeToolManager.BlockData data)
  {
    Transform prefabT = data.prefabT;
    if (!SelectionBoxManager.Instance.GetCategory("SleeperVolume").IsVisible() || Object.op_Equality((Object) SleeperVolumeToolManager.currentSelectionBox, (Object) null) && !SleeperVolumeToolManager.xRayOn)
    {
      ((Component) prefabT).gameObject.SetActive(false);
    }
    else
    {
      Vector3i vector3i1 = Vector3i.min;
      Vector3i vector3i2 = Vector3i.min;
      SelectionBox currentSelectionBox = SleeperVolumeToolManager.currentSelectionBox;
      if (Object.op_Inequality((Object) currentSelectionBox, (Object) null))
      {
        vector3i1 = Vector3i.FromVector3Rounded(((Bounds) ref currentSelectionBox.bounds).min);
        vector3i2 = Vector3i.FromVector3Rounded(((Bounds) ref currentSelectionBox.bounds).max);
      }
      Vector3i position = data.position;
      if (position.x >= vector3i1.x && position.x < vector3i2.x && position.y >= vector3i1.y && position.y < vector3i2.y && position.z >= vector3i1.z && position.z < vector3i2.z)
      {
        int index = 0;
        PrefabInstance selectedPrefabInstance = XUiC_WoPropsSleeperVolume.selectedPrefabInstance;
        Vector3i _pos = position - selectedPrefabInstance.boundingBoxPosition;
        Prefab.PrefabSleeperVolume sleeperVolume = selectedPrefabInstance.prefab.FindSleeperVolume(_pos);
        if (sleeperVolume != null && sleeperVolume.isPriority)
          index = 1;
        if (data.block.spawnMode == BlockSleeper.eMode.Bandit)
          index = 4;
        if (data.block.spawnMode == BlockSleeper.eMode.Infested)
          index = 5;
        ((Component) prefabT).gameObject.SetActive(true);
        SleeperVolumeToolManager.SetMats(prefabT, SleeperVolumeToolManager.typeMats[index]);
      }
      else if (!SleeperVolumeToolManager.InAnyVolume(position))
      {
        ((Component) prefabT).gameObject.SetActive(true);
        SleeperVolumeToolManager.SetMats(prefabT, SleeperVolumeToolManager.typeMats[2]);
      }
      else if (SleeperVolumeToolManager.xRayOn && Object.op_Equality((Object) SleeperVolumeToolManager.currentSelectionBox, (Object) null))
      {
        ((Component) prefabT).gameObject.SetActive(true);
        SleeperVolumeToolManager.SetMats(prefabT, SleeperVolumeToolManager.typeMats[3]);
      }
      else
        ((Component) prefabT).gameObject.SetActive(false);
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static bool InAnyVolume(Vector3i pos)
  {
    Vector3i zero1 = Vector3i.zero;
    Vector3i zero2 = Vector3i.zero;
    for (int index = 0; index < SleeperVolumeToolManager.registeredSleeperVolumes.Count; ++index)
    {
      SelectionBox registeredSleeperVolume = SleeperVolumeToolManager.registeredSleeperVolumes[index];
      zero1.RoundToInt(((Bounds) ref registeredSleeperVolume.bounds).min);
      zero2.RoundToInt(((Bounds) ref registeredSleeperVolume.bounds).max);
      if (pos.x >= zero1.x && pos.x < zero2.x && pos.y >= zero1.y && pos.y < zero2.y && pos.z >= zero1.z && pos.z < zero2.z)
        return true;
    }
    return false;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static void SetMats(Transform t, Material _mat)
  {
    int num = SleeperVolumeToolManager.xRayOn ? -200000000 : -200000;
    _mat.SetInt("_Offset", num);
    foreach (Renderer componentsInChild in ((Component) t).GetComponentsInChildren<MeshRenderer>())
      componentsInChild.sharedMaterial = _mat;
  }

  public static bool GetXRay() => SleeperVolumeToolManager.xRayOn;

  public static void SetXRay(bool _on)
  {
    if (SleeperVolumeToolManager.xRayOn == _on)
      return;
    SleeperVolumeToolManager.xRayOn = _on;
    int num = SleeperVolumeToolManager.xRayOn ? -200000000 : -200000;
    for (int index = 0; index < SleeperVolumeToolManager.typeMats.Count; ++index)
      SleeperVolumeToolManager.typeMats[index].SetInt("_Offset", num);
    SleeperVolumeToolManager.UpdateSleeperVisuals();
  }

  [PublicizedFrom(EAccessModifier.Private)]
  static SleeperVolumeToolManager()
  {
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public class BlockData
  {
    public BlockSleeper block;
    public Transform prefabT;
    public Vector3i position;
  }
}
