namespace Hviktor.Abstractions.Enums.Attributes;

/// <summary>
/// Represents the type of input field.
/// </summary>
/// <source>
/// <a href="https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Elements/input#input_types">MDN Web Docs - Input Types</a>
/// </source>
public enum InputType
{
    /// <summary>
    /// &lt;input&gt; elements of type text create basic single-line text fields.
    /// </summary>
    /// <source>
    /// <a href="https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Elements/input/text">MDN Web Docs - Input Types - Text</a>
    /// </source>
    Text,

    /// <summary>
    /// &lt;input&gt; elements of type <c>textarea</c> create multi-line text fields.
    /// </summary>
    TextArea,

    /// <summary>
    /// &lt;input&gt; elements of type <c>checkbox</c> are rendered by default as boxes that are checked (ticked) when activated, 
    /// like you might see in an official government paper form.
    /// </summary>
    /// <remarks>
    /// The exact appearance depends upon the operating system configuration under which the browser is running.<br/>
    /// Generally this is a square but it may have rounded corners. A checkbox allows you to select single values for submission in a form (or not).
    /// </remarks>
    /// <source>
    /// <a href="https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Elements/input/checkbox">MDN Web Docs - Input Types - Checkbox</a>
    /// </source>
    Checkbox,

    /// <summary>
    /// &lt;input&gt; elements of type <c>radio</c> are typically presented as small circles that are filled (selected) when activated.
    /// </summary>
    /// <remarks>
    /// The exact appearance depends upon the operating system configuration under which the browser is running.<br/>
    /// A radio button allows the user to select one option from a set.
    /// </remarks>
    /// <source>
    /// <a href="https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Elements/input/radio">MDN Web Docs - Input Types - Radio</a>
    /// </source>
    Radio,

    /// <summary>
    /// &lt;input&gt; elements of type <c>button</c> create clickable buttons, which can be used to submit forms or anywhere in a document for accessible, standard button functionality.
    /// </summary>
    Button,

    /// <summary>
    /// &lt;input&gt; elements with <c>type="file"</c> let the user choose one or more files from their device storage.
    /// Once chosen, the files can be uploaded to a server using
    /// <a href="https://developer.mozilla.org/en-US/docs/Learn_web_development/Extensions/Forms">form submission</a>,
    /// or manipulated using JavaScript code and <a href="https://developer.mozilla.org/en-US/docs/Web/API/File_API/Using_files_from_web_applications">the File API</a>.
    /// </summary>
    /// <source>
    /// <a href="https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Elements/input/file">MDN Web Docs - Input Types - File</a>
    /// </source>
    File,

    /// <summary>
    /// &lt;input&gt; elements of type <c>password</c> provide a way for the user to securely enter a password.
    /// </summary>
    /// <remarks>
    /// The element is presented as a one-line plain text editor control in which the text is obscured so that it cannot be read,
    /// usually by replacing each character with a symbol such as the asterisk ("*") or a dot ("•").<br/>
    /// This character will vary depending on the user agent and operating system.
    /// </remarks>
    /// <source>
    /// <a href="https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Elements/input/password">MDN Web Docs - Input Types - Password</a>
    /// </source>
    Password,

    /// <summary>
    /// &lt;input&gt; elements of type <c>email</c> are used to let the user enter and edit an email address, or,
    /// if the <a href="https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Attributes/multiple"><c>multiple</c></a> attribute is specified, a list of email addresses.
    /// </summary>
    /// <source>
    /// <a href="https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Elements/input/email">MDN Web Docs - Input Types - Email</a>
    /// </source>
    Email,

    /// <summary>
    /// &lt;input&gt; elements of type <c>number</c> are used to let the user enter a number. They include built-in validation to reject non-numerical entries.
    /// </summary>
    /// <remarks>
    /// The browser may opt to provide stepper arrows to let the user increase and decrease the value using their mouse or by tapping with a fingertip.
    /// </remarks>
    /// <source>
    /// <a href="https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Elements/input/number">MDN Web Docs - Input Types - Number</a>
    /// </source>
    Number,

    /// <summary>
    /// &lt;input&gt; elements of type <c>hidden</c> are used to hide an input from view, while still allowing form submission.
    /// </summary>
    /// <remarks>
    /// The input and change events do not apply to this input type. Hidden inputs cannot be focused even using JavaScript (e.g., hiddenInput.focus()). 
    /// </remarks>
    /// <source>
    /// <a href="https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Elements/input/hidden">MDN Web Docs - Input Types - Hidden</a>
    /// </source>
    Hidden,

