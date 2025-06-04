-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema frogger
-- -----------------------------------------------------
DROP SCHEMA IF EXISTS `frogger` ;

-- -----------------------------------------------------
-- Schema frogger
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `frogger` DEFAULT CHARACTER SET utf8mb3 ;
SHOW WARNINGS;
USE `frogger` ;

-- -----------------------------------------------------
-- Table `frogger`.`roles`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `frogger`.`roles` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `frogger`.`roles` (
  `rol_id` INT NOT NULL AUTO_INCREMENT,
  `rol_name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`rol_id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;

SHOW WARNINGS;
CREATE UNIQUE INDEX `rol_name_UNIQUE` ON `frogger`.`roles` (`rol_name` ASC) VISIBLE;

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `frogger`.`users`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `frogger`.`users` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `frogger`.`users` (
  `user_id` INT NOT NULL AUTO_INCREMENT,
  `username` VARCHAR(20) NOT NULL,
  `password` CHAR(60) NOT NULL,
  `email` VARCHAR(90) NOT NULL,
  `Roles_rol_id` INT NOT NULL,
  PRIMARY KEY (`user_id`, `Roles_rol_id`),
  CONSTRAINT `fk_users_roles`
    FOREIGN KEY (`Roles_rol_id`)
    REFERENCES `frogger`.`roles` (`rol_id`)
    ON DELETE RESTRICT
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;

SHOW WARNINGS;
CREATE UNIQUE INDEX `Username_UNIQUE` ON `frogger`.`users` (`username` ASC) VISIBLE;

SHOW WARNINGS;
CREATE UNIQUE INDEX `email_UNIQUE` ON `frogger`.`users` (`email` ASC) VISIBLE;

SHOW WARNINGS;
CREATE INDEX `fk_users_roles_idx` ON `frogger`.`users` (`Roles_rol_id` ASC) VISIBLE;

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `frogger`.`user_follows_user`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `frogger`.`user_follows_user` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `frogger`.`user_follows_user` (
  `user_who_follows` INT NOT NULL,
  `user_to_follow` INT NOT NULL,
  PRIMARY KEY (`user_who_follows`, `user_to_follow`),
  CONSTRAINT `fk_users_has_users_users1`
    FOREIGN KEY (`user_who_follows`)
    REFERENCES `frogger`.`users` (`user_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_users_has_users_users2`
    FOREIGN KEY (`user_to_follow`)
    REFERENCES `frogger`.`users` (`user_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;

SHOW WARNINGS;
CREATE INDEX `fk_users_has_users_users2_idx` ON `frogger`.`user_follows_user` (`user_to_follow` ASC) VISIBLE;

SHOW WARNINGS;
CREATE INDEX `fk_users_has_users_users1_idx` ON `frogger`.`user_follows_user` (`user_who_follows` ASC) VISIBLE;

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `frogger`.`record_scores`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `frogger`.`record_scores` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `frogger`.`record_scores` (
  `record_id` INT NOT NULL AUTO_INCREMENT,
  `record_scorescol` TIME NOT NULL,
  `users_user_id` INT NOT NULL,
  PRIMARY KEY (`record_id`, `users_user_id`),
  CONSTRAINT `fk_record_scores_users1`
    FOREIGN KEY (`users_user_id`)
    REFERENCES `frogger`.`users` (`user_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

SHOW WARNINGS;
CREATE INDEX `fk_record_scores_users1_idx` ON `frogger`.`record_scores` (`users_user_id` ASC) VISIBLE;

SHOW WARNINGS;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
