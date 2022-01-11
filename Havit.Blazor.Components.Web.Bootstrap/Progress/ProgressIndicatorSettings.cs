﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Settings for the <see cref="HxProgressIndicator"/> and derived components.
	/// </summary>
	public record ProgressIndicatorSettings
	{
		/// <summary>
		/// Debounce delay in miliseconds.
		/// </summary>
		public int? Delay { get; set; }
	}
}
