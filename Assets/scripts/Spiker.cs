using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Spiker : MonoBehaviour {

    /**
     * Trigger for when a character is attached to the spike.
     */
    public UnityEvent onKill = new UnityEvent();
    
    void OnTriggerEnter(Collider other)
    {
        // make the object stick on the spikes
        var body = other.gameObject.GetComponent<Rigidbody>();
        body.isKinematic = true;
        
        // kill the object if its an character
        var character = other.gameObject.GetComponent<Character>();
        if (character != null) // the character is defined
        {
            character.Kill();
        }

        // fix position of collided object to spike.
        //other.gameObject.transform.position = this.gameObject.transform.position;
        // the spike is not needed anymore
        this.gameObject.SetActive(false);

        // invoke event
        onKill.Invoke();
    }
}
