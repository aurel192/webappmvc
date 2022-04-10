CREATE TABLE `position` (
	`id` INT(11) NOT NULL AUTO_INCREMENT,
	`szamla` VARCHAR(50) NOT NULL,
	`ertekpapir` VARCHAR(50) NOT NULL,
	`tipus` VARCHAR(50) NULL DEFAULT NULL,
	`vetelar` FLOAT NOT NULL,
	`darab` FLOAT NOT NULL,
	`vetelikoltseg` FLOAT NOT NULL DEFAULT '0',
	`veteldatum` DATETIME NOT NULL,
	`eladasikoltseg` FLOAT NOT NULL DEFAULT '0',
	`lezart` VARCHAR(15) NOT NULL DEFAULT '0',
	`osztaleksum` FLOAT NOT NULL DEFAULT '0',
	`eladasiar` FLOAT NOT NULL DEFAULT '0',
	`eladasdatum` DATETIME NULL DEFAULT NULL,
	PRIMARY KEY (`id`)
)
COLLATE='latin1_swedish_ci'
ENGINE=InnoDB
AUTO_INCREMENT=22
;

/*
=CONCATENATE("INSERT INTO position (szamla,ertekpapir,vetelar,darab,vetelikoltseg,veteldatum,eladasiar,eladasikoltseg,lezart,osztaleksum,eladasdatum) VALUES('",A2,"', '",B2,"' ,",E2,", ",D2,",",O2, " ,'",TEXT(C2, "yyyy-mm-dd"),"',",G2,", ", P2,", '",I2,"' ,",K2,", '",TEXT(H2, "yyyy-mm-dd"),"');")
*/


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
-- Dumping data for table positions.position: ~21 rows (approximately)
/*!40000 ALTER TABLE `position` DISABLE KEYS */;
INSERT INTO `position` (`id`, `szamla`, `ertekpapir`, `tipus`, `vetelar`, `darab`, `vetelikoltseg`, `veteldatum`, `eladasikoltseg`, `lezart`, `osztaleksum`, `eladasiar`, `eladasdatum`) VALUES
	(1, 'OTP ERT', 'OTP', 'RESZVENY', 10090, 7, 413, '2017-11-15 00:00:00', 450, 'IGEN', 0, 10820, '2018-01-04 00:00:00'),
	(2, 'OTP TBSZ18', 'MAXIMA', 'BEFALAP', 3.7209, 52321, 211, '2018-01-12 00:00:00', 600, 'NEM', 0, 3.71863, '2018-12-06 00:00:00'),
	(3, 'OTP ERT', 'OTP', 'RESZVENY', 10730, 11, 413, '2018-05-11 00:00:00', 450, 'IGEN', 2070, 10930, '2018-11-08 00:00:00'),
	(4, 'OTP ERT', 'TREND', 'BEFALAP', 22905.1, 3, 216, '2018-03-20 00:00:00', 600, 'NEM', 0, 22954.2, '2018-12-06 00:00:00'),
	(5, 'OTP ERT', 'MAXIMA', 'BEFALAP', 3.7209, 13767, 0, '2018-01-17 00:00:00', 600, 'NEM', 0, 3.71863, '2018-12-06 00:00:00'),
	(6, 'OTP ERT', 'MAXIMA', 'BEFALAP', 3.7209, 13238, 216, '2018-05-08 00:00:00', 600, 'NEM', 0, 3.71863, '2018-12-06 00:00:00'),
	(7, 'KBC', 'ANY', 'RESZVENY', 1430, 20, 199, '2018-05-31 00:00:00', 199, 'NEM', 2754, 1280, '2018-12-07 00:00:00'),
	(8, 'KBC', 'ANY', 'RESZVENY', 1420, 20, 199, '2018-06-04 00:00:00', 199, 'NEM', 0, 1280, '2018-12-07 00:00:00'),
	(9, 'KBC', 'PLOTINUS', 'RESZVENY', 6000, 4, 199, '2018-06-04 00:00:00', 199, 'IGEN', 0, 6000, '2018-06-12 00:00:00'),
	(10, 'KBC', 'DUNAHOUSE', 'RESZVENY', 4280, 7, 199, '2018-06-04 00:00:00', 199, 'NEM', 1057, 3760, '2018-12-07 00:00:00'),
	(11, 'KBC', 'GSPARK', 'RESZVENY', 3770, 7, 199, '2018-06-12 00:00:00', 199, 'NEM', 0, 3550, '2018-11-29 00:00:00'),
	(12, 'KBC', 'RICHTER', 'RESZVENY', 5125, 7, 199, '2018-08-03 00:00:00', 199, 'IGEN', 0, 5195, '2018-08-07 00:00:00'),
	(13, 'KBC', 'MTEL', 'RESZVENY', 412, 95, 199, '2018-08-03 00:00:00', 199, 'IGEN', 0, 440, '2018-11-15 00:00:00'),
	(14, 'KBC', 'DUNAHOUSE', 'RESZVENY', 3740, 10, 199, '2018-11-27 00:00:00', 199, 'NEM', 0, 3760, '2018-12-07 00:00:00'),
	(15, 'KBC', 'RICHTER', 'RESZVENY', 5315, 6, 199, '2018-08-23 00:00:00', 199, 'IGEN', 0, 5410, '2018-08-30 00:00:00'),
	(16, 'MÁK-ERT', '1MÁP K2019/32', 'AK', 10000, 1, 0, '2018-07-08 00:00:00', 0, 'FIX', 0, 10250, '2019-07-08 00:00:00'),
	(17, 'MÁK-ERT', '1MÁP K2019/47', 'AK', 10000, 1, 0, '2018-11-20 00:00:00', 0, 'FIX', 0, 10250, '2019-11-20 00:00:00'),
	(18, 'MÁK-ERT', '2MÁP 2020/H5', 'AK', 10048, 1, 0, '2018-11-28 00:00:00', 0, 'FIX', 0, 10600, '2020-11-28 00:00:00'),
	(19, 'MÁK-ERT', '1MÁP K2019/50', 'AK', 10000, 1, 0, '2018-12-06 00:00:00', 0, 'FIX', 0, 10250, '2019-12-11 00:00:00'),
	(20, 'LTP ANYA', 'U START 4', 'LTP', 920000, 1, 6900, '2018-07-11 00:00:00', 5000, 'FIX', 0, 1119600, '2022-05-11 00:00:00'),
	(21, 'LTP AU', 'U START 4', 'LTP', 920000, 1, 6900, '2018-07-11 00:00:00', 5000, 'FIX', 0, 1119600, '2022-05-11 00:00:00');
