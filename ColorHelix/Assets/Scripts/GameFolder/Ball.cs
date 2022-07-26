using System.Collections;
using UnityEngine;


public class Ball : MonoBehaviour
{

    public static Ball instance;

    private static float z;

    public static Color currentColor;

    private SkinnedMeshRenderer meshRenderer;

    private BoxCollider bCollider;

    public float height = 0.58f, speed = 6;
    private float lerpAmount;

    private bool move, isRising, gameOver, displayed ,isPaused,isDead;
    
    public bool perfectStar, isFinishLevel;

    private float fastRun,currentSpeed;
    [SerializeField] private GameObject particleSpawn;
    [SerializeField] private int perfectStarCount = 0; 

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
        currentSpeed = speed;
        
        
      
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
        SpeedControl();


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
            if(PlayerPrefs.GetInt("Level") % 10 == 9 || PlayerPrefs.GetInt("Level") % 10 == 4)
            {
                //InterstitialAD.instance.RequestInterstitial();
            }
            StartCoroutine(CallConfetti());

            StartCoroutine(PlayNewLevel());
            
        }

    }

    private void OnTriggerEnter(Collider target)
    {
        if (target.gameObject.tag == "Star")
        {
            perfectStar = true;

            if(perfectStar == true)
            {
                perfectStarCount++;
                if (perfectStarCount == 5 )
                {
                    
                    StartCoroutine(FastRun());
                    
                }
            }
        }

    }


    IEnumerator PlayNewLevel()
    {
        levelCompleteSound.Play();
        Camera.main.GetComponent<CameraFollow>().enabled = false;
        yield return new WaitForSeconds(3f);
        move = false;
        Camera.main.GetComponent<CameraFollow>().Flash();
        
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        Camera.main.GetComponent<CameraFollow>().enabled = true;
        Ball.z = 0;

        GameController.instance.GenerateLevel();
        if (PlayerPrefs.GetInt("Level") % 5 == 0)
        {      
            isAdShowed = true;
            //InterstitialAD.instance.ShowInterstitial();
        }
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

    IEnumerator FastRun()
    {
        fastRun = speed;
        fastRun = fastRun * 1.25f;
        speed = fastRun;
        yield return new WaitForSeconds(2f);
        fastRun = currentSpeed;
        perfectStarCount = 0;
        speed = currentSpeed;
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

    IEnumerator CallConfetti()
    {
        particleSpawn.transform.position = GameController.instance.finishLine.transform.position;
        particleSpawn.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z + 1f);
        GameObject confetti;

        confetti = Instantiate(Resources.Load("Confetti") as GameObject, particleSpawn.transform.position, Quaternion.identity);

        yield return new WaitForSeconds(2f);

        Destroy(confetti.gameObject);

    }
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(0f, 0f, rb.velocity.z);

        if(flatVel.magnitude > speed)
        {
            Vector3 limitedVel = flatVel.normalized * speed;
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, limitedVel.z);
        }
    }
}
