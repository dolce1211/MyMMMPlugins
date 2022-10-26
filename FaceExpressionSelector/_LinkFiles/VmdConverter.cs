using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;
using MyUtility;

namespace MMDUtil
{
    public class VmdConverter
    {
        //ヘッダ部
        public const int _INFOLENGTH = 30;

        public const int _MODELNAMELENGTH = 20;
        public const int _FRAMEDATANUMLENGTH = 4;

        //フレームデータ部
        public const int _FRAMELENGTH = 111;

        public const int _BONENAMELENGTH = 15;
        public const int _FRANUMLENGTH = 4;
        public const int _AXISLENGTH = 4;
        public const int _QUOTALENGTH = 4;
        public const int _HOKANLENGTH = 64;

        public const int _MORPHLENGTH = 23;
        public const int _WEIGHT = 4;

        public static VmdEntity ReadFile(string path)
        {
            if (!File.Exists(path)) { return null; }
            if (System.IO.Path.GetExtension(path).ToLower() != ".vmd") { return null; }

            //vmdファイルをbyte配列へ
            byte[] bs = new byte[0];
            using (FileStream fs = new FileStream(path
                                                , FileMode.Open
                                                , FileAccess.Read))
            {
                bs = new byte[fs.Length];
                fs.Read(bs, 0, bs.Length);
            }

            return ReadFileInternal(bs);
        }

