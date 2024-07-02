using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTests : MonoBehaviour
{
    [SerializeField]
    private bool launchUnitTests;

    private void Start()
    {
        if(!launchUnitTests)
        {
            return;
        }

        Debug.Log("Launching unit tests.");
        LaunchUnitTests();
        Debug.Log("All unit tests finished.");
    }

    private void LaunchUnitTests()
    {
        IUnitTestCollection[] allCollections = GetComponents<IUnitTestCollection>();
        RunTestsInCollections(allCollections);
    }

    private static void RunTestsInCollections(IUnitTestCollection[] allCollections)
    {
        foreach (IUnitTestCollection collection in allCollections)
        {
            if(collection.IsTestDisabled())
            {
                continue;
            }

            collection.RunCollectionTests();
        }
    }

}
