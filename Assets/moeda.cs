using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moeda : MonoBehaviour
{
    private void OnDestroy()
    {
        ParticleSystem particula = GetComponent<ParticleSystem>();
        particula.Play();
    }
}
