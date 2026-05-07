namespace DFCStats.Business.Helpers
{
    public static class DateAndTimeHelper
    {
        /// <summary>
        /// A helper menthod which will take two date and time values and turn it into a 
        /// string showing the amount of years, months and days between the two values
        /// for example: 4 years, 3 months, 18 days
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static string TimeAsString(DateOnly date1, DateOnly date2)
        {
            // Defining some variables for date from and date to
            DateOnly dateFrom;
            DateOnly dateTo;

            // Defining Number of days in month; index 0=> january and 11=> December
            // February contain either 28 or 29 days, that's why here value is -1
            // which wil be calculated later.
            int[] monthDay = new int[12] { 31, -1, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

            // Defining variables for the years, months and days
            int year;
            int month;
            int day;

            int increment = 0;

            // Works out which date supplied to the fucntion is the date from and date to
            if (date1 > date2)
            {
                dateFrom = date2;
                dateTo = date1;
            } else {
                dateFrom = date1;
                dateTo = date2;
            }

            // Caculate the day
            if (dateFrom.Day > dateTo.Day)
                increment = monthDay[dateFrom.Month - 1];

            // If it is february month
            // if it's to day is less then from day
            if (increment == -1)
            {
                if (DateTime.IsLeapYear(dateFrom.Year))
                {
                    // leap year february contain 29 days
                    increment = 29;
                } else {
                    increment = 28;
                }
            }
            if (increment != 0)
            {
                day = (dateTo.Day + increment) - dateFrom.Day;
                increment = 1;
            } else {
                day = dateTo.Day - dateFrom.Day;
            }

            // Month calculation
            if ((dateFrom.Month + increment) > dateTo.Month)
            {
                month = (dateTo.Month + 12) - (dateFrom.Month + increment);
                increment = 1;
            } else {
                month = (dateTo.Month) - (dateFrom.Month + increment);
                increment = 0;
            }

            // Year calculation
            year = dateTo.Year - (dateFrom.Year + increment);

            // Works out what should be returned by the string
            string stringToReturn = string.Empty;

            if (year > 0)
            {
                if (year > 1)
                {
                    stringToReturn = string.Format("{0} years", year.ToString());
                } else {
                    stringToReturn = string.Format("{0} year", year.ToString());
                }

                // If month or day is greater than zero add a comma
                if (month > 0 || day > 0)
                    stringToReturn = stringToReturn + ", ";
            }

            if (month > 0)
            {
                if (month > 1)
                {
                    stringToReturn = stringToReturn + string.Format("{0} months", month.ToString());
                } else {
                    stringToReturn = stringToReturn + string.Format("{0} month", month.ToString());
                }

                // If day is greater than zero add a comma
                if (day > 0)
                    stringToReturn = stringToReturn + ", ";
            }

            if (day > 0)
            {
                if (day > 1)
                {
                    stringToReturn = stringToReturn + string.Format("{0} days", day.ToString());
                } else{
                    stringToReturn = stringToReturn + string.Format("{0} day", day.ToString());
                }
            }

            return stringToReturn;
        }
    }
}