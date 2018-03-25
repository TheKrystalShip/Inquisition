using System;

namespace Inquisition.Reporting
{
	public interface IReport
	{
		Guid Guid { get; set; }
		string ErrorMessage { get; set; }
		string StackTrace { get; set; }
		string Path { get; set; }
	}
}
