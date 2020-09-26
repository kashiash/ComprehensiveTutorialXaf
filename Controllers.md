*Kontrolery*

https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ViewController
https://blog.delegate.at/2018/03/10/xaf-best-practices-2017-03.html

### Standardowa wersja kontrolera

```csharp
public class ClearContactsTaskControllerV1 : ViewController
{
    SimpleAction ClearTasksAction;
    public ClearContactsTaskControllerV1()
    {
        TargetObjectType = typeof(Contact); //Wskazujemy, ze dotyczy tylko obiektów klasy Contact
        TargetViewType = ViewType.DetailView; // Dotyczy tylko widoków typu DetailView
    }
    protected override void OnActivated()
        {
            base.OnActivated();
        }
    protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }
   // ...
}
```
dokładnie ten sam efekt osiągniemy definiując kontroler w następujący sposób:
```csharp
public class ClearContactsTaskController : ViewController<DetailView>
{
    SimpleAction ClearTasksAction;
    public ClearContactsTaskController()
    {
        TargetObjectType = typeof(Contact);
        //TargetViewType = ViewType.DetailView; to juz jest niepotrzebne
}
...
```

albo tak:

```csharp
public class ClearContactsTaskControllerV2 : ObjectViewController<DetailView,Contact>
{
    SimpleAction ClearTasksAction;
    public ClearContactsTaskControllerV2()
    {
        //TargetObjectType = typeof(Contact); to juz nie jest potrzebne
        //TargetViewType = ViewType.DetailView;
    }
   // ...
}
```

## Kontroler pozwalający na dostęp do ustawień Grid'a na liście danych

[Na podstawie artykułu na stronie devexpress](https://docs.devexpress.com/eXpressAppFramework/113165/getting-started/comprehensive-tutorial/extend-functionality/access-grid-control-properties)

```csharp
public  class WinListViewController: ViewController<ListView>
  {
      GridListEditor gridListEditor = null;
      public WinListViewController()
      {
          ViewControlsCreated += WinAlternatingRowsController_ViewControlsCreated;
      }



      private void WinAlternatingRowsController_ViewControlsCreated(object sender, EventArgs e)
      {
          GridListEditor listEditor = ((ListView)View).Editor as GridListEditor;
          if (listEditor != null)
          {
              GridView gridView = listEditor.GridView;
              gridView.OptionsView.EnableAppearanceOddRow = true;
              //  gridView.Appearance.OddRow.BackColor = Color.FromArgb(244, 244, 244);
              gridView.OptionsView.ShowFooter = false;
              // gridView.OptionsView.GroupFooterShowMode = GroupFooterShowMode.VisibleIfExpanded;
              gridView.OptionsPrint.ExpandAllGroups = false;
              //   właczamy filtry pod nagłowkami
              gridView.OptionsView.ShowAutoFilterRow = true;
              // właczamy scroll - ustaw false

              //checboxy do zaznaczania
              //gridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;


              Object currentObject = this.View.Model.ModelClass;
              if (currentObject != null && currentObject.GetType() == typeof(DemoTask))
              {
                  // aby otrzymać multiline na gridzie
                  // właczamy zmiane rozmiru kolumn
                  gridView.OptionsView.RowAutoHeight = true;
                  gridView.OptionsView.ColumnAutoWidth = true;
              }

              else
              {
                  gridView.OptionsView.ColumnAutoWidth = false;
              }

          }
      }


  }
```

#### Simple Action

```csharp
public class ClearContactsTaskController : ViewController<DetailView>
 {

     SimpleAction ClearTasksAction;

     public ClearContactsTaskController()
     {
         TargetObjectType = typeof(Contact);


         ClearTasksAction = new SimpleAction(this, $"{GetType().FullName}.{nameof(ClearTasksAction)} ", PredefinedCategory.Edit)
         {
             Caption = "Clear tasks",
             ImageName = "Action_Clear",
             PaintStyle = ActionItemPaintStyle.Image,
             ToolTip = "Clear tasks",
             ConfirmationMessage = "Jesteś pewny tego czynu?",
             SelectionDependencyType = SelectionDependencyType.RequireSingleObject,
         };
         ClearTasksAction.Execute += ClearTasks;
     }
     private void ClearTasks(object sender, SimpleActionExecuteEventArgs e)
     {
         var currentObject = View.CurrentObject as Contact;

         while (currentObject.Tasks.Count > 0)
         {
             currentObject.Tasks.Remove(currentObject.Tasks[0]);
         }
         ObjectSpace.SetModified(currentObject);
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
```



#### Popup Window Action


##### Show Notes z tutoriala

```csharp
public  class PopupNotesController :ObjectViewController<DetailView,DemoTask>
    {
        PopupWindowShowAction ShowNotesAction;
        public PopupNotesController()
        {
            ShowNotesAction = new PopupWindowShowAction(this, $"{GetType().FullName}{nameof(ShowNotesAction)}", PredefinedCategory.Edit)
            {
                Caption = "Pokaż notatki",

            };
            ShowNotesAction.CustomizePopupWindowParams += ShowNotesAction_CustomizePopupWindowParams;
            ShowNotesAction.Execute += ShowNotesAction_Execute;
        }

        private void ShowNotesAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            string noteListViewId = Application.FindLookupListViewId(typeof(Note));
            CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(Note), noteListViewId);
            e.View = Application.CreateListView(noteListViewId, collectionSource, true);
        }

        private void ShowNotesAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            DemoTask task = (DemoTask)View.CurrentObject;
            foreach (Note note in e.PopupWindowViewSelectedObjects)
            {
                if (!string.IsNullOrEmpty(task.Description))
                {
                    task.Description += Environment.NewLine;
                }
                task.Description += note.Text;
            }
            if (((DetailView)View).ViewEditMode == ViewEditMode.View)
            {
                View.ObjectSpace.CommitChanges();
            }
        }
    }
```


##### Wywołanie okna popup z wybranym obiektem z listy

```csharp
public class ShowDetailViewController : ViewController<ListView> {
    public ShowDetailViewController() {
        PopupWindowShowAction showDetailViewAction = new PopupWindowShowAction(
            this, "ShowDetailView", PredefinedCategory.Edit);
            
            
        showDetailViewAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
        showDetailViewAction.TargetObjectsCriteria = "Not IsNewObject(This)";
        showDetailViewAction.CustomizePopupWindowParams += showDetailViewAction_CustomizePopupWindowParams;
    }
    void showDetailViewAction_CustomizePopupWindowParams(
        object sender, CustomizePopupWindowParamsEventArgs e) {
        IObjectSpace newObjectSpace = Application.CreateObjectSpace(View.ObjectTypeInfo.Type);
        Object objectToShow = newObjectSpace.GetObject(View.CurrentObject);
        if (objectToShow != null) {
            DetailView createdView = Application.CreateDetailView(newObjectSpace, objectToShow);
            createdView.ViewEditMode = ViewEditMode.Edit;
            e.View = createdView;
        }
    }
}
```

