using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField] private Transform ball = null;

    void FixedUpdate()
    {
        transform.position = new Vector2(transform.position.x, ball.position.y);
    }
}