        protected static VmdEntity ReadFileInternal(byte[] bs)
        {
            int pos = 0;

            //ヘッダ部を読み込み

            byte[] info = new byte[_INFOLENGTH];
            for (int i = 0; i < _INFOLENGTH - 1; i++)
            {
                info[i] = bs[pos + i];
            }
            pos += _INFOLENGTH;

            byte[] modelName = new byte[_MODELNAMELENGTH];
            for (int i = 0; i < _MODELNAMELENGTH - 1; i++)
            {
                modelName[i] = bs[pos + i];
            }
            pos += _MODELNAMELENGTH;

            byte[] frameDatanum = new byte[_FRAMEDATANUMLENGTH];
            for (int i = 0; i < _FRAMEDATANUMLENGTH - 1; i++)
            {
                frameDatanum[i] = bs[pos + i];
            }
            pos += _FRAMEDATANUMLENGTH;

            VmdEntity result = new VmdEntity(info, modelName, frameDatanum);

            //データ部を読み込み
            while ((pos + _FRAMELENGTH) < bs.Length)
            {
                //指定されたボーン件数に達したら抜ける  2019/11/23
                if (result.KeyList.Count >= result.FrameDataNum)
                    break;

                //ボーン名　
                byte[] boneName = new byte[_BONENAMELENGTH];
                for (int i = 0; i < _BONENAMELENGTH; i++)
                {
                    boneName[i] = bs[pos + i];
                }
                pos += _BONENAMELENGTH;

                //フレーム番号
                byte[] frameNum = new byte[_FRANUMLENGTH];
                for (int i = 0; i < _FRANUMLENGTH; i++)
                {
                    frameNum[i] = bs[pos + i];
                }
                pos += _FRANUMLENGTH;

                //X軸座標
                byte[] xAxis = new byte[_AXISLENGTH];
                for (int i = 0; i < _AXISLENGTH; i++)
                {
                    xAxis[i] = bs[pos + i];
                }
                pos += _AXISLENGTH;

                //Y軸座標
                byte[] yAxis = new byte[_AXISLENGTH];
                for (int i = 0; i < _AXISLENGTH; i++)
                {
                    yAxis[i] = bs[pos + i];
                }
                pos += _AXISLENGTH;

                //Z軸座標
                byte[] zAxis = new byte[_AXISLENGTH];
                for (int i = 0; i < _AXISLENGTH; i++)
                {
                    zAxis[i] = bs[pos + i];
                }
                pos += _AXISLENGTH;

                //X
                byte[] xRotate = new byte[_AXISLENGTH];
                for (int i = 0; i < _AXISLENGTH; i++)
                {
                    xRotate[i] = bs[pos + i];
                }
                pos += _AXISLENGTH;

                //Y
                byte[] yRotate = new byte[_AXISLENGTH];
                for (int i = 0; i < _AXISLENGTH; i++)
                {
                    yRotate[i] = bs[pos + i];
                }
                pos += _AXISLENGTH;

                //Z
                byte[] zRotate = new byte[_AXISLENGTH];
                for (int i = 0; i < _AXISLENGTH; i++)
                {
                    zRotate[i] = bs[pos + i];
                }
                pos += _AXISLENGTH;

                //W
                byte[] wRotate = new byte[_AXISLENGTH];
                for (int i = 0; i < _AXISLENGTH; i++)
                {
                    wRotate[i] = bs[pos + i];
                }
                pos += _AXISLENGTH;

                //補完パラメータ
                byte[] hokan = new byte[_HOKANLENGTH];
                for (int i = 0; i < _HOKANLENGTH; i++)
                {
                    hokan[i] = bs[pos + i];
                }
                pos += _HOKANLENGTH;

                VmdKeyEntity keyEntity = new VmdKeyEntity(boneName, frameNum, xAxis, yAxis, zAxis, xRotate, yRotate, zRotate, wRotate, hokan);

                result.KeyList.Add(keyEntity);
            }

            byte[] frameMorphNum = new byte[_FRAMEDATANUMLENGTH];
            for (int i = 0; i < _FRAMEDATANUMLENGTH - 1; i++)
            {
                frameMorphNum[i] = bs[pos + i];
            }
            pos += _FRAMEDATANUMLENGTH;

            result.FrameMorphNum = frameMorphNum.ToIntFromByte();
            //2019/11/23 モーフをサポートしていなかった…！
            while ((pos + _MORPHLENGTH) < bs.Length)
            {
                //指定されたボーン件数に達したら抜ける  2019/11/23
                if (result.MorphList.Count >= result.FrameMorphNum)
                    break;

                //モーフ名　
                byte[] morphName = new byte[_BONENAMELENGTH];
                for (int i = 0; i < _BONENAMELENGTH; i++)
                {
                    morphName[i] = bs[pos + i];
                }
                pos += _BONENAMELENGTH;

                //フレーム番号
                byte[] frameNum = new byte[_FRANUMLENGTH];
                for (int i = 0; i < _FRANUMLENGTH; i++)
                {
                    frameNum[i] = bs[pos + i];
                }
                pos += _FRANUMLENGTH;

                //ウェイト
                byte[] weight = new byte[_WEIGHT];
                for (int i = 0; i < _WEIGHT; i++)
                {
                    weight[i] = bs[pos + i];
                }
                pos += _WEIGHT;

                VmdKeyEntity keyEntity = new VmdKeyEntity(morphName, frameNum, weight);

                result.MorphList.Add(keyEntity);
            }

            /*
            byte[] testbyte = result.Output();

            for (int i = 0; i < testbyte.Length ; i++)
            {
                if(bs[i]!= testbyte[i])
                {
                    i = i * 1;
                }
            }

            */
            return result;
        }

        /// <summary>
        /// ratioの割合に応じた全親のキーをでっちあげます。
        /// </summary>
        /// <param name="vmd"></param>
        /// <param name="ratio"></param>
        public static void TryEditDetchAgeZenOya(VmdEntity vmd, int limitFrame)
        {
            float ratio = (float)limitFrame / (float)vmd.MaxFrame;
            if (ratio > 1) return;

            var zenoyaList = vmd.KeyList.Where(n => n.BoneName == "全ての親").OrderBy(n => n.FrameNum).ToList();
            if (zenoyaList != null && zenoyaList.Count == 2)
            {
                var latter = zenoyaList.LastOrDefault();
                var former = zenoyaList.FirstOrDefault();
                Vector3D fr = former.Quaternion.ToEular();

                if (latter.FrameNum == vmd.MaxFrame)
                {
                    latter.XAxis *= ratio;
                    latter.YAxis *= ratio;
                    latter.ZAxis *= ratio;
                    Vector3D lr = latter.Quaternion.ToEular();

                    Vector3D r = fr + (lr - fr) * ratio;
                    Quaternion q = r.ToQuatanion();

                    latter.XRotate = (float)q.X;
                    latter.YRotate = (float)q.Y;
                    latter.ZRotate = (float)q.Z;
                    latter.WRotate = (float)q.W;
                    latter.FrameNum = limitFrame;
                }
            }
        }
    }

