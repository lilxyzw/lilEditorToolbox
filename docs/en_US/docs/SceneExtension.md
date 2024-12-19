# Scene View Extensions

A button to turn on anti-aliasing in the scene view and a camera mode (Vertex Attribute) has been added to check the vertex data of the model.

> [!WARNING]
> Some functions do not work in the Metal environment because the camera mode extension uses a geometry shader.

## MSAA

Click the MSAA button on the toolbar above the Scene View to toggle anti-aliasing on and off. Anti-aliasing is always applied, allowing you to adjust materials in a way that is close to how they will actually appear.

## Camera Modes

This extension function will be enabled when you click the camera mode change button (which switches to Wireframe display, etc.) on the toolbar above the Scene View and select Vertex Attribute. A popup will appear in the upper left of the Scene View that allows you to switch the vertex data to be displayed, so you can change this to check the desired data.

