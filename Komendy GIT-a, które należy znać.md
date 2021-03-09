# Komendy GIT-a, które należy znać



<a href="https://blogprogramisty.net/komendy-git-a-ktore-nalezy-znac/?utm_source=rss&utm_medium=rss&utm_campaign=komendy-git-a-ktore-nalezy-znac" target="_blank">zródło</a>




Każdy szanujący swój czas deweloper powinien znać git-a oraz umieć go obsługiwać. Poniżej lista komend, które każdy użytkownik git-a powinien znać i używać. Komend wyszło 97. Niektóre są bardzo proste, nie których rzadko się używa ale znać je trzeba. Listę też można potraktować jak szybkie przypomnienie co i jak. Zapraszam na 97 komend git-a, które musisz znać aby go dobrze używać.

PS

Są jeszcze te komendy, których nie mogę zapamiętać a też się często przydają. https://blogprogramisty.net/komendy-git-a-ktorych-nie-moge-zapamietac/

```bash
git config user.name Przemek
```

Od czegoś trzeba zacząć. Skonfigurowanie nazwy użytkownika wydaje się dobre na początek. Można skonfigurować nazwę globalną dla wszystkich repozytoriów na użytkowniku lub na komputerze. Przełącznik –global

```bash
git config user.email Przemek@test.pl
```

Konfiguracja maila. Podobnie jak wyżej. Dlaczego od tego zaczynam, bo zawsze po jakiś czasie się okazywało, że mam albo nie prawidłowy (nie firmowy) mail i gdzieś tam w systemach się potem wyświetlało, że jest jakiś inny Przemek. Często jak commituje z domu to mam domowego maila podłączonego

```bash
git config core.editor
```

Komenda, która wyświetla lub ustawia podstawowy edytor do commit-ów innych operacji.

```bash
git add .
```

Dodaje wszystkie zmiany do indeksu (stage). Zmiany w plikach jak i nowe pliki.

```bash
git add -p
```

Dodaje zmiany do indeksu z możliwością wybierania konkretnych zmian w plikach. Dzięki temu możemy z jednego pliku, gdzie było kilka zmian dodać tylko pojedyncze zmiany a nie cały plik. Od momentu jak to poznałem, używam regularnie. Jest to bardzo dobra komenda choć nie jest doskonała i czasem nie da się rozdzielić zmian tak jak byśmy chcieli.

```bash
git commit -m “komentarz“
```

Komenda, która wszystkie zmiany z indeksu (stage) zbiera razem i robi z nich pojedynczy commit. Parametr -m służy do pisania komentarza do commit-a. Pierwsze 170 znaków wyświetla się z pojedynczej linii.

```bash
git init
```

Komenda do uruchomienia w katalogu z projektem aby zainicjować repozytorium git.

```bash
git help
```

Jak sama nazwa pomoc dla git-a. Jak się zapomni co i jak to można sobie pamięć odświeżyć. Jak by nie było internetu.

```bash
git status
```

Bardzo ważna komenda. Należy ją używać często i gęsto. Szczególnie przed każdą poważniejszą zmianą. Pokazuje aktualny status repozytorium, ile commit-ów jesteśmy do tyłu w porównaniu ze zdalnym repozytorium, jakie pliki są śledzone a jakie nie itd.

```bash
git diff
```

Pokazuje zmiany na poszczególnych plikach. Bez parametrów pokazuje po kolei wszystkie pliki. Z parametrem nazwa_pliku pokaże zmiany na danym pliku.

```bash
git difftool
```

Komenda, która odpala domyślne narzędzie do przeglądania różnic w plikach. Każdy może wybrać swoje narzędzie.

```bash
gitk
```

Przydatne narzędzie do gita posiadające GUI. Dzięki temu czasem możemy sobie ładnie zwizualizować co i jak wygląda. Głownie przydaje się, gdy na szybko chcemy coś sprawdzić a na komputerze nie ma zainstalowanego innego narzędzia np: GitExtension. Możemy być pewni, że gitk zawsze będzie dostępne. Warto o tym pamiętać

