\c HoneyRaes

INSERT INTO Customer (Name, Address)
VALUES ('Batman', '1244 Park Row, Gotham City, NJ');

INSERT INTO Customer (Name, Address)
VALUES ('Robin 2', '1244 Park Row, Gotham City, NJ');

INSERT INTO Customer (Name, Address)
VALUES ('Joker', '566 Bolland Park, Gotham City, NJ');

INSERT INTO Employee (Name, Specialty)
VALUES ('Jim Gordon', 'Phones');

INSERT INTO Employee (Name, Specialty)
VALUES ('Riddler', 'Game Consoles');

INSERT INTO ServiceTicket (CustomerId, EmployeeId, Description, Emergency, DateCompleted)
VALUES (1, 1, 'Pinball is stuck in the phone', TRUE, '2024-03-27 14:30:11');

INSERT INTO ServiceTicket (CustomerId, EmployeeId, Description, Emergency, DateCompleted)
VALUES (2, 2, 'Pinball is stuck in Xbox', FALSE, '2024-03-27 14:30:11');

INSERT INTO ServiceTicket (CustomerId, Description, Emergency)
VALUES (3, 'Pinball is stuck in pinball machine', FALSE);

INSERT INTO ServiceTicket (CustomerId, Description, Emergency)
VALUES (1, 'Pinball is stuck to another pinball', FALSE);

INSERT INTO ServiceTicket (CustomerId, EmployeeId, Description, Emergency)
VALUES (2, 1, 'Pinball is stuck in stomach', TRUE);



