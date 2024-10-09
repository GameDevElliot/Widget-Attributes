# Unity Widget-Attributes
Use attributes to createwidgets and labels ("handles") in the scene view, to visualise and manipulate data in the Scene View.

Instructions:
Include the line "using WidgetAttributes;" at the top of any monobehaviour class.
Use the c# square bracket attributes before (or above) Vector3 and rect fields. 
Add the monobehaviour script to an object in your scene if you haven't already.

Widget Attribute List:
[Widget]
[Label]
[ArrowTo(nameof(myVector3))]

Note:
Currently it only works fully with Vector 3 and Rect fields.
It currently only works for monobehaviours, and uses a custom Editor for the type monobehaviour. This includes child classes of the monobehaviour

Style attributes can be used on any field and make changes for the current field and any proceeding fields.

See the samples for some ideas.

More documentation to come soon.
