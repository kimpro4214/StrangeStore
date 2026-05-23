using System;
using UnityEngine;
using System.Collections;

public class DigPoint : MonoBehaviour
{
    public bool canDigging = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Shovel"))
        {
            canDigging = true;
            Debug.Log("»ð µé¾î¿È!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Shovel"))
        {
            canDigging = false;
            Debug.Log("»ð ³ª°¨!");
        }
    }
}
