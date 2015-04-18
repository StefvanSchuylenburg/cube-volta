using UnityEngine;
using System.Collections;

public class Spiker : MonoBehaviour {

    public Material deathMaterial;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    void OnTriggerEnter(Collider other)
    {

        var body = other.gameObject.GetComponent<Rigidbody>() as Rigidbody;
        body.isKinematic = true;
        var ren = other.gameObject.GetComponent<Renderer>() as Renderer;
        ren.material = deathMaterial;
    }
}
