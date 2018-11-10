using System;
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
			if (!IsValidDateRange(start, end))
			{
				return 0;
			}

			var s = start.ToString("yyyyMM");
			var budgets = _repo.GetAll();

			if (IsSameMonth(start, end))
			{
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
				DateTime tempDate = new DateTime(start.Year, start.Month, 1);
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
				} while (tempDate <= end);

				return aggrAmount;
			}
		}

		private static bool IsSameMonth(DateTime start, DateTime end)
		{
			return start.ToString("yyyyMM") == end.ToString("yyyyMM");
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