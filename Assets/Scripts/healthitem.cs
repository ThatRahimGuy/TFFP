using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class healthitem : MonoBehaviour
{
    public int healAmount = 1;
    public static event Action<int> OnHealthCollect;
    public void Collect()
    {
        OnHealthCollect?.Invoke(healAmount);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player")) Collect();
    }
}
