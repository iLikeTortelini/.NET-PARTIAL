# BUGS

The starter contains behaviors that contradict `ASSIGNMENT.md`. Write here every discrepancy you found.
Fill it in yourself, in your own words. There is no required number: what counts is that they are real.

| Which rule from ASSIGNMENT.md is violated (R1–R8) | Where the problem was in the starter (file, approximate line) | How you discovered it (test, reading, AI) | What you changed |
|---|---|---|---|
|R7 | in Services/BookingService.cs (linia 28), conditia era existing.StartUtc <= endUtc && startUtc <= existing.EndUtc. Din cauza lui <=, doua rezervari se atingeau la capat (una se termina la 12:00, alta incepe la 12:00) | AI | Am schimbat operatorul în < strict |
|R5 |In Services/BookingService.cs (metoda Create, liniile 8-31) nu exista nicio verificare daca startUtc este in trecut.| AI | Am adaugat o verificare care arunca exceptia BusinessException|  
|R6|In Services/BookingService.cs (metoda Create) lipsea complet restrictia de durata maxima de 4 ore.| AI | Am adaugat verificarea duratei maxime si o validare a datei de final|
|R8|in Services/BookingService.cs (linia 56, metoda Cancel) nu verifica daca rezervarea era deja Cancelled (trebuia 409 Conflict), iar la linia 26 nu ignora rezervarile Cancelled la suprapunere.| AI | Am adaugat o verificare a starii rezervarii la cancelare si am modificat logica de suprapunere|
|---|---|---|---|
