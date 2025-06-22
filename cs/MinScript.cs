// Decompiled with JetBrains decompiler
// Type: MinScript
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

#nullable disable
public class MinScript
{
  [PublicizedFrom(EAccessModifier.Private)]
  public const byte cVersion = 1;
  [PublicizedFrom(EAccessModifier.Private)]
  public const char cLineSepChar = '^';
  [PublicizedFrom(EAccessModifier.Private)]
  public const string cLineSepStr = "^";
  [PublicizedFrom(EAccessModifier.Private)]
  public List<MinScript.CmdLine> commandList = new List<MinScript.CmdLine>();
  [PublicizedFrom(EAccessModifier.Private)]
  public int curIndex;
  [PublicizedFrom(EAccessModifier.Private)]
  public float sleep;
  [PublicizedFrom(EAccessModifier.Private)]
  public const ushort cCmdNop = 0;
  [PublicizedFrom(EAccessModifier.Private)]
  public const ushort cCmdLog = 1;
  [PublicizedFrom(EAccessModifier.Private)]
  public const ushort cCmdLabel = 2;
  [PublicizedFrom(EAccessModifier.Private)]
  public const ushort cCmdLoop = 3;
  [PublicizedFrom(EAccessModifier.Private)]
  public const ushort cCmdSleep = 4;
  [PublicizedFrom(EAccessModifier.Private)]
  public const ushort cCmdSound = 40;
  [PublicizedFrom(EAccessModifier.Private)]
  public const ushort cCmdSpawn = 50;
  [PublicizedFrom(EAccessModifier.Private)]
  public const ushort cCmdWaitAlive = 51;
  [PublicizedFrom(EAccessModifier.Private)]
  public const ushort cCmdTrigger = 52;
  [PublicizedFrom(EAccessModifier.Private)]
  public static Dictionary<string, ushort> nameToCmds = new Dictionary<string, ushort>()
  {
    {
      "log",
      (ushort) 1
    },
    {
      "label",
      (ushort) 2
    },
    {
      "loop",
      (ushort) 3
    },
    {
      nameof (sleep),
      (ushort) 4
    },
    {
      "sound",
      (ushort) 40
    },
    {
      "spawn",
      (ushort) 50
    },
    {
      "trigger",
      (ushort) 52
    },
    {
      "waitalive",
      (ushort) 51
    }
  };
  [PublicizedFrom(EAccessModifier.Private)]
  public static byte[] tempBytes = new byte[256 /*0x0100*/];
  [PublicizedFrom(EAccessModifier.Private)]
  public static char[] tempChars = new char[256 /*0x0100*/];
  [PublicizedFrom(EAccessModifier.Private)]
  public EntityPlayer player;
  [PublicizedFrom(EAccessModifier.Private)]
  public float countScale = 1f;

  public static string ConvertFromUIText(string _text) => _text.Replace("\n", "^");

  public static string ConvertToUIText(string _text)
  {
    return _text == null ? string.Empty : _text.Replace("^", "\n");
  }

  public void SetText(string _text)
  {
    int num1 = 0;
    int length1 = _text.Length;
    int num2;
    for (int index = 0; index < length1; index = num2 + 1)
    {
      num2 = _text.IndexOf('^', index, length1 - index);
      if (num2 < 0)
        num2 = length1;
      while (index < length1 && _text[index] == ' ')
        ++index;
      int count = num2 - index;
      if (count > 0 && _text[index] != '/')
      {
        int num3 = _text.IndexOf(' ', index, count);
        if (num3 < 0)
          num3 = num2;
        string key = _text.Substring(index, num3 - index);
        MinScript.CmdLine cmdLine;
        if (MinScript.nameToCmds.TryGetValue(key, out cmdLine.command))
        {
          int startIndex = num3 + 1;
          cmdLine.parameters = (string) null;
          int length2 = num2 - startIndex;
          if (length2 > 0)
            cmdLine.parameters = _text.Substring(startIndex, length2);
          this.commandList.Add(cmdLine);
        }
      }
      ++num1;
    }
  }

  public void Reset() => this.curIndex = -1;

  public void Restart()
  {
    this.curIndex = 0;
    this.sleep = 0.0f;
  }

