// Decompiled with JetBrains decompiler
// Type: AvatarZombieController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
[Preserve]
public class AvatarZombieController : AvatarController
{
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public const int cOverrideLayerIndex = 1;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public const int cFullBodyLayerIndex = 2;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public const int cHitLayerIndex = 3;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public AvatarRootMotion rootMotion;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public Transform modelT;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public Transform bipedT;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public Transform rightHandT;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public AnimatorStateInfo baseStateInfo;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public AnimatorStateInfo overrideStateInfo;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public AnimatorStateInfo fullBodyStateInfo;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public AnimatorStateInfo hitStateInfo;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public bool isSuppressPain;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public bool isCrippled;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public bool isCrawler;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public bool isVisibleInit;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public bool isVisible;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public float idleTime;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public float crawlerTime;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public float actionTimeActive;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public float attackPlayingTime;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public bool isAttackImpact;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public float timeSpecialAttack2Playing;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public float timeRagePlaying;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int jumpState;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool isJumpStarted;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool isEating;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public int movementStateOverride = -1;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public bool headDismembered;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public bool leftUpperArmDismembered;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public bool leftLowerArmDismembered;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public bool rightUpperArmDismembered;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public bool rightLowerArmDismembered;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public bool leftUpperLegDismembered;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public bool leftLowerLegDismembered;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public bool rightUpperLegDismembered;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public bool rightLowerLegDismembered;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public bool isInDeathAnim;
  [PublicizedFrom(EAccessModifier.Protected)]
  [NonSerialized]
  public bool didDeathTransition;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public Material mainZombieMaterial;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public Material mainZombieMaterialCopy;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public Material gibCapMaterial;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public Material gibCapMaterialCopy;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public Material dismemberMat;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public SkinnedMeshRenderer skinnedMeshRenderer;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public SkinnedMeshRenderer smrLODOne;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public SkinnedMeshRenderer smrLODTwo;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public string rootDismmemberDir;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public string subFolderDismemberEntityName;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int altEntityMatId = -1;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public string altMatName;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public List<DismemberedPart> dismemberedParts = new List<DismemberedPart>();
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool isCensored;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public const string cEmissiveColor = "_EmissiveColor";
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public Vector3 defaultHeadPos;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public const string cElectrocuteKeyword = "_ELECTRIC_SHOCK_ON";

