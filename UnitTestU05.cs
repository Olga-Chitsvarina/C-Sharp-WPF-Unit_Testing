using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Frontend2.Hardware;
using System;

namespace UnitTestProject2
{
    [TestClass]
    public class UnitTestU05
    {
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void U05_bad_coin_kind()

        {
            List<VendingMachine> vendingMachines = new List<VendingMachine>();
            //==========================================================================================================
            //  CREATE(0; 1; 10;10;10):
            int[] coinKinds = { 0 };
            int selectionButtonCount = 1;
            int coinRackCapacity = 10;
            int popRackCapcity = 10;
            int receptacleCapacity = 10;
            // Should throw the Exception here:
            var vm = new VendingMachine(coinKinds, selectionButtonCount, coinRackCapacity, popRackCapcity, receptacleCapacity);
           
        }

        }
    }