```bash
git tag nazwa_taga commit_hash
```

Tagi są bardzo przydatne jak chcemy wersjonować swoje zmiany. Dzięki nimi możemy łatwo odnaleźć commit, które wskazuje na dany punkt w historii. Ta komenda wstawia tag na danych commit. Rekomenduje się, aby takie tagi tworzyć na własnych gałęziach a nie publicznych.

```bash
git tag nazwa_taga commit_hash -a -m – “komentarz“
```

Tzw: ciężki tag, czyli tag, który zawiera nazwę, autora, komentarz. Takie duże tagi pozwalają lepiej określić na co ten tag wskazuje. Używam go w narzędziach CI aby wbijać wersję na kolejne wdrożenie.

```bash
git tag -d nazwa_taga
```

Tak można usunąć tag z repozytorium.

```bash
git show master~1
```

Pokazuje commit i zmiany. Jeśli chodzi o ~ to tylda oznacza ile comitów wstecz mamy pokazać . ~1 to jeden commit wstecz, ~2 to dwa commity wstecz. ~ i ^ można używać w innych komendach, gdy te odnoszą się do commitów.

```bash
git show master^^
```

Komenda robi to samo co wyżej, z tą różnicą, że pokazuje zmiany w poprzednim commicie i w jeszcze poprzednim. Daszek oznacza 1 commit wstecz. Czyli ~2 to to samo to ^^.

```bash
git checkout nazwa_gałęzi
```

Komenda ta daje możliwość przełączenia się na inna gałąź. Ciekawostka o przełączaniu tutaj https://blogprogramisty.net/czego-nauczylem-w-14-tygodniu-pracy/ -> “GIT – dlaczego czasem można przenosić zmiany pomiędzy gałęziami a czasem nie”

```bash
git checkout nazwa_taga
```

Komenda ta daje możliwość przełączenia się do konkretnego tagu. Czasem jest to przydatne jak chcemy się przełączyć na daną wersję kodu, która jest otagowana. Tak łatwiej zapamiętać niż hash.
```bash
git checkout commit_hash
```

Komenda ta daje możliwość przełączenia się kodu do dowolnego commit-a. Uwaga gdy to zrobimy będziemy w trybie “Detach HEAD”.

```bash
git checkout –
```

Komenda dla geek-ów. – (kreska) oznacza, że przełączymy się do ostatnio używanej gałęzi. Przydatne jak robimy scalenia, wtedy często musimy zmieniać gałąź z jednej na drugą.

```bash
git checkout ścieżka_do_pliku
```

Komenda ta daje możliwość cofnięcia zmian do ostatniego commita dla pojedynczego pliku.

```bash
git checkout -b nazwa_gałęzi
```

Komenda ta umożliwia stworzenie nowej gałęzi o nazwie nazwa_gałęzi. Po jej utworzeniu git automatycznie się na nią przełączy.

```bash
git checkout — ścieżka_do_pliku
```

Ta komenda pozwala na przywrócenie stanu pliku do ostatniego commit-a. Coś robimy, nie podoba nam się i chcemy szybko cofnąć zmiany.

```bash
git checkout — wzorzec_do_wyszukania
```

To samo co wyżej ale pozwala przesłać jakiś konkretny wzorzec do wyszukiwania plików typu wildcards (czyli *.txt itd.)

```bash
git branch -d nazwa_gałęzi
```

Komenda ta daje możliwość skasowania lokalnej gałęzi tylko gdy zmiany na lokalnej gałęzi są scalone z master-em.

```bash
git branch -D nazwa_gałęzi
```

Komenda robi to co wyżej z tą różnicą, że nie sprawdza czy zmiany są scalone z gałęzią master.

```bash
git branch -a
```

Wyświetla listę wszystkich gałęzi, w tym tych ze zdalnego repozytorium.

```bash
git cherry-pick commit_hash
```

Bardzo przydatna komenda. Pozwala na zastosowanie zmian w całości z jednego commit-a (commit_hash) do aktualnej gałęzi. Jest to takie wybieranie pojedynczych commitów z dowolnych(czyli innych gałęzi) innych miejsc w repozytorium.

