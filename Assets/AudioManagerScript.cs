using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    public AudioClip glueHit;
    public AudioClip grab;
    public AudioClip hurt;
    public AudioClip shipHit;
    public AudioClip fittingTogether;
    public AudioClip shoot;
    public AudioClip breaking;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayGlueHit()
    {
        audioSource.PlayOneShot(glueHit);
    }

    public void PlayGrab()
    {
        audioSource.PlayOneShot(grab);
    }
    public void PlayHurt()
    {
        audioSource.PlayOneShot(hurt);
    }
    public void PlayShipHit()
    {
        audioSource.PlayOneShot(shipHit);
    }
    public void PlayFittingTogether()
    {
        audioSource.PlayOneShot(fittingTogether);
    }
    public void PlayShoot()
    {
        audioSource.PlayOneShot(shoot);
    }

    public void PlayBreaking()
    {
        audioSource.PlayOneShot(breaking);
    }

}
