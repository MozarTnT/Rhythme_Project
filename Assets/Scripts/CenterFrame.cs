using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterFrame : MonoBehaviour
{
    bool isMusicStart = false;

    public string bgmName = "";
    public void ResetMusic()
    {
        isMusicStart = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isMusicStart)
        {
            if (collision.CompareTag("Note"))
            {
                AudioManager.instance.PlayBGM(bgmName);
                isMusicStart = true;
            }
        }
    }
}
