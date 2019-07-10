﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Fusee.Math.Core
{
    public class Double4x4Test
    {
        #region Fields
        [Fact]
        public void Identity_IsIdentity()
        {
            var actual = double4x4.Identity;
            var expected = new double4x4(new double4(1, 0, 0, 0), new double4(0, 1, 0, 0), new double4(0, 0, 1, 0), new double4(0, 0, 0, 1));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Row0_IsRow()
        {
            var matrix = new double4x4(new double4(1, 0, 0, 0), new double4(0, 1, 0, 0), new double4(0, 0, 1, 0), new double4(0, 0, 0, 1));

            var actual = matrix.Row0;

            Assert.Equal(new double4(1, 0, 0, 0), actual);
        }

        [Fact]
        public void Row1_IsRow()
        {
            var matrix = new double4x4(new double4(1, 0, 0, 0), new double4(0, 1, 0, 0), new double4(0, 0, 1, 0), new double4(0, 0, 0, 1));

            var actual = matrix.Row1;

            Assert.Equal(new double4(0, 1, 0, 0), actual);
        }

        [Fact]
        public void Row2_IsRow()
        {
            var matrix = new double4x4(new double4(1, 0, 0, 0), new double4(0, 1, 0, 0), new double4(0, 0, 1, 0), new double4(0, 0, 0, 1));

            var actual = matrix.Row2;

            Assert.Equal(new double4(0, 0, 1, 0), actual);
        }

        [Fact]
        public void Row3_IsRow()
        {
            var matrix = new double4x4(new double4(1, 0, 0, 0), new double4(0, 1, 0, 0), new double4(0, 0, 1, 0), new double4(0, 0, 0, 1));

            var actual = matrix.Row3;

            Assert.Equal(new double4(0, 0, 0, 1), actual);
        }

        [Fact]
        public void Zero_IsZero()
        {
            var actual = double4x4.Zero;
            var expected = new double4x4(new double4(0, 0, 0, 0), new double4(0, 0, 0, 0), new double4(0, 0, 0, 0), new double4(0, 0, 0, 0));

            Assert.Equal(expected, actual);
        }
        #endregion

        #region Constructors

        [Fact]
        public void Constructor_Fromdouble4()
        {
            var actual = new double4x4(new double4(1, 1, 1, 1), new double4(1, 1, 1, 1), new double4(1, 1, 1, 1), new double4(1, 1, 1, 1));

            Assert.Equal(new double4x4(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1), actual);
        }

        #endregion

        #region Properties
        [Fact]
        public void Column0_IsColumn()
        {
            var matrix = double4x4.Identity;

            var actual = matrix.Column0;

            Assert.Equal(new double4(1, 0, 0, 0), actual);
        }

        [Fact]
        public void Column1_IsColumn()
        {
            var matrix = double4x4.Identity;

            var actual = matrix.Column1;

            Assert.Equal(new double4(0, 1, 0, 0), actual);
        }

        [Fact]
        public void Column2_IsColumn()
        {
            var matrix = double4x4.Identity;

            var actual = matrix.Column2;

            Assert.Equal(new double4(0, 0, 1, 0), actual);
        }

        [Fact]
        public void Column3_IsColumn()
        {
            var matrix = double4x4.Identity;

            var actual = matrix.Column3;

            Assert.Equal(new double4(0, 0, 0, 1), actual);
        }

        [Fact]
        public void Determinant_IsDeterminant()
        {
            var matrix = double4x4.Identity;

            var actual = matrix.Determinant;

            Assert.Equal(1, actual);
        }

        [Fact]
        public void IsAffine_IsAffine()
        {
            var matrix = double4x4.Identity;

            Assert.True(matrix.IsAffine);
        }

        [Fact]
        public void IsAffine_IsNotAffine()
        {
            var matrix = new double4x4(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);

            Assert.False(matrix.IsAffine);
        }

        [Fact]
        public void Offset_IsOffset()
        {
            var matrix = double4x4.Identity;

            Assert.Equal(new double3(0, 0, 0), matrix.Offset);
        }
        #endregion

        #region Instance

        [Fact]
        public void Invert_Instance()
        {
            var mat = new double4x4(1, 0, 0, 1, 0, 1, 0, 1, 0, 0, 1, 1, 0, 0, 0, 1);

            var actual = mat.Invert();

            Assert.Equal(new double4x4(1, 0, 0, -1, 0, 1, 0, -1, 0, 0, 1, -1, 0, 0, 0, 1), actual);
        }

        [Fact]
        public void AsArray_Instance()
        {
            var mat = new double4x4(1, 0, 0, 1, 0, 1, 0, 1, 0, 0, 1, 1, 0, 0, 0, 1);

            var actual = mat.AsArray;

            Assert.Equal(new double[] { 1, 0, 0, 1, 0, 1, 0, 1, 0, 0, 1, 1, 0, 0, 0, 1 }, actual);
        }

        [Fact]
        public void Transpose_Instance()
        {
            var mat = new double4x4(1, 0, 0, 1, 0, 1, 0, 1, 0, 0, 1, 1, 0, 0, 0, 1);

            var actual = mat.Transpose();

            Assert.Equal(new double4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1), actual);
        }

        #endregion

        #region this

        [Fact]
        public void This_GetWithIdx_IsValid()
        {
            var actual = new double4x4(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);

            Assert.Equal(0, actual[0, 0]);
            Assert.Equal(1, actual[0, 1]);
            Assert.Equal(2, actual[0, 2]);
            Assert.Equal(3, actual[0, 3]);

            Assert.Equal(4, actual[1, 0]);
            Assert.Equal(5, actual[1, 1]);
            Assert.Equal(6, actual[1, 2]);
            Assert.Equal(7, actual[1, 3]);

            Assert.Equal(8, actual[2, 0]);
            Assert.Equal(9, actual[2, 1]);
            Assert.Equal(10, actual[2, 2]);
            Assert.Equal(11, actual[2, 3]);

            Assert.Equal(12, actual[3, 0]);
            Assert.Equal(13, actual[3, 1]);
            Assert.Equal(14, actual[3, 2]);
            Assert.Equal(15, actual[3, 3]);
        }

        [Fact]
        public void This_SetWithIdx_IsValid()
        {
            var actual = double4x4.Zero;
            actual[0, 0] = 0;
            actual[0, 1] = 1;
            actual[0, 2] = 2;
            actual[0, 3] = 3;

            actual[1, 0] = 4;
            actual[1, 1] = 5;
            actual[1, 2] = 6;
            actual[1, 3] = 7;

            actual[2, 0] = 8;
            actual[2, 1] = 9;
            actual[2, 2] = 10;
            actual[2, 3] = 11;

            actual[3, 0] = 12;
            actual[3, 1] = 13;
            actual[3, 2] = 14;
            actual[3, 3] = 15;

            Assert.Equal(0, actual[0, 0]);
            Assert.Equal(1, actual[0, 1]);
            Assert.Equal(2, actual[0, 2]);
            Assert.Equal(3, actual[0, 3]);

            Assert.Equal(4, actual[1, 0]);
            Assert.Equal(5, actual[1, 1]);
            Assert.Equal(6, actual[1, 2]);
            Assert.Equal(7, actual[1, 3]);

            Assert.Equal(8, actual[2, 0]);
            Assert.Equal(9, actual[2, 1]);
            Assert.Equal(10, actual[2, 2]);
            Assert.Equal(11, actual[2, 3]);

            Assert.Equal(12, actual[3, 0]);
            Assert.Equal(13, actual[3, 1]);
            Assert.Equal(14, actual[3, 2]);
            Assert.Equal(15, actual[3, 3]);
        }

        [Theory]
        [MemberData(nameof(ThisException))]
        public void Invalid_GetWithIdx_Exception(int idx, string expected)
        {
            var actual = Assert.Throws<ArgumentOutOfRangeException>(() => double4x4.Zero[idx, idx]);

            Assert.Equal(expected, actual.ParamName);
        }

        [Theory]
        [MemberData(nameof(ThisException))]
        public void Invalid_SetWithIdx_Exception(int idx, string expected)
        {
            var actual = Assert.Throws<ArgumentOutOfRangeException>(() => { var d4 = double4x4.Zero; d4[idx, idx] = 10; });

            Assert.Equal(expected, actual.ParamName);
        }

        public static IEnumerable<object[]> ThisException()
        {
            yield return new object[] { 4, "Index 4,4 not eligible for a double4x4 type" };
            yield return new object[] { 5, "Index 5,5 not eligible for a double4x4 type" };
            yield return new object[] { -1, "Index -1,-1 not eligible for a double4x4 type" };
        }

        #endregion

        #region Methods

        #region CreateFromAxisAngle

        [Theory]
        [MemberData(nameof(GetAxisAngle))]
        public void CreateFromAxisAngle_MainAxes(double3 axis, float angle, double4x4 expected)
        {
            var actual = double4x4.CreateFromAxisAngle(axis, M.DegreesToRadians(angle));

            Assert.Equal(expected.M11, actual.M11, 7);
            Assert.Equal(expected.M12, actual.M12, 7);
            Assert.Equal(expected.M13, actual.M13, 7);
            Assert.Equal(expected.M14, actual.M14, 7);
            Assert.Equal(expected.M21, actual.M21, 7);
            Assert.Equal(expected.M22, actual.M22, 7);
            Assert.Equal(expected.M23, actual.M23, 7);
            Assert.Equal(expected.M24, actual.M24, 7);
            Assert.Equal(expected.M31, actual.M31, 7);
            Assert.Equal(expected.M32, actual.M32, 7);
            Assert.Equal(expected.M33, actual.M33, 7);
            Assert.Equal(expected.M34, actual.M34, 7);
            Assert.Equal(expected.M41, actual.M41, 7);
            Assert.Equal(expected.M42, actual.M42, 7);
            Assert.Equal(expected.M43, actual.M43, 7);
            Assert.Equal(expected.M44, actual.M44, 7);
        }

        #endregion

        #region CreateRotation[XYZ]

        [Fact]
        public void CreateRotationX_90Degrees()
        {
            var expected = new double4x4(1, 0, 0, 0, 0, 0, -1, 0, 0, 1, 0, 0, 0, 0, 0, 1);

            var actual = double4x4.CreateRotationX(M.DegreesToRadians(90));

            Assert.Equal(expected.M11, actual.M11, 7);
            Assert.Equal(expected.M12, actual.M12, 7);
            Assert.Equal(expected.M13, actual.M13, 7);
            Assert.Equal(expected.M14, actual.M14, 7);
            Assert.Equal(expected.M21, actual.M21, 7);
            Assert.Equal(expected.M22, actual.M22, 7);
            Assert.Equal(expected.M23, actual.M23, 7);
            Assert.Equal(expected.M24, actual.M24, 7);
            Assert.Equal(expected.M31, actual.M31, 7);
            Assert.Equal(expected.M32, actual.M32, 7);
            Assert.Equal(expected.M33, actual.M33, 7);
            Assert.Equal(expected.M34, actual.M34, 7);
            Assert.Equal(expected.M41, actual.M41, 7);
            Assert.Equal(expected.M42, actual.M42, 7);
            Assert.Equal(expected.M43, actual.M43, 7);
            Assert.Equal(expected.M44, actual.M44, 7);
        }

        [Fact]
        public void CreateRotationY_90Degrees()
        {
            var expected = new double4x4(0, 0, 1, 0, 0, 1, 0, 0, -1, 0, 0, 0, 0, 0, 0, 1);

            var actual = double4x4.CreateRotationY(M.DegreesToRadians(90));

            Assert.Equal(expected.M11, actual.M11, 7);
            Assert.Equal(expected.M12, actual.M12, 7);
            Assert.Equal(expected.M13, actual.M13, 7);
            Assert.Equal(expected.M14, actual.M14, 7);
            Assert.Equal(expected.M21, actual.M21, 7);
            Assert.Equal(expected.M22, actual.M22, 7);
            Assert.Equal(expected.M23, actual.M23, 7);
            Assert.Equal(expected.M24, actual.M24, 7);
            Assert.Equal(expected.M31, actual.M31, 7);
            Assert.Equal(expected.M32, actual.M32, 7);
            Assert.Equal(expected.M33, actual.M33, 7);
            Assert.Equal(expected.M34, actual.M34, 7);
            Assert.Equal(expected.M41, actual.M41, 7);
            Assert.Equal(expected.M42, actual.M42, 7);
            Assert.Equal(expected.M43, actual.M43, 7);
            Assert.Equal(expected.M44, actual.M44, 7);
        }

        [Fact]
        public void CreateRotationZ_90Degrees()
        {
            var expected = new double4x4(0, -1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);

            var actual = double4x4.CreateRotationZ(M.DegreesToRadians(90));

            Assert.Equal(expected.M11, actual.M11, 7);
            Assert.Equal(expected.M12, actual.M12, 7);
            Assert.Equal(expected.M13, actual.M13, 7);
            Assert.Equal(expected.M14, actual.M14, 7);
            Assert.Equal(expected.M21, actual.M21, 7);
            Assert.Equal(expected.M22, actual.M22, 7);
            Assert.Equal(expected.M23, actual.M23, 7);
            Assert.Equal(expected.M24, actual.M24, 7);
            Assert.Equal(expected.M31, actual.M31, 7);
            Assert.Equal(expected.M32, actual.M32, 7);
            Assert.Equal(expected.M33, actual.M33, 7);
            Assert.Equal(expected.M34, actual.M34, 7);
            Assert.Equal(expected.M41, actual.M41, 7);
            Assert.Equal(expected.M42, actual.M42, 7);
            Assert.Equal(expected.M43, actual.M43, 7);
            Assert.Equal(expected.M44, actual.M44, 7);
        }

        #endregion

        #region CreateTranslation

        [Fact]
        public void CreateTranslation_Doubles()
        {
            var expected = new double4x4(1, 0, 0, 4, 0, 1, 0, 3, 0, 0, 1, 2, 0, 0, 0, 1);

            var actual = double4x4.CreateTranslation(4, 3, 2);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CreateTranslation_Vector()
        {
            var expected = new double4x4(1, 0, 0, 4, 0, 1, 0, 3, 0, 0, 1, 2, 0, 0, 0, 1);

            var actual = double4x4.CreateTranslation(new double3(4, 3, 2));

            Assert.Equal(expected, actual);
        }

        #endregion

        #region CreateOrthographic

        [Fact]
        public void CreateOrthographic_Test()
        {
            var expected = new double4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 2, -3, 0, 0, 0, 1);

            var actual = double4x4.CreateOrthographic(2, 2, 1, 2);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CreateOrthographicOffCenter_Test()
        {
            var expected = new double4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 2, -3, 0, 0, 0, 1);

            var actual = double4x4.CreateOrthographicOffCenter(-1, 1, -1, 1, 1, 2);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CreateOrthographicOffCenterRH_Test()
        {
            var expected = new double4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, -2, -3, 0, 0, 0, 1);

            var actual = double4x4.CreateOrthographicOffCenterRH(-1, 1, -1, 1, 1, 2);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region CreatePerspective

        [Fact]
        public void CreatePerspectiveFieldOfView_Result()
        {
            var expected = new double4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 3, -4, 0, 0, 1, 0);

            var actual = double4x4.CreatePerspectiveFieldOfView(M.DegreesToRadians(90), 1, 1, 2);

            Assert.Equal(expected.M11, actual.M11, 7);
            Assert.Equal(expected.M12, actual.M12, 7);
            Assert.Equal(expected.M13, actual.M13, 7);
            Assert.Equal(expected.M14, actual.M14, 7);
            Assert.Equal(expected.M21, actual.M21, 7);
            Assert.Equal(expected.M22, actual.M22, 7);
            Assert.Equal(expected.M23, actual.M23, 7);
            Assert.Equal(expected.M24, actual.M24, 7);
            Assert.Equal(expected.M31, actual.M31, 7);
            Assert.Equal(expected.M32, actual.M32, 7);
            Assert.Equal(expected.M33, actual.M33, 7);
            Assert.Equal(expected.M34, actual.M34, 7);
            Assert.Equal(expected.M41, actual.M41, 7);
            Assert.Equal(expected.M42, actual.M42, 7);
            Assert.Equal(expected.M43, actual.M43, 7);
            Assert.Equal(expected.M44, actual.M44, 7);
        }

        [Fact]
        public void CreatePerspectiveOffCenter_Test()
        {
            var expected = new double4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 3, -4, 0, 0, 1, 0);

            var actual = double4x4.CreatePerspectiveOffCenter(-1, 1, -1, 1, 1, 2);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CreatePerspectiveOffCenterRH_Test()
        {
            var expected = new double4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, -3, -4, 0, 0, -1, 0);

            var actual = double4x4.CreatePerspectiveOffCenterRH(-1, 1, -1, 1, 1, 2);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(GetPerspectiveFOWException))]
        public void CreatePerspectiveFieldOfView_Exceptions(double fovy, double aspect, double zNear, double zFar, string expected)
        {
            var actual = Assert.Throws<ArgumentOutOfRangeException>(() =>
                double4x4.CreatePerspectiveFieldOfView(fovy, aspect, zNear, zFar));

            Assert.Equal(expected, actual.ParamName);
        }

        [Theory]
        [MemberData(nameof(GetPerspectiveOffCenterException))]
        public void CreatePerspectiveOffCenter_Exceptions(double zNear, double zFar, string expected)
        {
            var actual = Assert.Throws<ArgumentOutOfRangeException>(() =>
                double4x4.CreatePerspectiveOffCenter(-1, 1, -1, 1, zNear, zFar));

            Assert.Equal(expected, actual.ParamName);
        }

        [Theory]
        [MemberData(nameof(GetPerspectiveOffCenterException))]
        public void CreatePerspectiveOffCenter_ExceptionsRH(double zNear, double zFar, string expected)
        {
            var actual = Assert.Throws<ArgumentOutOfRangeException>(() =>
                double4x4.CreatePerspectiveOffCenterRH(-1, 1, -1, 1, zNear, zFar));

            Assert.Equal(expected, actual.ParamName);
        }

        #endregion

        #region Scale Functions

        [Fact]
        public void Scale_Double()
        {
            var expected = new double4x4(2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 1);

            var actual = double4x4.Scale(2);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Scale_Vector()
        {
            var expected = new double4x4(1, 0, 0, 0, 0, 2, 0, 0, 0, 0, 3, 0, 0, 0, 0, 1);

            var actual = double4x4.Scale(new double3(1, 2, 3));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Scale_ThreeDoubles()
        {
            var expected = new double4x4(1, 0, 0, 0, 0, 2, 0, 0, 0, 0, 3, 0, 0, 0, 0, 1);

            var actual = double4x4.Scale(1, 2, 3);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region CameraHelper

        [Fact]
        public void LookAt_Vectors()
        {
            var eye = new double3(1, 0, 0);
            var target = new double3(0, 0, 0);
            var up = new double3(0, 1, 0);
            var expected = new double4x4(0, 0, 1, 0, 0, 1, 0, 0, -1, 0, 0, 1, 0, 0, 0, 1);

            var actual = double4x4.LookAt(eye, target, up);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LookAtRH_Vectors()
        {
            var eye = new double3(1, 0, 0);
            var target = new double3(0, 0, 0);
            var up = new double3(0, 1, 0);
            var expected = new double4x4(0, 0, -1, 0, 0, 1, 0, 0, 1, 0, 0, -1, 0, 0, 0, 1);

            var actual = double4x4.LookAtRH(eye, target, up);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LookAt_Double()
        {
            var expected = new double4x4(0, 0, 1, 0, 0, 1, 0, 0, -1, 0, 0, 1, 0, 0, 0, 1);

            var actual = double4x4.LookAt(1, 0, 0, 0, 0, 0, 0, 1, 0);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Arithmetic Functions

        [Theory]
        [MemberData(nameof(GetAddition))]
        public void Add_Static(double4x4 left, double4x4 right, double4x4 expected)
        {
            var actual = double4x4.Add(left, right);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(GetSubtraction))]
        public void Substract_Static(double4x4 left, double4x4 right, double4x4 expected)
        {
            var actual = double4x4.Substract(left, right);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(GetMultiply))]
        public void Mult_Static(double4x4 left, double4x4 right, double4x4 expected)
        {
            var actual = double4x4.Mult(left, right);
        }

        #endregion

        #region Invert

        [Fact]
        public void Invert_Static()
        {
            var mat = new double4x4(1, 0, 0, 1, 0, 1, 0, 1, 0, 0, 1, 1, 0, 0, 0, 1);

            var actual = mat.Invert();

            Assert.Equal(new double4x4(1, 0, 0, -1, 0, 1, 0, -1, 0, 0, 1, -1, 0, 0, 0, 1), actual);
        }

        #endregion

        #region Transpose

        [Fact]
        public void Transpose_Static()
        {
            var mat = new double4x4(1, 0, 0, 1, 0, 1, 0, 1, 0, 0, 1, 1, 0, 0, 0, 1);

            var actual = mat.Transpose();

            Assert.Equal(new double4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1), actual);
        }

        #endregion

        #region Transform

        [Theory]
        [MemberData(nameof(GetTransform4D))]
        public void Transform_4D_Static(double4x4 mat, double4 vec, double4 expected)
        {
            var actual = double4x4.Transform(mat, vec);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(GetTransform4D))]
        public void TransformPremult_4D_Static(double4x4 mat, double4 vec, double4 expected)
        {
            var actual = double4x4.TransformPremult(vec, mat);

            Assert.Equal(expected, -actual);
        }

        [Theory]
        [MemberData(nameof(GetTransform3D))]
        public void Transform_3D_Static(double4x4 mat, double3 vec, double3 expected)
        {
            var actual = double4x4.Transform(mat, vec);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(GetTransform3D))]
        public void TransformPremult_3D_Static(double4x4 mat, double3 vec, double3 expected)
        {
            var actual = double4x4.TransformPremult(vec, mat);

            Assert.Equal(expected, -actual);
        }

        [Fact]
        public void TransformPerspective_3D()
        {
            var mat = new double4x4(1, 0, 0, 0, 0, 0, -1, 0, 0, 1, 0, 0, 0, 0, 0, 2);
            var vec = new double3(0, 1, 0);

            var actual = double4x4.TransformPerspective(mat, vec);

            Assert.Equal(new double3(0, 0, 0.5f), actual);
        }

        #endregion

        #endregion

        #region Operators

        [Theory]
        [MemberData(nameof(GetAddition))]
        public void Add_Operator(double4x4 left, double4x4 right, double4x4 expected)
        {
            var actual = left + right;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(GetSubtraction))]
        public void Subtract_Operator(double4x4 left, double4x4 right, double4x4 expected)
        {
            var actual = left - right;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(GetMultiply))]
        public void Mult_Operator(double4x4 left, double4x4 right, double4x4 expected)
        {
            var actual = left * right;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(GetTransform4D))]
        public void Transform_4D_Operator(double4x4 mat, double4 vec, double4 expected)
        {
            var actual = mat * vec;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(GetTransform4D))]
        public void TransformPremult_4D_Operator(double4x4 mat, double4 vec, double4 expected)
        {
            var actual = vec * mat;

            Assert.Equal(expected, -actual);
        }

        [Theory]
        [MemberData(nameof(GetTransform3D))]
        public void Transform_3D_Operator(double4x4 mat, double3 vec, double3 expected)
        {
            var actual = mat * vec;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(GetTransform3D))]
        public void TransformPremult_3D_Operator(double4x4 mat, double3 vec, double3 expected)
        {
            var actual = vec * mat;

            Assert.Equal(expected, -actual);
        }

        [Fact]
        public void Equality_IsEqual()
        {
            var a = new double4x4(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            var b = new double4x4(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);

            Assert.True(a == b);
        }

        [Fact]
        public void Equality_IsInequal()
        {
            var a = new double4x4(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            var b = new double4x4(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

            Assert.False(a == b);
        }

        [Fact]
        public void Inequality_IsEqual()
        {
            var a = new double4x4(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            var b = new double4x4(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);

            Assert.False(a != b);
        }

        [Fact]
        public void Inequality_IsInequal()
        {
            var a = new double4x4(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            var b = new double4x4(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

            Assert.True(a != b);
        }

        [Fact]
        public void Cast_FromDouble4x4()
        {
            var mat = new double4x4(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);

            var actual = (double4x4)mat;

            Assert.Equal(new double4x4(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1), actual);
        }

        #endregion

        #region Overrides

        [Fact]
        public void ToString_IsString()
        {
            var mat = new double4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);

            var actual = mat.ToString();

            Assert.Equal("(1, 0, 0, 0)\n(0, 1, 0, 0)\n(0, 0, 1, 0)\n(0, 0, 0, 1)", actual);
        }

        //TODO: GetHasCode
        //TODO: Equals(obj)

        #endregion

        #region IEnumerables

        public static IEnumerable<object[]> GetAxisAngle()
        {
            yield return new object[]
                {new double3(1, 0, 0), 90, new double4x4(1, 0, 0, 0, 0, 0, -1, 0, 0, 1, 0, 0, 0, 0, 0, 1)};
            yield return new object[]
                {new double3(0, 1, 0), 90, new double4x4(0, 0, 1, 0, 0, 1, 0, 0, -1, 0, 0, 0, 0, 0, 0, 1)};
            yield return new object[]
                {new double3(0, 0, 1), 90, new double4x4(0, -1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1)};
        }

        public static IEnumerable<object[]> GetMatrixEuler()
        {
            yield return new object[]
                {new double4x4(1, 0, 0, 0, 0, 0, -1, 0, 0, 1, 0, 0, 0, 0, 0, 1), new double3(90, 0, 0)};
            yield return new object[]
                {new double4x4(0, 0, 1, 0, 0, 1, 0, 0, -1, 0, 0, 0, 0, 0, 0, 1), new double3(0, 90, 0)};
            yield return new object[]
                {new double4x4(0, -1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1), new double3(0, 0, 90)};
        }

        public static IEnumerable<object[]> GetPerspectiveFOWException()
        {
            yield return new object[] { 0, 1, 1, 2, "fovy" };
            yield return new object[] { 4, 1, 1, 2, "fovy" };
            yield return new object[] { 1, 0, 1, 2, "aspect" };
            yield return new object[] { 1, 1, 0, 2, "zNear" };
            yield return new object[] { 1, 1, 1, 0, "zFar" };
            yield return new object[] { 1, 1, 2, 1, "zNear" };
        }

        public static IEnumerable<object[]> GetPerspectiveOffCenterException()
        {
            yield return new object[] { 0, 2, "zNear" };
            yield return new object[] { 1, 0, "zFar" };
            yield return new object[] { 2, 1, "zNear" };
        }

        public static IEnumerable<object[]> GetAddition()
        {
            var zero = new double4x4(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            var one = new double4x4(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);

            yield return new object[] { zero, one, one };
            yield return new object[] { one, zero, one };
            yield return new object[] { zero, zero, zero };
            yield return new object[] { one, one, new double4x4(2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2) };
        }

        public static IEnumerable<object[]> GetSubtraction()
        {
            var zero = new double4x4(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            var one = new double4x4(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);

            yield return new object[] { one, zero, one };
            yield return new object[] { zero, zero, zero };
            yield return new object[] { one, one, zero };
        }

        public static IEnumerable<object[]> GetMultiply()
        {
            var zero = new double4x4(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            var one = new double4x4(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            var id = new double4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);

            yield return new object[] { one, zero, zero };
            yield return new object[] { zero, one, zero };
            yield return new object[] { one, one, new double4x4(4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4) };
            yield return new object[] { one, id, one };
            yield return new object[] { id, one, one };
        }

        public static IEnumerable<object[]> GetTransform4D()
        {
            var x = new double4(1, 0, 0, 0);
            var y = new double4(0, 1, 0, 0);
            var z = new double4(0, 0, 1, 0);

            var xRot = new double4x4(1, 0, 0, 0, 0, 0, -1, 0, 0, 1, 0, 0, 0, 0, 0, 1);
            var yRot = new double4x4(0, 0, 1, 0, 0, 1, 0, 0, -1, 0, 0, 0, 0, 0, 0, 1);
            var zRot = new double4x4(0, -1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);

            yield return new object[] { xRot, y, z };
            yield return new object[] { yRot, z, x };
            yield return new object[] { zRot, x, y };
        }

        public static IEnumerable<object[]> GetTransform3D()
        {
            var x = new double3(1, 0, 0);
            var y = new double3(0, 1, 0);
            var z = new double3(0, 0, 1);

            var xRot = new double4x4(1, 0, 0, 0, 0, 0, -1, 0, 0, 1, 0, 0, 0, 0, 0, 1);
            var yRot = new double4x4(0, 0, 1, 0, 0, 1, 0, 0, -1, 0, 0, 0, 0, 0, 0, 1);
            var zRot = new double4x4(0, -1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);

            yield return new object[] { xRot, y, z };
            yield return new object[] { yRot, z, x };
            yield return new object[] { zRot, x, y };
        }

        #endregion
    }
}