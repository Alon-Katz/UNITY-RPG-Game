using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class CharacterCombat : MonoBehaviour
{
    const float combatCoolDown = 5;

    public float attackSpeed = 1f;
    public float attackDelay = .6f;
    public event System.Action OnAttack;
    public bool InCombat { get; private set; }
    private float lastAttackTime;
    private float attackCooldown = 0f;
    CharacterStats myStats;
    CharacterStats opponentStats;

    private void Start()
    {
        myStats = GetComponent<CharacterStats>();
    }

    private void Update()
    {
        attackCooldown -= Time.deltaTime;

        if (Time.time - lastAttackTime > combatCoolDown)
        {
            InCombat = false;
        }
    }

    public void Attack(CharacterStats targetStats)
    {
        if (attackCooldown <= 0f)
        {
            opponentStats = targetStats;
            OnAttack?.Invoke();

            attackCooldown = 1f / attackSpeed;
            InCombat = true;
            lastAttackTime = Time.time;
        }
    }

    public void AttackHit_AnimationEvent()
    {
        opponentStats.TakeDamage(myStats.damage.GetValue());
        if (opponentStats.currentHealth <= 0)
        {
            InCombat = false;
        }
    }
}
