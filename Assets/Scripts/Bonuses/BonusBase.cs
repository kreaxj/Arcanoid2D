using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusBase : MonoBehaviour
{
    [Header("Visuals")]
    public string bonusText = "+100";
    public int points = 100;
    public Color backgroundColor = Color.yellow;
    public Color textColor = Color.black;

    protected Text textComponent;
    protected SpriteRenderer spriteRenderer;

    protected virtual void Awake()
    {
        textComponent = GetComponentInChildren<Text>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        ApplyVisuals();
    }

    protected void ApplyVisuals()
    {
        spriteRenderer.color = backgroundColor;

        textComponent.text = bonusText;
        textComponent.color = textColor;
    }

    public virtual void BonusActivate(PlayerScript player)
    {
        Destroy(gameObject);
    }
}