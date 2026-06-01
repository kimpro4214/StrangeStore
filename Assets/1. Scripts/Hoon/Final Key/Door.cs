using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator animator;
    public float openDelay = 2.5f;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public IEnumerator OpenDoor()
    {
        yield return new WaitForSeconds(openDelay);
        animator.SetTrigger("Door_Open");
    }
    public void CloseDoor()
    {
        animator.SetTrigger("Door_Close");
    }
}
