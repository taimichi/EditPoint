using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Animator anim;


    //�J�[�\�����G�ꂽ�ꍇ
    public void OnPointerEnter(PointerEventData eventData)
    {
        anim.SetBool("isHover", true);
    }
    //�J�[�\�������ꂽ�ꍇ
    public void OnPointerExit(PointerEventData eventData)
    {
        anim.SetBool("isHover", false);
    }

}
