using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    [SerializeField]
    Rigidbody2D body;
    [SerializeField]
    GroundDetection detector;
    [SerializeField]
    float gravityUp;
    [SerializeField]
    float gravityDown;
    [SerializeField]
    float maxAngle;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        CheckJump();
        CheckGravity();
        HorizontalMovement();
    }

    [SerializeField]
    float maxVel;
    [SerializeField]
    float accel;
    [SerializeField]
    float horizontalDrag;
    [SerializeField]
    float accel_air;
    [SerializeField]
    float horizontalDrag_air;

    [SerializeField]
    float jumpVel;
    [SerializeField]
    float jumpNormalStrength;

    void CheckJump()
    {
        if(detector.InGround && Input.GetKey(KeyCode.Space))
        {
            Vector3 nr = (normal.normalized * jumpNormalStrength + Vector3.up).normalized;
            Vector3 velocityOnNormal = Vector3.Project(body.velocity, nr);
            body.velocity -= (Vector2)velocityOnNormal;
            body.velocity += (Vector2)nr * jumpVel;
        }
    }

    void HorizontalMovement()
    {
        Vector3 newRight = Vector3.Cross(detector.Normal, Vector3.forward);
        Vector3 velocityOnNormal = Vector3.Project(body.velocity,detector.Normal);
        Vector3 velocityOnright = (Vector3)body.velocity - velocityOnNormal;
        if (detector.InGround)
        {

            velocityOnright *= horizontalDrag;
            velocityOnright += accel * Time.deltaTime * newRight * Input.GetAxis("Horizontal");
        }
        else
        {

            velocityOnright *= horizontalDrag_air;
            velocityOnright += accel_air * Time.deltaTime * newRight * Input.GetAxis("Horizontal");
        }
        velocityOnright = Vector3.ClampMagnitude(velocityOnright, maxVel);
        body.velocity = velocityOnright + velocityOnNormal;
    }

    Vector3 normal;

    void CheckGravity()
    {
        float gravityToUse = gravityDown;
        if (detector.InGround || body.velocity.y > 0)
        {
            gravityToUse = gravityUp;
        }
        normal = detector.Normal;
        if (Vector3.Angle(normal, Vector3.up) > maxAngle)
            normal = Vector3.up;
        body.velocity += (Vector2)(-normal * Time.deltaTime * gravityToUse);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, normal);
    }
}
