using System;

namespace LW3
{
    class Program
    {
        static void Main(string[] args)
        {
            //Pharmacy pharmacy = new Pharmacy();
            Cure cure = new Cure("\n", 3, 250);
            Console.WriteLine(cure.CureToString());

            /*Cure cure1 = new Cure("Nurofen", 1, 50);
            Console.WriteLine(cure1.CureToString());
            cure.Fill(pharmacy);
            Console.WriteLine(cure.IsFilled);

            /*Pharmacy pharmacy = new Pharmacy();
            pharmacy.AddToDB(cure.CureName, cure.Quantity);
            foreach (var medicine in pharmacy.DataBase)
            {
                Console.WriteLine(cure.ResultToString());
            }
            pharmacy.RemoveFromDB(cure.CureName, 15);
            foreach (var medicine in pharmacy.DataBase)
            {
                Console.WriteLine($"{medicine.Key}: {medicine.Value}");
            }
            pharmacy.RemoveFromDB(cure.CureName, 40);
            foreach (var medicine in pharmacy.DataBase)
            {
                Console.WriteLine($"{medicine.Key}: {medicine.Value}");
            }
            pharmacy.RemoveFromDB("Abc", 40);
            foreach (var medicine in pharmacy.DataBase)
            {
                Console.WriteLine($"{medicine.Key}: {medicine.Value}");
            }*/
        }
    }
}
