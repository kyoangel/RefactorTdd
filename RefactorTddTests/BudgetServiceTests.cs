using Microsoft.VisualStudio.TestTools.UnitTesting;
using RefactorTdd;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorTdd.Tests
{
	[TestClass()]
	public class BudgetServiceTests
	{
		[TestMethod()]
		public void TotalAmountTest()
		{
			var budgetRepo = new BudgetRepo();
			Assert.Fail();
		}
	}

	public class Budget
	{
		public string YearMonth { get; set; }
		public int Amount { get; set; }
	}
}