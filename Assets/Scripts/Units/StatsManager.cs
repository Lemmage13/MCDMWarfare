using TMPro;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    [SerializeField] TextMeshPro Name;
    [SerializeField] TextMeshPro Tier;

    [SerializeField] TextMeshPro HP;

    [SerializeField] TextMeshPro NoA;
    [SerializeField] TextMeshPro DMG;

    [SerializeField] TextMeshPro ATK;
    [SerializeField] TextMeshPro DEF;
    [SerializeField] TextMeshPro POW;
    [SerializeField] TextMeshPro TOU;
    [SerializeField] TextMeshPro MOR;
    [SerializeField] TextMeshPro COM;

    public void InitialiseStats()
    {
        UpdateName();
        UpdateTier();
        UpdateHP();
        UpdateAllStats();
        ChangeColour();
    }
    public void UpdateName()
    {
        string name = GetComponentInParent<BaseUnit>().Name;
        Name.text = name;
    }
    public void UpdateTier()
    {
        int tier = GetComponentInParent<BaseUnit>().GetTier();
        Tier.text = RomanNumeralConverter(tier);
    }
    public void UpdateHP()
    {
        int hp = GetComponentInParent<BaseUnit>().HP;
        HP.text = hp.ToString();
    }
    public void UpdateStat(Stats stat)
    {
        int statnum = GetComponentInParent<BaseUnit>().GetStat(stat);

        switch (stat)
        {
            case Stats.ATK:
                if (statnum <= 0) 
                { ATK.text = statnum.ToString(); }
                else {ATK.text = "+"+statnum.ToString(); }
                break;
            case Stats.DEF: DEF.text = statnum.ToString(); break;
            case Stats.POW:
                if (statnum <= 0)
                { POW.text = statnum.ToString(); }
                else { POW.text = "+" + statnum.ToString(); }
                break;
            case Stats.TOU: TOU.text = statnum.ToString(); break;
            case Stats.MOR:
                if (statnum <= 0)
                { MOR.text = statnum.ToString(); }
                else { MOR.text = "+" + statnum.ToString(); }
                break;
            case Stats.COM:
                if (statnum <= 0)
                { COM.text = statnum.ToString(); }
                else { COM.text = "+" + statnum.ToString(); }
                break;
            case Stats.NoA: NoA.text = statnum.ToString(); break;
            case Stats.DMG: DMG.text = statnum.ToString(); break;
            default: break;
        }
    }
    void ChangeColour()
    {
        SpriteRenderer spriteRenderer = GetComponentInParent<SpriteRenderer>();
        if (GetComponentInParent<BaseUnit>().Side)
        {
            spriteRenderer.color = new Color(0.7F, 0.85F, 0.7F, 1F);
        }
        else if (!GetComponentInParent<BaseUnit>().Side)
        {
            spriteRenderer.color = new Color(0.85F, 0.7F, 0.7F, 1F);
        }
    }
    public void UpdateAllStats()
    {
        for (int i = 0; i < 8; i++)
        {
            UpdateStat((Stats)i);
        }
    }
    string RomanNumeralConverter(int n) //Only works up to 5, but it only needs to work up to 5
    {
        switch (n)
        {
            case 1: return "I";
            case 2: return "II";
            case 3: return "III";
            case 4: return "IV";
            case 5: return "V";
            default: return n.ToString();
        }
    }
}
