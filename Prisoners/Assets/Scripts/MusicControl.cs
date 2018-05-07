using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MusicControl : MonoBehaviour {

	public AudioMixerSnapshot[] fades;

	public AudioSource stingsource;
	public AudioClip stinger;

	public float transition = 0.4f;

	void Start()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		fades [scene.buildIndex].TransitionTo (transition);
		stingsource.clip = stinger;
		stingsource.Play ();
	}
}
