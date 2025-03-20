using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Windows.Media.Media3D;
using System.ComponentModel;
using System.Drawing;
using MikuMikuPlugin;
using static MMDUtil.MMDUtilility;
using System.Xml;
using System.Runtime.Remoting.Contexts;
using System.IO;
using System.Diagnostics;
using System.IO.Pipes;
using System.Security;
using System.Threading.Tasks;
using System.Threading;
using MMDUtil;

namespace MyUtility

{
    /// <summary>
    /// MMD、MMMどっちで動いてる？
    /// </summary>
    public enum OperatingMode
    {
        /// <summary>
        /// MMMで動いている
        /// </summary>
        OnMMM,

        /// <summary>
        /// MMDで動いている
        /// </summary>
        OnMMD
    }

    public static class MathUtil
    {
        public static float lerp(float x, float y, float s)
        {
            return x + s * (y - x);
        }
    }

    /// <summary>
    /// テキストボックスのヘルパです。
    /// </summary>
    public static class TextBoxHelper
    {
        /// <summary>
        /// テキストボックスの入力を数値に限定
        /// </summary>
        /// <param name="txtbox"></param>
        /// <returns></returns>
        public static bool LimitInputToNum(this TextBox txtbox, bool allowDecimal = true, bool allowMinus = true)
        {
            if (allowDecimal && allowMinus)
            {
                //正負の小数
                txtbox.KeyPress += new KeyPressEventHandler(KeyPressEventAll);
            }
            else if (allowDecimal && !allowMinus)
            {
                //正の小数
                txtbox.KeyPress += new KeyPressEventHandler(KeyPressEventPlusFloat);
            }
            else if (!allowDecimal && allowMinus)
            {
                //正負の整数
                txtbox.KeyPress += new KeyPressEventHandler(KeyPressEventAllInt);
            }
            else
            {
                //正の整数
                txtbox.KeyPress += new KeyPressEventHandler(KeyPressEventPlusInt);
            }

            return true;
        }

        /// <summary>
        /// 正負の小数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void KeyPressEventAll(object sender, KeyPressEventArgs e)
        {
            KeyPressEventInternal(sender, e, true, true);
        }

        /// <summary>
        /// //正負の整数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void KeyPressEventAllInt(object sender, KeyPressEventArgs e)
        {
            KeyPressEventInternal(sender, e, false, true);
        }

        /// <summary>
        /// 正の整数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void KeyPressEventPlusInt(object sender, KeyPressEventArgs e)
        {
            KeyPressEventInternal(sender, e, false, false);
        }

        /// <summary>
        /// 正の小数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void KeyPressEventPlusFloat(object sender, KeyPressEventArgs e)
        {
            KeyPressEventInternal(sender, e, true, false);
        }

        private static void KeyPressEventInternal(object sender, KeyPressEventArgs e, bool allowDecimal, bool allowMinus)
        {
            TextBox txtbox = (TextBox)sender;

            if ((e.KeyChar < '0' || '9' < e.KeyChar) && e.KeyChar != '\b')
            {
                if (e.KeyChar == '.')
                {
                    if (!allowDecimal) { e.Handled = true; }
                    if (txtbox.Text.IndexOf(".") > 0)
                    {
                        e.Handled = true;
                    }
                }
                else if (e.KeyChar == '-')
                {
                    if (!allowMinus) { e.Handled = true; }
                    if (txtbox.SelectionStart != 0 || txtbox.Text.IndexOf("-") > 0)
                    {
                        e.Handled = true;
                    }
                }
                else
                {
                    //押されたキーが 0～9でない場合は、イベントをキャンセルする
                    e.Handled = true;
                }
            }
        }
    }

    /// <summary>
    /// ファイルマネージャです。
    /// </summary>
    public class MyFileManager
    {
        /// <summary>
        /// ファイルオープン
        /// </summary>
        /// <returns></returns>
        public static string ShowOpenFileDialog(string argFilter = "", string argTitle = "")
        {
            string filter = "pmd/pmx Files(*.pmx, *.pmd) | *.pmx; *.pmd";
            if (argFilter != string.Empty) { filter = argFilter; }

            string title = "ファイル指定";
            if (argTitle != string.Empty) { title = argTitle; }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog.Filter = filter;// "pmx files (*.pmx)|*.pmx";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Title = title;
            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return string.Empty;
            }
            return openFileDialog.FileName;
        }

