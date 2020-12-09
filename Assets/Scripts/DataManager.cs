using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
	int goldCounter;
	int relicCounter;
	int ropeCounter;
	int faith;
	private bool ownRelicTracker;


	// Start is called before the first frame update
	void Start()
    {
		goldCounter = 3;
		relicCounter = 0;
		ropeCounter = 2;
		faith = 5;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	public int GoldCounter
	{
		get
		{
			return this.goldCounter;
		}
		set
		{
			this.goldCounter = value;
		}
	}

	public int RelicCounter
	{
		get
		{
			return this.relicCounter;
		}
		set
		{
			this.relicCounter = value;
		}
	}

	public int RopeCounter
	{
		get
		{
			return this.ropeCounter;
		}
		set
		{
			this.ropeCounter = value;
		}
	}

	public int Faith
	{
		get
		{
			return this.faith;
		}
		set
		{
			this.faith = value;
		}
	}

	public bool OwnRelicTracker
	{
		get
		{
			return this.ownRelicTracker;
		}
		set
		{
			this.ownRelicTracker = value;
		}
	}
}
