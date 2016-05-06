using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alsvior.Utility.Test
{
    [TestClass]
    public class TimeConversionTests
    {
      
        [TestMethod]
        public void CanStripHours()
        {
            //Arrange
            var dayStart = TimeConversion.ConvertToTimestamp(DateTime.SpecifyKind(DateTime.Now.Date, DateTimeKind.Utc));
            var testTimestamp = dayStart + 12345;

            //Act
            var testStripHours = TimeConversion.StripHours(testTimestamp);

            //Assert
            Assert.AreEqual(dayStart, testStripHours);
        }

        [TestMethod]
        public void CanGetUTCDayStartFromDateTime()
        {
            //Arrange
            var now = DateTime.Now;
            var utcDayStart = TimeConversion.ConvertToTimestamp(DateTime.SpecifyKind(now.Date, DateTimeKind.Utc));
            var localDayStart = TimeConversion.ConvertToTimestamp(now.Date);         
        }
    }
}
