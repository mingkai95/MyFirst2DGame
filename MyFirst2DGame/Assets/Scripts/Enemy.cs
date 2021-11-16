using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    #region//ê›íËÇ∑ÇÈïœêî
    [Header("îÌíeóÕ")]
    [SerializeField] float knockBackForce;
    [Header("ëÃóÕ")]
    [SerializeField] int hp = 100;
    [Header("çUåÇóÕ")]
    [SerializeField] int attack = 50;
    #endregion

    Rigidbody2D rb;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<IDamageable>().TakeDamage(this.transform.position, attack);
        }
    }

    public void TakeDamage(Vector3 damagerPos, int damageTaken)
    {
        if(hp > 0)
        {
            Vector3 knockBackDirection = new Vector3((this.transform.position - damagerPos).normalized.x, 1f, 0f);
            KnockBack(knockBackDirection);
            HandleHp(-damageTaken);
        }
    }

    void KnockBack(Vector3 knockBackDirection)
    {
        rb.AddForce(knockBackDirection * knockBackForce, ForceMode2D.Impulse);
    }

    void HandleHp(int number)
    {
        hp += number;
        if(hp <= 0)
        {
            anim.Play("Enemy_Dead");
            gameObject.layer = 6;
            //Destroy(gameObject);
        }
    }
}
