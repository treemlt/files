// Decompiled with JetBrains decompiler
// Type: GameEvent.SequenceActions.ActionResetSleepers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using UnityEngine.Scripting;

#nullable disable
namespace GameEvent.SequenceActions;

[Preserve]
public class ActionResetSleepers : BaseAction
{
  public override BaseAction.ActionCompleteStates OnPerformAction()
  {
    World world = GameManager.Instance.World;
    int sleeperVolumeCount = world.GetSleeperVolumeCount();
    for (int index = 0; index < sleeperVolumeCount; ++index)
      world.GetSleeperVolume(index)?.DespawnAndReset(world);
    Log.Out("Reset {0} sleeper volumes", new object[1]
    {
      (object) sleeperVolumeCount
    });
    return BaseAction.ActionCompleteStates.Complete;
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public override BaseAction CloneChildSettings() => (BaseAction) new ActionResetSleepers();
}
