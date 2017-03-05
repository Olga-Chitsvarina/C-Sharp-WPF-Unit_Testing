using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Frontend2.Hardware;
using Frontend2;

namespace UnitTestProject2
{
    [TestClass]
    public class UnitTest11
    {
        [TestMethod]
        public void T11_good_extract_before_sale_complex()
        {

            List<VendingMachine> vendingMachines = new List<VendingMachine>();          // Create list of VMs.

            //==========================================================================================================
            //  CREATE(100, 5, 25, 10; 3; 10; 10; 10):
            int[] coinKinds = { 100, 5, 25, 10 };
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
            //  CONFIGURE([0] "Coke", 250; "water", 250; "stuff", 205):
            List<string> popNames = new List<string>() { "Coke", "water", "stuff" };
            List<int> popCosts = new List<int>() { 250, 250, 205 };
            vm.Configure(popNames, popCosts);

            //===================================================================================================================
            //  COIN_LOAD([0] 1; 5, 1):
            int coinKindIndex = 1;
            List<Coin> coins = new List<Coin> { new Coin(5) };
            vm.CoinRacks[coinKindIndex].LoadCoins(coins);

            //  COIN_LOAD([0] 2; 25, 2):
            coinKindIndex = 2;
            coins = new List<Coin> { new Coin(25), new Coin(25) };
            vm.CoinRacks[coinKindIndex].LoadCoins(coins);

            //  COIN_LOAD([0] 3; 10, 1):
            coinKindIndex = 3;
            coins = new List<Coin> { new Coin(10) };
            vm.CoinRacks[coinKindIndex].LoadCoins(coins);

            //=====================================================================================================================
            //  POP_LOAD([0] 0; "Coke", 1):
            int popKindIndex = 0;
            List<PopCan> pops = new List<PopCan> { new PopCan("Coke") };
            vm.PopCanRacks[popKindIndex].LoadPops(pops);

            //  POP_LOAD([0] 1; "water", 1):
            popKindIndex = 1;
            pops = new List<PopCan> { new PopCan("water") };
            vm.PopCanRacks[popKindIndex].LoadPops(pops);

            //  POP_LOAD([0] 2; "stuff", 1):
            popKindIndex = 2;
            pops = new List<PopCan> { new PopCan("stuff") };
            vm.PopCanRacks[popKindIndex].LoadPops(pops);


            //======================================================================================================================
            //  PRESS([0] 0):
            vm.SelectionButtons[0].Press();

            //=====================================================================================================================
            //  EXTRACT([0])
            //  CHECK_DELIVERY(0):
            // Get actual list of items from the delivery chute and create expected list of items:
            var items = vm.DeliveryChute.RemoveItems();
            List<IDeliverable> itemsAsList = new List<IDeliverable>(items);
            List<IDeliverable> expectedItemsAsList = new List<IDeliverable> {};

            // Check to see if two lists are of the same length, if not, throw the exception:
            if (itemsAsList.Count == expectedItemsAsList.Count)
            {
                int i = 0;
                while (i < items.Length)
                {
                    // Compare element by element:
                    var element1 = items[i];
                    var element2 = expectedItemsAsList[i];
                    Assert.AreEqual(element1.ToString(), element2.ToString());
                    i++;
                }
            }
            // Lists have different length, they are not the same, throw the exception:
            else
            {
                throw new AssertFailedException();
            }
            //=======================================================================================================================
            //  INSERT([0] 100)
            //  INSERT([0] 100)
            //  INSERT([0] 100)
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));

            //=====================================================================================================================
            //  EXTRACT([0])
            //  CHECK_DELIVERY(0):
            // Get actual list of items from the delivery chute and create expected list of items:
            items = vm.DeliveryChute.RemoveItems();
            itemsAsList = new List<IDeliverable>(items);
            expectedItemsAsList = new List<IDeliverable> { };

            // Check to see if two lists are of the same length, if not, throw the exception:
            if (itemsAsList.Count == expectedItemsAsList.Count)
            {
                int i = 0;
                while (i < items.Length)
                {
                    // Compare element by element:
                    var element1 = items[i];
                    var element2 = expectedItemsAsList[i];
                    Assert.AreEqual(element1.ToString(), element2.ToString());
                    i++;
                }
            }
            // Lists have different length, they are not the same, throw the exception:
            else
            {
                throw new AssertFailedException();
            }

