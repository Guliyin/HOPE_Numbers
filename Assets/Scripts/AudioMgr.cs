using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sounds{
    read,
    correct,
    wrong,
    win
}

public class AudioMgr : MonoBehaviour
{
    private static AudioMgr instance;
    public static AudioMgr Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<AudioMgr>();
            return instance;
        }
    }

    private AudioSource audioSource;
    [SerializeField] private AudioClip[] clips;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlaySound(Sounds sound)
    {
        audioSource.clip = clips[(int)sound];
        audioSource.Play();
    }
}
