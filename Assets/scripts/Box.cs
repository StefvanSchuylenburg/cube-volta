using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("RotateRight"))
        {
            this.transform.Rotate(Vector3.forward * 90, Space.Self);
            Debug.Log("Rotating?");
        }
        else if (Input.GetButtonDown("RotateLeft"))
        {
            this.transform.Rotate(Vector3.forward * -90, Space.Self);
        }
	}
}
