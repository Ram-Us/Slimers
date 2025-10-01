using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using NUnit.Framework;
using Unity.Multiplayer.Center.Common;
using UnityEngine;

public class SlimeController : MonoBehaviour
{

    private float jumpPower = 3f;
    private float sideForce = 1f;
    private Rigidbody rb;
    private bool isGrounded=true;
    private Vector3 PresentPosition;
    private Vector3 PresentScale;
    private Vector3 NowPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        PresentPosition = new Vector3(0f, 3f, 0f);
        PresentScale = new Vector3(10f, 10f, 10f);
        //Debug.Log(PresentPosition);
    }

    // Update is called once per frame
    void Update()
    {
        // ジャンプ
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isGrounded = false;
        }
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
        }
        



        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))
        {
            transform.localScale = new Vector3(30f, 10f, 5f);
        }
        else if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow))
        {
            transform.localScale = new Vector3(3f, 10f, 35f);
            //transform.position = new Vector3(0f, 3.244f, 0f);
        }
        //transform.localScale = PresentScale;
        //transform.position = PresentPosition;


    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            transform.position = PresentPosition;
        }
    }
}
    
