using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class EnumUtil
{
    public static T GetNextCircular<T>(this T src) where T : struct
    {
        return GetCircular(src, false);
    }

    public static T GetPreviousCircular<T>(this T src) where T : struct
    {
        return GetCircular(src, true);
    }

    public static int GetEnumSize<T>() where T : struct
    {
        AssertIsEnum<T>();
        return Enum.GetNames(typeof(T)).Length;
    }


    private static void AssertIsEnum<T>() where T : struct
    {
        if (!typeof(T).IsEnum)
        {
            throw new ArgumentException($"{typeof(T)} is not an enum.");
        }
    }
    
    public static T GetValueByIndex<T>(int index) where T : struct
    {
        AssertIsEnum<T>();
        T[] arr = (T[])Enum.GetValues(typeof(T));

        return arr[index];
    }

    private static T GetCircular<T>(this T src, bool getPrevious) where T : struct
    {
        AssertIsEnum<T>();
        T[] arr = (T[])Enum.GetValues(src.GetType());

        int increment = getPrevious ? -1 : 1;
        int nextIndex = Array.IndexOf(arr, src) + increment;

        if(getPrevious)
        {
            bool fetchingPastFirst = nextIndex == -1;

            // Return last if the current is the first element.
            return fetchingPastFirst ? arr[arr.Length - 1] : arr[nextIndex];
        }


        bool fetchingPastLast = nextIndex == arr.Length;
        // Return first element if current is the last element.
        return fetchingPastLast ? arr[0] : arr[nextIndex];
    }

    /// <summary>
    /// === NOT TESTED ===
    /// </summary>
    public static IEnumerable<T> IterateValues<T>() where T : Enum
    {
        foreach (T val in Enum.GetValues(typeof(T)))
        {
            yield return val;
        }
    }

    public static IEnumerable<Enum> IterateFlags(this Enum @enum)
    {
        foreach (Enum val in Enum.GetValues(@enum.GetType()))
        {
            if(!@enum.HasFlag(val))
            {
                continue;
            }
            yield return val;
        }
    }

}