/*!40000 ALTER TABLE `position` ENABLE KEYS */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;



DELIMITER //
CREATE FUNCTION GetInstrLastPrice(tipus char(100), nev char(100))
  RETURNS DOUBLE DETERMINISTIC
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

DELIMITER //
CREATE FUNCTION GetInstrLastUpdate(tipus char(100), nev char(100))
  RETURNS DATETIME DETERMINISTIC
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


SELECT GetInstrLastPrice('RESZVENY','MTELEKOM');
SELECT GetInstrLastUpdate('RESZVENY','MTELEKOM');








-- UPDATE FUNCTION
DELIMITER //
CREATE FUNCTION RefreshPortfolio()
  RETURNS INT DETERMINISTIC
BEGIN
	DECLARE darab INT;
	DECLARE cnt INT;
   DECLARE lastUpdate DATETIME;
   DECLARE Price DOUBLE;
   DECLARE varLezart VARCHAR(50);
   DECLARE varTipus VARCHAR(50);
   DECLARE varErtekpapir VARCHAR(100);
	SET darab := 0;
	SET cnt := 0;
	    
	SELECT COUNT(*) into darab FROM position;
	
	WHILE cnt<=darab DO 
      SELECT lezart into varLezart  FROM position limit 1 offset cnt;
      SELECT tipus into varTipus FROM position limit 1 offset cnt;
      SELECT ertekpapir into varErtekpapir FROM position limit 1 offset cnt;      

      IF (varLezart = 'NEM') THEN
	      SET Price := GetInstrLastPrice(varTipus, varErtekpapir);
	      SET lastUpdate := GetInstrLastUpdate(varTipus, varErtekpapir);
			UPDATE position SET eladasiar  = Price WHERE id = cnt+1;
			UPDATE position SET eladasdatum = lastUpdate WHERE id = cnt+1;
      END IF;
      
		SET cnt = cnt + 1;
	END WHILE;
	
	RETURN darab;
