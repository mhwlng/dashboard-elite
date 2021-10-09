using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace MudBlazor
{
    public enum MudTripleIconButtonState
    {
        Primary,
        Secondary,
        Tertiary
    }

    public partial class MudTripleIconButton : MudComponentBase
    {
        [Parameter] public MudTripleIconButtonState State { get; set; }
        
        [Parameter] public string PrimaryIcon { get; set; }
        [Parameter] public string SecondaryIcon { get; set; }
        [Parameter] public string TertiaryIcon { get; set; }

        [Parameter] public string PrimaryTitle { get; set; }
        [Parameter] public string SecondaryTitle { get; set; }
        [Parameter] public string TertiaryTitle { get; set; }

        [Parameter] public Color PrimaryColor { get; set; } = Color.Default;
        [Parameter] public Color SecondaryColor { get; set; } = Color.Default;
        [Parameter] public Color TertiaryColor { get; set; } = Color.Default;

        [Parameter] public Size Size { get; set; } = Size.Medium;

        [Parameter] public Edge Edge { get; set; }

        [Parameter] public bool DisableRipple { get; set; }

        [Parameter] public bool Disabled { get; set; }

        [Parameter] public Variant Variant { get; set; } = Variant.Text;

        [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

    }
}
