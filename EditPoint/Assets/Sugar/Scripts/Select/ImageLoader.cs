using UnityEngine;
using UnityEngine.UI;
using System.IO;
using SFB;  // Standalone File Browser (�v�C���X�g�[��)

public class ImageLoader : MonoBehaviour
{
    public Image displayImage; // Unity��UI��Image�R���|�[�l���g��ݒ�

    public void LoadImage()
    {
        // ���[�U�[�Ƀt�@�C���I���𑣂�
        string[] paths = StandaloneFileBrowser.OpenFilePanel("�摜��I��", "", new[] {
            new ExtensionFilter("�摜�t�@�C��", "png", "jpg", "jpeg")
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

        if (texture.LoadImage(fileData)) // PNG/JPG��ǂݍ���
        {
            Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            displayImage.sprite = newSprite;
        }

        yield return null;
    }
}
