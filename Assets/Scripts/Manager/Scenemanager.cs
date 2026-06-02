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
            Fadein();
            Changescene(_sceneidx); 
        });
    }

    public void Fadein()
    {
        BlackInOut(FADE.OUT, 0.5f, fadepannel, this, () => { fadepannel.enabled = false; } );
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
