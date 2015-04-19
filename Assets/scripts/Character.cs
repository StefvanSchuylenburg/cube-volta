using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/**
 * A block that can be killed.
 */
public class Character : MonoBehaviour {

    public bool isAlive
    {
        get;
        private set;
    }

    /**
     * Trigger for when the Character becomes dead
     */
    public UnityEvent onDeath = new UnityEvent();

    void Awake()
    {
        isAlive = true;
    }

    /**
     * Kills the Character.
     * It is now no longer alive.
     * This does not influence his abilities to move.
     */
    public void Kill()
    {
        isAlive = false;
        onDeath.Invoke();
    }
}
