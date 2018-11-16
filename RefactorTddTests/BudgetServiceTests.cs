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
			_fakeBudgetRepo.AddBudget("201801", 31);
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
			_fakeBudgetRepo.AddBudget("201801", 31);

			var totalAmount = _budgetService.TotalAmount(new DateTime(2018, 1, 1), new DateTime(2018, 1, 3));
			AmountShouldBe(3, totalAmount);
		}

		[TestMethod()]
		public void InvalidDayRange()
		{
			_fakeBudgetRepo.AddBudget("201801", 31);

			var totalAmount = _budgetService.TotalAmount(new DateTime(2018, 1, 3), new DateTime(2018, 1, 1));
			AmountShouldBe(0, totalAmount);
		}

		[TestMethod()]
		public void DiffMonth_WithBudgets()
		{
			_fakeBudgetRepo.AddBudget("201801", 31);
			_fakeBudgetRepo.AddBudget("201802", 280);

			var totalAmount = _budgetService.TotalAmount(new DateTime(2018, 1, 31), new DateTime(2018, 2, 1));
			AmountShouldBe(11, totalAmount);
		}

		[TestMethod()]
		public void period_no_overlapping_before_budget_firstDay()
		{
			_fakeBudgetRepo.AddBudget("201802", 280);

			var totalAmount = _budgetService.TotalAmount(new DateTime(2018, 1, 31), new DateTime(2018, 1, 31));
			AmountShouldBe(0, totalAmount);
		}

		[TestMethod()]
		public void period_no_overlapping_after_budget_lastDay()
		{
			_fakeBudgetRepo.AddBudget("201802", 280);

			var totalAmount = _budgetService.TotalAmount(new DateTime(2018, 3, 31), new DateTime(2018, 3, 31));
			AmountShouldBe(0, totalAmount);
		}

		[TestMethod()]
		public void period_overlapping_budget_firstDay()
		{
			_fakeBudgetRepo.AddBudget("201802", 280);

			var totalAmount = _budgetService.TotalAmount(new DateTime(2018, 1, 31), new DateTime(2018, 2, 1));
			AmountShouldBe(10, totalAmount);
		}

		[TestMethod()]
		public void period_overlapping_budget_lastDay()
		{
			_fakeBudgetRepo.AddBudget("201802", 280);

			var totalAmount = _budgetService.TotalAmount(new DateTime(2018, 2, 28), new DateTime(2018, 3, 1));
			AmountShouldBe(10, totalAmount);
		}

		[TestMethod()]
		public void DiffYear_WithBudgets()
		{
			_fakeBudgetRepo.AddBudget("201712", 31000);
			_fakeBudgetRepo.AddBudget("201801", 31);

			var totalAmount = _budgetService.TotalAmount(new DateTime(2017, 12, 31), new DateTime(2018, 1, 1));
			AmountShouldBe(1001, totalAmount);
		}

		[TestMethod()]
		public void DiffYear_With_Cross_MultiMonthBudgets()
		{
			_fakeBudgetRepo.AddBudget("201712", 31000);
			_fakeBudgetRepo.AddBudget("201801", 31);
			_fakeBudgetRepo.AddBudget("201802", 280);
			var totalAmount = _budgetService.TotalAmount(new DateTime(2017, 12, 31), new DateTime(2018, 2, 1));
			AmountShouldBe(1041, totalAmount);
		}
	}

	public class FakeBudgetRepo : IBudgetRepo
	{
		private List<Budget> _budgets = new List<Budget>();

		public virtual List<Budget> GetAll()
		{
			return _budgets;
		}

		public void AddBudget(string yearMonth, int amount)
		{
			_budgets.Add(new Budget() { YearMonth = yearMonth, Amount = amount });
		}
	}
}