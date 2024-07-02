using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowDepthAiTests : MonoBehaviour, IUnitTestCollection
{
    [SerializeField]
    private bool isDisabled;

    public IUnitTest[] GetUnitTests()
    {
        return new IUnitTest[]
        {
            LowDepthStop,
        };
    }

    public bool IsTestDisabled()
    {
        return isDisabled;
    }

    public IEnumerable<TestAssert> LowDepthStop()
    {
        Field f = new Field("|o|x| |" +
                            "| |o| |" +
                            "| | | |");
        yield return FieldTestUtil.UseExpectedMoveTest(f, Player.CIRCLE, new Vector2Int(2, 0), 2, 3);
    }

}
