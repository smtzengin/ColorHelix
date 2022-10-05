using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{

    public static Ball instance;

    private static float z;

    public static Color currentColor;

    private SkinnedMeshRenderer meshRenderer;

    private BoxCollider bCollider;

    public float height = 0.58f, speed = 6;
    private float lerpAmount;

    private bool move, isRising, gameOver, displayed ,isPaused, isDead;
    
    public bool perfectStar, isFinishLevel;

    [SerializeField] private bool isAdShowed;

    public bool Displayed
    {
        get { return displayed; }    
        set { displayed = value; }
    }

    

    
    private AudioSource failSound, hitSound, levelCompleteSound;
    private Rigidbody rb;

    
    [SerializeField] private Animator anim; 
    [SerializeField] private GameObject touch;
    void Awake()
    {
        instance = this;

        failSound = GameObject.Find("FailSound").GetComponent<AudioSource>();
        hitSound = GameObject.Find("HitSound").GetComponent<AudioSource>();
        levelCompleteSound = GameObject.Find("LevelCompleteSound").GetComponent<AudioSource>();
        
        meshRenderer = GameObject.Find("Simple.Character").GetComponent<SkinnedMeshRenderer>();
        bCollider = GetComponent<BoxCollider>();

        rb= GetComponent<Rigidbody>();
        isAdShowed = false;
        
    }

    void Start()
    {
        move = false;
        isDead = false;
        isFinishLevel = false;
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
            Ball.z += speed * 0.025f;
            
        

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
        if (target.gameObject.tag == "Hit")
        {
            if (perfectStar && !displayed)
            {
                displayed = true;
                GameObject pointDisplay = Instantiate(Resources.Load("PointDisplayPerfect"), transform.position, Quaternion.identity) as GameObject;
                pointDisplay.GetComponent<PointDisplay>().SetText("PERFECT +" + PlayerPrefs.GetInt("Level") * 2);
            }
            else if (!perfectStar && !displayed)
            {
                displayed = true;
                GameObject pointDisplay = Instantiate(Resources.Load("PointDisplayPerfect"), transform.position, Quaternion.identity) as GameObject;
                pointDisplay.GetComponent<PointDisplay>().SetText("+" + PlayerPrefs.GetInt("Level"));
            }
            hitSound.Play();
            Destroy(target.transform.parent.gameObject);
        }

        if (target.gameObject.tag == "ColorBump")
        {
            lerpAmount = 0;
            speed += 0.1f;
            PlayerPrefs.SetFloat("Speed", speed);
            isRising = true;
        }

        if (target.gameObject.tag == "Fail")
        {
            StartCoroutine(GameOver());
            
        }

        if (target.gameObject.CompareTag("FinishLine"))
        {
            isFinishLevel = true;
            if(PlayerPrefs.GetInt("Level") % 5 == 1)
            {
               
            }
            InterstitialAD.instance.RequestInterstitial();
            StartCoroutine(PlayNewLevel());
        }

    }

    private void OnTriggerEnter(Collider target)
    {
        if (target.gameObject.tag == "Star")
        {
            perfectStar = true;
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
        if (PlayerPrefs.GetInt("Level") % 5 == 0)
        {
            
            isAdShowed = true;
        }
        InterstitialAD.instance.ShowInterstitial();

        isFinishLevel = false;
    }

    IEnumerator GameOver()
    {
        failSound.Play();
        gameOver = true;   
        GetComponent<BoxCollider>().enabled = false;
        move = false;
        isDead = true;
        anim.SetBool("isDead",true);

        yield return new WaitForSeconds(1.5f);
        Camera.main.GetComponent<CameraFollow>().Flash();
        gameOver = false;
        isDead = false;
        anim.SetBool("isDead", false);
        z = 0;
        GameController.instance.GenerateLevel();  
        meshRenderer.enabled = true;
    }

    
    public void PauseMove() 
    {
        move = false;
        touch.SetActive(false);
    }

    public void ResumeMove()
    {
        move = true;
        touch.SetActive(true);
    }
}
