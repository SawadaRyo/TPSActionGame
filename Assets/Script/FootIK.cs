using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootIK : MonoBehaviour
{
    /// <summary>右足のターゲット</summary>
    [SerializeField] Transform _rightTarget = default;
    /// <summary>左足のターゲット</summary>
    [SerializeField] Transform _leftTarget = default;
    /// <summary>右足の Position に対するウェイト</summary>
    [SerializeField, Range(0f, 1f)] float _rightPositionWeight = 0;
    /// <summary>右足の Rotation に対するウェイト</summary>
    [SerializeField, Range(0f, 1f)] float _rightRotationWeight = 0;
    /// <summary>左足の Position に対するウェイト</summary>
    [SerializeField, Range(0f, 1f)] float _leftPositionWeight = 0;
    /// <summary>左足の Rotation に対するウェイト</summary>
    [SerializeField, Range(0f, 1f)] float _leftRotationWeight = 0;
    Animator _anim = default;

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex)
    {
        // 右足に対して IK を設定する
        _anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, _rightPositionWeight);
        _anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, _rightRotationWeight);
        _anim.SetIKPosition(AvatarIKGoal.RightFoot, _rightTarget.position);
        _anim.SetIKRotation(AvatarIKGoal.RightFoot, _rightTarget.rotation);
        // 左足に対して IK を設定する
        _anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, _leftPositionWeight);
        _anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, _leftRotationWeight);
        _anim.SetIKPosition(AvatarIKGoal.LeftFoot, _leftTarget.position);
        _anim.SetIKRotation(AvatarIKGoal.LeftFoot, _leftTarget.rotation);
    }
}
