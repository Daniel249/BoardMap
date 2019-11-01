using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardMap.Economy
{
    class MarketPlace
    {
        // individual markets for each good
        Market[] markets;
        // last market prices
        public int[] lastPrices { get; private set; }

        // loop through and solve all markets
        public void solveMarkets() {
            for(int id = 1; id < markets.Length; id++) {
                // resolve current market and store last price for consumers next cycle
                markets[id].resolveMarket();
                lastPrices[id] = markets[id].lastPrice;
            }
        }

        // receive order
        public void processOrder(Tuple[] bundle) {
            for(int i = 0; i < bundle.Length; i++) {
                // get data from tuple
                int goodID = bundle[i].ID;
                int goodAmmount = bundle[i].Value;
                // add order to aggregateDemand of respective market
                markets[goodID].addDemand(goodAmmount);
            }
        }

        // add firm to supply in appropriate market
        public void addFirm(Firm _firm) {
            markets[_firm.productionID].Supply.Add(_firm);
        }

        // constructor
        public MarketPlace() {
            // calc number of marketable goods
            int goodsNum = Enum.GetNames(typeof(Goods)).Length;

            // init marketplace
            lastPrices = new int[goodsNum];
            markets = new Market[goodsNum];
            for(int id = 0; id < goodsNum; id++) {
                // init each market
                markets[id] = new Market(id);
                // init first prices
                lastPrices[id] = 1000;
            }
        }
    }
}
