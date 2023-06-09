using backgroundJob.Extensions;

namespace backgroundJob.Test.TestExtensions.TestDateTimeExtension
{
	[TestClass]
	public class TestGetNextInterval
	{
		[TestMethod]
		public void Next_interval_smaller_pivot()
		{
			var date = new DateTime(2023, 6, 8);
			var pivot = new DateTime(2023, 6, 6);
			var output = date.GetNextInterval(pivot, TimeSpan.FromDays(2));

			var expected = new DateTime(2023, 6, 10);
			Assert.AreEqual(expected, output);
		}

		[TestMethod]
		public void Next_interval_greater_pivot()
		{
			var date = new DateTime(2023, 6, 8);
			var pivot = new DateTime(2023, 6, 11);
			var output = date.GetNextInterval(pivot, TimeSpan.FromDays(2));

			var expected = new DateTime(2023, 6, 12);
			Assert.AreEqual(expected, output);
		}
	}
}
