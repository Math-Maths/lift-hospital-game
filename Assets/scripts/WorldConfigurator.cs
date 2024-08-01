using UnityEngine;

namespace LiftHospital
{

public class WorldConfigurator : MonoBehaviour
{

    public static WorldConfigurator Instance;
    public float ObstructionFadingSpeed = 1f;
    public Material transparentMaterial;

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

}

}