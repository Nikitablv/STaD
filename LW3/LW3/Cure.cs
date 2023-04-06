using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LW3
{
    public class Cure
    {
        public Cure(string cureName, int quantity, int price)
        {
            CureName = cureName;
            Quantity = quantity;
            Price = price;
        }
        public bool IsFilled { get; private set; }
        public string CureName
        {
            get
            {
                return cureName_;
            }

            set => cureName_ = (value.Length >= cureNameMax) ? "Unknown" : value;
        }
        public int Quantity 
        { 
            get
            {
                return quantity_;
            }

            set
            {
                if (value > quantityMax)
                    quantity_ = quantityMax;
                else
                    if (value < quantityMin)
                        quantity_ = quantityMin;
                    else
                        quantity_ = value;
            }
        }
        public int Price
        {
            get
            {
                return price_;
            }

            set
            {
                if (value > priceMax)
                    price_ = priceMax;
                else
                    if (value < priceMin)
                        price_ = priceMin;
                    else
                        price_ = value;
            }
        }
        public int TotalPrice()
        {
            return Quantity * Price;
        }
        public int DiscondCard()
        {
            return TotalPrice() - TotalPrice() * discond_ / 100;
        }
        public void Fill(IPharmacy pharmacy)
        {
            if (pharmacy.HasInventory(CureName, Quantity))
            {
                pharmacy.RemoveFromDB(CureName, Quantity);
                IsFilled = true;
            }
        }
        public void Remove(IPharmacy pharmacy)
        {
            pharmacy.AddToDB(CureName, Quantity);
            IsFilled = false;
        }
        public string CureToString()
        {
            return $"{CureName}:\n" +
                $"  Quantity: {Quantity}\n" +
                $"  Price per one: {Price}\n" +
                $"  Total price: {TotalPrice()}\n" +
                $"  With discond: {DiscondCard()}\n";
        }

        private int quantity_;
        private string cureName_;
        private int price_;
        private int discond_ = 10;
        private const int quantityMax = 1000;
        private const int quantityMin = 0;
        private const int priceMax = 10000;
        private const int priceMin = 0;
        private const int cureNameMax = 30;
    }
}