END//
DELIMITER ;



SELECT RefreshPortfolio();

SELECT szamla as Szamla,
ertekpapir as Ertekpapir,
DATE(veteldatum) as VetelIdeje,
darab as Darab,
vetelar as Vetelar,
eladasiar as EladasiAr,
@veteliertek := FLOOR(darab*vetelar) as VeteliErtek,
lezart as Lezart,
DATE(eladasdatum) as EladasDatum,
osztaleksum as OsztalekBevetel,
@eladasiertekosztalekkal := FLOOR(darab*eladasiar + osztaleksum - vetelikoltseg - eladasikoltseg) as EladasiArOsztalekkal,
@profit := FLOOR(@eladasiertekosztalekkal - @veteliertek) as Profit,
FLOOR(darab*eladasiar - @veteliertek) as ProfitKtsgNelkul,
TRUNCATE((((@eladasiertekosztalekkal-eladasikoltseg) / ((darab*vetelar) - vetelikoltseg)) -1 )* 100,2) as ProfitSzazalek,
DATEDIFF(eladasdatum,veteldatum) as Napok
FROM position
ORDER BY VetelIdeje

SELECT SUM(Profit) FROM (
	SELECT szamla as Szamla,
	ertekpapir as Ertekpapir,
	DATE(veteldatum) as VetelIdeje,
	darab as Darab,
	vetelar as Vetelar,
	@veteliertek := FLOOR(darab*vetelar) as VeteliErtek,
	lezart as Lezart,
	DATE(eladasdatum) as EladasDatum,
	eladasiar as EladasiAr,
	osztaleksum as OsztalekBevetel,
	@eladasiertekosztalekkal := FLOOR(darab*eladasiar + osztaleksum - vetelikoltseg - eladasikoltseg) as EladasiArOsztalekkal,
	@profit := FLOOR(@eladasiertekosztalekkal - @veteliertek) as Profit,
	TRUNCATE((((@eladasiertekosztalekkal-eladasikoltseg) / ((darab*vetelar) - vetelikoltseg)) -1 )* 100,2) as ProfitSzazalek,
	DATEDIFF(eladasdatum,veteldatum) as Napok
	FROM position
 	WHERE Szamla NOT LIKE '%LTP%'
) as temp;

/*
2018-11-11
Szamla;Ertekpapir;VetelIdeje;Darab;Vetelar;VeteliErtek;Lezart;EladasDatum;EladasiAr;OsztalekBevetel;EladasiArOsztalekkal;Profit;ProfitSzazalek;Napok
OTP TBSZ18;MAXIMA;2018-01-12;52321;3.7209;194681;NEM;2018-12-10;3.71611;0;193619;-1062;-0.74;332
OTP ERT;TREND;2018-03-20;3;22905.1;68715;NEM;2018-12-10;22802.3;0;67590;-1125;-2.20;265
OTP ERT;MAXIMA;2018-01-17;13767;3.7209;51225;NEM;2018-12-10;3.71611;0;50559;-666;-2.47;327
OTP ERT;MAXIMA;2018-05-08;13238;3.7209;49257;NEM;2018-12-10;3.71611;0;48377;-880;-2.57;216
KBC;ANY;2018-05-31;20;1430;28600;NEM;2018-12-11;1280;2754;27956;-644;-2.26;194
KBC;ANY;2018-06-04;20;1420;28400;NEM;2018-12-11;1280;0;25202;-3198;-11.34;190
KBC;DUNAHOUSE;2018-06-04;7;4280;29960;NEM;2018-12-11;3700;1057;26559;-3401;-11.42;190
KBC;GSPARK;2018-06-12;7;3770;26390;NEM;2018-12-11;3510;0;24172;-2218;-8.46;182
KBC;DUNAHOUSE;2018-11-27;10;3740;37400;NEM;2018-12-11;3700;0;36602;-798;-2.14;14
*/



INSERT INTO position (szamla,ertekpapir,tipus,vetelar,darab,vetelikoltseg,veteldatum,eladasikoltseg,lezart,osztaleksum,eladasiar,eladasdatum)



