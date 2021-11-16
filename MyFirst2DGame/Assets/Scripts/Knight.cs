using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour, IDamager, IDamageable
{
    #region//Inspectorで設定する変数
    [Header("移動速度")]
    [SerializeField] float moveSpeed = 2f;
    [Header("ジャンプ力")]
    [SerializeField] float jumpForce = 20f;
    [Header("攻撃力")]
    [SerializeField] int attack = 50;
    [Header("地面判定大きさ")]
    [SerializeField] Vector2 groundCheckBoxSize = new Vector2(0.5f, 0.1f);
    [Header("攻撃範囲")]
    [SerializeField] float attackArea = 0.3f;

    [SerializeField] InputEvent _inputEvent = default;
    [SerializeField] LayerMask groundLayerMask;
    #endregion

    #region//private 変数
    [SerializeField] Vector2 velocity;
    [SerializeField] Vector3 scale;

    [SerializeField] Transform attackPos;

    [SerializeField] float knockBackForce;

    [SerializeField] bool isOnGround;
    [SerializeField] bool isDamaging = false;

    Rigidbody2D rb;
    Animator anim;
    CapsuleCollider2D col;
    SpriteRenderer sr;
    #endregion

    #region//ライフサイクル
    void OnEnable()
    {
        _inputEvent.moveEvent += OnMove;
        _inputEvent.jumpEvent += OnJump;
        _inputEvent.attackEvent += OnAttack;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<CapsuleCollider2D>();
        sr = GetComponent<SpriteRenderer>();

        scale = transform.localScale;
    }

     void Update()
    {
        OnGroundCheck();
        HandleAnimation();
    }

    void FixedUpdate()
    {
        if (!isDamaging)
        {
            rb.velocity = new Vector2(velocity.x * moveSpeed, rb.velocity.y);
        }
    }

    void OnDisable()
    {
        _inputEvent.moveEvent -= OnMove;
        _inputEvent.jumpEvent -= OnJump;
        _inputEvent.attackEvent -= OnAttack;
    }
    #endregion

    #region//条件チェック関連
    void OnGroundCheck()
    {
        RaycastHit2D hit = Physics2D.BoxCast(col.bounds.center - new Vector3(0, col.bounds.extents.y + groundCheckBoxSize.y, 0), groundCheckBoxSize, 0f, Vector2.down, groundCheckBoxSize.y, groundLayerMask);
        isOnGround = hit.collider != null;
    }
    #endregion

    #region//操作関連
    void OnMove(Vector2 vector2)
    {
        velocity.x = vector2.x;
        if (velocity.x != 0)
        {
            scale.x = velocity.x;
            transform.localScale = scale;
        }
    }

    void OnJump()
    {
        if (isOnGround)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

    }

    void OnAttack()
    {
        if (isOnGround)
        {
            Collider2D[] damageableInRange = Physics2D.OverlapCircleAll(attackPos.position, attackArea);
            foreach(Collider2D obj in damageableInRange)
            {
                InflictDamage(obj.GetComponent<IDamageable>());
            }
        }
    }
    #endregion

    #region//ダメージ関連
    public void InflictDamage(IDamageable target)
    {
        target?.TakeDamage(this.transform.position, attack);
    }

    public void TakeDamage(Vector3 damagerPos, int damageTaken)
    {
        isDamaging = true;
        Vector3 knockBackDirection = new Vector3((this.transform.position - damagerPos).normalized.x, 1f, 0f);
        KnockBack(knockBackDirection);
    }

    void KnockBack(Vector3 knockBackDirection)
    {
        Debug.Log(knockBackDirection * knockBackForce);
        rb.AddForce(knockBackDirection * knockBackForce, ForceMode2D.Impulse);
        StartCoroutine(damageMe());
    }

    IEnumerator damageMe()
    {
        IEnumerator blink = BlinkThis();
        StartCoroutine(blink);
        yield return new WaitForSeconds(1);
        isDamaging = false;
        StopCoroutine(blink);
    }

    IEnumerator BlinkThis()
    {
        while (true)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(0.1f);
        }

    }
    #endregion

    #region//アニメーション関連
    private void HandleAnimation()
    {
        if (isOnGround)
        {
            if (velocity.x != 0)
            {
                anim.Play("Knight_Move");
            }
            else
            {
                anim.Play("Knight_Idle");
            }
        }
        else
        {
            anim.Play("Knight_Jump");
        }
    }
    #endregion

    #region//Gizmo関連
    private void OnDrawGizmos()
    {
        if (col != null)
        {
            //Ground Check
            Gizmos.color = Color.red;
            Vector3 colLocation = col.bounds.center - new Vector3(0, col.bounds.extents.y + groundCheckBoxSize.y, 0);
            Gizmos.DrawCube(colLocation, groundCheckBoxSize);
        }

        if (attackPos != null)
        {
            //Attack Check
            Gizmos.color = Color.grey;
            Gizmos.DrawWireSphere(attackPos.position, attackArea);
        }
    }
    #endregion
}
