using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int maxHp;
    public int hp;

    public float kbForce;
    public float kbTime;

    PlayerMovement pMove;
    Attack attack;
    Rigidbody2D rb;
    SpriteRenderer sprite;

    public Slider healthSlider;

    public UnityEvent onDamage;
    public UnityEvent onDeath;

    Vector2 knockback;

    private void Awake()
    {
        pMove = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        attack = GetComponent<Attack>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        hp = maxHp;
        healthSlider.maxValue = maxHp;
        healthSlider.value = maxHp;
        knockback = new Vector2(-kbForce * transform.localScale.x, rb.velocity.y);
    }

    public void Damage(int amount)
    {
        if (!attack.isBlocking)
        {
            onDamage.Invoke();
            hp -= amount;
            healthSlider.value = hp;
            if (hp <= 0 && pMove.currentState != "Player_death")
            {
                healthSlider.value = 0;
                Die();
            }
            sprite.color = Color.red;
            pMove.enabled = false;
            attack.enabled = false;
        }
        else sprite.color = Color.cyan;
        rb.velocity = knockback;
        Invoke("Refresh", kbTime);
    }

    private void Die()
    {
        pMove.ChangeAnimationState("Player_death");
        onDeath.Invoke();
    }

    private void Refresh()
    {
        sprite.color = Color.white;
        rb.velocity = Vector2.zero;
        pMove.enabled = true;
        attack.enabled = true;
    }
}
