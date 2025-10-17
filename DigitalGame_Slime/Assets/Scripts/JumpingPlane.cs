using UnityEngine;

public class JumpingPlane : MonoBehaviour
{
    [SerializeField]
    private float JumpSpeed = 100f;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * JumpSpeed,ForceMode.Impulse);
        }
    }
}
