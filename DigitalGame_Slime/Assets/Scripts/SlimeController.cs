using JetBrains.Annotations;
using Unity.Multiplayer.Center.Common;
using UnityEngine;

public class SlimeController : MonoBehaviour
{

    private Rigidbody rigidbody;
    private Vector3 PresentPosition;
    private Vector3 NowPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        PresentPosition = this.transform.position;
        //Debug.Log(PresentPosition);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))
        {
            transform.localScale = new Vector3(30f, 10f, 10f);
        }
        else if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow))
        {
            transform.localScale = new Vector3(10f, 10f, 30f);
        }
    }
}
