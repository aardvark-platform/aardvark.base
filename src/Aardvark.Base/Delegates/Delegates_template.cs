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
using System;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# Action comma = () => Out(", ");
    #region RefFunc

    //# for (int c = 1; c < 17; c++) {
    public delegate TRes RefFunc</*# c.ForEach(i => { */T__i__/*# }, comma); */, TRes>
            (/*# c.ForEach(i => { */ref T__i__ a__i__/*# }, comma); */);

    //# }

    #endregion

    #region FuncOut

    //# for (int o = 1; o < 9; o++) {
    //# for (int c = o; c < 17; c++) {
    public delegate TRes FuncOut__o__</*# c.ForEach(i => { */T__i__/*# }, comma); */, TRes>
            (/*# c.ForEach(i => { if (i >= c - o) {*/out /*# } */T__i__ a__i__/*# }, comma); */);

    //# }
    //# }
    #endregion

    #region FuncRef

    //# for (int r = 1; r < 9; r++) {
    //# for (int c = r; c < 17; c++) {
    public delegate TRes FuncRef__r__</*# c.ForEach(i => { */T__i__/*# }, comma); */, TRes>
            (/*# c.ForEach(i => { if (i >= c - r) {*/ref /*# } */T__i__ a__i__/*# }, comma); */);

    //# }
    //# }
    #endregion

    #region Recursive

    //# for (int c = 1; c < 5; c++) {
    public delegate Func</*# c.ForEach(i => { */T__i__/*# }, comma); */, TRes>
            Recursive</*# c.ForEach(i => { */T__i__/*# }, comma); */, TRes>
                    (Recursive</*# c.ForEach(i => { */T__i__/*# }, comma); */, TRes> r);

    //# }
    #endregion

    #region RefAction

    //# for (int c = 1; c < 17; c++) {
    public delegate void RefAction</*# c.ForEach(i => { */T__i__/*# }, comma); */>
            (/*# c.ForEach(i => { */ref T__i__ a__i__/*# }, comma); */);

    //# }

    #endregion

    #region ActionRef

    //# for (int r = 1; r < 9; r++) {
    //# for (int c = r; c < 17; c++) {
    public delegate void ActionRef__r__</*# c.ForEach(i => { */T__i__/*# }, comma); */>
            (/*# c.ForEach(i => { if (i >= c - r) {*/ref /*# } */T__i__ a__i__/*# }, comma); */);

    //# }
    //# }
    #endregion
}
