using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
