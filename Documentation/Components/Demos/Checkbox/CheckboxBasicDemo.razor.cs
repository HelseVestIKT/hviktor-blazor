using Documentation.Resources;
using Hviktor.Abstractions.Interfaces.Localization;
using Microsoft.AspNetCore.Components;

namespace Documentation.Components.Demos.Checkbox;

public partial class CheckboxBasicDemo : ComponentBase
{
    [Inject] protected IStringLocalizerService<DocumentationResources> Localizer { get; set; } = null!;

    private bool? checkboxOption3;
    private bool? checkboxOption2;
    private bool? checkboxOption1;
    protected bool CheckboxValue { get; set; }

    protected bool? IndeterminateCheckboxValue
    {
        get
        {
            var checkedCount = new[] { checkboxOption1, checkboxOption2, checkboxOption3 }.Count(x => x == true);
            return checkedCount switch
            {
                3 => true,
                0 => false,
                _ => null
            };
        }
    }

    private void OnIndeterminateCheckboxChanged(ChangeEventArgs args)
    {
        var newValue = args.Value is true;
        checkboxOption1 = newValue;
        checkboxOption2 = newValue;
        checkboxOption3 = newValue;
    }
}