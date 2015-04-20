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
     * Allows rotation in the third dimension.
     */
    public bool allow3dRotation = false;

    /**
     * Triggered when the Rotator either starts disallowing interactions or start allowing interactions.
     * When allowing the given bool will be true.
     */
    public BoolEvent onInteractChange = new BoolEvent();

    // Events for when states has changed.
    public UnityEvent onStartRotating = new UnityEvent();
    public UnityEvent onStartFalling = new UnityEvent();
    public UnityEvent onStartIdle = new UnityEvent();

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
            ToRotate(Vector3.forward);
        }
    }

    /**
     * Rotates the box to the right
     */
    public void RotateRight()
    {
        if (state == State.Idle)
        {
            ToRotate(-Vector3.forward);
        }
    }

    /**
     * Rotates the box away from you.
     */
    public void RotateForward()
    {
        if (state == State.Idle && allow3dRotation)
        {
            ToRotate(Vector3.right);
        }
    }


    /**
     * Rotates the box to you.
     */
    public void RotateBackward()
    {
        if (state == State.Idle && allow3dRotation)
        {
            ToRotate(Vector3.left);
        }
    }


	void Start () {
        state = State.Idle;
        onStartIdle.Invoke();
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
            RotateRight();
        }
        else if (Input.GetButtonDown("RotateLeft"))
        {
            RotateLeft();
        }
        else if (allow3dRotation)
        {
            if (Input.GetButtonDown("RotateForward"))
            {
                RotateForward();
            }
            else if (Input.GetButtonDown("RotateBackward"))
            {
                RotateBackward();
            }
        }
    }

    /**
     * Sets the state to rotating and sets the rotateTowards value.
     * @param around The vector to rotate around
     */
    private void ToRotate(Vector3 around)
    {
        // turn off gravity
        SetGravity(false);
        // disable interaction
        onInteractChange.Invoke(false);

        // the current rotation
        var rotation = this.transform.rotation;
        // where to rotate with
        var relAround = transform.InverseTransformDirection(around);
        var rotateWith = Quaternion.AngleAxis(90, relAround);

        // set the target
        rotateTowards = rotation * rotateWith;

        // and start rotating
        state = State.Rotating;
        onStartRotating.Invoke();
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
        onStartFalling.Invoke();

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
        var childBodies = this.gameObject.GetComponentsInChildren<Rigidbody>();
        foreach (var body in childBodies)
        {
            SnapToGrid(body.gameObject);
        }

        this.state = State.Idle;
        onStartIdle.Invoke();
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

    /// <summary>
    /// Snaps this object to the grid, assuming it's done with falling.
    /// </summary>
    /// <param name="gameObject"></param> the gameobject to snap
    private void SnapToGrid(GameObject gameObject)
    {
        // position relative to the rotator
        var relPos = transform.InverseTransformPoint(gameObject.transform.position);
        // rounding down
        var roundRelPos = new Vector3(Mathf.Round(relPos.x), Mathf.Round(relPos.y), Mathf.Round(relPos.z));
        // going back to world coordinates
        var worldPos = transform.TransformPoint(roundRelPos);

        // and setting the new value
        gameObject.transform.position = worldPos;
    }

    /**
     * Small class to use Bool events for this Rotator
     */
    [Serializable]
    public class BoolEvent : UnityEvent<bool> { }

}
