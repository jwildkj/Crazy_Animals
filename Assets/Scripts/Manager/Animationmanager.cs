using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Animationmanager : MonoBehaviour
{
    public static Animationmanager instance;
    public List<Animation> animations = new List<Animation>();
    Queue<Animation> animqueue = new Queue<Animation>();
    public bool isanimplaying()=> IsPlaying();

    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if (animqueue.Count > 0 && isanimplaying() == false)
        {
            animqueue.Dequeue().Play();
        }
    }
    public void PlayAnim(int _Animidx, string _Clipname = "", bool _Playimmediately = false)
    {

        if (_Clipname != "") animations[_Animidx].clip = animations[_Animidx].GetClip(_Clipname);

        if (_Playimmediately){
            animations[_Animidx].Play();
            return;
        }

        animqueue.Enqueue( animations[_Animidx]);
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
