using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UISlot : MonoBehaviour, IUISlot, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Slot slot;
    public Image slotSprite;
    public Button button;
    public bool isItemInSlot;

    public void AddItem(Slot newSlot)
    {
        slot = newSlot;
        slotSprite.sprite = slot.item.sprite;
        slotSprite.enabled = true;
        button.interactable = true;
        isItemInSlot = true;
    }

    public virtual void ClearSlot()
    {
        slot = GameEvents.current.GetEmptySlot();
        slotSprite.sprite = null;
        slotSprite.enabled = false;
        button.interactable = false;
        isItemInSlot = false;
    }

    public Slot GetSlot()
    {
        return slot;
    }

    public bool IsItemInSlot()
    {
        return isItemInSlot;
    }

    public abstract void OnRemoveButton();

    public abstract void OnPointerEnter(PointerEventData eventData);

    public abstract void OnPointerExit(PointerEventData eventData);

    public abstract void OnPointerClick(PointerEventData eventData); 
}
