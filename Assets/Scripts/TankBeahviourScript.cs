using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class TankBeahviourScript : MonoBehaviour 
{
	private Transform myTransform;
	public GameObject selongsong;
	public GameObject titikTembakan;
	private AudioSource audioSource;
	private string stateRotasiVertikal; //aman, bawah, atas
	// aman ---> bisa dilakukan interaksi vertikal atas bawah
	// bawah ---> mentol ke bawah mendekati 0 (nilairotasiY),, harus direset ke -0.5
	// atas ---> mentok ke atas mendekati 15 (inisialisasirotasiY),, haris direset ke -14.5

	public float kecepatanRotasi = 20;
	public float kecepatanAwalPeluru = 20;

	[HideInInspector] public float sudutMeriam;
	[HideInInspector] public float nilaiRotasiY;

	public GameObject objekTembakan;
	public GameObject objekLedakan;
	public GameObject peluruMeriam;
	public AudioClip audioTembakan;
	public AudioClip audioLedakan;

	// Use this for initialization
	void Start () 
	{
		myTransform = transform;

		//inisialisasi objek selongsong
		selongsong = myTransform.Find("selongsong").gameObject;

		// inisialisasi titik tembakan
		titikTembakan = selongsong.transform.Find("titiktembakan").gameObject;

		// inisialisasi komponen audio source
		audioSource = selongsong.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () 
	{
        #region Rotasi Horizontal
        if (Input.GetKey(KeyCode.T)) //rotasi horizontal berlawanan dengan jarum jam
        {
            myTransform.Rotate(Vector3.back * kecepatanRotasi * Time.deltaTime, Space.Self);
        }
        else if (Input.GetKey(KeyCode.U)) //rotasi horizontal searah dengan jarum jam
        {
            myTransform.Rotate(Vector3.forward * kecepatanRotasi * Time.deltaTime, Space.Self);
        }
		sudutMeriam = myTransform.localEulerAngles.z;
		#endregion

		#region Menentukan state
		nilaiRotasiY = 360 - selongsong.transform.localEulerAngles.x;
		if (nilaiRotasiY == 0 || nilaiRotasiY == 360)
        {
			stateRotasiVertikal = "aman";
        }
		else if ( nilaiRotasiY > 0 && nilaiRotasiY < 15)
        {
			stateRotasiVertikal = "aman";
        }
		else if (nilaiRotasiY > 15 && nilaiRotasiY < 16)
        {
			stateRotasiVertikal = "atas";
        }
		else if ( nilaiRotasiY > 350)
        {
			stateRotasiVertikal = "bawah";
        }
        #endregion

        #region Arah Rotasi Vertikal Berdasarkan State
		if (stateRotasiVertikal == "aman")
        {
			if ( Input.GetKey(KeyCode.Y))
            {
				selongsong.transform.Rotate(Vector3.left * kecepatanRotasi * Time.deltaTime, Space.Self);
            }
			else if ( Input.GetKey(KeyCode.H))
            {
				selongsong.transform.Rotate(Vector3.right * kecepatanRotasi * Time.deltaTime, Space.Self);
            }
        }
		else if ( stateRotasiVertikal == "bawah")
        {
			selongsong.transform.localEulerAngles = new Vector3(
				-0.5f, selongsong.transform.localEulerAngles.y, selongsong.transform.localEulerAngles.z);
        }
		else if ( stateRotasiVertikal == "atas")

        {
			selongsong.transform.localEulerAngles = new Vector3(
				-14.5f, selongsong.transform.localEulerAngles.y, selongsong.transform.localEulerAngles.z);
			
        }
		#endregion

		#region Penembakan
		if (Input.GetKeyDown(KeyCode.Space))
		{
			#region Init Peluru
			
				GameObject peluru = Instantiate(peluruMeriam, titikTembakan.transform.position, 
					Quaternion.Euler(selongsong.transform.localEulerAngles.x, 
					myTransform.localEulerAngles.z,
					0));
			#endregion

			#region Init Objek Tembakan
			GameObject efekTembakan = Instantiate(objekTembakan, titikTembakan.transform.position,
			Quaternion.Euler(
				selongsong.transform.localEulerAngles.x,
				myTransform.localEulerAngles.z, 0));

			Destroy(efekTembakan, 1f);
			#endregion

			#region Audio Objek Tambahan
			audioSource.PlayOneShot(audioTembakan);

            #endregion
        }

		#endregion

	}
}
