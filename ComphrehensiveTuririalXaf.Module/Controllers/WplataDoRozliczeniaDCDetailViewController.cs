using ComprehensiveTutorialXaf.Module.BusinessObjects;
using Demo1.Module.BusinessObjects;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensiveTutorialXaf.Module.Controllers
{
   public class WplataDoRozliczeniaDCDetailViewController :ObjectViewController<DetailView, WplataDoRozliczeniaDC>

    {

        SimpleAction PrzypiszFaktury;
        SimpleAction WyczyscListeFaktur;
        PopupWindowShowAction WybierzFakturyAction;
        public WplataDoRozliczeniaDCDetailViewController()
        {
            PrzypiszFaktury = new SimpleAction(
                this, $"{GetType().FullName}.{nameof(PrzypiszFaktury)}",
                DevExpress.Persistent.Base.PredefinedCategory.Edit)
            { 
            Caption = "Przypisz faktury",
            ImageName = "Action_Search",


            };
            PrzypiszFaktury.Execute += PrzypiszFaktury_Execute;

            WyczyscListeFaktur = new SimpleAction(
                this,
                $"{GetType().FullName}.{nameof(WyczyscListeFaktur)}",
                DevExpress.Persistent.Base.PredefinedCategory.Edit) {
                Caption = "Wyczyść listę faktur",
                ImageName = "Action_Clear",
            };
            WyczyscListeFaktur.Execute += WyczyscListeFaktur_Execute;

            WybierzFakturyAction = new PopupWindowShowAction(this, $"{GetType().FullName}.{nameof(WybierzFakturyAction)}", DevExpress.Persistent.Base.PredefinedCategory.Edit) { 
            Caption = "Wybierz faktury",
            ImageName= "BO_Invoice",
            };
            WybierzFakturyAction.CustomizePopupWindowParams += WybierzFakturyAction_CustomizePopupWindowParams;
            WybierzFakturyAction.Execute += WybierzFakturyAction_Execute;
        }



        private void WybierzFakturyAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
       
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            var roz = objectSpace.GetObject(ViewCurrentObject);
            var kli = objectSpace.GetObject(roz.Wplata.Klient);
            CriteriaOperator co =  CriteriaOperator.Parse("WartoscBrutto > SumaWplat And Klient = ?", kli);
            CollectionSourceBase collectionSource = new CollectionSource(objectSpace,typeof(Faktura));
         
            collectionSource.Criteria["FakturyDoSplaty"] = co; 
           string fakturaListViewId = Application.FindLookupListViewId(typeof(Faktura));
            e.View = Application.CreateListView(fakturaListViewId, collectionSource, true);
        }

        private void WybierzFakturyAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            var rozliczenie = ViewCurrentObject;
            foreach (var obj in e.PopupWindowViewSelectedObjects)
            {

                var faktura = (Faktura)ObjectSpace.GetObject(obj);
                var fakturaDoRozliczenia = new FakturaDoRozliczeniaDC();
                fakturaDoRozliczenia.Faktura = faktura;
        
                var kwotaDoRozliczenia = rozliczenie.Wplata.Nadplata - rozliczenie.SumaRozliczen;

                var kwotaFaktury = faktura.WartoscBrutto - faktura.SumaWplat;
                var kwotaDoFaktury = kwotaDoRozliczenia > kwotaFaktury ? kwotaFaktury  : kwotaDoRozliczenia;

                fakturaDoRozliczenia.KwotaDoRozliczenia = kwotaDoFaktury;

                rozliczenie.Naleznosci.Add(fakturaDoRozliczenia);
             //   rozliczenie.SumaRozliczen += kwotaDoFaktury ;
            }
            View.Refresh();
        }

        private void WyczyscListeFaktur_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var rozliczenie = ViewCurrentObject;
            rozliczenie.Naleznosci.Clear();
            View.Refresh();
        }

        private void PrzypiszFaktury_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var kwotaDoRozliczenia = RozliczanieWplatHelper.WyszukajFaktury(ObjectSpace, ViewCurrentObject);
            View.Refresh();
        }
    }
}
