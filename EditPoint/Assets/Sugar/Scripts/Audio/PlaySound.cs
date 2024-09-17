using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [Header("Ä¶‚·‚é‰¹‚ğ‚±‚±‚É“ü‚ê‚é")]
    [SerializeField] AudioClip[] clip;
    [Header("Ä¶‚·‚éBGM‚ğ‚±‚±‚É“ü‚ê‚é")]
    [SerializeField] AudioClip[] clipMusic;
    // Ä¶
    [SerializeField] AudioSource BGM;
    [SerializeField] AudioSource SE;

    public enum TYPE
    { 
        a,
        b,
        c
    }

    public void PlayBGM(int i)
    {
        BGM.clip = clipMusic[i];
        BGM.Play();
    }

    public void StopBGM()
    {
        BGM.Stop();
    }

    /// <summary>
    /// SE‚ğÄ¶‚·‚é‚æ
    /// </summary>
    /// <param name="i">Ä¶‚·‚éSE”Ô†</param>
    public void PlaySE(TYPE tYPE)
    {
        SE.PlayOneShot(clip[(int)tYPE]);
    }
}
