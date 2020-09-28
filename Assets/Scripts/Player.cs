using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private Ball ballControl = null;

    [SerializeField] private float speed = 1;
    [SerializeField] private float chargeAdd = 1;
    [SerializeField] private float chargeDecrease = 1;
    private float yVel = 0;

    [SerializeField] private AudioSource collapse = null;
    [SerializeField] private AudioSource chargeSound = null;
    [SerializeField] private float changePitch = 0;
    public Slider chargeSlider = null;
    private bool charging = false;

    private void Update()
    {
        yVel = Input.GetAxis("Vertical") * Mathf.Abs(ballControl.xVel);
        Charge(Input.GetButton("Jump"));
    }

    private void FixedUpdate()
    {
        Move(yVel);
    }

    private void Move(float y)
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + y * speed);
    }

    private void Charge(bool charge)
    {
        if (charge)
        {
            charging = true;

            chargeSound.enabled = true;
            chargeSound.loop = true;
            chargeSound.pitch += changePitch * Mathf.Abs(ballControl.xVel);

            chargeSlider.value += chargeAdd * Mathf.Abs(ballControl.xVel);
        }

        if (charging)
        {
            yVel = 0;
            Time.timeScale = ballControl.xVel > 1.25f ? 0.25f : 1f;

            if (!charge)
            {
                chargeSlider.value -= chargeDecrease * Mathf.Abs(ballControl.xVel);

                chargeSound.pitch -= changePitch * Mathf.Abs(ballControl.xVel) * 10;
            }

            if (chargeSlider.value == 0)
            {
                charging = false;

                collapse.PlayOneShot(collapse.clip);
                chargeSound.enabled = false;
                chargeSound.pitch = 1;

                Time.timeScale = 1;
            }
        }
    }
}

