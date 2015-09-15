using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectEuler
{
    partial class Program
    {
        private static void P019()
        {
            Calendary c = new Calendary(1, 1, 1900, DayOfWeek.Monday);
            if (c.IsLeapYear())
            {
                c.AddDays(366);
            }
            else
            {
                c.AddDays(365);
            }
            int daysToSunday = 6 - (int)c.DayOfWeek;
            c.AddDays(daysToSunday);
            int sundayCount = 0;
            do
            {
                if (c.Day == 1 && c.DayOfWeek == DayOfWeek.Sunday)
                {
                    sundayCount++;
                }
                c.AddDays(7);
            }
            while (c.Year <= 2000);
            Console.WriteLine($"P019: {sundayCount}");
        }

        private class Calendary
        {
            private int dayOfWeek;
            private int day;
            private int month;
            private int year;

            public Calendary(int day, int month, int year, DayOfWeek dayOfWeek)
            {
                this.Day = day;
                this.Month = month;
                this.Year = year;
                this.dayOfWeek = (int)dayOfWeek;
            }

            public int Day
            {
                get
                {
                    return day;
                }
                private set
                {
                    this.day = value;
                }
            }
            public int Month
            {
                get
                {
                    return month;
                }
                private set
                {
                    this.month = value;
                }
            }
            public int Year
            {
                get
                {
                    return year;
                }
                private set
                {
                    this.year = value;
                }
            }
            public DayOfWeek DayOfWeek
            {
                get
                {
                    return (DayOfWeek)(this.dayOfWeek % 7);
                }
            }

            public bool IsLeapYear()
            {
                if (this.Year % 100 == 0)
                {
                    if (this.Year % 400 == 0)
                    {
                        return true;
                    }
                    return false;
                }
                return this.Year % 4 == 0;
            }
            private int DaysInMonth()
            {
                if (this.Month == 2)
                {
                    return this.IsLeapYear() ? 29 : 28;
                }
                if (this.Month == 1 || // jan
                    this.Month == 3 || // mar
                    this.Month == 5 || // may
                    this.Month == 7 || // jul
                    this.Month == 8 || // aug
                    this.Month == 10 || // oct
                    this.Month == 12) // dec
                {
                    return 31;
                }
                return 30;
            }
            public void AddDays(int days = 1)
            {
                int daysleft = this.DaysInMonth() - this.Day + 1;
                if (days >= daysleft)
                {
                    this.Day = 1;
                    this.IncrementMonth();
                    this.dayOfWeek += daysleft;
                    this.AddDays(days - daysleft);
                }
                else
                {
                    this.Day += days;
                    this.dayOfWeek += days;
                }
            }
            private void IncrementMonth()
            {
                this.Month += 1;
                if (this.Month > 12)
                {
                    this.Month = 1;
                    this.Year += 1;
                }
            }
        }

        private enum DayOfWeek
        {
            Monday = 0,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday,
            Sunday
        }
    }
}
