using UnityEngine;

[System.Serializable]
public class SpriteClip
{
    public string name;           // "Idle" や "Walk"
    public Texture2D[] frames;    // コマ画像を順番に登録
    public float fps = 8f;
}

public class AnimControl : MonoBehaviour
{
    public SpriteClip[] clips;
    public string startClip = "Idle";
    public bool flipX;

    Renderer rend;
    SpriteClip current;
    float startTime;

    void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    void Start()
    {
        Play(startClip);
    }

    public void Play(string clipName)
    {
        if (current != null && current.name == clipName) return;

        foreach (var c in clips)
        {
            if (c.name != clipName) continue;
            current = c;
            startTime = Time.time;
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
        if (current == null || current.frames == null || current.frames.Length == 0) return;

        int frame = (int)((Time.time - startTime) * current.fps) % current.frames.Length;
        rend.material.mainTexture = current.frames[frame];

        // 反転は Tiling/Offset で行う(これまでと同じ向き)
        rend.material.mainTextureScale = new Vector2(flipX ? -1f : 1f, 1f);
        rend.material.mainTextureOffset = new Vector2(flipX ? 1f : 0f, 0f);
    }
}