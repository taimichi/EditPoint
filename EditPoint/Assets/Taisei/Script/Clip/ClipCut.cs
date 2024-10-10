using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClipCut : MonoBehaviour
{
    private RectTransform timebar;

    [SerializeField] private TimelineData timelineData;


    private void Awake()
    {
        timebar = GameObject.Find("Timebar").GetComponent<RectTransform>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
