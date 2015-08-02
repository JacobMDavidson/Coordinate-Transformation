# Coordinate Transformation

Performs a 2D coordinate transformation for converting coordinates from one coordinate system to another.

## Input

The input can be created in a separate text file, or within the provided editor itself. Use a semicolon within the input file to indicate a comment. This input must take the following form:

<table>
  <tbody>
    <tr>
      <td>3</td>
      <td></td>
      <td>;number of common points between the systems, in this case 3.</td>
    </tr>
    <tr>
      <td>X1</td>
      <td>Y1</td>
      <td>;The common points for the coordinate system to which the unknown points will be transformed</td>
    </tr>
    <tr>
      <td>X2</td>
      <td>Y2</td>
    </tr>
    <tr>
      <td>X3</td>
      <td>Y3</td>
    </tr>
    <tr>
      <td>U1</td>
      <td>V1</td>
      <td>;The common points for the coordinate system from which the unknown points will be transformed</td>
    </tr>
    <tr>
      <td>U2</td>
      <td>V2</td>
    </tr>
    <tr>
      <td>U3</td>
      <td>V3</td>
    </tr>
    <tr>
      <td>4</td>
      <td></td>
      <td>;Number of unknown points for which to solve</td>
    </tr>
    <tr>
      <td>U4</td>
      <td>V4</td>
    </tr>
    <tr>
      <td>U5</td>
      <td>V5</td>
    </tr>
    <tr>
      <td>U6</td>
      <td>V6</td>
    </tr>
    <tr>
      <td>U7</td>
      <td>V7</td>
    </tr>
  </tbody>
</table>

Once entered, select the "Run" button to generate the output. This output will generate the residual matrix, variance, and standard deviation of the calculation. This will help to determine any outliers that may exist within the input. The output will also provide the transformed coordinates for the unknown points.

## Actions

1. Open - Allows the user to browse for an input text file.
2. Save Input - Allows the user to save any changes made to the input file.
3. Run - Performs the 2D coordinate transformation and displays the output in the output text box.
4. Save Output - Allows the user to save the output to a chosen location.

To make a new input file without using a text editor, simply delete any existing input, enter the new input, and select "Save Input". The "Save Input" option always works as a save as, ensuring you won't overwrite any existing input data without warning.

## References

The calculation is based on the 2D linear coordinate transformation method detailed in chapter 18 of the fifth edition of *Adjustment Computations - Spatial Data Analysis* by Charles D. Ghilani.

The Matrix class used in this application is courtesy of [Ivan Kuckir](http://blog.ivank.net), and is available on [GitHub](https://github.com/darkdragon-001/LightweightMatrixCSharp).
