# Coordinate Transformation

Performs a 2D coordinate transformation for converting coordinates from one coordinate system to another.

## Input

The input can be created in a separate text file, or within the provided editor itself. Use a semicolon within the input file to indicate a comment. This input must take the following form:

|3||;number of common points between the systems, in this case 3.|
|X1|Y1|;The common points for the coordinate system that the unknown points will be transformed to|
|X2|Y2||
|X3|Y3||
|U1|V1|;The common points for the coordinate system that the unknown points will be transformed from|
|U2|V2||
|U3|V3||
|4||;Number of unknown points for which to solve|
|U4|V4||
|U5|V5||
|U6|V6||
|U7|V7||
{: rules="groups"}

Once entered, select the "Run" button to generate the output. This output will generate the residual matrix, variance, and standard deviation of the calculation. This will help to determine any outliers that may exist within the input. The output will also provide the transformed coordinates for the unknown points.

## Actions

1. Open - Allows the user to browse for an input text file.
2. Save Input - Allows the user to save any changes made to the input file.
3. Run - Performs the 2D coordinate transformation and displays the output in the output text box.
4. Save Output - Allows the user to save the output to a chosen location.

To make a new input file without using a text editor, simply delete any existing input, enter the new input, and select "Save Input". The "Save Input" option always works as a save as, ensuring you won't overwrite any existing input data without warning.
