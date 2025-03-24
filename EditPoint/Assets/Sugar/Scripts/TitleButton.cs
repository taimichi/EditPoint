using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // シーン遷移
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class TitleButton : MonoBehaviour
{
    /*----その他変数（コンポーネントとかスクリプト）----*/
    [SerializeField]
    Fade fade;          // FadeCanvas

    private PlaySound playSound;
    [SerializeField] private GameObject audioPanel;

    [SerializeField] private GameObject MenuBG;
    [SerializeField] private GameObject Menu;
    private Image MenuImage;
    [SerializeField] private GameObject MenuFunction;

    [SerializeField] private GameObject CreditPanel;

    private void Start()
    {
        playSound = GameObject.Find("AudioCanvas").GetComponent<PlaySound>();
        MenuImage = Menu.GetComponent<Image>();
    }

    public void STARTBUTTON()
    {
        // フェード
        fade.FadeIn(0.5f, () => {
            SceneManager.LoadScene("Select");
        });
    }

    private void Update()
    {
        if (audioPanel.activeSelf) { return; }

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            //左クリックor右クリックで次のシーンへ
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                playSound.PlaySE(PlaySound.SE_TYPE.enter);
                playSound.PlaySE(PlaySound.SE_TYPE.sceneChange);
                // フェード
                fade.FadeIn(1.5f, () => {
                    SceneManager.LoadSceneAsync("Talk");
                });
            }
        }
    }

    IEnumerator OpenMenuAnim()
    {
        MenuImage.DOFade(1, 0);
        MenuImage.DOFillAmount(1f, 0.2f);

        yield return new WaitUntil(() => MenuImage.fillAmount == 1);

        MenuFunction.SetActive(true);
    }

    //メニューを開く
    public void OnMenuOpen()
    {
        MenuBG.SetActive(true);
        Menu.SetActive(true);

        StartCoroutine(OpenMenuAnim());
    }

    IEnumerator CloseMenuAnim()
    {
        MenuImage.DOFade(0, 0.2f);
        yield return new WaitUntil(() => MenuImage.color.a == 0);
        Menu.SetActive(false);
        MenuBG.SetActive(false);
        MenuImage.fillAmount = 0;
    }

    //メニューを閉じる
    public void OnMenuClose()
    {
        MenuFunction.SetActive(false);
        StartCoroutine(CloseMenuAnim());
    }

    //スタッフクレジット
    public void OnCredit()
    {
        CreditPanel.SetActive(true);
    }
}

