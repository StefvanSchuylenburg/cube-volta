using UnityEngine;
using System.Collections;

public class main : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.name == "player" && gameObject.tag == "dead") {
            GUI.Label(new Rect(10, 10, 150, 100), "Ded");
        }
	}
}
