namespace Aardvark.Base
{
    /// <summary>
    /// A structure holding a double weight and an index.
    /// </summary>
    public struct WeightedIndex
    {
        public double Weight;
        public int Index;

        #region Constructor

        public WeightedIndex(double weight, int index)
        {
            Weight = weight;
            Index = index;
        }

        #endregion
    }
}
