using UnityEngine;

[System.Serializable]
public class SpriteClip
{
    public string name;         // "Idle" や "Walk" など
    public Texture2D texture;   // この状態のシート画像
    public int columns = 4;     // 横のコマ数
    public int rows = 1;        // 縦のコマ数
    public int frameCount = 4;  // 実際に使うコマ数(空きがある場合用)
    public float fps = 8f;      // 1秒あたりのコマ数

}

public class PlayerAnimControl : MonoBehaviour
{
    public SpriteClip[] clips;      // Inspectorで Idle と Walk を登録
    public string startClip = "Idle";

    Renderer rend;
    SpriteClip current;
    float startTime;
    public bool flipX;

    void Start()
    {
        rend = GetComponent<Renderer>();
        Play(startClip);
    }

    // 外から呼んで状態を切り替える
    public void Play(string clipName)
    {
        // すでに同じ状態なら何もしない(毎フレーム呼んでも巻き戻らない)
        if (current != null && current.name == clipName) return;

        foreach (var c in clips)
        {
            if (c.name != clipName) continue;

            current = c;
            startTime = Time.time;  // 0コマ目から始め直す
            rend.material.mainTexture = c.texture;
            rend.material.mainTextureScale = new Vector2(1f / c.columns, 1f / c.rows);
            return;
        }
        Debug.LogWarning("クリップが見つかりません: " + clipName);
    }

    public void SetFlip(bool flip)
    {
        flipX = flip;
    }

    void Update()
    {
        if (current == null) return;

        int frame = (int)((Time.time - startTime) * current.fps) % current.frameCount;
        int col = frame % current.columns;
        int row = frame / current.columns;

        float x = (float)col / current.columns;
        float y = 1f - (float)(row + 1) / current.rows;

        rend.material.mainTextureScale = new Vector2(1f / current.columns, 1f / current.rows);
        rend.material.mainTextureOffset = new Vector2(x, y);

        // 反転はQuadのスケールで行う
        var s = transform.localScale;
        s.x = Mathf.Abs(s.x) * (flipX ? -1f : 1f);
        transform.localScale = s;
    }
}