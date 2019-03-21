using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public GameObject destroyThis;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Break!");
        Object.Destroy(destroyThis);
    }
}