            //=====================================================================================================================
            //  UNLOAD([0])
            //  CHECK_TEARDOWN(65; 0; "water", "stuff"):
            //  Modify fields of the StoredContents obj, which corresponds to actual VM:
            var storedContents = new VendingMachineStoredContents();

            foreach (var coinRack in vm.CoinRacks)
            {
                // Add items to CoinInCoinsRacks field:
                storedContents.CoinsInCoinRacks.Add(coinRack.Unload());
            }

            // Modify PaymentCoinsInStorageBin field (add some items to it):
            storedContents.PaymentCoinsInStorageBin.AddRange(vm.StorageBin.Unload());

            foreach (var popCanRack in vm.PopCanRacks)
            {
                // Add items to PopCansInPopCanRacks field:
                storedContents.PopCansInPopCanRacks.Add(popCanRack.Unload());
            }

            // Modify fields of the StoredContents obj, based on expected values:
            VendingMachineStoredContents expectedStoredContents = new VendingMachineStoredContents();

            // Modify PaymentCoinsInStorageBin field of the expected one:
            List<Coin> expectedCoinsInStorageBin = new List<Coin>();
            foreach (var coin in expectedCoinsInStorageBin)
            {
                expectedStoredContents.PaymentCoinsInStorageBin.Add(coin);

            }

            // Create some lists in order to add them to CoinsInCoinRacks of the expected one: 
            List<Coin> expectedCoinRack0 = new List<Coin> {};
            List<Coin> expectedCoinRack1 = new List<Coin> { new Coin(5) };
            List<Coin> expectedCoinRack2 = new List<Coin> { new Coin(25), new Coin(25)};
            List<Coin> expectedCoinRack3 = new List<Coin> { new Coin(10)};
            List<List<Coin>> expectedListOfCoinRacks = new List<List<Coin>> { expectedCoinRack0, expectedCoinRack1, expectedCoinRack2, expectedCoinRack3 };
            foreach (var list in expectedListOfCoinRacks)
            {
                //  Modify CoinsInCoinRacks field by adding some items to it:
                expectedStoredContents.CoinsInCoinRacks.Add(list);
            }

            // Create some lists in order to add them to PopCansInPopCanRacks of the expected one: 
            List<PopCan> expectedPopRack0 = new List<PopCan> { new PopCan("Coke")};
            List<PopCan> expectedPopRack1 = new List<PopCan> { new PopCan("water") };
            List<PopCan> expectedPopRack2 = new List<PopCan> { new PopCan("stuff") };
            List<List<PopCan>> expectedListOfPopRacks = new List<List<PopCan>> { expectedPopRack0, expectedPopRack1, expectedPopRack2 };
            foreach (var list in expectedListOfPopRacks)
            {

                //  Modify PopCansInPopCanRacks field by adding some items to it:
                expectedStoredContents.PopCansInPopCanRacks.Add(list);
            }

            // Comparison of expected and actual VM's storage bins:
            // Check length of lists first, if different, throw the exception:
            if (storedContents.PaymentCoinsInStorageBin.Count == expectedStoredContents.PaymentCoinsInStorageBin.Count)
            {
                int i = 0;

                // Check each element of the list:
                while (i < storedContents.PaymentCoinsInStorageBin.Count)
                {
                    Assert.AreEqual(storedContents.PaymentCoinsInStorageBin[i].Value, expectedStoredContents.PaymentCoinsInStorageBin[i].Value);
                    i++;
                }
            }
            // Length is different, throw the exception:
            else
            {
                throw new AssertFailedException();
            }

            // Comparison of expected and actual VM's storage coin racks:
            // Check length of lists first, if different, throw the exception:
            if (storedContents.CoinsInCoinRacks.Count == expectedStoredContents.CoinsInCoinRacks.Count)
            {
                int i = 0;
                int j = 0;
                // Length is the same, check the inner lists:
                while (i < storedContents.CoinsInCoinRacks.Count)
                {
                    // Check the length of the innner lists, throw exception if different:
                    if (storedContents.CoinsInCoinRacks[i].Count == expectedStoredContents.CoinsInCoinRacks[i].Count)
                    {
                        // Compare the values of each elements in the inner lists:
                        while (j < storedContents.CoinsInCoinRacks[i].Count)
                        {
                            Assert.AreEqual(storedContents.CoinsInCoinRacks[i][j].Value, expectedStoredContents.CoinsInCoinRacks[i][j].Value);
                            j++;
                        }
                    }
                    // Length is different of the inner lists, throw the exception:
                    else
                    {
                        throw new AssertFailedException();
                    }
                    // Go to the next pair of inner lists:
                    j = 0;
                    i++;
                }
            }
            // Length of the outer lists is different, throw the exception:
            else
            {
                throw new AssertFailedException();
            }

