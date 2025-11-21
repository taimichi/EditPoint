using UnityEngine;

public class AllTexts : MonoBehaviour
{
    [SerializeField]
    private TextBoxSystem TextSystemScript;

    //表示させるメッセージ
    private string message;

    //表示させる名前
    private string charaName;

    public enum TEXT_MESSAGE
    {
        clear_stage1,
        clear_stage2,
        clear_stage3,
        clear_stage4,
    }

    #region 使い方説明
    //使い方
    //・ここにテキスト・名前・アイコン名などを書き込んでいく
    //・台詞・名前・アイコン名をここで一括管理する
    //・「<>」があるとこまでがひとつの台詞となる
    //・一番最後のところに「<>」があると動作が上手くいかなくなるため、
    //　つけないように注意してください
    //・assetフォルダーにResourcesフォルダーを作り、そこにアイコン用の画像を入れる
    //・ここに書くアイコン名はアイコン用画像の名前と同じにする
    //
    //表示させるとき
    //スクリプトに以下の文を追加
    //[SerializeField] private AllTexts alltextsscript;
    //[SerializeField] private GameObject talkUI;
    //int textNo;
    //上二つはどちらともゲームオブジェクトの「TalkUI」を設定する
    //
    //以下の文をupdateなどに追加することで台詞などを表示する
    //textNo = ○〇;←呼び出したいセリフの番号を設定
    //talkUI.SetActive(true);
    //alltextsscript.SetAllTexts(textNo);
    //
    //※使用例→「testSet」スクリプト
    #endregion

    public void SetAllTexts(TEXT_MESSAGE textCord)
    {
        switch (textCord)
        {
            case TEXT_MESSAGE.clear_stage1:
                message = "お<r=つか>疲</r>れ<r=さま>様</r>です♪<>" +
                    "この<r=せかい>世界</r>について" +
                    "<r=すこ>少</r>しは<r=りかい>理解</r>してもらえましたか？<>" +
                    "<r=み>見</r>ての<r=とお>通</r>り" +
                    "いくつもの<r=どうが>動画</r>ファイルに<r=もんだい>問題</r>が<r=お>起</r>こっています！<>" +
                    "<r=せかい>世界</r>を<r=ただ>正</r>してカチンコが<r=な>鳴</r>れば" +
                    "<r=ちつじょ>秩序</r>が<r=たも>保</r>たれます！<>" +
                    "あなたの<r=へんしゅうりょく>編集力</r>ならどこまででも<r=い>行</r>けます！\n" +
                    "このペースでどんどん<r=ただ>正</r>していきましょう！！！";
                charaName = "エディ";
                Debug.Log("ステージ１クリア");
                break;

            case TEXT_MESSAGE.clear_stage2:
                message = "すごいです！<r=ぐうぜん>偶然</r><r=あ>会</r>った<r=ひと>人</r>が" +
                    "こんなにも<r=ゆうしゅう>優秀</r>だなんて…!" +
                    "\n<r=かみさま>神様</r>に<r=かんしゃ>感謝</r>しなきゃですね♪<>" +
                    "<r=へんしゅうきのう>編集機能</r>も<r=つか>使</r>いこなせるようになってきましたね！<>" +
                    "ですが<r=つぎ>次</r>のフォルダから<r=つか>使</r>える「カット<r=きのう>機能</r>」は" +
                    "<r=しょうしょうふくざつ>少々複雑</r>なので<r=こころ>心</r>してかかってくださいね！<>" +
                    "それでは！<r=けんとう>健闘</r>を<r=いの>祈</r>ります！";
                charaName = "エディ";
                Debug.Log("ステージ２クリア");
                break;

            case TEXT_MESSAGE.clear_stage3:
                message = "<r=つぎ>次</r>が<r=さいご>最後</r>のフォルダになります…。<>" +
                    "もう<r=わたし>私</r>のサポートも<r=ひつよう>必要</r>ありませんね！<>" +
                    "この<r=せかい>世界</r>をあなたの<r=へんしゅう>編集</r>で<r=ただ>糾</r>す<r=ところ>所</r>、" +
                    "しっかりとこの<r=め>目</r>に<r=や>焼</r>き<r=つ>付</r>けますよ！";
                charaName = "エディ";
                Debug.Log("ステージ３クリア");
                break;

            case TEXT_MESSAGE.clear_stage4:
                message = "なんと…!すべての<r=えいぞう>映像</r>ファイルを<r=ただ>正</r>してしまうとは…!<>" +
                    "このソフトのアシスタントロボットとして<r=たいへんかんしゃ>大変感謝</r>いたします！<>" +
                    "あなたはこの<r=せかい>世界</r>の<r=えいゆう>英雄</r>であり" +
                    "<r=いちにんまえ>一人前</r>の<r=へんしゅうしゃ>編集者</r>です！<>" +
                    "あなたのその<r=ちから>力</r>で<r=ほか>他</r>の<r=せかい>世界</r>をも" +
                    "<r=すく>救</r>って<r=み>見</r>せてください！<>" +
                    "<r=たの>楽</r>しみにしています！<>" +
                    "それでは、また<r=べつ>別</r>の<r=せかい>世界</r>でお<r=あ>会</r>いしましょう！";
                charaName = "エディ";
                Debug.Log("ステージ4クリア");
                break;
        }
        TextSystemScript.SetTextPanel(message, charaName);
    }

    /// <summary>
    /// テキストボックスの現在の状態を取得
    /// </summary>
    public TextBoxSystem.TALK_STATE ReturnTalkState()
    {
        return TextSystemScript.ReturnNowTalkState();
    }

    /// <summary>
    /// テキストボックスを初期状態に
    /// </summary>
    public void ResetTalkState()
    {
        TextSystemScript.ResetTalkState();
    }
}
