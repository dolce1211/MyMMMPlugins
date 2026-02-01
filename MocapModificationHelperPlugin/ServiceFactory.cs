using MikuMikuPlugin;
using MoCapModificationHelperPlugin.service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoCapModificationHelperPlugin
{
    internal class ServiceFactory
    {
        public static bool IsBusy { get; set; } = false;

        public static BaseService Create(ServiceType service, Scene scene, IWin32Window applicationForm)
        {
            BaseService ret = null;
            switch (service)
            {
                case ServiceType.GapSelectorService:
                    ret = new service.GapSelectorService();
                    break;

                case ServiceType.ModifiedLayerSelectorService:
                    ret = new service.ModifiedLayerSelectorService();
                    break;

                case ServiceType.LayerBoneSelectorService:
                    ret = new service.LayerBoneSelectorService();
                    break;

                case ServiceType.SelectedKeysSaverService:
                    ret = new service.SelectedKeysSaverService();
                    break;

                case ServiceType.SelectedKeysLoaderService:
                    ret = new service.SelectedKeysLoaderService();
                    break;

                case ServiceType.InterpolateSetterService:
                    ret = new service.InterpolateService();
                    break;

                case ServiceType.FillDisplayFramesService:
                    ret = new service.FillDisplayFramesService();
                    break;

                case ServiceType.BlinkCancellerService:
                    ret = new service.BlinkCancellerService();
                    break;

                case ServiceType.EnableReverseMorphService:
                    ret = new service.EnableReverseMorphService();
                    break;

                default:
                    return null;
            }
            if (ret != null)
            {
                ret.Initialize(scene, applicationForm);
                return ret;
            }
            return null;
        }
    }
}