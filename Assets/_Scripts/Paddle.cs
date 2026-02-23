using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public float paddleY = -9.0f;
    public GameObject ballPrefab;
    public float ballYOffset = 0.3f;
    public float paddleMaxX = 6.25f;

    private float width;
    private bool isLaunched;

    void Start()
    {
        isLaunched = false;
    }

    // Update is called once per frame
    void Update()
    {
        //get mouse position
        Vector3 mouseScreenPos = Input.mousePosition;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        
        //clamp the X to where the paddle is allowed
        mouseWorldPos.x = Mathf.Clamp(mouseWorldPos.x, -1 * paddleMaxX, paddleMaxX);
        mouseWorldPos.y = paddleY;
        mouseWorldPos.z = 0;


        //move my x to mouse position
        transform.position = mouseWorldPos;
        

        //if ball isn't launched yet
        if(Input.GetMouseButtonDown(0))
        {
            // instantiate the ball
            Vector3 startPos = new Vector3(
                transform.position.x,
                transform.position.y + ballYOffset,
                0
                );
            GameObject obj = Object.Instantiate(ballPrefab, startPos, Quaternion.identity);
            obj.GetComponent<Ball>().Launch();
        }
    }

    public void SetSize(float size)
    {
        width = size;
    }
}
