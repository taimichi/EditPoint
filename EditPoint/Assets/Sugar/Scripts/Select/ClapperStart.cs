using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // シーン遷移

public class ClapperStart : MonoBehaviour
{
    // 動かす対象
    [SerializeField] GameObject obj;

    [SerializeField] Vector3 strPos;
    [SerializeField] Vector3 endpos;

    [SerializeField] float spdY = -1.0f;

    [SerializeField] Fade fade;          // FadeCanvas

    [SerializeField] Animator animator;

    float ofsY;
    private int num = 999;
    [SerializeField]bool isSelectMode = false;
    string name;

    int count = 0;

    private void Start()
    {
        // 選択シーンじゃないなら最初からスタート
        if (!isSelectMode) { num = 0; }
    }
    // Update is called once per frame
    void Update()
    {
        switch (num)
        {
            case 0: // 初期値にセット
                obj.transform.position = strPos;
                ofsY = strPos.y;
                num++;
                break;
            case 1: // 移動
                ofsY += spdY;
                obj.transform.position = new Vector2(0, ofsY);
                if (obj.transform.position.y <= endpos.y)
                {
                    num++;
                }
                break;
            case 2: // アニメーション起動
                animator.Play("Scene1");
                break;
            case 3: // シーン遷移 
                if(!isSelectMode)
                {
                    count++;
                    if (count >= 60)
                    {
                        Destroy(obj);
                    }
                    return;
                }
                count++;
                if (count <= 60) { return; }
                num++;
                SceneManager.LoadScene(name);
                //fade.FadeIn(0.5f, () => {

                //});
                break;
        }
    }

    public string SceneName
    {
        set
        {
            // シーンネームの取得
            name = value;
            num = 0; // 値がセットされたら動き出す
            Debug.Log(name);
        }
    }

    public void EndAnim()
    {
        num++;
    }
        
}
