using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    public List<GameObject> boxNoteList = new List<GameObject>();

    int[] judgementRecord = new int[5];


    [SerializeField] Transform Center = null;
    [SerializeField] RectTransform[] timingRect = null;
    Vector2[] timingBoxes = null;

    EffectManager theEffect;
    ScoreManager theScoreManager;
    ComboManager theComboManager;
    StageManager theStageManager;
    StatusManager theStatusManager;
    AudioManager theAudioManager;
    PlayerController thePlayer;


    void Start()
    {
        theAudioManager = AudioManager.instance;
        theEffect = FindObjectOfType<EffectManager>();
        theScoreManager = FindObjectOfType<ScoreManager>();
        theComboManager = FindObjectOfType<ComboManager>();
        theStageManager = FindObjectOfType<StageManager>();
        thePlayer = FindObjectOfType<PlayerController>();
        theStatusManager = FindObjectOfType<StatusManager>();

        //Ÿ�̹� �ڽ�

        timingBoxes = new Vector2[timingRect.Length];

        for (int i = 0; i < timingRect.Length; i++)
        {
            timingBoxes[i].Set(Center.localPosition.x - timingRect[i].rect.width / 2,
                               Center.localPosition.x + timingRect[i].rect.width / 2);
        }
    }

    public bool CheckTiming()
    {
        for (int i = 0; i < boxNoteList.Count; i++)
        {
            float t_notePosX = boxNoteList[i].transform.localPosition.x;

            for (int x = 0; x < timingBoxes.Length; x++)
            {
                if (timingBoxes[x].x <= t_notePosX && t_notePosX <= timingBoxes[x].y)
                {
                    // ��Ʈ ����
                    boxNoteList[i].GetComponent<Note>().HideNote();
                    boxNoteList.RemoveAt(i);

                    // ����Ʈ ����
                    if (x < timingBoxes.Length - 1)
                    {
                        theEffect.NoteHitEffect();
                    }

                    if (CheckCanNextPlate())
                    {
                        theScoreManager.IncreaseScore(x); // ���� ����
                        theStageManager.ShowNextPlates(); // ���� ���� ����
                        theEffect.JudgementEffect(x); // ���� ����
                        judgementRecord[x]++; // ���� ���
                        theStatusManager.CheckShield();
                    }
                    else
                    {
                        theEffect.JudgementEffect(5);
                    }

                    theAudioManager.PlaySFX("Clap");
                    return true;
                }
            }
        }
        //Debug.Log("Miss");

        theComboManager.ResetCombo();
        theEffect.JudgementEffect(timingBoxes.Length);
        MissRecord();
        return false;
    }


    bool CheckCanNextPlate()
    {
        if (Physics.Raycast(thePlayer.destPos, Vector3.down, out RaycastHit t_hitInfo, 1.1f))
        {
            if (t_hitInfo.transform.CompareTag("BasicPlate"))
            {
                BasicPlate t_plate = t_hitInfo.transform.GetComponent<BasicPlate>();
                if (t_plate.flag)
                {
                    t_plate.flag = false;
                    return true;
                }
            }
        }

        return false;
    }

    public int[] GetJudgementRecord()
    {
        return judgementRecord;
    }

    public void MissRecord()  // miss ���� ���
    {
        judgementRecord[4]++;
        theStatusManager.ResetShieldCombo();
    }

    public void Initialized()
    {
        for(int i = 0; i < judgementRecord.Length; i++)
        {
            judgementRecord[i] = 0;
        }
    }
}
