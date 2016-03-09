using Stylet;

namespace Barebones.Pages.MessageBox
{
	public class DialogViewModel : Screen
	{
	    private string _message;

	    public string Message
	    {
	        get { return _message; }
	        set
	        {
	            _message = value;
	            NotifyOfPropertyChange(() => Message);
	        }
	    }
	}
}