INSERT INTO position (szamla,ertekpapir,tipus,vetelar,darab,vetelikoltseg,veteldatum,eladasikoltseg,lezart,osztaleksum,eladasiar,eladasdatum) VALUES
('OTP ERT','ANY','RESZVENY',985,50,0,'2015-04-08',0,'IGEN',0,1006,'2015-04-17'),
('OTP ERT','ANY','RESZVENY',1129,26,0,'2015-04-28',0,'IGEN',1638,1000,'2015-12-17'),
('OTP ERT','BUX ETF','ETF',1327,44,0,'2017-02-28',0,'IGEN',0,1373,'2015-04-17'),
('OTP ERT','DANUBIUS HOTELS','RESZVENY',5640,4,0,'2015-04-22',0,'IGEN',0,7960,'2015-06-23'),
('OTP ERT','GSPARK','RESZVENY',2218,18,0,'2015-04-14',0,'IGEN',0,2301,'2015-04-20'),
('OTP ERT','GSPARK','RESZVENY',2218,3,0,'2015-04-14',0,'IGEN',0,2301,'2015-04-20'),
('OTP ERT','GSPARK','RESZVENY',2289,5,0,'2015-06-25',0,'IGEN',0,2303,'2015-07-13'),
('OTP ERT','GSPARK','RESZVENY',2289,12,0,'2015-06-25',0,'IGEN',0,2290,'2015-10-16'),
('OTP ERT','GSPARK','RESZVENY',2289,3,0,'2015-06-25',0,'IGEN',0,2326,'2015-07-13'),
('OTP ERT','MTEL','RESZVENY',420,20,0,'2015-04-20',0,'IGEN',0,417,'2015-12-07'),
('OTP ERT','OTP','RESZVENY',5690,5,0,'2015-04-20',0,'IGEN',0,5811,'2015-04-24'),
('OTP ERT','MAXIMA','BEFALAP',3.23752,8330,0,'2014-07-11',0,'IGEN',0,3.38369,'2015-04-13'),
('OTP ERT','MAXIMA','BEFALAP',3.23752,7049,0,'2014-07-11',0,'IGEN',0,3.36837,'2015-02-12'),
('OTP ERT','MAXIMA','BEFALAP',3.6088,12411,0,'2016-10-14',0,'IGEN',0,3.80738,'2017-12-08'),
('OTP ERT','MAXIMA','BEFALAP',3.20931,7724,0,'2014-08-12',0,'IGEN',0,3.38369,'2015-04-13'),
('OTP ERT','OPTIMA','BEFALAP',6.04583,3327,0,'2014-6-18',0,'IGEN',0,6.13287,'2015-5-7'),
('OTP ERT','OPTIMA','BEFALAP',6.03337,4275,0,'2014-6-10',0,'IGEN',0,6.13287,'2015-5-7'),
('OTP ERT','OPTIMA','BEFALAP',5.97146,3350,0,'2014-2-11',0,'IGEN',0,6.13287,'2015-5-7'),
('OTP ERT','OPTIMA','BEFALAP',5.9817,4144,0,'2014-3-11',0,'IGEN',0,6.13287,'2015-5-7'),
('OTP ERT','OPTIMA','BEFALAP',6.04583,3356,0,'2014-6-18',0,'IGEN',0,6.13287,'2015-5-7'),
('OTP ERT','PALETTA','BEFALAP',4.07779,5057,0,'2014-8-12',0,'IGEN',0,4.2569,'2015-4-21'),
('OTP ERT','PALETTA','BEFALAP',4.71948,10549,0,'2017-3-10',0,'IGEN',0,5.08173,'2018-1-12'),
('OTP ERT','QUALITY','BEFALAP',2.7109,22055,0,'2014-7-14',0,'IGEN',0,2.92274,'2015-4-7'),
('OTP ERT','SUPRA','BEFALAP',3.76499,5258,0,'2014-2-14',0,'IGEN',0,3.62384,'2015-11-19'),
('OTP ERT','SUPRA','BEFALAP',3.37846,6814,0,'2015-2-12',0,'IGEN',0,3.62384,'2015-11-19'),
('OTP ERT','SUPRA','BEFALAP',3.84974,12933,0,'2016-11-11',0,'IGEN',0,4.14277,'2017-4-20'),
('OTP ERT','PLOTINUS','BEFALAP',4399,3,0,'2015-6-25',0,'IGEN',0,4720,'2015-9-21'),
('OTP ERT','PLOTINUS','BEFALAP',4200,5,0,'2015-4-22',0,'IGEN',0,4300,'2015-5-26'),
('OTP ERT','PLOTINUS','BEFALAP',4399,7,0,'2015-6-25',0,'IGEN',0,4829,'2015-9-30'),
('OTP ERT','RABA','BEFALAP',1220,18,0,'2015-4-22',0,'IGEN',0,1314,'2015-8-10'),







