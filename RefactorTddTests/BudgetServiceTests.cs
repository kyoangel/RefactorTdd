using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace RefactorTdd.Tests
{
	[TestClass()]
	public class BudgetServiceTests
	{
		private FakeBudgetRepo _fakeBudgetRepo;
		private BudgetService _budgetService;

		[TestInitialize]
		public void Init()
		{
			_fakeBudgetRepo = new FakeBudgetRepo();
			_budgetService = new BudgetService(_fakeBudgetRepo);
		}

		[TestMethod()]
		public void No_budget_return_zero()
		{
			var totalAmount = _budgetService.TotalAmount(new DateTime(2018, 4, 1), new DateTime(2018, 4, 3));

			AmountShouldBe(0, totalAmount);
		}

		[TestMethod()]
		public void One_Day()
		{
			var totalAmount = _budgetService.TotalAmount(new DateTime(2018, 1, 1), new DateTime(2018, 1, 1));
			AmountShouldBe(1, totalAmount);
		}

		private void AmountShouldBe(int expected, double actual)
		{
			Assert.AreEqual(expected, actual);
		}

		

		[TestMethod()]
		public void SameMonth_MultiDays()

		{
			var totalAmount = _budgetService.TotalAmount(new DateTime(2018, 1, 1), new DateTime(2018, 1, 3));
			AmountShouldBe(3, totalAmount);
		}

		[TestMethod()]
		public void InvalidDayRange()
		{
			var totalAmount = _budgetService.TotalAmount(new DateTime(2018, 1, 3), new DateTime(2018, 1, 1));
			AmountShouldBe(0, totalAmount);

		}

		[TestMethod()]
		public void DiffMonth_WithBudgets()
		{
			var totalAmount = _budgetService.TotalAmount(new DateTime(2018, 1, 31), new DateTime(2018, 2, 1));
			AmountShouldBe(11, totalAmount);

		}
	}

	public class FakeBudgetRepo : IBudgetRepo
	{
		public virtual List<Budget> GetAll()
		{
			return new List<Budget>()
			{
				new Budget()
				{
					YearMonth = "201712",
					Amount = 31000
				},
				new Budget()
				{
					YearMonth = "201801",
					Amount = 31
				},
				new Budget()
				{
					YearMonth = "201802",
					Amount = 280
				}
			};
		}
	}
}