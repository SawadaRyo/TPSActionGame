using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class player : MonoBehaviour
{
    [SerializeField] Camera playerCamera = null;
    [SerializeField] float speed = 2f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float jumpGravityScale = 0.6f;
    [SerializeField] float accelerration = 10f;
    [SerializeField] float maxGroundAngle = 45;
    [SerializeField] float groundDistance = 0.01f;
    [SerializeField] LayerMask groundMask = ~0;
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
        Vector3 movementFoward = Vector3.forward;
        if(playerCamera != null)
        {
            Vector3 camareRight = playerCamera.transform.right;
            Vector3 cameraFoward = playerCamera.transform.forward;
        }
        rb.AddForce(new Vector3(movementInput.x, 0f, movementInput.y) * accelerration, ForceMode.Acceleration);
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
}
