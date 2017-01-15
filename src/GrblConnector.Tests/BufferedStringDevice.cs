﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrblConnector.Tests
{
	internal class BufferedStringDevice : GrblConnector.BufferedStreamDevice
	{
		string m_data;
		public BufferedStringDevice(string data) : base(1)
		{
			m_data = data;
		}
		protected override Task<System.IO.Stream> GetStreamAsync()
		{
			return Task.FromResult<System.IO.Stream>(new System.IO.MemoryStream(UTF8Encoding.UTF8.GetBytes(m_data)));
		}
	}
}