SELECT szamla as Szamla,
ertekpapir as Ertekpapir,
DATE(veteldatum) as VetelIdeje,
darab as Darab,
vetelar as Vetelar,
eladasiar as EladasiAr,
@veteliertek := FLOOR(darab*vetelar) as VeteliErtek,
lezart as Lezart,
DATE(eladasdatum) as EladasDatum,
osztaleksum as OsztalekBevetel,
@eladasiertekosztalekkal := FLOOR(darab*eladasiar + osztaleksum - vetelikoltseg - eladasikoltseg) as EladasiArOsztalekkal,
@profit := FLOOR(@eladasiertekosztalekkal - @veteliertek) as Profit,
FLOOR(darab*eladasiar - @veteliertek) as ProfitKtsgNelkul,
TRUNCATE((((@eladasiertekosztalekkal-eladasikoltseg) / ((darab*vetelar) - vetelikoltseg)) -1 )* 100,2) as ProfitSzazalek,
DATEDIFF(eladasdatum,veteldatum) as Napok
FROM position
WHERE Szamla LIKE '%MÁK%'
ORDER BY VetelIdeje;








SELECT 
@szamla := szamla as Szamla,
ertekpapir as Ertekpapir,
DATE(veteldatum) as VetelIdeje,
@darab :=darab as Darab,
vetelar as Vetelar,
eladasiar as EladasiAr,
@veteliertek := FLOOR(darab*vetelar) as VeteliErtek,
lezart as Lezart,
DATE(eladasdatum) as EladasDatum,
osztaleksum as OsztalekBevetel,
@eladasiar := FLOOR(eladasiar + osztaleksum - vetelikoltseg - eladasikoltseg) as EladasiAr,
@eladasiertekosztalekkal := FLOOR(darab*eladasiar + osztaleksum - vetelikoltseg - eladasikoltseg) as EladasiArOsztalekkal,
@profit := FLOOR(@eladasiertekosztalekkal - @veteliertek) as Profit,
FLOOR(darab*eladasiar - @veteliertek) as ProfitKtsgNelkul,
TRUNCATE((((@eladasiertekosztalekkal-eladasikoltseg) / ((darab*vetelar) - vetelikoltseg)) -1 )* 100,2) as ProfitSzazalek,
@napok := DATEDIFF(eladasdatum,veteldatum) as Napok,
@eveshozam := IF (Szamla LIKE '%MÁK%' OR Szamla LIKE '%LTP%',
                  TRUNCATE((POWER(((@eladasiar*@darab)/@veteliertek),365/@napok)-1)*100,2),
                  '') as EvesHozam,
IF(Szamla LIKE '%TBSZ%' OR Szamla LIKE '%LTP%',@eveshozam,TRUNCATE(@eveshozam*0.85,2)) AS NettoEvesHozam,
@nettoosszbevetel := IF(Szamla LIKE '%TBSZ%' OR Szamla LIKE '%LTP%',@profit,TRUNCATE(@profit*0.85,0)) AS NettoOsszBevetel,
IF (Szamla LIKE '%MÁK%' OR Szamla LIKE '%LTP%',TRUNCATE(@nettoosszbevetel/(@napok/365),0),'') AS Evente
FROM position
WHERE Szamla LIKE '%MÁK%' OR Szamla LIKE '%LTP%'
ORDER BY Szamla desc,VetelIdeje desc


