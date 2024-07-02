using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreakUnitTests : MonoBehaviour, IUnitTestCollection
{
    [SerializeField]
    private bool isTestDisabled;

    public IUnitTest[] GetUnitTests()
    {
        return new IUnitTest[]
        {
            Basic3CircleTest,
            //_3FiveTest,
            //BlockTest
        };
    }

    public bool IsTestDisabled()
    {
        return isTestDisabled;
    }

    public IEnumerable<TestAssert> Basic3CircleTest()
    {
        // | | |o|
        // | |o| |
        // |o| | |
        TileStreak t1 = TileStreakCtor.CreateTileStreakFromBatch(Player.CIRCLE, StreakStatus.BLOCKED, new Vector2Int[] {
            new Vector2Int(0, 0),
            new Vector2Int(1, 1),
            new Vector2Int(2, 2),
        });

        // | | |o|
        // | | |o|
        // | | |o|
        TileStreak t2 = TileStreakCtor.CreateTileStreakFromBatch(Player.CIRCLE, StreakStatus.BLOCKED, new Vector2Int[] {
            new Vector2Int(2, 2),
            new Vector2Int(2, 1),
            new Vector2Int(2, 0),
        });

        // | | | |
        // | |o| |
        // | | |o|
        
        TileStreak t3 = TileStreakCtor.CreateTileStreakFromBatch(Player.CIRCLE, StreakStatus.BLOCKED_ONE, new Vector2Int[]
        {
            new Vector2Int(1, 1),
            new Vector2Int(2, 0)
        });

        // | | | |
        // | |o|o|
        // | | | |
        TileStreak t4 = TileStreakCtor.CreateTileStreakFromBatch(Player.CIRCLE, StreakStatus.BLOCKED_ONE, new Vector2Int[]
        {
            new Vector2Int(1, 1),
            new Vector2Int(2, 1)
        });


        Field f = new Field("| | |o|" +
                            "| |o|o|" +
                            "|o| |o|");

        return RunStreakTest(3, f, new TileStreak[] {
            t1,
            t2,
            t3,
            t4
        });
    }

    public IEnumerable<TestAssert> _3FiveTest()
    {
        Field f = new Field("| | | | | |" +
                            "| | |o| | |" +
                            "| |x|o|x| |" +
                            "| | |o| | |" +
                            "| | | | | |");

        TileStreak t1 = TileStreakCtor.CreateTileStreakFromBatch(Player.CIRCLE, StreakStatus.FREE, new Vector2Int[]
        {
            new Vector2Int(2, 1),
            new Vector2Int(2, 2),
            new Vector2Int(2, 3),
        });

        return RunStreakTest(5, f, new TileStreak[]
        {
            t1
        });
    }

    public IEnumerable<TestAssert> BlockTest()
    {
        Field f = new Field(
                            "| | | | | |" +
                            "| | |o| | |" +
                            "| |x|x| | |" +
                            "| | |o| | |" +
                            "| | | | | |"
                            );
        TileStreak t1 = TileStreakCtor.CreateTileStreakFromBatch(Player.CROSS, StreakStatus.FREE, new Vector2Int[]
        {
            new Vector2Int(1, 2),
            new Vector2Int(2, 2),
        });

        return RunStreakTest(5, f, new TileStreak[]
        {
            t1
        });
    }


    private static IEnumerable<TestAssert> RunStreakTest(int piecesNeededToWin, Field f, TileStreak[] EXPECTED_STREAKS)
    {
        GameSetupValues.INSTANCE.ServerSetPiecesNeededToWin(piecesNeededToWin);
        StreakCollection streaks = EndChecker.CollectAllStreaks(f);

        EqualContainsAssert<TileStreak> containsAssert = new EqualContainsAssert<TileStreak>(streaks.GetStreaks(), EXPECTED_STREAKS, true);
        yield return containsAssert;
    }

}
