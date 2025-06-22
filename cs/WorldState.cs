// Decompiled with JetBrains decompiler
// Type: WorldState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
public class WorldState
{
  [PublicizedFrom(EAccessModifier.Private)]
  public static int CurrentSaveVersion = 22;
  [PublicizedFrom(EAccessModifier.Private)]
  public uint version;
  public string gameVersionString = "";
  public VersionInformation gameVersion;
  [PublicizedFrom(EAccessModifier.Private)]
  public float waterLevel;
  [PublicizedFrom(EAccessModifier.Private)]
  public int chunkSizeX;
  [PublicizedFrom(EAccessModifier.Private)]
  public int chunkSizeY;
  [PublicizedFrom(EAccessModifier.Private)]
  public int chunkSizeZ;
  [PublicizedFrom(EAccessModifier.Private)]
  public int chunkCount;
  public MemoryStream dynamicSpawnerState;
  public MemoryStream aiDirectorState;
  public int activeGameMode;
  public EnumChunkProviderId providerId;
  public int seed;
  public ulong worldTime;
  public ulong timeInTicks;
  public int nextEntityID;
  public long saveDataLimit;
  [PublicizedFrom(EAccessModifier.Private)]
  public SpawnPointList playerSpawnPoints;
  public MemoryStream sleeperVolumeState;
  public MemoryStream triggerVolumeState;
  public MemoryStream wallVolumeState;
  [CompilerGenerated]
  [PublicizedFrom(EAccessModifier.Private)]
  public string \u003CGuid\u003Ek__BackingField;

  public string Guid
  {
    get => this.\u003CGuid\u003Ek__BackingField;
    [PublicizedFrom(EAccessModifier.Private)] set => this.\u003CGuid\u003Ek__BackingField = value;
  }

