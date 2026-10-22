
using UnityEngine;

public class Bush : MonoBehaviour
{
    [Header("Material")]
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material collectedMaterial;
    [SerializeField] Item _itemData;
    [SerializeField] GameObject _itemPrefab;

    private bool _isCollected = false;

    /// <summary>
    /// 茂みからアイテムを取得する
    /// </summary>
    public void Interact(Inventory inventory)
    {
        // すでに取得していたらreturnで返す
        if (_isCollected)
        {
            return;
        }

        _isCollected = true;

        // マテリアルを変更
        meshRenderer.sharedMaterial = collectedMaterial;

        // ドロップ
        //inventory.AddItem(new(_itemData));
        Instantiate(_itemPrefab, new(transform.position.x, transform.position.y, transform.position.z - 0.5f), Quaternion.identity);
    }

    /// <summary>
    /// アイテム取得状態をリセットする
    /// </summary>
    public void ResetBush()
    {
        _isCollected = false;
        meshRenderer.sharedMaterial = normalMaterial;
    }
}