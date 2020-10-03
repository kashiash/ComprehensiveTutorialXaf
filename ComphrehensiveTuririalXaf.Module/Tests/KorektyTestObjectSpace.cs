using ComprehensiveTutorialXaf.Module.Factory;
using Demo1.Module.BusinessObjects;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensiveTutorialXaf.Module.Tests
{
    class KorektyTestObjectSpace
    {

        [Test]
        public void Test()
        {
            Assert.IsTrue(!string.IsNullOrEmpty(connectionString));
            Assert.IsNotNull(objectSpace);

        }

        [Test]
        [TestCase(1.666)]
        [TestCase(10.33333)]
        [TestCase(25)]
        [TestCase(0.3333333333)]
        public void TestPozycjeFaktur(decimal ilosc)
        {

            var produkt = objectSpace.FindObject<Produkt>(new BinaryOperator(nameof(Produkt.Nazwa), "Gacie"));

            Assert.IsNotNull(produkt);
            Assert.AreEqual("Gacie", produkt.Nazwa);

            var pozycja = objectSpace.CreateObject<PozycjaFaktury>();
            Assert.IsNotNull(pozycja);



            Assert.IsNotNull(pozycja, "brakuje produktu");
            pozycja.Produkt = produkt;

            Assert.IsNotNull(pozycja.Produkt, "brakuje produktu w pozycji faktury");
            Assert.AreEqual(produkt.Cena, pozycja.Cena);


            pozycja.Ilosc = 1;

            Assert.AreEqual(produkt.Cena, pozycja.WartoscNetto);

            pozycja.Ilosc = 5;

            Assert.AreEqual(produkt.Cena * 5, pozycja.WartoscNetto);
            Assert.AreEqual(pozycja.WartoscBrutto - pozycja.WartoscVAT, pozycja.WartoscNetto);


            pozycja.Ilosc = ilosc;

            Assert.AreEqual(produkt.Cena * ilosc, pozycja.WartoscNetto);
            Assert.AreEqual(pozycja.WartoscBrutto - pozycja.WartoscVAT, pozycja.WartoscNetto);
            Assert.AreEqual(pozycja.WartoscBrutto, pozycja.WartoscNetto + pozycja.WartoscVAT);
            Assert.AreEqual(pozycja.WartoscBrutto, pozycja.WartoscNetto * (1 + produkt.StawkaVAT.Stawka / 100));
        }

        [Test]
        [TestCase(1.333)]
        [TestCase(10.33333)]
        [TestCase(25)]
        [TestCase(0.3333333333)]
        public void TestFaktury(decimal ilosc)
        {

            var faktura = objectSpace.CreateObject<Faktura>();
            Assert.IsNotNull(faktura, "nie utworzono faktury");
            var pozycja = objectSpace.CreateObject<PozycjaFaktury>();
            Assert.IsNotNull(pozycja);
            var produkt = objectSpace.FindObject<Produkt>(new BinaryOperator(nameof(Produkt.Nazwa), "Gacie"));
            pozycja.Produkt = produkt;
            Assert.IsNotNull(pozycja.Produkt, "pozycja ma produkt");
            faktura.PozycjeFaktury.Add(pozycja);
            Assert.AreEqual(pozycja.WartoscBrutto, faktura.WartoscBrutto, "Suma faktury brutto zaraz po dodaniu pozycji");
            Assert.AreEqual(1, faktura.PozycjeFaktury.Count(), "Faktura ma pozycję");
            Assert.AreEqual(pozycja.WartoscBrutto, faktura.WartoscBrutto, "Suma faktury z jedna pozycja jest ok");
            var WartoscPrzedZmiana = faktura.WartoscBrutto;
            pozycja.Ilosc = ilosc;
            Assert.AreNotEqual(WartoscPrzedZmiana, faktura.WartoscBrutto);
            Assert.AreEqual(produkt.Cena * pozycja.Ilosc, faktura.WartoscNetto);

            faktura.Delete();
            objectSpace.CommitChanges();

        }

        [Test]
        public void TestFakturyWielopozycyjnej()

        {
            var factory = new InvoiceFactory(objectSpace);
            var faktura = factory.UtworzFakture(new string[] { "Gacie", "Skarpetki", "Skarpety" });
            Assert.IsNotNull(faktura, "nie utworzono faktury");
            Assert.AreEqual(3, faktura.PozycjeFaktury.Count());

            Assert.AreEqual("Gacie", faktura.PozycjeFaktury[0].Produkt.Nazwa);
            Assert.AreEqual("Skarpetki", faktura.PozycjeFaktury[1].Produkt.Nazwa);
            Assert.AreEqual("Skarpety", faktura.PozycjeFaktury[2].Produkt.Nazwa);

            Assert.AreEqual(100, faktura.PozycjeFaktury[0].WartoscNetto);
            Assert.AreEqual(33.33m, faktura.PozycjeFaktury[1].WartoscNetto);
            Assert.AreEqual(27.77, faktura.PozycjeFaktury[2].WartoscNetto);

            Assert.AreEqual(100 * 1.23, faktura.PozycjeFaktury[0].WartoscBrutto);
            Assert.AreEqual(33.33m * 1.07m, faktura.PozycjeFaktury[1].WartoscBrutto);
            Assert.AreEqual(27.77m * 1.07m, faktura.PozycjeFaktury[2].WartoscBrutto);

            Assert.AreEqual(100 * 1.23m + 33.33m * 1.07m + 27.77m * 1.07m, faktura.PozycjeFaktury.Sum(s => s.WartoscBrutto), "brutto pozycji i faktury nie jest zgodne");
            Assert.AreEqual(faktura.PozycjeFaktury.Sum(s => s.WartoscNetto), faktura.WartoscNetto, "netto pozycji i faktury nie jest zgodne");
            Assert.AreEqual(faktura.PozycjeFaktury.Sum(s => s.WartoscVAT), faktura.WartoscVAT, "vat pozycji i faktury nie jest zgodne");
            Assert.AreEqual(faktura.PozycjeFaktury.Sum(s => s.WartoscBrutto), faktura.WartoscBrutto, "brutto pozycji i faktury nie jest zgodne");

            faktura.Delete();
            objectSpace.CommitChanges();

        }

        [Test]
        public void TestKorektyCalkowitej()
        {
            var factory = new InvoiceFactory(objectSpace);



            Faktura faktura = factory.UtworzFakture(new string[] { "Gacie", "Skarpetki", "Skarpety" });
            Assert.IsNotNull(faktura, "nie utworzono faktury");
            Assert.AreEqual(3, faktura.PozycjeFaktury.Count());
            faktura.PrzeliczSumy();
            objectSpace.CommitChanges();

            var liczbaFaktur = objectSpace.GetObjectsQuery<Faktura>().Count();
            Assert.AreEqual(1, liczbaFaktur);

            var sumaFaktur = objectSpace.GetObjectsQuery<Faktura>().Sum(b => b.WartoscBrutto);
            Assert.AreEqual(188.377m, sumaFaktur);


            var korekta = factory.UtworzKorekteCalkowita(faktura);
            korekta.PrzeliczSumy();
            objectSpace.CommitChanges();

            Assert.IsNotNull(korekta, "nie utworzono faktury korygujacej");
            Assert.AreEqual(3, korekta.PozycjeFaktury.Count());
            Assert.AreSame(faktura.Klient, korekta.Klient, "Faktura wystawiona innego klienta niz korekta");
            Assert.IsNotNull(faktura.PozycjeFaktury[0].PozycjaKorygujaca);

            Assert.AreEqual(faktura.PozycjeFaktury[0].WartoscBrutto, faktura.PozycjeFaktury[0].PozycjaKorygujaca.WartoscBrutto * -1);
            Assert.AreEqual(faktura.PozycjeFaktury[0].WartoscBrutto * -1, korekta.PozycjeFaktury[0].WartoscBrutto);
            Assert.AreEqual(faktura.WartoscBrutto * -1, korekta.WartoscBrutto, "Brutto korekty nie zeruje sie z bruttem faktury");

            var SumaFakturPoKorekcie = objectSpace.GetObjectsQuery<Faktura>().Sum(b => b.WartoscBrutto);
            Assert.AreEqual(0, SumaFakturPoKorekcie, "Brutto korekty nie zeruje sie z bruttem faktury by objectQuery");

            faktura.Delete();
            korekta.Delete();
            objectSpace.CommitChanges();
        }

        [Test]
        public void TestKorektyCzesciowej()
        {
            IList<PozycjaFakturyDC> listaPozycjiFaktury = new List<PozycjaFakturyDC>();
            listaPozycjiFaktury.Add(new PozycjaFakturyDC()
            {
                Produkt = objectSpace.FindObject<Produkt>(new BinaryOperator(nameof(Produkt.Nazwa), "Gacie")),
                Ilosc = 5,
                CenaJednostkowaNetto = 30
            });
            listaPozycjiFaktury.Add(new PozycjaFakturyDC()
            {
                Produkt = objectSpace.FindObject<Produkt>(new BinaryOperator(nameof(Produkt.Nazwa), "Skarpetki")),
                Ilosc = 5,
                CenaJednostkowaNetto = 10
            }); ;

            listaPozycjiFaktury.Add(new PozycjaFakturyDC()
            {
                Produkt = objectSpace.FindObject<Produkt>(new BinaryOperator(nameof(Produkt.Nazwa), "Skarpety")),
                Ilosc = 5,
                CenaJednostkowaNetto = 20
            }); ;


            var factory = new InvoiceFactory(objectSpace);
            var faktura = factory.UtworzFakture(listaPozycjiFaktury);

            objectSpace.CommitChanges();
            Assert.IsNotNull(faktura, "nie utworzono faktury");
            Assert.AreEqual(3, faktura.PozycjeFaktury.Count());
            Assert.AreEqual(345, faktura.WartoscBrutto);

            IList<PozycjaKorygujacaFakturyDC> listaPozycjiDoSkorygowania = new List<PozycjaKorygujacaFakturyDC>();
            var korektaPusta = factory.UtworzKorekte(faktura, listaPozycjiDoSkorygowania);
            Assert.IsNull(korektaPusta, "Utworzono korektę chociaż nie powinno");
            if (korektaPusta != null)
            {
                korektaPusta.Delete();
            }

            objectSpace.CommitChanges();

            listaPozycjiDoSkorygowania.Add(new PozycjaKorygujacaFakturyDC()
            {
                Produkt = objectSpace.FindObject<Produkt>(new BinaryOperator(nameof(Produkt.Nazwa), "Gacie")),
                Ilosc = 0,
                CenaJednostkowaNetto = 30
            });
            listaPozycjiDoSkorygowania.Add(new PozycjaKorygujacaFakturyDC()
            {
                Produkt = objectSpace.FindObject<Produkt>(new BinaryOperator(nameof(Produkt.Nazwa), "Skarpetki")),
                Ilosc = 0,
                CenaJednostkowaNetto = 10
            });

            listaPozycjiDoSkorygowania.Add(new PozycjaKorygujacaFakturyDC()
            {
                Produkt = objectSpace.FindObject<Produkt>(new BinaryOperator(nameof(Produkt.Nazwa), "Skarpety")),
                Ilosc = 0,
                CenaJednostkowaNetto = 20
            });

            var korekta = factory.UtworzKorekte(faktura, listaPozycjiDoSkorygowania);
            Assert.IsNotNull(korekta, "nie utworzono faktury korygujacej");
            Assert.AreSame(faktura.Klient, korekta.Klient, "Faktura wystawiona innego klienta niz korekta");

            korekta.PrzeliczSumy();
            objectSpace.CommitChanges();

            Assert.AreEqual(3, korekta.PozycjeFaktury.Count(), "Korekta częsciowa ma zła liczbę pozycji");

            var SumaFakturPoKorekcie = objectSpace.GetObjectsQuery<Faktura>().Sum(b => b.WartoscBrutto);
            Assert.AreEqual(0, SumaFakturPoKorekcie, "Brutto korekty nie zeruje sie z bruttem faktury by objectQuery");

            faktura.Delete();
            korekta.Delete();
            objectSpace.CommitChanges();
        }

        #region setup



        IObjectSpace objectSpace;
        XPObjectSpaceProvider directProvider;
        string connectionString;


        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            XpoDefault.Session = null;

            connectionString = InMemoryDataStoreProvider.ConnectionString;
            directProvider = new XPObjectSpaceProvider(connectionString, null);
            objectSpace = directProvider.CreateObjectSpace();

            XafTypesInfo.Instance.RegisterEntity(typeof(Produkt));
            XafTypesInfo.Instance.RegisterEntity(typeof(PozycjaFaktury));
            XafTypesInfo.Instance.RegisterEntity(typeof(StawkaVAT));


            var gacie = objectSpace.CreateObject<Produkt>();
            gacie.Cena = 100;
            gacie.Nazwa = "Gacie";
            var stawka23 = objectSpace.CreateObject<StawkaVAT>();
            stawka23.Symbol = "23%";
            stawka23.Stawka = 23;
            gacie.StawkaVAT = stawka23;

            var skarpetki = objectSpace.CreateObject<Produkt>();
            skarpetki.Cena = 33.33m;
            skarpetki.Nazwa = "Skarpetki";

            var stawka7 = objectSpace.CreateObject<StawkaVAT>();
            stawka7.Symbol = "7%";
            stawka7.Stawka = 7;

            skarpetki.StawkaVAT = stawka7;

            var skarpety = objectSpace.CreateObject<Produkt>();
            skarpety.Cena = 27.77m;
            skarpety.Nazwa = "Skarpety";
            skarpety.StawkaVAT = stawka7;

            objectSpace.CommitChanges();

        }
        [SetUp]
        public void SetUp()
        {

            var a = 1;


        }
        #endregion
    }
}