```bash
git merge nazwa_gałęzi
```

Komenda podstawowa, która pozwala scalić zmiany z innej gałęzi na tą na, której jesteśmy.

```bash
git merge nazwa_gałęzi –squash
```

Robi to co wyżej ale z przełącznikiem –squash powala wszystkie zmiany z innej gałęzi połączyć w jeden commit. Bardzo przydatne jeśli chce się zachować ładną i czytelną historię. Często używam i polecam.

```bash
git merge nazwa_gałęzi –no-ff
```

Przełącznik –no-ff zapewnia nas, że nawet gdy nie ma konfliktów pomiędzy scalanymi gałęziami, historia nie zostanie spłaszczona do jednej linii i powstanie nowy commit. Przydatne, gdy np. wprowadzamy nowy feature i chcemy jednak zachować historię commitów utworzonych w trakcie pracy.

```bash
git diff –word-diff
```

Pokazuje wprowadzone zmiany z dokładnością do słów w linii kodu. Czasem czytelniejsze od zwykłego git diff.

```bash
git log –graph –oneline
```

Wyświetla historię commitów w formie skróconego grafu i ładniej oprawie razem z tagami.

```bash
git clone adres_do_repozytorium
```

Podstawowa komenda, która pozwala pobrać repozytorium na dysk lokalny. Uwaga! komenda ta nie pobiera gałęzi zdalnych

```bash
git branch -vv
```

Komenda ta pokazuje gałęzie, która mamy w repozytorium. -v oznacza verbose czyli pokazuje nie tylko nazwy gałęzi ale również ostatni hash i opis commita. dodatkowe v czyli very verbose oznacza, że do tych wszystkich informacji będzie jeszcze dodana nazwa gałęzi zdalnej.

```bash
git remote show lub git remote
```

Komanda ta pokazuje nazwy zdalnych repozytoriów. Domyślnie powinien być origin ale mogą też być inne.

```bash
git init –bare
```

Bardzo przydatna komenda, pozwalająca stworzyć zdalne repozytorium, do którego możemy robić komendy push i pull. Takie zdalne repozytoria trzymamy na dysku, spełniają świetną role backupu. Wystarczy zrobić poniższą komendę wtedy url naszego zdalnego repozytorium będzie po prostu ścieżką do tego folderu. Następnie dodajemy sobie do repozytorium nowy remote (2 komenda poniżej) i już możemy do niej wysyłać wszystko (komanda poniżej)

```bash
git push nazwa_repozytorium –mirror
```

Pozwala wysłać do repozytorium(nazwa_repozytorium) wszystko to co mamy u siebie lokalnie. Wszystkie gałęzie, wszystkie zmiany, wszystkie tagi. Zrobić takie lustro tego co mamy u siebie. To się przydaje do lokalnych backupów opisanych wyżej.

```bash
git remote add nazwa_repozytorium url_do_repozytorium
```

Komenda pozwalająca dodać zdalne repozytorium do którego będziemy mogli robić komendy push i pull.

Przykład: git remote add orgin2 https://github.com/przemekwa/LiczbyNaSlowaNetCore.git

```bash
git push
```

Co tu dużo pisać. Git push and go home : ) Komenda wysyła commity do zdalnego repozytorium.

```bash
git push origin -u nazwa_gałęzi_lokalnej:nazwa_gałęzi_zdalnej
```

Poza wysłaniem zmian, tworzy gałąź(nazwa_gałęzi_zdalnej) na zdalnym repozytorium. Dopisując dwukropek za nazwą nazwa_gałęzi_lokalnej, możemy nadać inną nazwę gałęzi w zdalnym repo. Gdy nadamy zdalnej gałęzi inną nazwę, musimy pamiętać przy następnych pushach, aby podawać jej pełną nazwę(nazwa_gałęzi_zdalnej). Może to być niewygodne, dlatego możemy użyć komendy…

```bash
git config push.default upstream
```

