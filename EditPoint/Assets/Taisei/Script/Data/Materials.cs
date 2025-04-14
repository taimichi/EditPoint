using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LayerMaterials")]
public class Materials : ScriptableObject
{
    public List<Material> MaterialData = new List<Material>();
}
