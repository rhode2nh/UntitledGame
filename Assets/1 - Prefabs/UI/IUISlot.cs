public interface IUISlot
{
    void AddItem(Slot newSlot);
    void ClearSlot();
    void OnRemoveButton();
    Slot GetSlot();
    bool IsItemInSlot();
}
