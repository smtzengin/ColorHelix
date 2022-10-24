using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController instance;

    public GameObject finishLine;

    private GameObject[] walls2, walls1;

    public Color[] colors;
    [HideInInspector]
    public Color hitColor, failColor;

    public int score;
    private int wallsSpawnNumber = 11, wallsCount = 0;
    private float z = 7;

    private bool colorBump;


    void Awake()
    {
        instance = this;
        GenerateColors();             
        PlayerPrefs.GetInt("Level", 1);
    }

    void Start()
    {
        GenerateLevel();
       
    }

    private void Update()
    {
        SumUpWalls();       
    }

    public void GenerateLevel()
    {
        GenerateColors();

        if(PlayerPrefs.GetInt("Level") == 0)
        {
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        }


        if (PlayerPrefs.GetInt("Level") >= 1 && PlayerPrefs.GetInt("Level") <= 10)
            wallsSpawnNumber = 14;
        else if (PlayerPrefs.GetInt("Level") >= 11 && PlayerPrefs.GetInt("Level") <= 50)
            wallsSpawnNumber = 17;
        else if (PlayerPrefs.GetInt("Level") >= 51 && PlayerPrefs.GetInt("Level") <= 100)
            wallsSpawnNumber = 19;
        else if (PlayerPrefs.GetInt("Level") >= 101 && PlayerPrefs.GetInt("Level") <= 150)
            wallsSpawnNumber = 20;
        else if (PlayerPrefs.GetInt("Level") >= 151 && PlayerPrefs.GetInt("Level") <= 200)
            wallsSpawnNumber = 21;
        else if (PlayerPrefs.GetInt("Level") >= 201 && PlayerPrefs.GetInt("Level") <= 400)
            wallsSpawnNumber = 22;

        else
            wallsSpawnNumber = 24;

        z = 7;

        DeleteWalls();
        colorBump = false;
        SpawnWalls();
    }

    void GenerateColors()
    {
        hitColor = colors[Random.Range(0, colors.Length)];

        failColor = colors[Random.Range(0, colors.Length)];
        while (failColor == hitColor)
            failColor = colors[Random.Range(0, colors.Length)];

        Ball.SetColor(hitColor);

    }

    public float GetFinishLineDistance()
    {
        return finishLine.transform.position.z;
    }

    void SumUpWalls()
    {
        walls1 = GameObject.FindGameObjectsWithTag("Wall1");

        if (walls1.Length > wallsCount)
            wallsCount = walls1.Length;

        if (wallsCount > walls1.Length)
        {
            wallsCount = walls1.Length;
            if (GameObject.Find("Ball").GetComponent<Ball>().perfectStar)
            {
                score += PlayerPrefs.GetInt("Level") * 2;
                GameObject.Find("Ball").GetComponent<Ball>().perfectStar = false;    
            }
            else
            {
                score += PlayerPrefs.GetInt("Level");
            }
        }
    }

    void DeleteWalls()
    {
        walls2 = GameObject.FindGameObjectsWithTag("Fail");

        if (walls2.Length > 1)
        {
            for (int i = 0; i < walls2.Length; i++)
            {
                Destroy(walls2[i].transform.parent.gameObject);
            }
        }

        Destroy(GameObject.FindGameObjectWithTag("ColorBump"));

    }

    void SpawnWalls()
    {
        for (int i = 0; i < wallsSpawnNumber; i++)
        {
            GameObject wall;

            if (Random.value <= 0.2 && !colorBump && PlayerPrefs.GetInt("Level") >= 10)
            {
                colorBump = true;
                wall = Instantiate(Resources.Load("ColorBump") as GameObject, transform.position, Quaternion.identity);
                
            }
            else if (Random.value <= 0.1 && PlayerPrefs.GetInt("Level") >= 6)
            {
                wall = Instantiate(Resources.Load("HardWalls") as GameObject, transform.position, Quaternion.identity);
             
            }
            else if (Random.value <= 0.2 && PlayerPrefs.GetInt("Level") >= 50)
            {
                wall = Instantiate(Resources.Load("HardWalls") as GameObject, transform.position, Quaternion.identity);

            }
            else if (Random.value <= 0.4 && PlayerPrefs.GetInt("Level") >= 100)
            {
                wall = Instantiate(Resources.Load("HardWalls") as GameObject, transform.position, Quaternion.identity);

            }
            else if (Random.value <= 0.7 && PlayerPrefs.GetInt("Level") >= 200)
            {
                wall = Instantiate(Resources.Load("HardWalls") as GameObject, transform.position, Quaternion.identity);

            }
            else if (i >= wallsSpawnNumber - 1 && !colorBump && PlayerPrefs.GetInt("Level") >= 3)
            {
                colorBump = true;
                wall = Instantiate(Resources.Load("ColorBump") as GameObject, transform.position, Quaternion.identity);
            }

            else
            {
                wall = Instantiate(Resources.Load("Wall") as GameObject, transform.position, Quaternion.identity);
            }



            wall.transform.SetParent(GameObject.Find("Helix").transform);
            wall.transform.localPosition = new Vector3(0, 0, z);
            float randomRotation = Random.Range(0, 360);
            wall.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, randomRotation));
            z += 7;

            if (i <= wallsSpawnNumber)
                finishLine.transform.position = new Vector3(0, 0.03f, z);
        }
    }

    
    


}
