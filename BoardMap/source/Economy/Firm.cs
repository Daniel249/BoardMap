using BoardMap.LandscapeNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardMap.Economy
{
    class Firm
    {
        // good id
        public int productionID { get; private set; }

        // reference to workers
        Population worker;
        // number of workers 
        public int workerNum { get; private set; }
        // ideal number of workers
        public int workerNumIdeal { get; private set; }
        // wage of worker pool
        public double wage { get; private set; }

        // firm values
        // labor intensivity
        public double laborIntensity { get; private set; }
        // labor productivity
        public double laborProductivity { get; private set; }

        // last production for ui
        public double production { get; private set; }

        // reference to state location
        public State stateRef { get; private set; }

        // make admin decisions
        public void Think() {
            // pay old wage
            // worker.payWage(wage, workerNum);
            // update wage
            wage = worker.wageIdeal;
            // hire workers 
            workerNum += worker.hireWorkers(workerNumIdeal - workerNum);
        }

        // return production and report value added
        public double operate(double price) {
            // calc mc*Ai*ai / wi
            double klammern = laborProductivity * laborIntensity * price / wage;
            // calc 1/(1 - ai)
            double potenz = 1 / (1 - laborIntensity);

            // calc labor first and update workerideal
            double labor = Math.Pow(klammern, potenz);
            workerNumIdeal = (int)labor;
            // calc production
            production = laborProductivity * Math.Pow(labor, laborIntensity);

            double revenue = production * price;
            stateRef.grossProduct += (int)revenue;

            return revenue;
        }

        // constructor 
        public Firm(int prodID, int _workerNum, State _state) {
            // init
            productionID = prodID;
            stateRef = _state;
            stateRef.stateEconomy.Add(this);

            wage = 0;
            laborIntensity = 0.7;
            // if good is food. more tiles = more productivity
            if (prodID == 1) {
                laborProductivity = 50 * _state.tiles.Length;
            } else {
                laborProductivity = 50;
            }

            workerNum = _workerNum;
            workerNumIdeal = workerNum;
            worker = _state.population;
            worker.hireWorkers(_workerNum);
            // add firm to countries' marketplace
            stateRef.country.marketPlace.addFirm(this);
        }
    }
}
