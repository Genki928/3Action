
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

/*
 * 時計回転用スクリプト
 */
public class ViewTime : MonoBehaviour
{
    [SerializeField] RectTransform clock;
    [SerializeField] LegStateManager state;

    [Header("時計設定")]
    [SerializeField] float phaseTime = 60f;

    float timer = 0f;

    void Update()
    {
        if (clock == null) return;

        if (state.State is ExploringState ||
            state.State is RaidState)
        {
            timer += Time.deltaTime;

            // 時間経過に応じて時計全体を回転
            float angle = (timer / phaseTime) * 360f + 90f;

            clock.localRotation =
                Quaternion.Euler(0f, 0f, +angle);
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.L))
        {
            SceneManager.LoadScene("result");
        }
    }
}
