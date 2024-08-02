using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace LiftHospital
{

public class PatientBehaviour : MonoBehaviour
{
    private BoxCollider[] myCollider;
    private Outline outline;
    public PatientState currentState;

    public enum PatientState
    {
        HELPLESS,
        CARRYING,
        RESTING
    }

    private void OnEnable()
    {
        ChangeCurrentState(PatientState.HELPLESS);
        outline = GetComponent<Outline>();
        outline.enabled = false;
        myCollider = GetComponents<BoxCollider>();
    }

    void ChangeCurrentState(PatientState state)
    {
        currentState = state;
    }

    public void GotPicked()
    {
        outline.enabled = false;
        ChangeCurrentState(PatientState.CARRYING);
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
        yield return new WaitForSeconds(3f);
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        Destroy(rb);
        if(currentState == PatientState.RESTING)
        {

        }
        else
        {
            ChangeCurrentState(PatientState.HELPLESS);
        }
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
        if(other.CompareTag(ConstantsValues.RestSpot))
        {
            BoxCollider restCollider = other.GetComponent<BoxCollider>();
            restCollider.enabled = false;
            MeshRenderer restRenderer = other.GetComponent<MeshRenderer>();
            restRenderer.enabled = false;
            Rigidbody rb = GetComponent<Rigidbody>();
            Destroy(rb);
            ChangeCurrentState(PatientState.RESTING);

            transform.position = other.transform.position;
            transform.rotation = other.transform.rotation;
            LevelManager.instance.UpdateFilledBeds();
        }
        else if(other.CompareTag(ConstantsValues.Player) && currentState == PatientState.HELPLESS)
        {
            outline.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag(ConstantsValues.Player))
        {
            outline.enabled = false;
        }
    }

}

}