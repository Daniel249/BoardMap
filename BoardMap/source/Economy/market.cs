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
        double aggDemand;
        // list of suppliers
        public List<Firm> Supply { get; private set; }

        // price from last tick for pops' bundle calculations
        public double lastPrice { get; private set; }
        
        // main market method
        public void resolveMarket() {
            // firms think
            for(int i = 0; i < Supply.Count; i++) {
                Supply[i].Think();
            }

            // calc all firms' supply
            double[] production = new double[Supply.Count];
            double totalProduction = 0;

            // calc new price
            double newPrice = calcPrice();
            // calc productions
            totalProduction = hypoPrice(newPrice, production);

            // add production to state gdp
            for (int i = 0; i < Supply.Count; i++) {
                Supply[i].operate(newPrice);
            }

            // find spot

            // pay wages

            // reset aggregate demand for next cycle
            aggDemand = 0;
            lastPrice = newPrice;
        }

        // calc with hypothetical price. store in production array and return total production
        double hypoPrice(double price, double[] production) {
            // init return
            double totalProduction = 0;
            // loop through supply firms
            for(int i = 0; i < Supply.Count; i++) {
                // calc firm production and add to total
                totalProduction += Supply[i].operate(price);
            }
            // return total
            return totalProduction;
        }

        double calcPrice() {
            double suma = 0;
            // loop through supply firms
            for (int i = 0; i < Supply.Count; i++) {
                Firm firm = Supply[i];
                // calc
                double klammern = firm.laborProductivity * firm.laborIntensity / firm.wage;
                double potenz = firm.laborIntensity / (1 - firm.laborIntensity);

                // calc and add 
                double temp = Math.Pow(klammern, potenz);
                double hold = firm.laborProductivity * temp;
                suma += hold;
            }
            // use revenue

            // return total
            double potenzz = 1f - Supply[0].laborIntensity;
            return Math.Pow( aggDemand / suma, potenzz);
        }

        // receive order from marketplace
        public void addDemand(double demandValue) {
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
