using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Spiker : MonoBehaviour {

    /// <summary>
    /// Texture of dead bock
    /// </summary>
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
        // change texture
        ren.material = deathMaterial;
        
        // fix position of collided object to spike.
        other.gameObject.transform.position = this.gameObject.transform.position;
        // the spike is not needed anymore
        this.gameObject.SetActive(false);
        
        
        if (other.gameObject.name == "player") {
            onKill.Invoke();
        }        
    }
}
