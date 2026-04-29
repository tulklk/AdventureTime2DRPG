using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float resetDelay = 3f; // Thời gian để platform quay lại vị trí ban đầu
    /*public float fallDelay = 0.5f;*/ // Thời gian trước khi platform rơi
    private Vector3 initialPosition;
    //private Quaternion initialRotation;
    private Rigidbody2D rb;
    private bool isFalling = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
        //initialRotation = transform.rotation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isFalling && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall()
    {
        isFalling = true;
        //yield return new WaitForSeconds(fallDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;

        yield return new WaitForSeconds(resetDelay);
        ResetPlatform();
    }

    private void ResetPlatform()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        transform.position = initialPosition;
        //transform.rotation = initialRotation;
        isFalling = false;
    }
}