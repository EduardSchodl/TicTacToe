using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class KeyValuePairUtil
{
    public static TKey[] PairArrayIntoKeyArray<TKey, TValue>(KeyValuePair<TKey, TValue>[] pairs)
    {
        TKey[] keyArray = new TKey[pairs.Length];

        for(int i = 0; i < pairs.Length; ++i)
        {
            KeyValuePair<TKey, TValue> pair = pairs[i];
            keyArray[i] = pair.Key;
        }

        return keyArray;
    }
}
