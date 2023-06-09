using backgroundJob.Extensions;

namespace backgroundJob.Test.TestExtensions.TestDateTimeExtension
{
	[TestClass]
	public class TestNextYearday_datetime
	{
		[TestMethod]
		public void Next_day()
		{
			var date = new DateTime(2023, 6, 8);
			var originalDate = new DateTime(2020, 6, 13);
			var output = date.GetNextYearday(originalDate);

			var expected = new DateTime(2023, 6, 13);
			Assert.AreEqual(expected, output);
		}

		[TestMethod]
		public void Next_yearday()
		{
			var date = new DateTime(2023, 6, 8);
			var originalDate = new DateTime(2020, 4, 10);
			var output = date.GetNextYearday(originalDate);

			var expected = new DateTime(2024, 4, 10);
			Assert.AreEqual(expected, output);
		}
	}
}
