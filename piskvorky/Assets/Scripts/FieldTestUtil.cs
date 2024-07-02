using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldTestUtil
{
    public static TestAssert UseExpectedMoveTest(Field f, Player movePlayer, Vector2Int expectedMove, int searchDepth, int piecesNeededToWin)
    {
        GameSetupValues.INSTANCE.ServerSetPiecesNeededToWin(piecesNeededToWin);

        MinMaxFieldNode root = new MinMaxFieldNode(null, f, movePlayer);

        Vector2Int bestMove = root.GetBestTreeMoveDepth(searchDepth);

        return new CmpTestAssert(bestMove, expectedMove);
    }

}
