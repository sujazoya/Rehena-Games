using UnityEngine;
using System.Collections;

public class SkyboxRotator : MonoBehaviour {


    private float skyBoxRot;
    public Material skyboxMaterial;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {

        // Rotating Skybox
        if (skyboxMaterial != null)
        {

            if (skyBoxRot >= 360) skyBoxRot = 0;

            skyboxMaterial.SetFloat("_Rotation", skyBoxRot);

            skyBoxRot += 0.02f;

        }

    }
}
