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
        GameManager.GameStateChanged += GameManager_GameStateChanged;
    }
    private void OnDestroy()
    {
        GameManager.GameStateChanged -= GameManager_GameStateChanged;
    }
    private void GameManager_GameStateChanged(GameState state)
    {
        
    }
    void Start()
    {
        BuildBattlefield();
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
                Spaces.Add(spawnedSpace);
            }
        }
        cam.transform.position = new Vector3((float)files / 2, (float)ranks + 0.15F, -10);
    }
    //NOTE ---- SERIOUS IMPROVEMENTS REQUIRED FOR THE MOVE/ATTACK PLATE SYSTEM 
    //--------- COORDINATE SYSTEM LIKELY REQUIRED
    public void MovePlate(Space unitSpace)
    {
        int index = Spaces.IndexOf(unitSpace);
        int rI = index + 1;
        int lI = index - 1;
        int uI = index + files;
        int dI = index - files;
        if ((index + 1) % files != 0) 
        {
            if (Spaces[rI].occupiedBy == null){ Spaces[rI].Movable.SetActive(true); }
        }
        if (index % files != 0)
        {
            if (Spaces[lI].occupiedBy == null) { Spaces[lI].Movable.SetActive(true); }
        }
        if (index + files <= Spaces.Count) 
        {
            if (Spaces[uI].occupiedBy == null) { Spaces[uI].Movable.SetActive(true); }
        }
        if (index >= files)
        {
            if (Spaces[dI].occupiedBy == null) { Spaces[dI].Movable.SetActive(true); }
        }
    }
    public void AttackPlate(Space unitSpace)
    {
        Unit unit = unitSpace.occupiedBy;
        
        int index = Spaces.IndexOf(unitSpace);
        int rI = index + 1;
        int lI = index - 1;
        int uI = index + files;
        int dI = index - files;
        if ((index + 1) % files != 0)
        {
            if (Spaces[rI].occupiedBy != null && !UnitManager.instance.CompareTeam(unit, Spaces[rI].occupiedBy)) 
                { Spaces[rI].Attackable.SetActive(true); }
        }
        if (index % files != 0)
        {
            if (Spaces[lI].occupiedBy != null && !UnitManager.instance.CompareTeam(unit, Spaces[lI].occupiedBy))
            { Spaces[lI].Attackable.SetActive(true); }
        }
        if (index + files <= Spaces.Count)
        {
            if (Spaces[uI].occupiedBy != null && !UnitManager.instance.CompareTeam(unit, Spaces[uI].occupiedBy)) 
            { Spaces[uI].Attackable.SetActive(true); }
        }
        if (index >= files)
        {
            if (Spaces[dI].occupiedBy != null && !UnitManager.instance.CompareTeam(unit, Spaces[dI].occupiedBy)) 
            { Spaces[dI].Attackable.SetActive(true); }
        }
    }
    public void SpawnPlate(bool onoff, bool side)
    {
        int start = 0;
        if (!side) { start = Spaces.Count / 2; }
        for (int i = start; i < Spaces.Count/2 + start; i++)
        {
            if (Spaces[i].occupiedBy == null) { Spaces[i].Movable.SetActive(onoff); }
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