…która powie gitowi, że ma wysyłać zmiany na gałąź, nawet gdy ta ma inną nazwę, pod warunkiem że jest ona skonfigurowana jako “upstream branch”.

```bash
git push –delete origin nazwa_gałęzi
```

Usuwa gałąź ze zdalnego repozytorium origin.

```bash
git remote rename
```

Możemy zmienić nazwę domyślnego adresu zdalnego. Z origin na origin2 : )

```bash
git remote update –prune
```

Usuwa gałęzie skasowane w zdalnym repozytorium. Jedna komenda a od razu robi się większy porządek.

```bash
git fetch
```

Aktualizuje historię commitów na obecnej gałęzi. Po tej komendzie nasz kod nie jest aktualizowany. To znaczy, że nic się nie zmieni w naszym lokalnym repozytorium ale będziemy mieć aktualną wiedzę na temat tego co jest w zdalnym repozytorium.

```bash
git pull
```

Najprościej mówiąc, wykonuje git fetch a potem git merge. Aktualizuje historię oraz nasz kod do ostatniego wysłanego do zdalnego repozytorium commita. Zanim zaczniemy prace w repozytorium to komanda ta sprawi, że będziemy mieć aktualne lokalne repozytorium.

```bash
git pull –rebase
```

Alternatywa normalnego pulla. Zamiast wykonywać fetch + merge, wykona się fetch + rebase. Zachowamy dzięki temu ładną liniową historię commitów. Czyli nasze commity na chwilę znikną, następnie pobierzemy zdalne repozytorium i następnie zastosujemy nasze commity. Wtedy wszystkie nasze zmiany znajdą się na początku.

```bash
git push –tags
```

Wysyła wszystkie tagi na zdalne repozytorium. Trzeba uważać, żeby nie zaśmiecić repozytorium swoimi tagami.

```bash
git push –follow-tags
```

Wysyła tylko “ciężkie” tagi. Zalecane, gdy chcemy wysyłać tagi, którymi chcemy się dzielić, do zdalnego repozytorium.

```bash
git config push.followTags true
```

Automatyzuje wysyłanie “ciężkich” tagów. Konfigurujemy git tak aby wszystkie ciężkie tagi wysyłał podczas push-a.

```bash
git fetch –prune –prune-tags
```

Usuwa lokalne tagi, które zostały usunięte zdalnie.

git fetch –no-tags
Aktualizuje historię zmian pomijając tagi.

git add -i
Włącza tryb interactive komendy git add. Polecam sprawdzić możliwości tego trybu w tym trybie możemy też użyć opcji patch, która pozwala rozdzielić jeden plik na kilka zmian.

git clean -n
Komanda do kasowania nowych plików, która pokazuje co by zostało skasowane. Jest to bezpieczna komanda do zorientowania się w sytuacji. Nie kasuje plików.

git clean -f
Komanda ta po prostu kasuje nowe pliki, które powstały. Które pliki się skasuje, można zobaczyć przy pomocy komendy wyżej czyli z przełącznikiem -n. Najczęściej jej używam gdy kasuje pliki z końcówką .orig, które powstają po komandach merge i rebase.

git clean -d
Komenda ta kasuje też katalogi

git clean -i
Komenda ta daje możliwość usuwania plików poprzez wejście do rozbudowanego menu, podobnie jak komenda git add -i czy git rebase -i

git clean -fdx
Usuwa wszystkie pliki, które nie są dodane do gita. Czy krótko mówiąc, wyczyści wszystko tak jak by repozytorium było ściągnięte komendą git clone … . Przydaje się to gdy chcemy się upewnić, że nasz kod się kompiluje i plik .gitigonre nie zabiera za dużo plików. Usuwa też katalogi.

git reset –hard
Komenda ta usuwa wszystko co robiliśmy i co dodawaliśmy (pliki, zmiany, pliki do indeksu itd.) a co nie było w commicie i przywraca stan do ostatniego commita. Taki jak by restart pracy. Komanda jest niebezpieczna, bo usuwa trwale to czego nie było w commicie więc można stracić pracę swoją. Używam w sumie dość często, bo łatwo dzięki temu wrócić na początek wszystkiego : )