        /// <summary>
        /// ファイルオープン
        /// </summary>
        /// <returns></returns>
        public static string ShowSaveFileDialog(string argFileName = "", string argFilter = "", string argTitle = "")
        {
            //SaveFileDialogクラスのインスタンスを作成
            SaveFileDialog sfd = new SaveFileDialog();

            //はじめのファイル名を指定する
            //はじめに「ファイル名」で表示される文字列を指定する
            sfd.FileName = "text.txt";
            if (!string.IsNullOrEmpty(argFileName)) { sfd.FileName = argFileName; }

            //はじめに表示されるフォルダを指定する
            sfd.InitialDirectory = Environment.CurrentDirectory;
            //[ファイルの種類]に表示される選択肢を指定する
            //指定しない（空の文字列）の時は、現在のディレクトリが表示される
            sfd.Filter = "txtファイル(*.txt)|*.txt|すべてのファイル(*.*)|*.*";
            if (!string.IsNullOrEmpty(argFilter)) { sfd.Filter = argFilter; }
            //[ファイルの種類]ではじめに選択されるものを指定する
            //2番目の「すべてのファイル」が選択されているようにする
            sfd.FilterIndex = 1;
            //タイトルを設定する
            sfd.Title = "ファイル保存";
            if (!string.IsNullOrEmpty(argTitle)) { sfd.Title = argTitle; }

            //ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
            sfd.RestoreDirectory = true;
            //既に存在するファイル名を指定したとき警告する
            //デフォルトでTrueなので指定する必要はない
            sfd.OverwritePrompt = true;
            //存在しないパスが指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            sfd.CheckPathExists = true;

            //ダイアログを表示する
            try
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                    return sfd.FileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return string.Empty;
        }

