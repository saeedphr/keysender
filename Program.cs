

using System;
using System.Windows.Forms;
using KeySender;
#nullable disable
namespace Program;

internal static class Program
{
  [STAThread]
  private static void Main()
  {
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);
    Application.Run(new KeySender.Main());
  }
}
