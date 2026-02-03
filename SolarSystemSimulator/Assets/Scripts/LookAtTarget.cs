using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    private Camera _cam;

    private void Awake()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        transform.LookAt(_cam.transform.position);
        transform.Rotate(0f, 180f, 0f);
    }
}
