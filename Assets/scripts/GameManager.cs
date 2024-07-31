using UnityEngine;

namespace LiftHospital
{

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    #region private fields

    private LevelData levelData;

    #endregion

    void Awake()
    {

        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        if(levelData = GameObject.FindAnyObjectByType<LevelData>())
        {
            //inicar o lvl
        }

    }

    

}

}