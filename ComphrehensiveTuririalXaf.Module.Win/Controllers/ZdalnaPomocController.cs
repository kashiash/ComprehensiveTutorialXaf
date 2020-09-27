using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensiveTutorialXaf.Module.Win.Controllers
{
  public  class ZdalnaPomocController: ViewController
    {
        SimpleAction ZdalnaPomocAction;
        public ZdalnaPomocController()
        {
            ZdalnaPomocAction = new SimpleAction(this, $"{GetType()}.{nameof(ZdalnaPomocAction)}", PredefinedCategory.Tools)
            {

                Caption = "Zdalna pomoc",

            };
            ZdalnaPomocAction.Execute += ZdalnaPomocAction_Execute;
        }

        private void ZdalnaPomocAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo { UseShellExecute = false, FileName = "mediqus_serwis.exe" };

            using (Process.Start(startInfo))
            {
            }
        }
    }
}
