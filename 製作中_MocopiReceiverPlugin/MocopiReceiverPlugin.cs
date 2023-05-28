using DxMath;
using MikuMikuPlugin;
using MMDUtil;
using MyUtility;
using Sony.SMF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Mocopi.Receiver.Core.MocopiUdpReceiver;

namespace MocopiReceiverPlugin
{
    public class MocopiReceiverPlugin : IResidentPlugin, ICanSavePlugin //
    {
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        private frmMain _frmMain = null;

        public Guid GUID => new Guid("{9005F13A-A53B-4BC3-B2C5-70903ADEDB4F}");

        public string Description => "Mocopiと接続するプラグインです。";

        public IWin32Window ApplicationForm { get; set; }

        /// <summary>
        /// ボタンに表示するテキスト
        /// 日本語環境で表示されるテキストです。改行する場合は Environment.NewLineを使用してください。
        /// </summary>
        public string Text => "Mocopi接続";

        public string EnglishText => "MocopiReceiverPlugin";

        public System.Drawing.Image Image => null;// Properties.Resources._32;

        public System.Drawing.Image SmallImage => null;//Properties.Resources._22;

        public Scene Scene { get; set; }

        public void Initialize()
        {
#if DEBUG
            AllocConsole();
#endif
        }

        private object _lockobj = new object();

        public void Enabled()
        {
            MMMUtilility.Initialize(this.ApplicationForm as Form, this.Scene);

            if (this._frmMain != null)
            {
                this._frmMain.Dispose();
                this._frmMain = null;
            }
            this._frmMain = new frmMain(this.Scene, this.ApplicationForm as Form);
            this._frmMain.Show(this.ApplicationForm);

            this._frmMain.BoneInfoReceived += (object s, BoneInfoEventArgs e) =>
            {
                if (_activemodel != null)
                {
                    if (e.Mode == 0)
                    {
                        lock (this._lockobj)
                        {
                            var hash = new Dictionary<Bone, List<MocopiBone>>();
                            foreach (var mbone in e.BoneList)
                            {
                                MocopiBoneEnum enm = (MocopiBoneEnum)e.BoneList.IndexOf(mbone);
                                Bone bone = null;
                                switch (enm)
                                {
                                    case MocopiBoneEnum.root:
                                        bone = _activemodel.Center;
                                        break;

                                    case MocopiBoneEnum.torso_1:
                                        bone = _activemodel.Upper;
                                        break;

                                    case MocopiBoneEnum.torso_2:
                                        break;

                                    case MocopiBoneEnum.torso_3:
                                        bone = _activemodel.Upper;
                                        break;

                                    case MocopiBoneEnum.torso_4:
                                        break;

                                    case MocopiBoneEnum.torso_5:
                                        bone = _activemodel.Upper2;
                                        break;

                                    case MocopiBoneEnum.torso_6:
                                        break;

                                    case MocopiBoneEnum.torso_7:
                                        bone = _activemodel.Neck;
                                        break;

                                    case MocopiBoneEnum.neck_1:
                                        bone = _activemodel.Neck;
                                        break;

                                    case MocopiBoneEnum.neck_2:
                                        bone = _activemodel.Head;
                                        break;

                                    case MocopiBoneEnum.head:
                                        bone = _activemodel.Head;
                                        break;

                                    case MocopiBoneEnum.l_shoulder:
                                        bone = _activemodel.LeftShoulder;
                                        break;

                                    case MocopiBoneEnum.l_up_arm:
                                        bone = _activemodel.LeftUpperArm;
                                        break;

                                    case MocopiBoneEnum.l_low_arm:
                                        bone = _activemodel.LeftLowerArm;
                                        break;

                                    case MocopiBoneEnum.l_hand:
                                        bone = _activemodel.LeftWrist;
                                        break;

                                    case MocopiBoneEnum.r_shoulder:
                                        bone = _activemodel.RightShoulder;
                                        break;

                                    case MocopiBoneEnum.r_up_arm:
                                        bone = _activemodel.RightUpperArm;
                                        break;

                                    case MocopiBoneEnum.r_low_arm:
                                        bone = _activemodel.RightLowerArm;
                                        break;

                                    case MocopiBoneEnum.r_hand:
                                        bone = _activemodel.RightWrist;
                                        break;

                                    case MocopiBoneEnum.l_up_leg:
                                        bone = _activemodel.LeftUpperLeg;
                                        break;

                                    case MocopiBoneEnum.l_low_leg:
                                        bone = _activemodel.LeftLowerLeg;
                                        break;

                                    case MocopiBoneEnum.l_foot:
                                        bone = _activemodel.LeftAnkle;
                                        break;

                                    case MocopiBoneEnum.l_toes:
                                        break;

                                    case MocopiBoneEnum.r_up_leg:
                                        bone = _activemodel.RightUpperLeg;
                                        break;

                                    case MocopiBoneEnum.r_low_leg:
                                        bone = _activemodel.RightLowerLeg;
                                        break;

                                    case MocopiBoneEnum.r_foot:
                                        bone = _activemodel.RightAnkle;
                                        break;

                                    case MocopiBoneEnum.r_toes:
                                        break;

                                    default:
                                        break;
                                }
                                if (bone != null)
                                {
                                    var lst = new List<MocopiBone>();
                                    if (hash.ContainsKey(bone))
                                        lst = hash[bone];
                                    else
                                        hash.Add(bone, lst);
                                    lst.Add(mbone);
                                }
                            }
                            foreach (var kvp in hash)
                            {
                                var bone = kvp.Key;
                                var v3 = new Vector3();
                                var pos = new Vector3();
                                foreach (var item in kvp.Value)
                                {
                                    var eular = item.Rotation.ToEularDxMath();
                                    v3 += new Vector3(eular.X, eular.Y, eular.Z);
                                    pos += item.Position;
                                }
                                var rot = v3.ToQuatanionDxMath();
                                if (kvp.Value.FirstOrDefault().BoneID == 0)
                                    bone.CurrentLocalMotion = new MotionData(pos, rot);
                                else
                                    bone.CurrentLocalMotion = new MotionData(new DxMath.Vector3(), rot);

                                if (kvp.Value.FirstOrDefault().BoneIDEnum == MocopiBoneEnum.l_up_arm)
                                {
                                    Console.WriteLine(v3.ToString());
                                    Debug.WriteLine($"{v3.ToString()} / {kvp.Value.FirstOrDefault().Rotation.ToString()}");
                                }
                            }
                        }
                    }
                }
            };
        }

