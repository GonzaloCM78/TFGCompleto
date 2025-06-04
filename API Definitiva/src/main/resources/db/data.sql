INSERT INTO frogger.roles VALUES (1, "ADMIN"), (2, "USER");

INSERT INTO frogger.users VALUES (1, "admin", "$2a$10$ijkPeb2bVjTYbmcOnataRuqrm79Q64brGZGGYQxgeG49HdwLjouJO", "admin@localhost.com", 1);
INSERT INTO frogger.users VALUES (2, "Joaquin", "$2a$10$ijkPeb2bVjTYbmcOnataRuqrm79Q64brGZGGYQxgeG49HdwLjouJO", "joaquin.borrego@vedruna.es", 2);
INSERT INTO frogger.users VALUES (3, "Jorge", "$2a$10$ijkPeb2bVjTYbmcOnataRuqrm79Q64brGZGGYQxgeG49HdwLjouJO","jorgejuan.munoz@vedruna.es", 2);
INSERT INTO frogger.users VALUES (4, "Joseca", "$2a$10$ijkPeb2bVjTYbmcOnataRuqrm79Q64brGZGGYQxgeG49HdwLjouJO", "josecarlos.moreno@vedruna.es", 2);
INSERT INTO frogger.record_scores VALUES (0, "00:05:47", 2), (0, "00:06:53", 3), (0, "23:59:59", 4);
INSERT INTO frogger.user_follows_user VALUES  (2,3), (3, 4), (4, 2), (4,3);