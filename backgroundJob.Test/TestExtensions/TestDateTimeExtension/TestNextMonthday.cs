using backgroundJob.Extensions;

namespace backgroundJob.Test.TestExtensions.TestDateTimeExtension
{
	[TestClass]
	public class TestNextMonthday
	{
		[TestMethod]
		public void Next_day()
		{
			var date = new DateTime(2023, 6, 8);
			var output = date.GetNextMonthday(14);

			var expected = new DateTime(2023, 6, 14);
			Assert.AreEqual(expected, output);
		}

		[TestMethod]
		public void Next_monthday()
		{
			var date = new DateTime(2023, 6, 8);
			var output = date.GetNextMonthday(4);

			var expected = new DateTime(2023, 7, 4);
			Assert.AreEqual(expected, output);
		}
	}
}
