using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextStart : MonoBehaviour
{
    [SerializeField] private TalkStart talkStart;
    [SerializeField] private GameObject clapper;
    [SerializeField] private GameObject textSystem;
    private bool b_start;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!b_start)
        {
            if (clapper == null)
            {
                textSystem.SetActive(true);
                b_start = true;
            }
        }
    }
}
