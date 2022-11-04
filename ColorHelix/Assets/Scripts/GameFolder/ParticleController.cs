using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    private ParticleSystem particle;
    private float particleZ;

    private void Start()
    {
        //particle.startColor = Ball.currentColor;
    }
    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }


    private void Update()
    {
        particleZ = Ball.GetZ() - 1f;
        transform.position = new Vector3(0, 1f, particleZ);
    }
}
