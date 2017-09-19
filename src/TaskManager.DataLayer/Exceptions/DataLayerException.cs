﻿using System;

namespace TaskManager.DataLayer.Exceptions
{
    [Serializable]
    public class DataLayerException : Exception
    {
        public DataLayerException()
        { }

        public DataLayerException(string message) : base(message)
        { }

        public DataLayerException(string message, Exception inner) : base(message, inner)
        { }
    }
}
