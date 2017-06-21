using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    Camera camera;

    float shakeAmount = 0;

    void Awake()
    {
        camera = GetComponent<Camera>();
    }

    public void Shake(float amt, float length)
    {
        shakeAmount = amt;
        InvokeRepeating("DoShake", 0, 0.01f);
        Invoke("StopShake", length);
    }

    void DoShake()
    {
        if (shakeAmount > 0)
        {
            Vector3 camPos = camera.transform.position;

            float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
            float offsetY = Random.value * shakeAmount * 2 - shakeAmount;
            camPos.x = offsetX;
            camPos.y = offsetY;
            camPos.z = 0f;
            camera.transform.localPosition = camPos;
        }
    }

    void StopShake()
    {
        CancelInvoke("DoShake");
        camera.transform.localPosition = Vector3.zero;
    }

}
