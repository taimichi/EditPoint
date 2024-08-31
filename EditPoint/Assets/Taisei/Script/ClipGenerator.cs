using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClipGenerator : MonoBehaviour
{
    [SerializeField] private GameObject timeBar;
    [SerializeField] private GameObject ClipPrefab;

    private int i_createCount = 0;


    void Start()
    {
    }

    void Update()
    {

    }

    //新しいクリップを生成
    public void ClipGene()
    {
        i_createCount++;
        Vector3 clipPos = new Vector3(timeBar.transform.position.x, 0, 0);
        GameObject clip = Instantiate(ClipPrefab, clipPos, timeBar.transform.rotation, this.gameObject.transform);
        clip.name = "CreateClip" + i_createCount;
        clip.tag = "CreateClip";

    }


    public int ReturnCount()
    {
        return i_createCount;
    }
}
