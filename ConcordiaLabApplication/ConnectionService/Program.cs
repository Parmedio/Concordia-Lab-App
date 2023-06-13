
Console.WriteLine("Hello, World!");

//Durante il giorno non deve fare niente, ad una determinata ora manda un sgnale al connection checker ad ogni x intervallo di tempo prestabilito


//A.Parametrizzare i valori per l'allineamento è importante perché l'orario e la finestra di tempo in cui avviene il passaggio del satellite possono variare a seconda del periodo dell'anno. Ad esempio, a causa della rotazione terrestre e dell'orbita del satellite, l'orario del passaggio può spostarsi leggermente nel corso dei mesi. Parametrizzando questi valori, è possibile adattare dinamicamente l'allineamento del sistema in base al periodo corrente, senza dover apportare modifiche al codice sorgente una volta che il software è stato distribuito sui sistemi della Concordia.

//B. Per coprire tutto l'anno solare senza dover modificare il codice dopo il deployment, è consigliabile definire i valori di allineamento già durante la fase di sviluppo o di configurazione del sistema. Questo può essere fatto utilizzando variabili di configurazione o parametri del sistema che consentono di impostare dinamicamente i valori di orario e finestra di tempo di passaggio del satellite in base al periodo dell'anno corrente.

//Ad esempio, è possibile definire una variabile di configurazione chiamata "orarioPassaggioSatellite" e una chiamata "finestraTempoPassaggioSatellite". Durante la configurazione iniziale del sistema o durante l'aggiornamento periodico delle configurazioni, è possibile impostare questi valori in base al periodo dell'anno. In questo modo, il sistema utilizzerà automaticamente i valori corretti per l'allineamento durante l'esecuzione, senza richiedere modifiche al codice sorgente.

//Questo approccio garantisce flessibilità nel gestire le variazioni dell'orario di passaggio del satellite nel corso dell'anno senza dover toccare il codice sorgente. Inoltre, facilita la manutenzione del sistema, in quanto i valori di allineamento possono essere modificati semplicemente aggiornando le configurazioni senza richiedere una nuova distribuzione del software.