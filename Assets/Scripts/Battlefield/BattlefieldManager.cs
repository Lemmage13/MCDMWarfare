using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlefieldManager : MonoBehaviour
{
    public int ranks = 4;
    public int files = 5;

    public Space space;
    void Start()
    {
        BuildBattlefield();
    }
    void BuildBattlefield()
    {
        for (int x = 0; x < files; x++)
        {
            for (int y = 0; y < ranks; y++)
            {
                float xpos = x * 1.5F;
                var spawnedSpace = Instantiate(space, new Vector3(xpos, y, -1), Quaternion.identity);
                spawnedSpace.name = $"Space {x}, {y}";
                spawnedSpace.transform.parent = this.transform;
            }
        }
        float frontStart = ranks + 0.3F;
        for (float x = 0; x < files; x++)
        {
            for (float y = 0; y < ranks; y++)
            {
                float xpos = x * 1.5F;
                float ypos = y + frontStart;
                var spawnedSpace = Instantiate(space, new Vector3(xpos, ypos, -1), Quaternion.identity);
                spawnedSpace.name = $"Front Space {files - x}, {ranks - y}";
                spawnedSpace.transform.parent = this.transform;
            }
        }
    }
}