    [DebuggerDisplay("{FrameDataNum}", Name = "キー数:{FrameDataNum} フレーム数:{MaxFrame}")]
    public class VmdEntity : ICloneable
    {
        private byte[] _info;

        /// <summary>
        /// 定型文
        /// </summary>
        public String INFO
        {
            get { return this._info.ToStringFromByte(); }
        }

        private byte[] _modelName;

        /// <summary>
        /// モデル名
        /// </summary>
        public string ModelName
        {
            get { return this._modelName.ToStringFromByte(); }
            set
            {
                this._modelName = value.ToByte(VmdConverter._MODELNAMELENGTH);
            }
        }

        private byte[] _frameDataNum;

        /// <summary>
        /// フレームデータ数
        /// </summary>
        public int FrameDataNum
        {
            get { return this._frameDataNum.ToIntFromByte(); }
            set
            {
                this._frameDataNum = value.ToByte(VmdConverter._FRAMEDATANUMLENGTH);
            }
        }

        private byte[] _frameMorphNum;

        /// <summary>
        /// モーフ数 2019/11/23
        /// </summary>
        public int FrameMorphNum
        {
            get { return this._frameMorphNum.ToIntFromByte(); }
            set
            {
                this._frameMorphNum = value.ToByte(VmdConverter._FRAMEDATANUMLENGTH);
            }
        }

        public List<VmdKeyEntity> KeyList { get; set; }
        public List<VmdKeyEntity> MorphList { get; set; } //2019/11/23

        /// <summary>
        /// このモーションの最後のフレーム
        /// </summary>
        public int MaxFrame
        {
            get
            {
                int result = 0;

                foreach (var key in this.KeyList)
                {
                    if (key.FrameNum > result)
                    {
                        if (key.FrameNum <= 1000000)//なんか変な取れ方をすることがあるみたいなので
                        {
                            result = key.FrameNum;
                        }
                    }
                }
                //2019/11/23 モーフをサポート。
                foreach (var key in this.MorphList)
                {
                    if (key.FrameNum > result)
                    {
                        if (key.FrameNum <= 1000000)//なんか変な取れ方をすることがあるみたいなので
                        {
                            result = key.FrameNum;
                        }
                    }
                }
                return result;
            }
        }

        #region "Methods"

        public VmdEntity CreateInvertVmd()
        {
            VmdEntity result = new VmdEntity(this._info, this._modelName, this._frameDataNum);
            result._frameMorphNum = this._frameMorphNum;
            result.MorphList = this.MorphList;
            result.KeyList = new List<VmdKeyEntity>();

            foreach (var key in this.KeyList)
            {
                var invertKey = key.CreateInvertKey();
                result.KeyList.Add(invertKey);
            }

            return result;
        }

        /// <summary>
        /// byte配列を出力します。
        /// </summary>
        /// <returns></returns>
        public byte[] Output(bool invertXZ)
        {
            int length = 54; // _INFOLENGTH + _MODELNAMELENGTH + _FRAMEDATANUMLENGTH
            length += this.KeyList.Count * VmdConverter._FRAMELENGTH;
            //2019/11/23 モーフのサポート
            length += VmdConverter._FRANUMLENGTH;
            length += this.MorphList.Count * VmdConverter._MORPHLENGTH;
            //length += 20;//謎の末尾20byte
            length += 4;

            int pos = 0;
            byte[] result = new byte[length];
            //定型文
            for (int i = 0; i < this._info.Length; i++)
            {
                result[pos + i] = this._info[i];
            }
            pos += this._info.Length;
            //モデル名
            for (int i = 0; i < this._modelName.Length; i++)
            {
                result[pos + i] = this._modelName[i];
            }
            pos += this._modelName.Length;
            //フレームデータ数
            for (int i = 0; i < this._frameDataNum.Length; i++)
            {
                result[pos + i] = this._frameDataNum[i];
            }
            pos += this._frameDataNum.Length;

            foreach (var key in this.KeyList.OrderBy(n => n.FrameNum))
            {
                byte[] keybyte = key.Output(invertXZ);
                for (int i = 0; i < keybyte.Length; i++)
                {
                    result[pos + i] = keybyte[i];
                }
                pos += keybyte.Length;
            }

            //モーフのサポート　2019/11/23
            for (int i = 0; i < this._frameMorphNum.Length; i++)
            {
                result[pos + i] = this._frameMorphNum[i];
            }
            pos += this._frameMorphNum.Length;
            foreach (var key in this.MorphList.OrderBy(n => n.FrameNum))
            {
                byte[] keybyte = key.OutputMorph();
                for (int i = 0; i < keybyte.Length; i++)
                {
                    result[pos + i] = keybyte[i];
                }
                pos += keybyte.Length;
            }

            return result;
        }

