using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using NUnit.Framework;
using TreeEditor;
using Unity.Multiplayer.Center.Common;
using Unity.VisualScripting;
using UnityEngine;

public class SlimeController : MonoBehaviour
{

    [SerializeField]
    private float jumpPower = 2f;
    [SerializeField]
    private float moveSpeed = 1f;


    [SerializeField]
    private float sideForce = 1f;
    private float moveX;
    private float moveZ;
    [SerializeField] private float slideForce = 5f;
    public float MoveX => moveX;
    public float MoveZ => moveZ;
    //private float sideForce = 1f;
    private Rigidbody rb;
    private Animator anim;
    private bool isGrounded = true;
    private bool isMoved_Right = false;
    private bool isMoved_Left = false;
    private bool isSliding = false;
    [SerializeField] private float slideSpeed = 50f;
    //UnityEditor.TransformWorldPlacementJSON:{"position":{"x":0.270501820966720581},"scale":{"x":0.08333331346511841,"y":0.08333331346511841,"z":0.08333331346511841}}9414424896,"y":5.71790075302124,"z":79.584228515625},"rotation":{"x":0.16977959871292115,"y":0.0,"z":0.0,"w":0.9854

    private Vector3 slideDirection;

    // 追加: 滑り終了判定用パラメータ
    [SerializeField] private float slopeStopAngle = 10f; // この角度以下なら「平ら」と見なして滑り終了
    [SerializeField] private float slideFriction = 0.90f; // 滑り速度の減衰係数（毎FixedUpdate）
    [SerializeField] private float minSlideSpeed = 0.05f; // これ以下になったら滑りを止める
    private float currentSlideSpeed;
    private bool isFinished = true;

    private Vector3 moveDir,moveSlp;
    AnimatorStateInfo stateInfo;
    //private bool isSquatting = false; // しゃがみ状態フラグ
    private bool squatInput = false;
    [SerializeField] private WalkingSpace walkingSpace;
    [SerializeField] private Transform point;

    //[SerializeField] private Camera MainCamera;


    private Vector3 PresentPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        Cursor.visible = false;
        PresentPosition = this.transform.position;
        


