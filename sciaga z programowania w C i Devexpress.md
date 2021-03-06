# sciaga z programowania w C# i Devexpress

## zbiór artykułów ze Strony Devexpress, które warto przeczytać
miejsca na stronie devexpress, do których czesto wracam


### Relacje w XPO

https://docs.devexpress.com/XPO/2041/create-a-data-model/relationships-between-objects

### Filtrowanie list / droplist
https://docs.devexpress.com/eXpressAppFramework/112681/task-based-help/filtering/how-to-implement-cascading-filtering-for-lookup-list-views

### budowanie zapytań w xpo
https://supportcenter.devexpress.com/ticket/details/k18431/xpo-how-to-select-data-from-multiple-tables

https://docs.devexpress.com/XPO/2034/query-and-shape-data

https://docs.devexpress.com/XPO/2537/query-and-shape-data/simplified-criteria-syntax

https://docs.devexpress.com/CoreLibraries/4928/devexpress-data-library/criteria-language-syntax

### XPI i LINQ
https://docs.devexpress.com/XPO/4060/query-and-shape-data/linq-to-xpo


### Jak XPO ładuje obiekty i kolekcje
https://supportcenter.devexpress.com/ticket/details/a643/how-xpo-reloads-objects-and-collections#

### XPO Dobre praktyki
https://supportcenter.devexpress.com/ticket/details/a2944/xpo-best-practices#

### Session management and casching
https://community.devexpress.com/blogs/xpo/archive/2006/03/27/session-management-and-caching.aspx

### How to: Use XPO Upcasting in XAF
upcasting czyli po naszemu rzutowanie typu

https://docs.devexpress.com/eXpressAppFramework/112797/task-based-help/business-model-design/express-persistent-objects-xpo/how-to-use-xpo-upcasting-in-xaf

### Atrybuty 
https://docs.devexpress.com/eXpressAppFramework/112701/concepts/business-model-design/data-annotations-in-data-model



### Wczytywanie plików z katalogu (tu export danych z excela )
* wczytywanie danych z ObjectSpace
* import z excela
* operacje na plikach
```csharp
var FilesDirectory = @"Schematy\DoZaimportowania\";
Console.WriteLine("Rozpoczeto import!");
//if (Directory.Exists(FilesDirectory))
//{

var files = Directory.GetFiles(FilesDirectory, "*.*");


using (XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(ZywienieCommonKonfiguracja.ConnectionString, null))
{
    using (IObjectSpace directObjectSpace = directProvider.CreateObjectSpace())
    {
        XafTypesInfo.Instance.RegisterEntity(typeof(Schemat));
        XafTypesInfo.Instance.RegisterEntity(typeof(PozycjaSchematu));
        foreach (var file in files)
        {
            Console.WriteLine($"Import pliku: {file}");
            var importer = new ImportSchematu(directObjectSpace);
            importer.ImportFile(file,1);
            directObjectSpace.CommitChanges();
        }
    }
}

//    }
Console.ReadKey();
```

### Operowanie na danych z użyciem UnitOfWork
* wczytywanie danych z UnitOfWork
```csharp

    using(var unitOfWork = new UnitOfWork(session.DataLayer))

    {
        try
        {
            Console.WriteLine("Import ");
            ImportData(unitOfWork, fileName);
            unitOfWork.CommitChanges();
        } catch(Exception ex)
        {
            MailSender.SendEmail(
                "Wystąpił wyjątek podczas importu  ",
                ex.ToString(),
                "jacek.kosinski@uta.pl");

            unitOfWork.CommitChanges();

            throw;
        }
    }

```



### Kontroler na liście pozwalający wybrać pliki do importu
* import z excela
* operacje na plikach
* Kontroler z simple action pozwalajacy wybrać pliki do importu
```csharp
public class SchematListViewController :ViewController<DevExpress.ExpressApp.ListView>
 {

     SimpleAction importSchematAction ;
     public SchematListViewController()
     {
         importSchematAction = new SimpleAction(this, $"{GetType().FullName}.{nameof(importSchematAction)}", DevExpress.Persistent.Base.PredefinedCategory.Unspecified) { 
         Caption= "Importuj schemat"
         
         };
         importSchematAction.Execute += ImportSchematAction_Execute;
     }

     private void ImportSchematAction_Execute(object sender, SimpleActionExecuteEventArgs e)
     {
         foreach (var file in PobierzPliki())
         {
             Console.WriteLine($"Import pliku: {file}");
             var importer = new ImportSchematu(ObjectSpace);
             importer.ImportFile(file, 1);
             ObjectSpace.CommitChanges();
         }
     }

     public string[] PobierzPliki()
     {

         OpenFileDialog openFileDialog1 = new OpenFileDialog();

         openFileDialog1.Filter = "(*.xls)|*.xls|(*.xlsx)|*.xlsx|All files (*.*)|*.*";
         // openFileDialog1.FilterIndex = 1;
         openFileDialog1.RestoreDirectory = true;
         openFileDialog1.Multiselect = true;

         if (openFileDialog1.ShowDialog() == DialogResult.OK)
         {

             return openFileDialog1.FileNames;
         }

         return new string[0];
     }
 }
```
