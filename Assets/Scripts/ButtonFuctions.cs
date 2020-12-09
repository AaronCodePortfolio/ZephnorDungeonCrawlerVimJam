using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonFuctions : MonoBehaviour
{
	public Button play;
	public Button instructions;
	public Button credits;

	private void Start()
	{
		play.onClick.AddListener(Play);
		instructions.onClick.AddListener(Instructions);
		credits.onClick.AddListener(Credits);
	}

	private void Play()
	{
		SceneManager.LoadScene("Assets/Scenes/Town.unity");

	}

	private void Instructions()
	{
		SceneManager.LoadScene("Assets/Scenes/Rules.unity");
	}

	private void Credits()
	{
		SceneManager.LoadScene("Assets/Scenes/Credits.unity");
	}
}

