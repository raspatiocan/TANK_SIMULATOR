using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerakPeluruScript : MonoBehaviour 
{
	private Transform myTransform;
	public float waktuTerbangPeluru;

	private TankBeahviourScript tankBehaviour;
	private float _kecAwal;
	private float _sudutTembak;
	private float _sudutMeriam;
	private Vector3 _posisiAwal;
	private AudioSource audioSource;

	public GameObject ledakan;
	public AudioClip audioLedakan;

	private GameManagerScript gameManager;


	// Use this for initialization
	void Start () 
	{
		myTransform = transform;

		// tankbehaviour mengambil nilai dari tankbehaviourscript yang ada di object torque
		tankBehaviour = GameObject.FindObjectOfType<TankBeahviourScript>();
		//tankBehaviour = GameObject.Find("Torque").GetComponent<TankBeahviourScript>();

		_kecAwal = tankBehaviour.kecepatanAwalPeluru;
		_sudutTembak = tankBehaviour.nilaiRotasiY;
		_sudutMeriam = tankBehaviour.sudutMeriam;

		_posisiAwal = myTransform.position;

		// init audiosource
		audioSource = GetComponent<AudioSource>();

		// //init gravity
		// _gravitasi = GameObject.FindObjectOfType<TankBeahviourScript>().gravity;

		//init gamemanager
		gameManager = GameObject.FindObjectOfType<GameManagerScript>();
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//timer
		waktuTerbangPeluru += Time.deltaTime;

		gameManager._lamaWaktuTerbangPeluru = this.waktuTerbangPeluru;
		
		myTransform.position = PosisiTerbangPeluru(_posisiAwal, _kecAwal, waktuTerbangPeluru, _sudutTembak, _sudutMeriam);
		
	}
	private Vector3 PosisiTerbangPeluru(Vector3 _posisiAwal, float _kecAwal, float _waktu,
		float _sudutTembak, float _sudutMeriam)
    {
		float _x = _posisiAwal.x + (_kecAwal * _waktu * Mathf.Sin(_sudutMeriam * Mathf.PI / 180)) ;
		float _y = _posisiAwal.y + ((_kecAwal * _waktu * Mathf.Sin(_sudutTembak * Mathf.PI / 180)) - (0.5f * 10 * Mathf.Pow(_waktu, 2)));
		float _z = _posisiAwal.z + (_kecAwal * _waktu * Mathf.Cos(_sudutMeriam * Mathf.PI / 180));

		return new Vector3(_x, _y, _z);
    }

	private void onTriggerEnter (Collider other)
    {
		if ( other.tag == "Land")
        {
			// Debug.Log("kena tanah");
			//hancurkan peluru
			Destroy(this.gameObject, 2f);

			//munculkan efek ledakan
			GameObject go = Instantiate(ledakan, myTransform.position, Quaternion.identity);
			Destroy(go, 3f);

			//munculkan audio ledakan
			audioSource.PlayOneShot(audioLedakan);
        }
    }
}

//Part 8 menit 36