using UnityEngine;
using System.Collections;

public class FixedZ : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        var pos = this.transform.position;
        this.transform.position = Vector3.Scale(pos, new Vector3(1f, 1f, 0f));
	}
}
