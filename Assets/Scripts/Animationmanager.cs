using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Animationmanager : MonoBehaviour
{
    public static Animationmanager instance;
    public List<Animation> animations = new List<Animation>();

    public bool isanimplaying()=> IsPlaying();
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
    }
    public void PlayAnim(int _Animidx, string _Clipname = "")
    {
        if (_Clipname == "") animations[_Animidx].Play();
        else animations[_Animidx].Play(_Clipname);

    }

    bool IsPlaying()
    {
        for (int i = 0; i < animations.Count; i++)
        {
            if (animations[i].isPlaying)
            {
                return true;
            }
        }
        return false;
    }
}
