using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelixCylinder : MonoBehaviour
{
    private GameObject helix;

    private void Awake()
    {
        helix = GameObject.Find("Helix");
    }

    private void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, helix.gameObject.transform.eulerAngles.z % 25);
    }
}
