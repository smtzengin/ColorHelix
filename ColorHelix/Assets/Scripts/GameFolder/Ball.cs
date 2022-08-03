using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private static float forward;
    public float height = 0.58f;
    public float speed = 6;
    private bool isMove,isRising,gameOver;
    private float lerpAmount;

    private static Color currrentColor;
    private MeshRenderer meshRenderer;

    private SpriteRenderer splash;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        splash = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        isMove = false;
        SetColor(GameController.instance.hitColor);
    }

    private void Update()
    {
        print(PlayerPrefs.GetInt("Level" , 1));

        if (Touch.IsPressing() && !gameOver)
        {
            isMove = true;
            GetComponent<SphereCollider>().enabled = true;
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
        gameOver = true;
        splash.color = currrentColor;
        splash.transform.position = new Vector3(0, 0.7f, Ball.forward - 0.05f);
        splash.transform.eulerAngles = new Vector3(0, 0, Random.value * 360);
        splash.enabled = true;

        meshRenderer.enabled = false;
        GetComponent<SphereCollider>().enabled = false;
        isMove = false;
        
        yield return new WaitForSeconds(1.5f);
        gameOver = false;
        forward = 0;
        GameController.instance.GenerateLevel();
        splash.enabled = false;
        meshRenderer.enabled = true;
        
    }

    IEnumerator PlayNewLevel()
    {
        Camera.main.GetComponent<CameraFollow>().enabled = false;
        yield return new WaitForSeconds(1.5f);
        isMove = false;
        //Flash
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1); 
        Camera.main.GetComponent<CameraFollow>().enabled = true;
        Ball.forward = 0;
        GameController.instance.GenerateLevel();

    }
}
