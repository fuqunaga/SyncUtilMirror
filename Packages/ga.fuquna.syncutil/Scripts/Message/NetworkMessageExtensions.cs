﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Mirror;

namespace SyncUtil
{
    public static class NetworkMessageExtensions
    {
        private static readonly NetworkWriter Writer = new();

        private static readonly Dictionary<Type, MethodInfo> WriteMethodTable = new();
        private static readonly object[] MiArgs = new object[1];
        
        public static byte[] ToBytes(this NetworkMessage msg)
        {
            var type = msg.GetType();
            if (!WriteMethodTable.TryGetValue(type, out var mi))
            {
                var miNonGeneric = typeof(NetworkWriter).GetMethod(nameof(NetworkWriter.Write));
                WriteMethodTable[type] = mi = miNonGeneric.MakeGenericMethod(type); 
            }
            
            Writer.Position = 0;
            MiArgs[0] = msg;
            mi.Invoke(Writer, MiArgs);
            
            return Writer.ToArray();
        }
    }
}