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
        public int wage { get; private set; }

        // firm values
        // labor intensivity
        public int laborIntensity { get; private set; }
        // labor productivity
        public int laborProductivity { get; private set; }

        // reference to state location
        State stateRef;

        // make admin decisions
        public void Think() {
            int wage = worker.wageIdeal;
        }
        
        // constructor 
        public Firm(int prodID, int _workerNum, Population _worker, State _state) {
            // init
            productionID = prodID;
            stateRef = _state;

            wage = 0;
            laborIntensity = 900;
            laborProductivity = 2000;

            workerNum = _workerNum;
            workerNumIdeal = workerNum;
            worker = _worker;
            worker.hireWorkers(_workerNum);
            // add firm to countries' marketplace
            stateRef.country.marketPlace.addFirm(this);
        }
    }
}
