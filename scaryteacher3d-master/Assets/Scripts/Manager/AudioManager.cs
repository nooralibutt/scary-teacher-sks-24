using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private AudioSource audioSource;

    [SerializeField] private AudioClip backGroundMusic;
    [SerializeField] private AudioClip levelMusic;
    [SerializeField] private AudioClip babyCrying;
    [SerializeField] private AudioClip AngryTeacher;
    [SerializeField] private AudioClip teacherCrying;

    public bool isMusicEnabled = true;
    public bool isSoundEnabled = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void PlayBackgroundMusic()
    {
        if (isMusicEnabled)
        {
            audioSource.clip = backGroundMusic;
            audioSource.Play();
        }
    }
    public void PlayLevelMusic()
    {
        if (isMusicEnabled)
        {
            audioSource.clip = levelMusic;
            audioSource.Play();
        }
    }

    public void PlaySpecialAudios(GameConstants.InGameConstants.SpecialAudioClips specialAudioClips)
    {
        if (isSoundEnabled)
        {
            switch (specialAudioClips)
            {
                case GameConstants.InGameConstants.SpecialAudioClips.BabyCrying:
                    audioSource.PlayOneShot(babyCrying);
                    break;
                case GameConstants.InGameConstants.SpecialAudioClips.TeacherAngry:
                    audioSource.PlayOneShot(AngryTeacher);
                    break;
                case GameConstants.InGameConstants.SpecialAudioClips.TeacherCrying:
                    audioSource.PlayOneShot(teacherCrying);
                    break;
            }
        }
    }
}
