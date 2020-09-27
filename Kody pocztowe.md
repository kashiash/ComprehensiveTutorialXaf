# Kody pocztowe

Struktura zgodna z danymi dostarczanymi przez Pocztę Polską


## Województwo
```csharp
[XafDisplayName("Województwo")]
   [NavigationItem("Administracyjne")]
   [XafDefaultProperty(nameof(NazwaWojewodztwa))]
   public class Wojewodztwo : XPObject
   {
       public Wojewodztwo(Session session) : base(session)
       { }

       string nazwa;

       [Size(SizeAttribute.DefaultStringMappingFieldSize)]
       public string NazwaWojewodztwa
       {
           get => nazwa;
           set => SetPropertyValue(nameof(NazwaWojewodztwa), ref nazwa, value);
       }


       [Association("Wojewodztwo-Powiaty"), DevExpress.Xpo.Aggregated]
       public XPCollection<Powiat> Powiaty
       {
           get
           {
               return GetCollection<Powiat>(nameof(Powiaty));
           }
       }

       [Association("Wojewodztwo-Gminy"), DevExpress.Xpo.Aggregated]
       public XPCollection<Gmina> Gminy
       {
           get
           {
               return GetCollection<Gmina>(nameof(Gminy));
           }
       }

       [Association("Wojewodztwo-KodyPocztowe"), DevExpress.Xpo.Aggregated]
       public XPCollection<KodPocztowy> KodyPocztowe
       {
           get
           {
               return GetCollection<KodPocztowy>(nameof(KodyPocztowe));
           }
       }
   }
```
## Powiat

```csharp
[NavigationItem("Administracyjne")]
  [XafDefaultProperty(nameof(NazwaPowiatu))]
  public class Powiat : XPObject
  {
      public Powiat(Session session) : base(session)
      { }


      Wojewodztwo wojewodztwo;
      string nazwaPowiatu;

      [Size(SizeAttribute.DefaultStringMappingFieldSize)]
      public string NazwaPowiatu
      {
          get => nazwaPowiatu;
          set => SetPropertyValue(nameof(NazwaPowiatu), ref nazwaPowiatu, value);
      }


      [Association("Wojewodztwo-Powiaty")]
      public Wojewodztwo Wojewodztwo
      {
          get => wojewodztwo;
          set => SetPropertyValue(nameof(Wojewodztwo), ref wojewodztwo, value);
      }

      [Association("Powiat-Gminy"), DevExpress.Xpo.Aggregated]
      public XPCollection<Gmina> Gminy
      {
          get
          {
              return GetCollection<Gmina>(nameof(Gminy));
          }
      }
      [Association("Powiat-KodyPocztowe"), DevExpress.Xpo.Aggregated]
      public XPCollection<KodPocztowy> KodyPocztowe
      {
          get
          {
              return GetCollection<KodPocztowy>(nameof(KodyPocztowe));
          }
      }
  }
```


## Gmina


```csharp
[NavigationItem("Administracyjne")]
   [XafDefaultProperty(nameof(NazwaGminy))]
   public class Gmina : XPObject
   {
       public Gmina(Session session) : base(session)
       { }


       Wojewodztwo wojewodztwo;
       Powiat powiat;
       string nazwaGminy;

       [Size(SizeAttribute.DefaultStringMappingFieldSize)]
       public string NazwaGminy
       {
           get => nazwaGminy;
           set => SetPropertyValue(nameof(NazwaGminy), ref nazwaGminy, value);
       }


       [Association("Wojewodztwo-Gminy")]
       public Wojewodztwo Wojewodztwo
       {
           get => wojewodztwo;
           set => SetPropertyValue(nameof(Wojewodztwo), ref wojewodztwo, value);
       }

       [Association("Powiat-Gminy")]
       public Powiat Powiat
       {
           get => powiat;
           set => SetPropertyValue(nameof(Powiat), ref powiat, value);
       }

       [Association("Gmina-KodyPocztowe"), DevExpress.Xpo.Aggregated]
       public XPCollection<KodPocztowy> KodyPocztowe
       {
           get
           {
               return GetCollection<KodPocztowy>(nameof(KodyPocztowe));
           }
       }
   }
```

