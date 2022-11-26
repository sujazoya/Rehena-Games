using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace TappyTower
{
    public class PlayerController : MonoBehaviour
    {
        public GameObject DefaultPosition;
        public static Vector3 OldCubeScale;
        public static event System.Action PlayerDied;
        public static event System.Action PlayerLostLife;
        public static event System.Action PlayerGetExtraLife;
        public static float height;
        public static float defaultHeight;
        public ParticleSystem scoreEffect;
        public ParticleSystem perfectEffect;
        Mesh characterMesh;
        bool isCreateCube = true;
        public static bool isPlay = false;
        Color defaultColor;
        float increaseScaleSpeed;
        public static int life;
        public static int score;
        GameObject oldCube;
        public Image countDownBar;
        bool isCountDown;
        float fillAmount;
        void OnEnable()
        {
            GameManager.GameStateChanged += OnGameStateChanged;
        }

        void OnDisable()
        {
            GameManager.GameStateChanged -= OnGameStateChanged;
        }

        void Start()
        {
            fillAmount = 1 / GameManager.Instance.countDownTime;
            defaultColor = scoreEffect.main.startColor.colorMax;
            if (CharacterManager.Instance.characters[CharacterManager.Instance.CurrentCharacterIndex] == null)
            {
                CharacterManager.Instance.CurrentCharacterIndex = 1;
            }
            GameObject character = (GameObject)Instantiate(CharacterManager.Instance.characters[CharacterManager.Instance.CurrentCharacterIndex], new Vector3(DefaultPosition.transform.position.x, DefaultPosition.transform.position.y, transform.position.z), Quaternion.Euler(0, 0, 0));
            character.transform.localScale = new Vector3(GameManager.Instance.defaultWidth, GameManager.Instance.defaultWidth, GameManager.Instance.defaultWidth);
            GameManager.Instance.height = (character.GetComponent<MeshFilter>().mesh.bounds.extents.y) * character.transform.lossyScale.y * 2.0f;
            oldCube = character;
            defaultHeight = GameManager.Instance.defaultHeight;
            DefaultPosition.transform.localScale = new Vector3(GameManager.Instance.defaultWidth, GameManager.Instance.defaultWidth, GameManager.Instance.defaultDepth);
            life = GameManager.Instance.life;
            height = GameManager.Instance.height;
            OldCubeScale = DefaultPosition.transform.localScale;
            //isPlay = true;
            characterMesh = character.GetComponent<MeshFilter>().sharedMesh;
            score = 0;
            transform.position = new Vector3(transform.position.x, DefaultPosition.transform.position.y + 8.0f + height, transform.position.z);
            //isCountDown = true;
            // Setup
        }
	    // Create effect when player score or lost life
        public void CreateScoreEffect(bool Score,GameObject newCube)
        {
            if (Score)
            {
                //height += GameManager.Instance.height;
                var main = scoreEffect.main;
                main.startColor = new Color(0.5f, 0, 0, 0.5f);
                scoreEffect.transform.position = new Vector3(newCube.transform.position.x, newCube.transform.position.y+0.003f, newCube.transform.position.z);
                //height -= GameManager.Instance.height;
                main.startSize = newCube.transform.localScale.x;
                if(main.startSize.constant < 2.0f)
                {
                    main.startSize = 2.0f;
                }
                scoreEffect.Play();
            }
            else
            {
                oldCube.GetComponent<CubeController>().enabled = false;
                oldCube = newCube;
                scoreEffect.transform.position = new Vector3(newCube.transform.position.x, newCube.transform.position.y+0.003f, newCube.transform.position.z);
                var main = scoreEffect.main;
                main.startColor = defaultColor;
                main.startSize = newCube.transform.localScale.x;
                if (main.startSize.constant < 2.0f)
                {
                    main.startSize = 2.0f;
                }
                scoreEffect.Play();
            }
        }
        // Create effect when player make new cube scale equal old cube scale
        public void CreatePerfectEffect(GameObject newCube)
        {
            oldCube.GetComponent<CubeController>().enabled = false;
            oldCube = newCube;
            var main = perfectEffect.main;
            perfectEffect.transform.position = new Vector3(newCube.transform.position.x, newCube.transform.position.y + 0.001f, newCube.transform.position.z);
            perfectEffect.GetComponent<ParticleSystemRenderer>().mesh = characterMesh;
            main.startSize3D = true;
            main.startSizeX= newCube.transform.localScale.x/1.75f;
            main.startSizeY = newCube.transform.localScale.y / 1.75f;
            main.startSizeZ = newCube.transform.localScale.z/1.75f;
            perfectEffect.Play();
//            NewCube.GetComponent<CubeController>().enabled = false;
        }
        // Create new cube when player touch
        void CreateNewCube()
        {
            GameObject character = (GameObject)Instantiate(CharacterManager.Instance.characters[CharacterManager.Instance.CurrentCharacterIndex], new Vector3(DefaultPosition.transform.position.x, DefaultPosition.transform.position.y + height, DefaultPosition.transform.position.z), Quaternion.Euler(0, 0, 0));
            character.GetComponent<CubeController>().enabled = true;
            isCreateCube = false;
        }
        // Update is called once per frame
        void Update()
        {
            if (isPlay)
            {
                if (Input.GetMouseButton(0) && isCreateCube)
                {
                    CreateNewCube();
                }
                if (Input.GetMouseButtonUp(0))
                {
                    countDownBar.fillAmount = 1.0f;
                    isCreateCube = true;
                    isCountDown = true;
                }

                if(score>= GameManager.Instance.scoreInscreaseSpeed)
                {
                    score = 0;
                    GameManager.Instance.IncreaseScaleSpeed += GameManager.Instance.inscreaseSpeed;
                    if (GameManager.Instance.inscreaseSpeed> GameManager.Instance.maxInscreaseScaleSpeed)
                    {
                        GameManager.Instance.inscreaseSpeed = GameManager.Instance.maxInscreaseScaleSpeed;
                    }
                }
                if(isCountDown)
                    countDownBar.fillAmount -= fillAmount * Time.deltaTime;
                if(countDownBar.fillAmount<=0)
                {
                    LostLife();
                    countDownBar.fillAmount = 1.0f;
                }
            }
            // Activities that take place every frame
        }

        // Listens to changes in game state
        void OnGameStateChanged(GameState newState, GameState oldState)
        {
            if (newState == GameState.Playing)
            {
                Invoke("GameStart", 0.1f);
                
                // Do whatever necessary when a new game starts
            }      
        }
        void GameStart()
        {
            isPlay = true;
            isCountDown = true;
        }
        //when new cube scale larger than old cube scale player will lost one life and create wrong sound
        public void LostLife()
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.wrong);
            life--;
            if(PlayerLostLife !=null)
            {
                PlayerLostLife();
            }
            
        }
        // Get extra life when player make new cube scale equal old cube

        public void GetExtraLife()
        {
            life++;
            if (life <= GameManager.Instance.life)
            {
                if (PlayerGetExtraLife != null)
                    PlayerGetExtraLife();
            }
            else
                life = GameManager.Instance.life;
        }
        // Calls this when the player dies and game over
        public void Die()
        {
            isPlay = false;
            // Fire event
            if (PlayerDied != null)
                countDownBar.enabled = false;
                PlayerDied();
        }
    }
}