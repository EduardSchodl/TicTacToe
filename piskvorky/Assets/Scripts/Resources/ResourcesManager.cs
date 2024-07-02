using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : MonoBehaviour
{

    private static ResourcesManager instance;

    public static ResourcesManager INSTANCE
    {
        get
        {
            return instance;
        }
        set
        {
            Assertions.AssertNull(instance);
            instance = value;
        }
    }

    private Sprite[] tilesSprites;

    private Sprite[] digitSprites;

    public Sprite GetDigitSpriteByNumber(int digit)
    {
        return digitSprites[digit];
    }

    public Sprite GetTileSpriteByDirection(Directions direction)
    {
        foreach(Sprite tileSprite in tilesSprites)
        {
            if(!IsTileSpriteEligibleByDir(tileSprite, direction))
            {
                continue;
            }

            return tileSprite;
        }

        Assertions.AssertStatementNotReached();
        return null;
    }

    public bool IsTileSpriteEligibleByDir(Sprite tileSprite, Directions direction)
    {
        string tileSpriteName = tileSprite.name;
        int indexOf_ = tileSpriteName.IndexOf('_');
        string dirPart = tileSpriteName.Substring(indexOf_ + 1);


        string directionPrefix = DirectionsUtil.GetDirectionPrefix(direction);
        return dirPart == directionPrefix;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        INSTANCE = this;
        tilesSprites = Resources.LoadAll<Sprite>("Sprites/PlayingField/Field/FieldSheet");
        digitSprites = Resources.LoadAll<Sprite>("Sprites/Numbers/digits");
    }
}
