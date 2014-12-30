namespace Aardvark.Base
{
    /// <summary>
    /// Associates a generation number with an object, which should
    /// change every time the object's observable state changes.
    /// 
    /// Observers SHOULD NOT test for changes using 'greater than',
    /// but SHOULD compare the current generation with a previously
    /// observed generation using 'not equal'.
    /// 
    /// Although most of the time a new generation number will be
    /// created by simply incrementing the current generation number,
    /// such a behaviour is not guaranteed.
    /// E.g. at the very least such an assumption will be violated
    /// when a generation number wraps around from int.MaxValue to
    /// int.MinValue. So always compare using 'not equal'.
    /// </summary>
    public interface IGenerational
    {
        /// <summary>
        /// Gets generation number.
        /// </summary>
        int Generation { get; }
    }
}
