using UnityEngine;

[CreateAssetMenu(fileName = "New Servicer Data", menuName = "Servicer Data")]
public class ServicerData : ScriptableObject
{
    public int price;
    public int prize_amount;
    public ePrizeType prize_type;
    public int pick_yes = 0;
    public int pick_total = 0;
    public int level;
}