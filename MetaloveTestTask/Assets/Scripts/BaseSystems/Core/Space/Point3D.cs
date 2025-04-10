namespace Scripts.BaseSystems.Space
{

    [System.Serializable]
    public struct Point3D
    {
        public int X;
        public int Y;
        public int Z;

        public Point3D( int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Point3D( Point3D pointToClone)
        {
            X = pointToClone.X;
            Y = pointToClone.Y;
            Z = pointToClone.Z;
        }

        public static Point3D operator +(Point3D a) => a;
        public static Point3D operator +(Point3D a, Point3D b) => new Point3D( a.X+b.X, a.Y+b.Y, a.Z+b.Z );

        public static Point3D operator -(Point3D a) => a;
        public static Point3D operator -(Point3D a, Point3D b) => new Point3D( a.X-b.X, a.Y-b.Y, a.Z-b.Z );

        public static Point3D operator *(Point3D a, Point3D b) => new Point3D( a.X*b.X, a.Y*b.Y, a.Z*b.Z );
        public static Point3D operator /(Point3D a, Point3D b) => new Point3D( a.X/b.X, a.Y/b.Y, a.Z/b.Z) ;

        public static bool operator ==(Point3D a, Point3D b)
        {
            if (a.X == b.X && a.Y == b.Y && a.Z == b.Z ) return true;

            return false; 
        }

        public static bool operator !=(Point3D a, Point3D b)
        {
            if (a.X == b.X && a.Y == b.Y && a.Z == b.Z) return false;

            return true;
        }

        public override string ToString() =>  "   X: "+X+"   Y: "+Y+"   Z: "+Z ;

        public override bool Equals(object obj)
        {
            if (obj is Point3D)
            {
                Point3D other = (Point3D)obj;
                return this == other;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }
    }
}
