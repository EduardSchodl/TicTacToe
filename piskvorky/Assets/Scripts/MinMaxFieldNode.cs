using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = System.Random;

public class MinMaxFieldNode : MinMaxNode
{

    public static readonly Stopwatch AI_EVALUATION_STOPWATCH = new Stopwatch();

    // Old ai configuration
    private const int SEARCH_RADIUS_AROUND_EXISTING_AI = 1;
    private const int SEARCH_RADIUS_AROUND_EXISTING_PLAYER = 2;

    private const int SEARCH_RADIUS_AROUND_LAST_TILES = 1;

    // New ai configuration
    private static bool USE_NEW_LAST_PLACED_AI => true;
    private const int LAST_PLACED_TILE_SEARCH_RADIUS = 1;


    /// <summary>
    /// The state of the <see cref="Field"/> associated with this <see cref="MinMaxFieldNode"/>.
    /// </summary>
    public Field FieldState
    {
        get;
        private set;
    }

    /// <summary>
    /// The player whose turn it was that resulted in the creation of this <see cref="MinMaxFieldNode"/>.
    /// </summary>
    public Player StatePlacingPlayer
    {
        get;
        private set;
    }

    /// <summary>
    /// 
    /// TODO: !! THIS TILE IS NOT SET AT THE ROOT !!
    /// 
    /// This tile represents the tile under which this <see cref="MinMaxFieldNode"/> node was created.
    /// This is the last placed tile by either the player or the ai.
    /// The root node starts at the end of the player's turn, right when the player
    /// has placed his tile.
    /// At the children of the root node, the state place location is set to one of the moves
    /// that the ai can make (therefore <see cref="StatePlaceLocation"/> at that node will represent
    /// the last ai move).
    /// </summary>
    public Vector2Int StatePlaceLocation
    {
        get;
        private set;
    }

    /// <summary>
    /// DEPRECATED CONSTRUCTOR, USE CREATE ROOT NODE
    /// </summary>
    public MinMaxFieldNode(MinMaxFieldNode parent, Field fieldState, Player placingPlayer, int depth = 0) : base(parent, depth)
    {
        StatePlacingPlayer = placingPlayer;

        // Copy field
        Field copiedField = fieldState.CopyField();
        FieldState = copiedField;

    }

    public static MinMaxFieldNode CreateRootNode(Field fieldState, Player playerWhoPlayed)
    {
        Field copiedField = fieldState.CopyField();

        MinMaxFieldNode minMaxFieldNode = new MinMaxFieldNode();
        minMaxFieldNode.FieldState = copiedField;
        
        minMaxFieldNode.StatePlacingPlayer = playerWhoPlayed;
        
        minMaxFieldNode.Depth = 0;

        return minMaxFieldNode;

    }
    
    public MinMaxFieldNode()
    {

    }

    /// <summary>
    /// Sub node constructor
    /// </summary>
    public MinMaxFieldNode(MinMaxFieldNode parent, Field fieldState, Vector2Int tilePosition, Player placingPlayer, int depth) : this(parent, fieldState, placingPlayer, depth)
    {
        StatePlaceLocation = tilePosition;
        FieldState.SetPlayerForTile(tilePosition, StatePlacingPlayer);
    }

    public Vector2Int GetBestTreeMove()
    {
        int searchDepth = AiDifficultyIntoValues.GetAiDifficultyFromSelected().TreeMoveSearchDepth;
        return GetBestTreeMoveDepth(searchDepth);
    }

