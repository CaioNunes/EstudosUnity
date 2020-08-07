using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    public Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayers; //detectar a layer que os inimigos estão

    public float attackRange = 0.5f;
    public int attackDamage = 40;

    public float attackRate = 2f;
    float nextAttackTime = 0f;

    // Update is called once per frame
    void Update()
    {

        if (Time.time >= nextAttackTime) {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }

        }
    }

    void Attack() {
        // Play an attack animation
        animator.SetTrigger("Attack");

        // Detect enemies in range off attack
        // Essa função cria um círculo a partir do ponto especificado, de raio do tamanho do range especificado e coleta todos os objetos em que o círculo colide.
        // Para trabalhar em 3D basta mudar para Physics, e o método é o OverlapSphere.
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies) {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);

        }

        // Damage them
    }
    //Função que permite visualizar o círculo que será gerado pela função OverlapCircleAll, para auxiliar nos ajustes.
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
