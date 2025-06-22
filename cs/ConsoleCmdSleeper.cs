// Decompiled with JetBrains decompiler
// Type: ConsoleCmdSleeper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
[Preserve]
public class ConsoleCmdSleeper : ConsoleCmdAbstract
{
  [PublicizedFrom(EAccessModifier.Private)]
  public Coroutine drawVolumesCo;

  [PublicizedFrom(EAccessModifier.Protected)]
  public override string[] getCommands()
  {
    return new string[1]{ "sleeper" };
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public override string getDescription() => "Drawn or list sleeper info";

  [PublicizedFrom(EAccessModifier.Protected)]
  public override string getHelp()
  {
    return "draw - toggle drawing for current player prefab\nlist - list for current player prefab\nlistall - list all\nr - reset all";
  }

  public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
  {
    if (_params.Count == 0)
    {
      SingletonMonoBehaviour<SdtdConsole>.Instance.Output(this.GetHelp());
    }
    else
    {
      switch (_params[0].ToLower())
      {
        case "draw":
          if (this.drawVolumesCo != null)
          {
            GameManager.Instance.StopCoroutine(this.drawVolumesCo);
            this.drawVolumesCo = (Coroutine) null;
            break;
          }
          this.drawVolumesCo = GameManager.Instance.StartCoroutine(this.DrawVolumes());
          break;
        case "listall":
          this.LogInfo(false);
          break;
        case "list":
          this.LogInfo(true);
          break;
        case "r":
          this.Reset();
          break;
        default:
          SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Command not recognized. <end/>");
          break;
      }
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void LogInfo(bool onlyPlayer)
  {
    World world = GameManager.Instance.World;
    if (world == null)
      return;
    EntityPlayerLocal primaryPlayer = onlyPlayer ? world.GetPrimaryPlayer() : (EntityPlayerLocal) null;
    int sleeperVolumeCount = world.GetSleeperVolumeCount();
    int num = 0;
    for (int index = 0; index < sleeperVolumeCount; ++index)
    {
      SleeperVolume sleeperVolume = world.GetSleeperVolume(index);
      if (Object.op_Implicit((Object) primaryPlayer))
      {
        if (sleeperVolume.PrefabInstance == primaryPlayer.prefab)
          sleeperVolume.Draw(3f);
        else
          continue;
      }
      ++num;
      this.Print("#{0} {1}", (object) index, (object) sleeperVolume.GetDescription());
    }
    this.Print("Sleeper volumes {0} of {1}", (object) num, (object) sleeperVolumeCount);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public IEnumerator DrawVolumes()
  {
    for (int n = 0; n < 99999; ++n)
    {
      World world = GameManager.Instance.World;
      if (world != null)
      {
        EntityPlayerLocal primaryPlayer = world.GetPrimaryPlayer();
        if (Object.op_Implicit((Object) primaryPlayer))
        {
          int sleeperVolumeCount = world.GetSleeperVolumeCount();
          for (int index = 0; index < sleeperVolumeCount; ++index)
          {
            SleeperVolume sleeperVolume = world.GetSleeperVolume(index);
            if (sleeperVolume.PrefabInstance == primaryPlayer.prefab)
              sleeperVolume.DrawDebugLines(1f);
          }
          int triggerVolumeCount = world.GetTriggerVolumeCount();
          for (int index = 0; index < triggerVolumeCount; ++index)
          {
            TriggerVolume triggerVolume = world.GetTriggerVolume(index);
            if (triggerVolume.PrefabInstance == primaryPlayer.prefab)
              triggerVolume.DrawDebugLines(1f);
          }
          yield return (object) new WaitForSeconds(0.5f);
        }
        else
          break;
      }
      else
        break;
    }
    this.drawVolumesCo = (Coroutine) null;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void Reset()
  {
    World world = GameManager.Instance.World;
    if (world == null)
      return;
    int sleeperVolumeCount = world.GetSleeperVolumeCount();
    for (int index = 0; index < sleeperVolumeCount; ++index)
      world.GetSleeperVolume(index)?.DespawnAndReset(world);
    this.Print("Reset {0}", (object) sleeperVolumeCount);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void Print(string _s, params object[] _values)
  {
    string _line = string.Format(_s, _values);
    SingletonMonoBehaviour<SdtdConsole>.Instance.Output(_line);
  }
}
