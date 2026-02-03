using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void IncreaseSpeed(float amount)
    {
        speed += amount;
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            // .normalized * speed para regular la velocidad en diagonal
            rb.linearVelocity = (Vector3.forward * Input.GetAxis("Vertical")
            + Vector3.right * Input.GetAxis("Horizontal")).normalized * speed;
        }
    }
}