using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Zene;

class ZeneProgram
{
    static void Main()
    {
        try
        {
            List<Zeneszam> zeneszamok = AdatokatBeolvas("musor.txt");
            if (zeneszamok.Count == 0)
            {
                Console.WriteLine("Az adatok beolvasása sikertelen. Az első 10 sor adatai kerülnek felhasználásra.");
                zeneszamok = MintaAdatok();
            }

            CsatornaSzamokEsDalokSzama(zeneszamok);

            TimeSpan claptonIdo = SzamolEricClaptonIdot(zeneszamok, 1);
            Console.WriteLine($"Az első csatornán az Eric Clapton számok között eltelt idő: {claptonIdo}");

            OmegaKezdetiDal(zeneszamok);

            Console.WriteLine("Kérem adja meg a felismert karaktereket:");
            string felismertKarakterek = Console.ReadLine();
            TalalatokatMentFajlba(zeneszamok, felismertKarakterek, "keres.txt");

            TimeSpan ujMusorVeg = SzamolUjMusorVegIdo(zeneszamok, 1);
            Console.WriteLine($"Az új műsorszerkezetben az adás végén: {ujMusorVeg}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hiba történt: {ex.Message}");
        }
    }
    // Mr. KálüBone bemutatkozom mi  vagyunk a nyolcasok teliberakom a mikrofont szanaszet szedem sakkmattot kaptatok verebek vagytok ti kvaratatok mondjatok meg mit akartatok
    static List<Zeneszam> AdatokatBeolvas(string fajlNev)
    {
        List<Zeneszam> zeneszamok = new List<Zeneszam>();
        try
        {
            string[] sorok = File.ReadAllLines(fajlNev);
            int zeneszamokSzama = int.Parse(sorok[0]);

            for (int i = 1; i <= zeneszamokSzama; i++)
            {
                string[] reszek = sorok[i].Split(' ');
                int csatorna = int.Parse(reszek[0]);
                int perc = int.Parse(reszek[1]);
                int masodperc = int.Parse(reszek[2]);
                string zeneszamAzonosito = reszek[3];

                Zeneszam zeneszam = new Zeneszam(csatorna, perc, masodperc, zeneszamAzonosito);
                zeneszamok.Add(zeneszam);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hiba a fájl beolvasása során: {ex.Message}");
        }

        return zeneszamok;
    }

    static List<Zeneszam> MintaAdatok()
    {
        List<Zeneszam> mintaAdatok = new List<Zeneszam>
        {
            new Zeneszam(1, 5, 3, "Deep Purple:Bad Attitude"),
            new Zeneszam(2, 3, 36, "Eric Clapton:Terraplane Blues"),
            new Zeneszam(3, 2, 46, "Eric Clapton:Crazy Country Hop"),
            new Zeneszam(3, 3, 25, "Omega:Ablakok")
            

        };

        return mintaAdatok;
    }

    static void CsatornaSzamokEsDalokSzama(List<Zeneszam> zeneszamok)
    {
        var dalokCsatornanak = zeneszamok.GroupBy(z => z.Csatorna).ToDictionary(g => g.Key, g => g.Count());
        Console.WriteLine("2. feladat:");
        foreach (var kvp in dalokCsatornanak)
        {
            Console.WriteLine($"A(z) {kvp.Key}. csatornán {kvp.Value} dal szólt.");
        }
    }

    static TimeSpan SzamolEricClaptonIdot(List<Zeneszam> zeneszamok, int csatorna)
    {
        var claptonDalok = zeneszamok.Where(z => z.Csatorna == csatorna && z.Eloado == "Eric Clapton");
        var elsoDal = claptonDalok.First();
        var utolsoDal = claptonDalok.Last();

        return utolsoDal.Vege - elsoDal.Kezdete;
    }

    static void OmegaKezdetiDal(List<Zeneszam> zeneszamok)
    {
        var omegaDal = zeneszamok.FirstOrDefault(z => z.Cim == "Omega:Legenda");
        if (omegaDal != null)
        {
            var masikDalok = zeneszamok.Where(z => z.Csatorna != omegaDal.Csatorna && z.Kezdete < omegaDal.Kezdete);

            Console.WriteLine("4. feladat:");
            Console.WriteLine($"A(z) {omegaDal.Csatorna}. csatornán a(z) Omega:Legenda dal elkezdődött.");
            Console.WriteLine($"A másik két csatornán ekkor szóltak:");
            foreach (var dal in masikDalok)
            {
                Console.WriteLine($"- {dal.Cim}");
            }
        }
        else
        {
            Console.WriteLine("4. feladat: Az Omega:Legenda dal nem található az adatok között.");
        }
    }

    static void TalalatokatMentFajlba(List<Zeneszam> zeneszamok, string felismertKarakterek, string fajlNev)
    {
        var talalatok = zeneszamok.Where(z => z.Cim.ToLower().Contains(felismertKarakterek.ToLower()))
                                .Select(z => z.Azonosito)
                                .ToList();

        try
        {
            using (StreamWriter iras = new StreamWriter(fajlNev))
            {
                iras.WriteLine(felismertKarakterek);
                foreach (var zeneszamAzonosito in talalatok)
                {
                    iras.WriteLine(zeneszamAzonosito);
                }
            }

            Console.WriteLine($"5. feladat: Az eredmények mentése a {fajlNev} fájlba sikeres.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hiba a fájl írása során: {ex.Message}");
        }
    }

    static TimeSpan SzamolUjMusorVegIdo(List<Zeneszam> zeneszamok, int csatorna)
    {
        var elsoDal = zeneszamok.Where(z => z.Csatorna == csatorna).First();
        var utolsoDal = zeneszamok.Where(z => z.Csatorna == csatorna).Last();
        TimeSpan ujMusorVegIdo = utolsoDal.Vege.Add(new TimeSpan(0, 3, 0));
        return ujMusorVegIdo;
    }
}
