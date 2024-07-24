using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectManager : MonoBehaviour
{
    [SerializeField] Animator noteHitAnimator = null;
    [SerializeField] Animator judgementAnimator = null;
    [SerializeField] Image judgementImage = null;
    [SerializeField] Sprite[] judgementSprite = null;

    string hit = "Hit";
    string end = "End";

    public void JudgementEffect(int p_num)
    {
        judgementImage.sprite = judgementSprite[p_num];
        judgementAnimator.SetTrigger(hit);
    }

    public void JudgementEffectEnd()
    {
        judgementImage.enabled = false;
        judgementAnimator.SetTrigger(end);
    }

    public void NoteHitEffect()
    {
        noteHitAnimator.SetTrigger(hit);
    }


}
