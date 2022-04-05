using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace JsonBuilder
{
    public class Card
    {
        public long NumberCard { get; set; }
        public string CardExpirationDate { get; set; }
        public string ConvertNumberCard
        {
            get
            {
                return String.Format("{0:0000 0000 0000 0000}", NumberCard);
            }
        }
        public Card() { }
        public Card(long numberOfCard, string cardExpirationDate)
        {
            NumberCard = numberOfCard;
            CardExpirationDate = cardExpirationDate;
        }
    }
}
