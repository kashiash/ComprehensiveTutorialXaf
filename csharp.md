#c# szybki start

Po prawie 30 latach odświdczenia w prograowaniu w Clarionie, zaczałem uczyć sie c# i bardzo irytowało mnie, ze nie potrafie zrobić prostych operacji w nowym języku. Dotyczy ło do dostepu do danych, konwersji typów , inne formy if else czy switch.

Niniejszym archiwizuję notatki z tamtych czasów, gdy o każda rzecz musiałem pytac googla czy stackoverflow.



### Czytanie pliku tekstowego

```csharp

     List<PcsConverter> listaPrzelicznikow = new List<PcsConverter>();

    Console.WriteLine("Import przeliczników");
    string path = @"c:\Synchronizator\PlikiDodatkowe\przeliczniki.csv";

    if (File.Exists(path))
    {
        StreamReader sr = new StreamReader(path);
        while (!(sr.EndOfStream))
        {
            string aLine = sr.ReadLine();
            if (aLine != "START") continue;

            while (!(sr.EndOfStream))
            {
                aLine = sr.ReadLine();
                aLine = aLine.Trim();
                string[] words = aLine.Split(';');
                if (words.Length == 4 && !string.IsNullOrEmpty(words[0]))
                {
                    var rec = new PcsConverter
                    {
                        Akronim = words[0],
                        JednostkaPierwotna = words[1],
                        JednostkaGlowna = words[2],
                        Przelicznik = decimal.Parse(words[3], CultureInfo.InvariantCulture)
                    };
                    listaPrzelicznikow.Add(rec);
                }
            }
        }
        sr.Close();
    }



    internal class PcsConverter
    {
        public PcsConverter()
        {
        }

        public string Akronim { get; internal set; }
        public string JednostkaPierwotna { get; internal set; }
        public string JednostkaGlowna { get; internal set; }
        public decimal Przelicznik { get; internal set; }
    }
```

