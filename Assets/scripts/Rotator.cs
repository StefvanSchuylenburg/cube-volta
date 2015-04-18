using UnityEngine;
using System.Collections;

/**
 * Rotates the current object around on certain key presses.
 */
public class Rotator : MonoBehaviour {

    /**
     * The speed to rotate with each second
     */
    public float rotationSpeed = 1f;

    /** The states this object can be in */
    private enum State { Idle, Rotating, Falling };

    /** the current state we are in. */
    private State state;

    /**
     * When state is rotating we will rotate to the given value.
     * When we are at this value, we transform into the falling state.
     */
    private Quaternion rotateTowards;

	void Start () {
        state = State.Idle;
	}
	
	// Update is called once per frame
	void Update () {
        // call the function belonging to the state
        switch (state)
        {
            case State.Idle:
                Idling();
                break;
            case State.Rotating:
                Rotating();
                break;
            case State.Falling:
                break;
            default:
                break;
        }
	}

    /**
     * We are waiting for user input.
     */
    private void Idling()
    {
        // waiting for key presses
        if (Input.GetButtonDown("RotateRight"))
        {
            ToRotate(true);
        }
        else if (Input.GetButtonDown("RotateLeft"))
        {
            ToRotate(false);
        }
    }

    /**
     * Sets the state to rotating and sets the rotateTowards value.
     * @param rotateRight when true we rotate to the right, otherwise to the left.
     */
    private void ToRotate(bool rotateRight)
    {
        // turn off gravity
        SetGravity(false);

        // the current rotation
        var rotation = this.transform.rotation;
        // where to rotate with
        float angle = rotateRight ? 90 : -90;
        var rotateWith = Quaternion.AngleAxis(angle, Vector3.forward);

        // set the target
        rotateTowards = rotation * rotateWith;

        // and start rotating!
        state = State.Rotating;
    }

    /**
     * Rotates this GameObject towards rotateTowards.
     */
    private void Rotating()
    {
        // rotating
        transform.rotation = Quaternion.Slerp(transform.rotation, rotateTowards, rotationSpeed * Time.deltaTime);
       
        // checking if we can move to the next state
        if (transform.rotation == rotateTowards) // relying on the == implementation of Quaternions (should be approxiomate).
        {
            ToFalling();
        }
    }

    /**
     * Sets state to falling again.
     */
    private void ToFalling()
    {
        // TODO: just for testing now
        state = State.Idle;

        // and fall again
        SetGravity(true);
    }


    /**
     * Turns gravity on or off (depending on useGravity).
     * This only affects the children of this object.
     */
    private void SetGravity(bool useGravity)
    {
        var childBodies = this.gameObject.GetComponentsInChildren<Rigidbody>();
        foreach (var body in childBodies)
        {
            body.useGravity = useGravity;
        }
    }

}
