using DxMath;
using MikuMikuPlugin;
using MyUtility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApplyOffsetRegidentPlugin
{
    public class ApplyOffsetRegidentPlugin : IResidentPlugin
    {
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        /// <summary>
        /// このプラグインのGUID
        /// </summary>
        public Guid GUID
        {
            get { return new Guid("3BE2A23A-5E2E-445F-9E40-3676C6E71748"); }
        }

        /// <summary>
        /// プラグインの名前や作者名、プラグインの説明
        /// </summary>
        public string Description
        {
            get { return "オフセット追加(常駐)"; }
        }

        /// <summary>
        /// メインフォーム
        /// MikuMikuMoving側から与えられます。
        /// ダイアログ表示やメッセージ表示に使用してください。
        /// </summary>
        public IWin32Window ApplicationForm { get; set; }

        /// <summary>
        /// ボタンに表示するテキスト
        /// 日本語環境で表示されるテキストです。改行する場合は Environment.NewLineを使用してください。
        /// </summary>
        public string Text
        {
            get { return "オフセット追加(常駐)"; }
        }

        /// <summary>
        /// ボタンに表示する英語テキスト
        /// 日本以外の環境で表示されるテキストです。
        /// </summary>
        public string EnglishText
        {
            get { return "AddOffsetRegident"; }
        }

        /// <summary>
        /// ボタンに表示するアイコン画像(32x32)
        /// nullだとデフォルト画像が表示されます。
        /// </summary>
        public Image Image
        {
            get { return Properties.Resources._32; }
        }

        /// <summary>
        /// 中コマンドバーに表示するアイコン画像(20x20)
        /// nullだとデフォルト画像が表示されます。
        /// </summary>
        public Image SmallImage
        {
            get { return Properties.Resources._22; }
        }

        /// <summary>
        /// シーンオブジェクト
        /// MikuMikuMoving側から与えられます。
        /// MikuMikuMovingのモデルやアクセサリといったオブジェクトにアクセスできます。
        /// </summary>
        public Scene Scene { get; set; }

        private frmMainMMM _frm = null;

        public void Disabled()
        {
            if (_frm != null)
            {
                _frm.Executed -= EventInvoked;
                _frm?.Dispose();
                _frm = null;
            }
        }

        public void Dispose()
        {
            if (_frm != null)
            {
                _frm.Executed -= EventInvoked;
                _frm?.Dispose();
                _frm = null;
            }
        }

        public void Enabled()
        {
            _selectedModel = null;
            _frm?.Dispose();
            _frm = null;
            _frm = new frmMainMMM(this.Scene);
            _frm.Executed += EventInvoked;
            _frm.Show(ApplicationForm);
        }

        public void Initialize()
        {
#if DEBUG
            AllocConsole();
#endif
        }

        private Model _selectedModel = null;

        public void Update(float Frame, float ElapsedTime)
        {
            if (Scene.ActiveModel?.ID != _selectedModel?.ID)
            {
                _selectedModel = Scene.ActiveModel;
                _frm.Text = $"オフセット付加({_selectedModel.Name})";
                _frm.ApplyCount(0, 0);
            }
            else if (Scene.ActiveModel == null)
            {
                _frm.Text = $"オフセット付加";
                _frm.ApplyCount(0, 0);
            }
            if (_selectedModel != null)
            {
                var selectedLayers = _selectedModel.Bones.SelectMany(n => n.SelectedLayers);
                if (selectedLayers != null)
                {
                    var i = selectedLayers.Sum(n => n.SelectedFrames.Count());
                    _frm.ApplyCount(0, i);
                }
            }
        }

        private void EventInvoked(object sender, ExecutedEventArgs e)
        {
            Args setting = e.Args;

            if (_selectedModel == null)
                return;

            var q = Quaternion.RotationYawPitchRoll((setting.Rotation.Y.ToRadians()), (setting.Rotation.X.ToRadians()), (setting.Rotation.Z.ToRadians()));
            foreach (var bone in _selectedModel.Bones)
            {
                var rotationOffset = new Lazy<Quaternion>(() => GetLocalRotation(setting.IsLocalR, bone, setting.Rotation, q));
                foreach (var i in bone.SelectedLayers)
                {
                    var local = i.CurrentLocalMotion;
                    var newLocal = new MotionData(local.Move, (bone.BoneFlags & BoneType.Rotate) != 0
                        ? setting.IsLocalR ? Quaternion.Multiply(local.Rotation, rotationOffset.Value) : Quaternion.Multiply(rotationOffset.Value, local.Rotation)
                        : local.Rotation);

                    if ((bone.BoneFlags & BoneType.XYZ) != 0)
                        newLocal.Move += GetLocalPosition(setting.IsLocalL, bone, setting.Position, newLocal.Rotation);

                    //reset += () => i.CurrentLocalMotion = local;
                    i.CurrentLocalMotion = newLocal;
                }

                if (e.Execute)
                    foreach (var i in bone.Layers.SelectMany(_ => _.SelectedFrames))
                    {
                        var resetPosition = i.Position;
                        var resetQuaternion = i.Quaternion;

                        //reset += () =>
                        //{
                        //    i.Position = resetPosition;
                        //    i.Quaternion = resetQuaternion;
                        //};

                        i.Quaternion = setting.IsLocalR ? Quaternion.Multiply(i.Quaternion, rotationOffset.Value) : Quaternion.Multiply(rotationOffset.Value, i.Quaternion);
                        i.Position += GetLocalPosition(setting.IsLocalL, bone, setting.Position, i.Quaternion);
                    }
            }

            ((Form)this.ApplicationForm).Refresh();
        }

        private static Vector3 GetLocalPosition(bool isLocal, Bone bone, Vector3 position, Quaternion quaternion)
        {
            if (!isLocal)
                return position;

            var rt = Vector3.Transform(bone == null ? position : Vector3.TransformCoordinate(position, GetLocalMatrix(bone)), quaternion);

            return new Vector3(rt.X, rt.Y, rt.Z);
        }

        private static Matrix GetLocalMatrix(Bone bone)
        {
            if ((bone.BoneFlags & BoneType.LocalAxis) == 0 &&
                bone.LocalAxisX == Vector3.UnitX &&
                bone.LocalAxisY == Vector3.UnitY &&
                bone.LocalAxisZ == Vector3.UnitZ)
                return Matrix.Identity;

            return Matrix.LookAtLH(Vector3.Zero, bone.LocalAxisZ, bone.LocalAxisY);
        }

        private static Quaternion GetLocalRotation(bool isLocal, Bone bone, Vector3 rotation, Quaternion quaternion)
        {
            if (!isLocal ||
                (bone.BoneFlags & BoneType.LocalAxis) == 0 &&
                bone.LocalAxisX == Vector3.UnitX &&
                bone.LocalAxisY == Vector3.UnitY &&
                bone.LocalAxisZ == Vector3.UnitZ)
                return quaternion;

            return Quaternion.Multiply(Quaternion.Multiply(Quaternion.RotationAxis(bone.LocalAxisY, rotation.Y), Quaternion.RotationAxis(bone.LocalAxisX, rotation.X)), Quaternion.RotationAxis(bone.LocalAxisZ, rotation.Z));
        }
    }
}