        /// <summary>
        /// 引数の数だけループさせます。
        /// </summary>
        /// <param name="loopCount"></param>
        public bool CreateLoop(int loopCount)
        {
            //ループを作る
            List<VmdKeyEntity> myKeyList = new List<VmdKeyEntity>();
            myKeyList.AddRange(this.KeyList.Select(n => (VmdKeyEntity)n.Clone()));

            for (int i = 0; i < loopCount - 1; i++)
            {
                this.MergeVmd(myKeyList);
            }

            return true;
        }

        /// <summary>
        /// 引数のkey群と結合します。
        /// </summary>
        /// <param name="argKeyList"></param>
        /// <returns></returns>
        public bool MergeVmd(List<VmdKeyEntity> argKeyList)
        {
            List<VmdKeyEntity> myKeyList = new List<VmdKeyEntity>();
            myKeyList.AddRange(this.KeyList);

            //自分のモーションの最終フレームのキーを集める
            int maxFrame = this.MaxFrame;
            Dictionary<string, VmdKeyEntity> lastFrameHash = new Dictionary<string, VmdKeyEntity>();
            foreach (var item in myKeyList.Where(n => n.FrameNum == maxFrame))
            {
                if (!lastFrameHash.ContainsKey(item.BoneName))
                {
                    lastFrameHash.Add(item.BoneName, item);
                }
            }

            //現時点の自分のモーションのうち、argKeyListの最初のフレームに最終フレームがかぶるものは上書きされるのでここで除く
            foreach (var item in argKeyList.Where(n => n.FrameNum == 0))
            {
                if (lastFrameHash.ContainsKey(item.BoneName))
                {
                    VmdKeyEntity delkey = lastFrameHash[item.BoneName];
                    myKeyList.Remove(delkey);
                }
            }

            //後ろにargKeyListを追加する
            foreach (var item in argKeyList.OrderBy(n => n.FrameNum))
            {
                VmdKeyEntity newItem = (VmdKeyEntity)item.Clone();
                newItem.FrameNum = item.FrameNum + (maxFrame);
                myKeyList.Add(newItem);
            }

            //KeyList,FrameDataNum更新
            this.KeyList = myKeyList;
            this.FrameDataNum = this.KeyList.Count();
            this.FrameMorphNum = this.MorphList.Count();

            return true;
        }

        #endregion "Methods"

        #region "IClonable"

        public virtual object Clone()
        {
            VmdEntity cln = (VmdEntity)this.MemberwiseClone();
            cln.KeyList = new List<VmdKeyEntity>();
            foreach (var item in this.KeyList)
            {
                VmdKeyEntity clnkey = (VmdKeyEntity)item.Clone();
                cln.KeyList.Add(clnkey);
            }

            cln.MorphList = new List<VmdKeyEntity>();
            if (this.MorphList != null)
            {
                foreach (var item in this.MorphList)
                {
                    VmdKeyEntity clnkey = (VmdKeyEntity)item.Clone();
                    cln.MorphList.Add(clnkey);
                }
            }
            //2020/03/06
            cln.FrameDataNum = cln.KeyList.Count();
            cln.FrameMorphNum = cln.MorphList.Count();
            return cln;
        }

