using ComprehensiveTutorialXaf.Module.BusinessObjects;
using Demo1.Module.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Templates;
using DevExpress.Office.Utils;
using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensiveTutorialXaf.Module.Controllers
{
    public class WplatyController : ViewController
    {
        public WplatyController()
        {
            TargetObjectType = typeof(Wplata);
            PopupWindowShowAction rozliczWplateAction =
                new PopupWindowShowAction(this, $"{GetType().FullName}.{nameof(rozliczWplateAction)}", PredefinedCategory.Edit)
                {
                    Caption = "Rozlicz wpłatę",
                    ImageName = "BO_Payment",
                    PaintStyle = ActionItemPaintStyle.Image,
                    ToolTip = "Przypisuje wpłate do nierozliczonych faktur",
                    ConfirmationMessage = "Jesteś pewny tego czynu?",
                    SelectionDependencyType = SelectionDependencyType.RequireSingleObject,
                };
            rozliczWplateAction.CustomizePopupWindowParams += rozliczWplateAction_CustomizePopupWindowParams;
            rozliczWplateAction.Execute += rozliczWplateAction_Execute;
        }

        private void rozliczWplateAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {



            IObjectSpace objectSpace = Application.CreateObjectSpace();
            e.Context = TemplateContext.PopupWindow;
            var rozliczenie = new WplataDoRozliczeniaDC();
            rozliczenie.Wplata = (Wplata)objectSpace.GetObject(View.CurrentObject);
            PrzygotujListeFakturDoRozliczenia(objectSpace,rozliczenie);
            e.View = Application.CreateDetailView(objectSpace, rozliczenie);
            ((DetailView)e.View).ViewEditMode = ViewEditMode.Edit;
        }
        private void rozliczWplateAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
          //  IObjectSpace objectSpace = Application.CreateObjectSpace();
            var wplata = (Wplata)ObjectSpace.GetObject(View.CurrentObject);
            if (wplata != null)
            { WplataDoRozliczeniaDC parameters = e.PopupWindow.View.CurrentObject as WplataDoRozliczeniaDC;
                foreach (var obj in parameters.Naleznosci)
                {
                    var roz = ObjectSpace.GetObject(obj);
                    if (roz.KwotaDoRozliczenia != 0)
                    {
                        var rozrachunek = ObjectSpace.CreateObject<Rozrachunek>();
                        rozrachunek.Wplata = wplata;
                        rozrachunek.Faktura = ObjectSpace.GetObject(roz.Faktura); 
                        rozrachunek.Kwota = roz.KwotaDoRozliczenia;
                    }

                }
            }
            //if (View is DetailView && ((DetailView)View).ViewEditMode == ViewEditMode.View)
            //{
            //    objectSpace.CommitChanges();
            //}
            if (View is ListView)
            {
                ObjectSpace.CommitChanges();
                View.ObjectSpace.Refresh();
            }
        }

        private void PrzygotujListeFakturDoRozliczenia(IObjectSpace objectSpace, WplataDoRozliczeniaDC rozliczenie)
        {

   
          RozliczanieWplatHelper.WyszukajFaktury(objectSpace,rozliczenie);

        }



        protected override void OnActivated()
        {
            base.OnActivated();
        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }
    }


}
