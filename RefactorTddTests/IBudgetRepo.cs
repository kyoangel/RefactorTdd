using System.Collections.Generic;

namespace RefactorTdd.Tests
{
	public interface IBudgetRepo
	{
		List<Budget> GetAll();
	}
}