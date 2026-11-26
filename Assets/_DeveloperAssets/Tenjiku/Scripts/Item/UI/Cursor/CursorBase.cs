using UnityEngine;
using UnityEngine.UI;

public abstract class CursorBase : MonoBehaviour
{
    // ----- フィールド ----- //
    protected InventoryManager _ui;
    protected ItemSlot _selectSlot;
    bool _active = true;

    // ----- メソッド ----- //
    /// <summary> カーソル上移動 </summary>
    public abstract void CursorUp();

    /// <summary> カーソル下移動 </summary>
    public abstract void CursorDown();

    /// <summary> カーソル右移動 </summary>
    public abstract void CursorRight();

    /// <summary> カーソル左移動 </summary>
    public abstract void CursorLeft();

    /// <summary> カーソル決定 </summary>
    public abstract void Interact();

    /// <summary> UIを閉じた時の処理 </summary>
    protected abstract void UIClosed();

    /// <summary> カテゴリを切り替える </summary>
    public virtual void CategoryChange(bool active)
    {
        // 表示を無効化
        _active = active;
        if (TryGetComponent<Image>(out var img))
            img.enabled = _active;
    }

    /// <summary> UIの設定 </summary>
    public void SetUI(InventoryManager ui) => _ui = ui;

    // イベント制御
    protected virtual void OnEnable()
    {
        _ui.OnCloseUI += UIClosed;
        _ui.OnToggleCategory += CategoryChange;
    }

    protected virtual void OnDisable()
    {
        _ui.OnCloseUI -= UIClosed;
        _ui.OnToggleCategory -= CategoryChange;
    }
}