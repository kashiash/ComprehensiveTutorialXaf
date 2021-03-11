# Logowanie z zewnętrznej aplikacji z parametrami

* 

* https://supportcenter.devexpress.com/ticket/details/k18099/how-to-show-a-specific-view-at-application-startup-right-after-the-logon-window-or-after#

* https://supportcenter.devexpress.com/ticket/details/q556109/how-to-open-a-view-using-information-from-command-line-arguments-passed-when-launching#

* https://supportcenter.devexpress.com/ticket/details/t693704/security-how-to-run-app-under-a-specific-user-and-log-in-automatically

* https://docs.devexpress.com/eXpressAppFramework/112982/task-based-help/security/how-to-use-custom-logon-parameters-and-authentication

Funkcjonalność pozwalająca wywołac konkretny Widok (ListView, DetailView) z linii komend


klasa do przekazywania parametrów:

```csharp
public static class SingleViewParameters
{

    public static string targetView = string.Empty;
    public static DevExpress.ExpressApp.View createdView;
    public static string userName = string.Empty;
    public static int objectId = 0;

}
```


przechwycenie parametrów:
    
    


```csharp
using System;
using System.Configuration;
using System.Windows.Forms;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Win;
using DevExpress.Logify.Win;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using Enimo.Module.BusinessObjects;
using JKXAF.CustomFunctions;
using kashiash.utils;

using Uta.Common;
using Uta.Module.Win.Controllers;

namespace Timex.Win
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] arguments)
        {
#if EASYTEST
            DevExpress.ExpressApp.Win.EasyTest.EasyTestRemotingRegistration.Register();
#endif

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            #region DEMO_REMOVE
            if (arguments.Length > 0)
            {
                string paramUser = (string)arguments.GetValue(0);
                if (paramUser.Contains("User="))
                {
                    SingleViewParameters.userName = paramUser.Replace("User=", "");
                }
                if (arguments.Length > 1)
                {
                    string paramView = (string)arguments.GetValue(1);
                    if (paramView.Contains("Type="))
                    {
                        SingleViewParameters.targetView = paramView.Replace("Type=", "");
                    }
                }
                if (arguments.Length > 2)
                {
                    string paramId = (string)arguments.GetValue(2);
                    if (paramId.Contains("Id="))
                    {
                        SingleViewParameters.objectId = XUtilConvert.ObjectToInt(paramId.Replace("Id=", ""));
                    }
                }
            }
            #endregion

            EditModelPermission.AlwaysGranted = System.Diagnostics.Debugger.IsAttached;
            if (Tracing.GetFileLocationFromSettings() == DevExpress.Persistent.Base.FileLocation.CurrentUserApplicationDataFolder)
            {
                Tracing.LocalUserAppDataPath = Application.LocalUserAppDataPath;
            }
            Tracing.Initialize();
            TimexWindowsFormsApplication winApplication = new TimexWindowsFormsApplication();
            SecurityAdapterHelper.Enable();
            winApplication.ConnectionString = AppConfig.ConnectionString;

            try
            {
                winApplication.UseLightStyle = true;
                winApplication.Setup();
                winApplication.Start();
            }
            catch (Exception e)
            {
                winApplication.HandleException(e);
            }
        }
        private static void WinApplication_CustomCheckCompatibility(object sender, CustomCheckCompatibilityEventArgs e)
        {
            e.Handled = true;
        }
    }
}
```



modyfikacja security prowidera:

```csharp
#region DEMO_REMOVE
AuthenticationStandard designedAuthentication = this.authenticationStandard1;
this.authenticationStandard1 = new AuthenticationStandardForTests();
this.securityStrategyComplex1.Authentication = this.authenticationStandard1;
this.authenticationStandard1.LogonParametersType = designedAuthentication.LogonParametersType;
this.authenticationStandard1.UserType = designedAuthentication.UserType;
#endregion
```



Widok, który obsluguje otwieranie okienek i ustawianie rozmiaru

