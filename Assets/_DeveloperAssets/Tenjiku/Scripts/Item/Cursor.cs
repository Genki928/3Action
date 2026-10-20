using UnityEngine;
using UnityEngine.InputSystem;

public class Cursor : MonoBehaviour
{
    [SerializeField] ItemSlot _baseSlot;
    ItemSlot _currentSlot;
    [SerializeField] InventoryManager _ui;

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
    }

    void OnDisable()
    {
        _ui.OnCloseCraftUI -= CursorCorrection;
    }
}