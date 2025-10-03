using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using NUnit.Framework;
using TreeEditor;
using Unity.Multiplayer.Center.Common;
using UnityEngine;

public class SlimeController : MonoBehaviour
{

    private float jumpPower = 3f;
    private float sideForce = 1f;
    private Rigidbody rb;
    private bool isGrounded=true;
    private bool isSquatting = false; // しゃがみ状態フラグ
    private Vector3 PresentPosition;
    private Vector3 PresentScale;
    private Vector3 NowPosition;
    private Vector3 SquatScale;
    private SkinnedMeshRenderer smr;
    private Mesh mesh;
    private int jumpIndex;
    private int squashIndex;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        PresentPosition = new Vector3(0f, 3f, 0f);
        PresentScale = new Vector3(20f, 20f, 0f);
        SquatScale = new Vector3(12f, 10f, 7f);
        //Debug.Log(PresentPosition);

        smr = GetComponentInChildren<SkinnedMeshRenderer>();
        mesh = smr.sharedMesh;

        // 名前一覧確認
        for (int i = 0; i < mesh.blendShapeCount; i++)
            Debug.Log("Blend " + i + ": " + mesh.GetBlendShapeName(i));

        jumpIndex = smr.sharedMesh.GetBlendShapeIndex("ジャンプ");
        squashIndex = smr.sharedMesh.GetBlendShapeIndex("しゃがむ");

        Debug.Log(jumpIndex + "," + squashIndex);
    }

    // Update is called once per frame
    void Update()
    {
        // ジャンプ
        Debug.Log(isGrounded);
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            smr.SetBlendShapeWeight(jumpIndex, 100f);
            isGrounded = false;
            Debug.Log("Jump");
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            smr.SetBlendShapeWeight(jumpIndex, 0f);
            Debug.Log("UnJump");
        }

        // ジャンプ
        if (Input.GetKeyDown(KeyCode.DownArrow) && isGrounded)
        {
            smr.SetBlendShapeWeight(squashIndex, 100f);
            Debug.Log("Down");
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            smr.SetBlendShapeWeight(squashIndex, 0f);
            Debug.Log("UnDown");
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
    
