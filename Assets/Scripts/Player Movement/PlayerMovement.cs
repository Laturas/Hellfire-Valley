using UnityEngine;
using static Mode;

enum Mode
{
    Walking,
    Flying,
}

public class PlayerMovement : MonoBehaviour
{
    //Ground
    [Header("Ground")]
    private float current_player_speed;
	private SOPlayerControls settings => SOManager.instance.playerControls;

    //Jump
    private const float dashSpeed = 6f;

    Vector3 bannedGroundNormal;

    //States
    bool crouched => Input.GetKey(KeyCode.LeftControl);
    Mode mode = Flying;

    [Header("Initialization")]
    [SerializeField] private CameraMovement camCon;

    private CapsuleCollider col;
    private Rigidbody rb;
    Vector3 dir = Vector3.zero;
    Vector3 groundNormal = Vector3.up;

    void Awake()
    {
        col = transform.gameObject.GetComponent<CapsuleCollider>();
        rb = transform.gameObject.GetComponent<Rigidbody>();
        col.material.dynamicFriction = 0f;
    }

    void Update()
    {
        //if (rb.linearDamping > 0.2f) {rb.linearDamping = 0f;}
        rb.useGravity = mode != Walking;

        dir = Direction();
        //if (dir.magnitude < 0.1f && getPlayerSpeed() < 0.5f) {rb.linearDamping = 11f;}

        if (activeCoyoteTime >= 0) {
            if (Input.GetKey(KeyCode.Space)) {Jump(); return;}
            activeCoyoteTime -= 0.1f;
        }
    }

    void FixedUpdate()
    {
        switch (mode)
        {
            case Walking:
                Walk(dir, current_player_speed, settings.playerAcceleration);
                break;
            case Flying:
                AirMove(dir, settings.playerAirSpeed, settings.playerAirAcceleration);
                break;
        }

        if (crouched)
        {
            if (0.6f < col.height - Time.deltaTime)
            {
                col.height = Mathf.Max(0.6f, col.height - Time.deltaTime * 10f);
                if (mode != Flying) col.transform.position += new Vector3(0, -Time.deltaTime * 5f, 0);
            }
            current_player_speed = 0.3f * settings.playerSpeed;
        }
        else
        {
            col.height = Mathf.Min(2f, col.height + Time.deltaTime * 5f);
            current_player_speed = settings.playerSpeed;
        }
    }

    private Vector3 Direction()
    {
        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");

        //Vector3 direction = new Vector3(hAxis, 0, vAxis);

        //return rb.transform.TransformDirection(direction);
        var fwd = camCon.gameObject.transform.forward;
        fwd.y = 0;
        fwd = fwd.normalized;
        var right = camCon.gameObject.transform.right;
        return fwd * vAxis + right * hAxis;
    }
	
    void OnCollisionEnter(Collision collision) {
        int count = collision.contactCount;
        if (count == 0) {return;}

        for (int i = 0; i < count; i++) // Uglier than the foreach but generates 0 garbage
        {
            ContactPoint contact = collision.GetContact(i);
            Vector3 vecToPoint = contact.point-transform.position;
            float posAngle = Vector3.Angle(vecToPoint, Vector3.up);
            if (posAngle > 150f && posAngle < 175)
			{
				transform.position += new Vector3(vecToPoint.x,0,0);
				return;
			}
        }
    }
    void OnCollisionStay(Collision collision)
    {
        int count = collision.contactCount;
        if (count == 0) {return;}

        float angle;
        for (int i = 0; i < count; i++) // Uglier than the foreach but generates 0 garbage
        {
            ContactPoint contact = collision.GetContact(i);
            angle = Vector3.Angle(contact.normal, Vector3.up);
            if (angle >= settings.wallFloorBarrierAngle) {continue;}

            EnterWalking();
            
            groundNormal = contact.normal;
            return;
        }
    }

    void OnCollisionExit() => EnterFlying();

