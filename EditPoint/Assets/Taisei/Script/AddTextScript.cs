using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTextScript : MonoBehaviour
{
    private float f_timer = 0;
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (f_timer >= 2f)
        {
            f_timer = 0;
            this.gameObject.SetActive(false);
        }
        f_timer += Time.fixedDeltaTime;
    }
}
