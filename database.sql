-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               10.4.13-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win64
-- HeidiSQL Version:             11.3.0.6295
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Dumping database structure for soaapp
CREATE DATABASE IF NOT EXISTS `soaapp` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `soaapp`;

-- Dumping structure for table soaapp.derivada
CREATE TABLE IF NOT EXISTS `derivada` (
  `idDif` int(11) NOT NULL AUTO_INCREMENT,
  `gradoDif` int(11) NOT NULL,
  `dif` varchar(50) NOT NULL,
  `idEq` int(11) NOT NULL,
  PRIMARY KEY (`idDif`),
  KEY `idDif` (`idDif`),
  KEY `ecuacion_dif_kf` (`idEq`),
  CONSTRAINT `ecuacion_dif_kf` FOREIGN KEY (`idEq`) REFERENCES `ecuacion` (`idEq`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='guarda la derivada y el grado';

-- Dumping data for table soaapp.derivada: ~0 rows (approximately)
/*!40000 ALTER TABLE `derivada` DISABLE KEYS */;
/*!40000 ALTER TABLE `derivada` ENABLE KEYS */;

-- Dumping structure for table soaapp.ecuacion
CREATE TABLE IF NOT EXISTS `ecuacion` (
  `idEq` int(11) NOT NULL AUTO_INCREMENT,
  `eq` varchar(50) NOT NULL,
  PRIMARY KEY (`idEq`) USING BTREE,
  KEY `idEq` (`idEq`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=latin1 COMMENT='guarda la ecuacion ingresada';

-- Dumping data for table soaapp.ecuacion: ~0 rows (approximately)
/*!40000 ALTER TABLE `ecuacion` DISABLE KEYS */;
/*!40000 ALTER TABLE `ecuacion` ENABLE KEYS */;

-- Dumping structure for procedure soaapp.guardarDif
DELIMITER //
CREATE PROCEDURE `guardarDif`(
	IN `idEq` INT,
	IN `grado` INT,
	IN `eqdif` VARCHAR(50)
)
    COMMENT 'guarda la deribada y el grado'
BEGIN
	INSERT INTO `derivada` (`gradoDif`, `dif`, `idEq`) VALUES (grado, eqdif, idEq);
END//
DELIMITER ;

-- Dumping structure for function soaapp.guardarEcuacion
DELIMITER //
CREATE FUNCTION `guardarEcuacion`(`equa` CHAR(50)
) RETURNS int(11)
    COMMENT 'guarda la ecuacion '
BEGIN
	INSERT INTO `ecuacion` (`eq`) VALUES (equa);
	RETURN (SELECT @@IDENTITY AS 'Identity');
END//
DELIMITER ;

-- Dumping structure for procedure soaapp.guardarSol
DELIMITER //
CREATE PROCEDURE `guardarSol`(
	IN `sol` DOUBLE,
	IN `idEq` INT
)
    COMMENT 'guarda la(s) solucione(s) de la ecuacion'
BEGIN
	INSERT INTO `soluciones` (`sol`, `idEq`) VALUES (sol, idEq);
END//
DELIMITER ;

-- Dumping structure for table soaapp.soluciones
CREATE TABLE IF NOT EXISTS `soluciones` (
  `idSol` int(11) NOT NULL AUTO_INCREMENT,
  `sol` double NOT NULL,
  `idEq` int(11) NOT NULL,
  PRIMARY KEY (`idSol`),
  KEY `idSol` (`idSol`),
  KEY `ecuacion_sol_fk` (`idEq`),
  CONSTRAINT `ecuacion_sol_fk` FOREIGN KEY (`idEq`) REFERENCES `ecuacion` (`idEq`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='guarda las soluciones de la ecuacion';

-- Dumping data for table soaapp.soluciones: ~0 rows (approximately)
/*!40000 ALTER TABLE `soluciones` DISABLE KEYS */;
/*!40000 ALTER TABLE `soluciones` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