  public void Run(SleeperVolume _sv, EntityPlayer _player, float _countScale)
  {
    if (this.commandList == null)
      return;
    this.player = _player;
    this.countScale = _countScale;
    this.curIndex = 0;
    this.sleep = 0.0f;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool IsRunning() => this.curIndex >= 0;

  public void Tick(SleeperVolume _sv)
  {
    if (this.curIndex < 0)
      return;
    if ((double) this.sleep > 0.0)
    {
      this.sleep -= 0.05f;
      if ((double) this.sleep > 0.0)
        return;
    }
    do
    {
      MinScript.CmdLine command = this.commandList[this.curIndex];
      switch (command.command)
      {
        case 1:
          Log.Out("MinScript " + command.parameters);
          break;
        case 4:
          this.sleep = float.Parse(command.parameters ?? "1");
          break;
        case 40:
          GameManager.Instance.PlaySoundAtPositionServer(_sv.Center, command.parameters, (AudioRolloffMode) 1, 100, _sv.GetPlayerTouchedToUpdateId());
          break;
        case 50:
          if (command.parameters != null)
          {
            string[] strArray = command.parameters.Split(' ', StringSplitOptions.None);
            if (strArray.Length >= 1)
            {
              float num1 = 1f;
              float num2 = 1f;
              if (strArray.Length >= 2)
              {
                num1 = float.Parse(strArray[1]);
                num2 = num1;
                if (strArray.Length >= 3)
                  num2 = float.Parse(strArray[2]);
              }
              _sv.AddSpawnCount(strArray[0], num1 * this.countScale, num2 * this.countScale);
              break;
            }
            break;
          }
          break;
        case 51:
          int num = int.Parse(command.parameters ?? "0");
          if (_sv.GetAliveCount() > num)
            return;
          break;
        case 52:
          if (Object.op_Implicit((Object) this.player))
          {
            if (command.parameters != null)
            {
              byte trigger = (byte) int.Parse(command.parameters);
              this.player.world.triggerManager.Trigger(this.player, _sv.PrefabInstance, trigger);
              break;
            }
            Log.Warning("MinScript trigger !param {0}", new object[1]
            {
              (object) _sv
            });
            break;
          }
          break;
      }
      if (++this.curIndex >= this.commandList.Count)
      {
        this.curIndex = -1;
        break;
      }
    }
    while ((double) this.sleep <= 0.0);
  }

  public static MinScript Read(BinaryReader _br)
  {
    int num1 = (int) _br.ReadByte();
    MinScript minScript = new MinScript();
    minScript.curIndex = (int) _br.ReadInt16();
    if (minScript.curIndex >= 0)
      minScript.sleep = _br.ReadSingle();
    int num2 = (int) _br.ReadUInt16();
    for (int index = 0; index < num2; ++index)
    {
      MinScript.CmdLine cmdLine;
      cmdLine.command = _br.ReadUInt16();
      cmdLine.parameters = (string) null;
      int num3 = (int) _br.ReadByte();
      if (num3 > 0)
      {
        _br.Read(MinScript.tempBytes, 0, num3);
        int chars = Encoding.UTF8.GetChars(MinScript.tempBytes, 0, num3, MinScript.tempChars, 0);
        cmdLine.parameters = new string(MinScript.tempChars, 0, chars);
      }
      minScript.commandList.Add(cmdLine);
    }
    return minScript;
  }

  public bool HasData() => this.commandList.Count > 0;

  public void Write(BinaryWriter _bw)
  {
    _bw.Write((byte) 1);
    _bw.Write((short) this.curIndex);
    if (this.curIndex >= 0)
      _bw.Write(this.sleep);
    int count = this.commandList.Count;
    _bw.Write((ushort) count);
    for (int index1 = 0; index1 < count; ++index1)
    {
      MinScript.CmdLine command = this.commandList[index1];
      _bw.Write(command.command);
      if (command.parameters != null && command.parameters.Length > 0)
      {
        for (int index2 = 0; index2 < command.parameters.Length; ++index2)
          MinScript.tempChars[index2] = command.parameters[index2];
        byte bytes = (byte) Encoding.UTF8.GetBytes(MinScript.tempChars, 0, command.parameters.Length, MinScript.tempBytes, 0);
        _bw.Write(bytes);
        _bw.Write(MinScript.tempBytes, 0, (int) bytes);
      }
      else
        _bw.Write((byte) 0);
    }
  }

  public override string ToString()
  {
    return $"cmds {this.commandList.Count}, index {this.curIndex}, sleep {this.sleep}";
  }

  [Conditional("DEBUG_MINSCRIPTLOG")]
  [PublicizedFrom(EAccessModifier.Private)]
  public static void LogMS(string format, params object[] args)
  {
    format = $"{GameManager.frameTime.ToCultureInvariantString()} {GameManager.frameCount} MinScript {format}";
    Log.Warning(format, args);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  static MinScript()
  {
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public struct CmdLine
  {
    public ushort command;
    public string parameters;
  }
}