            // Check to see if length of PopCansInPopCanRacks lists is the same:
            if (storedContents.PopCansInPopCanRacks.Count == expectedStoredContents.PopCansInPopCanRacks.Count)
            {
                int i = 0;
                int j = 0;
                // Length of outer lists is the same, check the inner lists:
                while (i < storedContents.PopCansInPopCanRacks.Count)
                {
                    // Make sure that the length of the inner lists is the same: 
                    if (storedContents.PopCansInPopCanRacks[i].Count == expectedStoredContents.PopCansInPopCanRacks[i].Count)
                    {
                        // Length of the inner lists is the same, compare the name of all elements in the inner lists:
                        while (j < storedContents.PopCansInPopCanRacks[i].Count)
                        {
                            Assert.AreEqual(storedContents.PopCansInPopCanRacks[i][j].Name, expectedStoredContents.PopCansInPopCanRacks[i][j].Name);
                            j++;
                        }
                    }
                    // Length of the inner lists is different, throw the exception:
                    else
                    {
                        throw new AssertFailedException();
                    }
                    j = 0;
                    i++;
                }
            }
            // Length of the outer lists is different, throw the exception:
            else
            {
                throw new AssertFailedException();
            }

            //===================================================================================================================
            //  COIN_LOAD([1] 1; 5, 1):
            coinKindIndex = 1;
            coins = new List<Coin> { new Coin(5) };
            vm.CoinRacks[coinKindIndex].LoadCoins(coins);

            //  COIN_LOAD([0] 2; 25, 2):
            coinKindIndex = 2;
            coins = new List<Coin> { new Coin(25), new Coin(25) };
            vm.CoinRacks[coinKindIndex].LoadCoins(coins);

            //  COIN_LOAD([0] 3; 10, 1):
            coinKindIndex = 3;
            coins = new List<Coin> { new Coin(10) };
            vm.CoinRacks[coinKindIndex].LoadCoins(coins);

            //=====================================================================================================================
            //  POP_LOAD([0] 0; "Coke", 1):
            popKindIndex = 0;
            pops = new List<PopCan> { new PopCan("Coke") };
            vm.PopCanRacks[popKindIndex].LoadPops(pops);

            //  POP_LOAD([0] 1; "B", 1):
            popKindIndex = 1;
            pops = new List<PopCan> { new PopCan("water") };
            vm.PopCanRacks[popKindIndex].LoadPops(pops);

            //  POP_LOAD([0] 2; "C", 1):
            popKindIndex = 2;
            pops = new List<PopCan> { new PopCan("stuff") };
            vm.PopCanRacks[popKindIndex].LoadPops(pops);



            //======================================================================================================================
            //  PRESS([0] 0):
            vm.SelectionButtons[0].Press();

            //=====================================================================================================================
            //  EXTRACT([0])
            //  CHECK_DELIVERY(50, "Coke"):
            // Get actual list of items from the delivery chute and create expected list of items:
            items = vm.DeliveryChute.RemoveItems();
            itemsAsList = new List<IDeliverable>(items);
            expectedItemsAsList = new List<IDeliverable> {new PopCan("Coke"), new Coin(25), new Coin(25) };

            // Check to see if two lists are of the same length, if not, throw the exception:
            if (itemsAsList.Count == expectedItemsAsList.Count)
            {
                int i = 0;
                while (i < items.Length)
                {
                    // Compare element by element:
                    var element1 = items[i];
                    var element2 = expectedItemsAsList[i];
                    Assert.AreEqual(element1.ToString(), element2.ToString());
                    i++;
                }
            }
            // Lists have different length, they are not the same, throw the exception:
            else
            {
                throw new AssertFailedException();
            }

