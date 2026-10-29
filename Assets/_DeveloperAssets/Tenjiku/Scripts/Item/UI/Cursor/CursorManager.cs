using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public interface ICursor
{
    public void CursorUp(InputAction.CallbackContext ctx);
    public void CursorDown(InputAction.CallbackContext ctx);
    public void CursorRight(InputAction.CallbackContext ctx);
    public void CursorLeft(InputAction.CallbackContext ctx);
}

public class CursorManager : MonoBehaviour
{
    // --- ÉvÉçÉpÉeÉB ---
    ItemSlot[] SlotList => _ui.ItemSlot;

    // --- ÉtÉBÅ[ÉãÉh ---
    [SerializeField] InventoryManager _ui;
    [SerializeField] InventoryCursor _invCursor;
    [SerializeField] CraftCursor _craCursor;
    bool _isInventory = true;

    // --- ÉÅÉ\ÉbÉh ---
    void Start()
    {
        // ï`âÊ
        Draw();
    }

    public void CursorUp(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if (!_ui.IsOpenUI) return;

        if (_ui.IsInventory) _invCursor.CursorUp();
        // ï`âÊ
        Draw();
    }

    public void CursorDown(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if (!_ui.IsOpenUI) return;

        if (_ui.IsInventory) _invCursor.CursorDown();

        // ï`âÊ
        Draw();
    }

    public void CursorRight(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (_ui.IsInventory) _invCursor.CursorRight();

        // ï`âÊ
        Draw();
    }

    public void CursorLeft(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (_ui.IsInventory) _invCursor.CursorLeft();

        // ï`âÊ
        Draw();
    }

    void Draw()
    {
    }

    public void Interact(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if (!_ui.IsOpenUI) return;

        Debug.Log(_ui.IsInventory);
        if (_ui.IsInventory) _invCursor.Interact();
        else
        {
            Debug.Log("testestestsetstestest");
            _ui.Inventory.AddItem(_ui.Cauldron.Craft());
        }
    }

    public void Throw(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if (!_ui.IsOpenUI) return;

        if (_ui.IsInventory) _invCursor.Throw();
    }

    void UIOpend()
    {
        ;
    }

    void UIClosed()
    {
        // ï`âÊ
        Draw();
    }

    void OnEnable()
    {
        _ui.OnOpenUI += UIOpend;
        _ui.OnCloseUI += UIClosed;
    }

    void OnDisable()
    {
        _ui.OnOpenUI -= UIOpend;
        _ui.OnCloseUI -= UIClosed;
    }
}