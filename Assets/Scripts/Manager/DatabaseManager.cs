using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public int[] score; // 스코어 점수 기록용


    private void Start()
    {
        LoadScore();
    }


    public void SaveScore()
    {
        for (int i = 0; i < score.Length; i++)
        {
            PlayerPrefs.SetInt($"Score{i + 1}", score[i]);
        }
    }

    public void LoadScore()
    {
        for (int i = 0; i < score.Length; i++)
        {
            if (PlayerPrefs.HasKey($"Score{i + 1}"))
            {
                score[i] = PlayerPrefs.GetInt($"Score{i + 1}");
            }
        }
    }

}
