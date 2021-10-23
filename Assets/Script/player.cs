using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class player : MonoBehaviour
{
    [SerializeField] Camera playerCamera = null;      //カメラ
    [SerializeField] GameObject avatar = null;        //AvatarObjectへの参照
    [SerializeField] float speed = 2f;　　　　　　  　//移動スピード
    [SerializeField] float jumpSpeed = 10f;　　　     //ジャンプ力
    [SerializeField] float jumpGravityScale = 0.6f;   //ジャンプ中の重力調整
    [SerializeField] float acceleration = 10f;        //移動加速度
    [SerializeField] float maxGroundAngle = 45;       //
    [SerializeField] float groundDistance = 0.01f;    //地面との相対距離
    [SerializeField] float turn = 0.7f;               //方向転換の滑らかさ
    [SerializeField] LayerMask groundMask = ~0;       //地面の接地判定のレイヤー
    
    Rigidbody rb;
    Rigidbody groundRigidbody = null;
    Vector3 groundNormal = Vector3.up;
    Vector3 groundContactPoint = Vector3.zero;
    Vector2 moveDirection = Vector2.zero;
    Vector2 movementInput = Vector2.zero;
    bool isOnGround = false;
    bool isJunping = false;
    CapsuleCollider capsuleCollider;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }
    void ApplyMotion()
    {
        Vector3 movementRight = Vector3.right;
        Vector3 movementForward = Vector3.forward;
        if(playerCamera != null)
        {
            Vector3 camareRight = playerCamera.transform.right;
            Vector3 cameraFoward = playerCamera.transform.forward;
            movementRight = ProjectOnPlane(camareRight, groundNormal).normalized;
            movementForward = ProjectOnPlane(cameraFoward, groundNormal).normalized;
            
        }
        Vector3 movement = movementRight * movementInput.x + movementForward * movementInput.y;
        if(avatar != null)
        {
            Vector3 rotateTarget = new Vector3(movement.x, 0f, movement.z);
            if (rotateTarget.magnitude > 0.1f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(rotateTarget);
                avatar.transform.rotation = Quaternion.Lerp(lookRotation, avatar.transform.rotation, turn);
            }
        }
        Vector3 velcity = rb.velocity;
        if(groundRigidbody != null)
        {
            velcity -= groundRigidbody.velocity;
        }
        Vector3 groundVelocity = ProjectOnPlane(rb.velocity, groundNormal);
        float groundAngle = 90f - Mathf.Asin(groundNormal.y) * 180f / Mathf.PI;
        bool movingDownhill = movement.y <= 0f;
        if(groundAngle <= maxGroundAngle || movingDownhill)
        {
            if (groundVelocity.magnitude < speed)
            {
                //rb.AddForce(new Vector3(movementInput.x, 0f, movementInput.y) * accelerration, ForceMode.Acceleration);
                rb.AddForce(movement * acceleration, ForceMode.Acceleration);
            }
        }
    }
    void FixedUpdate()
    {
        RaycastHit hitInfo = CheckForGround();
        isOnGround = hitInfo.collider != null;
        if (isOnGround)
        {
            ApplyMotion();
            groundNormal = hitInfo.normal;
            groundContactPoint = hitInfo.point;
            groundRigidbody = hitInfo.rigidbody;
        }
        else
        {
            groundNormal = Vector3.up;
            groundRigidbody = null;
            groundContactPoint = transform.TransformPoint(capsuleCollider.center+Vector3.down * capsuleCollider.height * 0.5f);
        }
        if (rb.velocity.y < 0)
        {
            isJunping = false;
        }
        if(isJunping)
        {
            rb.AddForce(Physics.gravity * rb.mass * (jumpGravityScale - 1f));
        }
    }
    void Jump(bool state)
    {
        if(state && isOnGround)
        {
            rb.velocity += Vector3.up * jumpSpeed;
            isJunping = true;
        }
        if(!state)
        {
            isJunping = false;
        }
    }
    void Move(Vector2 input)
    {
        movementInput = input;
        
    }
    void OnMove(InputValue inputValue)
    {
        Move(inputValue.Get<Vector2>());
    }
    void OnJump(InputValue inputValue)
    {
        Jump(inputValue.isPressed);
    }
    RaycastHit CheckForGround()
    {
        //Vector3 capsuleBottom = capsuleCollider.center + Vector3.down * capsuleCollider.height * 0.5f;
        //Vector3 feelPosition = transform.TransformPoint(capsuleBottom);
        //bool rayCastHit = Physics.Raycast(feelPosition, Vector3.down, groundDistance*2f,groundMask);
        float extent = Mathf.Max(0, capsuleCollider.height * 0.5f - capsuleCollider.radius);
        Vector3 origin = transform.TransformPoint(capsuleCollider.center + Vector3.down * extent) + Vector3.up * groundDistance;

        RaycastHit hitInfo;
        Ray sphereCastRay = new Ray(origin, Vector3.down);
        bool rayCastHit = Physics.SphereCast(sphereCastRay, capsuleCollider.radius, out hitInfo,groundDistance * 2f, groundMask);
        //Debug.Log(rayCastHit);
        return hitInfo;
    }
    Vector3 ProjectOnPlane(Vector3 vector,Vector3 normal)
    {
        return Vector3.Cross(normal,Vector3.Cross(vector,normal));
    }
}
