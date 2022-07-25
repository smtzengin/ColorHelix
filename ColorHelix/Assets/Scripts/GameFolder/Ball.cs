using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private static float forward;
    public float height = 0.58f;
    public float speed = 6;
    private bool isMove;

    private void Start()
    {
        isMove = false;
    }

    private void Update()
    {

        if (Touch.IsPressing())
        {
            isMove = true;
        }

        if (isMove)
            Ball.forward += speed * 0.025f;

        transform.position = new Vector3(0, height, Ball.forward);
    }

    public static float GetZ()
    {
        return forward;
    }
}
