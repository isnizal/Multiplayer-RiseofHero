using UnityEngine;

[CreateAssetMenu(fileName ="Achivement System", menuName ="Achievement")]
public class AchievementSlot : ScriptableObject
{
    //Achievement Panel Variables
    public int achID;
    public string achName;
    public int achLevelRequired;
    public string achPanelTitle;
    public string achPanelDesc;
    public string achReward;
    [Range(1, 9999)] public int achAmountRequired = 1;
}
