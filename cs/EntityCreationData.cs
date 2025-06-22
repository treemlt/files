// Decompiled with JetBrains decompiler
// Type: EntityCreationData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using System;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
[Preserve]
public class EntityCreationData
{
  [PublicizedFrom(EAccessModifier.Private)]
  public const int FileVersion = 30;
  public int entityClass;
  public Vector3 pos;
  public Vector3 rot;
  public int id;
  public bool onGround;
  public EntityStats stats;
  public int deathTime;
  public float lifetime = float.MaxValue;
  public int belongsPlayerId = -1;
  public int clientEntityId;
  public ItemValue holdingItem = ItemValue.None.Clone();
  public int teamNumber;
  public string entityName = "";
  public string skinTexture = "";
  public TileEntityLootContainer lootContainer;
  public TileEntityTrader traderData;
  [PublicizedFrom(EAccessModifier.Private)]
  public Vector3i homePosition;
  [PublicizedFrom(EAccessModifier.Private)]
  public int homeRange = -1;
  [PublicizedFrom(EAccessModifier.Private)]
  public EnumSpawnerSource spawnerSource;
  public ItemStack itemStack = ItemStack.Empty.Clone();
  public BlockValue blockValue;
  public TextureFullArray textureFull;
  public Vector3i blockPos;
  public Vector3 fallTreeDir;
  public int subType;
  public byte sleeperPose = byte.MaxValue;
  public PlayerProfile playerProfile;
  public BodyDamage bodyDamage;
  public bool isSleeper;
  public bool isSleeperPassive;
  public string spawnByName = "";
  public int spawnById = -1;
  public bool spawnByAllowShare;
  public EModelBase.HeadStates headState;
  public float overrideSize = 1f;
  public float overrideHeadSize = 1f;
  public bool isDancing;
  public byte readFileVersion;
  public MemoryStream entityData = new MemoryStream(0);

