using System;
using UnityEngine;

namespace LiftHospital
{

public class ChangeOppacity : MonoBehaviour
{

    private Transform playerPos;

    private void Start()
    {
        try
        {
            playerPos = GameObject.FindGameObjectWithTag(StringTag.Player).transform;
        }
        catch (Exception e)
        {
            Debug.LogError("No player found by the change oppacity. Error message: " + e);
        }
    }

    private void Update()
    {
        Ray ray = new Ray(transform.position, playerPos.position - transform.position);
        RaycastHit hitInfo;

        if(Physics.Raycast(ray, out hitInfo))
        {
            Debug.DrawLine(ray.origin, hitInfo.point, Color.green);
            Debug.Log(hitInfo.collider.gameObject.name);
        }
        else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.blue);
        }
    }
}

}