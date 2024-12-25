# C# Projekt - DT071G
## Resedagbok (TravelDiary) konsolapplikation

### Uppgiftsbeskrivning:
Denna uppgift ingår i det avslutande momentet av kursen DT071G och innefattar en projektuppgift skriven i C#. Uppgiften gick ut på att skapa en mer omfattande och användbar applikation som är konsol-, desktop-, webb- eller mobilbaserad. 
Jag har valt att skapa en konsolapplikation vid namn **TravelDiary**. Konsolapplikationen fungerar om en resedagbok där man kan dokumentera och samla alla sina resor på ett och samma ställe, för att kunna gå tillbaka och återuppleva minnen från sina tidigare äventyr. Applikationen gör det möjligt att registera detaljer om resor, såsom resmål, kontinent, datum, kostnad och reskompisar. För varje resa kan användaren också ange om det är en semester- eller jobbresa. Utöver detta innehåller applikationen funktionalitet för att hantera packlistor för kommande resor, där användaren kan lägga till, redigera, markera och ta bort objekt som behöver packas. 

Applikationen är byggd för att vara användarvänlig och flexibel. Det enkla menysystemet gör så att användaren kan navigera mellan de olika menyalternativen. Data lagras i JSON-filer, vilket säkerställer att informationen sparas och enkelt kan återhämtas vid nästa användning. 

### Funktionalitet: 
**Hantera resor:**
- Visa en lista över alla resor i resedagboken.
- Lägg till en ny resa med detaljer som destination, kontinent, start- och slutdatum, antal dagar, kostnad, reskamrater/soloresa och semester/jobbresa. 
- Redigera detaljer om en befintlig resa.
- Ta bort en resa från dagboken.

**Hantera packlistor:**
- Skapa en packlista för en kommande resa.
- Lägg till, ta bort eller markera objekt som packade i en packlista.
- Visa och hantera befintliga packlistor.
- Ta bort en hel packlista. 

### Validering och felhantering
Alla inmatningar från användaren valideras noggrant för att undvika ogiltig eller ofullständig data. Varje felmeddelande skrivs ut i röd text för att göra det än mer tydligt för användaren att input är felaktigt. Bekräftelsemeddelanden skrivs ut i grön text. 

### Teknologier och verktyg:
- **Språk:** C# (.NET)
- **Datahantering:** JSON för att spara och läsa resor och packlistor.
- **Validering:** Regex används för att validera inmatning, t.ex. namn och datumformat.
- **Färg i konsolen:** Specifik text i menyerna samt felmeddelanden och bekräftelsemeddelanden är färgad för bättre användarupplevelse.

### Såhär kör du programmet: 
1. Klona projektet med följande kommando:
```bash
git clone https://github.com/julieandersson/TravelDiary.git
```
2. Navigera till projektmappen i terminalen/komandotolken.
3. Kör följande kommando:
```bash
dotnet run
```
Utskrift av meny (visas direkt när programmet har startats):
``` bash
T R A V E L   D I A R Y

Hej och välkommen till resedagboken! Här kan du samla alla dina resor du gjort för att återuppleva fantastiska minnen och dokumentera dina äventyr på ett och samma ställe.

Gör ett val nedan för att ta dig vidare i din travel diary!

H A N T E R A   B E S Ö K T A   R E S O R
[1] - Visa alla resor i dagboken
[2] - Lägg till en resa i dagboken
[3] - Redigera en befintlig resa i dagboken
[4] - Ta bort en resa från dagboken

H A N T E R A   P A C K L I S T O R   F Ö R   K O M M A N D E   R E S O R
[5] - Skapa en packlista för kommande resa
[6] - Visa och hantera packlistor
[7] - Ta bort en packlista

[X] - Stäng resedagboken
```

### Tanke bakom konsolapplikationen
**TravelDiary** är ett praktiskt och välfungerande verktyg med potential till ytterligare vidare utveckling eller utökning i framtiden. Jag själv är en person som reser otroligt mycket och gillar att dokumentera och samla alla mina resor på ett ställe för att bevara resorna som ett minne, därav växte idén för TravelDiary fram. Det är också enkelt att man glömmer packa ner viktiga saker inför sina resor - därför valde jag att implementera en metod för att skapa packlistor, som man enkelt kan gå tillbaka till för att se så att allt är nedpackat, och hantera genom att lägga till fler föremål, markera det som är nedpackat eller ta bort föremål i listan.

### Skapad av:
- Julie Andersson
- Webbutvecklingsprogrammet på Mittuniversitetet i Sundsvall
- Projektuppgift i kursen DT071G Programmering i C#.NET

