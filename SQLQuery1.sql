﻿SELECT ksiazka.id, ksiazka.tytul, ksiazka.kategoria FROM ksiazka INNER JOIN egzemplarze ON ksiazka.id = egzemplarze.id_ksiazki WHERE egzemplarze.do_wyporzyczenia like 'tak'