using BoardMap.LandscapeNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardMap.Economy
{
    class Population
    {
        // total population size
        public int Size { get; private set; }
        // cash in hand
        int Money;
        // current wage asked to employers
        public int wageIdeal { get; private set; }
        // absolute unemployment value
        int unemployment;

        // bundle preferences
        Tuple[] preferences;

        // market reference 
        MarketPlace marketRef;

        // produce goods bundle to demand in market
        Tuple[] genBundle(int[] lastPrices) {
            // init bundle
            Tuple[] bundle = new Tuple[preferences.Length];

            // generate bundle
            // simple as it gets. half money on food and half on cotton
            int halfMoney = Money / 2;
            bundle[0] = new Tuple(1, halfMoney);
            bundle[1] = new Tuple(2, halfMoney);
            // substract from money
            Money -= halfMoney;
            Money -= halfMoney;

            return bundle;
        }

        // main demand method
        public void makeOrder() {
            // generate bundle
            Tuple[] bundle = genBundle(marketRef.lastPrices);
            // make order to marketplace
            marketRef.processOrder(bundle);

            // increase wage 
            // if unemployment below 5% increase wage 5% else decrease 5%
            if(unemployment < unemployment * 5 / 100) {
                wageIdeal += wageIdeal * 5 / 100;
            } else {
                wageIdeal -= wageIdeal * 5 / 100;
            }
        }

        // hire workers from population
        public int hireWorkers(int workerNum) {
            int hiredWorkers = 0;
            if(unemployment > workerNum) {
                unemployment -= workerNum;
                hiredWorkers = workerNum;
            } else {
                hiredWorkers = unemployment;
            }
            return hiredWorkers;
        }

        // constructor
        public Population(int _size, int _money, int _wage, State _state) {
            Size = _size;
            Money = _money;
            wageIdeal = _wage;
            unemployment = _size;
            marketRef = _state.country.marketPlace;

            // temporary arbitrary goods preferences
            // same unity preference for food and clothing
            preferences = new Tuple[]
            {
                new Tuple(1, 1),
                new Tuple(2, 1)
            };
        }
    }
}