using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableAnimationController : Activable
{
    public bool initialActive;
    private Animator animator;

    public override void activate() {
        animator.SetBool("isActive", true);
        base.activate();
    }

    public override void deactivate() {
        animator.SetBool("isActive", false);
        base.deactivate();
    }
    // Start is called before the first frame update
    public override void Start()
    {
        animator = GetComponent<Animator>();
        _active = initialActive;
        base.Start();
    }
}