## KodyPocztowe

```csharp
[NavigationItem("Administracyjne")]
   [XafDefaultProperty(nameof(Poczta))]
   public class KodPocztowy : XPObject
   {
       public KodPocztowy(Session session) : base(session)
       { }


       string poczta;
       string kodUpr;
       Wojewodztwo wojewodztwo;

       Powiat powiat;
       Gmina gmina;
       string numery;
       string ulica;
       string kod;
       string miejscowosc;



       [Size(10)]
       public string Kod
       {
           get => kod;
           set => SetPropertyValue(nameof(Kod), ref kod, value);
       }

       
       [Size(SizeAttribute.DefaultStringMappingFieldSize)]
       public string Poczta
       {
           get => poczta;
           set => SetPropertyValue(nameof(Poczta), ref poczta, value);
       }

       [Size(10)]
       public string KodUpr
       {
           get => kodUpr;
           set => SetPropertyValue(nameof(KodUpr), ref kodUpr, value);
       }
       [Size(SizeAttribute.DefaultStringMappingFieldSize)]
       public string Miejscowosc
       {
           get => miejscowosc;
           set => SetPropertyValue(nameof(Miejscowosc), ref miejscowosc, value);
       }

       [Size(SizeAttribute.DefaultStringMappingFieldSize)]
       public string Ulica
       {
           get => ulica;
           set => SetPropertyValue(nameof(Ulica), ref ulica, value);
       }

       [Size(SizeAttribute.DefaultStringMappingFieldSize)]
       public string Numery
       {
           get => numery;
           set => SetPropertyValue(nameof(Numery), ref numery, value);
       }


       [Association("Gmina-KodyPocztowe")]
       public Gmina Gmina
       {
           get => gmina;
           set => SetPropertyValue(nameof(Gmina), ref gmina, value);
       }

       [Association("Powiat-KodyPocztowe")]
       public Powiat Powiat
       {
           get => powiat;
           set => SetPropertyValue(nameof(Powiat), ref powiat, value);
       }



       [Association("Wojewodztwo-KodyPocztowe")]
       public Wojewodztwo Wojewodztwo
       {
           get => wojewodztwo;
           set => SetPropertyValue(nameof(Wojewodztwo), ref wojewodztwo, value);
       }
   }
```



Do tego przydałoby się to jakoś zaimportować


