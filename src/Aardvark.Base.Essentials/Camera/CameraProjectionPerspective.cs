

namespace Aardvark.Base
{
    public class CameraProjectionPerspective : ICameraProjectionPerspective
    {
        private Trafo3d m_trafo;
        private EventSource<Trafo3d> m_trafoChanges = new EventSource<Trafo3d>();
        private Box3d m_box;

        public CameraProjectionPerspective(double horizontalFovInDegrees, double near, double far, double aspectRatio = 1.0)
        {
            SetClippingParams(horizontalFovInDegrees, near, far, aspectRatio);
        }

        public CameraProjectionPerspective(double left, double right, double bottom, double top, double near, double far)
        {
            SetClippingParams(left, right, bottom, top, near, far);
        }

        public Trafo3d ProjectionTrafo { get { return m_trafo; } }

        public IEvent<Trafo3d> ProjectionTrafos { get { return m_trafoChanges; } }

        public Ray3d Unproject(V2d xyOnNearPlane)
        {
            return new Ray3d(V3d.Zero, new V3d(xyOnNearPlane, -m_box.Min.Z));
        }

        public void SetClippingParams(double left, double right, double bottom, double top, double near, double far)
        {
            m_box.Min.X = left;
            m_box.Max.X = right;
            m_box.Min.Y = bottom;
            m_box.Max.Y = top;
            m_box.Min.Z = near;
            m_box.Max.Z = far;

            UpdateProjectionTrafo();
        }

        public void SetClippingParams(double horizontalFovInDegrees, double near, double far, double aspectRatio = 1.0)
        {
            var d = System.Math.Tan(Conversion.RadiansFromDegrees(horizontalFovInDegrees) * 0.5) * near;
            m_box.Min.X = -d;
            m_box.Max.X = +d;
            m_box.Min.Y = -d / aspectRatio;
            m_box.Max.Y = +d / aspectRatio;
            m_box.Min.Z = near;
            m_box.Max.Z = far;

            UpdateProjectionTrafo();
        }

        public double Near
        {
            get { return m_box.Min.Z; }
            set
            {
                Requires.That(value > 0.0 && value < Far);

                var s = value / m_box.Min.Z;
                m_box.Min.X *= s; m_box.Max.X *= s;
                m_box.Min.Y *= s; m_box.Max.Y *= s;
                m_box.Min.Z = value;
                UpdateProjectionTrafo();
            }
        }

        public double Far
        {
            get { return m_box.Max.Z; }
            set
            {
                Requires.That(value > 0.0 && value > Near);
                m_box.Max.Z = value;
                UpdateProjectionTrafo();
            }
        }

        public Box2d ClippingWindow
        {
            get { return m_box.XY; }
            set
            {
                Requires.That(m_box.IsValid);
                m_box.Min.X = value.Min.X;
                m_box.Min.Y = value.Min.Y;
                m_box.Max.X = value.Max.X;
                m_box.Max.Y = value.Max.Y;
                UpdateProjectionTrafo();
            }
        }

        public double AspectRatio
        {
            get
            {
                return (m_box.Max.X - m_box.Min.X) / (m_box.Max.Y - m_box.Min.Y);
            }
            set
            {
                Requires.That(value > 0.0);
                m_box.Min.Y = m_box.Min.X / value;
                m_box.Max.Y = m_box.Max.X / value;
                UpdateProjectionTrafo();
            }
        }

        public double HorizontalFieldOfViewInDegrees
        {
            get
            {
                var l = System.Math.Atan2(m_box.Min.X, m_box.Min.Z);
                var r = System.Math.Atan2(m_box.Max.X, m_box.Min.Z);
                return Conversion.DegreesFromRadians(-l + r);
            }
        }

        private void UpdateProjectionTrafo()
        {
            var l = m_box.Min.X; var r = m_box.Max.X;
            var b = m_box.Min.Y; var t = m_box.Max.Y;
            var n = m_box.Min.Z; var f = m_box.Max.Z;

            m_trafo = new Trafo3d(
                new M44d(
                    (2 * n) / (r - l),                     0,     (r + l) / (r - l),                     0,
                                    0,     (2 * n) / (t - b),     (t + b) / (t - b),                     0,
                                    0,                     0,           f / (n - f),     (f * n) / (n - f),
                                    0,                     0,                    -1,                     0
                    ),                                                     
                                                                       
                new M44d(                                      
                    (r - l) / (2 * n),                     0,                     0,     (r + l) / (2 * n),
                                    0,     (t - b) / (2 * n),                     0,     (t + b) / (2 * n),
                                    0,                     0,                     0,                    -1,
                                    0,                     0,     (n - f) / (f * n),                 1 / n
                    )
                );

            m_trafoChanges.Emit(m_trafo);
        }
    }
}