```csharp
public class CustomViewController : ViewController
   {


       protected override void OnActivated()
       {
           base.OnActivated();
           //   PopupForm popupForm = Frame.Template as PopupForm;
           Form popupForm = Frame.Template as Form;
           if (popupForm != null)
           {
               popupForm.Shown += popupForm_Shown;
          
           }


           View.Closed += ClosedView;
       }
       // po zamknieciu view zamykamy całą aplikację
       private void ClosedView(object sender, EventArgs e)
       {
           if (View == SingleViewParameters.createdView)

           {
               ((DevExpress.ExpressApp.Win.WinShowViewStrategyBase)Application.ShowViewStrategy).CloseAllWindows();
               if (System.Windows.Forms.Application.MessageLoop)
               {
                   System.Windows.Forms.Application.Exit();
               }
               else
               {
                   System.Environment.Exit(1);
               }
           }
       }

       // ustawiamy rozmiar okna view
       void popupForm_Shown(object sender, EventArgs e)
       {
           Form templateForm = (Form)sender;
           templateForm.Shown -= popupForm_Shown;
           templateForm.WindowState = FormWindowState.Maximized;
           templateForm.MinimumSize = templateForm.Size;
           templateForm.MaximumSize = templateForm.Size;
       }
   }
```


i kontroler do wywołania odpowiednich view


```csharp
public class CustomWinShowStartupNavigationItemController : WinShowStartupNavigationItemController
 {
     protected override void ShowStartupNavigationItem(ShowNavigationItemController controller)
     {
         base.ShowStartupNavigationItem(controller);
         if (!string.IsNullOrEmpty(SingleViewParameters.targetView))
         {

             if (SingleViewParameters.objectId == 0) // nie mamy ID wiec wyswietlamy liste
             {
                 ((WinWindow)Application.MainWindow).Form.BeginInvoke(new MethodInvoker(() =>
               {
                   IObjectSpace os = Application.CreateObjectSpace();
                   DevExpress.ExpressApp.View view = Application.CreateListView(os, GetTypeByName(), true);
                   var sp = new ShowViewParameters();
                   sp.CreatedView = view;
                   sp.NewWindowTarget = NewWindowTarget.MdiChild;
                   sp.TargetWindow = TargetWindow.NewModalWindow;
                   var ss = new ShowViewSource(this.Frame, null);
                   SingleViewParameters.createdView = view;
                   Application.ShowViewStrategy.ShowView(sp, ss);
               }));
             }
             else // mamy id wiec wyswietlamy konkretny obiekt o tym id
             {
                 IObjectSpace os = Application.CreateObjectSpace();
                 object objectToEdit = GetObjectToEdit(os);

                 DetailView view = Application.CreateDetailView(os, SingleViewParameters.targetView, true, objectToEdit);
                 view.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;

                 var sp = new ShowViewParameters();
                 sp.CreatedView = view;
                 sp.NewWindowTarget = NewWindowTarget.MdiChild;
                 sp.TargetWindow = TargetWindow.NewModalWindow;
                 var ss = new ShowViewSource(this.Frame, null);
                 SingleViewParameters.createdView = view;
               
                 Application.ShowViewStrategy.ShowView(sp, ss);
             }
         }
     }

     private static object GetObjectToEdit(IObjectSpace os)
     {

         switch (GetObjectNameFromViewName())
         {

             case "UtaOne": return os.GetObjectByKey<UtaOne>(SingleViewParameters.objectId); ;
             case "Klienci": return os.GetObjectByKey<Klienci>(SingleViewParameters.objectId); ;
             case "Faktury": return os.GetObjectByKey<Faktury>(SingleViewParameters.objectId); ;
             case "Pojazdy": return os.GetObjectByKey<Pojazdy>(SingleViewParameters.objectId); ;
             case "Zabezpieczenia": return os.GetObjectByKey<Zabezpieczenia>(SingleViewParameters.objectId); ;
             case "Sprawy": return os.GetObjectByKey<YWindSprawy>(SingleViewParameters.objectId); ;
             case "Kontrahenci": return os.GetObjectByKey<YKontrahenci>(SingleViewParameters.objectId); ;
             case "RejestrZdarzen": return os.GetObjectByKey<RejestrZdarzen>(SingleViewParameters.objectId);
             //case "StacjePaliw": return typeof(StacjePaliw);
             //case "Koncerny": return typeof(Koncerny);
             //case "Decyzje": return typeof(DecyzjeEH);
             //case "Wytyczne": return typeof(WytyczneDecyzji);

             default: return os.GetObjectByKey<UtaOne>(SingleViewParameters.objectId); ;
         }
     }

     private static string GetObjectNameFromViewName()
     {
         int length = SingleViewParameters.targetView.IndexOf("_");
         var s=  SingleViewParameters.targetView.Substring(0, length);
         return s;
     }

     private Type GetTypeByName()
     {
         switch (SingleViewParameters.targetView)
         {
             case "UtaOne": return typeof(UtaOne);
             case "Klienci": return typeof(Klienci);
             case "Faktury": return typeof(Faktury);
             case "Pojazdy": return typeof(Pojazdy);
             case "Zabezpieczenia": return typeof(Zabezpieczenia);
             case "Sprawy": return typeof(YWindSprawy);
             case "Kontrahenci": return typeof(YKontrahenci);
             case "RejestrZdarzen": return typeof(RejestrZdarzen);
             case "StacjePaliw": return typeof(StacjePaliw);
             case "Koncerny": return typeof(Koncerny);
             case "Decyzje": return typeof(DecyzjeEH);
             case "Wytyczne": return typeof(WytyczneDecyzji);
             case "ZamUta": return typeof(ZamUta);

             default: return typeof(UtaOne);
         }
     }
 }
```


