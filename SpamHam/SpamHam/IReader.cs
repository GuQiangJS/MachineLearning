using System;
using System.Collections.Generic;

namespace SpamHam
{
    public interface IReader
    {
        Dictionary<string,DocType> Read();
    }
}