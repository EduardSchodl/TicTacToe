using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NetCustomWriters
{
    public static void WritePlayerNullable(this NetworkWriter writer, Player? value)
    {
        bool hasValue = value.HasValue;
        writer.WriteBool(hasValue);


        if(!hasValue)
        {
            return;
        }

        writer.Write(value.Value);
    }

    public static Player? ReadPlayerNullable(this NetworkReader reader)
    {
        bool hasValue = reader.ReadBool();

        if(!hasValue)
        {
            return null;
        }

        return reader.Read<Player>();
    }

}
