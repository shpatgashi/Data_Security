create database workers;
use workers;
CREATE TABLE `workers`.`worker` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `username` VARCHAR(45) NOT NULL,
  `password` VARCHAR(45) NOT NULL,
  `position` VARCHAR(45) NOT NULL,
  `salary` REAL NOT NULL,
  `bonuses` INT NOT NULL,
  `experience` INT NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `username_UNIQUE` (`username` ASC) VISIBLE);
