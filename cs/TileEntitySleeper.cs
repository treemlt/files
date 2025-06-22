// Decompiled with JetBrains decompiler
// Type: TileEntitySleeper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

#nullable disable
public class TileEntitySleeper : TileEntity
{
  [PublicizedFrom(EAccessModifier.Private)]
  public float priorityMultiplier;
  [PublicizedFrom(EAccessModifier.Private)]
  public int sightAngle;
  [PublicizedFrom(EAccessModifier.Private)]
  public int sightRange;
  [PublicizedFrom(EAccessModifier.Private)]
  public float hearingPercent;

  public TileEntitySleeper(Chunk _chunk)
    : base(_chunk)
  {
    this.priorityMultiplier = 1f;
    this.sightAngle = -1;
    this.sightRange = -1;
    this.hearingPercent = 1f;
  }

  public override TileEntity Clone()
  {
    TileEntitySleeper tileEntitySleeper = new TileEntitySleeper(this.chunk);
    tileEntitySleeper.localChunkPos = this.localChunkPos;
    tileEntitySleeper.priorityMultiplier = this.priorityMultiplier;
    tileEntitySleeper.sightAngle = this.sightAngle;
    tileEntitySleeper.sightRange = this.sightRange;
    tileEntitySleeper.hearingPercent = this.hearingPercent;
    return (TileEntity) tileEntitySleeper;
  }

  public override void CopyFrom(TileEntity _other)
  {
    TileEntitySleeper tileEntitySleeper = (TileEntitySleeper) _other;
    this.priorityMultiplier = tileEntitySleeper.priorityMultiplier;
    this.sightAngle = tileEntitySleeper.sightAngle;
    this.sightRange = tileEntitySleeper.sightRange;
    this.hearingPercent = tileEntitySleeper.hearingPercent;
  }

  public override TileEntityType GetTileEntityType() => TileEntityType.Sleeper;

  public void SetPriorityMultiplier(float _priorityMultiplier)
  {
    this.priorityMultiplier = _priorityMultiplier;
    this.setModified();
  }

  public float GetPriorityMultiplier() => this.priorityMultiplier;

  public void SetSightAngle(int _sightAngle)
  {
    this.sightAngle = _sightAngle;
    this.setModified();
  }

  public int GetSightAngle() => this.sightAngle;

  public void SetSightRange(int _sightRange)
  {
    this.sightRange = _sightRange;
    this.setModified();
  }

  public int GetSightRange() => this.sightRange;

  public void SetHearingPercent(float _hearingPercent)
  {
    this.hearingPercent = _hearingPercent;
    this.setModified();
  }

  public float GetHearingPercent() => this.hearingPercent;

  public override void read(PooledBinaryReader _br, TileEntity.StreamModeRead _eStreamMode)
  {
    base.read(_br, _eStreamMode);
    this.priorityMultiplier = _br.ReadSingle();
    this.sightRange = (int) _br.ReadInt16();
    this.hearingPercent = _br.ReadSingle();
    this.sightAngle = (int) _br.ReadInt16();
  }

  public override void write(PooledBinaryWriter _bw, TileEntity.StreamModeWrite _eStreamMode)
  {
    base.write(_bw, _eStreamMode);
    _bw.Write(this.priorityMultiplier);
    _bw.Write((short) this.sightRange);
    _bw.Write(this.hearingPercent);
    _bw.Write((short) this.sightAngle);
  }
}
