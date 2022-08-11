using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private float cameraZ;

    private Animator anim;

    private float time,speed = 2;

    private void Awake()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

    private void Update()
    {
        if(time < 1)
        {
            time += Time.deltaTime * speed;
            cameraZ = Mathf.Lerp(transform.position.z, -2.2f, time);
        }
        else
        {
            cameraZ = Ball.GetZ() - 2.2f;
        }

        
        transform.position = new Vector3(0, 2.2f, cameraZ);
    }


    public void Flash()
    {
        anim.SetTrigger("Flash");
        cameraZ = 0;
        time = 0;
    }
}