    public Vector2Int GetBestTreeMoveDepth(int maxDepth)
    {
        if (HasParent())
        {
            Debug.LogError($"{nameof(GetBestTreeMove)} is not being called on the root node. This cannot happen.");
            return Vector2Int.zero;
        }

        // Alpha beta is initialized at
        // α = -∞
        // β = ∞
        var result = AlphaDepthEvalNodeAndValue(maxDepth);

        List<(MinMaxNode node, int nodeValue)> sortedNodes = result.rootProcessedNodes.OrderByDescending((val) => val.nodeValue).ToList();

        AiDifficultyValues difficultyValues = AiDifficultyIntoValues.GetAiDifficultyFromSelected();
        double chanceToDoBestMove = difficultyValues.ChanceToDoBestMove;

        Random rng = new Random();

        Debug.Log("sortedNodes.count " + sortedNodes.Count);

        // Best move
        if (sortedNodes.Count == 1 || rng.NextDouble() < chanceToDoBestMove)
        {
            Debug.Log("using best");
            MinMaxFieldNode pickedFieldNode = result.pickedNode as MinMaxFieldNode;
            return pickedFieldNode.StatePlaceLocation;
        }

        Debug.Log("using worse");
        int depthRange = difficultyValues.WorseMoveDepthRange;

        int rangeStart = 1;
        int rangeEnd = System.Math.Min(rangeStart + depthRange, sortedNodes.Count);

        int pickedIndex = rng.Next(rangeStart, rangeEnd + 1);

        Debug.Log("picking " + pickedIndex);
        return (sortedNodes[pickedIndex].node as MinMaxFieldNode).StatePlaceLocation;
    }

    /*
    // Unused debugging method, if you need this, you are going
    // to have to write a manual ChildNode population method
    public MinMaxFieldNode PickChildByField(Field field)
    {
        MinMaxNode pickedNode = ChildNodes.Nodes().FirstOrDefault((childNode) => {
            MinMaxNode node = childNode.Value;
            MinMaxFieldNode fieldNode = node as MinMaxFieldNode;
            return fieldNode.FieldState.DataEquals(field);
            }).Value;
        return pickedNode as MinMaxFieldNode;
    }
    */

    /// <summary>
    /// Returns the value of this <see cref="MinMaxFieldNode"/>.
    /// The value is computed from the game state of this node (<see cref="EndChecker.CheckGameEnded(Field)"/>)
    /// if the node has no children (i.e. if the node is a terminal node). If the node
    /// is not a terminal node, then the value will be taken from one of its children, as decided
    /// by <see cref="PickChildNodeWithCorrectValue(int, int)"/>.
    /// 
    /// Terminal/Childless node => computes and returns its value
    /// Node with children => takes the value out of one of its children
    /// 
    /// </summary>
    protected override int RunTerminalNodeEvaluation()
    {
        return RunAdvancedTerminalNodeEvaluation();
    }

    private int RunAdvancedTerminalNodeEvaluation()
    {
        Player evalPlayer = GetEvalutationPlayerFromParent();

        AI_EVALUATION_STOPWATCH.Start();
        int evaluatedValue = FieldEvaluator.EvaluateFieldValue(FieldState, evalPlayer);
        AI_EVALUATION_STOPWATCH.Stop();

        return evaluatedValue;
    }

    /// <summary>
    /// Return the evalutation player which this node tree
    /// is computing for. Example: The O player has played his move
    /// and now it's the AI's turn, therefore the node tree
    /// is evaluating the best move for the X player, which will be returned
    /// by this method.
    /// </summary>
    public Player GetEvalutationPlayerFromParent()
    {
        // Explanation: root's state uses the state player as the player who has just
        // done their turn - therefore we are evaluating for the next one
        // and not the one who has just placed his mark
        return GetStatePlayerFromParent().GetNextCircular();
    }

    /// <summary>
    /// Returns the player found at the parent's state. In reality thils will
    /// be the player who just played his move (not the one we are evaluating the tree for, but the
    /// one before), since the root state will have
    /// the <see cref="Field"/> into which the player just placed his mark (<see cref="StatePlacingPlayer"/>, <see cref="StatePlaceLocation"/>)
    /// from which the tree is computing the evaluation player's moves.
    /// </summary>
    public Player GetStatePlayerFromParent()
    {
        // We arrived at root node
        if (!HasParent())
        {
            return StatePlacingPlayer;
        }

        return GetParentTyped<MinMaxFieldNode>().GetStatePlayerFromParent();
    }

    public static int CREATED_CHILDREN = 0;

