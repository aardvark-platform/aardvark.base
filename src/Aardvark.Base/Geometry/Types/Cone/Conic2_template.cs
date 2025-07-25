/*
    Copyright 2006-2025. The Aardvark Platform Team.

        https://aardvark.graphics

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Aardvark.Base;

#pragma warning disable IDE0290 // Use primary constructor

// AUTO GENERATED CODE - DO NOT CHANGE!

//# foreach (var isDouble in new[] { false, true }) {
//#   var ftype = isDouble ? "double" : "float";
//#   var ftype2 = isDouble ? "float" : "double";
//#   var tc = isDouble ? "d" : "f";
//#   var tc2 = isDouble ? "f" : "d";
//#   var type = "Conic2" + tc;
//#   var type2 = "Conic2" + tc2;
//#   var v2t = "V2" + tc;
#region __type__

/// <summary>
/// WARNING: Sektch!
/// Defines the conic section CXX x^2 + CYY y^2 + CZZ z^2 + CXY xy + CYZ yz + CYZ yz = 0
/// with (x,y,z) being homogenous coordinates.
/// </summary>
[DataContract]
[StructLayout(LayoutKind.Sequential)]
public partial struct __type__
{
    [DataMember]
    public __ftype__ CXX;
    [DataMember]
    public __ftype__ CYY;
    [DataMember]
    public __ftype__ CZZ;
    [DataMember]
    public __ftype__ CXY;
    [DataMember]
    public __ftype__ CXZ;
    [DataMember]
    public __ftype__ CYZ;

    public __type__(__ftype__ cxx, __ftype__ cyy, __ftype__ czz, __ftype__ cxy, __ftype__ cxz, __ftype__ cyz)
    {
        CXX = cxx; CYY = cyy; CZZ = czz; CXY = cxy; CXZ = cxz; CYZ = cyz;
    }


    public readonly __ftype__ Discriminant { get { return CXX * CYY - CXY * CXY; } }

    public readonly int ConicType
    {
        get
        {
            var d = Discriminant;
            return d > Constant<__ftype__>.PositiveTinyValue ? 1
                : d < Constant<__ftype__>.NegativeTinyValue ? -1 : 0;
        }
    }
}

#endregion

//# }
