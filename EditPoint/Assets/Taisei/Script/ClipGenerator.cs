using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipGenerator : MonoBehaviour
{
    [SerializeField] private GameObject timeBar;
    [SerializeField] private GameObject ClipPrefab;
    private BlockCreater blockCreater;


    void Start()
    {
        blockCreater = GameObject.Find("BlockCreater").GetComponent<BlockCreater>();   
    }

    void Update()
    {
        
    }

    public void ClipGene()
    {
        GameObject clip = Instantiate(ClipPrefab, timeBar.transform.position, timeBar.transform.rotation, this.gameObject.transform);
        clip.name = "CreateClip" + blockCreater.ReturnBlockCount();

    }
}
