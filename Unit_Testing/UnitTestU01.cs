using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Frontend2.Hardware;
using System;

namespace UnitTestProject2
{
    [TestClass]
    public class UnitTestU01
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void U01_bad_configure_before_construct()
        { 
                List<VendingMachine> vendingMachines = new List<VendingMachine>();          // Create list of VMs.
                //==================================================================================================================
                //  CONFIGURE([0] "Coke", 250; "water", 250; "stuff", 205):
                List<string> popNames = new List<string>() { "Coke", "water", "stuff" };
                List<int> popCosts = new List<int>() { 250, 250, 205 };
                vendingMachines[0].Configure(popNames, popCosts);                           // Should throw ArgumentOutOfRange Exception here, otherwise fails the test.
            }

    }
   }