```csharp
public class KodyPocztoweImporter : CSVImporter
   {
       UnitOfWork unitOfWork;
       Session _session;
       CultureInfo culture = CultureInfo.InvariantCulture;


       public void Import(string FileName, bool deleteFile = false)
       {

           if (File.Exists(FileName))
           {
               watch = new System.Diagnostics.Stopwatch();

               watch.Start();

               ImportujPlik(FileName, ';');
               if (unitOfWork != null)
               {
                   unitOfWork.CommitChanges();
               }

               Console.WriteLine($"Specification Value Import Time: {watch.ElapsedMilliseconds} ms");


               if (deleteFile)
               {
                   File.Delete(FileName);
               }
           }
       }



       public KodyPocztoweImporter()
       {
           _session = new Session() { ConnectionString = @"XpoProvider=DB2;Server=10.2.0.10:50000;User ID = mediqus; Password=GabosGabos2012;Database=MEDXTest;Current Schema=mediqus;Persist Security Info=true" };


       }

       public override void ImportRow(CsvRow csv)
       {
           if (unitOfWork == null)
           {
               unitOfWork = new UnitOfWork(_session.DataLayer);
           }
           // throw new NotImplementedException();

           var Miejscowosc = csv[1].Truncate(100);
           var Kod = csv[0];

           KodPocztowy rec = unitOfWork.FindObject<KodPocztowy>(CriteriaOperator.Parse("Miejscowosc = ? And Kod =?", Miejscowosc, Kod));
           if (rec == null)
            rec = new KodPocztowy(unitOfWork);

           //PNA     ; MIEJSCOWOŚĆ; ULICA; NUMERY; GMINA; POWIAT; WOJEWÓDZTWO
           //83 - 440; Abisynia; ; ; Karsin; kościerski; pomorskie


           var woj = unitOfWork.FindObject<Wojewodztwo>(new BinaryOperator("NazwaWojewodztwa", csv[6]));
           if (woj == null)
           {
               woj = new Wojewodztwo(unitOfWork);
               woj.NazwaWojewodztwa = csv[6].Truncate(100);
               woj.Save();
             unitOfWork.CommitChanges();
           }

           var pow = unitOfWork.FindObject<Powiat>(new BinaryOperator("NazwaPowiatu", csv[5]));
           if (pow == null)
           {
               pow = new Powiat(unitOfWork);
               pow.NazwaPowiatu = csv[5].Truncate(100);
               pow.Wojewodztwo = woj;
              pow.Save();
               unitOfWork.CommitChanges();
           }

           var gmi = unitOfWork.FindObject<Gmina>(new BinaryOperator("NazwaGminy", csv[4]));
           if (gmi == null)
           {
               gmi = new Gmina(unitOfWork);
               gmi.NazwaGminy = csv[4].Truncate(100);
               gmi.Wojewodztwo = woj;
               gmi.Powiat = pow;
              gmi.Save();
               unitOfWork.CommitChanges();

           }
           rec.Wojewodztwo = woj;
           rec.Powiat = pow;
           rec.Gmina = gmi;
           rec.Numery = csv[3].Truncate(100);
           rec.Ulica = csv[2].Truncate(100);
           rec.Miejscowosc = csv[1].Truncate(100);
           rec.Kod = csv[0];
           rec.KodUpr = rec.Kod.Replace("-", "");
   
           rec.Save();

           //  Console.WriteLine($"   {rec.value1}");
           if (rowCnt % 10000 == 0)
           {
               Console.WriteLine($"recs: {rowCnt} Execution Time: {watch.ElapsedMilliseconds} ms");
               unitOfWork.CommitChanges();
               unitOfWork.Dispose();
               unitOfWork = null;



               Console.WriteLine($"After commit Execution Time: {watch.ElapsedMilliseconds} ms");
               //Console.ReadLine();
               watch.Restart();

           }
       }

   }
```

do pomocy potzrebna jest klasa importu z plików csv:

