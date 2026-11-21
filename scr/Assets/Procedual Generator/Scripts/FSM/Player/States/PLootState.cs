using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLootState : StateBase
{
    public PLootState(Player _player) : base(_player)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        animator.SetTrigger("Pick");
    }
}
