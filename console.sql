CREATE TABLE  client (client_id uuid PRIMARY KEY , name text not null, age int not null);

INSERT INTO  client (client_id, name, age) 
VALUES
('550e8400-e29b-41d4-a716-446655440005', 'Bob Jonson', 20),
('550e8400-e29b-41d4-a716-446655440006', 'John Bobson', 21),
('550e8400-e29b-41d4-a716-446655440007', 'Bobson Johnson', 19),
('550e8400-e29b-41d4-a716-446655440008', 'Bobson Johnson', 22),
('550e8400-e29b-41d4-a716-446655440009', 'Bobson Johnson', 18);

CREATE TABLE account (account_id uuid PRIMARY KEY , amount decimal not null, currency_name text not null, client_id uuid not null,
                      FOREIGN KEY (client_id) REFERENCES client(client_id) );

INSERT INTO account (account_id, amount, currency_name, client_id) 
VALUES 
('550e8400-e29b-41d4-a716-446655440000', 1000.50, 'USD', '550e8400-e29b-41d4-a716-446655440005'),
('550e8400-e29b-41d4-a716-446655440001', 750.25, 'EUR', '550e8400-e29b-41d4-a716-446655440006'),
('550e8400-e29b-41d4-a716-446655440002', 7500.25, 'RUB', '550e8400-e29b-41d4-a716-446655440007'),
('550e8400-e29b-41d4-a716-446655440003', 1500.25, 'RUB', '550e8400-e29b-41d4-a716-446655440008'),
('550e8400-e29b-41d4-a716-446655440004', 10000.25, 'RUB', '550e8400-e29b-41d4-a716-446655440009');

CREATE TABLE  employee (employee_id uuid PRIMARY KEY , name text not null , age int not null );
INSERT INTO  employee (employee_id, name, age) 
VALUES
(gen_random_uuid(), 'John Doe', 20),
(gen_random_uuid(), 'Doe Johnson', 21),
(gen_random_uuid(), 'Eminem Marshal', 19),
(gen_random_uuid(), 'Patrick Baytman', 22),
(gen_random_uuid(), 'Jack Johnson', 18);

SELECT c.client_id, c.name, c.age, a.amount
FROM client c
JOIN account a ON c.client_id = a.client_id
WHERE a.amount < 5000
ORDER BY a.amount ASC;

SELECT c.client_id, c.name, c.age, a.amount
FROM client c
JOIN account a ON c.client_id = a.client_id
ORDER BY a.amount ASC
LIMIT 1;

SELECT SUM(a.amount) AS total_amount
FROM account a;

SELECT a.account_id, a.amount, a.currency_name, c.client_id, c.name, c.age
FROM account a
JOIN client c ON a.client_id = c.client_id;

SELECT c.client_id, c.name, c.age
FROM client c
ORDER BY c.age DESC;

SELECT c.age, COUNT(*) AS count_of_clients
FROM client c
GROUP BY c.age
HAVING COUNT(*) > 1;

SELECT c.age, ARRAY_AGG(c.name) AS clients
FROM client c
GROUP BY c.age;

SELECT c.client_id, c.name, c.age
FROM client c
LIMIT 3;
