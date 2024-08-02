using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

namespace LiftHospital
{

public class LevelManager : MonoBehaviour
{

    public static LevelManager instance;

    [SerializeField] private string levelName;
    [SerializeField] private float levelTime;
    [SerializeField] private TMP_Text timer;
    [SerializeField] GameObject loseHud;
    
    private int goalNum;
    private int currentFilledBeds;
    public bool isGameRunning = false;

    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        timer.text = "";
    }

    void Start()
    {
        goalNum = FindObjectsOfType(typeof(PatientBehaviour)).Length;
    }

    public void StartGame()
    {
        timer.text = levelTime.ToString();
        GameManager.instance.OnGameStart();
        isGameRunning = true;
        StartCoroutine(TimerDecrease());
    }

    IEnumerator TimerDecrease()
    {
        while(isGameRunning)
        {
            levelTime --;
            timer.text = levelTime.ToString();
            if(levelTime <= 0)
            {
                Debug.Log("Acabou :c");
                isGameRunning = false;
                loseHud.SetActive(true);
                GameManager.instance.OnGameEnd();
            }
            yield return new WaitForSeconds(1f);
        }
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