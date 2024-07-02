using System.Collections;
using UnityEngine;

public class FieldTile : MonoBehaviour
{
    private SpriteRenderer fieldSpriteRenderer;

    [SerializeField]
    private Sprite sprite;

    [SerializeField]
    private SpriteRenderer playerIndicatorSpriteRenderer;

    [SerializeField]
    private SpriteRenderer playerIndicatorHaloRenderer;

    private Player? indicatorShownPlayer = null;
    private StaticPlayerInfoSO IndicatorShownPlayerStaticInfoSO => StaticPlayerInfo.INSTANCE.GetPlayerInfoByID(indicatorShownPlayer.Value);

    private void Awake()
    {
        fieldSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateTileSprite(Vector2 tilePosition, Vector2 playingFieldDimensions)
    {
        Directions directionsToHide = Directions.NONE;

        directionsToHide |= tilePosition.x == 0 ? Directions.WEST : Directions.NONE;
        directionsToHide |= tilePosition.y == 0 ? Directions.SOUTH : Directions.NONE;
        directionsToHide |= tilePosition.x == playingFieldDimensions.x - 1 ? Directions.EAST : Directions.NONE;
        directionsToHide |= tilePosition.y == playingFieldDimensions.y - 1 ? Directions.NORTH : Directions.NONE;

        // No need to fetch any sprite, keep the original
        // one in the prefab.
        if(directionsToHide == Directions.NONE)
        {
            return;
        }


        Sprite newSprite = ResourcesManager.INSTANCE.GetTileSpriteByDirection(directionsToHide);
        fieldSpriteRenderer.sprite = newSprite;
    }

    /// <summary>
    /// Sets this tile's sprite to match the sprite of the player passed in the parameter.
    /// </summary>
    public void SetIndicatorToPlayer(Player? turnPlayer)
    {
        indicatorShownPlayer = turnPlayer;
        UpdateSpriteToIndicator();
    }

    public void UpdateSpriteToIndicator()
    {
        if (!indicatorShownPlayer.HasValue)
        {
            playerIndicatorSpriteRenderer.sprite = null;
            return;
        }

        StaticPlayerInfoSO playerInfo = IndicatorShownPlayerStaticInfoSO;
        playerIndicatorSpriteRenderer.sprite = playerInfo.PlayerFieldIndicator;
    }

    public void TurnOnTileHalo()
    {
        if(!indicatorShownPlayer.HasValue)
        {
            Debug.LogError("Tried to turn on the halo on a tile without a player indicator.");
            return;
        }

        playerIndicatorHaloRenderer.sprite = IndicatorShownPlayerStaticInfoSO.PlayerFieldHaloSprite;
    }

    public void TurnOffTileHalo()
    {
        playerIndicatorHaloRenderer.sprite = null;
    }

}
