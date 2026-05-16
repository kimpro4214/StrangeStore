using UnityEngine;

public enum ItemType { Apple, Money, MusicBox, Dumbbell, Fish }

public class TradableItem : MonoBehaviour
{
    public ItemType type;
}