            //=====================================================================================================================
            //  UNLOAD([0])
            //  CHECK_TEARDOWN(315; 0; "water", "stuff"):
            //  Modify fields of the StoredContents obj, which corresponds to actual VM:
            storedContents = new VendingMachineStoredContents();

            foreach (var coinRack in vm.CoinRacks)
            {
                // Add items to CoinInCoinsRacks field:
                storedContents.CoinsInCoinRacks.Add(coinRack.Unload());
            }

            // Modify PaymentCoinsInStorageBin field (add some items to it):
            storedContents.PaymentCoinsInStorageBin.AddRange(vm.StorageBin.Unload());

            foreach (var popCanRack in vm.PopCanRacks)
            {
                // Add items to PopCansInPopCanRacks field:
                storedContents.PopCansInPopCanRacks.Add(popCanRack.Unload());
            }

            // Modify fields of the StoredContents obj, based on expected values:
            expectedStoredContents = new VendingMachineStoredContents();

            // Modify PaymentCoinsInStorageBin field of the expected one:
            expectedCoinsInStorageBin = new List<Coin>();
            foreach (var coin in expectedCoinsInStorageBin)
            {
                expectedStoredContents.PaymentCoinsInStorageBin.Add(coin);

            }

            // Create some lists in order to add them to CoinsInCoinRacks of the expected one: 
            expectedCoinRack0 = new List<Coin> { new Coin(100), new Coin(100), new Coin(100) };
            expectedCoinRack1 = new List<Coin> { new Coin(5) };
            expectedCoinRack2 = new List<Coin> { };
            expectedCoinRack3 = new List<Coin> { new Coin(10)};
            expectedListOfCoinRacks = new List<List<Coin>> { expectedCoinRack0, expectedCoinRack1, expectedCoinRack2, expectedCoinRack3 };
            foreach (var list in expectedListOfCoinRacks)
            {
                //  Modify CoinsInCoinRacks field by adding some items to it:
                expectedStoredContents.CoinsInCoinRacks.Add(list);
            }

            // Create some lists in order to add them to PopCansInPopCanRacks of the expected one: 
            expectedPopRack0 = new List<PopCan>();
            expectedPopRack1 = new List<PopCan> { new PopCan("water") };
            expectedPopRack2 = new List<PopCan> { new PopCan("stuff") };
            expectedListOfPopRacks = new List<List<PopCan>> { expectedPopRack0, expectedPopRack1, expectedPopRack2 };
            foreach (var list in expectedListOfPopRacks)
            {

                //  Modify PopCansInPopCanRacks field by adding some items to it:
                expectedStoredContents.PopCansInPopCanRacks.Add(list);
            }

            // Comparison of expected and actual VM's storage bins:
            // Check length of lists first, if different, throw the exception:
            if (storedContents.PaymentCoinsInStorageBin.Count == expectedStoredContents.PaymentCoinsInStorageBin.Count)
            {
                int i = 0;

                // Check each element of the list:
                while (i < storedContents.PaymentCoinsInStorageBin.Count)
                {
                    Assert.AreEqual(storedContents.PaymentCoinsInStorageBin[i].Value, expectedStoredContents.PaymentCoinsInStorageBin[i].Value);
                    i++;
                }
            }
            // Length is different, throw the exception:
            else
            {
                throw new AssertFailedException();
            }

            // Comparison of expected and actual VM's storage coin racks:
            // Check length of lists first, if different, throw the exception:
            if (storedContents.CoinsInCoinRacks.Count == expectedStoredContents.CoinsInCoinRacks.Count)
            {
                int i = 0;
                int j = 0;
                // Length is the same, check the inner lists:
                while (i < storedContents.CoinsInCoinRacks.Count)
                {
                    // Check the length of the innner lists, throw exception if different:
                    if (storedContents.CoinsInCoinRacks[i].Count == expectedStoredContents.CoinsInCoinRacks[i].Count)
                    {
                        // Compare the values of each elements in the inner lists:
                        while (j < storedContents.CoinsInCoinRacks[i].Count)
                        {
                            Assert.AreEqual(storedContents.CoinsInCoinRacks[i][j].Value, expectedStoredContents.CoinsInCoinRacks[i][j].Value);
                            j++;
                        }
                    }
                    // Length is different of the inner lists, throw the exception:
                    else
                    {
                        throw new AssertFailedException();
                    }
                    // Go to the next pair of inner lists:
                    j = 0;
                    i++;
                }
            }
            // Length of the outer lists is different, throw the exception:
            else
            {
                throw new AssertFailedException();
            }

