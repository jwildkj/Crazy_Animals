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
        Image FadePannel = UImanager.instance.UIs[0].GetComponent<Image>();
        BlackInOut(FADE.IN, 0.5f, FadePannel, this, () => Changescene(_sceneidx));
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
        if (scene.name == "MainGame") 
            Delegate.OnMainGameLoaded?.Invoke();
        if (scene.name == "Prologue")
            Delegate.OnPrologueLoaded?.Invoke();
    }



}