        public void Disabled()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (this._frmMain != null)
            {
                this._frmMain.Close();
                this._frmMain.Dispose();
                this._frmMain = null;
            }
        }

        private string _prevActiveModelName = String.Empty;

        private ActiveModelEntity _activemodel = null;

        public void Update(float Frame, float ElapsedTime)
        {
            if (this.Scene.State != SceneState.Editing)
                return;

            if (this.Scene.ActiveModel == null)
            {
                this._activemodel = null;
            }
            else
            {
                if (_activemodel == null || _activemodel.Model.ID != this.Scene.ActiveModel.ID)
                {
                    _activemodel = new ActiveModelEntity(this.Scene.ActiveModel);
                }
            }
        }

        public Stream OnSaveProject()
        {
            MemoryStream stream = new MemoryStream();
            //if (this._frmMain != null)
            //{
            //    _memo = this._frmMain.GetArgs();
            //}
            //if (_memo != null)
            //{
            //    //テキストをbyte配列にして保存
            //    var filepath = _memo.FolderPath;
            //    int toggle = _memo.Toggle ? 1 : 0;
            //    int addframe = _memo.AddFrameNameToFileName ? 1 : 0;
            //    int topmost = _memo.TopMost ? 1 : 0;
            //    string fmt = _memo.PictureFormat.ToString();
            //    int lockmotion = _memo.LockMotion ? 1 : 0;
            //    var sb = new System.Text.StringBuilder();
            //    sb.AppendLine($"\"{filepath}\",{toggle},{addframe},{topmost},{fmt},{lockmotion}");

            //    byte[] buffer = System.Text.Encoding.Unicode.GetBytes(sb.ToString());
            //    stream.Write(buffer, 0, buffer.Length);
            //}

            return stream;
        }

