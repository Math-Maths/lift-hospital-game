using System.Linq;
using UnityEngine;

namespace LiftHospital
{

public class LevelData : MonoBehaviour
{

    public static LevelData instance;

    [SerializeField] private string levelName;
    
    private int goalNum;
    private int currentFilledBeds;

    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    void Start()
    {
        goalNum = FindObjectsOfType(typeof(PatientBehaviour)).Length;
        Debug.Log(goalNum);
    }

    public void UpdateFilledBeds()
    {
        currentFilledBeds++;

        if(goalNum <= currentFilledBeds)
        {
            Debug.Log("Ganho carai");
            //win the level
        }
    }

}

}