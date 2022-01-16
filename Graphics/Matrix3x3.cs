namespace GraphicsApp.Graphics;

// Матрица 3x3
public struct Matrix3x3
{
    // Значения в матрице
    public double M11, M12, M13,
                  M21, M22, M23,
                  M31, M32, M33;

    public Matrix3x3(double m11, double m12, double m13, 
                     double m21, double m22, double m23, 
                     double m31, double m32, double m33)
    {
        M11 = m11; M12 = m12; M13 = m13;
        M21 = m21; M22 = m22; M23 = m23;
        M31 = m31; M32 = m32; M33 = m33;
    }

    // Вычисляет матрицу поворота по трём заданным углам
    public static Matrix3x3 FromRotation(double aX, double aY, double aZ) => 
        FromRotationX(aX) * FromRotationY(aY) * FromRotationZ(aZ);

    // Вычисляет матрицу поворота вокруг оси X
    public static Matrix3x3 FromRotationX(double a) => new(
        1, 0,      0,
        0, Cos(a), -Sin(a),
        0, Sin(a), Cos(a)
    );

    // Вычисляет матрицу поворота вокруг оси Y
    public static Matrix3x3 FromRotationY(double a) => new(
        Cos(a),  0, Sin(a),
        0,       1, 0,
        -Sin(a), 0, Cos(a)
    );

    // Вычисляет матрицу поворота вокруг оси Z
    public static Matrix3x3 FromRotationZ(double a) => new(
        Cos(a), -Sin(a), 0,
        Sin(a), Cos(a),  0,
        0,      0,       1
    );

    // Умножение матрицы на матрицу
    public static Matrix3x3 operator *(Matrix3x3 a, Matrix3x3 b) => new(
        a.M11*b.M11 + a.M12*b.M21 + a.M13*b.M31,
        a.M11*b.M12 + a.M12*b.M22 + a.M13*b.M32,
        a.M11*b.M13 + a.M12*b.M23 + a.M13*b.M33,

        a.M21*b.M11 + a.M22*b.M21 + a.M23*b.M31,
        a.M21*b.M12 + a.M22*b.M22 + a.M23*b.M32,
        a.M21*b.M13 + a.M22*b.M23 + a.M23*b.M33,

        a.M31*b.M11 + a.M32*b.M21 + a.M33*b.M31,
        a.M31*b.M12 + a.M32*b.M22 + a.M33*b.M32,
        a.M31*b.M13 + a.M32*b.M23 + a.M33*b.M33
    );

    // Умножение матрицы на вектор
    public static Point3D operator *(Matrix3x3 m, Point3D p) => new(
        p.X * m.M11 + p.Y * m.M12 + p.Z * m.M13,
        p.X * m.M21 + p.Y * m.M22 + p.Z * m.M23,
        p.X * m.M31 + p.Y * m.M32 + p.Z * m.M33
    );
}