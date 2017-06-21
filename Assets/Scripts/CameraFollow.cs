using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    float xMax = 38.0f;
    float xMin = -31.0f;
    Camera playerCam;

    // Use this for initialization
    void Start()
    {
        playerCam = transform.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //playerCam.orthographicSize = (Screen.height / 100f) / 1.5f;

        if (target)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, 0.1f) + new Vector3(0, 0.2f, -20);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, xMin, xMax), transform.position.y, transform.position.z);
        }
    }
}
