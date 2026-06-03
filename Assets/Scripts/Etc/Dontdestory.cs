using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dontdestory : MonoBehaviour
{
    private static Dontdestory[] instance = new Dontdestory [2];
    [SerializeField] int id = 0;
    [SerializeField][Tooltip("월드 스페이스 캔버스는 씬 넘어가면 카메라가 빠지더라")] Canvas canvas;
    private void Awake()
    {
        if (instance[id] == null)
        {
            instance[id] = this;
            SceneManager.sceneLoaded += EnDisable;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            SceneManager.sceneLoaded -= EnDisable;
            Destroy(gameObject);
        }


    }

    void EnDisable(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(canvas && null == canvas.worldCamera)canvas.worldCamera = Camera.main;
        if (scene.name == "Shop" || scene.name == "Prologue" || scene.name == "Title")
            DisableUI();
        if (scene.name == "MainGame")
            EnableUI();
    }
    void EnableUI()
    {
        if(id == 1) { gameObject.SetActive(true); } 
    }
    void DisableUI()
    {
        if (id == 1) { gameObject.SetActive(false); }
    }

}
