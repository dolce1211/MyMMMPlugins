using MyUtility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceExpressionHelper
{
    internal class ScreenShotSaver
    {
        public static bool SaveScr(Rectangle rect, string dirpath, string filename
                        , LetterArgs letterArgs
                        , ref string retPicturePath)
        {
            retPicturePath = string.Empty;

            Bitmap bmp = new Bitmap(rect.Width, rect.Height);

            //Graphicsの作成
            using (Graphics g = Graphics.FromImage(bmp))
            {
                //画面全体をコピーする
                g.CopyFromScreen(rect.X, rect.Y, 0, 0, bmp.Size);
                if (letterArgs.NamePos > 0)
                {
                    var printFileName = filename;

                    g.AddTextToPicture(rect, letterArgs.NamePos, printFileName, letterArgs.Font, letterArgs.Color, letterArgs.RimColor, letterArgs.RimWidth);
                }
            }

            string saveFilename = System.IO.Path.GetFileNameWithoutExtension(filename); //拡張子を除外する

            char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();
            if (saveFilename.IndexOfAny(invalidChars) >= 0)
            {
                //ファイルに使えない文字あり⇒全角へ
                saveFilename = Microsoft.VisualBasic.Strings.StrConv(filename, Microsoft.VisualBasic.VbStrConv.Wide) + ".png";
            }
            string filepath = System.IO.Path.Combine(dirpath, saveFilename);

            filepath += ".png";
            if (System.IO.File.Exists(filepath))
            {
                //すでに同盟ファイルが存在していれば消す
                try
                {
                    System.IO.File.Delete(filepath);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            //ファイル保存(フルカラー)
            bmp.Save(filepath, System.Drawing.Imaging.ImageFormat.Png);

            bmp.Dispose();

            if (System.IO.File.Exists(filepath))
            {
                retPicturePath = filepath;
                return true;
            }
            return false;
        }
    }
}