using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreditsButtonManager : MonoBehaviour
{
	public Button back;

	// Start is called before the first frame update
	void Start()
	{
		Destroy(GameObject.Find("DataManager"));
		back.onClick.AddListener(Back);
	}

	private void Back()
	{
		SceneManager.LoadScene("Assets/Scenes/TitleScreen.unity");
	}
}
