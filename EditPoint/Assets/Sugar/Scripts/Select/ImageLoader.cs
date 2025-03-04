using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using SimpleFileBrowser; // Runtime File Browser �̃l�[���X�y�[�X

public class ImageLoader : MonoBehaviour
{
    public Image displayImage; // UI��Image�R���|�[�l���g

    public void LoadImage()
    {
        // ���[�U�[�Ƀt�@�C���I���𑣂�
        FileBrowser.ShowLoadDialog(
            (paths) => {
                if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
                {
                    StartCoroutine(LoadTexture(paths[0]));
                }
            },
            () => Debug.Log("�L�����Z������܂���"),
            FileBrowser.PickMode.Files,
            false,
            null,
            "�摜��I��",
            "�I��"
        );
    }

    private IEnumerator LoadTexture(string path)
    {
        byte[] fileData = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);

        if (texture.LoadImage(fileData)) // PNG/JPG��ǂݍ���
        {
            Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            displayImage.sprite = newSprite;
            displayImage.color = Color.white;
        }

        yield return null;
    }
}
