using UnityEngine.SceneManagement;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Range(-0.3f, 0.3f)] public float xVel = 0;
    [Range(-0.03f, 0.03f)] public float yVel = 0;
    [SerializeField] [Range(1, 1.5f)] private float force = 1;
    public float addForce = 0;

    [SerializeField] private Player player = null;
    [SerializeField] private Transform[] paddle = null;
    [SerializeField] public Transform bounds = null;

    [HideInInspector] public bool collisionLeft = false;
    [HideInInspector] public bool collisionRight = false;
    [HideInInspector] public bool passHalf = false;

    [SerializeField] private AudioSource hit = null;
    [SerializeField] private AudioSource powerHit = null;

    void Start()
    {
        xVel = Random.Range(0.15f, 0.3f);
        yVel = Random.Range(0.025f, 0.05f);
    }

    void Update()
    {
        xVel = Mathf.Abs(xVel) >= 1.5f ? xVel > 0 ? 1.5f : -1.5f : xVel;
        yVel = Mathf.Abs(yVel) >= 1.5f ? yVel > 0 ? 1.5f : -1.5f : yVel;

        if (player.chargeSlider.value > 0.5f && collisionLeft)
        {
                powerHit.PlayOneShot(powerHit.clip);
        }
        else if (collisionLeft || collisionRight)
        {
                hit.PlayOneShot(hit.clip);
        }
    }

    void FixedUpdate()
    {
        transform.position = new Vector2(transform.position.x + xVel, transform.position.y + yVel);

        if (inRangeX(paddle[0], transform) && inRangeY(paddle[0], transform) || inRangeX(paddle[1], transform) && inRangeY(paddle[1], transform))
        {
            if (transform.position.y >= bounds.localScale.y / 2 || transform.position.y <= -bounds.localScale.y / 2)
            {
                xVel = -xVel * (force + getAddedForce(paddle[0], transform, player.chargeSlider.value));
                yVel = -yVel * (force + getAngle(paddle[0], transform));
            }
            else
            {
                xVel = !passHalf ? -xVel * (force + getAddedForce(paddle[0], transform, player.chargeSlider.value)) : -xVel * force;
                yVel = yVel * force;
            }
        }

        collisionLeft = inRangeX(paddle[0], transform) && inRangeY(paddle[0], transform) ? true : false;
        collisionRight = inRangeX(paddle[1], transform) && inRangeY(paddle[1], transform) ? true : false;
        passHalf = (transform.position.x > bounds.position.x) ? true : false;

        if (transform.position.x < -bounds.localScale.x / 2)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        if (transform.position.x > bounds.localScale.x / 2)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }
    }

    public float getAngle(Transform x, Transform y)
    {
        if (inRangeY(x, y) && inRangeX(x, y))
        {
            return x.position.y - y.position.y;
        }
        else
        {
            return 0;
        }
    }

    public float getAddedForce(Transform x, Transform y, float z)
    {
        if (inRangeY(x, y) && inRangeX(x, y))
        {
            return z * Mathf.Abs(xVel);
        }
        else
        {
            return 0;
        }
    }

    public bool inRangeY (Transform x, Transform y)
    {
        float margin = (x.localScale.y / 2f) + (y.localScale.y / 2);
        if (y.position.y <= x.position.y + margin && y.position.y >= x.position.y - margin)
        {
            return true;
        }
        return false;
    }

    public bool inRangeX(Transform x, Transform y)
    {
        float margin = (x.localScale.x / 2) + (y.localScale.x / 2);
        float marginLeft = (x == paddle[0]) ? margin * 10 : margin;
        //if the ball is going too fast to register, only on the player side (left)

        if (y.position.x <= x.position.x + margin && y.position.x >= x.position.x - marginLeft)
        {
            return true;
        }
        return false;
    }
}
