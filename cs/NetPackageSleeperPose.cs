// Decompiled with JetBrains decompiler
// Type: NetPackageSleeperPose
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
[Preserve]
public class NetPackageSleeperPose : NetPackage
{
  [PublicizedFrom(EAccessModifier.Private)]
  public int m_targetId;
  [PublicizedFrom(EAccessModifier.Private)]
  public byte m_pose;

  public NetPackageSleeperPose Setup(int targetId, byte pose)
  {
    this.m_targetId = targetId;
    this.m_pose = pose;
    return this;
  }

  public override void read(PooledBinaryReader _reader)
  {
    this.m_targetId = _reader.ReadInt32();
    this.m_pose = _reader.ReadByte();
  }

  public override void write(PooledBinaryWriter _writer)
  {
    base.write(_writer);
    _writer.Write(this.m_targetId);
    _writer.Write(this.m_pose);
  }

  public override void ProcessPackage(World _world, GameManager _callbacks)
  {
    if (_world == null || !_world.IsRemote())
      return;
    EntityAlive entity = _world.GetEntity(this.m_targetId) as EntityAlive;
    if (Object.op_Equality((Object) entity, (Object) null))
      return;
    entity.TriggerSleeperPose((int) this.m_pose);
  }

  public override int GetLength() => 8;
}
