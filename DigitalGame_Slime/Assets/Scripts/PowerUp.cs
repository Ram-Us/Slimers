using Unity.VisualScripting;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    void Start()
    {
        this.gameObject.SetActive(true);
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
    }
}
