// Decompiled with JetBrains decompiler
// Type: BlockShapeModelEntity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
[Preserve]
public class BlockShapeModelEntity : BlockShapeInvisible
{
  [PublicizedFrom(EAccessModifier.Private)]
  public static string PropDamagedMesh = "MeshDamage";
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cMissingPrefabEntityPath = "@:Entities/Misc/block_missingPrefab.prefab";
  public string modelName;
  public Vector3 modelOffset;
  public int censorMode;
  [PublicizedFrom(EAccessModifier.Protected)]
  public string modelNameWithPath;
  [PublicizedFrom(EAccessModifier.Private)]
  public float LODCullScale = 1f;
  [PublicizedFrom(EAccessModifier.Private)]
  public new Bounds bounds;
  [PublicizedFrom(EAccessModifier.Private)]
  public bool isCustomBounds;
  [PublicizedFrom(EAccessModifier.Private)]
  public List<BlockShapeModelEntity.DamageState> damageStates;

  public BlockShapeModelEntity()
  {
    this.IsRotatable = true;
    this.IsNotifyOnLoadUnload = true;
  }

  public override void Init(Block _block)
  {
    base.Init(_block);
    this.modelNameWithPath = _block.Properties.Values["Model"];
    if (this.modelNameWithPath == null)
      throw new Exception("No model specified on block with name " + _block.GetBlockName());
    if (this.modelNameWithPath.Length > 0)
    {
      _block.Properties.ParseInt(EntityClass.PropCensor, ref this.censorMode);
      if (this.censorMode != 0 && Object.op_Implicit((Object) GameManager.Instance) && GameManager.Instance.IsGoreCensored())
      {
        if (this.modelNameWithPath.Contains("@"))
          this.modelNameWithPath = this.modelNameWithPath.Replace(".", "_CGore.");
        else if (!this.modelNameWithPath.Contains("."))
          this.modelNameWithPath += "_CGore";
      }
    }
    this.modelName = GameIO.GetFilenameFromPathWithoutExtension(this.modelNameWithPath);
    this.modelOffset = new Vector3(0.0f, 0.5f, 0.0f);
    _block.Properties.ParseVec("ModelOffset", ref this.modelOffset);
    _block.Properties.ParseFloat("LODCullScale", ref this.LODCullScale);
    _block.Properties.ParseInt("SymType", ref this.SymmetryType);
    string str;
    if (_block.Properties.Values.TryGetValue(BlockShapeModelEntity.PropDamagedMesh, out str))
    {
      string[] strArray = str.Split(',', StringSplitOptions.None);
      if (strArray.Length >= 2)
      {
        this.damageStates = new List<BlockShapeModelEntity.DamageState>();
        for (int index = 0; index < strArray.Length - 1; index += 2)
        {
          BlockShapeModelEntity.DamageState damageState;
          damageState.objName = strArray[index].Trim();
          damageState.health = float.Parse(strArray[index + 1]);
          this.damageStates.Add(damageState);
        }
      }
    }
    GameObjectPool.Instance.AddPooledObject(this.modelName, new GameObjectPool.LoadCallback(this.PoolLoadCallback), new GameObjectPool.CreateCallback(this.PoolCreateOnceToAllCallBack), new GameObjectPool.CreateCallback(this.PoolCreateCallBack));
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public Transform PoolLoadCallback()
  {
    Transform prefab = this.getPrefab();
    return !Object.op_Equality((Object) prefab, (Object) null) ? prefab : throw new Exception($"Model '{this.modelNameWithPath}' not found on block with name {this.block.GetBlockName()}");
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void PoolCreateOnceToAllCallBack(GameObject obj)
  {
    Collider component = ((Component) obj.transform).GetComponent<Collider>();
    if (!Object.op_Inequality((Object) component, (Object) null))
      return;
    switch (component)
    {
      case BoxCollider _:
        Vector3 center1 = ((BoxCollider) component).center;
        Vector3 size = ((BoxCollider) component).size;
        this.bounds = BoundsUtils.BoundsForMinMax(center1.x - size.x / 2f, center1.y - size.y / 2f, center1.z - size.z / 2f, center1.x + size.x / 2f, center1.y + size.y / 2f, center1.z + size.z / 2f);
        this.boundsArr[0] = this.bounds;
        this.isCustomBounds = true;
        break;
      case CapsuleCollider _:
        CapsuleCollider capsuleCollider = component as CapsuleCollider;
        Vector3 center2 = capsuleCollider.center;
        Vector3 vector3;
        // ISSUE: explicit constructor call
        ((Vector3) ref vector3).\u002Ector(capsuleCollider.radius * 2f, capsuleCollider.height, capsuleCollider.radius * 2f);
        this.bounds = BoundsUtils.BoundsForMinMax(center2.x - vector3.x / 2f, center2.y - vector3.y / 2f, center2.z - vector3.z / 2f, center2.x + vector3.x / 2f, center2.y + vector3.y / 2f, center2.z + vector3.z / 2f);
        this.boundsArr[0] = this.bounds;
        this.isCustomBounds = true;
        break;
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void PoolCreateCallBack(GameObject obj)
  {
    Transform transform = obj.transform;
    LODGroup component1 = ((Component) transform).GetComponent<LODGroup>();
    if (Object.op_Implicit((Object) component1))
    {
      LODFadeMode fadeMode = component1.fadeMode;
      if (fadeMode == 2)
        return;
      if (fadeMode == null)
      {
        component1.fadeMode = (LODFadeMode) 1;
        component1.animateCrossFading = true;
      }
      if (fadeMode == 1)
        component1.animateCrossFading = true;
      LOD[] loDs = component1.GetLODs();
      int index1 = loDs.Length - 1;
      float num1 = component1.size;
      if ((double) num1 < 0.40000000596046448)
      {
        num1 *= 3.8f;
        if ((double) num1 < 1.0)
          num1 = 1f;
      }
      else if ((double) num1 < 0.64999997615814209)
        num1 *= 2.5f;
      else if ((double) num1 < 0.949999988079071)
        num1 *= 1.5f;
      else if ((double) num1 >= 1.4500000476837158)
      {
        if ((double) num1 < 2.5)
          num1 *= 0.83f;
        else if ((double) num1 < 6.1999998092651367)
          num1 *= 0.64f;
        else
          num1 *= 0.45f;
      }
      float num2 = num1 * 0.02f * this.LODCullScale;
      if ((double) num2 > 0.10000000149011612)
        num2 = 0.1f;
      loDs[index1].screenRelativeTransitionHeight = num2;
      if (index1 > 0)
      {
        float num3 = num2;
        for (int index2 = index1 - 1; index2 >= 0; --index2)
        {
          float num4 = loDs[index2].screenRelativeTransitionHeight;
          if ((double) num4 - 0.02500000037252903 <= (double) num3)
          {
            num4 = num3 + 0.025f;
            loDs[index2].screenRelativeTransitionHeight = num4;
          }
          num3 = num4;
        }
      }
      component1.SetLODs(loDs);
    }
    else
    {
      if (transform.childCount != 0)
        return;
      MeshRenderer component2 = obj.GetComponent<MeshRenderer>();
      if (!Object.op_Implicit((Object) component2))
        return;
      LOD lod;
      lod.screenRelativeTransitionHeight = 0.025f;
      Renderer[] rendererArray = new Renderer[1]
      {
        (Renderer) component2
      };
      lod.renderers = rendererArray;
      lod.fadeTransitionWidth = 0.0f;
      LODGroup lodGroup = obj.AddComponent<LODGroup>();
      lodGroup.fadeMode = (LODFadeMode) 1;
      lodGroup.animateCrossFading = true;
      LOD[] lodArray = new LOD[1]{ lod };
      lodGroup.SetLODs(lodArray);
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public Transform getPrefab()
  {
    Transform prefab = DataLoader.LoadAsset<Transform>(this.modelNameWithPath);
    if (Object.op_Equality((Object) prefab, (Object) null))
    {
      Log.Error("Model '{0}' not found on block with name {1}", new object[2]
      {
        (object) this.modelNameWithPath,
        (object) this.block.GetBlockName()
      });
      prefab = DataLoader.LoadAsset<Transform>("@:Entities/Misc/block_missingPrefab.prefab");
      if (Object.op_Equality((Object) prefab, (Object) null))
        return (Transform) null;
    }
    else
      MeshLodOptimization.Apply(ref prefab);
    string withoutExtension = GameIO.GetFilenameFromPathWithoutExtension(this.modelNameWithPath);
    if (((Object) prefab).name != withoutExtension)
      Log.Error("Model has a wrong name '{0}'. Maybe check upper/lower case mismatch on block with name {1}?", new object[2]
      {
        (object) withoutExtension,
        (object) this.block.GetBlockName()
      });
    return prefab;
  }

  public Transform CloneModel(BlockValue _blockValue, Transform _parent)
  {
    Transform _t = Object.Instantiate<Transform>(this.getPrefab());
    _t.parent = _parent;
    Block block = _blockValue.Block;
    if ((double) block.tintColor.a > 0.0)
      UpdateLight.SetTintColor(_t, block.tintColor);
    Quaternion rotation = this.GetRotation(_blockValue);
    Vector3 rotatedOffset = this.GetRotatedOffset(block, rotation);
    _t.localPosition = Vector3.op_Addition(rotatedOffset, new Vector3(0.0f, -0.5f, 0.0f));
    _t.localRotation = rotation;
    return _t;
  }

  public Vector3 GetRotatedOffset(Block block, Quaternion rot)
  {
    Vector3 vector3_1 = Quaternion.op_Multiply(rot, this.modelOffset);
    Vector3 vector3_2 = Vector3.zero;
    vector3_2.y = -0.5f;
    if (block.isMultiBlock)
    {
      if ((block.multiBlockPos.dim.x & 1) == 0)
        vector3_2.x = -0.5f;
      if ((block.multiBlockPos.dim.z & 1) == 0)
        vector3_2.z = -0.5f;
    }
    vector3_2 = Quaternion.op_Multiply(rot, vector3_2);
    Vector3 rotatedOffset = Vector3.op_Addition(vector3_1, vector3_2);
    rotatedOffset.y += 0.5f;
    return rotatedOffset;
  }

  public override Quaternion GetRotation(BlockValue _blockValue)
  {
    return BlockShapeNew.GetRotationStatic((int) _blockValue.rotation);
  }

  public override Bounds[] GetBounds(BlockValue _blockValue)
  {
    if (!this.isCustomBounds)
      return base.GetBounds(_blockValue);
    Quaternion rotation = this.GetRotation(_blockValue);
    Vector3 vector3_1 = Vector3.op_Addition(Quaternion.op_Multiply(rotation, ((Bounds) ref this.bounds).min), this.modelOffset);
    Vector3 vector3_2 = Vector3.op_Addition(Quaternion.op_Multiply(rotation, ((Bounds) ref this.bounds).max), this.modelOffset);
    ((Bounds) ref this.boundsArr[0]).min = Vector3.op_Addition(new Vector3((double) vector3_2.x > (double) vector3_1.x ? vector3_1.x : vector3_2.x, (double) vector3_2.y > (double) vector3_1.y ? vector3_1.y : vector3_2.y, (double) vector3_2.z > (double) vector3_1.z ? vector3_1.z : vector3_2.z), new Vector3(0.5f, 0.0f, 0.5f));
    ((Bounds) ref this.boundsArr[0]).max = Vector3.op_Addition(new Vector3((double) vector3_2.x < (double) vector3_1.x ? vector3_1.x : vector3_2.x, (double) vector3_2.y < (double) vector3_1.y ? vector3_1.y : vector3_2.y, (double) vector3_2.z < (double) vector3_1.z ? vector3_1.z : vector3_2.z), new Vector3(0.5f, 0.0f, 0.5f));
    return this.boundsArr;
  }

  public override BlockValue RotateY(bool _bLeft, BlockValue _blockValue, int _rotCount)
  {
    if (_bLeft)
      _rotCount = -_rotCount;
    int rotation = (int) _blockValue.rotation;
    if (rotation >= 24)
    {
      _blockValue.rotation = (byte) ((rotation - 24 + _rotCount & 3) + 24);
    }
    else
    {
      int num = 90 * _rotCount;
      _blockValue.rotation = (byte) BlockShapeNew.ConvertRotationFree(rotation, Quaternion.AngleAxis((float) num, Vector3.up));
    }
    return _blockValue;
  }

  public override byte Rotate(bool _bLeft, int _rotation)
  {
    _rotation += _bLeft ? -1 : 1;
    if (_rotation > 10)
      _rotation = 0;
    if (_rotation < 0)
      _rotation = 10;
    return (byte) _rotation;
  }

  public override BlockValue MirrorY(bool _bAlongX, BlockValue _blockValue)
  {
    if (!_bAlongX)
    {
      switch (_blockValue.rotation)
      {
        case 0:
          _blockValue.rotation = (byte) 2;
          break;
        case 1:
          _blockValue.rotation = (byte) 1;
          break;
        case 2:
          _blockValue.rotation = (byte) 0;
          break;
        case 3:
          _blockValue.rotation = (byte) 3;
          break;
        case 4:
          _blockValue.rotation = (byte) 7;
          break;
        case 5:
          _blockValue.rotation = (byte) 6;
          break;
        case 6:
          _blockValue.rotation = (byte) 5;
          break;
        case 7:
          _blockValue.rotation = (byte) 4;
          break;
        case 8:
          _blockValue.rotation = (byte) 8;
          break;
        case 9:
          _blockValue.rotation = (byte) 9;
          break;
        case 10:
          _blockValue.rotation = (byte) 10;
          break;
        case 11:
          _blockValue.rotation = (byte) 11;
          break;
      }
    }
    else
    {
      switch (_blockValue.rotation)
      {
        case 0:
          _blockValue.rotation = (byte) 0;
          break;
        case 1:
          _blockValue.rotation = (byte) 3;
          break;
        case 2:
          _blockValue.rotation = (byte) 2;
          break;
        case 3:
          _blockValue.rotation = (byte) 1;
          break;
        case 4:
          _blockValue.rotation = (byte) 7;
          break;
        case 5:
          _blockValue.rotation = (byte) 6;
          break;
        case 6:
          _blockValue.rotation = (byte) 5;
          break;
        case 7:
          _blockValue.rotation = (byte) 4;
          break;
        case 8:
          _blockValue.rotation = (byte) 8;
          break;
        case 9:
          _blockValue.rotation = (byte) 11;
          break;
        case 10:
          _blockValue.rotation = (byte) 10;
          break;
        case 11:
          _blockValue.rotation = (byte) 9;
          break;
      }
    }
    return _blockValue;
  }

  public override void OnBlockValueChanged(
    WorldBase _world,
    Vector3i _blockPos,
    int _clrIdx,
    BlockValue _oldBlockValue,
    BlockValue _newBlockValue)
  {
    base.OnBlockValueChanged(_world, _blockPos, _clrIdx, _oldBlockValue, _newBlockValue);
    ChunkCluster chunkCluster = _world.ChunkClusters[_clrIdx];
    if (chunkCluster == null)
      return;
    Chunk chunkFromWorldPos = (Chunk) chunkCluster.GetChunkFromWorldPos(_blockPos.x, _blockPos.y, _blockPos.z);
    if (chunkFromWorldPos == null)
      return;
    BlockEntityData blockEntity = chunkFromWorldPos.GetBlockEntity(_blockPos);
    if (blockEntity == null || !blockEntity.bHasTransform)
      return;
    Block block = _newBlockValue.Block;
    if ((int) _newBlockValue.rotation != (int) _oldBlockValue.rotation)
      blockEntity.transform.localRotation = block.shape.GetRotation(_newBlockValue);
    blockEntity.blockValue = _newBlockValue;
    if (this.damageStates != null)
    {
      if (this.GetDamageStateIndex(_oldBlockValue) == this.GetDamageStateIndex(_newBlockValue))
        return;
      this.UpdateDamageState(_oldBlockValue, _newBlockValue, blockEntity);
    }
    else
    {
      int num = Mathf.Min(_newBlockValue.damage, block.MaxDamage) - 1;
      blockEntity.SetMaterialValue("_Damage", (float) num);
    }
  }

  public override void OnBlockAdded(
    WorldBase world,
    Chunk _chunk,
    Vector3i _blockPos,
    BlockValue _blockValue)
  {
    base.OnBlockAdded(world, _chunk, _blockPos, _blockValue);
    _chunk.AddEntityBlockStub(new BlockEntityData(_blockValue, _blockPos)
    {
      bNeedsTemperature = true
    });
    this.registerSleepers(_blockPos, _blockValue);
  }

  public override void OnBlockRemoved(
    WorldBase _world,
    Chunk _chunk,
    Vector3i _blockPos,
    BlockValue _blockValue)
  {
    base.OnBlockRemoved(_world, _chunk, _blockPos, _blockValue);
    _chunk.RemoveEntityBlockStub(_blockPos);
    if (!GameManager.Instance.IsEditMode() || !_blockValue.Block.IsSleeperBlock)
      return;
    Prefab.TransientSleeperBlockIncrement(_blockPos, -1);
    SleeperVolumeToolManager.UnRegisterSleeperBlock(_blockPos);
  }

  public override void OnBlockLoaded(
    WorldBase _world,
    int _clrIdx,
    Vector3i _blockPos,
    BlockValue _blockValue)
  {
    base.OnBlockLoaded(_world, _clrIdx, _blockPos, _blockValue);
    ChunkCluster chunkCluster = _world.ChunkClusters[_clrIdx];
    if (chunkCluster == null)
      return;
    Chunk chunkFromWorldPos = (Chunk) chunkCluster.GetChunkFromWorldPos(_blockPos);
    if (chunkFromWorldPos == null)
      return;
    chunkFromWorldPos.AddEntityBlockStub(new BlockEntityData(_blockValue, _blockPos)
    {
      bNeedsTemperature = true
    });
    this.registerSleepers(_blockPos, _blockValue);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void registerSleepers(Vector3i _blockPos, BlockValue _blockValue)
  {
    if (!GameManager.Instance.IsEditMode() || !_blockValue.Block.IsSleeperBlock)
      return;
    Prefab.TransientSleeperBlockIncrement(_blockPos, 1);
    ThreadManager.AddSingleTaskMainThread("OnBlockAddedOrLoaded.RegisterSleeperBlock", (ThreadManager.MainThreadTaskFunctionDelegate) ([PublicizedFrom(EAccessModifier.Internal)] (_param1) => SleeperVolumeToolManager.RegisterSleeperBlock(_blockValue, this.CloneModel(_blockValue, (Transform) null), _blockPos)));
  }

  public override void OnBlockEntityTransformBeforeActivated(
    WorldBase _world,
    Vector3i _blockPos,
    BlockValue _blockValue,
    BlockEntityData _ebcd)
  {
    base.OnBlockEntityTransformBeforeActivated(_world, _blockPos, _blockValue, _ebcd);
    if (GameManager.IsDedicatedServer)
      return;
    if (this.damageStates != null)
    {
      this.UpdateDamageState(_blockValue, _blockValue, _ebcd);
    }
    else
    {
      int num = (int) (10.0 * (double) _blockValue.damage) / _blockValue.Block.MaxDamage;
      _ebcd.SetMaterialValue("_Damage", (float) num);
    }
    if ((double) this.block.tintColor.a > 0.0)
    {
      _ebcd.SetMaterialColor("_Color", this.block.tintColor);
    }
    else
    {
      if ((double) this.block.defaultTintColor.a <= 0.0)
        return;
      _ebcd.SetMaterialColor("_Color", this.block.defaultTintColor);
    }
  }

  public override bool UseRepairDamageState(BlockValue _blockValue)
  {
    return this.damageStates.Count > 1 && this.GetDamageStateIndex(_blockValue) == this.damageStates.Count - 1;
  }

  public void UpdateDamageState(
    BlockValue _oldBlockValue,
    BlockValue _newBlockValue,
    BlockEntityData _data,
    bool bPlayEffects = true)
  {
    int damageStateIndex1 = this.GetDamageStateIndex(_oldBlockValue);
    int damageStateIndex2 = this.GetDamageStateIndex(_newBlockValue);
    bool flag = damageStateIndex2 > damageStateIndex1;
    if (flag)
    {
      Transform transform = _data.transform.Find("FX");
      if (Object.op_Implicit((Object) transform))
      {
        AudioPlayer componentInChildren1 = ((Component) transform).GetComponentInChildren<AudioPlayer>();
        if (Object.op_Implicit((Object) componentInChildren1))
          componentInChildren1.Play();
        ParticleSystem componentInChildren2 = ((Component) transform).GetComponentInChildren<ParticleSystem>();
        if (Object.op_Implicit((Object) componentInChildren2))
          componentInChildren2.Emit(10);
      }
    }
    for (int index = 0; index < this.damageStates.Count; ++index)
    {
      BlockShapeModelEntity.DamageState damageState = this.damageStates[index];
      if (!(damageState.objName == "-"))
      {
        GameObject gameObject = ((Component) _data.transform.Find(damageState.objName)).gameObject;
        gameObject.SetActive(index == damageStateIndex2);
        if (index == damageStateIndex2 & flag)
        {
          AudioSource component1 = gameObject.GetComponent<AudioSource>();
          if (Object.op_Inequality((Object) component1, (Object) null))
            component1.PlayDelayed(0.15f);
          AudioPlayer component2 = gameObject.GetComponent<AudioPlayer>();
          if (Object.op_Inequality((Object) component2, (Object) null))
            component2.Play();
          ParticleSystem component3 = gameObject.GetComponent<ParticleSystem>();
          if (Object.op_Implicit((Object) component3))
            component3.Emit(10);
        }
      }
    }
    UpdateLightOnAllMaterials component = ((Component) _data.transform).GetComponent<UpdateLightOnAllMaterials>();
    if (!Object.op_Inequality((Object) component, (Object) null))
      return;
    component.Reset();
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public int GetDamageStateIndex(BlockValue _blockValue)
  {
    float num = (float) (_blockValue.Block.MaxDamage - _blockValue.damage);
    int damageStateIndex1 = this.damageStates.Count - 1;
    for (int damageStateIndex2 = 0; damageStateIndex2 < damageStateIndex1; ++damageStateIndex2)
    {
      if ((double) num > (double) this.damageStates[damageStateIndex2 + 1].health)
        return damageStateIndex2;
    }
    return damageStateIndex1;
  }

  public float GetNextDamageStateDownHealth(BlockValue _blockValue)
  {
    return this.damageStates[Utils.FastMin(this.GetDamageStateCount() - 1, this.GetDamageStateIndex(_blockValue) + 1)].health;
  }

  public float GetNextDamageStateUpHealth(BlockValue _blockValue)
  {
    return this.damageStates[Utils.FastMax(0, this.GetDamageStateIndex(_blockValue) - 1)].health;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public int GetDamageStateCount() => this.damageStates.Count;

  public override float GetStepHeight(BlockValue _blockValue, BlockFace crossingFace)
  {
    return this.isCustomBounds && _blockValue.Block.IsCollideMovement ? ((Bounds) ref this.boundsArr[0]).size.y : base.GetStepHeight(_blockValue, crossingFace);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  static BlockShapeModelEntity()
  {
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public struct DamageState
  {
    public string objName;
    public float health;
  }
}
