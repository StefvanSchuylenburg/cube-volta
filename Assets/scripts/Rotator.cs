using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;

/**
 * Rotates the current object around on certain key presses.
 */
public class Rotator : MonoBehaviour {

    /**
     * The speed to rotate with each second
     */
    public float rotationSpeed = 1f;

    /**
     * How long rotating is disabled when the characters are falling.
     */
    public float fallTime = 1.5f;

    /**
     * Triggered when the Rotator either starts disallowing interactions or start allowing interactions.
     * When allowing the given bool will be true.
     */
    public BoolEvent onInteractChange = new BoolEvent();

    /**
     * How far two quaternions are allowed to be from each other before considering them equal.
     */
    private static float ANGLE_EPSILON = 2;

    /** The states this object can be in */
    private enum State { Idle, Rotating, Falling };

    /** the current state we are in. */
    private State state;

    /**
     * When state is rotating we will rotate to the given value.
     * When we are at this value, we transform into the falling state.
     */
    private Quaternion rotateTowards;

    /**
     * Whether a physics update has been done in the last frame.
     */
    private bool hasUpdateElapsed;

    /**
     * Rotates the box to the left
     */
    public void RotateLeft()
    {
        if (state == State.Idle)
        {
            ToRotate(false);
        }
    }

    /**
     * Rotates the box to the right
     */
    public void RotateRight()
    {
        if (state == State.Idle)
        {
            ToRotate(true);
        }
    }

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
                Falling();
                break;
            default:
                break;
        }
	}

    void FixedUpdate()
    {
        hasUpdateElapsed = true; // an update has elapsed
    }

    /**
     * We are waiting for user input.
     */
    private void Idling()
    {
        // waiting for key presses
        if (Input.GetButtonDown("RotateRight"))
        {
            ToRotate(false);
        }
        else if (Input.GetButtonDown("RotateLeft"))
        {
            ToRotate(true);
        }
    }

    /**
     * Sets the state to rotating and sets the rotateTowards value.
     * @param rotateLeft when true we rotate to the left, otherwise to the right.
     */
    private void ToRotate(bool rotateLeft)
    {
        // turn off gravity
        SetGravity(false);
        // disable interaction
        onInteractChange.Invoke(false);

        // the current rotation
        var rotation = this.transform.rotation;
        // where to rotate with
        float angle = rotateLeft ? 90 : -90;
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
        if (QuaternionClose(transform.rotation, rotateTowards))
        {
            // first make sure we are at towards (not just close to it)
            transform.rotation = rotateTowards;
            ToFalling();
        }
    }

    /**
     * Sets state to falling again.
     */
    private void ToFalling()
    {
        state = State.Falling;

        // and fall again
        SetGravity(true);

        // check whether an update will be done
        hasUpdateElapsed = false;
    }

    private void Falling()
    {
        // TODO: this method is still slow, especially the GetComponentsInChildren
        // check whether all objects are no longer moving
        var childBodies = this.gameObject.GetComponentsInChildren<Rigidbody>();

        // whether there is a child that has velocity left
        var anyMoves = false;
        foreach (var body in childBodies)
        {
            if (body.velocity != Vector3.zero)  // velocity is close to zero
            {
                anyMoves |= true;
                break;
            }
        }

        // go back to idle when required
        if (!anyMoves && hasUpdateElapsed)
        {
            ToIdle();
        }
    }

    /**
     * Sets state back to Idle and allows interaction again
     */
    private void ToIdle()
    {

        this.state = State.Idle;
        onInteractChange.Invoke(true);
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

    /**
     * Determines whether two quaternions are close
     */
    private bool QuaternionClose(Quaternion q, Quaternion p)
    {
        return Quaternion.Angle(q, p) <= ANGLE_EPSILON;
    }

    /**
     * Small class to use Bool events for this Rotator
     */
    [Serializable]
    public class BoolEvent : UnityEvent<bool> { }

}
