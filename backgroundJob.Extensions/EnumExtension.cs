namespace backgroundJob.Extensions
{
	public static class EnumExtension
	{
		public static void AddEnum<T>(this T value, T other) where T : struct, IConvertible
		{
			if (value.GetType().IsEnum)
			{
				var uvalue = Convert.ToUInt64(value);
				var uother = Convert.ToUInt64(other);
				var output = uvalue | uother;

				value = (T)Enum.Parse(value.GetType(), output.ToString());
			}
		}

		public static bool IsEnum<T>(this T value, T other) where T : struct, IConvertible
		{
			if (value.GetType().IsEnum)
			{
				var uvalue = Convert.ToUInt64(value);
				var uother = Convert.ToUInt64(other);
				var output = (uvalue & uother) > 0;
				return output;
			}
			return false;
		}

		public static void MovedEnum<T>(this T value, T other) where T : struct, IConvertible
		{
			if (value.GetType().IsEnum)
			{
				var uvalue = Convert.ToUInt64(value);
				var uother = Convert.ToUInt64(other);
				var output = uvalue &= ~uother;
				value = (T)Enum.Parse(value.GetType(), output.ToString());
			}
		}
	}
}
