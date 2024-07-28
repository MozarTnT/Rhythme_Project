using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField] GameObject goUI = null;

    [SerializeField] TextMeshProUGUI[] txtCount = null;
    [SerializeField] TextMeshProUGUI txtCoin = null;
    [SerializeField] TextMeshProUGUI txtScore = null;
    [SerializeField] TextMeshProUGUI txtMaxCombo = null;

    [SerializeField] GameObject JudgementImage;

    int currentSong = 0; // 현재 스테이지 참조용


    ScoreManager theScore;
    ComboManager theCombo;
    TimingManager theTiming;
    EffectManager theEffect;
    DatabaseManager theDatabase;
    void Start()
    {
        theScore = FindObjectOfType<ScoreManager>();
        theCombo = FindObjectOfType<ComboManager>();
        theTiming = FindObjectOfType<TimingManager>();
        theEffect = FindObjectOfType<EffectManager>();
        theDatabase = FindObjectOfType<DatabaseManager>();
    }

    public void SetCurrentSong(int p_songNum) // 현재 스테이지 참조용
    {
        currentSong = p_songNum;
    }

    public void ShowResult()
    {
        FindObjectOfType<CenterFrame>().ResetMusic();

        AudioManager.instance.StopBGM();

        theEffect.JudgementEffectEnd();

        goUI.SetActive(true);

        for(int i = 0; i < txtCount.Length; i++)
        {
            txtCount[i].text = "0";
        }

        txtCoin.text = "0";
        txtScore.text = "0";
        txtMaxCombo.text = "0";

        int[] t_judgement = theTiming.GetJudgementRecord();
        int t_currentScore = theScore.GetCurrentScore();
        int t_maxCombo = theCombo.GetMaxCombo();
        int t_coin = t_currentScore / 50;

        for(int i = 0; i < txtCount.Length; i ++)
        {
            txtCount[i].text = string.Format("{0:#,##0}", t_judgement[i]);
        }

        txtScore.text = string.Format("{0:#,##0}", t_currentScore);
        txtMaxCombo.text = string.Format("{0:#,##0}", t_maxCombo);
        txtCoin.text = string.Format("{0:#,##0}", t_coin);


        if(t_currentScore > theDatabase.score[currentSong])
        {
            theDatabase.score[currentSong] = t_currentScore;
            theDatabase.SaveScore();
        }


    }

    public void BtnMainMenu()
    {
        goUI.SetActive(false);
        GameManager.instance.MainMenu();
        theCombo.ResetCombo();
    }
}
