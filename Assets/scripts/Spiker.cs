using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Spiker : MonoBehaviour {

    public Material deathMaterial;
    public UnityEvent onKill = new UnityEvent();

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
        Debug.Log(other.gameObject.name);
     
        if (other.gameObject.name == "player") {
            onKill.Invoke();
        }        
    }
}
