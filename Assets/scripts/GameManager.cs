using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LiftHospital
{

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public Action OnGameStart;
    public Action OnGameEnd;

    void Awake()
    {

        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}

}