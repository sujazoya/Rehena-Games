using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TappyTower
{
    public class CubeController : MonoBehaviour
    {
        bool isNewCube = true;
        float newCubeScale;
        Vector3 oldCubeScale;
        bool isPlay;
        public GameObject roof;
        float height;
        bool isDead = false;
        bool isGround = true;

        // Use this for initialization
        void Start()
        {
            if (transform.position != GameManager.Instance.playerController.DefaultPosition.transform.position)
            {
                isGround = false;
            }
            // Create cube height equal with default height that user input
            if (!isGround)
                transform.localScale = new Vector3(GameManager.Instance.startWidth, GameManager.Instance.startWidth, GameManager.Instance.startWidth);
        }

        // Update is called once per frame

        IEnumerator CRZoom()
        {
            PlayerController.isPlay = false;
            var oriSize = Camera.main.orthographicSize;
            var targetSize = PlayerController.height/1.35f;
            var oriPosition = Camera.main.transform.position;
            var startTime = Time.time;
            float runTime = 0.5f;
            float timePast = 0;

            while (Time.time < startTime + runTime)
            {
                timePast += Time.deltaTime;
                float factor = timePast / runTime;
                if (PlayerController.height > GameManager.Instance.height * 7 && PlayerController.height <= GameManager.Instance.height * 20)
                    Camera.main.transform.position=oriPosition- new Vector3(0, factor * PlayerController.height*0.25f, 0);
                if (PlayerController.height > GameManager.Instance.height * 20 )
                    Camera.main.transform.position = oriPosition - new Vector3(0, factor * PlayerController.height * 0.2f, 0);
                Camera.main.orthographicSize = Mathf.Lerp(oriSize, targetSize, factor);
                yield return null;
            }
            GameManager.Instance.playerController.Die();
        }

        void Update()
        {
            if (PlayerController.life <= 0 & !isDead)
            {
                if (roof != null)
                {
                    GameObject _roof = (GameObject)Instantiate(roof, new Vector3(this.transform.position.x, this.transform.position.y + (this.GetComponent<MeshFilter>().mesh.bounds.extents.y) * this.transform.lossyScale.y * 2.0f, this.transform.position.z), Quaternion.Euler(0, 0, 0));
                    _roof.transform.localScale = transform.localScale;
                }
                if (PlayerController.height < GameManager.Instance.height * 7)
                {
                    PlayerController.height = GameManager.Instance.height * 7;
                }
                StartCoroutine(CRZoom());
                
                isDead = true;
            }
            if (isNewCube & !isGround)
            {
                oldCubeScale = PlayerController.OldCubeScale;
                if (transform.localScale.magnitude > oldCubeScale.magnitude * 1.5f)
                {
                    GameManager.Instance.playerController.LostLife();
                    GameManager.Instance.playerController.CreateScoreEffect(true, this.gameObject);
                    Destroy(gameObject);
                    isNewCube = false;
                }
                else
                {
                    // When player hold mouse or touch,the cube scale will increase
                    if (Input.GetMouseButton(0))
                        transform.localScale += new Vector3(GameManager.Instance.IncreaseScaleSpeed, GameManager.Instance.IncreaseScaleSpeed, GameManager.Instance.IncreaseScaleSpeed); // Increase cube scale when hold mouse or touch
                                                                                                                                                                                        // When player release mouse or touch,calculate cube scale to compare with the older one                                                                                                                            // When release mouse or touch,calculate current cube scale to compare with the older one
                    if (Input.GetMouseButtonUp(0))
                    {
                        newCubeScale = transform.localScale.magnitude;
                        if ((newCubeScale > oldCubeScale.magnitude) || (newCubeScale <= (oldCubeScale.magnitude * GameManager.Instance.minimumScale)))
                        {
                            GameManager.Instance.playerController.LostLife();
                            GameManager.Instance.playerController.CreateScoreEffect(true, this.gameObject);
                            Destroy(gameObject);
                        }
                        else if ((oldCubeScale.magnitude - newCubeScale) <= GameManager.Instance.deviation)
                        {
                            transform.localScale = oldCubeScale;
                            GameManager.Instance.playerController.transform.position += new Vector3(0, (this.GetComponent<MeshFilter>().mesh.bounds.extents.y) * transform.transform.lossyScale.y * 2.0f, 0);                            
                            PlayerController.OldCubeScale = transform.localScale;
                            PlayerController.height += (this.GetComponent<MeshFilter>().mesh.bounds.extents.y) * transform.transform.lossyScale.y * 2.0f;
                            SoundManager.Instance.PlaySound(SoundManager.Instance.unlock);
                            ScoreManager.Instance.AddScore(1);
                            CoinManager.Instance.AddCoins(1);
                            GameManager.Instance.playerController.CreatePerfectEffect(this.gameObject);
                            PlayerController.score += 1;
                            GameManager.Instance.playerController.GetExtraLife();
                        }
                        else
                        {
                            GameManager.Instance.playerController.transform.position += new Vector3(0, (this.GetComponent<MeshFilter>().mesh.bounds.extents.y) * transform.transform.lossyScale.y * 2.0f, 0);
                            PlayerController.score += 1;
                            PlayerController.height += (this.GetComponent<MeshFilter>().mesh.bounds.extents.y) * transform.transform.lossyScale.y * 2.0f;
                            SoundManager.Instance.PlaySound(SoundManager.Instance.score);
                            ScoreManager.Instance.AddScore(1);
                            PlayerController.OldCubeScale = transform.localScale;
                            GameManager.Instance.playerController.CreateScoreEffect(false, this.gameObject);
                        }
                        isNewCube = false;
                    }
                }
            }
        }
    }
}