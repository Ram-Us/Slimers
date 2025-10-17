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
    float moveZ = 2f;
    [SerializeField] private float slideForce = 5f;
    public float MoveZ => moveZ;
    //private float sideForce = 1f;
    private Rigidbody rb;
    private Animator anim;
    private bool isGrounded = true;
    private bool isMoved_Right = false;
    private bool isMoved_Left = false;
    private bool isSliding = false;
    [SerializeField] private float slideSpeed = 10f;
    //UnityEditor.TransformWorldPlacementJSON:{"position":{"x":0.270501820966720581},"scale":{"x":0.08333331346511841,"y":0.08333331346511841,"z":0.08333331346511841}}9414424896,"y":5.71790075302124,"z":79.584228515625},"rotation":{"x":0.16977959871292115,"y":0.0,"z":0.0,"w":0.9854

    private Vector3 slideDirection;

    // 追加: 滑り終了判定用パラメータ
    [SerializeField] private float slopeStopAngle = 10f; // この角度以下なら「平ら」と見なして滑り終了
    [SerializeField] private float slideFriction = 0.90f; // 滑り速度の減衰係数（毎FixedUpdate）
    [SerializeField] private float minSlideSpeed = 0.05f; // これ以下になったら滑りを止める
    private float currentSlideSpeed;
    private float baseSlideSpeed;
    private bool isFinished = true;

    AnimatorStateInfo stateInfo;
    //private bool isSquatting = false; // しゃがみ状態フラグ
    private bool squatInput = false;

    private Vector3 PresentCameraTransform = new Vector3(0.0815f, 0.00069f, 0.032f);
    private Vector3 PresentCameraoffset = new Vector3(0, -94.71f, -90f);//オイラー角
    private Vector3 MovingCameraTransform = new Vector3(0.082f,0.001f,0.06f);
    private Vector3 MovingCameraoffset = new Vector3(1.02f,250.3f,270f);//オイラー角
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Camera MainCamera;
    private Vector3 preLocal;
    private Vector3 moveLocal;

    private Vector3 PresentPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        baseSlideSpeed = slideSpeed;
        currentSlideSpeed = baseSlideSpeed;
        
        MainCamera.transform.localPosition = PresentCameraTransform;
        MainCamera.transform.localEulerAngles = PresentCameraoffset;
        preLocal = PresentCameraoffset;
        moveLocal = MovingCameraoffset;

        //localRotaionはQuaternionで管理されているので、eulerAnglesで確認可能
        Debug.Log("Before" + MainCamera.transform.localRotation + " " + MainCamera.transform.localEulerAngles + " "+PresentCameraoffset);
        //Debug.Log(PresentPosition);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal"); // A(-1) ～ D(+1)
        float moveZ = Input.GetAxis("Vertical");   // S(-1) ～ W(+1)
        
        // 滑っているときは滑り専用の処理（速度を減衰させつつ移動）
         // 移動方向をベクトルで作成
         Vector3 moveDir = new Vector3(moveX, 0, moveZ);
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
         

         if (isSliding)
         {
            // 重力を加える
            Vector3 gravity = Physics.gravity; // Unityの重力を取得
            rb.AddForce(gravity * 0.5f, ForceMode.Acceleration); // 重力をスケールして加える

            // 滑り方向に速度を加える
            rb.MovePosition(transform.position + slideDirection * currentSlideSpeed * Time.deltaTime);

            // 滑り速度を減衰
            currentSlideSpeed *= slideFriction;
            Debug.Log(currentSlideSpeed);

            if (currentSlideSpeed < minSlideSpeed)
            {
                isSliding = false;
                currentSlideSpeed = baseSlideSpeed;
                Debug.Log("滑りが減衰して終了しました");
            }
         }
         else
        {
             transform.position += moveDir * moveSpeed * Time.deltaTime;
         }
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
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Space"))
        {

            isGrounded = true;
            anim.ResetTrigger("Jump");
            anim.ResetTrigger("RightJump");
            anim.ResetTrigger("LeftJump");
            PresentPosition = this.transform.position;
        }
        if (collision.gameObject.CompareTag("Block"))
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 2f);
        }
        if (collision.gameObject.CompareTag("Fall"))
        {
            transform.position = new Vector3(PresentPosition.x, PresentPosition.y + 2f, PresentPosition.z);
            
        }
        if (collision.gameObject.CompareTag("Slope") && isFinished)
        {
            Debug.Log("Before" + MainCamera.transform.localPosition + " " + MainCamera.transform.localEulerAngles);
            MainCamera.transform.position = Vector3.Lerp(PresentCameraTransform, MovingCameraTransform, smoothSpeed);
            MainCamera.transform.rotation = Quaternion.Euler(Vector3.Lerp(PresentCameraoffset, MovingCameraoffset, smoothSpeed));
            Debug.Log("After" + MainCamera.transform.localPosition + " " + MainCamera.transform.localEulerAngles);

            // 坂に乗ったら滑り開始。滑り速度をリセット
            isSliding = true;
            currentSlideSpeed = baseSlideSpeed*3f; // 初期速度を3倍に
            anim.SetTrigger("Slope");
            anim.SetBool("Change_Slope", isSliding);
            Debug.Log("Slopeに乗ったよ (滑り開始)");
        }

        /*if (collision.gameObject.CompareTag("Wall"))
        {
            transform.position = PresentPosition;
        }*/
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Slope"))
        {

            
            // 坂の法線から滑る方向を計算（重力方向に沿って）
            ContactPoint contact = collision.contacts[0];
            Vector3 normal = contact.normal;

            // 坂の下方向（＝接地面の傾きに沿った重力方向）
            slideDirection = Vector3.Cross(Vector3.Cross(normal, Vector3.down), normal).normalized;
            isSliding = true;
            anim.SetBool("Change_Slope", isSliding);
            // 追加: 接触法線から傾斜角を計算し、十分に平らになったら滑り終了
            float slopeAngle = Vector3.Angle(normal, Vector3.up);
            if (slopeAngle < slopeStopAngle)
            {
                isSliding = false;
                currentSlideSpeed = baseSlideSpeed;
                anim.SetBool("Change_Slope", isSliding);
                Debug.Log($"Slopeが平らになったので滑り終了 (angle={slopeAngle:F1})");
            }
        }
            
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Slope"))
        {
            Debug.Log("Before" + MainCamera.transform.localPosition + " " + MainCamera.transform.localEulerAngles);
            MainCamera.transform.position = Vector3.Lerp(MovingCameraTransform, PresentCameraTransform, smoothSpeed);
            MainCamera.transform.rotation = Quaternion.Euler(Vector3.Lerp(MovingCameraoffset, PresentCameraoffset, smoothSpeed));
            isSliding = false;
            anim.SetBool("Change_Slope", isSliding);
            Debug.Log("Slopeから離れたよ");
            Debug.Log("After" + MainCamera.transform.localPosition + " " + MainCamera.transform.localEulerAngles);
        }
            
    }


    
    public void SlimeSpeed(float s)
    {
        moveZ = s;
        Debug.Log("SlimeSpeed" + moveZ);
    }
}

