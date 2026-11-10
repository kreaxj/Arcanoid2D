using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxBallBonus : BonusBase
{
    protected override void Awake()
    {
        base.Awake();
        bonusText = "+10";
        backgroundColor = Color.blue;
        textColor = Color.white;
    }

    public override void BonusActivate(PlayerScript player)
    {
        player.gameData.balls += 10;
        player.CreateBalls(10, false);

        Destroy(gameObject);
    }
}
