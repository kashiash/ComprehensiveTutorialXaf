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


        }

        private void PrzygotujListeFakturDoRozliczenia(IObjectSpace objectSpace, WplataDoRozliczeniaDC rozliczenie)
        {
          //  var listaFaktur = new BindingList<FakturaDoRozliczeniaDC>();
            var faktury = objectSpace.GetObjectsQuery<Faktura>().Where(f => f.Klient == rozliczenie.Wplata.Klient && f.SumaWplat < f.WartoscBrutto);

            decimal? kwotaDoRozliczenia = rozliczenie.Wplata.KwotaWplaty;
            foreach (var faktura in faktury)
            {
                //var pozycja = objectSpace.CreateObject<FakturaDoRozliczenia>(); // to nie zadziała bo faktura do rozliczania nie jest XPO !!!!
                var pozycja = new FakturaDoRozliczeniaDC();
                pozycja.Faktura = faktura;
                var kwotaFaktury = faktura.WartoscBrutto - faktura.SumaWplat;
                if (kwotaDoRozliczenia > kwotaFaktury)
                {
                    pozycja.KwotaDoRozliczenia = kwotaFaktury;
                    kwotaDoRozliczenia -= kwotaFaktury;
                }
                else
                {
                    pozycja.KwotaDoRozliczenia = kwotaDoRozliczenia;
                    kwotaDoRozliczenia = 0;
                }
                rozliczenie.Naleznosci.Add(pozycja);

            }

        
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
