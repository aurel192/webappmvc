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
                            WHERE Lezart LIKE '%IGEN%'
                            ORDER BY Szamla, Ertekpapir, VetelIdeje