  [PublicizedFrom(EAccessModifier.Protected)]
  public override void Awake()
  {
    base.Awake();
    this.modelT = EModelBase.FindModel(((Component) this).transform);
    this.assignStates();
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public void Start() => this.hitLayerIndex = 3;

  public override void SwitchModelAndView(string _modelName, bool _bFPV, bool _bMale)
  {
    if (!Object.op_Implicit((Object) this.bipedT))
    {
      this.bipedT = this.entity.emodel.GetModelTransform();
      this.rightHandT = this.FindTransform(this.entity.GetRightHandTransformName());
      this.SetAnimator(this.bipedT);
      if (this.entity.RootMotion)
      {
        this.rootMotion = ((Component) this.bipedT).gameObject.AddComponent<AvatarRootMotion>();
        this.rootMotion.Init((AvatarController) this, this.anim);
      }
    }
    this.SetWalkType(this.entity.GetWalkType());
    this._setBool(AvatarController.isDeadHash, this.entity.IsDead());
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public Transform FindTransform(string _name) => this.bipedT.FindInChildren(_name);

  public override void SetVisible(bool _b)
  {
    if (this.isVisible == _b && this.isVisibleInit)
      return;
    this.isVisible = _b;
    this.isVisibleInit = true;
    Transform bipedT = this.bipedT;
    if (!Object.op_Implicit((Object) bipedT))
      return;
    foreach (Renderer componentsInChild in ((Component) bipedT).GetComponentsInChildren<Renderer>(true))
      componentsInChild.enabled = _b;
  }

  public override Transform GetActiveModelRoot() => this.modelT;

  public override Transform GetRightHandTransform() => this.rightHandT;

  public override void SetInRightHand(Transform _transform)
  {
    this.idleTime = 0.0f;
    if (!Object.op_Implicit((Object) _transform))
      return;
    Quaternion identity = Quaternion.identity;
    _transform.SetParent(this.GetRightHandTransform(), false);
    if (this.entity.inventory != null && this.entity.inventory.holdingItem != null)
    {
      AnimationGunjointOffsetData.AnimationGunjointOffsets animationGunjointOffsets = AnimationGunjointOffsetData.AnimationGunjointOffset[this.entity.inventory.holdingItem.HoldType.Value];
      _transform.localPosition = animationGunjointOffsets.position;
      _transform.localRotation = Quaternion.Euler(animationGunjointOffsets.rotation);
    }
    else
    {
      _transform.localPosition = Vector3.zero;
      _transform.localRotation = identity;
    }
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public override void Update()
  {
    base.Update();
    float deltaTime = Time.deltaTime;
    if ((double) this.actionTimeActive > 0.0)
      this.actionTimeActive -= deltaTime;
    if ((double) this.attackPlayingTime > 0.0)
    {
      this.attackPlayingTime -= deltaTime;
      if ((double) this.attackPlayingTime <= 0.0)
        this.isAttackImpact = true;
    }
    if ((double) this.timeSpecialAttack2Playing > 0.0)
      this.timeSpecialAttack2Playing -= deltaTime;
    if ((double) this.timeRagePlaying > 0.0)
      this.timeRagePlaying -= deltaTime;
    if (!this.isVisible && (!Object.op_Implicit((Object) this.entity) || !this.entity.RootMotion || this.entity.isEntityRemote) || !Object.op_Implicit((Object) this.bipedT) || !((Component) this.bipedT).gameObject.activeInHierarchy || !Object.op_Implicit((Object) this.anim) || !this.anim.avatar.isValid || !((Behaviour) this.anim).enabled)
      return;
    this.UpdateLayerStateInfo();
    this.SetLayerWeights();
    float speedForward = this.entity.speedForward;
    this._setFloat(AvatarController.forwardHash, speedForward, false);
    if (!this.entity.IsDead())
    {
      if (this.movementStateOverride != -1)
      {
        this._setInt(AvatarController.movementStateHash, this.movementStateOverride, true);
        this.movementStateOverride = -1;
      }
      else
      {
        float num = speedForward * speedForward;
        this._setInt(AvatarController.movementStateHash, (double) num > (double) this.entity.moveSpeedAggro * (double) this.entity.moveSpeedAggro ? 3 : ((double) num > (double) this.entity.moveSpeed * (double) this.entity.moveSpeed ? 2 : ((double) num > 1.0 / 1000.0 ? 1 : 0)), false);
      }
    }
    if ((double) this.electrocuteTime > 0.30000001192092896 && !this.entity.emodel.IsRagdollActive)
      this._setTrigger(AvatarController.isElectrocutedHash);
    if (!((Component) this.bipedT).gameObject.activeInHierarchy)
      return;
    if (this.entity.IsInElevator() || this.entity.Climbing)
      this._setBool(AvatarController.isClimbingHash, true);
    else
      this._setBool(AvatarController.isClimbingHash, false);
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public void LateUpdate()
  {
    if (!Object.op_Implicit((Object) this.entity) || !Object.op_Implicit((Object) this.bipedT) || !((Component) this.bipedT).gameObject.activeInHierarchy || !Object.op_Implicit((Object) this.anim) || !((Behaviour) this.anim).enabled)
      return;
    this.UpdateLayerStateInfo();
    ItemClass holdingItem = this.entity.inventory.holdingItem;
    if (holdingItem.Actions[0] != null)
      holdingItem.Actions[0].UpdateNozzleParticlesPosAndRot(this.entity.inventory.holdingItemData.actionData[0]);
    if (holdingItem.Actions[1] != null)
      holdingItem.Actions[1].UpdateNozzleParticlesPosAndRot(this.entity.inventory.holdingItemData.actionData[1]);
    int fullPathHash = ((AnimatorStateInfo) ref this.baseStateInfo).fullPathHash;
    bool flag = this.anim.IsInTransition(0);
    if (!flag)
    {
      this.isJumpStarted = false;
      if (fullPathHash == this.jumpState)
        this._setBool(AvatarController.jumpHash, false);
    }
    if (this.isInDeathAnim)
    {
      if (((AnimatorStateInfo) ref this.baseStateInfo).tagHash == AvatarController.deathHash && (double) ((AnimatorStateInfo) ref this.baseStateInfo).normalizedTime >= 1.0 && !flag)
      {
        this.isInDeathAnim = false;
        if (this.entity.HasDeathAnim)
          this.entity.emodel.DoRagdoll(DamageResponse.New(true));
      }
      if (this.entity.HasDeathAnim && this.entity.RootMotion && this.entity.isCollidedHorizontally)
      {
        this.isInDeathAnim = false;
        this.entity.emodel.DoRagdoll(DamageResponse.New(true));
      }
    }
    if (!this.isCrawler || (double) Time.time - (double) this.crawlerTime <= 2.0)
      return;
    this.isSuppressPain = false;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void UpdateLayerStateInfo()
  {
    this.baseStateInfo = this.anim.GetCurrentAnimatorStateInfo(0);
    this.overrideStateInfo = this.anim.GetCurrentAnimatorStateInfo(1);
    this.fullBodyStateInfo = this.anim.GetCurrentAnimatorStateInfo(2);
    if (this.anim.layerCount <= 3)
      return;
    this.hitStateInfo = this.anim.GetCurrentAnimatorStateInfo(3);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void SetLayerWeights()
  {
    this.isSuppressPain = this.isSuppressPain && (this.anim.IsInTransition(2) || ((AnimatorStateInfo) ref this.fullBodyStateInfo).fullPathHash != 0);
    this.anim.SetLayerWeight(1, 1f);
    this.anim.SetLayerWeight(2, this.isSuppressPain || this.entity.bodyDamage.CurrentStun != EnumEntityStunType.None ? 0.0f : 1f);
  }

  public override void ResetAnimations()
  {
    base.ResetAnimations();
    this.anim.Play("None", 1, 0.0f);
    this.anim.Play("None", 2, 0.0f);
  }

  public override void SetMeleeAttackSpeed(float _speed)
  {
  }

  public override AvatarController.ActionState GetActionState()
  {
    if ((double) this.attackPlayingTime > 0.0 || ((AnimatorStateInfo) ref this.overrideStateInfo).tagHash == AvatarController.attackHash)
      return AvatarController.ActionState.Active;
    int tagHash = ((AnimatorStateInfo) ref this.fullBodyStateInfo).tagHash;
    if (tagHash == AvatarController.attackStartHash || (double) this.actionTimeActive > 0.0)
      return AvatarController.ActionState.Start;
    if (tagHash == AvatarController.attackReadyHash)
      return AvatarController.ActionState.Ready;
    return tagHash == AvatarController.attackHash ? AvatarController.ActionState.Active : AvatarController.ActionState.None;
  }

  public override bool IsActionActive() => this.GetActionState() != 0;

  public override void StartAction(int _animType)
  {
    if (_animType < 3000)
    {
      this.StartAnimationAttack();
    }
    else
    {
      this.idleTime = 0.0f;
      this._setInt(AvatarController.attackHash, _animType, true);
      this._setTrigger(AvatarController.attackTriggerHash);
      this.actionTimeActive = 0.2f;
    }
  }

  public override bool IsAnimationAttackPlaying()
  {
    return (double) this.attackPlayingTime > 0.0 || ((AnimatorStateInfo) ref this.overrideStateInfo).tagHash == AvatarController.attackHash || ((AnimatorStateInfo) ref this.fullBodyStateInfo).tagHash == AvatarController.attackHash;
  }

  public override void StartAnimationAttack()
  {
    if (!((Component) this.bipedT).gameObject.activeInHierarchy)
      return;
    this.idleTime = 0.0f;
    this.isAttackImpact = false;
    this.attackPlayingTime = 2f;
    float randomFloat = this.entity.rand.RandomFloat;
    int num1 = -1;
    if (!this.rightArmDismembered)
    {
      num1 = 0;
      if (!this.leftArmDismembered)
        num1 = this.entity.rand.RandomInt & 1;
    }
    else if (!this.leftArmDismembered)
      num1 = 1;
    int num2 = 8;
    if (num1 >= 0)
      num2 = num1;
    int walkType = this.entity.GetWalkType();
    if (walkType >= 20)
      num2 += walkType * 100;
    if (this.entity.IsBreakingDoors && num1 >= 0)
      num2 += 10;
    if (num2 <= 1)
    {
      if (walkType == 1)
        num2 += 100;
      else if ((double) this.entity.rand.RandomFloat < 0.25)
        num2 += 4;
    }
    this._setInt(AvatarController.attackHash, num2, true);
    this._setFloat(AvatarController.attackBlendHash, randomFloat);
    this._setTrigger(AvatarController.attackTriggerHash);
  }

  public override void SetAttackImpact()
  {
    if (this.isAttackImpact)
      return;
    this.isAttackImpact = true;
    this.attackPlayingTime = 0.1f;
  }

  public override bool IsAttackImpact() => this.isAttackImpact;

  public override bool IsAnimationHitRunning()
  {
    if ((double) this.hitWeight == 0.0)
      return false;
    int tagHash = ((AnimatorStateInfo) ref this.hitStateInfo).tagHash;
    return tagHash == AvatarController.hitStartHash || tagHash == AvatarController.hitHash && (double) ((AnimatorStateInfo) ref this.hitStateInfo).normalizedTime < 0.550000011920929 || this.anim.IsInTransition(3);
  }

  public override bool IsAnimationSpecialAttackPlaying() => false;

  public override void StartAnimationSpecialAttack(bool _b, int _animType)
  {
  }

  public override bool IsAnimationSpecialAttack2Playing()
  {
    return (double) this.timeSpecialAttack2Playing > 0.0;
  }

  public override void StartAnimationSpecialAttack2()
  {
    this.idleTime = 0.0f;
    this.timeSpecialAttack2Playing = 0.3f;
    this._setTrigger(AvatarController.specialAttack2Hash);
  }

  public override bool IsAnimationRagingPlaying() => (double) this.timeRagePlaying > 0.0;

  public override void StartAnimationRaging()
  {
    this.idleTime = 0.0f;
    this._setTrigger(AvatarController.rageHash);
    this.timeRagePlaying = 0.3f;
  }

  public override void StartAnimationElectrocute(float _duration)
  {
    base.StartAnimationElectrocute(_duration);
    this.idleTime = 0.0f;
  }

  public override bool IsAnimationDigRunning()
  {
    return AvatarController.digHash == ((AnimatorStateInfo) ref this.baseStateInfo).tagHash;
  }

  public override void StartAnimationDodge(float _blend)
  {
    this._setFloat(AvatarController.dodgeBlendHash, _blend);
    this._setBool(AvatarController.dodgeTriggerHash, true);
  }

  public override void StartAnimationJumping()
  {
    this.idleTime = 0.0f;
    if (Object.op_Equality((Object) this.bipedT, (Object) null) || !((Component) this.bipedT).gameObject.activeInHierarchy || !Object.op_Inequality((Object) this.anim, (Object) null))
      return;
    this._setBool(AvatarController.jumpHash, true);
  }

  public override void StartAnimationJump(AnimJumpMode jumpMode)
  {
    this.idleTime = 0.0f;
    if (Object.op_Equality((Object) this.bipedT, (Object) null) || !((Component) this.bipedT).gameObject.activeInHierarchy)
      return;
    this.isJumpStarted = true;
    if (!Object.op_Inequality((Object) this.anim, (Object) null))
      return;
    if (jumpMode == AnimJumpMode.Start)
    {
      this._setTrigger(AvatarController.jumpStartHash);
    }
    else
    {
      this._setTrigger(AvatarController.jumpLandHash);
      this._setInt(AvatarController.jumpLandResponseHash, 0, true);
    }
  }

  public override bool IsAnimationJumpRunning()
  {
    return this.isJumpStarted || AvatarController.jumpHash == ((AnimatorStateInfo) ref this.baseStateInfo).tagHash;
  }

  public override bool IsAnimationWithMotionRunning()
  {
    int tagHash = ((AnimatorStateInfo) ref this.baseStateInfo).tagHash;
    return tagHash == AvatarController.jumpHash || tagHash == AvatarController.moveHash;
  }

  public override void SetSwim(bool _enable)
  {
    int _walkType = -1;
    if (!_enable)
      _walkType = this.entity.GetWalkType();
    else
      this._setFloat(AvatarController.swimSelectHash, this.entity.rand.RandomFloat);
    this.SetWalkType(_walkType, true);
  }

  public override void BeginStun(
    EnumEntityStunType stun,
    EnumBodyPartHit _bodyPart,
    Utils.EnumHitDirection _hitDirection,
    bool _criticalHit,
    float random)
  {
    this._setInt(AvatarController.stunTypeHash, (int) stun, true);
    this._setInt(AvatarController.stunBodyPartHash, (int) _bodyPart.ToPrimary().LowerToUpperLimb(), true);
    this._setInt(AvatarController.hitDirectionHash, (int) _hitDirection, true);
    this._setFloat(AvatarController.HitRandomValueHash, random);
    this._setTrigger(AvatarController.beginStunTriggerHash);
    this._resetTrigger(AvatarController.endStunTriggerHash);
  }

  public override void EndStun() => this._setTrigger(AvatarController.endStunTriggerHash);

  public override bool IsAnimationStunRunning()
  {
    return ((AnimatorStateInfo) ref this.baseStateInfo).tagHash == AvatarController.stunHash;
  }

  public override void StartDeathAnimation(
    EnumBodyPartHit _bodyPart,
    int _movementState,
    float random)
  {
    this.idleTime = 0.0f;
    this.isInDeathAnim = true;
    this.didDeathTransition = false;
    if (Object.op_Equality((Object) this.bipedT, (Object) null) || !((Component) this.bipedT).gameObject.activeInHierarchy)
      return;
    if (Object.op_Inequality((Object) this.anim, (Object) null))
    {
      this.movementStateOverride = _movementState;
      this._setInt(AvatarController.movementStateHash, _movementState, true);
      this._setBool(AvatarController.isAliveHash, false);
      this._setInt(AvatarController.hitBodyPartHash, (int) _bodyPart.ToPrimary().LowerToUpperLimb(), true);
      this._setFloat(AvatarController.HitRandomValueHash, random);
      this.SetFallAndGround(false, this.entity.onGround);
    }
    if (Object.op_Equality((Object) this.bipedT, (Object) null) || !((Component) this.bipedT).gameObject.activeInHierarchy || !Object.op_Inequality((Object) this.anim, (Object) null))
      return;
    this._setTrigger(AvatarController.deathTriggerHash);
  }

  public override void StartEating()
  {
    if (this.isEating)
      return;
    this._setInt(AvatarController.attackHash, 0, true);
    this._setTrigger(AvatarController.beginCorpseEatHash);
    this.isEating = true;
  }

  public override void StopEating()
  {
    if (!this.isEating)
      return;
    this._setInt(AvatarController.attackHash, 0, true);
    this._setTrigger(AvatarController.endCorpseEatHash);
    this.isEating = false;
  }

  public override void StartAnimationHit(
    EnumBodyPartHit _bodyPart,
    int _dir,
    int _hitDamage,
    bool _criticalHit,
    int _movementState,
    float _random,
    float _duration)
  {
    if (this.isCrawler && (double) Time.time - (double) this.crawlerTime <= 2.0)
      return;
    this.InternalStartAnimationHit(_bodyPart, _dir, _hitDamage, _criticalHit, _movementState, _random, _duration);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void InternalStartAnimationHit(
    EnumBodyPartHit _bodyPart,
    int _dir,
    int _hitDamage,
    bool _criticalHit,
    int _movementState,
    float random,
    float _duration)
  {
    if (Object.op_Equality((Object) this.bipedT, (Object) null) || !((Component) this.bipedT).gameObject.activeInHierarchy)
      return;
    if (!this.CheckHit(_duration))
    {
      this.SetDataFloat(AvatarController.DataTypes.HitDuration, _duration);
    }
    else
    {
      this.idleTime = 0.0f;
      if (!Object.op_Implicit((Object) this.anim))
        return;
      this.movementStateOverride = _movementState;
      this._setInt(AvatarController.movementStateHash, _movementState, true);
      this._setInt(AvatarController.hitDirectionHash, _dir, true);
      this._setInt(AvatarController.hitDamageHash, _hitDamage, true);
      this._setFloat(AvatarController.HitRandomValueHash, random);
      this._setInt(AvatarController.hitBodyPartHash, (int) _bodyPart.ToPrimary().LowerToUpperLimb(), true);
      this.SetDataFloat(AvatarController.DataTypes.HitDuration, _duration);
      this._setTrigger(AvatarController.hitTriggerHash);
    }
  }

  public bool IsCrippled => this.isCrippled;

  public override void CrippleLimb(BodyDamage _bodyDamage, bool restoreState)
  {
    if (this.isCrippled || !_bodyDamage.bodyPartHit.IsLeg())
      return;
    int walkType = this.entity.GetWalkType();
    if (walkType == 5 || walkType >= 20)
      return;
    this.isCrippled = true;
    this.SetWalkType(5);
    this._setTrigger(AvatarController.movementTriggerHash);
  }

  public bool rightArmDismembered
  {
    [PublicizedFrom(EAccessModifier.Private)] get
    {
      return this.rightUpperArmDismembered || this.rightLowerArmDismembered;
    }
  }

  public bool leftArmDismembered
  {
    [PublicizedFrom(EAccessModifier.Private)] get
    {
      return this.leftUpperArmDismembered || this.leftLowerArmDismembered;
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public bool isCensoredContent()
  {
    EntityClass entityClass1 = this.entity.EntityClass;
    if ((entityClass1 != null ? (entityClass1.censorMode != 0 ? 1 : 0) : 1) == 0 || !GameManager.Instance.IsGoreCensored())
      return false;
    EntityClass entityClass2 = this.entity.EntityClass;
    if ((entityClass2 != null ? (entityClass2.censorType == 2 ? 1 : 0) : 0) != 0)
      return true;
    EntityClass entityClass3 = this.entity.EntityClass;
    return entityClass3 != null && entityClass3.censorType == 3;
  }

  public void CleanupDismemberedLimbs()
  {
    for (int index = 0; index < this.dismemberedParts.Count; ++index)
      this.dismemberedParts[index].ReadyForCleanup = true;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void _InitDismembermentMaterials()
  {
    if (Object.op_Implicit((Object) this.mainZombieMaterial))
      return;
    EModelBase emodel = this.entity.emodel;
    if (Object.op_Implicit((Object) emodel))
    {
      Transform meshTransform = emodel.meshTransform;
      if (Object.op_Implicit((Object) meshTransform))
      {
        Renderer component = ((Component) meshTransform).GetComponent<Renderer>();
        if (Object.op_Implicit((Object) component))
        {
          this.mainZombieMaterial = component.sharedMaterial;
          bool flag = this.entity.HasAnyTags(DismembermentManager.radiatedTag) && (this.mainZombieMaterial.HasProperty("_IsRadiated") || this.mainZombieMaterial.HasProperty("_Irradiated"));
          DismembermentManager instance = DismembermentManager.Instance;
          this.gibCapMaterial = !flag ? instance.GibCapsMaterial : instance.GibCapsRadMaterial;
        }
      }
    }
    this.isCensored = this.isCensoredContent();
  }

  public override void RemoveLimb(BodyDamage _bodyDamage, bool restoreState)
  {
    if (DismembermentManager.DebugDontCreateParts)
      return;
    DismembermentManager instance = DismembermentManager.Instance;
    int count = instance != null ? instance.parts.Count : 0;
    if (this.entity.isDisintegrated && count >= 25)
      return;
    this._InitDismembermentMaterials();
    EnumBodyPartHit bodyPartHit = _bodyDamage.bodyPartHit;
    EnumDamageTypes damageType = _bodyDamage.damageType;
    bool flag = damageType == EnumDamageTypes.Heat;
    if (this.isCensored)
      damageType = !DismembermentManager.BluntCensors.Contains(this.entity.EntityClass?.entityClassName) ? EnumDamageTypes.Piercing : EnumDamageTypes.Bashing;
    int num1 = 0;
    if (!this.headDismembered && (bodyPartHit & EnumBodyPartHit.Head) > EnumBodyPartHit.None)
    {
      this.headDismembered = true;
      ++num1;
      if (flag || (double) this.entity.OverrideHeadSize != 1.0)
        damageType = EnumDamageTypes.Piercing;
      Transform transform1 = this.FindTransform("Neck");
      this.MakeDismemberedPart(1U, damageType, transform1, restoreState);
      Transform transform2 = this.bipedT.Find("HeadAccessories");
      if (Object.op_Implicit((Object) transform2))
        ((Component) transform2).gameObject.SetActive(false);
    }
    if (!this.leftUpperLegDismembered && (bodyPartHit & EnumBodyPartHit.LeftUpperLeg) > EnumBodyPartHit.None)
    {
      this.leftUpperLegDismembered = true;
      ++num1;
      Transform transform = this.FindTransform("LeftUpLeg");
      this.MakeDismemberedPart(32U /*0x20*/, damageType, transform, restoreState);
    }
    if (!this.leftLowerLegDismembered && !this.leftUpperLegDismembered && (bodyPartHit & EnumBodyPartHit.LeftLowerLeg) > EnumBodyPartHit.None)
    {
      this.leftLowerLegDismembered = true;
      ++num1;
      Transform transform = this.FindTransform("LeftLeg");
      this.MakeDismemberedPart(64U /*0x40*/, damageType, transform, restoreState);
    }
    if (!this.rightUpperLegDismembered && (bodyPartHit & EnumBodyPartHit.RightUpperLeg) > EnumBodyPartHit.None)
    {
      this.rightUpperLegDismembered = true;
      ++num1;
      Transform transform = this.FindTransform("RightUpLeg");
      this.MakeDismemberedPart(128U /*0x80*/, damageType, transform, restoreState);
    }
    if (!this.rightLowerLegDismembered && !this.rightUpperLegDismembered && (bodyPartHit & EnumBodyPartHit.RightLowerLeg) > EnumBodyPartHit.None)
    {
      this.rightLowerLegDismembered = true;
      ++num1;
      Transform transform = this.FindTransform("RightLeg");
      this.MakeDismemberedPart(256U /*0x0100*/, damageType, transform, restoreState);
    }
    if (!this.leftUpperArmDismembered && (bodyPartHit & EnumBodyPartHit.LeftUpperArm) > EnumBodyPartHit.None)
    {
      this.leftUpperArmDismembered = true;
      ++num1;
      Transform transform = this.FindTransform("LeftArm");
      this.MakeDismemberedPart(2U, damageType, transform, restoreState);
    }
    if (!this.leftLowerArmDismembered && !this.leftUpperArmDismembered && (bodyPartHit & EnumBodyPartHit.LeftLowerArm) > EnumBodyPartHit.None)
    {
      this.leftLowerArmDismembered = true;
      ++num1;
      Transform transform = this.FindTransform("LeftForeArm");
      this.MakeDismemberedPart(4U, damageType, transform, restoreState);
    }
    if (!this.rightUpperArmDismembered && (bodyPartHit & EnumBodyPartHit.RightUpperArm) > EnumBodyPartHit.None)
    {
      this.rightUpperArmDismembered = true;
      ++num1;
      Transform transform = this.FindTransform("RightArm");
      this.MakeDismemberedPart(8U, damageType, transform, restoreState);
    }
    if (this.rightLowerArmDismembered || this.rightUpperArmDismembered || (bodyPartHit & EnumBodyPartHit.RightLowerArm) <= EnumBodyPartHit.None)
      return;
    this.rightLowerArmDismembered = true;
    int num2 = num1 + 1;
    Transform transform3 = this.FindTransform("RightForeArm");
    this.MakeDismemberedPart(16U /*0x10*/, damageType, transform3, restoreState);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public Transform SpawnLimbGore(Transform parent, string path, bool restoreState)
  {
    if (!Object.op_Implicit((Object) parent) || string.IsNullOrEmpty(path))
      return (Transform) null;
    string _uri1 = DismembermentManager.GetAssetBundlePath(path);
    GameObject gameObject1 = (GameObject) null;
    if (this.isCensored)
    {
      string _uri2 = _uri1.Replace(".", "_CGore.");
      GameObject gameObject2 = DataLoader.LoadAsset<GameObject>(_uri2);
      if (Object.op_Implicit((Object) gameObject2))
      {
        _uri1 = _uri2;
        gameObject1 = gameObject2;
      }
    }
    if (!Object.op_Implicit((Object) gameObject1))
      gameObject1 = DataLoader.LoadAsset<GameObject>(_uri1);
    if (!Object.op_Implicit((Object) gameObject1))
      return (Transform) null;
    GameObject gameObject3 = Object.Instantiate<GameObject>(gameObject1, parent);
    GorePrefab component = gameObject3.GetComponent<GorePrefab>();
    if (Object.op_Implicit((Object) component))
      component.restoreState = restoreState;
    return gameObject3.transform;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void ProcDismemberedPart(
    Transform t,
    Transform partT,
    DismemberedPartData part,
    uint bodyDamageFlag)
  {
    Transform recursive1 = partT.FindRecursive(part.targetBone);
    if (Object.op_Implicit((Object) recursive1))
    {
      if (!part.attachToParent)
      {
        Vector3 localScale = t.localScale;
        localScale.x /= Utils.FastMax(0.01f, recursive1.localScale.x);
        localScale.y /= Utils.FastMax(0.01f, recursive1.localScale.y);
        localScale.z /= Utils.FastMax(0.01f, recursive1.localScale.z);
        t.localScale = localScale;
      }
      if (!string.IsNullOrEmpty(part.childTargetObj))
      {
        Transform transform = new GameObject("scaleTarget").transform;
        transform.position = recursive1.position;
        for (int index = 0; index < recursive1.childCount; ++index)
          recursive1.GetChild(index).SetParent(transform);
        transform.SetParent(recursive1.parent);
        recursive1.SetParent(transform);
        transform.localScale = Vector3.zero;
      }
      if (!string.IsNullOrEmpty(part.insertBoneObj))
      {
        Transform transform = new GameObject("scaleTarget").transform;
        transform.position = recursive1.position;
        for (int index = 0; index < recursive1.childCount; ++index)
          recursive1.GetChild(index).SetParent(transform);
        transform.SetParent(recursive1);
        if (Vector3.op_Inequality(this.defaultHeadPos, Vector3.zero))
        {
          transform.position = this.defaultHeadPos;
          Vector3 localPosition = transform.localPosition;
          localPosition.z = (float) (-(double) localPosition.y * 0.5);
          transform.localPosition = localPosition;
        }
        transform.localScale = Vector3.zero;
      }
    }
    if (part.hasRotOffset)
      t.localEulerAngles = part.rot;
    if (DismembermentManager.DebugShowArmRotations)
      DismembermentManager.AddDebugArmObjects(partT, t);
    if (Vector3.op_Inequality(part.offset, Vector3.zero))
    {
      Transform recursive2 = t.FindRecursive("pos");
      if (Object.op_Implicit((Object) recursive2))
      {
        Transform transform = recursive2;
        transform.localPosition = Vector3.op_Addition(transform.localPosition, part.offset);
      }
    }
    if (part.particlePaths != null)
    {
      for (int index = 0; index < part.particlePaths.Length; ++index)
      {
        string particlePath = part.particlePaths[index];
        if (!string.IsNullOrEmpty(particlePath))
          DismembermentManager.SpawnParticleEffect(new ParticleEffect(particlePath, Vector3.op_Addition(t.position, Origin.position), Quaternion.identity, 1f, Color.white));
      }
    }
    Transform recursive3 = t.FindRecursive("pos");
    if (Object.op_Implicit((Object) recursive3))
    {
      Renderer[] componentsInChildren = ((Component) recursive3).GetComponentsInChildren<Renderer>(true);
      Material altMaterial = this.entity.emodel.AltMaterial;
      if (Object.op_Implicit((Object) altMaterial))
      {
        this.altMatName = ((Object) altMaterial).name;
        for (int index = 0; index < this.altMatName.Length; ++index)
        {
          char c = this.altMatName[index];
          if (char.IsDigit(c))
          {
            this.altEntityMatId = int.Parse(c.ToString());
            break;
          }
        }
      }
      else
      {
        foreach (char c in ((Object) this.mainZombieMaterial).name)
        {
          if (char.IsDigit(c))
          {
            this.altEntityMatId = int.Parse(c.ToString());
            break;
          }
        }
      }
      for (int index1 = 0; index1 < componentsInChildren.Length; ++index1)
      {
        Renderer renderer = componentsInChildren[index1];
        if (!Object.op_Implicit((Object) ((Component) renderer).GetComponent<ParticleSystem>()))
        {
          Material[] sharedMaterials = renderer.sharedMaterials;
          for (int index2 = 0; index2 < sharedMaterials.Length; ++index2)
          {
            Material material = sharedMaterials[index2];
            string name = ((Object) material).name;
            if ((!part.prefabPath.ContainsCaseInsensitive("head") || !name.ContainsCaseInsensitive("hair")) && (!((Object) renderer).name.ContainsCaseInsensitive("eye") || material.HasProperty("_IsRadiated") || material.HasProperty("_Irradiated")))
            {
              bool flag = false;
              for (int index3 = 0; index3 < DismembermentManager.DefaultBundleGibs.Length; ++index3)
              {
                flag = name.ContainsCaseInsensitive(DismembermentManager.DefaultBundleGibs[index3]);
                if (flag)
                {
                  if (name.ContainsCaseInsensitive("ZombieGibs_caps"))
                  {
                    if (!Object.op_Implicit((Object) this.gibCapMaterialCopy))
                    {
                      this.gibCapMaterialCopy = Object.Instantiate<Material>(this.gibCapMaterial);
                      ((Object) this.gibCapMaterialCopy).name = ((Object) this.gibCapMaterial).name.Replace("(global)", "(local)");
                    }
                    sharedMaterials[index2] = this.gibCapMaterialCopy;
                    break;
                  }
                  break;
                }
              }
              if (!flag && ((Object) material).name.Contains("HD_"))
              {
                if (!Object.op_Implicit((Object) this.mainZombieMaterialCopy))
                  this.mainZombieMaterialCopy = Object.Instantiate<Material>(this.mainZombieMaterial);
                sharedMaterials[index2] = this.mainZombieMaterialCopy;
              }
            }
          }
          renderer.materials = sharedMaterials;
        }
      }
    }
    if (this.entity.IsFeral && bodyDamageFlag == 1U)
    {
      this.setUpEyeMats(t);
      if (part.isDetachable)
      {
        Transform recursive4 = t.FindRecursive("Detachable");
        if (Object.op_Implicit((Object) recursive4))
          this.setUpEyeMats(recursive4);
      }
      Transform recursive5 = t.FindRecursive("FeralFlame");
      if (Object.op_Implicit((Object) recursive5) && !this.entity.HasAnyTags(DismembermentManager.radiatedTag))
      {
        ((Component) recursive5).gameObject.SetActive(true);
        string name = "large_flames_LOD (3)";
        Transform recursive6 = ((Component) this.entity).transform.FindRecursive(name);
        if (Object.op_Implicit((Object) recursive6))
          ((Component) recursive6).gameObject.SetActive(false);
        else
          Log.Warning("entity {0} no longer has a child named {1}", new object[2]
          {
            (object) ((Object) this.entity).name,
            (object) name
          });
      }
    }
    if (Object.op_Implicit((Object) this.dismemberMat) || string.IsNullOrEmpty(this.subFolderDismemberEntityName))
      return;
    string str1 = this.rootDismmemberDir + $"/gibs_{this.subFolderDismemberEntityName.ToLower()}";
    Material sharedMaterial = ((Renderer) this.skinnedMeshRenderer).sharedMaterial;
    if (this.entity.HasAnyTags(DismembermentManager.radiatedTag) && (sharedMaterial.HasProperty("_IsRadiated") || sharedMaterial.HasProperty("_Irradiated")))
      str1 += "_IsRadiated";
    Material material1 = (Material) null;
    if (!string.IsNullOrEmpty(part.dismemberMatPath))
    {
      string _uri = $"{this.rootDismmemberDir}/{part.dismemberMatPath}.mat";
      material1 = DataLoader.LoadAsset<Material>(_uri);
      if (Object.op_Implicit((Object) material1))
        str1 = _uri;
    }
    string _uri1 = str1 + (this.altEntityMatId != -1 ? (object) this.altEntityMatId : (object) "")?.ToString();
    if (this.isCensored)
    {
      string str2 = _uri1;
      _uri1 += "_CGore.mat";
      material1 = DataLoader.LoadAsset<Material>(_uri1);
      if (!Object.op_Implicit((Object) material1))
        _uri1 = str2;
    }
    if (!Object.op_Implicit((Object) material1))
      _uri1 += ".mat";
    Material material2 = DataLoader.LoadAsset<Material>(_uri1);
    if (!Object.op_Implicit((Object) material2))
    {
      int num = part.useMask ? 1 : 0;
    }
    else
    {
      this.dismemberMat = Object.Instantiate<Material>(material2);
      if (this.dismemberMat.HasColor("_EmissiveColor") && sharedMaterial.HasColor("_EmissiveColor"))
        this.dismemberMat.SetColor("_EmissiveColor", sharedMaterial.GetColor("_EmissiveColor"));
      ((Renderer) this.skinnedMeshRenderer).material = this.dismemberMat;
      if (Object.op_Implicit((Object) this.smrLODOne))
        ((Renderer) this.smrLODOne).material = this.dismemberMat;
      if (!Object.op_Implicit((Object) this.smrLODTwo))
        return;
      ((Renderer) this.smrLODTwo).material = this.dismemberMat;
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void setUpEyeMats(Transform t)
  {
    Transform recursive1 = t.FindRecursive("NormalEye");
    Transform recursive2 = t.FindRecursive("FeralEye");
    if (Object.op_Implicit((Object) recursive2) && !this.entity.HasAnyTags(DismembermentManager.radiatedTag))
    {
      if (Object.op_Implicit((Object) recursive1))
        ((Component) recursive1).gameObject.SetActive(false);
      ((Component) recursive2).gameObject.SetActive(true);
    }
    else
    {
      if (!Object.op_Implicit((Object) recursive1))
        return;
      MeshRenderer component = ((Component) recursive1).GetComponent<MeshRenderer>();
      if (!Object.op_Implicit((Object) component))
        return;
      Material material = Object.Instantiate<Material>(((Renderer) component).material);
      if (material.HasProperty("_IsRadiated"))
        material.SetFloat("_IsRadiated", 1f);
      if (material.HasProperty("_Irradiated"))
        material.SetFloat("_Irradiated", 1f);
      ((Renderer) component).material = material;
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void MakeDismemberedPart(
    uint bodyDamageFlag,
    EnumDamageTypes damageType,
    Transform partT,
    bool restoreState)
  {
    DismemberedPartData dismemberedPartData = DismembermentManager.DismemberPart(bodyDamageFlag, damageType, this.entity, true, DismembermentManager.DebugUseLegacy);
    if (dismemberedPartData == null)
      return;
    DismemberedPart part = new DismemberedPart(dismemberedPartData, bodyDamageFlag, damageType);
    this.dismemberedParts.Add(part);
    if (!Object.op_Implicit((Object) partT))
      return;
    Transform parent1 = (Transform) null;
    if (!string.IsNullOrEmpty(dismemberedPartData.targetBone))
    {
      Transform transform1 = partT.FindRecursive(dismemberedPartData.targetBone);
      if (!Object.op_Implicit((Object) transform1))
        transform1 = partT.FindParent(dismemberedPartData.targetBone);
      Transform transform2 = new GameObject("DynamicGore").transform;
      if (!dismemberedPartData.attachToParent)
        transform2.SetParent(transform1);
      else
        transform2.SetParent(transform1.parent);
      transform2.localPosition = Vector3.zero;
      transform2.localRotation = Quaternion.identity;
      transform2.localScale = Vector3.one;
      parent1 = transform2;
      this.defaultHeadPos = Vector3.zero;
      if (!dismemberedPartData.useMask)
      {
        if (!string.IsNullOrEmpty(dismemberedPartData.insertBoneObj))
          this.defaultHeadPos = transform1.FindRecursive(dismemberedPartData.insertBoneObj).position;
        transform1.localScale = dismemberedPartData.scale;
        this.scaleOutChildBones(transform1);
      }
      else
      {
        Collider component = ((Component) transform1).GetComponent<Collider>();
        if (Object.op_Implicit((Object) component))
          component.enabled = false;
        this.disableChildColliders(transform1);
      }
    }
    else
      partT.localScale = dismemberedPartData.scale;
    if (string.IsNullOrEmpty(dismemberedPartData.prefabPath))
      return;
    if (string.IsNullOrEmpty(this.rootDismmemberDir) && dismemberedPartData.prefabPath.Contains("/"))
    {
      this.subFolderDismemberEntityName = dismemberedPartData.prefabPath.Remove(dismemberedPartData.prefabPath.IndexOf("/"));
      this.rootDismmemberDir = $"@:Entities/Zombies/{this.subFolderDismemberEntityName}/Dismemberment";
    }
    if (!Object.op_Implicit((Object) parent1))
      parent1 = GameUtils.FindTagInChilds(this.bipedT, dismemberedPartData.propertyKey.Replace("DismemberTag_", ""));
    Transform transform = this.SpawnLimbGore(parent1, dismemberedPartData.prefabPath, restoreState);
    if (!Object.op_Implicit((Object) transform) || string.IsNullOrEmpty(dismemberedPartData.targetBone))
      return;
    if (DismembermentManager.DebugLogEnabled)
    {
      foreach (MeshFilter componentsInChild in ((Component) transform).GetComponentsInChildren<MeshFilter>())
      {
        Mesh sharedMesh = componentsInChild.sharedMesh;
        if (!Object.op_Implicit((Object) sharedMesh) || Object.op_Implicit((Object) sharedMesh) && sharedMesh.vertexCount == 0)
          Log.Warning($"{((object) this).GetType()} prefabPath {dismemberedPartData.prefabPath} partName {((Object) ((Component) componentsInChild).transform).name} is missing a mesh.");
      }
    }
    if (!Object.op_Implicit((Object) this.skinnedMeshRenderer))
    {
      this.skinnedMeshRenderer = ((Component) this.entity.emodel.meshTransform).GetComponent<SkinnedMeshRenderer>();
      Transform parent2 = ((Component) this.skinnedMeshRenderer).transform.parent;
      for (int index = 0; index < parent2.childCount; ++index)
      {
        Transform child = parent2.GetChild(index);
        if (((Object) child).name.ContainsCaseInsensitive("LOD1"))
          this.smrLODOne = ((Component) child).GetComponent<SkinnedMeshRenderer>();
        if (((Object) child).name.ContainsCaseInsensitive("LOD2"))
          this.smrLODTwo = ((Component) child).GetComponent<SkinnedMeshRenderer>();
      }
    }
    Renderer[] componentsInChildren = ((Component) transform).GetComponentsInChildren<Renderer>();
    bool flag = false;
    for (int index1 = 0; index1 < componentsInChildren.Length; ++index1)
    {
      Renderer renderer = componentsInChildren[index1];
      if (!Object.op_Implicit((Object) ((Component) renderer).GetComponent<ParticleSystem>()))
      {
        Material[] sharedMaterials = renderer.sharedMaterials;
        for (int index2 = 0; index2 < sharedMaterials.Length; ++index2)
        {
          Material material = sharedMaterials[index2];
          string name = ((Object) material).name;
          if ((!part.prefabPath.ContainsCaseInsensitive("head") || !name.ContainsCaseInsensitive("hair")) && ((Object) material.shader).name == "Game/Character" && !DismembermentManager.IsDefaultGib(name))
          {
            sharedMaterials[index2] = this.mainZombieMaterial;
            flag = true;
          }
        }
        if (flag)
          renderer.sharedMaterials = sharedMaterials;
      }
    }
    this.ProcDismemberedPart(transform, partT, dismemberedPartData, bodyDamageFlag);
    part.prefabT = transform;
    Transform _boneT1 = partT.FindRecursive(dismemberedPartData.targetBone);
    if (!Object.op_Implicit((Object) _boneT1))
      _boneT1 = partT.FindParent(dismemberedPartData.targetBone);
    part.targetT = _boneT1;
    if (dismemberedPartData.useMask)
    {
      if (dismemberedPartData.scaleOutLimb)
      {
        Transform _boneT2 = partT.FindRecursive(dismemberedPartData.targetBone);
        if (!Object.op_Implicit((Object) _boneT2))
          _boneT2 = partT.FindParent(dismemberedPartData.targetBone);
        if (!string.IsNullOrEmpty(dismemberedPartData.solTarget))
        {
          _boneT2 = partT.FindRecursive(dismemberedPartData.solTarget);
          if (!Object.op_Implicit((Object) _boneT2))
            _boneT2 = partT.FindParent(dismemberedPartData.solTarget);
        }
        this.scaleOutChildBones(_boneT2);
        if (dismemberedPartData.hasSolScale)
          _boneT2.localScale = dismemberedPartData.solScale;
      }
      else
        this.scaleOutChildBones(_boneT1);
      EnumBodyPartHit bodyPartHit = DismembermentManager.GetBodyPartHit(part.bodyDamageFlag);
      Transform tagInChildren = this.modelT.FindTagInChildren("D_Accessory");
      if (Object.op_Implicit((Object) tagInChildren))
      {
        DismembermentAccessoryMan component = ((Component) tagInChildren).GetComponent<DismembermentAccessoryMan>();
        if (Object.op_Implicit((Object) component))
          component.HidePart(bodyPartHit);
      }
      if (!dismemberedPartData.scaleOutLimb || !string.IsNullOrEmpty(dismemberedPartData.solTarget))
        this.setLimbShaderProps(bodyPartHit, part);
    }
    if (!dismemberedPartData.isDetachable)
      return;
    this.ActivateDetachableLimbs(bodyDamageFlag, damageType, transform, part);
  }

  [Conditional("DEBUG_DISMEMBERMENT")]
  [PublicizedFrom(EAccessModifier.Private)]
  public void logDismemberment(string _log)
  {
    if (!DismembermentManager.DebugLogEnabled)
      return;
    Log.Out($"{((object) this).GetType()?.ToString()} {_log}");
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void scaleOutChildBones(Transform _boneT)
  {
    if (_boneT.childCount <= 0)
      return;
    for (int index = 0; index < _boneT.childCount; ++index)
    {
      Transform child = _boneT.GetChild(index);
      if (Object.op_Implicit((Object) child) && !((Object) child).name.Equals("DynamicGore"))
        child.localScale = Vector3.zero;
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void disableChildColliders(Transform _boneT)
  {
    foreach (Collider componentsInChild in ((Component) _boneT).GetComponentsInChildren<Collider>())
    {
      if (Object.op_Implicit((Object) componentsInChild))
        componentsInChild.enabled = false;
    }
    foreach (CharacterJoint componentsInChild in ((Component) _boneT).GetComponentsInChildren<CharacterJoint>())
    {
      if (Object.op_Implicit((Object) componentsInChild))
      {
        Rigidbody component = ((Component) componentsInChild).GetComponent<Rigidbody>();
        Object.Destroy((Object) componentsInChild);
        Object.Destroy((Object) component);
      }
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void ActivateDetachableLimbs(
    uint bodyDamageFlag,
    EnumDamageTypes damageType,
    Transform partT,
    DismemberedPart part)
  {
    Transform entitiesTransform = GameManager.Instance.World.EntitiesTransform;
    if (!Object.op_Implicit((Object) entitiesTransform))
      return;
    Transform transform1 = entitiesTransform.Find("DismemberedLimbs");
    if (!Object.op_Implicit((Object) transform1))
    {
      transform1 = new GameObject("DismemberedLimbs").transform;
      transform1.SetParent(entitiesTransform);
      transform1.localPosition = Vector3.zero;
    }
    Transform recursive1 = partT.FindRecursive("Detachable");
    if (!Object.op_Implicit((Object) recursive1))
      return;
    EnumBodyPartHit bodyPartHit = DismembermentManager.GetBodyPartHit(bodyDamageFlag);
    Transform transform2 = new GameObject($"{this.entity.entityId}_{this.entity.EntityName}_{bodyPartHit}").transform;
    transform2.SetParent(transform1);
    part.SetDetachedTransform(transform2, recursive1);
    if (this.entity.IsBloodMoon)
      part.lifeTime /= 3f;
    if (this.leftLowerArmDismembered && bodyDamageFlag == 2U)
    {
      DismembermentManager.ActivateDetachable(recursive1, "HalfArm");
      this.hideDismemberedPart(bodyDamageFlag);
    }
    if (this.leftLowerLegDismembered && bodyDamageFlag == 32U /*0x20*/)
    {
      DismembermentManager.ActivateDetachable(recursive1, "HalfLeg");
      this.hideDismemberedPart(bodyDamageFlag);
    }
    if (this.rightLowerArmDismembered && bodyDamageFlag == 8U)
    {
      DismembermentManager.ActivateDetachable(recursive1, "HalfArm");
      this.hideDismemberedPart(bodyDamageFlag);
    }
    if (this.rightLowerLegDismembered && bodyDamageFlag == 128U /*0x80*/)
    {
      DismembermentManager.ActivateDetachable(recursive1, "HalfLeg");
      this.hideDismemberedPart(bodyDamageFlag);
    }
    if (!((Component) recursive1).gameObject.activeSelf)
      ((Component) recursive1).gameObject.SetActive(true);
    if ((double) this.entity.OverrideHeadSize != 1.0 && this.headDismembered && bodyDamageFlag == 1U)
    {
      float headBigSize = this.entity.emodel.HeadBigSize;
      part.overrideHeadSize = headBigSize;
      part.overrideHeadDismemberScaleTime = this.entity.OverrideHeadDismemberScaleTime;
      Transform transform3 = recursive1.Find("Physics");
      Transform transform4 = new GameObject("pivot").transform;
      transform4.SetParent(transform3);
      transform4.localScale = Vector3.one;
      for (int index = 0; index < part.targetT.childCount; ++index)
      {
        Transform child = part.targetT.GetChild(index);
        if (((Component) child).CompareTag("E_BP_Head"))
        {
          Transform recursive2 = recursive1.FindRecursive(bodyPartHit.ToString());
          if (Object.op_Implicit((Object) recursive2))
          {
            Renderer component = ((Component) recursive2).GetComponent<Renderer>();
            Transform transform5 = transform4;
            Vector3 position = child.position;
            Bounds bounds = component.bounds;
            Vector3 vector3_1 = Vector3.op_Subtraction(((Bounds) ref bounds).center, child.position);
            Vector3 vector3_2 = Vector3.op_Addition(position, vector3_1);
            transform5.position = vector3_2;
            part.pivotT = transform4;
            break;
          }
          transform4.position = child.position;
          part.pivotT = transform4;
          break;
        }
      }
      List<Transform> transformList = new List<Transform>();
      for (int index = 0; index < transform3.childCount; ++index)
      {
        Transform child = transform3.GetChild(index);
        if (Object.op_Inequality((Object) child, (Object) transform3))
          transformList.Add(child);
      }
      for (int index = 0; index < transformList.Count; ++index)
        transformList[index].SetParent(transform4);
      transform4.localScale = new Vector3(headBigSize, headBigSize, headBigSize);
    }
    recursive1.SetParent(transform2);
    DismembermentManager.Instance?.AddPart(part);
    string empty = string.Empty;
    foreach (Renderer componentsInChild in ((Component) recursive1).GetComponentsInChildren<Renderer>())
    {
      if (Object.op_Inequality((Object) componentsInChild, (Object) null))
      {
        Material[] sharedMaterials = componentsInChild.sharedMaterials;
        for (int index = 0; index < sharedMaterials.Length; ++index)
        {
          Material material = sharedMaterials[index];
          if (Object.op_Inequality((Object) material, (Object) null))
          {
            string name = ((Object) material).name;
            if ((!part.prefabPath.ContainsCaseInsensitive("head") || !name.ContainsCaseInsensitive("hair")) && (!((Object) componentsInChild).name.ContainsCaseInsensitive("eye") || material.HasProperty("_IsRadiated") || material.HasProperty("_Irradiated")))
            {
              if (name.ContainsCaseInsensitive("ZombieGibs_caps"))
                sharedMaterials[index] = this.gibCapMaterial;
              if (name.Contains("HD_"))
                sharedMaterials[index] = this.mainZombieMaterial;
              sharedMaterials[index].DisableKeyword("_ELECTRIC_SHOCK_ON");
            }
          }
        }
        componentsInChild.sharedMaterials = sharedMaterials;
      }
    }
    foreach (Behaviour componentsInChild in ((Component) recursive1).GetComponentsInChildren<Assets.DuckType.Jiggle.Jiggle>(true))
      componentsInChild.enabled = true;
    Rigidbody componentInChildren = ((Component) recursive1).GetComponentInChildren<Rigidbody>();
    if (!Object.op_Implicit((Object) componentInChildren))
      return;
    Vector3 vector3_3 = Vector3.op_Multiply(Vector3.up, this.entity.lastHitForce);
    float num1 = Vector3.Angle(this.entity.GetForwardVector(), this.entity.lastHitImpactDir);
    Rigidbody rigidbody1 = componentInChildren;
    Quaternion rotation1 = Quaternion.FromToRotation(this.entity.GetForwardVector(), this.entity.lastHitImpactDir);
    Vector3 vector3_4 = Vector3.op_Multiply(((Quaternion) ref rotation1).eulerAngles, (float) (1.0 + (double) num1 / 90.0));
    rigidbody1.AddTorque(vector3_4, (ForceMode) 1);
    componentInChildren.AddForce(Vector3.op_Multiply(Vector3.op_Addition(this.entity.lastHitImpactDir, vector3_3), this.entity.lastHitForce), (ForceMode) 1);
    string damageTag = DismembermentManager.GetDamageTag(damageType, this.entity.lastHitRanged);
    if (damageTag == "blunt")
    {
      if (damageType == EnumDamageTypes.Piercing)
        componentInChildren.AddForce(Vector3.op_Addition(this.entity.lastHitImpactDir, vector3_3), (ForceMode) 1);
      else
        componentInChildren.AddForce(Vector3.op_Addition(Vector3.op_Multiply(Vector3.op_Multiply(this.entity.lastHitImpactDir, this.entity.lastHitForce), 1.5f), Vector3.op_Multiply(vector3_3, 1.25f)), (ForceMode) 1);
    }
    if (damageTag == "blade")
    {
      float num2 = Vector3.Dot(this.entity.GetForwardVector(), this.entity.lastHitImpactDir);
      float num3 = Vector3.Dot(this.entity.GetForwardVector(), this.entity.lastHitEntityFwd);
      componentInChildren.AddForce((double) num2 < (double) num3 ? Vector3.op_Addition(Vector3.op_Multiply(Vector3.op_UnaryNegation(((Component) this.entity).transform.right), this.entity.lastHitForce), vector3_3) : Vector3.op_Addition(Vector3.op_Multiply(((Component) this.entity).transform.right, this.entity.lastHitForce), vector3_3), (ForceMode) 1);
      Rigidbody rigidbody2 = componentInChildren;
      Quaternion rotation2 = Quaternion.FromToRotation(this.entity.GetForwardVector(), this.entity.lastHitImpactDir);
      Vector3 vector3_5 = Vector3.op_Multiply(Vector3.op_Multiply(((Quaternion) ref rotation2).eulerAngles, (float) (1.0 + (double) num1 / 90.0)), this.entity.lastHitForce);
      rigidbody2.AddTorque(vector3_5, (ForceMode) 1);
    }
    if (damageType != EnumDamageTypes.Heat)
      return;
    float num4 = 2.67f;
    componentInChildren.AddForce(Vector3.op_Addition(Vector3.op_Multiply(this.entity.lastHitImpactDir, num4), Vector3.op_Multiply(Vector3.op_Multiply(Vector3.up, num4), 0.67f)), (ForceMode) 1);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public Material GetMainZombieBodyMaterial()
  {
    EModelBase emodel = this.entity.emodel;
    if (Object.op_Implicit((Object) emodel))
    {
      Transform meshTransform = emodel.meshTransform;
      if (Object.op_Implicit((Object) meshTransform))
        return ((Component) meshTransform).GetComponent<Renderer>().sharedMaterial;
    }
    return (Material) null;
  }

  public override void Electrocute(bool enabled)
  {
    base.Electrocute(enabled);
    if (enabled)
    {
      Material zombieBodyMaterial = this.GetMainZombieBodyMaterial();
      if (Object.op_Implicit((Object) zombieBodyMaterial))
        zombieBodyMaterial.EnableKeyword("_ELECTRIC_SHOCK_ON");
      if (Object.op_Implicit((Object) this.dismemberMat))
        this.dismemberMat.EnableKeyword("_ELECTRIC_SHOCK_ON");
      if (Object.op_Implicit((Object) this.mainZombieMaterialCopy))
        this.mainZombieMaterialCopy.EnableKeyword("_ELECTRIC_SHOCK_ON");
      if (Object.op_Implicit((Object) this.gibCapMaterialCopy))
        this.gibCapMaterialCopy.EnableKeyword("_ELECTRIC_SHOCK_ON");
      this.StartAnimationElectrocute(0.6f);
    }
    else
    {
      Material zombieBodyMaterial = this.GetMainZombieBodyMaterial();
      if (Object.op_Implicit((Object) zombieBodyMaterial))
        zombieBodyMaterial.DisableKeyword("_ELECTRIC_SHOCK_ON");
      if (Object.op_Implicit((Object) this.dismemberMat))
        this.dismemberMat.DisableKeyword("_ELECTRIC_SHOCK_ON");
      if (Object.op_Implicit((Object) this.mainZombieMaterialCopy))
        this.mainZombieMaterialCopy.DisableKeyword("_ELECTRIC_SHOCK_ON");
      if (Object.op_Implicit((Object) this.gibCapMaterialCopy))
        this.gibCapMaterialCopy.DisableKeyword("_ELECTRIC_SHOCK_ON");
      this.StartAnimationElectrocute(0.0f);
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void setLimbShaderProps(EnumBodyPartHit partHit, DismemberedPart part)
  {
    DismemberedPartData data = part.Data;
    if (!Object.op_Implicit((Object) this.dismemberMat))
      return;
    bool scaleOutLimb = data.scaleOutLimb;
    bool isLinked = data.isLinked;
    if (this.dismemberMat.HasProperty("_LeftLowerLeg") && (partHit & EnumBodyPartHit.LeftLowerLeg) > EnumBodyPartHit.None)
    {
      this.dismemberMat.SetFloat("_LeftLowerLeg", 1f);
      if (isLinked)
        this.dismemberMat.SetFloat("_LeftUpperLeg", 1f);
    }
    if (this.dismemberMat.HasProperty("_LeftUpperLeg") && (partHit & EnumBodyPartHit.LeftUpperLeg) > EnumBodyPartHit.None)
    {
      if (!isLinked)
        this.dismemberMat.SetFloat("_LeftUpperLeg", 1f);
      if (!scaleOutLimb)
        this.dismemberMat.SetFloat("_LeftLowerLeg", 1f);
    }
    if (this.dismemberMat.HasProperty("_RightLowerLeg") && (partHit & EnumBodyPartHit.RightLowerLeg) > EnumBodyPartHit.None)
    {
      this.dismemberMat.SetFloat("_RightLowerLeg", 1f);
      if (isLinked)
        this.dismemberMat.SetFloat("_RightUpperLeg", 1f);
    }
    if (this.dismemberMat.HasProperty("_RightUpperLeg") && (partHit & EnumBodyPartHit.RightUpperLeg) > EnumBodyPartHit.None)
    {
      if (!isLinked)
        this.dismemberMat.SetFloat("_RightUpperLeg", 1f);
      if (!scaleOutLimb)
        this.dismemberMat.SetFloat("_RightLowerLeg", 1f);
    }
    if (this.dismemberMat.HasProperty("_LeftLowerArm") && (partHit & EnumBodyPartHit.LeftLowerArm) > EnumBodyPartHit.None && !scaleOutLimb)
      this.dismemberMat.SetFloat("_LeftLowerArm", 1f);
    if (this.dismemberMat.HasProperty("_LeftUpperArm") && (partHit & EnumBodyPartHit.LeftUpperArm) > EnumBodyPartHit.None)
    {
      if (!isLinked)
        this.dismemberMat.SetFloat("_LeftUpperArm", 1f);
      if (!scaleOutLimb)
        this.dismemberMat.SetFloat("_LeftLowerArm", 1f);
    }
    if (this.dismemberMat.HasProperty("_RightLowerArm") && (partHit & EnumBodyPartHit.RightLowerArm) > EnumBodyPartHit.None && !scaleOutLimb)
      this.dismemberMat.SetFloat("_RightLowerArm", 1f);
    if (!this.dismemberMat.HasProperty("_RightUpperArm") || (partHit & EnumBodyPartHit.RightUpperArm) <= EnumBodyPartHit.None)
      return;
    if (!isLinked)
      this.dismemberMat.SetFloat("_RightUpperArm", 1f);
    if (scaleOutLimb)
      return;
    this.dismemberMat.SetFloat("_RightLowerArm", 1f);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void hideDismemberedPart(uint bodyDamageFlag)
  {
    uint lowerBodyPart = 0;
    if (bodyDamageFlag == 2U)
      lowerBodyPart = 4U;
    if (bodyDamageFlag == 8U)
      lowerBodyPart = 16U /*0x10*/;
    if (bodyDamageFlag == 32U /*0x20*/)
      lowerBodyPart = 64U /*0x40*/;
    if (bodyDamageFlag == 128U /*0x80*/)
      lowerBodyPart = 256U /*0x0100*/;
    if (lowerBodyPart == 0U)
      return;
    this.dismemberedParts.Find((Predicate<DismemberedPart>) ([PublicizedFrom(EAccessModifier.Internal)] (p) => (int) p.bodyDamageFlag == (int) lowerBodyPart))?.Hide();
  }

  public bool IsCrawler => this.isCrawler;

  public override void TurnIntoCrawler(bool restoreState)
  {
    if (this.isCrawler || this.entity.GetWalkType() == 21)
      return;
    this.isCrawler = true;
    this.crawlerTime = Time.time;
    this.isSuppressPain = true;
    this._setInt(AvatarController.hitBodyPartHash, 0, true);
    this.SetWalkType(21);
    this._setTrigger(AvatarController.toCrawlerTriggerHash);
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public override void _setInt(int _propertyHash, int _value, bool _netsync = true)
  {
    if (this.IsCrawler && _propertyHash == AvatarController.walkTypeHash && _value == 5)
      return;
    base._setInt(_propertyHash, _value, _netsync);
  }

  public override void TriggerSleeperPose(int pose, bool returningToSleep = false)
  {
    if (returningToSleep)
    {
      base.TriggerSleeperPose(pose, returningToSleep);
    }
    else
    {
      if (!Object.op_Implicit((Object) this.anim))
        return;
      this._setInt(AvatarController.sleeperPoseHash, pose, true);
      switch (pose)
      {
        case -2:
          this.anim.CrossFadeInFixedTime("Crouch Walk 8", 0.25f);
          break;
        case -1:
          this._setTrigger(AvatarController.sleeperTriggerHash);
          break;
        case 0:
          this.anim.Play(AvatarController.sleeperIdleSitHash);
          break;
        case 1:
          this.anim.Play(AvatarController.sleeperIdleSideRightHash);
          break;
        case 2:
          this.anim.Play(AvatarController.sleeperIdleSideLeftHash);
          break;
        case 3:
          this.anim.Play(AvatarController.sleeperIdleBackHash);
          break;
        case 4:
          this.anim.Play(AvatarController.sleeperIdleStomachHash);
          break;
        case 5:
          this.anim.Play(AvatarController.sleeperIdleStandHash);
          break;
      }
    }
  }
}
