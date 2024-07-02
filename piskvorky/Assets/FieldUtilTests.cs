using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

[SerializeField]
public class FieldUtilTests : MonoBehaviour, IUnitTestCollection
{
    [SerializeField]
    private bool isDisabled;
    public IUnitTest[] GetUnitTests()
    {
        return new IUnitTest[]
        {
            _3x3Test,
            RightCornerTestFieldified,
            MultipleTileFieldTest,
            _4x4FieldTest,
            FilledFieldTest,
            EmptyFieldTest,
            LeftTest
        };
    }

    public bool IsTestDisabled()
    {
        return isDisabled;
    }

    private IEnumerable<TestAssert> RunTest(Vector2Int centerTile, int radius, int fieldWidth, int fieldHeight, Vector2Int[] expectedTiles)
    {
        List<Vector2Int> collectedTiles = FieldUtil.CollectTilesAroundTileTopLeft(centerTile, radius, fieldWidth, fieldHeight);

        yield return new EqualContainsAssert<Vector2Int>(collectedTiles, expectedTiles, true);
    }

    private IEnumerable<TestAssert> RunFieldTest(Field f, Vector2Int[] expectedCollectedTiles)
    {
        KeyValuePair<Vector2Int, FieldTileData>[] collectedTiles = f.CollectTilesAroundFilledTiles();
        Vector2Int[] tilePositions = KeyValuePairUtil.PairArrayIntoKeyArray(collectedTiles);

        List<Vector2Int> tilePositionsList = new List<Vector2Int>();
        tilePositionsList.AddRange(tilePositions);

        yield return new EqualContainsAssert<Vector2Int>(tilePositionsList, expectedCollectedTiles, true);
    }

    private IEnumerable<TestAssert> _3x3Test()
    {
        return RunTest(new Vector2Int(1, 1), 2, 2, 2, new Vector2Int[] {
            new Vector2Int(0, 2),
            new Vector2Int(1, 2),
            new Vector2Int(2, 2),
            new Vector2Int(0, 1),
            new Vector2Int(2, 1),
            new Vector2Int(0, 0),
            new Vector2Int(1, 0),
            new Vector2Int(2, 0),
        });
    }

    private IEnumerable<TestAssert> RightCornerTestFieldified()
    {
        Field f = new Field("| | |o|" +
                            "| | | |" +
                            "| | | |");
        return RunFieldTest(f, new Vector2Int[] {
            new Vector2Int(1, 2),
            new Vector2Int(1, 1),
            new Vector2Int(2, 1),
        });
    }


    private IEnumerable<TestAssert> MultipleTileFieldTest()
    {
        Field f = new Field("| | |x|" +
                            "| |o| |" +
                            "| | | |");

        return RunFieldTest(f, new Vector2Int[] {
            new Vector2Int(0, 0),
            new Vector2Int(1, 0),
            new Vector2Int(2, 0),
            new Vector2Int(0, 1),
            new Vector2Int(2, 1),
            new Vector2Int(0, 2),
            new Vector2Int(1, 2),
        });
    }

    private IEnumerable<TestAssert> _4x4FieldTest()
    {
        Field f = new Field("| | | | |" +
                            "| | | |x|" +
                            "|o| | | |" +
                            "| | | | |");

        return RunFieldTest(f, new Vector2Int[] {
            new Vector2Int(0, 0),
            new Vector2Int(1, 0),
            new Vector2Int(1, 1),
            new Vector2Int(1, 2),
            new Vector2Int(0, 2),

            new Vector2Int(3, 1),
            new Vector2Int(2, 1),
            new Vector2Int(2, 2),
            new Vector2Int(2, 3),
            new Vector2Int(3, 3),
        });
    }

    private IEnumerable<TestAssert> FilledFieldTest()
    {
        Field f = new Field("|x|x|o|" +
                            "|o|o|x|" +
                            "|x|o|o|");

        return RunFieldTest(f, new Vector2Int[] { });
    }

    private IEnumerable<TestAssert> EmptyFieldTest()
    {
        Field f = new Field("| | | |" +
                            "| | | |" +
                            "| | | |");

        return RunFieldTest(f, new Vector2Int[] { });
    }

    private IEnumerable<TestAssert> LeftTest()
    {
        Field f = new Field("|o| | |" +
                            "|x| | |" +
                            "| | | |");

        return RunFieldTest(f, new Vector2Int[] { 
            new Vector2Int(0, 0),
            new Vector2Int(1, 0),
            new Vector2Int(1, 1),
            new Vector2Int(1, 2),
        });
    }

}
