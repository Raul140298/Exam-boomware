using System.Collections;
using UnityEngine;

public class Arcade : Servicer, HandledBehaviour.IHandlerBehaviour
{
	private Animator ani;
	private AudioSource audio_play;
	[SerializeField] private AudioClip audio_clip;
	[SerializeField] private eArcadeType arcade_type;
    
	//************************ CORE **************************************//
	protected override void Awake()
	{
		ani = transform.GetChild(0).GetComponent<Animator>();
		base.Awake();
	}

	void OnDestroy()
	{
		StopAudioPlay();
	}
    
	//************************ STATES **************************************//
	public void OnEnterState(string name, Animator animator, AnimatorStateInfo stateInfo)
	{
	}

	public void OnExitState(string name, Animator animator, AnimatorStateInfo stateInfo)
	{
	}
    
	//************************ POKE **************************************//
	public void InsertCoin() //Called when a player insert a coin
	{
		PlayAnimation();

		int level = Level;
		
		float timeToWait = 1;
		
		switch (arcade_type)
		{
			case eArcadeType.ArcadeHw:
				if (level == 1)
				{
					timeToWait = 6.5f;
				}
				else if (level == 2)
				{
					timeToWait = 5.6f;
				}
				else
				{
					timeToWait = 3f;
				}
				break;

			case eArcadeType.ArcadeInd:
				timeToWait = 10;
				break;

			case eArcadeType.BasketBra:
				if (level == 1)
				{
					timeToWait = 6.5f;
				}
				else
				{
					timeToWait = 5.6f;
				}
				break;
		}

		StartCoroutine(CRTEndCoin(timeToWait));

		if (audio_clip != null)
		{
			audio_play = gameObject.AddComponent<AudioSource>();
			audio_play.clip = audio_clip;
			audio_play.Play();
		}
	}

	IEnumerator CRTEndCoin(float timeToWait)
	{
		yield return new WaitForSeconds(timeToWait);
		EndCoin();
	}

	private void EndCoin()
	{
		StopAnimation();
		Collect();
		StopAudioPlay();
	}

	private void PlayAnimation()
	{
		ani.SetBool("isPlaying", true);
	}
	
	private void StopAnimation()
	{
		ani.SetBool("isPlaying", false);
	}

	private void StopAudioPlay()
	{
		if (audio_play != null)
		{
			audio_play.Stop();
		}
	}
}
