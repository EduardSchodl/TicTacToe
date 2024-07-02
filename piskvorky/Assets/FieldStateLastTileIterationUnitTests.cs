using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldStateLastTileIterationUnitTests : MonoBehaviour, IUnitTestCollection
{

    [SerializeField]
    private bool testDisabled;

    public IUnitTest[] GetUnitTests()
    {
        return new IUnitTest[]
        {
            Last1IterationTest
        };
    }

    public bool IsTestDisabled()
    {
        return testDisabled;
    }

    public IEnumerable<TestAssert> Last1IterationTest()
    {
        const int SEARCH_RADIUS = 1;

        Field f = new Field("| | | | | |" +
                            "| | | | | |" +
                            "| | | | | |" +
                            "| | | | | |" +
                            "| | | | | |"
                            );
        /*
            | | | | | |
            | | | | | |
            | | |o| | |
            | | | | | |
            | | | | | |
        */
        f.SetPlayerForTile(new Vector2Int(2, 2), Player.CIRCLE);

        List<Vector2Int> fetchedTiles = f.IterateEmptyTilesAroundLastManipulatedTilesCollectPositions(1, SEARCH_RADIUS).ToList();

        yield return CreateContainsAssert(fetchedTiles, new Vector2Int[] {
            new Vector2Int(1, 1),
            new Vector2Int(2, 1),
            new Vector2Int(3, 1),
            new Vector2Int(1, 2),
            new Vector2Int(3, 2),
            new Vector2Int(1, 3),
            new Vector2Int(2, 3),
            new Vector2Int(3, 3),
        });

        /*
            | | | | |x|
            | | | | | |
            | | |o| | |
            | | | | | |
            | | | | | |
        */
        f.SetPlayerForTile(new Vector2Int(4, 4), Player.CROSS);


        List<Vector2Int> fetchedTiles2 = f.IterateEmptyTilesAroundLastManipulatedTilesCollectPositions(1, SEARCH_RADIUS).ToList();

        yield return CreateContainsAssert(fetchedTiles2, new Vector2Int[] {
            new Vector2Int(3, 4),
            new Vector2Int(3, 3),
            new Vector2Int(4, 3),
        });

        
        List<Vector2Int> fetchedTiles2_2 = f.IterateEmptyTilesAroundLastManipulatedTilesCollectPositions(2, SEARCH_RADIUS).ToList();

        yield return CreateContainsAssert(fetchedTiles2_2, new Vector2Int[] {
            new Vector2Int(1, 1),
            new Vector2Int(2, 1),
            new Vector2Int(3, 1),
            new Vector2Int(1, 2),
            new Vector2Int(3, 2),
            new Vector2Int(1, 3),
            new Vector2Int(2, 3),
            new Vector2Int(3, 3),
            new Vector2Int(3, 4),
            new Vector2Int(4, 3),
        });

        /*
            | | | | |x|
            | | | | | |
            | | |o| | |
            | |o| | | |
            | | | | | |
        */
        f.SetPlayerForTile(new Vector2Int(1, 1), Player.CROSS);

        List<Vector2Int> fetchedTiles3 = f.IterateEmptyTilesAroundLastManipulatedTilesCollectPositions(1, SEARCH_RADIUS).ToList();

        // Fetch only from the last one
        yield return CreateContainsAssert(fetchedTiles3, new Vector2Int[] {
            new Vector2Int(0, 0),
            new Vector2Int(1, 0),
            new Vector2Int(2, 0),
            new Vector2Int(0, 1),
            new Vector2Int(2, 1),
            new Vector2Int(0, 2),
            new Vector2Int(1, 2),
        });

        List<Vector2Int> fetchedTiles3_1 = f.IterateEmptyTilesAroundLastManipulatedTilesCollectPositions(2, SEARCH_RADIUS).ToList();

        yield return CreateContainsAssert(fetchedTiles3_1, new Vector2Int[] {
            new Vector2Int(0, 0),
            new Vector2Int(1, 0),
            new Vector2Int(2, 0),
            new Vector2Int(0, 1),
            new Vector2Int(2, 1),
            new Vector2Int(0, 2),
            new Vector2Int(1, 2),
            new Vector2Int(3, 4),
            new Vector2Int(3, 3),
            new Vector2Int(4, 3),
        });

        List<Vector2Int> fetchedTiles3_2 = f.IterateEmptyTilesAroundLastManipulatedTilesCollectPositions(3, SEARCH_RADIUS).ToList();

        /*
            | | | | |x|
            | |w|w| | |
            | | |o|w| |
            | |o| |w| |
            | | | | | |
        */
        yield return CreateContainsAssert(fetchedTiles3_2, new Vector2Int[] {
            new Vector2Int(0, 0),
            new Vector2Int(1, 0),
            new Vector2Int(2, 0),
            new Vector2Int(0, 1),
            new Vector2Int(2, 1),
            new Vector2Int(0, 2),
            new Vector2Int(1, 2),
            new Vector2Int(3, 4),
            new Vector2Int(3, 3),
            new Vector2Int(4, 3),
            new Vector2Int(1, 3),
            new Vector2Int(2, 3),
            new Vector2Int(3, 1),
            new Vector2Int(3, 2),
        });


    }

    public static EqualContainsAssert<Vector2Int> CreateContainsAssert(List<Vector2Int> fetchedTiles, Vector2Int[] expectedTiles)
    {
        return new EqualContainsAssert<Vector2Int>(fetchedTiles, expectedTiles, true);

    }

    public static IEnumerable<TestAssert> TestLastTileIteration(Field field)
    {
        yield return new CmpTestAssert(true, true);
    }

}
