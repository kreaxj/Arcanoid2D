using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowBonus : BonusBase
{
    protected override void Awake()
    {
        base.Awake();
        bonusText = "Slow";
        backgroundColor = Color.green;
        textColor = Color.white;
    }

    public override void BonusActivate(PlayerScript player)
    {
        foreach (var ball in FindObjectsByType<BallScript>(FindObjectsSortMode.None))
            ball.ApplySpeed(0.2f);

        Destroy(gameObject);
    }
}
