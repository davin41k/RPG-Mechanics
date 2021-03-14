using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Resources
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        ParticleSystem particle;

        void Start()
        {
            particle = GetComponent<ParticleSystem>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!particle.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}