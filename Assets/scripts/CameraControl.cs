using UnityEngine;

namespace LiftHospital
{

public class CameraControl : MonoBehaviour
{
    [SerializeField] Transform playerPos;
    
    [SerializeField] Vector3 offset; 

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