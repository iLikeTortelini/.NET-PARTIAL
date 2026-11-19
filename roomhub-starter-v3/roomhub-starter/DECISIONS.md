# DECISIONS

Answer **yourself**, in your own words, in 3–5 sentences per question.
Refer to concrete files and classes from your own repo (for example `Features/Bookings/CreateBooking/...`).
An unanswered question gives 0 points.

## 1. A rule you moved
Pick one business rule (R1–R8). In which file did it live in the starter and in which file does it live now? Why there?

In starter, regula R6 (durata maxima de 4 ore) si regula R4 (attendees intre 1 si capacitatea salii) erau fie absente, fie verificate procedural in Services/BookingService.cs. Acum aceste reguli se afla in metoda factory Booking.Create din src/RoomHub.Api/Domain/Bookings/Booking.cs, iar proprietatile clasei Booking au private set. Le-am mutat acolo pentru a avea un Rich Domain Model.

## 2. Result instead of an exception
Why is `Result` a better choice than an exception for "the room is occupied"? What happens in your code when an `Error` is returned?

O sala ocupata (RoomOccupied) nu este o eroare neasteptata de sistem (cum ar fi picarea bazei de date), ci un caz normal de business prevazut in cerinte, deci controlul fluxului prin throw new BusinessException este lent si ascunde semnatura reala a metodei. In codul meu, cand newBooking.OverlapsWith(existing) este adevarat handlerul returneaza un Result.IsError = true impreuna cu un Error.Type.Conflict care semnaleaza problema.

## 3. Who knows about EF Core?
Which classes in your project know about EF Core and which do not? What do you gain and what do you pay with Repository + Unit of Work?

Lista claselor din aplicatie care sunt constiente de EF Core (si de baze de date in general) este mica si concentrata. Cele mai multe straturi ale aplicatiei, cum ar fi BookingHandler.cs sau metodele logicilor din SharedKernel si Features/Bookings, nu stiu nimic despre EF Core, .NET DbContext sau SQL, folosind doar Domain sau Application specific. Avantajul este ca pot schimba simplu de la EF Core la Dapper sau alt ORM fara sa risc sa rup codul din Business Logic.

## 4. The flow of a request
Draw in text (ASCII arrows) the path of a `POST /api/rooms/{roomId}/bookings` request through your project, with the **real** names of files and classes, from the endpoint to the database.

BookingHandler.CreateBookingRequest -> Booking.Create (receptarea corectitudinii) -> BookingRepository.AddAsync -> UnitOfWork.SaveAsync -> BookingRoomResult.Success.

## 5. What you rejected from what the AI gave you
Give a concrete example of code or an idea proposed by the AI that you corrected or rejected, and say why.

AI-ul a propus ca in GetRoomByIdEndpoint si GetBookingByIdEndpoint sa injectam direct repository-ul in endpoint sau sa lasam validarea de suprapunere (R7) cu expresia booleana direct in CreateBookingHandler. Am respins ideea de a lasa formula matematica de intersectie a intervalelor in handler si am mutat-o intr-o metoda de domeniu booking.OverlapsWith(existing)

## 6. If you had 30 more minutes
What would you improve and what risk did you leave in the project?

_(your answer here)_
