using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaFieldNodeTests : MonoBehaviour, IUnitTestCollection
{
    [SerializeField]
    private bool disableTest = false;
    public bool IsTestDisabled()
    {
        return disableTest;
    }

    public IUnitTest[] GetUnitTests()
    {
        return new IUnitTest[] {
            EasyStopWin,
            EasyStopWin2,
            StopWin1,
            //CENTER_FIELD_TEST_SHALLOW,
            FieldTest2,
            FieldTest3,
        };
    }

    public IEnumerable<TestAssert> EasyStopWin()
    {
        GameSetupValues.INSTANCE.ServerSetPiecesNeededToWin(3);

        Field f = new Field("|x|o| |" +
                            "|x|o|o|" +
                            "|o|x| |");

        MinMaxFieldNode root = new MinMaxFieldNode(null, f, Player.CIRCLE);
        
        Vector2Int bestMove = root.GetBestTreeMove();

        yield return new CmpTestAssert(bestMove, new Vector2Int(2, 2));
    }

    public IEnumerable<TestAssert> EasyStopWin2()
    {
        GameSetupValues.INSTANCE.ServerSetPiecesNeededToWin(3);

        Field f = new Field("| | | |" +
                            "| |x| |" +
                            "|o|o| |");

        MinMaxFieldNode root = new MinMaxFieldNode(null, f, Player.CIRCLE);
        
        Vector2Int bestMove = root.GetBestTreeMove();
        
        yield return new CmpTestAssert(bestMove, new Vector2Int(2, 0));
    }

    public IEnumerable<TestAssert> StopWin1()
    {
        GameSetupValues.INSTANCE.ServerSetPiecesNeededToWin(3);

        Field f = new Field("|x| | |" +
                            "| |o| |" +
                            "|o| | |");

        MinMaxFieldNode root = new MinMaxFieldNode(null, f, Player.CIRCLE);
        

        Vector2Int bestMove = root.GetBestTreeMove();
        
        yield return new CmpTestAssert(bestMove, new Vector2Int(2, 2));
    }

    /// <summary>
    /// Currently unused test. The ai with the shallow depth of 2
    /// is not able to make the guess that placing its mark into the
    /// unoccupied center square will guarantee a draw in the future.
    /// </summary>
    public IEnumerable<TestAssert> CENTER_FIELD_TEST_SHALLOW()
    {
        GameSetupValues.INSTANCE.ServerSetPiecesNeededToWin(3);

        Field f = new Field("|o| | |" + 
                            "| | | |" + 
                            "| | | |");

        MinMaxFieldNode root = new MinMaxFieldNode(null, f, Player.CIRCLE);
        
        Vector2Int bestMove = root.GetBestTreeMove();

        yield return new CmpTestAssert(bestMove, new Vector2Int(1, 1));
    }

    public IEnumerable<TestAssert> FieldTest2()
    {
        GameSetupValues.INSTANCE.ServerSetPiecesNeededToWin(3);

        Field f = new Field("|o| | |" +
                            "|o|x| |" +
                            "| | | |");

        MinMaxFieldNode root = new MinMaxFieldNode(null, f, Player.CIRCLE);
        
        Vector2Int bestMove = root.GetBestTreeMove();

        yield return new CmpTestAssert(bestMove, new Vector2Int(0, 0));
    }

    public IEnumerable<TestAssert> FieldTest3()
    {
        GameSetupValues.INSTANCE.ServerSetPiecesNeededToWin(3);

        Field f = new Field("|o| |o|" +
                            "|o|x| |" +
                            "|x| | |");

        MinMaxFieldNode root = new MinMaxFieldNode(null, f, Player.CIRCLE);
        
        Vector2Int bestMove = root.GetBestTreeMove();

        yield return new CmpTestAssert(bestMove, new Vector2Int(1, 2));
    }

}
