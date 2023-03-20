using System.Collections.Generic;
using UnityEngine;

public class BattlefieldManager : MonoBehaviour
{
    public static BattlefieldManager instance;
    public int ranks = 4;
    public int files = 5;
    public Vector3 posTweak;

    public Space space;
    public Transform cam;

    public List<Space> Spaces = new List<Space>();
    private void Awake()
    {
        instance = this;
    }
    private void BattleStateUpdated(BattleState state)
    {
        
    }
    void Start()
    {
        BuildBattlefield();
        UnitManager.instance.Spawning();
    }
    void BuildBattlefield()
    {
        for (int y = 0; y < ranks; y++)
        {
            for (int x = 0; x < files; x++)
            {
                float xpos = x * 1.5F;
                var spawnedSpace = Instantiate(space, new Vector3(xpos, y, -1), Quaternion.identity);
                spawnedSpace.name = $"Space {x},{y}";
                spawnedSpace.transform.parent = this.transform;
                spawnedSpace.x = x;
                spawnedSpace.y = y;
                Spaces.Add(spawnedSpace);
                spawnedSpace.Front = false;
            }
        }
        float frontStart = ranks + 0.3F;
        for (int y = 0; y < ranks; y++)
        {
            for (int x = 0; x < files; x++)
            {
                float xpos = x * 1.5F;
                float ypos = y + frontStart;
                var spawnedSpace = Instantiate(space, new Vector3(xpos, ypos, -1), Quaternion.identity);
                spawnedSpace.name = $"Front Space {x}, {y}";
                spawnedSpace.transform.parent = this.transform;
                spawnedSpace.x = x;
                spawnedSpace.y = y + ranks;
                Spaces.Add(spawnedSpace);
                spawnedSpace.Front = true;
            }
        }
        foreach (Space space in Spaces)
        {
            space.ListAdjacent();
        }
        cam.transform.position = new Vector3((float)files / 2, (float)ranks + 0.3F, -100);
    }
    public void clearPlates()
    {
        for (int i = 0; i < Spaces.Count; i++)
        {
            Spaces[i].Movable.SetActive(false);
            Spaces[i].Attackable.SetActive(false);
        }
    }
}
