using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace RefactorTdd
{
	public class BudgetService
	{
		private readonly IBudgetRepo _repo;

		private static void Main()
		{
		}

		public BudgetService(IBudgetRepo repo)
		{
			_repo = repo;
		}

		public double TotalAmount(DateTime start, DateTime end)
		{
			if (IsValidDateRange(start, end))
			{

				var s = start.ToString("yyyyMM");
				var budgets = _repo.GetAll();

				if (start.ToString("yyyyMM") != end.ToString("yyyyMM"))
				{
					DateTime tempDate = new DateTime(start.Year,start.Month,1);
					double aggrAmount = 0;
					do
					{
						var budgetByMonth =
							budgets.SingleOrDefault(x => x.YearMonth.Equals(tempDate.ToString("yyyyMM")));
						if (budgetByMonth != null)
						{
							if (tempDate.ToString("yyyyMM") == start.ToString("yyyyMM"))
								aggrAmount += AmountPerDayInMonth(budgetByMonth, start) *
								              (DateTime.DaysInMonth(start.Year, start.Month) - start.Day + 1);
							else if (tempDate.ToString("yyyyMM") == end.ToString("yyyyMM"))
								aggrAmount += AmountPerDayInMonth(budgetByMonth, end) * end.Day;
							else
								aggrAmount += budgetByMonth.Amount;
						}

						tempDate = tempDate.AddMonths(1);
					} while (tempDate.Year <= end.Year && tempDate.Month <= end.Month);

					return aggrAmount;
				}

				var budget = budgets.SingleOrDefault(x => x.YearMonth.Equals(s));
				if (budget == null)
				{
					return 0;
				}
				var budgetPerDay = budget.Amount / DateTime.DaysInMonth(start.Year, start.Month);
				return budgetPerDay * DaysInterval(start, end);
			}
			else
			{
				return 0;
			}
		}

		private static int AmountPerDayInMonth(Budget budgetByMonth, DateTime tempDate)
		{
			return budgetByMonth.Amount / DateTime.DaysInMonth(tempDate.Year, tempDate.Month);
		}


		private static bool IsValidDateRange(DateTime start, DateTime end)
		{
			return start <= end;
		}

		private static int DaysInterval(DateTime start, DateTime end)
		{
			return end.Subtract(start).Days + 1;
		}
	}
}