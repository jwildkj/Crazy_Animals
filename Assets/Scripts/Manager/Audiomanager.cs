using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audiomanager : MonoBehaviour
{
    public static Audiomanager instance;
    public bool Debug;
    private AudioSource BGMsource;
    private AudioSource SFXsource;
    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        BGMsource = transform.GetChild(0).GetComponent<AudioSource>();
        SFXsource = transform.GetChild(1).GetComponent<AudioSource>();        
        Delegate.OnTitleLoaded += ()=> {
            BGMsource.clip = Resourcemanager.instance.BGM[1];
            BGMsource.Play();
        };
        if (Debug) TitleBGM();
    }
    public void PlayBGMString(string _idx)
    {
        int result = 0;
        int.TryParse(_idx, out result);
        BGMsource.clip = Resourcemanager.instance.BGM[result];
        BGMsource.Play();
    }
    public void PlayBGM(int _idx)
    {
        BGMsource.clip = Resourcemanager.instance.BGM[_idx];
        BGMsource.Play();
    }
    public void PlaySFXString(string _idx)
    {
        int result = 0;
        int.TryParse(_idx, out result);
        SFXsource.clip = Resourcemanager.instance.SFX[result];
        SFXsource.Play();
    }
    public void PlaySFX(int _idx)
    {
        SFXsource.clip = Resourcemanager.instance.BGM[_idx];
        SFXsource.Play();
    }

    private void TitleBGM()
    {
        BGMsource.clip = Resourcemanager.instance.BGM[1];
        BGMsource.Play();
    }
}
