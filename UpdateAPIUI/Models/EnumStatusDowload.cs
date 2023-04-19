using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateAPIUI.Models
{
	public enum StatusDowload : byte
	{
		Error = 0,
		OK = 1,
		AbortDowload = 2,
	}
}
