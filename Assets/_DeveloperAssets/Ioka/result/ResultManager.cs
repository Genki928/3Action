using UnityEngine;
using TMPro;
using System.Collections;

public class ResultManager : MonoBehaviour
{
    [Header("リザルトデータ")]
    [SerializeField] private int totalScore=4000;

    [Header("リザルト UI")]
    [SerializeField] private TMP_Text totalscoreText;

    [Header("アニメーション")]
    [SerializeField] private float countDuration = 2.0f;//カウントしてからの経過時間


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        StartCoroutine(CountUP());
        //DisplayResult();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DisplayResult()
    {
        totalscoreText.text = totalScore.ToString()+"Gold";
    }

    private IEnumerator CountUP()
    {
        float elasped = 0.0f;

        while (elasped < countDuration)
        {
            elasped += Time.deltaTime;
            //カウントアップの進行度
            float progress = elasped / countDuration;
            int creentScore = Mathf.RoundToInt(Mathf.Lerp(0, totalScore, progress));

            totalscoreText.text = creentScore + "Gold";

            yield return null;
        }
        totalscoreText.text = totalScore + "Gold";
    }
}
