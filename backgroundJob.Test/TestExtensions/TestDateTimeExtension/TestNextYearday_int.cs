using backgroundJob.Extensions;

namespace backgroundJob.Test.TestExtensions.TestDateTimeExtension
{
	[TestClass]
	public class TestNextYearday_int
	{
		[TestMethod]
		public void Next_day()
		{
			var date = new DateTime(2023, 6, 8);
			var output = date.GetNextYearday(164);

			var expected = new DateTime(2023, 6, 13);
			Assert.AreEqual(expected, output);
		}

		[TestMethod]
		public void Next_yearday()
		{
			var date = new DateTime(2023, 6, 8);
			var output = date.GetNextYearday(4);

			var expected = new DateTime(2024, 1, 4);
			Assert.AreEqual(expected, output);
		}
	}
}
