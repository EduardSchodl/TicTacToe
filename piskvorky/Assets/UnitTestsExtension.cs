using UnityEngine;

public static class UnitTestsExtension
{
    
    public static void RunCollectionTests(this IUnitTestCollection collection)
    {
        string collectionName = collection.GetType().Name;

        IUnitTest[] tests = collection.GetUnitTests();

        foreach (IUnitTest test in tests)
        {
            string methodName = test.Method.Name;
            int assertionCounter = 0;

            foreach(TestAssert assert in test())
            {
                (bool assertionResult, string failMessage) = assert.TestAssertMethod();
                
                if(!assertionResult)
                {
                    Debug.LogError($"({collectionName}):({methodName}):({assertionCounter}) failed: {failMessage}");
                }

                ++assertionCounter;
            }
        }
    }
}