using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private static float forward;
    public float height = 0.58f;
    public float speed = 6;
    private bool isMove,isRising;
    private float lerpAmount;

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
        if (isRising)
        {
            currrentColor = Color.Lerp(meshRenderer.material.color, GameObject.FindGameObjectWithTag("ColorBump").GetComponent<ColorBump>().GetColor(), lerpAmount);
            lerpAmount += Time.deltaTime;
        }
        if(lerpAmount >= 1)
        {
            isRising = false;
        }
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
            Destroy(other.transform.parent.gameObject);
        }

        if(other.tag == "ColorBump")
        {
            lerpAmount = 0;
            isRising = true;
        }

        if(other.tag == "Fail")
        {
            StartCoroutine(GameOver());
        }


        if (other.CompareTag("FinishLine"))
        {
            StartCoroutine(PlayNewLevel());
        }
    }

    IEnumerator GameOver()
    {

        GameController.instance.GenerateLevel();
        Ball.forward = 0;
        isMove = false;
        yield break;
    }

    IEnumerator PlayNewLevel()
    {
        Camera.main.GetComponent<CameraFollow>().enabled = false;
        yield return new WaitForSeconds(1.5f);
        isMove = false;
        //Flash
        //Level++
        Camera.main.GetComponent<CameraFollow>().enabled = true;
        Ball.forward = 0;
        GameController.instance.GenerateLevel();

    }
}
