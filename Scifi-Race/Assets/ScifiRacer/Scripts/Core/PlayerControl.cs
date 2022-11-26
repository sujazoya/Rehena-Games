using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using GoogleMobileAds.Api;
using System;
/// <summary>
/// The player controller
/// </summary>
public class PlayerControl : MonoBehaviour 
{
	public enum MobileInputMode
	{
		Touch,
		Accelerometer
	}

	[Tooltip("The starting speed of the vehicle - in m/s")]
	public float speed = 80.0f;
	private float storedSpeed;
	[Tooltip("The steering speed of the vehicle")]
	public float steerSpeed = 8.0f;
	[Tooltip("The amount the vehicle tilts when turning - in degrees")]
	public float tiltAngle = 30.0f;
	[Tooltip("The speed at which the vehicle turns")]
	public float tiltSpeed = 10.0f;

	[Tooltip("The Collision radius of the vehicle")]
	public float collisionRadius = 0.9f;
	[Tooltip("A prefab to instantiate when the vehicle crashes")]
	public GameObject collisionParticles;
	[Tooltip("The default prefab to use for the vehicle model")]
	public GameObject defaultShipPrefab;

	[Header("Mobile Options")]
	[Tooltip("What input method to use on mobile devices")]
	public MobileInputMode inputMode = MobileInputMode.Accelerometer;
	[Tooltip("How sensitive the turning is to changes in the device orientation")]
	[Range(1.0f, 10.0f)]
	public float accelerometerSensitivity = 4.0f;

	[Tooltip("Make the player invincible - useful for testing")]
	public bool isInvincible = false;

	private float steer = 0.0f;				// current steering value
	private float tilt = 0.0f;				// current tilt angle
	private float speedMultiplier = 1.0f;	// speed multiplier for speedboosts/slow motion effects
	private bool crashed = false;			// have we crashed?
	private float startingSpeed;			// initial speed at the start of the game

	private GameObject shipModel;           // our vehicle model

	[SerializeField] GameObject warnPanel;
	[SerializeField] Button warnCancelButton;
	[SerializeField] Button warnContinueButton;

	[SerializeField] GameObject[] ParticleStream;

	[SerializeField] Text timerText;
    public static	PlayerControl _instance;

	private int powerupLayer;               // layer that powerup collectables are on
	[SerializeField] GameObject touch_Setup;


	public UnityEvent onRewarded;
	public UnityEvent onClose;
	private void AddEvents()
	{
		if (AdmobAdmanager.readyToShoAd)
		{
			AdmobAdmanager.Instance.CurrentRewardedAd().OnUserEarnedReward += HandleRewardBasedVideoRewarded;
			AdmobAdmanager.Instance.CurrentRewardedAd().OnAdClosed += HandleRewardedAdClosed;

		}
	}
	public void HandleRewardBasedVideoRewarded(object sender, Reward args)
	{
		onRewarded.Invoke();
	}
	public void HandleRewardedAdClosed(object sender, EventArgs args)
	{
		onClose.Invoke();
	}
	public bool IsAdAvailable()
	{
		if (!AdmobAdmanager.readyToShoAd)
		{
			return false;
		}
		return true;
	}

	private void OnDisable()
	{
		if (AdmobAdmanager.readyToShoAd)
		{
			AdmobAdmanager.Instance.CurrentRewardedAd().OnUserEarnedReward -= HandleRewardBasedVideoRewarded;
			AdmobAdmanager.Instance.CurrentRewardedAd().OnAdClosed -= HandleRewardedAdClosed;
		}
	}
	

	/// <summary>
	/// Get the current steering value of the vehicle (between -1 and 1)
	/// </summary>
	/// <value>The current steering value</value>
	public float Steer		{ get { return steer; } }

	/// <summary>
	/// Get the current speed of the vehicle in m/s
	/// </summary>
	/// <value>The current speed</value>
	public float Speed		{ get { return crashed ? 0.0f : speed * speedMultiplier; } }
	public static int InputSetup
	{
		get { return PlayerPrefs.GetInt("InputSetup", 0); }
		set { PlayerPrefs.SetInt("InputSetup", value); }
	}

