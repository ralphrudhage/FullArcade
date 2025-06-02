using UnityEngine;

public class Cloud : MonoBehaviour
{
    private const float speed = 0.2f;
    private const float leftLimit = -12f;
    private const float rightLimit = 12f;
    private const int direction = 1;
    
    private void Start()
    {
        // direction = Random.value > 0.5f ? 1 : -1;
        // speed = Random.Range(0f, 0.1f);
    }

    private void Update()
    {
        // Move the cloud
        transform.position += Vector3.right * (direction * speed * Time.deltaTime);

        // If cloud reaches the left limit, teleport to the right and continue moving left
        if (transform.position.x <= leftLimit)
        {
            transform.position = new Vector3(rightLimit, transform.position.y, transform.position.z);
        }
        // If cloud reaches the right limit, teleport to the left and continue moving right
        else if (transform.position.x >= rightLimit)
        {
            transform.position = new Vector3(leftLimit, transform.position.y, transform.position.z);
        }
    }
}
