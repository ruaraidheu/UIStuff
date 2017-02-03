# UIStuff
UIStuff is a C# library to make creating a UI in Monogame as easy as possible.

### Getting Started

Add a reference to the library.

Add a using.

```C#
using UIStuff;
```

Add an object of type UIController

```C#
UIController controller;
```

Initialise it.

```C#
controller = new UIController();
```

Add some menus.

The UIBase is like a canvas that you paint various controls onto such as images, text, buttons, etc.
It has a unique name so you can reference it easily later (Or reference "none" to get an invisible menu).
The type specifies whether it is part of the world or overlayed (world is not implemented).
The Overlaytype is returned by the update and draw methods so you can easily control whether the rest of your game is running or paused.
The Controls, you can add as many as you like per base they are layered in order, so the first one is at the bottom and each succeeding one is drawn over the top.
Each type of control has different parameters, a UIImage requires a Texture2D, UIText requires a string a spritefont and a color, etc. Although all of the have several shared properties, Positioning has three types: 

Absolute uses pixels for the units used in position and size Relative uses percent and Square is the same as relative but preserves aspect ratio (This means the height in size doesnt do anything). 

Origin is where on the page zero zero is.

Alignment is where on the object zero zero is (so if you want to center an object set the position to zero zero the origin to the center middle (only doing this will put the top left of the object in the center) and Alignment to center middle(this will put the middle of the object at zero zero)).

position (relative to origin).

size (although UIText finds this automatically from the spritefont).

```C#
controller.Add(
    new UIBase("example", UIBase.Type.over, UIBase.Overlaytype.Menu,
        //Background Image
        new UIImage(
            //You may want to use square instead to maintain aspect ratio
            UIControl.Positioning.Relative,
            UIControl.Origin.TopLeft,
            UIControl.Alignment.TopLeft,
            new Point(0, 0),
            new Size(100, 100),
            Content.Load<Texture2D>("bgimg"),
        ),
        //Title text
        new UIText(
            UIControl.Positioning.Relative,
            UIControl.Origin.TopCenter,
            UIControl.Alignment.TopCenter,
            new Point(0, 10),
            Content.Load<SpriteFont>("titlefont"),
            Color.White
        )
        //So on...
    )
);
```

Add the update in the update method in Game1.

```C#
controller.Update();
```

Or to add pausing capability.

```C#
UIBase.Overlaytype olt = controller.Update();
if (olt == UIBase.Overlaytype.Game || olt == UIBase.Overlaytype.Running)
{
    //Pausing Code
}
//Non-Pausing Code
```

And finally the draw call.

```C#
spriteBatch.Begin();
controller.Draw(spriteBatch, graphics.GraphicsDevice.Viewport);
spriteBatch.End();
```

Or adding pausing.

```C#
spriteBatch.Begin();
UIBase.Overlaytype olt = controller.Draw(spriteBatch, graphics.GraphicsDevice.Viewport);
if (olt == Game || olt == Paused)
{
    //Code will only be drawn at certain times
}
//Code will always be drawn
spriteBatch.End();
```
