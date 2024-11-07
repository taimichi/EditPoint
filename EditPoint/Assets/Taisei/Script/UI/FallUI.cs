using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FallUI : MonoBehaviour
{
    [SerializeField] private GameObject FallCanvas;
    [SerializeField] private GameObject FadeInImage;
    private RectTransform FIRect;
    [SerializeField] private GameObject FadeOutImage;
    private RectTransform FORect;

    private Vector2 FIstartSize;
    private Vector2 FOstartSize;

    private Vector3 FIstartPos;
    private Vector3 FOstartPos;

    private float fadeTimer = 0f;
    private const float MAX_FADETIME = 0.3f;

    private const float MAX_SIZE = 2750f;
    private const float MIN_SIZE = 540f;

    private float offset = 0;
    private float fadeSpeedSize;

    private const float MAX_POSY = 300f;
    private const float MIN_POSY = -800f;

    private float dis = 0;
    private float fadeSpeedPos;

    private bool startFade = false;
    /// <summary>
    /// false = ˆÃ“]  true = –¾“]
    /// </summary>
    private bool fade = false;

    void Start()
    {
        offset = MAX_SIZE - MIN_SIZE;
        dis = MAX_POSY - MIN_POSY;

        fadeSpeedSize = offset / MAX_FADETIME;
        fadeSpeedPos = dis / MAX_FADETIME;

        FIRect = FadeInImage.GetComponent<RectTransform>();
        FORect = FadeOutImage.GetComponent<RectTransform>();

        FIstartSize = FIRect.sizeDelta;
        FIstartPos = FadeInImage.transform.localPosition;
        FOstartSize = FORect.sizeDelta;
        FOstartPos = FadeOutImage.transform.localPosition;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            FadeStart();
        }

        if (startFade)
        {
            if (!fade)
            {
                Fall_FadeIn();
            }
            else
            {
                Fall_FadeOut();
            }
        }
    }

    /// <summary>
    /// ˆÃ“]
    /// </summary>
    private void Fall_FadeIn()
    {
        FIRect.sizeDelta += new Vector2(0, fadeSpeedSize * Time.deltaTime);
        FIRect.localPosition += new Vector3(0, fadeSpeedPos * Time.deltaTime, 0);
        if (fadeTimer >= MAX_FADETIME)
        {
            FadeInImage.SetActive(false);
            FadeOutImage.SetActive(true);
            fade = true;
            fadeTimer = 0;
        }
        fadeTimer += Time.deltaTime;
    }

    /// <summary>
    /// –¾“]
    /// </summary>
    private void Fall_FadeOut()
    {
        FORect.sizeDelta -= new Vector2(0, fadeSpeedSize * Time.deltaTime);
        FORect.localPosition += new Vector3(0, fadeSpeedPos * Time.deltaTime, 0);
        if (fadeTimer >= MAX_FADETIME)
        {
            FadeInImage.SetActive(true);
            FadeOutImage.SetActive(false);

            FIRect.sizeDelta = FIstartSize;
            FadeInImage.transform.localPosition = FIstartPos;
            FORect.sizeDelta = FOstartSize;
            FadeOutImage.transform.localPosition = FOstartPos;

            fade = false;
            startFade = false;
            fadeTimer = 0;

            FallCanvas.SetActive(false);
        }
        fadeTimer += Time.deltaTime;
    }

    public void FadeStart()
    {
        FallCanvas.SetActive(true);
        startFade = true;
    }
}
