using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using SimpleFileBrowser; // Runtime File Browser のネームスペース

public class ImageLoader : MonoBehaviour
{
    public Image displayImage; // UIのImageコンポーネント

    public void LoadImage()
    {
        // ユーザーにファイル選択を促す
        FileBrowser.ShowLoadDialog(
            (paths) => {
                if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
                {
                    StartCoroutine(LoadTexture(paths[0]));
                }
            },
            () => Debug.Log("キャンセルされました"),
            FileBrowser.PickMode.Files,
            false,
            null,
            "画像を選択",
            "選択"
        );
    }

    private IEnumerator LoadTexture(string path)
    {
        byte[] fileData = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);

        if (texture.LoadImage(fileData)) // PNG/JPGを読み込み
        {
            Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            displayImage.sprite = newSprite;
            displayImage.color = Color.white;
        }

        yield return null;
    }
}