	/// <summary>
	/// Get or set the current speed multiplier.
	/// </summary>
	/// <value>The current speed multiplier</value>
	public float SpeedMultiplier	
	{
		get { return speedMultiplier; }
		set { speedMultiplier = value; }
	}


	void Awake()
	{
		
		_instance = this;		

		powerupLayer = LayerMask.NameToLayer("Powerup");

		// remember our starting speed.
		startingSpeed = speed;

		// create the default vehicle model
		SetShipModel(defaultShipPrefab);

		Reset();
		if (InputSetup <= 0)
		{
			if (touch_Setup) { touch_Setup.SetActive(true); }
		}
		else if (InputSetup > 0)
		{

			if (touch_Setup) { touch_Setup.SetActive(false); }
			CheckInputSetup();
		}
		warnPanel.SetActive(false);
		storedSpeed = speed;
		timerText.gameObject.SetActive(false);
	}
    private void Start()
    {
		warnCancelButton.onClick.AddListener(OnGameover);
		warnContinueButton.onClick.AddListener(OnContinue);
		Invoke("AddEvents",3);
	}
	void OnContinue()
    {
		AdmobAdmanager.Instance.ShowRewardedAd();
    }
    public void Setting()
    {
		if (touch_Setup) { touch_Setup.SetActive(true); }
	}

    public void Reset()
	{
		steer = 0.0f;
		tilt = 0.0f;
		speedMultiplier = 1.0f;
		crashed = false;
		speed = startingSpeed;
	}
	[SerializeField]GameObject ship;
	public void SetShipModel(GameObject shipPrefab)
	{
		// destroy the current model if it exists
		if(shipModel != null)
			Destroy (shipModel);
		
		// instantiate a new vehicle model, and parent it to this object
		shipModel = Instantiate(shipPrefab, transform.position, transform.rotation) as GameObject;
		shipModel.transform.parent = transform;
		
	}
    private void OnEnable()
    {
		ship = GameObject.FindGameObjectWithTag("Ship");
		ParticleStream = new GameObject[2];
		ParticleStream[0] = ship.transform.Find("ParticleStreamRight").gameObject;
		ParticleStream[1] = ship.transform.Find("ParticleStreamLeft").gameObject;
		for (int i = 0; i < ParticleStream.Length; i++)
		{
			ParticleStream[i].SetActive(true);
		}
	}
    private float GetSteerInput()
	{
		if(!Application.isMobilePlatform)
		{
			// Get GamePad/Keyboard input.
			return Input.GetAxis("Horizontal");
		}
		else if(inputMode == MobileInputMode.Touch)
		{
			// Touch the left side of the screen to turn left, right side to turn right
			if(Input.touchCount > 0)
				return Input.GetTouch(0).position.x < (0.5f * Screen.width) ? -1.0f : 1.0f;
			else
				return 0.0f;
		}
		else
		{
			// Use the orientation of the device as the steering value
			return accelerometerSensitivity * Input.acceleration.x;
		}
	}

