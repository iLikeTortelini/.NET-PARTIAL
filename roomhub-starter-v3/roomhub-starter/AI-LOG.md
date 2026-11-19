# AI-LOG

Write down, in order, what you asked the AI. It is not a punishment: a missing or false log lowers the level D grade.

| Time (approx.) | What you asked (briefly) | What you kept | What you changed or rejected |
|---|---|---|---|
| 5 minute | Analiza codului starter fata de regulile R1–R8 din si reorganizarea endpoint-urilor in vertical slices |impartirea în cele 5 foldere din Features/Rooms si Features/Bookings si repararea conditiei de overlap (< în loc de <=). | - |
|4 minute |Refactorizarea modelelor anemice in Rich Domain (Room, Booking) cu Result<T> si Error, plus implementarea ICommandHandler/IQueryHandler si a regulilor R5 (TimeProvider) si R6 (max 4h).|Structura Result<T>, Error, Unit, metodele Room.Create, Booking.Create, Booking.Cancel si inregistrarea automata a handlerelor cu reflection|Am cerut mutarea regulii de overlap intr-o metoda expresiva OverlapsWith pe clasa Booking si pastrarea verificarii de existenta a salii (404)|
