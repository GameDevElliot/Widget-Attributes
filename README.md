Note: This tool is still in the early stages of development. Changes in near-future versions may occur which may affect the compilation of your scripts
# Unity Widget-Attributes

Use Widget-Attributes to quickly and easily visualise and manipulate data from your monobehaviours in the Scene View.

Handles are by no means, but the intention of Widget-Attributes is to save you time and energy, by making it easy to create interactable widgets in your scene without having to write a custom editor for each script.

Instructions:

    Download and extract the package into your Assets folder.
    Make sure to include the WidgetAttributes namespace by including the line "using WidgetAttributes;" at the top of any monobehaviour class.
    Use the c# square bracket attributes before (or above) Vectors and Rects.
    Add the monobehaviour script to an object in your scene if you haven't already.
    Widget Attribute List:

[Widget]

[Label]

[ArrowTo(nameof(myVector3))]

Notes:
    The widget attributes are compatible with Vector3, Vector2, Vector3int, Vecotr2int, float3 & float2 (Unity.Mathematics).
    It currently only works for monobehaviours, and uses a custom Editor for the type monobehaviour. This includes child classes of the monobehaviour
    Style attributes can be used on any field and make changes for the current field and any proceeding fields.
    See the samples for some ideas.

Follow on Github:
https://github.com/GameDevElliot/Widget-Attributes

If you like this tool, please consider donating.
https://gamedevelliot.itch.io/unity-widget-attributes
