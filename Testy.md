# Testy

## klasa bazowa do testów na memory object space 

```csharp
public class BaseTestMem
{
    #region setup
    private IObjectSpace objectSpace;
    private XPObjectSpaceProvider directProvider;
    private string connectionString;
     //  private DataGenerator generator;

     [OneTimeSetUp]
     public void OneTimeSetUp() { XpoDefault.Session = null; }

     [SetUp]
     public void SetUp()
     {
         connectionString = InMemoryDataStoreProvider.ConnectionString;
         directProvider = new XPObjectSpaceProvider(connectionString, null);
         objectSpace = directProvider.CreateObjectSpace();
     }

     [TearDown]
     public void TearDown()
     {
         directProvider.Dispose();
         objectSpace.Dispose();
         //  generator.Dispose();
     }
    #endregion
}
```


## klasa bazowa do testów na bazie danych:
```csharp
public   class BaseTestOnDB
{
    #region setup
    internal IObjectSpace objectSpace;
    internal XPObjectSpaceProvider directProvider;
    internal string connectionString;
    //  private DataGenerator generator;

    [OneTimeSetUp]
    public void OneTimeSetUp() { XpoDefault.Session = null; }

    [SetUp]
    public void SetUp()
    {
        directProvider = new XPObjectSpaceProvider(ApplicationSettings.ConnectionString, null);

        using(IObjectSpace directObjectSpace = directProvider.CreateObjectSpace())
            objectSpace = directProvider.CreateObjectSpace();
        // generator = new DataGenerator(objectSpace);
        XafTypesInfo.Instance.RegisterEntity(typeof(Cennik));
        XafTypesInfo.Instance.RegisterEntity(typeof(Kontrahenci));
        //objectSpace.CommitChanges();
    }

    [TearDown]
    public void TearDown()
    {
        directProvider.Dispose();
        objectSpace.Dispose();
        //  generator.Dispose();
    }
    #endregion
}
```

## następnie można tego używać w najstępujacy sposób:

```csharp
[TestFixture]
public class CennikiTest : BaseTestOnDB
{
    [Test]
    public void WypelnionoCennikiTest()
    {
        var produkty = objectSpace.GetObjectsQuery<Produkt>();
        Assert.IsNotNull(produkty);
        Assert.IsTrue(produkty.Count() > 0);

    }

}
```
