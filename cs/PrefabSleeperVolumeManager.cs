// Decompiled with JetBrains decompiler
// Type: PrefabSleeperVolumeManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class PrefabSleeperVolumeManager
{
  [PublicizedFrom(EAccessModifier.Private)]
  public static PrefabSleeperVolumeManager instance;
  [PublicizedFrom(EAccessModifier.Private)]
  public readonly List<PrefabInstance> clientPrefabs = new List<PrefabInstance>();

  public static PrefabSleeperVolumeManager Instance
  {
    get
    {
      if (PrefabSleeperVolumeManager.instance == null)
        PrefabSleeperVolumeManager.instance = new PrefabSleeperVolumeManager();
      return PrefabSleeperVolumeManager.instance;
    }
  }

  public void Cleanup()
  {
    this.clientPrefabs.Clear();
    DynamicPrefabDecorator dynamicPrefabDecorator = GameManager.Instance.GetDynamicPrefabDecorator();
    if (dynamicPrefabDecorator != null)
    {
      dynamicPrefabDecorator.OnPrefabLoaded -= new Action<PrefabInstance>(this.PrefabLoadedServer);
      dynamicPrefabDecorator.OnPrefabChanged -= new Action<PrefabInstance>(this.PrefabChangedServer);
      dynamicPrefabDecorator.OnPrefabRemoved -= new Action<PrefabInstance>(this.PrefabRemovedServer);
    }
    PrefabEditModeManager.Instance.OnPrefabChanged -= new Action<PrefabInstance>(this.PrefabChangedServer);
  }

  public void StartAsServer()
  {
    DynamicPrefabDecorator dynamicPrefabDecorator = GameManager.Instance.GetDynamicPrefabDecorator();
    dynamicPrefabDecorator.OnPrefabLoaded += new Action<PrefabInstance>(this.PrefabLoadedServer);
    dynamicPrefabDecorator.OnPrefabChanged += new Action<PrefabInstance>(this.PrefabChangedServer);
    dynamicPrefabDecorator.OnPrefabRemoved += new Action<PrefabInstance>(this.PrefabRemovedServer);
    PrefabEditModeManager.Instance.OnPrefabChanged += new Action<PrefabInstance>(this.PrefabChangedServer);
    GameManager.Instance.OnClientSpawned += new Action<ClientInfo>(this.SendAllPrefabs);
  }

  public void StartAsClient() => this.clientPrefabs.Clear();

  [PublicizedFrom(EAccessModifier.Private)]
  public void SendAllPrefabs(ClientInfo _toClient)
  {
    if (_toClient == null)
      return;
    foreach (PrefabInstance dynamicPrefab in GameManager.Instance.GetDynamicPrefabDecorator().GetDynamicPrefabs())
    {
      _toClient.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageEditorPrefabInstance>().Setup(NetPackageEditorPrefabInstance.EChangeType.Added, dynamicPrefab));
      for (int index = 0; index < dynamicPrefab.prefab.SleeperVolumes.Count; ++index)
        _toClient.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageEditorSleeperVolume>().Setup(NetPackageEditorSleeperVolume.EChangeType.Added, dynamicPrefab.id, index, dynamicPrefab.prefab.SleeperVolumes[index]));
      for (int index = 0; index < dynamicPrefab.prefab.TeleportVolumes.Count; ++index)
        _toClient.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageEditorTeleportVolume>().Setup(NetPackageEditorSleeperVolume.EChangeType.Added, dynamicPrefab.id, index, dynamicPrefab.prefab.TeleportVolumes[index]));
      for (int index = 0; index < dynamicPrefab.prefab.InfoVolumes.Count; ++index)
        _toClient.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageEditorInfoVolume>().Setup(NetPackageEditorSleeperVolume.EChangeType.Added, dynamicPrefab.id, index, dynamicPrefab.prefab.InfoVolumes[index]));
      for (int index = 0; index < dynamicPrefab.prefab.WallVolumes.Count; ++index)
        _toClient.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageEditorWallVolume>().Setup(NetPackageEditorSleeperVolume.EChangeType.Added, dynamicPrefab.id, index, dynamicPrefab.prefab.WallVolumes[index]));
      for (int index = 0; index < dynamicPrefab.prefab.TriggerVolumes.Count; ++index)
        _toClient.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageEditorTriggerVolume>().Setup(NetPackageEditorSleeperVolume.EChangeType.Added, dynamicPrefab.id, index, dynamicPrefab.prefab.TriggerVolumes[index]));
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void PrefabLoadedServer(PrefabInstance _prefabInstance)
  {
    SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageEditorPrefabInstance>().Setup(NetPackageEditorPrefabInstance.EChangeType.Added, _prefabInstance));
  }

  public void PrefabLoadedClient(
    int _prefabInstanceId,
    Vector3i _boundingBoxPosition,
    Vector3i _boundingBoxSize,
    string _prefabInstanceName,
    Vector3i _prefabSize,
    string _prefabFilename,
    int _prefabLocalRotation,
    int _yOffset)
  {
    PathAbstractions.AbstractedLocation location = PathAbstractions.PrefabsSearchPaths.GetLocation(_prefabFilename);
    PrefabInstance prefabInstance = new PrefabInstance(_prefabInstanceId, location, _boundingBoxPosition, (byte) 0, (Prefab) null, 0)
    {
      boundingBoxSize = _boundingBoxSize,
      name = _prefabInstanceName,
      prefab = new Prefab()
      {
        size = _prefabSize,
        location = location,
        yOffset = _yOffset
      }
    };
    prefabInstance.prefab.SetLocalRotation(_prefabLocalRotation);
    prefabInstance.CreateBoundingBox(false);
    this.clientPrefabs.Add(prefabInstance);
    if (this.clientPrefabs.Count != 1)
      return;
    PrefabEditModeManager.Instance.SetGroundLevel(_yOffset);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void PrefabChangedServer(PrefabInstance _prefabInstance)
  {
    SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageEditorPrefabInstance>().Setup(NetPackageEditorPrefabInstance.EChangeType.Changed, _prefabInstance));
  }

  public void PrefabChangedClient(
    int _prefabInstanceId,
    Vector3i _boundingBoxPosition,
    Vector3i _boundingBoxSize,
    string _prefabInstanceName,
    Vector3i _prefabSize,
    string _prefabFilename,
    int _prefabLocalRotation,
    int _yOffset)
  {
    PrefabInstance prefabInstance = this.GetPrefabInstance(_prefabInstanceId);
    if (prefabInstance == null)
    {
      Log.Error("Prefab not found: " + _prefabInstanceId.ToString());
    }
    else
    {
      PathAbstractions.AbstractedLocation location = PathAbstractions.PrefabsSearchPaths.GetLocation(_prefabFilename);
      prefabInstance.boundingBoxPosition = _boundingBoxPosition;
      prefabInstance.boundingBoxSize = _boundingBoxSize;
      prefabInstance.name = _prefabInstanceName;
      prefabInstance.prefab.size = _prefabSize;
      prefabInstance.prefab.location = location;
      prefabInstance.prefab.SetLocalRotation(_prefabLocalRotation);
      prefabInstance.prefab.yOffset = _yOffset;
      prefabInstance.CreateBoundingBox(false);
      if (this.clientPrefabs.IndexOf(prefabInstance) != 0)
        return;
      PrefabEditModeManager.Instance.SetGroundLevel(_yOffset);
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void PrefabRemovedServer(PrefabInstance _prefabInstance)
  {
    SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageEditorPrefabInstance>().Setup(NetPackageEditorPrefabInstance.EChangeType.Removed, _prefabInstance));
  }

  public void PrefabRemovedClient(int _prefabInstanceId)
  {
    for (int index1 = 0; index1 < this.clientPrefabs.Count; ++index1)
    {
      PrefabInstance clientPrefab = this.clientPrefabs[index1];
      if (clientPrefab.id == _prefabInstanceId)
      {
        this.clientPrefabs.RemoveAt(index1);
        for (int index2 = 0; index2 < clientPrefab.prefab.SleeperVolumes.Count; ++index2)
        {
          Prefab.PrefabSleeperVolume sleeperVolume = clientPrefab.prefab.SleeperVolumes[index2];
          sleeperVolume.used = false;
          clientPrefab.prefab.SetSleeperVolume(clientPrefab.name, clientPrefab.boundingBoxPosition, index2, sleeperVolume);
        }
        for (int index3 = 0; index3 < clientPrefab.prefab.TeleportVolumes.Count; ++index3)
        {
          Prefab.PrefabTeleportVolume teleportVolume = clientPrefab.prefab.TeleportVolumes[index3];
          clientPrefab.prefab.SetTeleportVolume(clientPrefab.name, clientPrefab.boundingBoxPosition, index3, teleportVolume);
        }
        for (int index4 = 0; index4 < clientPrefab.prefab.InfoVolumes.Count; ++index4)
        {
          Prefab.PrefabInfoVolume infoVolume = clientPrefab.prefab.InfoVolumes[index4];
          clientPrefab.prefab.SetInfoVolume(clientPrefab.name, clientPrefab.boundingBoxPosition, index4, infoVolume);
        }
        for (int index5 = 0; index5 < clientPrefab.prefab.WallVolumes.Count; ++index5)
        {
          Prefab.PrefabWallVolume wallVolume = clientPrefab.prefab.WallVolumes[index5];
          clientPrefab.prefab.SetWallVolume(clientPrefab.name, clientPrefab.boundingBoxPosition, index5, wallVolume);
        }
        for (int index6 = 0; index6 < clientPrefab.prefab.TriggerVolumes.Count; ++index6)
        {
          Prefab.PrefabTriggerVolume triggerVolume = clientPrefab.prefab.TriggerVolumes[index6];
          clientPrefab.prefab.SetTriggerVolume(clientPrefab.name, clientPrefab.boundingBoxPosition, index6, triggerVolume);
        }
        break;
      }
    }
  }

  public void AddSleeperVolumeServer(Vector3i _startPos, Vector3i _size)
  {
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
    {
      DynamicPrefabDecorator dynamicPrefabDecorator = GameManager.Instance.World.ChunkClusters[0].ChunkProvider.GetDynamicPrefabDecorator();
      if (dynamicPrefabDecorator == null)
        return;
      PrefabInstance prefabForBlockPos = GameUtils.FindPrefabForBlockPos(dynamicPrefabDecorator.GetDynamicPrefabs(), _startPos + new Vector3i(_size.x / 2, 0, _size.z / 2));
      if (prefabForBlockPos == null)
        return;
      int num = prefabForBlockPos.prefab.AddSleeperVolume(prefabForBlockPos.name, prefabForBlockPos.boundingBoxPosition, _startPos - prefabForBlockPos.boundingBoxPosition, _size, (short) 0, "GroupGenericZombie", 5, 6);
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageEditorSleeperVolume>().Setup(NetPackageEditorSleeperVolume.EChangeType.Added, prefabForBlockPos.id, num, prefabForBlockPos.prefab.SleeperVolumes[num]));
    }
    else
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageEditorAddSleeperVolume>().Setup(_startPos, _size));
  }

  public void UpdateSleeperPropertiesServer(
    int _prefabInstanceId,
    int _volumeId,
    Prefab.PrefabSleeperVolume _volumeSettings)
  {
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
    {
      this.AddUpdateSleeperPropertiesClient(_prefabInstanceId, _volumeId, _volumeSettings);
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage((NetPackage) NetPackageManager.GetPackage<NetPackageEditorSleeperVolume>().Setup(NetPackageEditorSleeperVolume.EChangeType.Changed, _prefabInstanceId, _volumeId, _volumeSettings));
    }
    else
      SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer((NetPackage) NetPackageManager.GetPackage<NetPackageEditorSleeperVolume>().Setup(NetPackageEditorSleeperVolume.EChangeType.Changed, _prefabInstanceId, _volumeId, _volumeSettings));
  }

  public void AddUpdateSleeperPropertiesClient(
    int _prefabInstanceId,
    int _volumeId,
    Prefab.PrefabSleeperVolume _volumeSettings)
  {
    PrefabInstance prefabInstance = this.GetPrefabInstance(_prefabInstanceId);
    if (prefabInstance == null)
    {
      Log.Error("Prefab not found: " + _prefabInstanceId.ToString());
    }
    else
    {
      prefabInstance.prefab.SetSleeperVolume(prefabInstance.name, prefabInstance.boundingBoxPosition, _volumeId, _volumeSettings);
      XUiC_WoPropsSleeperVolume.SleeperVolumeChanged(_prefabInstanceId, _volumeId);
    }
  }

  public PrefabInstance GetPrefabInstance(int _prefabId)
  {
    if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
      return GameManager.Instance.GetDynamicPrefabDecorator() == null ? (PrefabInstance) null : GameManager.Instance.GetDynamicPrefabDecorator().GetPrefab(_prefabId);
    foreach (PrefabInstance clientPrefab in this.clientPrefabs)
    {
      if (clientPrefab.id == _prefabId)
        return clientPrefab;
    }
    return (PrefabInstance) null;
  }
}
