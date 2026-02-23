using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float startSpeed = 8.0f;
    public float speedIncrementPct = .2f;
    public float maxSpeed = 12.0f;
    public float maxAngle = 45; //theoretically the max angle for bounce

    public int blocksForSpeedInc1 = 4;
    public int blocksForSpeedInc2 = 12;

    //when prog is solidified, these will be private
    public float currentSpeed;
    public int blocksBroken;

    private Rigidbody2D rb;
    private bool maxSpeedReached = false;
    

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Block"))
        {
            blocksBroken++;
            CheckForSpeedIncrease();
        }
        else if (collision.transform.CompareTag("TopWall"))
        {
            maxSpeedReached = true;
            CheckForSpeedIncrease();
        }
        // vary the angle predictably based on where on the paddle it hit
        else if (collision.transform.CompareTag("Paddle"))
        {
            //TODO: should check if ball is below paddle, bounce down if so

            //convert where it hit to an angle, from - maxAngle to + maxAngle
            float xDifference = transform.position.x -
                collision.transform.position.x;

            // *2 is because xDiff of .5 should yield the max angle
            float desiredAngle = xDifference * maxAngle * 2;

            Quaternion rot = Quaternion.AngleAxis(desiredAngle, Vector3.forward * -1);
            Vector3 newVector3 = rot * Vector3.up;

            Vector3 vel = newVector3.normalized * currentSpeed;
            
            rb.velocity = vel;
        }
    }

    private void CheckForSpeedIncrease()
    {
        if (maxSpeedReached)
        {
            currentSpeed = maxSpeed;
        }
        else if (blocksBroken == blocksForSpeedInc1 || blocksBroken == blocksForSpeedInc2)
        {
            currentSpeed += currentSpeed * speedIncrementPct;
        }

        //full logic:
        /*
         * Vector3 vel = rb.velocity;
         * vel = vel.normalized;
         * vel = vel * currentSpeed;
         * rb.velocity = vel;
         */
        rb.velocity = rb.velocity.normalized * currentSpeed;
    }

    public void Launch()
    {
        currentSpeed = startSpeed;
        Vector3 speed = Vector3.up + Vector3.right;
        speed = speed.normalized;
        rb.velocity = speed * startSpeed;
    }
}
