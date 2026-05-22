using UnityEngine;

public class LockItem : MonoBehaviour
{
    public bool unlocked = false;
    Animator animator;
    [HideInInspector]
    public KeyItem keyItem;

    public GameObject explosionParticle;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayUnlockAnimation()
    {
        animator.SetTrigger("Unlock");
    }

    public void DestroyLock()
    {
        Destroy(Instantiate(explosionParticle, transform.position, Quaternion.identity), 3f);
        if (keyItem != null) Destroy(keyItem.gameObject);
        Destroy(this.gameObject);
    }
}