            // Check to see if length of PopCansInPopCanRacks lists is the same:
            if (storedContents.PopCansInPopCanRacks.Count == expectedStoredContents.PopCansInPopCanRacks.Count)
            {
                int i = 0;
                int j = 0;
                // Length of outer lists is the same, check the inner lists:
                while (i < storedContents.PopCansInPopCanRacks.Count)
                {
                    // Make sure that the length of the inner lists is the same: 
                    if (storedContents.PopCansInPopCanRacks[i].Count == expectedStoredContents.PopCansInPopCanRacks[i].Count)
                    {
                        // Length of the inner lists is the same, compare the name of all elements in the inner lists:
                        while (j < storedContents.PopCansInPopCanRacks[i].Count)
                        {
                            Assert.AreEqual(storedContents.PopCansInPopCanRacks[i][j].Name, expectedStoredContents.PopCansInPopCanRacks[i][j].Name);
                            j++;
                        }
                    }
                    // Length of the inner lists is different, throw the exception:
                    else
                    {
                        throw new AssertFailedException();
                    }
                    j = 0;
                    i++;
                }
            }
            // Length of the outer lists is different, throw the exception:
            else
            {
                throw new AssertFailedException();
            }

            //==========================================================================================================
            //  CREATE(100, 5, 25, 10; 3; 10; 10; 10):
            coinKinds = new int[]{ 100, 5, 25, 10};
            selectionButtonCount = 3;
            coinRackCapacity = 10;
            popRackCapcity = 10;
            receptacleCapacity = 10;
            // Create VM:
            var vm2 = new VendingMachine(coinKinds, selectionButtonCount, coinRackCapacity, popRackCapcity, receptacleCapacity);
            VendingMachineLogic vmLogic2 = new VendingMachineLogic(vm2);
            //Add it to the list of VM's:
            vendingMachines.Add(vm2);

            index = 1;                                                                  // Index of VM which is going to be checked.
            vm = vendingMachines[index];                                                // Reference to the vending machine at index 0.

            //==================================================================================================================
            //  CONFIGURE([0] "A", 5; "B", 10; "C", 25)
            popNames = new List<string>() { "Coke", "water", "stuff" };
            popCosts = new List<int>() { 250, 250, 205 };
            vm.Configure(popNames, popCosts);

            //==================================================================================================================
            //  CONFIGURE([0] "A", 5; "B", 10; "C", 25)
            popNames = new List<string>() { "A", "B", "C" };
            popCosts = new List<int>() { 5, 10, 25 };
            vm.Configure(popNames, popCosts);

            //=====================================================================================================================
            //  UNLOAD([1])
            //  CHECK_TEARDOWN(0; 0)
            //  Modify fields of the StoredContents obj, which corresponds to actual VM:
            storedContents = new VendingMachineStoredContents();

            foreach (var coinRack in vm.CoinRacks)
            {
                // Add items to CoinInCoinsRacks field:
                storedContents.CoinsInCoinRacks.Add(coinRack.Unload());
            }

            // Modify PaymentCoinsInStorageBin field (add some items to it):
            storedContents.PaymentCoinsInStorageBin.AddRange(vm.StorageBin.Unload());

            foreach (var popCanRack in vm.PopCanRacks)
            {
                // Add items to PopCansInPopCanRacks field:
                storedContents.PopCansInPopCanRacks.Add(popCanRack.Unload());
            }

            // Modify fields of the StoredContents obj, based on expected values:
            expectedStoredContents= new VendingMachineStoredContents();

            // Modify PaymentCoinsInStorageBin field of the expected one:
            expectedCoinsInStorageBin = new List<Coin>();
            foreach (var coin in expectedCoinsInStorageBin)
            {
                expectedStoredContents.PaymentCoinsInStorageBin.Add(coin);

            }

