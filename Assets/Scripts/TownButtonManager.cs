using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TownButtonManager : MonoBehaviour
{
	public Button play;
	public Button buyRound;
	public Button buyRelicTracker;
	public Button buyRope;
	public Text goldCounter;
	public Text relicCounter;
	public Text ropeCounter;
	private DataManager DataManager;

	// Start is called before the first frame update
	void Start()
    {
		DataManager = GameObject.Find("DataManager").GetComponent<DataManager>();

		goldCounter.text = "" + DataManager.GoldCounter;
		relicCounter.text = "" + DataManager.RelicCounter;
		ropeCounter.text = "" + DataManager.RopeCounter;

		play.onClick.AddListener(Play);
		buyRound.onClick.AddListener(AddFaith);
		buyRelicTracker.onClick.AddListener(AddRelicTracker);
		buyRope.onClick.AddListener(AddRope);

		DataManager.OwnRelicTracker = false;
    }

	private void Play()
	{
		SceneManager.LoadScene("Assets/Scenes/DungeonScene.unity");
	}

	private void AddFaith()
	{
		if (DataManager.GoldCounter >= 4)
		{
			SubGold(4);
			DataManager.Faith = DataManager.Faith + 5;
		}
	}

	private void AddRelicTracker()
	{
		if(!DataManager.OwnRelicTracker && DataManager.GoldCounter >= 10)
		{
			SubGold(10);
			DataManager.OwnRelicTracker = true;
			buyRelicTracker.transform.GetChild(0).GetComponent<Text>().text = "Purchased";
		}
	}

	private void AddRope()
	{
		if (DataManager.GoldCounter >= 2)
		{
			SubGold(2);
			DataManager.RopeCounter = DataManager.RopeCounter + 1;
			ropeCounter.text = "" + DataManager.RopeCounter;
		}
	}

	private void SubGold(int sub)
	{
		DataManager.GoldCounter = DataManager.GoldCounter - sub;
		goldCounter.text = "" + DataManager.GoldCounter;
	}
}
