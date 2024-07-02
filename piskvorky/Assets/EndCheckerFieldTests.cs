using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCheckerFieldTests : MonoBehaviour, IUnitTestCollection
{
    [SerializeField]
    private bool disableTest = false;
    public bool IsTestDisabled()
    {
        return disableTest;
    }

    public IUnitTest[] GetUnitTests()
    {
        return new IUnitTest[]
        {
            TestEndCheckerDraws,
            TestEndCheckerUnfinishedGames,
            TestEndCheckerCircleGames,
            TestEndCheckerCrossGames
        };
    }

    public IEnumerable<TestAssert> TestEndCheckerDraws()
    {

        GameSetupValues.INSTANCE.ServerSetPiecesNeededToWin(3);

        Field f1 = new Field("|o|x|x|" +
                             "|x|x|o|" +
                             "|o|o|x|");
        yield return new EndGameAssert(f1, true, null);

        Field f2 = new Field("|x|o|x|" +
                             "|x|x|o|" +
                             "|o|x|o|");
        yield return new EndGameAssert(f2, true, null);

        Field f3 = new Field("|x|x|o|" +
                             "|o|o|x|" +
                             "|x|x|o|");
        yield return new EndGameAssert(f3, true, null);
        
    }

    public IEnumerable<TestAssert> TestEndCheckerUnfinishedGames()
    {
        GameSetupValues.INSTANCE.ServerSetPiecesNeededToWin(3);

        Field f1 = new Field("| | | |" +
                             "| | | |" +
                             "| | | |");
        yield return new EndGameAssert(f1, false, null);

        Field f2 = new Field("| | | |" +
                             "| |x| |" +
                             "| | |o|");
        yield return new EndGameAssert(f2, false, null);

        Field f3 = new Field("| | | |" +
                             "| |x|x|" +
                             "|o| |o|");
        yield return new EndGameAssert(f3, false, null);

        Field f4 = new Field("| |o| |" +
                             "| |x|x|" +
                             "|o|x|o|");
        yield return new EndGameAssert(f4, false, null);
        
        Field f5 = new Field("| | | |" +
                             "| |o| |" +
                             "| | | |");
        yield return new EndGameAssert(f5, false, null);

    }

    public IEnumerable<TestAssert> TestEndCheckerCircleGames()
    {
        GameSetupValues.INSTANCE.ServerSetPiecesNeededToWin(3);

        Field f1 = new Field("| |o|x|" +
                             "|x|o|x|" +
                             "| |o| |");
        yield return new EndGameAssert(f1, true, Player.CIRCLE);

        Field f2 = new Field("| | |o|" +
                             "| |o|x|" +
                             "|o|x|x|");
        yield return new EndGameAssert(f2, true, Player.CIRCLE);

        Field f3 = new Field("|o|o|o|" +
                             "|x|x|o|" +
                             "|x| |x|");
        yield return new EndGameAssert(f3, true, Player.CIRCLE);

    }

    public IEnumerable<TestAssert> TestEndCheckerCrossGames()
    {
        GameSetupValues.INSTANCE.ServerSetPiecesNeededToWin(3);

        Field f1 = new Field("|x|o| |" +
                             "| |x|o|" +
                             "| | |x|");
        yield return new EndGameAssert(f1, true, Player.CROSS);

        Field f2 = new Field("| |o|x|" +
                             "| |x|o|" +
                             "|x|o| |");
        yield return new EndGameAssert(f2, true, Player.CROSS);

        Field f3 = new Field("|x|o| |" +
                             "|x|x|o|" +
                             "|x|o|o|");
        
        yield return new EndGameAssert(f3, true, Player.CROSS);
    }

}
