using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSprite : MonoBehaviour
{
    List<SpriteRenderer> rends;
    public List<Sprite> sprites;

    void Start()
    {
        if(rends == null)    rends = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
        foreach(SpriteRenderer rend in rends)
            SelectSprite(rend);
    }

    void SelectSprite(SpriteRenderer rend)
    {
        if(sprites.Count<=0){
            Debug.LogWarning("Zero Sprites to select a random from, not randoming any sprite...");
            return;
        }
        int index = Random.Range(0, sprites.Count-1);
        rend.sprite = sprites[index];
    }
}
