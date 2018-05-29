# wfQuery
WinForms "port" of jQuery.  
VERY MUCH PROOF-OF-CONCEPT.  DO NOT USE IN PRODUCTION.  

## Why?
Why not?  It won't be terribly useful to many people.  But for the occasional person that needs to do a LOT of UI work at runtime, it might just be a life-saver.

Also, I like the jQuery syntax.  I wanted to see if I could bring that ease of use to WinForms.


## Syntax
Obviously js syntax is not 100% portable to c# - nor would you want it to be.  But I've tried to do my best to keep it as close as possible.  
I would have liked to access the query selector object via the `$` variable name, but c# does not allow that name.  

### Initialization
wfQuery operates on a single `Control` per instance of the `wfQuery` class.  Remember, `Form` derives from `Control`.  

After your control is fully loaded (e.g. after `Form.InitializeComponent()` is done), you can initialize a basic implementation like so:  

    using System;
    using System.Windows.Forms;
    using wfQuery;
    
    public partial class MainForm : Form {
      
        private wfQuery.wfQuery _ = null;
      
        public MainForm() {
        	InitializeComponent();
			
			_ = new wfQuery.wfQuery(this);
        }
    }
    

### Query Selectors
Selectors are currently very limited.  There are two kinds  
* Class selectors - select all items of a given Type.  e.g.  `.TextBox`
* Name selectors - select an item of a given name.  e.g. `#textBox2`

You may include multiple selectors in a query by separating them with commas:  
    
    _[".TextBox, .Button"]
    

### Property manipulation
Properties can be queried or manipulated via the `.Attr` method of the query results:  

	// This example will recieve all the TextBox.Text property values on the Form in an array.  
	var values = _[".TextBox"].Attr("Text");
	
	// This example sets all the TextBox.Text property values to "Hello World.".
	_[".TextBox"].Attr("Text", "Hello World.");
	
	
### Event handling
Arguable one of the most useful features of jQuery is its event handling.  
You can achieve much the same thing using wfQuery:  

	// Hook up a button click handler to all buttons on the form
	// that changes the clicked button's background color to Green.
	_[".Button"].On("Click", (s, args) => {
	    _[s].Attr("Color", Color.Green);
	});
	
As you can see, there's a little work to be done here.  It would be nice if `s` were of type `Control` rather than `Object`.
	
### Property Resolvers
Property resolvers do just what their name implies.  They are the DNS of property names.  Given a property name and a Type, a Property Resolver 
is responsible for returning (or setting) the correct value.  They also implement the property lookup symantics.  
For instance, the included `ReflectionPropertyResolver` reflects over each object to find the requested property.  On the other hand, 
the included `ReflectionCachePropertyResolver` caches any uncached Type structures (properties) the first time it encounters one.  Subsequent 
requests for a property on that Type are fulfilled from the cache.  This Property Resolver trades memory for cpu time.


### Now what?
Well, now I'll continue to work on this project in my spare time.  I have a ton of improvements in mind and will take them one at a time.  
I welcome all constructive criticism and feedback.  Feature requests will be taken - but I make no guarantees about if or when I can fulfil 
them.  This is open source, so please feel free to send me pull requests.  This is also my first GitHub-hosted project.  I've never run an 
open source project before.  So that aspect is quite new to me.  I welcome input on that as well.
