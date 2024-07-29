using System.Collections;
using UnityEngine;

namespace LiftHospital
{

public class PickObjectBehaviour : MonoBehaviour
{
    private BoxCollider[] myCollider;
    private Outline outline;

    private void OnEnable()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
        myCollider = GetComponents<BoxCollider>();
    }

    public void GotPicked()
    {
        outline.enabled = false;
        gameObject.tag = StringTag.PickedObject;
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
        StartCoroutine(EnablePicking());
    }

    IEnumerator EnablePicking()
    {
        Debug.Log("call");
        yield return new WaitForSeconds(3f);
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        Destroy(rb);
        gameObject.tag = StringTag.PickObject;
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.name == "ground" && gameObject.GetComponent<Rigidbody>())
        {
            transform.rotation = Quaternion.Euler(new Vector3(-transform.rotation.x, 0, -transform.rotation.z));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(StringTag.StillPlace))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            Destroy(rb);

            gameObject.tag = StringTag.PickedObject;
    
            transform.position = other.transform.position;
        }
        else if(other.CompareTag(StringTag.Player) && gameObject.tag == StringTag.PickObject)
        {
            outline.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag(StringTag.Player))
        {
            outline.enabled = false;
        }
    }

}

}