            // Create some lists in order to add them to CoinsInCoinRacks of the expected one: 
            expectedCoinRack0 = new List<Coin> { };
            expectedCoinRack1 = new List<Coin> {};
            expectedCoinRack2 = new List<Coin> {};
            expectedCoinRack3 = new List<Coin> {};
            expectedListOfCoinRacks = new List<List<Coin>> { expectedCoinRack0, expectedCoinRack1, expectedCoinRack2, expectedCoinRack3 };
            foreach (var list in expectedListOfCoinRacks)
            {
                //  Modify CoinsInCoinRacks field by adding some items to it:
                expectedStoredContents.CoinsInCoinRacks.Add(list);
            }

            // Create some lists in order to add them to PopCansInPopCanRacks of the expected one: 
            expectedPopRack0 = new List<PopCan> {};
            expectedPopRack1 = new List<PopCan> {};
            expectedPopRack2 = new List<PopCan> {};
            expectedListOfPopRacks = new List<List<PopCan>> { expectedPopRack0, expectedPopRack1, expectedPopRack2 };
            foreach (var list in expectedListOfPopRacks)
            {

                //  Modify PopCansInPopCanRacks field by adding some items to it:
                expectedStoredContents.PopCansInPopCanRacks.Add(list);
            }

            // Comparison of expected and actual VM's storage bins:
            // Check length of lists first, if different, throw the exception:
            if (storedContents.PaymentCoinsInStorageBin.Count == expectedStoredContents.PaymentCoinsInStorageBin.Count)
            {
                int i = 0;

                // Check each element of the list:
                while (i < storedContents.PaymentCoinsInStorageBin.Count)
                {
                    Assert.AreEqual(storedContents.PaymentCoinsInStorageBin[i].Value, expectedStoredContents.PaymentCoinsInStorageBin[i].Value);
                    i++;
                }
            }
            // Length is different, throw the exception:
            else
            {
                throw new AssertFailedException();
            }

            // Comparison of expected and actual VM's storage coin racks:
            // Check length of lists first, if different, throw the exception:
            if (storedContents.CoinsInCoinRacks.Count == expectedStoredContents.CoinsInCoinRacks.Count)
            {
                int i = 0;
                int j = 0;
                // Length is the same, check the inner lists:
                while (i < storedContents.CoinsInCoinRacks.Count)
                {
                    // Check the length of the innner lists, throw exception if different:
                    if (storedContents.CoinsInCoinRacks[i].Count == expectedStoredContents.CoinsInCoinRacks[i].Count)
                    {
                        // Compare the values of each elements in the inner lists:
                        while (j < storedContents.CoinsInCoinRacks[i].Count)
                        {
                            Assert.AreEqual(storedContents.CoinsInCoinRacks[i][j].Value, expectedStoredContents.CoinsInCoinRacks[i][j].Value);
                            j++;
                        }
                    }
                    // Length is different of the inner lists, throw the exception:
                    else
                    {
                        throw new AssertFailedException();
                    }
                    // Go to the next pair of inner lists:
                    j = 0;
                    i++;
                }
            }
            // Length of the outer lists is different, throw the exception:
            else
            {
                throw new AssertFailedException();
            }

            // Check to see if length of PopCansInPopCanRacks lists is the same:
            if (storedContents.PopCansInPopCanRacks.Count == expectedStoredContents.PopCansInPopCanRacks.Count)
            {
                int i = 0;
                int j = 0;
                // Length of outer lists is the same, check the inner lists:
                while (i < storedContents.PopCansInPopCanRacks.Count)
                {
                    // Make sure that the length of the inner lists is the same: 
                    if (storedContents.PopCansInPopCanRacks[i].Count == expectedStoredContents.PopCansInPopCanRacks[i].Count)
                    {
                        // Length of the inner lists is the same, compare the name of all elements in the inner lists:
                        while (j < storedContents.PopCansInPopCanRacks[i].Count)
                        {
                            Assert.AreEqual(storedContents.PopCansInPopCanRacks[i][j].Name, expectedStoredContents.PopCansInPopCanRacks[i][j].Name);
                            j++;
                        }
                    }
                    // Length of the inner lists is different, throw the exception:
                    else
                    {
                        throw new AssertFailedException();
                    }
                    j = 0;
                    i++;
                }
            }
            // Length of the outer lists is different, throw the exception:
            else
            {
                throw new AssertFailedException();
            }


