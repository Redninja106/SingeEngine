using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using Vortice.Direct3D;
using Vortice.Direct3D11;

namespace Singe.Editor
{
    /// <summary>
    /// Interaction logic for ToolWindow1Control.
    /// </summary>
    public partial class ToolWindow1Control : UserControl
    {
        ID3D11Device device;
        ID3D11DeviceContext context;
        ID3D11Texture2D tex;
        ID3D11RenderTargetView rtv;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolWindow1Control"/> class.
        /// </summary>
        public ToolWindow1Control()
        {
            this.InitializeComponent();

            var hr = D3D11.D3D11CreateDevice(null, Vortice.Direct3D.DriverType.Hardware, DeviceCreationFlags.Debug, new[] { FeatureLevel.Level_11_1 }, out device, out context);

            if (hr.Failure)
                throw new System.Exception();

            tex = device.CreateTexture2D(new Texture2DDescription(Vortice.DXGI.Format.R8G8B8A8_UNorm, 100, 100, 1, 1, BindFlags.RenderTarget | BindFlags.ShaderResource, Usage.Default, CpuAccessFlags.None, 1, 0, ResourceOptionFlags.None));

            rtv = device.CreateRenderTargetView(tex, new RenderTargetViewDescription(tex, RenderTargetViewDimension.Texture2D, tex.Description.Format, 0, 0));

        }

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            context.ClearRenderTargetView(rtv, Color.Red);

            d3dImage.Lock();
            if (d3dImage.SetBackBuffer(tex))
            {
                d3dImage.AddDirtyRect(new Int32Rect(0, 0, (int)d3dImage.Width, (int)d3dImage.Height));
            }
            else
            {
                throw new System.Exception();
            }
            d3dImage.Unlock();

        }
    }
}