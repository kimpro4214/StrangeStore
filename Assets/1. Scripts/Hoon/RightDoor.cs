using System.Collections;
using UnityEngine;

public class RightDoor : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public IEnumerator OpenDoor()
    {
        yield return new WaitForSeconds(2f);
        animator.SetTrigger("RightDoor_Open");
    }
}
