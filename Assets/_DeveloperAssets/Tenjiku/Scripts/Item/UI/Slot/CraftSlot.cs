using UnityEngine;
using UnityEngine.UI;

public class CraftSlot : MonoBehaviour, IItemSlot
{
    // --- プロパティ ---
    public ItemSlot Hotbar => _hotbar;
    public ItemSlot[] Slots => _slots;

    // --- フィールド ---
    [Header("▼ アイテムスロット")]
    [SerializeField] ItemSlot _hotbar;  // インベントリを閉じた時、カーソルにホットバーを戻す
    [SerializeField] ItemSlot[] _slots = new ItemSlot[4];    // 移動先のインベントリ

    // --- メソッド ---

}