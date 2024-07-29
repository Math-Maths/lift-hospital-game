using UnityEngine;

namespace LiftHospital
{

public class PickObjectBehaviour : MonoBehaviour
{
    private BoxCollider[] myCollider;

    private void OnEnable()
    {
        myCollider = GetComponents<BoxCollider>();
    }

    public void GotPicked()
    {
        gameObject.tag = "picked";
        foreach(var item in myCollider)
        {
            item.enabled = false;
        }
        
    }

    public void GotLoose()
    {
        foreach(var item in myCollider)
        {
            item.enabled = true;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.name == "ground" && gameObject.GetComponent<Rigidbody>())
        {
            gameObject.tag = "pick-obj";
            Rigidbody rb = GetComponent<Rigidbody>();
            Destroy(rb);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("stil-place"))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            Destroy(rb);

            transform.position = other.transform.position;
        }
    }

}

}