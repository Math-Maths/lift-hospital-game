using System;
using UnityEngine;

namespace LiftHospital
{

public class CameraControl : MonoBehaviour
{
    
    private Transform playerPos;
    
    [SerializeField] Vector3 offset; 


    private void Start()
    {
        try
        {
            playerPos = GameObject.FindGameObjectWithTag(StringTag.Player).transform;
        }
        catch (Exception e)
        {
            Debug.LogError("No player found by the camera. Error message: " + e);
        }
    }

    private void LateUpdate()
    {
        if (playerPos != null)
        {
            Vector3 newPosition = playerPos.position + offset;
            newPosition.y = transform.position.y; 
            transform.position = newPosition;
        }
    }
}

}