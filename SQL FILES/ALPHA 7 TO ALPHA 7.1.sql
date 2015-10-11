DELETE FROM texts WHERE identifier LIKE '%pet_cmd_%';

INSERT INTO texts(identifier, display_text) VALUES('pet_cmd_free', 'free');
INSERT INTO texts(identifier, display_text) VALUES('pet_cmd_sit', 'sit');
INSERT INTO texts(identifier, display_text) VALUES('pet_cmd_down', 'down');
INSERT INTO texts(identifier, display_text) VALUES('pet_cmd_here', 'here');
INSERT INTO texts(identifier, display_text) VALUES('pet_cmd_beg', 'beg');
INSERT INTO texts(identifier, display_text) VALUES('pet_cmd_play_dead', 'play dead');
INSERT INTO texts(identifier, display_text) VALUES('pet_cmd_stay', 'stay');
INSERT INTO texts(identifier, display_text) VALUES('pet_cmd_follow', 'follow');
INSERT INTO texts(identifier, display_text) VALUES('pet_cmd_stand', 'stand');
INSERT INTO texts(identifier, display_text) VALUES('pet_cmd_jump', 'jump');
INSERT INTO texts(identifier, display_text) VALUES('pet_cmd_speak', 'speak');
INSERT INTO texts(identifier, display_text) VALUES('pet_cmd_play', 'play');
INSERT INTO texts(identifier, display_text) VALUES('pet_cmd_silent', 'silent');
INSERT INTO texts(identifier, display_text) VALUES('pet_cmd_nest', 'nest');
INSERT INTO texts(identifier, display_text) VALUES('pet_cmd_drink', 'drink');
INSERT INTO texts(identifier, display_text) VALUES('pet_cmd_follow_left', 'follow left');
INSERT INTO texts(identifier, display_text) VALUES('pet_cmd_follow_right', 'follow right');
INSERT INTO texts(identifier, display_text) VALUES('pet_cmd_play_football', 'play football');
INSERT INTO texts(identifier, display_text) VALUES('pet_cmd_move_forwar', 'move forwar');
INSERT INTO texts(identifier, display_text) VALUES('pet_cmd_turn_left', 'turn left');
INSERT INTO texts(identifier, display_text) VALUES('pet_cmd_turn_right', 'turn right');
INSERT INTO texts(identifier, display_text) VALUES('pet_cmd_eat', 'eat');

ALTER TABLE `permissions_ranks` ADD `acc_staffpicks` ENUM('1', '0') NOT NULL DEFAULT '0';

ALTER TABLE `permissions_users` ADD `acc_staffpicks` ENUM('1', '0') NOT NULL DEFAULT '0';
