using System.Collections.Generic;

namespace RefactorTdd.Tests
{
	public class BudgetRepo : IBudgetRepo
	{
		public List<Budget> GetAll()
		{
			return new List<Budget>();
		}
	}
}