using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetection : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}


    [SerializeField]
    LayerMask groundLayers;

    [SerializeField]
    float skinValue = 0.2f;
    [SerializeField]
    float separationRay = 0.2f;

    bool inGround = false;
    public bool InGround { get { return inGround; } }

    Vector3 normal = Vector3.up;
    public Vector3 Normal { get { return normal; } }

    // Update is called once per frame
    void FixedUpdate () {
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position + normal * 0.5f + Vector3.Cross(normal, Vector3.forward) * separationRay, -normal, 0.5f + skinValue, groundLayers);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + normal * 0.5f - Vector3.Cross(normal, Vector3.forward) * separationRay, -normal, 0.5f + skinValue, groundLayers);
        inGround = hit1.collider != null || hit2.collider != null;
        if (inGround)
        {
            if(hit1.collider != null && hit2.collider == null) 
                normal = hit1.normal;
            else if(hit1.collider == null && hit2.collider != null)
                normal = hit2.normal;
            else
            {
                float angle1 = Vector3.Angle(hit1.normal, Vector3.up);
                float angle2 = Vector3.Angle(hit2.normal, Vector3.up);
                if(angle1 > angle2)
                    normal = hit1.normal;
                else
                    normal = hit2.normal;
            }
        }
        else
            normal = Vector3.up;
        Debug.DrawLine(transform.position, transform.position + normal, Color.green); 
        Debug.DrawLine(
            transform.position + normal * 0.5f + Vector3.Cross(normal, Vector3.forward) * separationRay,
            transform.position + normal * 0.5f + Vector3.Cross(normal, Vector3.forward) * separationRay - normal * (0.5f + skinValue),
            Color.blue
            );
        Debug.DrawLine(
            transform.position + normal * 0.5f - Vector3.Cross(normal, Vector3.forward) * separationRay,
            transform.position + normal * 0.5f - Vector3.Cross(normal, Vector3.forward) * separationRay - normal * (0.5f + skinValue),
            Color.blue
            );
    }
}
