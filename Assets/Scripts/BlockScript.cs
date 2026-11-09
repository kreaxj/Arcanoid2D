using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockScript : MonoBehaviour
{
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
                print(points);
                Destroy(gameObject);
                playerScript.BlockDestroyed(points);
            }
            else if (textComponent != null)
                textComponent.text = hitsToDestroy.ToString();
        }
    }
}