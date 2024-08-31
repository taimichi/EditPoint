using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStartObj : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.Simulate(Time.fixedDeltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        Physics2D.Simulate(Time.fixedDeltaTime);
    }
}