git reset –soft commit_hash
Komenda ta sprawa, że wszystkie zmiany trafią do indeksu (stage). Wszystkie czyli każdy commit od tego aktualnego do tego commit_hash. Jest to bezpiecznie o tyle, że nie tracimy zmian na zawsze.

git stash
Zapisuje zmiany w postaci ukrytego commita. Dość przydatne. To taki schowek aktualnej pracy ale tylko plików, które są śledzone. Aby dodać wszystkie pliki wtedy…

git stash -u
Zapisuje zmiany do schowa jak wyżej ale zapisuje wszystkie jakie te śledzone zmiany jaki i te nie śledzone. Wszystko wszystko zapisze.

git reset @^
Przywraca nasz kod do stanu z poprzedniego commita.

git config
Wyświetla możliwości komendy git config.

git config –list
Komanda ta wyświetla wszystkie ustawienia w gicie.

git clone –depth <liczba>
Klonuje zdalne repozytorium, przełącznik –depth pozwala nam na wybranie ilości ostatnich commitów, które chcemy pobrać.

git reset –hard ORIG_HEAD
Przywraca zmiany do stanu przed jakimiś poważnymi zmianami (np. merge lub rebase), gdy już je commitowaliśmy.

git merge –abort / git rebase –abort / git cheerry-pick –abort
Przywraca zmiany do stanu przed użyciem Git merge (gdy mamy jeszcze nierozwiązane konflikty).

git mergetool
Włącza domyślne narzędzie do mergowania. Jeśli go nie ustawiliśmy, git poleci nam narzędzie vimdiff. Korzystać z vimdiff do rozwiązywania konfliktów to tak jakby jeść zupę widelcem. By się z niego ewakuować, trzeba wpisać “:qa”. Warto skonfigurować lepszy mergetool za pomocą poniższych dwóch komend…

git mergetool –tool-help
Wyświetla listę narzędzi, które Git potrafi stosować do rozwiązywania konfliktów. Pokazuje takie, które mamy i takie, które możemy zastosować

git config merge.tool tool
Ustawia domyślne narzędzie do rozwiązywania konfliktów.

git config rerere.enabled true
Kolejna bardzo przydatna konfiguracja gita. Włącza tryb RERERE – REuse REcorded REsolutions of conflicted merges. To oznacza, że Git zapamięta sposób rozwiązania konfliktu i jeśli wystąpi on ponownie, wykona go za nas. Skraca to proces mergowania do minimum. Polecam!

git revert commit_hash
Odwraca zmiany z podanego commita. Jego działania jest takie, że bierze dany commit, cofa zmiany i ponownie commituje. Więc po tej komendzie w repozytorium będzie kolejny commit. Używam tego, gdy w kodzie są jakieś specyficzne ustawienia tylko dla mojej maszyny(connections_string) a, których nie chciał bym commitować. Gdy mam już wszystko gotowe to zanim zrobię PR to robię tego reverta na commicie z ustawieniami i dzięki temu nie wysyłam moich ustawień do repozytorium.

git commit –amend
Pozwala zmienić nazwę ostatniego commita. Jest to chyba najprostszy sposób aby to zrobić. Dzięki temu też nie musimy tworzyć nowego commita i po prostu edytujemy jego teskt.

git rebase -i commit_hash
Uruchamia rebase w trybie interactive, dzięki któremu możemy dowolnie edytować commity. Tu polecam wgłębić się w możliwe opcje tego trybu, gdyż jest ich całkiem sporo. Używam tego do scalania commitów w jeden. Wymaga trochę wprawy ale daje spore możliwości.

git push –force
Niebezpieczne, polecam zawsze używać tego niżej. Komenda ta pozwala wysłać zmiany lokalne do zdalnego repozytorium z pominięciem sprawdzenia czy jesteśmy zaktualiziwani z tą zdalną aby nie nadpisać czegoś. To tak jak by powiedzieć, że chcę od teraz aby to co lokalnie u mnie było zdalnie. Używam tego na swoich gałęziach, gdy wiem, że tylko ja ją używam głownie wtedy gdy robię reabase z masterem. Wtedy historia mi się zmiania i git push nie pozwoli mi wysłać żadnych zmian.

