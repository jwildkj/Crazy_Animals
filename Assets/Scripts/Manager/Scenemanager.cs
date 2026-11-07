using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Func;

public class Scenemanager : MonoBehaviour
{
    public static Scenemanager instance;
    [SerializeField] private Image fadepannel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += SceneChanged;
        }
        else
        {
            SceneManager.sceneLoaded -= SceneChanged;
            Destroy(gameObject);
        }
    }
    public void Changescene(int _sceneidx)
    {
        SceneManager.LoadScene(_sceneidx);
    }
    public void Changescene(string _sceneidx)
    {
        SceneManager.LoadScene(_sceneidx);
    }
    public void FadeOutAndChangeScene(string _sceneidx)
    {
        BlackInOut(FADE.IN, 0.5f, fadepannel, this, () => 
        { 
            FadeIn(0.5f);
            Changescene(_sceneidx); 
        });
    }

    public void FadeInOut(float _time)
    {
        BlackInOut(FADE.IN, _time, fadepannel, this, () =>
        {
            FadeIn(_time);
        });
    }

    public void FadeIn(float _time)
    {
        BlackInOut(FADE.OUT, _time, fadepannel, this, () => { fadepannel.enabled = false; } );
    }

    public void FadeOut(float _time)
    {
        BlackInOut(FADE.IN, _time, fadepannel, this);
    }

    public void Exit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    void SceneChanged(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == "InitialLogo")
            Invoke("LogoAnimation", 3);
        if (scene.name == "MainGame") 
            Delegate.OnMainGameLoaded?.Invoke();
        if (scene.name == "Prologue")
            Delegate.OnPrologueLoaded?.Invoke();
        if (scene.name == "Shop")
            Delegate.OnShopLoaded?.Invoke();
        if (scene.name == "Title")
            Delegate.OnTitleLoaded?.Invoke();
    }

    void LogoAnimation()
    {
        FadeOutAndChangeScene("Title");
    }



}
