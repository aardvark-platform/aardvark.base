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

#region Conic2f

/// <summary>
/// WARNING: Sektch!
/// Defines the conic section CXX x^2 + CYY y^2 + CZZ z^2 + CXY xy + CYZ yz + CYZ yz = 0
/// with (x,y,z) being homogenous coordinates.
/// </summary>
[DataContract]
[StructLayout(LayoutKind.Sequential)]
public partial struct Conic2f
{
    [DataMember]
    public float CXX;
    [DataMember]
    public float CYY;
    [DataMember]
    public float CZZ;
    [DataMember]
    public float CXY;
    [DataMember]
    public float CXZ;
    [DataMember]
    public float CYZ;

    public Conic2f(float cxx, float cyy, float czz, float cxy, float cxz, float cyz)
    {
        CXX = cxx; CYY = cyy; CZZ = czz; CXY = cxy; CXZ = cxz; CYZ = cyz;
    }


    public readonly float Discriminant { get { return CXX * CYY - CXY * CXY; } }

    public readonly int ConicType
    {
        get
        {
            var d = Discriminant;
            return d > Constant<float>.PositiveTinyValue ? 1
                : d < Constant<float>.NegativeTinyValue ? -1 : 0;
        }
    }
}

#endregion

#region Conic2d

/// <summary>
/// WARNING: Sektch!
/// Defines the conic section CXX x^2 + CYY y^2 + CZZ z^2 + CXY xy + CYZ yz + CYZ yz = 0
/// with (x,y,z) being homogenous coordinates.
/// </summary>
[DataContract]
[StructLayout(LayoutKind.Sequential)]
public partial struct Conic2d
{
    [DataMember]
    public double CXX;
    [DataMember]
    public double CYY;
    [DataMember]
    public double CZZ;
    [DataMember]
    public double CXY;
    [DataMember]
    public double CXZ;
    [DataMember]
    public double CYZ;

    public Conic2d(double cxx, double cyy, double czz, double cxy, double cxz, double cyz)
    {
        CXX = cxx; CYY = cyy; CZZ = czz; CXY = cxy; CXZ = cxz; CYZ = cyz;
    }


    public readonly double Discriminant { get { return CXX * CYY - CXY * CXY; } }

    public readonly int ConicType
    {
        get
        {
            var d = Discriminant;
            return d > Constant<double>.PositiveTinyValue ? 1
                : d < Constant<double>.NegativeTinyValue ? -1 : 0;
        }
    }
}

#endregion

