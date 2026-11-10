using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
enum BlockType
{
    Green,
    Blue,
    Red,
    Yellow,
    MovingYellow
}

public class BlockScript : MonoBehaviour
{
    [SerializeField] private BlockType _blockType;
    [SerializeField] private BonusScript _bonusScript;
    [SerializeField] private GameDataScript _gameData;
    public GameObject textObject;
    Text textComponent;
    public int hitsToDestroy;
    public int points;

    PlayerScript playerScript;

    protected virtual void Start()
    {
        if (textObject != null)
        {
            textComponent = textObject.GetComponent<Text>();
            textComponent.text = hitsToDestroy.ToString();
        }

        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            hitsToDestroy--;
            if (hitsToDestroy == 0)
            {
                BeforeDestroy();

                Destroy(gameObject);
                playerScript.BlockDestroyed(points);
            }
            else if (textComponent != null)
                textComponent.text = hitsToDestroy.ToString();
        }
    }

    private void BeforeDestroy()
    {
        if (_blockType != BlockType.Green) return;

        GameObject bonusObj = Instantiate(_bonusScript.gameObject, transform.position, Quaternion.identity);

        BonusKind kind = _gameData.GetRandomBonusKind();

        switch (kind)
        {
            case BonusKind.Slow: bonusObj.AddComponent<SlowBonus>(); break;
            case BonusKind.Fast: bonusObj.AddComponent<FastBonus>(); break;
            case BonusKind.Plus2: bonusObj.AddComponent<MiniBallBonus>(); break;
            case BonusKind.Plus10: bonusObj.AddComponent<MaxBallBonus>(); break;
        }
    }
}