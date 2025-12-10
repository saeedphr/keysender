// Decompiled with JetBrains decompiler
// Type: KeySender.Properties.Resources
// Assembly: KeySender, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 63449D79-070E-4FB3-B850-28E7F27EF192
// Assembly location: C:\Users\Administrator\Desktop\KeySender_v2.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace KeySender.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class Resources
{
  private static ResourceManager resourceMan;
  private static CultureInfo resourceCulture;

  internal Resources()
  {
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static ResourceManager ResourceManager
  {
    get
    {
      if (KeySender.Properties.Resources.resourceMan == null)
        KeySender.Properties.Resources.resourceMan = new ResourceManager("KeySender.Properties.Resources", typeof (KeySender.Properties.Resources).Assembly);
      return KeySender.Properties.Resources.resourceMan;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static CultureInfo Culture
  {
    get => KeySender.Properties.Resources.resourceCulture;
    set => KeySender.Properties.Resources.resourceCulture = value;
  }

  internal static Bitmap keys
  {
    get => (Bitmap) KeySender.Properties.Resources.ResourceManager.GetObject(nameof (keys), KeySender.Properties.Resources.resourceCulture);
  }

  internal static Bitmap paste
  {
    get => (Bitmap) KeySender.Properties.Resources.ResourceManager.GetObject(nameof (paste), KeySender.Properties.Resources.resourceCulture);
  }

  internal static Bitmap refresh
  {
    get
    {
      return (Bitmap) KeySender.Properties.Resources.ResourceManager.GetObject(nameof (refresh), KeySender.Properties.Resources.resourceCulture);
    }
  }

  internal static Bitmap send
  {
    get => (Bitmap) KeySender.Properties.Resources.ResourceManager.GetObject(nameof (send), KeySender.Properties.Resources.resourceCulture);
  }

  internal static Bitmap send1
  {
    get => (Bitmap) KeySender.Properties.Resources.ResourceManager.GetObject(nameof (send1), KeySender.Properties.Resources.resourceCulture);
  }

  internal static Bitmap send2
  {
    get => (Bitmap) KeySender.Properties.Resources.ResourceManager.GetObject(nameof (send2), KeySender.Properties.Resources.resourceCulture);
  }
}
