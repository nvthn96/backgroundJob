using backgroundJob.Extensions;
using backgroundJob.Infrastructure.Option;
using backgroundJob.Infrastructure.Service;

namespace backgroundJob.Infrastructure.Monitor
{
	public class TimedItem
	{
		public DateTime NextTimeRunning { get; set; }
		public IScopedService Service { get; set; }

		public TimedItem(IScopedService service)
		{
			Service = service;
			SetNextTimeStart();
		}

		public TimedItem(IScopedService service, ServiceOption options)
		{
			Service = service;
			Service.Option = options;
			SetNextTimeStart();
		}

		private void SetNextTimeStart()
		{
			NextTimeRunning = Service.Option.Repeat.IsEnum(ServiceRepeat.None)
				? DateTime.UtcNow
				: Service.Option.Timed!.StartTime;
		}

		public void SetNextTimeRunning()
		{
			var currentTime = DateTime.UtcNow;
			var option = Service.Option;

			switch (option.Repeat)
			{
				case ServiceRepeat.None:
					NextTimeRunning = currentTime;
					break;

				case ServiceRepeat.Second:
				case ServiceRepeat.Minute:
				case ServiceRepeat.Hour:
				case ServiceRepeat.Day:
					NextTimeRunning = NextTimeRunning.GetNextInterval(currentTime, option.Timed!.Interval);
					break;

				case ServiceRepeat.Week:
					{
						var dayOfWeek = NextTimeRunning.DayOfWeek;
						var listDayOfWeek = option.Timed!.DaysOfWeek;
						if (listDayOfWeek.Count == 1)
						{
							NextTimeRunning = NextTimeRunning.GetNextWeekday(dayOfWeek);
						}
						else
						{
							var nextIndex = (listDayOfWeek.IndexOf(dayOfWeek) + 1) % listDayOfWeek.Count;
							var nextValue = listDayOfWeek[nextIndex];
							NextTimeRunning = NextTimeRunning.GetNextWeekday(nextValue);
						}
					}
					break;

				case ServiceRepeat.Month:
					{
						var dayOfMonth = NextTimeRunning.Day;
						var listDayOfMonth = option.Timed!.DaysOfMonth;
						if (listDayOfMonth.Count == 1)
						{
							NextTimeRunning = NextTimeRunning.GetNextMonthday(dayOfMonth);
						}
						else
						{
							var nextIndex = (listDayOfMonth.IndexOf(dayOfMonth) + 1) % listDayOfMonth.Count;
							var nextValue = listDayOfMonth[nextIndex];
							NextTimeRunning = NextTimeRunning.GetNextMonthday(nextValue);
						}
					}
					break;

				case ServiceRepeat.Year:
					{
						var dayOfYear = NextTimeRunning;
						var listDayOfYear = option.Timed!.DaysOfYear;
						if (listDayOfYear.Count == 1)
						{
							NextTimeRunning = NextTimeRunning.GetNextYearday(dayOfYear);
						}
						else
						{
							var nextIndex = (listDayOfYear.IndexOf(dayOfYear) + 1) % listDayOfYear.Count;
							var nextValue = listDayOfYear[nextIndex];
							NextTimeRunning = NextTimeRunning.GetNextYearday(nextValue);
						}
					}
					break;
			}

		}
	}
}
