CREATE TABLE url
(
     _id BIGINT PRIMARY KEY AUTO_INCREMENT,
     url TEXT NOT NULL,
     shorted TEXT NOT NULL,
     view BIGINT NOT NULL,
     date TEXT NOT NULL,
     password TEXT
) ENGINE=INNODB;