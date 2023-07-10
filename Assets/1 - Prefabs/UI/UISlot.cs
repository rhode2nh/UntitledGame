using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UISlot : MonoBehaviour, IUISlot, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Slot slot;
    public Image slotSprite;
    public Button button;
    public bool isItemInSlot;
    public int index;
    public RectTransform originalPos;
    public GameObject m_DraggingIcon;
    public RectTransform m_DraggingPlane;
    public GameObject icon;

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

    public void OnBeginDrag(PointerEventData data) {
        if (slot.item == GameEvents.current.GetEmptyItem()) {
            return;
        }
        var canvas = gameObject.GetComponentInParent<Canvas>();
        if (canvas == null)
            return;

        // We have clicked something that can be dragged.
        // What we want to do is create an icon for this.
        m_DraggingIcon = Instantiate(icon);

        m_DraggingIcon.transform.SetParent(canvas.transform, false);
        m_DraggingIcon.transform.SetAsLastSibling();

        var background = m_DraggingIcon.GetComponent<Image>();
        Debug.Log(background.color);
        var image = m_DraggingIcon.transform.GetChild(0).GetComponent<Image>();
        // image.gameObject.SetActive(true);

        image.sprite = slotSprite.sprite;
        image.enabled = true;
        // image.SetNativeSize();

        m_DraggingPlane = canvas.transform as RectTransform;

        SetDraggedPosition(data);
    }

    public void OnDrag(PointerEventData data) {
        if (m_DraggingIcon != null) {
            SetDraggedPosition(data);
        }
    }

    public void SetDraggedPosition(PointerEventData data)
    {
        if (data.pointerEnter != null && data.pointerEnter.transform as RectTransform != null)
            m_DraggingPlane = data.pointerEnter.transform as RectTransform;

        var rt = m_DraggingIcon.GetComponent<RectTransform>();
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, data.position, data.pressEventCamera, out globalMousePos))
        {
            rt.position = globalMousePos;
            rt.rotation = m_DraggingPlane.rotation;
        }
    }

    public virtual void OnEndDrag(PointerEventData data) {
        if (m_DraggingIcon != null) {
            Destroy(m_DraggingIcon);
        }
        if (data.pointerEnter == null) {
            Slot curSlot = RemoveItemFromInventory(slot.id);
            GameEvents.current.DropItem(curSlot, true);
            return;
        }
        var slotToSwap = data.pointerEnter.GetComponent<UISlot>();
        if (slotToSwap == null) {
            return;
        }
        Debug.Log(slotToSwap.CanAddToInventory(slot));
        Debug.Log(CanAddToInventory(slotToSwap.slot));
        if (slotToSwap.CanAddToInventory(slot) && CanAddToInventory(slotToSwap.slot)) {
            Slot curSlot = RemoveItemFromInventory(slot.id);
            Slot otherSlot = slotToSwap.RemoveItemFromInventory(slotToSwap.slot.id);
            AddItemToInventory(otherSlot, index);
            slotToSwap.AddItemToInventory(curSlot, slotToSwap.index);
        }
    }

    public abstract void AddItemToInventory(Slot slot, int index);
    public abstract Slot RemoveItemFromInventory(string id);
    public abstract bool CanAddToInventory(Slot slot);
}
