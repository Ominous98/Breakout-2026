using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int hp = 1;

    public static int blockCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        //keep track of how many blocks are created/active
        blockCount++;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ball")) {
            hp--;
            if (hp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        //remember to reduce the number of blocks when I'm destroyed
        blockCount--;
    }
}
