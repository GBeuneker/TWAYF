using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBehaviour : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        GetComponent<Renderer>().sortingOrder = 100;
    }

    // Update is called once per frame
    void Update()
    {
        // Particle system has stopped emitting and there are no particles left
        if (!GetComponent<ParticleSystem>().IsAlive())
            Destroy(gameObject);
    }
}
