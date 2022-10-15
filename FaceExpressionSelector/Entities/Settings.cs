using MikuMikuPlugin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceExpressionHelper
{
    public class Args
    {
        /// <summary>
        /// 表情一覧
        /// </summary>
        public List<ExpressionItem> Items { get; set; } = new List<ExpressionItem>();

        /// <summary>
        /// 文字情報
        /// </summary>
        public LetterArgs LetterArgs { get; set; } = null;

        /// <summary>
        /// 直前のスクショ枠の場所・サイズ
        /// </summary>
        public Rectangle PreviousRectangle { get; set; } = new Rectangle();

        /// <summary>
        /// 常に手前
        /// </summary>
        public bool TopMost { get; set; } = false;

        /// <summary>
        /// 対象外の目・リップ・まゆモーフ一覧
        /// </summary>
        public List<string> ExceptionMainMorphs { get; set; } = new List<string>();

        /// <summary>
        /// 対象のその他モーフ一覧
        /// </summary>
        public List<string> TargetOtherMorphs { get; set; } = new List<string>();

        /// <summary>
        /// モデルごとのモーフ置換情報
        /// </summary>
        public List<ReplacedMorphNameItem> ReplacedMorphs { get; set; } = new List<ReplacedMorphNameItem>();

        /// <summary>
        /// 引数のモーフが処理対象ならtrue
        /// </summary>
        /// <param name="morph"></param>
        /// <returns></returns>
        public bool IsTargetMorph(MorphItem morph)
        {
            if (morph == null)
                return false;
            if (morph.MorphType == MMDUtil.MMDUtilility.MorphType.Other || morph.MorphType == MMDUtil.MMDUtilility.MorphType.none)
            {
                //その他モーフ
                return TargetOtherMorphs.Contains(morph.MorphName);
            }
            else
            {
                //目・リップ・まゆモーフ
                return !ExceptionMainMorphs.Contains(morph.MorphName);
            }
        }
    }

    /// <summary>
    /// 表情セット
    /// </summary>
    public class ExpressionItem
    {
        /// <summary>
        /// 表情名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// モーフ一覧
        /// </summary>
        public List<MorphItem> MorphItems { get; set; } = new List<MorphItem>();

        public void ChangeName(string newName)
        {
            //サムネをリネームする
            if (this._thumbnail != null)
                this._thumbnail.Dispose();
            this._thumbnail = null;

            var prevThumbnailPath = this.ThumbnailPath;
            this.Name = newName;
            var newThumbnailPath = this.ThumbnailPath;

            if (System.IO.File.Exists(prevThumbnailPath) && !System.IO.File.Exists(newThumbnailPath))
            {
                try
                {
                    System.IO.File.Move(prevThumbnailPath, newThumbnailPath);
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// サムネイルパス
        /// </summary>
        public string ThumbnailPath
        {
            get
            {
                var dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                dir = System.IO.Path.Combine(dir, "faceExpressions");
                return System.IO.Path.Combine(dir, $"{this.Name}.png");
            }
        }

        /// <summary>
        /// サムネイルイメージ
        /// </summary>
        private Image _thumbnail = null;

        /// <summary>
        /// サムネイルイメージを返します。
        /// </summary>
        public Image ThumbNail
        {
            get
            {
                if (_thumbnail == null)
                {
                    if (System.IO.File.Exists(this.ThumbnailPath))
                    {
                        try
                        {
                            var img = Image.FromFile(this.ThumbnailPath);
                            this._thumbnail = img.Clone() as Image;
                            img.Dispose();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                return _thumbnail;
            }
        }

        /// <summary>
        /// サムネイルファイルを削除します。
        /// </summary>
        public void TryDeleteThumbnail()
        {
            if (this._thumbnail != null)
                this._thumbnail.Dispose();
            this._thumbnail = null;
            if (System.IO.File.Exists(this.ThumbnailPath))
            {
                try
                {
                    System.IO.File.Delete(this.ThumbnailPath);
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// ReplacedMorphsを加味したモーフ一覧を返します。
        /// </summary>
        public List<DspMorphItem> GetReplacedItem(List<ReplacedMorphNameItem> replacedMorphs, string modelName)
        {
            var replace = replacedMorphs.Where(n => n.ModelName == modelName).FirstOrDefault();
            if (replace == null)
                //リプレイス対象なし
                return this.MorphItems.Select(n=> new DspMorphItem(n.Clone())).ToList();

            var hash = new Dictionary<string, ReplacedItem>();
            foreach (var rp in replace.ReplacedMorphList)
            {
                if (!hash.ContainsKey(rp.MorphName))
                    hash.Add(rp.MorphName, rp);
            }

            var ret = new List<DspMorphItem>();
            foreach (var morph in this.MorphItems)
            {
                var clone = new DspMorphItem(morph.Clone());
                if (hash.ContainsKey(clone.MorphName))
                {
                    var rp = hash[clone.MorphName];
                    clone.ReplacedItem = rp;
                }
                ret.Add(clone);
            }
            return ret;
        }
    }

    public class MorphItem
    {
        /// <summary>
        /// モーフ名
        /// </summary>
        public string MorphName { get; set; }

        /// <summary>
        /// モーフ量
        /// </summary>
        public float Weight { get; set; }

        /// <summary>
        /// どのパネルのモーフ？
        /// </summary>
        public MMDUtil.MMDUtilility.MorphType MorphType { get; set; }

        /// <summary>
        /// モーフタイプ＋名称
        /// </summary>
        public string MortphNameWithType
        {
            get
            {
                switch (this.MorphType)
                {
                    case MMDUtil.MMDUtilility.MorphType.Eye:
                        return $"目__{this.MorphName}";

                    case MMDUtil.MMDUtilility.MorphType.Lip:
                        return $"口__{this.MorphName}";

                    case MMDUtil.MMDUtilility.MorphType.Brow:
                        return $"眉__{this.MorphName}";

                    case MMDUtil.MMDUtilility.MorphType.Other:
                        return $"他__{this.MorphName}";

                    default:
                        return $"？__{this.MorphName}";
                }
            }
        }

        /// <summary>
        /// クローンを返します。
        /// </summary>
        /// <returns></returns>
        public MorphItem Clone()
        {
            //return this.MemberwiseClone() as MorphItem;
            return new MorphItem()
            {
                MorphName = this.MorphName,
                MorphType = this.MorphType,
                Weight = this.Weight,
            };
        }
    }
    /// <summary>
    /// 置換情報などを考慮したMorphItemです。
    /// </summary>
    public class DspMorphItem : MorphItem
    {
        /// <summary>
        /// 置換などを考慮した表示名
        /// </summary>
        public string DspMorphName
        {
            get
            {
                if (this.ReplacedItem != null)
                    return this.ReplacedItem.RepalcedMorphName;
                else
                    return this.MorphName;
            }
        }
        /// <summary>
        /// 無視するモーフならtrue
        /// </summary>
        public bool Ignore
        {
            get
            {
                if (this.ReplacedItem != null)
                    return this.ReplacedItem.Ignore;
                else
                    return false;
            }
        }
        public ReplacedItem ReplacedItem { get; set; }
        public DspMorphItem(MorphItem morphitem)
        {
            this.MorphName = morphitem.MorphName;
            this.MorphType = morphitem.MorphType;
            this.Weight = morphitem.Weight;
        }
    }


    /// <summary>
    /// モデルごとのモーフ置換情報
    /// </summary>
    public class ReplacedMorphNameItem
    {
        /// <summary>
        /// モデル名
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 置換モーフ情報
        /// </summary>
        public List<ReplacedItem> ReplacedMorphList { get; set; } = new List<ReplacedItem>();
    }

    /// <summary>
    /// モーフ置換情報
    /// </summary>
    public class ReplacedItem
    {
        /// <summary>
        /// 無視する
        /// </summary>
        public bool Ignore { get; set; }

        /// <summary>
        /// 元モーフ名
        /// </summary>
        public string MorphName { get; set; }

        /// <summary>
        /// 置換後モーフ名
        /// </summary>
        public string RepalcedMorphName { get; set; }
    }

    public class LetterArgs
    {
        /// <summary>
        /// 0:なし 1:下 2:上
        /// </summary>
        public int NamePos { get; set; }

        public String FontName { get; set; }
        public float FontSize { get; set; }
        public int FontStyle { get; set; }
        public int ColorInt { get; set; }
        public int RimColorInt { get; set; }
        public int RimWidth { get; set; }

        public Font Font
        {
            get { return new Font(FontName, FontSize, (FontStyle)this.FontStyle); }
        }

        public Color Color
        {
            get { return ColorTranslator.FromOle(this.ColorInt); }
        }

        public Color RimColor
        {
            get { return ColorTranslator.FromOle(this.RimColorInt); }
        }

        public static LetterArgs CreateInitialInstance()
        {
            var ret = new LetterArgs();
            //デフォルト値を指定
            Font defFont = new Font("メイリオ", 36, System.Drawing.FontStyle.Bold);
            ret = new LetterArgs();
            ret.FontName = defFont.Name;
            ret.FontSize = defFont.Size;
            ret.FontStyle = (int)defFont.Style;
            ret.ColorInt = ColorTranslator.ToOle(Color.Orange);
            ret.RimColorInt = ColorTranslator.ToOle(Color.Black);
            ret.RimWidth = 1;
            ret.NamePos = 1;

            return ret;
        }
    }
}