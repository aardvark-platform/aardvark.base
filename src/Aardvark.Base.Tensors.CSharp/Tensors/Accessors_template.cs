using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//# Action comma = () => Out(", ");
//# Action add = () => Out("+");

namespace Aardvark.Base
{
    public static partial class TensorAccessors
    {
        private static Dictionary<(Type, Type, Symbol),
                                  Func<long[], ITensorAccessors>> s_creatorMap
            = new Dictionary<(Type, Type, Symbol),
                             Func<long[], ITensorAccessors>>()
            {
                //# foreach (var tt in Meta.ColorFieldTypes) { var ttype = tt.Name;
                //# foreach (var ct in Meta.ColorFieldTypes) { var ctype = ct.Name;
                //#     var tt_from_ct = ct == tt ? "" : "Col." + ct.Caps + "To" + tt.Caps;
                //#     var ct_from_tt = ct == tt ? "" : "Col." + tt.Caps + "To" + ct.Caps; 
                #region ColorChannel __ttype__ as __ctype__

                {
                    (typeof(__ttype__), typeof(__ctype__), Intent.ColorChannel),
                    delta =>
                    {
                        return new TensorAccessors<__ttype__, __ctype__>()
                        {
                            Getter = (da, i) => __ct_from_tt__(da[i]),
                            Setter = (da, i, v) => da[i] = __tt_from_ct__(v),
                        };
                    }
                },

                #endregion

                //# } // ct
                //# } // tt
                //# var intentArray = new[] { "RGB", "BGR", "RGBA", "BGRA" };
                //# foreach (var tt in Meta.ColorFieldTypes) { var ttype = tt.Name;
                //# foreach (var ct in Meta.ColorTypes) { var ctype = ct.Name;
                //#     var cft = ct.FieldType;
                //#     var tmax = "(" + ttype +")" + Meta.ColorTypeOf(3, tt).MaxValue;
                //#     var cmax = "(" + cft.Name + ")" + ct.MaxValue;
                //#     string t_from_cf = "", cf_from_t = "";
                //#     if (tt != cft) {
                //#         t_from_cf = "Col." + cft.Caps + "To" + tt.Caps;
                //#         cf_from_t = "Col." + tt.Caps + "To" + cft.Caps;
                //#     }
                //# foreach (var intent in intentArray) {
                //#     var indices = (from f in ct.Fields select intent.IndexOf(f)).ToArray();
                #region __intent__ __ttype__s as __ctype__

                {
                    (typeof(__ttype__), typeof(__ctype__), Intent.__intent__),
                    delta =>
                    {
                        if (delta.Length < 3)
                            throw new ArgumentException("to few dimensions in tensor");
                        long d1 = delta[delta.Length - 1];
                        if (d1 == 1)
                            return new TensorAccessors<__ttype__, __ctype__>()
                            {
                                Getter = (da, i) =>
                                    new __ctype__(/*# indices.ForEach(i => { if (i >= 0) { */
                                            __cf_from_t__(da[i/*# if (i > 0) { */+__i__/*# } */])/*# } else { */
                                            /*# Out(cmax); } }, comma); */),
                                Setter = (da, i, v) =>
                                {
                                    //# intent.ForEach((f, i) => { if (Array.IndexOf(ct.Fields, f.ToString()) >= 0) {
                                    da[i/*# if (i > 0) { */+__i__/*# } */] = __t_from_cf__(v.__f__);
                                    //# } else {
                                    da[i/*# if (i > 0) { */+__i__/*# } */] = __tmax__;
                                    //# }});
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1/*# if (intent.Length == 4) { */, d3 = d1 + d2/*# }*/;
                            return new TensorAccessors<__ttype__, __ctype__>()
                            {
                                Getter = (da, i) =>
                                    new __ctype__(/*# indices.ForEach(i => { if (i >= 0) { */
                                            __cf_from_t__(da[i/*# if (i > 0) { */+d__i__/*# } */])/*# } else { */
                                            /*# Out(cmax); } }, comma); */),
                                Setter = (da, i, v) =>
                                {
                                    //# intent.ForEach((f, i) => { if (Array.IndexOf(ct.Fields, f.ToString()) >= 0) {
                                    da[i/*# if (i > 0) { */+d__i__/*# } */] = __t_from_cf__(v.__f__);
                                    //# } else {
                                    da[i/*# if (i > 0) { */+d__i__/*# } */] = __tmax__;
                                    //# }});
                                }
                            };
                        }
                    }
                },

                #endregion

                //# } // intent
                //# } // ct
                //# } // tt
            };
    }
}
