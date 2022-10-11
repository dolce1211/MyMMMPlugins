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
                            this._thumbnail = Image.FromFile(this.ThumbnailPath);
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