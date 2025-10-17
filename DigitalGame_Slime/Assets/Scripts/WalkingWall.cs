using UnityEngine;

public class WalkingWall : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;

    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 targetPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(startPosition, targetPosition, (Mathf.Sin(Time.time * moveSpeed) + 1f) / 2f);
    }
}
