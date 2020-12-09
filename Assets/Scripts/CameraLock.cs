using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLock : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
		gameObject.transform.rotation = Quaternion.identity;
    }
}
