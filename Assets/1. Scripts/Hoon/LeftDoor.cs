using System.Collections;
using UnityEngine;

public class LeftDoor : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public IEnumerator OpenDoor()
    {
        yield return new WaitForSeconds(2f);
        animator.SetTrigger("LeftDoor_Open");
    }
}
