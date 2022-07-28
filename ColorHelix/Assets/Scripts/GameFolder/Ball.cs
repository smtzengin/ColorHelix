using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private static float forward;
    public float height = 0.58f;
    public float speed = 6;
    private bool isMove;

    private static Color currrentColor;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

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

        UpdateColor();
    }

    void UpdateColor()
    {
        meshRenderer.sharedMaterial.color = currrentColor;
    }

    public static float GetZ()
    {
        return forward;
    }

    public static Color SetColor(Color color)
    {
        return currrentColor = color;
    }
    public static Color GetColor(Color color)
    {
        return currrentColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Hit")
        {
            print("WE HIT THE WALL");
        }

        if(other.tag == "Fail")
        {
            print("GAME OVER!");
        }


        if (other.CompareTag("FinishLine"))
        {
            print("LevelUP!");
        }
    }
}
