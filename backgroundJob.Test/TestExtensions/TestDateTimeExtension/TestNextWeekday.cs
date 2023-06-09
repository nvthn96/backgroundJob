using backgroundJob.Extensions;

namespace backgroundJob.Test.TestExtensions.TestDateTimeExtension
{
	[TestClass]
	public class TestNextWeekday
	{
		[TestMethod]
		public void Next_monday()
		{
			var date = new DateTime(2023, 6, 8);
			var output = date.GetNextWeekday(DayOfWeek.Monday);

			var expected = new DateTime(2023, 6, 12);
			Assert.AreEqual(expected, output);
		}

		[TestMethod]
		public void Next_tuesday()
		{
			var date = new DateTime(2023, 6, 8);
			var output = date.GetNextWeekday(DayOfWeek.Tuesday);

			var expected = new DateTime(2023, 6, 13);
			Assert.AreEqual(expected, output);
		}

		[TestMethod]
		public void Next_weekday()
		{
			var date = new DateTime(2023, 6, 8);
			var output = date.GetNextWeekday(DayOfWeek.Friday);

			var expected = new DateTime(2023, 6, 9);
			Assert.AreEqual(expected, output);
		}
	}
}
