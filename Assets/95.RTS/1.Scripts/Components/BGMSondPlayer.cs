using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMSondPlayer : BaseInteraction
{
    public MusicPlayType playType = MusicPlayType.PLAY;

    public int musicClip_index1 = 0;
    public int musicClip_index2 = 1;


    public float fadeTime = 1.0f;

    public Interpolate.EaseType easeType = Interpolate.EaseType.Linear;

    public void PlayMusic()
    {
        Debug.Log("PlayMusic");
        switch (this.playType)
        {
            case MusicPlayType.PLAY:
                Debug.Log("PlayBGM :" + this.musicClip_index1);
                SoundManager.Instance.PlayBGM(this.musicClip_index1);
                break;
            case MusicPlayType.PLAY_ONESHOT:
                SoundManager.Instance.PlayOneShot(this.musicClip_index1);
                break;
            case MusicPlayType.FADE_IN:
                SoundManager.Instance.FadeIn(this.musicClip_index1, this.fadeTime, this.easeType);
                break;
            case MusicPlayType.FADE_OUT:
                SoundManager.Instance.FadeOut(this.fadeTime, this.easeType);
                break;
            case MusicPlayType.FADE_TO:
                SoundManager.Instance.FadeTo(this.musicClip_index1, this.fadeTime, this.easeType);
                break;
            case MusicPlayType.STOP:
                SoundManager.Instance.BGMStop();
                break;
        }

    }

    private void Start()
    {
        if(EventStartType.AUTOSTART.Equals(this.startType))
        {
            Debug.Log("Play");
            PlayMusic();
        }

    }
    private void Update()
    {
        if(this.CheckVariables())
        {
            PlayMusic();
            if(this.repeatExecution == false)
            {
                RemoveVariableCondition(0);
            }
        }
       
    }


}
