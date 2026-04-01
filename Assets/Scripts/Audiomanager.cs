using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audiomanager : MonoBehaviour
{
    [SerializeField] private List< AudioClip> BGM;
    [SerializeField] private List< AudioClip> VFX;
    private AudioSource AudioSource;
    // Start is called before the first frame update
    private void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        AudioClip[] bgms = Resources.LoadAll<AudioClip>("Sounds/BGM");
        foreach (var item in bgms) BGM.Add(item);
        AudioClip[] vfxs = Resources.LoadAll<AudioClip>("Sounds/VFX");
        foreach (var item in bgms) VFX.Add(item);
    }
    public void PlayAudio()
    {
        AudioSource.Play();
    }
}
