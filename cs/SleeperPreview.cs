// Decompiled with JetBrains decompiler
// Type: SleeperPreview
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (Animator))]
public class SleeperPreview : MonoBehaviour
{
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public Animator animator;

  [PublicizedFrom(EAccessModifier.Private)]
  public void Awake() => this.animator = ((Component) this).GetComponent<Animator>();

  public void SetPose(int pose)
  {
  }

  public void SetRotation(float rot)
  {
    ((Component) this).transform.rotation = Quaternion.AngleAxis(rot, Vector3.up);
  }
}
