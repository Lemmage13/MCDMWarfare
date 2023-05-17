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
    public List<Space> OffFieldSpacesPlayer = new List<Space>();
    public List<Space> OffFieldSpacesDM = new List<Space>();
    private void Awake()
    {
        instance = this;
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
                AssignRank(spawnedSpace, ranks - 1 - y);
                spawnedSpace.Front = false;
                Spaces.Add(spawnedSpace);
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
                AssignRank(spawnedSpace, y);
                spawnedSpace.Front = true;
                Spaces.Add(spawnedSpace);
            }
        }
        foreach (Space space in Spaces)
        {
            space.ListAdjacent();
        }
        OffFieldSpaceGen();
        cam.GetComponent<CamMvmtController>().SetCamCentre(new Vector3((float)files / 2, (float)ranks + 0.3F, -100));
    }
    void AssignRank(Space space, int rank)
    {
        space.Rank = (Rank)rank;
    }
    void OffFieldSpaceGen()
    {
        int cavnum = 0;
        int ymax = 3;
        float xpos = this.transform.GetChild(0).transform.position.x;
        float ypos = this.transform.GetChild(0).transform.position.y;

        List<int[]> playerunits = UnitList.GetPlayerUnits();
        List<int[]> DMunits = UnitList.GetDMUnits();
        for (int i = 0; i < playerunits.Count; i++)
        {
            if ((UnitType)playerunits[i][1] == UnitType.Cavalry)
            {
                cavnum++;
                var spawnedSpace = Instantiate(space, new Vector3(xpos, ypos, -1), Quaternion.identity);
                OffFieldSpacesPlayer.Add(spawnedSpace);
                ypos--;
                if (cavnum % ymax == 0)
                {
                    xpos -= 1.5F;
                    ypos = this.transform.GetChild(0).transform.position.y;
                }
            }
        }
        //DM side
        xpos = this.transform.GetChild(1).transform.position.x;
        ypos = this.transform.GetChild(1).transform.position.y;
        cavnum = 0;
        for (int i = 0; i < DMunits.Count; i++)
        {
            if ((UnitType)DMunits[i][1] == UnitType.Cavalry)
            {
                cavnum++;
                var spawnedSpace = Instantiate(space, new Vector3(xpos, ypos, -1), Quaternion.identity);
                OffFieldSpacesDM.Add(spawnedSpace);
                ypos--;
                if (cavnum % ymax == 0)
                {
                    xpos -= 1.5F;
                    ypos = this.transform.GetChild(1).transform.position.y;
                }
            }
        }
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
