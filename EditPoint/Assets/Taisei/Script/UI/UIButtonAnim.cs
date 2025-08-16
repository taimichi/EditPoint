using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Animator anim;


    //カーソルが触れた場合
    public void OnPointerEnter(PointerEventData eventData)
    {
        anim.SetBool("isHover", true);
    }
    //カーソルが離れた場合
    public void OnPointerExit(PointerEventData eventData)
    {
        anim.SetBool("isHover", false);
    }

}
