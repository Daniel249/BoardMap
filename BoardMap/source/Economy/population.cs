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
        double Money;
        // current wage asked to employers
        public double wageIdeal { get; private set; }
        // absolute unemployment value
        public int unemployment { get; private set; }

        // bundle preferences
        Tuple[] preferences;

        // market reference 
        MarketPlace marketRef;

        // produce goods bundle to demand in market
        Tuple[] genBundle(double[] lastPrices) {
            // init bundle
            Tuple[] bundle = new Tuple[preferences.Length];

            // generate bundle
            // simple as it gets. half money on food and half on cotton
            double halfMoney = Money / 2;
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


            payWage(20, Size);
        }
        
        // update wage
        void updateWage() {
            // increase wage 
            // if unemployment below 5% increase wage 5% else decrease 5%
            if (unemployment == 0) {
                wageIdeal += 0.05;
            } else {
                wageIdeal -= 0.05;
            }
        }

        // hire workers from population
        public int hireWorkers(int workerNum) {
            int hiredWorkers = 0;

            // if not enough workers cap hiring
            if(unemployment < workerNum) { 
                hiredWorkers = unemployment;
                unemployment = 0;
            } else {
                // even if negative just substract 
                unemployment -= workerNum;
                hiredWorkers = workerNum;
            }
            return hiredWorkers;
        }

        public void payWage(double _wage, int workerNum) {
            // Money += _wage * workerNum;
            Money += 20 * workerNum;
            updateWage();
            // Money += _wage;
        }

        // constructor
        public Population(int _size, double _wage, State _state) {
            Size = _size;
            payWage(_wage, Size);
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