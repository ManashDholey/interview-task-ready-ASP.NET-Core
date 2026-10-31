using System;

public struct LargeVector
{
    public double X;
    public double Y;
    public double Z;
    // Pretend there are many more fields here, making it expensive to copy
}

class Program
{
    static void Main()
    {
        LargeVector vector = new LargeVector { X = 10.5, Y = 20.5, Z = 30.5 };

        // 1. Pass by implicit read-only reference (Recommended syntax)
        ProcessVector(vector);

        // 2. Pass by explicit read-only reference 
        ProcessVector(in vector);
    }

    // The 'in' modifier passes 'data' by reference without copying it, 
    // but guarantees it remains read-only.
    static void ProcessVector(in LargeVector data)
    {
        // Reading values is perfectly fine
        Console.WriteLine($"Vector coordinates: X={data.X}, Y={data.Y}, Z={data.Z}");

        // CS8331: Cannot assign to a member of variable 'data' because it is a read-only variable
        // data.X = 99.9; 
    }
}
