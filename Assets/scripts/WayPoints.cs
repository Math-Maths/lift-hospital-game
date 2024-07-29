using UnityEngine;

namespace LiftHospital
{

public class WayPoints : MonoBehaviour
{

    private Transform[] points = {};

    void Awake()
    {
        points = gameObject.GetComponentsInChildren<Transform>();
    }

    void OnDrawGizmos()
    {

        foreach(var item in points)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(item.position, .5f);
        }
    }

    

}

}