using System;
using System.Collections.Generic;
using System.Text;

namespace Aardvark.Base
{
    public interface IValidity
    {
        bool IsValid { get; }
        bool IsInvalid { get; }
    }
}
