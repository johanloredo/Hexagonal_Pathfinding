using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXController : Singleton<SFXController>
{
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip uiButtonSFX;

    [SerializeField]
    private AudioClip startHexSFX;
    [SerializeField]
    private AudioClip endHexSFX;
    [SerializeField]
    private AudioClip obstacleSFX;
    [SerializeField]
    private AudioClip obstacleRemoveSFX;

    [SerializeField]
    private AudioClip hexSpawnSFX;
    [SerializeField]
    private AudioClip noPathSFX;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayUIButton()
    {
        audioSource.PlayOneShot(uiButtonSFX);
    }

    public void PlayStartHex()
    {
        audioSource.PlayOneShot(startHexSFX);
    }
    public void PlayEndHex()
    {
        audioSource.PlayOneShot(endHexSFX);
    }

    public void PlayObstacle()
    {
        audioSource.PlayOneShot(obstacleSFX);
    }

    public void PlayObstacleRemove()
    {
        audioSource.PlayOneShot(obstacleRemoveSFX);
    }

    public void PlayHexSpawn()
    {
        audioSource.PlayOneShot(hexSpawnSFX);
    }

    public void PlayNoPath()
    {
        audioSource.PlayOneShot(noPathSFX);
    }
}
