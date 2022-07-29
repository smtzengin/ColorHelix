using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBump : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Color color;


    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        transform.parent = null;
        transform.rotation = Quaternion.EulerRotation(Vector3.zero);
        color = GameController.instance.colors[Random.Range(0, GameController.instance.colors.Length)];
        meshRenderer.material.color = color;
    }

    public Color GetColor()
    {
        return this.color;
    }

}
