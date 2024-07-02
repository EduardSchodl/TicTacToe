using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StaticPlayerInfo", menuName = "ScriptableObjects/Players/StaticPlayerInfo", order = 1)]
public class StaticPlayerInfoSO : ScriptableObject
{
    [SerializeField]
    private Player playerID;
    public Player PlayerID => playerID;

    [SerializeField]
    private Sprite playerFieldIndicator;
    public Sprite PlayerFieldIndicator => playerFieldIndicator;

    [SerializeField]
    private Sprite playerFieldHaloSprite;
    public Sprite PlayerFieldHaloSprite => playerFieldHaloSprite;

    [SerializeField]
    private string stringPlayerRepresentation;
    public string StringPlayerRepresentation => stringPlayerRepresentation;

    [SerializeField]
    private Color playerColor;

    public Color PlayerColor => playerColor;

}
