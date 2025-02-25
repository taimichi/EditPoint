using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapEffect : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject Trail;
    [SerializeField] private GameObject TapEffectPrefab;
    private Vector3 pos;

    private bool isCountStart = false;
    private float Count = 0;
    private const float MAX_TIME = 0.5f;    //トレイル開始時間まで(秒単位)

    private void Start()
    {
        pos = _camera.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 1;
        Trail.transform.position = pos;

        Trail.SetActive(false);
    }

    void Update()
    {
        if (isCountStart)
        {
            Count += Time.deltaTime;
        }
        pos = _camera.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 1;
        InputKey();
    }

    /// <summary>
    /// キーボードやクリックしたときの処理
    /// </summary>
    private void InputKey()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Instantiate(TapEffectPrefab, pos, Quaternion.identity);
            Trail.transform.position = pos;
            isCountStart = true;
        }

        if (Input.GetMouseButton(0))
        {
            if (Count >= MAX_TIME)
            {
                Trail.transform.position = pos;
                if (!Trail.activeSelf)
                {
                    Trail.SetActive(true);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isCountStart = false;
            Trail.SetActive(false);
        }
    }
}
