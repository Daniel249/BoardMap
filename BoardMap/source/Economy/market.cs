using BoardMap.LandscapeNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardMap.Economy
{
    class Market
    {
        // ID of good
        int ID;
        // total subscribed demmand. 1/1000
        long aggDemand;
        // list of suppliers
        public List<Firm> Supply { get; private set; }

        // price from last tick for pops' bundle calculations
        public int lastPrice { get; private set; }
        
        // main market method
        public void resolveMarket() {
            // firms think
            for(int i = 0; i < Supply.Count; i++) {
                Supply[i].Think();
            }
            // add all firms' supply

            // find spot

            // pay wages

            // reset aggregate demand for next cycle
            aggDemand = 0;
        }

        // receive order from marketplace
        public void addDemand(int demandValue) {
            aggDemand += demandValue;
        }


        // constructor
        public Market(int _id) {
            aggDemand = 0;
            ID = _id;
            lastPrice = 1000;
            // init suppliers list
            Supply = new List<Firm>();
        }
    }
}
