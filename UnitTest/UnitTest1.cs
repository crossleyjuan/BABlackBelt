using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServerLib;
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestCommandsLoad()
        {
            Dictionary<string, Command> commands = AsyncController.Commands;

            Assert.AreEqual(2, commands.Count);
        }
    }
}
