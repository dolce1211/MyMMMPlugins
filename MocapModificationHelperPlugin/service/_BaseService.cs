using MikuMikuPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyUtility;

namespace MoCapModificationHelperPlugin.service
{
    public abstract class BaseService
    {
        protected Scene Scene { get; set; }

        /// <summary>
        /// MMMのメインフォーム
        /// </summary>
        protected Form ApplicationForm { get; set; }

        public virtual void Initialize(Scene scene, IWin32Window applicationForm)
        {
            this.Scene = scene;
            this.ApplicationForm = (Form)applicationForm;
        }

        public void Execute(ConfigItem config)
        {
            this.ApplicationForm.Cursor = Cursors.WaitCursor;
            BeginAndEndUpdate(false);
            ServiceFactory.IsBusy = true;

            try
            {
                if (!PreExecute())
                    return;

                if (ExecuteInternal(config))
                    this.ApplicationForm.Refresh();
            }
            finally
            {
                this.ApplicationForm.Cursor = Cursors.Default;
                BeginAndEndUpdate(true);
                this.ApplicationForm.Refresh();
                Task.Delay(200).ContinueWith((t) =>
                {
                    ServiceFactory.IsBusy = false;
                });
            }
            PostExecute();
        }

        /// <summary>
        /// ウィンドウの描画を一時的に止めたり再開したりします。
        /// </summary>
        /// <param name="flg">false:描画を止める true:描画を再開する</param>
        protected internal void BeginAndEndUpdate(bool flg)
        {
            MMDUtil.MMDUtilility.BeginAndEndUpdate(this.ApplicationForm.Handle, flg);
            var otherWindow = MMDUtil.MMMUtilility.TryGetOtherWindow(true);
            if (otherWindow != null)
                MMDUtil.MMDUtilility.BeginAndEndUpdate(otherWindow.hWnd, flg);
        }

        public abstract bool ExecuteInternal(ConfigItem config);

        public virtual bool PreExecute()
        {
            return Scene.ActiveModel != null;
        }

        public virtual void PostExecute()
        { }
    }
}