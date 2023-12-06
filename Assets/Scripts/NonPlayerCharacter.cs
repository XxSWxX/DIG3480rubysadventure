using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//MAIN
public class NonPlayerCharacter : MonoBehaviour
{
    public float displayTime = 4.0f;
    public GameObject dialogBox;
    public AudioClip OpenDialogueClip;
    float timerDisplay;

    public AudioSource audioSource;

    void Start()
    {
        dialogBox.SetActive(false);
        timerDisplay = -1.0f;
        audioSource= GetComponent<AudioSource>();

    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    void Update()
    {
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                dialogBox.SetActive(false);
            }
        }
    }
    
    public void DisplayDialog()
    {
        timerDisplay = displayTime;
        dialogBox.SetActive(true);
        PlaySound(OpenDialogueClip);
    }
}