-- NYITOTT POZICIOK
	SELECT 
	@szamla := szamla as Szamla,
	ertekpapir as Ertekpapir,
	DATE(veteldatum) as VetelIdeje,
	@darab :=darab as Darab,
	vetelar as Vetelar,
	eladasiar as EladasiArfolyam,
	@veteliertek := FLOOR(darab*vetelar) as VeteliErtek,
	lezart as Lezart,
	DATE(eladasdatum) as EladasDatum,
	osztaleksum as OsztalekBevetel,
	@eladasiar := FLOOR(eladasiar + osztaleksum - vetelikoltseg - eladasikoltseg) as EladasiAr,
	@eladasiertekosztalekkal := FLOOR(darab*eladasiar + osztaleksum - vetelikoltseg - eladasikoltseg) as EladasiArOsztalekkal,
	@profit := FLOOR(@eladasiertekosztalekkal - @veteliertek) as Profit,
	FLOOR(darab*eladasiar - @veteliertek) as ProfitKtsgNelkul,
	TRUNCATE((((@eladasiertekosztalekkal-eladasikoltseg) / ((darab*vetelar) - vetelikoltseg)) -1 )* 100,2) as ProfitSzazalek,
	@napok := DATEDIFF(eladasdatum,veteldatum) as Napok,
	@eveshozam := IF (Szamla LIKE '%MÁK%' OR Szamla LIKE '%LTP%',
	                  TRUNCATE((POWER(((@eladasiar*@darab)/@veteliertek),365/@napok)-1)*100,2),
	                  '') as EvesHozam,
	IF(Szamla LIKE '%TBSZ%' OR Szamla LIKE '%LTP%',@eveshozam,TRUNCATE(@eveshozam*0.85,2)) AS NettoEvesHozam,
	@nettoosszbevetel := IF(Szamla LIKE '%TBSZ%' OR Szamla LIKE '%LTP%',@profit,TRUNCATE(@profit*0.85,0)) AS NettoOsszBevetel,
	IF (Szamla LIKE '%MÁK%' OR Szamla LIKE '%LTP%',TRUNCATE(@nettoosszbevetel/(@napok/365),0),'') AS Evente
	FROM position
	WHERE Lezart IN ('NEM','FIX') AND Szamla NOT LIKE '%LTP%'
	ORDER BY Szamla desc, Profit, VetelIdeje desc

SELECT
@szamla := szamla as Szamla,
ertekpapir as Ertekpapir,
DATE(veteldatum) as VetelIdeje,
@darab :=darab as Darab,
vetelar as Vetelar,
eladasiar as EladasiArfolyam,
eladasiar/vetelar as ArfolyamKulonbsegSzazalek,
@veteliertek := FLOOR(darab*vetelar) as VeteliErtek,
lezart as Lezart,
DATE(eladasdatum) as EladasDatum,
osztaleksum as OsztalekBevetel,
@eladasiar := FLOOR(eladasiar + osztaleksum - vetelikoltseg - eladasikoltseg) as EladasiAr,
@eladasiertekosztalekkal := FLOOR(darab*eladasiar + osztaleksum - vetelikoltseg - eladasikoltseg) as EladasiArOsztalekkal,
@profit := FLOOR(@eladasiertekosztalekkal - @veteliertek) as Profit,
FLOOR(darab*eladasiar - @veteliertek) as ProfitKtsgNelkul,
TRUNCATE((((@eladasiertekosztalekkal-eladasikoltseg) / ((darab*vetelar) - vetelikoltseg)) -1 )* 100,2) as ProfitSzazalek,
@napok := DATEDIFF(eladasdatum,veteldatum) as Ev,
@eveshozam := IF (Szamla LIKE '%MÁK%' OR Szamla LIKE '%LTP%',TRUNCATE((POWER(((@eladasiar*@darab)/@veteliertek),365/@napok)-1)*100,2),'') as EvesHozam,
IF(Szamla LIKE '%TBSZ%' OR Szamla LIKE '%LTP%' OR (Szamla LIKE '%MÁK%' AND veteldatum >= '2019-06-01'),@eveshozam,TRUNCATE(@eveshozam*0.85,2))  AS NettoEvesHozam,
@nettoosszbevetel := IF(Szamla LIKE '%TBSZ%' OR Szamla LIKE '%LTP%' OR (Szamla LIKE '%MÁK%' AND veteldatum >= '2019-06-01'),@profit,TRUNCATE(@profit*0.85,0)) AS NettoOsszBevetel,
TRUNCATE(@nettoosszbevetel/(@napok/365),0) AS Evente
FROM position
ORDER BY Szamla, Ertekpapir, VetelIdeje

