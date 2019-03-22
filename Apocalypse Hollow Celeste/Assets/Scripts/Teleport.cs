using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform thingToTeleport;
    [SerializeField] private Transform teleportTo;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        thingToTeleport.transform.position = teleportTo.transform.position;
    }
}
