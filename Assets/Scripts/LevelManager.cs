using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	static int ROOM_SIZE = 5;

	int numberOfRooms;
	List<Vector2> rooms = new List<Vector2>();
	Dictionary<Vector2, string> roomsToMake = new Dictionary<Vector2, string>();
	List<Vector2Int> around = new List<Vector2Int>();
	List<string> aroundCodes = new List<string>();
	List<GameObject> floorTiles;
	DataManager dataManager;
	[SerializeField] GameObject crackedTile;
	[SerializeField] GameObject pit;
	[SerializeField] GameObject ropedPit0;
	[SerializeField] GameObject ropedPit1;

	private void Start()
	{
		dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();

		around.Add(new Vector2Int(0, ROOM_SIZE));
		around.Add(new Vector2Int(ROOM_SIZE, 0));
		around.Add(new Vector2Int(0, -ROOM_SIZE));
		around.Add(new Vector2Int(-ROOM_SIZE, 0));

		aroundCodes.Add("**1*");
		aroundCodes.Add("***1");
		aroundCodes.Add("1***");
		aroundCodes.Add("*1**");

		//Rooms with new doors may only be generated when number of rooms is > 0
		numberOfRooms = 10 * (dataManager.RelicCounter + 1);

		//Generate the dungeon
		GenerateLevel();

		floorTiles = new List<GameObject>(GameObject.FindGameObjectsWithTag("Floor"));

		GenerateGold(10 * (dataManager.RelicCounter + 1));
	}

	void GenerateLevel()
	{
		//Start by placing the start room and connected roomsToMake
		Instantiate(Resources.Load<GameObject>("RoomSamples/EnteranceRoom"), new Vector2(0, 0), Quaternion.identity);
		roomsToMake.Add(around[0], "**1*");
		roomsToMake.Add(around[1], "***1");
		roomsToMake.Add(around[2], "1***");
		roomsToMake.Add(around[3], "*0**");

		//Add to the list of completed rooms
		rooms.Add(new Vector2(0, 0));

		//Generate all the rooms
		GenerateRooms();

	}

	void GenerateRooms()
	{
		int looper;
		int currentRoom;
		int codeIndex;
		int roomsLeftToPlace = 3; //counts the number of rooms with door connections (non 0000 rooms) left to be place
		List<Vector2> KeyList;
		bool treasureRoom;

		treasureRoom = false;

		//While there are rooms left to make
		do
		{
			//Get the current List of Keys
			KeyList = new List<Vector2>(roomsToMake.Keys);


			for (currentRoom = 0; currentRoom < KeyList.Count; currentRoom++)
			{
				//Determine if the room is a room currently without a door and needs to be skipped
				if (roomsToMake[KeyList[currentRoom]].Contains("1"))
				{	
					//Determine the status of numberOfRooms and roomsLeftToPlace
					if ((numberOfRooms == 0 || roomsLeftToPlace == 1) && !treasureRoom ) //Number of rooms has been reached so the treasure room gets placed
					{
						//Set this room to be the Treasure Room
						Instantiate(Resources.Load<GameObject>("RoomSamples/TreasureRoom"), KeyList[currentRoom], Quaternion.identity);

						GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().setTreasurePos(KeyList[currentRoom]);

						for(looper = 0; looper < around.Count; looper++)
						{
							roomsLeftToPlace = CheckAndCreateRoomToMake(KeyList[currentRoom], looper, '1', roomsLeftToPlace);
						}

						roomsLeftToPlace--;

						treasureRoom = true;
					}
					else if (numberOfRooms < 0)//Number of rooms has been exceeded no more open doors may be placed
					{
						//Set the current room's room code *s to 0s
						for (codeIndex = 0; codeIndex < 4; codeIndex++)
						{
							if (roomsToMake[KeyList[currentRoom]][codeIndex] == '*')
							{
								roomsToMake[KeyList[currentRoom]] = roomsToMake[KeyList[currentRoom]].Remove(codeIndex, 1).Insert(codeIndex, "0");
							}
						}
						
						//Generate the room
					 	roomsLeftToPlace = GenerateARoom(KeyList[currentRoom], roomsToMake[KeyList[currentRoom]], roomsLeftToPlace);

					}
					else//There are still rooms to be placed
					{
						//Generate the room
						roomsLeftToPlace = GenerateARoom(KeyList[currentRoom], roomsToMake[KeyList[currentRoom]], roomsLeftToPlace);
					}

					//Add to the list of completed rooms
					rooms.Add(KeyList[currentRoom]);

					//Remove this position from the rooms to make list
					roomsToMake.Remove(KeyList[currentRoom]);

					//Decrement the number of rooms counter
					numberOfRooms--;
				}
			}

			KeyList.Clear();
		} while (roomsLeftToPlace > 0);

		//All Rooms with doors have been places
		//All remaining roomsToMake are 0000 rooms and must be placed without adding to roomsToMake

		KeyList = new List<Vector2>(roomsToMake.Keys);

		for(currentRoom = 0; currentRoom < KeyList.Count; currentRoom++)
		{
			Instantiate(Resources.Load<GameObject>("RoomSamples/Room00000"), KeyList[currentRoom], Quaternion.identity);
		}

		roomsToMake.Clear();
	}

	private int GenerateARoom(Vector2 pos, string roomCode, int roomsLeftToPlace)
	{
		int looper;
		string newRoomCode;

		newRoomCode = "";
		
		//Determine if the room has at least one connecting doors
		if(roomCode.Contains("1"))
		{
			//Translate the room code from a string to a bool list
			for (looper = 0; looper < roomCode.Length; looper++)
			{
				if (roomCode[looper] == '*')
				{
					newRoomCode += Random.Range(0, 2);
				}
				else
				{
					newRoomCode += roomCode[looper];
				}

				roomsLeftToPlace = CheckAndCreateRoomToMake(pos, looper, newRoomCode[looper], roomsLeftToPlace);
			}

			//Get a random room with the roomCode
			Instantiate(Resources.Load<GameObject>("RoomSamples/Room" + newRoomCode + "0"), pos, Quaternion.identity);

			roomsLeftToPlace--;
		}
		else
		{
			//If there are no currently connecting rooms place a void room (Code 0000)
			Instantiate(Resources.Load<GameObject>("RoomSamples/Room00000"), pos, Quaternion.identity);
		}

		return roomsLeftToPlace;
	}

	private int CheckAndCreateRoomToMake(Vector2 pos, int codeIndex, char doorCode, int roomsLeftToPlace)
	{
		Vector2 posCheck = new Vector2();

		posCheck.x = pos.x + around[codeIndex].x;
		posCheck.y = pos.y + around[codeIndex].y;

		//Determine if this position is already in rooms to make
		if (roomsToMake.ContainsKey(posCheck))
		{
			if(doorCode == '1' && !roomsToMake[posCheck].Contains("1"))
			{
				roomsLeftToPlace++;
			}
			//Edit that roomToPlace room code to include a connecting room
			roomsToMake[posCheck] = roomsToMake[posCheck].Remove((codeIndex + 2) % 4, 1).Insert((codeIndex + 2) % 4, doorCode.ToString());
		}
		//Determine if there is not already a full room there
		else if(!rooms.Contains(posCheck))
		{
			if(doorCode == '1')
			{
				roomsLeftToPlace++;
			}
			//Add a roomToPlace with the code to include the connected room
			roomsToMake.Add(posCheck, "****");
			roomsToMake[posCheck] = roomsToMake[posCheck].Remove((codeIndex + 2) % 4, 1).Insert((codeIndex + 2) % 4, doorCode.ToString());
		}

		return roomsLeftToPlace;
	}

	private void GenerateGold(int goldAmmount)
	{
		int looper;
		int posiblePlacement;
		List<int> filledTiles = new List<int>();
		GameObject gold = Resources.Load<GameObject>("Prefabs/TreasurePile");

		for (looper = 0; looper < goldAmmount; looper++)
		{
			do
			{
				posiblePlacement = Random.Range(0, floorTiles.Count);
			} while (filledTiles.Contains(posiblePlacement));

			filledTiles.Add(posiblePlacement);

			Instantiate(gold, new Vector3(floorTiles[posiblePlacement].transform.position.x, floorTiles[posiblePlacement].transform.position.y, -0.1f), Quaternion.identity);
		}
	}

	public void BreakATile()
	{
		int randomTile;

		//Determine if the tile will break
		if(Random.Range(0f, 20f) >  Mathf.Sqrt(dataManager.Faith))
		{
			randomTile = Random.Range(0, floorTiles.Count);

			if (!floorTiles[randomTile].name.Contains("Cracked"))
			{
				floorTiles.Add(Instantiate(crackedTile, floorTiles[randomTile].transform.position, Quaternion.identity));
				Destroy(floorTiles[randomTile]);
				floorTiles.RemoveAt(randomTile);
			}
			else
			{
				Instantiate(pit, floorTiles[randomTile].transform.position, Quaternion.identity);
				Destroy(floorTiles[randomTile]);
				floorTiles.RemoveAt(randomTile);
			}
		}
	}

	public void BreakATile(GameObject tile)
	{
		if (!tile.name.Contains("Cracked"))
		{
			floorTiles.Add(Instantiate(crackedTile, tile.transform.position, Quaternion.identity));
			Destroy(tile);
			floorTiles.Remove(tile);
		}
		else
		{
			Instantiate(pit, tile.transform.position, Quaternion.identity);
			Destroy(tile);
			floorTiles.Remove(tile);
		}
	}

	public void RopeAPit(GameObject tile, int direction)
	{
		Instantiate(direction == 0 ? ropedPit0 : ropedPit1, tile.transform.position, Quaternion.identity);
		Destroy(tile);
		floorTiles.Remove(tile);
	}
}