  public WorldState()
  {
    this.providerId = EnumChunkProviderId.Disc;
    this.saveDataLimit = -1L;
    this.playerSpawnPoints = new SpawnPointList();
    this.GenerateNewGuid();
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public bool SaveLoad(
    string _filename,
    bool _load,
    bool _warnOnDifferentVersion,
    bool _infOnDiferentVersion)
  {
    lock (this)
    {
      Stream _stream = (Stream) null;
      try
      {
        if (_load)
        {
          try
          {
            _stream = SdFile.OpenRead(_filename);
          }
          catch (Exception ex)
          {
            Log.Error($"Opening saved game: {ex}");
          }
        }
        else
        {
          try
          {
            _stream = (Stream) new BufferedStream(SdFile.Open(_filename, FileMode.Create, FileAccess.Write, FileShare.Read));
          }
          catch (Exception ex)
          {
            Log.Error($"Opening buffer to save game: {ex}");
          }
        }
        return _stream != null && this.SaveLoad(_stream, _load, _warnOnDifferentVersion, _infOnDiferentVersion);
      }
      catch (Exception ex)
      {
        Log.Error($"Exception reading world header at pos {(_stream != null ? _stream.Position : 0L)}: {ex}");
        return false;
      }
      finally
      {
        _stream?.Dispose();
      }
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public bool SaveLoad(
    Stream _stream,
    bool _load,
    bool _warnOnDifferentVersion,
    bool _infOnDiferentVersion)
  {
    lock (this)
    {
      PooledBinaryWriter _t1 = (PooledBinaryWriter) null;
      PooledBinaryReader _t2 = (PooledBinaryReader) null;
      try
      {
        IBinaryReaderOrWriter binaryReaderOrWriter;
        if (_load)
        {
          this.chunkSizeX = this.chunkSizeY = this.chunkSizeZ = this.chunkCount = 0;
          _t2 = MemoryPools.poolBinaryReader.AllocSync(false);
          _t2.SetBaseStream(_stream);
          binaryReaderOrWriter = (IBinaryReaderOrWriter) _t2;
          int num1 = (int) binaryReaderOrWriter.ReadWrite(' ');
          char ch1 = binaryReaderOrWriter.ReadWrite(' ');
          char ch2 = binaryReaderOrWriter.ReadWrite(' ');
          byte num2 = binaryReaderOrWriter.ReadWrite((byte) 1);
          if (num1 != 116 || ch1 != 't' || ch2 != 'w' || num2 != (byte) 0)
          {
            Log.Error("Invalid magic bytes in world header");
            return false;
          }
        }
        else
        {
          _t1 = MemoryPools.poolBinaryWriter.AllocSync(false);
          _t1.SetBaseStream(_stream);
          binaryReaderOrWriter = (IBinaryReaderOrWriter) _t1;
          int num3 = (int) binaryReaderOrWriter.ReadWrite('t');
          int num4 = (int) binaryReaderOrWriter.ReadWrite('t');
          int num5 = (int) binaryReaderOrWriter.ReadWrite('w');
          int num6 = (int) binaryReaderOrWriter.ReadWrite((byte) 0);
        }
        this.version = binaryReaderOrWriter.ReadWrite((uint) WorldState.CurrentSaveVersion);
        if (_load)
        {
          if ((long) this.version > (long) WorldState.CurrentSaveVersion)
            return true;
          if (this.version > 11U)
          {
            this.gameVersionString = binaryReaderOrWriter.ReadWrite("");
            if (this.gameVersionString != Constants.cVersionInformation.LongString)
            {
              if (_warnOnDifferentVersion)
                Log.Warning("Loaded world file from different version: '{0}'", new object[1]
                {
                  (object) this.gameVersionString
                });
              else if (_infOnDiferentVersion)
                Log.Out("Loaded world file from different version: '{0}'", new object[1]
                {
                  (object) this.gameVersionString
                });
            }
          }
        }
        else
          binaryReaderOrWriter.ReadWrite(Constants.cVersionInformation.LongString);
        if (_load)
        {
          if (this.version > 14U)
            this.gameVersion = new VersionInformation((VersionInformation.EGameReleaseType) binaryReaderOrWriter.ReadWrite(1), binaryReaderOrWriter.ReadWrite(2), binaryReaderOrWriter.ReadWrite(0), binaryReaderOrWriter.ReadWrite(289));
          else
            VersionInformation.TryParseLegacyString(this.gameVersionString, out this.gameVersion);
        }
        else
        {
          binaryReaderOrWriter.ReadWrite(1);
          binaryReaderOrWriter.ReadWrite(2);
          binaryReaderOrWriter.ReadWrite(0);
          binaryReaderOrWriter.ReadWrite(289);
        }
        int num7 = (int) binaryReaderOrWriter.ReadWrite(0U);
        if (this.version > 6U)
          this.activeGameMode = binaryReaderOrWriter.ReadWrite(this.activeGameMode);
        int num8 = (int) binaryReaderOrWriter.ReadWrite(0U);
        this.waterLevel = binaryReaderOrWriter.ReadWrite(this.waterLevel);
        this.chunkSizeX = binaryReaderOrWriter.ReadWrite(this.chunkSizeX);
        this.chunkSizeZ = binaryReaderOrWriter.ReadWrite(this.chunkSizeY);
        this.chunkSizeY = binaryReaderOrWriter.ReadWrite(this.chunkSizeZ);
        this.chunkCount = binaryReaderOrWriter.ReadWrite(this.chunkCount);
        this.providerId = (EnumChunkProviderId) binaryReaderOrWriter.ReadWrite((int) this.providerId);
        this.seed = binaryReaderOrWriter.ReadWrite(this.seed);
        this.worldTime = binaryReaderOrWriter.ReadWrite(this.worldTime);
        if (this.version > 8U)
          this.timeInTicks = binaryReaderOrWriter.ReadWrite(this.timeInTicks);
        if (_load)
        {
          if (this.version == 10U)
          {
            long num9 = (long) binaryReaderOrWriter.ReadWrite(0UL);
          }
          if (this.version > 1U && this.version < 7U)
            binaryReaderOrWriter.ReadWrite(false);
          if (this.version > 4U && this.version < 7U)
          {
            binaryReaderOrWriter.ReadWrite(false);
            binaryReaderOrWriter.ReadWrite(false);
          }
          if (this.version > 5U)
            this.playerSpawnPoints.Read(binaryReaderOrWriter);
          else if (this.version > 2U)
          {
            this.playerSpawnPoints.Clear();
            int num10 = binaryReaderOrWriter.ReadWrite(0);
            for (int index = 0; index < num10; ++index)
              this.playerSpawnPoints.Add(new SpawnPoint(new Vector3i(binaryReaderOrWriter.ReadWrite(0), binaryReaderOrWriter.ReadWrite(0), binaryReaderOrWriter.ReadWrite(0))));
          }
        }
        else
        {
          int num11 = (int) binaryReaderOrWriter.ReadWrite((byte) 0);
          binaryReaderOrWriter.ReadWrite(0);
        }
        if (this.version > 3U)
          this.nextEntityID = binaryReaderOrWriter.ReadWrite(this.nextEntityID);
        if (_load)
          this.nextEntityID = Utils.FastMax(this.nextEntityID, 171);
        this.saveDataLimit = this.version < 21U ? -1L : binaryReaderOrWriter.ReadWrite(this.saveDataLimit);
        if (this.version > 7U)
        {
          int num12 = binaryReaderOrWriter.ReadWrite(this.dynamicSpawnerState != null ? (int) this.dynamicSpawnerState.Length : 0);
          if (_load)
          {
            if (num12 > 0)
            {
              this.dynamicSpawnerState = new MemoryStream(num12);
              this.dynamicSpawnerState.SetLength((long) num12);
              binaryReaderOrWriter.ReadWrite(this.dynamicSpawnerState.GetBuffer(), 0, num12);
              this.dynamicSpawnerState.Position = 0L;
            }
          }
          else if (this.dynamicSpawnerState != null)
          {
            this.dynamicSpawnerState.Position = 0L;
            StreamUtils.StreamCopy((Stream) this.dynamicSpawnerState, binaryReaderOrWriter.BaseStream);
          }
        }
        if (this.version > 10U)
        {
          int num13 = binaryReaderOrWriter.ReadWrite(this.aiDirectorState != null ? (int) this.aiDirectorState.Length : 0);
          if (_load)
          {
            if (num13 > 0)
            {
              this.aiDirectorState = new MemoryStream(num13);
              this.aiDirectorState.SetLength((long) num13);
              binaryReaderOrWriter.ReadWrite(this.aiDirectorState.GetBuffer(), 0, num13);
              this.aiDirectorState.Position = 0L;
            }
          }
          else if (this.aiDirectorState != null)
          {
            this.aiDirectorState.Position = 0L;
            StreamUtils.StreamCopy((Stream) this.aiDirectorState, binaryReaderOrWriter.BaseStream);
          }
        }
        if (this.version > 12U)
        {
          int num14 = binaryReaderOrWriter.ReadWrite(this.sleeperVolumeState != null ? (int) this.sleeperVolumeState.Length : 0);
          if (_load)
          {
            if (num14 > 0)
            {
              this.sleeperVolumeState = new MemoryStream(num14);
              this.sleeperVolumeState.SetLength((long) num14);
              binaryReaderOrWriter.ReadWrite(this.sleeperVolumeState.GetBuffer(), 0, num14);
              this.sleeperVolumeState.Position = 0L;
            }
          }
          else if (this.sleeperVolumeState != null)
          {
            this.sleeperVolumeState.Position = 0L;
            StreamUtils.StreamCopy((Stream) this.sleeperVolumeState, binaryReaderOrWriter.BaseStream);
          }
        }
        if (this.version >= 19U)
        {
          int num15 = binaryReaderOrWriter.ReadWrite(this.triggerVolumeState != null ? (int) this.triggerVolumeState.Length : 0);
          if (_load)
          {
            if (num15 > 0)
            {
              this.triggerVolumeState = new MemoryStream(num15);
              this.triggerVolumeState.SetLength((long) num15);
              binaryReaderOrWriter.ReadWrite(this.triggerVolumeState.GetBuffer(), 0, num15);
              this.triggerVolumeState.Position = 0L;
            }
          }
          else if (this.triggerVolumeState != null)
          {
            this.triggerVolumeState.Position = 0L;
            StreamUtils.StreamCopy((Stream) this.triggerVolumeState, binaryReaderOrWriter.BaseStream);
          }
        }
        if (this.version >= 20U)
        {
          int num16 = binaryReaderOrWriter.ReadWrite(this.wallVolumeState != null ? (int) this.wallVolumeState.Length : 0);
          if (_load)
          {
            if (num16 > 0)
            {
              this.wallVolumeState = new MemoryStream(num16);
              this.wallVolumeState.SetLength((long) num16);
              binaryReaderOrWriter.ReadWrite(this.wallVolumeState.GetBuffer(), 0, num16);
              this.wallVolumeState.Position = 0L;
            }
          }
          else if (this.wallVolumeState != null)
          {
            this.wallVolumeState.Position = 0L;
            StreamUtils.StreamCopy((Stream) this.wallVolumeState, binaryReaderOrWriter.BaseStream);
          }
        }
        bool flag = false;
        if (this.version > 11U)
        {
          long position = binaryReaderOrWriter.BaseStream.Position;
          int num17 = 0;
          if (this.version > 15U)
            num17 = binaryReaderOrWriter.ReadWrite(0);
          if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && this.version >= 22U)
          {
            if (_load)
            {
              int _loadSize = num17 - 4;
              if (_loadSize > 0)
              {
                flag = true;
                WeatherManager.Load(binaryReaderOrWriter, _loadSize);
              }
            }
            else
              WeatherManager.Save(binaryReaderOrWriter);
          }
          if (this.version > 15U)
          {
            if (_load)
            {
              if (binaryReaderOrWriter.BaseStream.Position != position + (long) num17)
              {
                if (flag)
                  Log.Out("Failed reading weather data from world header");
                binaryReaderOrWriter.BaseStream.Position = position + (long) num17;
              }
            }
            else
            {
              int num18 = (int) (binaryReaderOrWriter.BaseStream.Position - position);
              binaryReaderOrWriter.BaseStream.Position = position;
              binaryReaderOrWriter.ReadWrite(num18);
              binaryReaderOrWriter.BaseStream.Seek(0L, SeekOrigin.End);
            }
          }
        }
        if (this.version > 13U && (flag || this.version > 15U))
          this.Guid = binaryReaderOrWriter.ReadWrite(this.Guid);
        if (_load && string.IsNullOrEmpty(this.Guid))
          this.GenerateNewGuid();
        return true;
      }
      catch (Exception ex)
      {
        Log.Error("Exception reading world header at pos {0}:", new object[1]
        {
          (object) _stream.Position
        });
        Log.Exception(ex);
        return false;
      }
      finally
      {
        if (_t2 != null)
          MemoryPools.poolBinaryReader.FreeSync(_t2);
        if (_t1 != null)
          MemoryPools.poolBinaryWriter.FreeSync(_t1);
      }
    }
  }

  public bool Load(
    string _filename,
    bool _warnOnDifferentVersion = true,
    bool _infOnDiferentVersion = false,
    bool _makeExtraBackupOnSuccess = false)
  {
    // ISSUE: variable of a compiler-generated type
    WorldState.\u003C\u003Ec__DisplayClass29_0 cDisplayClass290;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass290._makeExtraBackupOnSuccess = _makeExtraBackupOnSuccess;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass290._filename = _filename;
    // ISSUE: reference to a compiler-generated field
    if (this.SaveLoad(cDisplayClass290._filename, true, _warnOnDifferentVersion, _infOnDiferentVersion))
    {
      // ISSUE: reference to a compiler-generated field
      WorldState.\u003CLoad\u003Eg__DoExtraBackup\u007C29_0(cDisplayClass290._filename, ref cDisplayClass290);
      return true;
    }
    // ISSUE: reference to a compiler-generated field
    Log.Warning("Failed loading world header file: " + cDisplayClass290._filename);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    SdFile.Copy(cDisplayClass290._filename, cDisplayClass290._filename + ".loadFailed", true);
    // ISSUE: reference to a compiler-generated field
    string str1 = cDisplayClass290._filename + ".bak";
    if (SdFile.Exists(str1))
    {
      Log.Out("Trying backup header: " + str1);
      if (this.SaveLoad(str1, true, _warnOnDifferentVersion, _infOnDiferentVersion))
      {
        WorldState.\u003CLoad\u003Eg__DoExtraBackup\u007C29_0(str1, ref cDisplayClass290);
        return true;
      }
      SdFile.Copy(str1, str1 + ".loadFailed", true);
      Log.Error("Failed loading backup header file!");
    }
    else
      Log.Out("No backup header!");
    // ISSUE: reference to a compiler-generated field
    string str2 = cDisplayClass290._filename + ".ext.bak";
    if (SdFile.Exists(str2))
    {
      Log.Out("Trying extra backup header (from last successful load): " + str2);
      if (this.SaveLoad(str2, true, _warnOnDifferentVersion, _infOnDiferentVersion))
        return true;
      SdFile.Copy(str2, str2 + ".loadFailed", true);
      Log.Error("Failed loading extra backup header file!");
    }
    else
      Log.Out("No extra backup header!");
    return false;
  }

  public bool Save(string _filename)
  {
    if (SdFile.Exists(_filename) && GameIO.FileSize(_filename) > 0L)
      SdFile.Copy(_filename, _filename + ".bak", true);
    return this.SaveLoad(_filename, false, false, false);
  }

  public bool Save(Stream _stream) => this.SaveLoad(_stream, false, false, false);

  public void SetFrom(World _world, EnumChunkProviderId _chunkProviderId)
  {
    this.waterLevel = WorldConstants.WaterLevel;
    this.chunkSizeX = 16 /*0x10*/;
    this.chunkSizeY = 16 /*0x10*/;
    this.chunkSizeZ = 256 /*0x0100*/;
    this.chunkCount = 0;
    this.providerId = _chunkProviderId;
    this.seed = _world.Seed;
    this.worldTime = _world.worldTime;
    this.timeInTicks = GameTimer.Instance.ticks;
    this.sleeperVolumeState = new MemoryStream();
    using (PooledBinaryWriter _bw = MemoryPools.poolBinaryWriter.AllocSync(false))
    {
      _bw.SetBaseStream((Stream) this.sleeperVolumeState);
      _world.WriteSleeperVolumes((BinaryWriter) _bw);
    }
    this.triggerVolumeState = new MemoryStream();
    using (PooledBinaryWriter _bw = MemoryPools.poolBinaryWriter.AllocSync(false))
    {
      _bw.SetBaseStream((Stream) this.triggerVolumeState);
      _world.WriteTriggerVolumes((BinaryWriter) _bw);
    }
    this.wallVolumeState = new MemoryStream();
    using (PooledBinaryWriter _bw = MemoryPools.poolBinaryWriter.AllocSync(false))
    {
      _bw.SetBaseStream((Stream) this.wallVolumeState);
      _world.WriteWallVolumes((BinaryWriter) _bw);
    }
    this.nextEntityID = EntityFactory.nextEntityID;
    this.activeGameMode = _world.GetGameMode();
    this.dynamicSpawnerState = new MemoryStream();
    if (_world.GetDynamiceSpawnManager() != null)
    {
      using (PooledBinaryWriter _bw = MemoryPools.poolBinaryWriter.AllocSync(false))
      {
        _bw.SetBaseStream((Stream) this.dynamicSpawnerState);
        _world.GetDynamiceSpawnManager().Write((BinaryWriter) _bw);
      }
    }
    this.aiDirectorState = new MemoryStream();
    if (_world.aiDirector != null)
    {
      using (PooledBinaryWriter stream = MemoryPools.poolBinaryWriter.AllocSync(false))
      {
        stream.SetBaseStream((Stream) this.aiDirectorState);
        _world.aiDirector.Save((BinaryWriter) stream);
      }
    }
    else
    {
      using (PooledBinaryWriter stream = MemoryPools.poolBinaryWriter.AllocSync(false))
      {
        stream.SetBaseStream((Stream) this.aiDirectorState);
        new AIDirector(_world).Save((BinaryWriter) stream);
      }
    }
  }

  public void ResetDynamicData()
  {
    this.worldTime = 0UL;
    this.timeInTicks = 0UL;
  }

  public void GenerateNewGuid() => this.Guid = Utils.GenerateGuid();

  [PublicizedFrom(EAccessModifier.Private)]
  static WorldState()
  {
  }

  [CompilerGenerated]
  [PublicizedFrom(EAccessModifier.Internal)]
  public static void \u003CLoad\u003Eg__DoExtraBackup\u007C29_0(
    string sourceFilename,
    [In] ref WorldState.\u003C\u003Ec__DisplayClass29_0 obj1)
  {
    // ISSUE: reference to a compiler-generated field
    if (!obj1._makeExtraBackupOnSuccess)
      return;
    // ISSUE: reference to a compiler-generated field
    string destFileName = obj1._filename + ".ext.bak";
    try
    {
      SdFile.Copy(sourceFilename, destFileName, true);
    }
    catch (Exception ex)
    {
      Log.Error($"Failed to make extra backup (due to successfully loading) by copying '{sourceFilename}' to '{destFileName}': {ex}");
    }
  }
}
