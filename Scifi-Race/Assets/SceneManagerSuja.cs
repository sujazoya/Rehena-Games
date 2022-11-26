using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerSuja : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

   public void LoadScene(string name)
    {
        Level_Loader.instance.LoadLevel(name);
    }
}
