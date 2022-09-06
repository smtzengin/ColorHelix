using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    private static float z;

    private static Color currentColor;

    //private MeshRenderer meshRenderer;
    private SkinnedMeshRenderer meshRenderer;
    //private SpriteRenderer splash;
    private BoxCollider bCollider;

    public float height = 0.58f, speed = 0.11f;
    private float lerpAmount;

    private bool move, isRising, gameOver, displayed;
    public bool perfectStar;
    
    private AudioSource failSound, hitSound, levelCompleteSound;
    private Rigidbody rb;

    void Awake()
    {
        failSound = GameObject.Find("FailSound").GetComponent<AudioSource>();
        hitSound = GameObject.Find("HitSound").GetComponent<AudioSource>();
        levelCompleteSound = GameObject.Find("LevelCompleteSound").GetComponent<AudioSource>();
        //meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer = GameObject.Find("Simple.Character").GetComponent<SkinnedMeshRenderer>();
        bCollider = GetComponent<BoxCollider>();

        rb= GetComponent<Rigidbody>();
        //splash = transform.GetChild(0).GetComponent<SpriteRenderer>();
        
    }

    void Start()
    {
        move = false;
        SetColor(GameController.instance.hitColor);
    }

    void Update()
    {
        if (Touch.IsPressing() && !gameOver)
        {
            move = true;
            bCollider.enabled = true;
        }

        if (move)
            Ball.z += speed;

        transform.position = new Vector3(0, height, Ball.z);

        displayed = false;
 

        UpdateColor();
    }

    void UpdateColor()
    {
        
        meshRenderer.sharedMaterial.color = currentColor;
        if (isRising)
        {
            currentColor = Color.Lerp(meshRenderer.material.color, GameObject.FindGameObjectWithTag("ColorBump").GetComponent<ColorBump>().GetColor()
                , lerpAmount);
            lerpAmount += Time.deltaTime;

            
        }
        if (lerpAmount >= 1)
            isRising = false;
    }

    public static float GetZ()
    {
        return Ball.z;
    }

    public static Color SetColor(Color color)
    {
        return currentColor = color;
    }

    public static Color GetColor()
    {
        return currentColor;
    }
    

    void OnCollisionEnter(Collision target)
    {
        if (target.collider.tag == "Hit")
        {
            if (perfectStar && !displayed)
            {
                displayed = true;
                GameObject pointDisplay = Instantiate(Resources.Load("PointDisplay"), transform.position, Quaternion.identity) as GameObject;
                pointDisplay.GetComponent<PointDisplay>().SetText("PERFECT +" + PlayerPrefs.GetInt("Level") * 2);
            }
            else if (!perfectStar && !displayed)
            {
                displayed = true;
                GameObject pointDisplay = Instantiate(Resources.Load("PointDisplay"), transform.position, Quaternion.identity) as GameObject;
                pointDisplay.GetComponent<PointDisplay>().SetText("+" + PlayerPrefs.GetInt("Level"));
            }
            hitSound.Play();
            Destroy(target.transform.parent.gameObject);
        }

        if (target.collider.tag == "ColorBump")
        {
            lerpAmount = 0;
            speed += 0.01f;
            isRising = true;
        }

        if (target.collider.tag == "Fail")
        {
            StartCoroutine(GameOver());
        }

        if (target.collider.CompareTag("FinishLine"))
        {
            StartCoroutine(PlayNewLevel());
        }

        if (target.collider.tag == "Star")
        {
            perfectStar = true;
        }

        if(target.collider.tag == "HitDeneme")
        {
            print("çarpýyom ya");
        }
    }

    IEnumerator PlayNewLevel()
    {
        levelCompleteSound.Play();
        Camera.main.GetComponent<CameraFollow>().enabled = false;
        yield return new WaitForSeconds(1.5f);
        move = false;
        Camera.main.GetComponent<CameraFollow>().Flash();
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        Camera.main.GetComponent<CameraFollow>().enabled = true;
        Ball.z = 0;
        GameController.instance.GenerateLevel();
    }

    IEnumerator GameOver()
    {
        failSound.Play();
        gameOver = true;
        //splash.color = currentColor;
        //splash.transform.position = new Vector3(0, 0.7f, Ball.z - 0.05f);
        //splash.transform.eulerAngles = new Vector3(0, 0, Random.value * 360);
        //splash.enabled = true;

        meshRenderer.enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        move = false;
        yield return new WaitForSeconds(1.5f);
        Camera.main.GetComponent<CameraFollow>().Flash();
        gameOver = false;
        z = 0;
        GameController.instance.GenerateLevel();
        //splash.enabled = false;
        meshRenderer.enabled = true;
    }

}
