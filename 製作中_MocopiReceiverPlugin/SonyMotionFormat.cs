/*
 * Copyright 2022 Sony Corporation
 */

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sony.SMF
{
    /// <summary>
    /// Class that converts received data so that it can be used (use "sony_motion_format.dll")
    /// </summary>
    public sealed class SonyMotionFormat
    {
        #region --Fields--

        /// <summary>
        /// Constant of library name
        /// </summary>
#if UNITY_IOS && !UNITY_EDITOR_OSX
        public const string SONY_MOTION_FORMAT_LIBRARY_NAME = "__Internal";
#else
        public const string SONY_MOTION_FORMAT_LIBRARY_NAME = "sony_motion_format\\sony_motion_format";
#endif

        #endregion --Fields--

        #region --Methods--

        /// <summary>
        /// Converts bytes to skeleton definition
        /// </summary>
        /// <param name="bytes_size">Byte size</param>
        /// <param name="bytes">Byte data</param>
        /// <param name="sender_ip">Sender IP address</param>
        /// <param name="sender_port">Sender port number</param>
        /// <param name="size">Size</param>
        /// <param name="joint_ids">Id of joints</param>
        /// <param name="parent_joint_ids">Id of parent joints</param>
        /// <param name="rotations_x">rotations in the X direction</param>
        /// <param name="rotations_y">rotations in the Y direction</param>
        /// <param name="rotations_z">rotations in the Z direction</param>
        /// <param name="rotations_w">rotations in the W direction</param>
        /// <param name="positions_x">X coordinate of position</param>
        /// <param name="positions_y">Y coordinate of position</param>
        /// <param name="positions_z">Z coordinate of position</param>
        /// <returns>Whether the conversion was successful or not</returns>
        [DllImport(SONY_MOTION_FORMAT_LIBRARY_NAME)]
        public static extern bool ConvertBytesToSkeletonDefinition(
            int bytes_size,
            byte[] bytes,
            out ulong sender_ip,
            out int sender_port,
            out int size,
            out IntPtr joint_ids,
            out IntPtr parent_joint_ids,
            out IntPtr rotations_x,
            out IntPtr rotations_y,
            out IntPtr rotations_z,
            out IntPtr rotations_w,
            out IntPtr positions_x,
            out IntPtr positions_y,
            out IntPtr positions_z
        );

        /// <summary>
        /// Converts bytes to frame data
        /// </summary>
        /// <param name="bytes_size">Byte size</param>
        /// <param name="bytes">Byte data</param>
        /// <param name="sender_ip">Sender IP address</param>
        /// <param name="sender_port">Sender port number</param>
        /// <param name="frame_id">Frame Id</param>
        /// <param name="timestamp">Timestamp</param>
        /// <param name="size">Size</param>
        /// <param name="joint_ids">Id of joints</param>
        /// <param name="rotations_x">rotations in the X direction</param>
        /// <param name="rotations_y">rotations in the Y direction</param>
        /// <param name="rotations_z">rotations in the Z direction</param>
        /// <param name="rotations_w">rotations in the W direction</param>
        /// <param name="positions_x">X coordinate of position</param>
        /// <param name="positions_y">Y coordinate of position</param>
        /// <param name="positions_z">Z coordinate of position</param>
        /// <returns>Whether the conversion was successful or not</returns>
        [DllImport(SONY_MOTION_FORMAT_LIBRARY_NAME)]
        public static extern bool ConvertBytesToFrameData(
            int bytes_size,
            byte[] bytes,
            out ulong sender_ip,
            out int sender_port,
            out int frame_id,
            out float timestamp,
            out int size,
            out IntPtr joint_ids,
            out IntPtr rotations_x,
            out IntPtr rotations_y,
            out IntPtr rotations_z,
            out IntPtr rotations_w,
            out IntPtr positions_x,
            out IntPtr positions_y,
            out IntPtr positions_z
        );

        /// <summary>
        /// Judges if the bytes are in SonyMotionFormat's format
        /// </summary>
        /// <param name="bytes_size">Byte size</param>
        /// <param name="bytes">Byte data</param>
        /// <returns>Correct format or not</returns>
        [DllImport(SONY_MOTION_FORMAT_LIBRARY_NAME)]
        public static extern bool IsSmfBytes(
            int bytes_size,
            byte[] bytes
        );

        /// <summary>
        /// Judges if the bytes are in skeleton definition
        /// </summary>
        /// <param name="bytes_size">Byte size</param>
        /// <param name="bytes">Byte data</param>
        /// <returns>whether it is a skeleton definition</returns>
        [DllImport(SONY_MOTION_FORMAT_LIBRARY_NAME)]
        public static extern bool IsSkeletonDefinitionBytes(
            int bytes_size,
            byte[] bytes
        );

        /// <summary>
        /// Judges if the bytes are in frame data
        /// </summary>
        /// <param name="bytes_size">Byte size</param>
        /// <param name="bytes">Byte data</param>
        /// <returns>whether it is a frame data</returns>
        [DllImport(SONY_MOTION_FORMAT_LIBRARY_NAME)]
        public static extern bool IsFrameDataBytes(
            int bytes_size,
            byte[] bytes
        );

        #endregion --Methods--
    }

    /// <summary>
    /// Class that converts received data so that it can be used (use "sony_motion_format.dll")
    /// </summary>
    public sealed class SonyMotionFormat2
    {
        #region --Methods--

        ///// <summary>
        ///// Converts bytes to skeleton definition
        ///// </summary>
        ///// <param name="bytes_size">Byte size</param>
        ///// <param name="bytes">Byte data</param>
        ///// <param name="sender_ip">Sender IP address</param>
        ///// <param name="sender_port">Sender port number</param>
        ///// <param name="size">Size</param>
        ///// <param name="joint_ids">Id of joints</param>
        ///// <param name="parent_joint_ids">Id of parent joints</param>
        ///// <param name="rotations_x">rotations in the X direction</param>
        ///// <param name="rotations_y">rotations in the Y direction</param>
        ///// <param name="rotations_z">rotations in the Z direction</param>
        ///// <param name="rotations_w">rotations in the W direction</param>
        ///// <param name="positions_x">X coordinate of position</param>
        ///// <param name="positions_y">Y coordinate of position</param>
        ///// <param name="positions_z">Z coordinate of position</param>
        ///// <returns>Whether the conversion was successful or not</returns>
        //public static bool ConvertBytesToSkeletonDefinition(
        //    int bytes_size,
        //    byte[] bytes,
        //    out ulong sender_ip,
        //    out int sender_port,
        //    out int size,
        //    out IntPtr joint_ids,
        //    out IntPtr parent_joint_ids,
        //    out IntPtr rotations_x,
        //    out IntPtr rotations_y,
        //    out IntPtr rotations_z,
        //    out IntPtr rotations_w,
        //    out IntPtr positions_x,
        //    out IntPtr positions_y,
        //    out IntPtr positions_z
        //)
        //{
        //}

        /// <summary>
        /// Converts bytes to frame data
        /// </summary>
        /// <param name="bytes_size">Byte size</param>
        /// <param name="bytes">Byte data</param>
        /// <param name="sender_ip">Sender IP address</param>
        /// <param name="sender_port">Sender port number</param>
        /// <param name="frame_id">Frame Id</param>
        /// <param name="timestamp">Timestamp</param>
        /// <param name="size">Size</param>
        /// <param name="joint_ids">Id of joints</param>
        /// <param name="rotations_x">rotations in the X direction</param>
        /// <param name="rotations_y">rotations in the Y direction</param>
        /// <param name="rotations_z">rotations in the Z direction</param>
        /// <param name="rotations_w">rotations in the W direction</param>
        /// <param name="positions_x">X coordinate of position</param>
        /// <param name="positions_y">Y coordinate of position</param>
        /// <param name="positions_z">Z coordinate of position</param>
        /// <returns>Whether the conversion was successful or not</returns>

        public static bool ConvertBytesToFrameData(
            int bytes_size,
            byte[] bytes,
            out ulong sender_ip,
            out int sender_port,
            out int frame_id,
            out float timestamp,
            out List<MocopiBone> bones
        )
        {
            var ip = GetSubArray(bytes, 59, 8);
            var port = GetSubArray(bytes, 75, 2);
            var fnum = GetSubArray(bytes, 93, 4);
            var time = GetSubArray(bytes, 105, 4);
            sender_ip = BitConverter.ToUInt64(ip, 0);
            sender_port = BitConverter.ToInt16(port, 0);
            timestamp = BitConverter.ToSingle(time, 0);
            frame_id = BitConverter.ToInt32(fnum, 0);

            bones = new List<MocopiBone>();
            var idx = 121;
            for (int i = 0; i < 27; i++)
            {
                var bnid = GetSubArray(bytes, idx + 12, 2);
                int boneId = BitConverter.ToInt16(bnid, 0);
                var tmp = idx + 22;

                var q = new DxMath.Quaternion();
                var p = new DxMath.Vector3();
                for (int j = 0; j < 7; j++)
                {
                    var trans = GetSubArray(bytes, tmp, 4);
                    switch (j)
                    {
                        case 0:
                            q.X = BitConverter.ToSingle(trans, 0);
                            break;

                        case 1:
                            q.Y = BitConverter.ToSingle(trans, 0);
                            break;

                        case 2:
                            q.Z = BitConverter.ToSingle(trans, 0);
                            break;

                        case 3:
                            q.W = BitConverter.ToSingle(trans, 0);
                            break;

                        case 4:
                            p.X = BitConverter.ToSingle(trans, 0);
                            break;

                        case 5:
                            p.Y = BitConverter.ToSingle(trans, 0);
                            break;

                        case 6:
                            p.Z = BitConverter.ToSingle(trans, 0);
                            break;

                        default:
                            break;
                    }
                    tmp += 4;
                }
                var c = new MocopiBone(boneId, q.X, q.Y, q.Z, q.W, p.X, p.Y, p.Z);
                bones.Add(c);
                idx += 54;
            }

            return true;
        }

        // バイト配列から一部分を抜き出す
        private static byte[] GetSubArray(byte[] src, int startIndex, int count)
        {
            byte[] dst = new byte[count];
            Array.Copy(src, startIndex, dst, 0, count);
            return dst;
        }

        /// <summary>
        /// Judges if the bytes are in SonyMotionFormat's format
        /// </summary>
        /// <param name="bytes_size">Byte size</param>
        /// <param name="bytes">Byte data</param>
        /// <returns>Correct format or not</returns>
        public static bool IsSmfBytes(
            int bytes_size,
            byte[] bytes
        )
        {
            return true;
        }

        /// <summary>
        /// Judges if the bytes are in skeleton definition
        /// </summary>
        /// <param name="bytes_size">Byte size</param>
        /// <param name="bytes">Byte data</param>
        /// <returns>whether it is a skeleton definition</returns>
        public static bool IsSkeletonDefinitionBytes(
            int bytes_size,
            byte[] bytes
        )
        {
            return (bytes_size == 1575);
        }

        /// <summary>
        /// Judges if the bytes are in frame data
        /// </summary>
        /// <param name="bytes_size">Byte size</param>
        /// <param name="bytes">Byte data</param>
        /// <returns>whether it is a frame data</returns>
        public static bool IsFrameDataBytes(
            int bytes_size,
            byte[] bytes
        )
        {
            return (bytes_size == 1575);
        }

        #endregion --Methods--
    }

    public class MocopiBone
    {
        public MocopiBoneEnum BoneIDEnum => (MocopiBoneEnum)this.BoneID;
        public int BoneID { get; }
        public DxMath.Quaternion Rotation { get; }
        public DxMath.Vector3 Position { get; }

        public MocopiBone(int boneId, float rotationsX, float rotationsY, float rotationsZ, float rotationsW,
               float positionX, float positionY, float positionZ)
        {
            this.BoneID = boneId;
            this.Rotation = new DxMath.Quaternion(rotationsX, rotationsY, rotationsZ, rotationsW);
            this.Position = new DxMath.Vector3(positionX, positionY, positionZ);
        }
    }

    public enum MocopiBoneEnum
    {
        root = 0,
        torso_1 = 1,
        torso_2 = 2,
        torso_3 = 3,
        torso_4 = 4,
        torso_5 = 5,
        torso_6 = 6,
        torso_7 = 7,
        neck_1 = 8,
        neck_2 = 9,
        head = 10,
        l_shoulder = 11,
        l_up_arm = 12,
        l_low_arm = 13,
        l_hand = 14,
        r_shoulder = 15,
        r_up_arm = 16,
        r_low_arm = 17,
        r_hand = 18,
        l_up_leg = 19,
        l_low_leg = 20,
        l_foot = 21,
        l_toes = 22,
        r_up_leg = 23,
        r_low_leg = 24,
        r_foot = 25,
        r_toes = 26,
    }
}