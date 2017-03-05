using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Frontend2.Hardware;
using Frontend2;
using System;


namespace UnitTestProject2
{
    [TestClass]
    public class UnitTestU03
    {
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void U03_bad_names_list()
        {
            List<VendingMachine> vendingMachines = new List<VendingMachine>();

            //==========================================================================================================
            //  CREATE(5, 10, 25, 100; 3; 10; 10; 10):
            int[] coinKinds = { 5, 10, 25, 100 };
            int selectionButtonCount = 3;
            int coinRackCapacity = 10;
            int popRackCapcity = 10;
            int receptacleCapacity = 10;
            // Create VM:
            var vm = new VendingMachine(coinKinds, selectionButtonCount, coinRackCapacity, popRackCapcity, receptacleCapacity);
            VendingMachineLogic vmLogic = new VendingMachineLogic(vm);
            //Add it to the list of VM's:
            vendingMachines.Add(vm);

            int index = 0;                                                              // Index of VM which is going to be checked.
            vm = vendingMachines[index];                                                // Reference to the vending machine at index 0.

            //==================================================================================================================
            // CONFIGURE([0] "Coke", 250; "water", 250)
            List<string> popNames = new List<string>() { "Coke", "water" };
            List<int> popCosts = new List<int>() { 250, 250};
            vm.Configure(popNames, popCosts);                                           // Should throw the Exception here
        }
    }
}
