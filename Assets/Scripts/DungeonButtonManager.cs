using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DungeonButtonManager : MonoBehaviour
{
	public Button yes;
	public Button no;

    // Start is called before the first frame update
    void Start()
    {
		yes.onClick.AddListener(Yes);
		no.onClick.AddListener(No);
    }

   private void Yes()
	{
		GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ExitResult(true);
	}

	private void No()
	{
		GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ExitResult(false);
	}
}
