public class AiDifficultyValues
{
    private int lastPlacedTileSearchLimit;
    public int LastPlacedTileSearchLimit => lastPlacedTileSearchLimit;


    /// <summary>
    /// The tree search depth should never be lower than 2 in order
    /// to minimize senseless moves. This is because search depth of 1
    /// would create only maximizing children, therefore the ai at this depth
    /// would be able to play moves that guarantee wins, but blocking enemy wins
    /// becomes basically impossible as there are no minimizing nodes for the player's move.
    /// 
    /// The actual used tree move search depth. Should be a multiple
    /// of 2.
    /// </summary>
    private int treeMoveSearchDepth;
    public int TreeMoveSearchDepth => treeMoveSearchDepth;

    private double chanceToDoBestMove;
    public double ChanceToDoBestMove => chanceToDoBestMove;

    private int worseMoveDepthRange;
    public int WorseMoveDepthRange => worseMoveDepthRange;

    public AiDifficultyValues(int lastPlacedTileSearchLimit,
        int treeMoveSearchDepth,
        double chanceToDoBestMove,
        int worseMoveDepthRange)
    {
        this.lastPlacedTileSearchLimit = lastPlacedTileSearchLimit;
        this.treeMoveSearchDepth = treeMoveSearchDepth;
        this.chanceToDoBestMove = chanceToDoBestMove;
        this.worseMoveDepthRange = worseMoveDepthRange;
    }




}
