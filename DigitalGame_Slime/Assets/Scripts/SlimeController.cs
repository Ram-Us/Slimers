using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using NUnit.Framework;
using TreeEditor;
using Unity.Multiplayer.Center.Common;
using UnityEngine;

public class SlimeController : MonoBehaviour
{

    private float jumpPower = 3f;  
    [SerializeField]
    private float moveSpeed = 2f;
    //private float sideForce = 1f;
    private Rigidbody rb;
    private Animator anim;
    private bool isGrounded=true;
    //private bool isSquatting = false; // しゃがみ状態フラグ
    private Vector3 PresentPosition;
    private Vector3 PresentScale;
    private Vector3 SquatScale;

    private bool squatInput = false;
    private bool sideSquat = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim=GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        PresentPosition = new Vector3(0f, 3f, 0f);
        PresentScale = new Vector3(15f, 15f, 15f);
        SquatScale = new Vector3(46f, 15f, 6f);
        //Debug.Log(PresentPosition);

    }

    // Update is called once per frame
    void Update()
    {


        /*AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0); // 0 = Base Layer

        if (stateInfo.IsName("Slime_Idle"))
        {
            Debug.Log("いまIdle中！");
        }
        else if (stateInfo.IsName("Slime_Jump"))
        {
            Debug.Log("いまJump中！");
        }
        else if (stateInfo.IsName("Slime_Squash"))
        {
            Debug.Log("いまSquash中！");
        }*/

        float mouseX = Input.GetAxis("Mouse X");
        Vector3 move = new Vector3(mouseX * moveSpeed * Time.fixedDeltaTime, 0, 0);
        rb.MovePosition(rb.position + move);

        squatInput = Input.GetMouseButton(1); // 右クリックを押してる間しゃがむ
        anim.SetBool("isSquatting", squatInput);
        //Debug.Log(squatInput);
        // ジャンプ
        //Debug.Log(isGrounded);
        if (Input.GetMouseButton(0) && isGrounded)
        {
            anim.SetTrigger("Jump");
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

            //smr.SetBlendShapeWeight(jumpIndex, 100f);
            isGrounded = false;
            Debug.Log("Jump");
        }


        //しゃがむ
        if (Input.GetMouseButtonDown(1) && isGrounded)
        {
            anim.SetTrigger("Squash");
            Debug.Log("Down");
        }
         if (Input.GetMouseButtonUp(1))
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
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            anim.ResetTrigger("Jump");
        }
        /*if (collision.gameObject.CompareTag("Wall"))
        {
            transform.position = PresentPosition;
        }*/
    }
}
    
