using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject blockPrefab;
    public Vector3 topLeftBlockPosition = new Vector3(-6.5f, 0f, 0f);
    public int numRows = 8;
    public int numCols = 14;
    public Color[] rowColors = new Color[8];

    private GameObject[,] activeBlocks;

    // Start is called before the first frame update
    void Start()
    {
        activeBlocks = new GameObject[numRows, numCols];
        SetUpBlocks();
    }

    // Update is called once per frame
    void Update()
    {
        if (Block.blockCount == 0)
        {
            Debug.Log("Level Complete");
        }
    }

    private void SetUpBlocks()
    {
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                Vector3 pos = new Vector3(
                    topLeftBlockPosition.x + col * 1,
                    topLeftBlockPosition.y - row * 0.5f,
                    0
                );
                GameObject obj = Instantiate(blockPrefab, pos, Quaternion.identity);
                obj.GetComponent<SpriteRenderer>().color = rowColors[row];
                activeBlocks[row, col] = obj;
            }
        }
        // Destroy(activeBlocks[2, 3]);
    }
}
