using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Windows.Media.Media3D;
using System.ComponentModel;

namespace MyUtility

{
    /// <summary>
    /// �R���g���[���p�w���p�ł��B
    /// </summary>
    public static class ControlHelper_DxMath
    {
        /// <summary>
        /// �I�C���[����N�H�[�^�j�I���ɕϊ����܂��B
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static DxMath.Quaternion ToQuatanionDxMath(this DxMath.Vector3 v)
        {
            Double Rcosx = Math.Cos(v.X / 180.0 * Math.PI / 2.0);
            Double Rcosy = Math.Cos(v.Y / 180.0 * Math.PI / 2.0);
            Double Rcosz = Math.Cos(v.Z / 180.0 * Math.PI / 2.0);

            Double Rsinx = Math.Sin(v.X / 180.0 * Math.PI / 2.0);
            Double Rsiny = Math.Sin(v.Y / 180.0 * Math.PI / 2.0);
            Double Rsinz = Math.Sin(v.Z / 180.0 * Math.PI / 2.0);

            Double w = (Rcosy * Rcosx * Rcosz + Rsiny * Rsinx * Rsinz);
            Double y = (Rsiny * Rcosx * Rcosz - Rcosy * Rsinx * Rsinz);
            Double x = (Rcosy * Rsinx * Rcosz + Rsiny * Rcosx * Rsinz);
            Double z = (Rcosy * Rcosx * Rsinz - Rsiny * Rsinx * Rcosz);

            return new DxMath.Quaternion((float)x, (float)y, (float)z, (float)w);
        }

        /// <summary>
        /// �N�H�[�^�j�I������I�C���[�ɕϊ����܂��B
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public static DxMath.Vector3 ToEularDxMath(this DxMath.Quaternion q)
        {
            Double ry = (Math.Atan2((2.0 * (q.W * q.Y + q.X * q.Z)), (1.0 - 2.0 * (q.X * q.X + q.Y * q.Y))) / Math.PI * 180.0);
            Double rx = (Math.Asin((2.0 * (q.W * q.X - q.Z * q.Y))) / Math.PI * 180.0);
            Double rz = (Math.Atan2((2.0 * (q.W * q.Z + q.X * q.Y)), (1.0 - 2.0 * (q.X * q.X + q.Z * q.Z))) / Math.PI * 180.0);
            return new DxMath.Vector3((float)rx, (float)ry, (float)rz);
        }

        public static float ToRadians(this float degrees)
        {
            return (float)(degrees / 180 * Math.PI);
        }

        public static DxMath.Quaternion AddEular(this DxMath.Quaternion q, DxMath.Vector3 angle)
        {
            var eular = q.ToEularDxMath();
            eular += angle;
            return eular.ToQuatanionDxMath();
        }

        /// <summary>
        /// Vector3�̊e�������w�肵�������_�����Ŏl�̌ܓ�����
        /// </summary>
        /// <param name="vector">�Ώۂ�Vector3</param>
        /// <param name="decimals">�ێ����鏬���_����</param>
        /// <returns>�l�̌ܓ����Vector3</returns>
        public static DxMath.Vector3 RoundVector3(this DxMath.Vector3 vector, int decimals)
        {
            return new DxMath.Vector3(
                (float)Math.Round(vector.X, decimals, MidpointRounding.AwayFromZero),
                (float)Math.Round(vector.Y, decimals, MidpointRounding.AwayFromZero),
                (float)Math.Round(vector.Z, decimals, MidpointRounding.AwayFromZero)
            );
        }

        /// <summary>
        /// Quaternion�̊e�������w�肵�������_�����Ŏl�̌ܓ�����
        /// </summary>
        /// <param name="quaternion">�Ώۂ�Quaternion</param>
        /// <param name="decimals">�ێ����鏬���_����</param>
        /// <returns>�l�̌ܓ����Quaternion</returns>
        public static DxMath.Quaternion RoundQuaternion(this DxMath.Quaternion quaternion, int decimals)
        {
            return new DxMath.Quaternion(
                (float)Math.Round(quaternion.X, decimals, MidpointRounding.AwayFromZero),
                (float)Math.Round(quaternion.Y, decimals, MidpointRounding.AwayFromZero),
                (float)Math.Round(quaternion.Z, decimals, MidpointRounding.AwayFromZero),
                (float)Math.Round(quaternion.W, decimals, MidpointRounding.AwayFromZero)
            );
        }
    }
}