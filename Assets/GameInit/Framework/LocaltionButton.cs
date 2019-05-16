using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[AddComponentMenu("UI/Local_Button", 30)]
public class LocaltionButton : Button
{
    private void OnPlayAni()
    {
        this.GetComponent<Animator>().Play("Pressed", 0, 0f);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (this.GetComponent<Button>().interactable)
            OnPlayAni();
    }

}
