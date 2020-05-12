﻿using System;
using System.Threading;

namespace TIC_TAC_TOE_NET_FRAMEWORK
{
    class Program
    {
        static void Main(string[] args)
        {
            GraczLudzki gA = new GraczLudzki();
            GraczKomputerowy gB = new GraczKomputerowy();
            gA.Imie = "Player";
            gB.Imie = "SI";
            gA.Znak = 'X';
            gB.Znak = '0';

            char[,] plansza = new char[3, 3] {
                { '1', '2', '3' },
                { '4', '5', '6' },
                { '7', '8', '9' }
            };

            char[,] planszaKopia = plansza.Clone() as char[,];

            // Petla na kolejne ruchy graczy
            bool koniecGry = false;
            bool ruchGraczaA = true;

            for (int runda = 0; runda < plansza.Length; ++runda)
            {
                Console.Clear();
                RysujPlansze(plansza);

                if (ruchGraczaA)
                {
                    Console.WriteLine("Movement performs: " + gA.Imie + ".");
                    koniecGry = gA.WykonajRuch(plansza, planszaKopia);
                    ruchGraczaA = false;
                }
                else
                {
                    Console.WriteLine("Movement performs: " + gB.Imie + ".");
                    koniecGry = gB.WykonajRuch(plansza, planszaKopia);
                    ruchGraczaA = true;
                }
                if (koniecGry)
                    break;
            }

            // End Game

            Console.Clear();
            RysujPlansze(plansza);
            Console.Write("End Game! ");
            if (koniecGry)
            {
                Console.Write("Won ");
                if (ruchGraczaA)
                    Console.WriteLine(gB.Imie + ".");
                else
                    Console.WriteLine(gA.Imie + ".");
            }
            else
                Console.WriteLine("Draw.");
            Console.ReadKey();
        }
        /////////////////////////////////////////////////////////
        static void RysujPlansze(char[,] plansza)
        {
            int wysokosc = plansza.GetLength(0);
            int szerokosc = plansza.GetLength(1);

            for (int i = 0; i < wysokosc; ++i)
            {
                for (int j = 0; j < szerokosc; ++j) 
                    Console.Write(plansza[i, j]);
                Console.WriteLine();
            }
        }
    }
    /////////////////////////////////////////////////////////
    interface IRuch
    {
        bool WykonajRuch(char[,] plansza, char[,] planszaKopia);
    }
    abstract class Gracz
    {
        public string Imie { get; set; }
        public char Znak { get; set; }

        public bool SprawdzCzyKoniecGry(char[,] plansza)
        {
            int wysokosc = plansza.GetLength(0);
            int szerokosc = plansza.GetLength(1);
            if (szerokosc != wysokosc)
                throw new Exception("The board is not a square!");

            // Check the lines
            for (int i = 0; i < wysokosc; ++i)
            {
                int sumaWiersza = 0;
                for (int j = 0; j < szerokosc; ++j)
                {
                    if (plansza[i, j] == Znak)
                        ++sumaWiersza;
                }
                if (sumaWiersza == szerokosc)
                    return true;
            }

            // Check  column
            for (int j = 0; j < szerokosc; ++j)
            {
                int sumaKolumny = 0;
                for (int i = 0; i < wysokosc; ++i)
                {
                    if (plansza[i, j] == Znak)
                        ++sumaKolumny;
                }
                if (sumaKolumny == wysokosc)
                    return true;
            }
            // Check diagonals
            int sumaPrzekatnejA = 0;
            int sumaPrzekatnejB = 0;
            for (int k = 0; k < szerokosc; ++k)
            {
                if (plansza[k, k] == Znak)
                    ++sumaPrzekatnejA;
                if (plansza[k, szerokosc - 1 - k] == Znak)
                    ++sumaPrzekatnejB;
            }
            if (sumaPrzekatnejA == szerokosc || sumaPrzekatnejB == szerokosc)
                return true;
            // Zadna z opcji, czyli jeszcze nie koniec gry
            return false;
        }
        public bool UmiescZnak(char c, char[,] plansza, char[,] planszaKopia)
        {
            int wysokosc = plansza.GetLength(0);
            int szerokosc = plansza.GetLength(1);
            if (wysokosc != plansza.GetLength(0) || szerokosc != plansza.GetLength(1))
                throw new Exception("The board is not a square!");
            for (int i = 0; i < wysokosc; ++i)
            {
                for (int j = 0; j < szerokosc; ++j)
                {
                    if ((plansza[i, j] == c) && (plansza[i, j] == planszaKopia[i, j]))
                    {
                        plansza[i, j] = Znak;
                        return true;
                    }
                }
            }
            return false;
        }
    }
    class GraczLudzki : Gracz, IRuch
    {
        public bool WykonajRuch(char[,] plansza, char[,] planszaKopia)
        {
            char wybranePole;
            do
            {
                Console.Write("Select empty slot: ");
                wybranePole = Console.ReadKey().KeyChar;
                Console.WriteLine();
            }
            while (!UmiescZnak(wybranePole, plansza, planszaKopia));

            return SprawdzCzyKoniecGry(plansza);
        }
    }
    class GraczKomputerowy : Gracz, IRuch
    {
        public bool WykonajRuch(char[,] plansza, char[,] planszaKopia)
        {
            Random rnd = new Random();
            char wybranePole;
            do
            {
                int m = rnd.Next(1, plansza.Length + 1);
                wybranePole = m.ToString()[0];
            }
            while (!UmiescZnak(wybranePole, plansza, planszaKopia));
            Thread.Sleep(2000);

            return SprawdzCzyKoniecGry(plansza);
        }
    }
}
