using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class TextBoxSystem : MonoBehaviour
{
    //テキストボックスオブジェクト
    [SerializeField] private GameObject TalkUICanvas;
    [SerializeField] private GameObject BackPanel;

    //トークUI
    private Text messageText;

    //表示するテキスト
    [SerializeField]
    [TextArea(1, 10)]
    private string allMessage = "今回はRPGでよく使われるメッセージ表示機能を作りたいと思います。\n"
            + "メッセージが表示されるスピードの調節も可能であり、改行にも対応します。\n"
            + "改善の余地がかなりありますが、               最低限の機能は備えていると思われます。\n"
            + "ぜひ活用してみてください。";

    //使用する分割文字列
    [SerializeField] private string splitString = "<>";
    //分割したテキスト
    private string[] splitMessage;
    //分割したメッセージの何番目か
    private int messageNum;
    //テキストスピード
    [SerializeField] private float textSpeed = 0.1f;
    //経過時間
    private float elapsedTime = 0f;
    //今見ている文字番号
    private int nowTextNum = 0;
    //マウスクリックを促すアイコン
    private Image clickIcon;
    //　クリックアイコンの点滅秒数
    [SerializeField]
    private float clickFlashTime = 0.2f;
    //　1回分のメッセージを表示したかどうか
    private bool isOneMessage = false;
    //　メッセージをすべて表示したかどうか
    private bool isEndMessage = false;

    //名前UI
    private Text nameText;
    //表示する名前
    [SerializeField]
    [TextArea(1, 5)]
    private string allName = "あああ<>" +
                           "いいい";
    //分割した名前
    private string[] splitName;
    //名前配列の何番目か
    private int nameNum;
    //今見ている名前番号
    private int nowNameNum = 0;
    //名前の表示したかどうか
    private bool isCheckName = false;

    //テキストボックスを表示中かどうか
    //false=表示してない　true=表示中
    private bool isTextOnOff = false;

    private float autoTimer = 0f;
    [SerializeField] float autoTimerLimit = 2f;

    private enum TextProgressionMode
    {
        auto,
        manual,
    }

    [SerializeField] private TextProgressionMode nowTextMode = TextProgressionMode.auto;

    private bool isSkip = false;
    private string message = "";
    [SerializeField] private GameObject CharaModel;


    void Start()
    {
        clickIcon = TalkUICanvas.transform.Find("TextPanel/Cursor").GetComponent<Image>();
        clickIcon.enabled = false;
        messageText = TalkUICanvas.transform.GetChild(0).GetComponentInChildren<RubyText>();
        messageText.text = "";

        nameText = TalkUICanvas.transform.GetChild(1).GetComponentInChildren<Text>();
        nameText.text = "";
        SetText(allMessage, allName);

        CharaModel.SetActive(false);
    }

    void Update()
    {
        //messageが終わっているか、メッセージがない場合はこれ以降何もしない
        if (isEndMessage || allMessage == null)
        {
            return;
        }

        //１回に表示するメッセージを表示していない
        if (!isOneMessage)
        {
            if (!isCheckName)
            {
                //名前表示
                nameText.text += splitName[nameNum].Substring(nowNameNum);
                isCheckName = true;
            }
            //テキスト表示時間を経過したらメッセージを追加
            if (elapsedTime >= textSpeed)
            {
                if(!isSkip && string.Compare(splitMessage[messageNum], nowTextNum, "<r=", 0, 3) == 0)
                {
                    isSkip = true;

                    int endTagPos = splitMessage[messageNum].IndexOf('>', nowTextNum);

                    for(int i = 0; i <= endTagPos - nowTextNum; i++)
                    {
                        message += splitMessage[messageNum][nowTextNum + i];
                    }

                    nowTextNum = endTagPos + 1;
                    return;
                }

                if(isSkip && string.Compare(splitMessage[messageNum], nowTextNum, "</r>", 0, 4) == 0)
                {
                    isSkip = false;

                    message += "</r>";

                    nowTextNum += 4;
                    return;
                }

                if (!isSkip)
                {
                    //メッセージ表示
                    //messageText.text += splitMessage[messageNum][nowTextNum];
                    message += splitMessage[messageNum][nowTextNum];
                    messageText.text = message;
                    elapsedTime = 0f;
                }
                else
                {
                    message += splitMessage[messageNum][nowTextNum];
                }
                nowTextNum++;

                //messageを全部表示、または行数が最大数表示された
                if (nowTextNum >= splitMessage[messageNum].Length)
                {
                    isOneMessage = true;
                }
            }

            elapsedTime += Time.deltaTime;
        }
        //１回に表示するメッセージを表示した
        else
        {
            if (nowTextMode == TextProgressionMode.manual)
            {
                elapsedTime += Time.deltaTime;

                //クリックアイコンを点滅する時間を超えた時、反転させる
                if (elapsedTime >= clickFlashTime)
                {
                    clickIcon.enabled = !clickIcon.enabled;
                    elapsedTime = 0f;
                }

                //エンターキーor左クリックを押したら次の文字表示処理
                if (Input.GetMouseButtonDown(0))
                {
                    NextTextSet();

                    //messageがすべて表示されていたらゲームオブジェクト自体の削除
                    if (messageNum >= splitMessage.Length)
                    {
                        isEndMessage = true;
                        isTextOnOff = false;
                        TalkUICanvas.SetActive(false);
                        BackPanel.SetActive(false);
                        CharaModel.SetActive(false);
                    }
                }
            }
            else if(nowTextMode == TextProgressionMode.auto)
            {
                autoTimer += Time.deltaTime;
                clickIcon.enabled = false;

                if (autoTimer >= autoTimerLimit)
                {
                    NextTextSet();

                    autoTimer = 0f;

                    //messageがすべて表示されていたらゲームオブジェクト自体の削除
                    if (messageNum >= splitMessage.Length)
                    {
                        isEndMessage = true;
                        isTextOnOff = false;
                        TalkUICanvas.SetActive(false);
                        BackPanel.SetActive(false);
                        CharaModel.SetActive(false);
                    }
                }
            }

            
        }
    }

    /// <summary>
    /// 次のテキストの設定処理
    /// </summary>
    private void NextTextSet()
    {
        message = "";
        nowTextNum = 0;
        messageNum++;
        messageText.text = "";
        clickIcon.enabled = false;
        elapsedTime = 0f;
        isOneMessage = false;

        if(nameNum + 1 < splitName.Length)
        {
            //現在の名前と次の名前が同じとき
            if(splitName[nameNum] != splitName[nameNum + 1])
            {
                nameText.text = "";
                isCheckName = false;
            }
            nowNameNum = 0;
            nameNum++;
        }

        isOneMessage = false;
    }

    /// <summary>
    /// 受け取ったテキストをもとに初期設定する
    /// </summary>
    /// <param name="message">テキスト本分</param>
    /// <param name="name">キャラの名前</param>
    void SetText(string message, string name)
    {
        this.message = "";
        this.allMessage = message;
        this.allName = name;
        //分割文字列で一回に表示するメッセージを分割する
        splitMessage = Regex.Split(allMessage, @"\s*" + splitString + @"\s*", RegexOptions.IgnorePatternWhitespace);
        splitName = Regex.Split(allName, @"\s*" + splitString + @"\s*", RegexOptions.IgnorePatternWhitespace);
        nowTextNum = 0;
        messageNum = 0;
        messageText.text = "";

        nowNameNum = 0;
        nameNum = 0;
        nameText.text = "";
        isCheckName = false;
        isOneMessage = false;
        isEndMessage = false;
        isTextOnOff = true;
    }

    //他のスクリプトから新しいメッセージを設定し、UIをアクティブにする
    public void SetTextPanel(string message, string name)
    {
        SetText(message, name);
        TalkUICanvas.SetActive(true);
        BackPanel.SetActive(true);
        CharaModel.SetActive(true);
    }

    public bool CheckTextOnOff()
    {
        return isTextOnOff;
    }
}
