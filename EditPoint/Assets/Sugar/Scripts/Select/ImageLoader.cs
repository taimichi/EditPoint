using UnityEngine;
using UnityEngine.UI;
using System.IO;
using SFB;  // Standalone File Browser (要インストール)

public class ImageLoader : MonoBehaviour
{
    public Image displayImage; // UnityのUIのImageコンポーネントを設定

    public void LoadImage()
    {
        // ユーザーにファイル選択を促す
        string[] paths = StandaloneFileBrowser.OpenFilePanel("画像を選択", "", new[] {
            new ExtensionFilter("画像ファイル", "png", "jpg", "jpeg")
        }, false);

        if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
        {
            StartCoroutine(LoadTexture(paths[0]));
        }
    }

    private System.Collections.IEnumerator LoadTexture(string path)
    {
        byte[] fileData = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);

        if (texture.LoadImage(fileData)) // PNG/JPGを読み込み
        {
            Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            displayImage.sprite = newSprite;
        }

        yield return null;
    }
}
