
#nullable disable
namespace KeySender;

public class ComboboxItem
{
  public string Text { get; set; }

  public int Value { get; set; }

  public override string ToString() => this.Text;
}
