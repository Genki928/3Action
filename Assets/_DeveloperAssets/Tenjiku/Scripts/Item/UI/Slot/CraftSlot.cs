using UnityEngine;
using UnityEngine.UI;

public class CraftSlot : MonoBehaviour
{
    // --- プロパティ ---
    // --- フィールド ---
    [Header("▼ アイテムスロット")]
    [SerializeField] Image _itemImg;

    private RectTransform rectTransform;
    private float angle;

    // --- メソッド ---
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Move(float speed, Vector3 center, float radiusX, float radiusY, float deco)
    {
        if (angle == 0.0f) angle += deco;
        angle += speed * Time.deltaTime;

        float x = center.x + Mathf.Cos(angle) * radiusX;
        float y = center.y + Mathf.Sin(angle) * radiusY;

        rectTransform.anchoredPosition = new Vector2(x, y);
    }

    public void SetImage(Sprite sprite, Color color)
    {
        _itemImg.sprite = sprite;
        _itemImg.color = color;
    }
}