    void EnterWalking()
    {
        if (mode == Walking) {return;}

        if (mode == Flying && crouched)
        {
            rb.AddForce(-rb.linearVelocity.normalized, ForceMode.VelocityChange);
        }
        mode = Walking;
    }

    void EnterFlying(bool wishFly = false)
    {
        if (mode == Flying)
        {
            return;
        }
        mode = Flying;
    }
    private float projectTimer;

    void Walk(Vector3 intended_direction, float maxSpeed, float acceleration)
    {
        activeCoyoteTime = settings.coyoteTimer;
        //if (jump) {Jump(); return;}

		intended_direction = intended_direction.normalized;

		Vector3 direction = new(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
		if (direction.magnitude > maxSpeed) acceleration *= direction.magnitude / maxSpeed;
		direction = intended_direction * maxSpeed - direction;

		if (direction.magnitude < 0.5f) acceleration *= direction.magnitude / 0.5f;

		direction = direction.normalized * acceleration;

		direction = Vector3.ProjectOnPlane(direction, groundNormal);
		rb.AddForce(direction, ForceMode.Acceleration);
        if (projectTimer <= 0) {
            rb.linearVelocity = Vector3.ProjectOnPlane(rb.linearVelocity, groundNormal);
        } else {projectTimer -= 0.1f;}
    }

    [SerializeField] private float activeCoyoteTime;

    void AirMove(Vector3 intended_direction, float maxSpeed, float acceleration)
    {
        intended_direction = intended_direction.normalized;

		Vector3 direction = new(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
		if (direction.magnitude + acceleration > maxSpeed)
            acceleration = Mathf.Max(0f, maxSpeed - direction.magnitude);
		direction = intended_direction * maxSpeed - direction;

		//if (direction.magnitude < 0.5f) acceleration *= direction.magnitude / 0.5f;

		direction = direction.normalized * acceleration;

		rb.AddForce(direction, ForceMode.Acceleration);

        // float projVel = Vector3.Dot(new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z), intended_direction); // Vector projection of Current velocity onto accelDir.
        // float accelVel = acceleration / 100f; // Accelerated velocity in direction of movment

        // // If necessary, truncate the accelerated velocity so the vector projection does not exceed max_velocity
        // if (projVel + accelVel > maxSpeed)
        //     accelVel = Mathf.Max(0f, maxSpeed - projVel);

        //rb.AddForce(intended_direction.normalized * accelVel, ForceMode.Acceleration);
    }

    void Jump()
    {   
        projectTimer = settings.stopProjectionTimer;
        activeCoyoteTime = -1f;
        float upForce = Mathf.Clamp(settings.jumpPower - rb.linearVelocity.y, 0, Mathf.Infinity);
        switch (mode)
        {
            case Walking:
                rb.AddForce(new Vector3(0, upForce, 0), ForceMode.VelocityChange);
                EnterFlying(true);
                break;
            default:
                break;
        }
    }
    Vector3 RotateToPlane(Vector3 vect, Vector3 normal)
    {
        Vector3 rotDir = Vector3.ProjectOnPlane(normal, Vector3.up);
        Quaternion rotation = Quaternion.AngleAxis(-90f, Vector3.up);
        rotDir = rotation * rotDir;
        float angle = -Vector3.Angle(Vector3.up, normal);
        rotation = Quaternion.AngleAxis(angle, rotDir);
        return rotation * vect;
    }

    
    Vector3 VectorToGround()
    {
        Vector3 position = transform.position;
        RaycastHit hit;
        if (Physics.Raycast(position, Vector3.down, out hit, 1f))
        {
            return hit.point - position;
        }
        return Vector3.positiveInfinity;
    }

    public string getMode() => mode.ToString();

    public float getPlayerSpeed() => rb.linearVelocity.magnitude < 0f ? -rb.linearVelocity.magnitude : rb.linearVelocity.magnitude;
}
