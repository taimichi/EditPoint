using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTextManager : MonoBehaviour
{
    [SerializeField] private GameObject AddText;

    public void AddObj()
    {
        AddText.SetActive(true);
    }
}
