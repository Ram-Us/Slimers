using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using NUnit.Framework;
using TreeEditor;
using Unity.Multiplayer.Center.Common;
using UnityEngine;

public class SlimeController : MonoBehaviour
{

    private float jumpPower = 3f;
    //private float sideForce = 1f;
    private Rigidbody rb;
    private Animator anim;
    private bool isGrounded=true;
    //private bool isSquatting = false; // しゃがみ状態フラグ
    private Vector3 PresentPosition;
    private Vector3 PresentScale;
    private Vector3 NowPosition;
    private Vector3 SquatScale;
    private SkinnedMeshRenderer smr;
    private Mesh mesh;
    private int jumpIndex;
    private int squashIndex;
    private bool squatInput = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim=GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        PresentPosition = new Vector3(0f, 3f, 0f);
        PresentScale = new Vector3(20f, 20f, 0f);
        SquatScale = new Vector3(12f, 10f, 7f);
        //Debug.Log(PresentPosition);

        smr = GetComponentInChildren<SkinnedMeshRenderer>();
        mesh = smr.sharedMesh;

        // 名前一覧確認
        //for (int i = 0; i < mesh.blendShapeCount; i++)
            //Debug.Log("Blend " + i + ": " + mesh.GetBlendShapeName(i));

        //jumpIndex = smr.sharedMesh.GetBlendShapeIndex("ジャンプ");
        //squashIndex = smr.sharedMesh.GetBlendShapeIndex("しゃがむ");

        //Debug.Log(jumpIndex + "," + squashIndex);
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
        squatInput = Input.GetKey(KeyCode.DownArrow); // ↓を押してる間しゃがむ
        anim.SetBool("isSquatting", squatInput);
        Debug.Log(squatInput);
        // ジャンプ
        //Debug.Log(isGrounded);
        if (Input.GetKey(KeyCode.UpArrow) && isGrounded)
        {
            anim.SetTrigger("Jump");
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

            //smr.SetBlendShapeWeight(jumpIndex, 100f);
            isGrounded = false;
            Debug.Log("Jump");
        }


        // ジャンプ
        if (Input.GetKeyDown(KeyCode.DownArrow) && isGrounded)
        {
            anim.SetTrigger("Squash");
            Debug.Log("Down");
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            squatInput = false;
        }/*
      
         // 右ジャンプ
         if (Input.GetKeyDown(KeyCode.RightArrow) && isGrounded)
         {
             rb.AddForce(new Vector3(sideForce, jumpPower, 0f), ForceMode.Impulse);
             isGrounded = false;
         }
         // 左ジャンプ
         if (Input.GetKeyDown(KeyCode.LeftArrow) && isGrounded)
         {
             rb.AddForce(new Vector3(-sideForce, jumpPower, 0f), ForceMode.Impulse);
             isGrounded = false;
         }*/
        // しゃがみ（押してる間ずっと）
        /*if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            isSquatting = true;
            // 縮んだ分だけ下に移動（接地させる）
            float offset = (SquatScale.z - PresentScale.z) / 2f;
            transform.position = PresentPosition - new Vector3(0, offset, 0);
        }
        else
        {
            isSquatting = false;
            transform.position = PresentPosition;
        }*/

        // ▼スケール処理
        /*if (!isGrounded)
        {
            
        }
        else
        {
            transform.localScale = Vector3.Lerp(
                transform.localScale,
                PresentScale,
                Time.deltaTime * 10f
            );
        }*/



        /*if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))
        {
            transform.localScale = new Vector3(30f, 10f, 5f);
        }
        else if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow))
        {
            transform.localScale = new Vector3(3f, 10f, 35f);
            //transform.position = new Vector3(0f, 3.244f, 0f);
        }*/
        //transform.localScale = PresentScale;
        //transform.position = PresentPosition;


    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        /*if (collision.gameObject.CompareTag("Wall"))
        {
            transform.position = PresentPosition;
        }*/
    }
}
    
