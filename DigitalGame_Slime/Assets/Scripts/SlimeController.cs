using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using NUnit.Framework;
using TreeEditor;
using Unity.Multiplayer.Center.Common;
using UnityEngine;

public class SlimeController : MonoBehaviour
{

    [SerializeField]
    private float jumpPower = 2f;  
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        PresentPosition = new Vector3(0f, 3f, 0f);
        PresentScale = new Vector3(12f, 12f, 12f);
        SquatScale = new Vector3(46f, 15f, 6f);
        
        //Debug.Log(PresentPosition);

    }

    // Update is called once per frame

    
    void Update()
    {

        float mouseX = Input.GetAxis("Mouse X");
        Vector3 move = new Vector3(mouseX * moveSpeed * Time.deltaTime, 0, 0);
        transform.position += move;
        squatInput = Input.anyKey; // 右クリックを押してる間しゃがむ
        anim.SetBool("Change", squatInput);
   
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

        //横伸び
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            anim.SetTrigger("HorizontalStretch");
            Debug.Log("yokonibiyon");
        }
        //縦伸び
        if (Input.GetKeyDown(KeyCode.S) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            anim.SetTrigger("VerticalStretch");
            Debug.Log("tatenobin");
        }

        if (Input.GetMouseButtonUp(1) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
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
    