            //===================================================================================================================
            //  COIN_LOAD([1] 1; 5, 1):
            coinKindIndex = 1;
            coins = new List<Coin> { new Coin(5) };
            vm.CoinRacks[coinKindIndex].LoadCoins(coins);

            //  COIN_LOAD([0] 2; 25, 2):
            coinKindIndex = 2;
            coins = new List<Coin> { new Coin(25), new Coin(25) };
            vm.CoinRacks[coinKindIndex].LoadCoins(coins);

            //  COIN_LOAD([0] 3; 10, 1):
            coinKindIndex = 3;
            coins = new List<Coin> { new Coin(10) };
            vm.CoinRacks[coinKindIndex].LoadCoins(coins);

            //=====================================================================================================================
            //  POP_LOAD([0] 0; "A", 1):
            popKindIndex = 0;
            pops = new List<PopCan> { new PopCan("A") };
            vm.PopCanRacks[popKindIndex].LoadPops(pops);

            //  POP_LOAD([0] 1; "B", 1):
            popKindIndex = 1;
            pops = new List<PopCan> { new PopCan("B") };
            vm.PopCanRacks[popKindIndex].LoadPops(pops);

            //  POP_LOAD([0] 2; "C", 1):
            popKindIndex = 2;
            pops = new List<PopCan> { new PopCan("C") };
            vm.PopCanRacks[popKindIndex].LoadPops(pops);

            //=======================================================================================================================
            //  INSERT([0] 10)
            //  INSERT([0] 5)
            //  INSERT([0] 10)
            vm.CoinSlot.AddCoin(new Coin(10));
            vm.CoinSlot.AddCoin(new Coin(5));
            vm.CoinSlot.AddCoin(new Coin(10));

            //======================================================================================================================
            //  PRESS([1] 2):
            vm.SelectionButtons[2].Press();

            //=====================================================================================================================
            //  EXTRACT([1])
            //  CHECK_DELIVERY(0, "C"):
            // Get actual list of items from the delivery chute and create expected list of items:
            items = vm.DeliveryChute.RemoveItems();
            itemsAsList = new List<IDeliverable>(items);
            expectedItemsAsList = new List<IDeliverable> { new PopCan("C") };

            // Check to see if two lists are of the same length, if not, throw the exception:
            if (itemsAsList.Count == expectedItemsAsList.Count)
            {
                int i = 0;
                while (i < items.Length)
                {
                    // Compare element by element:
                    var element1 = items[i];
                    var element2 = expectedItemsAsList[i];
                    Assert.AreEqual(element1.ToString(), element2.ToString());
                    i++;
                }
            }
            // Lists have different length, they are not the same, throw the exception:
            else
            {
                throw new AssertFailedException();
            }


            //=====================================================================================================================
            //  UNLOAD([1])
            //  CHECK_TEARDOWN(90; 0; "A", "B"):
            //  Modify fields of the StoredContents obj, which corresponds to actual VM:
            storedContents = new VendingMachineStoredContents();

            foreach (var coinRack in vm.CoinRacks)
            {
                // Add items to CoinInCoinsRacks field:
                storedContents.CoinsInCoinRacks.Add(coinRack.Unload());
            }

            // Modify PaymentCoinsInStorageBin field (add some items to it):
            storedContents.PaymentCoinsInStorageBin.AddRange(vm.StorageBin.Unload());

            foreach (var popCanRack in vm.PopCanRacks)
            {
                // Add items to PopCansInPopCanRacks field:
                storedContents.PopCansInPopCanRacks.Add(popCanRack.Unload());
            }

            // Modify fields of the StoredContents obj, based on expected values:
            expectedStoredContents = new VendingMachineStoredContents();

            // Modify PaymentCoinsInStorageBin field of the expected one:
            expectedCoinsInStorageBin = new List<Coin>();
            foreach (var coin in expectedCoinsInStorageBin)
            {
                expectedStoredContents.PaymentCoinsInStorageBin.Add(coin);

            }

