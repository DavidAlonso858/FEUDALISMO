using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player;

    private Vector3 offset;

    private float smoothSpeed = 5f;

    private void Awake()
    {
        // asigna el jugador con el tag asignado
        player = GameObject.FindGameObjectWithTag("Player").transform;
        offset = transform.position - player.position;
    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.position + offset, smoothSpeed * Time.deltaTime);
    }
}
