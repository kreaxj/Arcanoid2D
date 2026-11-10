using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBallBonus : BonusBase
{
    protected override void Awake()
    {
        base.Awake();
        bonusText = "+2";
        backgroundColor = Color.blue;
        textColor = Color.white;
    }

    public override void BonusActivate(PlayerScript player)
    {
        player.gameData.balls += 2;

        Destroy(gameObject);
    }
}
