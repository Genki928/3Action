using UnityEngine;
using UnityEngine.UI;

public interface IItemSlot
{
    // --- プロパティ ---
    public ItemSlot Hotbar {  get; }
    public ItemSlot[] Slots { get; }
}

public class ItemSlot : MonoBehaviour, IItemSlot
{
    // --- プロパティ ---
    public ItemSlot Hotbar => _hotbar;
    public ItemSlot[] Slots => slots;
    public ItemStack Stack => _itemStack;

    // --- フィールド ---
    [Header("▼ アイテムスロット")]
    [SerializeField] Text _count;
    [SerializeField] ItemSlot _hotbar;  // インベントリを閉じた時、カーソルにホットバーを戻す
    [SerializeField] ItemSlot[] slots = new ItemSlot[4];    // 移動先のインベントリ
    Color CLEAR = new(1.0f, 1.0f, 1.0f, 0.0f);
    ItemStack _itemStack;

    // --- メソッド ---
    public void GetItem(ItemStack stack = null)
    {
        _itemStack = stack;
        var image = GetComponent<Image>();
        if (stack != null)
        {
            // アイテムを描画
            image.sprite = stack.Item.Sprite;
            image.color = Color.white;

            // 個数のテキストを変更
            _count.text = stack.Count.ToString();
        }
        else
        {
            // 透明になる
            image.sprite = null;
            image.color = CLEAR;

            // テキストをリセット
            _count.text = "";
        }
    }
}

public enum Direct
{ 
    Top = 0,
    Bottom = 1,
    Right = 2,
    Left = 3,
}