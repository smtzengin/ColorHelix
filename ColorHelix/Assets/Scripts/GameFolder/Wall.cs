using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private GameObject wallFragment;
    private GameObject wall1, wall2;
    public GameObject perfectStar;

    private float rotationZ;
    private float rotationZMax = 180f;

    private bool smallWall;

    private void Awake()
    {
        wallFragment = Resources.Load("WallFragment") as GameObject;

    }

    private void Start()
    {
        SpawnWallFragments();
    }

    void SpawnWallFragments()
    {
        wall1 = new GameObject();
        wall2 = new GameObject();

        wall1.name = "Wall1";
        wall2.name = "Wall2";

        wall1.tag = "Wall1";
        wall2.tag = "Fail";

        wall1.transform.SetParent(transform);
        wall2.transform.SetParent(transform);

        

        wall2.AddComponent<BoxCollider>();
        wall2.GetComponent<BoxCollider>().size = new Vector3(1f, 1.85f, 0.2f);
        wall2.GetComponent<BoxCollider>().center = new Vector3(0.46f, 0, 0f);

        if(Random.value <= 0.2 && PlayerPrefs.GetInt("Level") >= 5)
        {
            smallWall = true;
        }

        if (smallWall)
            rotationZMax = 90;
        else
            rotationZMax = 180;

        for (int i = 0; i < 100; i++)
        {
            GameObject wallF = Instantiate(wallFragment, Vector3.zero, Quaternion.Euler(0, 0, rotationZ));
            rotationZ += 3.6f;

            if (rotationZ <= rotationZMax)
            {
                wallF.transform.SetParent(wall1.transform);
                wallF.gameObject.tag = "Hit";
                
            }
            else
            {
                wallF.transform.SetParent(wall2.transform);
            }
        }

        wall1.transform.localPosition = Vector3.zero;
        wall2.transform.localPosition = Vector3.zero;

        wall1.transform.localRotation = Quaternion.Euler(Vector3.zero);
        wall2.transform.localRotation = Quaternion.Euler(Vector3.zero);

        if (smallWall)
        {
            GameObject wallFragmentChild = wall1.transform.GetChild(12).gameObject;
            AddStar(wallFragmentChild);
        }

        else
        {
            GameObject wallFragmentChild = wall1.transform.GetChild(10).gameObject;
            AddStar(wallFragmentChild);
        }
        
    }

    void AddStar(GameObject wallFragmentChild)
    {
        GameObject star = Instantiate(perfectStar, transform.position, Quaternion.identity);
        star.transform.SetParent(wallFragmentChild.transform);
        star.transform.localPosition = new Vector3(0.05f, 0.75f, -0.06f);
    }
}
