using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            animator.SetBool("isRotate", true);
            animator.SetBool("isScale", false);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            animator.SetBool("isRotate", false);
            animator.SetBool("isScale", true);
        }
        else
        {
            animator.SetBool("isRotate", false);
            animator.SetBool("isScale", false);
        }
    }
}