            // Create some lists in order to add them to CoinsInCoinRacks of the expected one: 
            expectedCoinRack0 = new List<Coin> { };
            expectedCoinRack1 = new List<Coin> {new Coin(5), new Coin(5) };
            expectedCoinRack2 = new List<Coin> {new Coin(25), new Coin(25) };
            expectedCoinRack3 = new List<Coin> {new Coin(10), new Coin(10), new Coin(10) };
            expectedListOfCoinRacks = new List<List<Coin>> { expectedCoinRack0, expectedCoinRack1, expectedCoinRack2, expectedCoinRack3 };
            foreach (var list in expectedListOfCoinRacks)
            {
                //  Modify CoinsInCoinRacks field by adding some items to it:
                expectedStoredContents.CoinsInCoinRacks.Add(list);
            }

            // Create some lists in order to add them to PopCansInPopCanRacks of the expected one: 
            expectedPopRack0 = new List<PopCan> {new PopCan("A") };
            expectedPopRack1 = new List<PopCan> { new PopCan("B")};
            expectedPopRack2 = new List<PopCan> { };
            expectedListOfPopRacks = new List<List<PopCan>> { expectedPopRack0, expectedPopRack1, expectedPopRack2 };
            foreach (var list in expectedListOfPopRacks)
            {

                //  Modify PopCansInPopCanRacks field by adding some items to it:
                expectedStoredContents.PopCansInPopCanRacks.Add(list);
            }

            // Comparison of expected and actual VM's storage bins:
            // Check length of lists first, if different, throw the exception:
            if (storedContents.PaymentCoinsInStorageBin.Count == expectedStoredContents.PaymentCoinsInStorageBin.Count)
            {
                int i = 0;

                // Check each element of the list:
                while (i < storedContents.PaymentCoinsInStorageBin.Count)
                {
                    Assert.AreEqual(storedContents.PaymentCoinsInStorageBin[i].Value, expectedStoredContents.PaymentCoinsInStorageBin[i].Value);
                    i++;
                }
            }
            // Length is different, throw the exception:
            else
            {
                throw new AssertFailedException();
            }

            // Comparison of expected and actual VM's storage coin racks:
            // Check length of lists first, if different, throw the exception:
            if (storedContents.CoinsInCoinRacks.Count == expectedStoredContents.CoinsInCoinRacks.Count)
            {
                int i = 0;
                int j = 0;
                // Length is the same, check the inner lists:
                while (i < storedContents.CoinsInCoinRacks.Count)
                {
                    // Check the length of the innner lists, throw exception if different:
                    if (storedContents.CoinsInCoinRacks[i].Count == expectedStoredContents.CoinsInCoinRacks[i].Count)
                    {
                        // Compare the values of each elements in the inner lists:
                        while (j < storedContents.CoinsInCoinRacks[i].Count)
                        {
                            Assert.AreEqual(storedContents.CoinsInCoinRacks[i][j].Value, expectedStoredContents.CoinsInCoinRacks[i][j].Value);
                            j++;
                        }
                    }
                    // Length is different of the inner lists, throw the exception:
                    else
                    {
                        throw new AssertFailedException();
                    }
                    // Go to the next pair of inner lists:
                    j = 0;
                    i++;
                }
            }
            // Length of the outer lists is different, throw the exception:
            else
            {
                throw new AssertFailedException();
            }

            // Check to see if length of PopCansInPopCanRacks lists is the same:
            if (storedContents.PopCansInPopCanRacks.Count == expectedStoredContents.PopCansInPopCanRacks.Count)
            {
                int i = 0;
                int j = 0;
                // Length of outer lists is the same, check the inner lists:
                while (i < storedContents.PopCansInPopCanRacks.Count)
                {
                    // Make sure that the length of the inner lists is the same: 
                    if (storedContents.PopCansInPopCanRacks[i].Count == expectedStoredContents.PopCansInPopCanRacks[i].Count)
                    {
                        // Length of the inner lists is the same, compare the name of all elements in the inner lists:
                        while (j < storedContents.PopCansInPopCanRacks[i].Count)
                        {
                            Assert.AreEqual(storedContents.PopCansInPopCanRacks[i][j].Name, expectedStoredContents.PopCansInPopCanRacks[i][j].Name);
                            j++;
                        }
                    }
                    // Length of the inner lists is different, throw the exception:
                    else
                    {
                        throw new AssertFailedException();
                    }
                    j = 0;
                    i++;
                }
            }
            // Length of the outer lists is different, throw the exception:
            else
            {
                throw new AssertFailedException();
            }


        }





    }
}











