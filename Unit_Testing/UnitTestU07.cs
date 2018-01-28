using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Frontend2.Hardware;
using System;

namespace UnitTestProject2
{
    [TestClass]
    public class UnitTestU07
    {
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void U07_bad_button_number_2()
        {
            List<VendingMachine> vendingMachines = new List<VendingMachine>();          // Create list of VMs.

            //==========================================================================================================
            //  CREATE(5, 10, 25; 100;3):
            int[] coinKinds = { 5, 10, 25, 100 };
            int selectionButtonCount = 3;
            int coinRackCapacity = 0;
            int popRackCapcity = 0;
            int receptacleCapacity = 0;
            // Create VM:
            var vm = new VendingMachine(coinKinds, selectionButtonCount, coinRackCapacity, popRackCapcity, receptacleCapacity);
            VendingMachineLogic vmLogic = new VendingMachineLogic(vm);
            //Add it to the list of VM's:
            vendingMachines.Add(vm);

            int index = 0;                                                              // Index of VM which is going to be checked.
            vm = vendingMachines[index];                                                // Reference to the vending machine at index 0.

            //======================================================================================================================
            //  PRESS([0] 0):
          
                vm.SelectionButtons[-1].Press();                                            // Should throw the exception here
            }
            }

    }

