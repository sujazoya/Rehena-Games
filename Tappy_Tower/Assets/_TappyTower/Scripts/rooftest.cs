using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TappyTower
{
    public class rooftest : MonoBehaviour
    {
        bool IsNewCube = true;
        float NewCubeScale;
        Vector3 OldCubeScale;
        bool isPlay;
        public GameObject roof;
        float height;
        // Use this for initialization
        void Start()
        {
            // Create cube height equal with default height that user input
            transform.localScale = new Vector3(GameManager.Instance.startWidth, GameManager.Instance.startWidth, GameManager.Instance.startWidth);
        }
        // Update is called once per frame
        void OnBecameInvisible()
        {
            Destroy(gameObject); // Destroy game object when camera can't see it anymore
        }

        
        void Update()
        {
            if(!IsNewCube)
            {
                GetComponent<rooftest>().enabled = false;
            }
            if (IsNewCube)
            {
                OldCubeScale = PlayerController.OldCubeScale;
                if (transform.localScale.magnitude > OldCubeScale.magnitude * 1.5f)
                {
                    GameManager.Instance.playerController.LostLife();
                    GameManager.Instance.playerController.CreateScoreEffect(true, this.gameObject);
                    Destroy(gameObject);
                    IsNewCube = false;
                }
                // When player hold mouse or touch,the cube scale will increase
                if (Input.GetMouseButton(0))
                    transform.localScale += new Vector3(GameManager.Instance.IncreaseScaleSpeed, GameManager.Instance.IncreaseScaleSpeed, GameManager.Instance.IncreaseScaleSpeed); // Increase cube scale when hold mouse or touch
                Debug.Log("new cube " + transform.localScale.magnitude);
                // When player release mouse or touch,calculate cube scale to compare with the older one                                                                                                                            // When release mouse or touch,calculate current cube scale to compare with the older one
                if (Input.GetMouseButtonUp(0))
                {
                    Debug.Log("oldcube " + OldCubeScale.magnitude * GameManager.Instance.minimumScale);
                    NewCubeScale = transform.localScale.magnitude;
                    if ((NewCubeScale > OldCubeScale.magnitude) || (NewCubeScale <= (OldCubeScale.magnitude * GameManager.Instance.minimumScale)))
                    {
                        GameManager.Instance.playerController.LostLife();
                        GameManager.Instance.playerController.CreateScoreEffect(true, this.gameObject);
                        Destroy(gameObject);
                        if(PlayerController.life<=0)
                        {
                            GameObject _roof = (GameObject)Instantiate(roof,this.transform.position, Quaternion.Euler(0, 0, 0));
                            _roof.transform.localScale = this.transform.localScale;
                            GameManager.Instance.playerController.Die();
                        }
                    }
                    else if ((OldCubeScale.magnitude - NewCubeScale) <= GameManager.Instance.deviation)
                    {
                        GameManager.Instance.playerController.transform.position += new Vector3(0, (this.GetComponent<MeshFilter>().mesh.bounds.extents.y) * transform.transform.lossyScale.y * 2.0f,0);
                        transform.localScale = OldCubeScale;
                        SoundManager.Instance.PlaySound(SoundManager.Instance.unlock);
                        ScoreManager.Instance.AddScore(1);
                        CoinManager.Instance.AddCoins(1);
                        PlayerController.OldCubeScale = transform.localScale;
                        PlayerController.height += (this.GetComponent<MeshFilter>().mesh.bounds.extents.y) * transform.transform.lossyScale.y * 2.0f;
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
                    IsNewCube = false;
                }
            }
        }
    }
}