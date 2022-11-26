using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scifi_Menu : MonoBehaviour
{
    public  string levelName;

    public GameObject otherGames;

    public void LoadLevel(int levelNo)
    {
        ScifiSceneCreater.SceneType = levelNo;
        Level_Loader.instance.LoadLevel(levelName);

    }

    public void ShowOtherGames()
    {
        if (otherGames)
        {
            otherGames.SetActive(true);
        }
    }
    public void BackFromOther()
    {
        if (otherGames)
        {
            otherGames.SetActive(false);
        }
    }

    public void ShowPolici()
    {
        string policyLink = GoogleSheetHandler.terms_url;
        Application.OpenURL(policyLink);
    }
}
