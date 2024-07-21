using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    public float noteSpeed = 400f;

    Image noteImage;

    private void OnEnable()
    {
        if(noteImage == null)
            noteImage = GetComponent<Image>();

        noteImage.enabled = true;
    }
    void Update()
    {
        transform.localPosition += Vector3.right * noteSpeed * Time.deltaTime;
    }

    public void HideNote()
    {
        noteImage.enabled = false;
    }

    public bool GetNoteFlag()
    {
        Debug.Log(noteImage.enabled);
        return noteImage.enabled;
    }
}
