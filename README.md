# ServicePro
A complete solution for small service center

Server (Contains)
1.	Models
                  Get Set Models for every database tables

2.	NHibernate (Mapping & NHibernate Helper )
             Mapping:  NHibernate Mapping to model to understand NHibernate about Model constraints like PK, FK, NULL like that
NHibernate Helper:  Contains DB connection and Mapped Model to work with NHibernate

3.	Helper (General Helper)

Maintaining CURD operation for every controller with some additional methods


4.	Common

This class contains some build in functions helps to development 
                Like,
	Fetch Table
This will return a table
	Fetch List
This will return a list
	Duplicate Check
Checking Records if it’s already exist in DB
	Is Value Exist
Checking Table with particular column and value if exits in Table return true
	Get Type
Returns Type Details as Table to use on Combo Box and more while development
	Get Grid List
Returns IList for all Grid for binding data
	 Get Grid List Count
Returns Grid List Count for handling paging for Grid



5.	CommonUtil
This Class contains some build in functions helps to development
		Like,
	Log functions ( like Info, Debug, Error )
	Copy To Data Table
It will convert IList into Data Table 

ServicePro (Contains)
1.	UI
Contains UI for product
Like,
	Bootstrap
	Dist
Its contains all CSS and JS files for UI based on AdminLTE
	Font-Awesome
	JQuery Grid
	Plugins ( Like JQuery, Toastr, Select2, DateTime Picker )

2.	Controller
It’s used to handle all user request and send back them a response


3.	View
It’s contains view


4.	Scripts
It’s contains all Java script files for handling scripting for every view 

5.	Log
It’s used to log information 


6.	Log4net.config
Configuration file for Log4Net