        #endregion "IClonable"

        #region "Constructor"

        public VmdEntity(byte[] info
                        , byte[] modelName
                        , byte[] frameDataNum)
        {
            this._info = info;
            this._modelName = modelName;
            this._frameDataNum = frameDataNum;
            this.KeyList = new List<VmdKeyEntity>();
            this.MorphList = new List<VmdKeyEntity>();//2019/11/23
        }

        #endregion "Constructor"
    }

    /// <summary>
    /// キーフレーム単位の情報を表すエンティティです。
    /// </summary>
    [DebuggerDisplay("{FrameNum}", Name = "{BoneName},frame{FrameNum}")]
    public class VmdKeyEntity : ICloneable
    {
        private bool _isMorph = false;

        /// <summary>
        /// モーフならTrue
        /// </summary>
        public bool IsMorph
        {
            get { return _isMorph; }
        }

        /// <summary>
        /// ユニークなキーを返します。
        /// </summary>
        public string Key
        {
            get
            {
                return this.BoneName + this.FrameNum.ToString();
            }
        }

        private byte[] _boneName;

        /// <summary>
        /// ボーン名
        /// </summary>
        public String BoneName
        {
            get { return this._boneName.ToStringFromByte(); }
            set
            {
                this._boneName = value.ToByte(VmdConverter._BONENAMELENGTH);
            }
        }

        private byte[] _morphName;

        /// <summary>
        /// モーフ名
        /// </summary>
        public String MorphName
        {
            get { return this._morphName.ToStringFromByte(); }
            set
            {
                this._morphName = value.ToByte(VmdConverter._BONENAMELENGTH);
            }
        }

        private byte[] _weight;

        /// <summary>
        /// モーフウェイト
        /// </summary>
        public float Weight
        {
            get { return this._weight.ToFloatFromByte(); }
            set
            {
                this._weight = value.ToByte(VmdConverter._BONENAMELENGTH);
            }
        }

        private byte[] _frameNum;

        /// <summary>
        /// フレーム数
        /// </summary>
        public int FrameNum
        {
            get { return this._frameNum.ToIntFromByte(); }
            set
            {
                this._frameNum = value.ToByte(VmdConverter._FRANUMLENGTH);
            }
        }

        private byte[] _xAxis;

        public float XAxis
        {
            get { return this._xAxis.ToFloatFromByte(); }
            set
            {
                this._xAxis = value.ToByte(VmdConverter._AXISLENGTH);
            }
        }

        private byte[] _yAxis;

        public float YAxis
        {
            get { return this._yAxis.ToFloatFromByte(); }
            set
            {
                this._yAxis = value.ToByte(VmdConverter._AXISLENGTH);
            }
        }

        private byte[] _zAxis;

        public float ZAxis
        {
            get { return this._zAxis.ToFloatFromByte(); }
            set
            {
                this._zAxis = value.ToByte(VmdConverter._AXISLENGTH);
            }
        }

        private byte[] _xRotate;

        public float XRotate
        {
            get { return this._xRotate.ToFloatFromByte(); }
            set
            {
                this._xRotate = value.ToByte(VmdConverter._AXISLENGTH);
            }
        }

        private byte[] _yRotate;

        public float YRotate
        {
            get { return this._yRotate.ToFloatFromByte(); }
            set
            {
                this._yRotate = value.ToByte(VmdConverter._AXISLENGTH);
            }
        }

        private byte[] _zRotate;

        public float ZRotate
        {
            get { return this._zRotate.ToFloatFromByte(); }
            set
            {
                this._zRotate = value.ToByte(VmdConverter._AXISLENGTH);
            }
        }

        private byte[] _wRotate;

        public float WRotate
        {
            get { return this._wRotate.ToFloatFromByte(); }
            set
            {
                this._wRotate = value.ToByte(VmdConverter._AXISLENGTH);
            }
        }

        public DxMath.Vector3 Position
        {
            get
            {
                return new DxMath.Vector3(this.XAxis, this.YAxis, this.ZAxis);
            }
        }

