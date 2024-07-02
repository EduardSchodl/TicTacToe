using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CmpTestAssert : TestAssert
{
    private object o1;
    private object o2;
    private const string FAIL_MESSAGE_TEMPLATE = "{0} doesn't equal {1}";

    public CmpTestAssert(object o1, object o2)
    {
        this.o1 = o1;
        this.o2 = o2;
    }

    public override (bool assertSucceded, string failMessage) TestAssertMethod()
    {
        string failMessage = string.Format(FAIL_MESSAGE_TEMPLATE, o1, o2);
        bool assertSucceeded = o1.Equals(o2);
        return (assertSucceeded, failMessage);
    }
}