```csharp
public abstract class CSVImporter
{

    public int rowCnt = 0;
    public Stopwatch watch;



    public void ImportujPlik(string fileName)
    {
        Console.WriteLine($"Import CSV file {fileName}");
        watch = new System.Diagnostics.Stopwatch();

        watch.Start();
        using (CsvFileReader reader = new CsvFileReader(fileName, ','))
        {


            CsvRow row = new CsvRow();
            string lastValue = string.Empty;
            while (reader.ReadRow(row))
            {
                int liczbaKolumn = row.Count;


                if (rowCnt > 0)
                {
                    ImportRow(row);

                }
                rowCnt++;
            }
        }
    }


    public void ImportujPlik(string fileName, char separator)
    {
        Console.WriteLine($"Import CSV file {fileName}");
        watch = new System.Diagnostics.Stopwatch();

        watch.Start();
        using (CsvFileReader reader = new CsvFileReader(fileName, separator))
        {


            CsvRow row = new CsvRow();
            string lastValue = string.Empty;
            while (reader.ReadRow(row))
            {
                int liczbaKolumn = row.Count;
                //    var a = row[1];
                //   Console.WriteLine(rowCnt);

                if (rowCnt > 0)
                {
                    ImportRow(row);

                }
                rowCnt++;
            }
        }
    }

    public abstract void ImportRow(CsvRow row);

    public decimal StringToDecimal(string text)
    {
        try
        {
            return decimal.Parse(text, new CultureInfo("pl-PL"));
        }
        catch (Exception)
        {

            return 0;
        }
    }

    public static DateTime StringToDate(string text)
    {
        string datatext = text.Replace("\"", string.Empty);
        if (datatext != "NULL" && datatext != string.Empty)
        {
            try
            {
                return DateTime.Parse(datatext);

            }
            catch (FormatException)
            {
                Console.WriteLine("{0} is not in the correct format.", datatext);
                return DateTime.MinValue;
            }

        }
        return DateTime.MinValue;
    }

    public static Int64 StringToInt64(string text)
    {
        try
        {
            return Int64.Parse(text);
        }
        catch
        {
            return 0;
        }
    }


    public static int StringToInt(string text)
    {
        try
        {
            return int.Parse(text);
        }
        catch
        {
            return 0;
        }
    }


}
```
A do jej obsługi klasa która kuma co to CSV:
(znalezione gdzieś w internetach, nie pytać co skąd i dlaczego ;))
```csharp
public abstract class CSVImporter
 {

     public int rowCnt = 0;
     public Stopwatch watch;



     public void ImportujPlik(string fileName)
     {
         Console.WriteLine($"Import CSV file {fileName}");
         watch = new System.Diagnostics.Stopwatch();

         watch.Start();
         using (CsvFileReader reader = new CsvFileReader(fileName, ','))
         {


             CsvRow row = new CsvRow();
             string lastValue = string.Empty;
             while (reader.ReadRow(row))
             {
                 int liczbaKolumn = row.Count;


                 if (rowCnt > 0)
                 {
                     ImportRow(row);

                 }
                 rowCnt++;
             }
         }
     }


     public void ImportujPlik(string fileName, char separator)
     {
         Console.WriteLine($"Import CSV file {fileName}");
         watch = new System.Diagnostics.Stopwatch();

         watch.Start();
         using (CsvFileReader reader = new CsvFileReader(fileName, separator))
         {


             CsvRow row = new CsvRow();
             string lastValue = string.Empty;
             while (reader.ReadRow(row))
             {
                 int liczbaKolumn = row.Count;
                 //    var a = row[1];
                 //   Console.WriteLine(rowCnt);

                 if (rowCnt > 0)
                 {
                     ImportRow(row);

                 }
                 rowCnt++;
             }
         }
     }

     public abstract void ImportRow(CsvRow row);

     public decimal StringToDecimal(string text)
     {
         try
         {
             return decimal.Parse(text, new CultureInfo("pl-PL"));
         }
         catch (Exception)
         {

             return 0;
         }
     }

     public static DateTime StringToDate(string text)
     {
         string datatext = text.Replace("\"", string.Empty);
         if (datatext != "NULL" && datatext != string.Empty)
         {
             try
             {
                 return DateTime.Parse(datatext);

             }
             catch (FormatException)
             {
                 Console.WriteLine("{0} is not in the correct format.", datatext);
                 return DateTime.MinValue;
             }

         }
         return DateTime.MinValue;
     }

     public static Int64 StringToInt64(string text)
     {
         try
         {
             return Int64.Parse(text);
         }
         catch
         {
             return 0;
         }
     }


     public static int StringToInt(string text)
     {
         try
         {
             return int.Parse(text);
         }
         catch
         {
             return 0;
         }
     }


 }
```

Sam importer to prosty program, który oczekuje ze w katalogu lokalnym będzie podkatalog z plikiem z danymi:


```csharp
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Rozpoczeto import kodów pocztowych!");
        var watch = new System.Diagnostics.Stopwatch();

        watch.Start();

        var MakeImporter = new KodyPocztoweImporter();
        MakeImporter.Import(".\\UTF8KodyPocztowe\\spispna-cz1.txt");

                        watch.Stop();

        Console.WriteLine($"Complete Execution Time: {watch.ElapsedMilliseconds} ms");

        Console.ReadLine();
    }
}
```