  public EntityCreationData()
  {
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public EntityCreationData(EntityCreationData _other)
  {
    this.entityClass = _other.entityClass;
    this.pos = _other.pos;
    this.rot = _other.rot;
    this.id = _other.id;
    this.onGround = _other.onGround;
    this.stats = _other.stats != null ? _other.stats.SimpleClone() : (EntityStats) null;
    this.deathTime = _other.deathTime;
    this.lifetime = _other.lifetime;
    this.itemStack = _other.itemStack;
    this.belongsPlayerId = _other.belongsPlayerId;
    this.clientEntityId = _other.clientEntityId;
    this.holdingItem = _other.holdingItem;
    this.teamNumber = _other.teamNumber;
    this.entityName = _other.entityName;
    this.skinTexture = _other.skinTexture;
    this.subType = _other.subType;
    this.lootContainer = _other.lootContainer != null ? (TileEntityLootContainer) _other.lootContainer.Clone() : (TileEntityLootContainer) null;
    this.traderData = _other.traderData != null ? (TileEntityTrader) _other.traderData.Clone() : (TileEntityTrader) null;
    this.homePosition = _other.homePosition;
    this.homeRange = _other.homeRange;
    this.entityData = _other.entityData;
    this.readFileVersion = _other.readFileVersion;
    this.playerProfile = _other.playerProfile;
    this.bodyDamage = _other.bodyDamage;
    this.sleeperPose = _other.sleeperPose;
    this.isSleeper = _other.isSleeper;
    this.isSleeperPassive = _other.isSleeperPassive;
    this.spawnByName = _other.spawnByName;
    this.spawnById = _other.spawnById;
    this.spawnByAllowShare = _other.spawnByAllowShare;
    this.headState = _other.headState;
    this.overrideSize = _other.overrideSize;
    this.overrideHeadSize = _other.overrideHeadSize;
    this.isDancing = _other.isDancing;
  }

  public EntityCreationData(XmlElement _entityElement) => this.readXml(_entityElement);

  public void ApplyToEntity(Entity _e)
  {
    EntityAlive entityAlive = _e as EntityAlive;
    if (Object.op_Implicit((Object) entityAlive))
    {
      if (this.stats != null)
        entityAlive.SetStats(this.stats);
      if (entityAlive.Health <= 0)
        entityAlive.HasDeathAnim = false;
      entityAlive.SetDeathTime(this.deathTime);
      entityAlive.setHomeArea(this.homePosition, this.homeRange);
      EntityPlayer entityPlayer = _e as EntityPlayer;
      if (Object.op_Implicit((Object) entityPlayer))
        entityPlayer.playerProfile = this.playerProfile;
      entityAlive.bodyDamage = this.bodyDamage;
      entityAlive.IsSleeper = this.isSleeper;
      if (entityAlive.IsSleeper)
        entityAlive.IsSleeperPassive = this.isSleeperPassive;
      entityAlive.CurrentHeadState = this.headState;
      entityAlive.IsDancing = this.isDancing;
    }
    _e.lootContainer = this.lootContainer;
    _e.spawnByAllowShare = this.spawnByAllowShare;
    _e.spawnById = this.spawnById;
    _e.spawnByName = this.spawnByName;
    EntityTrader entityTrader = _e as EntityTrader;
    if (Object.op_Implicit((Object) entityTrader))
      entityTrader.TileEntityTrader = this.traderData;
    if (this.sleeperPose != byte.MaxValue && Object.op_Implicit((Object) entityAlive))
      entityAlive.TriggerSleeperPose((int) this.sleeperPose);
    _e.SetSpawnerSource(this.spawnerSource);
    if (this.entityData.Length <= 0L)
      return;
    this.entityData.Position = 0L;
    try
    {
      using (PooledBinaryReader _br = MemoryPools.poolBinaryReader.AllocSync(false))
      {
        _br.SetBaseStream((Stream) this.entityData);
        _e.Read(this.readFileVersion, (BinaryReader) _br);
      }
    }
    catch (Exception ex)
    {
      Log.Exception(ex);
      Log.Error("Error loading entity " + ((object) _e)?.ToString());
    }
  }

  public EntityCreationData(Entity _e, bool _bNetworkWrite = true)
  {
    this.entityClass = _e.entityClass;
    this.id = _e.entityId;
    this.pos = _e.position;
    this.rot = _e.rotation;
    this.onGround = _e.onGround;
    this.belongsPlayerId = _e.belongsPlayerId;
    this.clientEntityId = _e.clientEntityId;
    this.lifetime = _e.lifetime;
    this.lootContainer = _e.lootContainer;
    this.spawnerSource = _e.GetSpawnerSource();
    this.spawnById = _e.spawnById;
    this.spawnByAllowShare = _e.spawnByAllowShare;
    this.spawnByName = _e.spawnByName;
    switch (_e)
    {
      case EntityAlive _:
        EntityAlive entityAlive = _e as EntityAlive;
        if (entityAlive.inventory != null)
          this.holdingItem = entityAlive.inventory.holdingItemItemValue;
        this.stats = entityAlive.Stats;
        this.deathTime = entityAlive.GetDeathTime();
        this.teamNumber = entityAlive.TeamNumber;
        this.entityName = entityAlive.EntityName;
        this.skinTexture = string.Empty;
        this.homePosition = entityAlive.getHomePosition().position;
        this.homeRange = entityAlive.getMaximumHomeDistance();
        this.bodyDamage = entityAlive.bodyDamage;
        this.sleeperPose = entityAlive.IsSleeping ? (byte) entityAlive.lastSleeperPose : byte.MaxValue;
        this.isSleeper = entityAlive.IsSleeper;
        this.isSleeperPassive = entityAlive.IsSleeperPassive;
        switch (_e)
        {
          case EntityPlayer _:
            this.playerProfile = (_e as EntityPlayer).playerProfile;
            break;
          case EntityTrader _:
            this.traderData = ((EntityTrader) _e).TileEntityTrader;
            break;
        }
        this.headState = entityAlive.GetHeadState();
        this.overrideSize = entityAlive.OverrideSize;
        this.overrideHeadSize = entityAlive.OverrideHeadSize;
        this.isDancing = entityAlive.IsDancing;
        break;
      case EntityItem _:
        this.itemStack = ((EntityItem) _e).itemStack;
        break;
      case EntityFallingBlock _:
        EntityFallingBlock entityFallingBlock = _e as EntityFallingBlock;
        this.blockValue = entityFallingBlock.GetBlockValue();
        this.textureFull = entityFallingBlock.GetTextureFull();
        break;
      case EntityFallingTree _:
        EntityFallingTree entityFallingTree = _e as EntityFallingTree;
        this.blockPos = entityFallingTree.GetBlockPos();
        this.fallTreeDir = entityFallingTree.GetFallTreeDir();
        break;
    }
    using (PooledBinaryWriter _bw = MemoryPools.poolBinaryWriter.AllocSync(false))
    {
      _bw.SetBaseStream((Stream) this.entityData);
      _e.Write((BinaryWriter) _bw, _bNetworkWrite);
    }
    this.readFileVersion = (byte) 30;
  }

  public EntityCreationData Clone() => new EntityCreationData(this);

  public void read(PooledBinaryReader _br, bool _bNetworkRead)
  {
    this.readFileVersion = _br.ReadByte();
    byte readFileVersion = this.readFileVersion;
    this.entityClass = _br.ReadInt32();
    bool flag = this.entityClass == EntityClass.playerMaleClass || this.entityClass == EntityClass.playerFemaleClass;
    this.id = _br.ReadInt32();
    this.lifetime = _br.ReadSingle();
    this.pos.x = _br.ReadSingle();
    this.pos.y = _br.ReadSingle();
    this.pos.z = _br.ReadSingle();
    this.rot.x = _br.ReadSingle();
    this.rot.y = _br.ReadSingle();
    this.rot.z = _br.ReadSingle();
    this.onGround = _br.ReadBoolean();
    this.bodyDamage = BodyDamage.Read((BinaryReader) _br, (int) readFileVersion);
    if (readFileVersion >= (byte) 8)
    {
      if (_br.ReadBoolean())
      {
        this.stats = flag ? (EntityStats) new PlayerEntityStats() : new EntityStats();
        this.stats.Read((BinaryReader) _br);
      }
    }
    else
    {
      int num1 = (int) _br.ReadInt16();
      int num2 = (int) _br.ReadInt16();
      if (readFileVersion >= (byte) 7)
      {
        int num3 = (int) _br.ReadInt16();
        int num4 = (int) _br.ReadInt16();
      }
    }
    this.deathTime = (int) _br.ReadInt16();
    if (readFileVersion >= (byte) 2 && _br.ReadBoolean())
    {
      this.lootContainer = (TileEntityLootContainer) TileEntity.Instantiate((TileEntityType) _br.ReadInt32(), (Chunk) null);
      this.lootContainer.read(_br, _bNetworkRead ? TileEntity.StreamModeRead.FromServer : TileEntity.StreamModeRead.Persistency);
    }
    if (readFileVersion >= (byte) 3)
    {
      this.homePosition = new Vector3i(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32());
      this.homeRange = (int) _br.ReadInt16();
    }
    if (readFileVersion >= (byte) 5)
      this.spawnerSource = (EnumSpawnerSource) _br.ReadByte();
    if (this.entityClass == EntityClass.itemClass)
    {
      this.belongsPlayerId = readFileVersion > (byte) 5 ? _br.ReadInt32() : (int) _br.ReadInt16();
      if (readFileVersion >= (byte) 27)
        this.clientEntityId = _br.ReadInt32();
      this.itemStack = ItemStack.Empty.Clone();
      if (readFileVersion < (byte) 14)
        this.itemStack.ReadOld((BinaryReader) _br);
      else
        this.itemStack.Read((BinaryReader) _br);
      if (readFileVersion >= (byte) 3)
      {
        int num = (int) _br.ReadSByte();
      }
    }
    else if (this.entityClass == EntityClass.fallingBlockClass)
    {
      this.blockValue = new BlockValue(_br.ReadUInt32());
      if (readFileVersion < (byte) 29)
      {
        this.textureFull.Fill(0L);
        this.textureFull[0] = _br.ReadInt64();
      }
      else
        this.textureFull.Read((BinaryReader) _br);
    }
    else if (this.entityClass == EntityClass.fallingTreeClass)
    {
      this.blockPos = StreamUtils.ReadVector3i((BinaryReader) _br);
      this.fallTreeDir = StreamUtils.ReadVector3((BinaryReader) _br);
    }
    else if (flag)
    {
      this.holdingItem.Read((BinaryReader) _br);
      this.teamNumber = (int) _br.ReadByte();
      this.entityName = _br.ReadString();
      this.skinTexture = _br.ReadString();
      if (readFileVersion > (byte) 12)
        this.playerProfile = !_br.ReadBoolean() ? (PlayerProfile) null : PlayerProfile.Read((BinaryReader) _br);
    }
    if (readFileVersion > (byte) 9)
    {
      int count = (int) _br.ReadUInt16();
      if (count > 0)
        this.entityData = new MemoryStream(_br.ReadBytes(count));
    }
    if (readFileVersion > (byte) 23 && _br.ReadBoolean())
    {
      this.traderData = (TileEntityTrader) TileEntity.Instantiate((TileEntityType) _br.ReadInt32(), (Chunk) null);
      this.traderData.read(_br, _bNetworkRead ? TileEntity.StreamModeRead.FromServer : TileEntity.StreamModeRead.Persistency);
    }
    if (!_bNetworkRead)
      return;
    this.sleeperPose = _br.ReadByte();
    this.isSleeper = _br.ReadBoolean();
    this.spawnById = _br.ReadInt32();
    this.spawnByName = _br.ReadString();
    this.spawnByAllowShare = _br.ReadBoolean();
    this.headState = (EModelBase.HeadStates) _br.ReadByte();
    this.overrideSize = _br.ReadSingle();
    this.overrideHeadSize = _br.ReadSingle();
    this.isDancing = _br.ReadBoolean();
    if (!this.isSleeper)
      return;
    this.isSleeperPassive = _br.ReadBoolean();
  }

  public void write(PooledBinaryWriter _bw, bool _bNetworkWrite)
  {
    _bw.Write((byte) 30);
    _bw.Write(this.entityClass);
    _bw.Write(this.id);
    _bw.Write(this.lifetime);
    _bw.Write(this.pos.x);
    _bw.Write(this.pos.y);
    _bw.Write(this.pos.z);
    _bw.Write(this.rot.x);
    _bw.Write(this.rot.y);
    _bw.Write(this.rot.z);
    _bw.Write(this.onGround);
    this.bodyDamage.Write((BinaryWriter) _bw);
    _bw.Write(this.stats != null);
    if (this.stats != null)
      this.stats.Write((BinaryWriter) _bw);
    _bw.Write((short) this.deathTime);
    _bw.Write(this.lootContainer != null);
    if (this.lootContainer != null)
    {
      _bw.Write((int) this.lootContainer.GetTileEntityType());
      this.lootContainer.write(_bw, _bNetworkWrite ? TileEntity.StreamModeWrite.ToClient : TileEntity.StreamModeWrite.Persistency);
    }
    _bw.Write(this.homePosition.x);
    _bw.Write(this.homePosition.y);
    _bw.Write(this.homePosition.z);
    _bw.Write((short) this.homeRange);
    _bw.Write((byte) this.spawnerSource);
    if (this.entityClass == EntityClass.itemClass)
    {
      _bw.Write(this.belongsPlayerId);
      _bw.Write(this.clientEntityId);
      this.itemStack.Write((BinaryWriter) _bw);
      _bw.Write((sbyte) 0);
    }
    else if (this.entityClass == EntityClass.fallingBlockClass)
    {
      _bw.Write(this.blockValue.rawData);
      this.textureFull.Write((BinaryWriter) _bw);
    }
    else if (this.entityClass == EntityClass.fallingTreeClass)
    {
      StreamUtils.Write((BinaryWriter) _bw, this.blockPos);
      StreamUtils.Write((BinaryWriter) _bw, this.fallTreeDir);
    }
    else if (this.entityClass == EntityClass.playerMaleClass || this.entityClass == EntityClass.playerFemaleClass)
    {
      ItemValue.Write(this.holdingItem, (BinaryWriter) _bw);
      _bw.Write((byte) this.teamNumber);
      _bw.Write(this.entityName);
      _bw.Write(this.skinTexture);
      _bw.Write(this.playerProfile != null);
      if (this.playerProfile != null)
        this.playerProfile.Write((BinaryWriter) _bw);
    }
    int length = (int) this.entityData.Length;
    _bw.Write((ushort) length);
    if (length > 0)
      _bw.Write(this.entityData.ToArray());
    _bw.Write(this.traderData != null);
    if (this.traderData != null)
    {
      _bw.Write((int) this.traderData.GetTileEntityType());
      this.traderData.write(_bw, _bNetworkWrite ? TileEntity.StreamModeWrite.ToClient : TileEntity.StreamModeWrite.Persistency);
    }
    if (!_bNetworkWrite)
      return;
    _bw.Write(this.sleeperPose);
    _bw.Write(this.isSleeper);
    _bw.Write(this.spawnById);
    _bw.Write(this.spawnByName);
    _bw.Write(this.spawnByAllowShare);
    _bw.Write((byte) this.headState);
    _bw.Write(this.overrideSize);
    _bw.Write(this.overrideHeadSize);
    _bw.Write(this.isDancing);
    if (!this.isSleeper)
      return;
    _bw.Write(this.isSleeperPassive);
  }

  public void readXml(XmlElement _entityElement)
  {
    this.entityClass = _entityElement.HasAttribute("type") ? EntityClass.FromString(_entityElement.GetAttribute("type")) : throw new Exception("No 'type' element found in entity tag!");
    this.pos = _entityElement.HasAttribute("position") ? StringParsers.ParseVector3(_entityElement.GetAttribute("position")) : throw new Exception("No 'position' element found in entity tag!");
    this.rot = _entityElement.HasAttribute("rotation") ? StringParsers.ParseVector3(_entityElement.GetAttribute("rotation")) : throw new Exception("No 'rotation' element found in entity tag!");
    this.id = -1;
  }

  public void writeXml(StreamWriter _sw)
  {
    _sw.WriteLine($"    <entity type=\"{EntityClass.list[this.entityClass].entityClassName}\" position=\"{this.pos.x.ToCultureInvariantString()},{this.pos.y.ToCultureInvariantString()},{this.pos.z.ToCultureInvariantString()}\" rotation=\"{this.rot.x.ToCultureInvariantString()},{this.rot.y.ToCultureInvariantString()},{this.rot.z.ToCultureInvariantString()}\" />");
  }

  public override string ToString()
  {
    return $"{EntityClass.list[this.entityClass].entityClassName} {this.entityName} id={this.id.ToString()} pos={this.pos.ToCultureInvariantString()}";
  }
}
