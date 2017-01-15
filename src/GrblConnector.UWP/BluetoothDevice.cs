﻿﻿//
// Copyright (c) 2014 Morten Nielsen
//
// Licensed under the Microsoft Public License (Ms-PL) (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://opensource.org/licenses/Ms-PL.html
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
#if NETFX_CORE
using BTDevice = Windows.Devices.Bluetooth.Rfcomm.RfcommDeviceService;
using Windows.Devices.Bluetooth.Rfcomm;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
#else
using BTDevice = Windows.Networking.Proximity.PeerInformation;
#endif

namespace GrblConnector
{
	/// <summary>
	/// A Bluetooth Grbl device
	/// </summary>
	public class BluetoothDevice : GrblDevice
	{
		private BTDevice m_device;
		private StreamSocket m_socket;

		/// <summary>
		/// Initializes a new instance of the <see cref="BluetoothDevice"/> class.
		/// </summary>
		/// <param name="device">The device.</param>
		public BluetoothDevice(BTDevice device)
		{
			m_device = device;
		}

		/// <summary>
		/// Creates the stream the GrblDevice is working on top off.
		/// </summary>
		/// <returns></returns>
		protected override async Task<System.IO.Stream> OpenStreamAsync()
		{
			var socket = new Windows.Networking.Sockets.StreamSocket();
			await socket.ConnectAsync(
#if NETFX_CORE
				m_device.ConnectionHostName,
								m_device.ConnectionServiceName);
#else
								m_device.HostName, "1");
#endif
			m_socket = socket;
			return socket.InputStream.AsStreamForRead();
		}

		/// <summary>
		/// Closes the stream the GrblDevice is working on top off.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <returns></returns>
		protected override Task CloseStreamAsync(System.IO.Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException("stream");
			stream.Dispose();
			m_socket.Dispose();
			m_socket = null;
			return Task.FromResult(true);
		}
	}
}
