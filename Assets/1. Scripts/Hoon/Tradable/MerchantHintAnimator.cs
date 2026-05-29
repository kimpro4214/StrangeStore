using System.Collections;
using UnityEngine;

public class MerchantHintAnimator : MonoBehaviour
{

    [SerializeField] private Animator animator;

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    public void PlayHintSequence(ItemType item)
    {
        if (animator == null) return;

        string targetTrigger = "";
        switch (item)
        {
            case ItemType.Apple:
                targetTrigger = "HintStandingGreeting";
                break;
            case ItemType.Dumbbell:
                targetTrigger = "HintVictory";
                break;
            case ItemType.Money:
                targetTrigger = "HintSpinning";
                break;
            default:
                Debug.Log("»˘∆Æ æ∆¿Ã≈€¿Ã æ∆¥‘.");
                return;
        }

        animator.SetTrigger(targetTrigger);
    }
}
