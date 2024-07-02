using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqualContainsAssert<T> : TestAssert
{
    private List<T> list;

    private T[] checkedObjects;

    private string FAIL_MESSAGE = "{0} is not found in the list.";

    private string STRICT_MODE_FAIL = "List's length ({0}) is not the same as expected objects' ({1})";

    private bool strictMode;

    private EqualContainsAssertCustomFunc<T> customEqualsFunction;

    public EqualContainsAssert(List<T> list, T[] checkedObjects) : this(list, checkedObjects, false)
    {
    }

    /// <summary>
    /// UNTESTED FUNCTIONALITY
    /// </summary>
    public static EqualContainsAssert<T> CreateEqualContainsAssertWCustomFunc(List<T> list, T[] checkedObjects, bool strictMode, EqualContainsAssertCustomFunc<T> customFunc)
    {
        EqualContainsAssert<T> assert = new EqualContainsAssert<T>(list, checkedObjects, strictMode);
        assert.customEqualsFunction = customFunc;
        return assert;
    }

    public EqualContainsAssert(List<T> list, T[] checkedObjects, bool strictMode)
    {
        this.list = list;
        this.checkedObjects = checkedObjects;
        this.strictMode = strictMode;
    }

    public override (bool assertSucceded, string failMessage) TestAssertMethod()
    {
        List<T> consumedElements = new List<T>();

        if(strictMode)
        {
            bool failed = list.Count != checkedObjects.Length;
            string failMessage = string.Format(STRICT_MODE_FAIL, list.Count, checkedObjects.Length);
            if(failed)
            {
                return (false, failMessage);
            }
        }

        foreach (T checkedObject in checkedObjects)
        {
            bool assertSucceededForObject = EqualsContainsObject(checkedObject, consumedElements);

            if(assertSucceededForObject)
            {
                continue;
            }

            string failMessage = string.Format(FAIL_MESSAGE, checkedObject);
            return (false, failMessage);
        }

        return (true, null);
    }

    private bool EqualsContainsObject(T @object, List<T> consumedElements)
    {
        foreach (T listElement in list)
        {
            if(consumedElements.Contains(listElement))
            {
                continue;
            }

            if (PerformAssertionComparison(@object, listElement))
            {
                consumedElements.Add(listElement);
                return true;
            }
        }

        return false;
    }

    private bool PerformAssertionComparison(T object1, T object2)
    { 
        if(customEqualsFunction == null)
        {
            return object1.Equals(object2);
        }

        return customEqualsFunction(object1, object2);
    }
}

public delegate bool EqualContainsAssertCustomFunc<T>(T obj1, T obj2);
