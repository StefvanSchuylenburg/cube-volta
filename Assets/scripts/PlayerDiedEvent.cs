using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PlayerDiedEvent : UnityEvent
{

	// Use this for initialization
	void Start () {
        Debug.Log("init");
        this.AddListener(callback);
	}

    private void callback()
    {
        Debug.Log("got here");
        var obj = GameObject.Find("dead");
        //var renderer = obj.GetComponent<MeshRenderer>();        
        //renderer.enabled = true;
        obj.transform.position = new Vector3(-5, -1, 5);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    
}
