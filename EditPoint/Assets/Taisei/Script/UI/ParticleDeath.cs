using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDeath : MonoBehaviour
{
    private float timer = 0f;
    private const float lifeTime = 0.5f;

    void Update()
    {
        if(timer >= lifeTime)
        {
            timer = 0;
            Destroy(this.gameObject);
        }
        timer += Time.deltaTime;
    }
}
