using MikuMikuPlugin;
using MyUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBlinkerPlugin.UI
{
    public class frmMainMMM : frmMainBase
    {
        private Scene _scene = null;

        public frmMainMMM(Scene scene)
        {
            this._scene = scene;

            this.ShowInTaskbar = false;

            base.mmdSelectorControl1.Visible = false;
            this.Height -= base.mmdSelectorControl1.Height - 50;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            //
            // mmdSelectorControl1
            //
            this.mmdSelectorControl1.Location = new System.Drawing.Point(0, 373);
            this.mmdSelectorControl1.Size = new System.Drawing.Size(301, 52);
            //
            // lblModel
            //
            this.lblModel.Size = new System.Drawing.Size(0, 20);
            this.lblModel.Text = "";
            //
            // frmMainMMM
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(301, 373);
            this.Name = "frmMainMMM";
            this.Load += new System.EventHandler(this.frmMainMMM_Load);
            this.ResumeLayout(false);
        }

        private void frmMainMMM_Load(object sender, EventArgs e)
        {
        }

        public void ModelChanged(Scene scene)
        {
            this.Text = $"まばたき作成";
            this._currentModel = null;
            if (scene?.ActiveModel != null && scene.ActiveModel != this._currentModel)
            {
                var currentModel = MMMModel2ModelItem(scene.ActiveModel);
                this.ApplyModel(currentModel);
            }
        }

        private ModelItem MMMModel2ModelItem(Model model)
        {
            var ret = new ModelItem();
            ret.ModelName = model.Name;
            ret.EyeMorphItems.AddRange(model.Morphs.Where(n => n.PanelType == PanelType.Eyes).Select(n => this.MMMMorph2MorphItem(n)));
            ret.BrowMorphItems.AddRange(model.Morphs.Where(n => n.PanelType == PanelType.Brow).Select(n => this.MMMMorph2MorphItem(n)));
            ret.Bones.AddRange(model.Bones.Select(n => n.Name));

            return ret;
        }

        private MorphItem MMMMorph2MorphItem(Morph morph)
        {
            var ret = new MorphItem();
            ret.MorphType = morph.PanelType.ToMyPanelType();
            ret.Weight = morph.CurrentWeight;
            ret.MorphName = morph.Name;
            ret.ComboBoxIndex = -1;
            return ret;
        }
    }
}