        public Quaternion Quaternion
        {
            get
            {
                return new Quaternion(this.XRotate, this.YRotate, this.ZRotate, this.WRotate);
            }
        }

        private byte[] _hokan;

        public string Hokan
        {
            get { return this._hokan.ToStringFromByte(); }
            set
            {
                this._hokan = value.ToByte(VmdConverter._AXISLENGTH);
            }
        }

        public VmdKeyEntity CreateInvertKey()
        {
            string boneName = this.BoneName;
            if (boneName.IndexOf("左") >= 0)
                boneName = boneName.Replace("左", "右");
            else if (boneName.IndexOf("右") >= 0)
                boneName = boneName.Replace("右", "左");

            Vector3D trans = new Vector3D(this.XAxis * -1, this.YAxis, this.ZAxis);
            Vector3D eularRot = this.Quaternion.ToEular();
            eularRot = Vector3D.Multiply(eularRot, -1f);
            eularRot.X *= -1;

            Quaternion q = eularRot.ToQuatanion();
            var result = new VmdKeyEntity(boneName, this.FrameNum, (int)trans.X, (int)trans.Y, (int)trans.Z
                                                                            , (int)q.X, (int)q.Y, (int)q.Z, (int)q.W);

            return result;
        }

        #region "Methods"

        /// <summary>
        /// byte配列を出力します。
        /// </summary>
        /// <returns></returns>
        public byte[] Output(bool invertXZ)
        {
            int pos = 0;
            byte[] result = new byte[VmdConverter._FRAMELENGTH];
            //ボーン名

            //背景移動モードなら移動○○ではなく背景○○を動かす
            if (invertXZ && this.BoneName.IndexOf("移動") >= 0)
            {
                Console.WriteLine("before:" + this.BoneName);
                string backNameStr = this.BoneName.Replace("移動", "背景");
                Console.WriteLine("after:" + backNameStr);
                byte[] backName = backNameStr.ToByte(VmdConverter._BONENAMELENGTH);
                for (int i = 0; i < backName.Length; i++)
                {
                    result[pos + i] = backName[i];
                }
            }
            else
            {
                //Console.WriteLine("nochange:" + this.BoneName);
                for (int i = 0; i < this._boneName.Length; i++)
                {
                    result[pos + i] = this._boneName[i];
                }
            }

            pos += this._boneName.Length;
            //フレーム数
            for (int i = 0; i < this._frameNum.Length; i++)
            {
                result[pos + i] = this._frameNum[i];
            }
            pos += this._frameNum.Length;

            //X位置

            for (int i = 0; i < this._xAxis.Length; i++)
            {
                result[pos + i] = this._xAxis[i];
            }
            pos += this._xAxis.Length;

            //Y位置
            for (int i = 0; i < this._yAxis.Length; i++)
            {
                result[pos + i] = this._yAxis[i];
            }
            pos += this._yAxis.Length;

            //Z位置
            for (int i = 0; i < this._zAxis.Length; i++)
            {
                result[pos + i] = this._zAxis[i];
            }
            pos += this._zAxis.Length;

            //Xquot
            for (int i = 0; i < this._xRotate.Length; i++)
            {
                result[pos + i] = this._xRotate[i];
            }
            pos += this._xRotate.Length;

            //Yquot
            for (int i = 0; i < this._yRotate.Length; i++)
            {
                result[pos + i] = this._yRotate[i];
            }
            pos += this._yRotate.Length;

            //Zquot
            for (int i = 0; i < this._zRotate.Length; i++)
            {
                result[pos + i] = this._zRotate[i];
            }
            pos += this._zRotate.Length;

            //Wquot
            for (int i = 0; i < this._wRotate.Length; i++)
            {
                result[pos + i] = this._wRotate[i];
            }
            pos += this._wRotate.Length;

            //補完
            for (int i = 0; i < this._hokan.Length; i++)
            {
                result[pos + i] = this._hokan[i];
            }
            pos += this._hokan.Length;

            return result;
        }

