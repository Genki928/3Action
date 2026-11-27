using UnityEngine;
using TMPro;
using System.Collections;

public class ResultManager : MonoBehaviour
{
    [Header("リザルトデータ")]
    [SerializeField] int totalScore;
    [SerializeField] int nowScore ;

    [Header("リザルト UI")]
    [SerializeField] TMP_Text totalScoreText;
    [SerializeField] TMP_Text nowScoreText;

    [Header("アニメーション")]
    [SerializeField] private float countDuration = 2.0f;//カウントしてからの経過時間

    [Header("オーディオ")]
    [SerializeField] AudioClip countupSE;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       nowScore = CurrentScore();

        StartCoroutine(CountUP());

        SoundManager.Instance.PlaySE(countupSE);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int CurrentScore()
    {
        int score = 0;

        if(Inventory.ResultItems==null)
        {
            Debug.LogWarning("ResultItemtahiahajhijhijahjj");
            return 0;
        }

        foreach (var item in Inventory.ResultItems)
        {
            if (item.Item == null) continue;

            score += item.Count;
        }
        return score;
    }
    private IEnumerator CountUP()
    {
        float elasped = 0.0f;

        while (elasped < countDuration)
        {
            elasped += Time.deltaTime;
            //カウントアップの進行度
            float progress = elasped / countDuration;
            int creentScore = Mathf.RoundToInt(Mathf.Lerp(0, nowScore, progress));

            nowScoreText.text = creentScore + "Gold";

           

            yield return null;
        }
        nowScoreText.text = nowScore + "Gold";
    }
}
