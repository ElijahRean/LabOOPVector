using System;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Security.Cryptography;
using static Vector4D;

public abstract class Vector
{


    public static Vector operator +(Vector vector1, Vector vector2)
    {
        return vector1.Add(vector2);
    }

    public static Vector operator -(Vector vector1, Vector vector2)
    {
        return vector1.Subtract(vector2);
    }

    public static double operator *(Vector vector1, Vector vector2)
    {
        return vector1.Dot(vector2);
    }

    public static Vector operator *(Vector vector, double scalar)
    {
        return vector.Multiply(scalar);
    }

    public static bool operator >(Vector vector1, Vector vector2)
    {
        return vector1.Length > vector2.Length;
    }

    public static bool operator <(Vector vector1, Vector vector2)
    {
        return vector1.Length < vector2.Length;
    }

    protected abstract Vector Add(Vector vector);
    protected abstract Vector Subtract(Vector vector);
    protected abstract double Dot(Vector vector);
    protected abstract Vector Multiply(double scalar);

    public abstract double Length { get; }
    public abstract Vector Normalize();
    public abstract override string ToString();

    public static void Info()
    {
        Console.WriteLine("This is a vector.");
    }
}

public class Vector3D : Vector
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }

    public Vector3D(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    protected override Vector Add(Vector other)
    {
        if (other is Vector3D vector)
        {
            return new Vector3D(X + vector.X, Y + vector.Y, Z + vector.Z);
        }
        throw new ArgumentException("Invalid vector type.");
    }

    protected override Vector Subtract(Vector other)
    {
        if (other is Vector3D vector)
        {
            return new Vector3D(X - vector.X, Y - vector.Y, Z - vector.Z);
        }
        throw new ArgumentException("Invalid vector type.");
    }

    protected override double Dot(Vector other)
    {
        if (other is Vector3D vector)
        {
            return X * vector.X + Y * vector.Y + Z * vector.Z;
        }
        throw new ArgumentException("Invalid vector type.");
    }

    protected override Vector Multiply(double scalar)
    {
        return new Vector3D(scalar * X, scalar * Y, scalar * Z);
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        Vector3D other = (Vector3D)obj;
        return X == other.X && Y == other.Y && Z == other.Z;
    }

    public override int GetHashCode()
    {
        return (X, Y, Z).GetHashCode();
    }

    public override double Length
    {
        get { return Math.Sqrt(X * X + Y * Y + Z * Z); }
    }

    public override Vector Normalize()
    {
        double length = Length;
        if (length == 0)
        {
            throw new VectorNormalizationException("Cannot normalize a zero-length vector.");
        }
        return new Vector3D(X / length, Y / length, Z / length);
    }
    public override string ToString()
    {
        return $"({X}, {Y}, {Z})";
    }

    public static bool operator ==(Vector3D v1, Vector3D v2)
    {
        return v1.Equals(v2);
    }

    public static bool operator !=(Vector3D v1, Vector3D v2)
    {
        return !v1.Equals(v2);
    }

    public static Vector3D operator +(Vector3D v1, Vector3D v2)
    {
        return (Vector3D)v1.Add(v2);
    }

    public static Vector3D operator -(Vector3D v1, Vector3D v2)
    {
        return (Vector3D)v1.Subtract(v2);
    }

    public static double operator *(Vector3D v1, Vector3D v2)
    {
        return v1.Dot(v2);
    }

    public static Vector3D operator *(Vector3D v, double scalar)
    {
        return (Vector3D)v.Multiply(scalar);
    }


}