    /// <summary>
    /// Returns the tiles on which new <see cref="MinMaxFieldNode"/>s will be created.
    /// These nodes are direct children from this node.
    /// Without any limitation, each empty tile represents one branch from this <see cref="MinMaxFieldNode"/>.
    /// Practically speaking, without any limitations, the ai will consider all possible moves for the player and the ai.
    /// With limitations (for optimization purposes), the ai will only consider certain tiles, improving computation
    /// times, but reducing the ai's mark pick quality.
    /// </summary>
    public IEnumerable<KeyValuePair<Vector2Int, FieldTileData>> PickTilesToCreateChildrenFrom()
    {
        if(USE_NEW_LAST_PLACED_AI)
        {
            return PickTilesToCreateChildrenFromOptimized();
        }


        // maximizing = ai's turn
        bool maximizing = IsMaximizing();

        if(maximizing)
        {
            // Ai's turn
            return FieldState.IterateEmptyTilesAroundFilledTiles(SEARCH_RADIUS_AROUND_EXISTING_AI);
        }
        else
        {
            // Player's turn
            return FieldState.IterateEmptyTilesAroundFilledTiles(SEARCH_RADIUS_AROUND_EXISTING_PLAYER);
        }


    }

    /*
    public KeyValuePair<Vector2Int, FieldTileData> GetStatePlaceLocationWithData()
    {
        KeyValuePair<Vector2Int, FieldTileData> statePlaceLocationWithData = FieldState.GetTileAsKeyValuePair(StatePlaceLocation);
        return statePlaceLocationWithData;
    }

    public IEnumerable<KeyValuePair<Vector2Int, FieldTileData>> IterateEmptyTilesAroundStatePlaceLocation(HashSet<Vector2Int> processedTiles)
    {
        // Get the tile data as a key value pair for the last placed tile (placed by either the player or the ai),
        // which is the same as the StatePlaceLocation.
        KeyValuePair<Vector2Int, FieldTileData> statePlaceLocationWithData = GetStatePlaceLocationWithData();

        IEnumerable<KeyValuePair<Vector2Int, FieldTileData>>
            lastTileSurroundings = FieldState.IterateEmptyTilesAroundTile(statePlaceLocationWithData, processedTiles, SEARCH_RADIUS_AROUND_LAST_TILES);

        return lastTileSurroundings;
    }
    */

    public IEnumerable<KeyValuePair<Vector2Int, FieldTileData>> PickTilesToCreateChildrenFromOptimized()
    {
        int tileSearchLimit = AiDifficultyIntoValues.GetAiDifficultyFromSelected().LastPlacedTileSearchLimit;

        // First get the empty tiles around the last tile / StateLocation tile
        IEnumerable<KeyValuePair<Vector2Int, FieldTileData>> tilesAroundLastPlacedTiles =
            FieldState.IterateEmptyTilesAroundLastManipulatedTiles(tileSearchLimit, LAST_PLACED_TILE_SEARCH_RADIUS);

        return tilesAroundLastPlacedTiles;

    }


    public override IEnumerable<MinMaxNode> GetNextChildNode()
    {
        AI_EVALUATION_STOPWATCH.Start();
        // No need to go further down the tree
        // if the game has ended.
        // This check is absolutely necessary as the tree would grow
        // beyond reachable game states into nonsense
        if (EndChecker.CheckGameEnded(FieldState).gameEnded)
        {
            yield break;
        }
        AI_EVALUATION_STOPWATCH.Stop();

        // The AI's moves are limited to be put only around
        // filed tiles. The minimizing player's moves are
        // not restricted as he can decide to place them anywhere.
        IEnumerable<KeyValuePair<Vector2Int, FieldTileData>> iteratedTiles = PickTilesToCreateChildrenFrom();

        foreach (KeyValuePair<Vector2Int, FieldTileData> emptyTile in iteratedTiles)
        {
            MinMaxFieldNode childNode = CreateChildNode(emptyTile.Key);

            ++CREATED_CHILDREN;
            yield return childNode;
        }


    }

    public MinMaxFieldNode CreateChildNode(Vector2Int emptyTilePosition)
    {
        return new MinMaxFieldNode(this, FieldState, emptyTilePosition, StatePlacingPlayer.GetNextCircular(), Depth + 1);
    }

    public override string ToString()
    {
        return $"field: {FieldState}";
    }

}
