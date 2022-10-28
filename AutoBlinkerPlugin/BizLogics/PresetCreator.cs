using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBlinkerPlugin
{
    public class PresetCreator
    {
        public static List<FavEntity> CreateFavs()
        {
            var ret = new List<FavEntity>();

            ret.Add(new FavEntity()
            {
                FavName = "シンプル",
                DoHokan = true
            });

            ret.Add(new FavEntity()
            {
                FavName = "ぴょこん",
                DoHokan = true,
                DoEyeSync = true,
                DoEyebrowSync = true,
                DoHandouStart = true,
                DoHandouEnd = true,
                EnterFrames=3,
                BlinkingFrames=4,
                HandouFramesStart=3,
                HandouFramesEnd=3,
            });

            ret.Add(new FavEntity()
            {
                FavName = "リアル風",
                DoHokan = true,
                DoEyeSync = true,
                DoEyebrowSync = true,
                DoYuruyaka = true,
                ExitFrames = 6,
            });

            return ret;
        }

        public static List<string> CreateExceptions()
        {
            return new List<string>()
            {
                "ハイライト",
                "ハイライト拡大",
                "ハイライト大",
                "ハイライト消",
                "ハイライト消し",
                "ハイライト縮小",
                "ハイライト小",
                "瞳小",
                "瞳縮小",
                "瞳大",
                "瞳拡大",
                "ぐるぐる",
                "ぐるぐる目",
                "グルグル",
                "グルグル目",
                "絶望",
                "ハート",
                "ハート目",
                "ハート瞳",
                "♡",
                "はぁと",
                "きらきら",
                "きらきら目",
                "キラキラ",
                "キラキラ目",
                "キラキラ拡大",
                "キラキラ縮小",
                "しいたけ",
                "☆",
                "星",
                "星目",
                "星瞳",
                "はちゅ目",
                "涙",
                "瞳丸",
                "縦潰し",
                "横潰し",
                "瞳横潰",
                "瞳縦潰",
                "瞳縦",
                "瞳横",
                "丸目",
                "瞳消し",
                "白目",
                "恐ろしい子！",
                "ｺｯﾁﾐﾝﾅ",
                "コッチミンナ",
                "こっち見んな",
                "こっちみんな",
                "カメラ目線",
                "ガーン",
                "ガーン目",
            };
        }
    }
}