        /// <summary>
        /// このアセンブリの現在のパスを返します。
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentAssemblyPath()
        {
            string result = string.Empty;
            result = System.Reflection.Assembly.GetExecutingAssembly().Location;
            result = System.IO.Path.GetDirectoryName(result);
            return result;
        }
    }

    public static class SerializerExtension
    {
        public static bool Serialize<T>(this T entity, string fileName)
        {
            return Serializer.Serialize<T>(entity, fileName);
        }

        public static string SerializeToString<T>(this T entity)
        {
            return Serializer.SerializeToString<T>(entity);
        }

        public static T Deserialize<T>(this string fileName)
        {
            return Serializer.Deserialize<T>(fileName);
        }

        public static T DeserializeFromString<T>(this string xml)
        {
            return Serializer.DeserializeFromString<T>(xml);
        }
    }

    /// <summary>
    /// XMLシリアライザです。
    /// </summary>
    public class Serializer
    {
        public static bool Serialize<T>(T entity, string fileName)
        {
            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
            }

            //XmlSerializerオブジェクトを作成
            //オブジェクトの型を指定する
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(T));
            //書き込むファイルを開く（UTF-8 BOM無し）
            System.IO.StreamWriter sw = new System.IO.StreamWriter(
                fileName, false, new System.Text.UTF8Encoding(false));
            //シリアル化し、XMLファイルに保存する
            serializer.Serialize(sw, entity);
            //ファイルを閉じる
            sw.Close();

            return true;
        }

        public static string SerializeToString<T>(T entity)
        {
            //XmlSerializerオブジェクトを作成
            //オブジェクトの型を指定する
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (System.IO.StringWriter sww = new System.IO.StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    serializer.Serialize(writer, entity);
                    return sww.ToString();
                }
            }
        }

        public static T Deserialize<T>(string fileName)
        {
            T result = default(T);

            if (!System.IO.File.Exists(fileName))
            {
                return result;
            }

            //XmlSerializerオブジェクトを作成
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(T));

            try
            {
                //読み込むファイルを開く
                System.IO.StreamReader sr = new System.IO.StreamReader(
                    fileName, new System.Text.UTF8Encoding(false));
                //XMLファイルから読み込み、逆シリアル化する
                result = (T)serializer.Deserialize(sr);
                //ファイルを閉じる
                sr.Close();
                return result;
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public static T DeserializeFromString<T>(string xml)
        {
            try
            {
                //XmlSerializerオブジェクトを作成
                System.Xml.Serialization.XmlSerializer serializer =
                    new System.Xml.Serialization.XmlSerializer(typeof(T));
                using (XmlReader reader = XmlReader.Create(new StringReader(xml)))
                {
                    //XMLファイルから読み込み、逆シリアル化する
                    var result = (T)serializer.Deserialize(reader);
                    //ファイルを閉じる
                    reader.Close();
                    return result;
                }
            }
            catch (Exception)
            {
                return default;
            }
        }
    }

    /// <summary>
    /// 変換用ヘルパです。
    /// </summary>
    public static class ParseHelper
    {
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern Int32 SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(
            HandleRef hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private const int WM_SETREDRAW = 0x000B;
        private const int WM_PAINT = 0x000F;

        /// <summary>
        /// コントロールの描画を止めます。
        /// </summary>
        /// <param name="flg">
        /// false:描画を止める
        /// true:描画を再開する
        /// </param>
        public static void BeginAndEndUpdate(this Control control, bool flg)
        {
            if (flg)
                control.EndUpdate();
            else
                control.BeginUpdate();
        }

        /// <summary>
        /// コントロールの再描画を停止させる
        /// </summary>
        /// <param name="control">対象のコントロール</param>
        public static void BeginUpdate(this Control control)
        {
            SendMessage(new HandleRef(control, control.Handle),
                WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// コントロールの再描画を再開させる
        /// </summary>
        /// <param name="control">対象のコントロール</param>
        public static void EndUpdate(this Control control, bool noRefresh = false)
        {
            SendMessage(new HandleRef(control, control.Handle),
                WM_SETREDRAW, new IntPtr(1), IntPtr.Zero);
            //control.Invalidate();
            if (!noRefresh)
                control.Refresh();
        }

        /// <summary>
        /// コントロールの再描画を停止させる
        /// </summary>
        /// <param name="control">対象のコントロール</param>
        public static void BeginUpdate(this IntPtr hWnd)
        {
            SendMessage(hWnd,
                WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// コントロールの再描画を再開させる
        /// </summary>
        /// <param name="control">対象のコントロール</param>
        public static void EndUpdate(this IntPtr hWnd)
        {
            SendMessage(hWnd,
                WM_SETREDRAW, new IntPtr(1), IntPtr.Zero);
        }
    }

    public static class GeneralHelper
    {
        public static bool SetEnabled(this Control container, bool enabled, bool includeMyself)
        {
            List<Control> children = new List<Control>();
            container.CollectControl(includeMyself, ref children);
            foreach (Control ctrl in children)
            {
                ctrl.Enabled = enabled;
            }
            return true;
        }

        public static int CollectControl(this Control container, bool includeMyself, ref List<Control> retCtrls)
        {
            if (retCtrls == null) { retCtrls = new List<Control>(); }
            if (includeMyself) { retCtrls.Add(container); }

            foreach (Control ctr in container.Controls)
            {
                if (ctr.HasChildren)
                {
                    List<Control> children = new List<Control>();
                    ctr.CollectControl(false, ref children);
                    foreach (Control child in children)
                    {
                        retCtrls.Add(child);
                    }
                }
                else
                {
                    retCtrls.Add(ctr);
                }
            }

            return retCtrls.Count;
        }

        /// <summary>
        /// エラーを出さないTrimです。
        /// </summary>
        public static string TrimSafe(this string str)
        {
            if (str == null)
                return string.Empty;

            return str.Trim();
        }

        /// <summary>
        /// 文字列の末尾から指定した長さの文字列を取得する
        /// </summary>
        /// <param name="str">文字列</param>
        /// <param name="len">長さ</param>
        /// <returns>取得した文字列</returns>
        public static string Right(this string str, int len)
        {
            if (len < 0)
            {
                throw new ArgumentException("引数'len'は0以上でなければなりません。");
            }
            if (str == null)
            {
                return "";
            }
            if (str.Length <= len)
            {
                return str;
            }
            return str.Substring(str.Length - len, len);
        }

        /// <summary>
        /// カタカナに変換します。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToKatakana(this string str)
        {
            return Microsoft.VisualBasic.Strings.StrConv(str, Microsoft.VisualBasic.VbStrConv.Katakana, 0x411);
        }

        public static MMDUtil.MMDUtilility.MorphType ToMyPanelType(this PanelType pnltype)
        {
            var morphtype = MMDUtil.MMDUtilility.MorphType.none;
            switch (pnltype)
            {
                case PanelType.Brow:
                    morphtype = MMDUtil.MMDUtilility.MorphType.Brow;
                    break;

                case PanelType.Eyes:
                    morphtype = MMDUtil.MMDUtilility.MorphType.Eye;
                    break;

                case PanelType.Mouth:
                    morphtype = MMDUtil.MMDUtilility.MorphType.Lip;
                    break;

                case PanelType.Etc:
                    morphtype = MMDUtil.MMDUtilility.MorphType.Other;
                    break;

                default:
                    break;
            }
            return morphtype;
        }

        /// <summary>
        /// 画像に文字を書き込みます。
        /// </summary>
        /// <param name="g"></param>
        /// <param name="name"></param>
        /// <param name="font"></param>
        /// <param name="letterColor"></param>
        /// <param name="rimColor"></param>
        /// <param name="rimWidth"></param>
        public static void AddTextToPicture(this Graphics g, Rectangle rect
                                            , int pos, string letters, Font font, Color letterColor, Color rimColor, float rimWidth)
        {
            Size nopadSize = TextRenderer.MeasureText(g, letters, font
                            , new Size(1000, 1000), TextFormatFlags.NoPadding);

            int top = 10;
            if (pos == 1)
            {
                //下
                top = rect.Height - nopadSize.Height + top;
            }

            System.Drawing.Drawing2D.SmoothingMode prevsm = g.SmoothingMode;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //GraphicsPathオブジェクトの作成
            System.Drawing.Drawing2D.GraphicsPath gp =
                new System.Drawing.Drawing2D.GraphicsPath();
            //GraphicsPathに文字列を追加する
            FontFamily ff = new FontFamily(font.Name);
            gp.AddString(letters, ff, (int)font.Style, font.Size,
                new Point(10, top), StringFormat.GenericDefault);

            Brush lb = new SolidBrush(letterColor);
            Pen rp = new Pen(rimColor, rimWidth);

            //文字列の中を塗りつぶす
            g.FillPath(lb, gp);
            //文字列の縁を描画する
            g.DrawPath(rp, gp);

            //リソースを解放する
            ff.Dispose();

            g.SmoothingMode = prevsm;
            //g.Dispose();
        }
    }

    /// <summary>
    /// コントロール用ヘルパです。
    /// </summary>
    public static class ControlHelper
    {
        /// <summary>
        /// フォーム位置のセーブを試みます。
        /// </summary>
        /// <param name="pluginName">プラグイン名</param>
        /// <param name="frm"></param>
        /// <returns></returns>
        public static bool TrySaveMyPosition(string pluginName, Form frm)
        {
            var path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var remembermePath = System.IO.Path.Combine(path, "rememberme");

            if (System.IO.File.Exists(remembermePath))
            {
                var value = $"{frm.Location.X},{frm.Location.Y}";
                Microsoft.Win32.Registry.SetValue(
                            $@"HKEY_CURRENT_USER\Software\MMMPlugin\{pluginName}", "formLocation", value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// フォーム位置のロードを試みます。
        /// </summary>
        /// <param name="pluginName">プラグイン名</param>
        /// <param name="frm"></param>
        /// <returns></returns>
        public static bool TryLoadMyPosition(string pluginName, Form frm)
        {
            var path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var remembermePath = System.IO.Path.Combine(path, "rememberme");

            if (System.IO.File.Exists(remembermePath))
            {
                var obj = Microsoft.Win32.Registry.GetValue(
                            $@"HKEY_CURRENT_USER\Software\MMMPlugin\{pluginName}", "formLocation", string.Empty);
                if (obj != null)
                {
                    var value = obj.ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        var array = value.Split(',');
                        if (array.Length >= 2)
                        {
                            frm.StartPosition = FormStartPosition.Manual;
                            frm.Location = new Point(array[0].ToInt(), array[1].ToInt());
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// byte配列を文字列に変換して返します。
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        public static string ToStringFromByte(this byte[] bs)
        {
            string result = System.Text.Encoding.GetEncoding("Shift_JIS").GetString(bs).Replace("\0", "");
            //謎の文字が挟まる事があるので補正 2019/11/23;
            char c = (char)63729;
            string rep = c.ToString();
            if (result.IndexOf(rep) >= 0)
                result = result.Replace(rep, "");

            return result;
        }

        /// <summary>
        /// byte配列をintに変換して返します。
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        public static int ToIntFromByte(this byte[] bs)
        {
            return BitConverter.ToInt32(bs, 0); ;
        }

        /// <summary>
        /// byte配列をintに変換して返します。
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        public static float ToFloatFromByte(this byte[] bs)
        {
            return BitConverter.ToSingle(bs, 0); ;
        }

        /// <summary>
        /// intをbyte配列に変換して返します。
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        public static byte[] ToByte(this int val, int length)
        {
            byte[] data = BitConverter.GetBytes(val);
            byte[] result = new byte[length];
            for (int i = 0; i < length; i++)
            {
                if (i < data.Length)
                {
                    result[i] = data[i];
                }
                else
                {
                    result[i] = 0;
                }
            }

            return result;
        }

        /// <summary>
        /// floatをbyte配列に変換して返します。
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        public static byte[] ToByte(this float val, int length)
        {
            byte[] data = BitConverter.GetBytes(val);
            byte[] result = new byte[length];
            for (int i = 0; i < length; i++)
            {
                if (i < data.Length)
                {
                    result[i] = data[i];
                }
                else
                {
                    result[i] = 0;
                }
            }
            return result;
        }

        /// <summary>
        /// 文字列をbyte配列に変換して返します。
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        public static byte[] ToByte(this string str, int length)
        {
            byte[] data = Encoding.GetEncoding("shift_jis").GetBytes(str);

            byte[] result = new byte[length];
            for (int i = 0; i < length; i++)
            {
                if (i < data.Length)
                {
                    result[i] = data[i];
                }
                else
                {
                    result[i] = 0;
                }
            }
            return result;
        }

        public static float ToFloat(this string value)
        {
            float result = 0f;
            try
            {
                result = float.Parse(value);
            }
            catch (Exception)
            {
                result = 0f;
            }
            return result;
        }

        public static int ToInt(this string value)
        {
            int result = 0;
            try
            {
                result = int.Parse(value);
            }
            catch (Exception)
            {
                result = 0;
            }
            return result;
        }

        /// <summary>
        /// オイラーからクォータニオンに変換します。
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Quaternion ToQuatanion(this Vector3D v)
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

            return new Quaternion(x, y, z, w);
        }

        /// <summary>
        /// クォータニオンからオイラーに変換します。
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public static Vector3D ToEular(this Quaternion q)
        {
            Double ry = (Math.Atan2((2.0 * (q.W * q.Y + q.X * q.Z)), (1.0 - 2.0 * (q.X * q.X + q.Y * q.Y))) / Math.PI * 180.0);
            Double rx = (Math.Asin((2.0 * (q.W * q.X - q.Z * q.Y))) / Math.PI * 180.0);
            Double rz = (Math.Atan2((2.0 * (q.W * q.Z + q.X * q.Y)), (1.0 - 2.0 * (q.X * q.X + q.Z * q.Z))) / Math.PI * 180.0);
            return new Vector3D(rx, ry, rz);
        }

        /// <summary>
        /// クォータニオンからダブルの配列に変換します。
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public static double[] ToDoubleArray(this Quaternion q)
        {
            return new double[] { q.X, q.Y, q.Z, q.W };
        }

        /// <summary>
        /// クォータニオンからダブルの配列に変換します。
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public static float[] ToFloatArray(this Quaternion q)
        {
            return new float[] { (float)q.X, (float)q.Y, (float)q.Z, (float)q.W };
        }
    }

    /// <summary>
    /// アクティブモデル変更時イベントの引数クラスです。
    /// </summary>
    public class ActiveModelChangedEventArgs : EventArgs
    {
        public ActiveModelChangedEventArgs(IMMDModel currentActiveModel)
        {
            this.CurrentActiveModel = currentActiveModel;
        }

        public ActiveModelChangedEventArgs(string currentActiveModelName)
        {
            this.CurrentActiveModel = new SimpleMMDModel() { ModelName = currentActiveModelName };
        }

        /// <summary>
        /// 現在のアクティブなモデル名
        /// </summary>
        public IMMDModel CurrentActiveModel { get; }
    }

    public interface IMMDModel
    {
        string ModelName { get; }
    }

    public interface IMMDModelWithExtension : IMMDModel
    {
        string Extension { get; }
    }

    public class SimpleMMDModel : IMMDModel
    {
        /// <summary>
        /// モデル名
        /// </summary>
        public string ModelName { get; set; }
    }
}