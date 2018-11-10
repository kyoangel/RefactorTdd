using System.Collections.Generic;


namespace RefactorTdd
{
	public interface IBudgetRepo
	{
		List<Budget> GetAll();
	}
}