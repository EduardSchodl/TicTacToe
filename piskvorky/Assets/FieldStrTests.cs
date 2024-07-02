using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldStrTests : MonoBehaviour, IUnitTestCollection
{
    [SerializeField]
    private bool testDisabled;

    public IUnitTest[] GetUnitTests()
    {
        return new IUnitTest[]
        {
            LengthTests
        };
    }

    public bool IsTestDisabled()
    {
        return testDisabled;
    }

    public IEnumerable<TestAssert> LengthTests()
    {
        // Old length without | | |
        yield return RunTestForLength(1, false);
        yield return RunTestForLength(2, false);
        yield return RunTestForLength(4, false);
        yield return RunTestForLength(9, false);
        yield return RunTestForLength(16, false);
        yield return RunTestForLength(25, false);

        // New lengths with | | |
        yield return RunTestForLength(3, true);
        yield return RunTestForLength(10, true);
        yield return RunTestForLength(21, true);
        yield return RunTestForLength(36, true);
        yield return RunTestForLength(55, true);
    }

    public TestAssert RunTestForLength(int strLength, bool expectedMakesSquare)
    {
        bool makesSquare = MathUtil.FieldStrMakesSquare(strLength).makesSquare;
        return new CmpTestAssert(makesSquare, expectedMakesSquare);
    }
}
