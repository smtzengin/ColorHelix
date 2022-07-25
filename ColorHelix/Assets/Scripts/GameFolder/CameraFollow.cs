using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private float cameraZ;

    private void Update()
    {
        cameraZ = Ball.GetZ() - 2.2f;
        transform.position = new Vector3(0, 2.2f, cameraZ);
    }
}
