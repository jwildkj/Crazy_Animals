using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    }



}
