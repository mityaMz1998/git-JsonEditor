using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace JsonBuilder
{
    public class Person
    {
        public int ID { get; set; }
        public string FIO { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool Male { get; set; }
        public string MaritalStatus { get; set; }
        public int? CountOfChildren { get; set; }
        public string ConvertMale 
        {
            get { return Male ? "M" : "F";}
        }
        public string ConvertCountOfChildren 
        { 
            get 
            { 
                return CountOfChildren == null ? "Is unknown" : CountOfChildren.ToString(); 
            }
        }
        public List<Card> ListCard { get; set; }
        public Person(){ }
        public Person(int id, string fio, DateTime dob, bool male, string marStat, int? cntChild, List<Card> listCard)
        {
            ID = id;
            FIO = fio;
            DateOfBirth = dob;
            Male = male;
            MaritalStatus = marStat;
            CountOfChildren = cntChild;
            ListCard = listCard;
        }
    }
}
