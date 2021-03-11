using SharpGen.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using Vortice.Direct3D11;
using Vortice.Direct3D9;
using Vortice.DXGI;

namespace Singe.Editor
{
	public unsafe class D3D11Image : D3DImage
    {
		static IDirect3D9Ex m_D3D9;
		static IDirect3DDevice9Ex m_D3D9Device;

		static D3D11Image()
        {
			InitD3D9(Win32.GetDesktopWindow());
        }

		public bool SetBackBuffer(ID3D11Texture2D texture)
		{
			/* Check if the user just wants to clear the D3DImage backbuffer */
			if (texture == null)
			{
				SetBackBuffer(D3DResourceType.IDirect3DSurface9, IntPtr.Zero);
				return true;
			}


			int width = 0;
			int height = 0;
			Vortice.DXGI.Format format = Vortice.DXGI.Format.Unknown;

			/* Get the texture description.  This is needed to recreate
				* the surface in the 9Ex device */
			Texture2DDescription textureDesc = texture.Description;

			width = textureDesc.Width;
			height = textureDesc.Height;
			format = textureDesc.Format;

			/* The shared handle of the D3D resource */
			IntPtr hSharedHandle = IntPtr.Zero;

			/* Shared texture pulled through the 9Ex device */
			IDirect3DTexture9 pTexture = null;

			/* Shared surface, pulled through the shared texture */
			IDirect3DSurface9 pSurface;

			var dxgiRsc = texture.QueryInterface<IDXGIResource>();

			/* Get the shared handle for the given resource */
			if (GetSharedHandle(dxgiRsc, out hSharedHandle).Failure)
				throw new Exception("Could not aquire shared resource handle");

			dxgiRsc.Dispose();

			/* Get the shared surface.  In this case its really a texture =X */
			if (GetSharedSurface(hSharedHandle, ref pTexture, width, height, format).Failure)
				throw new Exception("Could not create shared resource");

			/* Get surface level 0, which we need for the D3DImage */
			pSurface = pTexture.GetSurfaceLevel(0);

			/* Done with the texture */

			Lock();
			/* Set the backbuffer of the D3DImage */
			SetBackBuffer(D3DResourceType.IDirect3DSurface9, pSurface.NativePointer);
			Unlock();

			pSurface.Dispose();

			return true;
		}

		static Vortice.Direct3D9.Format ConvertDXGIToD3D9Format(Vortice.DXGI.Format format)
		{
			switch (format)
			{
				case Vortice.DXGI.Format.B8G8R8A8_UNorm:
					return Vortice.Direct3D9.Format.A8R8G8B8;
				case Vortice.DXGI.Format.B8G8R8A8_UNorm_SRgb:
					return Vortice.Direct3D9.Format.A8R8G8B8;
				case Vortice.DXGI.Format.B8G8R8X8_UNorm:
					return Vortice.Direct3D9.Format.X8R8G8B8;
				case Vortice.DXGI.Format.R8G8B8A8_UNorm:
					return Vortice.Direct3D9.Format.A8B8G8R8;
				case Vortice.DXGI.Format.R8G8B8A8_UNorm_SRgb:
					return Vortice.Direct3D9.Format.A8B8G8R8;
				default:
					return Vortice.Direct3D9.Format.Unknown;
			};
		}

		static Result GetSharedSurface(IntPtr hSharedHandle, ref IDirect3DTexture9 ppTexture, int width, int height, Vortice.DXGI.Format format)
		{
			Vortice.Direct3D9.Format D3D9Format;

			/* Convert the DXGI format to a D3D9 format */
			if ((D3D9Format = ConvertDXGIToD3D9Format(format)) == Vortice.Direct3D9.Format.Unknown)
				/* We're boned =X */
				return Result.InvalidArg;

			Result hr = Result.Ok;

			/* Create the texture locally, but provide the shared handle.
			 * This doesn't really create a new texture, but simply
			 * pulls the D3D10/11 resource in the 9Ex device */

			try
			{
				ppTexture = m_D3D9Device.CreateTexture(width, height, 1,
												  Vortice.Direct3D9.Usage.RenderTarget,
												  D3D9Format,
												  Pool.Default,
												  new IntPtr(&hSharedHandle));
			}
			catch(Exception ex)
            {
				hr = Result.GetResultFromException(ex); 
            }

			return hr;
		}

		static Result GetSharedHandle(IDXGIResource pSurface, out IntPtr pHandle)
		{
			Result hr = Result.Ok;

			pHandle = IntPtr.Zero;

			try
			{
				pHandle = pSurface.SharedHandle;
			}
			catch(Exception ex)
            {
				hr = Result.GetResultFromException(ex);
            }

			return hr;
		}

		static Result InitD3D9(IntPtr hWnd)
		{
			Result hr;

			IDirect3D9Ex d3D9;
			hr = D3D9.Create9Ex(out d3D9);

			m_D3D9 = d3D9;

			if (m_D3D9 == null)
				return Result.Fail;

			m_D3D9 = d3D9;

			Vortice.Direct3D9.PresentParameters d3dpp = new Vortice.Direct3D9.PresentParameters();
			d3dpp.Windowed = true;
			d3dpp.SwapEffect = Vortice.Direct3D9.SwapEffect.Discard;
			d3dpp.DeviceWindowHandle = hWnd;
			d3dpp.PresentationInterval = PresentInterval.Immediate;

			IDirect3DDevice9Ex d3D9Device = null;

			try
			{
				d3D9Device = m_D3D9.CreateDeviceEx(0,
											DeviceType.Hardware,
											hWnd,
											CreateFlags.HardwareVertexProcessing | CreateFlags.Multithreaded | CreateFlags.FpuPreserve,
											d3dpp);
			}
			catch(Exception ex)
            {
				hr = Result.GetResultFromException(ex); 
            }

			if (hr.Failure)
				return hr;

			m_D3D9Device = d3D9Device;

			return hr;
		}

		private static class Win32
        {
            [DllImport("user32.dll", SetLastError = false)] public static extern IntPtr GetDesktopWindow();
        }
    }
}