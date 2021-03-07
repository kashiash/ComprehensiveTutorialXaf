# sciaga z programowania w C# i Devexpress


## Zawartość
* [Wstęp](#wstęp)
* [Interesujace linki](#linki)
* [Przykłady aplikacji w XAF](#sample-projects)
* [Podstawy w XPO](#podstawy-xpo)
* [Podstawy w XAF](#podstawy-xaf)
* [Gotowe fragmenty kodu do wykorzystania](#fragmenty-kodu)
* <a href="Controllers.md" target="_blank">Kontrolery</a>
* <a href="Views.md" target="_blank">Widoki w XAF</a>

## Wstęp

Po co pamietać, skoro można zapisać ;)


## Linki
Zbiór artykułów ze Strony Devexpress, które warto przeczytać
miejsca na stronie devexpress, do których czesto wracam, a nie zawsze pamiętam jak je znaleźć.

#### Dokumentacja XAF
* <a href="https://docs.devexpress.com/eXpressAppFramework/112670/expressapp-framework" target="_blank">Dokumentacja XAF</a>


* <a href="https://github.com/jjcolumb/awesome-xaf" target="_blank">zbiór wszystkiego o XAF</a>


## Sample projects
* [Sample Projects](https://github.com/DevExpress-Examples)-Official Sample Projects, by DevExpress.



## Podstawy XPO



* <a href="https://docs.devexpress.com/XPO/2041/create-a-data-model/relationships-between-objects" target="_blank">Relationships Between Objects</a>

* <a href="https://docs.devexpress.com/eXpressAppFramework/113569/concepts/business-model-design/data-types-supported-by-built-in-editors/collection-properties/collection-properties-in-xpo" target="_blank">Collection Properties in XPO</a>

#### budowanie zapytań w xpo
* https://supportcenter.devexpress.com/ticket/details/k18431/xpo-how-to-select-data-from-multiple-tables

* https://docs.devexpress.com/XPO/2034/query-and-shape-data

* https://docs.devexpress.com/XPO/2537/query-and-shape-data/simplified-criteria-syntax

* https://docs.devexpress.com/CoreLibraries/4928/devexpress-data-library/criteria-language-syntax


#### XPO i LINQ
https://docs.devexpress.com/XPO/4060/query-and-shape-data/linq-to-xpo


#### Jak XPO ładuje obiekty i kolekcje
https://supportcenter.devexpress.com/ticket/details/a643/how-xpo-reloads-objects-and-collections#

#### XPO Dobre praktyki
https://supportcenter.devexpress.com/ticket/details/a2944/xpo-best-practices#

#### Session management and caching
https://community.devexpress.com/blogs/xpo/archive/2006/03/27/session-management-and-caching.aspx

#### How to: Use XPO Upcasting in XAF
upcasting czyli po naszemu rzutowanie typu np lekarz na pracownik czy pacjent jako osoba

* https://docs.devexpress.com/eXpressAppFramework/112797/task-based-help/business-model-design/express-persistent-objects-xpo/how-to-use-xpo-upcasting-in-xaf



## Podstawy XAF

* <a href="https://docs.devexpress.com/eXpressAppFramework/113710/concepts/data-manipulation-and-business-logic/ways-to-implement-business-logic" target="_blank">Ways to Implement Business Logic</a>

* <a href="https://docs.devexpress.com/eXpressAppFramework/113711/concepts/data-manipulation-and-business-logic/create-read-update-and-delete-data" target="_blank">Create, Read, Update and Delete Data</a>

* <a href="https://docs.devexpress.com/eXpressAppFramework/113103/concepts/controllers-and-actions/define-the-scope-of-controllers-and-actions" target="_blank">Define the Scope of Controllers and Actions</a>

### Filtrowanie list / droplist
* https://docs.devexpress.com/eXpressAppFramework/113204/concepts/filtering/current-object-parameter
* https://docs.devexpress.com/eXpressAppFramework/112681/task-based-help/filtering/how-to-implement-cascading-filtering-for-lookup-list-views

* https://github.com/DevExpress-Examples/XAF_how-to-filter-lookup-list-views-e218

### Atrybuty 
https://docs.devexpress.com/eXpressAppFramework/112701/concepts/business-model-design/data-annotations-in-data-model


## Fragmenty kodu

procedury prawie gotowe do ctrl-c ctrl-v do uzycia we wlasnych projektach


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

 
