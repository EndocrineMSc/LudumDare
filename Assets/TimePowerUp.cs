using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;
using Utilites;
using DG.Tweening;

public class TimePowerUp : MonoBehaviour
{
    private void Start()
    {
        transform.DOScale(0.5f, 1f).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            transform.DOKill(gameObject);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().DeliveryBagTimer += 10;
            LevelEvents.Instance.TimePowerUpPickedUp?.Invoke();
            Destroy(gameObject);
        }
    }
}
