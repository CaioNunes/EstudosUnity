using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{

    [SerializeField] private float m_JumpForce = 400f; //Força do pulo
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f; //Suavização do movimento
    [SerializeField] private bool m_AirControl = false; // Se o personagem pode ser controlado no ar
    [SerializeField] private LayerMask m_WhatIsGround; // Qual layer é o "Ground"
    [SerializeField] private Transform m_GroundCheck; // Verifica se está no chão
    [SerializeField] private Transform m_CeilingCheck; // Verifica se há um teto sobre ele

    const float k_GroundedRadius = .2f; //Raio ao redor do objeto pra verificar o chão
    private bool m_Grounded; //Se o player está ou não no chão
    const float k_CeilingRadius = .2f; //Raio ao redor do objeto para verificar o teto
    private Rigidbody2D m_RigidBody2D;
    private bool m_FacingRight = true; //Se o player está virado pra direita
    private Vector3 m_Velocity = Vector3.zero;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake()
    {
        m_RigidBody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        //A função vai gerar um círculo de raio definido pelo k_GroundedRadius, a partir do ponto m_GroundCheck que vai detectar os colisores
        //de tudo que estiver definido pela variável m_WhatIsGround
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].gameObject != gameObject) {
                m_Grounded = true;

                if (!wasGrounded) {
                    OnLandEvent.Invoke();
                }
            }
        }
    }

    public void Move(float move, bool jump)
    {
        if (m_Grounded || m_AirControl) {

            Vector3 targetVelocity = new Vector2(move * 10f, m_RigidBody2D.velocity.y);
            
            // Altera gradualmente um vetor em direção a uma meta desejada ao longo do tempo.
            // Vetor da velocidade, Vetor da velocidade alvo, ref VelocidadeAtual, float tempo de suavização
            m_RigidBody2D.velocity = Vector3.SmoothDamp(m_RigidBody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            if (move > 0 && !m_FacingRight)
            {
                Flip();
            }
            else if (move < 0 && m_FacingRight) {
                Flip();
            }
        }

        if (m_Grounded && jump) {
            m_Grounded = false;
            m_RigidBody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
    }

    void Flip() {
        m_FacingRight = !m_FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }


}
