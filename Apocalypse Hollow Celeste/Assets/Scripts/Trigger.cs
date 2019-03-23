using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] private GameObject destroyThis;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Object.Destroy(destroyThis);
    }
}
