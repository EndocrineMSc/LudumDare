using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;
using Utilites;

public class TimePowerUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().DeliveryBagTimer += 10;
            LevelEvents.Instance.TimePowerUpPickedUp?.Invoke();
            Destroy(gameObject);
        }
    }
}
