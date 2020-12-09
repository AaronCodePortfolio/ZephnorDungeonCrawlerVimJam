using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicNameGenerator : MonoBehaviour
{
	List<string> bases = new List<string>();
	List<string> names = new List<string>();
	List<string> objects = new List<string>();
	List<string> adjetives = new List<string>();
	List<string> preAdjetives = new List<string>();
	// Start is called before the first frame update
	void Start()
    {
		bases.Add("<name>'s <object> of <adjetive>");
		bases.Add("The <preAdjetive> <object> of <adjetive>");
		bases.Add("The <object> of <preAdjetive> <adjetive>");

		names.Add("Anna");
		names.Add("Malik");
		names.Add("Lady Jingle");
		names.Add("Dallas");
		names.Add("Rurik");

		objects.Add("Dagger");
		objects.Add("Gauntlet");
		objects.Add("Stone");
		objects.Add("Skillet");
		objects.Add("Carving");
		objects.Add("Bow");
		objects.Add("Helmet");
		objects.Add("Boots");

		//objects.Add("<preAdjetive> <object>");

		adjetives.Add("Flames");
		adjetives.Add("Songs");
		adjetives.Add("Sparks");
		adjetives.Add("Frost");
		adjetives.Add("Shadows");
		adjetives.Add("Stars");
		adjetives.Add("Speed");
		adjetives.Add("Dragons");

		//adjetives.Add("<adjetive> and <adjetive>");

		preAdjetives.Add("Blazing");
		preAdjetives.Add("Singing");
		preAdjetives.Add("Electrifying");
		preAdjetives.Add("Frozen");
		preAdjetives.Add("Shadowed");
		preAdjetives.Add("Starlight");
		preAdjetives.Add("Speeding");
		preAdjetives.Add("Draconic");
		preAdjetives.Add("Forbidden");

		//preAdjetives.Add("<preAdjetive> and <preAdjetive>");

		GenerateRelicName();
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	string GenerateRelicName()
	{
		string relicName;

		//Initialize relic name to a random base
		relicName = bases[Random.Range(0, bases.Count)];

		//While there is any unfilled brackets in the relic name
		do
		{
			relicName = relicName.Replace("<name>", names[Random.Range(0, names.Count)]);
			relicName = relicName.Replace("<object>", objects[Random.Range(0, objects.Count)]);
			relicName = relicName.Replace("<adjetive>", adjetives[Random.Range(0, adjetives.Count)]);
			relicName = relicName.Replace("<preAdjetive>", preAdjetives[Random.Range(0, preAdjetives.Count)]);
		} while (relicName.Contains("<"));

		Debug.Log(relicName);

		return relicName;
	}
}
