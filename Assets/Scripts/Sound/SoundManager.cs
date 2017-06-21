using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public AudioSource 
        playerFxSource,
        treeFxSource,
        spellFxSource,
        fireballFxSource,
        ultiFxSource,
        musicSource;
    public AudioClip 
        blinkSFX,
        meleeSFX,
        rangeSFX,
        ultiSFX,
        lowHpSFX,
        deadSFX,
        damagedSFX,
        killSlimeSFX,
        levelUpSFX,
        treeLowSFX,
        treeDeadSFX;

    public static SoundManager instance = null;       
    public float lowPitchRange = 0.95f;              
    public float highPitchRange = 1.05f;            


    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void PlayerPlaySingle(AudioClip clip)
    {
        playerFxSource.clip = clip;
        playerFxSource.Play();
    }

    public void PlayerPlayOneShot(AudioClip clip)
    {
        playerFxSource.PlayOneShot(clip);
    }

    public void SpellPlaySingle(AudioClip clip)
    {
        spellFxSource.clip = clip;
        spellFxSource.Play();
    }

    public void SpellPlayOneShot(AudioClip clip)
    {
        spellFxSource.PlayOneShot(clip);
    }

    public void TreePlaySingle(AudioClip clip)
    {
        treeFxSource.clip = clip;
        treeFxSource.Play();
    }

    public void PlayerPlayDelayed(AudioClip clip, float delay)
    {
        playerFxSource.clip = clip;
        playerFxSource.PlayDelayed(delay);
    }

    public void SpellPlayDelayed(AudioClip clip, float delay)
    {
        spellFxSource.clip = clip;
        spellFxSource.PlayDelayed(delay);
    }

    public void TreePlayDelayed(AudioClip clip, float delay)
    {
        treeFxSource.clip = clip;
        treeFxSource.PlayDelayed(delay);
    }

    public void FireballPlaySingle(AudioClip clip)
    {
        fireballFxSource.clip = clip;
        fireballFxSource.Play();
    }

    public void FireballPlayDelayed(AudioClip clip, float delay)
    {
        fireballFxSource.clip = clip;
        fireballFxSource.PlayDelayed(delay);
    }

    public void UltiPlaySingle(AudioClip clip)
    {
        ultiFxSource.clip = clip;
        ultiFxSource.Play();
    }

    public void UltiPlayDelayed(AudioClip clip, float delay)
    {
        ultiFxSource.clip = clip;
        ultiFxSource.PlayDelayed(delay);
    }
}