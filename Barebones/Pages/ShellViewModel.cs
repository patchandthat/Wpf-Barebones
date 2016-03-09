using Barebones.Pages.MessageBox;
using Stylet;

namespace Barebones.Pages
{
    public class ShellViewModel : Screen
    {
	    private readonly IWindowManager _windowManager;
	    private string _width;
	    private string _height;

	    public ShellViewModel(IWindowManager windowManager, IModelValidator<ShellViewModel> validator) : base(validator)
	    {
		    _windowManager = windowManager;
	    }

	    public string Width
	    {
		    get { return _width; }
		    set
		    {
			    SetAndNotify(ref _width, value);
				NotifyOfPropertyChange(() => CanShowADialog);
			}
	    }

	    public string Height
	    {
		    get { return _height; }
		    set
		    {
			    SetAndNotify(ref _height, value);
				NotifyOfPropertyChange(() => CanShowADialog);
		    }
	    }

	    public bool CanShowADialog => Validate();

	    public void ShowADialog()
	    {
		    var message = $"Area: {int.Parse(Width) * int.Parse(Height)}";
		    _windowManager.ShowDialog(new DialogViewModel() { Message = message, DisplayName = "Modal Dialog"});
	    }
    }
}
