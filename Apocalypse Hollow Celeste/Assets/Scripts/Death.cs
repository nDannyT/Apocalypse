using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform respawn;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.transform.position = respawn.transform.position;
        //Debug.Log("Teleport!");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}