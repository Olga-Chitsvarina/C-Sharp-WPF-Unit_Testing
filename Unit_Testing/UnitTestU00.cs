using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Frontend2.Hardware;
using Frontend2;
using System;

namespace UnitTestProject2
{
    [TestClass]
    public class UnitTestMyTestU01
    {
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void MyTestU01_emty_string_name()
        {
            List<VendingMachine> vendingMachines = new List<VendingMachine>();

            //==========================================================================================================
            //  CREATE(5, 10, 25, 100; 3; 10; 10; 10):
            int[] coinKinds = { 5, 10, 25, 100 };
            int selectionButtonCount = 1;
            int coinRackCapacity = 10;
            int popRackCapcity = 10;
            int receptacleCapacity = 10;
            // Create VM:
            var vm = new VendingMachine(coinKinds, selectionButtonCount, coinRackCapacity, popRackCapcity, receptacleCapacity);
            VendingMachineLogic vmLogic = new VendingMachineLogic(vm);
            //Add it to the list of VM's:
            vendingMachines.Add(vm);

            int index = 0;                                                              // Index of VM which is going to be checked.
            vm = vendingMachines[index];

            //==================================================================================================================
            //  CONFIGURE([0] "Coke", 250; "water", 250; "stuff", 205):
            List<string> popNames = new List<string>() { ""};
            List<int> popCosts = new List<int>() { 250 };
            vm.Configure(popNames, popCosts);                       // Should throw the exception because of empty string pop name.
        }
       
    }
}
