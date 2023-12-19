using System.Collections;
using UnityEngine;

public class EggletAnimationController : CharacterAnimationController
{
    public override void Init(CharacterAnimationMgr mgr)
    {
        base.Init(mgr);
        print("Do special init operation.");
    }
}
