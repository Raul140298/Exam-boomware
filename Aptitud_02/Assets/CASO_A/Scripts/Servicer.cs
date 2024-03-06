using UnityEngine;

public class Servicer : MonoBehaviour
{
    //PARAMS
    private int price;
    private int prize_amount;
    private ePrizeType prize_type;
    private float prize_chance;
    private int max_prize = 5;

    [SerializeField] private ServicerData data;
    [SerializeField] private Vector2 collect_position;

    protected virtual void Awake()
    {
    }

    protected virtual void Start()
    {
        data = GetComponent<ServicerData>();
        price = data.price;
        prize_amount = data.prize_amount;
        prize_type = data.prize_type;
        prize_chance = (float)data.pick_yes / data.pick_total;
    }

    protected void Collect()
    {
        Vector2 p = new Vector2(transform.position.x, transform.position.y) + collect_position;

        bool shouldGivePrize = prize_chance >= Random.Range(0f, 1f) && max_prize > 0;
        
        switch (prize_type)
        {
            case ePrizeType.gems:
                if (shouldGivePrize)
                {
                    max_prize--;
                    //give "prize_amount" gems
                }
                break;

            case ePrizeType.ingredients:
                if (shouldGivePrize)
                {
                    max_prize--;
                    //give "prize_amount" ingredients
                }
                break;
            
            case ePrizeType.coins:
                //give "price" coins
                break;
        }
    }

    public int Level => data.level;
}
