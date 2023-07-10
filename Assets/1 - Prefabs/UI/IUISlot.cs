public interface IUISlot
{
    void AddItem(Slot newSlot);
    void ClearSlot();
    Slot GetSlot();
    bool IsItemInSlot();
}
