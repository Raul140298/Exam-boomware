using UnityEngine;
using System.Collections;

public class ArcadeBasketBra : Servicer, HandledBehaviour.IHandlerBehaviour {

	private Animator ani;
	private AudioSource audio_play;
	public AudioClip audio_clip;

//**********************************************************************//
//************************ CORE **************************************//
//**********************************************************************//
	protected override void Awake(){
		ani = transform.GetChild (0).GetComponent<Animator> ();

		base.Awake ();
	}

	protected override void Start ()
	{
		base.Start ();
	}

	void OnDestroy(){
		if (audio_play != null)
			audio_play.Stop ();
	}

//**********************************************************************//
//************************ STATES **************************************//
//**********************************************************************//
	public void OnEnterState(string name, Animator animator, AnimatorStateInfo stateInfo){

	}

	public void OnExitState(string name, Animator animator, AnimatorStateInfo stateInfo){

	}


//**********************************************************************//
//************************ POKE **************************************//
//**********************************************************************//
	//un cliente toca la maquina para empezar a operar
	public void Poke(){
		InsertCoin();
	}

	void InsertCoin(){
		ani.SetBool ("isPlaying", true);

		int level = GetComponent<ServicerData>().level;
		Invoke ("EndCoin", level == 1 ? 6.5f : 5.6f);

		//play sound
		audio_play = gameObject.AddComponent<AudioSource>();
		audio_play.clip = audio_clip;
		audio_play.Play();
	}

	void EndCoin(){
		ani.SetBool ("isPlaying", false);

		Collect ();

		//stop loop
		if (audio_play != null)
			audio_play.Stop ();
	}

}
