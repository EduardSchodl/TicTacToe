using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldEvaluatorTests : MonoBehaviour, IUnitTestCollection
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
        return true;
    }

    private IEnumerable<TestAssert> RunEvaluatorTest(Field field, Player observerPlayer, int expectedValue)
    {
        int evaluationValue = FieldEvaluator.EvaluateFieldValue(field, observerPlayer);
        yield return new CmpTestAssert(evaluationValue, expectedValue);
    }

}
