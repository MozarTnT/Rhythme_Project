using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public int bpm = 0;
    double currentTime = 0d;

    [SerializeField] Transform tfNoteAppear = null;

    TimingManager theTimingManager;
    EffectManager theeffectManager;
    ComboManager theComboManager;

    private void Start()
    {
        theTimingManager = GetComponent<TimingManager>();
        theeffectManager = FindObjectOfType<EffectManager>();
        theComboManager = FindObjectOfType<ComboManager>();
    }
    void Update()
    {
        if(GameManager.instance.isStartGame)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= 60d / bpm)
            {
                GameObject t_note = ObjectPool.instance.noteQueue.Dequeue();
                t_note.transform.position = tfNoteAppear.position;

                t_note.SetActive(true);
                theTimingManager.boxNoteList.Add(t_note);
                currentTime -= 60d / bpm; // 0으로 초기화 하면 time.deltatime을 더해주면서 오차가 생김
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Note"))
        {
            if (collision.GetComponent<Note>().GetNoteFlag())
            {
                theTimingManager.MissRecord();
                theeffectManager.JudgementEffect(4); // 벗어낫을때 miss 연출
                theComboManager.ResetCombo();
            }


            theTimingManager.boxNoteList.Remove(collision.gameObject);
            ObjectPool.instance.noteQueue.Enqueue(collision.gameObject);
            collision.gameObject.SetActive(false);

        }
    }


    public void RemoveNote() // Goal 도착 후 note 비활성화
    {
        GameManager.instance.isStartGame = false;

        for(int i = 0; i < theTimingManager.boxNoteList.Count; i++)
        {
            theTimingManager.boxNoteList[i].SetActive(false); // 남은 노트들 비활성화
            ObjectPool.instance.noteQueue.Enqueue(theTimingManager.boxNoteList[i]); // 오브젝트 풀에 반납
        }

        theTimingManager.boxNoteList.Clear();
    }
}