	void Update () 
	{
		// get the steering input
		float steerValue = GetSteerInput();
		// vary the steering speed with the speed multiplier (but only a little, otherwise there's no benefit to slow motion)
		float steerSpd = steerSpeed * (Mathf.Lerp(speedMultiplier, 1.0f, 0.5f));
		// smoothly lerp our current steer value towards the target input value
		steer = Mathf.Lerp (steer, steerSpd * steerValue, tiltSpeed * Time.deltaTime);

		// tilt the vehicle as we steer
		float targetTilt = -steerValue * tiltAngle;
		tilt = Mathf.Lerp (tilt, targetTilt, tiltSpeed * Time.deltaTime);
		Vector3 rot = transform.eulerAngles;
		rot.z = tilt;
		transform.eulerAngles = rot;

		// check for collisions (use a slightly longer distance than we actually travelled to make sure we don't miss
		// any collisions - if we test the exact distance we've moved then we occasionaly fly straight through obstacles).
		// Note that because it is the scenery that moves, not the player, we can't use concave mesh colliders because they
		// must always be static. Simple Box colliders are usually just fine, we don't need very accurate collision volumes.
		float distMoved = 1.25f * Speed * Time.deltaTime;
		float extraOffset = collisionRadius;
		RaycastHit hit;
		if(Physics.SphereCast(transform.position - extraOffset * Vector3.forward, collisionRadius, Vector3.forward, out hit, distMoved + 2.0f * extraOffset))
		{
			if(hit.collider.gameObject.layer == powerupLayer)
			{
				// inform the powerup than we've collected it so it can do its thing.
				PowerupCollectable powerup = hit.collider.gameObject.GetComponent<PowerupCollectable>();
				if(powerup != null)
					powerup.OnCollected();
			}
			else if(!isInvincible)
			{
				Warn();
                // We've crashed!
                // instantiate our crash particles
                //GameObject particles = Instantiate(collisionParticles, transform.position, Quaternion.identity) as GameObject;
                //// destroy the particles object after 2 seconds.
                //Destroy(particles, 2.0f);

                //SoundManager.PlaySfx("Explosion");

                //// come to a complete stop and hide the player model
                //crashed = true;
                //steer = 0.0f;
                //gameObject.SetActive(false);

                //// inform the game manager of the incident
                //GameManager.OnGameOver();
            }
		}
	}
	public void OnRewarded()
    {
		warnPanel.gameObject.SetActive(false);		
		SoundManager.PlayMusic("GameLoop");
		speed = storedSpeed;
		isInvincible = true;
		StartCoroutine(MakeVicibleTheShip());
		ParticleStream = new GameObject[2];
		ParticleStream[0] = ship.transform.Find("ParticleStreamRight").gameObject;
		ParticleStream[1] = ship.transform.Find("ParticleStreamLeft").gameObject;
		for (int i = 0; i < ParticleStream.Length; i++)
		{
			ParticleStream[i].SetActive(true);
		} 
	}
	IEnumerator MakeVicibleTheShip()
    {
		timerText.gameObject.SetActive(true);
		timerText.text = "00.5";
		yield return new WaitForSeconds(0.5f);
		int t = 5;
		while (t > 0)
		{
			t--;
			timerText.text = "00: "
				+ t;
			yield return new WaitForSeconds(1);
		}
		timerText.gameObject.SetActive(false);
		isInvincible = false;
	}
	void Warn()
    {
		warnPanel.SetActive(true);
		speed = 0;      
		
		MusicManager.PauseMusic(0.5f);
		//AdmobAdmanager.Instance.ShowRewardedAd();
        if (ship)
        {
			ParticleStream = new GameObject[2];
			ParticleStream[0] = ship.transform.Find("ParticleStreamRight").gameObject;
			ParticleStream[1] = ship.transform.Find("ParticleStreamLeft").gameObject;
			if (ParticleStream.Length > 0)
			{
				for (int i = 0; i < ParticleStream.Length; i++)
				{
					ParticleStream[i].SetActive(false);
				}
			}
		}
		
	}
	public void OnGameover()
    {
		warnPanel.gameObject.SetActive(false);
		// We've crashed!
		// instantiate our crash particles
		GameObject particles = Instantiate(collisionParticles, transform.position, Quaternion.identity) as GameObject;
		// destroy the particles object after 2 seconds.
		Destroy(particles, 2.0f);

		SoundManager.PlaySfx("Explosion");

		// come to a complete stop and hide the player model
		crashed = true;
		steer = 0.0f;
		gameObject.SetActive(false);

		// inform the game manager of the incident
		GameManager.OnGameOver();
	}
	void CheckInputSetup()
    {
        if (InputSetup == 1)
        {
			inputMode = MobileInputMode.Touch;
			if (touch_Setup) { touch_Setup.SetActive(false); }
		}
		else if(InputSetup == 2)
        {
			inputMode = MobileInputMode.Accelerometer;
			if (touch_Setup) { touch_Setup.SetActive(false); }
		}

    }
	public void TouchInput()
    {
		InputSetup = 1;
		CheckInputSetup();
	}
	public void TiltInput()
	{
		InputSetup = 2;
		CheckInputSetup();
	}
	void OnDrawGizmosSelected()
	{
		// draw a preview of our collision sphere.
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, collisionRadius);
	}
}
