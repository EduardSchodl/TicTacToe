using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TestAssert
{
    public abstract (bool assertSucceded, string failMessage) TestAssertMethod();
}
