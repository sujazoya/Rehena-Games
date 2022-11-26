using UnityEngine;
using System.Collections;

namespace TappyTower
{
    public class CharacterManager : MonoBehaviour
    {
        public static CharacterManager Instance;

        public static readonly string CURRENT_CHARACTER_KEY = "SGLIB_CURRENT_CHARACTER";
 //       public static readonly string CURRENT_CHARACTER_ROOF_KEY = "SGLIB_CURRENT_ROOF";

        public int CurrentCharacterIndex
        {
            get
            {
                return
                    PlayerPrefs.GetInt(CURRENT_CHARACTER_KEY, 0);
            }
            set
            {
                PlayerPrefs.SetInt(CURRENT_CHARACTER_KEY, value);
                PlayerPrefs.Save();
            }
        }
        //public int CurrentCharacterRoofIndex
        //{
        //    get
        //    {
        //        return
        //            PlayerPrefs.GetInt(CURRENT_CHARACTER_ROOF_KEY, 0);
        //    }
        //    set
        //    {
        //        PlayerPrefs.SetInt(CURRENT_CHARACTER_ROOF_KEY, value);
        //        PlayerPrefs.Save();
        //    }
        //}

        public GameObject[] characters;
//        public GameObject[] roof;

        void Awake()
        {
            if (Instance)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}