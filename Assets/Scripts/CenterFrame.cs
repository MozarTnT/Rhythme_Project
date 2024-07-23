using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterFrame : MonoBehaviour
{

    bool isMusicStart = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isMusicStart)
        {
            if (collision.CompareTag("Note"))
            {
                AudioManager.instance.PlayBGM("BGM0");
                isMusicStart = true;
            }
        }
    }
}
