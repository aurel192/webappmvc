-- --------------------------------------------------------
-- Host:                         80.211.196.146
-- Server version:               8.0.16 - MySQL Community Server - GPL
-- Server OS:                    Linux
-- HeidiSQL Verzió:              9.5.0.5196
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- Dumping database structure for positions
CREATE DATABASE IF NOT EXISTS `positions` /*!40100 DEFAULT CHARACTER SET latin1 COLLATE latin1_spanish_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `positions`;

-- Dumping structure for függvény positions.GetInstrLastPrice
DELIMITER //
CREATE DEFINER=`root`@`%` FUNCTION `GetInstrLastPrice`(tipus char(100), nev char(100)) RETURNS double
    DETERMINISTIC
BEGIN
    CASE tipus
        WHEN 'RESZVENY' THEN 
            RETURN (SELECT DockerMySqlDb2.portfolio_hunstock_data.Close as PRICE
                    FROM DockerMySqlDb2.portfolio_hunstock
                    JOIN DockerMySqlDb2.portfolio_hunstock_data on DockerMySqlDb2.portfolio_hunstock_data.StockId = DockerMySqlDb2.portfolio_hunstock.id
                    WHERE DockerMySqlDb2.portfolio_hunstock.name = nev
                    ORDER BY date DESC
                LIMIT 1);
        WHEN 'BEFALAP' THEN
            RETURN (SELECT DockerMySqlDb2.portfolio_hunfund_data.Price as PRICE
                    FROM DockerMySqlDb2.portfolio_hunfund
                    JOIN DockerMySqlDb2.portfolio_hunfund_data on DockerMySqlDb2.portfolio_hunfund_data.FundId = DockerMySqlDb2.portfolio_hunfund.id
                    WHERE DockerMySqlDb2.portfolio_hunfund.name LIKE CONCAT('%',nev,'%')
                    ORDER BY date DESC
                    LIMIT 1);
    END CASE;
END//
DELIMITER ;

-- Dumping structure for függvény positions.GetInstrLastUpdate
DELIMITER //
CREATE DEFINER=`root`@`%` FUNCTION `GetInstrLastUpdate`(tipus char(100), nev char(100)) RETURNS datetime
    DETERMINISTIC
BEGIN
    CASE tipus
        WHEN 'RESZVENY' THEN 
            RETURN (SELECT DockerMySqlDb2.portfolio_hunstock_data.date as DATUM
                    FROM DockerMySqlDb2.portfolio_hunstock
                    JOIN DockerMySqlDb2.portfolio_hunstock_data on DockerMySqlDb2.portfolio_hunstock_data.StockId = DockerMySqlDb2.portfolio_hunstock.id
                    WHERE DockerMySqlDb2.portfolio_hunstock.name = nev
                    ORDER BY date DESC
                LIMIT 1);
        WHEN 'BEFALAP' THEN
            RETURN (SELECT DockerMySqlDb2.portfolio_hunfund_data.date as DATUM
                    FROM DockerMySqlDb2.portfolio_hunfund
                    JOIN DockerMySqlDb2.portfolio_hunfund_data on DockerMySqlDb2.portfolio_hunfund_data.FundId = DockerMySqlDb2.portfolio_hunfund.id
                    WHERE DockerMySqlDb2.portfolio_hunfund.name LIKE CONCAT('%',nev,'%')
                    ORDER BY date DESC
                    LIMIT 1);
    END CASE;
END//
DELIMITER ;

-- Dumping structure for tábla positions.koin
CREATE TABLE IF NOT EXISTS `koin` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `datum` date NOT NULL,
  `kategoria` varchar(50) NOT NULL,
  `cimkek` varchar(100) DEFAULT NULL,
  `osszeg` mediumint(9) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3664 DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
-- Dumping structure for tábla positions.position
CREATE TABLE IF NOT EXISTS `position` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `szamla` varchar(50) NOT NULL,
  `ertekpapir` varchar(50) NOT NULL,
  `tipus` varchar(50) DEFAULT NULL,
  `vetelar` float NOT NULL,
  `darab` float NOT NULL,
  `vetelikoltseg` float NOT NULL DEFAULT '0',
  `veteldatum` datetime NOT NULL,
  `eladasikoltseg` float NOT NULL DEFAULT '0',
  `lezart` varchar(15) NOT NULL DEFAULT '0',
  `osztaleksum` float NOT NULL DEFAULT '0',
  `eladasiar` float NOT NULL DEFAULT '0',
  `eladasdatum` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=63 DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
-- Dumping structure for függvény positions.RefreshPortfolio
DELIMITER //
CREATE DEFINER=`root`@`%` FUNCTION `RefreshPortfolio`() RETURNS int(11)
    DETERMINISTIC
BEGIN
	DECLARE darab INT;
	DECLARE cnt INT;
   DECLARE lastUpdate DATETIME;
   DECLARE Price DOUBLE;
   DECLARE varLezart VARCHAR(50);
   DECLARE varTipus VARCHAR(50);
   DECLARE varErtekpapir VARCHAR(100);
  	DECLARE varid INT;
	SET darab := 0;
	SET cnt := 0;
	    
	SELECT COUNT(*) into darab FROM position;
	
	WHILE cnt<=darab DO 
      SELECT lezart into varLezart  FROM position limit 1 offset cnt;
      SELECT tipus into varTipus FROM position limit 1 offset cnt;
      SELECT ertekpapir into varErtekpapir FROM position limit 1 offset cnt;
      SELECT id into varid FROM position limit 1 offset cnt;
      

      IF (varLezart = 'NEM') THEN
	      SET Price := GetInstrLastPrice(varTipus, varErtekpapir);
	      SET lastUpdate := GetInstrLastUpdate(varTipus, varErtekpapir);
			UPDATE position SET eladasiar  = Price WHERE id = varid;
			UPDATE position SET eladasdatum = lastUpdate WHERE id = varid;
      END IF;
      
		SET cnt = cnt + 1;
	END WHILE;
	
	RETURN darab;
END//
DELIMITER ;

-- Dumping structure for függvény positions.ToDate
DELIMITER //
CREATE DEFINER=`root`@`%` FUNCTION `ToDate`(
	`d` DATETIME

) RETURNS text CHARSET latin1
    NO SQL
BEGIN
    RETURN CONCAT(DATE_FORMAT(d, "%Y"),'.',DATE_FORMAT(d, "%m"),'.',DATE_FORMAT(d, "%d"));
END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
