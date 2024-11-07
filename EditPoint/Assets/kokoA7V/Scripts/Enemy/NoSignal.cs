using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NoSignal : MonoBehaviour
{
    private float timer = 0f;
    private float maxTime = 1.0f;

    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //}

        if (timer >= maxTime)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Reset();
        }

        timer += Time.fixedDeltaTime;
    }

    /// <summary>
    /// �S�ă��Z�b�g
    /// </summary>
    public void OnReLode()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// �v���C���[�̈ʒu�ƃ^�C���o�[�Ȃǈꕔ�̂݃��Z�b�g
    /// </summary>
    public void Reset()
    {
        TimeBar timeBar = GameObject.Find("Timebar").GetComponent<TimeBar>();
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        PlayerController plController = GameObject.Find("Player").GetComponent<PlayerController>();

        timeBar.OnReStart();
        gm.OnReset();
        plController.OnPlayerReset();

        this.gameObject.SetActive(false);
    }
}
