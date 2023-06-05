using UnityEngine;

//combo with damage multiplyer

public class Attack : MonoBehaviour
{
    public KeyCode punch = KeyCode.G;
    public KeyCode kick = KeyCode.H;
    public KeyCode block = KeyCode.F;
    public PlayerMovement pMove;
    public LayerMask lMask;
    public bool isAttacking = false;
    public int attackCount = 0;
    float attackPressInterval = 0.15f;
    bool attackPressed = false;
    float maxDistance;
    Vector3 offset;
    float attackDelay = 0.1f;
    public bool isBlocking = false;

    enum DamageType
    {
        punch,
        kick,
        none
    }
    DamageType dmgType;
    public int damage;

    const string PLAYER_BLOCK_STAND = "Player_block_stand";
    const string PLAYER_BLOCK_CROUCH = "Player_block_crouch";
    const string PLAYER_L_PUNCH_STAND = "Player_light_punch_stand";
    const string PLAYER_L_PUNCH_CROUCH = "Player_light_punch_crouch";
    const string PLAYER_H_PUNCH_STAND = "Player_hard_punch_stand";
    const string PLAYER_H_PUNCH_CROUCH = "Player_hard_punch_crouch";
    const string PLAYER_L_KICK_STAND = "Player_light_kick_stand";
    const string PLAYER_L_KICK_CROUCH = "Player_light_kick_crouch";
    const string PLAYER_H_KICK_STAND = "Player_hard_kick_stand";
    const string PLAYER_H_KICK_CROUCH = "Player_hard_kick_crouch";

    private void Update()
    {
        if (Input.GetKeyDown(punch) && !isAttacking && !isBlocking)
        {
            attackPressed = true;
            if (!isAttacking && attackPressed && attackCount == 0)
            {
                Invoke("PerformAttack", attackPressInterval);
            }
            attackCount++;
            dmgType = DamageType.punch;
        }

        if (Input.GetKeyDown(kick) && !isAttacking && !isBlocking)
        {
            attackPressed = true;
            if (!isAttacking && attackPressed && attackCount == 0)
            {
                Invoke("PerformAttack", attackPressInterval);
            }
            attackCount++;
            dmgType = DamageType.kick;
        }

        if (Input.GetKey(block) && pMove.isGrounded && !isAttacking && !attackPressed)
        {
            isBlocking = true;
            if (pMove.isCrouched) pMove.ChangeAnimationState(PLAYER_BLOCK_CROUCH);
            else pMove.ChangeAnimationState(PLAYER_BLOCK_STAND);
        }
        else isBlocking = false;
    }

    private void FixedUpdate()
    {

    }

    private void PerformAttack()
    {
        if (isBlocking) return;
        isAttacking = true;
        if (pMove.isGrounded && !pMove.isCrouched)
        {
            if (dmgType == DamageType.punch)
            {
                if (attackCount == 1)
                {
                    pMove.ChangeAnimationState(PLAYER_L_PUNCH_STAND);
                    maxDistance = 2.5f;
                    offset = new Vector3(0, 1, 0);
                    damage = 5;
                    attackDelay = 0.1f;
                }
                else if (attackCount > 1)
                {
                    pMove.ChangeAnimationState(PLAYER_H_PUNCH_STAND);
                    maxDistance = 3f;
                    offset = new Vector3(0, 1, 0);
                    damage = 10;
                    attackDelay = 0.25f;
                }
            }

            else if (dmgType == DamageType.kick)
            {
                if (attackCount == 1)
                {
                    pMove.ChangeAnimationState(PLAYER_L_KICK_STAND);
                    maxDistance = 2.4f;
                    offset = new Vector3(0, -0.6f, 0);
                    damage = 5;
                    attackDelay = 0.1f;
                }
                else if (attackCount > 1)
                {
                    pMove.ChangeAnimationState(PLAYER_H_KICK_STAND);
                    maxDistance = 3.5f;
                    offset = new Vector3(0, 1.65f, 0);
                    damage = 12;
                    attackDelay = 0.3f;
                }
            }
        }

        else if (pMove.isGrounded && pMove.isCrouched)
        {
            if (dmgType == DamageType.punch)
            {
                if (attackCount == 1)
                {
                    pMove.ChangeAnimationState(PLAYER_L_PUNCH_CROUCH);
                    maxDistance = 2.5f;
                    offset = new Vector3(0, 0.5f, 0);
                    damage = 4;
                    attackDelay = 0.1f;
                }
                else if (attackCount > 1)
                {
                    pMove.ChangeAnimationState(PLAYER_H_PUNCH_CROUCH);
                    maxDistance = 2.5f;
                    offset = new Vector3(0, 0.5f, 0);
                    damage = 7;
                    attackDelay = 0.25f;
                }
            }

            else if (dmgType == DamageType.kick)
            {
                if (attackCount == 1)
                {
                    pMove.ChangeAnimationState(PLAYER_L_KICK_CROUCH);
                    maxDistance = 3f;
                    offset = new Vector3(0, -1f, 0);
                    damage = 5;
                    attackDelay = 0.1f;
                }
                else if (attackCount > 1)
                {
                    pMove.ChangeAnimationState(PLAYER_H_KICK_CROUCH);
                    maxDistance = 4.5f;
                    offset = new Vector3(0, -1f, 0);
                    damage = 8;
                    attackDelay = 0.3f;
                }
            }
        }
        //var animLength = pMove.animator.GetCurrentAnimatorStateInfo(0).length;
        //Invoke("CastAttack", animLength / 2);
        Invoke("CastAttack", attackDelay);
    }

    private void CastAttack()
    {
        var hit = Physics2D.Raycast(transform.position + offset, Vector2.right * transform.localScale.x, maxDistance, lMask);
        if (hit)
        {
            var health = hit.transform.GetComponent<Health>();
            if (health)
            {
                health.Damage(damage);
            }
        }

        isAttacking = false;
        attackPressed = false;
        attackCount = 0;
        dmgType = DamageType.none;
        damage = 0;
    }
}
