using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandIK : MonoBehaviour
{
    /// <summary>右手のターゲット</summary>
    [SerializeField] Transform rightTarget = default;
    /// <summary>左手のターゲット</summary>
    [SerializeField] Transform leftTarget = default;
    /// <summary>右手の Position に対するウェイト</summary>
    [SerializeField, Range(0f, 1f)] float rightPositionWeight = 0;
    /// <summary>右手の Rotation に対するウェイト</summary>
    [SerializeField, Range(0f, 1f)] float rightRotationWeight = 0;
    /// <summary>左手の Position に対するウェイト</summary>
    [SerializeField, Range(0f, 1f)] float leftPositionWeight = 0;
    /// <summary>左手の Rotation に対するウェイト</summary>
    [SerializeField, Range(0f, 1f)] float leftRotationWeight = 0;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex)
    {
        // 右手に対して IK を設定する
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightPositionWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightRotationWeight);
        animator.SetIKPosition(AvatarIKGoal.RightHand, rightTarget.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, rightTarget.rotation);
        // 左手に対して IK を設定する
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftPositionWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftRotationWeight);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftTarget.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftTarget.rotation);
    }
}
