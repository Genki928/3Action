using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    public ItemSlot SelectingItem => _currentSlot;
    [SerializeField] ItemSlot _baseSlot;
    ItemSlot _currentSlot;
    [SerializeField] InventoryManager _ui;
    bool _active = true;

    bool IsMoveCursor(InputAction.CallbackContext ctx) => _active && ctx.performed;

    void Start()
    {
        _currentSlot = _baseSlot;
        Draw();
    }

    public void CursorUp(InputAction.CallbackContext ctx)
    {
        if (!IsMoveCursor(ctx)) return;
        if (!_ui.IsOpenUI) return;

        _currentSlot = _currentSlot.Slots[(int)Direct.Top];
        Draw();
    }

    public void CursorDown(InputAction.CallbackContext ctx)
    {
        if (!IsMoveCursor(ctx)) return;
        if (!_ui.IsOpenUI) return;

        _currentSlot = _currentSlot.Slots[(int)Direct.Bottom];
        Draw();
    }

    public void CursorRight(InputAction.CallbackContext ctx)
    {
        if (!IsMoveCursor(ctx)) return;

        _currentSlot = _currentSlot.Slots[(int)Direct.Right];
        Draw();
    }

    public void CursorLeft(InputAction.CallbackContext ctx)
    {
        if (!IsMoveCursor(ctx)) return;

        _currentSlot = _currentSlot.Slots[(int)Direct.Left];
        Draw();
    }

    public void ChangeDrawing(bool active)
    {
        _active = active;
        if (TryGetComponent<Image>(out var img))
            img.enabled = _active;
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
        _ui.OnToggleCategory += ChangeDrawing;
    }

    void OnDisable()
    {
        _ui.OnCloseCraftUI -= CursorCorrection;
        _ui.OnToggleCategory -= ChangeDrawing;
    }
}