using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BodyType
{
    STATIC,
    DYNAMIC
}


[System.Serializable]
public class RigidBody3D : MonoBehaviour
{
    [Header("Gravity Simulation")]
    public float gravityScale;
    public float mass;
    public BodyType bodyType;
    public float timer;
    public bool isFalling;
    public bool firstTickFix;

    [Header("Attributes")]
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 acceleration;
    public float gravity;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0.0f;
        gravity = -0.001f;
        velocity = Vector3.zero;
        acceleration = new Vector3(0.0f, gravity * gravityScale, 0.0f);
        if (bodyType == BodyType.DYNAMIC)
        {
            isFalling = true;
        }
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (name != "Player")
        {
            transform.position = position;
        }
       
        if (bodyType == BodyType.DYNAMIC)
        {
            if (isFalling)
            {
                acceleration.y = gravity * gravityScale;
            }
            else
            {
                acceleration.y = 0.0f;
                velocity.y = 0.0f;
                // Set acceleration due to friction if body is not falling
                if (velocity.x > 0.0f)
                {
                    acceleration.x = this.gameObject.GetComponent<CubeBehaviour>().frictionUnder * gravity * gravityScale;
                }
                else if (velocity.x < 0.0f)
                {
                    acceleration.x = -1.0f * this.gameObject.GetComponent<CubeBehaviour>().frictionUnder  * gravity * gravityScale;
                }
                if (velocity.z > 0.0f)
                {
                    acceleration.z = this.gameObject.GetComponent<CubeBehaviour>().frictionUnder * gravity * gravityScale;
                }
                else if (velocity.z < 0.0f)
                {
                    acceleration.z = -1.0f * this.gameObject.GetComponent<CubeBehaviour>().frictionUnder * gravity * gravityScale;
                }
            }
            if (firstTickFix)
            {
                timer += Time.deltaTime;
                
                if (gravityScale < 0)
                {
                    gravityScale = 0;
                }

                //if (isPositive(velocity.x) && isPositive(acceleration.x) && acceleration.x != 0.0f)
                //{
                //    velocity.x = 0.0f;
                //    acceleration.x = 0.0f;
                //}
                //if (isPositive(velocity.z) && isPositive(acceleration.z) && acceleration.z != 0.0f)
                //{
                //    velocity.z = 0.0f;
                //    acceleration.z = 0.0f;
                //}
                if (Mathf.Abs(velocity.x) <= Mathf.Abs(acceleration.x))
                {
                    velocity.x = 0.0f;
                    acceleration.x = 0.0f;
                }
                if (Mathf.Abs(velocity.z) <= Mathf.Abs(acceleration.z))
                {
                    velocity.z = 0.0f;
                    acceleration.z = 0.0f;
                }

                if (gravityScale > 0)
                {
                    velocity.x += acceleration.x;
                    velocity.y += acceleration.y * 0.5f * timer * timer;
                    velocity.z += acceleration.z;
                    transform.position += velocity;
                }
            }
            else if (!firstTickFix)
            {
                firstTickFix = true;
            }
        }
        position = transform.position;
    }

    public void Stop()
    {
        timer = 0;
        isFalling = false;
        firstTickFix = false;
    }

    private bool isPositive(float f)
    {
        if (f >= 0.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