class Vector4D : Vector3D
{
    public double W { get; set; }
    public Vector4D() : this(0, 0, 0, 0)
    {
        W = 0;
    }
    public Vector4D(double x, double y, double z, double w) : base(x, y, z)
    {
        W = w;
    }
    protected override Vector Add(Vector other)
    {
        if (other is Vector3D)
        {
            Vector3D otherVector = (Vector3D)other;
            return new Vector4D(X + otherVector.X, Y + otherVector.Y, Z + otherVector.Z, W);
        }
        else if (other is Vector4D)
        {
            Vector4D otherVector = (Vector4D)other;
            return new Vector4D(X + otherVector.X, Y + otherVector.Y, Z + otherVector.Z, W + otherVector.W);
        }
        else
        {
            throw new ArgumentException("Invalid argument type for Add operation."); ;
        }
    }
    protected override Vector Subtract(Vector other)
    {
        if (other is Vector3D)
        {
            Vector3D otherVector = (Vector3D)other;
            return new Vector4D(X - otherVector.X, Y - otherVector.Y, Z - otherVector.Z, W);
        }
        else if (other is Vector4D)
        {
            Vector4D otherVector = (Vector4D)other;
            return new Vector4D(X - otherVector.X, Y - otherVector.Y, Z - otherVector.Z, W - otherVector.W);
        }
        else
        {
            throw new ArgumentException("Invalid argument type for Subtract operation.");
        }
    }
    protected override double Dot(Vector other)
    {
        if (other is Vector3D)
        {
            Vector3D otherVector = (Vector3D)other;
            return X * otherVector.X + Y * otherVector.Y + Z * otherVector.Z;
        }
        else if (other is Vector4D)
        {
            Vector4D otherVector = (Vector4D)other;
            return X * otherVector.X + Y * otherVector.Y + Z * otherVector.Z + W * otherVector.W;
        }
        else
        {
            throw new ArgumentException("Invalid argument type for Dot operation.");
        }
    }
    protected override Vector Multiply(double scalar)
    {
        return new Vector4D(X * scalar, Y * scalar, Z * scalar, W * scalar);
    }
    public new bool Equals(object other)
    {
        if (other == null || GetType() != other.GetType())
        {
            return false;
        }

        Vector4D otherVector = (Vector4D)other;
        return X == otherVector.X && Y == otherVector.Y && Z == otherVector.Z && W == otherVector.W;
    }
    public double Length
    {
        get { return Math.Sqrt(X * X + Y * Y + Z * Z + W * W); }
    }
    public Vector4D Normalize()
    {
        double length = Length;
        if (length == 0)
        {
            throw new VectorNormalizationException("Cannot normalize a zero-length vector.");
        }
        return new Vector4D(X / length, Y / length, Z / length, W / length);
    }
    public override string ToString()
    {
        return $"({X}, {Y}, {Z}, {W})";
    }



    public class VectorNormalizationException : Exception
    {
        public VectorNormalizationException(string message) : base(message)
        {
        }
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Lab OOP 05, st78829 4101BNA,I.Petrovs");
            Console.WriteLine("Enter the dimensionality of the vector (3 or 4):");
            int dimension = int.Parse(Console.ReadLine());

            Vector vector;
            if (dimension == 3)
            {
                Console.WriteLine("Enter the values for vector 1 (x, y, z):");
                double x1 = double.Parse(Console.ReadLine());
                double y1 = double.Parse(Console.ReadLine());
                double z1 = double.Parse(Console.ReadLine());
                vector = new Vector3D(x1, y1, z1);


            }
            else if (dimension == 4)
            {
                Console.WriteLine("Enter the values for vector 1 (x, y, z, w):");
                double x1 = double.Parse(Console.ReadLine());
                double y1 = double.Parse(Console.ReadLine());
                double z1 = double.Parse(Console.ReadLine());
                double w1 = double.Parse(Console.ReadLine());
                vector = new Vector4D(x1, y1, z1, w1);
            }
            else
            {
                Console.WriteLine("Invalid dimensionality.");
                return;
            }

            Vector vector2;
            if (dimension == 3)
            {
                Console.WriteLine("Enter the values for vector 2 (x, y, z):");
                double x2 = double.Parse(Console.ReadLine());
                double y2 = double.Parse(Console.ReadLine());
                double z2 = double.Parse(Console.ReadLine());
                vector2 = new Vector3D(x2, y2, z2);
            }
            else if (dimension == 4)
            {
                Console.WriteLine("Enter the values for vector 2 (x, y, z, w):");
                double x2 = double.Parse(Console.ReadLine());
                double y2 = double.Parse(Console.ReadLine());
                double z2 = double.Parse(Console.ReadLine());
                double w2 = double.Parse(Console.ReadLine());
                vector2 = new Vector4D(x2, y2, z2, w2);
            }
            else
            {
                Console.WriteLine("Invalid dimensionality.");
                return;
            }

            Vector addition = vector + vector2;
            Vector subtraction = vector - vector2;
            double dotProduct = vector * vector2;
            Vector scalarMultiply = vector * 2;
            double length = vector.Length;

            Console.WriteLine($"Addition: {addition}");
            Console.WriteLine($"Subtraction: {subtraction}");
            Console.WriteLine($"Dot Product: {dotProduct}");
            Console.WriteLine($"Scalar Multiply: {scalarMultiply}");
            Console.WriteLine($"Length: {length}");


            if (vector is Vector3D)
            {
                Vector3D vector3D = (Vector3D)vector;
                Console.WriteLine($"Normalized Vector: {vector3D.Normalize()}");
            }
            else if (vector is Vector4D)
            {
                Vector4D vector4D = (Vector4D)vector;
                Console.WriteLine($"Normalized Vector: {vector4D.Normalize()}");
            }


            if (length > vector2.Length)
            {
                Console.WriteLine("Vector 1 is longer than Vector 2.");
            }
            else if (length < vector2.Length)
            {
                Console.WriteLine("Vector 1 is shorter than Vector 2.");
            }
            else
            {
                Console.WriteLine("Vector 1 and Vector 2 are of equal length.");
            }

            Vector.Info();
        }
    }
}