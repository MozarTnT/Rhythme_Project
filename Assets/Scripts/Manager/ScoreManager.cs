using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtScore = null;

    [SerializeField] int increaseScore = 10;

    int currentScore;

    [SerializeField] float[] weight = null;
    [SerializeField] int comboBonusScore = 10;

    Animator myAnim;
    string animScoreUp = "ScoreUp";

    ComboManager theCombo;

    void Start()
    {
        myAnim = GetComponent<Animator>();
        theCombo = FindObjectOfType<ComboManager>();

        currentScore = 0;
        txtScore.text = "0";
    }

    public void IncreaseScore(int p_JudgementState)
    {
        // 콤보 증가
        theCombo.IncreaseCombo();

        // 콤보 보너스 점수 계산
        int t_currentCombo = theCombo.GetCurrentCombo();
        int t_bonusComboScore = (t_currentCombo / 10) * comboBonusScore;


        // 판정 가중치 계산
        int t_increaseScore = increaseScore + t_bonusComboScore;
        t_increaseScore = (int)(t_increaseScore * weight[p_JudgementState]);


        // 점수 반영
        currentScore += t_increaseScore;
        txtScore.text = string.Format("{0:#,##0}", currentScore);


        // 애니메이션 실행
        myAnim.SetTrigger(animScoreUp);
    }
}
