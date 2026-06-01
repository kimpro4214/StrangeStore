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
        AudioManager.Instance.Play3D(SoundName.open_final_door, transform.position);
    }
    public void CloseDoor()
    {
        animator.SetTrigger("Door_Close");
    }
}
