// Decompiled with JetBrains decompiler
// Type: RandomPositionGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class RandomPositionGenerator
{
  public static Vector3 Calc(EntityAlive _entity, int _maxXZ, int _maxY)
  {
    Vector3 destPos;
    if (!RandomPositionGenerator.calc(_entity, _maxXZ, _maxY, false, out destPos) && _entity.isSwimming)
      RandomPositionGenerator.calc(_entity, _maxXZ, _maxY, true, out destPos);
    return destPos;
  }

  public static Vector3 CalcTowards(
    EntityAlive _entity,
    int _minXZ,
    int _maxXZ,
    int _maxY,
    Vector3 _position)
  {
    Vector3 _dirV = Vector3.op_Subtraction(_position, _entity.position);
    return RandomPositionGenerator.CalcInDir(_entity, _minXZ, _maxXZ, _maxY, _dirV);
  }

  public static Vector3 CalcAway(
    EntityAlive _entity,
    int _minXZ,
    int _maxXZ,
    int _maxY,
    Vector3 _position)
  {
    Vector3 _dirV = Vector3.op_Subtraction(_entity.position, _position);
    return RandomPositionGenerator.CalcInDir(_entity, _minXZ, _maxXZ, _maxY, _dirV);
  }

  public static Vector3 CalcInDir(
    EntityAlive _entity,
    int _minXZ,
    int _maxXZ,
    int _maxY,
    Vector3 _dirV)
  {
    Vector3 destPos;
    if (!RandomPositionGenerator.calcDir(_entity, _minXZ, _maxXZ, _maxY, _dirV, false, out destPos) && _entity.isSwimming)
      RandomPositionGenerator.calcDir(_entity, _minXZ, _maxXZ, _maxY, _dirV, true, out destPos);
    return destPos;
  }

  public static Vector3 CalcNear(EntityAlive _entity, Vector3 target, int _xzDist, int _yDist)
  {
    GameRandom rand = _entity.rand;
    int num1 = rand.RandomRange(2 * _xzDist) - _xzDist;
    int num2 = rand.RandomRange(2 * _yDist) - _yDist;
    int num3 = rand.RandomRange(2 * _xzDist) - _xzDist;
    return new Vector3((float) (num1 + Utils.Fastfloor(target.x)), (float) (num2 + Utils.Fastfloor(target.y)), (float) (num3 + Utils.Fastfloor(target.z)));
  }

  public static Vector3 CalcPositionInDirection(
    Entity _entity,
    Vector3 _startPos,
    Vector3 _dirV,
    float _dist,
    float _randomAngle)
  {
    World world = _entity.world;
    _dirV.y = 0.0f;
    Vector3 normalized = ((Vector3) ref _dirV).normalized;
    Matrix4x4 matrix4x4 = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0.0f, _randomAngle * (_entity.rand.RandomFloat - 0.5f), 0.0f), Vector3.one);
    Vector3 vector3 = ((Matrix4x4) ref matrix4x4).MultiplyVector(normalized);
    Vector3i blockPos = World.worldToBlockPos(_startPos);
    Chunk chunkFromWorldPos;
    do
    {
      blockPos.x = (int) ((double) _startPos.x + (double) vector3.x * (double) _dist);
      blockPos.z = (int) ((double) _startPos.z + (double) vector3.z * (double) _dist);
      chunkFromWorldPos = (Chunk) world.GetChunkFromWorldPos(blockPos.x, 0, blockPos.z);
      if (chunkFromWorldPos == null)
        _dist -= 8f;
      else
        break;
    }
    while ((double) _dist > 0.0);
    if ((double) _dist <= 0.0 || chunkFromWorldPos == null)
      return Vector3.zero;
    int blockXz1 = World.toBlockXZ(blockPos.x);
    int blockXz2 = World.toBlockXZ(blockPos.z);
    BlockValue blockNoDamage1 = chunkFromWorldPos.GetBlockNoDamage(blockXz1, blockPos.y, blockXz2);
    if (blockNoDamage1.Block.IsMovementBlocked((IBlockAccess) world, blockPos, blockNoDamage1, BlockFaceFlag.None))
    {
      while (++blockPos.y < 256 /*0x0100*/)
      {
        BlockValue blockNoDamage2 = chunkFromWorldPos.GetBlockNoDamage(blockXz1, blockPos.y, blockXz2);
        if (!blockNoDamage2.Block.IsMovementBlocked((IBlockAccess) world, blockPos, blockNoDamage2, BlockFaceFlag.None))
          return (Vector3) blockPos;
      }
    }
    else
    {
      while (--blockPos.y >= 0)
      {
        BlockValue blockNoDamage3 = chunkFromWorldPos.GetBlockNoDamage(blockXz1, blockPos.y, blockXz2);
        if (blockNoDamage3.Block.IsMovementBlocked((IBlockAccess) world, blockPos, blockNoDamage3, BlockFaceFlag.None))
        {
          ++blockPos.y;
          return (Vector3) blockPos;
        }
      }
    }
    return Vector3.zero;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static bool calc(
    EntityAlive _entity,
    int _xzDist,
    int _yDist,
    bool canSwim,
    out Vector3 destPos)
  {
    GameRandom rand = _entity.rand;
    World world = _entity.world;
    ChunkCluster chunkCache = world.ChunkCache;
    Vector3 _worldPos = _entity.position;
    if (_entity.IsSleeper)
      _worldPos = _entity.SleeperSpawnPosition;
    Vector3i blockPos = World.worldToBlockPos(_worldPos);
    bool flag1 = false;
    if (_entity.hasHome())
      flag1 = (double) _entity.getHomePosition().getDistance(blockPos.x, blockPos.y, blockPos.z) + 4.0 < (double) (_entity.getMaximumHomeDistance() + _xzDist);
    for (int index1 = 0; index1 < 30; ++index1)
    {
      Vector3i _pos;
      _pos.x = rand.RandomRange(2 * _xzDist) - _xzDist;
      _pos.z = rand.RandomRange(2 * _xzDist) - _xzDist;
      _pos.y = rand.RandomRange(2 * _yDist) - _yDist;
      _pos.x += blockPos.x;
      _pos.y += blockPos.y;
      _pos.z += blockPos.z;
      BlockValue block = chunkCache.GetBlock(_pos);
      if (block.isair && (canSwim || !world.IsWater(_pos)) && (!flag1 || _entity.isWithinHomeDistance(_pos.x, _pos.y, _pos.z)) && _pos.y >= 0)
      {
        if (!canSwim)
        {
          bool flag2 = false;
          Vector3i vector3i = _pos;
          for (int index2 = 0; index2 < 10; ++index2)
          {
            --vector3i.y;
            block = chunkCache.GetBlock(vector3i);
            if (world.IsWater(vector3i))
            {
              flag2 = true;
              break;
            }
            if (block.Block.IsMovementBlocked((IBlockAccess) world, vector3i, block, BlockFaceFlag.None))
              break;
          }
          if (flag2)
            continue;
        }
        destPos = new Vector3((float) _pos.x + 0.5f, (float) _pos.y, (float) _pos.z + 0.5f);
        return true;
      }
    }
    destPos = Vector3.zero;
    return false;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static bool calcDir(
    EntityAlive _entity,
    int _distMinXZ,
    int _distMaxXZ,
    int _distMaxY,
    Vector3 _directionVec,
    bool canSwim,
    out Vector3 destPos)
  {
    if (Vector3.op_Equality(_directionVec, Vector3.zero))
      return RandomPositionGenerator.calc(_entity, _distMaxXZ, _distMaxY, canSwim, out destPos);
    GameRandom rand = _entity.rand;
    ChunkCluster chunkCache = _entity.world.ChunkCache;
    Vector3i blockPos = World.worldToBlockPos(_entity.position);
    if (_distMaxXZ < _distMinXZ)
      _distMaxXZ = _distMinXZ;
    bool flag = false;
    if (_entity.hasHome())
    {
      float num = _entity.getHomePosition().getDistance(blockPos.x, blockPos.y, blockPos.z) + 1f;
      if ((double) _distMinXZ > (double) num)
        _distMinXZ = (int) num;
      if ((double) _distMaxXZ > (double) num)
        _distMaxXZ = (int) num;
      flag = (double) (_entity.getMaximumHomeDistance() + _distMaxXZ) - (double) num >= 2.0;
    }
    int _maxExclusive = _distMaxXZ - _distMinXZ;
    Vector2 vector2_1;
    vector2_1.x = _directionVec.x;
    vector2_1.y = _directionVec.z;
    ((Vector2) ref vector2_1).Normalize();
    for (int index = 0; index < 30; ++index)
    {
      double num1 = ((double) rand.RandomFloat * 80.0 - 40.0) * (Math.PI / 180.0);
      float num2 = (float) (_distMinXZ + rand.RandomRange(_maxExclusive));
      float num3 = Mathf.Sin((float) num1);
      float num4 = Mathf.Cos((float) num1);
      Vector2 vector2_2;
      vector2_2.x = (float) ((double) vector2_1.x * (double) num4 - (double) vector2_1.y * (double) num3);
      vector2_2.y = (float) ((double) vector2_1.x * (double) num3 + (double) vector2_1.y * (double) num4);
      vector2_2.x *= num2;
      vector2_2.y *= num2;
      Vector3i _pos;
      _pos.x = Utils.Fastfloor(vector2_2.x);
      _pos.z = Utils.Fastfloor(vector2_2.y);
      _pos.y = rand.RandomRange(2 * _distMaxY) - _distMaxY;
      _pos.x += blockPos.x;
      _pos.y += blockPos.y;
      _pos.z += blockPos.z;
      if (chunkCache.GetBlock(_pos).isair && (canSwim || !_entity.world.IsWater(_pos)) && (!flag || _entity.isWithinHomeDistance(_pos.x, _pos.y, _pos.z)))
      {
        destPos = new Vector3((float) _pos.x + 0.5f, (float) _pos.y, (float) _pos.z + 0.5f);
        return true;
      }
    }
    destPos = Vector3.zero;
    return false;
  }
}
