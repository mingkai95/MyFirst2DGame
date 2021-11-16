using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableItem : MonoBehaviour, IDamageable
{
    [SerializeField] int hp = 100;
    [SerializeField] Sprite brokeSprite;

    SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(Vector3 damagerPos, int damageTaken)
    {
        if (hp > 0)
        {
            HandleHp(-damageTaken);
        }
    }

    void HandleHp(int number)
    {
        hp += number;
        if (hp <= 0)
        {
            sr.sprite = brokeSprite;
            //Destroy(gameObject);
        }
    }
}