git push –force-with-lease
To jest trochę mniej inwazyjne, bo sprawdza czy ktoś w między czasie nie dodał czegoś do gałęzi i nasz push miał by to usunąć na zawsze. Gdyby to wystąpiło to zobaczmy komunikat i push się nie uda.

git blame ścieżka_do_pliku
Pokazuje autora każdej linii kodu w podanym pliku.

git gui blame
Wyświetla to ładniej niż komenda wyżej :)

git log -S “tekst”
Bardzo dobra komenda, wyszukuje w treści commitów (czyli w różnicach na danych plikach), konkretnej frazy w tym przypadku słowa tekst. Da się dzięki temu odnaleźć dość konkretnie commit, który zawiera taką frazę.

git log -S “tekst” -p
Komenda robi to co powyżej ale przełącznik -p pokazuje nie tylko sam commit ale też dokładnie zmiany w plikach gdzie poszukiwana fraza została znaleziona. Power +1 : )

git log –format=%h
Komanda robi oczywiście log z commitów ale używa też przełącznika format, który daje możliwość formatowanie tego jak taki pojedynczy commit się wyświetli. %h to skrócony hash commita.

Generalnie używam tego:

```sql
git log –graph –pretty=format:”%Cred%h%Creset -%C(yellow)%d%Creset %s %Cgreen(%cr) %C(bold blue)<%an>%Creset” –abbrev-commit
```

ale polecam poszukać czegoś co będzie wam odpowiadać.

```sql
git bisect start
```

Włącza tryb Bisecting, czyli rozpoczyna wyszukiwanie commita, który wywołał według nas jakiś błąd.

```sql
git bisect good
```

Oznacza commit jako dobry, czyli nie posiadający szukanego błędu.

```sql
git bisect bad
```

Oznacza commit jako zły. Po użyciu tych trzech komend, git automatycznie rozpocznie proces szukania błędnego commita. Zadanie jakie teraz do nas należy, to określanie gitowi, czy commit, który on “scheckoutował”, jest dobry czy zły. Po kilku krokach git wyświetli nam pierwszy zły commit, zawierający szukany błąd.

```sql
git archive –format zip –output nazwa_pliku.zip master
```

Bardzo przydatna komenda, bo pozwala cały kod wrzucić do ZIP-a. Często klienci chcą mieć kod źródłowy i dzięki temu szybko można im wysłać aktualną wersję.

# Podsumowanie
Dużo tych komend ale większość z nich trzeba znać aby używać git-a sensownie i mieć z niego pożytek. Często spotykam niestety programistów, którzy ograniczają się do commit-a, push i tyle. Potem nie umiejąc docenić jak dobrze pracuje się z git-em chodzą i narzekają. Git to narzędzie, które może bardzo przyspieszyć pracę ale trzeba umieć z tego skorzystać. Polecam używać i ćwiczyć. Jeśli macie jakiś komendy, które należało by dopisać to śmiało dopisze i będziecie mieli to w jednym miejscu.





# Usuwanie gałęzi zdalnych i lokalnych
## Pobieranie zdalnych gałęzi
# Usuwanie gałęzi zdalnych i lokalnych


Lokalne:

git branch -d branch_name
git branch -D branch_name
Zdalne:

git push origin -d <branch>  
Pobieranie zdalnych gałęzi
Źródło SO

Aby pobrać zdalną gałąź należy znać jej nazwę

git fetch --all
Potem wystarczy zrobić poniższą komendę:

git checkout -t <name of remote>/test

## Kopiowanie repo do innego serwera:



```bash
git clone --mirror <URL to my OLD repo location>
cd <New directory where your OLD repo was cloned>
git remote set-url origin <URL to my NEW repo location>
git push --mirror origin
```


