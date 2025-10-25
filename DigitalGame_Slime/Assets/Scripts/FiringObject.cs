using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class FiringObject : MonoBehaviour
{
    [SerializeField] private Vector3 StartPoint,EndPoint;
    [SerializeField] private float FireSpeed = 10f;
    [SerializeField] private GameObject cutter;

    void Start()
    {
        cutter.SetActive(false);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            cutter.SetActive(true);
            transform.position = Vector3.Lerp(StartPoint, EndPoint, (Mathf.Sin(Time.time * FireSpeed) + 1f) / 2f);
            Debug.Log("発射"+cutter.name);
        }
    }
}
