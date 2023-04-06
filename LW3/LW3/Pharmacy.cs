using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LW3
{
    public interface IPharmacy
    {
        /*public Pharmacy() { }
        public Dictionary<string, int> DataBase
        {
            get
            {
                return dataBase;
            }
        }*/

        bool HasInventory(string productName, int quantity);
        /*{
            if (dataBase.ContainsKey(productName))
            {
                return dataBase[productName] >= quantity;
            }
            return false;
        }*/
        void AddToDB(string productName, int quantity);
        /*{
            if (!dataBase.ContainsKey(productName))
            {
                dataBase.Add(productName, quantity);
            }
            else
            {
                dataBase[productName] += quantity;
            }

        }*/
        void RemoveFromDB(string productName, int quantity);
        /*{
            dataBase[productName] -= quantity;
        }*/

        //private Dictionary<string, int> dataBase = new Dictionary<string, int>();
    }
}