-- MÁK
SELECT
szamla as 'Szamla',
ertekpapir as 'Ertekpapir',
SUM(vetelar) AS 'VetelAr',
SUM(eladasiar) as 'Eladasi Ar',
SUM(eladasiar) - SUM(vetelar) as 'Profit',
TRUNCATE(((SUM(eladasiar) / SUM(vetelar) - 1) * 100),2) as 'Profit%',
DATE(eladasdatum) as 'Lejarat',
TRUNCATE(DATEDIFF(DATE(eladasdatum),CURDATE())/365,2) 'HatralevoEvek'
FROM position
WHERE Lezart = 'FIX'
GROUP BY Szamla, Ertekpapir, eladasdatum
ORDER BY Szamla, Ertekpapir

-- COCA COLA OSZTALÉKHOZAM
SELECT 
YEAR(timestamp) as Year,
ROUND(AVG(adjusted_close),2) as AtlagAr,
SUM(Szazalek) as EvesSzazalekosOsztalekHozam
FROM
	(SELECT timestamp,adjusted_close,dividend_amount,  ROUND((dividend_amount/adjusted_close)*100,2) as Szazalek
	FROM av_time_series_daily_adjusted_data
	JOIN av_time_series_daily_adjusted ON av_time_series_daily_adjusted.id = av_time_series_daily_adjusted_data.avtsdaId
	WHERE av_time_series_daily_adjusted.symbol = 'KO' and dividend_amount > 0
	order by timestamp) as TEMP
GROUP BY YEAR(timestamp)
ORDER BY YEAR(timestamp)


-- NYITOTT POZICIOK
SELECT
szamla,
ertekpapir,
TRUNCATE(SUM(darab*vetelar),0) as NettoVetelar,
TRUNCATE(SUM(darab*eladasiar),0) as NettoEladasiar,
TRUNCATE(SUM(darab*eladasiar) - SUM(darab*vetelar),0) as NettoArfolyamKulonbseg,
TRUNCATE(SUM(darab*eladasiar-vetelikoltseg) - SUM(darab*vetelar+eladasikoltseg),0) as BruttoArfolyamKulonbseg,
TRUNCATE(SUM(osztaleksum),0) as Osztalek,
TRUNCATE(SUM(darab*eladasiar-vetelikoltseg+osztaleksum) - SUM(darab*vetelar+eladasikoltseg),0) as BruttoArfolyamKulonbsegOsztalekkal,
TRUNCATE(SUM(darab*vetelar)/SUM(darab),2) as NettoVeteliAtlagar,
TRUNCATE(SUM(darab*eladasiar)/SUM(darab),2) as NettoEladasiAtlagar,
TRUNCATE(SUM(darab*vetelar+vetelikoltseg+eladasikoltseg)/SUM(darab),2) as NullszaldosAr,
TRUNCATE(SUM(darab*vetelar+vetelikoltseg+eladasikoltseg-osztaleksum)/SUM(darab),2) as NullszaldosArOsztalekkal
FROM
(
	SELECT
	szamla,
	ertekpapir,
	DATE(veteldatum) as veteldatum,
	darab,
	vetelar,
	eladasiar,
	DATE(eladasdatum) as eladasdatum,
	osztaleksum,
	vetelikoltseg,
	eladasikoltseg
	FROM position
	WHERE Lezart = 'NEM'
	ORDER BY szamla, ertekpapir, veteldatum
) as temp
GROUP BY szamla, ertekpapir


-- KOIN 
SET NAMES 'utf8';
SET CHARACTER SET latin1;

SELECT kategoria
FROM koin
GROUP BY kategoria


SELECT szamla, 
SUM(vetelar*darab) as VeteliErtek,
SUM(vetelar*darab-eladasikoltseg-vetelikoltseg+osztaleksum) as EladasiErtek
FROM position
WHERE lezart IN ('NEM','FIX')
GROUP BY szamla;


SELECT * FROM position WHERE lezart IN ('NEM','FIX');