        public void OnLoadProject(Stream stream)
        {
            byte[] buffer;

            //やらなくてもいいですが、まあ一応。
            stream.Seek(0, SeekOrigin.Begin);

            //保存されたテキストをStreamから取得
            buffer = new byte[stream.Length];
            //int count = stream.Read(buffer, 0, buffer.Length);
            //var folderPath = string.Empty;
            //int toggle = 0;
            //int addrame = 0;
            //int topmost = 0;
            //int lockMotion = 0;
            //PictureFormat fmt = PictureFormat.png;
            //if (count == buffer.Length)
            //{
            //    var tmp = System.Text.Encoding.Unicode.GetString(buffer);
            //    var array = tmp.Split(',');
            //    if (array.Length > 0)
            //        folderPath = array[0].Replace($"\"", "");
            //    if (array.Length > 1)
            //        toggle = array[1].ToInt();
            //    if (array.Length > 2)
            //        addrame = array[2].ToInt();
            //    if (array.Length > 3)
            //        topmost = array[3].ToInt();
            //    if (array.Length > 4)
            //    {
            //        var buf = array[4].Trim().ToLower();
            //        foreach (PictureFormat xfmt in Enum.GetValues(typeof(PictureFormat)).Cast<PictureFormat>())
            //        {
            //            if (buf == xfmt.ToString().ToLower())
            //            {
            //                fmt = xfmt;
            //                break;
            //            }
            //        }
            //    }
            //    if (array.Length > 5)
            //        lockMotion = array[5].ToInt();

            //    this._memo = new Args()
            //    {
            //        FolderPath = folderPath,
            //        Toggle = (toggle > 0),
            //        AddFrameNameToFileName = (addrame > 0),
            //        TopMost = (topmost > 0),
            //        PictureFormat = fmt,
            //        LockMotion = (lockMotion > 0)
            //    };
            //}
            //if (this._frmMain != null)
            //{
            //    this._frmMain.SetArgs(this._memo);
            //}
        }
    }

    public class ActiveModelEntity
    {
        public Model Model { get; }

        public Bone Center { get; } = null;
        public Bone Upper { get; } = null;
        public Bone Upper2 { get; } = null;
        public Bone Neck { get; } = null;
        public Bone Head { get; } = null;
        public Bone LeftShoulder { get; } = null;
        public Bone LeftUpperArm { get; } = null;
        public Bone LeftLowerArm { get; } = null;
        public Bone LeftWrist { get; } = null;

        public Bone RightShoulder { get; } = null;
        public Bone RightUpperArm { get; } = null;
        public Bone RightLowerArm { get; } = null;
        public Bone RightWrist { get; } = null;

        public Bone LeftUpperLeg { get; } = null;
        public Bone LeftLowerLeg { get; } = null;
        public Bone LeftAnkle { get; } = null;

        public Bone RightUpperLeg { get; } = null;
        public Bone RightLowerLeg { get; } = null;
        public Bone RightAnkle { get; } = null;

        public ActiveModelEntity(Model activeModel)
        {
            this.Model = activeModel;
            if (this.Model != null)
            {
                foreach (var bone in this.Model.Bones)
                {
                    switch (bone.Name)
                    {
                        case "センター":
                            this.Center = bone;
                            break;

                        case "上半身":
                            this.Upper = bone;
                            break;

                        case "上半身2":
                            this.Upper2 = bone;
                            break;

                        case "首":
                            this.Neck = bone;
                            break;

                        case "頭":
                            this.Head = bone;
                            break;

                        case "右肩":
                            this.LeftShoulder = bone;
                            break;

                        case "右腕":
                            this.LeftUpperArm = bone;
                            break;

                        case "右ひじ":
                            this.LeftLowerArm = bone;
                            break;

                        case "右手首":
                            this.LeftWrist = bone;
                            break;

                        case "左肩":
                            this.RightShoulder = bone;
                            break;

                        case "左腕":
                            this.RightUpperArm = bone;
                            break;

                        case "左ひじ":
                            this.RightLowerArm = bone;
                            break;

                        case "左手首":
                            this.RightWrist = bone;
                            break;

                        case "右足":
                            this.LeftUpperLeg = bone;
                            break;

                        case "右ひざ":
                            this.LeftLowerLeg = bone;
                            break;

                        case "右足首":
                            this.LeftAnkle = bone;
                            break;

                        case "左足":
                            this.RightUpperLeg = bone;
                            break;

                        case "左ひざ":
                            this.RightLowerLeg = bone;
                            break;

                        case "左足首":
                            this.RightAnkle = bone;
                            break;

                        default:
                            break;
                    }
                }
            }
        }
    }
}