    /// <summary>
    /// &lt;input&gt; elements of type <c>tel</c> are used to let the user enter and edit a telephone number.<br/>
    /// </summary>
    /// <remarks>
    /// Unlike <a href="https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Elements/input/email"><c>&lt;input type="email"&gt;</c></a>
    /// and <a href="https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Elements/input/url"><c>&lt;input type="url"&gt;</c></a>,
    /// the input value is not automatically validated to a particular format before the form can be submitted,
    /// because formats for telephone numbers vary so much around the world.
    /// </remarks>
    /// <source>
    /// <a href="https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Elements/input/tel">MDN Web Docs - Input Types - Tel</a>
    /// </source>
    Tel,

    /// <summary>
    /// &lt;input&gt; elements of type <c>url</c> are used to let the user enter and edit a URL.
    /// </summary>
    /// <source>
    /// <a href="https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Elements/input/url">MDN Web Docs - Input Types - URL</a>
    /// </source>
    Url,

    /// <summary>
    /// &lt;input&gt; elements of type <c>search</c> are text fields designed for the user to enter search queries into.<br/>
    /// These are functionally identical to <c>text</c> inputs,
    /// but may be styled differently by the <a href="https://developer.mozilla.org/en-US/docs/Glossary/User_agent">user agent</a>
    /// </summary>
    /// <source>
    /// <a href="https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Elements/input/search">MDN Web Docs - Input Types - Search</a>
    /// </source>
    Search,

    /// <summary>
    /// &lt;input&gt; elements of <c>type="date"</c> create input fields that let the user enter a date.
    /// </summary>
    /// <remarks>
    /// The appearance of the date picker input UI varies based on the browser and operating system.
    /// The value is normalized to the format <c>yyyy-mm-dd</c>.<br/><br/>
    /// The resulting value includes the year, month, and day, but not the time.
    /// The <a href="https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Elements/input/time">time</a>
    /// and <a href="https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Elements/input/datetime-local">datetime-local</a> input types support time and date+time input.
    /// </remarks>
    /// <source>
    /// <a href="https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Elements/input/date">MDN Web Docs - Input Types - Date</a>
    /// </source>
    Date,

    /// <summary>
    /// &lt;input&gt; elements of type <c>time</c> create input fields designed to let the user easily enter a time (hours and minutes, and optionally seconds).
    /// </summary>
    /// <remarks>
    /// While the control's user interface appearance is based on the browser and operating system, the features are the same.<br/>
    /// The value is always a 24-hour <c>HH:mm</c> or <c>HH:mm:ss</c> formatted time, with leading zeros, regardless of the UI's input format.
    /// </remarks>
    /// <source>
    /// <a href="https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Elements/input/time">MDN Web Docs - Input Types - Time</a>
    /// </source>
    Time,

    /// <summary>
    /// &lt;input&gt; elements of type <c>datetime-local</c> create input controls that let the user easily enter both a date and a time,
    /// including the year, month, and day as well as the time in hours and minutes.
    /// </summary>
    /// <source>
    /// <a href="https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Elements/input/datetime-local">MDN Web Docs - Input Types - Datetime Local</a>
    /// </source>
    DateTimeLocal,

    /// <summary>
    /// &lt;input&gt; elements of type <c>month</c> create input fields that let the user enter a month and year allowing a month and year to be easily entered.
    /// The value is a string whose value is in the format <c>YYYY-MM</c>, where <c>YYYY</c> is the four-digit year and <c>MM</c> is the month number.
    /// </summary>
    /// <source>
    /// <a href="https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Elements/input/month">MDN Web Docs - Input Types - Month</a>
    /// </source>
    Month,

    /// <summary>
    /// &lt;input&gt; elements of type <c>week</c> create input fields allowing easy entry of a year plus the
    /// <a href="https://en.wikipedia.org/wiki/ISO_8601#Week_dates">ISO 8601 week number</a>
    /// during that year (i.e., week 1 to <a href="https://en.wikipedia.org/wiki/ISO_8601#Week_dates">52 or 53</a>).
    /// </summary>
    /// <source>
    /// <a href="https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Elements/input/week">MDN Web Docs - Input Types - Week</a>
    /// </source>
    Week,

    /// <summary>
    /// &lt;input&gt; elements of type <c>color</c> create input fields that let the user select a color.
    /// </summary>
    Color,

    /// <summary>
    /// &lt;input&gt; elements of type <c>submit</c> are used to submit forms.
    /// </summary>
    Submit,

    /// <summary>
    /// &lt;input&gt; elements of type <c>reset</c> are used to reset forms.
    /// </summary>
    Reset,

    /// <summary>
    /// &lt;tooltip&gt; elements of type <c>describedby</c> create tooltip with aria attribute <c>aria-describedby</c>.
    /// <c>aria-describedby</c> is used to establish relationships between trigger and their tooltip overlay.
    /// </summary>
    DescribedBy,

    /// <summary>
    /// &lt;tooltip&gt; elements of type <c>labelledby</c> create tooltip with aria attribute <c>aria-labelledby</c>.
    /// <c>aria-labelledby</c> is used to establish relationships between trigger and their tooltip overlay.
    /// </summary>
    LabelledBy
}