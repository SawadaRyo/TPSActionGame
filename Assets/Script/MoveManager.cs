using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(StaminaManager))]
public class MoveManager : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float runSpeed = 15f;
    [SerializeField] float avoidanceSpeed = 25;
    [SerializeField] float turn = 0.7f;
    [SerializeField] GameObject avatar;

    Vector3 movement;
    Vector3 getTergetRengeCenter = Vector3.zero;
    Vector2 movementInput = Vector2.zero;
    Animator animator;
    Rigidbody rb;
    bool isAttack;//攻撃コマンド判定
    bool runFlg; //ダッシュコマンド
    bool groundFlg; //接地判定
    bool avoidanceCount; //回避入力判定
    CapsuleCollider capsuleCollider;
    SphereCollider sphereCollider;
    StaminaManager staminaManager;

    public bool living; //生死判定
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); //Rigitbodyを取得
        animator = GetComponent<Animator>(); //Animatorを取得
        capsuleCollider = GetComponent<CapsuleCollider>(); //CapsuleColliderを取得
        sphereCollider = GetComponent<SphereCollider>(); //SphereColliderを取得
        staminaManager = GetComponent<StaminaManager>(); //StaminaManagerを取得
        isAttack = false; 
        avoidanceCount = false;
        living = true;
    }
    void Action()
    {
        //移動処理
        Vector3 movementRight = Vector3.right;　//左右のベクトルを取得
        Vector3 movementForward = Vector3.forward;　//上下のベクトルを取得 
        movement = movementRight * movementInput.x + movementForward * movementInput.y;　//左スティック入力
        movement = Camera.main.transform.TransformDirection(movement);　//カメラの方向に合わせる
        movement.y = 0;
        movement.Normalize();

        if (avatar != null)
        {
            //方向転換をスムーズにする。
            Vector3 rotateTarget = new Vector3(movement.x, 0f, movement.z);
            if (rotateTarget.magnitude > 0.1f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(rotateTarget);
                avatar.transform.rotation = Quaternion.Lerp(lookRotation, avatar.transform.rotation, turn);
            }
        }
        if (groundFlg)
        {
            rb.velocity = movement * moveSpeed; //移動
            if (runFlg) rb.velocity = movement * runSpeed; //ダッシュ
        }
    }
    bool IsGround()
    {
        var distance = 0.2f;//Rayの射程
        float extent = Mathf.Max(0, capsuleCollider.height * 0.5f - capsuleCollider.radius);
        Vector3 origin = transform.TransformPoint(capsuleCollider.center + Vector3.down * extent) + Vector3.up * distance; //SphereCastの中心点
        //Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        Ray sphereCastRay = new Ray(origin, Vector3.down); //Rayを飛ばす方向
        bool hitInfo = Physics.SphereCast(sphereCastRay, capsuleCollider.radius,distance * 2f); //Rayを飛ばした方向に地面があるか判定
        return hitInfo;
    }

    

    void FixedUpdate()
    {
        groundFlg = IsGround();
        Action();
        //Debug.Log(avoidanceCount);
    }
    void Update()
    {
        Vector3 moveSpeed = rb.velocity;
        moveSpeed.y = 0;
        animator.SetFloat("MoveSpeed", moveSpeed.magnitude);
        animator.SetBool("OnGround", groundFlg);
    }
    void Move(Vector2 input)
    {
        movementInput = input;
    }
    public void Attack(bool state)
    {
        if (animator == null) return;//animaterの未取得防止
        if (state && staminaManager.AttackFlg)//スタミナが0の時攻撃できない
        {
            animator.SetTrigger("Slash");
            //Debug.Log("true");
            isAttack = true;
        }
        else
        {
            //Debug.Log("false");
            isAttack = false;
        }
    }
    void Avoidance(bool state)
    {
        if (animator == null) return;
        if (state) animator.SetTrigger("Avoidance");
    }
    void OnMove(InputValue inputValue)
    {
        if (!living) return;
        if (isAttack) return;
        Move(inputValue.Get<Vector2>());
        //Debug.Log(inputValue.Get<Vector2>());
    }
    void OnAttack(InputValue inputValue)
    {
        if (!living) return;
        Attack(inputValue.isPressed);
        Debug.Log(inputValue.isPressed);
    }
    void OnAvoidance(InputValue inputValue)
    {
        if (!living) return;
        Avoidance(inputValue.isPressed);
    }
    void AvoidanceOn()
    {
        //Vector3 forward = transform.InverseTransformPoint(gameObject.transform.position);
        //rb.AddForce(forward * avoidanceSpeed, ForceMode.Impulse);
        rb.AddForce(movement * avoidanceSpeed, ForceMode.Acceleration);
    }
}
