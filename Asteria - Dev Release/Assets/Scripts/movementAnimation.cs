using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementAnimation : MonoBehaviour


{
    Animator animator;
    int IsWalkingHash;
    int IsRunningHash;
    int LeftHash;
    int RightHash;
    int JumpHash;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        IsWalkingHash = Animator.StringToHash("IsMoving");
        IsRunningHash = Animator.StringToHash("IsRunning");
        LeftHash = Animator.StringToHash("Left");
        RightHash = Animator.StringToHash("Right");
        JumpHash = Animator.StringToHash("Jump");

    }

    // Update is called once per frame
    void Update()
    {
        bool IsRunning = animator.GetBool(IsRunningHash);
        bool IsWalking = animator.GetBool(IsWalkingHash);
        bool Left = animator.GetBool(LeftHash);
        bool Right = animator.GetBool(RightHash);
        bool Jump = animator.GetBool(JumpHash);
        bool forwardPressed = Input.GetKey("w");
        bool runPressed = Input.GetKey("left shift");
        bool leftpressed = Input.GetKey("a");
        bool rightpressed = Input.GetKey("d");
        bool jumppressed = Input.GetKey("space");

        if (!IsWalking && forwardPressed)
        {
            animator.SetBool(IsWalkingHash, true);
        }
        if (IsWalking && !forwardPressed)
        {
            animator.SetBool(IsWalkingHash, false);
        }
        if (!IsRunning && (forwardPressed && runPressed))
        {
            animator.SetBool(IsRunningHash, true);
        }
        if (IsRunning && (!forwardPressed || !runPressed))
        {
            animator.SetBool(IsRunningHash, false);
        }
        if (!Left && leftpressed && !runPressed)
        {
            animator.SetBool(LeftHash, true);
        }
        if (Left && !leftpressed && !runPressed)
        {
            animator.SetBool(LeftHash, false);
        }
        if (!Right && rightpressed && !runPressed)
        {
            animator.SetBool(RightHash, true);
        }
        if (Right && !rightpressed && !runPressed)
        {
            animator.SetBool(RightHash, false);
        }
        if (!Jump && jumppressed)
        {
            animator.SetBool(JumpHash, true);
        }
        if (Jump && !jumppressed)
        {
            animator.SetBool(JumpHash, false);
        }
        
        
    }
}
