namespace backgroundJob.Extensions
{
	public static class DateTimeExtension
	{
		public static DateTime GetNextInterval(this DateTime dateTime, DateTime pivot, TimeSpan interval)
		{
			var isPivotGreater = pivot > dateTime;
			if (isPivotGreater == false) return dateTime.Add(interval);

			var times = Math.Ceiling((pivot - dateTime) / interval);
			var output = dateTime.Add(times * interval);

			return output;
		}

		public static DateTime GetNextWeekday(this DateTime dateTime, DayOfWeek dayOfWeek)
		{
			var nextDayOfWeekCount = (int)dayOfWeek + 7;
			var duration = (nextDayOfWeekCount - (int)dateTime.DayOfWeek) % 7;
			if (duration == 0) duration = 7;

			var output = dateTime.AddDays(duration);
			return output;
		}

		public static DateTime GetNextMonthday(this DateTime dateTime, int dayOfMonth)
		{
			var isNextMonth = dateTime.Day > dayOfMonth;

			var year = dateTime.Year;
			var month = isNextMonth ? dateTime.Month + 1 : dateTime.Month;
			var day = dayOfMonth;

			var output = new DateTime(year, month, day);
			return output;
		}

		public static DateTime GetNextYearday(this DateTime dateTime, int dayOfYear)
		{
			var isNextYear = dateTime.DayOfYear > dayOfYear;

			var year = isNextYear ? dateTime.Year + 1 : dateTime.Year;
			var output = new DateTime(year, 1, 1).AddDays(dayOfYear - 1);
			return output;
		}

		public static DateTime GetNextYearday(this DateTime dateTime, DateTime date)
		{
			var isNextYear = (dateTime.Month > date.Month)
				|| (dateTime.Month == date.Month && dateTime.Day > date.Day);

			var year = isNextYear ? dateTime.Year + 1 : dateTime.Year;
			var output = new DateTime(year, date.Month, date.Day);
			return output;
		}
	}
}
