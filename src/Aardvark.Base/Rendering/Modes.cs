namespace Aardvark.Base.Rendering
{
    public enum AlphaTestFunction
    {
        None,
        Never,
        Less,
        Equal,
        LessOrEqual,
        Greater,
        NotEqual,
        GreaterOrEqual,
        Always,
    }

    public enum FillMode
    {
        Fill,
        Line,
        Point
    }

    public enum DepthTestComparison
    {
        None,
        Less,
        LessOrEqual,
        Greater,
        GreaterOrEqual,
        Equal,
        NotEqual,
        Never,
        Always
    }
    
    /// <summary>
    /// Winding order types
    /// </summary>
    public enum WindingOrder
    {
        /// <summary>
        /// Clockwise
        /// </summary>
        Clockwise,
        /// <summary>
        /// CounterClockwise
        /// </summary>
        CounterClockwise
    }

    /// <summary>
    /// CullMode options
    /// </summary>
    public enum CullMode
    {
        /// <summary>
        /// No culling / draw all faces
        /// </summary>
        None,
        /// <summary>
        /// Draw only back faces
        /// </summary>
        Front,
        /// <summary>
        /// Draw only front faces
        /// </summary>
        Back,
        /// <summary>
        /// Cull all faces / lines and points will be drawn
        /// </summary>
        FrontAndBack
    }
}
