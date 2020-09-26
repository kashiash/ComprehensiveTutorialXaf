# Views

Wyświetlanie danych na ekranie


https://docs.devexpress.com/eXpressAppFramework/112611/concepts/ui-construction/views


XAF tworzy 3 widoki dla każdej klasy zdefiniowanej jako BussinesObject

* DetailView 

![](https://docs.devexpress.com/expressappframework/images/views_detailview_win132339.png)
* ListView

Służy do wyświetlania danych w formie listy z wyszukiwaniem sortowaniem itp


<a href="https://docs.devexpress.com/eXpressAppFramework/113683/concepts/ui-construction/views/list-view-data-access-modes" target="_blank">[Data access modes - sposoby pobierania danych]</a>


<a href="https://blog.delegate.at/2020/09/21/fixing-an-n1-performance-problem-in-xaf-xpo-with-totally-undocumented-apis.html" target="_blank">[Świetny artykuł o optymalizacji wyświetlania]</a>



![](https://docs.devexpress.com/expressappframework/images/views_listview_win132343.png)

* LookupListView
to uproszczona wersja ListView


w prosty sposób w kodzie możemy dowiedzieć się jak te widoki się nazywają używając 
DevExpress.ExpressApp.Model.NodeGenerators.ModelNodeIdHelper

```csharp
var detailViewId = ModelNodeIdHelper.GetDetailViewId(typeof(Contact)); 
var listViewId = ModelNodeIdHelper.GetListViewId(typeof(Contact)); 
var lookupListViewId = ModelNodeIdHelper.GetLookupListViewId(typeof(Contact));
```

Dodatkowo można pobrać nazwę widoku zagnieżdżonego np Listę zadań przypisaną konkretnej osobie

```csharp
var nestedListViewId = ModelNodeIdHelper.GetNestedListViewId(typeof(Contact), nameof(Contact.Tasks));
```




* DashboardView (nie mylić z Dashbordami)

![](https://docs.devexpress.com/expressappframework/images/views_dashboardview_win132346.png)

to jest szerszy temat ...
W tym przypadku jest to grupa widoków wymienionych powyżej dla których dedykowanym kontrolerem sterujemy zależnościami pomiędzy nimi. Przeznaczone do umieszczenia kilku niezaleznych widoków obok siebie

