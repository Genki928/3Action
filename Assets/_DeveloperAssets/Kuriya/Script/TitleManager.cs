
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    // メニューの種類
    public enum MenuType
    {
        GameStart,
        Continue,
        Options,
        Exit
    }

    // メニュー項目
    [System.Serializable]
    public class MenuItem
    {
        public MenuType type;
        public string sceneName;
        public Text text;
    }

    [Header("メニュー設定")]
    [SerializeField]
    private List<MenuItem> menuItems = new List<MenuItem>();

    [Header("文字色")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color selectedColor = Color.yellow;

    [Header("文字サイズ")]
    [SerializeField] private int normalFontSize = 100;
    [SerializeField] private int selectedFontSize = 120;

    // 現在の選択位置
    private int arrowPos = 0;

    // 二重実行防止
    private bool isChangingScene = false;

    void Start()
    {
        UpdateTextColor();
    }

    // 上移動
    public void Up(InputAction.CallbackContext ctx)
    {
        if (isChangingScene || !ctx.performed) return;
        if (menuItems.Count == 0) return;

        arrowPos--;

        if (arrowPos < 0)
            arrowPos = menuItems.Count - 1;

        UpdateTextColor();
    }

    // 下移動
    public void Down(InputAction.CallbackContext ctx)
    {
        if (isChangingScene || !ctx.performed) return;
        if (menuItems.Count == 0) return;

        arrowPos++;

        if (arrowPos >= menuItems.Count)
            arrowPos = 0;

        UpdateTextColor();
    }

    // テキスト色の更新
    private void UpdateTextColor()
    {
        for (int i = 0; i < menuItems.Count; i++)
        {
            if (menuItems[i].text == null) continue;

            bool isSelected = (i == arrowPos);

            // 文字色
            menuItems[i].text.color =
                isSelected ? selectedColor : normalColor;

            // 文字サイズ
            menuItems[i].text.fontSize =
                isSelected ? selectedFontSize : normalFontSize;
        }
    }

    // 決定
    public void Submit(InputAction.CallbackContext ctx)
    {
        if (isChangingScene || !ctx.performed) return;
        if (menuItems.Count == 0) return;

        ExecuteMenu(menuItems[arrowPos]);
    }

    // メニューごとの処理
    private void ExecuteMenu(MenuItem item)
    {
        switch (item.type)
        {
            case MenuType.GameStart:
                LoadScene(item.sceneName);
                break;

            case MenuType.Continue:
                // セーブデータ読み込み処理
                break;

            case MenuType.Options:
                // 設定画面への処理
                break;

            case MenuType.Exit:
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
                break;
        }
    }

    // シーン遷移
    private void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName)) return;

        isChangingScene = true;
        SceneManager.LoadScene(sceneName);
    }
}