        //localRotaionはQuaternionで管理されているので、eulerAnglesで確認可能
        //Debug.Log(PresentPosition);

    }

    // Update is called once per frame
    void FixedUpdate()
    {


        // 滑っているときは滑り専用の処理（速度を減衰させつつ移動）
        // 移動方向をベクトルで作成
        moveDir = new Vector3(moveX, 0, moveZ);
        moveSlp = new Vector3(moveX, 0, 0);
         if (moveX > 0)
         {
             isMoved_Right = true;
             isMoved_Left = false;
         }
         else if (moveX < 0)
         {
             isMoved_Left = true;
             isMoved_Right = false;
         }
         else
         {
             isMoved_Left = false;
             isMoved_Right = false;
         }
 
         // 正規化して一定速度に保つ
         // Rigidbodyを使って移動（物理演算対応）
         

         // 通常移動
        
        
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");
        moveDir = new Vector3(moveX, 0, moveZ);



        if (isSliding)
        {
            // 坂を一定速度で滑る
            //rb.MovePosition(transform.position + slideDirection * slideSpeed * Time.deltaTime);
            moveSpeed = 0.5f;
            transform.position += (moveSlp * moveSpeed * 5 + slideDirection * slideSpeed) * Time.deltaTime;
        }
        else
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
        //Debug.Log("これはSlimeController"+walkingSpace.IsMoving);
     }

    void Update()
    {
        stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        //a = Input.GetMouseButton(0);
        //Debug.Log(stateInfo.fullPathHash);
        //Debug.Log(a+" "+ isGrounded);


        squatInput = Input.GetMouseButton(0) || Input.GetMouseButton(1)
         || Input.GetKey(KeyCode.LeftShift); // 右クリックを押してる間しゃがむ
        anim.SetBool("Change", squatInput);



        //Debug.Log(squatInput);
        // ジャンプ
        //Debug.Log(isGrounded);


        if (stateInfo.IsName("Slime_HorizontalStretch") &&
         Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            anim.SetTrigger("Jump_Stretch");
            //Debug.Log("JumpStretch");
        }
        if (stateInfo.IsName("Slime_VerticalStretch") &&
         Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            anim.SetTrigger("Jump_Stretch");
            //Debug.Log("JumpStretch2");
        }
        //Debug.Log(isMoved_Left + " " + isMoved_Right);


        if (Input.GetKeyDown(KeyCode.Space) && isMoved_Left && isGrounded)
        {
            anim.SetTrigger("LeftJump");
            rb.AddForce(new Vector3(-sideForce, jumpPower, 0f), ForceMode.Impulse);
            isGrounded = false;
            //Debug.Log("migtobi");
        }
        if (Input.GetKeyDown(KeyCode.Space) && isMoved_Right && isGrounded)
        {
            anim.SetTrigger("RightJump");
            rb.AddForce(new Vector3(sideForce, jumpPower, 0f), ForceMode.Impulse);
            isGrounded = false;
            //Debug.Log("hidaritobi");
        }



        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {

            anim.SetTrigger("Jump");
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

            //smr.SetBlendShapeWeight(jumpIndex, 100f);
            isGrounded = false;
            //Debug.Log("Jump");
        }


        //しゃがむ
        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)
        {
            anim.SetTrigger("Squash");
            //Debug.Log("Down");
        }

        //横伸び
        if (Input.GetMouseButtonDown(0) && isGrounded)
        {
            anim.SetTrigger("HorizontalStretch");
            //Debug.Log("yokonibiyon");
        }
        //縦伸び
        if (Input.GetMouseButtonDown(1) && isGrounded)
        {
            anim.SetTrigger("VerticalStretch");
            //Debug.Log("tatenobin");
        }
        if (Input.GetKeyDown(KeyCode.Space) && isMoved_Left && isGrounded)
        {
            anim.SetTrigger("LeftJump");
            rb.AddForce(new Vector3(-sideForce, jumpPower, 0f), ForceMode.Impulse);
            isGrounded = false;
            //Debug.Log("migtobi");
        }
        if (Input.GetKeyDown(KeyCode.Space) && isMoved_Right && isGrounded)
        {
            anim.SetTrigger("RightJump");
            rb.AddForce(new Vector3(sideForce, jumpPower, 0f), ForceMode.Impulse);
            isGrounded = false;
            //Debug.Log("hidaritobi");
        }




        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            squatInput = false;
            /*if (sideSquat)
            {
                transform.localScale = Vector3.Lerp(SquatScale, PresentScale, 0.5f);
                sideSquat = false;
                Debug.Log("sideSquatをfalseにしたよ");
            }*/
        }

        /*if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))
        {
            Debug.Log("伸びたよ！！");
            anim.SetTrigger("Squash");
            transform.localScale = Vector3.Lerp(PresentScale, SquatScale, 0.5f);
            sideSquat = true;
            
        }     */







    }
    void OnCollisionEnter(Collision collision)
    {


        //Debug.Log(a+" "+ isGrounded);
        if (collision.gameObject.CompareTag("Ground") )
        {

            isGrounded = true;
            anim.ResetTrigger("Jump");
            anim.ResetTrigger("RightJump");
            anim.ResetTrigger("LeftJump");
        }
        if (collision.gameObject.CompareTag("Block"))
        {
            this.transform.position = point.position;
        }
        if (collision.gameObject.CompareTag("Fall"))
        {
            transform.position = new Vector3(PresentPosition.x, PresentPosition.y, PresentPosition.z);
            if (walkingSpace.IsMoving)
            {
                walkingSpace.PositionReset();
            }
            
            

        }
        if (collision.gameObject.CompareTag("Slope") && isFinished)
        {

            ContactPoint contact = collision.contacts[0];
            Vector3 normal = contact.normal;

            // 坂の下方向を求める
            slideDirection = Vector3.Cross(Vector3.Cross(normal, Vector3.down), normal).normalized;

            isSliding = true;
            isGrounded = true;
            //anim.SetTrigger("Slope");
            //anim.SetBool("Change_Slope", true);
            Debug.Log("Slopeに乗ったよ (滑り開始)");
        }


        /*if (collision.gameObject.CompareTag("Wall"))
        {
            transform.position = PresentPosition;
        }*/
    }
    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.CompareTag("Middle"))
        {
            PresentPosition = this.transform.position;
            Debug.Log("中間ポイント");
        }
        if(other.gameObject.CompareTag("Coin")){
            GameManager.Instance.AddScore(10);
            Destroy(other.gameObject);
        }
       
    }

    void OnCollisionStay(Collision collision)
    {
        // 坂に接触している間は滑り続ける
        if (collision.gameObject.CompareTag("Slope"))
        {
            isSliding = true;
            //anim.SetBool("Change_Slope", true);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // 坂から離れたら滑りを終了（空中に出た場合）
        if (collision.gameObject.CompareTag("Slope"))
        {
            isSliding = false;
            //anim.SetBool("Change_Slope", false);
            Debug.Log("Slopeから離れた → 滑り終了");
        }
    }
            
    


    
    public void SlimeSpeed(float s)
    {
        moveZ = s;
        Debug.Log("SlimeSpeed" + moveZ);
    }
}

