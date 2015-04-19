using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Spiker : MonoBehaviour {
    
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
    }
}
