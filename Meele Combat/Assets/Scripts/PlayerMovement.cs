using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;

    private float horizontalMove = 0f;

    public float runSpeed = 40f;

    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));


    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime);

    }
}