        public byte[] OutputMorph()
        {
            int pos = 0;

            byte[] result = new byte[VmdConverter._MORPHLENGTH];
            //モーフ名
            //Console.WriteLine("nochange:" + this.BoneName);
            for (int i = 0; i < this._morphName.Length; i++)
            {
                result[pos + i] = this._morphName[i];
            }
            pos += this._morphName.Length;

            //フレーム数
            for (int i = 0; i < this._frameNum.Length; i++)
            {
                result[pos + i] = this._frameNum[i];
            }
            pos += this._frameNum.Length;

            if (this.MorphName == "笑い")
                this.MorphName = this.MorphName;

            //ウェイト
            for (int i = 0; i < this._weight.Length; i++)
            {
                result[pos + i] = this._weight[i];
            }
            pos += this._weight.Length;

            return result;
        }

        #endregion "Methods"

        #region "IClonable"

        public virtual object Clone()
        {
            return (VmdKeyEntity)this.MemberwiseClone();
        }

        #endregion "IClonable"

        #region "Constructors"

        public VmdKeyEntity(byte[] morphName
                        , byte[] frameNum
                        , byte[] weight)
        {
            this._morphName = morphName;
            this._boneName = morphName;
            this._frameNum = frameNum;
            this._weight = weight;
            //モーフだよフラグをTreuに
            this._isMorph = true;
        }

        public VmdKeyEntity(byte[] boneName
                        , byte[] frameNum
                        , byte[] xAxis
                        , byte[] yAxis
                        , byte[] zAxis
                        , byte[] xRotate
                        , byte[] yRotate
                        , byte[] zRotate
                        , byte[] wRotate
                        , byte[] hokan)
        {
            this._boneName = boneName;
            this._frameNum = frameNum;
            this._xAxis = xAxis;
            this._yAxis = yAxis;
            this._zAxis = zAxis;
            this._xRotate = xRotate;
            this._yRotate = yRotate;
            this._zRotate = zRotate;
            this._wRotate = wRotate;
            this._hokan = hokan;
        }

        public VmdKeyEntity(string boneName
                        , int frameNum
                        , float xAxis = 0
                        , float yAxis = 0
                        , float zAxis = 0
                        , float xRotate = 0
                        , float yRotate = 0
                        , float zRotate = 0
                        , float wRotate = 1
                        )
        {
            byte[] base_bezier = new byte[64] { 0x14, 0x14, 0x00, 0x00, 0x14, 0x14, 0x14, 0x14, 0x6B, 0x6B, 0x6B, 0x6B, 0x6B, 0x6B, 0x6B, 0x6B, 0x14, 0x14, 0x14, 0x14, 0x14, 0x14, 0x14, 0x6B, 0x6B, 0x6B, 0x6B, 0x6B, 0x6B, 0x6B, 0x6B, 0x00, 0x14, 0x14, 0x14, 0x14, 0x14, 0x14, 0x6B, 0x6B, 0x6B, 0x6B, 0x6B, 0x6B, 0x6B, 0x6B, 0x00, 0x00, 0x14, 0x14, 0x14, 0x14, 0x14, 0x6B, 0x6B, 0x6B, 0x6B, 0x6B, 0x6B, 0x6B, 0x6B, 0x00, 0x00, 0x00 };

            this._boneName = boneName.ToByte(VmdConverter._BONENAMELENGTH);
            this._frameNum = frameNum.ToByte(VmdConverter._FRANUMLENGTH);
            this._xAxis = xAxis.ToByte(VmdConverter._AXISLENGTH);
            this._yAxis = yAxis.ToByte(VmdConverter._AXISLENGTH);
            this._zAxis = zAxis.ToByte(VmdConverter._AXISLENGTH);
            this._xRotate = xRotate.ToByte(VmdConverter._AXISLENGTH);
            this._yRotate = yRotate.ToByte(VmdConverter._AXISLENGTH);
            this._zRotate = zRotate.ToByte(VmdConverter._AXISLENGTH);
            this._wRotate = wRotate.ToByte(VmdConverter._AXISLENGTH);
            this._hokan = base_bezier;
        }

        #endregion "Constructors"
    }
}