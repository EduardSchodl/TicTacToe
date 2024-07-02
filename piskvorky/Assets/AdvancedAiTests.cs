using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedAiTests : MonoBehaviour, IUnitTestCollection
{
    [SerializeField]
    private bool testDisabled;

    public IUnitTest[] GetUnitTests()
    {
        return new IUnitTest[]
        {
            
        };
    }

    public bool IsTestDisabled()
    {
        return testDisabled;
    }

    private TestAssert RunAdvancedTest(Field f, Player turnPlayer, int piecesNeededToWin, int maxDepth, Vector2Int expectedMove)
    {
        GameSetupValues.INSTANCE.ServerSetPiecesNeededToWin(piecesNeededToWin);

        MinMaxFieldNode root = new MinMaxFieldNode(null, f, turnPlayer);
        
        Vector2Int bestMove = root.GetBestTreeMoveDepth(maxDepth);

        return new CmpTestAssert(bestMove, expectedMove);
    }

}
