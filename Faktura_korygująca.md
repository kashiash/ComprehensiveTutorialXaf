# Faktura korygująca

## Całkowita
wartość faktury i korekty pozmianie =0
wartość sumy pozycji po zminiae = 0
ilośc pozycji bez zmian
kazda pozycka+ odpowiadajaca korekta = 0

### częściowa na podstawie roznicy
* przekazujemy towar i wartosci róznicowe (mogą nie byc zgodne z zasada netto + vat = brutto)
* korygowac możemy towar, ilosc, sume netto/vat/brutto - cena jednostkowa wyliczana z neeto/ilosc

### częściowa na podstawie wartości oczekiwanej po korecke

* wysyłam info jak ma byc poprawnie, a program wystawia korekte aby wykazac róznicę

## inne zalożenia

* ~~tylko in minus?? a co gdy zmiana towaru~~
* kontrahent ten sam
*  posiada opis :powód korekty
*  

## działanie UI
 pow wystawieniu korekty powinien pojawić sie ekran z wystawiona korektą
na ekranie maja byc widoczne dane do faktury
3 listy :
** pozycje przed korektą
** pozycje po korekcie
** róznica w pozycjach

### ficzery i inne pomysły z czapy - co wcale nie oznacza ze nie należy ich zrobić
* zmieniając bieżacy rekord fajnie byloby kolorem podswietlac, pozycje na pozostalych listach dotyczace tej samej pozycji
* zmieniając szerokość kolumny ilosc na jednym gridzie, synchronizowac szerokosc na pozostalych gridach

Ewentualne materiały jak toz robić są tutaj:

<a href="https://supportcenter.devexpress.com/ticket/details/t928633/xaf-win-how-to-programmatically-find-and-select-a-record-on-list-view-gri" target="_blank">https://supportcenter.devexpress.com/ticket/details/t928633/xaf-win-how-to-programmatically-find-and-select-a-record-on-list-view-gri</a>d

<a href="https://supportcenter.devexpress.com/ticket/details/cs47517/how-to-set-the-default-current-object-after-opening-a-listview" target="_blank">https://supportcenter.devexpress.com/ticket/details/cs47517/how-to-set-the-default-current-object-after-opening-a-listview</a>


<a href="https://supportcenter.devexpress.com/ticket/details/t905770/can-i-set-the-default-selected-objects-when-showing-a-popup-list-view" target="_blank">https://supportcenter.devexpress.com/ticket/details/t905770/can-i-set-the-default-selected-objects-when-showing-a-popup-list-view</a>

<a href="https://supportcenter.devexpress.com/ticket/details/t496358/xaf-select-focus-a-specific-row-on-opening-a-list-view" target="_blank">https://supportcenter.devexpress.com/ticket/details/t496358/xaf-select-focus-a-specific-row-on-opening-a-list-view</a>
