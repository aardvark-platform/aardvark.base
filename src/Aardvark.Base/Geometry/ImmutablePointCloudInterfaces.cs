using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Aardvark.Base
{
    /// <summary>
    /// Immutable point cloud.
    /// </summary>
    public interface IImmutablePointCloud
    {
        /// <summary>
        /// Enumerates all points in this point cloud.
        /// Ordering of points is consistent across multiple calls.
        /// </summary>
        IEnumerable<IImmutablePointCloudPoint> Points { get; }

        /// <summary>
        /// Gets array of all point positions.
        /// Ordering is consistent with Points, Normals, and Colors.
        /// </summary>
        V3d[] Positions { get; }

        /// <summary>
        /// Gets array of all point normals.
        /// Ordering is consistent with Points, Positions, and Colors.
        /// </summary>
        V3d[] Normals { get; }

        /// <summary>
        /// Gets array of all point colors.
        /// Ordering is consistent with Points, Positions, and Normals.
        /// </summary>
        C4b[] Colors { get; }

        /// <summary>
        /// Gets number of points in this point cloud.
        /// </summary>
        long Count { get; }

        /// <summary>
        /// If false, then property Positions will be null;
        /// </summary>
        bool HasPositions { get; }

        /// <summary>
        /// If false, then property Normals will be null.
        /// </summary>
        bool HasNormals { get; }

        /// <summary>
        /// If false, then property Colors will be null.
        /// </summary>
        bool HasColors { get; }

        /// <summary>
        /// Returns new point cloud consisting of specified points.
        /// </summary>
        IImmutablePointCloud Subset(IEnumerable<IImmutablePointCloudPoint> points);

        /// <summary>
        /// Returns new point cloud with updated positions.
        /// If the point cloud has no positions, then positions for all points will be created (update or V3d.Zero).
        /// </summary>
        IImmutablePointCloud UpdatePositions(IDictionary<IImmutablePointCloudPoint, V3d> updates);

        /// <summary>
        /// Returns new point cloud with updated normals.
        /// If the point cloud has no normals, then normals for all points will be created (update or V3d.Zero).
        /// </summary>
        IImmutablePointCloud UpdateNormals(IDictionary<IImmutablePointCloudPoint, V3d> updates);

        /// <summary>
        /// Returns new point cloud with updated colors.
        /// If the point cloud has no colors, then colors for all points will be created (update or C4b.Black).
        /// </summary>
        IImmutablePointCloud UpdateColors(IDictionary<IImmutablePointCloudPoint, C4b> updates);
    }

    /// <summary>
    /// Immutable point in a point cloud.
    /// </summary>
    public interface IImmutablePointCloudPoint
    {
        V3d Position { get; }

        V3d Normal { get; }

        C4b Color { get; }

        bool HasPosition { get; }

        bool HasNormals { get; }

        bool HasColor { get; }
    }

    public static class IImmutablePointCloudExtensions
    {
        public static XElement ToXml(this IImmutablePointCloud self)
        {
            return new XElement("IImmutablePointCloud",
                new XElement("Positions", self.Positions.Select(x => new XElement("p", x))),
                new XElement("Normals", self.Normals.Select(x => new XElement("n", x))),
                new XElement("Colors", self.Colors.Select(x => new XElement("c", x)))
                );
        }
    }
}
