using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Cursor : MonoBehaviour
{
    [SerializeField] ItemSlot _baseSlot;
    ItemSlot _currentSlot;
    [SerializeField] InventoryManager _ui;
    //[SerializeField] float scaleSizeMax = 1.1f;
    //[SerializeField] float scaleSizeMin = 0.9f;
    //[SerializeField] float scalingSize = 0.5f;
    bool scale = true;

    bool IsMoveCursor(InputAction.CallbackContext ctx) => !_ui.IsOpenUI || !ctx.performed;

    void Start()
    {
        _currentSlot = _baseSlot;
        Draw();
    }

    public void CursorUp(InputAction.CallbackContext ctx)
    {
        if (IsMoveCursor(ctx)) return;

        _currentSlot = _currentSlot.Slots[(int)Direct.Top];
        Draw();
    }

    public void CursorDown(InputAction.CallbackContext ctx)
    {
        if (IsMoveCursor(ctx)) return;

        _currentSlot = _currentSlot.Slots[(int)Direct.Bottom];
        Draw();
    }

    public void CursorRight(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        _currentSlot = _currentSlot.Slots[(int)Direct.Right];
        Draw();
    }

    public void CursorLeft(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        _currentSlot = _currentSlot.Slots[(int)Direct.Left];
        Draw();
    }

    public void CategoryChange(InventoryCategory category)
    {
        if (category == InventoryCategory.Craft)
        {
            _baseSlot = _currentSlot;
        }
    }

    void Draw()
    {
        transform.position = _currentSlot.transform.position;
    }

    void CursorCorrection()
    {
        _currentSlot = _currentSlot.Hotbar;
        Draw();
    }

    void OnEnable()
    {
        _ui.OnCloseCraftUI += CursorCorrection;
        _ui.OnChangedCategory += CategoryChange;
    }

    void OnDisable()
    {
        _ui.OnCloseCraftUI -= CursorCorrection;
        _ui.OnChangedCategory -= CategoryChange;
    }
}