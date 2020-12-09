using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class PlayerController : MonoBehaviour
{
	private int movementFrames = 10;
	private int framesElapsed;
	private Vector3 startPos;
	private Vector3 endPos;
	[SerializeField] Sprite walkRight;
	[SerializeField] Sprite walkLeft;
	LevelManager DungeonMaster;
	UIManager UIManager;
	private Text leaveDungeonQuery;
	private GameObject arrow;
	private Vector3 treasurePos;
	private DataManager dataManager;

	float wobble = 0;
	bool wobbleing;
	bool moving;
	bool falling;
	bool treasureGot;
	bool menuHold;
	bool leaving;
	bool moveArrow;
	GameObject isOnCracked;
	

    // Start is called before the first frame update
    void Start()
    {
		dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();
		arrow = gameObject.transform.GetChild(1).gameObject;
		DungeonMaster = GameObject.Find("DungeonBuilder").GetComponent<LevelManager>();
		UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
		leaveDungeonQuery = GameObject.Find("LeaveDungeonQuery").GetComponent<Text>();
		leaveDungeonQuery.gameObject.SetActive(false);
		startPos = gameObject.transform.position;
		endPos = AddVectorThree(startPos, Vector3.right);
		framesElapsed = 1;
		moving = true;
		wobbleing = true;
		isOnCracked = null;
		falling = false;
		treasureGot = false;
		menuHold = false;
		leaving = false;
		moveArrow = true;

		if(!dataManager.OwnRelicTracker)
		{
			arrow.SetActive(false);
		}
	}


	// Update is called once per frame
	void Update()
    {
		if(!menuHold)
		{
			if(dataManager.OwnRelicTracker)
			{
				if (moveArrow)
				{
					RotateArrow();
				}
			}
			
		
			if (moving)
			{
				if (falling)
				{
					gameObject.transform.Rotate(new Vector3(0, 0, 2));
					gameObject.transform.localScale -= new Vector3(.1f, .1f, 0);
				}

				gameObject.transform.position = Vector3.Lerp(startPos, endPos, (1f / (float)movementFrames) * (float)framesElapsed);
				framesElapsed++;

				if (framesElapsed > movementFrames)
				{
					moving = false;
					if (falling)
					{
						Destroy(dataManager);
						SceneManager.LoadScene("Assets/Scenes/TitleScreen.unity");
					}
					else if(leaving)
					{
						if(!treasureGot)
						{
							dataManager.Faith = dataManager.Faith - 5;
						}
						SceneManager.LoadScene("Assets/Scenes/Town.unity");
					}
				}

			}
			else
			{
				if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
				{
					AttemptMovement(Vector3.up);
				}
				else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
				{
					AttemptMovement(Vector3.right);
					gameObject.GetComponent<SpriteRenderer>().sprite = walkRight;
				}
				else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
				{
					AttemptMovement(Vector3.down);
				}
				else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
				{
					AttemptMovement(Vector3.left);
					gameObject.GetComponent<SpriteRenderer>().sprite = walkLeft;
				}
			}

			if (wobbleing)
			{
				if (wobble == 0 && !moving)
				{
					wobbleing = false;
					//gameObject.transform.position = new Vector3(Mathf.FloorToInt(gameObject.transform.position.x), Mathf.FloorToInt(gameObject.transform.position.y), -.2f);
				}

				//gameObject.transform.RotateAround(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - .2f, gameObject.transform.position.z), new Vector3(0f, 0f, 1f), .5f * Mathf.Sin(wobble + 1.57f));
				gameObject.transform.Rotate(0, 0, Mathf.Sin(wobble + 1.57f));

				wobble += .05f;
				if (wobble > 6)
				{
					wobble = 0;
					gameObject.transform.rotation = Quaternion.identity;
				}
			}
		}
		
	}

	private Vector3 AddVectorThree(Vector3 vect1, Vector3 vect2)
	{
		return new Vector3(vect1.x + vect2.x, vect1.y + vect2.y, vect1.z + vect2.z);
	}

	private void AttemptMovement(Vector2 direction)
	{
		//Determine if the player is stepping off a cracked tile
		if (isOnCracked != null)
		{
			DungeonMaster.BreakATile(isOnCracked);
			isOnCracked = null;
		}

		if (MoveCheck(direction))
		{
			startPos = gameObject.transform.position;
			endPos = AddVectorThree(startPos, direction);
			framesElapsed = 1;
			moving = true;
			wobbleing = true;

			//Break a tile
			DungeonMaster.BreakATile();

			if (treasureGot)
			{
				DungeonMaster.BreakATile();
			}
		}
	}

	private bool MoveCheck(Vector2 direction)
	{
		RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector3(gameObject.transform.position.x + direction.x, gameObject.transform.position.y + direction.y), direction, .1f, ~0, -1, 1);

		foreach (RaycastHit2D hit in hits)
		{
			if(hit.transform.gameObject.name.Contains("Wall"))
			{
				return false;
			}
			else if(hit.transform.gameObject.name.Contains("TreasurePile"))
			{
				Destroy(hit.transform.gameObject);
				UIManager.AddGold(1);
			}
			else if(hit.transform.gameObject.name.Contains("TreasureChest"))
			{
				arrow.SetActive(false);
				moveArrow = false;
				Destroy(hit.transform.gameObject);
				UIManager.AddChest(1);
				dataManager.Faith = dataManager.Faith + 10;
			}
			else if(hit.transform.gameObject.name.Contains("Cracked"))
			{
				isOnCracked = hit.transform.gameObject;
			}
			else if(hit.transform.gameObject.name.Contains("pit"))
			{
				if(UIManager.Rope > 0)
				{
					DungeonMaster.RopeAPit(hit.transform.gameObject, (int)Mathf.Abs(direction.x));
					UIManager.RemoveRope(1);
				}
				else
				{
					falling = true;
					movementFrames += 30;
				}
			}
			else if(hit.transform.gameObject.name.Contains("Stairs"))
			{
				leaveDungeonQuery.gameObject.SetActive(true);
				menuHold = true;
				return false;
			}
		}
		return true;
	}

	public void ExitResult(bool leave)
	{
		leaveDungeonQuery.gameObject.SetActive(false);
		if (leave)
		{
			startPos = gameObject.transform.position;
			endPos = AddVectorThree(startPos, Vector3.left);
			framesElapsed = 1;
			moving = true;
			wobbleing = true;
			leaving = true;
		}

		menuHold = false;
	}

	private void RotateArrow()
	{
		//float t = treasurePos.x - gameObject.transform.position.x;
		//float p = treasurePos.y - gameObject.transform.position.y;
		//float n = Mathf.Sqrt((t * t) + (p * p));

		//Quaternion rotation = Quaternion.LookRotation(treasurePos - gameObject.transform.position, transform.TransformDirection(Vector3.up));

		Vector3 diff = treasurePos - gameObject.transform.position;

		diff.Normalize();

		arrow.transform.rotation = Quaternion.Euler(0f, 0f, (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg));//new Quaternion(0, 0, rotation.z, rotation.w);//LookAt(treasurePos);// .Rotate(new Vector3(0, 0, (float)Math.Acos((((n * n) + (t * t) - (p * p)) / (2 * n * t)))));
	}

	public void setTreasurePos(Vector2 pos)
	{
		treasurePos = pos;
	}
}
