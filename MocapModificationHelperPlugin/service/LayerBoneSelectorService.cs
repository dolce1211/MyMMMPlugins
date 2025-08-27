using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoCapModificationHelperPlugin.service
{
    /// <summary>
    /// 選択中のキーからレイヤーボーンのみを抽出して選択する機能を提供するクラス
    /// </summary>
    internal class LayerBoneSelectorService : BaseService
    {
        public override bool ExecuteInternal(int mode)
        {
            if
                (this.Scene.ActiveModel == null)
                return false;

            //レイヤーボーンのみ選択する
            var selectedMainLayers = this.Scene.ActiveModel.Bones.SelectMany(n => n.SelectedLayers.Where(l =>
            {
                if (l.Selected)
                {
                    if (mode == 1)
                    {
                        //shift押してるとメインレイヤーのみ残す
                        return (l.LayerID != 0);
                    }
                    else
                    {
                        //shift押してないとメインレイヤー以外を残す
                        return (l.LayerID == 0);
                    }
                }
                return false;
            }
            )).ToList();

            if (selectedMainLayers != null)
            {
                selectedMainLayers.ForEach(n => n.Selected = false);
                selectedMainLayers.SelectMany(n => n.Frames
                                            .Where(m => m.Selected)).ToList()
                                  .ForEach(n => n.Selected = false);
            }
            return true;
        }
    }
}