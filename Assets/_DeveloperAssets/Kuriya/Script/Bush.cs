
using UnityEngine;

public class Bush : MonoBehaviour
{
    [Header("Material")]
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material collectedMaterial;

    private bool _isCollected = false;

    /// <summary>
    /// 茂みからアイテムを取得する
    /// </summary>
    public void Interact()
    {
        // すでに取得していたらreturnで返す
        if (_isCollected)
        {
            return;
        }

        _isCollected = true;

        // マテリアルを変更
        meshRenderer.sharedMaterial = collectedMaterial;

        Debug.Log("item get");
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