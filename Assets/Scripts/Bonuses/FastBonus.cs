using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastBonus : BonusBase
{
    protected override void Awake()
    {
        base.Awake();
        bonusText = "Fast";
        backgroundColor = Color.red;
        textColor = Color.white;
    }

    public override void BonusActivate(PlayerScript player)
    {
        foreach (var ball in FindObjectsByType<BallScript>(FindObjectsSortMode.None))
            ball.ApplySpeed(1.9f);

        Destroy(gameObject);
    }
}
