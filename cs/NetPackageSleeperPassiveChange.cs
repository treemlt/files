// Decompiled with JetBrains decompiler
// Type: NetPackageSleeperPassiveChange
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
[Preserve]
public class NetPackageSleeperPassiveChange : NetPackage
{
  [PublicizedFrom(EAccessModifier.Private)]
  public int m_targetId;

  public override NetPackageDirection PackageDirection => NetPackageDirection.ToClient;

  public NetPackageSleeperPassiveChange Setup(int targetId)
  {
    this.m_targetId = targetId;
    return this;
  }

  public override void read(PooledBinaryReader _reader) => this.m_targetId = _reader.ReadInt32();

  public override void write(PooledBinaryWriter _writer)
  {
    base.write(_writer);
    _writer.Write(this.m_targetId);
  }

  public override void ProcessPackage(World _world, GameManager _callbacks)
  {
    if (_world == null || !_world.IsRemote())
      return;
    EntityAlive entity = _world.GetEntity(this.m_targetId) as EntityAlive;
    if (Object.op_Equality((Object) entity, (Object) null))
      return;
    entity.IsSleeperPassive = false;
  }

  public override int GetLength() => 8;
}
