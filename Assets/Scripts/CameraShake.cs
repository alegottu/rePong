using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float maxDuration = 1;
    [SerializeField] private float magnitude = 1;
    public bool shake = false;
    private float duration = 0;

    private Transform cam = null;
    Vector3 originalPos = Vector3.zero;

    void Awake()
    {
        duration = maxDuration;

        if (cam == null)
        {
            cam = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        originalPos = cam.localPosition;
    }

    void Update()
    {
        if (duration > 0 && shake)
        {
            cam.localPosition = originalPos + Random.insideUnitSphere * magnitude;
            duration -= Time.deltaTime;
        }
        else if (duration <= 0)
        {
            shake = false;
            duration = maxDuration;
            cam.localPosition = originalPos;
        }
    }
}
