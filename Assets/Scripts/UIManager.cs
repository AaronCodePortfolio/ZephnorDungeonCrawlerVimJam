using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[SerializeField] Canvas dungeonUI;

	DataManager dataManager;

	Text goldCounterDisplay;
	Text chestCounterDisplay;
	Text ropeCounterDisplay;


	private void Start()
	{
		dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();
		goldCounterDisplay = dungeonUI.transform.GetChild(0).GetComponent<Text>();
		chestCounterDisplay = dungeonUI.transform.GetChild(3).GetComponent<Text>();
		ropeCounterDisplay = dungeonUI.transform.GetChild(5).GetComponent<Text>();
		goldCounterDisplay.text = "" + dataManager.GoldCounter;
		chestCounterDisplay.text = "" + dataManager.RelicCounter;
		ropeCounterDisplay.text = "" + dataManager.RopeCounter;
	}

	public void AddGold(int add)
	{
		dataManager.GoldCounter += add;

		goldCounterDisplay.text = "" + dataManager.GoldCounter;
	}


	public void AddChest(int add)
	{
		dataManager.RelicCounter += add;

		chestCounterDisplay.text = "" + dataManager.RelicCounter;
	}

	public int Rope
	{
		get
		{
			return this.dataManager.RopeCounter;
		}
	}

	public void RemoveRope(int sub)
	{
		dataManager.RopeCounter -= sub;

		ropeCounterDisplay.text = "" + dataManager.RopeCounter;
	}
}
