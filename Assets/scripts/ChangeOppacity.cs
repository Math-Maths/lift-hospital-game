using System;
using System.Collections.Generic;
using UnityEngine;

namespace LiftHospital
{

public class ChangeOppacity : MonoBehaviour
{

     public Transform player;
    public Vector3 offest;
    [SerializeField]
    private List<Transform> ObjectToHide = new List<Transform>();
    private List<Transform> ObjectToShow = new List<Transform>();
    private Dictionary<Transform, Material> originalMaterials = new Dictionary<Transform, Material>();

    void Start()
    {

    }

    private void LateUpdate()
    {
        ManageBlockingView();

        foreach (var obstruction in ObjectToHide)
        {
            HideObstruction(obstruction);
        }

        foreach (var obstruction in ObjectToShow)
        {
            ShowObstruction(obstruction);
        }
    }
   
    void ManageBlockingView()
    {
        Vector3 playerPosition = player.transform.position + offest;
        float characterDistance = Vector3.Distance(transform.position, playerPosition);
        int layerNumber = LayerMask.NameToLayer(ConstantsValues.ScenarioLayer);
        int layerMask = 1 << layerNumber;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, playerPosition - transform.position, characterDistance, layerMask);
        if (hits.Length > 0)
        {
            // Repaint all the previous obstructions. Because some of the stuff might be not blocking anymore
            foreach (var obstruction in ObjectToHide)
            {
                ObjectToShow.Add(obstruction);
            }

            ObjectToHide.Clear();

            // Hide the current obstructions
            foreach (var hit in hits)
            {
                Transform obstruction = hit.transform;
                ObjectToHide.Add(obstruction);
                ObjectToShow.Remove(obstruction);
                SetModeTransparent(obstruction);
            }
        }
        else
        {
            // Mean that no more stuff is blocking the view and sometimes all the stuff is not blocking as the same time
           
            foreach (var obstruction in ObjectToHide)
            {
                ObjectToShow.Add(obstruction);
            }

            ObjectToHide.Clear();

        }
    }

    private void HideObstruction(Transform obj)
    {
        //obj.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        var color = obj.GetComponent<Renderer>().material.color;
        color.a = Mathf.Max(.21f, color.a - WorldConfigurator.Instance.ObstructionFadingSpeed * Time.deltaTime);
        obj.GetComponent<Renderer>().material.color = color;

    }

    private void SetModeTransparent(Transform tr)
    {
        MeshRenderer renderer = tr.GetComponent<MeshRenderer>();
        Material originalMat = renderer.sharedMaterial;
        if (!originalMaterials.ContainsKey(tr))
        {
            originalMaterials.Add(tr, originalMat);
        }
        else
        {
            return;
        }
        Material materialTrans = new Material(WorldConfigurator.Instance.transparentMaterial);
        //materialTrans.CopyPropertiesFromMaterial(originalMat);
        renderer.material = materialTrans;
        renderer.material.mainTexture = originalMat.mainTexture;
    }

    private void SetModeOpaque(Transform tr)
    {
        if (originalMaterials.ContainsKey(tr))
        {
            tr.GetComponent<MeshRenderer>().material = originalMaterials[tr];
            originalMaterials.Remove(tr);
        }

    }

    private void ShowObstruction(Transform obj)
    {
        var color = obj.GetComponent<Renderer>().material.color;
        color.a = Mathf.Min(1, color.a + WorldConfigurator.Instance.ObstructionFadingSpeed * Time.deltaTime);
        obj.GetComponent<Renderer>().material.color = color;
        if (Mathf.Approximately(color.a, 1f))
        {
            SetModeOpaque(obj);
        }
    }

    
    #region First external try

    // public Transform player;
    // public Vector3 offset;
    // public Transform[] obstructions;

    // private int oldHitsNumber;

    // void Start()
    // {
    //     oldHitsNumber = 0;
    // }

    // private void LateUpdate()
    // {
    //     viewObstructed();
    // }

    // void viewObstructed()
    // {
    //     float characterDistance = Vector3.Distance(transform.position, player.transform.position);
    //     int layerNumber = LayerMask.NameToLayer(ConstStrings.ScenarioLayer);
    //     int layerMask = 1 << layerNumber;
    //     RaycastHit[] hits = Physics.RaycastAll(transform.position, player.position - transform.position, characterDistance, layerMask);
    //     if (hits.Length > 0)
    //     {
    //         // Means that some stuff is blocking the view
    //         int newHits = hits.Length - oldHitsNumber;

    //         if (obstructions != null && obstructions.Length > 0 && newHits < 0)
    //         {
    //             // Repaint all the previous obstructions. Because some of the stuff might be not blocking anymore
    //             for (int i = 0; i < obstructions.Length; i++)
    //             {
    //                 obstructions[i].gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    //             }
    //         }
    //         obstructions = new Transform[hits.Length];
    //         // Hide the current obstructions
    //         for (int i = 0; i < hits.Length; i++)
    //         {
    //             Transform obstruction = hits[i].transform;
    //             obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
    //             obstructions[i] = obstruction;
    //         }
    //         oldHitsNumber = hits.Length;
    //     }
    //     else
    //     {
    //         // Mean that no more stuff is blocking the view and sometimes all the stuff is not blocking as the same time
    //         if (obstructions != null && obstructions.Length > 0)
    //         {
    //             for (int i = 0; i < obstructions.Length; i++)
    //             {
    //                 obstructions[i].gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    //             }
    //             oldHitsNumber = 0;
    //             obstructions = null;
    //         }
    //     }
    // }

    #endregion

    #region Ray cast
    // private Transform playerPos;

    // private void Start()
    // {
    //     try
    //     {
    //         playerPos = GameObject.FindGameObjectWithTag(StringTag.Player).transform;
    //     }
    //     catch (Exception e)
    //     {
    //         Debug.LogError("No player found by the change oppacity. Error message: " + e);
    //     }
    // }

    // private void Update()
    // {
    //     Ray ray = new Ray(transform.position, playerPos.position - transform.position);
    //     RaycastHit hitInfo;

    //     if(Physics.Raycast(ray, out hitInfo))
    //     {
    //         Debug.DrawLine(ray.origin, hitInfo.point, Color.green);
    //         Debug.Log(hitInfo.collider.gameObject.name);
    //     }
    //     else
    //     {
    //         Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.blue);
    //     }
    // }

    #endregion
}

}