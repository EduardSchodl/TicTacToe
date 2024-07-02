using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LinkedListUtil
{

    public static IEnumerable<LinkedListNode<T>> Nodes<T>(this LinkedList<T> list)
    {
        for(LinkedListNode<T> node = list.First; node != null; node = node.Next)
        {
            yield return node;
        }
    }

}
