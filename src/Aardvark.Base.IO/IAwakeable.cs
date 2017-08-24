namespace Aardvark.VRVis
{
    /// <summary>
    /// Implement this to awake an object from decoding after all fields of
    /// the object have already been decoded. This is normally used to
    /// initialize member variables that serve as caches.
    /// </summary>
    public interface IAwakeable
    {
        /// <summary>
        /// This method is called directly after an object has been
        /// deserialized. It get the version of the coded object as a
        /// parameter.
        /// </summary>
        void Awake(int codedVersion);
    }
}
