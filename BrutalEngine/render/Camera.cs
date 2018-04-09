using System;
using OpenTK;

namespace BrutalEngine.render
{
    public class Camera
    {
        public const float NearPlane = 0.1f;
        public const float FarPlane = 1000f;
        public const float Fov = 70;

        public Matrix4 View { get; private set; }
        public Matrix4 Projection { get; private set; }

        private float _pitch;
        public float Yaw = MathHelper.PiOver2;

        public Vector3 Pos;

        public Vector2 Left
        {
            get
            {
                var s = (float)Math.Sin(-(Yaw + MathHelper.PiOver2));
                var c = (float)Math.Cos(Yaw + MathHelper.PiOver2);

                return new Vector2(s, c).Normalized();
            }
        }

        public Vector2 Forward
        {
            get
            {
                var s = -(float)Math.Sin(-Yaw);
                var c = -(float)Math.Cos(Yaw);

                return new Vector2(s, c).Normalized();
            }
        }

        public Camera()
        {
            UpdateViewMatrix();
            UpdateProjectionMatrix();
        }

        public float Pitch
        {
            get => _pitch;

            set => _pitch = MathHelper.Clamp(value, -MathHelper.PiOver2, MathHelper.PiOver2);
        }

        public void UpdateViewMatrix()
        {
            var x = Matrix4.CreateRotationX(Pitch);
            var y = Matrix4.CreateRotationY(Yaw);

            var t = Matrix4.CreateTranslation(-Pos);

            View = t * y * x;
        }

        public void UpdateProjectionMatrix()
        {
            var matrix = Matrix4.Identity;

            var aspectRatio = (float)Game.Instance.Width / Game.Instance.Height;
            var yScale = (float)(1f / Math.Tan(MathHelper.DegreesToRadians(Fov / 2f)));
            var xScale = yScale / aspectRatio;
            var frustumLength = FarPlane - NearPlane;

            matrix.M11 = xScale;
            matrix.M22 = yScale;
            matrix.M33 = -((FarPlane + NearPlane) / frustumLength);
            matrix.M34 = -1;
            matrix.M43 = -(2 * NearPlane * FarPlane / frustumLength);
            matrix.M44 = 0;

            Projection = matrix;
        }

    }
}
