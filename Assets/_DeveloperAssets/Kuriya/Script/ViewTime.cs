using UnityEngine;
using UnityEngine.UI;
/*
 * タイマー描画用スクリプト
 */
public class ViewTime : MonoBehaviour
{
    [SerializeField] GameObject txt;
    [SerializeField] LegStateManager state;

    Text text;
    private void Start()
    {
        text = txt.GetComponent<Text>();
    }
    // Update is called once per frame
    void Update()
    {
        if(state.State is ExploringState)
        {
            text.text = "フェーズ：昼";
        }
        else if(state.State is RaidState)
        {
            text.text = "フェーズ：夜";
        }
    }
}