### materiały żródłowe

* <a href="https://docs.devexpress.com/eXpressAppFramework/112803/concepts/ui-construction/views/ways-to-show-a-view" target="_blank">https://docs.devexpress.com/eXpressAppFramework/112803/concepts/ui-construction/views/ways-to-show-a-view</a>


* <a href="https://supportcenter.devexpress.com/ticket/details/t693704/security-how-to-run-app-under-a-specific-user-and-log-in-automatically" target="_blank">https://supportcenter.devexpress.com/ticket/details/t693704/security-how-to-run-app-under-a-specific-user-and-log-in-automatically</a>

* <a href="https://docs.devexpress.com/eXpressAppFramework/112982/task-based-help/security/how-to-use-custom-logon-parameters-and-authentication" target="_blank">https://docs.devexpress.com/eXpressAppFramework/112982/task-based-help/security/how-to-use-custom-logon-parameters-and-authentication</a>

* <a href="https://supportcenter.devexpress.com/ticket/details/q556109/how-to-open-a-view-using-information-from-command-line-arguments-passed-when-launching" target="_blank">https://supportcenter.devexpress.com/ticket/details/q556109/how-to-open-a-view-using-information-from-command-line-arguments-passed-when-launching</a>

* <a href="https://supportcenter.devexpress.com/ticket/details/k18099/how-to-show-a-specific-view-at-application-startup-right-after-the-logon-window-or-after" target="_blank">https://supportcenter.devexpress.com/ticket/details/k18099/how-to-show-a-specific-view-at-application-startup-right-after-the-logon-window-or-after</a>


### dla wersji webowej do rozpoznania w przyszłości:

* <a href="https://dennisgaravsky.blogspot.com/2015/05/redirecting-from-external-hyperlink-to.html" target="_blank">https://dennisgaravsky.blogspot.com/2015/05/redirecting-from-external-hyperlink-to.html</a>

* <a href="https://supportcenter.devexpress.com/ticket/details/t344730/how-to-get-a-hyperlink-or-url-for-a-certain-view-of-an-xaf-web-application" target="_blank">https://supportcenter.devexpress.com/ticket/details/t344730/how-to-get-a-hyperlink-or-url-for-a-certain-view-of-an-xaf-web-application</a>