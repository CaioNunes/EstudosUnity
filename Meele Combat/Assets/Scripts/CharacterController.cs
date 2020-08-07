using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{


    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f; //Suavização do movimento
    private Rigidbody2D m_RigidBody2D;
    private bool m_FacingRight = true; //Se o player está virado pra direita
    private Vector3 m_Velocity = Vector3.zero;

    private void Awake()
    {
        m_RigidBody2D = GetComponent<Rigidbody2D>();
    }


    public void Move(float move) {
        Vector3 targetVelocity = new Vector2(move * 10f, m_RigidBody2D.velocity.y);

        m_RigidBody2D.velocity = Vector3.SmoothDamp(m_RigidBody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

        if (move > 0 && !m_FacingRight)
        {
            Flip();
        }
        else if (move < 0 && m_FacingRight) {
            Flip();
        }

  
    }

    void Flip() {
        m_FacingRight = !m_FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;

        transform.localScale = theScale;
    }
}
