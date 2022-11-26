using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Scene_Locker : MonoBehaviour
{
    [SerializeField] float needRecordDistance;
    [SerializeField] Text needRecordDistance_Text;
    Button levelButton;

    [SerializeField] Text RecordDistance_Text;
    // Start is called before the first frame update
    void Start()
    {
        levelButton = GetComponentInParent<Button>();
        if(GameManager.RecordDistance> needRecordDistance)
        {
            transform.gameObject.SetActive(false);
        }
        else
        {
            transform.gameObject.SetActive(true);
            levelButton.interactable = false;
            if (needRecordDistance_Text)
            {
                needRecordDistance_Text.text ="Need Record Distance : " +needRecordDistance.ToString();
            }
        }
        if (RecordDistance_Text)
        {
            RecordDistance_Text.text = GameManager.RecordDistance.ToString();
        }
    }
   
}
