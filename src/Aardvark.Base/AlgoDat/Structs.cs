//using System;
//using System.Collections.Generic;
//using System.Diagnostics.Contracts;
//using System.Linq;
//using System.Text;

//namespace Aardvark.Base
//{
//    #region Volatile Array

//    /// <summary>
//    /// A wrapper for a normal array for safe parallel (multi-threaded) writing.
//    /// </summary>
//    public struct VolatileArray<T>
//    {
//        public volatile T[] Array;

//        public VolatileArray(T[] array) { Array = array; }
//    }

//    #endregion

//    #region StructsExtensions

//    public static class StructsExtensions
//    {
//        #region IndexedValue

//        public static IndexedValue<T> IndexedValue<T>(this T self, int index) => new IndexedValue<T>(index, self);

//        #endregion
        
//        #region VolatileArray

//        public static VolatileArray<T> ToVolatile<T>(this T[] array) => new VolatileArray<T>(array);

//        #endregion
//    }

//    #endregion
//}